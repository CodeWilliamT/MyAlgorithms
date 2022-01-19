
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HTM_BSP;
using System.Windows.Forms;

namespace CameraZaxisScanModel
{
    class AxisPart
    {
        /// <summary>运动速率(-1.0~1.0)</param>
        public static Double speed;
        /// <param name="a_Z">轴号(0-based)</param>
        public static Int32 a_Z;
        /// <summary>轴运动模式</summary>
        public static Int32 mode =(Int32)HTM_BSP.MotionMode.ABS;
        public static bool HTMOnline;
        public static Double Pos_minLimit = -23;
        public static Double Pos_maxLimit = 0;
        public enum HTMOnline_mode
        {
            OFF=1,
            ON=0
        }
        public static void Initialize(HTMOnline_mode mode)
        {
            try
            {
                INIT_PARA _init = new INIT_PARA();
                _init.config_file = Application.StartupPath + @"\HTMSystem\Paras.db";
                _init.max_axis_num = 1;
                _init.max_io_num = 0;
                _init.max_dev_num = 0;
                _init.offline_mode = (byte)mode;         //OFFLINE mode 0在线1离线
                _init.use_aps_card = 0;
                _init.use_htnet_card = 1;          //1 - such card exists in the system
                int err;
                err = HTM.Init(ref _init);
                if(err<0)
                {
                    throw new Exception("HTM初始化失败！");
                }
                speed = 1.0;
                a_Z = 0;
                HTMOnline=((byte)mode)==0?true:false;
                Z_SetSV();
                Z_Home();
                
            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
            }
        }
        /// <summary>
        /// Z轴励磁,by TWL
        /// </summary>
        public static void Z_SetSV()
        {
            if (!HTMOnline) return;
            int errCode;
            //发送运动指令
            if ((errCode = HTM.SetSVON(a_Z,1)) < 0)
            {
                throw new Exception("Z轴励磁失败！");
            }
            if ((errCode = HTM.SVDone(a_Z)) < 0)
            {
                throw new Exception("Z轴励磁失败！");
            }
        }
        /// <summary>
        /// Z轴励磁,by TWL
        /// </summary>
        public static void Z_Home()
        {
            if (!HTMOnline) return;
            int errCode;
            //发送运动指令
            if ((errCode = HTM.Home(a_Z)) < 0)
            {
                throw new Exception("Z轴回零失败！");
            }
            if ((errCode = HTM.HomeDone(a_Z)) < 0)
            {
                throw new Exception("Z轴回零失败！");
            }
        }
        /// <summary>
        /// Z轴绝对运动,by TWL
        /// </summary>
        /// <param name="zPos">终止位置单位mm或deg</param>
        public static void Z_Move(Double zPos)
        {
            if (zPos > Pos_maxLimit)
            {
                throw new Exception("Z轴运动指令超运动上限！不发送！");
            }
            if (zPos < Pos_minLimit)
            {
                throw new Exception("Z轴运动指令超运动下限！不发送！");
            }
            if (!HTMOnline) return;
            int errCode;
            //发送运动指令
            if ((errCode = HTM.Move(a_Z, zPos, speed, mode)) < 0)
            {
                throw new Exception("Z轴运动指令发送失败！");
            }
            // 3. 等待运动完成
            if ((errCode = HTM.Done(a_Z)) < 0)
            {
                throw new Exception("Z轴运动失败！");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="zRSPos">相对运动距离单位mm或deg</param>
        public static void Z_RSMove(Double zRSPos)
        {
            Double zPos = Get_Z_NowPos() + zRSPos;
            if (zPos > Pos_maxLimit)
            {
                throw new Exception("Z轴运动指令超运动上限！不发送！");
            }
            if (zPos < Pos_minLimit)
            {
                throw new Exception("Z轴运动指令超运动下限！不发送！");
            }
            if (!HTMOnline) return;
            int errCode;
            //发送运动指令
            if ((errCode = HTM.RSMove(a_Z, zRSPos, speed)) < 0)
            {
                throw new Exception("Z轴运动指令发送失败！");
            }
            // 3. 等待运动完成
            if ((errCode = HTM.Done(a_Z)) < 0)
            {
                throw new Exception("Z轴运动失败！");
            }
        }
        //获取Z轴当前位置
        public static double Get_Z_NowPos()
        {
            return HTM.GetFbkPos(a_Z);
        }
    }
}
