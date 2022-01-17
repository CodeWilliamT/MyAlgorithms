using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using HalconDotNet;
using NationalInstruments.CWIMAQControls;
using niimaqdx;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;

namespace NIImageAcquisition
{
    public class NIToHalcon
    {
        //public event EventHandler<Resource> GrabImageDone;
              
        #region 私有变量
        private string camName = "";
        private int triggerSource = 1;
        private bool isConnected = false;
        private PixelFormatEnum pixelFormat = PixelFormatEnum.Mono_8;
        private int timeOut = 500;//ms
        private HObject image = new HObject();
        private int img_width, img_height;
        private CWIMAQdx.Session session = new CWIMAQdx.Session();
        private byte[] buffer;
        //private CancellationTokenSource cts = null;
        //private Thread thread = null;
        private Resource r = new Resource();
        #endregion
        /*-----------------------------------方法---------------------------------------------*/
        public NIToHalcon(string cam_name)
        {
            this.camName = cam_name;
            HOperatorSet.GenEmptyObj(out image);
        }

        public enum PixelFormatEnum
        {
            Mono_8 = 17301505,
            Mono_12 = 17825797,
            Mono_12_packed = 17563654,
            YUV_422_packed = 34603039,
            YUV_422_YUYV__packed = 34603058
        }
        //
        public bool InitCamera() { return true; }
        //打开相机
        public bool OpenCamera()
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
                err = CWIMAQdx.SetAttribute(session, "CameraAttributes::AcquisitionTrigger::TriggerSource", CWIMAQdx.ValueType.U32, this.triggerSource);
                if (err != CWIMAQdx.Error.Success) return false;
                err = CWIMAQdx.SetAttribute(session, "CameraAttributes::ImageFormat::PixelFormat", CWIMAQdx.ValueType.U32, this.pixelFormat);
                if (err != CWIMAQdx.Error.Success) return false;
                err = CWIMAQdx.SetAttribute(session, "AcquisitionAttributes::TimeOut", CWIMAQdx.ValueType.U32, this.timeOut);
                if (err != CWIMAQdx.Error.Success) return false;

                object width, height;
                err = CWIMAQdx.GetAttribute(session, "CameraAttributes::AOI::Width", CWIMAQdx.ValueType.I64, out width);
                err = CWIMAQdx.GetAttribute(session, "CameraAttributes::AOI::Height", CWIMAQdx.ValueType.I64, out height);
                img_width = Convert.ToInt32(width);
                img_height=Convert.ToInt32(height);
                int len = img_width * img_height;
                buffer = new byte[len];
                CWIMAQdx.ConfigureAcquisition(session, true, 1);
                CWIMAQdx.StartAcquisition(session);
                //cts = new CancellationTokenSource();
                //thread = new Thread(new ThreadStart(SnapFunc));
                //thread.IsBackground = true;
                //thread.Start();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        //关闭相机
        public bool CloseCamera()
        {
            try
            {
                //if(cts!=null) cts.Cancel();
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
        public void SetTriggerSource(int source)
        {
            //0:软触发
            //1:硬触发
            this.triggerSource = source;
        }
        //获取相机增益
        public void GetGain(out double gain)
        {
            object gain_obj;
            CWIMAQdx.GetAttribute(session, "CameraAttributes::AnalogControls::GainRaw", CWIMAQdx.ValueType.I64, out gain_obj);
            gain = Convert.ToDouble(gain_obj);
        }
        //获取相机曝光时间
        public void GetExposure(out double exposure)
        {
            object exp_obj;
            CWIMAQdx.GetAttribute(session, "CameraAttributes::AcquisitionTrigger::ExposureTimeRaw", CWIMAQdx.ValueType.I64, out exp_obj);
            exposure = Convert.ToDouble(exp_obj);
        }
        //设置相机增益
        public void SetGain(double gain)
        {
            CWIMAQdx.SetAttribute(session, "CameraAttributes::AnalogControls::GainRaw", CWIMAQdx.ValueType.I64, gain);
        }
        //设置相机曝光时间
        public void SetExposure(double exposure)
        {
            CWIMAQdx.SetAttribute(session, "CameraAttributes::AcquisitionTrigger::ExposureTimeAbs", CWIMAQdx.ValueType.I64, exposure);
        }
        //设置相机图像格式
        public void SetPixelFormat(PixelFormatEnum pix_format)
        {
            this.pixelFormat = pix_format;
        }
        public void GetPixelFormat(out PixelFormatEnum pix_format)
        {
            object _pix_format = "";
            CWIMAQdx.GetAttribute(session, "CameraAttributes::ImageFormat::PixelFormat", CWIMAQdx.ValueType.U32, out _pix_format);
            pix_format = (PixelFormatEnum)Enum.Parse(typeof(PixelFormatEnum), _pix_format.ToString());
        }
        /// <summary>
        /// 软件触发采集图像，主动采集
        /// </summary>
        /// <returns></returns>
        public Acqusition Snap()
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
        public Acqusition Hard_Snap()
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
        private void Snap1()
        {
            CWIMAQdx.SetAttribute(session, "CameraAttributes::AcquisitionTrigger::TriggerSoftware", CWIMAQdx.ValueType.String, "Execute");
        }

        //private void SnapFunc()
        //{
        //    while (true)
        //    {
        //        if (cts.Token.IsCancellationRequested) return;
        //        int bufferActual = 0;
        //        CWIMAQdx.Error err = CWIMAQdx.GetImageData(session, ref buffer, CWIMAQdx.BufferNumberMode.Next, 0, out bufferActual);
        //        if (err == CWIMAQdx.Error.Success)
        //        {
        //            unsafe
        //            {
        //                fixed (byte* p = buffer)
        //                {
        //                    IntPtr ptr = (IntPtr)p;
        //                    image.Dispose();
        //                    //黑白格式
        //                    if (this.pixelFormat.ToString().Contains("Mono"))
        //                    {
        //                        HOperatorSet.GenImage1(out image, "byte", img_width, img_height, ptr);
        //                    }
        //                    //彩色格式
        //                    else
        //                    {

        //                    }
        //                }
        //            }
        //            if (GrabImageDone != null)
        //            {
        //                Acqusition a = new Acqusition();
        //                a.Image.Dispose();
        //                HOperatorSet.CopyImage(this.image, out a.Image);
        //                this.image.Dispose();
        //                a.GrabStatus = "GrabPass";
        //                a.Error = "";
        //                r.GrabResult.Enqueue(a);
        //                GrabImageDone(this, r);
        //            }
        //        }
        //        else if (err != CWIMAQdx.Error.Timeout)
        //        {
        //            if (GrabImageDone != null)
        //            {
        //                Acqusition a = new Acqusition();
        //                a.Image.Dispose();
        //                a.GrabStatus = "GrabFail";
        //                a.Error = err.ToString();
        //                r.GrabResult.Enqueue(a);
        //                GrabImageDone(this, r);
        //            }
        //        }
        //    }
        //}
        
/*------------------------------结尾------------------------------------------------------*/
    }
    public partial class Resource : EventArgs
    {
        public Resource() { }
        public Queue<Acqusition> GrabResult = new Queue<Acqusition>();
    }
    public class Acqusition
    {
        public Acqusition() { }

        public HObject Image = new HObject();// { get; set; }
        public string GrabStatus;// { get; set; }
        public string Error;// { get; set; }
    }
}
