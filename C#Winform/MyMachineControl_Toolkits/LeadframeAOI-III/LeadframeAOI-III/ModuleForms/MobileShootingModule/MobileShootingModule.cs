/* 
 * 飞拍模块
 * byLL 2018/8/7
 */

using HalconDotNet;
using HT_Lib;
using IniDll;
using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using ToolKits.HTCamera;
using MobileShooting;
using System.Threading;

namespace LeadframeAOI
{
    /// <summary>
    /// 飞拍模块，使用时在App类建实例并初始化
    /// </summary>
    class MobileShootingModule : BaseModule
    {
        /// <summary>
        /// 线扫是否继续工作
        /// </summary>
        public static bool workFlag = true;
        public MobileShootingModule(String para_file, String para_table) : base(para_file, para_table) { }

        /// <summary>
        /// 一次轴运动线性触发次数
        /// </summary>
        public int LineAxisTrigCount;

        /// <summary>
        /// 非线性触发轴移动次数
        /// </summary>
        public int NoLineAxisMoveCount;

        /// <summary>
        /// 传入点位的行数
        /// </summary>
        public int rowCount;

        /// <summary>
        /// 传入点位的列数
        /// </summary>
        public int columnCount;

        private double SingleViewWidth;
        /// <summary>
        /// 单个相机视野宽度
        /// </summary>
        private bool IsGettingImage = false;

        /// <summary>
        /// 单个相机视野高度
        /// </summary>
        private double SingleViewHeight;

        /// <summary>
        /// 单个芯片区域宽度
        /// </summary>
        private double SingleDieWidth;

        /// <summary>
        /// 单个芯片区域高度
        /// </summary>
        private double SingleDieHeight;

        /// <summary>
        /// 线性触发起始位置
        /// </summary>
        private double LineAxisTrigStart;

        /// <summary>
        /// 线性触发结束位置
        /// </summary>
        private double LineAxisTrigEnd;

        /// <summary>
        /// 线性触发间隔
        /// </summary>
        private double LineAxisTrigInterval;

        /// <summary>
        /// 非线性触发轴单次移动间隔
        /// </summary>
        private double NoLineAxisMoveInterval;

        /// <summary>
        /// 轴运动起始点坐标
        /// </summary>
        private PointD MoveStartPoint = new PointD();

        /// <summary>
        /// 轴运动终止点坐标
        /// </summary>
        private PointD MoveEndPoint = new PointD();

        /// <summary>
        /// 第一个视野的相机坐标
        /// </summary>
        private PointD FirstViewPoint = new PointD();

        /// <summary>
        /// 图像临时存放地址
        /// </summary>
        private string ImageTemporaryFolderPath;

        /// <summary>
        /// 整理后的图像存放地址
        /// </summary>
        private string ImageFolderPath;


        /// <summary>
        /// 所有芯片的理论信息矩阵
        /// </summary>
        private ImagePosition[,] dieMatrix;


        /// <summary>
        /// 初始化按行飞拍所需的参数
        /// 在Process.TaskChuck() App.obj_Chuck.CurrentModuleState == ModuleState,Chuck_Scan_Init中调用
        /// </summary>
        /// <param name="list_NominalDie">所有芯片理论信息</param>
        /// <param name="rowCount">行总数</param>
        /// <param name="columnCount">列总数</param>
        public void InitialScanRowParameters(List<ImagePosition> list_NominalDie, double ViewWidth, double ViewHeight, double DieWidth, double DieHeight,
                                             int rowCount,
                                             int columnCount)
        {

            this.rowCount = rowCount;
            this.columnCount = columnCount;
            dieMatrix = new ImagePosition[rowCount, columnCount];
            foreach (ImagePosition die in list_NominalDie)
            {
                dieMatrix[die.r, die.c] = die;
            }
            //拍摄范围，首尾两个芯片的中心
            double shootRangeWidth = Math.Abs(dieMatrix[0, 0].x - dieMatrix[rowCount - 1, columnCount - 1].x);
            double shootRangeHeight = Math.Abs(dieMatrix[0, 0].y - dieMatrix[rowCount - 1, columnCount - 1].y);

            //单个芯片区域大小
            SingleDieWidth = DieWidth;
            SingleDieHeight = DieHeight;
            //拍摄间隔，重叠一个芯片大小的范围
            SingleViewWidth = ViewWidth;
            SingleViewHeight = ViewHeight;
            LineAxisTrigInterval = SingleViewWidth - SingleDieWidth;
            //TODO:这样有个问题，如果相机拍摄范围的高度正好是一行，减掉后移动间隔将为0或者很小
            //     所以需要获取芯片本身的大小…
            NoLineAxisMoveInterval = SingleViewHeight;
            //计算触发轴拍照次数和非触发轴移动次数，向上取整
            LineAxisTrigCount = (int)Math.Ceiling((decimal)(shootRangeWidth / LineAxisTrigInterval));
            NoLineAxisMoveCount = rowCount;
            //第一个视野的坐标
            FirstViewPoint.X = dieMatrix[0, 0].x;
            FirstViewPoint.Y = dieMatrix[0, 0].y;
            LineAxisTrigStart = FirstViewPoint.X;
            LineAxisTrigEnd = LineAxisTrigStart + (LineAxisTrigCount - 1) * LineAxisTrigInterval;
            //轴运动起始点向外偏移1个视野
            MoveStartPoint.X = FirstViewPoint.X - SingleViewWidth;
            MoveStartPoint.Y = FirstViewPoint.Y;
            MoveEndPoint.X = LineAxisTrigEnd + SingleViewWidth;
            MoveEndPoint.Y = FirstViewPoint.Y;

            //区域照片临时存放的文件夹，这里应该使用Vision类的文件夹
            ImageTemporaryFolderPath = Application.StartupPath + "\\Image\\Temp";
            //整理后的图片存放文件夹，这里应该使用Vision类的文件夹
            ImageFolderPath = Application.StartupPath + "\\Image";


            //返回后在Process中调用下面一句，进入扫描行任务
            //App.obj_Chuck.CurrentModuleState = ModuleState.Chuck_ScanRow;
        }

        /// <summary>
        /// 开始按行飞拍
        /// 在Process.TaskChuck() App.obj_Chuck.CurrentModuleState == ModuleState,Chuck_ScanRow中调用
        /// </summary>
        /// <param name="camera">飞拍使用的相机</param>
        /// <param name="lineTrigAxisIndex">触发轴序号，这里是相机X轴</param>
        /// <param name="noLineTrigAxisIndex">非触发轴序号，这里是相机Y轴</param>
        /// <returns></returns>
        public bool StartMobileShooting_X(Obj_Camera camera, int lineTrigAxisIndex, int noLineTrigAxisIndex)
        {
            workFlag = true;
            IsGettingImage = false;
            if (!ASMoveAxis(lineTrigAxisIndex, noLineTrigAxisIndex, MoveStartPoint.X, MoveStartPoint.Y))
            {
                return false;
            }
            for (int i = 0; i < NoLineAxisMoveCount; i++)
            {
                if (!workFlag)
                {
                    workFlag = true;
                    return true;
                }
                //5、移动到当前行
                if (i != 0)
                {
                    if (!ASMoveSingleAxis(noLineTrigAxisIndex, i%2==0?dieMatrix[i, 0].y: dieMatrix[i, columnCount - 1].y))
                    {
                        return false;
                    }
                }
                //互换数，用于交换起点和终点，i为偶数时等0，i为奇数时等1
                //当前行触发起点
                double currentTrigStart = i % 2 == 0 ? dieMatrix[i, 0].x : dieMatrix[i, columnCount - 1].x;
                //当前行触发终点
                double currentTrigEnd = i % 2 == 1 ? dieMatrix[i, 0].x : dieMatrix[i, columnCount - 1].x;
                //当前行移动终点
                double currentMoveEndX = i % 2 == 1 ? dieMatrix[i, 0].x - SingleViewWidth : dieMatrix[i, columnCount - 1].x + SingleViewWidth;
                //当前行触发起点
                //double currentTrigStart =  LineAxisTrigStart;
                ////当前行触发终点
                //double currentTrigEnd = LineAxisTrigEnd;
                ////当前行移动终点
                //double currentMoveEndX =MoveEndPoint.X;
                //1、清空相机缓存
                ClearCameraBuffer(camera);
                //2、设置触发参数
                if (!SetTriggerParameterAndEnableTrig(lineTrigAxisIndex,
                                                      currentTrigStart,
                                                      currentTrigEnd,
                                                      LineAxisTrigInterval))
                {
                    return false;
                }

                //3、获取照片并处理，异步进行
                
                //if (!GetImages(camera, lineTrigAxisIndex, i))
                //{
                //    return false;
                //}
                Task<bool> task = new Task<bool>(() => GetImages(camera, lineTrigAxisIndex, i));
                DateTime t = DateTime.Now;
                task.Start();
                //等待异步结果
                //4、移动线性触发轴
                if (!ASMoveAxis(lineTrigAxisIndex, noLineTrigAxisIndex, currentMoveEndX, i % 2 == 1 ? dieMatrix[i, 0].y : dieMatrix[i, columnCount - 1].y, true))
                {
                    return false;
                }
                //HTLog.Info("运动用毫秒:"+ DateTime.Now.Subtract(t).TotalMilliseconds);
                task.Wait();
                //HTLog.Info("拍照用毫秒:" + DateTime.Now.Subtract(t).TotalMilliseconds);
                if (!task.Result)
                {
                    return false;
                }
                //6、图像检测
                //App.obj_Vision.VisionState = VisionState.ScanRow;

            }
            ////整体处理图片，存放位置
            //Task<bool> t = new Task<bool>(() => SortImages());
            //t.Start();

            ////返回初始位置
            //if (!ASMoveAxis(lineTrigAxisIndex, noLineTrigAxisIndex, MoveStartPoint.X, MoveStartPoint.Y))
            //{
            //    return false;
            //}



            //返回后在Process中调用下面一句，进入下架任务
            //App.obj_Chuck.CurrentModuleState = ModuleState.Chuck_Unload;
            return true;
        }

        /// <summary>
        /// 清空相机缓存
        /// </summary>
        private void ClearCameraBuffer(Obj_Camera camera)
        {
            camera.acq.Dispose();
            camera.Camera.ClearImageQueue();
        }

        /// <summary>
        /// 设置触发参数,开启触发板
        /// </summary>
        /// <param name="lineTrigAxisIndex">触发轴序号</param>
        /// <param name="startPos">触发起始点</param>
        /// <param name="endPos">触发终止点</param>
        /// <param name="trigInterval">触发距离间隔</param>
        public bool SetTriggerParameterAndEnableTrig(int lineTrigAxisIndex,
                                                     double startPos,
                                                     double endPos,
                                                     double trigInterval)
        {
            HTM.TRIG_LINEAR linear = new HTM.TRIG_LINEAR
            {
                startPos = startPos,
                endPos = endPos,
                interval = trigInterval
            };
            if (HTM.SetLinTrigPos(lineTrigAxisIndex, ref linear) != 0)
            {
                errString = String.Format("触发板设置触发点位失败");
                return false;
            }
            if (HTM.SetLinTrigEnable(lineTrigAxisIndex, 1) != 0)
            {
                errString = String.Format("触发板开启失败");
                return false;
            }
            return true;
        }

        /// <summary>
        /// 单个轴绝对运动
        /// </summary>
        /// <param name="axisIndex">轴序号</param>
        /// <param name="position">目标坐标</param>
        private bool ASMoveSingleAxis(int axisIndex, double position, bool isTrig = false)
        {
            if (RunMode == (int)SystemRunMode.MODE_OFFLINE) return true;
            if (!App.obj_Chuck.Ready)
            {
                errString = "Chuck模块未初始化！";
                return false;
            }
            HTM.MOTION_PARA mp = HTM.GetMotionPara(axisIndex);
            if (isTrig)
            {
                int Fps = App.obj_Chuck.Fps;
                double maxSpeed = LineAxisTrigInterval * Fps;
                mp.vMax = maxSpeed;
            }
            //运动
            if ((errCode = HTM.Move(axisIndex, position, speed, HTM.MotionMode.AS, ref mp)) < 0)
            {
                errString = "发送轴运动指令失败";
                return false;
            }
            if ((errCode = HTM.Done(axisIndex)) < 0)
            {
                errString = "Chuck模块轴运动失败！";
                return false;
            }
            return true;
        }
        /// <summary>
        /// 双个轴绝对运动
        /// </summary>
        /// <param name="axisIndex1_trig">轴序号</param>
        /// <param name="position1">目标坐标</param>
        private bool ASMoveAxis(int axisIndex1_trig, int axisIndex2, double position1, double position2, bool isTrig = false)
        {
            if (RunMode == (int)SystemRunMode.MODE_OFFLINE) return true;
            if (!App.obj_Chuck.Ready)
            {
                errString = "Chuck模块未初始化！";
                return false;
            }
            HTM.MOTION_PARA mp = HTM.GetMotionPara(axisIndex1_trig);
            if (isTrig)
            {
                int Fps = App.obj_Chuck.Fps;
                double maxSpeed = LineAxisTrigInterval * Fps;
                mp.vMax = maxSpeed;
            }//运动
            if ((errCode = HTM.Move(axisIndex1_trig, position1, speed, HTM.MotionMode.AS, ref mp)) < 0)
            {
                errString = "发送轴运动指令失败";
                return false;
            }
            if ((errCode = HTM.Move(axisIndex2, position2, speed, HTM.MotionMode.AS)) < 0)
            {
                errString = "发送轴运动指令失败";
                return false;
            }
            if ((errCode = HTM.Done(axisIndex1_trig)) < 0)
            {
                errString = "Chuck模块轴运动失败！";
                return false;
            }
            if ((errCode = HTM.Done(axisIndex2)) < 0)
            {
                errString = "Chuck模块轴运动失败！";
                return false;
            }
            return true;
        }
        /// <summary>
        /// 从相机获取图像，并存入临时文件夹
        /// </summary>
        /// <param name="camera">相机</param>
        /// <param name="viewRowIndex">当前拍摄行编号</param>
        private bool GetImages(Obj_Camera camera, int lineTrigAxisIdx, int viewRowIndex)
        {
            Acquisition anquistion = camera.Camera.GetFrames(LineAxisTrigCount, 5000);
            if (anquistion.index == -1)
            {
                errString = "连续采图未能获取到图像";
                return false;
            }
            int TrigCnt = HTM.GetTrigCnt(lineTrigAxisIdx);

            //if (anquistion.index + 1 != TrigCnt || TrigCnt != LineAxisTrigCount)
            if (anquistion.index + 1 != TrigCnt)
            {
                errString = "连续采图丢帧：触发" + TrigCnt + "得图" + (anquistion.index + 1) + "张";
                return false;
            }
            if (!Directory.Exists(ImageTemporaryFolderPath))
            {
                Directory.CreateDirectory(ImageTemporaryFolderPath);
            }
            HObject singleImage = null;
            ImageCache imageCache = null;
            try
            {
                int a = LineAxisTrigCount;
                if (singleImage != null)
                {
                    singleImage.Dispose();
                    singleImage = null;
                }
                for (int i = 0; i < anquistion.index + 1; i++)
                {
                    imageCache = new ImageCache();
                    HOperatorSet.SelectObj(anquistion.Image, out imageCache._2dImage, i + 1);
                    //HOperatorSet.WriteImage(singleImage, "tiff", 0, "D:\\Row" + viewRowIndex + "\\" + i + ".tiff");
                    //图像按最新的顺序获取
                    //第1、3、5..行，列编号倒着算
                    //第0、2、4、6..行，列编号正着算
                    imageCache.c = viewRowIndex % 2 == 0 ? i : (LineAxisTrigCount - 1 - i);
                    imageCache.r = viewRowIndex;
                    imageCache.X = viewRowIndex % 2==0?dieMatrix[imageCache.r, 0].x  + i * LineAxisTrigInterval:
                        dieMatrix[imageCache.r, columnCount - 1].x - i * LineAxisTrigInterval;
                    double deltaY = (dieMatrix[imageCache.r, columnCount - 1].y - dieMatrix[imageCache.r, 0].y)/ (LineAxisTrigCount - 1 )* imageCache.c;
                    imageCache.Y = viewRowIndex % 2 == 0 ? dieMatrix[imageCache.r, 0].y+ deltaY: dieMatrix[imageCache.r, columnCount - 1].y-deltaY;
                    if (App.obj_Process.qAllImageCache != null) App.obj_Process.qAllImageCache.Enqueue(imageCache);
                }
                anquistion.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                errString = "图片存储发生错误 详细内容：" + System.Environment.NewLine + ex.ToString();
                return false;
            }
        }

    }
}
