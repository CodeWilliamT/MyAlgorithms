using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using HTM_BSP;

namespace HTM_interface
{
    public partial class Form_HTM : Form
    {
        //****************私有变量****************************
        const int AXIS_MaxNum = 100;//数组上限
        const int IO_MaxNum = 100;//数组上限
        const int Device_MaxNum = 100;//数组上限
        static Form_HTM _instance = null;//当前窗体实例变量
        UserControl_Axis userControl_axis;//轴信息页面
        UserControl_IOconfig userControl_ioconfig;//io信息页面
        UserControl_Others userControl_others;//其他设备信息页面
        Form_Lv_Inf form_lv_inf = null;//版本信息窗体

        //****************接口变量****************************
        /// <summary>
        /// 轴数据
        /// </summary>
        public Axis_Data[] axis_data = new Axis_Data[AXIS_MaxNum];
        /// <summary>
        /// io数据
        /// </summary>
        public IO_Data[] io_data = new IO_Data[IO_MaxNum];
        /// <summary>
        /// 其他设备数据
        /// </summary>
        public Device_Data[] device_data = new Device_Data[Device_MaxNum];
        /// <summary>
        /// 轴数据数
        /// </summary>
        public int num_axis = 20;
        /// <summary>
        /// io数据数
        /// </summary>
        public int num_io = 30;
        /// <summary>
        /// 其他设备数据
        /// </summary>
        public int num_device = 20;
        /// <summary>
        /// 当前版本号
        /// </summary>
        public string Lv_program = "x64_1.00_2018.01.08";
        //*****************************************************
        public Form_HTM()
        {
            try
            {
                InitializeComponent();
                Set_Axis_Data();
                Set_IO_Data();
                Set_Device_Data();
                label_Lv_program.Text = Lv_program;
                _instance = this;
                userControl_axis = new UserControl_Axis();
                userControl_ioconfig = new UserControl_IOconfig();
                userControl_others = new UserControl_Others();

                userControl_axis.Dock = DockStyle.Fill;
                userControl_ioconfig.Dock = DockStyle.Fill;
                userControl_others.Dock = DockStyle.Fill;

                this.tabPage1.Controls.Add(userControl_axis);
                this.tabPage2.Controls.Add(userControl_ioconfig);
                this.tabPage3.Controls.Add(userControl_others);
            }
            catch (Exception ex)
            {
                MessageBox.Show("初始化失败！\n" + ex.Message);
            }
        }
        #region 公共接口方法
        /// <summary>
        /// 获取当前窗体实例
        /// </summary>
        /// <returns>窗体实例</returns>
        public static Form_HTM Get_Form()
        {
            if (_instance == null || _instance.IsDisposed)
            {
                _instance = new Form_HTM();
            }
            _instance.WindowState = FormWindowState.Normal;
            _instance.Activate();
            return _instance;
        }

        /// <summary>
        /// 构造指定颜色大小的图
        /// </summary>
        /// <param name="color">颜色</param>
        /// <param name="width">宽度</param>
        /// <param name="height">高度</param>
        /// <returns>bitmap实例</returns>
        public Bitmap GenColorImage(Color color, int width, int height)
        {
            Bitmap bitmap_color = new Bitmap(width, height);
            Graphics g = Graphics.FromImage(bitmap_color);
            Rectangle rect = new Rectangle(0, 0, bitmap_color.Width, bitmap_color.Height);
            SolidBrush brush_Color = new System.Drawing.SolidBrush(color);
            g.FillRectangle(brush_Color, rect);
            return bitmap_color;
        }
        #endregion
        #region 数据信息结构
        /// <summary>
        /// 轴信息数据
        /// </summary>
        public struct Axis_Data
        {
            /// <summary>Axis Name</summary>
            public String axisName;
            /// <summary>Axis/Node Address 轴号/节点号</summary>
            public UInt16 nodeAddr;
            /// <summary>指令位置(mm)</summary>
            public Double cmdPos;
            /// <summary>反馈信号(mm)</summary>
            public Double fbkPos;
            /// <summary>正限位信号(0/1)</summary>
            public Byte pel;
            /// <summary>负限位信号(0/1)</summary>
            public Byte mel;
            /// <summary>原点信号(0/1)</summary>
            public Byte org;
            /// <summary>报警信号(0/1)</summary>
            public Byte alm;
            /// <summary>到位信号(0/1)</summary>
            public Byte atp;
            /// <summary>励磁信号(0/1)</summary>
            public Byte svon;

        }
        /// <summary>
        /// IO信息数据
        /// </summary>
        public struct IO_Data
        {
            /// <summary>IO序号</summary>
            public UInt16 index;
            /// <summary>IO名称(备注，不超过32个字节)</summary>
            public String ioName;
            /// <summary>总线号</summary>
            public UInt16 busNo;
            /// <summary>节点号</summary>
            public UInt16 nodeAddr;
            /// <summary>端口号</summary>
            public UInt16 portNo;
            /// <summary>IO方向(0-输入,1-输出)</summary>
            public Byte ioSrc;
            /// <summary>极性(0-默认,1-取反)</summary>
            public Byte polarity;
            /// <summary>状态</summary>
            public Byte state;
        }
        /// <summary>
        /// 设备信息数据
        /// </summary>
        public struct Device_Data
        {
            /// <summary>IO序号</summary>
            public UInt16 index;
            /// <summary>设备名称</summary>
            public String devName;
            /// <summary>总线号</summary>
            public UInt16 busNo;
            /// <summary>节点号</summary>
            public UInt16 nodeAddr;
            /// <summary>端口号</summary>
            public UInt16 portNo;
        }
        #endregion
        #region 数据结构私有初始化方法
        /// <summary>
        /// 轴信息初始化
        /// </summary>
        private void Set_Axis_Data()
        {
            for (int i = 0; i < num_axis; i++)
            {
                axis_data[i].nodeAddr = (UInt16)i;
                axis_data[i].axisName = Convert.ToString(Convert.ToChar('a' + i)) + Convert.ToString(Convert.ToChar('a' + i)) + Convert.ToString(Convert.ToChar('a' + i)) + Convert.ToString(Convert.ToChar('a' + i));
                axis_data[i].cmdPos = 0.10 * i;
                axis_data[i].fbkPos = 0.101 * i;
                axis_data[i].pel = 1;
                axis_data[i].mel = 1;
                axis_data[i].org = 1;
                axis_data[i].alm = 1;
                axis_data[i].atp = 1;
                axis_data[i].svon = 0;
            }
        }
        /// <summary>
        /// IO信息初始化
        /// </summary>
        private void Set_IO_Data()
        {
            for (int i = 0; i < num_io; i++)
            {
                io_data[i].index = (UInt16)i;
                io_data[i].ioName = Convert.ToString(Convert.ToChar('a' + i)) + Convert.ToString(Convert.ToChar('a' + i)) + Convert.ToString(Convert.ToChar('a' + i)) + Convert.ToString(Convert.ToChar('a' + i));
                io_data[i].busNo = (ushort)(i / 4);
                io_data[i].nodeAddr = (ushort)(i % 4);
                io_data[i].portNo = (ushort)((i + 1) / 2);
                io_data[i].ioSrc = (byte)(i % 2);
                io_data[i].polarity = 0;
                io_data[i].state = 0;
            }
        }
        /// <summary>
        /// 其他设备信息初始化
        /// </summary>
        private void Set_Device_Data()
        {
            for (int i = 0; i < num_device; i++)
            {
                device_data[i].index = (UInt16)i;
                device_data[i].devName = Convert.ToString(Convert.ToChar('a' + i)) + Convert.ToString(Convert.ToChar('a' + i)) + Convert.ToString(Convert.ToChar('a' + i)) + Convert.ToString(Convert.ToChar('a' + i));
                device_data[i].busNo = (ushort)(i / 4);
                device_data[i].nodeAddr = (ushort)(i % 4);
                device_data[i].portNo = (ushort)((i + 1) / 2);
            }
        }
        #endregion
        private void label_Lv_Inf_Click(object sender, EventArgs e)
        {
            try
            {
                form_lv_inf = new Form_Lv_Inf();
                form_lv_inf.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误:\n" + ex.Message);
            }
        }

    }
}