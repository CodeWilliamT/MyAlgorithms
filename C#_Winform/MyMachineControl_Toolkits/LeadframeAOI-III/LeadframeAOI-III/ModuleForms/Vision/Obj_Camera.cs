using System;
using System.Collections.Generic;
using ToolKits.HTCamera;
using HalconDotNet;
using System.IO;
using HT_Lib;
using IniDll;
using System.ComponentModel;

namespace LeadframeAOI
{
    public class Obj_Camera
    {

        #region 公共接口变量
        /// <summary>相机总数</summary>
        public static int Num_Camera = 1;
        /// <summary>当前相机</summary>
        public static int SelectedIndex = 0;
        /// <summary>默认相机</summary>
        public static int DefaultSelectedIdx = 0;
        //**************相机配置属性**********************
        /// <summary>相机名</summary>
        public string cameraName;
        /// <summary>相机配置文件目录</summary>
        public string camFile;
        /// <summary>相机类型</summary>
        public CameraEnum cameraType;
        /// <summary>是否启用</summary>
        public bool isEnable;
        /// <summary>曝光值</summary>
        public Double exposure;
        /// <summary>增益值</summary>
        public Double gain;
        /// <summary>是否相机触发</summary>
        public bool isCameraTrigger;
        /// <summary>是否软触发</summary>
        public bool isSoftwareTrigger;
        /// <summary>是否X镜像</summary>
        public bool isMirrorX;
        /// <summary>是否Y镜像</summary>
        public bool isMirrorY;

        //**************相机应用属性**********************
        /// <summary>相机图像存储目录</summary>
        public string cameraPath;
        /// <summary>相机是否正在连续采集</summary>
        public bool isGrab;
        /// <summary>
        /// 相机所拍图像内存对象
        /// </summary>
        public Acquisition acq;

        public HObject ImageIC, ImagePCB, ImageLine;


        [BrowsableAttribute(false)]
        public CameraMgr Camera
        {
            get { return camera; }
            set { camera = value; }
        }

        [CategoryAttribute("相机初始化属性"), DisplayNameAttribute("相机名"), DescriptionAttribute("相机名")]
        public string CameraName { get { return cameraName; } set { cameraName = value; } }
        [CategoryAttribute("相机初始化属性"), DisplayNameAttribute("相机配置文件目录"), DescriptionAttribute("相机配置文件目录")]
        public string CamFile { get { return camFile; } set { camFile = value; } }
        [CategoryAttribute("相机初始化属性"), DisplayNameAttribute("相机类型"), DescriptionAttribute("相机类型")]
        public CameraEnum CameraType { get { return cameraType; } set { cameraType = value; } }
        [CategoryAttribute("相机初始化属性"), DisplayNameAttribute("是否启用"), DescriptionAttribute("是否启用")]
        public bool IsEnable { get { return isEnable; } set { isEnable = value; } }
        [CategoryAttribute("相机配置用属性"), DisplayNameAttribute("曝光值"), DescriptionAttribute("曝光值")]
        public double Exposure { get { return exposure; } set { exposure = value; } }
        [CategoryAttribute("相机配置用属性"), DisplayNameAttribute("增益值"), DescriptionAttribute("增益值")]
        public double Gain { get { return gain; } set { gain = value; } }
        [CategoryAttribute("相机配置用属性"), DisplayNameAttribute("是否相机触发"), DescriptionAttribute("是否相机触发")]
        public bool IsCameraTrigger { get { return isCameraTrigger; } set { isCameraTrigger = value; } }
        [CategoryAttribute("相机配置用属性"), DisplayNameAttribute("是否软触发"), DescriptionAttribute("是否软触发")]
        public bool IsSoftwareTrigger { get { return isSoftwareTrigger; } set { isSoftwareTrigger = value; } }
        [CategoryAttribute("相机配置用属性"), DisplayNameAttribute("是否X镜像"), DescriptionAttribute("是否X镜像")]
        public bool IsMirrorX { get { return isMirrorX; } set { isMirrorX = value; } }
        [CategoryAttribute("相机配置用属性"), DisplayNameAttribute("是否Y镜像"), DescriptionAttribute("是否Y镜像")]
        public bool IsMirrorY { get { return isMirrorY; } set { isMirrorY = value; } }

        #endregion

        #region 私有变量
        private HObject _images = null;
        private HTuple _imagesIndex = null;
        private int _preRow = 0; //记录visionScanPoint 扫描信号中行变化
        private HObjectVector _imagesVector = new HObjectVector(1);
        private HTupleVector _imagesIndexVector = new HTupleVector(1);

        /// <summary>相机对象</summary>
        private CameraMgr camera;
        #endregion


        public Obj_Camera()
        {
            this.camera = null;
            this.cameraName = "";
            this.camFile = "";
            this.cameraType = 0;
            this.isEnable = true;
            this.exposure = 40;
            this.gain = 0;
            this.isCameraTrigger = false;
            this.isSoftwareTrigger = false;
            this.isMirrorX = false;
            this.isMirrorY = false;


            this.acq = new Acquisition();
            this.cameraPath = "";
            this.isGrab = false;
        }

        #region 公共接口方法
        /// <summary>
        /// 初始化图像内存对象，相机，并打开相机
        /// </summary>
        public void Initialize()
        {
            acq = new Acquisition();
            if (!InitCamera())
            {
                HTUi.PopError("初始化相机失败！");
                return;
            }
            if (!OpenCamera())
            {
                HTUi.PopError("打开相机失败！");
                return;
            }
        }


        /// <summary>
        /// 初始化相机
        /// </summary>
        /// <returns></returns>
        public Boolean InitCamera()
        {
            //return true;
            //初始化相机
            try
            {
                switch (cameraType)
                {
                    case CameraEnum.MVSCamera://海康相机
                        camera = new CameraMgr(cameraType, cameraName);
                        camera.IsCameraTrigger = false;
                        camera.IsSoftwareTrigger = false;
                        camera.IsMirrorX = false;
                        camera.IsMirrorY = false;
                        return camera.InitCamera();
                    case CameraEnum.BaslerEuresysCamLink://Euresys采集卡相机
                        if (camFile == "") camFile = App.programDir + @"\Camfile\basler\beA4000-62km_P62RG_0828.cam";
                        camera = new CameraMgr(cameraType, cameraName, camFile);
                        camera.SetPixelFormat(ToolKits.HTCamera.EuresysPixelFormat.Y8);
                        camera.IsCameraTrigger = false;
                        camera.IsSoftwareTrigger = false;
                        camera.IsMirrorX = false;
                        camera.IsMirrorY = false;
                        return camera.InitCamera();
                    case CameraEnum.SVSEuresysCamLink:
                        if (camFile == "") camFile = App.programDir + @"\Camfile\SVS\HR25000CCL_10TAP_8Bit.cam";
                        if (cameraName == "") cameraName = "COM8";
                        camera = new CameraMgr(cameraType, cameraName, camFile);
                        camera.SetPixelFormat(ToolKits.HTCamera.EuresysPixelFormat.BAYER8);
                        camera.IsCameraTrigger = false;
                        camera.IsSoftwareTrigger = false;
                        camera.IsMirrorX = false;
                        camera.IsMirrorY = false;

                        return camera.InitCamera();
                    case CameraEnum.GenaralEuresyCamLink:
                        if (camFile == "") camFile = App.programDir + @"\Camfile\Genaral\S-25A30c_P32RG-0418.cam";
                        if (cameraName == "") cameraName = "COM8";
                        camera = new CameraMgr(cameraType, cameraName, camFile);
                        camera.SetPixelFormat(ToolKits.HTCamera.EuresysPixelFormat.BAYER8);
                        camera.IsCameraTrigger = false;
                        camera.IsSoftwareTrigger = false;
                        camera.IsMirrorX = false;
                        camera.IsMirrorY = false;

                        return camera.InitCamera();
                    default:
                        return false;
                }
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 打开相机
        /// </summary>
        /// <returns></returns>
        public Boolean OpenCamera()
        {
            //return true;
            try
            {
                if (!camera.OpenCamera()) return false;
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 关闭相机
        /// </summary>
        /// <returns></returns>
        public Boolean CloseCamera()
        {
            //return true;
            return camera.CloseCamera();
        }
        /// <summary>
        /// 更改触发方式
        /// </summary>
        /// <param name="isSoftwareTrigger">改成非软触发(硬触发)还是软触发</param>
        /// <returns>是否成功</returns>
        public Boolean ChangeTriggerSource(bool isSoftwareTrigger)
        {
            return camera.ChangeTriggerSource(isSoftwareTrigger);
        }

        /// <summary>
        /// 设置增益值
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool SetGain(Double value)
        {
            gain = value;
            return camera.SetGain(value);
            //throw new Exception("配置增益失败");
            //return true;
        }
        /// <summary>
        /// 获取增益值
        /// </summary>
        /// <param name="value"></param>
        public void GetGain(out Double value)
        {
            camera.GetGain(out value);
        }
        /// <summary>
        /// 设置曝光值
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool SetExposure(Double value)
        {
            exposure = value;
            if (!camera.SetExposure(value))
                throw new Exception("配置曝光失败");
            return true;
        }
        /// <summary>
        /// 获取曝光值
        /// </summary>
        /// <param name="value"></param>
        public void GetExposure(out Double value)
        {
            camera.GetExposure(out value);
        }
        /// <summary>
        /// 软触发拍照
        /// </summary>
        /// <param name="imageNum"></param>
        /// <param name="timeOut"></param>
        /// <returns></returns>
        public Acquisition Snap(int imageNum = 1, int timeOut = -1)
        {
            return camera.Snap(imageNum, timeOut);
        }
        /// <summary>
        /// 硬触发后，获取触发时行列坐标，获取的图片并且保存，by TWL
        /// </summary>
        /// <param name="rowIndex">行号</param>
        /// <param name="columnIndex">列号</param>
        /// <param name="imageNum">获取张数</param>
        /// <param name="timeOut">超时时间</param>
        /// <returns></returns>
        public Boolean ScanPoint(int rowIndex, int columnIndex, int imageIndex = 0, int imageNum = 1, int timeOut = -1)
        {
            cameraPath = App.obj_Vision.frameFolder;
            int imgIndex = imageIndex;
            if (camera.IsSoftwareTrigger) acq = camera.Snap(imageNum, timeOut);
            else acq = camera.GetFrames(imageNum, timeOut);
            if (acq.GrabStatus == "GrabPass")
            {
                //新建时间文件夹存图
                string sPath = "";
                sPath = cameraPath + "\\" + rowIndex.ToString() + "-" + columnIndex.ToString();
                if (!Directory.Exists(cameraPath))
                {
                    Directory.CreateDirectory(cameraPath);
                }

                HObject ImageSrc = new HObject();
                for (int i = 0; i < imageNum; i++)
                {
                    //从acq里取图并保存图片，行号-列号.tiff  
                    HOperatorSet.SelectObj(acq.Image, out ImageSrc, i + 1);
                    FormJobs.Instance.ShowImage(ImageSrc, null);
                    App.obj_Vision.ShowImage(frmCaptureImage.Instance.htWindow, ImageSrc, null);
                    if (imgIndex == 0) ImageIC = acq.Image.CopyObj(1, -1);
                    else if (imgIndex == 1) ImagePCB = acq.Image.CopyObj(1, -1);
                    else if (imgIndex == 2) ImageLine = acq.Image.CopyObj(1, -1);
                    if (!Directory.Exists(sPath))
                    {
                        Directory.CreateDirectory(sPath);
                    }
                    //if (false == App.obj_SystemConfig.ImageNgSave)
                    //{
                    //    HOperatorSet.WriteImage(ImageSrc, "tiff", 0, sPath + "\\" + imgIndex.ToString() + ".tiff");
                    //}
                }
                //图片内存存在Vision.acq里，外存于指定目录下时间文件夹内
                acq.Dispose();
                return true;
            }
            else
            {
                acq.Dispose();
                return false;
            }
        }
        /// <summary>
        /// 获取标定图像
        /// </summary>
        /// <param name="rowIndex">此处为移动的x坐标</param>
        /// <param name="columnIndex">移动的y坐标</param>
        /// <param name="imageIndex">标识获取的第几副图片默认为-1</param>
        /// <param name="imageNum">一次拍1副图像</param>
        /// <param name="timeOut">超时</param>
        /// <returns></returns>
        public Boolean ScanCalibrationPoint(Double xCoords, Double yCoords, int imageIndex = 0, int imageNum = 1, int timeOut = -1)
        {
            //int imgIndex = imageIndex;
            //Thread.Sleep(200);
            if (camera.IsSoftwareTrigger) acq = camera.Snap(imageNum, timeOut);
            else acq = camera.GetFrames(imageNum, 5000);
            if (acq.GrabStatus == "GrabPass")
            {
                //新建时间文件夹存图
                string sPath = "";
                //    + "\\" + DateTime.Now.ToString("yyyy-MM-dd") + "-"
                //    + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString();
                if (!Directory.Exists(App.obj_Operations.CalibImagePath))
                {
                    Directory.CreateDirectory(App.obj_Operations.CalibImagePath);
                }
                HObject ImageSrc = new HObject();
                //for (int i = 0; i < imageNum; i++)
                //{
                //    //从acq里取图并保存图片，行号-列号.tiff
                HOperatorSet.SelectObj(acq.Image, out ImageSrc, 1);
                App.obj_Vision.ShowImage(frmCaptureImage.Instance.htWindow, ImageSrc, null);
                //(1, -1) 从头到尾都复制
                //if (imgIndex == 0) ImageIC = Acq.Image.CopyObj(1, -1);
                //else if (imgIndex == 1) ImagePCB = Acq.Image.CopyObj(1, -1);
                //else if (imgIndex == 2) ImageLine = Acq.Image.CopyObj(1, -1);
                //GetExposure(out exposure);
                //GetGain(out gain);
                sPath = App.obj_Operations.CalibImagePath + "\\" + ((int)xCoords).ToString() + "-" + ((int)yCoords).ToString();
                //if (!Directory.Exists(sPath))
                //{
                //  Directory.CreateDirectory(sPath);
                //}

                HOperatorSet.WriteImage(ImageSrc, "tiff", 0, sPath + ".tiff");
                //图片内存存在Vision.acq里，外存于指定目录下时间文件夹内
                //Acq.Dispose();
                return true;
            }
            else
            {
                acq.Dispose();
                return false;
            }
        }
        /// <summary>
        /// 硬触发后，获取触发时获取的图片，by TWL
        /// </summary>
        /// <param name="rowIndex">行号</param>
        /// <param name="columnIndex">列号</param>
        /// <param name="imageNum">获取张数</param>
        /// <param name="timeOut">超时时间</param>
        /// <returns></returns>
        public Boolean ScanTestOnePoint(int imageIndex = 0, int imageNum = 1, int timeOut = -1)
        {
            int imgIndex = imageIndex;
            if (camera.IsSoftwareTrigger) acq = camera.Snap(imageNum, timeOut);
            else acq = camera.GetFrames(imageNum, timeOut);
            if (acq.GrabStatus == "GrabPass")
            {
                //新建时间文件夹存图
                if (!Directory.Exists(App.obj_Vision.TestImagePath))
                {
                    Directory.CreateDirectory(App.obj_Vision.TestImagePath);
                }
                HObject ImageSrc = new HObject();
                for (int i = 0; i < imageNum; i++)
                {
                    //从acq里取图并保存图片，行号-列号.tiff
                    HOperatorSet.SelectObj(acq.Image, out ImageSrc, i + 1);
                    App.obj_Vision.ShowImage(frmCaptureImage.Instance.htWindow, ImageSrc, null);
                    HOperatorSet.WriteImage(ImageSrc, "tiff", 0, App.obj_Vision.TestImagePath + "\\" + imgIndex.ToString() + ".tiff");
                }
                //图片内存存在Vision.acq里，外存于指定目录下时间文件夹内
                acq.Dispose();
                return true;
            }
            else
            {
                acq.Dispose();
                return false;
            }
        }

        public Boolean ScanTestFramePoints(int rowIndex, int columnIndex, int imageIndex = 0, int imageNum = 1, int timeOut = -1)
        {
            int imgIndex = imageIndex;
            if (camera.IsSoftwareTrigger) acq = camera.Snap(imageNum, timeOut);
            else acq = camera.GetFrames(imageNum, timeOut);
            if (acq.GrabStatus == "GrabPass")
            {

                //新建时间文件夹存图
                if (!Directory.Exists(App.obj_Vision.TestImagePath))
                {
                    Directory.CreateDirectory(App.obj_Vision.TestImagePath);
                }
                HObject ImageSrc = new HObject();
                for (int i = 0; i < imageNum; i++)
                {
                    //从acq里取图并保存图片，行号-列号.tiff
                    HOperatorSet.SelectObj(acq.Image, out ImageSrc, i + 1);

                    FormUV2XY.Instance.ShowImage(FormUV2XY.Instance.htWindowCalibration, ImageSrc, null);
                    string sPath = App.obj_Vision.TestImagePath + "\\" + rowIndex.ToString() + "-" + columnIndex.ToString();
                    if (!Directory.Exists(sPath))
                    {
                        Directory.CreateDirectory(sPath);
                    }
                    HOperatorSet.WriteImage(ImageSrc, "tiff", 0, sPath + "\\" + imgIndex.ToString() + ".tiff");
                }
                //图片内存存在Vision.acq里，外存于指定目录下时间文件夹内
                acq.Dispose();
                return true;
            }
            else
            {
                acq.Dispose();
                return false;
            }
        }

        /// <summary>
        /// 行扫描硬触发后，获取触发时获取的图片并且保存，by TWL
        /// </summary>
        /// <param name="rowIndex">行号</param>
        /// <param name="imageNum">触发点数目</param>
        /// <param name="timeOut">超时时间</param>
        /// <returns></returns>
        public Boolean ScanRow(int rowIndex, int imageNum = 1, int timeOut = -1)
        {
            return true;
            //Acq = camera.GetFrames(imageNum, timeOut);
            //if (acq.GrabStatus == "GrabPass")
            //{
            //    //新建时间文件夹存图
            //    string sPath = imageFolder + "\\" + DateTime.Now.ToString("yyyy-MM-dd") + "-"
            //        + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString();
            //    if (!Directory.Exists(sPath))
            //    {
            //        Directory.CreateDirectory(sPath);
            //    }
            //    HObject ImageSrc = new HObject();
            //    for (int i = 0; i < imageNum; i++)
            //    {
            //        //从acq里取图并保存图片，行号-列号.tiff
            //        HOperatorSet.SelectObj(Acq.Image, out ImageSrc, i + 1);
            //        HOperatorSet.WriteImage(ImageSrc, "tiff", 0, sPath +
            //            "\\" + "Row" + rowIndex.ToString() + "-No" + i.ToString() + ".tiff");
            //    }
            //    //图片内存存在Vision.acq里，外存于指定目录imageFolder下时间文件夹内
            //    Acq.Dispose();
            //    return true;
            //}
            //else
            //{
            //    Acq.Dispose();
            //    return false;
            //}
        }

        /// <summary>
        /// 扫描二维码
        /// </summary>
        /// <param name="imgNum"></param>
        /// <returns></returns>
        public bool ScanCode(out string answer)
        {
            try
            {
                //Acquisition acq = camera.GetFrames(1, 5000);
                //BarcodeReader reader = null;
                //Bitmap btmp;
                //GenertateRGBBitmap(acq.Image, out btmp);
                //Result result = reader.Decode(btmp); //通过reader解码  
                //answer = result.Text; //显示解析结果 
                //acq.Dispose();
                answer = "123";
                return true;
            }
            catch
            {
                answer = "";
                return false;
            }
        }

        /// <summary>
        /// 采集单张或多张图片
        /// </summary>
        /// <param name="image"></param>
        /// <param name="imageNum"></param>
        /// <param name="timeOut"></param>
        /// <returns></returns>
        public string CaputreImages(ref HObject image, int imageNum, int timeOut)
        {
            string errStr = "";
            if (App.obj_Vision.RunMode == Convert.ToInt16(SystemRunMode.MODE_OFFLINE)) return errStr;
            try
            {
                if (camera.IsSoftwareTrigger) acq = camera.Snap(imageNum, timeOut);
                else acq = camera.GetFrames(imageNum, timeOut);
                if (acq.GrabStatus == "GrabPass")
                {
                    if (image == null || !image.IsInitialized())
                    {
                        image = acq.Image;
                    }
                    else
                    {
                        HOperatorSet.ConcatObj(image, acq.Image, out image);
                    }
                    //FormJobs.Instance.ShowImage(acq.Image, null);
                    //frmCaptureImage.Instance.ShowImage(frmCaptureImage.Instance.htHalconTestImage, acq.Image, null);
                }
                else
                {
                    errStr = "采图失败!详细信息:采图超时!";
                }
                //camera.ClearImageQueue();
            }
            catch (Exception ex)
            {
                errStr = string.Format("采图失败!详细信息:{0}!", ex.ToString());
            }
            return errStr;
        }
        #endregion
    }
}
