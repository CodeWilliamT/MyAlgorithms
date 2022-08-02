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
using AutomaticAOI;
using ToolKits.SocketTool;

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
        public override void GetGain(out double gain)
        {
            object gain_obj;
            CWIMAQdx.GetAttribute(session, "CameraAttributes::AnalogControls::GainRaw", CWIMAQdx.ValueType.I64, out gain_obj);
            gain = Convert.ToDouble(gain_obj);
        }
        //获取相机曝光时间
        public override void GetExposure(out double exposure)
        {
            object exp_obj;
            CWIMAQdx.GetAttribute(session, "CameraAttributes::AcquisitionTrigger::ExposureTimeRaw", CWIMAQdx.ValueType.I64, out exp_obj);
            exposure = Convert.ToDouble(exp_obj);
        }
        //设置相机增益
        public override bool SetGain(double gain)
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
        public override bool SetExposure(double exposure)
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
        //private HObject image;
        private Camera camera = null;
        private string sn = "";
        private string pixelType = BaslerToHalcon.PixelFormat.Mono8;
        //private string triggerSource = "";
        private bool isSoftwareTrigger = true;
        private bool isMirrorX = false;
        private bool isMirrorY = false;
        private PixelDataConverter converter = new PixelDataConverter();
        //private Resource r = new Resource();
        //System.Collections.Concurrent.ConcurrentQueue<Acquisition> acqQueue = new System.Collections.Concurrent.ConcurrentQueue<Acquisition>();
        //ConcurrentQueue<Acquisition>acqQueue=new ConcurrentQueue<Acquisition>();
        static object lockObj = new object();
        #endregion

        static Version Sfnc2_0_0 = new Version(2, 0, 0);
        static PLCamera.PixelFormatEnum PixelFormat = new PLCamera.PixelFormatEnum();
        static PLCamera.SequencerTriggerSourceEnum TriggerSource = new PLCamera.SequencerTriggerSourceEnum();
        //public event EventHandler<Acquisition> GrabImageDone;
        System.Collections.Concurrent.ConcurrentQueue<IGrabResult> qBuffers = new System.Collections.Concurrent.ConcurrentQueue<IGrabResult>();
        System.Collections.Concurrent.ConcurrentQueue<Acquisition> acqQueue = new System.Collections.Concurrent.ConcurrentQueue<Acquisition>();
        //private int imgWidth = 0;
        //private int imgHeight = 0;
        public BaslerToHalcon(string serialNumber)
        {
            this.sn = serialNumber;
            //HOperatorSet.GenEmptyObj(out image);
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
                    CloseCamera();
                    Thread.Sleep(50);
                }
                //软触发使用
                if (this.isSoftwareTrigger) camera.CameraOpened += Configuration.SoftwareTrigger;
                camera.StreamGrabber.ImageGrabbed += StreamGrabber_ImageGrabbed;

                camera.Open();
                camera.Parameters[BaslerToHalcon.PixelFormat].TrySetValue(this.pixelType);
                camera.Parameters[PLCamera.ReverseX].SetValue(this.isMirrorX);
                //camera.Parameters[PLCamera.ReverseY].SetValue(this.isMirrorY);
                camera.Parameters[PLCamera.TriggerSelector].SetValue("FrameStart");
                camera.Parameters[PLCamera.TriggerMode].SetValue("On");
                //硬触发使用
                if (!this.isSoftwareTrigger) camera.Parameters[PLCamera.TriggerSource].SetValue("Line1");
                camera.StreamGrabber.Start(GrabStrategy.LatestImages, GrabLoop.ProvidedByStreamGrabber);
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
            if (!CloseCamera()) return false;
            this.isSoftwareTrigger = isSoftwareTrigger;
            if (!OpenCamera())
            {
                this.isSoftwareTrigger = !isSoftwareTrigger;
                return false;
            }

            return true;
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
                //while (this.qBuffers.Count > 0)
                //{
                //    IGrabResult buffer;
                //    this.qBuffers.TryDequeue(out buffer);
                //    buffer.Dispose();
                //    buffer = null;
                //}
                while (this.acqQueue.Count > 0)
                {
                    Acquisition acq;
                    acqQueue.TryDequeue(out acq);
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
        public Acquisition Snap1(int imageNum = 1, int timeOut = -1)
        {
            Acquisition _acq = new Acquisition();
            try
            {
                timeOut = (timeOut == -1) ? 100000 : timeOut;
                camera.ExecuteSoftwareTrigger();
                DateTime t1 = DateTime.Now;

                while (true)
                {
                    if (DateTime.Now.Subtract(t1).TotalMilliseconds > timeOut)//采集超时500ms
                    {
                        ClearImageQueue();
                        _acq.GrabStatus = "GrabFail:TimeOut";
                        return _acq;
                    }
                    if (_acq.index + 1 == imageNum)
                    {
                        return _acq;
                    }
                    if (qBuffers.Count < 1) continue;
                    IGrabResult grabResult;
                    qBuffers.TryDequeue(out grabResult);
                    HObject image = new HObject();
                    HOperatorSet.GenEmptyObj(out image);
                    try
                    {

                        if (grabResult != null)
                        {
                            byte[] buffer = grabResult.PixelData as byte[];
                            unsafe
                            {
                                fixed (byte* p = buffer)
                                {
                                    IntPtr ptr = (IntPtr)p;
                                    image.Dispose();
                                    if (this.pixelType == BaslerToHalcon.PixelFormat.Mono8)
                                    {
                                        //黑白图像
                                        HOperatorSet.GenImage1(out image, "byte", grabResult.Width, grabResult.Height, ptr);
                                    }
                                    else if (!this.pixelType.Contains("Mono"))
                                    {
                                        Bitmap bitMap = new Bitmap(grabResult.Width, grabResult.Height, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
                                        // Lock the bits of the bitmap.
                                        BitmapData bmpData = bitMap.LockBits(new Rectangle(0, 0, bitMap.Width, bitMap.Height), ImageLockMode.ReadWrite, bitMap.PixelFormat);
                                        // Place the pointer to the buffer of the bitmap.
                                        converter.OutputPixelFormat = PixelType.BGRA8packed;
                                        IntPtr ptrBmp = bmpData.Scan0;
                                        converter.Convert(ptrBmp, bmpData.Stride * bitMap.Height, grabResult); //Exception handling TODO
                                        //bitMap.Save("E:\\bitMap.png");

                                        //RGB彩色图像
                                        HOperatorSet.GenImageInterleaved(out image, ptrBmp, "bgrx", grabResult.Width, grabResult.Height, -1, "byte", 0, 0, 0, 0, -1, 0);
                                        bitMap.UnlockBits(bmpData);
                                        bitMap.Dispose();
                                    }
                                    else
                                    {
                                        image.Dispose();
                                        _acq.GrabStatus = "current pixel format not support!";
                                        return _acq;
                                    }
                                }
                            }
                            HOperatorSet.ConcatObj(_acq.Image, image, out _acq.Image);
                            _acq.GrabStatus = "GrabPass";
                            _acq.index++;
                            //HOperatorSet.WriteImage(img, "tiff", 0, "E:\\test.tiff");
                        }
                        else
                        {
                            image.Dispose();
                            _acq.GrabStatus = "GrabFail";
                            return _acq;
                        }
                    }
                    catch (Exception ex)
                    {
                        image.Dispose();
                        _acq.GrabStatus = "GrabFail:" + ex.Message;
                        return _acq;
                    }
                    finally
                    {
                        grabResult.Dispose();
                        grabResult = null;
                    }
                }
            }
            catch (Exception ex)
            {
                _acq.GrabStatus = "GrabFail:" + ex.Message;
                return _acq;
            }
        }
        public Acquisition GetFrames1(int imageNum = 1, int timeOut = -1)
        {
            Acquisition _acq = new Acquisition();
            try
            {
                timeOut = (timeOut == -1) ? 100000 : timeOut;
                DateTime t1 = DateTime.Now;

                while (true)
                {
                    if (DateTime.Now.Subtract(t1).TotalMilliseconds > timeOut)//采集超时500ms
                    {
                        ClearImageQueue();
                        _acq.GrabStatus = "GrabFail:TimeOut";
                        return _acq;
                    }
                    if (_acq.index + 1 == imageNum)
                    {
                        return _acq;
                    }
                    if (qBuffers.Count < 1) continue;
                    IGrabResult grabResult;
                    qBuffers.TryDequeue(out grabResult);
                    HObject image = new HObject();
                    HOperatorSet.GenEmptyObj(out image);
                    try
                    {
                        if (grabResult != null)
                        {
                            byte[] buffer = grabResult.PixelData as byte[];
                            unsafe
                            {
                                fixed (byte* p = buffer)
                                {
                                    IntPtr ptr = (IntPtr)p;
                                    image.Dispose();
                                    if (this.pixelType == BaslerToHalcon.PixelFormat.Mono8)
                                    {
                                        //黑白图像
                                        HOperatorSet.GenImage1(out image, "byte", grabResult.Width, grabResult.Height, ptr);
                                    }
                                    else if (!this.pixelType.Contains("Mono"))
                                    {
                                        Bitmap bitMap = new Bitmap(grabResult.Width, grabResult.Height, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
                                        // Lock the bits of the bitmap.
                                        BitmapData bmpData = bitMap.LockBits(new Rectangle(0, 0, bitMap.Width, bitMap.Height), ImageLockMode.ReadWrite, bitMap.PixelFormat);
                                        // Place the pointer to the buffer of the bitmap.
                                        converter.OutputPixelFormat = PixelType.BGRA8packed;
                                        IntPtr ptrBmp = bmpData.Scan0;
                                        converter.Convert(ptrBmp, bmpData.Stride * bitMap.Height, grabResult); //Exception handling TODO
                                        //bitMap.Save(String.Format("E:\\bitMap{0}.png", _acq.index + 1));

                                        //RGB彩色图像
                                        HOperatorSet.GenImageInterleaved(out image, ptrBmp, "bgrx", grabResult.Width, grabResult.Height, -1, "byte", 0, 0, 0, 0, -1, 0);
                                        bitMap.UnlockBits(bmpData);
                                        bitMap.Dispose();
                                    }
                                    else
                                    {
                                        image.Dispose();
                                        _acq.GrabStatus = "current pixel format not support!";
                                        return _acq;
                                    }
                                }
                            }
                            HOperatorSet.ConcatObj(_acq.Image, image, out _acq.Image);
                            _acq.GrabStatus = "GrabPass";
                            _acq.index++;
                            //HOperatorSet.WriteImage(img, "tiff", 0, "E:\\test.tiff");
                        }
                        else
                        {
                            image.Dispose();
                            _acq.GrabStatus = "GrabFail";
                            return _acq;
                        }
                    }
                    catch (Exception ex)
                    {
                        image.Dispose();
                        _acq.GrabStatus = "GrabFail:" + ex.Message;
                        return _acq;
                    }
                    finally
                    {
                        grabResult.Dispose();
                        grabResult = null;
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
                    HObject image = new HObject();
                    HOperatorSet.GenEmptyObj(out image);
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
        void StreamGrabber_ImageGrabbed1(object sender, ImageGrabbedEventArgs e)
        {
            using (e.GrabResult)
            {
                // Image grabbed successfully?
                if (e.GrabResult.GrabSucceeded)
                {
                    //imgWidth = e.GrabResult.Width;
                    //imgHeight = e.GrabResult.Height;
                    //byte[] buffer = e.GrabResult.PixelData as byte[];
                    qBuffers.Enqueue(e.GrabResult.Clone());
                    //num++;
                }
                else
                {
                    qBuffers.Enqueue(null);
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

            const Int32 k_millisecondsToSleep = 100;
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
        public override bool SetExposure(double exposureTime)
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
        public override void GetExposure(out double exposureTime)
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
        public override bool SetGain(double gain)
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
        public override void GetGain(out double gain)
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
        //System.Collections.Concurrent.ConcurrentQueue<Acquisition> acqQueue = new System.Collections.Concurrent.ConcurrentQueue<Acquisition>();
        static object lockObj = new object();
        private bool initMatrox = true;
        //matrox val 
        private MIL_ID milApplication = MIL.M_NULL;     // Application identifier.
        private MIL_ID milSystem = MIL.M_NULL;          // System identifier.
        //private MIL_ID milDisplay = MIL.M_NULL;         // Display identifier.
        private MIL_ID milDigitizer = MIL.M_NULL;           // Digitizer identifier.
        private MIL_ID milImage = MIL.M_NULL;           // Image buffer identifier.
        private MIL_INT imageSizeX = 0;
        private MIL_INT imageSizeY = 0;
        private MIL_INT sizeBit = 0;
        private MIL_INT COLOR_PATTERN = MIL.M_BAYER_GB;

        private const int FUNCTION_SUPPORTED_IMAGE_TYPE = (8 + MIL.M_UNSIGNED);
        private const int BUFFERING_SIZE_MAX = 400;
        //MIL_ID[] buffers = new MIL_ID[BUFFERING_SIZE_MAX];
        int num = 0;
        System.Collections.Concurrent.ConcurrentDictionary<int, MIL_ID> dBuf = new System.Collections.Concurrent.ConcurrentDictionary<int, MIL_ID>();
        private UserHookData userHookData = new UserHookData();
        GCHandle hUserData;
        MIL_DIG_HOOK_FUNCTION_PTR hookFunctionDelegate;

        MIL_ID[] MilGrabBufferList = new MIL_ID[BUFFERING_SIZE_MAX];
        MIL_INT MilGrabBufferListSize;
        MIL_INT n = 0;
        /// <summary>
        /// 是否为采集彩色图像模式
        /// </summary>
        private bool colorImage = false;
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

                if (!initMatrox) return true;

                //Allocate a default MIL application, system, display and image.
                MIL.MappAllocDefault(MIL.M_DEFAULT, ref milApplication, ref milSystem, MIL.M_NULL, ref milDigitizer, ref milImage);
                // If no allocation errors.
                if (MIL.MappGetError(MIL.M_DEFAULT, MIL.M_GLOBAL, MIL.M_NULL) != 0)
                {
                    return false;
                }
                MIL_INT sizeBand = this.colorImage ? 3 : 1;
                imageSizeX = MIL.MdigInquire(milDigitizer, MIL.M_SIZE_X, MIL.M_NULL);
                imageSizeY = MIL.MdigInquire(milDigitizer, MIL.M_SIZE_Y, MIL.M_NULL);
                sizeBit = MIL.MdigInquire(milDigitizer, MIL.M_SIZE_BIT, MIL.M_NULL);
                //MIL.MbufAllocColor(milSystem, SizeBand, SizeX, SizeY, SizeBit, MIL.M_IMAGE + MIL.M_GRAB + MIL.M_DISP, ref milImage);
                // Allocate the grab buffers and clear them.
                MIL.MappControl(MIL.M_DEFAULT, MIL.M_ERROR, MIL.M_PRINT_DISABLE);
                for (MilGrabBufferListSize = 0; MilGrabBufferListSize < BUFFERING_SIZE_MAX; MilGrabBufferListSize++)
                {
                    MIL.MbufAllocColor(milSystem, sizeBand, imageSizeX, imageSizeY, sizeBit, MIL.M_IMAGE + MIL.M_GRAB + MIL.M_DISP,
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
                //MIL.MdigGrabContinuous(milDigitizer, milImage);
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
                switch (value)
                {
                    case "BayerGB8":
                        this.COLOR_PATTERN = MIL.M_BAYER_GB;
                        break;
                    default:
                        break;
                }
                this.colorImage = !value.Contains("Mono");
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
                num = 0;
                int count = this.dBuf.Count;
                for (int i = 0; i < count; i++)
                {
                    MIL.MbufClear(this.dBuf[i], 0);
                }
                this.dBuf.Clear();
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
            ICommandParameterExtensions.TryExecute(camera.Parameters[PLCamera.TriggerSoftware]);
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
                int threadNum = 10;
                int maxThreadNum = Math.Min(threadNum, imageNum);
                Parallel.For(0, maxThreadNum, i =>
                {
                    acqs[i] = new Acquisition();
                    ImageConvert(start, i, maxThreadNum, imageNum, ref acqs, timeOut);
                });
                //ParallelOptions pOption = new ParallelOptions();
                //pOption.MaxDegreeOfParallelism = 10;
                //Parallel.For(0, imageNum, (i) =>
                //    {
                //        acqs[i] = new Acquisition();
                //        ImageConvert(start, i, ref acqs[i].Image, ref acqs[i].GrabStatus, timeOut);
                //    });
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
                double time2 = DateTime.Now.Subtract(start).TotalMilliseconds - time1;

                File.AppendAllText("E:\\GrabLog.csv", time1.ToString() + "," + time2.ToString() + "\r\n");

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
        private void ImageConvert(DateTime start, int ind, ref HObject image, ref string grabStatus, int timeOut = -1)
        {
            while (DateTime.Now.Subtract(start).TotalMilliseconds < timeOut)
            {
                if (!this.dBuf.ContainsKey(ind))
                    continue;
                if (this.dBuf[ind] == MIL.M_NULL)
                    continue;
                try
                {
                    MIL_INT imageDataPtr = MIL.M_NULL;
                    MIL_INT SrcImageType = 0;
                    MIL_INT SrcImagePitchByte = 0;
                    // Lock buffers for direct access.
                    MIL.MbufControl(this.dBuf[ind], MIL.M_LOCK, MIL.M_DEFAULT);
                    // get image information.
                    MIL.MbufInquire(this.dBuf[ind], MIL.M_HOST_ADDRESS, ref imageDataPtr);
                    //MIL.MbufInquire(this.dBuf[ind], MIL.M_SIZE_X, ref imageSizeX);
                    //MIL.MbufInquire(this.dBuf[ind], MIL.M_SIZE_Y, ref imageSizeY);
                    MIL.MbufInquire(this.dBuf[ind], MIL.M_TYPE, ref SrcImageType);
                    MIL.MbufInquire(this.dBuf[ind], MIL.M_PITCH_BYTE, ref SrcImagePitchByte);

                    if (imageDataPtr != MIL.M_NULL && SrcImageType == FUNCTION_SUPPORTED_IMAGE_TYPE)
                    {
                        image.Dispose();
                        unsafe
                        {
                            //lock (this)
                            {
                                IntPtr ptr = imageDataPtr;
                                if (this.pixelType == BaslerMtxCamLinkToHalcon.PixelFormat.Mono8)
                                {
                                    //黑白图像
                                    HOperatorSet.GenImage1Rect(out image, ptr, Convert.ToInt32(imageSizeX), Convert.ToInt32(imageSizeY), Convert.ToInt32(SrcImagePitchByte), 8, 8, "true", 0);
                                }
                                else if (!this.pixelType.Contains("Mono"))
                                {
                                    //RGB彩色图像
                                    HOperatorSet.GenImageInterleaved(out image, ptr, "bgrx", Convert.ToInt32(imageSizeX), Convert.ToInt32(imageSizeY), -1, "byte", 0, 0, 0, 0, -1, 0);
                                }
                                else
                                {
                                    grabStatus = "current pixel format not support!";
                                    return;
                                }
                            }
                        }
                        grabStatus = "GrabPass";
                    }
                    else
                    {
                        grabStatus = "GrabFail";
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
                    // Unlock buffers.
                    MIL.MbufControl(this.dBuf[ind], MIL.M_UNLOCK, MIL.M_DEFAULT);
                    // Clear the buffer.
                    MIL.MbufClear(this.dBuf[ind], 0);
                    this.dBuf[ind] = MIL.M_NULL;
                    MIL_ID buf;
                    this.dBuf.TryRemove(ind, out buf);
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
                if (this.colorImage)
                    MIL.MbufBayer(ModifiedBufferId, ModifiedBufferId, MIL.M_DEFAULT, this.COLOR_PATTERN);
                if (this.dBuf.ContainsKey(num))
                {
                    MIL.MbufClear(this.dBuf[num], 0);
                    this.dBuf[num] = ModifiedBufferId;
                }
                else this.dBuf.TryAdd(num, ModifiedBufferId);
                num++;
                return 0;
            }
            else
            {
                return -1;
            }

        }

    }
    public class BaslerVirtualToHalcon : BaseCamera
    {
        MainForm mf = MainForm.GetMainForm();

        public BaslerVirtualToHalcon(string serialNumber)
        {
            if (!HTCamLineInterface.CreateCamInstance(mf.lTcp[Structs.TcpType.IMAGE_TCP], serialNumber))
            {
                mf.ShowErrMsg("发送“创建切割线相机实例”指令失败！");
                return;
            }
            ComData _comData;
            if (!mf.lTcp[Structs.TcpType.IMAGE_TCP].ReceiveMessage
                (
                Structs.CmdType.CREATE_CAM_INSTANCE,
                1000,
                out _comData
                ))
            {
                mf.ShowErrMsg("接收“创建切割线相机实例”回复指令失败！");
                return;
            }
            if (_comData.result != 1)
                mf.ShowErrMsg("创建切割线相机实例失败！");
        }
        public override bool InitCamera()
        {
            try
            {
                //if (camera == null)
                //{
                //    camera = new Camera(sn);
                //}
                if (!HTCamLineInterface.InitCamera(mf.lTcp[Structs.TcpType.IMAGE_TCP]))
                    return false;
                ComData _comData;
                if (!mf.lTcp[Structs.TcpType.IMAGE_TCP].ReceiveMessage
                (
                Structs.CmdType.INIT_CAMERA,
                10000,
                out _comData
                )) return false;
                if (_comData.result != 1) return false;

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
                //if (camera.IsOpen)
                //{
                //    CloseCamera();
                //    Thread.Sleep(50);
                //}
                ////软触发使用
                //if (this.isSoftwareTrigger) camera.CameraOpened += Configuration.SoftwareTrigger;
                //camera.StreamGrabber.ImageGrabbed += StreamGrabber_ImageGrabbed;

                //ICamera iCamera = camera.Open();
                //camera.Parameters[BaslerVirtualToHalcon.PixelFormat].TrySetValue(this.pixelType);
                //camera.Parameters[PLCamera.ReverseX].SetValue(this.isMirrorX);
                ////camera.Parameters[PLCamera.ReverseY].SetValue(this.isMirrorY);
                ////camera.Parameters[PLCamera.TriggerSelector].SetValue("FrameStart");
                //camera.Parameters[PLCamera.TriggerMode].SetValue("On");
                ////硬触发使用
                //if (!this.isSoftwareTrigger) camera.Parameters[PLCamera.TriggerSource].SetValue("Line1");
                //camera.StreamGrabber.Start(GrabStrategy.LatestImages, GrabLoop.ProvidedByStreamGrabber);

                if (!HTCamLineInterface.OpenCamera(mf.lTcp[Structs.TcpType.IMAGE_TCP]))
                    return false;
                ComData _comData;
                if (!mf.lTcp[Structs.TcpType.IMAGE_TCP].ReceiveMessage
                (
                Structs.CmdType.OPEN_CAMERA,
                10000,
                out _comData
                )) return false;
                if (_comData.result != 1) return false;

                return true;
            }
            catch (Exception)
            {
                //CloseCamera();
                return false;
            }
        }
        public override bool CloseCamera()
        {
            try
            {
                //if (camera == null) return true;
                //if (camera.IsOpen)
                //{
                //    if (camera.StreamGrabber.IsGrabbing) camera.StreamGrabber.Stop();
                //    camera.CameraOpened -= Configuration.SoftwareTrigger;
                //    camera.StreamGrabber.ImageGrabbed -= StreamGrabber_ImageGrabbed;
                //    camera.Close();
                //    if (acqQueue != null) acqQueue.Clear();
                //}

                if (!HTCamLineInterface.CloseCamera(mf.lTcp[Structs.TcpType.IMAGE_TCP]))
                    return false;
                ComData _comData;
                if (!mf.lTcp[Structs.TcpType.IMAGE_TCP].ReceiveMessage
                (
                Structs.CmdType.CLOSE_CAMERA,
                10000,
                out _comData
                ))
                    return false;
                if (_comData.result != 1) return false;

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public override bool IsSoftwareTrigger
        {
            set
            {
                //this.isSoftwareTrigger = value;
                HTCamLineInterface.IsSoftWareTrigger(mf.lTcp[Structs.TcpType.IMAGE_TCP], value);
                ComData _comData;
                if (!mf.lTcp[Structs.TcpType.IMAGE_TCP].ReceiveMessage
                (
                Structs.CmdType.IS_SW_TRIGGER,
                300,
                out _comData
                ))
                {
                    mf.ShowErrMsg("主程序接收“设置切割线相机触发方式”回复指令失败！");
                    return;
                }
                if (_comData.result != 1)
                {
                    mf.ShowErrMsg("切割线程序处理“设置切割线相机触发方式”的指令失败！");
                    return;
                }
            }
            get
            {
                HTCamLineInterface.GetCamTriggerMode(mf.lTcp[Structs.TcpType.IMAGE_TCP]);
                ComData _comData;
                if (!mf.lTcp[Structs.TcpType.IMAGE_TCP].ReceiveMessage
                (
                Structs.CmdType.GET_CAM_TRIGGER_MODE,
                300,
                out _comData
                ))
                {
                    mf.ShowErrMsg("主程序接收“获取切割线相机触发方式”回复指令失败！");
                    return false;
                }
                if (_comData.result != 1)
                {
                    mf.ShowErrMsg("切割线程序处理“获取切割线相机触发方式”的指令失败！");
                    return false;
                }
                return _comData.iData[0] == 1 ? true : false;
            }
        }
        public override bool IsConnected
        {
            get
            {
                return true;// this.camera.IsConnected;
            }
        }
        public override bool IsMirrorX
        {
            set
            {
                //this.isMirrorX = value; 
            }
            get
            {
                return true;//return this.isMirrorX;
            }
        }
        public override bool IsMirrorY
        {
            set
            {
                //this.isMirrorY = value;
            }
            get
            {
                return true;//return this.isMirrorY;
            }
        }
        public override bool ChangeTriggerSource(bool isSoftwareTrigger)
        {
            //if (!CloseCamera()) return false;
            //this.isSoftwareTrigger = isSoftwareTrigger;
            //return OpenCamera();
            if (!HTCamLineInterface.ChangeTriggerSource(mf.lTcp[Structs.TcpType.IMAGE_TCP], isSoftwareTrigger))
                return false;
            ComData _comData;
            if (!mf.lTcp[Structs.TcpType.IMAGE_TCP].ReceiveMessage
            (
                Structs.CmdType.CHANGE_TRIGGER_SOURCE,
                5000,
                out _comData
            )) return false;
            if (_comData.result != 1) return false;

            return true;

        }
        public override bool SetGain(double value)
        {
            try
            {
                if (!HTCamLineInterface.SetGain(mf.lTcp[Structs.TcpType.IMAGE_TCP], value))
                    return false;
                ComData _comData;
                if (!mf.lTcp[Structs.TcpType.IMAGE_TCP].ReceiveMessage
                    (
                    Structs.CmdType.SET_GAIN,
                    300,
                    out _comData
                    )) return false;
                if (_comData.result != 1) return false;

                return true;
            }
            catch (Exception)
            {
                //camera.Parameters[PLCamera.GainAuto].TrySetValue(PLCamera.GainAuto.Continuous);
                return false;
            }

        }
        public override void GetGain(out double value)
        {
            try
            {
                value = -1;
                //if (camera.GetSfncVersion() < Sfnc2_0_0)
                //{
                //    // In previous SFNC versions, GainRaw is an integer parameter.
                //    value = camera.Parameters[PLCamera.GainRaw].GetValuePercentOfRange();
                //}
                //else // For SFNC 2.0 cameras, e.g. USB3 Vision cameras
                //{
                //    // In SFNC 2.0, Gain is a float parameter.
                //    value = camera.Parameters[PLUsbCamera.Gain].GetValuePercentOfRange();
                //    // For USB cameras, Gamma is always enabled.
                //}
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
                if (!HTCamLineInterface.SetExposure(mf.lTcp[Structs.TcpType.IMAGE_TCP], value))
                    return false;
                ComData _comData;
                if (!mf.lTcp[Structs.TcpType.IMAGE_TCP].ReceiveMessage
                    (
                    Structs.CmdType.SET_EXPOSURE,
                    300,
                    out _comData
                    )) return false;
                if (_comData.result != 1) return false;

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
                //if (camera.GetSfncVersion() < Sfnc2_0_0)
                //{
                //    value = camera.Parameters[PLCamera.ExposureTimeRaw].GetValue();
                //}
                //else
                //{
                //    value = camera.Parameters[PLCamera.ExposureTime].GetValue();
                //}
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
                //camera.Parameters[PLCamera.GammaEnable].TrySetValue(value);    //if available
            }
            catch (Exception)
            {
                //camera.Parameters[PLCamera.GammaEnable].TrySetValue(true);
            }
        }
        public void GetGamma(out bool value)
        {
            try
            {
                value = false;
                //value = camera.Parameters[PLCamera.GammaEnable].GetValue();
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

                if (!HTCamLineInterface.SetPixelFormat(mf.lTcp[Structs.TcpType.IMAGE_TCP], value))
                    return false;
                ComData _comData;
                if (!mf.lTcp[Structs.TcpType.IMAGE_TCP].ReceiveMessage
                (
                    Structs.CmdType.SET_PIXEL_FORMAT,
                    300,
                    out _comData
                )) return false;
                if (_comData.result != 1) return false;

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
                //value = camera.Parameters[BaslerToHalcon.PixelFormat].GetValue();
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
                if (!HTCamLineInterface.ClearImageQueue(mf.lTcp[Structs.TcpType.IMAGE_TCP]))
                    return false;
                ComData _comData;
                if (!mf.lTcp[Structs.TcpType.IMAGE_TCP].ReceiveMessage
                (
                    Structs.CmdType.CLEAR_IMAGE_QUEUE,
                    1000,
                    out _comData
                )) return false;
                if (_comData.result != 1) return false;


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
            //删除存在的图像
            string[] imgFiles = Directory.GetFiles(Path.GetDirectoryName(mf.lineSnapImgPath));
            for (int i = 0; i < imgFiles.Length; i++)
            {
                if (System.Text.RegularExpressions.Regex.IsMatch(Path.GetFileNameWithoutExtension(imgFiles[i]), @"^([0-9]*_[0-9]*_[0-9]*_[0-9]*)$"))
                    File.Delete(imgFiles[i]);
            }

            Acquisition _acq = new Acquisition();
            if (!HTCamLineInterface.Snap(mf.lTcp[Structs.TcpType.IMAGE_TCP], imageNum, mf.lineSnapImgPath, timeOut))
            {
                _acq.GrabStatus = "GrabFail:Send snap cmd time out!";
                return _acq;
            }
            ComData _comData;
            if (!mf.lTcp[Structs.TcpType.IMAGE_TCP].ReceiveMessage
            (
            Structs.CmdType.SNAP,
            1000,
            out _comData
            ))
            {
                _acq.GrabStatus = "GrabFail:Revceive snap cmd reply time out";
                return _acq;
            }
            if (_comData.result != 1)
            {
                _acq.GrabStatus = ToolKits.FunctionModule.Common.CharToString(_comData.cData);
                return _acq;
            }
            //send ok,snap ok,recevie ok,
            //load img from path;
            try
            {
                _acq.Image.Dispose();
                Thread.Sleep(100);
                //ToolKits.FunctionModule.Vision.read_image_fast(out _acq.Image, "X:\\0_0_0_0.join");//  mf.lineSnapImgPath + "\\0_0_0_0.tiff");
                HOperatorSet.ReadImage(out _acq.Image, mf.lineSnapImgPath);
                _acq.GrabStatus = "GrabPass";
                _acq.index = _comData.iData[0];

                if (File.Exists(mf.lineSnapImgPath))
                    File.Delete(mf.lineSnapImgPath);

                return _acq;
            }
            catch (Exception ex)
            {
                if (File.Exists(mf.lineSnapImgPath))
                    File.Delete(mf.lineSnapImgPath);
                _acq.GrabStatus = "GrabFail:" + ex.Message;
                return _acq;
            }


        }
        public override Acquisition GetFrames(int imageNum = 1, int timeOut = -1)
        {
            //删除存在的图像
            string[] imgFiles = Directory.GetFiles(Path.GetDirectoryName(mf.lineSnapImgPath));
            for (int i = 0; i < imgFiles.Length; i++)
            {
                if (System.Text.RegularExpressions.Regex.IsMatch(Path.GetFileNameWithoutExtension(imgFiles[i]), @"^([0-9]*_[0-9]*_[0-9]*_[0-9]*)$"))
                    File.Delete(imgFiles[i]);
            }

            Acquisition _acq = new Acquisition();
            if (!HTCamLineInterface.GetFrames(mf.lTcp[Structs.TcpType.IMAGE_TCP], imageNum, mf.lineSnapImgPath, timeOut))
            {
                _acq.GrabStatus = "GrabFail:Send snap cmd time out!";
                return _acq;
            }
            ComData _comData;
            if (!mf.lTcp[Structs.TcpType.IMAGE_TCP].ReceiveMessage
            (
            Structs.CmdType.GET_FRAMES,
            10000,
            out _comData
            ))
            {
                _acq.GrabStatus = "GrabFail:Revceive snap cmd reply time out";
                return _acq;
            }
            if (_comData.result != 1)
            {
                _acq.GrabStatus = ToolKits.FunctionModule.Common.CharToString(_comData.cData);// "GrabFail:Snap Fail!";
                return _acq;
            }
            //send ok,snap ok,recevie ok,
            //load img from path;
            try
            {
                _acq.Image.Dispose();
                Thread.Sleep(100);
                //ToolKits.FunctionModule.Vision.read_image_fast(out _acq.Image,"X:\\0_0_0_0.join");// mf.lineSnapImgPath + "\\0_0_0_0.tiff");
                HOperatorSet.ReadImage(out _acq.Image, mf.lineSnapImgPath);
                _acq.GrabStatus = "GrabPass";
                _acq.index = _comData.iData[0];

                //if (File.Exists(mf.lineSnapImgPath))
                //    File.Delete(mf.lineSnapImgPath);

                return _acq;
            }
            catch (Exception ex)
            {
                //if (File.Exists(mf.lineSnapImgPath))
                //    File.Delete(mf.lineSnapImgPath);

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
    }
    public class BaslerEuresysCamLinkToHalcon : BaseCamera
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
            ThrowOnMultiCamError(NativeMethods.McGetParamInst(instance, parameterId, out  value),
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
        private string pixelType = EuresysPixelFormat.BAYER8;
        private bool isSoftwareTrigger = true;
        private bool isMirrorX = false;
        private bool isMirrorY = false;
        System.Collections.Concurrent.ConcurrentQueue<Acquisition> acqQueue = new System.Collections.Concurrent.ConcurrentQueue<Acquisition>();
        private const int MAX_BUFFER_NUM = 400;
        SIGNALINFO[] buffers = new SIGNALINFO[MAX_BUFFER_NUM];
        private int num = 0;
        // The MultiCam object that controls the acquisition
        private UInt32 channel;
        static Version Sfnc2_0_0 = new Version(2, 0, 0);
        #endregion
        public BaslerEuresysCamLinkToHalcon(string serialNumber)
        {
            this.sn = serialNumber;
        }
        public override bool InitCamera()
        {
            try
            {
                for (int i = 0; i < MAX_BUFFER_NUM; i++)
                {
                    SIGNALINFO signalInfo = new SIGNALINFO();
                    signalInfo.Signal = -1;
                    buffers[i] = signalInfo;
                }
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
                if (camera.IsOpen)
                {
                    CloseCamera();
                    Thread.Sleep(50);
                }
                camera.Open();
                // Open MultiCam driver
                OpenDriver();

                // Enable error logging
                SetParam(CONFIGURATION, "ErrorLog", "error.log");

                // Create a channel and associate it with the first connector on the first board

                //SetParam(BOARD + 0, "BoardTopology", "MONO_DECA");

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
                SetParam(channel, "TrigMode", "SOFT");
                // Choose the number of images to acquire
                //MultiCamToHalcon.SetParam(channel, "SeqLength_Fr", MultiCamToHalcon.INDETERMINATE);

                // Register the callback function
                multiCamCallback = new CALLBACK(MultiCamCallback);
                RegisterCallback(channel, multiCamCallback, channel);

                // Enable the signals corresponding to the callback functions
                SetParam(channel, SignalEnable + SIG_SURFACE_PROCESSING, "ON");
                SetParam(channel, SignalEnable + SIG_ACQUISITION_FAILURE, "ON");

                // Prepare the channel in order to minimize the acquisition sequence startup latency
                SetParam(channel, "ChannelState", "READY");
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
            if (!CloseCamera()) return false;
            this.isSoftwareTrigger = isSoftwareTrigger;
            if (!OpenCamera())
            {
                this.isSoftwareTrigger = !isSoftwareTrigger;
                return false;
            }

            return true;
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
                while (this.acqQueue.Count > 0)
                {
                    Acquisition acq;
                    acqQueue.TryDequeue(out acq);
                    acq.Dispose();
                }
                num = 0;
                for (int i = 0; i < MAX_BUFFER_NUM; i++)
                {
                    if (buffers[i].Signal != -1)
                    {
                        buffers[i] = new SIGNALINFO();
                        buffers[i].Signal = -1;
                    }
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
                String channelState;
                GetParam(channel, "ChannelState", out channelState);
                if (channelState != "ACTIVE")
                    SetParam(channel, "ChannelState", "ACTIVE");
                // Generate a soft trigger event
                SetParam(channel, "ForceTrig", "TRIG");
                DateTime t1 = DateTime.Now;

                for (int i = 0; i < buffers.Length; i++)
                {
                    if (DateTime.Now.Subtract(t1).TotalMilliseconds > timeOut)//采集超时500ms
                    {
                        for (int j = i; j < buffers.Length; j++)
                        {
                            if (buffers[j].Signal == -1) break;
                            buffers[j] = new SIGNALINFO();
                            buffers[j].Signal = -1;
                        }
                        _acq.GrabStatus = "GrabFail:TimeOut";
                        return _acq;
                    }
                    if (_acq.index + 1 == imageNum)
                    {
                        return _acq;
                    }
                    if (buffers[i].Signal == -1)
                    {
                        i--;
                        continue;
                    }
                    try
                    {
                        if (buffers[i].Signal != -1)
                        {
                            UInt32 currentChannel = (UInt32)buffers[i].Context;
                            //statusBar.Text = "Processing";
                            UInt32 currentSurface = buffers[i].SignalInfo;
                            // Update the image with the acquired image buffer data 
                            Int32 width, height, bufferPitch;
                            IntPtr bufferAddress;
                            GetParam(currentChannel, "ImageSizeX", out width);
                            GetParam(currentChannel, "ImageSizeY", out height);
                            GetParam(currentChannel, "BufferPitch", out bufferPitch);
                            GetParam(currentSurface, "SurfaceAddr", out bufferAddress);

                            HObject img = new HObject();
                            img.Dispose();
                            unsafe
                            {
                                //lock (this)
                                {
                                    if (this.pixelType == EuresysPixelFormat.BAYER8)
                                    {
                                        //黑白图像
                                        //HOperatorSet.GenImage1(out image, "byte", Convert.ToInt32(imageSizeX), Convert.ToInt32(imageSizeY), ptr);
                                        //HOperatorSet.GenImage1Rect(out img, ptr, Convert.ToInt32(imageSizeX), Convert.ToInt32(imageSizeY), Convert.ToInt32(SrcImagePitchByte), 8, 8, "true", 0);

                                    }
                                    else if (!this.pixelType.Contains("Mono"))
                                    {
                                        //RGB彩色图像
                                        HOperatorSet.GenImageInterleaved(out img, bufferAddress, "rgb", width + 2, height, -1, "byte", 0, 0, 0, 0, -1, 0);
                                    }
                                    else
                                    {
                                        img.Dispose();
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
                        buffers[i] = new SIGNALINFO();
                        buffers[i].Signal = -1;
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
                            if (buffers[j].Signal == -1) break;
                            buffers[j] = new SIGNALINFO();
                            buffers[j].Signal = -1;
                        }
                        _acq.GrabStatus = "GrabFail:TimeOut";
                        return _acq;
                    }
                    if (_acq.index + 1 == imageNum)
                    {
                        return _acq;
                    }
                    if (buffers[i].Signal == -1)
                    {
                        i--;
                        continue;
                    }
                    try
                    {
                        if (buffers[i].Signal != -1)
                        {
                            UInt32 currentChannel = (UInt32)buffers[i].Context;
                            //statusBar.Text = "Processing";
                            UInt32 currentSurface = buffers[i].SignalInfo;
                            // Update the image with the acquired image buffer data 
                            Int32 width, height, bufferPitch;
                            IntPtr bufferAddress;
                            GetParam(currentChannel, "ImageSizeX", out width);
                            GetParam(currentChannel, "ImageSizeY", out height);
                            GetParam(currentChannel, "BufferPitch", out bufferPitch);
                            GetParam(currentSurface, "SurfaceAddr", out bufferAddress);

                            HObject img = new HObject();
                            img.Dispose();
                            unsafe
                            {
                                //lock (this)
                                {
                                    if (this.pixelType == EuresysPixelFormat.BAYER8)
                                    {
                                        //黑白图像
                                        //HOperatorSet.GenImage1(out image, "byte", Convert.ToInt32(imageSizeX), Convert.ToInt32(imageSizeY), ptr);
                                        //HOperatorSet.GenImage1Rect(out img, ptr, Convert.ToInt32(imageSizeX), Convert.ToInt32(imageSizeY), Convert.ToInt32(SrcImagePitchByte), 8, 8, "true", 0);

                                    }
                                    else if (!this.pixelType.Contains("Mono"))
                                    {
                                        //RGB彩色图像
                                        HOperatorSet.GenImageInterleaved(out img, bufferAddress, "rgb", width + 2, height, -1, "byte", 0, 0, 0, 0, -1, 0);
                                    }
                                    else
                                    {
                                        img.Dispose();
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
                        buffers[i] = new SIGNALINFO();
                        buffers[i].Signal = -1;
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
            buffers[num] = signalInfo;
            num++;
        }
        private void AcqFailureCallback(SIGNALINFO signalInfo)
        {
            signalInfo.Signal = -1;

            buffers[num] = signalInfo;
            num++;
        }
        #endregion

    }

}
