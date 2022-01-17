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
using System.IO;
using System.ComponentModel;
namespace LeadframeAOI
{

    /// <summary>
    /// <summary>
    /// Class for Load/unload magzine module
    /// </summary>
    class UnLoadModule : BaseModule
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
        /// 导轨夹爪Z轴闭合抓取位
        /// </summary>
        public Double z_JawFrameCatchPos = 0;
        /// <summary>
        /// 导轨夹爪Z轴张开位
        /// </summary>
        public Double z_JawFrameLoosePos = 0;
        #endregion


        #region 下料独有
        /// <summary>
        /// Y轴NG下片位
        /// </summary>
        public Double y_UnLoadNgFramePos = 0;
        /// <summary>
        /// 下料传输推片位（推杆感应位）
        /// </summary>
        public Double x_TransfeRightTouchPushPos = 0;
        /// <summary>
        /// 下料传输推片结束位
        /// </summary>
        public Double x_TransfeRightJawPushOverCstPos = 0;
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
        public double Y_LoadMgzPos { get { return y_LoadMgzPos; } set { y_LoadMgzPos = value; } }
        [CategoryAttribute("流程属性"), DisplayNameAttribute("上料盒位-Z坐标(mm)"), DescriptionAttribute("料盒搬运机构Z轴,Z方向")]
        public double Z_LoadMgzPos { get { return z_LoadMgzPos; } set { z_LoadMgzPos = value; } }
        [CategoryAttribute("流程属性"), DisplayNameAttribute("下料盒位-Y坐标(mm)"), DescriptionAttribute("料盒搬运机构Y轴,Y方向")]
        public double Y_UnLoadMgzPos { get { return y_UnLoadMgzPos; } set { y_UnLoadMgzPos = value; } }
        [CategoryAttribute("流程属性"), DisplayNameAttribute("下料盒位-Z坐标(mm)"), DescriptionAttribute("料盒搬运机构Z轴,Z方向")]
        public double Z_UnLoadMgzPos { get { return z_UnLoadMgzPos; } set { z_UnLoadMgzPos = value; } }
        [CategoryAttribute("流程属性"), DisplayNameAttribute("夹爪张开位(mm)"), DescriptionAttribute("料盒固料夹爪位置,Z方向")]
        public double Z_JawOpenedPos { get { return z_JawOpenedPos; } set { z_JawOpenedPos = value; } }
        [CategoryAttribute("流程属性"), DisplayNameAttribute("夹爪闭合位(mm)"), DescriptionAttribute("料盒固料夹爪位置,Z方向")]
        public double Z_JawClosedPos { get { return z_JawClosedPos; } set { z_JawClosedPos = value; } }

        [CategoryAttribute("流程属性"), DisplayNameAttribute("导轨右夹爪夹取闭合位(mm)"), DescriptionAttribute("导轨右侧夹爪位置,Z方向")]
        public double Z_JawFrameCatchPos { get { return z_JawFrameCatchPos; } set { z_JawFrameCatchPos = value; } }

        [CategoryAttribute("流程属性"), DisplayNameAttribute("导轨左夹爪夹取张开位(mm)"), DescriptionAttribute("导轨左侧夹爪位置,Z方向")]
        public double Z_JawFrameLoosePos { get { return z_JawFrameLoosePos; } set { z_JawFrameLoosePos = value; } }
        [CategoryAttribute("流程属性"), DisplayNameAttribute("导轨右夹爪左安全位(mm)"), DescriptionAttribute("导轨右侧夹爪位置,X方向")]
        public double X_TransfeJawLeftSafeCstPos { get { return x_TransfeJawLeftSafeCstPos; } set { x_TransfeJawLeftSafeCstPos = value; } }
        [CategoryAttribute("流程属性"), DisplayNameAttribute("导轨右夹爪右安全位(mm)"), DescriptionAttribute("导轨右侧夹爪位置,X方向")]
        public double X_TransfeJawRightSafeCstPos { get { return x_TransfeJawRightSafeCstPos; } set { x_TransfeJawRightSafeCstPos = value; } }

        [CategoryAttribute("流程属性-下料独有"), DisplayNameAttribute("NG料下片点位-Y坐标(mm)"), DescriptionAttribute("料盒搬运机构Y轴,Y方向")]
        public double Y_UnLoadNgFramePos { get { return y_UnLoadNgFramePos; } set { y_UnLoadNgFramePos = value; } }
        [CategoryAttribute("流程属性-下料独有"), DisplayNameAttribute("导轨右推杆感应位(mm)"), DescriptionAttribute("导轨右侧夹爪位置,X方向")]
        public double X_TransfeRightTouchPushPos { get { return x_TransfeRightTouchPushPos; } set { x_TransfeRightTouchPushPos = value; } }
        [CategoryAttribute("流程属性-下料独有"), DisplayNameAttribute("下料推杆结束位(mm)"), DescriptionAttribute("导轨右侧夹爪位置,X方向")]
        public double X_TransfeRightJawPushOverCstPos { get { return x_TransfeRightJawPushOverCstPos; } set { x_TransfeRightJawPushOverCstPos = value; } }
        #endregion

        #region 公共方法
        public UnLoadModule(String para_file, String para_table) : base(para_file, para_table) { }

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
            if (Left)
            {
            }
            else
            {
            }

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

                errString = String.Format("夹爪内有料盒,请取出");
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
                        errString = String.Format("{0}刹车信号关闭失败", HTM.GetAxisName(device_LoadUnload.a_Z)); ;
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
                    errString = String.Format("下料机构不良品夹爪气缸复位位失败"); ;
                    goto _end;
                }
                if (!CheckErr(HTM.WaitDI, device_LoadUnload.i_Jaw_NgMgz_Opened, 1, timeDelay))
                {
                    errString = String.Format("未检测到下料机构不良品夹爪张开"); ;
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
                //if(!CheckErr(HTM.ASMoveOver,device_LoadUnload.a_JawZ, z_JawOpenedPos,speed))
                //{
                //    errString = String.Format("夹爪内有料盒,夹爪打开失败");
                //    ret = -1;
                //    goto _end;
                //}
                errString = String.Format("夹爪内有料盒,请取出");
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
            ////3.判断夹抓是否有料盒
            //if (1 == HTM.ReadDI(device_LoadUnload.i_Jaw_HaveMgz))
            //{
            //    //有料盒则打开 夹爪
            //    //if(!CheckErr(HTM.ASMoveOver,device_LoadUnload.a_JawZ, z_JawOpenedPos,speed))
            //    //{
            //    //    errString = String.Format("夹爪内有料盒,夹爪打开失败");
            //    //    ret = -1;
            //    //    goto _end;
            //    //}
            //    errString = String.Format("夹爪内有料盒,请取出");
            //    ret = -1;
            //    goto _end;
            //}
            //4.打开夹爪
            if (!CheckErr(HTM.ASMoveOver, device_LoadUnload.a_JawZ, z_JawOpenedPos, speed))
            {
                errString = String.Format("夹爪打开失败");
                ret = -1;
                goto _end;
            }
            //7.下平台推料盒
            if (!CheckErr(HTM.Speed, device_LoadUnload.a_Y_Btm, -1.0 * speed))
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
                    errString = String.Format("{0}S内未检测到料盒到位，下平台料盒已空", 30, Left ? "上料机构" : "下料机构");
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
                errString = String.Format("夹爪运动失败");
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
                    errString = String.Format("夹料盒超时");

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
                    errString = String.Format("料盒未正常夹住");
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
                    errString = String.Format("检测满仓信号超时");
                    ret = -1;
                    goto _end;
                }
                //3.1 运动停止前检测到满仓信号
                if (1 == HTM.ReadDI(device_LoadUnload.i_Mgz_Full_TOP))
                {
                    //y轴停止
                    if (!CheckErr(HTM.Stop, device_LoadUnload.a_Y))
                    {
                        errString = String.Format("{0}推片X轴停止失败", HTM.GetAxisName(device_LoadUnload.a_Y));
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
                        Thread.Sleep(500);
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
            //3.夹住料片
            if (!CloseJaw())
            {
                errString = String.Format("传送夹爪夹紧失败！");
                ret = false;
                goto _end;
            }
            //4.拖到最右端
            if (!CheckErr(HTM.ASMoveOver, device_LoadUnload.a_JawX, x_ChuckLoadFramePos, speed))
            {
                errString = String.Format("{0}运动失败", HTM.GetAxisName(device_LoadUnload.a_JawX));
                ret = false;
                goto _end;
            }
            //5.松开料片
            if (!OpenJaw())
            {
                errString = String.Format("传送夹爪打开失败！");
                ret = false;
                goto _end;
            }
            //6.走到最左端
            if (!CheckErr(HTM.ASMoveOver, device_LoadUnload.a_JawX, (x_TransfeJawLeftSafeCstPos + x_TransfeJawRightSafeCstPos) / 2, speed))
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
            //2.运动到左安全位  中间安全位
            if (!CheckErr(HTM.ASMoveOver, device_LoadUnload.a_JawX, x_TransfeJawLeftSafeCstPos, speed))
            {
                errString = String.Format("{0}运动失败", HTM.GetAxisName(device_LoadUnload.a_JawX));
                ret = false;
            }
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
                        errString = String.Format("执行降下下料推杆命令失败");
                        ret = false;
                        goto _end;
                    }
                    if (1 != HTM.WaitDI(device_LoadUnload.i_Unload_PushCstLiftCylinderDown, 1, 10))
                    {
                        errString = String.Format("下降下料推杆超时失败");
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
        /// 下料料盒去接料
        /// </summary>
        /// <param name="Index">用第几层</param>
        /// <param name="OkCst">用OK品料盒去接料</param>
        /// <returns></returns>
        public Boolean UnlaodMgzMove2ReceiveFrame(int Index, Boolean OkCst = true)
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

            //1.Z,Y轴运动到上片位
            if (!CheckErr(HTM.ASMoveOver, device_LoadUnload.a_Y, (OkCst) ? (y_LoadUnLoadFramePos + (Index - 1) * heightSlotLayer_y) : y_UnLoadNgFramePos, speed))
            {
                errString = String.Format("{0}运动失败", HTM.GetAxisName(device_LoadUnload.a_Y));
                ret = false;
                goto _end;
            }
            if (!CheckErr(HTM.ASMoveOver, device_LoadUnload.a_Z, (z_LoadUnLoadFramePos + (Index - 1) * heightSlotLayer), speed))
            {
                errString = String.Format("{0}运动失败", HTM.GetAxisName(device_LoadUnload.a_Z));
                ret = false;
                goto _end;
            }

            ret = true;
        _end:

            return ret;
        }
        /// <summary>
        /// 功能：把右等待位的片子传到传输机构最右端  by M.Bing
        /// </summary>
        /// <param name="Index">第几片，与上料处对应</param>
        /// <param name="OkCst">true 良品cst </param>
        /// <returns></returns>
        public Boolean TransferFrameToMgz()
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
            //if (1 == HTM.ReadDI(device_LoadUnload.i_Transfer_HaveCst))
            //{
            //    errString = String.Format("下料端有料片阻挡！");
            //    goto _end;
            //}
            #endregion

            #region 4.动作流程
            //如果推杆处于抬升状态,下降推杆
            if (1 != HTM.WaitDI(device_LoadUnload.i_Unload_PushCstLiftCylinderDown, 1, 10))
            {
                if (!CheckErr(HTM.WriteDO, device_LoadUnload.o_Unload_PushCstLiftCylinder, 0))
                {
                    errString = String.Format("执行降下下料推杆命令失败");
                    ret = false;
                    goto _end;
                }
                if (1 != HTM.WaitDI(device_LoadUnload.i_Unload_PushCstLiftCylinderDown, 1, 10))
                {
                    errString = String.Format("下降下料推杆超时失败");
                    ret = false;
                    goto _end;
                }
            }
            double t0 = HTHelper.DateTime.GetTimeSec();
            int moveStatus = 0;  //单次运动结果  1--检测到左端传感器熄灭  -1--移动到右侧极限也没有检测到传感器熄灭
            while (true)
            {
                if ((t0 - HTHelper.DateTime.GetTimeSec()) > 20)
                {
                    errString = String.Format("{0}运动到传输轴右传感器亮起超时！", HTM.GetAxisName(device_LoadUnload.a_JawX));
                    ret = false;
                    goto _end;
                }

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
                //if (!CheckErr(HTM.ASMoveOver, device_LoadUnload.a_JawX, x_TransfeRightJawPushOverCstPos, speed))
                //{
                //    errString = String.Format("{0}运动失败", HTM.GetAxisName(device_LoadUnload.a_JawX));
                //    ret = false;
                //    goto _end;
                //}
                //4.移动到最右端
                //if (!CheckErr(HTM.ASMoveOver, device_LoadUnload.a_JawX, x_TransfeRightJawPushCstPos, speed))
                //{
                //    errString = String.Format("{0}运动失败", HTM.GetAxisName(device_LoadUnload.a_JawX));
                //    ret = false;
                //    goto _end;
                //}
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
                        errString = String.Format("{0}运动到传输轴右传感器亮起超时！", HTM.GetAxisName(device_LoadUnload.a_JawX));
                        ret = false;
                        goto _end;
                    }
                    //获取传输轴右传感器状态
                    if (1 == HTM.ReadDI(device_LoadUnload.i_Transfer_HaveCst))
                    {
                        //停止
                        if (HTM.Stop(device_LoadUnload.a_JawX, HTM.StopMode.EMG) < 0)
                        {
                            errString = String.Format("{0}停止异常", HTM.GetAxisName(device_LoadUnload.a_JawX));
                            ret = false;
                            goto _end;
                        }
                        Thread.Sleep(200);
                        //if (HTM.RSMoveOver(device_LoadUnload.a_JawX, 10, speed) < 0)
                        //{
                        //    errString = String.Format("{0}运动异常", HTM.GetAxisName(device_LoadUnload.a_JawX));
                        //    ret = false;
                        //    goto _end;
                        //}
                        //Thread.Sleep(200);
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
                } while (true);
                if (moveStatus == 1)
                {
                    break;
                }
            }
            //8.张开夹爪
            if (!OpenJaw())
            {
                errString = String.Format("传送夹爪打开失败！");
                ret = false;
                goto _end;
            }
            ////2.运动到右安全位
            //if (!CheckErr(HTM.ASMoveOver, device_LoadUnload.a_JawX, x_TransfeJawRightSafeCstPos, speed))
            //{
            //    errString = String.Format("{0}运动失败", HTM.GetAxisName(device_LoadUnload.a_JawX));
            //    ret = false;
            //}
            ////9.退回到下料推杆推料位
            //if (!CheckErr(HTM.ASMoveOver, device_LoadUnload.a_JawX, x_TransfeRightJawPushCstPos, speed))
            //{
            //    errString = String.Format("{0}运动失败", HTM.GetAxisName(device_LoadUnload.a_JawX));
            //    ret = false;
            //}
            #endregion

            ret = true;
        _end:
            #region 异常及后续动作处理
            if (!ret)
            {
                //1.打开传输夹爪
                if (!OpenJaw())
                {
                    errString = String.Format("传送夹爪打开失败！");
                    ret = false;
                }
                //2.运动到右安全位
                if (!CheckErr(HTM.ASMoveOver, device_LoadUnload.a_JawX, x_TransfeJawRightSafeCstPos, speed))
                {
                    errString = String.Format("{0}运动失败", HTM.GetAxisName(device_LoadUnload.a_JawX));
                    ret = false;
                }
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
        /// 功能：把frame从最右端推入料盒（片已经部分在料盒内）  by M.Bing
        /// </summary>
        /// <returns></returns>
        public Boolean UnloadFrameToMgz()
        {
            bool ret = false;
            //if ((ret = TransferFrameToMgz()) == false) goto _end;
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
            if (1 == HTM.ReadDI(device_LoadUnload.i_FrameBendChecker))
            {
                errString = String.Format("料片曲翘，无法推送！");
                return false;
            }
            //判定有无抬升空间
            double x_TransfeRightPushStartPos = (x_TransfeRightTouchPushPos - FrameLength - 5) > x_TransfeJawLeftSafeCstPos ? (x_TransfeRightTouchPushPos - FrameLength - 5) : x_TransfeJawLeftSafeCstPos;
            if ((x_TransfeRightTouchPushPos - FrameLength - 5) < x_TransfeJawLeftSafeCstPos)
            {
                errString = String.Format("料片过长，没有推杆抬升空间！");
                return false;
            }
            //1.下料夹爪移动到推料位
            if (!CheckErr(HTM.ASMoveOver, device_LoadUnload.a_JawX, x_TransfeRightPushStartPos, speed))
            {
                errString = String.Format("{0}运动失败", HTM.GetAxisName(device_LoadUnload.a_JawX));
                ret = false;
                goto _end;
            }
            //2.抬起推杆
            if (!CheckErr(HTM.WriteDO, device_LoadUnload.o_Unload_PushCstLiftCylinder, 1))
            {
                errString = String.Format("执行抬起下料推杆命令失败");
                ret = false;
                goto _end;
            }
            if (1 != HTM.WaitDI(device_LoadUnload.i_Unload_PushCstLiftCylinderUp, 1, 10))
            {
                errString = String.Format("抬起下料推杆超时失败");
                ret = false;
                goto _end;
            }
            //3.推杆推出
            if (!CheckErr(HTM.ASMove, device_LoadUnload.a_JawX, x_TransfeRightJawPushOverCstPos, 0.4 * speed))
            {
                errString = String.Format("{0}运动失败", HTM.GetAxisName(device_LoadUnload.a_JawX));
                ret = false;
                goto _end;
            }
            //4.检测防撞信号 
            double t0 = HTHelper.DateTime.GetTimeSec();
            do
            {
                //推杆推出到位
                if (1 == HTM.OnceDone(device_LoadUnload.a_JawX))
                {
                    break;
                }
                if ((t0 - HTHelper.DateTime.GetTimeSec() > 10))
                {
                    errString = String.Format("下料推杆推出超时");
                    ret = false;
                    goto _end;
                }
                if (1 == HTM.ReadDI(device_LoadUnload.i_FrameBendChecker))
                {
                    errString = String.Format("料片曲翘，无法推送");
                    if (HTM.Stop(device_LoadUnload.a_JawX, HTM.StopMode.EMG) < 0)
                    {
                        errString = String.Format("料片曲翘!且夹爪停止运动失败");
                    }
                    ret = false;
                    goto _end;
                }
                if (1 == HTM.ReadDI(device_LoadUnload.i_PushCst_Err))
                {
                    if (HTM.Stop(device_LoadUnload.a_JawX, HTM.StopMode.EMG) < 0)
                    {
                        errString = String.Format("下料推杆推片发生碰撞!且夹爪停止运动失败");
                    }
                    errString = String.Format("下料推杆推片发生碰撞!停止运动");
                    ret = false;
                    goto _end;
                }
            } while (true);
            #endregion
            ret = true;
        _end:
            #region 异常处理及后续处理
            double NowX = 0;
            NowX = HTM.GetFbkPos(device_LoadUnload.a_JawX);
            //推杆缩回
            if (!CheckErr(HTM.ASMoveOver, device_LoadUnload.a_JawX, NowX - 10 > x_TransfeRightPushStartPos ? NowX - 10 : x_TransfeRightPushStartPos, speed))
            {
                errString = String.Format("{0}运动失败", HTM.GetAxisName(device_LoadUnload.a_JawX));
                ret = false;
            }
            //推杆下降
            if (!CheckErr(HTM.WriteDO, device_LoadUnload.o_Unload_PushCstLiftCylinder, 0))
            {
                errString = String.Format("执行下降下料推杆命令失败");
                ret = false;
            }
            if (1 != HTM.WaitDI(device_LoadUnload.i_Unload_PushCstLiftCylinderDown, 1, 10))
            {
                errString = String.Format("下降下料推杆超时失败");
                ret = false;
            }

            //运动到右安全位
            if (!CheckErr(HTM.ASMoveOver, device_LoadUnload.a_JawX, x_TransfeJawRightSafeCstPos, speed))
            {
                errString = String.Format("{0}运动失败", HTM.GetAxisName(device_LoadUnload.a_JawX));
                ret = false;
            }

            #endregion

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
            if (0 != HTM.SetSVOver(device_LoadUnload.a_JawZ))
            {
                errString = String.Format("夹爪励磁失败");
                ret = false;
                goto _end;
            }
            if (0 != HTM.HomeOver(device_LoadUnload.a_JawZ))
            {
                errString = String.Format("夹爪(回零)打开失败");
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
                errString = String.Format("夹爪运动失败");
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
                    errString = String.Format("夹料盒超时");

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
                    errString = String.Format("料盒未正常夹住");
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

            if (App.obj_SystemConfig.JawCatchMode == 1)
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
                errString = String.Format("{0}使能失败", HTM.GetAxisName(device_LoadUnload.o_Z_Stopping));
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
