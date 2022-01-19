using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolKits.HTCamera;
using HalconDotNet;
using System.IO;
using IniDll;
using System.Windows.Forms;
using HTHalControl;

namespace CameraZaxisScanModel
{
    public class Obj_Camera
    {
        /// <summary>
        /// 相机数目
        /// </summary>
        public static int Num_Camera=1;
        /// <summary>
        /// 当前被选择使用的相机，默认为0;
        /// </summary>
        public static int SelectedIndex=0;
        /// <summary>
        /// 相机对象
        /// </summary>
        public CameraMgr camera;
        /// <summary>
        /// 相机名
        /// </summary>
        public string cameraName;
        /// <summary>
        /// 相机类型
        /// </summary>
        public CameraEnum cameraType;
        /// <summary>
        /// 曝光值
        /// </summary>
        public Double exposure;
        /// <summary>
        /// 增益值
        /// </summary>
        public Double gain;
        /// <summary>
        /// 该相机若多次拍照，拍照次数
        /// </summary>
        public int num_catch;
        /// <summary>
        /// 该相机若多次拍照，各次拍照曝光值
        /// </summary>
        public Double[] Cur_Exposure;
        /// <summary>
        /// 该相机若多次拍照，各次拍照增益值
        /// </summary>
        public Double[] Cur_Gain;
        /// <summary>
        /// 相机所拍图像内存对象
        /// </summary>
        public Acquisition acq;
        /// <summary>
        /// 相机图像存储目录
        /// </summary>
        public string cameraPath;
        /// <summary>
        /// 相机是否正在连续采集
        /// </summary>
        public bool isGrab;

        public HObject ImageIC, ImagePCB, ImageLine;
        public HTWindowControl htWindow = null;
        public Obj_Camera()
        {
            this.camera = null;
            this.cameraName = "";
            this.cameraType = 0;
            this.exposure = 40;
            this.gain = 0;
            this.num_catch = 3;
            this.Cur_Exposure = new Double[3] { 400, 40, 40 };
            this.Cur_Gain = new Double[3] { 0, 0, 0 };
            this.acq = new Acquisition();
            this.cameraPath = "";
            this.isGrab = false;
            this.htWindow = null;
        }

        public Obj_Camera(HTWindowControl htWindow)
        {
            this.camera = null;
            this.cameraName = "";
            this.cameraType = 0;
            this.exposure = 40;
            this.gain = 0;
            this.num_catch = 3;
            this.Cur_Exposure =new Double[3] { 400, 40, 40 };
            this.Cur_Gain = new Double[3] { 0, 0, 0 };
            this.acq = new Acquisition();
            this.cameraPath = "";
            this.isGrab = false;
            this.htWindow = htWindow;
        }

        /// <summary>
        /// 初始化图像内存对象，相机，并打开相机
        /// </summary>
        public void Initialize()
        {
            acq = new Acquisition();
            if (!InitCamera())
            {
                MessageBox.Show("初始化相机失败！");
                return;
            }
            if (!OpenCamera())
            {
                MessageBox.Show("打开相机失败！");
                return;
            }
        }

        
        /// <summary>
        /// 初始化相机
        /// </summary>
        /// <returns></returns>
        public Boolean InitCamera()
        {
            //初始化相机
            try
            {
                switch (cameraType)
                {
                    case CameraEnum.MultiCamToHalcon://Euresys采集卡相机
                        camera = new CameraMgr(cameraType, cameraName);
                        camera.SetPixelFormat(ToolKits.HTCamera.EuresysPixelFormat.Y8);
                        return camera.InitCamera();
                    default: 
                        camera = new CameraMgr(cameraType, cameraName);
                        return camera.InitCamera();
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
            try
            {
                if (!camera.OpenCamera()) return false;
                GetExposure(out exposure);
                GetGain(out gain);
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 关闭相机
        /// </summary>
        /// <returns></returns>
        public Boolean CloseCamera()
        {
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
            return camera.SetExposure(value);
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
        public Boolean ScanPoint(int rowIndex, int columnIndex, int imageIndex = 0, int imageNum = 1, string visionPath="", int timeOut = -1)
        {
            cameraPath = visionPath;
            int imgIndex = imageIndex;
            if (camera.IsSoftwareTrigger) acq = camera.Snap(imageNum, timeOut);
            else acq = camera.GetFrames(imageNum, timeOut);
            if (acq.GrabStatus == "GrabPass")
            {
                //新建时间文件夹存图
                string sPath = "";
                //    + "\\" + DateTime.Now.ToString("yyyy-MM-dd") + "-"
                //    + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString();
                if (!Directory.Exists(cameraPath))
                {
                    Directory.CreateDirectory(cameraPath);
                }
                HObject ImageSrc = new HObject();
                for (int i = 0; i < imageNum; i++)
                {
                    //从acq里取图并保存图片，行号-列号.tiff  
                    HOperatorSet.SelectObj(acq.Image, out ImageSrc, i + 1);
                    Program.mainWindow.ShowImage(Program.mainWindow.htWindow, ImageSrc, null);
                    if (imgIndex == 0) ImageIC = acq.Image.CopyObj(1, -1);
                    else if (imgIndex == 1) ImagePCB = acq.Image.CopyObj(1, -1);
                    else if (imgIndex == 2) ImageLine = acq.Image.CopyObj(1, -1);
                    //GetExposure(out exposure);
                    //GetGain(out gain);
                    sPath = cameraPath + "\\" + rowIndex.ToString() + "-" + columnIndex.ToString();
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
    }
}
