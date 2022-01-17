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

namespace HTCamera
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
        public override Acqusition Snap(int imageNum, int timeOut)
        {
            CWIMAQdx.SetAttribute(session, "CameraAttributes::AcquisitionTrigger::TriggerSoftware", CWIMAQdx.ValueType.String, "Execute");
            Acqusition acq = new Acqusition();
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
        public override Acqusition Hard_Snap()
        {
            Acqusition acq = new Acqusition();
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
        private PixelDataConverter converter = new PixelDataConverter();
        //private Resource r = new Resource();
        Queue<Acqusition> acqQueue = new Queue<Acqusition>();
        //ConcurrentQueue<Acqusition>acqQueue=new ConcurrentQueue<Acqusition>();
        static object lockObj = new object();
        #endregion

        static Version Sfnc2_0_0 = new Version(2, 0, 0);
        static PLCamera.PixelFormatEnum PixelFormat = new PLCamera.PixelFormatEnum();
        static PLCamera.SequencerTriggerSourceEnum TriggerSource = new PLCamera.SequencerTriggerSourceEnum();
        //public event EventHandler<Acqusition> GrabImageDone;

        public BaslerToHalcon(string serialNumber)
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

                ICamera iCamera = camera.Open();
                camera.Parameters[BaslerToHalcon.PixelFormat].TrySetValue(this.pixelType);
                camera.Parameters[PLCamera.ReverseX].SetValue(this.isMirrorX);
                //camera.Parameters[PLCamera.ReverseY].SetValue(this.isMirrorY);
                //camera.Parameters[PLCamera.TriggerSelector].SetValue("FrameStart");
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
                    if (acqQueue != null) acqQueue.Clear();
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
            if (!CloseCamera()) return false;
            this.isSoftwareTrigger = isSoftwareTrigger;
            return OpenCamera();
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
                    this.acqQueue.Dequeue().Image.Dispose();
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
        public override Acqusition Snap(int imageNum = 1, int timeOut = -1)
        {
            Acqusition _acq = new Acqusition();
            try
            {
                timeOut = (timeOut == -1) ? Timeout.Infinite : timeOut;
                camera.ExecuteSoftwareTrigger();
                DateTime t1 = DateTime.Now;
                while (true)
                {
                    lock (lockObj)
                    {
                        if (DateTime.Now.Subtract(t1).TotalMilliseconds > timeOut * (_acq.index + 2))//采集超时500ms
                        {
                            while (acqQueue.Count > 0)
                            {
                                acqQueue.Dequeue().Image.Dispose();
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
                                switch (acqQueue.Peek().GrabStatus)
                                {
                                    case "GrabPass":
                                        HOperatorSet.ConcatObj(_acq.Image, acqQueue.Dequeue().Image, out _acq.Image);
                                        _acq.index++;
                                        break;
                                    case "GrabFail":
                                        acqQueue.Dequeue().Image.Dispose();
                                        _acq.GrabStatus = "GrabFail:the latest image grab failed.";
                                        _acq.index++;
                                        return _acq;
                                    default:
                                        _acq.GrabStatus = acqQueue.Peek().GrabStatus;
                                        acqQueue.Dequeue().Image.Dispose();
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
        public override Acqusition GetFrames(int imageNum = 1, int timeOut = -1)
        {
            Acqusition _acq = new Acqusition();
            try
            {
                timeOut = (timeOut == -1) ? Timeout.Infinite : timeOut;
                DateTime t1 = DateTime.Now;
                while (true)
                {
                    lock (lockObj)
                    {
                        if (DateTime.Now.Subtract(t1).TotalMilliseconds > timeOut * (_acq.index + 2))//采集超时500ms
                        {
                            while (acqQueue.Count > 0)
                            {
                                acqQueue.Dequeue().Image.Dispose();
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
                                switch (acqQueue.Peek().GrabStatus)
                                {
                                    case "GrabPass":
                                        HOperatorSet.ConcatObj(_acq.Image, acqQueue.Dequeue().Image, out _acq.Image);
                                        _acq.index++;
                                        break;
                                    case "GrabFail":
                                        acqQueue.Dequeue().Image.Dispose();
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
        public override Task<Acqusition> GetFramesAsync(int imageNum = 1, int timeOut = -1)
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
                Acqusition _acq = new Acqusition();
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
            if (!CloseCamera()) return false;
            this.isSoftwareTrigger = isSoftwareTrigger;
            return OpenCamera();
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
        public override Acqusition Snap(int imageNum, int timeOut)
        {
            Acqusition acq = new Acqusition();
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
        Queue<Acqusition> acqQueue = new Queue<Acqusition>();
        static object lockObj = new object();
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
        private const int BUFFERING_SIZE_MAX = 20;
        private UserHookData userHookData = new UserHookData();
        GCHandle hUserData;
        MIL_DIG_HOOK_FUNCTION_PTR hookFunctionDelegate;

        MIL_ID[] MilGrabBufferList = new MIL_ID[BUFFERING_SIZE_MAX];
        MIL_INT MilGrabBufferListSize;
        MIL_INT n = 0;
        #endregion

        static Version Sfnc2_0_0 = new Version(2, 0, 0);
        static PLCamera.PixelFormatEnum PixelFormat = new PLCamera.PixelFormatEnum();
        static PLCamera.SequencerTriggerSourceEnum TriggerSource = new PLCamera.SequencerTriggerSourceEnum();
        ICommandParameter icp;
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
                //硬触发使用
                if (!this.isSoftwareTrigger) camera.Parameters[PLCamera.TriggerSource].SetValue("Line1");
                ////camera.Parameters[PLCamera.LineSelector].SetValue("Line1");
                

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
                for (n = 0; n < 2 && MilGrabBufferListSize > 0; n++)
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
                    camera.CameraOpened -= Configuration.SoftwareTrigger;
                    camera.Close();
                }
                MIL.MdigHalt(milDigitizer);
                MIL.MdigProcess(milDigitizer, MilGrabBufferList, MilGrabBufferListSize, MIL.M_STOP, MIL.M_DEFAULT, hookFunctionDelegate, GCHandle.ToIntPtr(hUserData));
                hUserData.Free();
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
            if (!CloseCamera()) return false;
            this.isSoftwareTrigger = isSoftwareTrigger;
            return OpenCamera();
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
                while (this.acqQueue.Count>0)
                {
                    this.acqQueue.Dequeue().Image.Dispose();
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public Acqusition Snap1(int imageNum, int timeOut)
        {
            Acqusition _acq = new Acqusition();
            try
            {
                //Task.Factory.StartNew(() =>
                //{
                //    Thread.Sleep(300);
                //    camera.ExecuteSoftwareTrigger();
                //});
                //use matrox cameralink grab image
                // Clear the buffer.
                MIL.MbufClear(milImage, 0);
                // Lock buffers for direct access.
                MIL.MbufControl(milImage, MIL.M_LOCK, MIL.M_DEFAULT);
                // Monoshot grab.
                //MIL.MdigGrab(milDigitizer, milImage);
                //MIL.MbufSave("E:\\test.mim", milImage);
                //MIL.MbufLoad("E:\\Image1.mim", milImage);
                // get image information.
                MIL.MbufInquire(milImage, MIL.M_HOST_ADDRESS, ref imageDataPtr);
                MIL.MbufInquire(milImage, MIL.M_SIZE_X, ref imageSizeX);
                MIL.MbufInquire(milImage, MIL.M_SIZE_Y, ref imageSizeY);
                MIL.MbufInquire(milImage, MIL.M_TYPE, ref SrcImageType);
                MIL.MbufInquire(milImage, MIL.M_PITCH_BYTE, ref SrcImagePitchByte);

                if (imageDataPtr != MIL.M_NULL && SrcImageType == FUNCTION_SUPPORTED_IMAGE_TYPE)
                {
                    unsafe
                    {
                        IntPtr ptr = imageDataPtr;

                        image.Dispose();
                        if (this.pixelType == BaslerMtxCamLinkToHalcon.PixelFormat.Mono8)
                        {
                            //黑白图像
                            //HOperatorSet.GenImage1(out image, "byte", Convert.ToInt32(imageSizeX), Convert.ToInt32(imageSizeY), ptr);
                            image.Dispose();
                            HOperatorSet.GenImage1Rect(out image, ptr, Convert.ToInt32(imageSizeX), Convert.ToInt32(imageSizeY), Convert.ToInt32(SrcImagePitchByte), 8, 8, "true", 0);
                        }
                        else
                        {
                            //RGB彩色图像
                            //HOperatorSet.GenImageInterleaved(out image, ptr, "bgrx", e.GrabResult.Width, e.GrabResult.Height, -1, "byte", 0, 0, 0, 0, -1, 0);
                        }
                    }
                    //HOperatorSet.WriteImage(image, "bmp", 0, "E:\\cmlk.bmp");
                    _acq.Image = image.CopyObj(1, -1);
                    _acq.GrabStatus = "GrabPass";
                }
                else
                {
                    _acq.GrabStatus = "GrabFail";
                }

                return _acq;
            }
            catch (Exception)
            {
                _acq.GrabStatus = "GrabFail";
                return _acq;
            }
            finally
            {
                // Unlock buffers.
                MIL.MbufControl(milImage, MIL.M_UNLOCK, MIL.M_DEFAULT);
                //MIL.MbufFree(milImage);
            }
        }
        public override Acqusition Snap(int imageNum = 1, int timeOut = -1)
        {
            Acqusition _acq = new Acqusition();
            try
            {
                timeOut = (timeOut == -1) ? Timeout.Infinite : timeOut;
                //camera.ExecuteSoftwareTrigger();
                //Task.Factory.StartNew(() =>
                //    {
                        
                        ICommandParameterExtensions.TryExecute(camera.Parameters[PLCamera.TriggerSoftware]);

                    //});
                DateTime t1 = DateTime.Now;
                while (true)
                {
                    lock (lockObj)
                    {
                        if (DateTime.Now.Subtract(t1).TotalMilliseconds > timeOut * (_acq.index + 2))//采集超时500ms
                        {
                            while (acqQueue.Count > 0)
                            {
                                acqQueue.Dequeue().Image.Dispose();
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
                                switch (acqQueue.Peek().GrabStatus)
                                {
                                    case "GrabPass":
                                        HOperatorSet.ConcatObj(_acq.Image, acqQueue.Dequeue().Image, out _acq.Image);
                                        _acq.index++;
                                        break;
                                    case "GrabFail":
                                        acqQueue.Dequeue().Image.Dispose();
                                        _acq.GrabStatus = "GrabFail:the latest image grab failed.";
                                        _acq.index++;
                                        return _acq;
                                    default:
                                        _acq.GrabStatus = acqQueue.Peek().GrabStatus;
                                        acqQueue.Dequeue().Image.Dispose();
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
        public override Acqusition GetFrames(int imageNum = 1, int timeOut = -1)
        {
            Acqusition _acq = new Acqusition();
            try
            {
                timeOut = (timeOut == -1) ? Timeout.Infinite : timeOut;
                DateTime t1 = DateTime.Now;
                while (true)
                {
                    lock (lockObj)
                    {
                        if (DateTime.Now.Subtract(t1).TotalMilliseconds > timeOut * (_acq.index + 2))//采集超时500ms
                        {
                            while (acqQueue.Count > 0)
                            {
                                acqQueue.Dequeue().Image.Dispose();
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
                                switch (acqQueue.Peek().GrabStatus)
                                {
                                    case "GrabPass":
                                        HOperatorSet.ConcatObj(_acq.Image, acqQueue.Dequeue().Image, out _acq.Image);
                                        _acq.index++;
                                        break;
                                    case "GrabFail":
                                        acqQueue.Dequeue().Image.Dispose();
                                        _acq.GrabStatus = "GrabFail:the latest image grab failed.";
                                        _acq.index++;
                                        return _acq;
                                    default:
                                        _acq.GrabStatus = acqQueue.Peek().GrabStatus;
                                        acqQueue.Dequeue().Image.Dispose();
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
        public override Task<Acqusition> GetFramesAsync(int imageNum = 1, int timeOut = -1)
        {
            return Task.Run(() =>
            {
                return GetFrames(imageNum, timeOut);
            });
        }
        WindowsFormsApplication1.Form1 frm1 = WindowsFormsApplication1.Form1.GetForm1();
        int num = 0;
        private MIL_INT HookFunction(MIL_INT HookType, MIL_ID HookId, IntPtr HookDataPtr)
        {
            num++;
            Acqusition _acq = new Acqusition();
            //frm1.textBox1.Text = num.ToString();
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

                try
                {
                    // Lock buffers for direct access.
                    MIL.MbufControl(ModifiedBufferId, MIL.M_LOCK, MIL.M_DEFAULT);
                    // get image information.
                    MIL.MbufInquire(ModifiedBufferId, MIL.M_HOST_ADDRESS, ref imageDataPtr);
                    MIL.MbufInquire(ModifiedBufferId, MIL.M_SIZE_X, ref imageSizeX);
                    MIL.MbufInquire(ModifiedBufferId, MIL.M_SIZE_Y, ref imageSizeY);
                    MIL.MbufInquire(ModifiedBufferId, MIL.M_TYPE, ref SrcImageType);
                    MIL.MbufInquire(ModifiedBufferId, MIL.M_PITCH_BYTE, ref SrcImagePitchByte);

                    if (imageDataPtr != MIL.M_NULL && SrcImageType == FUNCTION_SUPPORTED_IMAGE_TYPE)
                    {
                        unsafe
                        {
                            lock (this)
                            {
                                IntPtr ptr = imageDataPtr;

                                image.Dispose();
                                if (this.pixelType == BaslerMtxCamLinkToHalcon.PixelFormat.Mono8)
                                {
                                    //黑白图像
                                    //HOperatorSet.GenImage1(out image, "byte", Convert.ToInt32(imageSizeX), Convert.ToInt32(imageSizeY), ptr);
                                    HOperatorSet.GenImage1Rect(out image, ptr, Convert.ToInt32(imageSizeX), Convert.ToInt32(imageSizeY), Convert.ToInt32(SrcImagePitchByte), 8, 8, "true", 0);
                                    HOperatorSet.CropPart(image, out image, 0, 0, Convert.ToInt32(imageSizeX), Convert.ToInt32(imageSizeY));
                                }
                                else if (!this.pixelType.Contains("Mono"))
                                {
                                    //RGB彩色图像
                                    //HOperatorSet.GenImageInterleaved(out image, ptr, "bgrx", e.GrabResult.Width, e.GrabResult.Height, -1, "byte", 0, 0, 0, 0, -1, 0);
                                }
                                else
                                {
                                    if (acqQueue != null)
                                    {
                                        _acq.Dispose();
                                        _acq.GrabStatus = "current pixel format not support!";
                                        acqQueue.Enqueue(_acq);
                                        return -1;
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
                            return -1;
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (acqQueue != null)
                    {
                        _acq.Dispose();
                        _acq.GrabStatus = "GrabFail:" + ex.Message;
                        acqQueue.Enqueue(_acq);
                        return -1;
                    }
                }
                finally
                {
                    // Unlock buffers.
                    MIL.MbufControl(ModifiedBufferId, MIL.M_UNLOCK, MIL.M_DEFAULT);
                    // Clear the buffer.
                    MIL.MbufClear(ModifiedBufferId, 0);
                }
            }
            else
            {
                if (acqQueue != null)
                {
                    _acq.Dispose();
                    _acq.GrabStatus = "GrabFail";
                    acqQueue.Enqueue(_acq);
                    return -1;
                }
            }

            return 0;
        }

    }
}
