using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ToolKits.HTCamera
{
    public class CameraMgr
    {
        private BaseCamera baseCamera;
        private CameraEnum camera_type;
        public CameraEnum CameraType
        {
            get { return this.camera_type; }
            set { this.camera_type = value; }
        }
        public bool IsConnected
        {
            get { return baseCamera.IsConnected; }
        }
        public bool IsSoftwareTrigger
        {
            get { return baseCamera.IsSoftwareTrigger; }
            set { baseCamera.IsSoftwareTrigger = value; }
        }
        public bool IsCameraTrigger
        {
            get { return baseCamera.IsCameraTrigger; }
            set { baseCamera.IsCameraTrigger = value; }
        }
        public bool IsMirrorX
        {
            get { return baseCamera.IsMirrorX; }
            set { baseCamera.IsMirrorX = value; }
        }
        public bool IsMirrorY
        {
            get { return baseCamera.IsMirrorY; }
            set { baseCamera.IsMirrorY = value; }
        }
        /// <summary>
        /// 实例化相机管理类
        /// </summary>
        /// <param name="camType">相机类型</param>
        /// <param name="camName">NI接口，相机名称为相机的ID，其它为相机的SN</param>
        public CameraMgr(CameraEnum camType, string camName, string camFile = "")
        {
            this.camera_type = camType;
            switch (camType)
            {
                case CameraEnum.BaslerNI:
                    baseCamera = new BaslerFromNIToHalcon(camName);
                    break;
                case CameraEnum.Basler:
                    baseCamera = new BaslerToHalcon(camName);
                    break;
                //case CameraEnum.PointGray:
                //    baseCamera = new PointGreyToHalcon(camName);
                //    break;
                case CameraEnum.SVS:
                    baseCamera = new SvsToHalcon();
                    break;
                //case CameraEnum.BaslerMatroxCamLink:
                //    baseCamera = new BaslerMtxCamLinkToHalcon(camName);
                //    break;
                case CameraEnum.BaslerVirtualToHalcon:
                    //baseCamera = new BaslerVirtualToHalcon(camName);
                    break;
                case CameraEnum.BaslerEuresysCamLink:
                    baseCamera = new BaslerEuresysCamLink(camName, camFile);
                    break;
                case CameraEnum.MVSCamera:
                    baseCamera = new MVSCamToHalcon(camName);
                    break;
                case CameraEnum.SVSEuresysCamLink:
                    baseCamera = new SVSEuresysCamLink(camName, camFile);
                    break;
                case CameraEnum.GenaralEuresyCamLink:
                    baseCamera = new GenaralEuresyCamLink(camName, camFile);
                    break;
                default:
                    MessageBox.Show("无法识别该相机类型！");
                    break;
            }
        }
        public bool InitCamera()
        {
            if (baseCamera == null) return false;
            return baseCamera.InitCamera();
        }
        public bool OpenCamera()
        {
            if (baseCamera == null) return false;
            return baseCamera.OpenCamera();
        }
        public bool CloseCamera()
        {
            if (baseCamera == null) return false;
            return baseCamera.CloseCamera();
        }
        public bool ChangeTriggerSource(bool isSoftwareTrigger)
        {
            if (baseCamera == null) return false;
            return baseCamera.ChangeTriggerSource(isSoftwareTrigger);
        }
        public bool ChangeTriggerLight(bool isTriggerLight)
        {
            if (baseCamera == null) return false;
            return baseCamera.ChangeTriggerLight(isTriggerLight);
        }
        public bool SetGain(Double value)
        {
            if (baseCamera == null) return false;
            return baseCamera.SetGain(value);
        }
        public void GetGain(out Double value)
        {
            value = 0;
            if (baseCamera != null)
                baseCamera.GetGain(out value);
        }
        public bool SetExposure(Double value)
        {
            if (baseCamera == null) return false;
            return baseCamera.SetExposure(value);
        }
        public void GetExposure(out Double value)
        {
            value = 0;
            if (baseCamera != null)
                baseCamera.GetExposure(out value);
        }
        public bool SetPixelFormat(object pixelFormat)
        {
            bool val = false;
            switch (this.camera_type)
            {
                case CameraEnum.BaslerNI:
                    val = baseCamera.BaslerFromNISetPixelFormat((BaslerFromNIPixelFormat)pixelFormat);
                    break;
                case CameraEnum.Basler:
                    val = baseCamera.BaslerSetPixelFormat(pixelFormat.ToString());
                    break;
                case CameraEnum.PointGray:
                    val = baseCamera.PointGraySetPixelFormat((PointGreyPixelFormat)pixelFormat);
                    break;
                case CameraEnum.SVS:
                    break;
                case CameraEnum.BaslerMatroxCamLink:
                    val = baseCamera.BaslerSetPixelFormat(pixelFormat.ToString());
                    break;
                case CameraEnum.BaslerVirtualToHalcon:
                    val = baseCamera.BaslerSetPixelFormat(pixelFormat.ToString());
                    break;
                case CameraEnum.BaslerEuresysCamLink:
                    val = baseCamera.EuresysSetPixelFormat(pixelFormat.ToString());
                    break;
                case CameraEnum.SVSEuresysCamLink:
                    val = baseCamera.EuresysSetPixelFormat(pixelFormat.ToString());
                    break;
                case CameraEnum.GenaralEuresyCamLink:
                    val = baseCamera.EuresysSetPixelFormat(pixelFormat.ToString());
                    break;
                default:
                    MessageBox.Show("未知的像素格式");
                    return false;
            }
            return val;
        }
        public void GetPixelFormat(out object pixelFormat)
        {
            pixelFormat = "";
            switch (this.camera_type)
            {
                case CameraEnum.BaslerNI:
                    BaslerFromNIPixelFormat _pixelFormat1;
                    baseCamera.BaslerFromNIGetPixelFormat(out _pixelFormat1);
                    pixelFormat = _pixelFormat1;
                    break;
                case CameraEnum.Basler:
                    string _pixelFormat2;
                    baseCamera.BaslerGetPixelFormat(out _pixelFormat2);
                    pixelFormat = _pixelFormat2;
                    break;
                case CameraEnum.PointGray:
                    PointGreyPixelFormat _pixelFormat3;
                    baseCamera.PointGrayGetPixelFormat(out _pixelFormat3);
                    pixelFormat = _pixelFormat3;
                    break;
                case CameraEnum.SVS:
                    break;
                case CameraEnum.BaslerMatroxCamLink:
                    string _pixelFormat4;
                    baseCamera.BaslerGetPixelFormat(out _pixelFormat4);
                    pixelFormat = _pixelFormat4;
                    break;
                case CameraEnum.BaslerEuresysCamLink:
                    string _pixelFormat5;
                    baseCamera.EuresysGetPixelFormat(out _pixelFormat5);
                    pixelFormat = _pixelFormat5;
                    break;
                case CameraEnum.GenaralEuresyCamLink:
                    break;
                default:
                    break;
            }
        }
        public Acquisition Snap(int imageNum = 1, int timeOut = -1)
        {
            if (baseCamera == null)
            {
                Acquisition acq = new Acquisition();
                acq.GrabStatus = "相机未初始化!";
                return acq;
            }
            return baseCamera.Snap(imageNum, timeOut);
        }
        public Acquisition Hard_Snap()
        {
            if (baseCamera == null)
            {
                Acquisition acq = new Acquisition();
                acq.GrabStatus = "相机未初始化!";
                return acq;
            }
            return baseCamera.Hard_Snap();
        }
        public Acquisition GetFrames(int imageNum = 1, int timeOut = -1)
        {
            if (baseCamera == null)
            {
                Acquisition acq = new Acquisition();
                acq.GrabStatus = "相机未初始化!";
                return acq;
            }
            return baseCamera.GetFrames(imageNum, timeOut);
        }
        public Task<Acquisition> GetFramesAsync(int imageNum = 1, int timeOut = -1)
        {
            if (baseCamera == null)
            {
                {
                    return Task.Run(() =>
                    {
                        Acquisition acq = new Acquisition();
                        acq.GrabStatus = "相机未初始化!";
                        return acq;
                    });
                }
            }
            return baseCamera.GetFramesAsync(imageNum, timeOut);
        }
        public bool ClearImageQueue()
        {
            if (baseCamera != null) return baseCamera.ClearImageQueue();
            return true;
        }
    }
}
