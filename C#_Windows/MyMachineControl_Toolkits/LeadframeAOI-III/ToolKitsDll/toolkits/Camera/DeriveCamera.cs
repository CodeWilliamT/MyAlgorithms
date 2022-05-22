using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Data;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.InteropServices;
using System.Threading;
using HalconDotNet;
using NationalInstruments.CWIMAQControls;
using niimaqdx;
using FlyCapture2Managed;
using Basler.Pylon;
using Matrox.MatroxImagingLibrary;
using MvCamCtrl.NET;

namespace ToolKits.HTCamera
{
    public class BaslerFromNIToHalcon : BaseCamera
    {
        //public event EventHandler<Resource> GrabImageDone;
        #region 私有变量
        private string camName = "";
        private bool isSoftwareTrigger = true;
        private bool isConnected = false;
        private bool isMirrorX = false;
        private bool isMirrorY = false;
        private BaslerFromNIPixelFormat pixelFormat = BaslerFromNIPixelFormat.Mono_8;
        private int timeOut = 500;//ms
        private HObject image = new HObject();
        private int img_width, img_height;
        private CWIMAQdx.Session session = new CWIMAQdx.Session();
        private byte[] buffer;
        //private CancellationTokenSource cts = null;
        //private Thread thread = null;
        //private Resource r = new Resource();
        #endregion
        /*-----------------------------------方法---------------------------------------------*/
        public BaslerFromNIToHalcon(string cam_name)
        {
            this.camName = cam_name;
            HOperatorSet.GenEmptyObj(out image);
        }
        public override bool InitCamera() { return true; }
        //打开相机
        public override bool OpenCamera()
        {
            try
            {
                if (isConnected)
                {
                    if (!CloseCamera()) return false;
                }
                CWIMAQdx.Error err;
                err = CWIMAQdx.OpenCamera(this.camName, CWIMAQdx.CameraControlMode.Controller, out session);
                if (err != CWIMAQdx.Error.Success) return false;
                else isConnected = true;
                err = CWIMAQdx.SetAttribute(session, "CameraAttributes::AcquisitionTrigger::AcquisitionMode", CWIMAQdx.ValueType.U32, 2);
                if (err != CWIMAQdx.Error.Success) return false;
                err = CWIMAQdx.SetAttribute(session, "CameraAttributes::AcquisitionTrigger::TriggerMode", CWIMAQdx.ValueType.U32, 1);
                if (err != CWIMAQdx.Error.Success) return false;
                int triggerSource = (this.isSoftwareTrigger) ? 0 : 1;    //0:软触发;1:硬触发
                err = CWIMAQdx.SetAttribute(session, "CameraAttributes::AcquisitionTrigger::TriggerSource", CWIMAQdx.ValueType.U32, triggerSource);
                if (err != CWIMAQdx.Error.Success) return false;
                err = CWIMAQdx.SetAttribute(session, "CameraAttributes::ImageFormat::PixelFormat", CWIMAQdx.ValueType.U32, this.pixelFormat);
                if (err != CWIMAQdx.Error.Success) return false;
                err = CWIMAQdx.SetAttribute(session, "CameraAttributes::ImageFormat::ReverseX", CWIMAQdx.ValueType.Bool, this.isMirrorX);
                if (err != CWIMAQdx.Error.Success) return false;
                err = CWIMAQdx.SetAttribute(session, "AcquisitionAttributes::TimeOut", CWIMAQdx.ValueType.U32, this.timeOut);
                if (err != CWIMAQdx.Error.Success) return false;
                object width, height;
                err = CWIMAQdx.GetAttribute(session, "CameraAttributes::AOI::Width", CWIMAQdx.ValueType.I64, out width);
                err = CWIMAQdx.GetAttribute(session, "CameraAttributes::AOI::Height", CWIMAQdx.ValueType.I64, out height);
                img_width = Convert.ToInt32(width);
                img_height = Convert.ToInt32(height);
                int len = img_width * img_height;
                buffer = new byte[len];
                CWIMAQdx.ConfigureAcquisition(session, true, 1);
                CWIMAQdx.StartAcquisition(session);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        //关闭相机
        public override bool CloseCamera()
        {
            try
            {
                if (isConnected)
                {
                    CWIMAQdx.Error err;
                    err = CWIMAQdx.UnconfigureAcquisition(session);
                    err = CWIMAQdx.StopAcquisition(session);
                    err = CWIMAQdx.CloseCamera(session);
                    if (err == CWIMAQdx.Error.Success) isConnected = false;
                    else isConnected = true;
                    return !isConnected;
                }
                else return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        /// <summary>
        /// OpenCamera之前设置触发源
        /// </summary>
        /// <param name="source"></param>
        public override bool IsSoftwareTrigger
        {
            //0:软触发
            //1:硬触发
            get { return this.isSoftwareTrigger; }
            set { this.isSoftwareTrigger = value; }
        }
        public override bool IsConnected
        {
            get { return this.isConnected; }
        }
        public override bool IsMirrorX
        {
            get
            {
                return this.isMirrorX;
            }
            set
            {
                this.isMirrorX = value;
            }
        }
        public override bool IsMirrorY
        {
            set { this.isMirrorY = value; }
            get { return this.isMirrorY; }
        }
        public override bool ChangeTriggerSource(bool isSoftwareTrigger)
        {
            if (isSoftwareTrigger == this.isSoftwareTrigger) return true;
            //0:软触发
            //1:硬触发
            int triggerSource = isSoftwareTrigger ? 0 : 1;
            try
            {
                if (isConnected)
                {
                    //暂停采集和改变配置
                    CWIMAQdx.Error err;
                    err = CWIMAQdx.UnconfigureAcquisition(session);
                    err = CWIMAQdx.StopAcquisition(session);

                    //改变配置
                    err = CWIMAQdx.SetAttribute(session, "CameraAttributes::AcquisitionTrigger::TriggerSource", CWIMAQdx.ValueType.U32, triggerSource);
                    if (err != CWIMAQdx.Error.Success)
                        return false;

                    //重新配置和启动采集
                    CWIMAQdx.ConfigureAcquisition(session, true, 1);
                    CWIMAQdx.StartAcquisition(session);

                    return true;
                }
                else
                    return true;
            }
            catch (Exception)
            {
                return false;
            }

            //首先暂停采集

            //改变配置
        }
        //获取相机增益
        public override void GetGain(out Double gain)
        {
            object gain_obj;
            CWIMAQdx.GetAttribute(session, "CameraAttributes::AnalogControls::GainRaw", CWIMAQdx.ValueType.I64, out gain_obj);
            gain = Convert.ToDouble(gain_obj);
        }
        //获取相机曝光时间
        public override void GetExposure(out Double exposure)
        {
            object exp_obj;
            CWIMAQdx.GetAttribute(session, "CameraAttributes::AcquisitionTrigger::ExposureTimeRaw", CWIMAQdx.ValueType.I64, out exp_obj);
            exposure = Convert.ToDouble(exp_obj);
        }
        //设置相机增益
        public override bool SetGain(Double gain)
        {
            try
            {
                CWIMAQdx.Error err = CWIMAQdx.SetAttribute(session, "CameraAttributes::AnalogControls::GainRaw", CWIMAQdx.ValueType.I64, gain);
                if (err != 0) return false;
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
        //设置相机曝光时间
        public override bool SetExposure(Double exposure)
        {
            try
            {
                CWIMAQdx.Error err = CWIMAQdx.SetAttribute(session, "CameraAttributes::AcquisitionTrigger::ExposureTimeAbs", CWIMAQdx.ValueType.I64, exposure);
                if (err != 0) return false;
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
        //设置相机图像格式
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pix_format"></param>
        /// <returns></returns>
        public override bool BaslerFromNISetPixelFormat(BaslerFromNIPixelFormat pix_format)
        {
            try
            {
                this.pixelFormat = pix_format;
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
        public override void BaslerFromNIGetPixelFormat(out BaslerFromNIPixelFormat pix_format)
        {
            object _pix_format = "";
            CWIMAQdx.GetAttribute(session, "CameraAttributes::ImageFormat::PixelFormat", CWIMAQdx.ValueType.U32, out _pix_format);
            pix_format = (BaslerFromNIPixelFormat)Enum.Parse(typeof(BaslerFromNIPixelFormat), _pix_format.ToString());
        }
        /// <summary>
        /// 软件触发采集图像，主动采集
        /// </summary>
        /// <returns></returns>
        public override Acquisition Snap(int imageNum, int timeOut)
        {
            CWIMAQdx.SetAttribute(session, "CameraAttributes::AcquisitionTrigger::TriggerSoftware", CWIMAQdx.ValueType.String, "Execute");
            Acquisition acq = new Acquisition();
            int bufferActual = 0;
            CWIMAQdx.Error err = CWIMAQdx.GetImageData(session, ref buffer, CWIMAQdx.BufferNumberMode.Next, 0, out bufferActual);
            if (err == CWIMAQdx.Error.Success)
            {
                unsafe
                {
                    fixed (byte* p = buffer)
                    {
                        IntPtr ptr = (IntPtr)p;
                        image.Dispose();
                        //黑白格式
                        if (this.pixelFormat.ToString().Contains("Mono"))
                        {
                            HOperatorSet.GenImage1(out image, "byte", img_width, img_height, ptr);
                        }
                        //彩色格式
                        else
                        {

                        }
                    }
                }
                acq.Image.Dispose();
                acq.Image = image;
                acq.GrabStatus = "GrabPass";
            }
            else
            {
                acq.Image.Dispose();
                acq.GrabStatus = "GrabFail";
            }
            return acq;
        }
        /// <summary>
        /// 硬件触发采集图像，被动采集
        /// </summary>
        /// <returns></returns>
        public override Acquisition Hard_Snap()
        {
            Acquisition acq = new Acquisition();
            int bufferActual = 0;
            CWIMAQdx.Error err = CWIMAQdx.GetImageData(session, ref buffer, CWIMAQdx.BufferNumberMode.Next, 0, out bufferActual);
            if (err == CWIMAQdx.Error.Success)
            {
                unsafe
                {
                    fixed (byte* p = buffer)
                    {
                        IntPtr ptr = (IntPtr)p;
                        image.Dispose();
                        //黑白格式
                        if (this.pixelFormat.ToString().Contains("Mono"))
                        {
                            HOperatorSet.GenImage1(out image, "byte", img_width, img_height, ptr);
                        }
                        //彩色格式
                        else
                        {

                        }
                    }
                }
                acq.Image.Dispose();
                acq.Image = image;
                acq.GrabStatus = "GrabPass";
            }
            else
            {
                acq.Image.Dispose();
                acq.GrabStatus = "GrabFail";
            }
            return acq;
        }

        /*------------------------------结尾------------------------------------------------------*/
    }
    public class BaslerToHalcon : BaseCamera
    {
        #region 私有变量
        private HObject image;
        private Camera camera = null;
        private string sn = "";
        private string pixelType = BaslerToHalcon.PixelFormat.Mono8;
        //private string triggerSource = "";
        private bool isSoftwareTrigger = true;
        private bool isMirrorX = false;
        private bool isMirrorY = false;
        private bool isTrggerLight = false;
        private PixelDataConverter converter = new PixelDataConverter();
        //private Resource r = new Resource();
        System.Collections.Concurrent.ConcurrentQueue<Acquisition> acqQueue = new System.Collections.Concurrent.ConcurrentQueue<Acquisition>();
        //ConcurrentQueue<Acquisition>acqQueue=new ConcurrentQueue<Acquisition>();
        static object lockObj = new object();
        #endregion

        static Version Sfnc2_0_0 = new Version(2, 0, 0);
        static PLCamera.PixelFormatEnum PixelFormat = new PLCamera.PixelFormatEnum();
        static PLCamera.SequencerTriggerSourceEnum TriggerSource = new PLCamera.SequencerTriggerSourceEnum();
        //public event EventHandler<Acquisition> GrabImageDone;

        public BaslerToHalcon(string serialNumber)
        {
            this.sn = serialNumber;
            HOperatorSet.GenEmptyObj(out image);
            Environment.SetEnvironmentVariable("PYLON_GIGE_HEARTBEAT", "1000");
        }
        public override bool InitCamera()
        {
            try
            {
                if (camera == null)
                {
                    camera = new Camera(sn);
                    //grabThread = new Thread(new ThreadStart(GrabFunction));
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }
        public override bool OpenCamera()
        {
            try
            {
                if (camera.IsOpen)
                {
                    if (camera.StreamGrabber.IsGrabbing) camera.StreamGrabber.Stop();
                    camera.Close();
                    Thread.Sleep(50);
                }
                //软触发使用
                if (this.isSoftwareTrigger) camera.CameraOpened += Configuration.SoftwareTrigger;
                camera.StreamGrabber.ImageGrabbed += StreamGrabber_ImageGrabbed;

                ICamera iCamera = camera.Open();
                camera.Parameters[BaslerToHalcon.PixelFormat].TrySetValue(this.pixelType);
                camera.Parameters[PLCamera.ReverseX].SetValue(this.isMirrorX);
                //camera.Parameters[PLCamera.ReverseY].SetValue(this.isMirrorY);
                //camera.Parameters[PLCamera.TriggerSelector].SetValue("FrameStart");
                //camera.Parameters[PLCamera.TriggerSelector].SetValue("FrameStart");
                camera.Parameters[PLCamera.TriggerSource].SetValue("Software");
                camera.Parameters[PLCamera.TriggerMode].SetValue("On");
                camera.Parameters[PLCamera.LineSelector].SetValue("Line1");
                //string a = camera.Parameters[PLCamera.LineMode].GetValue();
                //a = camera.Parameters[PLCamera.LineSelector].GetValue();
                //硬触发使用
                if (!this.isSoftwareTrigger) camera.Parameters[PLCamera.TriggerSource].SetValue("Line1");
                camera.StreamGrabber.Start(GrabStrategy.LatestImages, GrabLoop.ProvidedByStreamGrabber);
                return true;
            }
            catch (Exception e)
            {
                string err = e.Message;
                CloseCamera();
                return false;
            }
        }
        public override bool CloseCamera()
        {
            try
            {
                if (camera == null) return true;
                if (camera.IsOpen)
                {
                    if (camera.StreamGrabber.IsGrabbing) camera.StreamGrabber.Stop();
                    camera.CameraOpened -= Configuration.SoftwareTrigger;
                    camera.StreamGrabber.ImageGrabbed -= StreamGrabber_ImageGrabbed;
                    camera.Close();
                    //if (acqQueue != null) acqQueue.Clear();
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public override bool IsSoftwareTrigger
        {
            get { return this.isSoftwareTrigger; }
        }
        public override bool IsTriggerLight
        {
            get { return this.isTrggerLight; }
        }
        public override bool IsConnected
        {
            get
            {
                return this.camera.IsConnected;
            }
        }
        public override bool IsMirrorX
        {
            set { this.isMirrorX = value; }
            get { return this.isMirrorX; }
        }
        public override bool IsMirrorY
        {
            set { this.isMirrorY = value; }
            get { return this.isMirrorY; }
        }
        public override bool ChangeTriggerSource(bool isSoftwareTrigger)
        {
            if (isSoftwareTrigger == this.isSoftwareTrigger) return true;
            try
            {
                //if (isSoftwareTrigger)
                //{
                //    camera.Parameters[PLCamera.TriggerSelector].SetValue("FrameStart");
                //    camera.Parameters[PLCamera.TriggerMode].SetValue("On");
                //}
                //else
                //{
                //    camera.Parameters[PLCamera.TriggerSelector].SetValue("FrameStart");
                //    camera.Parameters[PLCamera.TriggerMode].SetValue("On");
                //}
                this.isSoftwareTrigger = isSoftwareTrigger;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public override bool ChangeTriggerLight(bool isTrggerLight)
        {
            if (isTrggerLight == this.isTrggerLight) return true;
            try
            {
                if (isTrggerLight)
                {

                    camera.Parameters[PLCamera.LineSelector].SetValue("Out1");
                    camera.Parameters[PLCamera.LineSource].SetValue("ExposureActive");
                }
                else
                {
                    camera.Parameters[PLCamera.LineSelector].SetValue("Line1");
                }
                this.isTrggerLight = isTrggerLight;
                return true;
            }
            catch
            {
                return false;
            }
        }
        public override bool SetGain(double value)
        {
            try
            {
                // Some camera models may have auto functions enabled. To set the gain value to a specific value,
                // the Gain Auto function must be disabled first (if gain auto is available).
                if (!camera.Parameters[PLCamera.GainAuto].TrySetValue(PLCamera.GainAuto.Off)) // Set GainAuto to Off if it is writable.
                    return false;
                // Features, e.g. 'Gain', are named according to the GenICam Standard Feature Naming Convention (SFNC).
                // The SFNC defines a common set of features, their behavior, and the related parameter names. 
                // This ensures the interoperability of cameras from different camera vendors. 
                // Cameras compliant with the USB3 Vision standard are based on the SFNC version 2.0.
                // Basler GigE and Firewire cameras are based on previous SFNC versions.
                // Accordingly, the behavior of these cameras and some parameters names will be different.
                // The SFNC version can be used to handle differences between camera device models.
                if (camera.GetSfncVersion() < Sfnc2_0_0)
                {
                    // In previous SFNC versions, GainRaw is an integer parameter.
                    camera.Parameters[PLCamera.GainRaw].SetValuePercentOfRange(Convert.ToInt32(value));
                }
                else // For SFNC 2.0 cameras, e.g. USB3 Vision cameras
                {
                    // In SFNC 2.0, Gain is a float parameter.
                    camera.Parameters[PLUsbCamera.Gain].SetValuePercentOfRange(value);
                    // For USB cameras, Gamma is always enabled.
                }
                return true;
            }
            catch (Exception)
            {
                camera.Parameters[PLCamera.GainAuto].TrySetValue(PLCamera.GainAuto.Continuous);
                return false;
            }

        }
        public override void GetGain(out double value)
        {
            try
            {
                if (camera.GetSfncVersion() < Sfnc2_0_0)
                {
                    // In previous SFNC versions, GainRaw is an integer parameter.
                    value = camera.Parameters[PLCamera.GainRaw].GetValuePercentOfRange();
                }
                else // For SFNC 2.0 cameras, e.g. USB3 Vision cameras
                {
                    // In SFNC 2.0, Gain is a float parameter.
                    value = camera.Parameters[PLUsbCamera.Gain].GetValuePercentOfRange();
                    // For USB cameras, Gamma is always enabled.
                }
            }
            catch (Exception)
            {
                value = -1;
            }
        }
        public override bool SetExposure(double value)
        {
            try
            {
                if (!camera.Parameters[PLCamera.ExposureAuto].TrySetValue(PLCamera.ExposureAuto.Off))
                    return false;
                if (camera.GetSfncVersion() < Sfnc2_0_0)
                {
                    return camera.Parameters[PLCamera.ExposureTimeRaw].TrySetValue(Convert.ToInt32(value));
                }
                else
                {
                    return camera.Parameters[PLCamera.ExposureTime].TrySetValue(value);
                }

            }
            catch (Exception)
            {
                //camera.Parameters[PLCamera.ExposureTime].TrySetValue(100);
                return false;
            }
        }
        public override void GetExposure(out double value)
        {
            try
            {
                if (camera.GetSfncVersion() < Sfnc2_0_0)
                {
                    value = camera.Parameters[PLCamera.ExposureTimeRaw].GetValue();
                }
                else
                {
                    value = camera.Parameters[PLCamera.ExposureTime].GetValue();
                }
            }
            catch (Exception)
            {
                value = -1;
            }

        }
        public void SetGamma(bool value)
        {
            try
            {
                // GammaEnable is a boolean parameter.
                camera.Parameters[PLCamera.GammaEnable].TrySetValue(value);    //if available
            }
            catch (Exception)
            {
                camera.Parameters[PLCamera.GammaEnable].TrySetValue(true);
            }
        }
        public void GetGamma(out bool value)
        {
            try
            {
                value = camera.Parameters[PLCamera.GammaEnable].GetValue();
            }
            catch (Exception)
            {
                value = false;
            }
        }
        /// <summary>设置相机的像素格式
        /// 设置相机的像素格式
        /// </summary>
        /// <param name="value">value可以设置为如:BaslerToHalcon.PixelFormat.Mono8</param>
        public override bool BaslerSetPixelFormat(string value)
        {
            //value = BaslerToHalcon.PixelFormat.Mono8; 

            try
            {
                this.pixelType = value;
                return true;
                //return camera.Parameters[BaslerToHalcon.PixelFormat].TrySetValue(value);
            }
            catch (Exception)
            {
                //camera.Parameters[BaslerToHalcon.PixelFormat].TrySetValue(BaslerToHalcon.PixelFormat.Mono8);
                return false;
            }

        }
        public override void BaslerGetPixelFormat(out string value)
        {
            try
            {
                value = camera.Parameters[BaslerToHalcon.PixelFormat].GetValue();
            }
            catch (Exception)
            {
                value = "";
            }

        }
        public override bool ClearImageQueue()
        {
            try
            {
                while (this.acqQueue.Count > 0)
                {
                    //this.acqQueue.Dequeue().Image.Dispose();
                    Acquisition acq;
                    this.acqQueue.TryDequeue(out acq);
                    acq.Dispose();
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        /// <summary>
        /// 软触发采集图像
        /// </summary>
        /// <param name="imageNum"></param>
        /// <param name="timeOut"></param>
        /// <returns></returns>
        public override Acquisition Snap(int imageNum = 1, int timeOut = -1)
        {
            Acquisition _acq = new Acquisition();
            try
            {
                timeOut = (timeOut == -1) ? 100000 : timeOut;
                camera.ExecuteSoftwareTrigger();
                DateTime t1 = DateTime.Now;
                while (true)
                {
                    lock (lockObj)
                    {
                        if (DateTime.Now.Subtract(t1).TotalMilliseconds > timeOut)//采集超时500ms
                        {
                            while (acqQueue.Count > 0)
                            {
                                Acquisition acq;
                                acqQueue.TryDequeue(out acq);
                                acq.Image.Dispose();
                                //acqQueue.Dequeue().Image.Dispose();
                            }
                            _acq.GrabStatus = "GrabFail:TimeOut";
                            return _acq;
                        }
                        else
                        {
                            if (_acq.index >= imageNum - 1)
                            {
                                _acq.GrabStatus = "GrabPass";
                                return _acq;
                            }
                            if (acqQueue.Count > 0)
                            {
                                Acquisition acq;
                                acqQueue.TryDequeue(out acq);
                                switch (acq.GrabStatus)
                                {
                                    case "GrabPass":
                                        HOperatorSet.ConcatObj(_acq.Image, acq.Image, out _acq.Image);
                                        _acq.index++;
                                        break;
                                    case "GrabFail":
                                        acq.Image.Dispose();
                                        _acq.GrabStatus = "GrabFail:the latest image grab failed.";
                                        _acq.index++;
                                        return _acq;
                                    default:
                                        _acq.GrabStatus = acq.GrabStatus;
                                        acq.Image.Dispose();
                                        _acq.index++;
                                        return _acq;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _acq.GrabStatus = "GrabFail:" + ex.Message;
                return _acq;
            }
        }
        public override Acquisition GetFrames(int imageNum = 1, int timeOut = -1)
        {
            Acquisition _acq = new Acquisition();
            try
            {
                timeOut = (timeOut == -1) ? 100000 : timeOut;
                DateTime t1 = DateTime.Now;
                while (true)
                {
                    lock (lockObj)
                    {
                        if (DateTime.Now.Subtract(t1).TotalMilliseconds > timeOut)//采集超时500ms
                        {
                            while (acqQueue.Count > 0)
                            {
                                Acquisition acq;
                                acqQueue.TryDequeue(out acq);
                                acq.Image.Dispose();
                            }
                            _acq.GrabStatus = "GrabFail:TimeOut";
                            return _acq;
                        }
                        else
                        {
                            if (_acq.index >= imageNum - 1)
                            {
                                _acq.GrabStatus = "GrabPass";
                                return _acq;
                            }
                            if (acqQueue.Count > 0)
                            {
                                Acquisition acq;
                                acqQueue.TryDequeue(out acq);
                                switch (acq.GrabStatus)
                                {
                                    case "GrabPass":
                                        HOperatorSet.ConcatObj(_acq.Image, acq.Image, out _acq.Image);
                                        _acq.index++;
                                        break;
                                    case "GrabFail":
                                        acq.Image.Dispose();
                                        _acq.GrabStatus = "GrabFail:the latest image grab failed.";
                                        _acq.index++;
                                        return _acq;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _acq.GrabStatus = "GrabFail:" + ex.Message;
                return _acq;
            }
        }
        public override Task<Acquisition> GetFramesAsync(int imageNum = 1, int timeOut = -1)
        {
            return Task.Run(() =>
            {
                return GetFrames(imageNum, timeOut);
            });
        }
        void StreamGrabber_ImageGrabbed(object sender, ImageGrabbedEventArgs e)
        {
            using (e.GrabResult)
            {
                Acquisition _acq = new Acquisition();
                // Image grabbed successfully?
                if (e.GrabResult.GrabSucceeded)
                {
                    byte[] buffer = e.GrabResult.PixelData as byte[];
                    unsafe
                    {
                        fixed (byte* p = buffer)
                        {
                            IntPtr ptr = (IntPtr)p;
                            image.Dispose();
                            if (this.pixelType == BaslerToHalcon.PixelFormat.Mono8)
                            {
                                //黑白图像
                                HOperatorSet.GenImage1(out image, "byte", e.GrabResult.Width, e.GrabResult.Height, ptr);
                            }
                            else if (!this.pixelType.Contains("Mono"))
                            {
                                Bitmap bitMap = new Bitmap(e.GrabResult.Width, e.GrabResult.Height, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
                                // Lock the bits of the bitmap.
                                BitmapData bmpData = bitMap.LockBits(new Rectangle(0, 0, bitMap.Width, bitMap.Height), ImageLockMode.ReadWrite, bitMap.PixelFormat);
                                // Place the pointer to the buffer of the bitmap.
                                converter.OutputPixelFormat = PixelType.BGRA8packed;
                                IntPtr ptrBmp = bmpData.Scan0;
                                converter.Convert(ptrBmp, bmpData.Stride * bitMap.Height, e.GrabResult); //Exception handling TODO
                                //bitMap.Save("E:\\bitMap.png");

                                //RGB彩色图像
                                HOperatorSet.GenImageInterleaved(out image, ptrBmp, "bgrx", e.GrabResult.Width, e.GrabResult.Height, -1, "byte", 0, 0, 0, 0, -1, 0);
                                bitMap.UnlockBits(bmpData);
                                bitMap.Dispose();
                            }
                            else
                            {
                                if (acqQueue != null)
                                {
                                    _acq.Dispose();
                                    _acq.GrabStatus = "current pixel format not support!";
                                    acqQueue.Enqueue(_acq);
                                    return;
                                }
                            }
                        }
                    }
                    if (acqQueue != null)
                    {
                        _acq.Dispose();
                        _acq.Image = image.CopyObj(1, -1);
                        _acq.GrabStatus = "GrabPass";
                        acqQueue.Enqueue(_acq);
                    }
                }
                else
                {
                    if (acqQueue != null)
                    {
                        _acq.Dispose();
                        _acq.GrabStatus = "GrabFail";
                        acqQueue.Enqueue(_acq);
                    }
                }
            }
        }

    }
    public class SvsToHalcon : BaseCamera
    {
        public SvsToHalcon() { }
        public override bool IsConnected
        { get { return false; } }
    }
    public class PointGreyToHalcon : BaseCamera
    {
        //public event EventHandler<Resource> GrabImageDone;
        private ManagedCameraBase camera;
        private ManagedImage rawImage;
        private ManagedImage convertImage;
        private ManagedPGRGuid guid;
        private uint _serialNumber;
        private HObject image = new HObject();
        private PointGreyPixelFormat _pixelFormat = PointGreyPixelFormat.PixelFormatMono8;
        private bool isSoftwareTrigger = true;
        private int timeOut = 500;//ms
        private bool isMirrorX = false;

        public PointGreyToHalcon(string serialNumber)
        {
            this._serialNumber = Convert.ToUInt32(serialNumber);
            HOperatorSet.GenEmptyObj(out this.image);
            rawImage = new ManagedImage();
            convertImage = new ManagedImage();
        }

        public override bool IsSoftwareTrigger
        {
            set { this.isSoftwareTrigger = value; }
            get { return this.isSoftwareTrigger; }
        }
        public override bool IsConnected
        {
            get { return this.camera.IsConnected(); }
        }
        public override bool IsMirrorX
        {
            set { this.isMirrorX = value; }
            get { return this.isMirrorX; }
        }
        static bool CheckSoftwareTriggerPresence(ManagedCamera cam)
        {
            const uint k_triggerInq = 0x530;
            uint regVal = cam.ReadRegister(k_triggerInq);

            if ((regVal & 0x10000) != 0x10000)
            {
                return false;
            }

            return true;
        }
        static bool PollForTriggerReady(ManagedCamera cam)
        {
            const uint k_softwareTrigger = 0x62C;

            uint regVal = 0;
            DateTime t1 = DateTime.Now;
            do
            {
                if (DateTime.Now.Subtract(t1).TotalMilliseconds > 1000)
                {
                    return false;
                }
                regVal = cam.ReadRegister(k_softwareTrigger);
            }
            while ((regVal >> 31) != 0);

            return true;
        }
        static bool PowerUpForCamera(ManagedCamera cam)
        {
            // Power on the camera
            const uint k_cameraPower = 0x610;
            const uint k_powerVal = 0x80000000;
            cam.WriteRegister(k_cameraPower, k_powerVal);

            const int k_millisecondsToSleep = 100;
            uint regVal = 0;

            // Wait for camera to complete power-up
            do
            {
                System.Threading.Thread.Sleep(k_millisecondsToSleep);

                regVal = cam.ReadRegister(k_cameraPower);

            } while ((regVal & k_powerVal) == 0);

            return true;
        }
        static bool FireSoftwareTrigger(ManagedCamera cam)
        {
            const uint k_softwareTrigger = 0x62C;
            const uint k_fireVal = 0x80000000;

            cam.WriteRegister(k_softwareTrigger, k_fireVal);

            return true;
        }
        static bool SetMirrorXForCamera(ManagedCameraBase camera, bool isMirrorX)
        {
            uint iidcVersion = 132;
            uint sk_imageDataFmtReg = 0x1048;
            uint sk_mirrorImageCtrlReg = 0x1054;
            uint mirrorCtrlRegister = sk_imageDataFmtReg;
            uint mirrorMask = 0x1 << 8;
            uint value = 0;
            CameraInfo camInfo = camera.GetCameraInfo();
            if (camInfo.iidcVersion >= iidcVersion)
            {
                mirrorCtrlRegister = sk_mirrorImageCtrlReg;
                mirrorMask = 0x1;
            }
            try
            {
                value = camera.ReadRegister(mirrorCtrlRegister);
            }
            catch (Exception)
            {
                return false;
            }
            if (isMirrorX)
            {
                value |= mirrorMask;
            }
            else
            {
                value &= ~mirrorMask;
            }
            try
            {
                camera.WriteRegister(mirrorCtrlRegister, value);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
        public override bool InitCamera()
        {
            try
            {
                ManagedBusManager busMgr = new ManagedBusManager();
                guid = busMgr.GetCameraFromSerialNumber(this._serialNumber);
                InterfaceType ifType = busMgr.GetInterfaceTypeFromGuid(guid);

                if (ifType == InterfaceType.GigE)
                {
                    this.camera = new ManagedGigECamera();
                }
                else
                {
                    this.camera = new ManagedCamera();
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public override bool OpenCamera()
        {
            try
            {
                this.camera.Connect(guid);
                if (!isSoftwareTrigger)
                {
                    // Check for external trigger support
                    TriggerModeInfo triggerModeInfo = this.camera.GetTriggerModeInfo();
                    if (triggerModeInfo.present != true)
                    {
                        //Console.WriteLine("Camera does not support external trigger! Exiting...\n");
                        return false;
                    }
                }

                // Get current trigger settings
                TriggerMode triggerMode = this.camera.GetTriggerMode();

                // Set camera to trigger mode 0
                // A source of 7 means software trigger
                triggerMode.onOff = true;
                triggerMode.mode = 0;
                triggerMode.parameter = 0;
                if (isSoftwareTrigger)
                {
                    // A source of 7 means software trigger
                    triggerMode.source = 7;
                }
                else
                {
                    // Triggering the camera externally using source 0.
                    triggerMode.source = 0;
                }
                //Set the trigger mode
                this.camera.SetTriggerMode(triggerMode);

                // Poll to ensure camera is ready
                bool retVal = PollForTriggerReady((ManagedCamera)this.camera);
                if (retVal != true)
                {
                    triggerMode.onOff = false;
                    this.camera.SetTriggerMode(triggerMode);
                    this.camera.Disconnect();
                    return false;
                }
                // Get the camera configuration
                FC2Config config = this.camera.GetConfiguration();
                // Set the grab timeout to 5 seconds
                config.grabTimeout = this.timeOut;

                // Set the camera configuration
                this.camera.SetConfiguration(config);

                if (!SetMirrorXForCamera(this.camera, this.isMirrorX))
                {
                    return false;
                }
                // Camera is ready, start capturing images

                this.camera.StartCapture();
                if (isSoftwareTrigger)
                {
                    if (CheckSoftwareTriggerPresence((ManagedCamera)this.camera) == false)
                    {
                        //Console.WriteLine("SOFT_ASYNC_TRIGGER not implemented on this camera!  Stopping application\n");
                        return false;
                    }
                }
                else
                {
                    //Console.WriteLine("Trigger the camera by sending a trigger pulse to GPIO%d.\n",
                    //  triggerMode.source);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public override bool CloseCamera()
        {
            try
            {
                if (this.camera != null)
                {
                    if (this.camera.IsConnected())
                    {
                        // Get current trigger settings
                        TriggerMode triggerMode = this.camera.GetTriggerMode();
                        this.camera.StopCapture();
                        triggerMode.onOff = false;
                        this.camera.SetTriggerMode(triggerMode);
                        this.camera.Disconnect();
                    }
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public override bool PointGraySetPixelFormat(PointGreyPixelFormat pixelFormat)
        {
            try
            {
                _pixelFormat = pixelFormat;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public override void PointGrayGetPixelFormat(out PointGreyPixelFormat pixelFormat)
        {
            pixelFormat = _pixelFormat;
        }
        public override bool ChangeTriggerSource(bool isSoftwareTrigger)
        {
            if (isSoftwareTrigger == this.isSoftwareTrigger) return true;
            if (!CloseCamera()) return false;
            this.isSoftwareTrigger = isSoftwareTrigger;
            if (!OpenCamera())
            {
                this.isSoftwareTrigger = !isSoftwareTrigger;
                return false;
            }

            return true;
        }
        public override bool SetExposure(Double exposureTime)
        {
            try
            {
                CameraProperty cp = this.camera.GetProperty(PropertyType.Shutter);
                cp.absValue = (float)exposureTime;
                cp.autoManualMode = false;
                this.camera.SetProperty(cp);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public override void GetExposure(out Double exposureTime)
        {
            try
            {
                CameraProperty cp = this.camera.GetProperty(PropertyType.Shutter);
                exposureTime = cp.absValue;
            }
            catch (Exception)
            {
                exposureTime = -1;//ms
            }
        }
        public override bool SetGain(Double gain)
        {
            try
            {
                CameraProperty cp = this.camera.GetProperty(PropertyType.Gain);
                cp.absValue = (float)gain;
                cp.autoManualMode = false;
                this.camera.SetProperty(cp);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public override void GetGain(out Double gain)
        {
            try
            {
                CameraProperty cp = this.camera.GetProperty(PropertyType.Gain);
                gain = cp.absValue;
            }
            catch (Exception)
            {
                gain = -1;
            }
        }
        public override Acquisition Snap(int imageNum, int timeOut)
        {
            Acquisition acq = new Acquisition();
            try
            {
                if (isSoftwareTrigger)
                {
                    //Check that the trigger is ready
                    bool retVal = PollForTriggerReady((ManagedCamera)this.camera);
                    if (retVal != true)
                    {
                        acq.Image.Dispose();
                        acq.GrabStatus = "GrabFail";
                        return acq;
                    }
                    // Fire software trigger
                    retVal = FireSoftwareTrigger((ManagedCamera)this.camera);
                    if (retVal != true)
                    {
                        acq.Image.Dispose();
                        acq.GrabStatus = "GrabFail";
                        return acq;
                    }
                }
                //ManagedImage rawImage = new ManagedImage();
                try
                {
                    // Retrieve an image
                    //Thread.Sleep(100);
                    this.camera.RetrieveBuffer(rawImage);
                }
                catch (Exception)
                {
                    acq.Image.Dispose();
                    acq.GrabStatus = "GrabFail";
                    return acq;
                }
                //ManagedImage convertImage = new ManagedImage();
                rawImage.Convert((FlyCapture2Managed.PixelFormat)_pixelFormat, convertImage);
                unsafe
                {
                    byte* p = convertImage.data;
                    IntPtr ptr = (IntPtr)p;
                    if (_pixelFormat.ToString().Contains("Mono"))//黑白格式
                    {
                        this.image.Dispose();
                        HOperatorSet.GenImage1(out this.image, "byte", rawImage.cols, rawImage.rows, ptr);
                    }
                    else//彩色格式
                    {
                        this.image.Dispose();
                    }
                }
                acq.Image.Dispose();
                acq.Image = this.image.CopyObj(1, -1);
                acq.GrabStatus = "GrabPass";
                return acq;
            }
            catch (Exception)
            {
                acq.Image.Dispose();
                acq.GrabStatus = "GrabFail";
                return acq;
            }
        }
    }
    public class BaslerMtxCamLinkToHalcon : BaseCamera
    {
        class UserHookData
        {
            public MIL_ID MilDigitizer;
            public MIL_ID MilImage;
        }
        #region 私有变量
        private HObject image;
        private Camera camera = null;
        private string sn = "";
        private string pixelType = BaslerMtxCamLinkToHalcon.PixelFormat.Mono8;
        private bool isSoftwareTrigger = true;
        private bool isMirrorX = false;
        private bool isMirrorY = false;
        System.Collections.Concurrent.ConcurrentQueue<Acquisition> acqQueue = new System.Collections.Concurrent.ConcurrentQueue<Acquisition>();
        static object lockObj = new object();
        private bool initMatrox = true;
        //matrox val 
        private MIL_ID milApplication = MIL.M_NULL;     // Application identifier.
        private MIL_ID milSystem = MIL.M_NULL;          // System identifier.
        //private MIL_ID milDisplay = MIL.M_NULL;         // Display identifier.
        private MIL_ID milDigitizer = MIL.M_NULL;           // Digitizer identifier.
        private MIL_ID milImage = MIL.M_NULL;           // Image buffer identifier.
        private MIL_INT imageDataPtr = MIL.M_NULL;
        private MIL_INT imageSizeX = 0;
        private MIL_INT imageSizeY = 0;
        private MIL_INT SrcImageType = 0;
        private MIL_INT SrcImagePitchByte = 0;
        private const int FUNCTION_SUPPORTED_IMAGE_TYPE = (8 + MIL.M_UNSIGNED);
        private const int BUFFERING_SIZE_MAX = 400;
        MIL_ID[] buffers = new MIL_ID[BUFFERING_SIZE_MAX];
        int num = 0;
        private UserHookData userHookData = new UserHookData();
        GCHandle hUserData;
        MIL_DIG_HOOK_FUNCTION_PTR hookFunctionDelegate;

        MIL_ID[] MilGrabBufferList = new MIL_ID[BUFFERING_SIZE_MAX];
        MIL_INT MilGrabBufferListSize;
        MIL_INT n = 0;
        //UInt64[] BIT_MASK = new UInt64[1] { 65536 };
        #endregion

        static Version Sfnc2_0_0 = new Version(2, 0, 0);
        static PLCamera.PixelFormatEnum PixelFormat = new PLCamera.PixelFormatEnum();
        static PLCamera.SequencerTriggerSourceEnum TriggerSource = new PLCamera.SequencerTriggerSourceEnum();
        public BaslerMtxCamLinkToHalcon(string serialNumber)
        {
            this.sn = serialNumber;
            HOperatorSet.GenEmptyObj(out image);
        }
        public override bool InitCamera()
        {
            try
            {
                if (camera == null)
                {
                    camera = new Camera(sn);
                    initMatrox = true;
                    //grabThread = new Thread(new ThreadStart(GrabFunction));
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }
        public override bool OpenCamera()
        {
            try
            {
                if (camera.IsOpen)
                {
                    CloseCamera();
                    Thread.Sleep(50);
                }

                //软触发使用
                if (this.isSoftwareTrigger) camera.CameraOpened += Configuration.SoftwareTrigger;
                camera.Open();
                camera.Parameters[BaslerMtxCamLinkToHalcon.PixelFormat].TrySetValue(this.pixelType);
                camera.Parameters[PLCamera.ReverseX].SetValue(this.isMirrorX);
                camera.Parameters[PLCamera.TriggerMode].SetValue("On");
                //硬触发使用`
                if (!this.isSoftwareTrigger) camera.Parameters[PLCamera.TriggerSource].SetValue("Line1");
                ////camera.Parameters[PLCamera.LineSelector].SetValue("Line1");

                if (!initMatrox) return true;

                //Allocate a default MIL application, system, display and image.
                MIL.MappAllocDefault(MIL.M_DEFAULT, ref milApplication, ref milSystem, MIL.M_NULL, ref milDigitizer, ref milImage);
                // If no allocation errors.
                if (MIL.MappGetError(MIL.M_DEFAULT, MIL.M_GLOBAL, MIL.M_NULL) != 0)
                {
                    return false;
                }

                // Allocate the grab buffers and clear them.
                MIL.MappControl(MIL.M_DEFAULT, MIL.M_ERROR, MIL.M_PRINT_DISABLE);
                for (MilGrabBufferListSize = 0; MilGrabBufferListSize < BUFFERING_SIZE_MAX; MilGrabBufferListSize++)
                {
                    MIL.MbufAllocColor(milSystem, MIL.MdigInquire(milDigitizer, MIL.M_SIZE_BAND, MIL.M_NULL),
                                   MIL.MdigInquire(milDigitizer, MIL.M_SIZE_X, MIL.M_NULL),
                                   MIL.MdigInquire(milDigitizer, MIL.M_SIZE_Y, MIL.M_NULL),
                                   8 + MIL.M_UNSIGNED, MIL.M_IMAGE + MIL.M_GRAB + MIL.M_PROC,
                                   ref MilGrabBufferList[MilGrabBufferListSize]);

                    if (MilGrabBufferList[MilGrabBufferListSize] != MIL.M_NULL)
                    {
                        MIL.MbufClear(MilGrabBufferList[MilGrabBufferListSize], 0);
                    }
                    else
                    {
                        break;
                    }
                }
                // Free buffers to leave space for possible temporary buffers.
                for (n = 0; n < 0 && MilGrabBufferListSize > 0; n++)
                {
                    MilGrabBufferListSize--;
                    MIL.MbufFree(MilGrabBufferList[MilGrabBufferListSize]);
                }

                if (MilGrabBufferListSize == 0)
                {
                    //Console.WriteLine("!!! No grab buffers have been allocated. Need to set more Non-Paged Memory. !!!");

                    //MIL.MappFreeDefault(MilApplication, MilSystem, MilDisplay, MilDigitizer, MilImageDisp);
                    //Console.WriteLine("Press <Enter> to end.");
                    //Console.ReadKey();
                    //return 1;
                }

                //MIL.MappControlMp(MIL.M_DEFAULT, MIL.M_CORE_AFFINITY_MASK, MIL.M_DEFAULT, MIL.M_USER_DEFINED, BIT_MASK);
                userHookData.MilDigitizer = this.milDigitizer;
                userHookData.MilImage = this.milImage;
                // get a handle to the HookDataStruct object in the managed heap, we will use this 
                // handle to get the object back in the callback function
                hUserData = GCHandle.Alloc(userHookData);

                // Start the sequence acquisition. The preprocessing and encoding hook function 
                // is called for every frame grabbed.
                hookFunctionDelegate = new MIL_DIG_HOOK_FUNCTION_PTR(HookFunction);

                // Hook a function to the start of each frame to print the current frame index.
                //MIL.MdigHookFunction(milDigitizer, MIL.M_GRAB_START, hookFunctionDelegate, GCHandle.ToIntPtr(hUserData));
                /* Start a digProcess to show the live camera output. */
                MIL.MdigProcess(milDigitizer, MilGrabBufferList, MilGrabBufferListSize, MIL.M_START, MIL.M_DEFAULT, hookFunctionDelegate, GCHandle.ToIntPtr(hUserData));
                // Grab continuously.
                MIL.MdigGrabContinuous(milDigitizer, milImage);
                //MIL.MbufClear(milImage, 0);

                return true;
            }
            catch (Exception)
            {
                CloseCamera();
                return false;
            }
        }
        public override bool CloseCamera()
        {
            try
            {
                if (camera == null) return true;
                if (camera.IsOpen)
                {
                    if (this.isSoftwareTrigger) camera.CameraOpened -= Configuration.SoftwareTrigger;
                    camera.Close();
                }
                if (!initMatrox) return true;

                MIL.MdigHalt(milDigitizer);
                MIL.MdigProcess(milDigitizer, MilGrabBufferList, MilGrabBufferListSize, MIL.M_STOP, MIL.M_DEFAULT, hookFunctionDelegate, GCHandle.ToIntPtr(hUserData));
                if (hUserData.IsAllocated) hUserData.Free();
                // Free defaults.
                MIL.MappFreeDefault(milApplication, milSystem, MIL.M_NULL, milDigitizer, milImage);
                milApplication = MIL.M_NULL;
                milSystem = MIL.M_NULL;
                milDigitizer = MIL.M_NULL;
                milImage = MIL.M_NULL;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public override bool IsSoftwareTrigger
        {
            set { this.isSoftwareTrigger = value; }
            get { return this.isSoftwareTrigger; }
        }
        public override bool IsConnected
        {
            get
            {
                return this.camera.IsConnected;
            }
        }
        public override bool IsMirrorX
        {
            set { this.isMirrorX = value; }
            get { return this.isMirrorX; }
        }
        public override bool IsMirrorY
        {
            set { this.isMirrorY = value; }
            get { return this.isMirrorY; }
        }
        public override bool ChangeTriggerSource(bool isSoftwareTrigger)
        {
            if (isSoftwareTrigger == this.isSoftwareTrigger) return true;
            initMatrox = false;
            if (!CloseCamera()) return false;
            this.isSoftwareTrigger = isSoftwareTrigger;
            if (!OpenCamera())
            {
                this.isSoftwareTrigger = !isSoftwareTrigger;
                return false;
            }

            return true;
        }
        public override bool SetGain(Double value)
        {
            try
            {
                // Some camera models may have auto functions enabled. To set the gain value to a specific value,
                // the Gain Auto function must be disabled first (if gain auto is available).
                if (!camera.Parameters[PLCamera.GainAuto].TrySetValue(PLCamera.GainAuto.Off)) // Set GainAuto to Off if it is writable.
                    return false;
                // Features, e.g. 'Gain', are named according to the GenICam Standard Feature Naming Convention (SFNC).
                // The SFNC defines a common set of features, their behavior, and the related parameter names. 
                // This ensures the interoperability of cameras from different camera vendors. 
                // Cameras compliant with the USB3 Vision standard are based on the SFNC version 2.0.
                // Basler GigE and Firewire cameras are based on previous SFNC versions.
                // Accordingly, the behavior of these cameras and some parameters names will be different.
                // The SFNC version can be used to handle differences between camera device models.
                if (camera.GetSfncVersion() < Sfnc2_0_0)
                {
                    // In previous SFNC versions, GainRaw is an integer parameter.
                    camera.Parameters[PLCamera.GainRaw].SetValuePercentOfRange(Convert.ToInt32(value));
                }
                else // For SFNC 2.0 cameras, e.g. USB3 Vision cameras
                {
                    // In SFNC 2.0, Gain is a float parameter.
                    camera.Parameters[PLUsbCamera.Gain].SetValuePercentOfRange(value);
                    // For USB cameras, Gamma is always enabled.
                }
                return true;
            }
            catch (Exception)
            {
                if (camera != null && camera.IsOpen)
                    camera.Parameters[PLCamera.GainAuto].TrySetValue(PLCamera.GainAuto.Continuous);
                return false;
            }

        }
        public override void GetGain(out Double value)
        {
            try
            {
                if (camera.GetSfncVersion() < Sfnc2_0_0)
                {
                    // In previous SFNC versions, GainRaw is an integer parameter.
                    value = camera.Parameters[PLCamera.GainRaw].GetValuePercentOfRange();
                }
                else // For SFNC 2.0 cameras, e.g. USB3 Vision cameras
                {
                    // In SFNC 2.0, Gain is a float parameter.
                    value = camera.Parameters[PLUsbCamera.Gain].GetValuePercentOfRange();
                    // For USB cameras, Gamma is always enabled.
                }
            }
            catch (Exception)
            {
                value = -1;
            }
        }
        public override bool SetExposure(Double value)
        {
            try
            {
                if (!camera.Parameters[PLCamera.ExposureAuto].TrySetValue(PLCamera.ExposureAuto.Off))
                    return false;
                if (camera.GetSfncVersion() < Sfnc2_0_0)
                {
                    return camera.Parameters[PLCamera.ExposureTimeRaw].TrySetValue(Convert.ToInt32(value));
                }
                else
                {
                    return camera.Parameters[PLCamera.ExposureTime].TrySetValue(value);
                }

            }
            catch (Exception)
            {
                //camera.Parameters[PLCamera.ExposureTime].TrySetValue(100);
                return false;
            }
        }
        public override void GetExposure(out Double value)
        {
            try
            {
                if (camera.GetSfncVersion() < Sfnc2_0_0)
                {
                    value = camera.Parameters[PLCamera.ExposureTimeRaw].GetValue();
                }
                else
                {
                    value = camera.Parameters[PLCamera.ExposureTime].GetValue();
                }
            }
            catch (Exception)
            {
                value = -1;
            }

        }
        public void SetGamma(bool value)
        {
            try
            {
                // GammaEnable is a boolean parameter.
                camera.Parameters[PLCamera.GammaEnable].TrySetValue(value);    //if available
            }
            catch (Exception)
            {
                if (camera != null)
                    camera.Parameters[PLCamera.GammaEnable].TrySetValue(true);
            }
        }
        public void GetGamma(out bool value)
        {
            try
            {
                value = camera.Parameters[PLCamera.GammaEnable].GetValue();
            }
            catch (Exception)
            {
                value = false;
            }
        }
        /// <summary>设置相机的像素格式
        /// 设置相机的像素格式
        /// </summary>
        /// <param name="value">value可以设置为如:BaslerToHalcon.PixelFormat.Mono8</param>
        public override bool BaslerSetPixelFormat(string value)
        {
            //value = BaslerToHalcon.PixelFormat.Mono8; 

            try
            {
                this.pixelType = value;
                return true;
                //return camera.Parameters[BaslerToHalcon.PixelFormat].TrySetValue(value);
            }
            catch (Exception)
            {
                //camera.Parameters[BaslerToHalcon.PixelFormat].TrySetValue(BaslerToHalcon.PixelFormat.Mono8);
                return false;
            }

        }
        public override void BaslerGetPixelFormat(out string value)
        {
            try
            {
                value = camera.Parameters[BaslerMtxCamLinkToHalcon.PixelFormat].GetValue();
            }
            catch (Exception)
            {
                value = "";
            }

        }
        public override bool ClearImageQueue()
        {
            try
            {
                while (this.acqQueue.Count > 0)
                {
                    //this.acqQueue.Dequeue().Image.Dispose();
                    Acquisition acq;
                    acqQueue.TryDequeue(out acq);
                    acq.Dispose();
                }
                num = 0;
                for (int j = 0; j < buffers.Length; j++)
                {
                    //if (buffers[j] == MIL.M_NULL) break;
                    MIL.MbufClear(buffers[j], 0);
                    buffers[j] = MIL.M_NULL;
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public override Acquisition Snap(int imageNum = 1, int timeOut = -1)
        {
            Acquisition _acq = new Acquisition();
            try
            {
                timeOut = (timeOut == -1) ? 100000 : timeOut;
                ICommandParameterExtensions.TryExecute(camera.Parameters[PLCamera.TriggerSoftware]);
                DateTime t1 = DateTime.Now;

                for (int i = 0; i < buffers.Length; i++)
                {
                    if (DateTime.Now.Subtract(t1).TotalMilliseconds > timeOut)//采集超时500ms
                    {
                        for (int j = i; j < buffers.Length; j++)
                        {
                            if (buffers[j] == MIL.M_NULL) break;
                            MIL.MbufClear(buffers[j], 0);
                            buffers[j] = MIL.M_NULL;
                        }
                        _acq.GrabStatus = "GrabFail:TimeOut";
                        return _acq;
                    }
                    if (_acq.index + 1 == imageNum)
                    {
                        return _acq;
                    }
                    if (buffers[i] == MIL.M_NULL)
                    {
                        i--;
                        continue;
                    }
                    try
                    {
                        // Lock buffers for direct access.
                        MIL.MbufControl(buffers[i], MIL.M_LOCK, MIL.M_DEFAULT);
                        // get image information.
                        MIL.MbufInquire(buffers[i], MIL.M_HOST_ADDRESS, ref imageDataPtr);
                        MIL.MbufInquire(buffers[i], MIL.M_SIZE_X, ref imageSizeX);
                        MIL.MbufInquire(buffers[i], MIL.M_SIZE_Y, ref imageSizeY);
                        MIL.MbufInquire(buffers[i], MIL.M_TYPE, ref SrcImageType);
                        MIL.MbufInquire(buffers[i], MIL.M_PITCH_BYTE, ref SrcImagePitchByte);

                        if (imageDataPtr != MIL.M_NULL && SrcImageType == FUNCTION_SUPPORTED_IMAGE_TYPE)
                        {
                            HObject img = new HObject();
                            img.Dispose();
                            unsafe
                            {
                                //lock (this)
                                {
                                    IntPtr ptr = imageDataPtr;
                                    if (this.pixelType == BaslerMtxCamLinkToHalcon.PixelFormat.Mono8)
                                    {
                                        //黑白图像
                                        //HOperatorSet.GenImage1(out image, "byte", Convert.ToInt32(imageSizeX), Convert.ToInt32(imageSizeY), ptr);
                                        HOperatorSet.GenImage1Rect(out img, ptr, Convert.ToInt32(imageSizeX), Convert.ToInt32(imageSizeY), Convert.ToInt32(SrcImagePitchByte), 8, 8, "true", 0);
                                        //byte a = *((byte *) ptr);
                                        //HOperatorSet.CropPart(image, out image, 0, 0, Convert.ToInt32(imageSizeX), Convert.ToInt32(imageSizeY));
                                        //HOperatorSet.WriteImage(image, "tiff", 0, "E:\\test.tiff");
                                    }
                                    else if (!this.pixelType.Contains("Mono"))
                                    {
                                        //RGB彩色图像
                                        //HOperatorSet.GenImageInterleaved(out image, ptr, "bgrx", e.GrabResult.Width, e.GrabResult.Height, -1, "byte", 0, 0, 0, 0, -1, 0);
                                    }
                                    else
                                    {
                                        _acq.GrabStatus = "current pixel format not support!";
                                        return _acq;
                                    }
                                }
                            }
                            HOperatorSet.ConcatObj(_acq.Image, img, out _acq.Image);
                            _acq.GrabStatus = "GrabPass";
                            _acq.index++;
                            //HOperatorSet.WriteImage(img, "tiff", 0, "E:\\test.tiff");
                        }
                        else
                        {
                            _acq.GrabStatus = "GrabFail";
                            return _acq;
                        }
                    }
                    catch (Exception ex)
                    {
                        _acq.GrabStatus = "GrabFail:" + ex.Message;
                        return _acq;
                    }
                    finally
                    {
                        // Unlock buffers.
                        MIL.MbufControl(buffers[i], MIL.M_UNLOCK, MIL.M_DEFAULT);
                        // Clear the buffer.
                        MIL.MbufClear(buffers[i], 0);
                        buffers[i] = MIL.M_NULL;
                    }
                }
                return _acq;

            }
            catch (Exception ex)
            {
                _acq.GrabStatus = "GrabFail:" + ex.Message;
                return _acq;
            }
            finally
            {
                num = 0;
            }
        }
        public override Acquisition GetFrames(int imageNum = 1, int timeOut = -1)
        {
            Acquisition _acq = new Acquisition();
            try
            {
                timeOut = (timeOut == -1) ? 100000 : timeOut;
                DateTime t1 = DateTime.Now;

                for (int i = 0; i < buffers.Length; i++)
                {
                    if (DateTime.Now.Subtract(t1).TotalMilliseconds > timeOut)//采集超时500ms
                    {
                        for (int j = i; j < buffers.Length; j++)
                        {
                            if (buffers[j] == MIL.M_NULL) break;
                            MIL.MbufClear(buffers[j], 0);
                            buffers[j] = MIL.M_NULL;
                        }
                        _acq.GrabStatus = "GrabFail:TimeOut";
                        return _acq;
                    }
                    if (_acq.index + 1 == imageNum)
                    {
                        return _acq;
                    }
                    if (buffers[i] == MIL.M_NULL)
                    {
                        i--;
                        continue;
                    }
                    try
                    {
                        // Lock buffers for direct access.
                        MIL.MbufControl(buffers[i], MIL.M_LOCK, MIL.M_DEFAULT);
                        // get image information.
                        MIL.MbufInquire(buffers[i], MIL.M_HOST_ADDRESS, ref imageDataPtr);
                        MIL.MbufInquire(buffers[i], MIL.M_SIZE_X, ref imageSizeX);
                        MIL.MbufInquire(buffers[i], MIL.M_SIZE_Y, ref imageSizeY);
                        MIL.MbufInquire(buffers[i], MIL.M_TYPE, ref SrcImageType);
                        MIL.MbufInquire(buffers[i], MIL.M_PITCH_BYTE, ref SrcImagePitchByte);

                        if (imageDataPtr != MIL.M_NULL && SrcImageType == FUNCTION_SUPPORTED_IMAGE_TYPE)
                        {
                            HObject img = new HObject();
                            img.Dispose();
                            unsafe
                            {
                                //lock (this)
                                {
                                    IntPtr ptr = imageDataPtr;
                                    if (this.pixelType == BaslerMtxCamLinkToHalcon.PixelFormat.Mono8)
                                    {
                                        //黑白图像
                                        //HOperatorSet.GenImage1(out image, "byte", Convert.ToInt32(imageSizeX), Convert.ToInt32(imageSizeY), ptr);
                                        HOperatorSet.GenImage1Rect(out img, ptr, Convert.ToInt32(imageSizeX), Convert.ToInt32(imageSizeY), Convert.ToInt32(SrcImagePitchByte), 8, 8, "true", 0);
                                        //byte a = *((byte*)ptr);
                                        //HOperatorSet.CropPart(image, out image, 0, 0, Convert.ToInt32(imageSizeX), Convert.ToInt32(imageSizeY));
                                        //HOperatorSet.WriteImage(img, "tiff", 0, "E:\\test.tiff");
                                    }
                                    else if (!this.pixelType.Contains("Mono"))
                                    {
                                        //RGB彩色图像
                                        //HOperatorSet.GenImageInterleaved(out image, ptr, "bgrx", e.GrabResult.Width, e.GrabResult.Height, -1, "byte", 0, 0, 0, 0, -1, 0);
                                    }
                                    else
                                    {
                                        //if (acqQueue != null)
                                        //{
                                        //    _acq.Dispose();
                                        //    _acq.GrabStatus = "current pixel format not support!";
                                        //    acqQueue.Enqueue(_acq);
                                        //    mf.time = DateTime.Now.Subtract(start).TotalMilliseconds;
                                        //    return -1;
                                        //}
                                        _acq.GrabStatus = "current pixel format not support!";
                                        return _acq;
                                    }
                                }
                            }
                            //if (acqQueue != null)
                            //{
                            //    _acq.Dispose();
                            //    _acq.Image = image.CopyObj(1, -1);
                            //    _acq.GrabStatus = "GrabPass";
                            //    mf.time = DateTime.Now.Subtract(start).TotalMilliseconds;
                            //    acqQueue.Enqueue(_acq);
                            //}                            
                            HOperatorSet.ConcatObj(_acq.Image, img, out _acq.Image);
                            _acq.GrabStatus = "GrabPass";
                            _acq.index++;
                            //HOperatorSet.WriteImage(img, "tiff", 0, "E:\\test.tiff");
                        }
                        else
                        {
                            //if (acqQueue != null)
                            //{
                            //    _acq.Dispose();
                            //    _acq.GrabStatus = "GrabFail";
                            //    acqQueue.Enqueue(_acq);
                            //    mf.time = DateTime.Now.Subtract(start).TotalMilliseconds;
                            //    return -1;
                            //}
                            _acq.GrabStatus = "GrabFail";
                            return _acq;
                        }
                    }
                    catch (Exception ex)
                    {
                        //if (acqQueue != null)
                        //{
                        //    //_acq.Dispose();
                        //    //_acq.GrabStatus = "GrabFail:" + ex.Message;
                        //    //acqQueue.Enqueue(_acq);
                        //    //mf.time = DateTime.Now.Subtract(start).TotalMilliseconds;
                        //    //return -1;
                        //}
                        _acq.GrabStatus = "GrabFail:" + ex.Message;
                        return _acq;
                    }
                    finally
                    {
                        // Unlock buffers.
                        MIL.MbufControl(buffers[i], MIL.M_UNLOCK, MIL.M_DEFAULT);
                        // Clear the buffer.
                        MIL.MbufClear(buffers[i], 0);
                        buffers[i] = MIL.M_NULL;
                    }
                }
                return _acq;
            }
            catch (Exception ex)
            {
                _acq.GrabStatus = "GrabFail:" + ex.Message;
                return _acq;
            }
            finally
            {
                num = 0;
            }
        }
        public override Task<Acquisition> GetFramesAsync(int imageNum = 1, int timeOut = -1)
        {
            return Task.Run(() =>
            {
                return GetFrames(imageNum, timeOut);
            });
        }
        private MIL_INT HookFunction(MIL_INT HookType, MIL_ID HookId, IntPtr HookDataPtr)
        {
            // this is how to check if the user data is null, the IntPtr class
            // contains a member, Zero, which exists solely for this purpose
            if (!IntPtr.Zero.Equals(HookDataPtr))
            {
                // get the handle to the ProcessingHookDataStruct object back from the IntPtr
                GCHandle hUserData = GCHandle.FromIntPtr(HookDataPtr);

                // get a reference to the ProcessingHookDataStruct object
                UserHookData UserHookDataPtr = hUserData.Target as UserHookData;
                MIL_ID ModifiedBufferId = MIL.M_NULL;

                // Retrieve the MIL_ID of the grabbed buffer.
                MIL.MdigGetHookInfo(HookId, MIL.M_MODIFIED_BUFFER + MIL.M_BUFFER_ID, ref ModifiedBufferId);

                buffers[num] = ModifiedBufferId;
                num++;
                return 0;
            }
            else
            {
                return -1;
            }
            //}

        }
    }
    public class IOMtxCamLinkToHalcon : BaseCamera
    {
        class UserHookData
        {
            public MIL_ID MilDigitizer;
            public MIL_ID MilImage;
        }
        #region 私有变量
        private HObject image;
        //private Camera camera = null;
        private string sn = "";
        //private string pixelType = IOMtxCamLinkToHalcon.PixelFormat.Mono8;
        private bool isSoftwareTrigger = true;
        private bool isMirrorX = false;
        private bool isMirrorY = false;
        System.Collections.Concurrent.ConcurrentQueue<Acquisition> acqQueue = new System.Collections.Concurrent.ConcurrentQueue<Acquisition>();
        static object lockObj = new object();
        private bool initMatrox = true;
        //matrox val 
        private MIL_ID milApplication = MIL.M_NULL;     // Application identifier.
        private MIL_ID milSystem = MIL.M_NULL;          // System identifier.
        //private MIL_ID milDisplay = MIL.M_NULL;         // Display identifier.
        private MIL_ID milDigitizer = MIL.M_NULL;           // Digitizer identifier.
        private MIL_ID milImage = MIL.M_NULL;           // Image buffer identifier.
        private MIL_INT imageDataPtr = MIL.M_NULL;
        private StringBuilder milPixelFormat;
        private MIL_INT imageSizeX = 0;
        private MIL_INT imageSizeY = 0;
        private MIL_INT SrcImageType = 0;
        private MIL_INT SrcImagePitchByte = 0;
        private const int FUNCTION_SUPPORTED_IMAGE_TYPE = (8 + MIL.M_UNSIGNED);
        private const int BUFFERING_SIZE_MAX = 400;
        MIL_ID[] buffers = new MIL_ID[BUFFERING_SIZE_MAX];
        int num = 0;
        private UserHookData userHookData = new UserHookData();
        GCHandle hUserData;
        MIL_DIG_HOOK_FUNCTION_PTR hookFunctionDelegate;

        MIL_ID[] MilGrabBufferList = new MIL_ID[BUFFERING_SIZE_MAX];
        MIL_INT MilGrabBufferListSize;
        MIL_INT n = 0;
        //UInt64[] BIT_MASK = new UInt64[1] { 65536 };
        #endregion

        //static Version Sfnc2_0_0 = new Version(2, 0, 0);
        //static PLCamera.PixelFormatEnum PixelFormat = new PLCamera.PixelFormatEnum();
        //static PLCamera.SequencerTriggerSourceEnum TriggerSource = new PLCamera.SequencerTriggerSourceEnum();

        System.IO.Ports.SerialPort spIO = null;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="serialNumber">串口端口号，非相机序列号</param>
        public IOMtxCamLinkToHalcon(string serialNumber)
        {
            this.sn = serialNumber;
            HOperatorSet.GenEmptyObj(out image);
        }
        public override bool InitCamera()
        {
            try
            {
                if (spIO == null)
                {
                    spIO = new System.IO.Ports.SerialPort();
                    spIO.PortName = this.sn;
                    spIO.BaudRate = 9600;
                    spIO.DataBits = 8;
                    spIO.StopBits = System.IO.Ports.StopBits.One;
                    spIO.Parity = System.IO.Ports.Parity.None;
                    initMatrox = true;
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }
        public override bool OpenCamera()
        {
            try
            {
                if (spIO.IsOpen)
                {
                    CloseCamera();
                    Thread.Sleep(50);
                }
                spIO.Open();

                if (!initMatrox) return true;

                //Allocate a default MIL application, system, display and image.
                MIL.MappAllocDefault(MIL.M_DEFAULT, ref milApplication, ref milSystem, MIL.M_NULL, ref milDigitizer, ref milImage);
                // If no allocation errors.
                if (MIL.MappGetError(MIL.M_DEFAULT, MIL.M_GLOBAL, MIL.M_NULL) != 0)
                {
                    return false;
                }

                // Allocate the grab buffers and clear them.
                MIL.MappControl(MIL.M_DEFAULT, MIL.M_ERROR, MIL.M_PRINT_DISABLE);
                for (MilGrabBufferListSize = 0; MilGrabBufferListSize < BUFFERING_SIZE_MAX; MilGrabBufferListSize++)
                {
                    MIL.MbufAllocColor(milSystem, MIL.MdigInquire(milDigitizer, MIL.M_SIZE_BAND, MIL.M_NULL),
                                   MIL.MdigInquire(milDigitizer, MIL.M_SIZE_X, MIL.M_NULL),
                                   MIL.MdigInquire(milDigitizer, MIL.M_SIZE_Y, MIL.M_NULL),
                                   8 + MIL.M_UNSIGNED, MIL.M_IMAGE + MIL.M_GRAB + MIL.M_PROC,
                                   ref MilGrabBufferList[MilGrabBufferListSize]);

                    if (MilGrabBufferList[MilGrabBufferListSize] != MIL.M_NULL)
                    {
                        MIL.MbufClear(MilGrabBufferList[MilGrabBufferListSize], 0);
                    }
                    else
                    {
                        break;
                    }
                }
                // Free buffers to leave space for possible temporary buffers.
                for (n = 0; n < 0 && MilGrabBufferListSize > 0; n++)
                {
                    MilGrabBufferListSize--;
                    MIL.MbufFree(MilGrabBufferList[MilGrabBufferListSize]);
                }

                if (MilGrabBufferListSize == 0)
                {
                    //Console.WriteLine("!!! No grab buffers have been allocated. Need to set more Non-Paged Memory. !!!");

                    //MIL.MappFreeDefault(MilApplication, MilSystem, MilDisplay, MilDigitizer, MilImageDisp);
                    //Console.WriteLine("Press <Enter> to end.");
                    //Console.ReadKey();
                    //return 1;
                }

                //MIL.MappControlMp(MIL.M_DEFAULT, MIL.M_CORE_AFFINITY_MASK, MIL.M_DEFAULT, MIL.M_USER_DEFINED, BIT_MASK);
                userHookData.MilDigitizer = this.milDigitizer;
                userHookData.MilImage = this.milImage;
                // get a handle to the HookDataStruct object in the managed heap, we will use this 
                // handle to get the object back in the callback function
                hUserData = GCHandle.Alloc(userHookData);

                // Start the sequence acquisition. The preprocessing and encoding hook function 
                // is called for every frame grabbed.
                hookFunctionDelegate = new MIL_DIG_HOOK_FUNCTION_PTR(HookFunction);

                // Hook a function to the start of each frame to print the current frame index.
                //MIL.MdigHookFunction(milDigitizer, MIL.M_GRAB_START, hookFunctionDelegate, GCHandle.ToIntPtr(hUserData));
                /* Start a digProcess to show the live camera output. */
                MIL.MdigProcess(milDigitizer, MilGrabBufferList, MilGrabBufferListSize, MIL.M_START, MIL.M_DEFAULT, hookFunctionDelegate, GCHandle.ToIntPtr(hUserData));
                // Grab continuously.
                MIL.MdigGrabContinuous(milDigitizer, milImage);
                //MIL.MbufClear(milImage, 0);
                //MIL_INT len;
                //MIL.MdigInquireFeature(milDigitizer, MIL.M_FEATURE_VALUE_AS_STRING + MIL.M_STRING_SIZE,
                //                       "PixelFormat", MIL.M_DEFAULT, len);
                //milPixelFormat = new MIL_ID[len];
                milPixelFormat = null;
                milPixelFormat = new StringBuilder();
                MIL.MdigInquireFeature(milDigitizer, MIL.M_FEATURE_VALUE_AS_STRING,
                                       "PixelFormat", MIL.M_DEFAULT, milPixelFormat);

                return true;
            }
            catch (Exception)
            {
                CloseCamera();
                return false;
            }
        }
        public override bool CloseCamera()
        {
            try
            {
                if (spIO == null) return true;
                if (spIO.IsOpen)
                {
                    spIO.Close();
                }
                if (!initMatrox) return true;

                MIL.MdigHalt(milDigitizer);
                MIL.MdigProcess(milDigitizer, MilGrabBufferList, MilGrabBufferListSize, MIL.M_STOP, MIL.M_DEFAULT, hookFunctionDelegate, GCHandle.ToIntPtr(hUserData));
                if (hUserData.IsAllocated) hUserData.Free();
                // Free defaults.
                MIL.MappFreeDefault(milApplication, milSystem, MIL.M_NULL, milDigitizer, milImage);
                milApplication = MIL.M_NULL;
                milSystem = MIL.M_NULL;
                milDigitizer = MIL.M_NULL;
                milImage = MIL.M_NULL;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public override bool IsSoftwareTrigger
        {
            set { this.isSoftwareTrigger = value; }
            get { return this.isSoftwareTrigger; }
        }
        public override bool IsConnected
        {
            get
            {
                return false;
            }
        }
        public override bool IsMirrorX
        {
            set { this.isMirrorX = value; }
            get { return this.isMirrorX; }
        }
        public override bool IsMirrorY
        {
            set { this.isMirrorY = value; }
            get { return this.isMirrorY; }
        }
        public override bool ChangeTriggerSource(bool isSoftwareTrigger)
        {
            if (isSoftwareTrigger == this.isSoftwareTrigger) return true;
            initMatrox = false;
            if (!CloseCamera()) return false;
            this.isSoftwareTrigger = isSoftwareTrigger;
            if (!OpenCamera())
            {
                this.isSoftwareTrigger = !isSoftwareTrigger;
                return false;
            }

            return true;
        }
        public override bool SetGain(Double value)
        {
            try
            {

                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }
        public override void GetGain(out Double value)
        {
            try
            {
                value = -1;
            }
            catch (Exception)
            {
                value = -1;
            }
        }
        public override bool SetExposure(Double value)
        {
            try
            {
                string val = String.Format("ex1 {0}", Convert.ToInt32(value).ToString("x8"));
                spIO.Write(val);

                return false;
            }
            catch (Exception)
            {
                //camera.Parameters[PLCamera.ExposureTime].TrySetValue(100);
                return false;
            }
        }
        public override void GetExposure(out Double value)
        {
            try
            {
                value = -1;
            }
            catch (Exception)
            {
                value = -1;
            }

        }
        public void SetGamma(bool value)
        {

        }
        public void GetGamma(out bool value)
        {
            try
            {
                value = false;
            }
            catch (Exception)
            {
                value = false;
            }
        }
        /// <summary>设置相机的像素格式
        /// 设置相机的像素格式
        /// </summary>
        /// <param name="value">value可以设置为如:BaslerToHalcon.PixelFormat.Mono8</param>
        public override bool BaslerSetPixelFormat(string value)
        {
            //value = BaslerToHalcon.PixelFormat.Mono8; 

            try
            {
                //this.pixelType = value;
                return false;
                //return camera.Parameters[BaslerToHalcon.PixelFormat].TrySetValue(value);
            }
            catch (Exception)
            {
                //camera.Parameters[BaslerToHalcon.PixelFormat].TrySetValue(BaslerToHalcon.PixelFormat.Mono8);
                return false;
            }

        }
        public override void BaslerGetPixelFormat(out string value)
        {
            try
            {
                value = "";
            }
            catch (Exception)
            {
                value = "";
            }

        }
        public override bool ClearImageQueue()
        {
            try
            {
                while (this.acqQueue.Count > 0)
                {
                    //this.acqQueue.Dequeue().Image.Dispose();
                    Acquisition acq;
                    acqQueue.TryDequeue(out acq);
                    acq.Dispose();
                }
                num = 0;
                for (int j = 0; j < buffers.Length; j++)
                {
                    //if (buffers[j] == MIL.M_NULL) break;
                    MIL.MbufClear(buffers[j], 0);
                    buffers[j] = MIL.M_NULL;
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public override Acquisition Snap(int imageNum = 1, int timeOut = -1)
        {
            Acquisition _acq = new Acquisition();
            _acq.GrabStatus = "GrabFail";
            return _acq;
            //try
            //{
            //    timeOut = (timeOut == -1) ? 100000 : timeOut;
            //    ICommandParameterExtensions.TryExecute(camera.Parameters[PLCamera.TriggerSoftware]);
            //    DateTime t1 = DateTime.Now;

            //    for (int i = 0; i < buffers.Length; i++)
            //    {
            //        if (DateTime.Now.Subtract(t1).TotalMilliseconds > timeOut)//采集超时500ms
            //        {
            //            for (int j = i; j < buffers.Length; j++)
            //            {
            //                if (buffers[j] == MIL.M_NULL) break;
            //                MIL.MbufClear(buffers[j], 0);
            //                buffers[j] = MIL.M_NULL;
            //            }
            //            _acq.GrabStatus = "GrabFail:TimeOut";
            //            return _acq;
            //        }
            //        if (_acq.index + 1 == imageNum)
            //        {
            //            return _acq;
            //        }
            //        if (buffers[i] == MIL.M_NULL)
            //        {
            //            i--;
            //            continue;
            //        }
            //        try
            //        {
            //            // Lock buffers for direct access.
            //            MIL.MbufControl(buffers[i], MIL.M_LOCK, MIL.M_DEFAULT);
            //            // get image information.
            //            MIL.MbufInquire(buffers[i], MIL.M_HOST_ADDRESS, ref imageDataPtr);
            //            MIL.MbufInquire(buffers[i], MIL.M_SIZE_X, ref imageSizeX);
            //            MIL.MbufInquire(buffers[i], MIL.M_SIZE_Y, ref imageSizeY);
            //            MIL.MbufInquire(buffers[i], MIL.M_TYPE, ref SrcImageType);
            //            MIL.MbufInquire(buffers[i], MIL.M_PITCH_BYTE, ref SrcImagePitchByte);

            //            if (imageDataPtr != MIL.M_NULL && SrcImageType == FUNCTION_SUPPORTED_IMAGE_TYPE)
            //            {
            //                HObject img = new HObject();
            //                img.Dispose();
            //                unsafe
            //                {
            //                    //lock (this)
            //                    {
            //                        IntPtr ptr = imageDataPtr;
            //                        if (this.pixelType == IOMtxCamLinkToHalcon.PixelFormat.Mono8)
            //                        {
            //                            //黑白图像
            //                            //HOperatorSet.GenImage1(out image, "byte", Convert.ToInt32(imageSizeX), Convert.ToInt32(imageSizeY), ptr);
            //                            HOperatorSet.GenImage1Rect(out img, ptr, Convert.ToInt32(imageSizeX), Convert.ToInt32(imageSizeY), Convert.ToInt32(SrcImagePitchByte), 8, 8, "true", 0);
            //                            //byte a = *((byte *) ptr);
            //                            //HOperatorSet.CropPart(image, out image, 0, 0, Convert.ToInt32(imageSizeX), Convert.ToInt32(imageSizeY));
            //                            //HOperatorSet.WriteImage(image, "tiff", 0, "E:\\test.tiff");
            //                        }
            //                        else if (!this.pixelType.Contains("Mono"))
            //                        {
            //                            //RGB彩色图像
            //                            //HOperatorSet.GenImageInterleaved(out image, ptr, "bgrx", e.GrabResult.Width, e.GrabResult.Height, -1, "byte", 0, 0, 0, 0, -1, 0);
            //                        }
            //                        else
            //                        {
            //                            _acq.GrabStatus = "current pixel format not support!";
            //                            return _acq;
            //                        }
            //                    }
            //                }
            //                HOperatorSet.ConcatObj(_acq.Image, img, out _acq.Image);
            //                _acq.GrabStatus = "GrabPass";
            //                _acq.index++;
            //                //HOperatorSet.WriteImage(img, "tiff", 0, "E:\\test.tiff");
            //            }
            //            else
            //            {
            //                _acq.GrabStatus = "GrabFail";
            //                return _acq;
            //            }
            //        }
            //        catch (Exception ex)
            //        {
            //            _acq.GrabStatus = "GrabFail:" + ex.Message;
            //            return _acq;
            //        }
            //        finally
            //        {
            //            // Unlock buffers.
            //            MIL.MbufControl(buffers[i], MIL.M_UNLOCK, MIL.M_DEFAULT);
            //            // Clear the buffer.
            //            MIL.MbufClear(buffers[i], 0);
            //            buffers[i] = MIL.M_NULL;
            //        }
            //    }
            //    return _acq;

            //}
            //catch (Exception ex)
            //{
            //    _acq.GrabStatus = "GrabFail:" + ex.Message;
            //    return _acq;
            //}
            //finally
            //{
            //    num = 0;
            //}
        }
        public override Acquisition GetFrames(int imageNum = 1, int timeOut = -1)
        {
            Acquisition _acq = new Acquisition();
            try
            {
                timeOut = (timeOut == -1) ? 100000 : timeOut;
                DateTime t1 = DateTime.Now;

                for (int i = 0; i < buffers.Length; i++)
                {
                    if (DateTime.Now.Subtract(t1).TotalMilliseconds > timeOut)//采集超时500ms
                    {
                        for (int j = i; j < buffers.Length; j++)
                        {
                            if (buffers[j] == MIL.M_NULL) break;
                            MIL.MbufClear(buffers[j], 0);
                            buffers[j] = MIL.M_NULL;
                        }
                        _acq.GrabStatus = "GrabFail:TimeOut";
                        return _acq;
                    }
                    if (_acq.index + 1 == imageNum)
                    {
                        return _acq;
                    }
                    if (buffers[i] == MIL.M_NULL)
                    {
                        i--;
                        continue;
                    }
                    try
                    {
                        // Lock buffers for direct access.
                        MIL.MbufControl(buffers[i], MIL.M_LOCK, MIL.M_DEFAULT);
                        // get image information.
                        MIL.MbufInquire(buffers[i], MIL.M_HOST_ADDRESS, ref imageDataPtr);
                        MIL.MbufInquire(buffers[i], MIL.M_SIZE_X, ref imageSizeX);
                        MIL.MbufInquire(buffers[i], MIL.M_SIZE_Y, ref imageSizeY);
                        MIL.MbufInquire(buffers[i], MIL.M_TYPE, ref SrcImageType);
                        MIL.MbufInquire(buffers[i], MIL.M_PITCH_BYTE, ref SrcImagePitchByte);

                        if (imageDataPtr != MIL.M_NULL && SrcImageType == FUNCTION_SUPPORTED_IMAGE_TYPE)
                        {
                            HObject img = new HObject();
                            img.Dispose();
                            unsafe
                            {
                                //lock (this)
                                {
                                    IntPtr ptr = imageDataPtr;
                                    //if (this.pixelType == BaslerMtxCamLinkToHalcon.PixelFormat.Mono8)
                                    //{
                                    //    //黑白图像
                                    //    //HOperatorSet.GenImage1(out image, "byte", Convert.ToInt32(imageSizeX), Convert.ToInt32(imageSizeY), ptr);
                                    //    HOperatorSet.GenImage1Rect(out img, ptr, Convert.ToInt32(imageSizeX), Convert.ToInt32(imageSizeY), Convert.ToInt32(SrcImagePitchByte), 8, 8, "true", 0);
                                    //    //byte a = *((byte*)ptr);
                                    //    //HOperatorSet.CropPart(image, out image, 0, 0, Convert.ToInt32(imageSizeX), Convert.ToInt32(imageSizeY));
                                    //    //HOperatorSet.WriteImage(img, "tiff", 0, "E:\\test.tiff");
                                    //}
                                    //else if (!this.pixelType.Contains("Mono"))
                                    //{
                                    //    //RGB彩色图像
                                    //    //HOperatorSet.GenImageInterleaved(out image, ptr, "bgrx", e.GrabResult.Width, e.GrabResult.Height, -1, "byte", 0, 0, 0, 0, -1, 0);
                                    //}
                                    if (this.milPixelFormat.ToString() == "RGB")
                                    {

                                    }
                                    else
                                    {
                                        _acq.GrabStatus = "current pixel format not support!";
                                        return _acq;
                                    }
                                }
                            }
                            HOperatorSet.ConcatObj(_acq.Image, img, out _acq.Image);
                            _acq.GrabStatus = "GrabPass";
                            _acq.index++;
                        }
                        else
                        {
                            _acq.GrabStatus = "GrabFail";
                            return _acq;
                        }
                    }
                    catch (Exception ex)
                    {
                        _acq.GrabStatus = "GrabFail:" + ex.Message;
                        return _acq;
                    }
                    finally
                    {
                        // Unlock buffers.
                        MIL.MbufControl(buffers[i], MIL.M_UNLOCK, MIL.M_DEFAULT);
                        // Clear the buffer.
                        MIL.MbufClear(buffers[i], 0);
                        buffers[i] = MIL.M_NULL;
                    }
                }
                return _acq;
            }
            catch (Exception ex)
            {
                _acq.GrabStatus = "GrabFail:" + ex.Message;
                return _acq;
            }
            finally
            {
                num = 0;
            }
        }
        public override Task<Acquisition> GetFramesAsync(int imageNum = 1, int timeOut = -1)
        {
            return Task.Run(() =>
            {
                return GetFrames(imageNum, timeOut);
            });
        }
        private MIL_INT HookFunction(MIL_INT HookType, MIL_ID HookId, IntPtr HookDataPtr)
        {
            // this is how to check if the user data is null, the IntPtr class
            // contains a member, Zero, which exists solely for this purpose
            if (!IntPtr.Zero.Equals(HookDataPtr))
            {
                // get the handle to the ProcessingHookDataStruct object back from the IntPtr
                GCHandle hUserData = GCHandle.FromIntPtr(HookDataPtr);

                // get a reference to the ProcessingHookDataStruct object
                UserHookData UserHookDataPtr = hUserData.Target as UserHookData;
                MIL_ID ModifiedBufferId = MIL.M_NULL;

                // Retrieve the MIL_ID of the grabbed buffer.
                MIL.MdigGetHookInfo(HookId, MIL.M_MODIFIED_BUFFER + MIL.M_BUFFER_ID, ref ModifiedBufferId);

                buffers[num] = ModifiedBufferId;
                num++;
                return 0;
            }
            else
            {
                return -1;
            }
            //}

        }
    }
    public class BaslerEuresysCamLink : BaseCamera
    {
        #region 相机自带函数
        /// <summary>
        /// Native functions imported from the MultiCam C API.
        /// </summary>
        #region Native Methods
        class NativeMethods
        {
            private NativeMethods() { }

            [DllImport("MultiCam.dll")]
            internal static extern Int32 McOpenDriver(IntPtr instanceName);
            [DllImport("MultiCam.dll")]
            internal static extern Int32 McCloseDriver();
            [DllImport("MultiCam.dll")]
            internal static extern Int32 McCreate(UInt32 modelInstance, out UInt32 instance);
            [DllImport("MultiCam.dll")]
            internal static extern Int32 McCreateNm(String modelName, out UInt32 instance);
            [DllImport("MultiCam.dll")]
            internal static extern Int32 McDelete(UInt32 instance);
            [DllImport("MultiCam.dll")]
            internal static extern Int32 McSetParamInt(UInt32 instance, UInt32 parameterId, Int32 value);
            [DllImport("MultiCam.dll")]
            internal static extern Int32 McSetParamNmInt(UInt32 instance, String parameterName, Int32 value);
            [DllImport("MultiCam.dll")]
            internal static extern Int32 McSetParamStr(UInt32 instance, UInt32 parameterId, String value);
            [DllImport("MultiCam.dll")]
            internal static extern Int32 McSetParamNmStr(UInt32 instance, String parameterName, String value);
            [DllImport("MultiCam.dll")]
            internal static extern Int32 McSetParamFloat(UInt32 instance, UInt32 parameterId, Double value);
            [DllImport("MultiCam.dll")]
            internal static extern Int32 McSetParamNmFloat(UInt32 instance, String parameterName, Double value);
            [DllImport("MultiCam.dll")]
            internal static extern Int32 McSetParamInst(UInt32 instance, UInt32 parameterId, UInt32 value);
            [DllImport("MultiCam.dll")]
            internal static extern Int32 McSetParamNmInst(UInt32 instance, String parameterName, UInt32 value);
            [DllImport("MultiCam.dll")]
            internal static extern Int32 McSetParamPtr(UInt32 instance, UInt32 parameterId, IntPtr value);
            [DllImport("MultiCam.dll")]
            internal static extern Int32 McSetParamNmPtr(UInt32 instance, String parameterName, IntPtr value);
            [DllImport("MultiCam.dll")]
            internal static extern Int32 McSetParamInt64(UInt32 instance, UInt32 parameterId, Int64 value);
            [DllImport("MultiCam.dll")]
            internal static extern Int32 McSetParamNmInt64(UInt32 instance, String parameterName, Int64 value);
            [DllImport("MultiCam.dll")]
            internal static extern Int32 McGetParamInt(UInt32 instance, UInt32 parameterId, out Int32 value);
            [DllImport("MultiCam.dll")]
            internal static extern Int32 McGetParamNmInt(UInt32 instance, String parameterName, out Int32 value);
            [DllImport("MultiCam.dll")]
            internal static extern Int32 McGetParamStr(UInt32 instance, UInt32 parameterId, IntPtr value, UInt32 maxLength);
            [DllImport("MultiCam.dll")]
            internal static extern Int32 McGetParamNmStr(UInt32 instance, String parameterName, IntPtr value, UInt32 maxLength);
            [DllImport("MultiCam.dll")]
            internal static extern Int32 McGetParamFloat(UInt32 instance, UInt32 parameterId, out Double value);
            [DllImport("MultiCam.dll")]
            internal static extern Int32 McGetParamNmFloat(UInt32 instance, String parameterName, out Double value);
            [DllImport("MultiCam.dll")]
            internal static extern Int32 McGetParamInst(UInt32 instance, UInt32 parameterId, out UInt32 value);
            [DllImport("MultiCam.dll")]
            internal static extern Int32 McGetParamNmInst(UInt32 instance, String parameterName, out UInt32 value);
            [DllImport("MultiCam.dll")]
            internal static extern Int32 McGetParamPtr(UInt32 instance, UInt32 parameterId, out IntPtr value);
            [DllImport("MultiCam.dll")]
            internal static extern Int32 McGetParamPtr(UInt32 instance, UInt32 parameterId, out Microsoft.Win32.SafeHandles.SafeWaitHandle value);
            [DllImport("MultiCam.dll")]
            internal static extern Int32 McGetParamNmPtr(UInt32 instance, String parameterName, out IntPtr value);
            [DllImport("MultiCam.dll")]
            internal static extern Int32 McGetParamInt64(UInt32 instance, UInt32 parameterId, out Int64 value);
            [DllImport("MultiCam.dll")]
            internal static extern Int32 McGetParamNmInt64(UInt32 instance, String parameterName, out Int64 value);
            [DllImport("MultiCam.dll")]
            internal static extern Int32 McRegisterCallback(UInt32 instance, CALLBACK callbackFunction, UInt32 context);
            [DllImport("MultiCam.dll")]
            internal static extern Int32 McWaitSignal(UInt32 instance, Int32 signal, UInt32 timeout, out SIGNALINFO info);
            [DllImport("MultiCam.dll")]
            internal static extern Int32 McGetSignalInfo(UInt32 instance, Int32 signal, out SIGNALINFO info);
        }
        #endregion

        #region Private Constants
        private const Int32 MAX_VALUE_LENGTH = 1024;
        #endregion

        #region Default object instance Constants
        public const UInt32 CONFIGURATION = 0x20000000;
        public const UInt32 BOARD = 0xE0000000;
        public const UInt32 CHANNEL = 0X8000FFFF;
        public const UInt32 DEFAULT_SURFACE_HANDLE = 0x4FFFFFFF;
        #endregion

        #region Specific parameter values Constants
        public const Int32 INFINITE = -1;
        public const Int32 INDETERMINATE = -1;
        public const Int32 DISABLE = 0;
        #endregion

        #region Signal handling Constants
        public const UInt32 SignalEnable = (24 << 14);

        public const Int32 SIG_ANY = 0;
        public const Int32 SIG_SURFACE_PROCESSING = 1;
        public const Int32 SIG_SURFACE_FILLED = 2;
        public const Int32 SIG_UNRECOVERABLE_OVERRUN = 3;
        public const Int32 SIG_FRAMETRIGGER_VIOLATION = 4;
        public const Int32 SIG_START_EXPOSURE = 5;
        public const Int32 SIG_END_EXPOSURE = 6;
        public const Int32 SIG_ACQUISITION_FAILURE = 7;
        public const Int32 SIG_CLUSTER_UNAVAILABLE = 8;
        public const Int32 SIG_RELEASE = 9;
        public const Int32 SIG_END_ACQUISITION_SEQUENCE = 10;
        public const Int32 SIG_START_ACQUISITION_SEQUENCE = 11;
        public const Int32 SIG_END_CHANNEL_ACTIVITY = 12;

        public const Int32 SIG_GOLOW = (1 << 12);
        public const Int32 SIG_GOHIGH = (2 << 12);
        public const Int32 SIG_GOOPEN = (3 << 12);
        #endregion

        #region Signal handling Type Definitions
        public delegate void CALLBACK(ref SIGNALINFO signalInfo);


        public const UInt32 SignalHandling = (74 << 14);
        public const Int32 SIGNALHANDLING_ANY = 1;
        public const Int32 SIGNALHANDLING_CALLBACK_SIGNALING = 2;
        public const Int32 SIGNALHANDLING_WAITING_SIGNALING = 3;
        public const Int32 SIGNALHANDLING_OS_EVENT_SIGNALING = 4;

        public const UInt32 SurfaceState = (31 << 14);
        public const Int32 SURFACESTATE_FREE = 1;
        public const Int32 SURFACESTATE_FILLING = 2;
        public const Int32 SURFACESTATEe_FILLED = 3;
        public const Int32 SURFACESTATE_PROCESSING = 4;
        public const Int32 SURFACESTATE_RESERVED = 5;

        public const UInt32 SignalEvent = (25 << 14);


        [StructLayout(LayoutKind.Sequential)]
        public struct SIGNALINFO
        {
            public IntPtr Context;
            public UInt32 Instance;
            public Int32 Signal;
            public UInt32 SignalInfo;
            public UInt32 SignalContext;
        };
        #endregion

        #region Error handling Methods
        private static String GetErrorMessage(Int32 errorCode)
        {
            const UInt32 ErrorDesc = (98 << 14);
            String errorDescription;
            UInt32 status = (UInt32)Math.Abs(errorCode);
            IntPtr text = Marshal.AllocHGlobal(MAX_VALUE_LENGTH + 1);
            if (NativeMethods.McGetParamStr(CONFIGURATION, ErrorDesc + status, text, MAX_VALUE_LENGTH) != 0)
                errorDescription = "Unknown error";
            else
                errorDescription = Marshal.PtrToStringAnsi(text);
            Marshal.FreeHGlobal(text);
            return errorDescription;
        }
        class MultiCamException : System.Exception
        {
            public MultiCamException(String error) : base(error) { }
        }
        private static void ThrowOnMultiCamError(Int32 status, String action)
        {
            if (status != 0)
            {
                String error = action + ": " + GetErrorMessage(status);
                throw new MultiCamException(error);
            }
        }
        #endregion

        #region Driver connection Methods
        public static void OpenDriver()
        {
            ThrowOnMultiCamError(NativeMethods.McOpenDriver((IntPtr)null),
                "Cannot open MultiCam driver");
        }

        public static void CloseDriver()
        {
            ThrowOnMultiCamError(NativeMethods.McCloseDriver(),
                "Cannot close MultiCam driver");
        }
        #endregion

        #region Object creation/deletion Methods
        public static void Create(UInt32 modelInstance, out UInt32 instance)
        {
            ThrowOnMultiCamError(NativeMethods.McCreate(modelInstance, out instance),
                String.Format("Cannot create '{0}' instance", modelInstance));
        }

        public static void Create(String modelName, out UInt32 instance)
        {
            ThrowOnMultiCamError(NativeMethods.McCreateNm(modelName, out instance),
                String.Format("Cannot create '{0}' instance", modelName));
        }

        public static void Delete(UInt32 instance)
        {
            ThrowOnMultiCamError(NativeMethods.McDelete(instance),
                String.Format("Cannot delete '{0}' instance", instance));
        }
        #endregion

        #region Parameter 'setter' Methods
        public static void SetParam(UInt32 instance, UInt32 parameterId, Int32 value)
        {
            ThrowOnMultiCamError(NativeMethods.McSetParamInt(instance, parameterId, value),
                String.Format("Cannot set parameter '{0}' to value '{1}'", parameterId, value));
        }

        public static void SetParam(UInt32 instance, String parameterName, Int32 value)
        {
            ThrowOnMultiCamError(NativeMethods.McSetParamNmInt(instance, parameterName, value),
                String.Format("Cannot set parameter '{0}' to value '{1}'", parameterName, value));
        }

        public static void SetParam(UInt32 instance, UInt32 parameterId, String value)
        {
            ThrowOnMultiCamError(NativeMethods.McSetParamStr(instance, parameterId, value),
                String.Format("Cannot set parameter '{0}' to value '{1}'", parameterId, value));
        }

        public static void SetParam(UInt32 instance, String parameterName, String value)
        {
            ThrowOnMultiCamError(NativeMethods.McSetParamNmStr(instance, parameterName, value),
                String.Format("Cannot set parameter '{0}' to value '{1}'", parameterName, value));
        }

        public static void SetParam(UInt32 instance, UInt32 parameterId, Double value)
        {
            ThrowOnMultiCamError(NativeMethods.McSetParamFloat(instance, parameterId, value),
                String.Format("Cannot set parameter '{0}' to value '{1}'", parameterId, value));
        }

        public static void SetParam(UInt32 instance, String parameterName, Double value)
        {
            ThrowOnMultiCamError(NativeMethods.McSetParamNmFloat(instance, parameterName, value),
                String.Format("Cannot set parameter '{0}' to value '{1}'", parameterName, value));
        }

        public static void SetParam(UInt32 instance, UInt32 parameterId, UInt32 value)
        {
            ThrowOnMultiCamError(NativeMethods.McSetParamInst(instance, parameterId, value),
                String.Format("Cannot set parameter '{0}' to value '{1}'", parameterId, value));
        }

        public static void SetParam(UInt32 instance, String parameterName, UInt32 value)
        {
            ThrowOnMultiCamError(NativeMethods.McSetParamNmInst(instance, parameterName, value),
                String.Format("Cannot set parameter '{0}' to value '{1}'", parameterName, value));
        }

        public static void SetParam(UInt32 instance, UInt32 parameterId, IntPtr value)
        {
            ThrowOnMultiCamError(NativeMethods.McSetParamPtr(instance, parameterId, value),
                String.Format("Cannot set parameter '{0}' to value '{1}'", parameterId, value));
        }

        public static void SetParam(UInt32 instance, String parameterName, IntPtr value)
        {
            ThrowOnMultiCamError(NativeMethods.McSetParamNmPtr(instance, parameterName, value),
                String.Format("Cannot set parameter '{0}' to value '{1}'", parameterName, value));
        }

        public static void SetParam(UInt32 instance, UInt32 parameterId, Int64 value)
        {
            ThrowOnMultiCamError(NativeMethods.McSetParamInt64(instance, parameterId, value),
                String.Format("Cannot set parameter '{0}' to value '{1}'", parameterId, value));
        }

        public static void SetParam(UInt32 instance, String parameterName, Int64 value)
        {
            ThrowOnMultiCamError(NativeMethods.McSetParamNmInt64(instance, parameterName, value),
                String.Format("Cannot set parameter '{0}' to value '{1}'", parameterName, value));
        }
        #endregion

        #region Parameter 'getter' Methods
        public static void GetParam(UInt32 instance, UInt32 parameterId, out Int32 value)
        {
            ThrowOnMultiCamError(NativeMethods.McGetParamInt(instance, parameterId, out value),
                String.Format("Cannot get parameter '{0}'", parameterId));
        }

        public static void GetParam(UInt32 instance, String parameterName, out Int32 value)
        {
            ThrowOnMultiCamError(NativeMethods.McGetParamNmInt(instance, parameterName, out value),
                String.Format("Cannot get parameter '{0}'", parameterName));
        }

        public static void GetParam(UInt32 instance, UInt32 parameterId, out String value)
        {
            IntPtr text = Marshal.AllocHGlobal(MAX_VALUE_LENGTH + 1);
            try
            {
                ThrowOnMultiCamError(NativeMethods.McGetParamStr(instance, parameterId, text, MAX_VALUE_LENGTH),
                    String.Format("Cannot get parameter '{0}'", parameterId));
                value = Marshal.PtrToStringAnsi(text);
            }
            finally
            {
                Marshal.FreeHGlobal(text);
            }
        }

        public static void GetParam(UInt32 instance, String parameterName, out String value)
        {
            IntPtr text = Marshal.AllocHGlobal(MAX_VALUE_LENGTH + 1);
            try
            {
                ThrowOnMultiCamError(NativeMethods.McGetParamNmStr(instance, parameterName, text, MAX_VALUE_LENGTH),
                    String.Format("Cannot get parameter '{0}'", parameterName));
                value = Marshal.PtrToStringAnsi(text);
            }
            finally
            {
                Marshal.FreeHGlobal(text);
            }
        }

        public static void GetParam(UInt32 instance, UInt32 parameterId, out Double value)
        {
            ThrowOnMultiCamError(NativeMethods.McGetParamFloat(instance, parameterId, out value),
                String.Format("Cannot get parameter '{0}'", parameterId));
        }

        public static void GetParam(UInt32 instance, String parameterName, out Double value)
        {
            ThrowOnMultiCamError(NativeMethods.McGetParamNmFloat(instance, parameterName, out value),
                String.Format("Cannot get parameter '{0}'", parameterName));
        }

        public static void GetParam(UInt32 instance, UInt32 parameterId, out UInt32 value)
        {
            ThrowOnMultiCamError(NativeMethods.McGetParamInst(instance, parameterId, out value),
                String.Format("Cannot get parameter '{0}'", parameterId));
        }

        public static void GetParam(UInt32 instance, String parameterName, out UInt32 value)
        {
            ThrowOnMultiCamError(NativeMethods.McGetParamNmInst(instance, parameterName, out value),
                String.Format("Cannot get parameter '{0}'", parameterName));
        }

        public static void GetParam(UInt32 instance, UInt32 parameterId, out IntPtr value)
        {
            ThrowOnMultiCamError(NativeMethods.McGetParamPtr(instance, parameterId, out value),
                String.Format("Cannot get parameter '{0}'", parameterId));
        }


        public static void GetParam(UInt32 instance, UInt32 parameterId, out Microsoft.Win32.SafeHandles.SafeWaitHandle value)
        {
            ThrowOnMultiCamError(NativeMethods.McGetParamPtr(instance, parameterId, out value),
                String.Format("Cannot get parameter '{0}'", parameterId));
        }

        public static void GetParam(UInt32 instance, String parameterName, out IntPtr value)
        {
            ThrowOnMultiCamError(NativeMethods.McGetParamNmPtr(instance, parameterName, out value),
                String.Format("Cannot get parameter '{0}'", parameterName));
        }

        public static void GetParam(UInt32 instance, UInt32 parameterId, out Int64 value)
        {
            ThrowOnMultiCamError(NativeMethods.McGetParamInt64(instance, parameterId, out value),
                String.Format("Cannot get parameter '{0}'", parameterId));
        }

        public static void GetParam(UInt32 instance, String parameterName, out Int64 value)
        {
            ThrowOnMultiCamError(NativeMethods.McGetParamNmInt64(instance, parameterName, out value),
                String.Format("Cannot get parameter '{0}'", parameterName));
        }
        #endregion

        #region Signal handling Methods
        public static void RegisterCallback(UInt32 instance, CALLBACK callbackFunction, UInt32 context)
        {
            ThrowOnMultiCamError(NativeMethods.McRegisterCallback(instance, callbackFunction, context),
                "Cannot register callback");
        }

        public static void WaitSignal(UInt32 instance, Int32 signal, UInt32 timeout, out SIGNALINFO info)
        {
            ThrowOnMultiCamError(NativeMethods.McWaitSignal(instance, signal, timeout, out info),
                "WaitSignal error");
        }

        public static void GetSignalInfo(UInt32 instance, Int32 signal, out SIGNALINFO info)
        {
            ThrowOnMultiCamError(NativeMethods.McGetSignalInfo(instance, signal, out info),
                "Cannot get signal information");
        }
        #endregion

        #endregion

        #region 配置参数
        private Camera camera = null;
        private string sn = "";
        CALLBACK multiCamCallback;
        private string camFile = "acA2040-180kc_P180SC";
        private string pixelType = PixelFormat.Mono8;
        private bool isSoftwareTrigger = true;
        private bool isCameraTrigger = true;
        private bool isMirrorX = false;
        private bool isMirrorY = false;
        static PLCamera.PixelFormatEnum PixelFormat = new PLCamera.PixelFormatEnum();
        System.Collections.Concurrent.ConcurrentDictionary<int, byte[]> dBuffer = new System.Collections.Concurrent.ConcurrentDictionary<int, byte[]>();
        private const int MAX_BUFFER_NUM = 100;
        private byte[][] buffer = new byte[MAX_BUFFER_NUM][];
        private int num = 0;
        // The MultiCam object that controls the acquisition
        private UInt32 channel;
        private int width, height, bufferPitch;
        static Version Sfnc2_0_0 = new Version(2, 0, 0);
        #endregion
        public BaslerEuresysCamLink(string serialNumber, string camFile)
        {
            this.sn = serialNumber;
            this.camFile = camFile;
        }
        public override bool InitCamera()
        {
            try
            {
                if (camera == null)
                {
                    camera = new Camera(sn);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }
        public override bool OpenCamera()
        {
            try
            {
                // Open MultiCam driver
                OpenDriver();
                // Enable error logging
                SetParam(CONFIGURATION, "ErrorLog", "error.log");

                // Create a channel and associate it with the first connector on the first board

                SetParam(BOARD + 0, "BoardTopology", "MONO_DECA");

                Create("CHANNEL", out channel);
                //不同板卡设置不同索引？
                SetParam(channel, "DriverIndex", 0);
                // For all Domino boards except Domino Symphony
                SetParam(channel, "Connector", "M");
                // Choose the CAM file
                SetParam(channel, "CamFile", camFile);
                // Choose the pixel color format
                SetParam(channel, "ColorFormat", pixelType);

                // Choose the way the first acquisition is triggered
                SetParam(channel, "AcquisitionMode", "HFR");
                SetParam(channel, "TrigMode", "IMMEDIATE");
                // SetParam(channel, "TrigMode", "HARD");
                SetParam(channel, "NextTrigMode", "SAME");
                // SetParam(channel, "ImageFlipY", "ON");
                SetParam(channel, "TriggerSkipHold", "HOLD");
                SetParam(channel, "Expose", "WIDTH");
                // Choose the number of images to acquire
                SetParam(channel, "SeqLength_Fr", INDETERMINATE);
                SetParam(channel, "TrigCtl", "ISO");
                SetParam(channel, "TrigLine", "IIN1");

                GetParam(channel, "ImageSizeX", out width);
                GetParam(channel, "ImageSizeY", out height);
                GetParam(channel, "BufferPitch", out bufferPitch);

                // Register the callback function
                multiCamCallback = new CALLBACK(MultiCamCallback);
                RegisterCallback(channel, multiCamCallback, channel);

                // Enable the signals corresponding to the callback functions
                SetParam(channel, SignalEnable + SIG_SURFACE_PROCESSING, "ON");
                SetParam(channel, SignalEnable + SIG_ACQUISITION_FAILURE, "ON");
                for (int i = 0; i < buffer.GetLength(0); i++)
                {
                    buffer[i] = new byte[bufferPitch * height];
                }
                //
                if (camera.IsOpen)
                {
                    CloseCamera();
                    Thread.Sleep(50);
                }
                camera.Open();
                //camera.Parameters[BaslerEuresysCamLinkToHalcon.PixelFormat].TrySetValue(this.pixelType);
                camera.Parameters[PLCamera.ReverseX].SetValue(this.isMirrorX);
                camera.Parameters[PLCamera.ReverseX].SetValue(this.isMirrorY);
                camera.Parameters[PLCamera.TriggerMode].SetValue("On");
                camera.Parameters[PLCamera.ExposureMode].SetValue("TriggerWidth");
                camera.Parameters[PLCamera.ClTapGeometry].SetValue("Geometry1X10_1Y");
                ////硬触发使用
                //if (!this.isSoftwareTrigger) camera.Parameters[PLCamera.TriggerSource].SetValue("Line1");
                ChangeTriggerSource(this.isSoftwareTrigger);
                return true;
            }
            catch (MultiCamException)
            {
                CloseCamera();
                return false;
            }
        }
        public override bool CloseCamera()
        {
            try
            {
                if (camera == null) return true;
                if (camera.IsOpen)
                {
                    //if (this.isSoftwareTrigger) camera.CameraOpened -= Configuration.SoftwareTrigger;
                    camera.Close();
                }
                CloseDriver();
                return true;
            }
            catch (MultiCamException)
            {
                return false;
            }
        }
        public override bool IsCameraTrigger
        {
            set { this.isCameraTrigger = value; }
            get { return this.isCameraTrigger; }
        }
        public override bool IsSoftwareTrigger
        {
            set { this.isSoftwareTrigger = value; }
            get { return this.isSoftwareTrigger; }
        }
        public override bool IsConnected
        {
            get
            {
                return this.camera.IsConnected;
            }
        }
        public override bool IsMirrorX
        {
            set { this.isMirrorX = value; }
            get { return this.isMirrorX; }
        }
        public override bool IsMirrorY
        {
            set { this.isMirrorY = value; }
            get { return this.isMirrorY; }
        }
        public override bool ChangeTriggerSource(bool isSoftwareTrigger)
        {
            SetParam(channel, "ChannelState", "IDLE");
            if (isCameraTrigger)
            {
                SetParam(channel, "TrigMode", "IMMEDIATE");
                //camera.Parameters[PLCamera.TriggerMode].SetValue("On");
                if (isSoftwareTrigger)
                    camera.Parameters[PLCamera.TriggerSource].SetValue("Software Trigger");
                else
                    camera.Parameters[PLCamera.TriggerSource].SetValue("CC1");
            }
            else
            {
                //camera.Parameters[PLCamera.TriggerMode].SetValue("Off");
                if (isSoftwareTrigger)
                    SetParam(channel, "TrigMode", "SOFT");
                else
                {
                    SetParam(channel, "TrigMode", "HARD");
                    //SetParam(channel, "TrigCtl", "ISO");
                    //SetParam(channel, "TrigLine", "IIN1");
                }
            }
            SetParam(channel, "ChannelState", "ACTIVE");
            this.isSoftwareTrigger = isSoftwareTrigger;
            return true;
        }
        public override bool SetGain(double value)
        {
            try
            {
                // Some camera models may have auto functions enabled. To set the gain value to a specific value,
                // the Gain Auto function must be disabled first (if gain auto is available).
                //if (!camera.Parameters[PLCamera.GainAuto].TrySetValue(PLCamera.GainAuto.Off)) // Set GainAuto to Off if it is writable.
                //    return false;
                // Features, e.g. 'Gain', are named according to the GenICam Standard Feature Naming Convention (SFNC).
                // The SFNC defines a common set of features, their behavior, and the related parameter names. 
                // This ensures the interoperability of cameras from different camera vendors. 
                // Cameras compliant with the USB3 Vision standard are based on the SFNC version 2.0.
                // Basler GigE and Firewire cameras are based on previous SFNC versions.
                // Accordingly, the behavior of these cameras and some parameters names will be different.
                // The SFNC version can be used to handle differences between camera device models.
                if (camera.GetSfncVersion() < Sfnc2_0_0)
                {
                    // In previous SFNC versions, GainRaw is an integer parameter.
                    camera.Parameters[PLCamera.GainRaw].SetValuePercentOfRange(Convert.ToInt32(value));
                }
                else // For SFNC 2.0 cameras, e.g. USB3 Vision cameras
                {
                    // In SFNC 2.0, Gain is a float parameter.
                    camera.Parameters[PLUsbCamera.Gain].SetValuePercentOfRange(value);
                    // For USB cameras, Gamma is always enabled.
                }
                return true;
            }
            catch (Exception)
            {
                if (camera != null && camera.IsOpen)
                    camera.Parameters[PLCamera.GainAuto].TrySetValue(PLCamera.GainAuto.Continuous);
                return false;
            }

        }
        public override void GetGain(out double value)
        {
            try
            {
                if (camera.GetSfncVersion() < Sfnc2_0_0)
                {
                    // In previous SFNC versions, GainRaw is an integer parameter.
                    value = camera.Parameters[PLCamera.GainRaw].GetValuePercentOfRange();
                }
                else // For SFNC 2.0 cameras, e.g. USB3 Vision cameras
                {
                    // In SFNC 2.0, Gain is a float parameter.
                    value = camera.Parameters[PLUsbCamera.Gain].GetValuePercentOfRange();
                    // For USB cameras, Gamma is always enabled.
                }
            }
            catch (Exception)
            {
                value = -1;
            }
        }
        public override bool SetExposure(double value)
        {
            try
            {
                //if (!camera.Parameters[PLCamera.ExposureAuto].TrySetValue(PLCamera.ExposureAuto.Off))
                //    return false;
                //if (camera.GetSfncVersion() < Sfnc2_0_0)
                //{
                //    return camera.Parameters[PLCamera.ExposureTimeRaw].TrySetValue(Convert.ToInt32(value));
                //}
                //else
                //{
                //    return camera.Parameters[PLCamera.ExposureTime].TrySetValue(value);
                //}

                SetParam(channel, "Expose_us", (int)value);
                return true;
            }
            catch (Exception)
            {
                //camera.Parameters[PLCamera.ExposureTime].TrySetValue(100);
                return false;
            }
        }
        public override void GetExposure(out double value)
        {
            try
            {
                //if (camera.GetSfncVersion() < Sfnc2_0_0)
                //{
                //    value = camera.Parameters[PLCamera.ExposureTimeRaw].GetValue();
                //}
                //else
                //{
                //    value = camera.Parameters[PLCamera.ExposureTime].GetValue();
                //}

                int expose = -1;
                GetParam(channel, "Expose_us", out expose);
                value = expose;
            }
            catch (Exception)
            {
                value = -1;
            }

        }
        public void SetGamma(bool value)
        {
            try
            {
                // GammaEnable is a boolean parameter.
                camera.Parameters[PLCamera.GammaEnable].TrySetValue(value);    //if available
            }
            catch (Exception)
            {
                if (camera != null)
                    camera.Parameters[PLCamera.GammaEnable].TrySetValue(true);
            }
        }
        public void GetGamma(out bool value)
        {
            try
            {
                value = camera.Parameters[PLCamera.GammaEnable].GetValue();
            }
            catch (Exception)
            {
                value = false;
            }
        }
        /// <summary>设置相机的像素格式
        /// 设置相机的像素格式
        /// </summary>
        /// <param name="value">value可以设置为如:BaslerToHalcon.PixelFormat.Mono8</param>
        public override bool EuresysSetPixelFormat(string value)
        {
            try
            {
                this.pixelType = value;
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }
        public override void EuresysGetPixelFormat(out string value)
        {
            try
            {
                value = this.pixelType;
            }
            catch (Exception)
            {
                value = "";
            }

        }
        public override bool ClearImageQueue()
        {
            try
            {
                num = 0;
                int count = this.dBuffer.Count;
                for (int i = 0; i < count; i++)
                {
                    if (this.dBuffer[i] != null)
                    {
                        this.dBuffer[i] = null;
                    }
                }
                this.dBuffer.Clear();
                //bool isNotEmpty = false;
                //for (int i = 0; i < this.buffer.GetLength(0); i++)
                //{
                //    isNotEmpty = this.buffer[i].Any(p => p != 0);
                //    if (!isNotEmpty) break;
                //    this.buffer[i] = null;
                //    this.buffer[i] = new byte[bufferPitch * height];
                //}
                GC.Collect();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public override Acquisition Snap(int imageNum = 1, int timeOut = -1)
        {
            Acquisition _acq = new Acquisition();

            if (this.isCameraTrigger)
            {
                ICommandParameterExtensions.TryExecute(camera.Parameters[PLCamera.TriggerSoftware]);
            }
            else
                SetParam(channel, "ForceTrig", "TRIG");
            _acq = GetFrames(imageNum, timeOut);
            return _acq;
        }
        public override Acquisition GetFrames(int imageNum = 1, int timeOut = -1)
        {
            Acquisition _acq = new Acquisition();
            try
            {
                DateTime start = DateTime.Now;
                timeOut = (timeOut == -1) ? 100000 : timeOut;
                Acquisition[] acqs = new Acquisition[imageNum];
                int threadNum = 2;
                int maxThreadNum = Math.Min(threadNum, imageNum);
                Parallel.For(0, maxThreadNum, i =>
                {
                    acqs[i] = new Acquisition();
                    ImageConvert(start, i, maxThreadNum, imageNum, ref acqs, timeOut);
                });
                double time1 = DateTime.Now.Subtract(start).TotalMilliseconds;

                for (int i = 0; i < imageNum; i++)
                {
                    _acq.GrabStatus = acqs[i].GrabStatus;
                    if (acqs[i].GrabStatus == "GrabPass")
                    {
                        HOperatorSet.ConcatObj(_acq.Image, acqs[i].Image, out _acq.Image);
                        _acq.index++;
                    }
                }
                return _acq;
            }
            catch (Exception ex)
            {
                _acq.GrabStatus = "GrabFail:" + ex.Message;
                return _acq;
            }
            finally
            {
                num = 0;
            }
        }
        public override Task<Acquisition> GetFramesAsync(int imageNum = 1, int timeOut = -1)
        {
            return Task.Run(() =>
            {
                return GetFrames(imageNum, timeOut);
            });
        }
        #region 调用的函数
        private void ImageConvert(DateTime start, int ind, ref HObject image, ref string grabStatus, int timeOut = -1)
        {
            while (DateTime.Now.Subtract(start).TotalMilliseconds < timeOut)
            {
                if (!this.dBuffer.ContainsKey(ind))
                    continue;
                if (this.dBuffer[ind] == null)
                    continue;
                try
                {
                    byte[] buffer = this.dBuffer[ind];
                    unsafe
                    {
                        fixed (byte* p = buffer)
                        {
                            IntPtr ptr = (IntPtr)p;
                            image.Dispose();
                            if (this.pixelType == EuresysPixelFormat.Y8)
                            {
                                //黑白图像
                                HOperatorSet.GenImage1(out image, "byte", Convert.ToInt32(width), Convert.ToInt32(height), ptr);
                            }
                            else if (this.pixelType == EuresysPixelFormat.BAYER8)
                            {
                                HOperatorSet.GenImage1(out image, "byte", Convert.ToInt32(width), Convert.ToInt32(height), ptr);
                                HOperatorSet.CfaToRgb(image, out image, "bayer_rg", "bilinear");
                            }
                            else
                            {
                                image.Dispose();
                                grabStatus = "current pixel format not support!";
                                return;
                            }
                        }
                        grabStatus = "GrabPass";
                    }
                    return;
                }
                catch (Exception ex)
                {
                    grabStatus = "GrabFail:" + ex.Message;
                    return;
                }
                finally
                {
                    this.dBuffer[ind] = null;
                }
            }
            grabStatus = "GrabTimeOut";
            return;
        }
        private void ImageConvert(DateTime startTime, int start, int step, int imageNum, ref Acquisition[] acqs, int timeOut = -1)
        {
            for (int i = start; i < imageNum; i += step)
            {
                acqs[i] = new Acquisition();
                ImageConvert(startTime, i, ref acqs[i].Image, ref acqs[i].GrabStatus, timeOut);
            }
        }

        DateTime start;
        private void MultiCamCallback(ref SIGNALINFO signalInfo)
        {
            start = DateTime.Now;
            switch (signalInfo.Signal)
            {
                case SIG_SURFACE_PROCESSING:
                    ProcessingCallback(signalInfo);
                    break;
                case SIG_ACQUISITION_FAILURE:
                    AcqFailureCallback(signalInfo);
                    break;
                default:
                    throw new MultiCamException("Unknown signal");
            }
        }
        private void ProcessingCallback(SIGNALINFO signalInfo)
        {
            if (num >= MAX_BUFFER_NUM)
            {
                ClearImageQueue();
            }
            UInt32 currentSurface = signalInfo.SignalInfo;
            // Update the image with the acquired image buffer data 
            IntPtr bufferAddress = IntPtr.Zero;
            GetParam(currentSurface, "SurfaceAddr", out bufferAddress);
            Marshal.Copy(bufferAddress, buffer[num], 0, bufferPitch * height);
            if (this.dBuffer.ContainsKey(num))
            {
                this.dBuffer[num] = null;
                this.dBuffer[num] = buffer[num];
            }
            else this.dBuffer.TryAdd(num, buffer[num]);
            double time1 = DateTime.Now.Subtract(start).TotalMilliseconds;
            num++;
        }
        private void AcqFailureCallback(SIGNALINFO signalInfo)
        {

        }
        #endregion

    }
    public class MVSCamToHalcon : BaseCamera
    {
        #region 私有变量

        private HObject image;
        private MyCamera.MV_CC_DEVICE_INFO device;
        private string sn = "";
        private string pixelType = "";
        //private string triggerSource = "";
        private bool isSoftwareTrigger = true;
        private bool isMirrorX = false;
        private bool isMirrorY = false;
        private bool isTrggerLight = false;
        private bool isConnected = false;
        System.Collections.Concurrent.ConcurrentQueue<Acquisition> acqQueue = new System.Collections.Concurrent.ConcurrentQueue<Acquisition>();
        static object lockObj = new object();
        MyCamera.cbOutputdelegate ImageCallback;

        MyCamera.MV_CC_DEVICE_INFO_LIST m_pDeviceList;
        CameraOperator m_pOperator;
        bool m_bGrabbing;

        UInt32 m_nBufSizeForSaveImage = 3072 * 2048 * 3 * 3 + 2048;
        byte[] m_pBufForSaveImage = new byte[3072 * 2048 * 3 * 3 + 2048];
        private List<string> DeviceList;
        bool m_bSaveBmp = false;
        bool m_bSaveJpg = false;

        #endregion

        //public event EventHandler<Acquisition> GrabImageDone;

        public MVSCamToHalcon(string serialNumber)
        {
            m_pDeviceList = new MyCamera.MV_CC_DEVICE_INFO_LIST();
            m_pOperator = new CameraOperator();
            m_bGrabbing = false;
            this.sn = serialNumber;
            HOperatorSet.GenEmptyObj(out image);
            Environment.SetEnvironmentVariable("PYLON_GIGE_HEARTBEAT", "1000");
        }
        public override bool InitCamera()
        {
            try
            {
                DeviceListAcq();
                if (m_pDeviceList.nDeviceNum == 0)
                {
                    //MessageBox.Show("无设备，请选择");
                    return false;
                }
                for (int i = 0; i < m_pDeviceList.nDeviceNum; i++)
                {
                    if (DeviceList[i].Contains(sn))
                    {
                        device = (MyCamera.MV_CC_DEVICE_INFO)Marshal.PtrToStructure(m_pDeviceList.pDeviceInfo[i],
                                                                  typeof(MyCamera.MV_CC_DEVICE_INFO));
                        return true;
                    }
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }

        }
        public override bool OpenCamera()
        {
            try
            {

                int nRet = -1;
                nRet = m_pOperator.Open(ref device);
                if (MyCamera.MV_OK != nRet)
                {
                    //MessageBox.Show("设备打开失败!");
                    return false;
                }

                //设置采集连续模式
                m_pOperator.SetEnumValue("AcquisitionMode", 2);
                //m_pOperator.SetEnumValue("TriggerMode", 0);

                //注册回调函数
                ImageCallback = new MyCamera.cbOutputdelegate(ImageCallBack);
                nRet = m_pOperator.RegisterImageCallBack(ImageCallback, IntPtr.Zero);
                if (CameraOperator.CO_OK != nRet)
                {
                    //MessageBox.Show("注册回调失败!");
                    return false;
                }
                m_pOperator.SetEnumValue("TriggerSource", 7);//设为软触发
                this.isConnected = true;
                m_pOperator.SetEnumValue("AcquisitionMode", 2);// 工作在连续模式
                m_pOperator.SetEnumValue("TriggerMode", 1);    // 连续模式0，触发模式1
                nRet = m_pOperator.StartGrabbing();
                if (MyCamera.MV_OK != nRet)
                {
                    //MessageBox.Show("开始取流失败！");
                    this.m_bGrabbing = false;
                    return false;
                }
                ChangeTriggerSource(isSoftwareTrigger);
                this.m_bGrabbing = true;
                return true;
            }
            catch (Exception e)
            {
                string err = e.Message;
                CloseCamera();
                return false;
            }
        }
        public override bool CloseCamera()
        {
            try
            {
                //停止采集
                m_pOperator.StopGrabbing();
                //关闭设备
                m_pOperator.Close();

                //取流标志位清零
                m_bGrabbing = false;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public override bool IsSoftwareTrigger
        {
            get { return this.isSoftwareTrigger; }
            set { this.isSoftwareTrigger = value; }
        }
        public override bool IsTriggerLight
        {
            get { return this.isTrggerLight; }
        }
        public override bool IsConnected
        {
            get
            {
                return this.isConnected;
            }
        }
        public override bool IsMirrorX
        {
            set { this.isMirrorX = value; }
            get { return this.isMirrorX; }
        }
        public override bool IsMirrorY
        {
            set { this.isMirrorY = value; }
            get { return this.isMirrorY; }
        }
        public override bool ChangeTriggerSource(bool isSoftwareTrigger)
        {
            //if (isSoftwareTrigger == this.isSoftwareTrigger) return true;
            try
            {
                if (isSoftwareTrigger)
                {
                    m_pOperator.SetEnumValue("TriggerSource", 7);
                }
                else
                {
                    m_pOperator.SetEnumValue("TriggerSource", 0);
                    m_pOperator.SetEnumValue("TriggerActivation", 0);
                }
                this.isSoftwareTrigger = isSoftwareTrigger;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public override bool ChangeTriggerLight(bool isTrggerLight)
        {
            if (isTrggerLight == this.isTrggerLight) return true;
            try
            {
                if (isTrggerLight)
                {
                    if (m_pOperator.SetEnumValue("LineSelector", 1) != 0)
                        return false;
                    if (m_pOperator.SetEnumValue("LineMode", 0) != 0)
                        return false;
                }
                else
                {
                    if (m_pOperator.SetEnumValue("LineSelector", 0) != 0)
                        return false;
                    if (m_pOperator.SetEnumValue("LineMode", 0) != 0)
                        return false;
                }
                this.isTrggerLight = isTrggerLight;
                return true;
            }
            catch
            {
                return false;
            }
        }
        public override bool SetGain(double value)
        {
            try
            {
                int nRet;
                m_pOperator.SetEnumValue("GainAuto", 0);
                nRet = m_pOperator.SetFloatValue("Gain", (float)value);
                if (nRet != CameraOperator.CO_OK)
                {
                    return false;
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }
        public override void GetGain(out double value)
        {
            try
            {
                float fGain = 0;
                m_pOperator.GetFloatValue("Gain", ref fGain);
                value = (double)fGain;
            }
            catch (Exception)
            {
                value = -1;
            }
        }
        public override bool SetExposure(double value)
        {
            try
            {
                int nRet;

                m_pOperator.SetEnumValue("ExposureAuto", 0);
                nRet = m_pOperator.SetFloatValue("ExposureTime", (float)value);
                if (nRet != CameraOperator.CO_OK)
                {
                    return false;
                }
                return true;

            }
            catch (Exception)
            {
                //camera.Parameters[PLCamera.ExposureTime].TrySetValue(100);
                return false;
            }
        }
        public override void GetExposure(out double value)
        {
            try
            {
                float fExposure = 0;
                m_pOperator.GetFloatValue("ExposureTime", ref fExposure);
                value = (double)fExposure;
            }
            catch (Exception)
            {
                value = -1;
            }

        }
        public bool SetAcquisitionFrameRate(double value)
        {
            try
            {
                int nRet;
                nRet = m_pOperator.SetFloatValue("AcquisitionFrameRate", (float)value);
                if (nRet != CameraOperator.CO_OK)
                {
                    return false;
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public void GetAcquisitionFrameRate(out double value)
        {
            try
            {
                float fFrameRate = 0;
                m_pOperator.GetFloatValue("ResultingFrameRate", ref fFrameRate);
                value = (double)fFrameRate;
            }
            catch (Exception)
            {
                value = -1;
            }
        }
        /// <summary>设置相机的像素格式
        /// 设置相机的像素格式
        /// </summary>
        /// <param name="value">value可以设置为如:BaslerToHalcon.PixelFormat.Mono8</param>
        public override bool BaslerSetPixelFormat(string value)
        {
            //value = BaslerToHalcon.PixelFormat.Mono8; 

            try
            {
                this.pixelType = value;
                return true;
                //return camera.Parameters[BaslerToHalcon.PixelFormat].TrySetValue(value);
            }
            catch (Exception)
            {
                //camera.Parameters[BaslerToHalcon.PixelFormat].TrySetValue(BaslerToHalcon.PixelFormat.Mono8);
                return false;
            }

        }
        public override void BaslerGetPixelFormat(out string value)
        {
            try
            {
                value = "";
            }
            catch (Exception)
            {
                value = "";
            }

        }
        public override bool ClearImageQueue()
        {
            try
            {
                if (this.acqQueue != null)
                {
                    foreach (var item in this.acqQueue)
                    {
                        item.Dispose();
                    }
                }
                this.acqQueue = new System.Collections.Concurrent.ConcurrentQueue<Acquisition>();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        /// <summary>
        /// 软触发采集图像
        /// </summary>
        /// <param name="imageNum"></param>
        /// <param name="timeOut"></param>
        /// <returns></returns>
        public override Acquisition Snap(int imageNum = 1, int timeOut = -1)
        {
            Acquisition _acq = new Acquisition();
            try
            {
                timeOut = (timeOut == -1) ? 100000 : timeOut;
                int nRet = m_pOperator.CommandExecute("TriggerSoftware");
                if (MyCamera.MV_OK != nRet)
                {
                    _acq.GrabStatus = "GrabFail:TriggerFail";
                    return _acq;
                }
                DateTime t1 = DateTime.Now;
                while (true)
                {
                    lock (lockObj)
                    {
                        if (DateTime.Now.Subtract(t1).TotalMilliseconds > timeOut)//采集超时500ms
                        {
                            while (acqQueue.Count > 0)
                            {
                                Acquisition acq;
                                acqQueue.TryDequeue(out acq);
                                acq.Image.Dispose();
                                //acqQueue.Dequeue().Image.Dispose();
                            }
                            _acq.GrabStatus = "GrabFail:TimeOut";
                            return _acq;
                        }
                        else
                        {
                            if (_acq.index >= imageNum - 1)
                            {
                                _acq.GrabStatus = "GrabPass";
                                return _acq;
                            }
                            if (acqQueue.Count > 0)
                            {
                                Acquisition acq;
                                acqQueue.TryDequeue(out acq);
                                switch (acq.GrabStatus)
                                {
                                    case "GrabPass":
                                        HOperatorSet.ConcatObj(_acq.Image, acq.Image, out _acq.Image);
                                        _acq.index++;
                                        break;
                                    case "GrabFail":
                                        acq.Image.Dispose();
                                        _acq.GrabStatus = "GrabFail:the latest image grab failed.";
                                        _acq.index++;
                                        return _acq;
                                    default:
                                        _acq.GrabStatus = acq.GrabStatus;
                                        acq.Image.Dispose();
                                        _acq.index++;
                                        return _acq;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _acq.GrabStatus = "GrabFail:" + ex.Message;
                return _acq;
            }
        }
        public override Acquisition GetFrames(int imageNum = 1, int timeOut = -1)
        {
            Acquisition _acq = new Acquisition();
            try
            {
                timeOut = (timeOut == -1) ? 100000 : timeOut;
                DateTime t1 = DateTime.Now;
                while (true)
                {
                    lock (lockObj)
                    {
                        if (DateTime.Now.Subtract(t1).TotalMilliseconds > timeOut)//采集超时500ms
                        {
                            while (acqQueue.Count > 0)
                            {
                                Acquisition acq;
                                acqQueue.TryDequeue(out acq);
                                acq.Image.Dispose();
                            }
                            _acq.GrabStatus = "GrabFail:TimeOut";
                            return _acq;
                        }
                        else
                        {
                            if (_acq.index >= imageNum - 1)
                            {
                                _acq.GrabStatus = "GrabPass";
                                return _acq;
                            }
                            if (acqQueue.Count > 0)
                            {
                                Acquisition acq;
                                acqQueue.TryDequeue(out acq);
                                switch (acq.GrabStatus)
                                {
                                    case "GrabPass":
                                        HOperatorSet.ConcatObj(_acq.Image, acq.Image, out _acq.Image);
                                        _acq.index++;
                                        break;
                                    case "GrabFail":
                                        acq.Image.Dispose();
                                        _acq.GrabStatus = "GrabFail:the latest image grab failed.";
                                        _acq.index++;
                                        return _acq;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _acq.GrabStatus = "GrabFail:" + ex.Message;
                return _acq;
            }
        }
        public override Task<Acquisition> GetFramesAsync(int imageNum = 1, int timeOut = -1)
        {
            return Task.Run(() =>
            {
                return GetFrames(imageNum, timeOut);
            });
        }


        #region 相机原生类、方法

        private void DeviceListAcq()
        {
            int nRet;
            /*创建设备列表*/
            System.GC.Collect();
            DeviceList = new List<string>();
            if (DeviceList != null) DeviceList.Clear();
            nRet = CameraOperator.EnumDevices(MyCamera.MV_GIGE_DEVICE | MyCamera.MV_USB_DEVICE, ref m_pDeviceList);
            if (0 != nRet)
            {
                //MessageBox.Show("枚举设备失败!");
                return;
            }

            //在窗体列表中显示设备名
            for (int i = 0; i < m_pDeviceList.nDeviceNum; i++)
            {
                MyCamera.MV_CC_DEVICE_INFO device = (MyCamera.MV_CC_DEVICE_INFO)Marshal.PtrToStructure(m_pDeviceList.pDeviceInfo[i], typeof(MyCamera.MV_CC_DEVICE_INFO));
                if (device.nTLayerType == MyCamera.MV_GIGE_DEVICE)
                {
                    IntPtr buffer = Marshal.UnsafeAddrOfPinnedArrayElement(device.SpecialInfo.stGigEInfo, 0);
                    MyCamera.MV_GIGE_DEVICE_INFO gigeInfo = (MyCamera.MV_GIGE_DEVICE_INFO)Marshal.PtrToStructure(buffer, typeof(MyCamera.MV_GIGE_DEVICE_INFO));
                    if (gigeInfo.chUserDefinedName != "")
                    {
                        DeviceList.Add("GigE: " + gigeInfo.chUserDefinedName);
                    }
                    else
                    {
                        DeviceList.Add("GigE: " + gigeInfo.chManufacturerName + " " + gigeInfo.chModelName + " (" + gigeInfo.chSerialNumber + ")");
                    }
                }
                else if (device.nTLayerType == MyCamera.MV_USB_DEVICE)
                {
                    IntPtr buffer = Marshal.UnsafeAddrOfPinnedArrayElement(device.SpecialInfo.stUsb3VInfo, 0);
                    MyCamera.MV_USB3_DEVICE_INFO usbInfo = (MyCamera.MV_USB3_DEVICE_INFO)Marshal.PtrToStructure(buffer, typeof(MyCamera.MV_USB3_DEVICE_INFO));
                    if (usbInfo.chUserDefinedName != "")
                    {
                        DeviceList.Add("USB: " + usbInfo.chUserDefinedName);
                    }
                    else
                    {
                        DeviceList.Add("USB: " + usbInfo.chManufacturerName + " " + usbInfo.chModelName + " (" + usbInfo.chSerialNumber + ")");
                    }
                }

            }

            //选择第一项
            //if (m_pDeviceList.nDeviceNum != 0)
            //{
            //    cbDeviceList.SelectedIndex = 0;
            //}
        }
        //回调函数
        private void ImageCallBack(IntPtr pData, ref MyCamera.MV_FRAME_OUT_INFO pFrameInfo, IntPtr pUser)
        {
            Acquisition _acq = new Acquisition();
            image.Dispose();

            //if ((3 * pFrameInfo.nFrameLen + 2048) > m_nBufSizeForSaveImage)
            //{
            //    m_nBufSizeForSaveImage = 3 * pFrameInfo.nFrameLen + 2048;
            //    m_pBufForSaveImage = new byte[m_nBufSizeForSaveImage];
            //}
            try
            {
                //IntPtr pImage = Marshal.UnsafeAddrOfPinnedArrayElement(m_pBufForSaveImage, 0);

                //MyCamera.MV_SAVE_IMAGE_PARAM stSaveParam = new MyCamera.MV_SAVE_IMAGE_PARAM();
                //stSaveParam.enImageType = MyCamera.MV_SAVE_IAMGE_TYPE.MV_Image_Bmp;
                //stSaveParam.enPixelType = pFrameInfo.enPixelType;
                //stSaveParam.pData = pData;
                //stSaveParam.nDataLen = pFrameInfo.nFrameLen;
                //stSaveParam.nHeight = pFrameInfo.nHeight;
                //stSaveParam.nWidth = pFrameInfo.nWidth;
                //stSaveParam.pImageBuffer = pImage;
                //stSaveParam.nBufferSize = m_nBufSizeForSaveImage;
                //stSaveParam.nImageLen = 0;
                //int nRet = m_pOperator.SaveImage(ref stSaveParam);
                //if (CameraOperator.CO_OK != nRet)
                //{
                //    //MessageBox.Show("保存失败!");
                //    return;
                //}

                ////FileStream file = new FileStream("D://image.bmp", FileMode.Create, FileAccess.Write);
                ////file.Write(m_pBufForSaveImage, 0, (int)stSaveParam.nImageLen);
                ////file.Close();
                unsafe
                {
                    if (pFrameInfo.enPixelType == MyCamera.MvGvspPixelType.PixelType_Gvsp_Mono8)
                    {
                        //HOperatorSet.GenImage1Rect(out image, pImage, Convert.ToInt32(pFrameInfo.nWidth), Convert.ToInt32(pFrameInfo.nHeight), Convert.ToInt32(pFrameInfo.nWidth), 8, 8, "true", 0);
                        HOperatorSet.GenImage1(out image, "byte", Convert.ToInt32(pFrameInfo.nWidth), Convert.ToInt32(pFrameInfo.nHeight), pData);

                    }
                    else
                    {
                        if (acqQueue != null)
                        {
                            image.Dispose();
                            _acq.Dispose();
                            _acq.GrabStatus = "current pixel format not support!";
                            acqQueue.Enqueue(_acq);
                            return;
                        }
                    }
                }
                if (acqQueue != null)
                {
                    _acq.Dispose();
                    _acq.Image = image.CopyObj(1, -1);
                    _acq.GrabStatus = "GrabPass";
                    acqQueue.Enqueue(_acq);
                }
                //MyCamera.MV_SAVE_IMAGE_PARAM stSaveParam = new MyCamera.MV_SAVE_IMAGE_PARAM();
                //stSaveParam.enImageType = MyCamera.MV_SAVE_IAMGE_TYPE.MV_Image_Bmp;
                //stSaveParam.enPixelType = pFrameInfo.enPixelType;
                //stSaveParam.pData = pData;
                //stSaveParam.nDataLen = pFrameInfo.nFrameLen;
                //stSaveParam.nHeight = pFrameInfo.nHeight;
                //stSaveParam.nWidth = pFrameInfo.nWidth;
                //stSaveParam.pImageBuffer = pImage;
                //stSaveParam.nBufferSize = m_nBufSizeForSaveImage;
                //stSaveParam.nImageLen = 0;


                //nRet = m_pOperator.SaveImage(ref stSaveParam);
                //if (CameraOperator.CO_OK != nRet)
                //{
                //    //MessageBox.Show("保存失败!");
                //    return;
                //}

                //FileStream file = new FileStream("image.bmp", FileMode.Create, FileAccess.Write);
                //file.Write(m_pBufForSaveImage, 0, (int)stSaveParam.nImageLen);
                //file.Close();
                //MessageBox.Show("保存成功!");
            }
            catch (Exception ex)
            {
                return;
            }

        }

        class CameraOperator
        {
            public const int CO_FAIL = -1;
            public const int CO_OK = 0;
            private MyCamera m_pCSI;
            //private delegate void ImageCallBack(IntPtr pData, ref MyCamera.MV_FRAME_OUT_INFO pFrameInfo, IntPtr pUser);

            public CameraOperator()
            {
                // m_pDeviceList = new MyCamera.MV_CC_DEVICE_INFO_LIST();
                m_pCSI = new MyCamera();
            }

            /****************************************************************************
             * @fn           EnumDevices
             * @brief        枚举可连接设备
             * @param        nLayerType       IN         传输层协议：1-GigE; 4-USB;可叠加
             * @param        stDeviceList     OUT        设备列表
             * @return       成功：0；错误：错误码
             ****************************************************************************/
            public static int EnumDevices(uint nLayerType, ref MyCamera.MV_CC_DEVICE_INFO_LIST stDeviceList)
            {
                return MyCamera.MV_CC_EnumDevices_NET(nLayerType, ref stDeviceList);
            }


            /****************************************************************************
             * @fn           Open
             * @brief        连接设备
             * @param        stDeviceInfo       IN       设备信息结构体
             * @return       成功：0；错误：-1
             ****************************************************************************/
            public int Open(ref MyCamera.MV_CC_DEVICE_INFO stDeviceInfo)
            {
                if (null == m_pCSI)
                {
                    m_pCSI = new MyCamera();
                    if (null == m_pCSI)
                    {
                        return CO_FAIL;
                    }
                }

                int nRet;
                nRet = m_pCSI.MV_CC_CreateDevice_NET(ref stDeviceInfo);
                if (MyCamera.MV_OK != nRet)
                {
                    return CO_FAIL;
                }

                nRet = m_pCSI.MV_CC_OpenDevice_NET();
                if (MyCamera.MV_OK != nRet)
                {
                    return CO_FAIL;
                }
                return CO_OK;
            }


            /****************************************************************************
             * @fn           Close
             * @brief        关闭设备
             * @param        none
             * @return       成功：0；错误：-1
             ****************************************************************************/
            public int Close()
            {
                int nRet;

                nRet = m_pCSI.MV_CC_CloseDevice_NET();
                if (MyCamera.MV_OK != nRet)
                {
                    return CO_FAIL;
                }

                nRet = m_pCSI.MV_CC_DestroyDevice_NET();
                if (MyCamera.MV_OK != nRet)
                {
                    return CO_FAIL;
                }
                return CO_OK;
            }


            /****************************************************************************
             * @fn           StartGrabbing
             * @brief        开始采集
             * @param        none
             * @return       成功：0；错误：-1
             ****************************************************************************/
            public int StartGrabbing()
            {
                int nRet;
                //开始采集
                nRet = m_pCSI.MV_CC_StartGrabbing_NET();
                if (MyCamera.MV_OK != nRet)
                {
                    return CO_FAIL;
                }
                return CO_OK;
            }



            /****************************************************************************
             * @fn           StopGrabbing
             * @brief        停止采集
             * @param        none
             * @return       成功：0；错误：-1
             ****************************************************************************/
            public int StopGrabbing()
            {
                int nRet;
                nRet = m_pCSI.MV_CC_StopGrabbing_NET();
                if (MyCamera.MV_OK != nRet)
                {
                    return CO_FAIL;
                }
                return CO_OK;
            }


            /****************************************************************************
             * @fn           RegisterImageCallBack
             * @brief        注册取流回调函数
             * @param        CallBackFunc          IN        回调函数
             * @param        pUser                 IN        用户参数
             * @return       成功：0；错误：-1
             ****************************************************************************/
            public int RegisterImageCallBack(MyCamera.cbOutputdelegate CallBackFunc, IntPtr pUser)
            {
                int nRet;
                nRet = m_pCSI.MV_CC_RegisterImageCallBack_NET(CallBackFunc, pUser);
                if (MyCamera.MV_OK != nRet)
                {
                    return CO_FAIL;
                }
                return CO_OK;
            }


            /****************************************************************************
             * @fn           RegisterExceptionCallBack
             * @brief        注册异常回调函数
             * @param        CallBackFunc          IN        回调函数
             * @param        pUser                 IN        用户参数
             * @return       成功：0；错误：-1
             ****************************************************************************/
            public int RegisterExceptionCallBack(MyCamera.cbExceptiondelegate CallBackFunc, IntPtr pUser)
            {
                int nRet;
                nRet = m_pCSI.MV_CC_RegisterExceptionCallBack_NET(CallBackFunc, pUser);
                if (MyCamera.MV_OK != nRet)
                {
                    return CO_FAIL;
                }
                return CO_OK;
            }


            /****************************************************************************
             * @fn           GetOneFrame
             * @brief        获取一帧图像数据
             * @param        pData                 IN-OUT            数据数组指针
             * @param        pnDataLen             IN                数据大小
             * @param        nDataSize             IN                数组缓存大小
             * @param        pFrameInfo            OUT               数据信息
             * @return       成功：0；错误：-1
             ****************************************************************************/
            public int GetOneFrame(IntPtr pData, ref UInt32 pnDataLen, UInt32 nDataSize, ref MyCamera.MV_FRAME_OUT_INFO pFrameInfo)
            {
                pnDataLen = 0;
                int nRet = m_pCSI.MV_CC_GetOneFrame_NET(pData, nDataSize, ref pFrameInfo);
                if (MyCamera.MV_OK != nRet)
                {
                    return CO_FAIL;
                }

                pnDataLen = (uint)(pFrameInfo.nWidth * pFrameInfo.nWidth * (((((UInt32)pFrameInfo.enPixelType) >> 16) & 0xffff) >> 3));

                return CO_OK;
            }


            /****************************************************************************
             * @fn           Display
             * @brief        显示图像
             * @param        hWnd                  IN        窗口句柄
             * @return       成功：0；错误：-1
             ****************************************************************************/
            public int Display(IntPtr hWnd)
            {
                int nRet;
                nRet = m_pCSI.MV_CC_Display_NET(hWnd);
                if (MyCamera.MV_OK != nRet)
                {
                    return CO_FAIL;
                }
                return CO_OK;
            }


            /****************************************************************************
             * @fn           GetIntValue
             * @brief        获取Int型参数值
             * @param        strKey                IN        参数键值，具体键值名称参考HikCameraNode.xls文档
             * @param        pnValue               OUT       返回值
             * @return       成功：0；错误：-1
             ****************************************************************************/
            public int GetIntValue(string strKey, ref UInt32 pnValue)
            {

                MyCamera.MVCC_INTVALUE stParam = new MyCamera.MVCC_INTVALUE();
                int nRet = m_pCSI.MV_CC_GetIntValue_NET(strKey, ref stParam);
                if (MyCamera.MV_OK != nRet)
                {
                    return CO_FAIL;
                }

                pnValue = stParam.nCurValue;

                return CO_OK;
            }


            /****************************************************************************
             * @fn           SetIntValue
             * @brief        设置Int型参数值
             * @param        strKey                IN        参数键值，具体键值名称参考HikCameraNode.xls文档
             * @param        nValue                IN        设置参数值，具体取值范围参考HikCameraNode.xls文档
             * @return       成功：0；错误：-1
             ****************************************************************************/
            public int SetIntValue(string strKey, UInt32 nValue)
            {

                int nRet = m_pCSI.MV_CC_SetIntValue_NET(strKey, nValue);
                if (MyCamera.MV_OK != nRet)
                {
                    return CO_FAIL;
                }
                return CO_OK;
            }



            /****************************************************************************
             * @fn           GetFloatValue
             * @brief        获取Float型参数值
             * @param        strKey                IN        参数键值，具体键值名称参考HikCameraNode.xls文档
             * @param        pValue                OUT       返回值
             * @return       成功：0；错误：-1
             ****************************************************************************/
            public int GetFloatValue(string strKey, ref float pfValue)
            {
                MyCamera.MVCC_FLOATVALUE stParam = new MyCamera.MVCC_FLOATVALUE();
                int nRet = m_pCSI.MV_CC_GetFloatValue_NET(strKey, ref stParam);
                if (MyCamera.MV_OK != nRet)
                {
                    return CO_FAIL;
                }

                pfValue = stParam.fCurValue;

                return CO_OK;
            }


            /****************************************************************************
             * @fn           SetFloatValue
             * @brief        设置Float型参数值
             * @param        strKey                IN        参数键值，具体键值名称参考HikCameraNode.xls文档
             * @param        fValue                IN        设置参数值，具体取值范围参考HikCameraNode.xls文档
             * @return       成功：0；错误：-1
             ****************************************************************************/
            public int SetFloatValue(string strKey, float fValue)
            {
                int nRet = m_pCSI.MV_CC_SetFloatValue_NET(strKey, fValue);
                if (MyCamera.MV_OK != nRet)
                {
                    return CO_FAIL;
                }
                return CO_OK;
            }


            /****************************************************************************
             * @fn           GetEnumValue
             * @brief        获取Enum型参数值
             * @param        strKey                IN        参数键值，具体键值名称参考HikCameraNode.xls文档
             * @param        pnValue               OUT       返回值
             * @return       成功：0；错误：-1
             ****************************************************************************/
            public int GetEnumValue(string strKey, ref UInt32 pnValue)
            {
                MyCamera.MVCC_ENUMVALUE stParam = new MyCamera.MVCC_ENUMVALUE();
                int nRet = m_pCSI.MV_CC_GetEnumValue_NET(strKey, ref stParam);
                if (MyCamera.MV_OK != nRet)
                {
                    return CO_FAIL;
                }

                pnValue = stParam.nCurValue;

                return CO_OK;
            }



            /****************************************************************************
             * @fn           SetEnumValue
             * @brief        设置Float型参数值
             * @param        strKey                IN        参数键值，具体键值名称参考HikCameraNode.xls文档
             * @param        nValue                IN        设置参数值，具体取值范围参考HikCameraNode.xls文档
             * @return       成功：0；错误：-1
             ****************************************************************************/
            public int SetEnumValue(string strKey, UInt32 nValue)
            {
                int nRet = m_pCSI.MV_CC_SetEnumValue_NET(strKey, nValue);
                if (MyCamera.MV_OK != nRet)
                {
                    return CO_FAIL;
                }
                return CO_OK;
            }



            /****************************************************************************
             * @fn           GetBoolValue
             * @brief        获取Bool型参数值
             * @param        strKey                IN        参数键值，具体键值名称参考HikCameraNode.xls文档
             * @param        pbValue               OUT       返回值
             * @return       成功：0；错误：-1
             ****************************************************************************/
            public int GetBoolValue(string strKey, ref bool pbValue)
            {
                int nRet = m_pCSI.MV_CC_GetBoolValue_NET(strKey, ref pbValue);
                if (MyCamera.MV_OK != nRet)
                {
                    return CO_FAIL;
                }

                return CO_OK;
            }


            /****************************************************************************
             * @fn           SetBoolValue
             * @brief        设置Bool型参数值
             * @param        strKey                IN        参数键值，具体键值名称参考HikCameraNode.xls文档
             * @param        bValue                IN        设置参数值，具体取值范围参考HikCameraNode.xls文档
             * @return       成功：0；错误：-1
             ****************************************************************************/
            public int SetBoolValue(string strKey, bool bValue)
            {
                int nRet = m_pCSI.MV_CC_SetBoolValue_NET(strKey, bValue);
                if (MyCamera.MV_OK != nRet)
                {
                    return CO_FAIL;
                }
                return CO_OK;
            }


            /****************************************************************************
             * @fn           GetStringValue
             * @brief        获取String型参数值
             * @param        strKey                IN        参数键值，具体键值名称参考HikCameraNode.xls文档
             * @param        strValue              OUT       返回值
             * @return       成功：0；错误：-1
             ****************************************************************************/
            public int GetStringValue(string strKey, ref string strValue)
            {
                MyCamera.MVCC_STRINGVALUE stParam = new MyCamera.MVCC_STRINGVALUE();
                int nRet = m_pCSI.MV_CC_GetStringValue_NET(strKey, ref stParam);
                if (MyCamera.MV_OK != nRet)
                {
                    return CO_FAIL;
                }

                strValue = stParam.chCurValue;

                return CO_OK;
            }


            /****************************************************************************
             * @fn           SetStringValue
             * @brief        设置String型参数值
             * @param        strKey                IN        参数键值，具体键值名称参考HikCameraNode.xls文档
             * @param        strValue              IN        设置参数值，具体取值范围参考HikCameraNode.xls文档
             * @return       成功：0；错误：-1
             ****************************************************************************/
            public int SetStringValue(string strKey, string strValue)
            {
                int nRet = m_pCSI.MV_CC_SetStringValue_NET(strKey, strValue);
                if (MyCamera.MV_OK != nRet)
                {
                    return CO_FAIL;
                }
                return CO_OK;
            }


            /****************************************************************************
             * @fn           CommandExecute
             * @brief        Command命令
             * @param        strKey                IN        参数键值，具体键值名称参考HikCameraNode.xls文档
             * @return       成功：0；错误：-1
             ****************************************************************************/
            public int CommandExecute(string strKey)
            {
                int nRet = m_pCSI.MV_CC_SetCommandValue_NET(strKey);
                if (MyCamera.MV_OK != nRet)
                {
                    return CO_FAIL;
                }
                return CO_OK;
            }


            /****************************************************************************
             * @fn           SaveImage
             * @brief        保存图片
             * @param        pSaveParam            IN        保存图片配置参数结构体
             * @return       成功：0；错误：-1
             ****************************************************************************/
            public int SaveImage(ref MyCamera.MV_SAVE_IMAGE_PARAM pSaveParam)
            {
                int nRet;
                nRet = m_pCSI.MV_CC_SaveImage_NET(ref pSaveParam);
                if (MyCamera.MV_OK != nRet)
                {
                    return CO_FAIL;
                }
                return CO_OK;
            }
        }
        #endregion
    }
    public class SVSEuresysCamLink : BaseCamera
    {
        #region 相机自带函数
        /// <summary>
        /// Native functions imported from the MultiCam C API.
        /// </summary>
        #region Native Methods
        class NativeMethods
        {
            private NativeMethods() { }

            [DllImport("MultiCam.dll")]
            internal static extern int McOpenDriver(IntPtr instanceName);
            [DllImport("MultiCam.dll")]
            internal static extern int McCloseDriver();
            [DllImport("MultiCam.dll")]
            internal static extern int McCreate(UInt32 modelInstance, out UInt32 instance);
            [DllImport("MultiCam.dll")]
            internal static extern int McCreateNm(String modelName, out UInt32 instance);
            [DllImport("MultiCam.dll")]
            internal static extern int McDelete(UInt32 instance);
            [DllImport("MultiCam.dll")]
            internal static extern int McSetParamInt(UInt32 instance, UInt32 parameterId, int value);
            [DllImport("MultiCam.dll")]
            internal static extern int McSetParamNmInt(UInt32 instance, String parameterName, int value);
            [DllImport("MultiCam.dll")]
            internal static extern int McSetParamStr(UInt32 instance, UInt32 parameterId, String value);
            [DllImport("MultiCam.dll")]
            internal static extern int McSetParamNmStr(UInt32 instance, String parameterName, String value);
            [DllImport("MultiCam.dll")]
            internal static extern int McSetParamFloat(UInt32 instance, UInt32 parameterId, Double value);
            [DllImport("MultiCam.dll")]
            internal static extern int McSetParamNmFloat(UInt32 instance, String parameterName, Double value);
            [DllImport("MultiCam.dll")]
            internal static extern int McSetParamInst(UInt32 instance, UInt32 parameterId, UInt32 value);
            [DllImport("MultiCam.dll")]
            internal static extern int McSetParamNmInst(UInt32 instance, String parameterName, UInt32 value);
            [DllImport("MultiCam.dll")]
            internal static extern int McSetParamPtr(UInt32 instance, UInt32 parameterId, IntPtr value);
            [DllImport("MultiCam.dll")]
            internal static extern int McSetParamNmPtr(UInt32 instance, String parameterName, IntPtr value);
            [DllImport("MultiCam.dll")]
            internal static extern int McSetParamInt64(UInt32 instance, UInt32 parameterId, Int64 value);
            [DllImport("MultiCam.dll")]
            internal static extern int McSetParamNmInt64(UInt32 instance, String parameterName, Int64 value);
            [DllImport("MultiCam.dll")]
            internal static extern int McGetParamInt(UInt32 instance, UInt32 parameterId, out int value);
            [DllImport("MultiCam.dll")]
            internal static extern int McGetParamNmInt(UInt32 instance, String parameterName, out int value);
            [DllImport("MultiCam.dll")]
            internal static extern int McGetParamStr(UInt32 instance, UInt32 parameterId, IntPtr value, UInt32 maxLength);
            [DllImport("MultiCam.dll")]
            internal static extern int McGetParamNmStr(UInt32 instance, String parameterName, IntPtr value, UInt32 maxLength);
            [DllImport("MultiCam.dll")]
            internal static extern int McGetParamFloat(UInt32 instance, UInt32 parameterId, out Double value);
            [DllImport("MultiCam.dll")]
            internal static extern int McGetParamNmFloat(UInt32 instance, String parameterName, out Double value);
            [DllImport("MultiCam.dll")]
            internal static extern int McGetParamInst(UInt32 instance, UInt32 parameterId, out UInt32 value);
            [DllImport("MultiCam.dll")]
            internal static extern int McGetParamNmInst(UInt32 instance, String parameterName, out UInt32 value);
            [DllImport("MultiCam.dll")]
            internal static extern int McGetParamPtr(UInt32 instance, UInt32 parameterId, out IntPtr value);
            [DllImport("MultiCam.dll")]
            internal static extern int McGetParamPtr(UInt32 instance, UInt32 parameterId, out Microsoft.Win32.SafeHandles.SafeWaitHandle value);
            [DllImport("MultiCam.dll")]
            internal static extern int McGetParamNmPtr(UInt32 instance, String parameterName, out IntPtr value);
            [DllImport("MultiCam.dll")]
            internal static extern int McGetParamInt64(UInt32 instance, UInt32 parameterId, out Int64 value);
            [DllImport("MultiCam.dll")]
            internal static extern int McGetParamNmInt64(UInt32 instance, String parameterName, out Int64 value);
            [DllImport("MultiCam.dll")]
            internal static extern int McRegisterCallback(UInt32 instance, CALLBACK callbackFunction, UInt32 context);
            [DllImport("MultiCam.dll")]
            internal static extern int McWaitSignal(UInt32 instance, int signal, UInt32 timeout, out SIGNALINFO info);
            [DllImport("MultiCam.dll")]
            internal static extern int McGetSignalInfo(UInt32 instance, int signal, out SIGNALINFO info);
        }
        #endregion

        #region Private Constants
        private const int MAX_VALUE_LENGTH = 1024;
        #endregion

        #region Default object instance Constants
        public const UInt32 CONFIGURATION = 0x20000000;
        public const UInt32 BOARD = 0xE0000000;
        public const UInt32 CHANNEL = 0X8000FFFF;
        public const UInt32 DEFAULT_SURFACE_HANDLE = 0x4FFFFFFF;
        #endregion

        #region Specific parameter values Constants
        public const int INFINITE = -1;
        public const int INDETERMINATE = -1;
        public const int DISABLE = 0;
        #endregion

        #region Signal handling Constants
        public const UInt32 SignalEnable = (24 << 14);

        public const int SIG_ANY = 0;
        public const int SIG_SURFACE_PROCESSING = 1;
        public const int SIG_SURFACE_FILLED = 2;
        public const int SIG_UNRECOVERABLE_OVERRUN = 3;
        public const int SIG_FRAMETRIGGER_VIOLATION = 4;
        public const int SIG_START_EXPOSURE = 5;
        public const int SIG_END_EXPOSURE = 6;
        public const int SIG_ACQUISITION_FAILURE = 7;
        public const int SIG_CLUSTER_UNAVAILABLE = 8;
        public const int SIG_RELEASE = 9;
        public const int SIG_END_ACQUISITION_SEQUENCE = 10;
        public const int SIG_START_ACQUISITION_SEQUENCE = 11;
        public const int SIG_END_CHANNEL_ACTIVITY = 12;

        public const int SIG_GOLOW = (1 << 12);
        public const int SIG_GOHIGH = (2 << 12);
        public const int SIG_GOOPEN = (3 << 12);
        #endregion

        #region Signal handling Type Definitions
        public delegate void CALLBACK(ref SIGNALINFO signalInfo);


        public const UInt32 SignalHandling = (74 << 14);
        public const int SIGNALHANDLING_ANY = 1;
        public const int SIGNALHANDLING_CALLBACK_SIGNALING = 2;
        public const int SIGNALHANDLING_WAITING_SIGNALING = 3;
        public const int SIGNALHANDLING_OS_EVENT_SIGNALING = 4;

        public const UInt32 SurfaceState = (31 << 14);
        public const int SURFACESTATE_FREE = 1;
        public const int SURFACESTATE_FILLING = 2;
        public const int SURFACESTATEe_FILLED = 3;
        public const int SURFACESTATE_PROCESSING = 4;
        public const int SURFACESTATE_RESERVED = 5;

        public const UInt32 SignalEvent = (25 << 14);


        [StructLayout(LayoutKind.Sequential)]
        public struct SIGNALINFO
        {
            public IntPtr Context;
            public UInt32 Instance;
            public int Signal;
            public UInt32 SignalInfo;
            public UInt32 SignalContext;
        };
        #endregion

        #region Error handling Methods
        private static String GetErrorMessage(int errorCode)
        {
            const UInt32 ErrorDesc = (98 << 14);
            String errorDescription;
            UInt32 status = (UInt32)Math.Abs(errorCode);
            IntPtr text = Marshal.AllocHGlobal(MAX_VALUE_LENGTH + 1);
            if (NativeMethods.McGetParamStr(CONFIGURATION, ErrorDesc + status, text, MAX_VALUE_LENGTH) != 0)
                errorDescription = "Unknown error";
            else
                errorDescription = Marshal.PtrToStringAnsi(text);
            Marshal.FreeHGlobal(text);
            return errorDescription;
        }
        class MultiCamException : System.Exception
        {
            public MultiCamException(String error) : base(error) { }
        }
        private static void ThrowOnMultiCamError(int status, String action)
        {
            if (status != 0)
            {
                String error = action + ": " + GetErrorMessage(status);
                throw new MultiCamException(error);
            }
        }
        #endregion

        #region Driver connection Methods
        public static void OpenDriver()
        {
            ThrowOnMultiCamError(NativeMethods.McOpenDriver((IntPtr)null),
                "Cannot open MultiCam driver");
        }

        public static void CloseDriver()
        {
            ThrowOnMultiCamError(NativeMethods.McCloseDriver(),
                "Cannot close MultiCam driver");
        }
        #endregion

        #region Object creation/deletion Methods
        public static void Create(UInt32 modelInstance, out UInt32 instance)
        {
            ThrowOnMultiCamError(NativeMethods.McCreate(modelInstance, out instance),
                String.Format("Cannot create '{0}' instance", modelInstance));
        }

        public static void Create(String modelName, out UInt32 instance)
        {
            ThrowOnMultiCamError(NativeMethods.McCreateNm(modelName, out instance),
                String.Format("Cannot create '{0}' instance", modelName));
        }

        public static void Delete(UInt32 instance)
        {
            ThrowOnMultiCamError(NativeMethods.McDelete(instance),
                String.Format("Cannot delete '{0}' instance", instance));
        }
        #endregion

        #region Parameter 'setter' Methods
        public static void SetParam(UInt32 instance, UInt32 parameterId, int value)
        {
            ThrowOnMultiCamError(NativeMethods.McSetParamInt(instance, parameterId, value),
                String.Format("Cannot set parameter '{0}' to value '{1}'", parameterId, value));
        }

        public static void SetParam(UInt32 instance, String parameterName, int value)
        {
            ThrowOnMultiCamError(NativeMethods.McSetParamNmInt(instance, parameterName, value),
                String.Format("Cannot set parameter '{0}' to value '{1}'", parameterName, value));
        }

        public static void SetParam(UInt32 instance, UInt32 parameterId, String value)
        {
            ThrowOnMultiCamError(NativeMethods.McSetParamStr(instance, parameterId, value),
                String.Format("Cannot set parameter '{0}' to value '{1}'", parameterId, value));
        }

        public static void SetParam(UInt32 instance, String parameterName, String value)
        {
            ThrowOnMultiCamError(NativeMethods.McSetParamNmStr(instance, parameterName, value),
                String.Format("Cannot set parameter '{0}' to value '{1}'", parameterName, value));
        }

        public static void SetParam(UInt32 instance, UInt32 parameterId, Double value)
        {
            ThrowOnMultiCamError(NativeMethods.McSetParamFloat(instance, parameterId, value),
                String.Format("Cannot set parameter '{0}' to value '{1}'", parameterId, value));
        }

        public static void SetParam(UInt32 instance, String parameterName, Double value)
        {
            ThrowOnMultiCamError(NativeMethods.McSetParamNmFloat(instance, parameterName, value),
                String.Format("Cannot set parameter '{0}' to value '{1}'", parameterName, value));
        }

        public static void SetParam(UInt32 instance, UInt32 parameterId, UInt32 value)
        {
            ThrowOnMultiCamError(NativeMethods.McSetParamInst(instance, parameterId, value),
                String.Format("Cannot set parameter '{0}' to value '{1}'", parameterId, value));
        }

        public static void SetParam(UInt32 instance, String parameterName, UInt32 value)
        {
            ThrowOnMultiCamError(NativeMethods.McSetParamNmInst(instance, parameterName, value),
                String.Format("Cannot set parameter '{0}' to value '{1}'", parameterName, value));
        }

        public static void SetParam(UInt32 instance, UInt32 parameterId, IntPtr value)
        {
            ThrowOnMultiCamError(NativeMethods.McSetParamPtr(instance, parameterId, value),
                String.Format("Cannot set parameter '{0}' to value '{1}'", parameterId, value));
        }

        public static void SetParam(UInt32 instance, String parameterName, IntPtr value)
        {
            ThrowOnMultiCamError(NativeMethods.McSetParamNmPtr(instance, parameterName, value),
                String.Format("Cannot set parameter '{0}' to value '{1}'", parameterName, value));
        }

        public static void SetParam(UInt32 instance, UInt32 parameterId, Int64 value)
        {
            ThrowOnMultiCamError(NativeMethods.McSetParamInt64(instance, parameterId, value),
                String.Format("Cannot set parameter '{0}' to value '{1}'", parameterId, value));
        }

        public static void SetParam(UInt32 instance, String parameterName, Int64 value)
        {
            ThrowOnMultiCamError(NativeMethods.McSetParamNmInt64(instance, parameterName, value),
                String.Format("Cannot set parameter '{0}' to value '{1}'", parameterName, value));
        }
        #endregion

        #region Parameter 'getter' Methods
        public static void GetParam(UInt32 instance, UInt32 parameterId, out int value)
        {
            ThrowOnMultiCamError(NativeMethods.McGetParamInt(instance, parameterId, out value),
                String.Format("Cannot get parameter '{0}'", parameterId));
        }

        public static void GetParam(UInt32 instance, String parameterName, out int value)
        {
            ThrowOnMultiCamError(NativeMethods.McGetParamNmInt(instance, parameterName, out value),
                String.Format("Cannot get parameter '{0}'", parameterName));
        }

        public static void GetParam(UInt32 instance, UInt32 parameterId, out String value)
        {
            IntPtr text = Marshal.AllocHGlobal(MAX_VALUE_LENGTH + 1);
            try
            {
                ThrowOnMultiCamError(NativeMethods.McGetParamStr(instance, parameterId, text, MAX_VALUE_LENGTH),
                    String.Format("Cannot get parameter '{0}'", parameterId));
                value = Marshal.PtrToStringAnsi(text);
            }
            finally
            {
                Marshal.FreeHGlobal(text);
            }
        }

        public static void GetParam(UInt32 instance, String parameterName, out String value)
        {
            IntPtr text = Marshal.AllocHGlobal(MAX_VALUE_LENGTH + 1);
            try
            {
                ThrowOnMultiCamError(NativeMethods.McGetParamNmStr(instance, parameterName, text, MAX_VALUE_LENGTH),
                    String.Format("Cannot get parameter '{0}'", parameterName));
                value = Marshal.PtrToStringAnsi(text);
            }
            finally
            {
                Marshal.FreeHGlobal(text);
            }
        }

        public static void GetParam(UInt32 instance, UInt32 parameterId, out Double value)
        {
            ThrowOnMultiCamError(NativeMethods.McGetParamFloat(instance, parameterId, out value),
                String.Format("Cannot get parameter '{0}'", parameterId));
        }

        public static void GetParam(UInt32 instance, String parameterName, out Double value)
        {
            ThrowOnMultiCamError(NativeMethods.McGetParamNmFloat(instance, parameterName, out value),
                String.Format("Cannot get parameter '{0}'", parameterName));
        }

        public static void GetParam(UInt32 instance, UInt32 parameterId, out UInt32 value)
        {
            ThrowOnMultiCamError(NativeMethods.McGetParamInst(instance, parameterId, out value),
                String.Format("Cannot get parameter '{0}'", parameterId));
        }

        public static void GetParam(UInt32 instance, String parameterName, out UInt32 value)
        {
            ThrowOnMultiCamError(NativeMethods.McGetParamNmInst(instance, parameterName, out value),
                String.Format("Cannot get parameter '{0}'", parameterName));
        }

        public static void GetParam(UInt32 instance, UInt32 parameterId, out IntPtr value)
        {
            ThrowOnMultiCamError(NativeMethods.McGetParamPtr(instance, parameterId, out value),
                String.Format("Cannot get parameter '{0}'", parameterId));
        }


        public static void GetParam(UInt32 instance, UInt32 parameterId, out Microsoft.Win32.SafeHandles.SafeWaitHandle value)
        {
            ThrowOnMultiCamError(NativeMethods.McGetParamPtr(instance, parameterId, out value),
                String.Format("Cannot get parameter '{0}'", parameterId));
        }

        public static void GetParam(UInt32 instance, String parameterName, out IntPtr value)
        {
            ThrowOnMultiCamError(NativeMethods.McGetParamNmPtr(instance, parameterName, out value),
                String.Format("Cannot get parameter '{0}'", parameterName));
        }

        public static void GetParam(UInt32 instance, UInt32 parameterId, out Int64 value)
        {
            ThrowOnMultiCamError(NativeMethods.McGetParamInt64(instance, parameterId, out value),
                String.Format("Cannot get parameter '{0}'", parameterId));
        }

        public static void GetParam(UInt32 instance, String parameterName, out Int64 value)
        {
            ThrowOnMultiCamError(NativeMethods.McGetParamNmInt64(instance, parameterName, out value),
                String.Format("Cannot get parameter '{0}'", parameterName));
        }
        #endregion

        #region Signal handling Methods
        public static void RegisterCallback(UInt32 instance, CALLBACK callbackFunction, UInt32 context)
        {
            ThrowOnMultiCamError(NativeMethods.McRegisterCallback(instance, callbackFunction, context),
                "Cannot register callback");
        }

        public static void WaitSignal(UInt32 instance, int signal, UInt32 timeout, out SIGNALINFO info)
        {
            ThrowOnMultiCamError(NativeMethods.McWaitSignal(instance, signal, timeout, out info),
                "WaitSignal error");
        }

        public static void GetSignalInfo(UInt32 instance, int signal, out SIGNALINFO info)
        {
            ThrowOnMultiCamError(NativeMethods.McGetSignalInfo(instance, signal, out info),
                "Cannot get signal information");
        }
        #endregion

        #endregion
        #region 配置参数
        CALLBACK multiCamCallback;
        private string sn = "";
        private string camFile = "beA4000-62km_P62RG";
        private string pixelType = EuresysPixelFormat.Y8;
        private bool isSoftwareTrigger = true;
        private bool isCameraTrigger = true;
        private bool isMirrorX = false;
        private bool isMirrorY = false;
        private const int MAX_BUFFER_NUM = 100;
        System.Collections.Concurrent.ConcurrentDictionary<int, byte[]> dBuffer = new System.Collections.Concurrent.ConcurrentDictionary<int, byte[]>();
        private int num = 0;
        // The MultiCam object that controls the acquisition
        private UInt32 channel;
        int width, height, bufferPitch;
        //与相机串口通讯接口
        private System.IO.Ports.SerialPort sPort;
        private byte[][] buffer = new byte[MAX_BUFFER_NUM][];

        #endregion
        public SVSEuresysCamLink(string serialNumber, string camFile)
        {
            this.sn = serialNumber;
            this.camFile = camFile;// "C:\\Users\\1\\Desktop\\camfile\\HR25000CCL_10TAP_8Bit.cam";
        }
        public override bool InitCamera()
        {
            try
            {
                if (this.sPort != null && this.sPort.IsOpen)
                {
                    this.sPort.Close();
                    this.sPort.Dispose();
                }
                this.sPort = new System.IO.Ports.SerialPort(this.sn, 115200, System.IO.Ports.Parity.None, 8);
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }
        public override bool OpenCamera()
        {
            try
            {
                // Open MultiCam driver
                OpenDriver();
                // Enable error logging
                SetParam(CONFIGURATION, "ErrorLog", "error.log");

                // Create a channel and associate it with the first connector on the first board

                SetParam(BOARD + 0, "BoardTopology", "MONO_DECA");

                SetParam(BOARD + 0, "SerialControlA", sn);

                Create("CHANNEL", out channel);
                //不同板卡设置不同索引？
                SetParam(channel, "DriverIndex", 0);
                // For all Domino boards except Domino Symphony
                SetParam(channel, "Connector", "M");
                // Choose the CAM file
                SetParam(channel, "CamFile", camFile);
                // Choose the pixel color format
                SetParam(channel, "ColorMethod", "BAYER");
                SetParam(channel, "ColorRegistration", "RG");
                SetParam(channel, "ColorFormat", pixelType);

                // Choose the way the first acquisition is triggered
                SetParam(channel, "AcquisitionMode", "HFR");
                SetParam(channel, "TrigMode", "IMMEDIATE");
                //SetParam(channel, "TrigMode", "HARD");
                SetParam(channel, "NextTrigMode", "SAME");
                //SetParam(channel, "ImageFlipY", "ON");
                SetParam(channel, "TriggerSkipHold", "HOLD");
                SetParam(channel, "Expose", "WIDTH");
                // Choose the number of images to acquire
                SetParam(channel, "SeqLength_Fr", INDETERMINATE);
                SetParam(channel, "TrigCtl", "ISO");
                SetParam(channel, "TrigLine", "IIN1");
                SetParam(channel, "AcqTimeout_ms", "-1");

                GetParam(channel, "ImageSizeX", out width);
                GetParam(channel, "ImageSizeY", out height);
                GetParam(channel, "BufferPitch", out bufferPitch);
                if (isMirrorX) SetParam(channel, "ImageFlipX", "ON");
                if (isMirrorY) SetParam(channel, "ImageFlipY", "ON");
                // Register the callback function
                multiCamCallback = new CALLBACK(MultiCamCallback);
                RegisterCallback(channel, multiCamCallback, channel);

                // Enable the signals corresponding to the callback functions
                SetParam(channel, SignalEnable + SIG_SURFACE_PROCESSING, "ON");
                SetParam(channel, SignalEnable + SIG_ACQUISITION_FAILURE, "ON");
                for (int i = 0; i < buffer.GetLength(0); i++)
                {
                    buffer[i] = new byte[bufferPitch * height];
                }
                // Prepare the channel in order to minimize the acquisition sequence startup latency
                this.sPort.Open();
                //user set
                //this.sPort.WriteLine("VBF00,1\r\n");
                //string reply1 = this.sPort.ReadExisting();
                //if (reply1.Contains("Error")) return false;

                //set camera trigger mode on
                this.sPort.WriteLine("VB23C,1\r\n");
                string reply = this.sPort.ReadExisting();
                if (reply.Contains("Error"))
                    return false;
                //set camera exposure mode:trigger width
                this.sPort.WriteLine("VB254,1\r\n");
                reply = this.sPort.ReadExisting();
                if (reply.Contains("Error"))
                    return false;
                ChangeTriggerSource(this.isSoftwareTrigger);
                return true;
            }
            catch (MultiCamException)
            {
                CloseCamera();
                return false;
            }
        }
        public override bool CloseCamera()
        {
            try
            {
                ClearImageQueue();
                CloseDriver();
                if (this.sPort != null && this.sPort.IsOpen)
                {
                    this.sPort.Close();
                    this.sPort.Dispose();
                }
                return true;
            }
            catch (MultiCamException)
            {
                return false;
            }
        }
        public override bool IsSoftwareTrigger
        {
            set { this.isSoftwareTrigger = value; }
            get { return this.isSoftwareTrigger; }
        }
        public override bool IsCameraTrigger
        {
            set { this.isCameraTrigger = value; }
            get { return this.isCameraTrigger; }
        }
        public override bool IsConnected
        {
            get
            {
                return false;
            }
        }
        public override bool IsMirrorX
        {
            set { this.isMirrorX = value; }
            get { return this.isMirrorX; }
        }
        public override bool IsMirrorY
        {
            set { this.isMirrorY = value; }
            get { return this.isMirrorY; }
        }
        public override bool ChangeTriggerSource(bool isSoftwareTrigger)
        {
            SetParam(channel, "ChannelState", "IDLE");
            if (isCameraTrigger)
            {
                SetParam(channel, "TrigMode", "IMMEDIATE");
                //set camera trigger mode on
                //this.sPort.WriteLine("VB23C,1\r\n");
                //string reply = this.sPort.ReadExisting();
                //if (reply.Contains("Error"))
                //    return false;
                if (isSoftwareTrigger)
                    this.sPort.WriteLine("VB240,0\r\n");
                else
                    this.sPort.WriteLine("VB240,1\r\n");
                string reply = this.sPort.ReadExisting();
                if (reply.Contains("Error"))
                    return false;
            }
            else
            {
                //set camera trigger mode off
                //this.sPort.WriteLine("VB23C,0\r\n");
                //string reply = this.sPort.ReadExisting();
                //if (reply.Contains("Error"))
                //    return false;
                if (isSoftwareTrigger)
                    SetParam(channel, "TrigMode", "SOFT");
                else
                {
                    SetParam(channel, "TrigMode", "HARD");
                    //SetParam(channel, "TrigCtl", "ISO");
                    //SetParam(channel, "TrigLine", "IIN1");
                }
            }
            SetParam(channel, "ChannelState", "ACTIVE");
            this.isSoftwareTrigger = isSoftwareTrigger;
            return true;
        }
        public override bool SetGain(double value)
        {
            try
            {
                if (value < 0) value = 0;
                if (value > 10) value = 10;
                double unit = 1000;// 28.2386;
                string gain = Convert.ToString((int)(value * unit), 16).ToUpper();
                this.sPort.WriteLine("VB048," + gain + "\r\n");
                string reply = this.sPort.ReadExisting();
                if (reply.Contains("Error")) return false;
                //if (!camera.Parameters[PLCamera.GainRaw].TrySetValue((long)value)) return false;
                //MultiCamToHalcon.SetParam(channel, "Gain_us", value);
                //    return false;
                return true;
            }
            catch (Exception)
            {
                return false;
            }


        }
        public override void GetGain(out double value)
        {
            try
            {
                double unit = 1000;// 28.2386;
                value = -1;
                this.sPort.WriteLine("rVB048\r\n");
                string reply = this.sPort.ReadExisting();
                if (reply.Contains("Error"))
                    return;
                int ind = reply.IndexOfAny(new char[] { '\r' });
                value = Int32.Parse(reply.Substring(0, ind), System.Globalization.NumberStyles.HexNumber);
                value = (int)(value / unit);
                // camera.Parameters[PLCamera.GainRaw].GetValue();
                //MultiCamToHalcon.GetParam(channel, "Gain_us", out value);
            }
            catch (Exception)
            {
                value = -1;
            }
        }
        public override bool SetExposure(double value)
        {
            try
            {
                //string exposure = Convert.ToString((int)value, 16);
                //this.sPort.WriteLine("VB02C," + exposure + "\r\n");
                //string reply = this.sPort.ReadExisting();
                //if (reply.Contains("Error")) return false;
                SetParam(channel, "Expose_us", (int)value);

                //if (!camera.Parameters[PLCamera.ExposureTimeAbs].TrySetValue(value)) return false;
                //MultiCamToHalcon.SetParam(channel, "Expose_us", value);
                return true;
            }
            catch (Exception)
            {
                //camera.Parameters[PLCamera.ExposureTime].TrySetValue(100);
                return false;
            }
        }
        public override void GetExposure(out double value)
        {
            try
            {
                value = -1;
                //this.sPort.WriteLine("rVB02C\r\n");
                //string reply = this.sPort.ReadExisting();
                //if (reply.Contains("Error"))
                //    return;
                //int ind = reply.IndexOfAny(new char[] { '\r' });
                //value = Int32.Parse(reply.Substring(0, ind), System.Globalization.NumberStyles.HexNumber);

                int expose = -1;
                GetParam(channel, "Expose_us", out expose);
                value = expose;

                // camera.Parameters[PLCamera.ExposureTimeAbs].GetValue();
                //MultiCamToHalcon.GetParam(channel, "Expose_us", out value);
            }
            catch (Exception)
            {
                value = -1;
            }

        }
        public void SetGamma(bool value)
        {

        }
        public void GetGamma(out bool value)
        {
            try
            {
                value = false;
            }
            catch (Exception)
            {
                value = false;
            }
        }
        /// <summary>设置相机的像素格式
        /// 设置相机的像素格式
        /// </summary>
        /// <param name="value">value可以设置为如:BaslerToHalcon.PixelFormat.Mono8</param>
        public override bool EuresysSetPixelFormat(string value)
        {
            try
            {
                this.pixelType = value;
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }
        public override void EuresysGetPixelFormat(out string value)
        {
            try
            {
                value = this.pixelType;
            }
            catch (Exception)
            {
                value = "";
            }

        }
        public override bool ClearImageQueue()
        {
            try
            {
                num = 0;
                int count = this.dBuffer.Count;
                for (int i = 0; i < count; i++)
                {
                    if (this.dBuffer[i] != null)
                    {
                        this.dBuffer[i] = null;
                    }
                }
                this.dBuffer.Clear();
                //bool isNotEmpty = false;
                //for (int i = 0; i < this.buffer.GetLength(0); i++)
                //{
                //    isNotEmpty = this.buffer[i].Any(p => p != 0);
                //    if (!isNotEmpty) break;
                //    this.buffer[i] = null;
                //    this.buffer[i] = new byte[bufferPitch * height];
                //}
                GC.Collect();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public override Acquisition Snap(int imageNum = 1, int timeOut = -1)
        {
            Acquisition _acq = new Acquisition();
            // Generate a soft trigger event
            if (this.isCameraTrigger)
            {
                this.sPort.WriteLine("VB248,0<newline>");
                string reply = this.sPort.ReadExisting();
                if (reply.Contains("Error"))
                {
                    _acq.GrabStatus = "GrabFail:Camera gen soft trigger falied!";
                    return _acq;
                }
            }
            else
            {
                SetParam(channel, "ForceTrig", "TRIG");
            }
            _acq = GetFrames(imageNum, timeOut);
            return _acq;
        }
        public override Acquisition GetFrames(int imageNum = 1, int timeOut = -1)
        {
            Acquisition _acq = new Acquisition();
            try
            {
                DateTime start = DateTime.Now;
                timeOut = (timeOut == -1) ? 100000 : timeOut;
                Acquisition[] acqs = new Acquisition[imageNum];
                int threadNum = 2;
                int maxThreadNum = Math.Min(threadNum, imageNum);
                Parallel.For(0, maxThreadNum, i =>
                {
                    acqs[i] = new Acquisition();
                    ImageConvert(start, i, maxThreadNum, imageNum, ref acqs, timeOut);
                });
                double time1 = DateTime.Now.Subtract(start).TotalMilliseconds;

                for (int i = 0; i < imageNum; i++)
                {
                    _acq.GrabStatus = acqs[i].GrabStatus;
                    if (acqs[i].GrabStatus == "GrabPass")
                    {
                        HOperatorSet.ConcatObj(_acq.Image, acqs[i].Image, out _acq.Image);
                        _acq.index++;
                    }
                }
                return _acq;
            }
            catch (Exception ex)
            {
                _acq.GrabStatus = "GrabFail:" + ex.Message;
                return _acq;
            }
            finally
            {
                num = 0;
            }
        }
        public override Task<Acquisition> GetFramesAsync(int imageNum = 1, int timeOut = -1)
        {
            return Task.Run(() =>
            {
                return GetFrames(imageNum, timeOut);
            });
        }
        #region 调用的函数
        private void ImageConvert(DateTime start, int ind, ref HObject image, ref string grabStatus, int timeOut = -1)
        {
            while (DateTime.Now.Subtract(start).TotalMilliseconds < timeOut)
            {
                if (!this.dBuffer.ContainsKey(ind))
                    continue;
                if (this.dBuffer.Count == 0 || this.dBuffer[ind] == null)
                    continue;
                try
                {
                    byte[] buffer = this.dBuffer[ind];
                    unsafe
                    {
                        fixed (byte* p = buffer)
                        {
                            IntPtr ptr = (IntPtr)p;
                            image.Dispose();
                            if (this.pixelType == EuresysPixelFormat.Y8)
                            {
                                //黑白图像
                                HOperatorSet.GenImage1(out image, "byte", Convert.ToInt32(width), Convert.ToInt32(height), ptr);
                            }
                            else if (this.pixelType == EuresysPixelFormat.BAYER8)
                            {
                                HOperatorSet.GenImage1(out image, "byte", Convert.ToInt32(width), Convert.ToInt32(height), ptr);
                                //HOperatorSet.WriteImage(image, "tiff", 0, "D:\\bayer.tiff");
                                HOperatorSet.CfaToRgb(image, out image, "bayer_rg", "bilinear");
                                //HOperatorSet.WriteImage(image, "tiff", 0, "D:\\color.tiff");
                            }
                            else
                            {
                                image.Dispose();
                                grabStatus = "current pixel format not support!";
                                return;
                            }

                        }
                        grabStatus = "GrabPass";
                    }
                    return;
                }
                catch (Exception ex)
                {
                    grabStatus = "GrabFail:" + ex.Message;
                    return;
                }
                finally
                {
                    this.dBuffer[ind] = null;
                }
            }
            grabStatus = "GrabTimeOut";
            return;
        }
        private void ImageConvert(DateTime startTime, int start, int step, int imageNum, ref Acquisition[] acqs, int timeOut = -1)
        {
            for (int i = start; i < imageNum; i += step)
            {
                acqs[i] = new Acquisition();
                ImageConvert(startTime, i, ref acqs[i].Image, ref acqs[i].GrabStatus, timeOut);
            }
        }
        private void MultiCamCallback(ref SIGNALINFO signalInfo)
        {
            switch (signalInfo.Signal)
            {
                case SIG_SURFACE_PROCESSING:
                    ProcessingCallback(signalInfo);
                    break;
                case SIG_ACQUISITION_FAILURE:
                    AcqFailureCallback(signalInfo);
                    break;
                default:
                    throw new MultiCamException("Unknown signal");
            }
        }
        private void ProcessingCallback(SIGNALINFO signalInfo)
        {
            if (num >= MAX_BUFFER_NUM)
            {
                ClearImageQueue();
            }
            UInt32 currentSurface = signalInfo.SignalInfo;
            // Update the image with the acquired image buffer data 
            IntPtr bufferAddress;
            GetParam(currentSurface, "SurfaceAddr", out bufferAddress);
            Marshal.Copy(bufferAddress, buffer[num], 0, bufferPitch * height);
            if (this.dBuffer.ContainsKey(num))
            {
                this.dBuffer[num] = null;
                this.dBuffer[num] = buffer[num];
            }
            else this.dBuffer.TryAdd(num, buffer[num]);
            num++;
        }
        private void AcqFailureCallback(SIGNALINFO signalInfo)
        {

        }
        #endregion


    }

    public class GenaralEuresyCamLink : BaseCamera
    {
        #region 相机自带函数
        /// <summary>
        /// Native functions imported from the MultiCam C API.
        /// </summary>
        #region Native Methods
        class NativeMethods
        {
            private NativeMethods() { }

            [DllImport("MultiCam.dll")]
            internal static extern int McOpenDriver(IntPtr instanceName);
            [DllImport("MultiCam.dll")]
            internal static extern int McCloseDriver();
            [DllImport("MultiCam.dll")]
            internal static extern int McCreate(UInt32 modelInstance, out UInt32 instance);
            [DllImport("MultiCam.dll")]
            internal static extern int McCreateNm(String modelName, out UInt32 instance);
            [DllImport("MultiCam.dll")]
            internal static extern int McDelete(UInt32 instance);
            [DllImport("MultiCam.dll")]
            internal static extern int McSetParamInt(UInt32 instance, UInt32 parameterId, int value);
            [DllImport("MultiCam.dll")]
            internal static extern int McSetParamNmInt(UInt32 instance, String parameterName, int value);
            [DllImport("MultiCam.dll")]
            internal static extern int McSetParamStr(UInt32 instance, UInt32 parameterId, String value);
            [DllImport("MultiCam.dll")]
            internal static extern int McSetParamNmStr(UInt32 instance, String parameterName, String value);
            [DllImport("MultiCam.dll")]
            internal static extern int McSetParamFloat(UInt32 instance, UInt32 parameterId, Double value);
            [DllImport("MultiCam.dll")]
            internal static extern int McSetParamNmFloat(UInt32 instance, String parameterName, Double value);
            [DllImport("MultiCam.dll")]
            internal static extern int McSetParamInst(UInt32 instance, UInt32 parameterId, UInt32 value);
            [DllImport("MultiCam.dll")]
            internal static extern int McSetParamNmInst(UInt32 instance, String parameterName, UInt32 value);
            [DllImport("MultiCam.dll")]
            internal static extern int McSetParamPtr(UInt32 instance, UInt32 parameterId, IntPtr value);
            [DllImport("MultiCam.dll")]
            internal static extern int McSetParamNmPtr(UInt32 instance, String parameterName, IntPtr value);
            [DllImport("MultiCam.dll")]
            internal static extern int McSetParamInt64(UInt32 instance, UInt32 parameterId, Int64 value);
            [DllImport("MultiCam.dll")]
            internal static extern int McSetParamNmInt64(UInt32 instance, String parameterName, Int64 value);
            [DllImport("MultiCam.dll")]
            internal static extern int McGetParamInt(UInt32 instance, UInt32 parameterId, out int value);
            [DllImport("MultiCam.dll")]
            internal static extern int McGetParamNmInt(UInt32 instance, String parameterName, out int value);
            [DllImport("MultiCam.dll")]
            internal static extern int McGetParamStr(UInt32 instance, UInt32 parameterId, IntPtr value, UInt32 maxLength);
            [DllImport("MultiCam.dll")]
            internal static extern int McGetParamNmStr(UInt32 instance, String parameterName, IntPtr value, UInt32 maxLength);
            [DllImport("MultiCam.dll")]
            internal static extern int McGetParamFloat(UInt32 instance, UInt32 parameterId, out Double value);
            [DllImport("MultiCam.dll")]
            internal static extern int McGetParamNmFloat(UInt32 instance, String parameterName, out Double value);
            [DllImport("MultiCam.dll")]
            internal static extern int McGetParamInst(UInt32 instance, UInt32 parameterId, out UInt32 value);
            [DllImport("MultiCam.dll")]
            internal static extern int McGetParamNmInst(UInt32 instance, String parameterName, out UInt32 value);
            [DllImport("MultiCam.dll")]
            internal static extern int McGetParamPtr(UInt32 instance, UInt32 parameterId, out IntPtr value);
            [DllImport("MultiCam.dll")]
            internal static extern int McGetParamPtr(UInt32 instance, UInt32 parameterId, out Microsoft.Win32.SafeHandles.SafeWaitHandle value);
            [DllImport("MultiCam.dll")]
            internal static extern int McGetParamNmPtr(UInt32 instance, String parameterName, out IntPtr value);
            [DllImport("MultiCam.dll")]
            internal static extern int McGetParamInt64(UInt32 instance, UInt32 parameterId, out Int64 value);
            [DllImport("MultiCam.dll")]
            internal static extern int McGetParamNmInt64(UInt32 instance, String parameterName, out Int64 value);
            [DllImport("MultiCam.dll")]
            internal static extern int McRegisterCallback(UInt32 instance, CALLBACK callbackFunction, UInt32 context);
            [DllImport("MultiCam.dll")]
            internal static extern int McWaitSignal(UInt32 instance, int signal, UInt32 timeout, out SIGNALINFO info);
            [DllImport("MultiCam.dll")]
            internal static extern int McGetSignalInfo(UInt32 instance, int signal, out SIGNALINFO info);
        }
        #endregion

        #region Private Constants
        private const int MAX_VALUE_LENGTH = 1024;
        #endregion

        #region Default object instance Constants
        public const UInt32 CONFIGURATION = 0x20000000;
        public const UInt32 BOARD = 0xE0000000;
        public const UInt32 CHANNEL = 0X8000FFFF;
        public const UInt32 DEFAULT_SURFACE_HANDLE = 0x4FFFFFFF;
        #endregion

        #region Specific parameter values Constants
        public const int INFINITE = -1;
        public const int INDETERMINATE = -1;
        public const int DISABLE = 0;
        #endregion

        #region Signal handling Constants
        public const UInt32 SignalEnable = (24 << 14);

        public const int SIG_ANY = 0;
        public const int SIG_SURFACE_PROCESSING = 1;
        public const int SIG_SURFACE_FILLED = 2;
        public const int SIG_UNRECOVERABLE_OVERRUN = 3;
        public const int SIG_FRAMETRIGGER_VIOLATION = 4;
        public const int SIG_START_EXPOSURE = 5;
        public const int SIG_END_EXPOSURE = 6;
        public const int SIG_ACQUISITION_FAILURE = 7;
        public const int SIG_CLUSTER_UNAVAILABLE = 8;
        public const int SIG_RELEASE = 9;
        public const int SIG_END_ACQUISITION_SEQUENCE = 10;
        public const int SIG_START_ACQUISITION_SEQUENCE = 11;
        public const int SIG_END_CHANNEL_ACTIVITY = 12;

        public const int SIG_GOLOW = (1 << 12);
        public const int SIG_GOHIGH = (2 << 12);
        public const int SIG_GOOPEN = (3 << 12);
        #endregion

        #region Signal handling Type Definitions
        public delegate void CALLBACK(ref SIGNALINFO signalInfo);


        public const UInt32 SignalHandling = (74 << 14);
        public const int SIGNALHANDLING_ANY = 1;
        public const int SIGNALHANDLING_CALLBACK_SIGNALING = 2;
        public const int SIGNALHANDLING_WAITING_SIGNALING = 3;
        public const int SIGNALHANDLING_OS_EVENT_SIGNALING = 4;

        public const UInt32 SurfaceState = (31 << 14);
        public const int SURFACESTATE_FREE = 1;
        public const int SURFACESTATE_FILLING = 2;
        public const int SURFACESTATEe_FILLED = 3;
        public const int SURFACESTATE_PROCESSING = 4;
        public const int SURFACESTATE_RESERVED = 5;

        public const UInt32 SignalEvent = (25 << 14);


        [StructLayout(LayoutKind.Sequential)]
        public struct SIGNALINFO
        {
            public IntPtr Context;
            public UInt32 Instance;
            public int Signal;
            public UInt32 SignalInfo;
            public UInt32 SignalContext;
        };
        #endregion

        #region Error handling Methods
        private static String GetErrorMessage(int errorCode)
        {
            const UInt32 ErrorDesc = (98 << 14);
            String errorDescription;
            UInt32 status = (UInt32)Math.Abs(errorCode);
            IntPtr text = Marshal.AllocHGlobal(MAX_VALUE_LENGTH + 1);
            if (NativeMethods.McGetParamStr(CONFIGURATION, ErrorDesc + status, text, MAX_VALUE_LENGTH) != 0)
                errorDescription = "Unknown error";
            else
                errorDescription = Marshal.PtrToStringAnsi(text);
            Marshal.FreeHGlobal(text);
            return errorDescription;
        }
        class MultiCamException : System.Exception
        {
            public MultiCamException(String error) : base(error) { }
        }
        private static void ThrowOnMultiCamError(int status, String action)
        {
            if (status != 0)
            {
                String error = action + ": " + GetErrorMessage(status);
                throw new MultiCamException(error);
            }
        }
        #endregion

        #region Driver connection Methods
        public static void OpenDriver()
        {
            ThrowOnMultiCamError(NativeMethods.McOpenDriver((IntPtr)null),
                "Cannot open MultiCam driver");
        }

        public static void CloseDriver()
        {
            ThrowOnMultiCamError(NativeMethods.McCloseDriver(),
                "Cannot close MultiCam driver");
        }
        #endregion

        #region Object creation/deletion Methods
        public static void Create(UInt32 modelInstance, out UInt32 instance)
        {
            ThrowOnMultiCamError(NativeMethods.McCreate(modelInstance, out instance),
                String.Format("Cannot create '{0}' instance", modelInstance));
        }

        public static void Create(String modelName, out UInt32 instance)
        {
            ThrowOnMultiCamError(NativeMethods.McCreateNm(modelName, out instance),
                String.Format("Cannot create '{0}' instance", modelName));
        }

        public static void Delete(UInt32 instance)
        {
            ThrowOnMultiCamError(NativeMethods.McDelete(instance),
                String.Format("Cannot delete '{0}' instance", instance));
        }
        #endregion

        #region Parameter 'setter' Methods
        public static void SetParam(UInt32 instance, UInt32 parameterId, int value)
        {
            ThrowOnMultiCamError(NativeMethods.McSetParamInt(instance, parameterId, value),
                String.Format("Cannot set parameter '{0}' to value '{1}'", parameterId, value));
        }

        public static void SetParam(UInt32 instance, String parameterName, int value)
        {
            ThrowOnMultiCamError(NativeMethods.McSetParamNmInt(instance, parameterName, value),
                String.Format("Cannot set parameter '{0}' to value '{1}'", parameterName, value));
        }

        public static void SetParam(UInt32 instance, UInt32 parameterId, String value)
        {
            ThrowOnMultiCamError(NativeMethods.McSetParamStr(instance, parameterId, value),
                String.Format("Cannot set parameter '{0}' to value '{1}'", parameterId, value));
        }

        public static void SetParam(UInt32 instance, String parameterName, String value)
        {
            ThrowOnMultiCamError(NativeMethods.McSetParamNmStr(instance, parameterName, value),
                String.Format("Cannot set parameter '{0}' to value '{1}'", parameterName, value));
        }

        public static void SetParam(UInt32 instance, UInt32 parameterId, Double value)
        {
            ThrowOnMultiCamError(NativeMethods.McSetParamFloat(instance, parameterId, value),
                String.Format("Cannot set parameter '{0}' to value '{1}'", parameterId, value));
        }

        public static void SetParam(UInt32 instance, String parameterName, Double value)
        {
            ThrowOnMultiCamError(NativeMethods.McSetParamNmFloat(instance, parameterName, value),
                String.Format("Cannot set parameter '{0}' to value '{1}'", parameterName, value));
        }

        public static void SetParam(UInt32 instance, UInt32 parameterId, UInt32 value)
        {
            ThrowOnMultiCamError(NativeMethods.McSetParamInst(instance, parameterId, value),
                String.Format("Cannot set parameter '{0}' to value '{1}'", parameterId, value));
        }

        public static void SetParam(UInt32 instance, String parameterName, UInt32 value)
        {
            ThrowOnMultiCamError(NativeMethods.McSetParamNmInst(instance, parameterName, value),
                String.Format("Cannot set parameter '{0}' to value '{1}'", parameterName, value));
        }

        public static void SetParam(UInt32 instance, UInt32 parameterId, IntPtr value)
        {
            ThrowOnMultiCamError(NativeMethods.McSetParamPtr(instance, parameterId, value),
                String.Format("Cannot set parameter '{0}' to value '{1}'", parameterId, value));
        }

        public static void SetParam(UInt32 instance, String parameterName, IntPtr value)
        {
            ThrowOnMultiCamError(NativeMethods.McSetParamNmPtr(instance, parameterName, value),
                String.Format("Cannot set parameter '{0}' to value '{1}'", parameterName, value));
        }

        public static void SetParam(UInt32 instance, UInt32 parameterId, Int64 value)
        {
            ThrowOnMultiCamError(NativeMethods.McSetParamInt64(instance, parameterId, value),
                String.Format("Cannot set parameter '{0}' to value '{1}'", parameterId, value));
        }

        public static void SetParam(UInt32 instance, String parameterName, Int64 value)
        {
            ThrowOnMultiCamError(NativeMethods.McSetParamNmInt64(instance, parameterName, value),
                String.Format("Cannot set parameter '{0}' to value '{1}'", parameterName, value));
        }
        #endregion

        #region Parameter 'getter' Methods
        public static void GetParam(UInt32 instance, UInt32 parameterId, out int value)
        {
            ThrowOnMultiCamError(NativeMethods.McGetParamInt(instance, parameterId, out value),
                String.Format("Cannot get parameter '{0}'", parameterId));
        }

        public static void GetParam(UInt32 instance, String parameterName, out int value)
        {
            ThrowOnMultiCamError(NativeMethods.McGetParamNmInt(instance, parameterName, out value),
                String.Format("Cannot get parameter '{0}'", parameterName));
        }

        public static void GetParam(UInt32 instance, UInt32 parameterId, out String value)
        {
            IntPtr text = Marshal.AllocHGlobal(MAX_VALUE_LENGTH + 1);
            try
            {
                ThrowOnMultiCamError(NativeMethods.McGetParamStr(instance, parameterId, text, MAX_VALUE_LENGTH),
                    String.Format("Cannot get parameter '{0}'", parameterId));
                value = Marshal.PtrToStringAnsi(text);
            }
            finally
            {
                Marshal.FreeHGlobal(text);
            }
        }

        public static void GetParam(UInt32 instance, String parameterName, out String value)
        {
            IntPtr text = Marshal.AllocHGlobal(MAX_VALUE_LENGTH + 1);
            try
            {
                ThrowOnMultiCamError(NativeMethods.McGetParamNmStr(instance, parameterName, text, MAX_VALUE_LENGTH),
                    String.Format("Cannot get parameter '{0}'", parameterName));
                value = Marshal.PtrToStringAnsi(text);
            }
            finally
            {
                Marshal.FreeHGlobal(text);
            }
        }

        public static void GetParam(UInt32 instance, UInt32 parameterId, out Double value)
        {
            ThrowOnMultiCamError(NativeMethods.McGetParamFloat(instance, parameterId, out value),
                String.Format("Cannot get parameter '{0}'", parameterId));
        }

        public static void GetParam(UInt32 instance, String parameterName, out Double value)
        {
            ThrowOnMultiCamError(NativeMethods.McGetParamNmFloat(instance, parameterName, out value),
                String.Format("Cannot get parameter '{0}'", parameterName));
        }

        public static void GetParam(UInt32 instance, UInt32 parameterId, out UInt32 value)
        {
            ThrowOnMultiCamError(NativeMethods.McGetParamInst(instance, parameterId, out value),
                String.Format("Cannot get parameter '{0}'", parameterId));
        }

        public static void GetParam(UInt32 instance, String parameterName, out UInt32 value)
        {
            ThrowOnMultiCamError(NativeMethods.McGetParamNmInst(instance, parameterName, out value),
                String.Format("Cannot get parameter '{0}'", parameterName));
        }

        public static void GetParam(UInt32 instance, UInt32 parameterId, out IntPtr value)
        {
            ThrowOnMultiCamError(NativeMethods.McGetParamPtr(instance, parameterId, out value),
                String.Format("Cannot get parameter '{0}'", parameterId));
        }


        public static void GetParam(UInt32 instance, UInt32 parameterId, out Microsoft.Win32.SafeHandles.SafeWaitHandle value)
        {
            ThrowOnMultiCamError(NativeMethods.McGetParamPtr(instance, parameterId, out value),
                String.Format("Cannot get parameter '{0}'", parameterId));
        }

        public static void GetParam(UInt32 instance, String parameterName, out IntPtr value)
        {
            ThrowOnMultiCamError(NativeMethods.McGetParamNmPtr(instance, parameterName, out value),
                String.Format("Cannot get parameter '{0}'", parameterName));
        }

        public static void GetParam(UInt32 instance, UInt32 parameterId, out Int64 value)
        {
            ThrowOnMultiCamError(NativeMethods.McGetParamInt64(instance, parameterId, out value),
                String.Format("Cannot get parameter '{0}'", parameterId));
        }

        public static void GetParam(UInt32 instance, String parameterName, out Int64 value)
        {
            ThrowOnMultiCamError(NativeMethods.McGetParamNmInt64(instance, parameterName, out value),
                String.Format("Cannot get parameter '{0}'", parameterName));
        }
        #endregion

        #region Signal handling Methods
        public static void RegisterCallback(UInt32 instance, CALLBACK callbackFunction, UInt32 context)
        {
            ThrowOnMultiCamError(NativeMethods.McRegisterCallback(instance, callbackFunction, context),
                "Cannot register callback");
        }

        public static void WaitSignal(UInt32 instance, int signal, UInt32 timeout, out SIGNALINFO info)
        {
            ThrowOnMultiCamError(NativeMethods.McWaitSignal(instance, signal, timeout, out info),
                "WaitSignal error");
        }

        public static void GetSignalInfo(UInt32 instance, int signal, out SIGNALINFO info)
        {
            ThrowOnMultiCamError(NativeMethods.McGetSignalInfo(instance, signal, out info),
                "Cannot get signal information");
        }
        #endregion

        #endregion
        #region 配置参数
        CALLBACK multiCamCallback;
        private string sn = "";
        private string camFile = "beA4000-62km_P62RG";
        private string pixelType = EuresysPixelFormat.Y8;
        private bool isSoftwareTrigger = true;
        private bool isCameraTrigger = true;
        private bool isMirrorX = false;
        private bool isMirrorY = false;
        private const int MAX_BUFFER_NUM = 100;
        System.Collections.Concurrent.ConcurrentDictionary<int, byte[]> dBuffer = new System.Collections.Concurrent.ConcurrentDictionary<int, byte[]>();
        private int num = 0;
        // The MultiCam object that controls the acquisition
        private UInt32 channel;
        int width, height, bufferPitch;
        //与相机串口通讯接口
        private System.IO.Ports.SerialPort sPort;
        private byte[][] buffer = new byte[MAX_BUFFER_NUM][];

        #endregion
        public GenaralEuresyCamLink(string serialNumber, string camFile)
        {
            this.camFile = camFile;// "C:\\Users\\1\\Desktop\\camfile\\HR25000CCL_10TAP_8Bit.cam";
        }
        public override bool InitCamera()
        {
            try
            {
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }
        public override bool OpenCamera()
        {
            try
            {
                // Open MultiCam driver
                OpenDriver();
                // Enable error logging
                SetParam(CONFIGURATION, "ErrorLog", "error.log");

                // Create a channel and associate it with the first connector on the first board

                SetParam(BOARD + 0, "BoardTopology", "MONO_DECA");

                //SetParam(BOARD + 0, "SerialControlA", sn);

                Create("CHANNEL", out channel);
                //不同板卡设置不同索引？
                SetParam(channel, "DriverIndex", 0);
                // For all Domino boards except Domino Symphony
                SetParam(channel, "Connector", "M");
                // Choose the CAM file
                SetParam(channel, "CamFile", camFile);
                // Choose the pixel color format
                SetParam(channel, "ColorMethod", "BAYER");
                SetParam(channel, "ColorRegistration", "RG");
                SetParam(channel, "ColorFormat", pixelType);

                // Choose the way the first acquisition is triggered
                SetParam(channel, "AcquisitionMode", "HFR");
                SetParam(channel, "TrigMode", "IMMEDIATE");
                //SetParam(channel, "TrigMode", "HARD");
                SetParam(channel, "NextTrigMode", "SAME");
                //SetParam(channel, "ImageFlipY", "ON");
                SetParam(channel, "TriggerSkipHold", "HOLD");
                SetParam(channel, "Expose", "WIDTH");
                // Choose the number of images to acquire
                SetParam(channel, "SeqLength_Fr", INDETERMINATE);
                SetParam(channel, "TrigCtl", "ISO");
                SetParam(channel, "TrigLine", "IIN1");
                SetParam(channel, "AcqTimeout_ms", "-1");

                GetParam(channel, "ImageSizeX", out width);
                GetParam(channel, "ImageSizeY", out height);
                GetParam(channel, "BufferPitch", out bufferPitch);
                if (isMirrorX) SetParam(channel, "ImageFlipX", "ON");
                if (isMirrorY) SetParam(channel, "ImageFlipY", "ON");
                // Register the callback function
                multiCamCallback = new CALLBACK(MultiCamCallback);
                RegisterCallback(channel, multiCamCallback, channel);

                // Enable the signals corresponding to the callback functions
                SetParam(channel, SignalEnable + SIG_SURFACE_PROCESSING, "ON");
                SetParam(channel, SignalEnable + SIG_ACQUISITION_FAILURE, "ON");
                for (int i = 0; i < buffer.GetLength(0); i++)
                {
                    buffer[i] = new byte[bufferPitch * height];
                }
                ChangeTriggerSource(this.isSoftwareTrigger);
                return true;
            }
            catch (MultiCamException)
            {
                CloseCamera();
                return false;
            }
        }
        public override bool CloseCamera()
        {
            try
            {
                ClearImageQueue();
                CloseDriver();
                return true;
            }
            catch (MultiCamException)
            {
                return false;
            }
        }
        public override bool IsSoftwareTrigger
        {
            set { this.isSoftwareTrigger = value; }
            get { return this.isSoftwareTrigger; }
        }
        public override bool IsCameraTrigger
        {
            set { this.isCameraTrigger = value; }
            get { return this.isCameraTrigger; }
        }
        public override bool IsConnected
        {
            get
            {
                return false;
            }
        }
        public override bool IsMirrorX
        {
            set { this.isMirrorX = value; }
            get { return this.isMirrorX; }
        }
        public override bool IsMirrorY
        {
            set { this.isMirrorY = value; }
            get { return this.isMirrorY; }
        }
        public override bool ChangeTriggerSource(bool isSoftwareTrigger)
        {
            SetParam(channel, "ChannelState", "IDLE");
            if (isCameraTrigger)
            {
                SetParam(channel, "TrigMode", "IMMEDIATE");
                //set camera trigger mode on
                //this.sPort.WriteLine("VB23C,1\r\n");
                //string reply = this.sPort.ReadExisting();
                //if (reply.Contains("Error"))
                //    return false;
            }
            else
            {
                //set camera trigger mode off
                //this.sPort.WriteLine("VB23C,0\r\n");
                //string reply = this.sPort.ReadExisting();
                //if (reply.Contains("Error"))
                //    return false;
                if (isSoftwareTrigger)
                    SetParam(channel, "TrigMode", "SOFT");
                else
                {
                    SetParam(channel, "TrigMode", "HARD");
                    //SetParam(channel, "TrigCtl", "ISO");
                    //SetParam(channel, "TrigLine", "IIN1");
                }
            }
            SetParam(channel, "ChannelState", "ACTIVE");
            this.isSoftwareTrigger = isSoftwareTrigger;
            return true;
        }
        public override bool SetGain(double value)
        {
            try
            {
                //if (!camera.Parameters[PLCamera.GainRaw].TrySetValue((long)value)) return false;
                //MultiCamToHalcon.SetParam(channel, "Gain_us", value);
                //    return false;
                return true;
            }
            catch (Exception)
            {
                return false;
            }


        }
        public override void GetGain(out double value)
        {
            try
            {

                value = -1;
                // camera.Parameters[PLCamera.GainRaw].GetValue();
                //MultiCamToHalcon.GetParam(channel, "Gain_us", out value);
            }
            catch (Exception)
            {
                value = -1;
            }
        }
        public override bool SetExposure(double value)
        {
            try
            {
                //string exposure = Convert.ToString((int)value, 16);
                //this.sPort.WriteLine("VB02C," + exposure + "\r\n");
                //string reply = this.sPort.ReadExisting();
                //if (reply.Contains("Error")) return false;
                SetParam(channel, "Expose_us", (int)value);

                //if (!camera.Parameters[PLCamera.ExposureTimeAbs].TrySetValue(value)) return false;
                //MultiCamToHalcon.SetParam(channel, "Expose_us", value);
                return true;
            }
            catch (Exception)
            {
                //camera.Parameters[PLCamera.ExposureTime].TrySetValue(100);
                return false;
            }
        }
        public override void GetExposure(out double value)
        {
            try
            {
                value = -1;
                //this.sPort.WriteLine("rVB02C\r\n");
                //string reply = this.sPort.ReadExisting();
                //if (reply.Contains("Error"))
                //    return;
                //int ind = reply.IndexOfAny(new char[] { '\r' });
                //value = Int32.Parse(reply.Substring(0, ind), System.Globalization.NumberStyles.HexNumber);

                int expose = -1;
                GetParam(channel, "Expose_us", out expose);
                value = expose;

                // camera.Parameters[PLCamera.ExposureTimeAbs].GetValue();
                //MultiCamToHalcon.GetParam(channel, "Expose_us", out value);
            }
            catch (Exception)
            {
                value = -1;
            }

        }
        public void SetGamma(bool value)
        {

        }
        public void GetGamma(out bool value)
        {
            try
            {
                value = false;
            }
            catch (Exception)
            {
                value = false;
            }
        }
        /// <summary>设置相机的像素格式
        /// 设置相机的像素格式
        /// </summary>
        /// <param name="value">value可以设置为如:BaslerToHalcon.PixelFormat.Mono8</param>
        public override bool EuresysSetPixelFormat(string value)
        {
            try
            {
                this.pixelType = value;
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }
        public override void EuresysGetPixelFormat(out string value)
        {
            try
            {
                value = this.pixelType;
            }
            catch (Exception)
            {
                value = "";
            }

        }
        public override bool ClearImageQueue()
        {
            try
            {
                num = 0;
                int count = this.dBuffer.Count;
                for (int i = 0; i < count; i++)
                {
                    if (this.dBuffer[i] != null)
                    {
                        this.dBuffer[i] = null;
                    }
                }
                this.dBuffer.Clear();
                //bool isNotEmpty = false;
                //for (int i = 0; i < this.buffer.GetLength(0); i++)
                //{
                //    isNotEmpty = this.buffer[i].Any(p => p != 0);
                //    if (!isNotEmpty) break;
                //    this.buffer[i] = null;
                //    this.buffer[i] = new byte[bufferPitch * height];
                //}
                GC.Collect();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public override Acquisition Snap(int imageNum = 1, int timeOut = -1)
        {
            Acquisition _acq = new Acquisition();
            // Generate a soft trigger event
            if (this.isCameraTrigger)
            {
                this.sPort.WriteLine("VB248,0<newline>");
                string reply = this.sPort.ReadExisting();
                if (reply.Contains("Error"))
                {
                    _acq.GrabStatus = "GrabFail:Camera gen soft trigger falied!";
                    return _acq;
                }
            }
            else
            {
                SetParam(channel, "ForceTrig", "TRIG");
            }
            _acq = GetFrames(imageNum, timeOut);
            return _acq;
        }
        public override Acquisition GetFrames(int imageNum = 1, int timeOut = -1)
        {
            Acquisition _acq = new Acquisition();
            try
            {
                DateTime start = DateTime.Now;
                timeOut = (timeOut == -1) ? 100000 : timeOut;
                Acquisition[] acqs = new Acquisition[imageNum];
                int threadNum = 2;
                int maxThreadNum = Math.Min(threadNum, imageNum);
                Parallel.For(0, maxThreadNum, i =>
                {
                    acqs[i] = new Acquisition();
                    ImageConvert(start, i, maxThreadNum, imageNum, ref acqs, timeOut);
                });
                double time1 = DateTime.Now.Subtract(start).TotalMilliseconds;

                for (int i = 0; i < imageNum; i++)
                {
                    _acq.GrabStatus = acqs[i].GrabStatus;
                    if (acqs[i].GrabStatus == "GrabPass")
                    {
                        HOperatorSet.ConcatObj(_acq.Image, acqs[i].Image, out _acq.Image);
                        _acq.index++;
                    }
                }
                return _acq;
            }
            catch (Exception ex)
            {
                _acq.GrabStatus = "GrabFail:" + ex.Message;
                return _acq;
            }
            finally
            {
                num = 0;
            }
        }
        public override Task<Acquisition> GetFramesAsync(int imageNum = 1, int timeOut = -1)
        {
            return Task.Run(() =>
            {
                return GetFrames(imageNum, timeOut);
            });
        }
        #region 调用的函数
        private void ImageConvert(DateTime start, int ind, ref HObject image, ref string grabStatus, int timeOut = -1)
        {
            while (DateTime.Now.Subtract(start).TotalMilliseconds < timeOut)
            {
                if (!this.dBuffer.ContainsKey(ind))
                    continue;
                if (this.dBuffer.Count == 0 || this.dBuffer[ind] == null)
                    continue;
                try
                {
                    byte[] buffer = this.dBuffer[ind];
                    unsafe
                    {
                        fixed (byte* p = buffer)
                        {
                            IntPtr ptr = (IntPtr)p;
                            image.Dispose();
                            if (this.pixelType == EuresysPixelFormat.Y8)
                            {
                                //黑白图像
                                HOperatorSet.GenImage1(out image, "byte", Convert.ToInt32(width), Convert.ToInt32(height), ptr);
                            }
                            else if (this.pixelType == EuresysPixelFormat.BAYER8)
                            {
                                HOperatorSet.GenImage1(out image, "byte", Convert.ToInt32(width), Convert.ToInt32(height), ptr);
                                //HOperatorSet.WriteImage(image, "tiff", 0, "D:\\bayer.tiff");
                                HOperatorSet.CfaToRgb(image, out image, "bayer_rg", "bilinear");
                                //HOperatorSet.WriteImage(image, "tiff", 0, "D:\\color.tiff");
                            }
                            else
                            {
                                image.Dispose();
                                grabStatus = "current pixel format not support!";
                                return;
                            }

                        }
                        grabStatus = "GrabPass";
                    }
                    return;
                }
                catch (Exception ex)
                {
                    grabStatus = "GrabFail:" + ex.Message;
                    return;
                }
                finally
                {
                    this.dBuffer[ind] = null;
                }
            }
            grabStatus = "GrabTimeOut";
            return;
        }
        private void ImageConvert(DateTime startTime, int start, int step, int imageNum, ref Acquisition[] acqs, int timeOut = -1)
        {
            for (int i = start; i < imageNum; i += step)
            {
                acqs[i] = new Acquisition();
                ImageConvert(startTime, i, ref acqs[i].Image, ref acqs[i].GrabStatus, timeOut);
            }
        }
        private void MultiCamCallback(ref SIGNALINFO signalInfo)
        {
            switch (signalInfo.Signal)
            {
                case SIG_SURFACE_PROCESSING:
                    ProcessingCallback(signalInfo);
                    break;
                case SIG_ACQUISITION_FAILURE:
                    AcqFailureCallback(signalInfo);
                    break;
                default:
                    throw new MultiCamException("Unknown signal");
            }
        }
        private void ProcessingCallback(SIGNALINFO signalInfo)
        {
            if (num >= MAX_BUFFER_NUM)
            {
                ClearImageQueue();
            }
            UInt32 currentSurface = signalInfo.SignalInfo;
            // Update the image with the acquired image buffer data 
            IntPtr bufferAddress;
            GetParam(currentSurface, "SurfaceAddr", out bufferAddress);
            Marshal.Copy(bufferAddress, buffer[num], 0, bufferPitch * height);
            if (this.dBuffer.ContainsKey(num))
            {
                this.dBuffer[num] = null;
                this.dBuffer[num] = buffer[num];
            }
            else this.dBuffer.TryAdd(num, buffer[num]);
            num++;
        }
        private void AcqFailureCallback(SIGNALINFO signalInfo)
        {

        }
        #endregion


    }

    //public class SVSEuresysCamLink : BaseCamera
    //{
    //    #region 相机自带函数
    //    /// <summary>
    //    /// Native functions imported from the MultiCam C API.
    //    /// </summary>
    //    #region Native Methods
    //    class NativeMethods
    //    {
    //        private NativeMethods() { }

    //        [DllImport("MultiCam.dll")]
    //        internal static extern int McOpenDriver(IntPtr instanceName);
    //        [DllImport("MultiCam.dll")]
    //        internal static extern int McCloseDriver();
    //        [DllImport("MultiCam.dll")]
    //        internal static extern int McCreate(UInt32 modelInstance, out UInt32 instance);
    //        [DllImport("MultiCam.dll")]
    //        internal static extern int McCreateNm(String modelName, out UInt32 instance);
    //        [DllImport("MultiCam.dll")]
    //        internal static extern int McDelete(UInt32 instance);
    //        [DllImport("MultiCam.dll")]
    //        internal static extern int McSetParamInt(UInt32 instance, UInt32 parameterId, int value);
    //        [DllImport("MultiCam.dll")]
    //        internal static extern int McSetParamNmInt(UInt32 instance, String parameterName, int value);
    //        [DllImport("MultiCam.dll")]
    //        internal static extern int McSetParamStr(UInt32 instance, UInt32 parameterId, String value);
    //        [DllImport("MultiCam.dll")]
    //        internal static extern int McSetParamNmStr(UInt32 instance, String parameterName, String value);
    //        [DllImport("MultiCam.dll")]
    //        internal static extern int McSetParamFloat(UInt32 instance, UInt32 parameterId, Double value);
    //        [DllImport("MultiCam.dll")]
    //        internal static extern int McSetParamNmFloat(UInt32 instance, String parameterName, Double value);
    //        [DllImport("MultiCam.dll")]
    //        internal static extern int McSetParamInst(UInt32 instance, UInt32 parameterId, UInt32 value);
    //        [DllImport("MultiCam.dll")]
    //        internal static extern int McSetParamNmInst(UInt32 instance, String parameterName, UInt32 value);
    //        [DllImport("MultiCam.dll")]
    //        internal static extern int McSetParamPtr(UInt32 instance, UInt32 parameterId, IntPtr value);
    //        [DllImport("MultiCam.dll")]
    //        internal static extern int McSetParamNmPtr(UInt32 instance, String parameterName, IntPtr value);
    //        [DllImport("MultiCam.dll")]
    //        internal static extern int McSetParamInt64(UInt32 instance, UInt32 parameterId, Int64 value);
    //        [DllImport("MultiCam.dll")]
    //        internal static extern int McSetParamNmInt64(UInt32 instance, String parameterName, Int64 value);
    //        [DllImport("MultiCam.dll")]
    //        internal static extern int McGetParamInt(UInt32 instance, UInt32 parameterId, out int value);
    //        [DllImport("MultiCam.dll")]
    //        internal static extern int McGetParamNmInt(UInt32 instance, String parameterName, out int value);
    //        [DllImport("MultiCam.dll")]
    //        internal static extern int McGetParamStr(UInt32 instance, UInt32 parameterId, IntPtr value, UInt32 maxLength);
    //        [DllImport("MultiCam.dll")]
    //        internal static extern int McGetParamNmStr(UInt32 instance, String parameterName, IntPtr value, UInt32 maxLength);
    //        [DllImport("MultiCam.dll")]
    //        internal static extern int McGetParamFloat(UInt32 instance, UInt32 parameterId, out Double value);
    //        [DllImport("MultiCam.dll")]
    //        internal static extern int McGetParamNmFloat(UInt32 instance, String parameterName, out Double value);
    //        [DllImport("MultiCam.dll")]
    //        internal static extern int McGetParamInst(UInt32 instance, UInt32 parameterId, out UInt32 value);
    //        [DllImport("MultiCam.dll")]
    //        internal static extern int McGetParamNmInst(UInt32 instance, String parameterName, out UInt32 value);
    //        [DllImport("MultiCam.dll")]
    //        internal static extern int McGetParamPtr(UInt32 instance, UInt32 parameterId, out IntPtr value);
    //        [DllImport("MultiCam.dll")]
    //        internal static extern int McGetParamPtr(UInt32 instance, UInt32 parameterId, out Microsoft.Win32.SafeHandles.SafeWaitHandle value);
    //        [DllImport("MultiCam.dll")]
    //        internal static extern int McGetParamNmPtr(UInt32 instance, String parameterName, out IntPtr value);
    //        [DllImport("MultiCam.dll")]
    //        internal static extern int McGetParamInt64(UInt32 instance, UInt32 parameterId, out Int64 value);
    //        [DllImport("MultiCam.dll")]
    //        internal static extern int McGetParamNmInt64(UInt32 instance, String parameterName, out Int64 value);
    //        [DllImport("MultiCam.dll")]
    //        internal static extern int McRegisterCallback(UInt32 instance, CALLBACK callbackFunction, UInt32 context);
    //        [DllImport("MultiCam.dll")]
    //        internal static extern int McWaitSignal(UInt32 instance, int signal, UInt32 timeout, out SIGNALINFO info);
    //        [DllImport("MultiCam.dll")]
    //        internal static extern int McGetSignalInfo(UInt32 instance, int signal, out SIGNALINFO info);
    //    }
    //    #endregion

    //    #region Private Constants
    //    private const int MAX_VALUE_LENGTH = 1024;
    //    #endregion

    //    #region Default object instance Constants
    //    public const UInt32 CONFIGURATION = 0x20000000;
    //    public const UInt32 BOARD = 0xE0000000;
    //    public const UInt32 CHANNEL = 0X8000FFFF;
    //    public const UInt32 DEFAULT_SURFACE_HANDLE = 0x4FFFFFFF;
    //    #endregion

    //    #region Specific parameter values Constants
    //    public const int INFINITE = -1;
    //    public const int INDETERMINATE = -1;
    //    public const int DISABLE = 0;
    //    #endregion

    //    #region Signal handling Constants
    //    public const UInt32 SignalEnable = (24 << 14);

    //    public const int SIG_ANY = 0;
    //    public const int SIG_SURFACE_PROCESSING = 1;
    //    public const int SIG_SURFACE_FILLED = 2;
    //    public const int SIG_UNRECOVERABLE_OVERRUN = 3;
    //    public const int SIG_FRAMETRIGGER_VIOLATION = 4;
    //    public const int SIG_START_EXPOSURE = 5;
    //    public const int SIG_END_EXPOSURE = 6;
    //    public const int SIG_ACQUISITION_FAILURE = 7;
    //    public const int SIG_CLUSTER_UNAVAILABLE = 8;
    //    public const int SIG_RELEASE = 9;
    //    public const int SIG_END_ACQUISITION_SEQUENCE = 10;
    //    public const int SIG_START_ACQUISITION_SEQUENCE = 11;
    //    public const int SIG_END_CHANNEL_ACTIVITY = 12;

    //    public const int SIG_GOLOW = (1 << 12);
    //    public const int SIG_GOHIGH = (2 << 12);
    //    public const int SIG_GOOPEN = (3 << 12);
    //    #endregion

    //    #region Signal handling Type Definitions
    //    public delegate void CALLBACK(ref SIGNALINFO signalInfo);


    //    public const UInt32 SignalHandling = (74 << 14);
    //    public const int SIGNALHANDLING_ANY = 1;
    //    public const int SIGNALHANDLING_CALLBACK_SIGNALING = 2;
    //    public const int SIGNALHANDLING_WAITING_SIGNALING = 3;
    //    public const int SIGNALHANDLING_OS_EVENT_SIGNALING = 4;

    //    public const UInt32 SurfaceState = (31 << 14);
    //    public const int SURFACESTATE_FREE = 1;
    //    public const int SURFACESTATE_FILLING = 2;
    //    public const int SURFACESTATEe_FILLED = 3;
    //    public const int SURFACESTATE_PROCESSING = 4;
    //    public const int SURFACESTATE_RESERVED = 5;

    //    public const UInt32 SignalEvent = (25 << 14);


    //    [StructLayout(LayoutKind.Sequential)]
    //    public struct SIGNALINFO
    //    {
    //        public IntPtr Context;
    //        public UInt32 Instance;
    //        public int Signal;
    //        public UInt32 SignalInfo;
    //        public UInt32 SignalContext;
    //    };
    //    #endregion

    //    #region Error handling Methods
    //    private static String GetErrorMessage(int errorCode)
    //    {
    //        const UInt32 ErrorDesc = (98 << 14);
    //        String errorDescription;
    //        UInt32 status = (UInt32)Math.Abs(errorCode);
    //        IntPtr text = Marshal.AllocHGlobal(MAX_VALUE_LENGTH + 1);
    //        if (NativeMethods.McGetParamStr(CONFIGURATION, ErrorDesc + status, text, MAX_VALUE_LENGTH) != 0)
    //            errorDescription = "Unknown error";
    //        else
    //            errorDescription = Marshal.PtrToStringAnsi(text);
    //        Marshal.FreeHGlobal(text);
    //        return errorDescription;
    //    }
    //    class MultiCamException : System.Exception
    //    {
    //        public MultiCamException(String error) : base(error) { }
    //    }
    //    private static void ThrowOnMultiCamError(int status, String action)
    //    {
    //        if (status != 0)
    //        {
    //            String error = action + ": " + GetErrorMessage(status);
    //            throw new MultiCamException(error);
    //        }
    //    }
    //    #endregion

    //    #region Driver connection Methods
    //    public static void OpenDriver()
    //    {
    //        ThrowOnMultiCamError(NativeMethods.McOpenDriver((IntPtr)null),
    //            "Cannot open MultiCam driver");
    //    }

    //    public static void CloseDriver()
    //    {
    //        ThrowOnMultiCamError(NativeMethods.McCloseDriver(),
    //            "Cannot close MultiCam driver");
    //    }
    //    #endregion

    //    #region Object creation/deletion Methods
    //    public static void Create(UInt32 modelInstance, out UInt32 instance)
    //    {
    //        ThrowOnMultiCamError(NativeMethods.McCreate(modelInstance, out instance),
    //            String.Format("Cannot create '{0}' instance", modelInstance));
    //    }

    //    public static void Create(String modelName, out UInt32 instance)
    //    {
    //        ThrowOnMultiCamError(NativeMethods.McCreateNm(modelName, out instance),
    //            String.Format("Cannot create '{0}' instance", modelName));
    //    }

    //    public static void Delete(UInt32 instance)
    //    {
    //        ThrowOnMultiCamError(NativeMethods.McDelete(instance),
    //            String.Format("Cannot delete '{0}' instance", instance));
    //    }
    //    #endregion

    //    #region Parameter 'setter' Methods
    //    public static void SetParam(UInt32 instance, UInt32 parameterId, int value)
    //    {
    //        ThrowOnMultiCamError(NativeMethods.McSetParamInt(instance, parameterId, value),
    //            String.Format("Cannot set parameter '{0}' to value '{1}'", parameterId, value));
    //    }

    //    public static void SetParam(UInt32 instance, String parameterName, int value)
    //    {
    //        ThrowOnMultiCamError(NativeMethods.McSetParamNmInt(instance, parameterName, value),
    //            String.Format("Cannot set parameter '{0}' to value '{1}'", parameterName, value));
    //    }

    //    public static void SetParam(UInt32 instance, UInt32 parameterId, String value)
    //    {
    //        ThrowOnMultiCamError(NativeMethods.McSetParamStr(instance, parameterId, value),
    //            String.Format("Cannot set parameter '{0}' to value '{1}'", parameterId, value));
    //    }

    //    public static void SetParam(UInt32 instance, String parameterName, String value)
    //    {
    //        ThrowOnMultiCamError(NativeMethods.McSetParamNmStr(instance, parameterName, value),
    //            String.Format("Cannot set parameter '{0}' to value '{1}'", parameterName, value));
    //    }

    //    public static void SetParam(UInt32 instance, UInt32 parameterId, Double value)
    //    {
    //        ThrowOnMultiCamError(NativeMethods.McSetParamFloat(instance, parameterId, value),
    //            String.Format("Cannot set parameter '{0}' to value '{1}'", parameterId, value));
    //    }

    //    public static void SetParam(UInt32 instance, String parameterName, Double value)
    //    {
    //        ThrowOnMultiCamError(NativeMethods.McSetParamNmFloat(instance, parameterName, value),
    //            String.Format("Cannot set parameter '{0}' to value '{1}'", parameterName, value));
    //    }

    //    public static void SetParam(UInt32 instance, UInt32 parameterId, UInt32 value)
    //    {
    //        ThrowOnMultiCamError(NativeMethods.McSetParamInst(instance, parameterId, value),
    //            String.Format("Cannot set parameter '{0}' to value '{1}'", parameterId, value));
    //    }

    //    public static void SetParam(UInt32 instance, String parameterName, UInt32 value)
    //    {
    //        ThrowOnMultiCamError(NativeMethods.McSetParamNmInst(instance, parameterName, value),
    //            String.Format("Cannot set parameter '{0}' to value '{1}'", parameterName, value));
    //    }

    //    public static void SetParam(UInt32 instance, UInt32 parameterId, IntPtr value)
    //    {
    //        ThrowOnMultiCamError(NativeMethods.McSetParamPtr(instance, parameterId, value),
    //            String.Format("Cannot set parameter '{0}' to value '{1}'", parameterId, value));
    //    }

    //    public static void SetParam(UInt32 instance, String parameterName, IntPtr value)
    //    {
    //        ThrowOnMultiCamError(NativeMethods.McSetParamNmPtr(instance, parameterName, value),
    //            String.Format("Cannot set parameter '{0}' to value '{1}'", parameterName, value));
    //    }

    //    public static void SetParam(UInt32 instance, UInt32 parameterId, Int64 value)
    //    {
    //        ThrowOnMultiCamError(NativeMethods.McSetParamInt64(instance, parameterId, value),
    //            String.Format("Cannot set parameter '{0}' to value '{1}'", parameterId, value));
    //    }

    //    public static void SetParam(UInt32 instance, String parameterName, Int64 value)
    //    {
    //        ThrowOnMultiCamError(NativeMethods.McSetParamNmInt64(instance, parameterName, value),
    //            String.Format("Cannot set parameter '{0}' to value '{1}'", parameterName, value));
    //    }
    //    #endregion

    //    #region Parameter 'getter' Methods
    //    public static void GetParam(UInt32 instance, UInt32 parameterId, out int value)
    //    {
    //        ThrowOnMultiCamError(NativeMethods.McGetParamInt(instance, parameterId, out value),
    //            String.Format("Cannot get parameter '{0}'", parameterId));
    //    }

    //    public static void GetParam(UInt32 instance, String parameterName, out int value)
    //    {
    //        ThrowOnMultiCamError(NativeMethods.McGetParamNmInt(instance, parameterName, out value),
    //            String.Format("Cannot get parameter '{0}'", parameterName));
    //    }

    //    public static void GetParam(UInt32 instance, UInt32 parameterId, out String value)
    //    {
    //        IntPtr text = Marshal.AllocHGlobal(MAX_VALUE_LENGTH + 1);
    //        try
    //        {
    //            ThrowOnMultiCamError(NativeMethods.McGetParamStr(instance, parameterId, text, MAX_VALUE_LENGTH),
    //                String.Format("Cannot get parameter '{0}'", parameterId));
    //            value = Marshal.PtrToStringAnsi(text);
    //        }
    //        finally
    //        {
    //            Marshal.FreeHGlobal(text);
    //        }
    //    }

    //    public static void GetParam(UInt32 instance, String parameterName, out String value)
    //    {
    //        IntPtr text = Marshal.AllocHGlobal(MAX_VALUE_LENGTH + 1);
    //        try
    //        {
    //            ThrowOnMultiCamError(NativeMethods.McGetParamNmStr(instance, parameterName, text, MAX_VALUE_LENGTH),
    //                String.Format("Cannot get parameter '{0}'", parameterName));
    //            value = Marshal.PtrToStringAnsi(text);
    //        }
    //        finally
    //        {
    //            Marshal.FreeHGlobal(text);
    //        }
    //    }

    //    public static void GetParam(UInt32 instance, UInt32 parameterId, out Double value)
    //    {
    //        ThrowOnMultiCamError(NativeMethods.McGetParamFloat(instance, parameterId, out value),
    //            String.Format("Cannot get parameter '{0}'", parameterId));
    //    }

    //    public static void GetParam(UInt32 instance, String parameterName, out Double value)
    //    {
    //        ThrowOnMultiCamError(NativeMethods.McGetParamNmFloat(instance, parameterName, out value),
    //            String.Format("Cannot get parameter '{0}'", parameterName));
    //    }

    //    public static void GetParam(UInt32 instance, UInt32 parameterId, out UInt32 value)
    //    {
    //        ThrowOnMultiCamError(NativeMethods.McGetParamInst(instance, parameterId, out value),
    //            String.Format("Cannot get parameter '{0}'", parameterId));
    //    }

    //    public static void GetParam(UInt32 instance, String parameterName, out UInt32 value)
    //    {
    //        ThrowOnMultiCamError(NativeMethods.McGetParamNmInst(instance, parameterName, out value),
    //            String.Format("Cannot get parameter '{0}'", parameterName));
    //    }

    //    public static void GetParam(UInt32 instance, UInt32 parameterId, out IntPtr value)
    //    {
    //        ThrowOnMultiCamError(NativeMethods.McGetParamPtr(instance, parameterId, out value),
    //            String.Format("Cannot get parameter '{0}'", parameterId));
    //    }


    //    public static void GetParam(UInt32 instance, UInt32 parameterId, out Microsoft.Win32.SafeHandles.SafeWaitHandle value)
    //    {
    //        ThrowOnMultiCamError(NativeMethods.McGetParamPtr(instance, parameterId, out value),
    //            String.Format("Cannot get parameter '{0}'", parameterId));
    //    }

    //    public static void GetParam(UInt32 instance, String parameterName, out IntPtr value)
    //    {
    //        ThrowOnMultiCamError(NativeMethods.McGetParamNmPtr(instance, parameterName, out value),
    //            String.Format("Cannot get parameter '{0}'", parameterName));
    //    }

    //    public static void GetParam(UInt32 instance, UInt32 parameterId, out Int64 value)
    //    {
    //        ThrowOnMultiCamError(NativeMethods.McGetParamInt64(instance, parameterId, out value),
    //            String.Format("Cannot get parameter '{0}'", parameterId));
    //    }

    //    public static void GetParam(UInt32 instance, String parameterName, out Int64 value)
    //    {
    //        ThrowOnMultiCamError(NativeMethods.McGetParamNmInt64(instance, parameterName, out value),
    //            String.Format("Cannot get parameter '{0}'", parameterName));
    //    }
    //    #endregion

    //    #region Signal handling Methods
    //    public static void RegisterCallback(UInt32 instance, CALLBACK callbackFunction, UInt32 context)
    //    {
    //        ThrowOnMultiCamError(NativeMethods.McRegisterCallback(instance, callbackFunction, context),
    //            "Cannot register callback");
    //    }

    //    public static void WaitSignal(UInt32 instance, int signal, UInt32 timeout, out SIGNALINFO info)
    //    {
    //        ThrowOnMultiCamError(NativeMethods.McWaitSignal(instance, signal, timeout, out info),
    //            "WaitSignal error");
    //    }

    //    public static void GetSignalInfo(UInt32 instance, int signal, out SIGNALINFO info)
    //    {
    //        ThrowOnMultiCamError(NativeMethods.McGetSignalInfo(instance, signal, out info),
    //            "Cannot get signal information");
    //    }
    //    #endregion

    //    #endregion
    //    #region 配置参数
    //    CALLBACK multiCamCallback;
    //    private string sn = "";
    //    private string camFile = "beA4000-62km_P62RG";
    //    private string pixelType = EuresysPixelFormat.Y8;
    //    private bool isSoftwareTrigger = true;
    //    private bool isCameraTrigger = true;
    //    private bool isMirrorX = false;
    //    private bool isMirrorY = false;
    //    private const int MAX_BUFFER_NUM = 200;
    //    System.Collections.Concurrent.ConcurrentDictionary<int, byte[]> dBuffer = new System.Collections.Concurrent.ConcurrentDictionary<int, byte[]>();
    //    private int num = 0;
    //    // The MultiCam object that controls the acquisition
    //    private UInt32 channel;
    //    int width, height, bufferPitch;
    //    //与相机串口通讯接口
    //    private System.IO.Ports.SerialPort sPort;
    //    private byte[][] buffer = new byte[MAX_BUFFER_NUM][];

    //    #endregion
    //    public SVSEuresysCamLink(string serialNumber, string camFile)
    //    {
    //        this.sn = serialNumber;
    //        this.camFile = camFile;// "C:\\Users\\1\\Desktop\\camfile\\HR25000CCL_10TAP_8Bit.cam";
    //    }
    //    public override bool InitCamera()
    //    {
    //        try
    //        {
    //            if (this.sPort != null && this.sPort.IsOpen)
    //            {
    //                this.sPort.Close();
    //                this.sPort.Dispose();
    //            }
    //            this.sPort = new System.IO.Ports.SerialPort(this.sn, 115200, System.IO.Ports.Parity.None, 8);
    //            return true;
    //        }
    //        catch (Exception)
    //        {
    //            return false;
    //        }

    //    }
    //    public override bool OpenCamera()
    //    {
    //        try
    //        {
    //            // Open MultiCam driver
    //            OpenDriver();
    //            // Enable error logging
    //            SetParam(CONFIGURATION, "ErrorLog", "error.log");

    //            // Create a channel and associate it with the first connector on the first board

    //            SetParam(BOARD + 0, "BoardTopology", "MONO_DECA");

    //            SetParam(BOARD + 0, "SerialControlA", sn);

    //            Create("CHANNEL", out channel);
    //            //不同板卡设置不同索引？
    //            SetParam(channel, "DriverIndex", 0);
    //            // For all Domino boards except Domino Symphony
    //            SetParam(channel, "Connector", "M");
    //            // Choose the CAM file
    //            SetParam(channel, "CamFile", camFile);
    //            // Choose the pixel color format
    //            SetParam(channel, "ColorMethod", "BAYER");
    //            SetParam(channel, "ColorRegistration", "RG");
    //            SetParam(channel, "ColorFormat", pixelType);

    //            // Choose the way the first acquisition is triggered
    //            SetParam(channel, "AcquisitionMode", "HFR");
    //            SetParam(channel, "TrigMode", "IMMEDIATE");
    //            //SetParam(channel, "TrigMode", "HARD");
    //            SetParam(channel, "NextTrigMode", "SAME");
    //            //SetParam(channel, "ImageFlipY", "ON");
    //            SetParam(channel, "TriggerSkipHold", "HOLD");
    //            SetParam(channel, "Expose", "INTCTL");
    //            // Choose the number of images to acquire
    //            SetParam(channel, "SeqLength_Fr", INDETERMINATE);
    //            SetParam(channel, "TrigCtl", "ISO");
    //            SetParam(channel, "TrigLine", "IIN1");
    //            SetParam(channel, "AcqTimeout_ms", "-1");

    //            GetParam(channel, "ImageSizeX", out width);
    //            GetParam(channel, "ImageSizeY", out height);
    //            GetParam(channel, "BufferPitch", out bufferPitch);
    //            if (isMirrorX) SetParam(channel, "ImageFlipX", "ON");
    //            if (isMirrorY) SetParam(channel, "ImageFlipY", "ON");
    //            // Register the callback function
    //            multiCamCallback = new CALLBACK(MultiCamCallback);
    //            RegisterCallback(channel, multiCamCallback, channel);

    //            // Enable the signals corresponding to the callback functions
    //            SetParam(channel, SignalEnable + SIG_SURFACE_PROCESSING, "ON");
    //            SetParam(channel, SignalEnable + SIG_ACQUISITION_FAILURE, "ON");
    //            for (int i = 0; i < buffer.GetLength(0); i++)
    //            {
    //                buffer[i] = new byte[bufferPitch * height];
    //            }
    //            // Prepare the channel in order to minimize the acquisition sequence startup latency
    //            this.sPort.Open();
    //            ChangeTriggerSource(this.isSoftwareTrigger);
    //            return true;
    //        }
    //        catch (MultiCamException)
    //        {
    //            CloseCamera();
    //            return false;
    //        }
    //    }
    //    public override bool CloseCamera()
    //    {
    //        try
    //        {
    //            ClearImageQueue();
    //            CloseDriver();
    //            if (this.sPort != null && this.sPort.IsOpen)
    //            {
    //                this.sPort.Close();
    //                this.sPort.Dispose();
    //            }
    //            return true;
    //        }
    //        catch (MultiCamException)
    //        {
    //            return false;
    //        }
    //    }
    //    public override bool IsSoftwareTrigger
    //    {
    //        set { this.isSoftwareTrigger = value; }
    //        get { return this.isSoftwareTrigger; }
    //    }
    //    public override bool IsCameraTrigger
    //    {
    //        set { this.isCameraTrigger = value; }
    //        get { return this.isCameraTrigger; }
    //    }
    //    public override bool IsConnected
    //    {
    //        get
    //        {
    //            return false;
    //        }
    //    }
    //    public override bool IsMirrorX
    //    {
    //        set { this.isMirrorX = value; }
    //        get { return this.isMirrorX; }
    //    }
    //    public override bool IsMirrorY
    //    {
    //        set { this.isMirrorY = value; }
    //        get { return this.isMirrorY; }
    //    }
    //    public override bool ChangeTriggerSource(bool isSoftwareTrigger)
    //    {
    //        SetParam(channel, "ChannelState", "IDLE");
    //        if (isCameraTrigger)
    //        {
    //            SetParam(channel, "TrigMode", "IMMEDIATE");
    //            //set camera trigger mode on
    //            this.sPort.WriteLine("VB23C,1\r\n");
    //            if (isSoftwareTrigger)
    //                this.sPort.WriteLine("VB240,0\r\n");
    //            else
    //                this.sPort.WriteLine("VB240,1\r\n");
    //            string reply = this.sPort.ReadExisting();
    //            if (reply.Contains("Error"))
    //                return false;
    //        }
    //        else
    //        {
    //            //set camera trigger mode off
    //            this.sPort.WriteLine("VB23C,0\r\n");
    //            string reply = this.sPort.ReadExisting();
    //            if (reply.Contains("Error"))
    //                return false;
    //            if (isSoftwareTrigger)
    //                SetParam(channel, "TrigMode", "SOFT");
    //            else
    //            {
    //                SetParam(channel, "TrigMode", "HARD");
    //                //SetParam(channel, "TrigCtl", "ISO");
    //                //SetParam(channel, "TrigLine", "IIN1");
    //            }
    //        }
    //        SetParam(channel, "ChannelState", "ACTIVE");
    //        this.isSoftwareTrigger = isSoftwareTrigger;
    //        return true;
    //    }
    //    public override bool SetGain(double value)
    //    {
    //        try
    //        {
    //            double unit = 28.2386;
    //            string gain = Convert.ToString((int)(value * unit), 16);
    //            this.sPort.WriteLine("VB048," + gain + "\r\n");
    //            string reply = this.sPort.ReadExisting();
    //            if (reply.Contains("Error")) return false;
    //            //if (!camera.Parameters[PLCamera.GainRaw].TrySetValue((long)value)) return false;
    //            //MultiCamToHalcon.SetParam(channel, "Gain_us", value);
    //            //    return false;
    //            return true;
    //        }
    //        catch (Exception)
    //        {
    //            return false;
    //        }

    //    }
    //    public override void GetGain(out double value)
    //    {
    //        try
    //        {
    //            double unit = 28.2386;
    //            value = -1;
    //            this.sPort.WriteLine("rVB048\r\n");
    //            string reply = this.sPort.ReadExisting();
    //            if (reply.Contains("Error"))
    //                return;
    //            int ind = reply.IndexOfAny(new char[] { '\r' });
    //            value = Int32.Parse(reply.Substring(0, ind), System.Globalization.NumberStyles.HexNumber);
    //            value = (int)(value / unit);
    //            // camera.Parameters[PLCamera.GainRaw].GetValue();
    //            //MultiCamToHalcon.GetParam(channel, "Gain_us", out value);
    //        }
    //        catch (Exception)
    //        {
    //            value = -1;
    //        }
    //    }
    //    public override bool SetExposure(double value)
    //    {
    //        try
    //        {
    //            string exposure = Convert.ToString((int)value, 16);
    //            this.sPort.WriteLine("VB02C," + exposure + "\r\n");
    //            string reply = this.sPort.ReadExisting();
    //            if (reply.Contains("Error")) return false;
    //            //if (!camera.Parameters[PLCamera.ExposureTimeAbs].TrySetValue(value)) return false;
    //            //MultiCamToHalcon.SetParam(channel, "Expose_us", value);
    //            return false;
    //        }
    //        catch (Exception)
    //        {
    //            //camera.Parameters[PLCamera.ExposureTime].TrySetValue(100);
    //            return false;
    //        }
    //    }
    //    public override void GetExposure(out double value)
    //    {
    //        try
    //        {
    //            value = -1;
    //            this.sPort.WriteLine("rVB02C\r\n");
    //            string reply = this.sPort.ReadExisting();
    //            if (reply.Contains("Error"))
    //                return;
    //            int ind = reply.IndexOfAny(new char[] { '\r' });
    //            value = Int32.Parse(reply.Substring(0, ind), System.Globalization.NumberStyles.HexNumber);
    //            // camera.Parameters[PLCamera.ExposureTimeAbs].GetValue();
    //            //MultiCamToHalcon.GetParam(channel, "Expose_us", out value);
    //        }
    //        catch (Exception)
    //        {
    //            value = -1;
    //        }

    //    }
    //    public void SetGamma(bool value)
    //    {

    //    }
    //    public void GetGamma(out bool value)
    //    {
    //        try
    //        {
    //            value = false;
    //        }
    //        catch (Exception)
    //        {
    //            value = false;
    //        }
    //    }
    //    /// <summary>设置相机的像素格式
    //    /// 设置相机的像素格式
    //    /// </summary>
    //    /// <param name="value">value可以设置为如:BaslerToHalcon.PixelFormat.Mono8</param>
    //    public override bool EuresysSetPixelFormat(string value)
    //    {
    //        try
    //        {
    //            this.pixelType = value;
    //            return true;
    //        }
    //        catch (Exception)
    //        {
    //            return false;
    //        }

    //    }
    //    public override void EuresysGetPixelFormat(out string value)
    //    {
    //        try
    //        {
    //            value = this.pixelType;
    //        }
    //        catch (Exception)
    //        {
    //            value = "";
    //        }

    //    }
    //    public override bool ClearImageQueue()
    //    {
    //        try
    //        {
    //            num = 0;
    //            int count = this.dBuffer.Count;
    //            for (int i = 0; i < count; i++)
    //            {
    //                if (this.dBuffer[i] != null)
    //                {
    //                    this.dBuffer[i] = null;
    //                }
    //            }
    //            this.dBuffer.Clear();
    //            for (int i = 0; i < this.buffer.GetLength(0); i++)
    //            {
    //                this.buffer[i] = null;
    //                this.buffer[i] = new byte[bufferPitch * height];
    //            }
    //            GC.Collect();
    //            return true;
    //        }
    //        catch (Exception)
    //        {
    //            return false;
    //        }
    //    }
    //    public override Acquisition Snap(int imageNum = 1, int timeOut = -1)
    //    {
    //        Acquisition _acq = new Acquisition();
    //        // Generate a soft trigger event
    //        if (this.isCameraTrigger)
    //        {
    //            this.sPort.WriteLine("VB248,0<newline>");
    //            string reply = this.sPort.ReadExisting();
    //            if (reply.Contains("Error"))
    //            {
    //                _acq.GrabStatus = "GrabFail:Camera gen soft trigger falied!";
    //                return _acq;
    //            }
    //        }
    //        else
    //        {
    //            SetParam(channel, "ForceTrig", "TRIG");
    //        }
    //        _acq = GetFrames(imageNum, timeOut);
    //        return _acq;
    //    }
    //    public override Acquisition GetFrames(int imageNum = 1, int timeOut = -1)
    //    {
    //        Acquisition _acq = new Acquisition();
    //        try
    //        {
    //            DateTime start = DateTime.Now;
    //            timeOut = (timeOut == -1) ? 100000 : timeOut;
    //            Acquisition[] acqs = new Acquisition[imageNum];
    //            int threadNum = 2;
    //            int maxThreadNum = Math.Min(threadNum, imageNum);
    //            Parallel.For(0, maxThreadNum, i =>
    //            {
    //                acqs[i] = new Acquisition();
    //                ImageConvert(start, i, maxThreadNum, imageNum, ref acqs, timeOut);
    //            });
    //            double time1 = DateTime.Now.Subtract(start).TotalMilliseconds;

    //            for (int i = 0; i < imageNum; i++)
    //            {
    //                _acq.GrabStatus = acqs[i].GrabStatus;
    //                if (acqs[i].GrabStatus == "GrabPass")
    //                {
    //                    HOperatorSet.ConcatObj(_acq.Image, acqs[i].Image, out _acq.Image);
    //                    _acq.index++;
    //                }
    //            }
    //            return _acq;
    //        }
    //        catch (Exception ex)
    //        {
    //            _acq.GrabStatus = "GrabFail:" + ex.Message;
    //            return _acq;
    //        }
    //        finally
    //        {
    //            num = 0;
    //        }
    //    }
    //    public override Task<Acquisition> GetFramesAsync(int imageNum = 1, int timeOut = -1)
    //    {
    //        return Task.Run(() =>
    //        {
    //            return GetFrames(imageNum, timeOut);
    //        });
    //    }
    //    #region 调用的函数
    //    private void ImageConvert(DateTime start, int ind, ref HObject image, ref string grabStatus, int timeOut = -1)
    //    {
    //        while (DateTime.Now.Subtract(start).TotalMilliseconds < timeOut)
    //        {
    //            if (!this.dBuffer.ContainsKey(ind))
    //                continue;
    //            if (this.dBuffer.Count == 0 || this.dBuffer[ind] == null)
    //                continue;
    //            try
    //            {
    //                byte[] buffer = this.dBuffer[ind];
    //                unsafe
    //                {
    //                    fixed (byte* p = buffer)
    //                    {
    //                        IntPtr ptr = (IntPtr)p;
    //                        image.Dispose();
    //                        if (this.pixelType == EuresysPixelFormat.Y8)
    //                        {
    //                            //黑白图像
    //                            HOperatorSet.GenImage1(out image, "byte", Convert.ToInt32(width), Convert.ToInt32(height), ptr);
    //                        }
    //                        else if (this.pixelType == EuresysPixelFormat.BAYER8)
    //                        {
    //                            HOperatorSet.GenImage1(out image, "byte", Convert.ToInt32(width), Convert.ToInt32(height), ptr);
    //                            //HOperatorSet.WriteImage(image, "tiff", 0, "D:\\bayer.tiff");
    //                            HOperatorSet.CfaToRgb(image, out image, "bayer_rg", "bilinear");
    //                            //HOperatorSet.WriteImage(image, "tiff", 0, "D:\\color.tiff");
    //                        }
    //                        else
    //                        {
    //                            image.Dispose();
    //                            grabStatus = "current pixel format not support!";
    //                            return;
    //                        }

    //                    }
    //                    grabStatus = "GrabPass";
    //                }
    //                return;
    //            }
    //            catch (Exception ex)
    //            {
    //                grabStatus = "GrabFail:" + ex.Message;
    //                return;
    //            }
    //            finally
    //            {
    //                this.dBuffer[ind] = null;
    //            }
    //        }
    //        grabStatus = "GrabTimeOut";
    //        return;
    //    }
    //    private void ImageConvert(DateTime startTime, int start, int step, int imageNum, ref Acquisition[] acqs, int timeOut = -1)
    //    {
    //        for (int i = start; i < imageNum; i += step)
    //        {
    //            acqs[i] = new Acquisition();
    //            ImageConvert(startTime, i, ref acqs[i].Image, ref acqs[i].GrabStatus, timeOut);
    //        }
    //    }
    //    private void MultiCamCallback(ref SIGNALINFO signalInfo)
    //    {
    //        switch (signalInfo.Signal)
    //        {
    //            case SIG_SURFACE_PROCESSING:
    //                ProcessingCallback(signalInfo);
    //                break;
    //            case SIG_ACQUISITION_FAILURE:
    //                AcqFailureCallback(signalInfo);
    //                break;
    //            default:
    //                throw new MultiCamException("Unknown signal");
    //        }
    //    }
    //    private void ProcessingCallback(SIGNALINFO signalInfo)
    //    {
    //        if (num > MAX_BUFFER_NUM)
    //        {
    //            ClearImageQueue();
    //        }
    //        UInt32 currentSurface = signalInfo.SignalInfo;
    //        // Update the image with the acquired image buffer data 
    //        IntPtr bufferAddress;
    //        GetParam(currentSurface, "SurfaceAddr", out bufferAddress);
    //        Marshal.Copy(bufferAddress, buffer[num], 0, bufferPitch * height);
    //        if (this.dBuffer.ContainsKey(num))
    //        {
    //            this.dBuffer[num] = null;
    //            this.dBuffer[num] = buffer[num];
    //        }
    //        else this.dBuffer.TryAdd(num, buffer[num]);
    //        num++;
    //    }
    //    private void AcqFailureCallback(SIGNALINFO signalInfo)
    //    {

    //    }
    //    #endregion


    //}
}
