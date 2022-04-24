using ToolKits.ADLinkModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ToolKits.ADLinkModule
{
    /// <summary>
    /// 板卡管理
    /// </summary>
    public class BoardCardMgr
    {
        #region 字段
        /// <summary>
        /// 板卡管理静态实例
        /// </summary>
        private static readonly BoardCardMgr m_boardCardMgr;

        /// <summary>
        /// 运动板卡
        /// </summary>
        public GeneralMotionCard m_motionCard;

        /// <summary>
        /// 左侧镜子轴(音圈电机)
        /// </summary>
        //private ExecuteBody m_leftMirrorAxis;

        /// <summary>
        /// 右侧镜子轴(音圈电机)
        /// </summary>
        //private ExecuteBody m_rightMiroorAxis;

        /// <summary>
        /// Z轴(步进电机)
        /// </summary>
        public ExecuteBody m_zAxis;

        /// <summary>
        /// 初始化标志
        /// </summary>
        private bool initFlag = false;
        #endregion

        #region 属性
        /// <summary>
        ///板卡管理静态实例，单实例 
        /// </summary>        
        public static BoardCardMgr BoardCardInstance
        {
            get { return m_boardCardMgr; }
        }

        /// <summary>
        /// 运动板卡
        /// </summary>
        public GeneralMotionCard MotionCard
        {
            get { return m_motionCard; }
        }

        /// <summary>
        /// 左侧镜子轴
        /// </summary>
        //public ExecuteBody LeftMirrorAxis
        //{
        //    //get { return m_leftMirrorAxis; }
        //}
        #endregion

        #region 构造器
        /// <summary>
        /// 实例化板卡对象
        /// </summary>
        static BoardCardMgr()
        {
            m_boardCardMgr = new BoardCardMgr();
        }

        private BoardCardMgr()
        {

        }
        #endregion

        #region 方法
        #region 资源初始化与释放
        public bool Init(double pulseEqu)
        {
            try
            {
                if (initFlag == false)
                {
                    m_motionCard = new APS168BoardCard();

                    //轴
                    //m_leftMirrorAxis = new ExecuteBody("左侧镜子轴");
                    //m_leftMirrorAxis.AxisLocationInfo = new AxisLocationInfo(0, 1);                               //轴位置信息(必须外部配好)
                    //m_leftMirrorAxis.AxisSignalParams = new AxisSignalParams(0, 0, 0, 0, 0, 0, 0, 0, 0);          //轴有效电平信息(必须外部配好)

                    m_zAxis = new ExecuteBody("步进Z轴");
                    m_zAxis.AxisLocationInfo = new AxisLocationInfo(0, 0);                               //轴位置信息(必须外部配好)
                    m_zAxis.AxisSignalParams = new AxisSignalParams(1, 0, 1, 1, 1, 1, 0, 0, 0);          //轴有效电平信息(必须外部配好)

                    //加载该轴配置文件
                    m_zAxis.TransmissionParams = new TransmissionParams(pulseEqu);
                    m_zAxis.VelocityCurveParams = new VelocityCurveParams(0, 100, 10000, 10000, 0, 0);
                    


                    //IO点

                    //初始化板卡
                    if(m_motionCard.InitBoard() < 0)
                    {
                        return false;
                    }
                    //配置各轴(必须配置)
                    m_motionCard.SetAxisSignalConfig(m_zAxis);
                    m_motionCard.SetServo(m_zAxis, 1);

                    initFlag = true;
                }               
            }
            catch (Exception)
            {
            }
            return true;
        }
        public bool Close()
        {
            try
            {
                if (initFlag == true)
                {
                    if (m_motionCard.CloseBoard() != 0)
                        return false;                 
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion

        #region 轴信息存取
        /// <summary>
        /// 从文件中读取轴配置信息
        /// </summary>
        /// <returns></returns>
        public int readAxisConfig(ExecuteBody executeBody)
        {
            //1. 轴位置信息直接在程序里面写死，不在文件中配置

            //2. 传动信息，只保存电机的脉冲当量

            //3. 轴电平信息直接在程序里面写死，不在文件中配置

            //4. 速度文件保存，保存加速度，减速度，曲线因子，最大速度，超时时间

            //5. 软限位保存，保存是否使能，正限位位置，负限位位置

            //6. 回零保存，保存模式，方向，Z向有效性，回零速度，找原点速度，原点偏移，超时时间
            return 0;

        }

        /// <summary>
        /// 将轴配置写入文件
        /// </summary>
        /// <returns></returns>
        public int writeAxisConfig(ExecuteBody executeBody)
        {
            return 0;
        }
        #endregion
        #endregion
    }
}
