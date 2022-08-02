using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalconDotNet;
using ToolKits.HTCamera;
using MathNet.Numerics.LinearAlgebra;
using System.Threading;
using HT_Lib;
using Utils;
using System.Reflection;
using System.ComponentModel;
using System.Data.SQLite;
using System.IO;



namespace LeadframeAOI
{
    public class Show
    {
        public HObject Image;
        public int CamInd;
        /// <summary>
        /// 0表示仅显示图像，1表示自动聚焦,2表示匹配
        /// </summary>
        public int Mode;
        /// <summary>
        /// 映射点中心行坐标
        /// </summary>
        public double Row;
        /// <summary>
        /// 映射点中心列坐标
        /// </summary>
        public double Column;

        public Show()
        {
            Row = 0;
            Column = 0;
        }
        public void Dispose()
        {
            if (Image != null) Image.Dispose();
        }
    }

    /// <summary>
    /// 多颗die外观检测方法，多颗die点位标定，连续拍照方法
    /// </summary>
    public class Operations : Base
    {
        /// <summary>
        /// UV2XY模版存放路径
        /// </summary>
        private string _calibrUV2XYModelPath = string.Empty;

        //RC2XY模版存放路径
        private string _calibRC2XYModelPath = @"D:\Models";

        //标定图像存放目录
        private string _calibImagePath = string.Empty;
        private static string _calibrationUV2XYParameter = string.Empty;
        private string _calibrationRC2XYParameter = string.Empty;
        public Model modelCalibration;
        private SQLiteConnection sqlCon;    //连接

        #region 第一个Block三个角的坐标
        //RC2XY用到的参数
        private Double xcoordsTopLeft;
        private Double ycoordsTopLeft;
        private Double rowTopLeft;
        private Double columnTopLeft;

        private Double xcoordsBottomRight;
        private Double ycoordsBottomRight;
        private Double rowBottomRight;
        private Double columnBottomRight;

        private Double xcoordsTopRight;
        private Double ycoordsTopRight;
        #endregion
        private Double _zCalib;  //产品标定时的Z轴聚焦位
        private Double _m = 0;   //视野中 row number
        private Double _n = 0;   //视野中 column number
        private HTuple _width = new HTuple();
        private HTuple _height = new HTuple();
        /// <summary>
        /// Uv2XY标定结果
        /// </summary>
        public HTuple MatUV2XY = new HTuple();
        private HTuple _rcHxy = new HTuple();



        private Double rowTopRight;
        private Double columnTopRight;
        private Double xStep;
        private Double yStep;
        private Double _xBlock1TopLeft; //第一个Bolck左上角X
        private Double _yBlock1TopLeft;//第一个Block左上角
        private double _xUv2xy;

        /// <summary>
        /// 连续拍照图像是否保存
        /// </summary>
        private Boolean _liningTrigImagesSaving = false;

        public Double ZCalib
        {
            get { return _zCalib; }
            set { _zCalib = value; }
        }
        public double XUv2xy
        {
            get { return _xUv2xy; }
            set { _xUv2xy = value; }
        }
        private double _yUv2xy;

        public double YUv2xy
        {
            get { return _yUv2xy; }
            set { _yUv2xy = value; }
        }
        private double _zUv2xy;

        public double ZUv2xy
        {
            get { return _zUv2xy; }
            set { _zUv2xy = value; }
        }

        public HTuple RcHxy
        {
            get { return _rcHxy; }
            set { _rcHxy = value; }
        }

        List<Double[]> points = new List<Double[]>();

        List<Model> models = new List<Model>();

        [CategoryAttribute("左上角标记点"), DisplayNameAttribute("X坐标")]
        public double XcoordsTopLeft
        {
            get { return xcoordsTopLeft; }
            set
            {
                xcoordsTopLeft = value;
            }
        }
        [CategoryAttribute("左上角标记点"), DisplayNameAttribute("Y坐标")]
        public double YcoordsTopLeft
        {
            get { return ycoordsTopLeft; }
            set
            {
                ycoordsTopLeft = value;
            }
        }

        [CategoryAttribute("左上角标记点"), DisplayNameAttribute("行坐标")]
        public double RowTopLeft
        {
            get { return rowTopLeft; }
            set
            {
                rowTopLeft = value;
            }
        }

        [CategoryAttribute("左上角标记点"), DisplayNameAttribute("列坐标")]
        public double ColumnTopLeft
        {
            get { return columnTopLeft; }
            set
            {
                columnTopLeft = value;
            }
        }

        [CategoryAttribute("右下角标记点"), DisplayNameAttribute("X坐标")]
        public double XcoordsBottomRight
        {
            get { return xcoordsBottomRight; }
            set
            {
                xcoordsBottomRight = value;
            }
        }

        [CategoryAttribute("右下角标记点"), DisplayNameAttribute("Y坐标")]
        public double YcoordsBottomRight
        {
            get { return ycoordsBottomRight; }
            set
            {
                ycoordsBottomRight = value;
            }
        }
        [CategoryAttribute("右下角标记点"), DisplayNameAttribute("行坐标")]

        public double RowBottomRight
        {
            get { return rowBottomRight; }
            set
            {
                rowBottomRight = value;
            }
        }
        [CategoryAttribute("右下角标记点"), DisplayName("列坐标")]

        public double ColumnBottomRight
        {
            get { return columnBottomRight; }
            set
            {
                columnBottomRight = value;
            }
        }
        [CategoryAttribute("右上角标记点"), DisplayNameAttribute("X坐标")]

        public double XcoordsTopRight
        {
            get { return xcoordsTopRight; }
            set
            {
                xcoordsTopRight = value;
            }
        }
        [CategoryAttribute("右上角标记点"), DisplayNameAttribute("Y坐标")]

        public double YcoordsTopRight
        {
            get { return ycoordsTopRight; }
            set
            {
                ycoordsTopRight = value;
            }
        }
        [CategoryAttribute("右上角标记点"), DisplayNameAttribute("行坐标")]

        public double RowTopRight
        {
            get { return rowTopRight; }
            set
            {
                rowTopRight = value;
            }
        }
        [CategoryAttribute("右上角标记点"), DisplayNameAttribute("列坐标")]

        public double ColumnTopRight
        {
            get { return columnTopRight; }
            set
            {
                columnTopRight = value;
            }
        }

        [CategoryAttribute("Block1左上角标记点"), DisplayNameAttribute("X坐标")]
        public double XBlock1TopLeft
        {
            get { return _xBlock1TopLeft; }
            set
            {
                _xBlock1TopLeft = value;
            }
        }
        [CategoryAttribute("Block1左上角标记点"), DisplayNameAttribute("Y坐标")]
        public double YBlock1TopLeft
        {
            get
            {
                return _yBlock1TopLeft;
            }
            set
            {
                _yBlock1TopLeft = value;
            }
        }
        [BrowsableAttribute(false)]

        public string CalibrUV2XYModelPath
        {
            get { return _calibrUV2XYModelPath; }
            set
            {
                _calibrUV2XYModelPath = value;
            }
        }
        [BrowsableAttribute(false)]

        public string CalibRC2XYModelPath
        {
            get { return _calibRC2XYModelPath; }
            set
            {
                _calibRC2XYModelPath = value;
            }
        }
        [BrowsableAttribute(false)]
        public static string CalibrationUV2XYParameter
        {
            get
            {
                return _calibrationUV2XYParameter;
            }
            set
            {
                _calibrationUV2XYParameter = value;
            }
        }

        [BrowsableAttribute(false)]
        public string CalibrationRC2XYParameter
        {
            get
            {
                return _calibrationRC2XYParameter;
            }
            set
            {
                _calibrationRC2XYParameter = value;
            }
        }

        [BrowsableAttribute(false)]
        public Double M
        {
            get { return _m; }
            set { _m = value; }
        }
        [BrowsableAttribute(false)]
        public Double N
        {
            get { return _n; }
            set { _n = value; }
        }
        [BrowsableAttribute(false)]
        public string CalibImagePath
        {
            get { return _calibImagePath; }
            set { _calibImagePath = value; }
        }

        public bool LiningTrigImagesSaving { get { return _liningTrigImagesSaving; } set { _liningTrigImagesSaving = value; } }

        public event EventHandler<Show> GrabImageDoneHandler;
        public Operations(String para_file, String para_table) : base(para_file, para_table) { }
        /// <summary>
        /// 已测试
        /// 软触发拍照下，计算相机的uv->xy坐标系变换的操作，如large相机
        /// 要求之前设置好相机/光源/触发至“合理”状态
        /// [x]   =   [ H1  H2  H3  ]    [u]
        /// [y]   =   [ H4  H5  H6  ] *  [v]
        ///                              [1]
        /// </summary>
        /// <param name="matUV2XY">uv->xy变换矩阵，将2*3的矩阵按照行堆叠的方式存储为1*6的数组, ref传递方式，需要数组有一个初始值</param>
        /// <param name="cam">相机控制类，注意曝光时间需要设置合适。软触发拍照建议曝光时间在100ms左右</param>
        /// <param name="modelID">匹配的ncc模板</param>
        /// <param name="score_thresh">匹配分数，需要大于0.5</param>
        /// <param name="axisPara">运动轴信息（xy插补），具体包括轴速，加速度</param>
        /// <param name="initPoint">初始点，中间仅适用了xy信息，需要保证走到初始点时室内内包含完整的，可匹配的model</param>
        /// <param name="lightInd">软触发拍照对应的光源id，需要事先设置好对应光源合适的时间</param>
        /// <param name="axisInd">x，y轴号</param>
        /// <param name="xyRange">计算变换时，以初始点为中心，随机走位的范围，单位mm</param>
        /// <param name="nPoints">随机走位多少个点</param>
        /// <param name="lightDelay">软触发拍照光源的延迟，单位us，典型值10us</param>
        /// <param name="timeOut">单次运动的延迟，单位ms</param>
        /// <param name="winID">(调试用）显示窗口ID</param>
        /// <returns>操作是否成功</returns>
        public bool calibUV2XY
                (
                int camInd,
                ref HTuple matUV2XY,
                Model model,
                double xyRange,
                int nPoints = 20
                )

        {
            //if (model.ReadModel(CalibrUV2XYModelPath) == false)
            //{
            //    HTUi.PopError("加载标定模版失败！");
            //    return false;
            //}
            if (App.obj_Chuck.XYZ_Move(_xUv2xy, _yUv2xy, _zUv2xy) == false)
            {
                HTUi.PopError("无法移动至标定点位置！");
                return false;
            }
            Thread.Sleep(200);
            //  App.obj_light.FlashMultiLight(LightUseFor.ScanPoint1st);
            App.obj_Vision.obj_camera[camInd].Camera.ClearImageQueue();
            App.obj_Chuck.SWPosTrig();//位置触发
            Thread.Sleep(10);
            HObject image;
            HOperatorSet.GenEmptyObj(out image);
            string ret = App.obj_Vision.obj_camera[camInd].CaputreImages(ref image, 1, 1000);//获取图片
            if (ret != "")
            {
                HTUi.PopError("采集图像失败！");
                return false;
            }
            //3. match
            HTuple u, v, angle;
            bool status = matchModel(camInd, ref image, model, out u, out v, out angle);
            HObject showRegion = new HObject();
            showRegion.Dispose();
            HOperatorSet.GenCrossContourXld(out showRegion, u, v, 512, 0);
            FormUV2XY.Instance.ShowImage(FormUV2XY.Instance.htWindowCalibration, image, showRegion);
            image.Dispose();
            if (!status)
            {
                HTUi.PopError("获取匹配初始位置图像失败");
                return false;

            }
            HTuple xArr = new HTuple(), yArr = new HTuple(), uArr = new HTuple(), vArr = new HTuple();
            Random rand = new Random();
            //4. for ... snap , match, add <u,v,x,y>
            for (int i = 0; i < nPoints; i++)
            {
                DateTime t1 = DateTime.Now;
                //大于或等于 0.0 且小于 1.0 的双精度浮点数
                double x = (rand.NextDouble() - 0.5) * xyRange + _xUv2xy;
                double y = (rand.NextDouble() - 0.5) * xyRange + _yUv2xy;
                if (App.obj_Chuck.XY_Move(x, y) == false)
                {
                    errString = "无法移动至标定点位置";
                    HTUi.PopError(errString);
                    return false;
                }
                Thread.Sleep(200); ;
                App.obj_Vision.obj_camera[camInd].Camera.ClearImageQueue();
                App.obj_Chuck.SWPosTrig();//位置触发
                Thread.Sleep(10);
                HOperatorSet.GenEmptyObj(out image);
                ret = App.obj_Vision.obj_camera[camInd].CaputreImages(ref image, 1, 1000);//获取图片
                if (ret != "")
                {
                    HTUi.PopError("采集图像失败！");
                    return false;
                }
                if (matchModel(camInd, ref image, model, out u, out v, out angle)) //found something
                {
                    xArr.Append(x); yArr.Append(y); uArr.Append(u); vArr.Append(v);
                }
                showRegion.Dispose();
                HOperatorSet.GenCrossContourXld(out showRegion, u, v, 512, 0);
                FormUV2XY.Instance.ShowImage(FormUV2XY.Instance.htWindowCalibration, image, showRegion);
                image.Dispose();

                //if (matchModel(acq1.Image, modelID, score_thresh, out u, out v, out angle)) //found something
                //{
                //    xArr.Append(x); yArr.Append(y); uArr.Append(u); vArr.Append(v);
                //}
            }
            if (xArr.Length < 10)
            {
                HTUi.PopError("有效点数不够");
                return false;
            }
            //5. least square estimation
            Matrix<double> In = Matrix<double>.Build.Dense(3, xArr.Length, 1.0); //by default In[2,:] = 1.0
            Matrix<double> Out = Matrix<double>.Build.Dense(2, xArr.Length);
            Out.SetRow(0, xArr.ToDArr());
            Out.SetRow(1, yArr.ToDArr());
            In.SetRow(0, uArr.ToDArr());
            In.SetRow(1, vArr.ToDArr());
            Matrix<double> A = vec2Mat(In, Out);
            //6. move to center of uv space
            double[] aArr = A.ToRowWiseArray(); //need to be tested
            _calibrationUV2XYParameter = string.Join(",", aArr.ToArray());
            //parse
            if (matUV2XY == null)
            {
                matUV2XY = new HTuple();
            }
            for (int i = 0; i < 6; i++)
            {
                matUV2XY.Append(aArr[i]);
            }
            //HTuple uvHxy = matUV2XY;
            //HTuple xyHuv = new HTuple();
            //HOperatorSet.HomMat2dInvert(uvHxy, out xyHuv);
            //HTuple uvHxy1 = new HTuple();
            //HOperatorSet.VectorToHomMat2d(uArr, vArr, xArr, yArr, out uvHxy1);
            //HTuple xyHuv1 = new HTuple();
            //HOperatorSet.VectorToHomMat2d(xArr, yArr, uArr, vArr, out xyHuv1);
            //HTuple _xyHuv1 = new HTuple();
            //HOperatorSet.HomMat2dInvert(uvHxy1, out _xyHuv1);

            //double d = 1.0 / (uvHxy[0] * uvHxy[4] - uvHxy[1] * uvHxy[3]);
            //double a0 = uvHxy[4] * d;
            //double a1 = -1 * uvHxy[1] * d;

            //double sx =1.0/( Math.Sqrt(Math.Pow(xyHuv[0], 2) + Math.Pow(xyHuv[3], 2)));

            return true;
        }

        /// <summary>
        /// 图像匹配函数，测试使用，可重新编写
        /// 只实现了简单ncc匹配，不支持roi内匹配，只能匹配一个目标
        /// </summary>
        /// <param name="image">输入图像</param>
        /// <param name="model">模型</param>
        /// <param name="u">返回匹配行坐标，pixel，如果失败为null</param>
        /// <param name="v">返回匹配列坐标，pixel，如果失败为null</param>
        /// <param name="angle">返回匹配角度，弧度，如果失败为null</param>
        /// <returns>是否匹配成功</returns>
        public bool matchModel(int camInd, ref HObject image, Model model, out HTuple u, out HTuple v, out HTuple angle)
        {
            //HTuple _u = new HTuple(), _v = new HTuple(), _angle = new HTuple();
            //bool matched =  matchModel(image, model.modelID, model.scoreThresh.D, out _u, out _v, out _angle);
            //u = _u; v = _v; angle = _angle;
            //return matched;
            u = null;
            v = null;
            angle = null;
            try
            {
                HTuple scores = new HTuple();
                // HTuple a = new HTuple(); HTuple b = new HTuple();

                HTuple us = new HTuple(), vs = new HTuple();
                HTuple score;
                HObject cross = null;
                HOperatorSet.GenEmptyObj(out cross);

                HObject updateShowCont = null;
                HOperatorSet.GenEmptyObj(out updateShowCont);
                HTuple updateDefRow = new HTuple(), updateDefCol = new HTuple(), hom2d, iFlag = new HTuple();
                HObject matchRegion = (model.matchRegion != null && model.matchRegion.IsInitialized()) ? model.matchRegion : image;
                //HOperatorSet.FindNccModel(image, modelID, -0.39, 0.78, 0.5, 1, 0.5, "true", 0, out u, out v, out angle, out score);
                //模版句柄
                for (int i = 0; i < model.modelID.Length; i++)
                {
                    updateShowCont.Dispose();

                    HOperatorSet.GenEmptyObj(out model.showContour);
                    //匹配起始角度为-1rad，范围为2rad,阈值为0.5
                    ToolKits.FunctionModule.Vision.find_model(image, matchRegion, model.showContour, out updateShowCont, model.modelType, model.modelID[i], -0.1, 0.2,
                       0.5, 1, model.defRows[i], model.defCols[i], out u, out v, out angle, out score, out updateDefRow, out updateDefCol,
                                        out hom2d, out iFlag);

                    if (iFlag.I == 0)
                    {
                        scores.Append(score);
                        us.Append(updateDefRow);
                        vs.Append(updateDefCol);
                    }
                    else
                    {
                        us.Append(0);
                        vs.Append(0);
                        scores.Append(0);
                        updateShowCont.Dispose();
                        cross.Dispose();
                        return false;
                    }
                }
                HTuple sortInd;
                //排数 从小到大
                HOperatorSet.TupleSortIndex(scores, out sortInd);
                //sortInd ？？？
                HTuple maxInd = sortInd[sortInd.Length - 1];
                score = scores[maxInd];
                bool status = false;
                //double
                if (score.D < 0.01)
                {
                    u = null;
                    v = null;
                    angle = null;
                    return false;
                }
                else
                {
                    u = us[maxInd];
                    v = vs[maxInd];
                    angle = 0;
                    status = true;
                }
                if (this.GrabImageDoneHandler != null && camInd >= 0)
                {
                    //Show _show = new Show();
                    //_show.CamInd = camInd;
                    //_show.Image = image.CopyObj(1, -1);
                    //_show.Mode = 2;
                    //_show.Row = status ? u.D : 0;
                    //_show.Column = status ? v.D : 0;
                    //;
                    //this.GrabImageDoneHandler(null, _show);
                }
                HOperatorSet.GenCrossContourXld(out cross, updateDefRow, updateDefCol, 300, 0);
                //App.mainWindow.ShowImage(frmCalibration.Instance.htWindowCalibration, image, cross);
                HObject ImageSrc = new HObject();
                //for (int i = 0; i < imageNum; i++)
                //{
                //    //从acq里取图并保存图片，行号-列号.tiff
                HOperatorSet.SelectObj(image, out ImageSrc, 1);
                ////frmCalibration.Instance.ShowImage(frmCalibration.Instance.htWindowCalibration, image, cross);
                FormUV2XY.Instance.ShowImage(FormUV2XY.Instance.htWindowCalibration, ImageSrc, cross);
                //HOperatorSet.WriteContourXldDxf(updateShowCont, "D://CONT");
                //htWindow.RefreshWindow(image, cross, "");
                cross.Dispose();
                updateShowCont.Dispose();
                return status;
            }
            catch (HalconException EXP)
            {
                string errMsg = EXP.Message;
                return false;
            }
        }
        /// <summary>
        /// 3个点及其以上经过测试，2个点情况尚未测试
        /// 通过最小二乘方法，计算输出点和输出点之间的映射关系
        /// least square estimate
        /// Out = A* In, solve A from (In,Out), where \hat A = Out*In.T*(In*In.T)^-1
        /// </summary>
        /// <param name="In">输入点，每一列都是一个独立点</param>
        /// <param name="Out">输出点，每一列都是一个独立点</param>
        /// <returns>转换矩阵</returns>
        public Matrix<double> vec2Mat(Matrix<double> In, Matrix<double> Out)
        {
            if (In.ColumnCount == 2)
            //[A] = a[cos(b), sin(b), tx; -sin(b), cos(b), ty]
            //convert from python:
            //dx = x[1]-x[0];dy = y[1]-y[0];dX = X[1]-X[0];dY = Y[1]-Y[0]
            //a = np.linalg.norm([dX, dY], 2)/(np.linalg.norm([dx,dy],2)+0.00000001)
            //b = -np.math.atan2(dY, dX) + np.math.atan2(dy,dx)
            //A11 = a*np.math.cos(b);A12 = a*np.math.sin(b);A21 = - A12;A22 = A11
            //tx = np.mean(X) - A11*np.mean(x) - A12*np.mean(y)
            //ty = np.mean(Y) - A21*np.mean(x) - A22*np.mean(y)
            //return np.array([[A11, A12, tx], [A21, A22, ty]])
            {
                double dx = In[0, 1] - In[0, 0];
                double dy = In[1, 1] - In[1, 0];
                double dX = Out[0, 1] - Out[0, 0];
                double dY = Out[1, 1] - Out[1, 0];
                double a = Math.Sqrt((dY * dY + dX * dX) / (dx * dx + dy * dy + 1e-10));
                double b = -Math.Atan2(dY, dX) + Math.Atan2(dy, dx);
                double A11 = a * Math.Cos(b); double A12 = a * Math.Sin(b); double A21 = -A12; double A22 = A11;
                double tx = (Out[0, 0] + Out[0, 1]) * 0.5 - A11 * (In[0, 0] + In[0, 1]) * 0.5 - A12 * (In[1, 0] + In[1, 1]) * 0.5;
                double ty = (Out[1, 0] + Out[1, 1]) * 0.5 - A21 * (In[0, 0] + In[0, 1]) * 0.5 - A22 * (In[1, 0] + In[1, 1]) * 0.5;
                return Matrix<double>.Build.DenseOfArray(new[,] { { A11, A12, tx }, { A21, A22, ty } });
            }
            else
            //[A] = [A, B, tx; C, D, ty]
            {
                return (Out * In.Transpose()) * (In * In.Transpose()).Inverse();
            }
        }

        /// <summary>
        /// 已知uv->xy矩阵，给定du，dv，计算相应的dx和dy
        /// </summary>
        /// <param name="uvHxy">1*6的数组</param>
        /// <param name="du">du（行），像素</param>
        /// <param name="dv">dv（列），像素</param>
        /// <param name="dx">dx，单位mm</param>
        /// <param name="dy">dy，单位mm</param>
        public void calDxDy(
                HTuple uvHxy,
                double du,
                double dv,
                out double dx,
                out double dy
            )
        //matUV2XY:  H = [H1,...,H6], mapping u,v,1 --> x,y
        //[dx]   =   [ H1  H2 ]    [du]
        //[dy]   =   [ H4  H5 ] *  [dv]
        //DxDy from u,v,1 or r,c,1 will be the same call
        {
            dx = uvHxy[0].D * du + uvHxy[1].D * dv;
            dy = uvHxy[3].D * du + uvHxy[4].D * dv;
        }

        /// <summary>
        /// 已测试，建议使用
        /// 标定rcHxyz的主要程序，可以选择是否需要大视野相机协助定位
        /// 如果选择大视野相机协助，则开始所有的points点，都会经过大视野相机的定位更新，然后再移动到2d相机内更新
        /// 如果不选择大视野相机协助，则所有的points点直接送入2d相机视野内搜索、匹配、匹配聚焦更新
        /// 注意，这里大视野相机更新的点，可以和2d相机更新的点选择不一样。大视野相机辅助定位是通过计算rcHxy的初始值，然后在2d相机细定位时，
        /// 推算出2d相机的更新点。
        /// </summary>
        /// <param name="useLargeCam">是否选择大视野相机协助，调试时候建议选择，如果wafer放置重复精度足够高可以不选，这样会更快</param>
        /// <param name="useLarge2Pts">是否使用大视野两点定位</param>
        /// <param name="initMarkPointsLarge">初始更新后的大视野定位点信息</param>
        /// <param name="rcHxyz">ref，rcHxyz结果，1*9数组，对应3*3矩阵（行堆叠）</param>
        /// <param name="markPoints2d">2d相机内需要更新的点</param>
        /// <param name="markPointsLarge">large相机内需要更新的点</param>
        /// <param name="cam2d">2d相机控制</param>
        /// <param name="camLarge">large相机控制</param>
        /// <param name="models2d">2d模型，List，每个元素对应相应点匹配需要的模型</param>
        /// <param name="modelsLarge">large模型，List，每个元素对应相应点匹配需要的模型</param>
        /// <param name="xyAxisPara">xy运动轴参数</param>
        /// <param name="zAxisPara">z运动轴参数</param>
        /// <param name="focusPara">自动聚焦参数</param>
        /// <param name="uvHxy2d">2d相机的uvHxy矩阵，1*6</param>
        /// <param name="uvHxyLarge">large相机的uvHxy矩阵，1*6</param>
        /// <param name="xyCenter2d">1*2的double数组，标定板在2d相机视野中心对应的xy坐标</param>
        /// <param name="xyCenterLarge">1*2的large数组，标定板在2d相机视野中心对应的xy坐标</param>
        /// <param name="winID">调试时，显示的窗口ID</param>
        /// <param name="pixelSpan2d">2d相机搜索是的搜索步长，像素，建议设置为FOV的1/4</param>
        /// <param name="pixelSpanLarge">large相机搜索是的搜索步长，像素，建议设置为FOV的1/4</param>
        /// <param name="motionTimeOut">运动延迟，ms，默认5000</param>
        /// <param name="lightDelay">large相机软触发拍照的光源延迟，ms</param>
        /// <param name="maxSteps2d">2d相机搜索的最大次数，不超过121</param>
        /// <param name="maxStepsLarge">large相机搜索的最大次数，不超过121</param>
        /// <param name="xAxisID">x轴ID，默认0</param>
        /// <param name="yAxisID">y轴ID，默认1</param>
        /// <param name="zAxisID">2d的z轴ID，默认2</param>
        /// <param name="triggerID2d">2d的拍照触发ID，默认1</param>
        /// <param name="lightIDLarge">大视野相机光源ID，默认18</param>
        /// <returns>操作是否成功</returns>
        public Boolean calibrationRC2XY(Boolean newProd = false)
        {
            try
            {
                models.Clear();
                if (loadModels(CalibRC2XYModelPath) == false)
                {
                    return false;
                }
                XcoordsTopLeft = 323.339;
                YcoordsTopLeft = -28.91;

                XcoordsTopRight = 276.537;
                YcoordsTopRight = -29.263;

                XcoordsBottomRight = 275.537;
                YcoordsBottomRight = -86.618;

                _xBlock1TopLeft = 262.537;
                _yBlock1TopLeft = -29.263;

                points.Clear();

                if (MoveTakePicture(0, CalibRC2XYModelPath, XcoordsTopLeft, YcoordsTopLeft, models[0], 0, 0) == false)
                {
                    return false;
                }
                if (MoveTakePicture(0, CalibRC2XYModelPath, XcoordsTopRight, YcoordsTopRight, models[1], 0, 8) == false)
                {
                    return false;

                }
                if (MoveTakePicture(0, CalibRC2XYModelPath, XcoordsBottomRight, YcoordsBottomRight, models[2], 10, 8) == false)
                {
                    return false;
                }
                if (newProd == true)
                {
                    if (MoveTakePicture(0, CalibRC2XYModelPath, _xBlock1TopLeft, _yBlock1TopLeft, models[0], 0, 0) == false)
                    {
                        return false;
                    }
                }
                calRC2XY(points);
                return true;
            }
            catch (Exception EXP)
            {
                HTLog.Error(EXP.Message);
                return false;
            }

            //loadModels(CalibRC2XYModelPath);
            //MoveTakePicture(1,CalibRC2XYModelPath,XcoordsTopLeft,YcoordsTopLeft,models[0],RowTopLeft,ColumnTopLeft);
            //MoveTakePicture(1, CalibRC2XYModelPath, XcoordsTopRight, YcoordsTopRight, models[1], RowTopRight, ColumnTopRight);
            //MoveTakePicture(1, CalibRC2XYModelPath, XcoordsBottomRight, YcoordsBottomRight, models[2], RowBottomRight, ColumnBottomRight);
            //return calRC2XY(points);
        }
        /// <summary>
        /// 加载所有模版
        /// </summary>
        /// <param name="modelPath">模版上一层文件夹</param>
        /// <returns></returns>
        private Boolean loadModels(string modelPath)
        {
            HTuple Dirs = new HTuple();
            HOperatorSet.ListFiles(modelPath, "directories", out Dirs);
            //顺序 左上、右上、右下、左下
            for (int i = 0; i < Dirs.Length; i++)
            {
                models.Add(new Model());
                if (models[i].ReadModel(Dirs[i].S) == false)
                {
                    HTUi.PopError("加载标定模版失败！");
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="camInd"></param>
        /// <param name="modelPath">模版文件上层文件夹</param>
        /// <param name="xCoords"></param>
        /// <param name="yCoords"></param>
        /// <returns></returns>
        public Boolean MoveTakePicture(Int32 camInd, HTuple modelPath, Double xCoords, Double yCoords, Model model, Double row, Double column)
        {
            if (App.obj_Chuck.XY_Move(xCoords, yCoords) == false)
            {
                HTUi.PopError("无法移动至标定点位置！");
                return false;
            }
            if (App.obj_Chuck.XYZ_Move(xCoords, yCoords, this._zCalib) == false)
            {
                HTUi.PopError("无法移动至标定点位置！");
                return false;
            }
            Thread.Sleep(200);
            // App.obj_light.FlashMultiLight(LightUseFor.ScanPoint1st);
            App.obj_Chuck.SWPosTrig();//位置软触发触发
            Thread.Sleep(10);

            //拍照
            if (App.obj_Vision.obj_camera[camInd].ScanCalibrationPoint(xCoords, yCoords, 1, 1, 200) == false)
            {
                HTUi.PopError("拍照存图异常");
                App.obj_Vision.Acq.Dispose();
                App.obj_Vision.obj_camera[camInd].acq.Dispose();
                return false;
            }
            App.obj_Vision.Acq = App.obj_Vision.obj_camera[camInd].acq;
            HTuple width, height;
            HOperatorSet.GetImageSize(App.obj_Vision.Acq.Image, out width, out height);
            _width = width;
            _height = height;
            double uCenter;
            double vCenter;
            uCenter = (height.D - 1) * 0.5;
            vCenter = (width.D - 1) * 0.5;
            HTuple u, v, angle;
            bool status = matchModel(camInd, ref App.obj_Vision.Acq.Image, model, out u, out v, out angle); //默认只找到一个？
            if (status == false)
            {
                HTUi.PopError("无法匹配模版");
                App.obj_Vision.Acq.Dispose();
                return false;
            }
            //double delta_u = uCenter - u.D;
            //double delta_v = vCenter - v.D;
            double delta_u = -u.D + uCenter;
            double delta_v = vCenter - v.D;
            double delta_x, delta_y;
            //HTuple mat2xy;
            calDxDy(MatUV2XY, delta_u, delta_v, out delta_x, out delta_y);
            Double[] markPoint = new double[4];
            markPoint[0] = xCoords + delta_x;
            markPoint[1] = yCoords + delta_y;
            markPoint[2] = row;
            markPoint[3] = column;

            points.Add(markPoint);
            App.obj_Vision.Acq.Dispose();
            App.obj_Vision.obj_camera[camInd].acq.Dispose();

            if (App.obj_Chuck.XY_Move(markPoint[0], markPoint[1]) == false)
            {
                HTUi.PopError("无法移动至标定点位置！");
                return false;
            }
            return true;
        }

        public void CalculateBlockDistance(Double deltaColumnBlock, out Double xDeltaBlock, out Double yDeltaBlock)
        {
            xDeltaBlock = _rcHxy[1] * deltaColumnBlock;
            yDeltaBlock = _rcHxy[4] * deltaColumnBlock;
        }

        /// <summary>
        /// 将Block间距mm换算为列数
        /// </summary>
        /// <param name="_deltaColumnBlock"></param>
        public void CalculateDeltaBlockCol(out Double _deltaColumnBlock)
        {
            Matrix<double> J = Matrix<double>.Build.Dense(3, 3, 1.0);
            J.At(0, 0, _rcHxy[0]);
            J.At(0, 1, _rcHxy[1]);
            J.At(0, 2, _rcHxy[2]);

            J.At(1, 0, _rcHxy[3]);
            J.At(1, 1, _rcHxy[4]);
            J.At(1, 2, _rcHxy[5]);

            J.At(2, 0, 0);
            J.At(2, 1, 0);
            J.At(2, 2, 1);

            Matrix<double> K = J.Inverse();
            double[] aArr = K.ToRowWiseArray();
            _deltaColumnBlock = K.At(1, 0) * points[3][0] + K.At(1, 1) * points[3][1] + K.At(1, 2);
            points.Clear();
        }


        /// <summary>
        /// 通过点位信息计算RC2XY matrix [X, Y] = matrix*[ r , c]
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>
        public HTuple calRC2XY(List<Double[]> points)
        {
            _rcHxy = new HTuple();
            //int numPoints = points.Count;
            int numPoints = 3;
            if (points.Count < 3) return null;
            double[] x = new double[numPoints];
            double[] y = new double[numPoints];
            double[] r = new double[numPoints];
            double[] c = new double[numPoints];
            for (int i = 0; i < numPoints; i++)
            {
                x[i] = points[i][0];
                y[i] = points[i][1];
                r[i] = points[i][2];
                c[i] = points[i][3];
            }
            //5. least square estimation
            //Dense(row,column,every value)
            Matrix<double> In = Matrix<double>.Build.Dense(3, numPoints, 1.0); //by default In[2,:] = 1.0
            Matrix<double> Out = Matrix<double>.Build.Dense(2, numPoints);

            Out.SetRow(0, x);
            Out.SetRow(1, y);

            In.SetRow(0, r);
            In.SetRow(1, c);
            Matrix<double> A = vec2Mat(In, Out);
            //6. move to center of uv space
            double[] aArr = A.ToRowWiseArray(); //need to be tested
            _calibrationRC2XYParameter = string.Join(",", aArr.ToArray());
            for (int i = 0; i < 6; i++)
            {
                _rcHxy.Append(aArr[i]);
            }
            return _rcHxy;
        }
        /// <summary>
        /// 读取参数，如果数据库中不含该参数则直接报错退出
        /// </summary>
        /// <returns>返回bool类型表示成功或失败，如果保存失败可以通过GetErrorString获取错误信息</returns>
        public override Boolean Read()
        {
            Boolean ret = true;
            try
            {
                sqlCon = new SQLiteConnection(@"DATA SOURCE=" + paraFile + @"; VERSION=3");//改动
                if (sqlCon.State == System.Data.ConnectionState.Closed)
                {
                    sqlCon.Open();
                }
                SQLiteCommand cmd = new SQLiteCommand(sqlCon);
                SQLiteDataReader reader;
                PropertyInfo[] infos = this.GetType().GetProperties();
                foreach (PropertyInfo fi in infos)
                {
                    if ((fi.PropertyType.Name == "Int32" || fi.PropertyType.Name == "Double" || fi.PropertyType.Name == "Boolean" || fi.PropertyType.Name == "String") == false)
                    {
                        continue;
                    }
                    cmd.CommandText = String.Format("SELECT * FROM {0} WHERE [Para] = '{1}'", paraTable, fi.Name);//懂
                    reader = cmd.ExecuteReader();
                    if (!reader.HasRows)   //确保所有参数被赋值成功
                    {
                        ret = false;
                        errString = String.Format("数据库中没有参数[{0}]", fi.Name);
                        break;
                    }
                    else
                    {
                        reader.Read();
                        switch (fi.PropertyType.Name)
                        {
                            case "Int32":
                                fi.SetValue(this, Convert.ToInt32(reader["Value"]));
                                break;
                            case "Double":
                                fi.SetValue(this, Convert.ToDouble(reader["Value"]));
                                break;
                            case "Boolean":
                                fi.SetValue(this, Convert.ToBoolean(Convert.ToInt32(reader["Value"])));
                                break;
                            case "String":
                                fi.SetValue(this, Convert.ToString(reader["Value"]));
                                break;
                        }
                    }
                    reader.Close();
                }
                int index = _calibrationUV2XYParameter.IndexOf(",");
                if (index > -1)
                {
                    string[] _stringUV2XY = _calibrationUV2XYParameter.Split(',');
                    double num;
                    for (int i = 0; i < _stringUV2XY.Length; i++)
                    {
                        if (!Double.TryParse(_stringUV2XY[i], out num))
                        {
                            errString = "String转Double失败";
                            HTLog.Error(errString);
                            ret = false;
                        }
                        MatUV2XY.Append(num);
                    }
                }
                int indexRC2XY = _calibrationRC2XYParameter.IndexOf(",");
                if (indexRC2XY > -1)
                {
                    string[] _stringRC2XY = _calibrationRC2XYParameter.Split(',');
                    double numRC2XY;
                    for (int i = 0; i < _stringRC2XY.Length; i++)
                    {
                        if (!Double.TryParse(_stringRC2XY[i], out numRC2XY))
                        {
                            errString = "String转Double失败";
                            HTLog.Error(errString);
                            ret = false;
                        }
                        _rcHxy.Append(numRC2XY);
                    }
                }


            }
            catch (Exception exp)
            {
                errCode = -1;
                errString = exp.ToString();
                ret = false;
            }
            sqlCon.Close();
            return ret;
        }

        /// <summary>
        /// 保存属性
        /// </summary>
        /// <returns></returns>
        public override bool Save()
        {
            Boolean ret = true;
            try
            {
                sqlCon = new SQLiteConnection(@"DATA SOURCE=" + paraFile + @"; VERSION=3");//改动
                if (sqlCon.State == System.Data.ConnectionState.Closed)
                {
                    sqlCon.Open();
                }
                String sql = "CREATE TABLE IF NOT EXISTS " + paraTable + "(Para TEXT PRIMARY KEY NOT NULL, Value TEXT NOT NULL)";
                SQLiteCommand cmd = new SQLiteCommand(sql, sqlCon);
                cmd.ExecuteNonQuery();
                PropertyInfo[] infos = this.GetType().GetProperties();//type.GetField
                foreach (PropertyInfo fi in infos)
                {
                    switch (fi.PropertyType.Name)
                    {
                        case "String":
                        case "Int32":
                        case "Boolean":
                        case "Double":
                            cmd.CommandText = String.Format("REPLACE INTO {0}(Para, Value) VALUES(@_Para, @_Value)", paraTable);//1234之类的？
                            cmd.Parameters.Add("_Para", System.Data.DbType.String).Value = fi.Name;
                            cmd.Parameters.Add("_Value", System.Data.DbType.Object).Value = fi.GetValue(this);
                            cmd.ExecuteNonQuery();
                            break;
                    }
                }
                sqlCon.Close();
            }
            catch (Exception exp)
            {
                errCode = -1;
                errString = exp.ToString();
                sqlCon.Close();
                ret = false;
            }
            return ret;
        }

        public override bool Equals(object obj)
        {
            var operations = obj as Operations;
            return operations != null &&
                   XcoordsTopLeft == operations.XcoordsTopLeft &&
                   YcoordsTopLeft == operations.YcoordsTopLeft &&
                   RowTopLeft == operations.RowTopLeft &&
                   ColumnTopLeft == operations.ColumnTopLeft &&
                   XcoordsBottomRight == operations.XcoordsBottomRight &&
                   YcoordsBottomRight == operations.YcoordsBottomRight &&
                   RowBottomRight == operations.RowBottomRight &&
                   ColumnBottomRight == operations.ColumnBottomRight &&
                   XcoordsTopRight == operations.XcoordsTopRight &&
                   YcoordsTopRight == operations.YcoordsTopRight &&
                   RowTopRight == operations.RowTopRight &&
                   ColumnTopRight == operations.ColumnTopRight &&
                   CalibrUV2XYModelPath == operations.CalibrUV2XYModelPath &&
                   CalibRC2XYModelPath == operations.CalibRC2XYModelPath;
        }

        /// <summary>
        /// 先计算出计算出rc和uv的关系再用height除单个像素u，和v
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="_rcHxy"></param>
        /// <param name="MatUV2XY"></param>
        /// <param name="M">行数</param>
        /// <param name="N">列数</param>
        public void CalcMN(out double M, out double N)
        {
            //[dx]       [ H1  H2]    [dr]             [dx]       [ J1  J2]    [du]
            //[dy]   =   [ H4  H5] *  [dc]             [dy]   =   [ J4  H5] *  [dv]      ==>
            //===============================================================================
            //[du]       [ J1  J2]^(-1)    [ H1  H2]    [dr]      [ K1  K2]    [dr]
            //[dv]   =   [ J4  J5]      *  [ H4  H5]  * [dc]  =   [ K3  K4] *  [dc]
            //===============================================================================
            //M = 1/height*du/dr = ceil(1/height*K1)
            //N = 1/width*dv/dc  = ceil(1/width*K4)
            //M = height*dr/du 
            //N = width*dc/dv

            //新建稠密矩阵rcHxy = xy = marix rc 2X2 每个值为1.0
            Matrix<double> H = Matrix<double>.Build.Dense(2, 2, 1.0);
            H.At(0, 0, _rcHxy[0]);
            H.At(0, 1, _rcHxy[1]);
            H.At(1, 0, _rcHxy[3]);
            H.At(1, 1, _rcHxy[4]);

            // matrix inverse is UV2XY
            Matrix<double> J = Matrix<double>.Build.Dense(2, 2, 1.0);
            J.At(0, 0, MatUV2XY[0]);
            J.At(0, 1, MatUV2XY[1]);
            J.At(1, 0, MatUV2XY[3]);
            J.At(1, 1, MatUV2XY[4]);
            Matrix<double> K = J.Inverse() * H;
            double[] aArr = K.ToRowWiseArray();

            //double _r = Math.Abs(1.0 / height * K.At(0, 0));
            //_r = height/ (dr/dv)  _c = width/(dc/du)
            //确定A(0，0)和A()
            double _r = Math.Abs(_height.D * 0.95 / K.At(0, 0));//行数
            double _c = Math.Abs(_width.D * 0.95 / K.At(1, 1));//列数

            //double _r = 3;//行数
            //double _c = 4;//列数
            //M = (int)(Math.Abs(_r)) + 1;行
            //N = (int)(Math.Abs(_c)) + 1; 列
            M = _r > 0.5 ? Convert.ToDouble(Math.Floor(_r)) : _r;//行，Floor向下取整
            N = _c > 0.5 ? Convert.ToDouble(Math.Floor(_c)) : _c;//列，Floor向下取整
            _m = M; //3
            _n = N; //4
        }

        /// <summary>
        /// 计算运动步长
        /// </summary>
        /// <param name="m">视野中最大行数</param>
        /// <param name="n">视野中最大列数</param>
        /// <param name="xStep">x方向运动步长</param>
        /// <param name="yStep">y方向运动步长</param>
        public void MoveStep(Double m, Double n, out Double xStep, out Double yStep)
        {
            xStep = 0;
            yStep = 0;
            xStep = Math.Abs((m) * _rcHxy[0].D + (n) * _rcHxy[1].D);
            yStep = Math.Abs((m) * _rcHxy[3].D + (n) * _rcHxy[4].D);
        }
        /// <summary>
        /// 计算运动步长
        /// </summary>
        /// <param name="m">视野中最大行数</param>
        /// <param name="n">视野中最大列数</param>
        /// <param name="xStep">x方向运动步长</param>
        /// <param name="yStep">y方向运动步长</param>
        public void dimentation(out Double xSize, out Double ySize)
        {
            xSize = 0;
            ySize = 0;
            xSize = Math.Abs(_rcHxy[0].D + _rcHxy[1].D);
            ySize = Math.Abs(_rcHxy[3].D + _rcHxy[4].D);
        }
        /// <summary>
        /// 未考虑Block时，照片中心点位
        /// </summary>
        /// <param name="r">拍照的行数</param>
        /// <param name="c">拍照的列数</param>
        /// <param name="xCenterDie">照片中心</param>
        /// <param name="yCenterDie">照片中心</param>
        public void DieCoords(Int32 r, Int32 c, out Double xCenterDie, out Double yCenterDie)
        {
            xCenterDie = 0;//第一张图片中心x坐标
            yCenterDie = 0;//第一张图片中心y坐标
            //firstDie r = zero， c = zero 
            Double xFirstDie = (((_m - 1) * _rcHxy[0].D + (_n - 1) * _rcHxy[1].D + _rcHxy[2].D) + (_rcHxy[2].D)) / 2.0;
            Double yFirstDie = (((_m - 1) * _rcHxy[3].D + (_n - 1) * _rcHxy[4].D + _rcHxy[5].D) + (_rcHxy[5].D)) / 2.0;

            //Two 
            xCenterDie = (r - 0) * _m * _rcHxy[0].D + (c - 0) * _n * _rcHxy[1].D + xFirstDie;
            yCenterDie = (r - 0) * _m * _rcHxy[3].D + (c - 0) * _n * _rcHxy[4].D + yFirstDie;
            //centerXaxisCoords = (((r+m-1) * _rcHxy[0].D + (c + n - 1) * _rcHxy[1].D + _rcHxy[2].D) + (r * _rcHxy[0].D + c * _rcHxy[1].D + _rcHxy[2].D)) /2.0;
            //centerYaxisCoords = (((r + m - 1) * _rcHxy[3].D + (c + n - 1) * _rcHxy[4].D + _rcHxy[5].D) + (r  * _rcHxy[3].D + c * _rcHxy[4].D + _rcHxy[5].D)) / 2.0;
        }

        public void calculateCalibPoints(int r1, int c1, int r2, int c2, int r3, int c3, int r4, int c4)
        {
            //App.obj_Pdt.GetPosByRowCol(r1, c1, out xcoordsTopLeft, out ycoordsTopLeft, true);
            //App.obj_Pdt.GetPosByRowCol(r2, c2, out xcoordsTopRight, out ycoordsTopRight, true);
            //App.obj_Pdt.GetPosByRowCol(r3, c3, out xcoordsBottomRight, out ycoordsBottomRight, true);
            //App.obj_Pdt.GetPosByRowCol(r4, c4, out _xBlock1TopLeft, out _yBlock1TopLeft, true);

        }

        /// <summary>
        /// Z轴线性扫描,函数返回值为"",执行正确,否则为响应错误信息
        /// </summary>
        /// <param name="camInd">相机索引</param>
        /// <param name="imgPos">图像Z轴点位集合</param>
        /// <param name="images">图像集合</param>
        /// <param name="timeOut">收集图像超时时间,单位ms</param>
        /// <returns></returns>
        public string Z_LineTrigger(int camInd, out HTuple imgPos, out HObject images, int timeOut = 5000)
        {
            string err = "";
            imgPos = new HTuple();
            images = new HObject();
            HOperatorSet.GenEmptyObj(out images);
            images.Dispose();
            try
            {
                if (Math.Abs(App.obj_Chuck.z_TrigStart - App.obj_Chuck.z_TrigEnd) < Math.Abs(App.obj_Chuck.z_TrigStep))
                {
                    err = "触发起点和终点位置距离太小,已小于一个步距!";
                    return err;
                }
                if (App.obj_Chuck.z_TrigStep == 0)
                {
                    err = "触发步距不能为0!";
                    return err;
                }
                if (App.obj_Chuck.Fps == 0)
                {
                    err = "相机帧率不能为0!";
                    return err;
                }
                if (!App.obj_Chuck.MoveToZStart())
                {
                    err = "运动至触发起始点位失败";
                    throw new Exception(err);
                }
                App.obj_Chuck.SetTriggerPos();
                int trigNum = 0;
                App.obj_Vision.obj_camera[camInd].Camera.ClearImageQueue();
                if (!App.obj_Chuck.Move2ZEnd(out trigNum))
                {
                    err = App.obj_Chuck.GetLastErrorString();
                    throw new Exception(err);
                }
                Acquisition acq = App.obj_Vision.obj_camera[camInd].Camera.GetFrames(trigNum, timeOut);
                if (acq.index + 1 != trigNum)
                {
                    err = String.Format("期望采集{0}张图像,实际采集{1}张图像!", trigNum, acq.index + 1);
                    acq.Dispose();
                    return err;
                }
                images = acq.Image;
                //imgPos
                double step = App.obj_Chuck.z_TrigStart > App.obj_Chuck.z_TrigEnd ? -Math.Abs(App.obj_Chuck.z_TrigStep) : Math.Abs(App.obj_Chuck.z_TrigStep);
                for (int i = 0; i < trigNum; i++)
                {
                    imgPos.Append(App.obj_Chuck.z_TrigStart + i * step);
                }
            }
            catch (Exception ex)
            {
                err = ex.Message;
            }
            return err;
        }

        /// <summary>
        /// 线性触发获取多张图片并通过算法检测
        /// </summary>
        /// <param name="cameraInd">相机索引</param>
        /// <param name="filePath">多张图片保存路劲</param>
        /// <param name="test">测试与否</param>
        /// <param name="posList">返回的位置列表</param>
        /// <param name="outImage">返回算法合成后图片</param>
        public void Z_LineTrigger(int cameraInd, string filePath, Boolean test, out List<double> posList, out HObject outImage, out Boolean imagesNum)
        {
            int trigNum = 0;
            imagesNum = true;
            HOperatorSet.GenEmptyObj(out outImage);
            posList = new List<Double>();
            if (App.obj_Chuck.MoveToZStart() == false)
            {
                throw new Exception("运动至触发起始点位失败");
            }
            //1. clear buffer
            Acquisition acq = new Acquisition();
            App.obj_Vision.obj_camera[cameraInd].acq.Dispose();
            App.obj_Vision.obj_camera[cameraInd].Camera.ClearImageQueue();
            //2. set trigger position
            App.obj_Chuck.SetTriggerPos();
            //3. move to end position
            if (App.obj_Chuck.Move2ZEnd(out trigNum) == false)
            {
                throw new Exception(App.obj_Chuck.GetLastErrorString());
            }
            //4. Get Frames
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            acq = App.obj_Vision.obj_camera[cameraInd].Camera.GetFrames(trigNum, 1000);
            if (acq.index == -1)
            {
                throw new Exception("连续采图未能获取到图像");
            }
            //_liningTrigImagesSaving = true;
            //if (test||_liningTrigImagesSaving)        //设置保存连续拍照图像操作
            //{
            //    HOperatorSet.WriteImage(acq.Image, "tiff", 0, filePath + "\\IMAGE.tiff");
            //}
            //保存连续拍照所获取图片
            if (acq.index + 1 != trigNum)
            {
                imagesNum = false;
                return;
            }
            Directory.CreateDirectory(filePath + "\\3DAutoFocus");
            for (int i = 0; i < trigNum; i++)
            {
                HOperatorSet.WriteImage(acq.Image.SelectObj(i + 1), "tiff", 0, filePath + "\\3DAutoFocus\\" + i + ".tiff");
            }
            //HOperatorSet.WriteImage(acq.Image, "tiff", 0, path + "\\IMAGE.tiff");
            //6. Algrithom

            HObject ho_Depth;
            HObject ho_Confidence;
            HObject ho_SharpImage;
            HTuple hv_FocusHeigth;
            HTuple hv_iFlag;
            //  App.obj_Vision.auto_focus_via_dff(acq.Image, out  ho_Depth, out  ho_Confidence, out  ho_SharpImage,
            //out  hv_FocusHeigth, out  hv_iFlag);
            //  outImage = ho_SharpImage.CopyObj(1, -1);
            //  HOperatorSet.ConcatObj(outImage, ho_Depth, out outImage);
            //  HOperatorSet.ConcatObj(outImage, ho_Confidence, out outImage);

            //  if (-1 == hv_iFlag)
            //  {
            //      throw new Exception("连续聚焦3D检测失败");
            //  }
            //  //if (test == true)
            //  //{
            //  //    HOperatorSet.WriteImage(ho_SharpImage, "tiff", 0, filePath + "\\IMAGE.tiff");
            //  //}
            //  posList = new List<double>();
            //  foreach (var item in hv_FocusHeigth.DArr)
            //  {
            //     Double posFoucs =  item * (App.obj_Chuck.z_TrigEnd - App.obj_Chuck.z_TrigStart) / trigNum;
            //      posList.Add(posFoucs + App.obj_Chuck.z_TrigStart);
            //  }
        }
    }
}
