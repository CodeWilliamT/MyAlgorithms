using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Data.SQLite;
using System.Data;
using System.ComponentModel;
using IniDll;
using System.IO;
using System.Windows.Forms;
using Utils;


namespace LeadframeAOI
{
    /// <summary>
    /// 图像信息类，该类保存拍照时图像的参数
    /// 曝光值、增益值、同轴光下图像均值、环形光下图像均值、Z轴聚焦位
    /// </summary>
    class ImageInformation:Base
    {
        private Int32 _cameraName = 0;
        private Double _exposure = 0;
        private Double _gain = 0;
        private Double _focusPos = 0;
        private Boolean _ringLightEnable = true;
        private Double _ringImageMeans = 0;
        private Boolean _ringLightEnable1 = true;
        private Double _ringImageMeans1 = 0;
        private Boolean _ringLightEnable2 = true;
        private Double _ringImageMeans2 = 0;
        private Boolean _ringLightEnable3 = true;
        private Double _ringImageMeans3 = 0;
        private Boolean _ringLightEnable4 = true;
        private Double _ringImageMeans4 = 0;
        private Boolean _coaxialLightEnable1 = true;
        private Double _coaxialImageMeans1 = 0;
        private Boolean _coaxialLightEnable2 = true;
        private Double _coaxialImageMeans2 = 0;
        private Boolean _useAutoFocus = false;
        private Int32 _Fps = 0;
        private Double _zFocusStart = 0;
        private Double _zFocusEnd = 0;
        private Double _trigInterval = 0;
        private Boolean _use2D = false;

        [BrowsableAttribute(false)]
        public string ParaFile;

        private SQLiteConnection sqlCon=new SQLiteConnection();    //连接
        private SQLiteDataAdapter da = new SQLiteDataAdapter();

        /// <summary>
        /// 类中属性名称的字符串
        /// </summary>
        private string _propertiesSring = String.Empty;

        public DataSet ds = new DataSet();

        [BrowsableAttribute(false)]
        public string ParaTable
        {
            get { return paraTable; }
            set { paraTable = value; }
        }

        [DisplayNameAttribute("相机序号")]
        public int CameraName
        {
            get { return _cameraName; }
            set { _cameraName = value; }
        }
        [ DisplayNameAttribute("曝光值")]
        public double Exposure
        {
            get { return  _exposure; }
            set { _exposure = value; }
        }
        [DisplayNameAttribute("增益值")]
        public double Gain
        {
            get {return  _gain; }
            set { _gain = value; }
        }
        [DisplayNameAttribute("上同轴光选择")]
        public bool CoaxialLightEnable1
        {
            get { return _coaxialLightEnable1; }
            set { _coaxialLightEnable1 = value; }
        }
        [DisplayNameAttribute("上同轴光")]
        public double CoaxialImageMeans1
        {
            get { return _coaxialImageMeans1; }
            set { _coaxialImageMeans1 = value; }
        }
        [DisplayNameAttribute("下同轴光选择")]
        public bool CoaxialLightEnable2
        {
            get { return _coaxialLightEnable2; }
            set { _coaxialLightEnable2 = value; }
        }
        [DisplayNameAttribute("下同轴光")]
        public double CoaxialImageMeans2
        {
            get { return _coaxialImageMeans2; }
            set { _coaxialImageMeans2 = value; }
        }
        [DisplayNameAttribute("环形光选择")]
        public bool RingLightEnable
        {
            get { return _ringLightEnable; }
            set { _ringLightEnable = value; }
        }
        [DisplayNameAttribute("环形光")]
        public double RingImageMeans
        {
            get { return _ringImageMeans; }
            set { _ringImageMeans = value; }
        }

        [DisplayNameAttribute("多通环光通1选择")]
        public bool RingLightEnable1
        {
            get { return _ringLightEnable1; }
            set { _ringLightEnable1 = value; }
        }
        [DisplayNameAttribute("多通环光通1")]
        public double RingImageMeans1
        {
            get { return _ringImageMeans1; }
            set { _ringImageMeans1 = value; }
        }

        [DisplayNameAttribute("多通环光通2选择")]
        public bool RingLightEnable2
        {
            get { return _ringLightEnable2; }
            set { _ringLightEnable2 = value; }
        }
        [DisplayNameAttribute("多通环光通2")]
        public double RingImageMeans2
        {
            get { return _ringImageMeans2; }
            set { _ringImageMeans2 = value; }
        }

        [DisplayNameAttribute("多通环光通3选择")]
        public bool RingLightEnable3
        {
            get { return _ringLightEnable3; }
            set { _ringLightEnable3 = value; }
        }
        [DisplayNameAttribute("多通环光通3")]
        public double RingImageMeans3
        {
            get { return _ringImageMeans3; }
            set { _ringImageMeans3 = value; }
        }

        [DisplayNameAttribute("多通环光通4选择")]
        public bool RingLightEnable4
        {
            get { return _ringLightEnable4; }
            set { _ringLightEnable4 = value; }
        }
        [DisplayNameAttribute("多通环光通4")]
        public double RingImageMeans4
        {
            get { return _ringImageMeans4; }
            set { _ringImageMeans4 = value; }
        }

        [DisplayNameAttribute("聚焦位")]
        public double FocusPos
        {
            get { return _focusPos; }
            set { _focusPos = value; }
        }
        [DisplayNameAttribute("2D拍照")]
        public bool Use2D
        {
            get { return _use2D; }
            set { _use2D = value; }
        }
        [DisplayNameAttribute("3D聚焦")]
        public bool UseAutoFocus
        {
            get { return _useAutoFocus; }
            set { _useAutoFocus = value; }
        }
        [DisplayNameAttribute("拍照帧率")]
        public int Fps
        {
            get { return _Fps; }
            set { _Fps = value; }
        }
        [DisplayNameAttribute("触发起点")]
        public double ZFocusStart
        {
            get { return _zFocusStart; }
            set { _zFocusStart = value; }
        }
        [DisplayNameAttribute("触发终点")]
        public double ZFocusEnd
        {
            get { return _zFocusEnd; }
            set { _zFocusEnd = value; }
        }
        [DisplayNameAttribute("触发间隔")]
        public double TrigInterval
        {
            get { return _trigInterval; }
            set { _trigInterval = value; }
        }
        /// <summary>
        /// 配置表单内数据入设备
        /// </summary>
        /// <param name="_selectIndex"></param>
        public static void ConfigAllData(int _selectIndex)
        {
            bool ringEnable = App.obj_ImageInformSet[_selectIndex].RingLightEnable;
            double ringTime = App.obj_ImageInformSet[_selectIndex].RingImageMeans;
            bool ringEnable1 = App.obj_ImageInformSet[_selectIndex].RingLightEnable1;
            double ringTime1 = App.obj_ImageInformSet[_selectIndex].RingImageMeans1;
            bool ringEnable2 = App.obj_ImageInformSet[_selectIndex].RingLightEnable2;
            double ringTime2 = App.obj_ImageInformSet[_selectIndex].RingImageMeans2;
            bool ringEnable3 = App.obj_ImageInformSet[_selectIndex].RingLightEnable3;
            double ringTime3 = App.obj_ImageInformSet[_selectIndex].RingImageMeans3;
            bool ringEnable4 = App.obj_ImageInformSet[_selectIndex].RingLightEnable4;
            double ringTime4 = App.obj_ImageInformSet[_selectIndex].RingImageMeans4;
            bool coaxEnable1 = App.obj_ImageInformSet[_selectIndex].CoaxialLightEnable1;
            double coaxTime1 = App.obj_ImageInformSet[_selectIndex].CoaxialImageMeans1;
            bool coaxEnable2 = App.obj_ImageInformSet[_selectIndex].CoaxialLightEnable2;
            double coaxTime2 = App.obj_ImageInformSet[_selectIndex].CoaxialImageMeans2;
            double exposure = App.obj_ImageInformSet[_selectIndex].Exposure;
            double gain = App.obj_ImageInformSet[_selectIndex].Gain;
            //2. 配置
            if (App.obj_Vision.obj_camera[App.obj_ImageInformSet[_selectIndex].CameraName].IsEnable)
            {
                App.obj_Vision.obj_camera[App.obj_ImageInformSet[_selectIndex].CameraName].SetExposure(exposure);
                App.obj_Vision.obj_camera[App.obj_ImageInformSet[_selectIndex].CameraName].SetGain(gain);
            }
            App.obj_light.SetRingLight(ringEnable, ringTime);
            App.obj_light.SetRingLight_Ex(1,ringEnable1, ringTime1);
            App.obj_light.SetRingLight_Ex(2, ringEnable2, ringTime2);
            App.obj_light.SetRingLight_Ex(3, ringEnable3, ringTime3);
            App.obj_light.SetRingLight_Ex(4, ringEnable4, ringTime4);
            App.obj_light.SetCoaxLight1(coaxEnable1, coaxTime1);
            App.obj_light.SetCoaxLight2(coaxEnable2, coaxTime2);
            App.obj_Chuck.Fps = App.obj_ImageInformSet[_selectIndex].Fps;
        }

        public ImageInformation(string paraFile, string paraTable):base(paraFile,paraTable)
        {
            ParaFile = paraFile;
        }

        /// <summary>
        /// 创建数据表格,先创建key值，再通过插入得到完整表格
        /// </summary>
        public void CreateTable()
        {
            try
            {
                sqlCon = new SQLiteConnection(@"DATA SOURCE=" + paraFile + @"; VERSION=3");//改动
                if (sqlCon.State == System.Data.ConnectionState.Closed)
                {
                    sqlCon.Open();
                }
                String sql = "CREATE TABLE IF NOT EXISTS " + paraTable + "(ImageKey INTEGER PRIMARY KEY AUTOINCREMENT,CameraName REAL NOT NULL,Exposure REAL NOT NULL,Gain REAL NOT NULL,RingMeans REAL NOT NULL,CoaxialMeans REAL NOT NULL,Zpos REAL NOT NULL)";
                SQLiteCommand cmd = new SQLiteCommand(sql, sqlCon);
                cmd.ExecuteNonQuery();
                sqlCon.Close();
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

        /// <summary>
        /// 插入一行数据
        /// </summary>
        /// <param name="imageKey"></param>
        /// <param name="cameraName"></param>
        /// <param name="exposure"></param>
        /// <param name="gain"></param>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <param name=""></param>
        public void insertData( )
        {
            try
            {
                sqlCon = new SQLiteConnection(@"DATA SOURCE=" + paraFile + @"; VERSION=3");//改动
                if (sqlCon.State == System.Data.ConnectionState.Closed)
                {
                    sqlCon.Open();
                }
                //_imageKey = 0;
                //表格列为ImageKey ，CameraName，Exposure，Gain ，RingMeans ，CoaxialMeans ，Zpos  + workNumber + "'
                string sql = @"INSERT INTO '" + paraTable + "' VALUES(null,'" + _cameraName + "','" + _exposure + "','" + _gain + "','" + _ringLightEnable + "','" + _ringImageMeans + "','"+ _coaxialLightEnable1 + "','" + _coaxialImageMeans1 + "','" + _coaxialLightEnable2+ "','" + _coaxialImageMeans2 + "','" + _focusPos + "')";
                //string sql = @"INSERT INTO '" + TbName + "'(Username ,Password , Worknumber) VALUES('" + userName + "','" + passWord + "','" + workNumber + "')";
                SQLiteCommand cmd = new SQLiteCommand(sql, sqlCon);
                cmd.ExecuteNonQuery();
                sqlCon.Close();
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

        /// <summary>
        /// DB中的表格绑定dataset，更新或启动的时候使用
        /// </summary>
        public void FillDataSet()
        {
            try
            {
                sqlCon = new SQLiteConnection(@"DATA SOURCE=" + paraFile + @"; VERSION=3");//改动
                if (sqlCon.State == System.Data.ConnectionState.Closed)
                {
                    sqlCon.Open();
                }
                //表格列为ImageKey ，CameraName，Exposure，Gain ，RingMeans ，CoaxialMeans ，Zpos  + workNumber + "'
                //CameraName REAL NOT NULL, Exposure REAL NOT NULL,Gain REAL NOT NULL, RingMeans REAL NOT NULL,CoaxialMeans REAL NOT NULL, Zpos REAL NOT NULL
                string sql = "select ImageKey as '键',CameraName as '相机序号', Exposure as '曝光值', Gain as '增益值',RingMeans as'环形光',CoaxialMeans as '同轴光',Zpos as '聚焦位' from '" + paraTable + "'";
                //string sql = "select CameraName as '相机序号', Exposure as '曝光值', Gain as '增益值',RingMeans as'环形光图像均值',CoaxialMeans as '同轴光图像均值',Zpos as '聚焦位' from '" + paraTable + "'";

                //string sql = "select * from '" + paraTable + "'";
                SQLiteCommand cmd = new SQLiteCommand(sql, sqlCon);
                cmd.ExecuteNonQuery();
                //da = new SQLiteDataAdapter();
                da.SelectCommand = cmd;
                ds.Clear();
                da.Fill(ds, paraTable);
                ds.Tables[0].Columns[0].ColumnMapping = MappingType.Hidden;
                sqlCon.Close();
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }
        /// <summary>
        /// 删除某数据
        /// </summary>
        /// <param name="rowNumber"></param>
        public void DelDataGrid(Int32 rowNumber)
        {
            //ds.AcceptChanges();
            //da = new SQLiteDataAdapter();
            SQLiteCommandBuilder scb = new SQLiteCommandBuilder(da);
            ds.Tables[0].Rows[rowNumber].Delete();
            da.DeleteCommand = scb.GetDeleteCommand();
            da.Update(ds, paraTable);
            da.UpdateCommand = scb.GetUpdateCommand();
            ds.AcceptChanges();
        }
        /// <summary>
        /// 更新数据表
        /// </summary>
        /// <param name="dgv"></param>
        public void updateData(DataGridView dgv)
        {
            DataTable dt = dgv.DataSource as DataTable;
            SQLiteCommandBuilder scb = new SQLiteCommandBuilder(da);
            da.DeleteCommand = scb.GetUpdateCommand();
            //ds.AcceptChanges();
            da.Update(ds, paraTable);
            da.Update(dt);
        }
        public class CameraVariables : BaseModule
        {
            private string _paraFile = String.Empty;
            private string _paraTable = string.Empty;
            public double Exposure = 0;
            public double Gain = 0;
            public CameraVariables(String para_file, String para_table) : base(para_file, para_table) { }
        }


        /// <summary>
        /// 保存参数，仅仅保存Public属性的Int32, Boolean, String, Double四种类型参数
        /// </summary>
        /// <returns>返回bool类型表示成功或失败，如果保存失败可以通过GetErrorString获取错误信息</returns>
        public virtual Boolean Save()
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
        /// <summary>
        /// 读取参数，如果数据库中不含该参数则直接报错退出
        /// </summary>
        /// <returns>返回bool类型表示成功或失败，如果保存失败可以通过GetErrorString获取错误信息</returns>
        public virtual Boolean Read()
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
                        //ret = false;
                        errString = String.Format("数据库中没有参数[{0}]", fi.Name);

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
                            default:
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

