using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using HT_Lib;
using Utils;
using HalconDotNet;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using System.IO;
using System.ComponentModel;

namespace LeadframeAOI
{
    public enum Operation
    {
        PLATE = 0,    //压板
        VACUUM = 1    //真空
    }
    public struct Device_Chuck
    {

        #region 轴
        /// <summary>
        /// 相机X轴
        /// </summary>
        public int a_X;
        /// <summary>
        /// 相机Y轴
        /// </summary>
        public int a_Y;
        /// <summary>
        /// 相机Z轴
        /// </summary>
        public int a_Z;
        /// <summary>
        /// 导轨间距Y轴 
        /// </summary>
        public int a_Y_Track;
        #endregion


        /// <summary>
        /// 压板前上信号
        /// </summary>
        public int i_PlateFrontUp;
        /// <summary>
        /// 压板前下信号
        /// </summary>
        public int i_PlateFrontDown;
        /// <summary>
        /// 压板后上信号
        /// </summary>
        public int i_PlateBehindUp;
        /// <summary>
        /// 压板后下信号
        /// </summary>
        public int i_PlateBehindDown;
        /// <summary>
        /// 压板气缸  
        /// </summary>
        public int o_Plate;
        /// <summary>
        /// 真空控制开关
        /// </summary>
        public int o_Vacum;
        /// <summary>
        /// 吹气开关
        /// </summary>
        public int o_Blow;
        /// <summary>
        /// 真空信号
        /// </summary>         
        public int i_Vacum;
        /// <summary>
        /// 基板载台气缸开关
        /// </summary>
        public int o_Base;
        /// <summary>
        /// 基板载台气缸上信号
        /// </summary>
        public int i_BaseUp;
        /// <summary>
        /// 基板载台气缸下信号
        /// </summary>
        public int i_BaseDown;

        /// <summary>
        /// 位置触发板号
        /// </summary>
        public Int32 PosTrigIdx;           //pos trigger board index  
        /// <summary>喷印器墨水信号，1有0无 </summary>
        public int i_HaveInk;
        /// <summary>喷印器运行中信号,1运行，0待机 </summary>
        public int i_JetRun;

    }
    class ChuckModule : BaseModule
    {
        #region 公共变量
        public Device_Chuck dev_chuck = new Device_Chuck();
        private DieInfo m = new DieInfo(); //可知行列与对应的xyz坐标
        private double _preValue = 0;

        private Double z_focus;            //z轴聚焦位
        public Double x_Scan;              //X轴拍照位
        public Double y_Scan;              //Y轴拍照位
        public Double z_Scan;              //Z轴聚焦位 不同产品的聚焦位不一样
        public Double x_step;              //X方向触发信号步距
        public Double y_step;              //Y方向触发信号步距
        public Double x_Const;             //触发时轴运动偏移系数
        public Double x_ScanCode;          //X轴二维码拍照位
        public Double y_ScanCode;          //Y轴二维码拍照位
        public Double z_ScanCode;          //Z轴二维码聚焦位
        public Double x_FirstDie;          //标定第一个芯片x位置
        public Double y_FirstDie;          //标定第一个芯片y位置
        public Double z_FirstDie;          //标定第一个芯片z位置
        public Double z_TrigStart;         //Z轴触发位起点 图像访问量
        public Double z_TrigEnd;           //Z轴触发位终点 图像访问量
        public Double z_TrigStep;          //Z轴触发步距 图像访问量
        public Double z_Const;             //触发时轴运动偏移系数
        public Double x_Calib;             //uv2xy标定点x
        public Double y_Calib;             //uv2xy标定点y
        public Double z_Calib;             //uv2xy标定点z
        public Int32 calibNum;               //uv2xy标定点数
        public Double x_range;             //uv2xy标定时x移动范围
        public Double y_range;             //uv2xy标定时y移动范围
        public Int32 calib_usedNum;          //uv2xy标定有效点数,不能小于该值
        public Double RXMarkingPen = 0;     // 打点笔相对相机中心X距离 打点器-相机 打点器-位于中心的顶升气缸
        public Double RYMarkingPen = 0;    //打点笔相对相机中心Y距离 打点器-相机
        public Double AZMarkingPen = 0;
        public Double X_MarkingPen = 0;
        public Double Y_MarkingPen = 0;
        public Double x_Unload;            //X轴下料位
        public Double ref_z;              //参考原点当前的Z坐标(扫描时Z轴点位)

        public Double x_Load;              //X轴上料位
        public Double z_Safe;              //Z轴安全位
        public Double ref_x;                //参考原点
        public Double ref_y;               //参考原点

        public Double ref_Mark_x;                //参考原点
        public Double ref_Mark_y;
        public Double ref_Mark_z;
        public Double mark_Focus_z;

        public Int32 Fps = 0;

        /// <summary>
        /// 获得当前芯片聚焦位置
        /// </summary>
        [BrowsableAttribute(false)]
        public Double FocusPos
        {
            set { z_focus = value; }
            get { return z_focus; }
        }

        /// <summary>
        /// 获得operation的值,by LPY
        /// </summary>
        private Operation operation = Operation.PLATE;
        [BrowsableAttribute(false)]
        public Operation UsedOperation
        {
            set { operation = value; }
            get { return operation; }
        }
        [BrowsableAttribute(false)]
        /// <summary>
        /// 获得当前芯片行列及对应位置信息
        /// </summary>
        public DieInfo CurrentDieInfo
        {
            set { m = value; }
            get { return m; }
        }

        //[CategoryAttribute("流程属性"), DisplayNameAttribute("载台检测位(mm)"), DescriptionAttribute("载台左侧上料夹爪的位置")]
        //public double X_Load { get { return x_Load; }set{ x_Load = value;} }
        [CategoryAttribute("机构属性"), DisplayNameAttribute("相机Z轴安全位(mm)"), DescriptionAttribute("相机Z轴安全位")]
        public double Z_Safe { get { return z_Safe; }set{ z_Safe = value;} }
        [CategoryAttribute("机构属性"), DisplayNameAttribute("相机相对原点X坐标(mm)"), DescriptionAttribute("相机机构相对与不同料片不变的点位的X坐标")]
        public double Ref_x { get { return ref_x; }set{ ref_x = value;} }
        [CategoryAttribute("机构属性"), DisplayNameAttribute("相机相对原点Y坐标(mm)"), DescriptionAttribute("相机机构相对与不同料片不变的点位的Y坐标")]
        public double Ref_y { get { return ref_y; }set{ ref_y = value;} }
        [CategoryAttribute("机构属性"), DisplayNameAttribute("喷印器对相机X偏移(mm)"), DescriptionAttribute("喷印器相对相机X偏移,负左正右")]
        public double Ref_Mark_x { get { return ref_Mark_x; } set { ref_Mark_x = value; } }
        [CategoryAttribute("机构属性"), DisplayNameAttribute("喷印器对相机Y偏移(mm)"), DescriptionAttribute("喷印器相对相机Y偏移,负下正上")]
        public double Ref_Mark_y { get { return ref_Mark_y; } set { ref_Mark_y = value; } }
        [CategoryAttribute("机构属性"), DisplayNameAttribute("喷印标定喷印高度Z(mm)"), DescriptionAttribute("喷印器标定时喷印高度Z, 负下正上")]
        public double Ref_Mark_z { get { return ref_Mark_z; } set { ref_Mark_z = value; } }

        [CategoryAttribute("机构属性"), DisplayNameAttribute("标定样片相机聚焦高度Z(mm)"), DescriptionAttribute("标定样片相机聚焦高度Z,负下正上")]
        public double Mark_Focus_z { get { return mark_Focus_z; } set { mark_Focus_z = value; } }
        #endregion
        #region 公共方法

        public ChuckModule(String para_file, String para_table) : base(para_file, para_table) { }
        public void Initialize(Device_Chuck devInfo)
        {
            this.dev_chuck = devInfo;
        }
        public Boolean SaveFps()
        {
            System.Data.SQLite.SQLiteConnection sqlCon = null;    //连接
            Boolean ret = true;
            try
            {
                sqlCon = new System.Data.SQLite.SQLiteConnection(@"DATA SOURCE=" + paraFile + @"; VERSION=3");//改动
                if (sqlCon.State == System.Data.ConnectionState.Closed)
                {
                    sqlCon.Open();
                }
                String sql = "CREATE TABLE IF NOT EXISTS " + paraTable + "(Para TEXT PRIMARY KEY NOT NULL, Value TEXT NOT NULL)";
                System.Data.SQLite.SQLiteCommand cmd = new System.Data.SQLite.SQLiteCommand(sql, sqlCon);
                cmd.ExecuteNonQuery();
                System.Reflection.FieldInfo[] infos = this.GetType().GetFields();//type.GetField
                cmd.CommandText = String.Format("REPLACE INTO {0}(Para, Value) VALUES(@_Para, @_Value)", paraTable);//1234之类的？
                cmd.Parameters.Add("_Para", System.Data.DbType.String).Value = "Fps";
                cmd.Parameters.Add("_Value", System.Data.DbType.Object).Value = Fps;
                cmd.ExecuteNonQuery();
                sqlCon.Close();
            }
            catch (Exception exp)
            {
                errCode = -1;
                errString = exp.ToString();
                if (sqlCon != null)
                    sqlCon.Close();
                ret = false;
            }
            return ret;
        }

        /// <summary>
        /// 根据产品文件固料方式设置chuckmodule里operation
        /// </summary>
        /// <param name="opr">固料方式</param>
        public void SetOperation(int opr)
        {
            if (opr == 0)
            {
                operation = Operation.PLATE;
            }
            else if (opr == 1)
            {
                operation = Operation.VACUUM;
            }
        }

        public void GetCurrentDieInfo(DieInfo die)
        {
            m.Copy(die);
        }
        /// <summary>
        /// Chuck模块回零,by LPY
        /// </summary>
        /// <returns>返回bool类型表示成功或者失败</returns>
        public override Boolean Home()
        {
            if (runMode == Convert.ToInt16(SystemRunMode.MODE_OFFLINE))
            {
                return true;
            }
            ready = false;

            //1、励磁和IO复位
            HTM.SetSVON(dev_chuck.a_X, 1);
            HTM.SetSVON(dev_chuck.a_Y, 1);
            HTM.SetSVON(dev_chuck.a_Z, 1);
            //HTM.SetSVON(dev_chuck.a_Y_Track, 1);
            if ((errCode = HTM.SVDone(dev_chuck.a_X, dev_chuck.a_Y, dev_chuck.a_Z)) < 0)
            {
                errString = "基板载台X轴、相机Y轴或相机Z轴励磁失败！";
                goto _end;
            }
            //2、Z轴回零
            HTM.Home(dev_chuck.a_Z);
            if ((errCode = HTM.HomeDone(dev_chuck.a_Z)) < 0)
            {
                errString = "相机Z轴回零失败！";
                goto _end;
            }
            //3.抬起压板
            if (!CheckErr(HTM.WriteDO, dev_chuck.o_Plate, 1))
            {
                errString = string.Format("{0}置位操作失败!", HTM.GetIoName(dev_chuck.o_Plate));
                goto _end;
            }
            //if (!CheckErr(HTM.WaitDI, dev_chuck.i_PlateBehindUp, dev_chuck.i_PlateFrontUp, 1, 5))
            //{
            //    errString = string.Format("压板升起超时!");
            //    goto _end;
            //}
            //4.下降载台
            if (App.obj_SystemConfig.CatchMode > 0)
            {
                if (!CheckErr(HTM.WriteDO, dev_chuck.o_Base, 0))
                {
                    errString = string.Format("{0}复位操作失败!", HTM.GetIoName(dev_chuck.o_Base));
                    goto _end;
                }
                if (!CheckErr(HTM.WaitDI, dev_chuck.i_BaseUp, 0, 5))
                {
                    errString = string.Format("载台气缸下降超时!");
                    goto _end;
                }
            }
            if (App.obj_SystemConfig.CatchMode > 1)
            {
                ////5.关闭真空
                if (!CheckErr(HTM.WriteDO, dev_chuck.o_Vacum, 0))
                {
                    errString = string.Format("{0}复位操作失败!", HTM.GetIoName(dev_chuck.o_Vacum));
                    goto _end;
                }
                if (!CheckErr(HTM.WriteDO, dev_chuck.o_Blow, 1))
                {
                    errString = string.Format("{0}复位操作失败!", HTM.GetIoName(dev_chuck.o_Blow));
                    goto _end;
                }
                if (!CheckErr(HTM.WriteDO, dev_chuck.o_Blow, 0))
                {
                    errString = string.Format("{0}复位操作失败!", HTM.GetIoName(dev_chuck.o_Blow));
                    goto _end;
                }
                if (!CheckErr(HTM.WaitDI, dev_chuck.i_Vacum, 0, 5))
                {
                    errString = string.Format("真空关闭超时!");
                    goto _end;
                }
            }
            //6.其他轴回零
            HTM.Home(dev_chuck.a_Y);
            HTM.Home(dev_chuck.a_X);
            //HTM.Home(dev_chuck.a_Y_Track);
            if ((errCode = HTM.HomeDone(dev_chuck.a_Y)) < 0)
            {
                errString = "相机Y轴回零失败！";
                goto _end;
            }
            if ((errCode = HTM.HomeDone(dev_chuck.a_X)) < 0)
            {
                errString = "Chuck模块X轴回零失败！";
                goto _end;
            }
            //if ((errCode = HTM.HomeDone(dev_chuck.a_Y_Track)) < 0)
            //{
            //    errString = "导轨间距Y轴回零失败！";
            //    goto _end;
            //}
            ready = true;
            _end:
            return ready;
        }
        public bool Fix_Track(Double trackPos)
        {
            if (runMode == Convert.ToInt16(SystemRunMode.MODE_OFFLINE))
            {
                return true;
            }
            Boolean ret = false;
            //确认模块ready
            //ready = true;
            if (ready == false)
            {
                errString = "Chuck模块未初始化！";
                goto _end;
            }
            double nowPos = HTM.GetFbkPos(dev_chuck.a_Y_Track);
            if(trackPos.ToString("F2")== nowPos.ToString("F2"))
            {

                ret = true;
                goto _end;
            }
            if ((errCode = HTM.SetSVOver(dev_chuck.a_Y_Track)) < 0)
            {
                errString = "Chuck模块导轨励磁失败！";
                goto _end;
            }
            if ((errCode =HTM.HomeOver(dev_chuck.a_Y_Track)) < 0)
            {
                errString = "Chuck模块导轨回零失败！";
                goto _end;
            }
            if ((errCode = HTM.MoveOver(dev_chuck.a_Y_Track, trackPos, speed, HTM.MotionMode.AS)) < 0)
            {
                errString = "Chuck模块导轨运动失败！";
                goto _end;
            }
            ret = true;
            _end:
            return ret;
        }
        /// <summary>
        /// 设置触发板触发点位
        /// </summary>
        /// <param name="s"></param>
        /// <param name="e"></param>
        /// <param name="inter"></param>
        /// <returns></returns>
        public bool SetTriggerPos()
        {
            int ret = 0;
            dev_chuck.PosTrigIdx = 2;
            var line = new HTM.TRIG_LINEAR { startPos = z_TrigStart, endPos = z_TrigEnd, interval = z_TrigStep };

            ret = HTM.SetLinTrigPos(dev_chuck.PosTrigIdx, ref line);
            if (ret != 0)
            {
                errString = String.Format("触发板设置触发点位失败");
                return false;
            }
            ret = HTM.SetLinTrigEnable(dev_chuck.PosTrigIdx, 1);
            if (ret != 0)
            {
                errString = String.Format("触发板开启失败");
                return false;
            }
            return true;
        }
        /// <summary>
        /// Z轴线性触发运动 需先在本模块设置 z_TrigStart z_TrigEnd z_TrigStep 
        /// </summary>
        /// <returns></returns>
        public bool Z_LineTrigger()
        {
            //1.运动到起点
            if (MoveToZStart() == false)
            {
                return false;
            }
            //2.设置触发点位参数
            if (SetTriggerPos() == false)
            {
                return false;
            }
            //3.移动到终点
            if (Move2ZEnd() == false)
            {
                return false;
            }

            return true;
        }


        //io操作
        /// <summary>
        /// 设置真空开关信号,by LPY
        /// </summary>
        /// <param name="on">1或者0表示开关两种状态</param>
        public void SwitchVacum(int on = 1)
        { HTM.WriteDO(dev_chuck.o_Vacum, on); }
        /// <summary>
        /// 判断真空是否开,by LPY
        /// </summary>
        /// <returns>返回int类型表示成功或者失败</returns>
        public int IsVacumON()
        {
            return HTM.ReadDI(dev_chuck.i_Vacum);
        }
        /// <summary>
        /// 判断基板气缸是否到达上位,by LPY
        /// </summary>
        /// <returns>返回int类型表示成功或者失败</returns>
        public int IsBaseUp()
        { return HTM.ReadDI(dev_chuck.i_BaseUp); }
        /// <summary>
        /// 判断基板气缸是否到达下位,by LPY
        /// </summary>
        /// <returns>返回int类型表示成功或者失败</returns>
        public int IsBaseOff()
        { return HTM.ReadDI(dev_chuck.i_BaseDown); }
        /// <summary>
        /// 判断压板气缸是否到达上位,by LPY
        /// </summary>
        /// <returns>返回int类型表示成功或者失败</returns>
        public int IsPlateUp()
        { return HTM.ReadDI(dev_chuck.i_PlateFrontUp); }
        /// <summary>
        /// 判断压板气缸是否到达下位,by LPY
        /// </summary>
        /// <returns>返回int类型表示成功或者失败</returns>
        public int IsPlateDown()
        { return HTM.ReadDI(dev_chuck.i_PlateFrontDown); }

        public int IsHaveInk()
        {
            return HTM.ReadDI(dev_chuck.i_HaveInk);
        }
        public int IsJetRun()
        {

            return HTM.ReadDI(dev_chuck.i_JetRun);
        }
        //轴操作

        /// <summary>
        /// Z轴相对对运动,by TWL
        /// </summary>
        /// <param name="xPos">终止位置</param>
        /// <param name="mode">运动模式</param>
        /// <returns>返回bool类型表示成功或者失败</returns>
        public Boolean Z_RSMove(Double RSPos, Double speed = 1.0)
        {
            if (runMode == Convert.ToInt16(SystemRunMode.MODE_OFFLINE))
            {
                return true;
            }
            Boolean ret = false;
            //确认模块ready
            if (ready == false)
            {
                errCode = -1;
                errString = "Chuck模块未初始化！";
                goto _end;
            }
            //发送运动指令
            if ((errCode = HTM.RSMove(dev_chuck.a_Z, RSPos, speed)) < 0)
            {
                errString = "Chuck模块Z轴运动指令发送失败！";
                goto _end;
            }
            // 3. 等待运动完成
            if ((errCode = HTM.Done(dev_chuck.a_Z)) < 0)
            {
                errString = "Chuck模块Z轴运动失败！";
                goto _end;
            }
            ret = true;
        _end:
            return ret;
        }
        /// <summary>
        /// Z轴绝对运动,by LPY
        /// </summary>
        /// <param name="zPos">终止位置</param>
        /// <param name="mode">运动模式</param>
        /// <returns>返回bool类型表示成功或者失败</returns>
        public Boolean Z_Move(Double zPos)
        {
            if (runMode == Convert.ToInt16(SystemRunMode.MODE_OFFLINE))
            {
                return true;
            }
            Boolean ret = false;
            //确认模块ready
            //ready = true;
            if (ready == false)
            {
                errCode = -1;
                errString = "Chuck模块未初始化！";
                goto _end;
            }

            //HTM_BSP.MOTION_PARA vPara = HTM.GetMotionPara(dev_chuck.a_Z);
            if ((errCode = HTM.Move(dev_chuck.a_Z, zPos, speed, HTM.MotionMode.AS)) < 0)
            {
                errString = "发送轴运动指令失败";
                return false;
            }
            ////发送运动指令
            //if ((errCode = HTM.ASMove(dev_chuck.a_Z, zPos, speed)) < 0)
            //{
            //    errString = "Chuck模块Z轴运动指令发送失败！";
            //    goto _end;
            //}
            // 3. 等待运动完成
            Thread.Sleep(100);
            if ((errCode = HTM.Done(dev_chuck.a_Z)) < 0)
            {
                errString = "Chuck模块Z轴运动失败！";
                goto _end;
            }
            ret = true;
            _end:
            return ret;
        }
        /// <summary>
        /// Y轴相对对运动,by LPY
        /// </summary>
        /// <param name="xPos">终止位置</param>
        /// <param name="mode">运动模式</param>
        /// <returns>返回bool类型表示成功或者失败</returns>
        public Boolean Y_RSMove(Double RSPos, Double speed=1.0)
        {
            if (runMode == Convert.ToInt16(SystemRunMode.MODE_OFFLINE))
            {
                return true;
            }
            Boolean ret = false;
            //确认模块ready
            if (ready == false)
            {
                errCode = -1;
                errString = "Chuck模块未初始化！";
                goto _end;
            }
            //发送运动指令
            if ((errCode = HTM.RSMove(dev_chuck.a_Y, RSPos, speed)) < 0)
            {
                errString = "Chuck模块Y轴运动指令发送失败！";
                goto _end;
            }
            // 3. 等待运动完成
            if ((errCode = HTM.Done(dev_chuck.a_Y)) < 0)
            {
                errString = "Chuck模块Y轴运动失败！";
                goto _end;
            }
            ret = true;
            _end:
            return ret;
        }
        /// <summary>
        /// Y轴绝对运动,by LPY
        /// </summary>
        /// <param name="yPos">终止位置</param>
        /// <param name="mode">运动模式</param>
        /// <returns>返回bool类型表示成功或者失败</returns>
        public Boolean Y_Move(Double yPos)
        {
            if (runMode == Convert.ToInt16(SystemRunMode.MODE_OFFLINE))
            {
                return true;
            }
            Boolean ret = false;
            //确认模块ready
            if (ready == false)
            {
                errCode = -1;
                errString = "Chuck模块未初始化！";
                goto _end;
            }
            //发送运动指令
            if ((errCode = HTM.ASMove(dev_chuck.a_Y, yPos, speed)) < 0)
            {
                errString = "Chuck模块Y轴运动指令发送失败！";
                goto _end;
            }
            // 3. 等待运动完成
            if ((errCode = HTM.Done(dev_chuck.a_Y)) < 0)
            {
                errString = "Chuck模块Y轴运动失败！";
                goto _end;
            }
            ret = true;
            _end:
            return ret;
        }
        /// <summary>
        /// X轴相对对运动,by LPY
        /// </summary>
        /// <param name="xPos">终止位置</param>
        /// <param name="mode">运动模式</param>
        /// <returns>返回bool类型表示成功或者失败</returns>
        public Boolean X_RSMove(Double RSPos, Double speed = 1.0)
        {
            if (runMode == Convert.ToInt16(SystemRunMode.MODE_OFFLINE))
            {
                return true;
            }
            Boolean ret = false;
            //确认模块ready
            if (ready == false)
            {
                errCode = -1;
                errString = "Chuck模块未初始化！";
                goto _end;
            }
            //发送运动指令
            if ((errCode = HTM.RSMove(dev_chuck.a_X, RSPos, speed)) < 0)
            {
                errString = "Chuck模块X轴运动指令发送失败！";
                goto _end;
            }
            // 3. 等待运动完成
            if ((errCode = HTM.Done(dev_chuck.a_X)) < 0)
            {
                errString = "Chuck模块X轴运动失败！";
                goto _end;
            }
            ret = true;
            _end:
            return ret;
        }
        /// <summary>
        /// X轴绝对运动,by LPY
        /// </summary>
        /// <param name="xPos">终止位置</param>
        /// <param name="mode">运动模式</param>
        /// <returns>返回bool类型表示成功或者失败</returns>
        public Boolean X_Move(Double xPos)
        {
            if (runMode == Convert.ToInt16(SystemRunMode.MODE_OFFLINE))
            {
                return true;
            }
            Boolean ret = false;
            //确认模块ready
            if (ready == false)
            {
                errCode = -1;
                errString = "Chuck模块未初始化！";
                goto _end;
            }
            //发送运动指令
            if ((errCode = HTM.ASMove(dev_chuck.a_X, xPos, speed)) < 0)
            {
                errString = "Chuck模块X轴运动指令发送失败！";
                goto _end;
            }
            // 3. 等待运动完成
            if ((errCode = HTM.Done(dev_chuck.a_X)) < 0)
            {
                errString = "Chuck模块X轴运动失败！";
                goto _end;
            }
            ret = true;
            _end:
            return ret;
        }
        /// <summary>
        /// X、Y轴绝对运动,by LPY
        /// </summary>
        /// <param name="xPos">X终止位置</param>
        /// <param name="yPos">Y终止位置</param>
        /// <param name="mode">运动模式</param>
        /// <returns>返回bool类型表示成功或者失败</returns>
        public Boolean XZ_Move(Double xPos, Double zPos)
        {
            if (runMode == Convert.ToInt16(SystemRunMode.MODE_OFFLINE))
            {
                return true;
            }
            Boolean ret = false;
            //确认模块ready
            if (ready == false)
            {
                errCode = -1;
                errString = "Chuck模块未初始化！";
                goto _end;
            }
            //发送运动指令
            if ((errCode = HTM.ASMove(dev_chuck.a_X, xPos, speed)) < 0)
            {
                errString = "Chuck模块X轴运动指令发送失败！";
                goto _end;
            }
            if ((errCode = HTM.ASMove(dev_chuck.a_Z, zPos, speed)) < 0)
            {
                errString = "Chuck模块Z轴运动指令发送失败！";
                goto _end;
            }
            // 3. 等待运动完成
            if ((errCode = HTM.Done(dev_chuck.a_X)) < 0)
            {
                errString = "Chuck模块X轴运动失败！";
                goto _end;
            }

            if ((errCode = HTM.Done(dev_chuck.a_Z)) < 0)
            {
                errString = "Chuck模块Z轴运动失败！";
                goto _end;
            }
            ret = true;
            _end:
            return ret;
        }
        /// <summary>
        /// X、Y轴绝对运动,by LPY
        /// </summary>
        /// <param name="xPos">X终止位置</param>
        /// <param name="yPos">Y终止位置</param>
        /// <param name="mode">运动模式</param>
        /// <returns>返回bool类型表示成功或者失败</returns>
        public Boolean XY_Move(Double xPos, Double yPos)
        {
            if (runMode == Convert.ToInt16(SystemRunMode.MODE_OFFLINE))
            {
                return true;
            }
            Boolean ret = false;
            //确认模块ready
            if (ready == false)
            {
                errCode = -1;
                errString = "Chuck模块未初始化！";
                goto _end;
            }
            //发送运动指令
            if ((errCode = HTM.ASMove(dev_chuck.a_X, xPos, speed)) < 0)
            {
                errString = "Chuck模块X轴运动指令发送失败！";
                goto _end;
            }
            if ((errCode = HTM.ASMove(dev_chuck.a_Y, yPos, speed)) < 0)
            {
                errString = "Chuck模块Y轴运动指令发送失败！";
                goto _end;
            }
            // 3. 等待运动完成
            if ((errCode = HTM.Done(dev_chuck.a_X)) < 0)
            {
                errString = "Chuck模块X轴运动失败！";
                goto _end;
            }
            if ((errCode = HTM.Done(dev_chuck.a_Y)) < 0)
            {
                errString = "Chuck模块Y轴运动失败！";
                goto _end;
            }
            ret = true;
            _end:
            return ret;
        }
        /// <summary>
        /// X、Y、Z轴绝对运动,by LPY
        /// </summary>
        /// <param name="xPos">X终止位置</param>
        /// <param name="yPos">Y终止位置</param>
        /// <param name="zPos">Z终止位置</param>
        /// <param name="mode">运动模式</param>
        /// <returns>返回bool类型表示成功或者失败</returns>
        public Boolean XYZ_Move(Double xPos, Double yPos, Double zPos)
        {
            if (runMode == Convert.ToInt16(SystemRunMode.MODE_OFFLINE))
            {
                return true;
            }
            Boolean ret = false;
            //确认模块ready
            if (ready == false)
            {
                errCode = -1;
                errString = "Chuck模块未初始化！";
                goto _end;
            }
            //发送运动指令
            if ((errCode = HTM.ASMove(dev_chuck.a_X, xPos, speed)) < 0)
            {
                errString = "Chuck模块X轴运动指令发送失败！";
                goto _end;
            }
            if ((errCode = HTM.ASMove(dev_chuck.a_Y, yPos, speed)) < 0)
            {
                errString = "Chuck模块Y轴运动指令发送失败！";
                goto _end;
            }
            if ((errCode = HTM.ASMove(dev_chuck.a_Z, zPos, speed)) < 0)
            {
                errString = "Chuck模块Z轴运动指令发送失败！";
                goto _end;
            }
            // 3. 等待运动完成
            if ((errCode = HTM.Done(dev_chuck.a_X)) < 0)
            {
                errString = "Chuck模块X轴运动失败！";
                goto _end;
            }
            if ((errCode = HTM.Done(dev_chuck.a_Y)) < 0)
            {
                errString = "Chuck模块Y轴运动失败！";
                goto _end;
            }

            if ((errCode = HTM.Done(dev_chuck.a_Z)) < 0)
            {
                errString = "Chuck模块Z轴运动失败！";
                goto _end;
            }
            ret = true;
            _end:
            return ret;
        }
        /// <summary>
        /// XY轴做插补运动,by LPY
        /// </summary>
        /// <param name="xPos">X终止位置</param>
        /// <param name="yPos">Y终止位置</param>
        /// <returns>返回bool类型表示成功或者失败</returns>
        public bool XY_LineInterp(Double xPos, Double yPos)
        {
            if (runMode == Convert.ToInt16(SystemRunMode.MODE_OFFLINE))
            {
                return true;
            }
            bool ret = false;
            //确认模块ready
            if (ready == false)
            {
                errCode = -1;
                errString = "Chuck模块未初始化！";
                goto _end;
            }
            Int32[] axisList = new Int32[2] { dev_chuck.a_X, dev_chuck.a_Y };
            Double[] posList = new Double[2] { xPos, yPos };

            if ((errCode = HTM.Line(dev_chuck.a_X, dev_chuck.a_Y, xPos, yPos, 1.0, HTM.MotionMode.AS)) < 0)
            {
                errString = "发送XY轴插补运动指令失败！";
                goto _end;
            }
            // 3. 等待运动完成
            if ((errCode = HTM.Done(dev_chuck.a_X)) < 0)
            {
                errString = "Chuck模块X轴运动失败！";
                goto _end;
            }
            if ((errCode = HTM.Done(dev_chuck.a_Y)) < 0)
            {
                errString = "Chuck模块Y轴运动失败！";
                goto _end;
            }
            ret = true;
            _end:
            return ret;
        }
        //点位操作
        /// <summary>
        /// 获得当前相机X轴的点位,by TWL
        /// </summary>
        public double GetXPos()
        {
            return HTM.GetFbkPos(dev_chuck.a_X);
        }
        /// <summary>
        /// 获得当前相机X轴的点位,by TWL
        /// </summary>
        public double GetYPos()
        {
            return HTM.GetFbkPos(dev_chuck.a_Y);
        }
        /// <summary>
        /// 获得当前相机X轴的点位,by TWL
        /// </summary>
        public double GetZPos()
        {
            return HTM.GetFbkPos(dev_chuck.a_Z);
        }
        /// <summary>
        /// 获得当前X轴的点位作为上料点位,by LPY
        /// </summary>
        public void GetXLoadPos()
        {
            x_Load = HTM.GetFbkPos(dev_chuck.a_X);
        }
        /// <summary>
        /// 获取当前X轴的点位作为下料点位,by LPY
        /// </summary>
        public void GetXUnloadPos()
        {
            x_Unload = HTM.GetFbkPos(dev_chuck.a_X);
        }
        /// <summary>
        /// 获取当前X轴的点位作为第一颗芯片坐标x,by LPY
        /// </summary>
        public void GetXFirstDiePos()
        {
            x_FirstDie = HTM.GetFbkPos(dev_chuck.a_X);
        }
        /// <summary>
        /// 获取当前Y轴的点位作为第一颗芯片坐标y,by LPY
        /// </summary>
        public void GetYFirstDiePos()
        {
            y_FirstDie = HTM.GetFbkPos(dev_chuck.a_Y);
        }
        /// <summary>
        /// 获取当前Z轴的点位作为第一颗芯片坐标z,by LPY
        /// </summary>
        public void GetZFirstDiePos()
        {
            z_FirstDie = HTM.GetFbkPos(dev_chuck.a_Z);
        }
        /// <summary>
        /// 获取当前Z轴点位作为Z轴安全位,by LPY
        /// </summary>
        public void GetZSafePos()
        {
            z_Safe = HTM.GetFbkPos(dev_chuck.a_Z);
        }
        /// <summary>
        /// 获取当前X轴的点位作为二维码识别坐标x,by LPY
        /// </summary>
        public void GetXCodePos()
        {
            x_ScanCode = HTM.GetFbkPos(dev_chuck.a_X);
        }
        /// <summary>
        /// 获取当前Y轴的点位作为二维码识别坐标y,by LPY
        /// </summary>
        public void GetYCodePos()
        {
            y_ScanCode = HTM.GetFbkPos(dev_chuck.a_Y);
        }
        /// <summary>
        /// 获取当前Z轴的点位作为二维码识别坐标z,by LPY
        /// </summary>
        public void GetZCodePos()
        {
            z_ScanCode = HTM.GetFbkPos(dev_chuck.a_Z);
        }
        /// <summary>
        /// 获得Z轴触发起点位置
        /// </summary>
        public void GetZTrigStartPos()
        {
            z_TrigStart = HTM.GetFbkPos(dev_chuck.a_Z);
        }
        /// <summary>
        /// 获得Z轴触发终点位置
        /// </summary>
        public void GetZTrigEndPos()
        {
            z_TrigEnd = HTM.GetFbkPos(dev_chuck.a_Z);
        }
        /// <summary>
        /// 获得系统标定某个芯片的中心
        /// </summary>
        public void GetCalibDieCenter()
        {
            x_Calib = HTM.GetFbkPos(dev_chuck.a_X);
            y_Calib = HTM.GetFbkPos(dev_chuck.a_Y);
            z_Calib = HTM.GetFbkPos(dev_chuck.a_Z);
        }

        //动作
        /// <summary>
        /// 移动到上料位准备接料,by LPY
        /// </summary>
        /// <returns>返回bool类型表示成功或者失败</returns>
        public Boolean MoveToLoadWait(bool pressPlate = true, Boolean loadMannual = false)
        {
            if (runMode == Convert.ToInt16(SystemRunMode.MODE_OFFLINE))
            {
                return true;
            }
            Boolean ret = false;

            if (Z_Move(z_Safe) == false)
            {
                errString = "Z轴运动至安全位失败";
                goto _end;
            }
            if (pressPlate == true)
            {
                HTM.WriteDO(dev_chuck.o_Vacum, 0);
                HTM.WriteDO(dev_chuck.o_Plate, 0); //压板气缸取反
                if (HTM.WaitDI(dev_chuck.i_Vacum, 0, 5) < 0)
                {
                    errString = "基板载台真空关闭超时！";
                    goto _end;
                }
                if (HTM.WaitDI(dev_chuck.i_PlateBehindUp, dev_chuck.i_PlateFrontUp, 5) < 0)
                {
                    errString = "载台压板气缸抬升超时！";
                    goto _end;
                }
            }
            else
            {
                if (operation == Operation.VACUUM)
                {
                    HTM.WriteDO(dev_chuck.o_Vacum, 0);
                }
                if (operation == Operation.PLATE)
                {
                    HTM.WriteDO(dev_chuck.o_Plate, 0); //压板气缸取反
                }
                if (operation == Operation.VACUUM)
                {
                    if (HTM.WaitDI(dev_chuck.i_Vacum, 0, 5) < 0)
                    {
                        errString = "基板载台真空关闭超时！";
                        goto _end;
                    }
                }
                if (operation == Operation.PLATE)
                {
                    if (HTM.WaitDI(dev_chuck.i_PlateFrontUp, dev_chuck.i_PlateBehindUp, 5) < 0)
                    {
                        errString = "压板气缸抬升超时！";
                        goto _end;
                    }
                }
            }
            if (X_Move(x_Load) == false)
            {
                errString = "Chuck模块X轴运动至上料位置失败！";
                goto _end;
            }
            HTM.WriteDO(dev_chuck.o_Base, Convert.ToInt32(loadMannual));
            if (!loadMannual)
            {
                if (HTM.WaitDI(dev_chuck.i_BaseDown, 1, 5) < 0)
                {
                    errString = "基板载台气缸下降超时！";
                    goto _end;
                }
            }
            else
            {
                if (HTM.WaitDI(dev_chuck.i_BaseUp, 1, 5) < 0)
                {
                    errString = "基板载台气缸上升超时！";
                    goto _end;
                }
            }

            ret = true;
            _end:
            return ret;
        }

        public Boolean LoadMannually(bool io_effect = true)
        {
            if (runMode == Convert.ToInt16(SystemRunMode.MODE_OFFLINE))
            {
                return true;
            }
            Boolean ret = false;
            if (io_effect)
            {
                HTM.WriteDO(dev_chuck.o_Base, 1);
                if (HTM.WaitDI(dev_chuck.i_BaseUp, 1, 5) < 0)
                {
                    errString = "基板载台气缸上升超时！";
                    goto _end;
                }
            }
            ret = true;
            _end:
            return ret;
        }
        /// <summary>
        /// 移动到二维码拍照位,by LPY
        /// </summary>
        /// <returns>返回bool类型表示成功或者失败</returns>
        public Boolean MoveToScanCodePos()
        {
            if (runMode == Convert.ToInt16(SystemRunMode.MODE_OFFLINE))
            {
                return true;
            }
            Boolean ret = false;
            //if(!CatchFrame())
            //    goto _end;
            if (XY_Move(x_ScanCode, y_ScanCode) == false)
            { goto _end; }
            ret = true;
            _end:
            return ret;
        }


        /// <summary>
        /// 移动到标定点位，并给出位置触发信号，by LPY
        /// </summary>
        /// <param name="m">某一芯片行列信息以及对应的xyz信息</param>
        /// <returns>返回bool类型表示成功或者失败</returns>
        public bool MoveToCalibPos(bool io_effect = true)
        {

            if (runMode == Convert.ToInt16(SystemRunMode.MODE_OFFLINE))
            {
                HTLog.Debug("运动到标定位置成功！");
                return true;
            }
            bool ret = false;
            if (io_effect)
            {
                HTM.WriteDO(dev_chuck.o_Base, 1);
                if (operation == Operation.VACUUM)
                {
                    SwitchVacum(1);
                }
                if (operation == Operation.PLATE)
                {
                    HTM.WriteDO(dev_chuck.o_Plate, 1); //压板气缸取反
                }
                if (HTM.WaitDI(dev_chuck.i_BaseUp, 1, 5) < 0)
                {
                    errString = "基板载台气缸上升超时！";
                    goto _end;
                }
                if (operation == Operation.PLATE)
                {
                    if (HTM.WaitDI(dev_chuck.i_PlateFrontDown, dev_chuck.i_PlateBehindDown, 5) < 0)
                    {
                        errString = "压板气缸下降超时！";
                        goto _end;
                    }
                }
            }
            if (XYZ_Move(m.x, m.y, m.z) == false)
            {
                goto _end;
            }
            ret = true;
            _end:
            return ret;
        }
        /// <summary>
        /// 移动到扫描点位，并给出位置触发信号，by LPY
        /// </summary>
        /// <param name="m">某一芯片行列信息以及对应的xyz信息</param>
        /// <returns>返回bool类型表示成功或者失败</returns>
        public bool ScanPoint(bool pressPlate = true)
        {
            if (runMode == Convert.ToInt16(SystemRunMode.MODE_OFFLINE))
            {
                HTLog.Debug("扫描点位成功！");
                return true;
            }
            bool ret = false;
            if (pressPlate == true)
            {
                HTM.WriteDO(dev_chuck.o_Base, 1);
                SwitchVacum(1);
                HTM.WriteDO(dev_chuck.o_Plate, 1); //压板气缸取反
                if (HTM.WaitDI(dev_chuck.i_BaseUp, 1, 5) < 0)
                {
                    errString = "基板载台气缸上升超时！";
                    goto _end;
                }
                if (HTM.WaitDI(dev_chuck.i_PlateBehindDown, dev_chuck.i_PlateFrontDown, 5) < 0)
                {
                    errString = "压板气缸下降超时！";
                    goto _end;
                }
            }
            else
            {
                HTM.WriteDO(dev_chuck.o_Base, 1);
                if (operation == Operation.VACUUM)
                {
                    SwitchVacum(1);
                }
                if (operation == Operation.PLATE)
                {
                    HTM.WriteDO(dev_chuck.o_Plate, 1); //压板气缸取反
                }
                if (HTM.WaitDI(dev_chuck.i_BaseUp, 1, 5) < 0)
                {
                    errString = "基板载台气缸上升超时！";
                    goto _end;
                }
                if (operation == Operation.PLATE)
                {
                    if (HTM.WaitDI(dev_chuck.i_PlateFrontDown, dev_chuck.i_PlateBehindDown, 5) < 0)
                    {
                        errString = "压板气缸下降超时！";
                        goto _end;
                    }
                }
            }
            if (XYZ_Move(m.x, m.y, m.z) == false)
            {
                goto _end;
            }
            ret = true;
            _end:
            return ret;
        }
        /// <summary>
        /// 移动到下料位，并完成下料准备动作，by LPY
        /// </summary>
        /// <returns>返回bool类型表示成功或者失败</returns>
        /// <summary>
        /// 移动到下料位，by LPY
        /// </summary>
        /// <returns>返回bool类型表示成功或者失败</returns>
        public Boolean MoveToUnloadPos()
        {
            if (runMode == Convert.ToInt16(SystemRunMode.MODE_OFFLINE))
            {
                return true;
            }
            Boolean ret = false;

            if (XZ_Move(x_Unload, z_Safe) == false)
            {
                goto _end;
            }
            ret = true;
            _end:
            return ret;
        }
        /// <summary>
        /// 固定料片
        /// </summary>
        /// <param name="pressPlate"></param>
        /// <returns></returns>
        public Boolean CatchFrame(bool pressPlate = true)
        {
            bool ret = false;
            if (runMode == Convert.ToInt16(SystemRunMode.MODE_OFFLINE))
            {
                return true;
            }

            //if (!CheckErr(HTM.WaitDI, dev_chuck.i_PlateBehindDown,dev_chuck.i_PlateFrontDown, 1, 5))
            //{
            //    errString = "压板气缸下降超时失败!";
            //    ret = false;
            //    goto _end;
            //}

            //1.载台升起
            if (App.obj_SystemConfig.CatchMode > 0)
            {
                if (!CheckErr(HTM.WriteDO, dev_chuck.o_Base, 1))
                {
                    errString = "载台气缸动作失败!";
                    ret = false;
                    goto _end;
                }
                if (!CheckErr(HTM.WaitDI, dev_chuck.i_BaseUp, 1, 5))
                {
                    errString = "载台升起超时失败!";
                    ret = false;
                    goto _end;
                }
            }
            //2.真空打开
            if (App.obj_SystemConfig.CatchMode > 1)
            {
                if (!CheckErr(HTM.WriteDO, dev_chuck.o_Vacum, 1))
                {
                    errString = string.Format("{0}复位操作失败!", HTM.GetIoName(dev_chuck.o_Vacum));
                    goto _end;
                }
            }
            //3.压板下降
            if (!CheckErr(HTM.WriteDO, dev_chuck.o_Plate, 0))
            {
                errString = "压板气缸动作失败!";
                ret = false;
                goto _end;
            }
            //if (!CheckErr(HTM.WaitDI, dev_chuck.i_Vacum, 1, 5))
            //{
            //    errString = string.Format("真空关闭超时!");
            //    goto _end;
            //}
            ret = true;
            _end:
            return ret;
        }
        /// <summary>
        /// 下料工作，by LPY
        /// </summary>
        /// <returns>返回bool类型表示成功或者失败</returns>
        public Boolean LooseFrame(bool pressPlate = true)
        {
            bool ret = false;
            if (runMode == Convert.ToInt16(SystemRunMode.MODE_OFFLINE))
            {
                return true;
            }
            //1.压板上升
            if (!CheckErr(HTM.WriteDO, dev_chuck.o_Plate, 1))
            {
                errString = "压板气缸动作失败!";
                ret = false;
                goto _end;
            }
            //if (!CheckErr(HTM.WaitDI, dev_chuck.i_PlateFrontUp, dev_chuck.i_PlateBehindUp, 1, 5))
            //{
            //    errString = "压板气缸抬起超时失败!";
            //    ret = false;
            //    goto _end;
            //}

            //2.关闭真空，瞬间吹气

            if (App.obj_SystemConfig.CatchMode > 1)
            {
                if (!CheckErr(HTM.WriteDO, dev_chuck.o_Vacum, 0))
                {
                    errString = string.Format("{0}复位操作失败!", HTM.GetIoName(dev_chuck.o_Vacum));
                    goto _end;
                }
                if (!CheckErr(HTM.WriteDO, dev_chuck.o_Blow, 1))
                {
                    errString = string.Format("{0}指令失败!", HTM.GetIoName(dev_chuck.o_Blow));
                    goto _end;
                }
                if (!CheckErr(HTM.WriteDO, dev_chuck.o_Blow, 0))
                {
                    errString = string.Format("{0}指令失败!", HTM.GetIoName(dev_chuck.o_Blow));
                    goto _end;
                }
                if (!CheckErr(HTM.WaitDI, dev_chuck.i_Vacum, 0, 5))
                {
                    errString = string.Format("真空关闭超时!");
                    goto _end;
                }
            }
            //3.载台降下
            if (App.obj_SystemConfig.CatchMode > 0)
            {
                if (!CheckErr(HTM.WriteDO, dev_chuck.o_Base, 0))
                {
                    errString = "载台气缸动作失败!";
                    ret = false;
                    goto _end;
                }
                if (!CheckErr(HTM.WaitDI, dev_chuck.i_BaseUp, 0, 5))
                {
                    errString = "载台气缸下降超时失败!";
                    ret = false;
                    goto _end;
                }
            }
            ret = true;
            _end:
            return ret;
        }
        /// <summary>
        /// Z轴移动到安全位，by LPY
        /// </summary>
        /// <returns>返回bool类型表示成功或者失败</returns>
        public Boolean MoveToSafeZPos()
        {
            if (runMode == Convert.ToInt16(SystemRunMode.MODE_OFFLINE))
            {
                return true;
            }
            Boolean ret = false;
            if (Z_Move(z_Safe) == false)
            {
                goto _end;
            }
            ret = true;
            _end:
            return ret;
        }
        /// <summary>
        /// 移动到聚焦位，by LPY
        /// </summary>
        /// <returns>返回bool类型表示成功或者失败</returns>
        public bool MoveToZFocus()
        {
            if (runMode == Convert.ToInt16(SystemRunMode.MODE_OFFLINE))
            {
                HTLog.Debug("Z轴移动到聚焦位成功！");
                return true;
            }
            bool ret = false;
            if (Z_Move(z_focus) == false)
            {
                goto _end;
            }
            ret = true;
            _end:
            return ret;
        }
        /// <summary>
        /// 移动至第一颗芯片的点位
        /// </summary>
        /// <returns>返回bool类型表示成功或者失败</returns>
        public bool MoveToFirstDiePos(bool pressPlate = true)
        {
            if (runMode == Convert.ToInt16(SystemRunMode.MODE_OFFLINE))
            {
                return true;
            }
            Boolean ret = false;
            if (pressPlate == true)
            {
                HTM.WriteDO(dev_chuck.o_Vacum, 1);
                HTM.WriteDO(dev_chuck.o_Base, 1);
                HTM.WriteDO(dev_chuck.o_Plate, 1); //压板气缸取反
                if (HTM.WaitDI(dev_chuck.i_Vacum, 1, 5) < 0)
                {
                    errString = "基板载台真空信号打开失败！";
                    goto _end;
                }
                if (HTM.WaitDI(dev_chuck.i_BaseUp, 1, 5) < 0)
                {
                    errString = "基板载台气缸上升超时！";
                    goto _end;
                }
                if (HTM.WaitDI(dev_chuck.i_PlateBehindDown, dev_chuck.i_PlateFrontDown, 5) < 0)
                {
                    errString = "压板气缸下降超时！";
                    goto _end;
                }
            }
            else
            {
                if (operation == Operation.VACUUM)
                {
                    HTM.WriteDO(dev_chuck.o_Vacum, 1);
                }
                HTM.WriteDO(dev_chuck.o_Base, 1);
                if (operation == Operation.PLATE)
                {
                    HTM.WriteDO(dev_chuck.o_Plate, 1); //压板气缸取反
                }
                if (HTM.WaitDI(dev_chuck.i_BaseUp, 1, 5) < 0)
                {
                    errString = "基板载台气缸上升超时！";
                    goto _end;
                }
                if (operation == Operation.PLATE)
                {
                    if (HTM.WaitDI(dev_chuck.i_PlateFrontDown, dev_chuck.i_PlateBehindDown, 5) < 0)
                    {
                        errString = "压板气缸下降超时！";
                        goto _end;
                    }
                }
                if (operation == Operation.VACUUM)
                {
                    if (HTM.WaitDI(dev_chuck.i_Vacum, 1, 5) < 0)
                    {
                        errString = "基板载台真空打开超时！";
                        goto _end;
                    }
                }
            }
            if (XYZ_Move(x_FirstDie, y_FirstDie, z_FirstDie) == false)
            { goto _end; }
            ret = true;
            _end:
            return ret;
        }
        /// <summary>
        /// 移动至系统标定的某一个芯片中心
        /// </summary>
        /// <returns>返回bool类型表示成功或者失败</returns>
        public bool MoveToCalibDieCenter(bool io_effect = true)
        {
            if (runMode == Convert.ToInt16(SystemRunMode.MODE_OFFLINE))
            {
                return true;
            }
            Boolean ret = false;
            if (io_effect)
            {
                if (operation == Operation.VACUUM)
                {
                    HTM.WriteDO(dev_chuck.o_Vacum, 1);
                }
                HTM.WriteDO(dev_chuck.o_Base, 1);
                if (operation == Operation.PLATE)
                {
                    HTM.WriteDO(dev_chuck.o_Plate, 1); //压板气缸取反
                }
                if (HTM.WaitDI(dev_chuck.i_BaseUp, 1, 5) < 0)
                {
                    errString = "基板载台气缸上升超时！";
                    goto _end;
                }
                if (operation == Operation.PLATE)
                {
                    if (HTM.WaitDI(dev_chuck.i_PlateBehindDown, dev_chuck.i_PlateFrontDown, 1, 5) < 0)
                    {
                        errString = "压板气缸下降超时！";
                        goto _end;
                    }
                }
                if (operation == Operation.VACUUM)
                {
                    if (HTM.WaitDI(dev_chuck.i_Vacum, 1, 5) < 0)
                    {
                        errString = "基板载台真空打开超时！";
                        goto _end;
                    }
                }
            }
            if (XYZ_Move(x_Calib, y_Calib, z_Calib) == false)
            { goto _end; }
            ret = true;
            _end:
            return ret;
        }
        /// <summary>
        /// 获得参考原点
        /// </summary>
        public void GetRefPos()
        {
            ref_x = HTM.GetFbkPos(dev_chuck.a_X);
            ref_y = HTM.GetFbkPos(dev_chuck.a_Y);
            ref_z = HTM.GetFbkPos(dev_chuck.a_Z);
        }
        public void GetScanCodePos()
        {
            x_ScanCode = HTM.GetFbkPos(dev_chuck.a_X);
            y_ScanCode = HTM.GetFbkPos(dev_chuck.a_Y);
            z_ScanCode = HTM.GetFbkPos(dev_chuck.a_Z);
        }
        public void GetMarkPenPosXY()
        {
            X_MarkingPen = HTM.GetFbkPos(dev_chuck.a_X);
            Y_MarkingPen = HTM.GetFbkPos(dev_chuck.a_Y);
        }

        public void GetMarkPenPosZ()
        {
            AZMarkingPen = HTM.GetFbkPos(dev_chuck.a_Z);
        }
        public void calMarkPenOffset()
        {
            RXMarkingPen = X_MarkingPen - ref_x;
            RYMarkingPen = Y_MarkingPen - ref_y;
        }

        public Boolean MoveMarkingPenXy()
        {
            if (runMode == Convert.ToInt16(SystemRunMode.MODE_OFFLINE))
            {
                HTLog.Debug("移动至打点器点位成功！");
                return true;
            }
            bool ret = false;
            if (XY_Move(X_MarkingPen, Y_MarkingPen) == false)
            {
                goto _end;
            }
            ret = true;
            _end:
            return ret;
        }

        public Boolean MoveMarkingPenZ()
        {
            if (runMode == Convert.ToInt16(SystemRunMode.MODE_OFFLINE))
            {
                HTLog.Debug("移动至打点器点位成功！");
                return true;
            }
            bool ret = false;
            if (Z_Move(AZMarkingPen) == false)
            {
                goto _end;
            }
            ret = true;
            _end:
            return ret;
        }
        /// <summary>
        /// 获得第一颗芯片的位置
        /// </summary>
        /// <param name="x_offset">x方向距离原点的偏移量</param>
        /// <param name="y_offset">y方向距离原点的偏移量</param>
        public void CalFistDiePos(Double x_offset, Double y_offset)
        {
            x_FirstDie = ref_x + x_offset;
            y_FirstDie = ref_y + y_offset;
            z_FirstDie = ref_z;
        }
        public void CalFistDiePos()
        {
            x_FirstDie = HTM.GetFbkPos(dev_chuck.a_X);
            y_FirstDie = HTM.GetFbkPos(dev_chuck.a_Y);
            z_FirstDie = HTM.GetFbkPos(dev_chuck.a_Z);
        }
        /// <summary>
        /// 获取Z轴触发板 触发次数
        /// </summary>
        /// <param name="trigNum"></param>
        /// <returns></returns>
        public bool GetZTrigCnt(out int trigNum)
        {
            trigNum = 0;
            trigNum = HTM.GetTrigCnt(dev_chuck.PosTrigIdx);
            return true;
        }
        /// <summary>
        /// Z轴移动到聚焦初始位置
        /// </summary>
        /// <returns>返回bool类型表示成功或者失败</returns>
        public bool MoveToZStart()
        {
            bool ret = false;

            if (runMode == Convert.ToInt16(SystemRunMode.MODE_OFFLINE))
            {
                return true;
            }
            if (Ready == false)
            {
                errString = "Chuck模块未初始化！";
                return false;
            }
            if (HTM.SetLinTrigEnable(dev_chuck.PosTrigIdx, 0) != 0)
            {
                errString = string.Format("清除板卡触发点位失败");
                goto _end;
            }
            _preValue = (z_TrigEnd - z_TrigStart > 0) ? 0.05 : -0.05;
            if (Z_Move(z_TrigStart - _preValue) == false)
            {
                errString = string.Format("z轴运动失败");
                goto _end;
            }
            ret = true;
            _end:
            return ret;
        }
        /// <summary>
        /// Z轴移动到聚焦终止位置
        /// </summary>
        /// <returns>返回bool类型表示成功或者失败</returns>

        public bool Move2ZEnd()
        {
            if (runMode == Convert.ToInt16(SystemRunMode.MODE_OFFLINE))
            {
                return true;
            }
            if (Ready == false)
            {
                errString = "Chuck模块未初始化！";
                return false;
            }
            double maxSpeed = z_TrigStep * Fps;
            HTM.MOTION_PARA mp = HTM.GetMotionPara(dev_chuck.a_Z);
            mp.vMax = maxSpeed;
            //运动
            if ((errCode = HTM.Move(dev_chuck.a_Z, z_TrigEnd + _preValue, 1, HTM.MotionMode.AS, ref mp)) < 0)
            {
                errString = "发送轴运动指令失败";
                return false;
            }
            //检测轴是否运动完成
            if ((errCode = HTM.Done(dev_chuck.a_Z)) < 0)
            {
                errString = "轴运动到线性触发终点位置失败";
                return false;
            }
            return true;
        }
        /// <summary>
        /// Z轴移动到聚焦终止位置
        /// </summary>
        /// <param name="trigNum">触发点位数目</param>
        /// <returns></returns>
        public bool Move2ZEnd(out int trigNum)
        {
            trigNum = 0;
            if (runMode == Convert.ToInt16(SystemRunMode.MODE_OFFLINE))
            {
                return true;
            }
            if (Ready == false)
            {
                errString = "Chuck模块未初始化！";
                return false;
            }
            double maxSpeed = z_TrigStep * Fps;
            HTM.MOTION_PARA mp = HTM.GetMotionPara(dev_chuck.a_Z);
            mp.vMax = maxSpeed;
            //运动
            if ((errCode = HTM.Move(dev_chuck.a_Z, z_TrigEnd + _preValue, 1, HTM.MotionMode.AS, ref mp)) < 0)
            {
                errString = "发送轴运动指令失败";
                return false;
            }
            //检测轴是否运动完成
            if ((errCode = HTM.Done(dev_chuck.a_Z)) < 0)
            {
                errString = "轴运动到线性触发终点位置失败";
                return false;
            }
            trigNum = HTM.GetTrigCnt(dev_chuck.PosTrigIdx);
            return true;
        }
        /// <summary>
        /// 获取二维码的位置
        /// </summary>
        /// <param name="x_offset">x方向距离原点的偏移量</param>
        /// <param name="y_offset">y方向距离原点的偏移量</param>
        public void CalCodePos(Double x_offset, Double y_offset)
        {
            x_ScanCode = ref_x + x_offset;
            y_ScanCode = ref_y + y_offset;
            z_ScanCode = ref_z;
        }

        public bool Move2RefPos(bool pressPlate = true)
        {
            if (runMode == Convert.ToInt16(SystemRunMode.MODE_OFFLINE))
            {
                return true;
            }
            Boolean ret = false;
            if (pressPlate == true)
            {
                HTM.WriteDO(dev_chuck.o_Vacum, 1);
                HTM.WriteDO(dev_chuck.o_Base, 1);
                HTM.WriteDO(dev_chuck.o_Plate, 1); //压板气缸取反
                if (HTM.WaitDI(dev_chuck.i_Vacum, 1, 5) < 0)
                {
                    errString = "基板载台真空打开超时！";
                    goto _end;
                }
                if (HTM.WaitDI(dev_chuck.i_BaseUp, 1, 5) < 0)
                {
                    errString = "基板载台气缸上升超时！";
                    goto _end;
                }
                if (HTM.WaitDI(dev_chuck.i_PlateFrontDown, dev_chuck.i_PlateBehindDown, 5) < 0)
                {
                    errString = "压板气缸下降超时！";
                    goto _end;
                }
            }
            else
            {
                if (operation == Operation.VACUUM)
                {
                    HTM.WriteDO(dev_chuck.o_Vacum, 1);
                }
                HTM.WriteDO(dev_chuck.o_Base, 1);
                //HTM.WriteDO(dev_chuck.o_Limit, 1);
                if (operation == Operation.PLATE)
                {
                    HTM.WriteDO(dev_chuck.o_Plate, 1); //压板气缸取反
                }
                if (operation == Operation.VACUUM)
                {
                    if (HTM.WaitDI(dev_chuck.i_Vacum, 1, 5) < 0)
                    {
                        errString = "基板载台真空打开超时！";
                        goto _end;
                    }
                }
                if (HTM.WaitDI(dev_chuck.i_BaseUp, 1, 5) < 0)
                {
                    errString = "基板载台气缸上升超时！";
                    goto _end;
                }
                if (operation == Operation.PLATE)
                {
                    if (HTM.WaitDI(dev_chuck.i_PlateBehindDown, dev_chuck.i_PlateFrontDown, 5) < 0)
                    {
                        errString = "压板气缸下降超时！";
                        goto _end;
                    }
                }
            }
            if (XYZ_Move(ref_x, ref_y, ref_z) == false)
            { goto _end; }
            ret = true;
            _end:
            return ret;
        }

        public Boolean Move2MarkingPos(Double PosX, Double PosY)
        {
            Boolean ret = false;

            if (XYZ_Move(PosX + RXMarkingPen, PosY + RYMarkingPen, AZMarkingPen) == false)
            {
                goto _end;
            }

            ret = true;
            _end:
            return ret;
        }

        //硬触发信号

        public void SWPosTrig(int AIdx = 0)
        {
            switch (AIdx)
            {
                case 0:
                    HTM.SWPosTrig(dev_chuck.a_X);
                    break;
                case 1:
                    HTM.SWPosTrig(dev_chuck.a_Z);
                    break;
                default:
                    HTM.SWPosTrig(dev_chuck.a_Z);
                    break;
            }
        }

        public bool Move2ImagePosition(double x, double y, double z)
        {
            bool ret = false; ;
            if (XYZ_Move(x, y, z) == false)
            {
                errString = "相机三轴运动失败!";
                ret = false;
                goto _end;
            }
            ret = true;
            _end:
            return ret;
        }
        /// <summary>
        /// 在某位置开始喷码
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <param name="IsX"></param>
        /// <param name="rXorY"></param>
        /// <returns></returns>
        public bool EC_Printer(double x, double y, double z,bool IsX,double rXorY)
        {
            if (runMode == Convert.ToInt16(SystemRunMode.MODE_OFFLINE))
            {
                return true;
            }
            Boolean ret = false;
            //确认模块ready
            if (ready == false)
            {
                errCode = -1;
                errString = "Chuck模块未初始化！";
                goto _end;
            }
            if (App.obj_SystemConfig.Marking && App.obj_Pdt.UseMarker)
            {
                if (IsHaveInk() == 0)
                {
                    errCode = -1;
                    errString = "喷印器无墨水！";
                    goto _end;
                }
                if (App.obj_Chuck.IsJetRun() == 0)
                {
                    errCode = -1;
                    errString = "喷印器未启动！";
                    goto _end;
                }
            }
            else
            {
                errCode = -1;
                errString = "未启用喷印器！";
                goto _end;
            }
            //发送运动指令
            if (!XYZ_Move(x, y, z))
            {
                errString = "Chuck模块三轴运动失败！";
                goto _end;
            }
            App.ec_Jet.TriggerPrint();
            if (IsX)
            {
                if (!X_Move(x + rXorY))
                {
                    errString = "Chuck模块三轴运动失败！";
                    goto _end;
                }
                //if (!X_Move(x - rXorY))
                //{
                //    errString = "Chuck模块三轴运动失败！";
                //    goto _end;
                //}
            }
            else
            {
                if (!Y_Move(y + rXorY))
                {
                    errString = "Chuck模块三轴运动失败！";
                    goto _end;
                }
                //if (!Y_Move(y - rXorY))
                //{
                //    errString = "Chuck模块三轴运动失败！";
                //    goto _end;
                //}
            }
            ret = true;
        _end:
            return ret;
        }

        /// <summary>
        /// 直接开始喷码
        /// </summary>
        /// <param name="IsX"></param>
        /// <param name="rXorY"></param>
        /// <returns></returns>
        public bool EC_Printer(bool IsX, double rXorY)
        {
            if (runMode == Convert.ToInt16(SystemRunMode.MODE_OFFLINE))
            {
                return true;
            }
            Boolean ret = false;
            //确认模块ready
            if (ready == false)
            {
                errCode = -1;
                errString = "Chuck模块未初始化！";
                goto _end;
            }
            double x= App.obj_Chuck.GetXPos();
            double y= App.obj_Chuck.GetYPos();
            App.ec_Jet.TriggerPrint();
            if (IsX)
            {
                if (!X_Move(x + rXorY))
                {
                    errString = "Chuck模块三轴运动失败！";
                    goto _end;
                }
                if (!X_Move(x))
                {
                    errString = "Chuck模块三轴运动失败！";
                    goto _end;
                }
            }
            else
            {
                if (!Y_Move(y + rXorY))
                {
                    errString = "Chuck模块三轴运动失败！";
                    goto _end;
                }
                if (!Y_Move(y))
                {
                    errString = "Chuck模块三轴运动失败！";
                    goto _end;
                }
            }
            ret = true;
        _end:
            return ret;
        }
        #endregion

    }
}
