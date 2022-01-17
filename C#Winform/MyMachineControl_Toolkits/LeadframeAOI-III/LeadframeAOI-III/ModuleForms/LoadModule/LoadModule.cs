using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using HT_Lib;
using System.Data.SQLite;
using Utils;
using HalconDotNet;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using System.IO;
using System.ComponentModel;
namespace LeadframeAOI
{

    enum AxisIndex
    {
        /// <summary>
        /// Y轴  前后
        /// </summary>
        AXIS_Y = 0,
        /// <summary>
        /// Z轴  上下
        /// </summary>
        AXIS_Z,
        /// <summary>
        /// 料盒夹爪
        /// </summary>
        AXIS_Z_JAW,
        /// <summary>
        /// 下平台推送Y
        /// </summary>
        AXIS_BTM_PUSH,
        /// <summary>
        /// 推杆
        /// </summary>
        AXIS_ROD_PUSH,
        /// <summary>
        /// 传输夹爪X轴
        /// </summary>
        AXIS_X_TRS,
    }
    ///// <summary>
    ///// 功能：配置轴，IO的结构。
    ///// </summary>

    //上下料通用

    public struct Device_LoadUnload
    {
        #region 轴
        //上下料通用
        /// <summary>
        /// //抬升轴
        /// </summary>
        public int a_Z { get; set; }
        /// <summary>
        /// //夹爪Z轴
        /// </summary>                                     
        public int a_JawZ { get; set; }
        /// <summary>
        /// //调整Y 电机
        /// </summary>                                  
        public int a_Y { get; set; }
        /// <summary>
        /// //下平台直流Y
        /// </summary>                              
        public int a_Y_Btm { get; set; }
        /// <summary>
        /// 上下料传输机构夹爪运动轴
        /// </summary>
        public int a_JawX { get; set; }

        /// <summary>
        /// 上下传输机构夹爪闭合运动轴
        /// </summary>
        public int a_JawFrame { get; set; }
        //上料独有
        /// <summary>
        ///  //上片推片步进电机
        /// </summary>
        public int a_X_Push { get; set; }
        #endregion

        #region IO
        #region 共用部分
        /// <summary>
        /// 夹爪夹紧信号
        /// </summary>
        public int i_Jaw_Closed { get; set; }
        /// <summary>
        /// 上平台满仓信号
        /// </summary>
        public int i_Mgz_Full_TOP { get; set; }
        /// <summary>
        /// 下平台有无信号
        /// </summary>
        public int i_Mgz_Full_BTM { get; set; }
        /// <summary>
        /// Z轴步进刹车信号
        /// </summary>
        public int o_Z_Stopping { get; set; }
        /// <summary>
        /// 夹爪有无料盒信号
        /// </summary>
        public int i_Jaw_HaveMgz { get; set; }
        /// <summary>
        /// 传送台上/下料有无基板信号
        /// </summary>
        public int i_Transfer_HaveCst { get; set; }
        /// <summary>
        /// chuck台左右端检测cst传感器
        /// </summary>
        public int i_Chuck_HaveCst { get; set; }
        /// <summary>
        /// 传输机构夹爪气缸上
        /// </summary>
        public int i_Transfer_JawCylinder_Up { get; set; }
        /// <summary>
        /// 传输机构夹爪气缸下
        /// </summary>
        public int i_Transfer_JawCylinder_Down { get; set; }
        /// <summary>
        /// 传输机构夹爪气缸动作
        /// </summary>
        public int o_Transfer_JawCylinder { get; set; }
        /// <summary>
        /// 顶杆推送防撞信号
        /// </summary>
        public int i_PushCst_Err { get; set; }

        #endregion
        #region 上料独有
        /// <summary>
        /// 扫描Cassette信号
        /// </summary>
        public int i_HaveCst { get; set; }

        #endregion
        #region 下料独有
        /// <summary>
        /// 下料推杆 抬起气缸
        /// </summary>
        public int o_Unload_PushCstLiftCylinder { get; set; }
        /// <summary>
        /// 下料推杆机构气缸上信号
        /// </summary>
        public int i_Unload_PushCstLiftCylinderUp { get; set; }
        /// <summary>
        /// 下料推杆机构气缸下信号
        /// </summary>
        public int i_Unload_PushCstLiftCylinderDown { get; set; }

        /// <summary>
        /// 不良品箱夹紧信号
        /// </summary>
        public int i_Jaw_NgMgz_Closed { get; set; }
        /// <summary>
        /// 不良品箱松开信号
        /// </summary>
        public int i_Jaw_NgMgz_Opened { get; set; }
        /// <summary>
        /// 不良品箱夹紧电磁阀
        /// </summary>
        public int o_Jaw_NgMgz_Close { get; set; }
        /// <summary>
        /// 不良品箱侧边有无信号
        /// </summary>
        public int i_NgMgz_Side_ON { get; set; }
        /// <summary>
        /// 不良品箱底部有无信号
        /// </summary>
        public int i_NgMgz_Btm_ON { get; set; }
        /// <summary>
        /// 打点气缸上
        /// </summary>
        public int i_MarkCylinderUp { get; set; }
        /// <summary>
        /// 打点气缸下
        /// </summary>
        public int i_MarkCylinderDown { get; set; }
        /// <summary>
        /// 打点气缸
        /// </summary>
        public int o_MarkCylinder { get; set; }
        /// <summary>
        /// 料片曲翘信号
        /// </summary>
        public int i_FrameBendChecker { get; set; }
        #endregion

        //  public  
        #endregion
    }
    /// <summary>
    /// <summary>
    /// Class for Load/unload magzine module
    /// </summary>
    class LoadModule : BaseModule
    {
        #region 公共变量
        private Device_LoadUnload device_LoadUnload = new Device_LoadUnload();

        /// <summary>
        /// 是否继续上料盒
        /// </summary>
        public bool DoLoadMgz = false;
        //点位参数
        #region  上下料共有
        /// <summary>
        /// Y轴上料盒点位
        /// </summary>
        public Double y_LoadMgzPos = 0;
        /// <summary>
        /// Z轴上料盒点位
        /// </summary>
        public Double z_LoadMgzPos = 0;
        /// <summary>
        /// Y轴下料盒点位
        /// </summary>
        public Double y_UnLoadMgzPos = 0;
        /// <summary>
        /// Z轴下料盒点位
        /// </summary>
        public Double z_UnLoadMgzPos = 0;
        /// <summary>
        /// 夹爪打开位
        /// </summary>
        public Double z_JawOpenedPos = 0;
        /// <summary>
        /// 夹爪夹紧位
        /// </summary>
        public Double z_JawClosedPos = 0;
        /// <summary>
        /// 上/下料传输机构夹爪左端安全位
        /// </summary>
        public Double x_TransfeJawLeftSafeCstPos = 0;
        /// <summary>
        /// 上/下料传输机构夹爪右端安全位
        /// </summary>
        public Double x_TransfeJawRightSafeCstPos = 0;
        /// <summary>
        /// 左侧夹爪推片到载台位
        /// </summary>
        public Double x_ChuckLoadFramePos = 0;
        /// <summary>
        /// 左侧夹爪推片到等待位
        /// </summary>
        public Double x_ChuckWaitFramePos = 0;

        /// <summary>
        ///  下平台推料盒 安全位置
        /// </summary>
        public Double y_BtmPushMgzSafePos = 0;
        /// <summary>
        ///  Y轴上平台推料结束位  此点位取消  by M.Bing
        /// </summary>
        public Double y_UnLoadMgzPushOverPos = 0;
        /// <summary>
        /// Y轴上片初始点位
        /// </summary>
        public Double y_LoadUnLoadFramePos = 0;
        /// <summary>
        /// Z轴最上上片位， 也就是最上面一片的上片位
        /// </summary>
        public Double z_LoadUnLoadFramePos = 0;
        /// <summary>
        /// Z轴上片初始位， 也就是被推第一片的位置
        /// </summary>
        public Double z_LoadUnLoadFirstPos = 0;
        /// <summary>
        /// 导轨夹爪Z轴闭合位
        /// </summary>
        public Double z_JawFrameCatchPos=0;
        /// <summary>
        /// 导轨夹爪Z轴张开位
        /// </summary>
        public Double z_JawFrameLoosePos = 0;
        #endregion

        #region 上料独有
        /// <summary>
        /// 推杆安全位
        /// </summary>
        public Double x_PushRodSafePos = 0;
        /// <summary>
        /// 推杆等待位
        /// </summary>
        public Double x_PushRodWaitPos = 0;
        /// <summary>
        /// 推杆电机推入到位点位
        /// </summary>
        public Double x_PushRodOverPos = 0;
        #endregion
        

        #region 料盒及料片参数
        /// <summary>
        /// 料片层数
        /// </summary>
        public Int32 slotLayersCount = 1;
        /// <summary>
        /// 料片层高
        /// </summary>
        public Double heightSlotLayer = 0;
        /// <summary>
        /// 料片层偏y
        /// </summary>
        public Double heightSlotLayer_y = 0;
        /// <summary>
        /// 最底层料片距料盒底部的高度
        /// </summary>
        private Double heightLastSlot = 0;
        /// <summary>
        /// 料盒的总高度
        /// </summary>
        private Double mgzHeight = 0;
        /// <summary>
        /// 料片的长度
        /// </summary>
        public Double FrameLength = 0;
        #endregion

        #region 其他参数
        /// <summary>
        /// Chuck左传感器到chuck中心的距离
        /// </summary>
        private double letfSnsr2ChuckCentDis = 0;
        #endregion
        /// <summary>
        /// 运动超时时间
        /// </summary>
        private List<Double> scanPos = new List<Double>();

        [CategoryAttribute("流程属性"), DisplayNameAttribute("上料盒位-Y坐标(mm)"), DescriptionAttribute("料盒搬运机构Y轴,Y方向")]
        public double Y_LoadMgzPos { get {return y_LoadMgzPos; }set{y_LoadMgzPos = value;  } }
        [CategoryAttribute("流程属性"), DisplayNameAttribute("上料盒位-Z坐标(mm)"), DescriptionAttribute("料盒搬运机构Z轴,Z方向")]
        public double Z_LoadMgzPos { get {return z_LoadMgzPos; }set{z_LoadMgzPos = value;  } }
        [CategoryAttribute("流程属性"), DisplayNameAttribute("下料盒位-Y坐标(mm)"), DescriptionAttribute("料盒搬运机构Y轴,Y方向")]
        public double Y_UnLoadMgzPos { get {return y_UnLoadMgzPos; }set{y_UnLoadMgzPos = value;  } }
        [CategoryAttribute("流程属性"), DisplayNameAttribute("下料盒位-Z坐标(mm)"), DescriptionAttribute("料盒搬运机构Z轴,Z方向")]
        public double Z_UnLoadMgzPos { get {return z_UnLoadMgzPos; }set{z_UnLoadMgzPos = value;  } }
        [CategoryAttribute("流程属性"), DisplayNameAttribute("夹爪张开位(mm)"), DescriptionAttribute("料盒固料夹爪位置,Z方向")]
        public double Z_JawOpenedPos { get {return z_JawOpenedPos; }set{z_JawOpenedPos = value;  } }
        [CategoryAttribute("流程属性"), DisplayNameAttribute("夹爪闭合位(mm)"), DescriptionAttribute("料盒固料夹爪位置,Z方向")]
        public double Z_JawClosedPos { get {return z_JawClosedPos; }set{z_JawClosedPos = value;  } }

        [CategoryAttribute("流程属性"), DisplayNameAttribute("导轨左夹爪夹取闭合位(mm)"), DescriptionAttribute("导轨左侧夹爪位置,Z方向")]
        public double Z_JawFrameCatchPos { get { return z_JawFrameCatchPos; } set { z_JawFrameCatchPos = value; } }

        [CategoryAttribute("流程属性"), DisplayNameAttribute("导轨左夹爪夹取张开位(mm)"), DescriptionAttribute("导轨左侧夹爪位置,Z方向")]
        public double Z_JawFrameLoosePos { get { return z_JawFrameLoosePos; } set { z_JawFrameLoosePos = value; } }
        [CategoryAttribute("流程属性"), DisplayNameAttribute("导轨左夹爪左安全位(mm)"), DescriptionAttribute("导轨左侧夹爪位置,X方向")]
        public double X_TransfeJawLeftSafeCstPos { get {return x_TransfeJawLeftSafeCstPos; }set{x_TransfeJawLeftSafeCstPos = value;  } }

        [CategoryAttribute("流程属性"), DisplayNameAttribute("导轨左夹爪等待位(mm)"), DescriptionAttribute("导轨左侧夹爪位置,X方向")]
        public double X_ChuckWaitFramePos { get { return x_ChuckWaitFramePos; } set { x_ChuckWaitFramePos = value; } }
        [CategoryAttribute("流程属性"), DisplayNameAttribute("导轨左夹爪右安全位(mm)"), DescriptionAttribute("导轨左侧夹爪位置,X方向")]
        public double X_TransfeJawRightSafeCstPos { get {return x_TransfeJawRightSafeCstPos; }set{x_TransfeJawRightSafeCstPos = value;  } }

        [CategoryAttribute("料盒属性"), DisplayNameAttribute("上料推杆安全位(mm)"), DescriptionAttribute("上料推杆位置,X方向")]
        public Double Load_x_PushRodSafePos
        {
            get { return x_PushRodSafePos; }
            set
            {
                x_PushRodSafePos = value;
            }
        }

        //[CategoryAttribute("流程属性-上料独有"), DisplayNameAttribute("导轨检测位(mm)"), DescriptionAttribute("导轨左侧夹爪位置,X方向")]
        //public double X_ChuckLoadFramePos { get {return x_ChuckLoadFramePos; }set{value= x_ChuckLoadFramePos = value;  }}
        #endregion


        #region 公共方法
        public LoadModule(String para_file, String para_table) : base(para_file, para_table) { }

        public void Initialize(Device_LoadUnload deviceInfo)
        {
            device_LoadUnload = deviceInfo;
        }

        /// <summary>
        /// 功能：上下料模块回零  by M.Bing
        /// </summary>
        /// <param name="Left">true->上料模块；false->下料模块</param>
        /// <returns></returns>
        public override Boolean Home(bool Left = true)
        {
            if (runMode == Convert.ToInt16(SystemRunMode.MODE_OFFLINE))
            {
                return true;
            }
            ready = false;
            //if (!CheckErr(HTM.SetSVON, device_LoadUnload.a_Y, 1))
            //{
            //    errString = String.Format("{0}励磁指令发送失败", HTM.GetAxisName(device_LoadUnload.a_Y)); ;
            //    goto _end;
            //}
            //if (!CheckErr(HTM.SVDone, device_LoadUnload.a_Y))
            //{
            //    errString = String.Format("{0}励磁失败", HTM.GetAxisName(device_LoadUnload.a_Y)); ;
            //    goto _end;
            //}
            //if (!CheckErr(HTM.Home, device_LoadUnload.a_Y))
            //{
            //    errString = String.Format("{0}发送回零指令失败", HTM.GetAxisName(device_LoadUnload.a_Y)); ;
            //    goto _end;
            //}

            //if (!CheckErr(HTM.HomeDone, device_LoadUnload.a_Y))
            //{
            //    errString = String.Format("{0}回零失败", HTM.GetAxisName(device_LoadUnload.a_Y)); ;
            //    goto _end;
            //}
            if (App.obj_SystemConfig.JawCatchMode == 0)
            {
                if (0 != HTM.SetSVOver(device_LoadUnload.a_JawFrame))
                {
                    errString = String.Format("夹爪励磁失败!");
                    goto _end;
                }
                if (0 != HTM.HomeOver(device_LoadUnload.a_JawFrame))
                {
                    errString = String.Format("夹爪(回零)打开失败!");
                    goto _end;
                }
            }
            if (App.obj_SystemConfig.LoadManually)
            {
                if (0 != HTM.SetSVOver(device_LoadUnload.a_JawX))
                {
                    errString = String.Format("夹爪励磁失败!");
                    goto _end;
                }
                if (0 != HTM.HomeOver(device_LoadUnload.a_JawX))
                {
                    errString = String.Format("夹爪(回零)打开失败!");
                    goto _end;
                }
                //右夹爪X移动到右等待位
                if (!Left)
                {
                    if (!CheckErr(HTM.ASMoveOver, device_LoadUnload.a_JawX, x_TransfeJawRightSafeCstPos, speed))
                    {
                        errString = errString = String.Format("{0}运动失败", HTM.GetAxisName(device_LoadUnload.a_JawX)); ;
                        goto _end;
                    }
                }
                ready = true;
                goto _end;
            }
            int[] axisList = new int[] {
                                            device_LoadUnload.a_X_Push,
                                            device_LoadUnload.a_JawZ,
                                            device_LoadUnload.a_Y,
                                            device_LoadUnload.a_Y_Btm,
                                            device_LoadUnload.a_JawX,
                                            device_LoadUnload.a_Z
                                        };
            #region  1.检查上下料端与传送机构间有无料片阻挡
            if (!CheckErr(HTM.WaitDI, device_LoadUnload.i_Transfer_HaveCst, 0, timeDelay))
            {
                errString = String.Format("{0}与传送机构间由于料片阻挡无法运动", Left ? "上料机构" : "下料机构");
                goto _end;
            }
            if (1 == HTM.ReadDI(device_LoadUnload.i_Jaw_HaveMgz))
            {
                //有料盒则打开 夹爪

                errString = String.Format("夹爪内有料盒,请取出!");
                goto _end;
            }
            #endregion

            #region 2.励磁各个电机

            for (int i = 0; i < axisList.Count(); i++)
            {
                if (!Left && (i == 0)) //下料机构无推送杆
                {
                    continue;
                }
                if (axisList[i] == device_LoadUnload.a_Z)
                {
                    if (!CheckErr(HTM.WriteDO, device_LoadUnload.o_Z_Stopping, 1))
                    {
                        errString = String.Format("{0}刹车信号关闭失败!", HTM.GetAxisName(device_LoadUnload.a_Z)); ;
                        goto _end;
                    }
                }
                if (!CheckErr(HTM.SetSVON, axisList[i], 1))
                {
                    errString = String.Format("{0}励磁指令发送失败", HTM.GetAxisName(axisList[i])); ;
                    goto _end;
                }
            }

            for (int i = 0; i < axisList.Count(); i++)
            {
                if (!Left && (i == 0)) //下料机构无推送杆
                {
                    continue;
                }
                if (!CheckErr(HTM.SVDone, axisList[i]))
                {
                    errString = String.Format("{0}励磁失败", HTM.GetAxisName(axisList[i])); ;
                    goto _end;
                }
            }
            #endregion




            //
            //HTM.SetSVOver(1, 2, 3);
            //HTM.HomeOver(1, 2, 3);



            #region 3.回零各个轴

            for (int i = 0; i < axisList.Count() - 1; i++)
            {
                if (!Left && (i == 0)) //下料机构无推送杆
                {
                    continue;
                }
                if (axisList[i] != device_LoadUnload.a_Y)//不回Y轴
                {
                    if (!CheckErr(HTM.Home, axisList[i]))
                    {
                        errString = String.Format("{0}发送回零指令失败", HTM.GetAxisName(axisList[i])); ;
                        goto _end;
                    }
                }
            }
            for (int i = 0; i < axisList.Count() - 1; i++)
            {
                if (!Left && (i == 0)) //下料机构无推送杆
                {
                    continue;
                }
                if (axisList[i] != device_LoadUnload.a_Y)//不回Y轴
                {
                    if (!CheckErr(HTM.HomeDone, axisList[i]))
                    {
                        errString = String.Format("{0}回零失败", HTM.GetAxisName(axisList[i])); ;
                        goto _end;
                    }
                }
            }
            //Y轴回零
            if (!CheckErr(HTM.Home, device_LoadUnload.a_Y))
            {
                errString = String.Format("{0}发送回零指令失败", HTM.GetAxisName(device_LoadUnload.a_Y)); ;
                goto _end;
            }

            if (!CheckErr(HTM.HomeDone, device_LoadUnload.a_Y))
            {
                errString = String.Format("{0}回零失败", HTM.GetAxisName(device_LoadUnload.a_Y)); ;
                goto _end;
            }
            //Z轴最后回零
            if (!CheckErr(HTM.Home, device_LoadUnload.a_Z))
            {
                errString = String.Format("{0}发送回零指令失败", HTM.GetAxisName(device_LoadUnload.a_Z)); ;
                goto _end;
            }

            if (!CheckErr(HTM.HomeDone, device_LoadUnload.a_Z))
            {
                errString = String.Format("{0}回零失败", HTM.GetAxisName(device_LoadUnload.a_Z)); ;
                goto _end;
            }
            //右夹爪X移动到右等待位
            if (!Left)
            {
                if (!CheckErr(HTM.ASMoveOver, device_LoadUnload.a_JawX, x_TransfeJawRightSafeCstPos, speed))
                {
                    errString = errString = String.Format("{0}运动失败", HTM.GetAxisName(device_LoadUnload.a_JawX)); ;
                    goto _end;
                }
            }
            #endregion

            #region 4.复位气缸
            if (!Left)
            {
                if (!CheckErr(HTM.WriteDO, device_LoadUnload.o_Jaw_NgMgz_Close, 0))
                {
                    errString = String.Format("下料机构不良品夹爪气缸复位位失败!"); ;
                    goto _end;
                }
                if (!CheckErr(HTM.WaitDI, device_LoadUnload.i_Jaw_NgMgz_Opened, 1, timeDelay))
                {
                    errString = String.Format("未检测到下料机构不良品夹爪张开!"); ;
                    goto _end;
                }
            }
            #endregion

            ready = true;
            _end:
            return ready;
        }

        /// <summary>
        /// 上料盒 0-上料盒正常  -1动作过程发生错误 -2-无料盒  by M.Bing
        /// </summary>
        /// <param name="Left"></param>
        /// <returns>0-上料盒正常  -1动作过程发生错误 -2-无料盒</returns>
        public int LoadMagezine(bool Left = true)
        {
            int ret = -1;
            DoLoadMgz = true;
            #region 1.检查运行模式
            if (runMode == Convert.ToInt16(SystemRunMode.MODE_OFFLINE))
            {
                DoLoadMgz = false;
                return 0;
            }
            #endregion

            #region 2.检查回零状态
            if (!ready)
            {
                DoLoadMgz = false;
                errString = String.Format("{0}未回零！", Left ? "上料机构" : "下料机构");
                return -1;
            }
            #endregion

            #region 3.检查是否有料片在料盒与传送机构之间
            if (!CheckErr(HTM.WaitDI, device_LoadUnload.i_Transfer_HaveCst, 0, timeDelay))
            {
                errString = String.Format("{0}与传送机构间由于料片阻挡无法运动", Left ? "上料机构" : "下料机构");
                goto _end;
            }
            #endregion

            #region 4.动作流程
            if (Left)
            {
                //1.推杆回安全位
                if (!PushRodSafe())
                {
                    return -1;
                }
            }
            //2.下平台推送Y回安全位  这一步可以省掉

            //Y轴运动到0点
            if (!CheckErr(HTM.ASMoveOver, device_LoadUnload.a_Y, 0.0, speed))
            {
                errString = errString = String.Format("{0}运动失败", HTM.GetAxisName(device_LoadUnload.a_Y)); ;
                ret = -1;
                goto _end;
            }
            //3.判断夹抓是否有料盒
            if (1 == HTM.ReadDI(device_LoadUnload.i_Jaw_HaveMgz))
            {
                //有料盒则打开 夹爪
                //if (!CheckErr(HTM.ASMoveOver, device_LoadUnload.a_JawZ, z_JawOpenedPos, speed))
                //{
                //    errString = String.Format("夹爪内有料盒,夹爪打开失败!");
                //    ret = -1;
                //    goto _end;
                //}
                errString = String.Format("夹爪内有料盒,请取出!");
                ret = -1;
                goto _end;
            }
            //3.Z轴运动到接料盒位
            if (!CheckErr(HTM.ASMoveOver, device_LoadUnload.a_Z, z_LoadMgzPos, speed))
            {
                errString = errString = String.Format("{0}运动失败", HTM.GetAxisName(device_LoadUnload.a_Z)); ;
                ret = -1;
                goto _end;
            }

            if (!CheckErr(HTM.ASMoveOver, device_LoadUnload.a_Y, y_LoadMgzPos, speed))
            {
                errString = errString = String.Format("{0}运动失败", HTM.GetAxisName(device_LoadUnload.a_Y)); ;
                ret = -1;
                goto _end;
            }
            //3.判断夹抓是否有料盒
            //if (1 == HTM.ReadDI(device_LoadUnload.i_Jaw_HaveMgz))
            //{
            //    ////有料盒则打开 夹爪
            //    //if (!CheckErr(HTM.ASMoveOver, device_LoadUnload.a_JawZ, z_JawOpenedPos, speed))
            //    //{
            //    //    errString = String.Format("夹爪内有料盒,夹爪打开失败!");
            //    //    ret = -1;
            //    //    goto _end;
            //    //}
            //    errString = String.Format("夹爪内有料盒,请取出!");
            //    ret = -1;
            //    goto _end;
            //}
            //4.打开夹爪
            if (!CheckErr(HTM.ASMoveOver, device_LoadUnload.a_JawZ, z_JawOpenedPos, speed))
            {
                errString = String.Format("夹爪打开失败!");
                ret = -1;
                goto _end;
            }
            //7.下平台推料盒
            if (!CheckErr(HTM.Speed, device_LoadUnload.a_Y_Btm, -1.0*speed))
            {
                errString = String.Format("{0}运动指令发送异常", HTM.GetAxisName(device_LoadUnload.a_Y_Btm));
                goto _end;
            }
            //8.检测夹爪内是否有料盒
            //检测是否有料盒阻塞式检测(20为从无到有期间的时间)等待将时间 超时时间内检测到料盒  上料盒成功  否则无料盒可用
            double t1 = HTHelper.DateTime.GetTimeSec();
            while (true)
            {
                if (!DoLoadMgz)
                {
                    //停止推料盒
                    if ((errCode = HTM.Stop(device_LoadUnload.a_Y_Btm)) < 0)
                    {
                        errString = String.Format("{0}急停失败", HTM.GetAxisName(device_LoadUnload.a_Y_Btm));
                        goto _end;
                    }
                    ret = 0;
                    goto _end;
                }
                if ((HTHelper.DateTime.GetTimeSec() - t1) > 30)
                {
                    //停止推料盒
                    if ((errCode = HTM.Stop(device_LoadUnload.a_Y_Btm)) < 0)
                    {
                        errString = String.Format("{0}急停失败", HTM.GetAxisName(device_LoadUnload.a_Y_Btm));
                        goto _end;
                    }
                    errString = String.Format("{0}S内未检测到料盒到位，下平台料盒已空!", 30, Left ? "上料机构" : "下料机构");
                    ret = -2;
                    goto _end;
                }
                if (HTM.ReadDI(device_LoadUnload.i_Jaw_HaveMgz) == 1)
                {
                    break;
                }
            }
            //9.运动到最小料盒也能夹紧的位置
            if (!CheckErr(HTM.ASMove, device_LoadUnload.a_JawZ, -450.0, speed))
            {
                errString = String.Format("夹爪运动失败!");
                ret = -1;
                goto _end;
            }
            //循环检测夹爪到位信号
            double t0 = HTHelper.DateTime.GetTimeSec();
            do
            {
                if ((HTHelper.DateTime.GetTimeSec() - t0) > 60)
                {
                    ret = -1;
                    errString = String.Format("夹料盒超时!");

                    if ((errCode = HTM.Stop(device_LoadUnload.a_JawZ)) < 0)
                    {
                        errString = String.Format("{0}急停失败", HTM.GetAxisName(device_LoadUnload.a_JawZ));
                        goto _end;
                    }

                    goto _end;
                }
                //读取夹紧信号
                if (HTM.ReadDI(device_LoadUnload.i_Jaw_Closed) == 1)
                {
                    if ((errCode = HTM.Stop(device_LoadUnload.a_JawZ)) < 0)
                    {
                        errString = String.Format("{0}急停失败", HTM.GetAxisName(device_LoadUnload.a_JawZ));
                        goto _end;
                    }
                    break;
                }
                //轴停止运动了
                if (HTM.OnceDone(device_LoadUnload.a_JawZ) == 1)
                {
                    if ((errCode = HTM.Stop(device_LoadUnload.a_Y_Btm)) < 0)
                    {
                        errString = String.Format("{0}急停失败", HTM.GetAxisName(device_LoadUnload.a_Y_Btm));
                        goto _end;
                    }
                    errString = String.Format("料盒未正常夹住!");
                    ret = -1;
                    goto _end;
                }
            } while (true);

            //10.停止推料盒
            if ((errCode = HTM.Stop(device_LoadUnload.a_Y_Btm)) < 0)
            {
                errString = String.Format("{0}急停失败", HTM.GetAxisName(device_LoadUnload.a_Y_Btm));
                goto _end;
            }
            //Z轴抬升
            if (!CheckErr(HTM.RSMoveOver, device_LoadUnload.a_Z, 30.0, speed))
            {
                errString = errString = String.Format("{0}运动失败", HTM.GetAxisName(device_LoadUnload.a_Z)); ;
                ret = -1;
                goto _end;
            }
            //Y轴退回到上片点位
            if (!CheckErr(HTM.ASMoveOver, device_LoadUnload.a_Y, y_LoadUnLoadFramePos, speed))
            {
                errString = errString = String.Format("{0}运动失败", HTM.GetAxisName(device_LoadUnload.a_Z)); ;
                ret = -1;
                goto _end;
            }
            //11.Z运动到上片起始位
            if (!CheckErr(HTM.ASMoveOver, device_LoadUnload.a_Z, z_LoadUnLoadFirstPos, speed))
            {
                errString = errString = String.Format("{0}运动失败", HTM.GetAxisName(device_LoadUnload.a_Z)); ;
                ret = -1;
                goto _end;
            }
            //夹爪X移动到右等待位
            if (!Left)
            {
                if (!CheckErr(HTM.ASMoveOver, device_LoadUnload.a_JawX, x_TransfeJawRightSafeCstPos, speed))
                {
                    errString = errString = String.Format("{0}运动失败", HTM.GetAxisName(device_LoadUnload.a_JawX)); ;
                    ret = -1;
                    goto _end;
                }
            }
            #endregion
            ret = 0;
            _end:
            DoLoadMgz = false;
            return ret;
        }

        /// <summary>
        /// 获取轴位置
        /// </summary>
        /// <param name="axis"></param>
        /// <returns></returns>
        public Double GetFbkPos(AxisIndex axis)
        {
            Double temp = 0;

            switch (axis)
            {
                case AxisIndex.AXIS_Y:
                    temp = HTM.GetFbkPos(device_LoadUnload.a_Y);
                    break;
                case AxisIndex.AXIS_Z:
                    temp = HTM.GetFbkPos(device_LoadUnload.a_Z);
                    break;
                case AxisIndex.AXIS_Z_JAW:
                    temp = HTM.GetFbkPos(device_LoadUnload.a_JawZ);
                    break;
                case AxisIndex.AXIS_BTM_PUSH:
                    temp = HTM.GetFbkPos(device_LoadUnload.a_Y_Btm);
                    break;
                case AxisIndex.AXIS_ROD_PUSH:
                    temp = HTM.GetFbkPos(device_LoadUnload.a_X_Push);
                    break;
                case AxisIndex.AXIS_X_TRS:
                    temp = HTM.GetFbkPos(device_LoadUnload.a_JawX);
                    break;
                default:
                    break;
            }

            return temp;
        }
        /// <summary>
        /// 阻塞式检测轴上未开放IO的状态（临时）
        /// </summary>
        /// <param name="axisIdx">轴号</param>
        /// <param name="Bit">IO Bit(0-based). MIO_ALM(0)：报警; MIO_PEL(1)：正限; MIO_MEL(2)：负限; MIO_ORG(3)：原点; MIO_INP(6)：到位; MIO_SVON(7)：励磁; MIO_SPEL(11)：软正限; MIO_SMEL(12)：软负限 </param>
        /// <param name="IOResult">IO状态 1or0</param>
        /// <param name="timeDelay">阻塞时间 单位毫秒</param>
        /// <returns>0-成功；-1-超时</returns>
        public int CheckMotionIOBit(int axisIdx, int Bit, int IOResult, Double timeDelay_second)
        {
            int err = 0;
            DateTime t_new;
            Double delta = 0;
            DateTime t_start = DateTime.Now;
            while (true)
            {
                if (HTM.GetMotionIO(axisIdx, (HTM.MotionIO)Bit) == IOResult) break;
                t_new = DateTime.Now;
                delta = t_new.Subtract(t_start).TotalSeconds;
                if (delta > timeDelay_second)
                {
                    err = -1;
                    break;
                }
                Thread.Sleep(10);
            }
            return err;
        }

        /// <summary>
        /// 功能：下料盒 0-成功；-1-轴IO异常；-2-下平台满仓 by M.Bing
        /// </summary>
        /// <returns>0-成功；-1-轴IO异常；-2-下平台满仓</returns>
        public int UnloadMagezine(Boolean Left)
        {
            int ret = -1;

            #region 1.检查运行模式
            if (runMode == Convert.ToInt16(SystemRunMode.MODE_OFFLINE))
            {
                return 0;
            }
            #endregion

            #region 2.检查回零状态
            if (!ready)
            {
                errString = String.Format("{0}未回零！", Left ? "上料机构" : "下料机构");
                return -1;
            }
            #endregion

            #region 3.检查是否有料片在料盒与传送机构之间
            if (!CheckErr(HTM.WaitDI, device_LoadUnload.i_Transfer_HaveCst, 0, timeDelay))
            {
                errString = String.Format("{0}与传送机构间由于料片阻挡无法运动", Left ? "上料机构" : "下料机构");
                goto _end;
            }
            #endregion

            #region 4.检查下料盒仓是否已满
            if (1 == HTM.ReadDI(device_LoadUnload.i_Mgz_Full_TOP))
            {
                errString = String.Format("上平台料盒仓已满！");
                return -2;
            }
            #endregion

            #region 4.检测夹爪内是否有料盒
            if (0 == HTM.ReadDI(device_LoadUnload.i_Jaw_HaveMgz))
            {
                errString = String.Format("夹爪内无料盒！");
                return -1;
            }
            #endregion

            #region 5.动作流程
            if (Left)
            {
                //0.推杆回安全位
                if (!PushRodSafe())
                {
                    return -1;
                }
            }
            //1.z轴运动到下料盒位
            if (!CheckErr(HTM.ASMoveOver, device_LoadUnload.a_Z, z_UnLoadMgzPos + 25, speed))
            {
                errString = String.Format("{0}运动失败", HTM.GetAxisName(device_LoadUnload.a_Z));
                ret = -1;
                goto _end;
            }
            //2.y轴开始推料
            if (!CheckErr(HTM.ASMove, device_LoadUnload.a_Y, y_UnLoadMgzPos, speed))
            {
                errString = String.Format("{0}运动失败", HTM.GetAxisName(device_LoadUnload.a_Y));
                ret = -1;
                goto _end;
            }




            //3.判断推的过程中是否有满仓信号  
            double t0 = HTHelper.DateTime.GetTimeSec();
            do
            {
                if ((t0 - HTHelper.DateTime.GetTimeSec()) > 60)
                {
                    errString = String.Format("检测满仓信号超时!");
                    ret = -1;
                    goto _end;
                }
                //3.1 运动停止前检测到满仓信号
                if (1 == HTM.ReadDI(device_LoadUnload.i_Mgz_Full_TOP))
                {
                    //y轴停止
                    if (!CheckErr(HTM.Stop, device_LoadUnload.a_Y))
                    {
                        errString = String.Format("{0}推片X轴停止失败!", HTM.GetAxisName(device_LoadUnload.a_Y));
                        ret = -1;
                        goto _end;
                    }
                    //y轴退回，运动到上下料位  报满仓信号
                    if (!CheckErr(HTM.ASMove, device_LoadUnload.a_Y, y_LoadUnLoadFramePos, speed))
                    {
                        errString = String.Format("{0}运动失败", HTM.GetAxisName(device_LoadUnload.a_Y));
                        ret = -1;
                        goto _end;
                    }
                    ret = -2;
                    goto _end;
                }
                //3.2  运动停止前未检测到满仓信号 
                if (1 == HTM.OnceDone(device_LoadUnload.a_Y))
                {
                    //Z轴下降  放料盒  此处点位要注意
                    if (!CheckErr(HTM.RSMoveOver, device_LoadUnload.a_Z, -25.0, speed))
                    {
                        errString = String.Format("{0}运动失败", HTM.GetAxisName(device_LoadUnload.a_Z));
                        ret = -1;
                        goto _end;
                    }
                    //夹爪松开
                    if (!CheckErr(HTM.ASMoveOver, device_LoadUnload.a_JawZ, z_JawOpenedPos, speed))
                    {
                        errString = String.Format("{0}运动失败", HTM.GetAxisName(device_LoadUnload.a_JawZ));
                        ret = -1;
                        goto _end;
                    }
                    //Z轴再下降一点
                    if (!CheckErr(HTM.RSMoveOver, device_LoadUnload.a_Z, -5.0, speed))
                    {
                        errString = String.Format("{0}运动失败", HTM.GetAxisName(device_LoadUnload.a_Z));
                        ret = -1;
                        goto _end;
                    }
                    //y轴退回，运动到上下料位
                    if (!CheckErr(HTM.ASMove, device_LoadUnload.a_Y, y_LoadUnLoadFramePos, speed))
                    {
                        errString = String.Format("{0}运动失败", HTM.GetAxisName(device_LoadUnload.a_Y));
                        ret = -1;
                        goto _end;
                    }
                    break;
                }
                Thread.Sleep(100);
            } while (true);

            #endregion
            ret = 0;
            _end:
            return ret;
        }

        /// <summary>
        /// 功能：从Mgz上load Frame到传送机构   0 -- 成功   -1 -- 失败  -2 -- 该层无料片  by M.Bing
        /// </summary>
        /// <param name="Index">扫描到的第几片，从1开始</param>
        /// <returns></returns>
        public int LoadFrameFromMgz(int Index)
        {

            int ret = -1;
            #region 1.检查运行模式
            if (runMode == Convert.ToInt16(SystemRunMode.MODE_OFFLINE))
            {
                return 0;
            }
            #endregion

            #region 2.检查回零状态
            if (!ready)
            {
                errString = String.Format("{0}未回零！", true ? "上料机构" : "下料机构");
                return -1;
            }
            #endregion

            #region 3.检查是否有料片在料盒与传送机构之间
            if (!CheckErr(HTM.WaitDI, device_LoadUnload.i_Transfer_HaveCst, 0, timeDelay))
            {
                errString = String.Format("{0}与传送机构间由于料片阻挡无法运动", true ? "上料机构" : "下料机构");
                ret = -1;
                goto _end;
            }
            #endregion

            #region 4.检测夹爪内是否有料盒
            if (0 == HTM.ReadDI(device_LoadUnload.i_Jaw_HaveMgz))
            {
                errString = String.Format("夹爪内无料盒！");
                ret = -1;
                goto _end;
            }
            #endregion

            #region 5.检查取片层是否在范围内
            if (Index > slotLayersCount)
            {
                errString = "取料层数已超出料盒层数";
                ret = -1;
                goto _end;
            }
            #endregion

            #region 6.动作流程

            //0.打开传输夹爪
            if (!OpenJaw())
            {
                errString = String.Format("传送夹爪打开失败！");
                ret = -1;
                goto _end;
            }
            //1.X,Z,Y轴运动到上片位
            if (!CheckErr(HTM.ASMoveOver, device_LoadUnload.a_JawX, x_TransfeJawLeftSafeCstPos, speed))
            {
                errString = String.Format("{0}运动失败", HTM.GetAxisName(device_LoadUnload.a_JawX));
                ret = -1;
                goto _end;
            }
            if (!CheckErr(HTM.ASMoveOver, device_LoadUnload.a_Y, y_LoadUnLoadFramePos + (Index - 1) * heightSlotLayer_y, speed))
            {
                errString = String.Format("{0}运动失败", HTM.GetAxisName(device_LoadUnload.a_Y));
                ret = -1;
                goto _end;
            }
            if (!CheckErr(HTM.ASMoveOver, device_LoadUnload.a_Z, (z_LoadUnLoadFramePos + (Index - 1) * heightSlotLayer), speed))
            {
                errString = String.Format("{0}运动失败", HTM.GetAxisName(device_LoadUnload.a_Z));
                ret = -1;
                goto _end;
            }
            Thread.Sleep(1000);
            ////1.5. 是否有料片？
            //if (0 == HTM.ReadDI(device_LoadUnload.i_HaveCst))
            //{
            //    errString = String.Format("当前层未检测到料片!");
            //    ret = -2;
            //    goto _end;
            //}
            //2.推片X轴开始运动
            if (!CheckErr(HTM.ASMove, device_LoadUnload.a_X_Push, x_PushRodOverPos, speed))
            {
                errString = String.Format("{0}运动失败", HTM.GetAxisName(device_LoadUnload.a_X_Push));
                ret = -1;
                goto _end;
            }
            //3.在推片X轴运动过程中 判断是否有推杆碰撞信号 且要判断是否有料片
            double t0 = HTHelper.DateTime.GetTimeSec();
            do
            {
                if ((t0 - HTHelper.DateTime.GetTimeSec()) > 60)
                {
                    errString = String.Format("{0}推片X轴推片运动超时!", HTM.GetAxisName(device_LoadUnload.a_X_Push));
                    ret = -1;
                    goto _end;
                }
                //3.1 运动停止前检测到碰撞信号  报异常 退回
                if (1 == HTM.ReadDI(device_LoadUnload.i_PushCst_Err))
                {

                    if (HTM.Stop(device_LoadUnload.a_X_Push, HTM.StopMode.EMG) < 0)
                    {
                        errString = String.Format("{0}推片X轴停止失败!", HTM.GetAxisName(device_LoadUnload.a_X_Push));
                        ret = -1;
                        goto _end;
                    }
                    if (PushRodLeft() == false)
                    {
                        errString = String.Format("{0}推片X轴未能回到安全位", HTM.GetAxisName(device_LoadUnload.a_X_Push));
                        ret = -1;
                        goto _end;
                    }
                    errString = String.Format("推片时，料片受到阻挡！");
                    ret = -1;
                    goto _end;
                }
                //3.3 运动停止前未检测到碰撞信号 且扫描到cast推料成功 退回
                if (1 == HTM.OnceDone(device_LoadUnload.a_X_Push))
                {
                    if (PushRodLeft() == false)
                    {
                        errString = String.Format("{0}推片X轴未能回到安全位", HTM.GetAxisName(device_LoadUnload.a_X_Push));
                        ret = -1;
                        goto _end;
                    }
                    break;
                }
            } while (true);
            //4. 传输机构传感器是否检测到料片
            if (0 == HTM.ReadDI(device_LoadUnload.i_Transfer_HaveCst))
            {
                errString = String.Format("当前层未推到料片!");
                ret = -2;
                goto _end;
                errString = String.Format("料片未正确推到传输台！");
                ret = -1;
                goto _end;
            }
            #endregion
            ret = 0;
            _end:
            if (PushRodLeft() == false)
            {
                errString = String.Format("{0}推片X轴未能回到安全位", HTM.GetAxisName(device_LoadUnload.a_X_Push));
                ret = -1;
            }
            return ret;
        }

        /// <summary>
        /// 功能：把frame从初始位移动到等待位（传送机构左端传感器由亮变不亮）by M.Bing
        /// </summary>
        /// <returns></returns>
        public Boolean TransferFrame2WaitPos()
        {
            Boolean ret = false;

            #region 1.检查运行模式
            if (runMode == Convert.ToInt16(SystemRunMode.MODE_OFFLINE))
            {
                return true;
            }
            #endregion

            #region 2.检查回零状态
            if (!ready)
            {
                errString = String.Format("上料机构未回零！");
                return false;
            }
            #endregion

            #region 3.检查是否有料片可以移动
            if (0 == HTM.ReadDI(device_LoadUnload.i_Transfer_HaveCst))
            {
                errString = String.Format("无料片可以移动！");
                goto _end;
            }
            #endregion

            #region 4.动作流程

            double t0 = HTHelper.DateTime.GetTimeSec();
            int moveStatus = 0;  //单次运动结果  1--检测到左端传感器熄灭  -1--移动到右侧极限也没有检测到传感器熄灭
            while (true)
            {
                if ((t0 - HTHelper.DateTime.GetTimeSec()) > 20)
                {
                    errString = String.Format("{0}运动到载台右传感器亮起超时！", HTM.GetAxisName(device_LoadUnload.a_JawX));
                    ret = false;
                    goto _end;
                }

                //1.打开传输夹爪
                if (!OpenJaw())
                {
                    errString = String.Format("传送夹爪打开失败！");
                    ret = false;
                    goto _end;
                }
                //2.运动到左接料位  也就是运动到左边极限位
                if (!CheckErr(HTM.ASMoveOver, device_LoadUnload.a_JawX, x_TransfeJawLeftSafeCstPos, speed))
                {
                    errString = String.Format("{0}运动到左边极限位失败", HTM.GetAxisName(device_LoadUnload.a_JawX));
                    ret = false;
                    goto _end;
                }

                //3.传输夹爪夹紧
                if (!CloseJaw())
                {
                    errString = String.Format("传送夹爪夹紧失败！");
                    ret = false;
                    goto _end;
                }
                //4.运动到载台左传感器亮
                if (!CheckErr(HTM.ASMove, device_LoadUnload.a_JawX, x_TransfeJawRightSafeCstPos, speed))
                {
                    errString = String.Format("{0}运动失败", HTM.GetAxisName(device_LoadUnload.a_JawX));
                    ret = false;
                    goto _end;
                }
                do
                {
                    if ((t0 - HTHelper.DateTime.GetTimeSec()) > 20)
                    {
                        errString = String.Format("{0}运动到载台左传感器亮起超时！", HTM.GetAxisName(device_LoadUnload.a_JawX));
                        ret = false;
                        goto _end;
                    }
                    //获取Chuck台传感器状态
                    if (1 == HTM.ReadDI(device_LoadUnload.i_Chuck_HaveCst))
                    {
                        if (HTM.Stop(device_LoadUnload.a_JawX, HTM.StopMode.EMG) < 0)
                        {
                            errString = String.Format("{0}停止异常", HTM.GetAxisName(device_LoadUnload.a_JawX));
                            ret = false;
                            goto _end;
                        }
                        Thread.Sleep(300);
                        moveStatus = 1;   //此时片子已经全部在传输机构上了
                        break;
                    }
                    //获取传输夹爪X轴是否移动到右安全位了
                    if (1 == HTM.OnceDone(device_LoadUnload.a_JawX))
                    {
                        if (HTM.Stop(device_LoadUnload.a_JawX, HTM.StopMode.EMG) < 0)
                        {
                            errString = String.Format("{0}停止异常", HTM.GetAxisName(device_LoadUnload.a_JawX));
                            ret = false;
                            goto _end;
                        }
                        moveStatus = -1;  //移动到最右端片子还未完全在传输机构上  回去再移一次
                        break;
                    }
                    Thread.Sleep(20);
                } while (true);
                if (moveStatus == 1)
                {
                    break;
                }
            }
            //5.再继续运动5mm 给后面上料片流出距离
            // if(!CheckErr(HTM.RSMoveOver, device_LoadUnload.a_JawX,5.0,-speed))
            //{
            //    errString = String.Format("{0}再继续运动失败", HTM.GetAxisName(device_LoadUnload.a_JawX));
            //    ret = false;
            //    goto _end;
            //}
            #endregion

            ret = true;
            _end:
            #region 异常及后续动作处理
            //1.打开传输夹爪
            if (!OpenJaw())
            {
                errString = String.Format("传送夹爪打开失败！");
                ret = false;
            }
            //2.运动到左安全位
            if (!CheckErr(HTM.ASMoveOver, device_LoadUnload.a_JawX, x_TransfeJawLeftSafeCstPos + (x_TransfeJawRightSafeCstPos - x_TransfeJawLeftSafeCstPos) / 2, speed))
            {
                errString = String.Format("{0}运动失败", HTM.GetAxisName(device_LoadUnload.a_JawX));
                ret = false;
            }
            #endregion
            return ret;
        }
        /// <summary>
        /// 功能：把frame从等待位移动到检测位
        /// 前提：Chuck做好接料准备
        /// </summary>
        /// <returns></returns>
        public Boolean TransferFrame2LoadPos(Boolean LoadManually = true)
        {
            Boolean ret = false;

            #region 1.检查运行模式
            if (runMode == Convert.ToInt16(SystemRunMode.MODE_OFFLINE))
            {
                return true;
            }
            #endregion

            #region 2.检查回零状态
            if (!ready)
            {
                errString = String.Format("上料机构未回零！");
                return false;
            }
            #endregion

            #region 3.动作流程
            //若料片长度小于运动距离
            if (x_ChuckLoadFramePos - x_TransfeJawLeftSafeCstPos >= FrameLength+10+(x_ChuckLoadFramePos-x_ChuckWaitFramePos))
            {
                //1.张开夹爪
                if (!OpenJaw())
                {
                    errString = String.Format("传送夹爪打开失败！");
                    ret = false;
                    goto _end;
                }

                //2.运动到最左端
                if (!CheckErr(HTM.ASMoveOver, device_LoadUnload.a_JawX, x_ChuckWaitFramePos - FrameLength-10- (x_ChuckLoadFramePos - x_ChuckWaitFramePos), speed))
                {
                    errString = String.Format("{0}运动失败", HTM.GetAxisName(device_LoadUnload.a_JawX));
                    ret = false;
                    goto _end;
                }
                //3.夹住夹爪
                if (!CloseJaw())
                {
                    errString = String.Format("传送夹爪夹紧失败！");
                    ret = false;
                    goto _end;
                }
                //4.拖到最右端
                if (!CheckErr(HTM.ASMoveOver, device_LoadUnload.a_JawX, x_ChuckLoadFramePos, 0.3*speed))
                {
                    errString = String.Format("{0}运动失败", HTM.GetAxisName(device_LoadUnload.a_JawX));
                    ret = false;
                    goto _end;
                }
            }
            //若料片长度大于运动距离
            else
            {
                //A:
                //1.张开夹爪
                if (!OpenJaw())
                {
                    errString = String.Format("传送夹爪打开失败！");
                    ret = false;
                    goto _end;
                }

                //2.运动到最左端
                if (!CheckErr(HTM.ASMoveOver, device_LoadUnload.a_JawX, x_TransfeJawLeftSafeCstPos, speed))
                {
                    errString = String.Format("{0}运动失败", HTM.GetAxisName(device_LoadUnload.a_JawX));
                    ret = false;
                    goto _end;
                }
                //3.夹住夹爪
                if (!CloseJaw())
                {
                    errString = String.Format("传送夹爪夹紧失败！");
                    ret = false;
                    goto _end;
                }
                //if(x_ChuckLoadFramePos - x_TransfeJawLeftSafeCstPos <
                //    FrameLength + 10 + (x_ChuckLoadFramePos - x_ChuckWaitFramePos) - (x_ChuckLoadFramePos - x_TransfeJawLeftSafeCstPos))
                //{
                //    FrameLength = FrameLength - (x_ChuckLoadFramePos - x_TransfeJawLeftSafeCstPos);
                //    goto A;
                //}
                //4.拖到最右端
                if (!CheckErr(HTM.ASMoveOver, device_LoadUnload.a_JawX, x_ChuckLoadFramePos-5, speed))
                {
                    errString = String.Format("{0}运动失败", HTM.GetAxisName(device_LoadUnload.a_JawX));
                    ret = false;
                    goto _end;
                }
                //5.松开夹爪
                if (!OpenJaw())
                {
                    errString = String.Format("传送夹爪打开失败！");
                    ret = false;
                    goto _end;
                }

                //6.夹爪左移
                if (!CheckErr(HTM.ASMoveOver, device_LoadUnload.a_JawX, x_ChuckLoadFramePos + x_ChuckWaitFramePos - FrameLength- x_TransfeJawLeftSafeCstPos-10, speed))
                {
                    errString = String.Format("{0}运动失败", HTM.GetAxisName(device_LoadUnload.a_JawX));
                    ret = false;
                    goto _end;
                }
                //7.夹紧夹爪
                if (!CloseJaw())
                {
                    errString = String.Format("传送夹爪夹紧失败！");
                    ret = false;
                    goto _end;
                }
                //8.向右边推料
                if (!CheckErr(HTM.ASMoveOver, device_LoadUnload.a_JawX, x_ChuckLoadFramePos, 0.3 * speed))
                {
                    errString = String.Format("{0}运动失败", HTM.GetAxisName(device_LoadUnload.a_JawX));
                    ret = false;
                    goto _end;
                }
            }
            //9.张开夹爪
            if (!OpenJaw())
            {
                errString = String.Format("传送夹爪打开失败！");
                ret = false;
                goto _end;
            }
            ////10.夹爪走回右安全位
            //if (!CheckErr(HTM.ASMoveOver, device_LoadUnload.a_JawX, x_TransfeJawRightSafeCstPos, speed))
            //{
            //    errString = String.Format("{0}运动失败", HTM.GetAxisName(device_LoadUnload.a_JawX));
            //    ret = false;
            //}
            #endregion
            ret = true;
            _end:
            #region 异常及后续动作处理
            //1.打开传输夹爪
            if (!OpenJaw())
            {
                errString = String.Format("传送夹爪打开失败！");
                ret = false;
            }
            //HTLog.Info("打开传输夹爪" + "耗秒：" + (DateTime.Now.Subtract(t1).TotalMilliseconds / 1000).ToString("F3"));
            //t1 = DateTime.Now;
            //2.运动到左安全位  中间安全位
            if (!CheckErr(HTM.ASMoveOver, device_LoadUnload.a_JawX, x_TransfeJawLeftSafeCstPos, speed))
            {
                errString = String.Format("{0}运动失败", HTM.GetAxisName(device_LoadUnload.a_JawX));
                ret = false;
            }
            //HTLog.Info("运动到左安全位" + "耗秒：" + (DateTime.Now.Subtract(t1).TotalMilliseconds / 1000).ToString("F3"));
            //HTLog.Info("运动到检测位总计" + "耗秒：" + (DateTime.Now.Subtract(t2).TotalMilliseconds / 1000).ToString("F3"));
            #endregion
            return ret;
        }
        /// <summary>
        /// 功能：下片到右等待位  by M.bing
        /// </summary>
        /// <returns></returns>
        public Boolean UnloadMove2Wait()
        {
            Boolean ret = false;

            #region 1.检查运行模式
            if (runMode == Convert.ToInt16(SystemRunMode.MODE_OFFLINE))
            {
                return true;
            }
            #endregion

            #region 2.检查回零状态
            if (!ready)
            {
                errString = String.Format("下料机构未回零！");
                return false;
            }
            #endregion

            #region 3.动作流程

            double t0 = HTHelper.DateTime.GetTimeSec();
            int moveStatus = 0;  //单次运动结果  1--检测到chuck右端传感器熄灭  -1--移动到右侧极限也没有检测到chuck右端传感器熄灭
            bool chuckHaveCstFlag = false;  //chuck右端是否有Cst
            while (true)
            {
                if ((t0 - HTHelper.DateTime.GetTimeSec()) > 20)
                {
                    errString = String.Format("{0}运动到上料传感器熄灭超时！", HTM.GetAxisName(device_LoadUnload.a_JawX));
                    ret = false;
                    goto _end;
                }
                if (1 == HTM.WaitDI(device_LoadUnload.i_Unload_PushCstLiftCylinderUp, 1, 10))
                {
                    if (!CheckErr(HTM.WriteDO, device_LoadUnload.o_Unload_PushCstLiftCylinder, 0))
                    {
                        errString = String.Format("执行降下下料推杆命令失败!");
                        ret = false;
                        goto _end;
                    }
                    if (1 != HTM.WaitDI(device_LoadUnload.i_Unload_PushCstLiftCylinderDown, 1, 10))
                    {
                        errString = String.Format("下降下料推杆超时失败!");
                        ret = false;
                        goto _end;
                    }
                }
                //1.打开传输夹爪
                if (!OpenJaw())
                {
                    errString = String.Format("传送夹爪打开失败！");
                    ret = false;
                    goto _end;
                }
                //2.运动到右传输机构左极限
                if (!CheckErr(HTM.ASMoveOver, device_LoadUnload.a_JawX, x_TransfeJawLeftSafeCstPos, speed))
                {
                    errString = String.Format("{0}运动失败", HTM.GetAxisName(device_LoadUnload.a_JawX));
                    ret = false;
                    goto _end;
                }
                //3.传输夹爪夹紧
                if (!CloseJaw())
                {
                    errString = String.Format("传送夹爪夹紧失败！");
                    ret = false;
                    goto _end;
                }
                //4.运动到等待位的过程中 检测chuck右端传感器
                if (!CheckErr(HTM.ASMove, device_LoadUnload.a_JawX, x_TransfeJawRightSafeCstPos, speed))
                {
                    errString = String.Format("{0}运动失败", HTM.GetAxisName(device_LoadUnload.a_JawX));
                    ret = false;
                    goto _end;
                }
                do
                {
                    if ((t0 - HTHelper.DateTime.GetTimeSec()) > 20)
                    {
                        errString = String.Format("{0}运动到chuck右端传感器熄灭超时！", HTM.GetAxisName(device_LoadUnload.a_JawX));
                        ret = false;
                        goto _end;
                    }
                    //获取chuck右端传感器状态
                    if ((1 == HTM.ReadDI(device_LoadUnload.i_Chuck_HaveCst)) && !chuckHaveCstFlag)
                    {
                        chuckHaveCstFlag = true;
                    }
                    if ((0 == HTM.ReadDI(device_LoadUnload.i_Chuck_HaveCst)) && chuckHaveCstFlag)
                    {
                        if (HTM.Stop(device_LoadUnload.a_JawX, HTM.StopMode.DEC) < 0)
                        {
                            errString = String.Format("{0}停止异常", HTM.GetAxisName(device_LoadUnload.a_JawX));
                            ret = false;
                            goto _end;
                        }
                        moveStatus = 1;
                        break;
                    }
                    //获取传输夹爪x轴是否移动到右等待位了
                    if (1 == HTM.OnceDone(device_LoadUnload.a_JawX))
                    {
                        if (HTM.Stop(device_LoadUnload.a_JawX, HTM.StopMode.DEC) < 0)
                        {
                            errString = String.Format("{0}停止异常", HTM.GetAxisName(device_LoadUnload.a_JawX));
                            ret = false;
                            goto _end;
                        }
                        moveStatus = -1;
                        break;
                    }
                    Thread.Sleep(100);
                } while (true);
                if (moveStatus == 1)
                {
                    break;
                }
            }
            //5.再继续运动5mm 给后面上料片留出距离
            if (!CheckErr(HTM.RSMoveOver, device_LoadUnload.a_JawX, 5.0, speed))
            {
                errString = String.Format("{0}再继续运动失败", HTM.GetAxisName(device_LoadUnload.a_JawX));
                ret = false;
                goto _end;
            }
            #endregion

            ret = true;
            _end:
            #region 异常及后续动作处理
            //1.打开传输夹爪
            if (!OpenJaw())
            {
                errString = String.Format("传送夹爪打开失败！");
                ret = false;
            }
            //2.运动到左安全位  中间安全位
            if (!CheckErr(HTM.ASMoveOver, device_LoadUnload.a_JawX, x_TransfeJawLeftSafeCstPos + (x_TransfeJawRightSafeCstPos - x_TransfeJawLeftSafeCstPos) / 2, speed))
            {
                errString = String.Format("{0}运动失败", HTM.GetAxisName(device_LoadUnload.a_JawX));
                ret = false;
            }
            #endregion
            return ret;
        }
        
        /// <summary>
        /// 用于手动模式下料  将料片推到传输机构右端传感器不亮 by M.bing
        /// </summary>
        /// <returns></returns>
        public Boolean TransferFrameToRight()
        {
            Boolean ret = false;

            #region 1.检查运行模式
            if (runMode == Convert.ToInt16(SystemRunMode.MODE_OFFLINE))
            {
                return true;
            }
            #endregion

            #region 2.检查回零状态
            if (!ready)
            {
                errString = String.Format("上料机构未回零！");
                return false;
            }
            #endregion

            #region 3.检查下料端有料片阻挡
            if (1 == HTM.ReadDI(device_LoadUnload.i_Transfer_HaveCst))
            {
                errString = String.Format("下料端有料片阻挡！");
                goto _end;
            }
            #endregion

            #region 4.动作流程


            //1.打开夹爪
            if (!OpenJaw())
            {
                errString = String.Format("传送夹爪打开失败！");
                ret = false;
                goto _end;
            }
            //2.走到左端夹住等待位料片
            if (!CheckErr(HTM.ASMoveOver, device_LoadUnload.a_JawX, x_TransfeJawLeftSafeCstPos, speed))
            {
                errString = String.Format("{0}运动失败", HTM.GetAxisName(device_LoadUnload.a_JawX));
                ret = false;
                goto _end;
            }
            //3.夹紧料片
            if (!CloseJaw())
            {
                errString = String.Format("传送夹爪夹紧失败！");
                ret = false;
                goto _end;
            }
            //4.移动到最右端
            if (!CheckErr(HTM.ASMoveOver, device_LoadUnload.a_JawX, x_TransfeJawRightSafeCstPos, speed))
            {
                errString = String.Format("{0}运动失败", HTM.GetAxisName(device_LoadUnload.a_JawX));
                ret = false;
                goto _end;
            }
            //5.松开夹爪
            if (!OpenJaw())
            {
                errString = String.Format("传送夹爪打开失败！");
                ret = false;
                goto _end;
            }
            //6.向左移动
            if (!CheckErr(HTM.ASMoveOver, device_LoadUnload.a_JawX, x_TransfeJawLeftSafeCstPos, speed))
            {
                errString = String.Format("{0}运动失败", HTM.GetAxisName(device_LoadUnload.a_JawX));
                ret = false;
                goto _end;
            }
            //7.夹住料片
            if (!CloseJaw())
            {
                errString = String.Format("传送夹爪夹紧失败！");
                ret = false;
                goto _end;
            }
            //7.推到右极限
            if (!CheckErr(HTM.ASMoveOver, device_LoadUnload.a_JawX, x_TransfeJawRightSafeCstPos, speed))
            {
                errString = String.Format("{0}运动失败", HTM.GetAxisName(device_LoadUnload.a_JawX));
                ret = false;
                goto _end;
            }
            //8.推到右极限的过程检测传感器点亮 则停止
            double t0 = HTHelper.DateTime.GetTimeSec();
            do
            {
                //1.检测超时
                if ((t0 - HTHelper.DateTime.GetTimeSec()) > 20)
                {
                    errString = "料片移动到最右端超时！";
                    ret = false;
                    goto _end;
                }
                //2.检测传感器状态
                if (HTM.ReadDI(device_LoadUnload.i_Transfer_HaveCst) == 1)
                {
                    break;
                }
                //3.检测轴是否运动到最右端后停止
                if (HTM.OnceDone(device_LoadUnload.a_JawX) == 1)
                {
                    errString = "夹爪移动到最右端还未检测到料片！";
                    ret = false;
                    goto _end;
                }

            } while (true);

            #endregion

            ret = true;

            _end:
            //1.打开夹爪
            if (!OpenJaw())
            {
                errString = String.Format("传送夹爪打开失败！");
                ret = false;
                goto _end;
            }
            //2.走到中间安全位
            if (!CheckErr(HTM.ASMoveOver, device_LoadUnload.a_JawX, (x_TransfeJawLeftSafeCstPos + x_TransfeJawRightSafeCstPos) / 2, speed))
            {
                errString = String.Format("{0}运动失败", HTM.GetAxisName(device_LoadUnload.a_JawX));
                ret = false;
                goto _end;
            }
            return ret;
        }
        
        /// <summary>
        /// 机器推杆推料位置(防止由于阻挡而无法到达工作位置)  by M.Bing
        /// </summary>
        /// <param name="timeDelay_second"></param>
        /// <returns></returns>
        public Boolean PushRodRight(Double timeDelay_second)
        {
            Boolean ret = false;

            #region 1.检查运行模式
            if (runMode == Convert.ToInt16(SystemRunMode.MODE_OFFLINE))
            {
                return true;
            }
            #endregion

            #region 2.检查回零状态
            if (!ready)
            {
                errString = String.Format("{0}未回零！", true ? "上料机构" : "下料机构");
                return false;
            }
            #endregion

            #region 动作流程

            //1.推杆电机右运动
            if ((errCode = HTM.ASMove(device_LoadUnload.a_X_Push, x_PushRodOverPos, speed)) < 0)
            {
                errString = String.Format("{0}运动失败", HTM.GetAxisName(device_LoadUnload.a_X_Push));
                ret = false;
                goto _end;
            }
            //2.检测是否有碰撞及是否运动到到点位

            Double t0 = HTHelper.DateTime.GetTimeSec();
            while (true)
            {
                if (1 == HTM.OnceDone(device_LoadUnload.a_X_Push))
                {
                    ret = true;
                    break;
                }
                if (HTHelper.DateTime.GetTimeSec() - t0 > timeDelay_second)
                {
                    errString = String.Format("{0}推杆运动超时", HTM.GetAxisName(device_LoadUnload.a_X_Push));
                    ret = false;
                    goto _end;
                }
                if (HTM.ReadDI(device_LoadUnload.i_PushCst_Err) == 1)
                {
                    errString = String.Format("{0}推杆被阻挡", HTM.GetAxisName(device_LoadUnload.a_X_Push));
                    ret = false;
                    goto _end;
                }
                Thread.Sleep(20);
            }
            #endregion
            ret = true;
            _end:
            if (!CheckErr(HTM.Stop, device_LoadUnload.a_X_Push))
            {
                errString = String.Format("{0}停止失败", HTM.GetAxisName(device_LoadUnload.a_X_Push));
                ret = false;
            }
            return ret;
        }

        /// <summary>
        /// 检查是否有料盒
        /// </summary>
        /// <returns></returns>
        public Boolean IsHaveMgz()
        {
            Boolean ret = false;
            #region 1.检查运行模式
            if (runMode == Convert.ToInt16(SystemRunMode.MODE_OFFLINE))
            {
                return true;
            }
            #endregion

            #region 2.检查回零状态
            //if (!ready)
            //{
            //    errString = String.Format("{0}未回零！", true ? "上料机构" : "下料机构");
            //    return false;
            //}
            #endregion
            #region 动作

            //3.判断夹抓是否有料盒
            if (0 == HTM.ReadDI(device_LoadUnload.i_Jaw_HaveMgz))
            {
                //有料盒则打开 夹爪
                //if (!CheckErr(HTM.ASMoveOver, device_LoadUnload.a_JawZ, z_JawOpenedPos, speed))
                //{
                //    errString = String.Format("夹爪内有料盒,夹爪打开失败!");
                //    ret = -1;
                //    goto _end;
                //}
                ret = false;
                goto _end;
            }
            ret = true;
        #endregion
        _end:
            return ret;
        }

        /// <summary>
        /// 打开料盒夹爪
        /// </summary>
        /// <returns></returns>
        public Boolean OpenMgzJaw()
        {
            Boolean ret = false;
            #region 1.检查运行模式
            if (runMode == Convert.ToInt16(SystemRunMode.MODE_OFFLINE))
            {
                return true;
            }
            #endregion

            #region 2.检查回零状态
            //if (!ready)
            //{
            //    errString = String.Format("{0}未回零！", true ? "上料机构" : "下料机构");
            //    return false;
            //}
            #endregion
            # region 动作
            //1.打开料盒夹爪
            if(0!=HTM.SetSVOver(device_LoadUnload.a_JawZ))
            {
                errString = String.Format("夹爪励磁失败!");
                ret = false;
                goto _end;
            }
            if (0!=HTM.HomeOver(device_LoadUnload.a_JawZ))
            {
                errString = String.Format("夹爪(回零)打开失败!");
                ret = false;
                goto _end;
            }
            ret = true;
        #endregion
        _end:
            return ret;
        }
        /// <summary>
        /// 闭合料盒夹爪
        /// </summary>
        /// <returns></returns>
        public Boolean CloseMgzJaw()
        {
            Boolean ret = false;
            #region 1.检查运行模式
            if (runMode == Convert.ToInt16(SystemRunMode.MODE_OFFLINE))
            {
                return true;
            }
            #endregion

            #region 2.检查回零状态
            if (!ready)
            {
                errString = String.Format("{0}未回零！", true ? "上料机构" : "下料机构");
                return false;
            }
            #endregion
            # region 动作
            //1.闭合料盒夹爪
            //运动到最小料盒也能夹紧的位置
            if (!CheckErr(HTM.ASMove, device_LoadUnload.a_JawZ, -450.0, speed))
            {
                errString = String.Format("夹爪运动失败!");
                ret = false;
                goto _end;
            }
            //循环检测夹爪到位信号
            double t0 = HTHelper.DateTime.GetTimeSec();
            do
            {
                if ((HTHelper.DateTime.GetTimeSec() - t0) > 60)
                {
                    ret = false;
                    errString = String.Format("夹料盒超时!");

                    if ((errCode = HTM.Stop(device_LoadUnload.a_JawZ)) < 0)
                    {
                        errString = String.Format("{0}急停失败", HTM.GetAxisName(device_LoadUnload.a_JawZ));
                        goto _end;
                    }

                    goto _end;
                }
                //读取夹紧信号
                if (HTM.ReadDI(device_LoadUnload.i_Jaw_Closed) == 1)
                {
                    if ((errCode = HTM.Stop(device_LoadUnload.a_JawZ)) < 0)
                    {
                        errString = String.Format("{0}急停失败", HTM.GetAxisName(device_LoadUnload.a_JawZ));
                        goto _end;
                    }
                    break;
                }
                //轴停止运动了
                if (HTM.OnceDone(device_LoadUnload.a_JawZ) == 1)
                {
                    if ((errCode = HTM.Stop(device_LoadUnload.a_Y_Btm)) < 0)
                    {
                        errString = String.Format("{0}急停失败", HTM.GetAxisName(device_LoadUnload.a_Y_Btm));
                        goto _end;
                    }
                    errString = String.Format("料盒未正常夹住!");
                    ret = false;
                    goto _end;
                }
            } while (true);
            ret = true;
        #endregion
        _end:
            return ret;
        }
        /// <summary>
        /// 打开导轨夹爪
        /// </summary>
        /// <returns></returns>
        public Boolean OpenJaw()
        {
            Boolean ret = false;
            #region 1.检查运行模式
            if (runMode == Convert.ToInt16(SystemRunMode.MODE_OFFLINE))
            {
                return true;
            }
            #endregion

            #region 2.检查回零状态

            if (App.obj_SystemConfig.JawCatchMode == 0)
            {
                if (!ready)
                {
                    errString = String.Format("{0}未回零！", true ? "上料机构" : "下料机构");
                    return false;
                }
            }
            #endregion
            #region 动作
            //1.打开传输夹爪

            if (App.obj_SystemConfig.JawCatchMode==1)
            {
                if (!CheckErr(HTM.WriteDO, device_LoadUnload.o_Transfer_JawCylinder, 1))
                {
                    errString = String.Format("传送夹爪打开命令执行失败！");
                    ret = false;
                    goto _end;
                }
                if (!CheckErr(HTM.WaitDI, device_LoadUnload.i_Transfer_JawCylinder_Up, 1, 5))
                {
                    errString = String.Format("传送夹爪打开失败！");
                    ret = false;
                    goto _end;
                }
            }
            else
            {
                if (!CheckErr(HTM.ASMoveOver, device_LoadUnload.a_JawFrame, z_JawFrameLoosePos, speed))
                {
                    errString = String.Format("夹爪运动失败!");
                    ret = false;
                    goto _end;
                }
            }
            ret = true;
        #endregion
        _end:
            return ret;
        }
        /// <summary>
        /// 闭合导轨夹爪
        /// </summary>
        /// <returns></returns>
        public Boolean CloseJaw()
        {
            Boolean ret = false;
            #region 1.检查运行模式
            if (runMode == Convert.ToInt16(SystemRunMode.MODE_OFFLINE))
            {
                return true;
            }
            #endregion

            #region 2.检查回零状态

            if (App.obj_SystemConfig.JawCatchMode == 0)
            {
                if (!ready)
                {
                    errString = String.Format("{0}未回零！", true ? "上料机构" : "下料机构");
                    return false;
                }
            }
            #endregion
            #region 动作
            if (App.obj_SystemConfig.JawCatchMode == 1)
            {
                if (!CheckErr(HTM.WriteDO, device_LoadUnload.o_Transfer_JawCylinder, 0))
                {
                    errString = String.Format("传送夹爪夹紧命令执行失败！");
                    ret = false;
                    goto _end;
                }
                if (!CheckErr(HTM.WaitDI, device_LoadUnload.i_Transfer_JawCylinder_Up, 0, 5))
                {
                    errString = String.Format("传送夹爪夹紧失败！");
                    ret = false;
                    goto _end;
                }
            }
            else
            {
                //1.闭合传输夹爪
                if (!CheckErr(HTM.ASMoveOver, device_LoadUnload.a_JawFrame, z_JawFrameCatchPos, speed))
                {
                    errString = String.Format("夹爪运动失败!");
                    ret = false;
                    goto _end;
                }
            }
            ret = true;
        #endregion
        _end:
            return ret;
        }
        /// <summary>
        /// 推杆回到产品左位置    by M.Bing
        /// </summary>
        /// <returns></returns>
        public Boolean PushRodLeft()
        {
            Boolean ret = false;
            #region 1.检查运行模式
            if (runMode == Convert.ToInt16(SystemRunMode.MODE_OFFLINE))
            {
                return true;
            }
            #endregion

            #region 2.检查回零状态
            if (!ready)
            {
                errString = String.Format("{0}未回零！", true ? "上料机构" : "下料机构");
                return false;
            }
            #endregion
            # region 推片X回到安全位
            if (!CheckErr(HTM.ASMoveOver, device_LoadUnload.a_X_Push, x_PushRodWaitPos, speed))
            {
                errString = String.Format("{0}到安全位置运动失败!", HTM.GetAxisName(device_LoadUnload.a_X_Push));
                goto _end;
            }
            ret = true;
            #endregion
            _end:
            if ((errCode = HTM.Stop(device_LoadUnload.a_X_Push)) < 0)
            {
                errString = String.Format("{0}急停失败", HTM.GetAxisName(device_LoadUnload.a_X_Push));
                ret = false;
            }
            return ret;
        }

        /// <summary>
        /// 推杆退回到安全位置    by M.Bing
        /// </summary>
        /// <returns></returns>
        public Boolean PushRodSafe()
        {
            Boolean ret = false;
            #region 1.检查运行模式
            if (runMode == Convert.ToInt16(SystemRunMode.MODE_OFFLINE))
            {
                return true;
            }
            #endregion

            #region 2.检查回零状态
            if (!ready)
            {
                errString = String.Format("{0}未回零！", true ? "上料机构" : "下料机构");
                return false;
            }
            #endregion
            # region 推片X回到安全位
            if (!CheckErr(HTM.ASMoveOver, device_LoadUnload.a_X_Push, x_PushRodSafePos, speed))
            {
                errString = String.Format("{0}到安全位置运动失败!", HTM.GetAxisName(device_LoadUnload.a_X_Push));
                goto _end;
            }
            ret = true;
        #endregion
        _end:
            if ((errCode = HTM.Stop(device_LoadUnload.a_X_Push)) < 0)
            {
                errString = String.Format("{0}急停失败", HTM.GetAxisName(device_LoadUnload.a_X_Push));
                ret = false;
            }
            return ret;
        }
        /// <summary>
        /// 检测NG料盒是否安装到位 
        /// </summary>
        /// <returns>true 存在 false 不存在</returns>
        public bool CheckNgMgzExist()
        {
            if ((1 == HTM.ReadDI(device_LoadUnload.i_NgMgz_Btm_ON)) && (1 == HTM.ReadDI(device_LoadUnload.i_NgMgz_Side_ON)))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 传输机构最左端或最右端是否有料片
        /// </summary>
        /// <returns></returns>
        public bool CheckLeftHaveFrame()
        {
            #region 1.检查运行模式
            if (runMode == Convert.ToInt16(SystemRunMode.MODE_OFFLINE))
            {
                return true;
            }
            #endregion

            #region 2.检查回零状态
            if (!ready)
            {
                errString = String.Format("{0}未回零！", "机构");
                return false;
            }
            #endregion
            if (1 == HTM.ReadDI(device_LoadUnload.i_Transfer_HaveCst))
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// NG料盒操作
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public bool NgMgzJawCylinderFunc(int state)
        {
            #region 1.检查运行模式
            if (runMode == Convert.ToInt16(SystemRunMode.MODE_OFFLINE))
            {
                return true;
            }
            #endregion

            #region 2.检查回零状态
            if (!ready)
            {
                errString = String.Format("{0}未回零！", "机构");
                return false;
            }
            #endregion
            if (!CheckErr(HTM.WriteDO, device_LoadUnload.o_Jaw_NgMgz_Close, state))
            {
                return false;
            }
            if (state == 0)
            {
                if (HTM.WaitDI(device_LoadUnload.i_Jaw_NgMgz_Closed, 1, 20) == 0)
                {
                    return false;
                }
            }
            if (state == 0)
            {
                if (HTM.WaitDI(device_LoadUnload.i_Jaw_NgMgz_Opened, 1, 20) == 0)
                {
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// 使能上/下料机构Z轴刹车
        /// </summary>
        /// <param name="Left">true上料机构,false下料机构</param>
        /// <returns></returns>
        public Boolean EnableAxisZStoper(bool Left = true)
        {
            Boolean ret = false;
            #region 1.检查运行模式
            if (runMode == Convert.ToInt16(SystemRunMode.MODE_OFFLINE))
            {
                return true;
            }
            #endregion

            #region 2.检查回零状态
            if (!ready)
            {
                errString = String.Format("{0}未回零！", true ? "上料机构" : "下料机构");
                return false;
            }
            #endregion
            # region 使能上下料机构刹车
            if (!CheckErr(HTM.WriteDO, device_LoadUnload.o_Z_Stopping, 0))
            {
                errString = String.Format("{0}使能失败!", HTM.GetAxisName(device_LoadUnload.o_Z_Stopping));
                goto _end;
            }
            ret = true;
            #endregion
            _end:
            return ret;
        }
        #endregion
    }
}