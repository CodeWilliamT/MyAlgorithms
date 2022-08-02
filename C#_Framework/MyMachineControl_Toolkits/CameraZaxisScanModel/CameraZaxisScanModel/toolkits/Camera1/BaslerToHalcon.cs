using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using HalconDotNet;
using Basler.Pylon;

namespace BaslerDll
{
    public class BaslerToHalcon
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
        //private 
        //private Resource r = new Resource();
        Queue<Acqusition> acqQueue = new Queue<Acqusition>();
        #endregion
        
        static Version Sfnc2_0_0 = new Version(2, 0, 0);
        public static PLCamera.PixelFormatEnum PixelFormat = new PLCamera.PixelFormatEnum();
        static PLCamera.SequencerTriggerSourceEnum TriggerSource = new PLCamera.SequencerTriggerSourceEnum();
        //public event EventHandler<Resource> GrabImageDone;
       
        public BaslerToHalcon(string serialNumber)
        {
            this.sn = serialNumber;
            HOperatorSet.GenEmptyObj(out image);
        }
        public bool InitCamera()
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
        public bool OpenCamera()
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

                camera.Open();
                camera.Parameters[PLCamera.ReverseX].SetValue(this.isMirrorX);
                camera.Parameters[PLCamera.ReverseY].SetValue(this.isMirrorY);
                //camera.Parameters[PLCamera.TriggerSelector].SetValue("FrameStart");
                camera.Parameters[PLCamera.TriggerMode].SetValue("On");
                //硬触发使用
                if (!this.isSoftwareTrigger) camera.Parameters[PLCamera.TriggerSource].SetValue("Line1");
                camera.StreamGrabber.Start(GrabStrategy.LatestImages, GrabLoop.ProvidedByStreamGrabber);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool CloseCamera()
        {
            try
            {
                if (camera.IsOpen)
                {
                    if (camera.StreamGrabber.IsGrabbing) camera.StreamGrabber.Stop();
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
        public bool IsSoftwareTrigger
        {
            set { this.isSoftwareTrigger = value; }
            get { return this.isSoftwareTrigger; }
        }
        public bool IsMirrorX
        {
            set { this.isMirrorX = value; }
            get { return this.isMirrorX; }
        }
        public bool IsMirrorY
        {
            set { this.isMirrorY = value; }
            get { return this.isMirrorY; }
        }
        public void SetGain(string value)
        {
            try
            {
                // Some camera models may have auto functions enabled. To set the gain value to a specific value,
                // the Gain Auto function must be disabled first (if gain auto is available).
                camera.Parameters[PLCamera.GainAuto].TrySetValue(PLCamera.GainAuto.Off); // Set GainAuto to Off if it is writable.

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
                    camera.Parameters[PLUsbCamera.Gain].SetValuePercentOfRange(Convert.ToDouble(value));
                    // For USB cameras, Gamma is always enabled.
                }
            }
            catch (Exception)
            {
                camera.Parameters[PLCamera.GainAuto].TrySetValue(PLCamera.GainAuto.Continuous);
            }

        }
        public void GetGain(out double value)
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
        public void SetExposure(double value)
        {
            try
            {
                camera.Parameters[PLCamera.ExposureTime].TrySetValue(value);
            }
            catch (Exception)
            {
                camera.Parameters[PLCamera.ExposureTime].TrySetValue(100);
            }
        }
        public void GetExposure(out double value)
        {
            try
            {
                value = camera.Parameters[PLCamera.ExposureTime].GetValue();
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
        public void SetPixelFormat(string value)
        {
            //value = BaslerToHalcon.PixelFormat.Mono8; 
            this.pixelType = value;
            try
            {
                camera.Parameters[BaslerToHalcon.PixelFormat].TrySetValue(value);
            }
            catch (Exception)
            {
                camera.Parameters[BaslerToHalcon.PixelFormat].TrySetValue(BaslerToHalcon.PixelFormat.Mono8);
            }

        }
        public void GetPixelFormat(out string value)
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
        public Acqusition Snap()
        {
            Acqusition _acq = new Acqusition();
            try
            {
                //SnapFunction();
                camera.ExecuteSoftwareTrigger();
                DateTime t1 = DateTime.Now;
                while (true)
                {
                    if (DateTime.Now.Subtract(t1).TotalMilliseconds > 500)//采集超时500ms
                    {
                        _acq.GrabStatus = "GrabFail";
                        return _acq;
                    }
                    else
                    {
                        if (acqQueue.Count > 0)
                        {
                            return acqQueue.Dequeue();
                        }
                    }
                }
            }
            catch (Exception)
            {
                _acq.GrabStatus = "GrabFail";
                return _acq;
            }
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
                                else
                                {
                                    //RGB彩色图像
                                    HOperatorSet.GenImageInterleaved(out image, ptr, "bgrx", e.GrabResult.Width, e.GrabResult.Height, -1, "byte", 0, 0, 0, 0, -1, 0);
                                }
                            }
                        }
                        if (acqQueue != null)
                        {
                            _acq.Dispose();
                            _acq.Image = image.CopyObj(1,-1);
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

        //void SnapFunction()
        //{
        //    //camera.StreamGrabber.Start();
        //    for (int i = 0; i < 1; i++)
        //    {
        //        //DateTime t1 = DateTime.Now;
        //        //camera.ExecuteSoftwareTrigger();
                
        //        IGrabResult grabResult = camera.StreamGrabber.RetrieveResult(2000, TimeoutHandling.ThrowException);
        //        using (grabResult)
        //        {
        //            // Image grabbed successfully?
        //            if (grabResult.GrabSucceeded)
        //            {
        //                byte[] buffer = grabResult.PixelData as byte[];
        //                unsafe
        //                {
        //                    fixed (byte* p = buffer)
        //                    {
        //                        IntPtr ptr = (IntPtr)p;
        //                        image.Dispose();
        //                        if (this.pixelType == BaslerToHalcon.PixelFormat.Mono8)
        //                        {
        //                            //黑白图像
        //                            HOperatorSet.GenImage1(out image, "byte", grabResult.Width, grabResult.Height, ptr);
                                  
        //                        }
        //                        else
        //                        {
        //                            //RGB彩色图像
        //                            HOperatorSet.GenImageInterleaved(out image, ptr, "bgrx", grabResult.Width, grabResult.Height, -1, "byte", 0, 0, 0, 0, -1, 0);
        //                        }
        //                    }
        //                }
        //                if (GrabImageDone != null)
        //                {
        //                    Acqusition a = new Acqusition();
        //                    a.Image.Dispose();
        //                    HOperatorSet.CopyImage(this.image, out a.Image);
        //                    //a.Image = img;
        //                    a.GrabStatus = "GrabPass";
        //                    a.Error = "";
        //                    r.GrabResult.Enqueue(a);
        //                    GrabImageDone(this, r);
        //                }
        //            }
        //            else
        //            {
        //                if (GrabImageDone != null)
        //                {
        //                    this.image.Dispose();
        //                    Acqusition a = new Acqusition();
        //                    a.GrabStatus = "GrabFail";
        //                    a.Error = grabResult.ErrorDescription;
        //                    r.GrabResult.Enqueue(a);
        //                    GrabImageDone(this, r);
        //                }
        //                //Console.WriteLine("Error: {0} {1}", grabResult.ErrorCode, grabResult.ErrorDescription);
        //            }
        //        }
        //        //TimeSpan ts = DateTime.Now.Subtract(t1);
        //        //double time = ts.TotalMilliseconds;
        //        //mf.tbMsg.AppendText(i.ToString() + ":" + time.ToString()+"\r\n");
        //    }
        //    //camera.StreamGrabber.Stop();
        //}
    }

    public partial class Resource : EventArgs
    {
        public Resource() { }
        //public Resource(Acqusition a)
        //{
        //    this.Image = a.Image;
        //    this.GrabStatus = a.GrabStatus;
        //    this.Error = a.Error;
        //}
        //public HObject Image = new HObject();// { get; set; }
        //public string GrabStatus;// { get; set; }
        //public string Error;// { get; set; }
        public Queue<Acqusition> GrabResult = new Queue<Acqusition>();
    }
    public class Acqusition
    {
        public Acqusition() { }

        public HObject Image = new HObject();// { get; set; }
        public string GrabStatus;// { get; set; }
        public string Error;// { get; set; }
        public void Dispose()
        {
            this.Image.Dispose();
            this.GrabStatus = "";
            this.Error = "";
        }
    }
}
