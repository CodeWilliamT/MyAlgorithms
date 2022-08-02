using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalconDotNet;
using System.Data.SQLite;
using System.Reflection;
using Utils;
using HT_Lib;




namespace LeadframeAOI
{
    /// <summary>
    /// 光源亮度标定
    /// </summary>
    class BrightnessCalibration : Base
    {
        /// <summary>
        /// 拍照数目
        /// </summary>
        public int ImagesNumber { get; set; }

        /// <summary>
        /// 光源触发时间间隔
        /// </summary>
        public int DeltaTime { get; set; }

        /// <summary>
        /// 亮度字符串
        /// </summary>
        private string _brightnessStr = String.Empty;

        /// <summary>
        /// 图像均值字符串
        /// </summary>
        private string _meansStr = String.Empty;

        /// <summary>
        /// 同轴光图像均值
        /// </summary>
        private double[] _imagesMeansCoaxial;

        /// <summary>
        /// 同轴光亮度
        /// </summary>
        private double[] _brightnessesCoaxial;

        /// <summary>
        /// 环形光下图像均值
        /// </summary>
        private double[] _imagesMeansRing;
        private double[] _brightnessesRing;

        SQLiteConnection sqlCon;    //连接

        public BrightnessCalibration(String paraFile, String paraTable) : base(paraFile, paraFile) { }

        /// <summary>
        /// 采集图像并获取图像均值
        /// </summary>
        /// <param name="cameraNo">光圈可调相机序号0/1/2取值</param>
        /// <returns>返回成功与否标志</returns>
        public Boolean captureImages(int cameraNo, Boolean coaxialSelected)
        {
            if (true == coaxialSelected)//同轴光选择
            {
                _imagesMeansCoaxial = new double[ImagesNumber];
                _brightnessesCoaxial = new double[ImagesNumber];
                for (int i = 0; i < ImagesNumber; i++)
                {
                    _brightnessesCoaxial[i] = i * DeltaTime;//3ms间隔
                 //   App.obj_light.SetHTNetLightTime(App.obj_light.CoaxialLightDev, i * DeltaTime);//同轴光亮度调节
                    App.obj_light.SetCoaxLight(true, i * DeltaTime);
                    App.obj_Chuck.SWPosTrig();

                    //参数选择，每次拍一张，第一张图片
                    bool ret = App.obj_Vision.obj_camera[cameraNo].ScanPoint(-1, -1, 0, 1, 5000); ;//中间带光圈的相机获取图片
                    if (!ret)
                    {
                        throw new Exception("图像采集失败");
                    }

                    HObject image = null;
                    HOperatorSet.GenEmptyObj(out image);
                    HTuple means = new HTuple();
                    HTuple deviation = new HTuple();

                    //scanpoint 选择第0张图像，所以imageic
                    image = App.obj_Vision.obj_camera[cameraNo].ImageIC;
                    HOperatorSet.Intensity(image, image, out means, out deviation);
                    _imagesMeansCoaxial[i] = means.D;//保存获取后的图像均值
                }
            }
            else
            {
                _imagesMeansRing = new double[ImagesNumber];
                _brightnessesRing = new double[ImagesNumber];
                for (int i = 0; i < ImagesNumber; i++)
                {
                    //环形光0~255等分
                    _brightnessesCoaxial[i] = i * 255 / ImagesNumber;
                   // App.obj_light.SetIntensity(1, Convert.ToInt32(_brightnessesCoaxial[i]));
                    App.obj_light.SetCoaxLight(true, Convert.ToDouble(_brightnessesCoaxial[i]));
              

                    App.obj_Chuck.SWPosTrig();
                    bool ret = App.obj_Vision.obj_camera[cameraNo].ScanPoint(-1, -1, 0, 1, 5000);
                    if (!ret)
                    {
                        throw new Exception("图像采集失败");
                    }
                    //scanpoint 选择第0张图像，所以imageic
                    HObject image = null;
                    HOperatorSet.GenEmptyObj(out image);
                    HTuple means = new HTuple();
                    HTuple deviation = new HTuple();
                    image = App.obj_Vision.obj_camera[cameraNo].ImageIC;
                    HOperatorSet.Intensity(image, image, out means, out deviation);
                    _imagesMeansCoaxial[i] = means.D;//保存获取后的图像均值
                }
            }
            return true;
        }

        /// <summary>
        /// 图像均值已知量，求光源亮度未知量
        /// </summary>
        /// <param name="knownQuantity">已知灰度值</param>
        /// <param name="brightness">光源亮度</param>
        public void SoluteBrightness(double knownQuantity, Boolean coaxial, out double brightness)
        {
            if (true == coaxial)
            {
                //HTM自带，曲线拟合，求值
                HTHelper.Math.LineInterp(_imagesMeansCoaxial, _brightnessesCoaxial, ImagesNumber, knownQuantity, out brightness);
            }
            else
            {
                HTHelper.Math.LineInterp(_imagesMeansRing, _brightnessesRing, ImagesNumber, knownQuantity, out brightness);
            }

        }

        /// <summary>
        /// 已知光源亮度，求图像均值
        /// </summary>
        /// <param name="knownQuantity">光源亮度</param>
        /// <param name="imageMeans">图像均值</param>
        public void SoluteMeans(double knownQuantity, Boolean coaxial, out double imageMeans)
        {
            if (true)
            {
                HTHelper.Math.LineInterp(_brightnessesCoaxial, _imagesMeansCoaxial, ImagesNumber, knownQuantity, out imageMeans);

            }
            else
            {
                HTHelper.Math.LineInterp(_brightnessesRing, _imagesMeansRing, ImagesNumber, knownQuantity, out imageMeans);
            }
        }

        /// <summary>
        /// 保存图像均值数组与光源亮度数组至本地
        /// 将数组变成字符串用,分割
        /// </summary>
        public Boolean SaveArray()
        {
            var brightnesses = new System.Text.StringBuilder();
            var meanses = new System.Text.StringBuilder();
            if (null == _brightnessesCoaxial)
            {
                return false;
            }
            brightnesses.Append(_brightnessesCoaxial[0]);
            meanses.Append(_imagesMeansCoaxial[0]);
            for (int i = 1; i < ImagesNumber; i++)//同轴光标定保存
            {

                brightnesses.Append(",");
                brightnesses.Append(_brightnessesCoaxial[i]);
                meanses.Append(",");
                meanses.Append(_imagesMeansCoaxial[i]);
            }
            if (null == _brightnessesRing)
            {
                return false;
            }
            for (int i = 0; i < ImagesNumber; i++)//环形光标定保存
            {
                brightnesses.Append(",");
                brightnesses.Append(_brightnessesRing[i]);
                meanses.Append(",");
                meanses.Append(_imagesMeansRing[i]);
            }

            _brightnessStr = brightnesses.ToString();
            _meansStr = meanses.ToString();
            Save();
            return true;
        }

        /// <summary>
        /// 从本地读取图像均值与光源亮度至数组
        /// </summary>
        public void LoadArray()
        {
            Read();
            string[] strBrightnesses = _brightnessStr.Split(',');
            string[] strMeanses = _meansStr.Split(',');
            for (int i = 0; i < ImagesNumber; i++)
            {
                _brightnessesCoaxial[i] = Convert.ToDouble(strBrightnesses[i]);
                _imagesMeansCoaxial[i] = Convert.ToDouble(strMeanses[i]);
            }
            for (int i = 0; i < ImagesNumber; i++)
            {
                _brightnessesRing[i] = Convert.ToDouble(strBrightnesses[i + ImagesNumber]);
                _imagesMeansRing[i] = Convert.ToDouble(strMeanses[i + ImagesNumber]);
            }
        }


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
                PropertyInfo[] infos = this.GetType().GetProperties();

                var trans = cmd.Connection.BeginTransaction();
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
                trans.Commit();
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
    }
}
