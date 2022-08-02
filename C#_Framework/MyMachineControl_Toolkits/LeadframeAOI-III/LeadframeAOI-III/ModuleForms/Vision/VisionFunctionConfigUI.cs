using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Utils;

namespace LeadframeAOI
{
    public partial class VisionFunctionConfig_UI : Form
    {
        public VisionFunctionConfig_UI()
        {
            InitializeComponent();
        }
        private void btnSaveConfig_Click(object sender, EventArgs e)
        {
            try
            {
                //模板参数

                //hvec_vec_model_param[0]:模板存放目录
                Program.visionFunction.model_path = tb_model_path.Text.Trim();//模板存放的目录

                //hvec_vec_model_param[1]:PCB黄金模板比对参数
                Program.visionFunction.pcb_sub_reg_num = Convert.ToDouble(tb_pcb_sub_reg_num.Text.Trim());//=0;//子区域个数，Int32, [0,9],default:0
                Program.visionFunction.pcb_sobel_scale = Convert.ToDouble(tb_pcb_sobel_scale.Text.Trim());//=0.2;//梯度图系数,Double,[0,2.0], 0.2
                Program.visionFunction.pcb_dark_thresh = Convert.ToDouble(tb_pcb_dark_thresh.Text.Trim());//=4.0;//暗缺陷阈值,Double, [0.0, 10.0],4.0
                Program.visionFunction.pcb_light_thresh = Convert.ToDouble(tb_pcb_light_thresh.Text.Trim());//=8;//亮缺陷阈值，Double，[0.0,10.0],4.0

                //hvec_vec_model_param[2]:IC黄金模板比对参数
                Program.visionFunction.ic_sub_reg_num = Convert.ToDouble(tb_ic_sub_reg_num.Text.Trim());//=0;
                Program.visionFunction.ic_sobel_scale = Convert.ToDouble(tb_ic_sobel_scale.Text.Trim());//=0.5;
                Program.visionFunction.ic_dark_thresh = Convert.ToDouble(tb_ic_dark_thresh.Text.Trim());//=7;
                Program.visionFunction.ic_light_thresh = Convert.ToDouble(tb_ic_light_thresh.Text.Trim());//=7;

                //检测参数

                //hvec_vec_inspect_param[0]:PCB外观检测参数
                Program.visionFunction.pcb_score_thresh = Convert.ToDouble(tb_pcb_score_thresh.Text.Trim());//=0.6;// 匹配分数阈值，Double，[0,0,1.0], 0.8
                Program.visionFunction.pcb_angle_start = Convert.ToDouble(tb_pcb_angle_start.Text.Trim());//=-0.1;//匹配搜索起始角度(弧度)，Double,[-0.5,0]，-0.1
                Program.visionFunction.pcb_angle_extent = Convert.ToDouble(tb_pcb_angle_extent.Text.Trim());//=0.2;//匹配搜索角度范围(弧度)，Double，[0,1],0.2
                Program.visionFunction.pcb_search_size = Convert.ToDouble(tb_pcb_search_size.Text.Trim());//=1001;//匹配搜索区域大小(pix)，Int32，[0,1500],500
                Program.visionFunction.pcb_closing_size = Convert.ToDouble(tb_pcb_closing_size.Text.Trim());//=3;//闭运算核大小(pix),Int32,[1,9],3
                Program.visionFunction.pcb_select_operation = tb_pcb_select_operation.Text.Trim();//="and";//缺陷区域选择逻辑关系,string,['and','or'], 'and'
                Program.visionFunction.pcb_width_thresh = Convert.ToDouble(tb_pcb_width_thresh.Text.Trim());//=50;//缺陷宽度阈值(pix),Double, [0,2000], 50
                Program.visionFunction.pcb_height_thresh = Convert.ToDouble(tb_pcb_height_thresh.Text.Trim());//=50;//缺陷高度阈值(pix),Double, [0,2000], 50
                Program.visionFunction.pcb_area_thresh = Convert.ToDouble(tb_pcb_area_thresh.Text.Trim());//=4000;//缺陷面积阈值(pix),Double, [0,4000000], 4000

                //hvec_vec_inspect_param[1]:IC外观检测参数
                Program.visionFunction.ic_score_thresh = Convert.ToDouble(tb_ic_score_thresh.Text.Trim());//=0.6;
                Program.visionFunction.ic_angle_start = Convert.ToDouble(tb_ic_angle_start.Text.Trim());//=-0.1;
                Program.visionFunction.ic_angle_extent = Convert.ToDouble(tb_ic_angle_extent.Text.Trim());//=0.2;
                Program.visionFunction.ic_search_size = Convert.ToDouble(tb_ic_search_size.Text.Trim());//=1001;
                Program.visionFunction.ic_closing_size = Convert.ToDouble(tb_ic_closing_size.Text.Trim());//=1;
                Program.visionFunction.ic_select_operation = tb_ic_select_operation.Text.Trim();//="and";
                Program.visionFunction.ic_width_thresh = Convert.ToDouble(tb_ic_width_thresh.Text.Trim());//=20;
                Program.visionFunction.ic_height_thresh = Convert.ToDouble(tb_ic_height_thresh.Text.Trim());//=20;
                Program.visionFunction.ic_area_thresh = Convert.ToDouble(tb_ic_area_thresh.Text.Trim());//=225;

                //hvec_vec_inspect_param[2]:IC偏移检测参数

                Program.visionFunction.pos_row_thresh = Convert.ToDouble(tb_pos_row_thresh.Text.Trim());//=50;//IC允许偏移的行坐标阈值(pix),Double, [0, 1000], 50
                Program.visionFunction.pos_col_thresh = Convert.ToDouble(tb_pos_col_thresh.Text.Trim());//=50;//IC允许偏移的列坐标阈值(pix),Double, [0, 1000], 50
                Program.visionFunction.pos_angle_thresh = Convert.ToDouble(tb_pos_angle_thresh.Text.Trim());//=0.1;//IC允许的偏移角度阈值(弧度),Double, [0, 1], 0.1

                //hvec_vec_inspect_param[3]:金线外观检测
                Program.visionFunction.line_ic_radius_low = Convert.ToDouble(tb_line_ic_radius_low.Text.Trim());//=8;// IC焊球半径下限，Double，[1,20],8
                Program.visionFunction.line_ic_radius_high = Convert.ToDouble(tb_line_ic_radius_high.Text.Trim());//=13;//IC焊球半径上限，Double，[1,30],13
                Program.visionFunction.line_pcb_radius_low = Convert.ToDouble(tb_line_pcb_radius_low.Text.Trim());//=9;//PCB焊球半径下限，Double，[1,20],9
                Program.visionFunction.line_pcb_radius_high = Convert.ToDouble(tb_line_pcb_radius_high.Text.Trim());//=16;//PCB焊球半径上限，Double，[1,30],16
                Program.visionFunction.line_num = Convert.ToDouble(tb_line_num.Text.Trim());//=69;//金线条数，Int32，[1,100],根据产品定,69
                Program.visionFunction.line_search_len1 = Convert.ToDouble(tb_line_search_len1.Text.Trim());//=20;//金线搜索范围(pix)，Int32，[5,50]
                Program.visionFunction.line_thresh = Convert.ToDouble(tb_line_thresh.Text.Trim());//=1;//金线提取二阶导阈值，Double，[0.4, 5.0], 1
                Program.visionFunction.line_width = Convert.ToDouble(tb_line_width.Text.Trim());//=6;//金线宽度(pix),Int32 [2,15]，6
                Program.visionFunction.line_min_seg_length = Convert.ToDouble(tb_line_min_seg_length.Text.Trim());//=7;//金线线段最小长度阈值(pix)，Int32，[1,20],7



                //hvec_vec_inspect_param[4]:崩边检测
                Program.visionFunction.chipping_inspect_size = Convert.ToDouble(tb_chipping_inspect_size.Text.Trim());//=30;//崩边检测范围(pix)，Int32，[1,50],30
                Program.visionFunction.chipping_low_thresh = Convert.ToDouble(tb_chipping_low_thresh.Text.Trim());//=50;//崩边灰度下限值，Int32，[1,100]，50
                Program.visionFunction.chipping_high_thresh = Convert.ToDouble(tb_chipping_high_thresh.Text.Trim());//=255;//崩边灰度上限值，Int32， [100,255],255
                Program.visionFunction.chipping_opening_size = Convert.ToDouble(tb_chipping_opening_size.Text.Trim());//=5;//开运算核大小,Int32,[1,9],5
                Program.visionFunction.chipping_area_thresh = Convert.ToDouble(tb_chipping_area_thresh.Text.Trim());//=100;//崩边区域最小面积阈值，Int32，[10,500]
                Program.visionFunction.chipping_len1_thresh = Convert.ToDouble(tb_chipping_len1_thresh.Text.Trim());//=5;//崩边的长阈值(pix)，Int32，[0,100],5
                Program.visionFunction.chipping_len2_thresh = Convert.ToDouble(tb_chipping_len2_thresh.Text.Trim());//=5;//崩边的宽阈值(pix)，Int32，[0,100],5
                Program.visionFunction.chipping_select_operation = tb_chipping_select_operation.Text.Trim();//="and";//缺陷区域选择逻辑关系,string,['and','or'], 'and'

                //hvec_vec_inspect_param[5]:划痕检测
                Program.visionFunction.scratch_is_gauss = Convert.ToInt32(tb_scratch_is_gauss.Text.Trim());//=1;//是否采用高斯法检测，Int32，[0,1],1
                Program.visionFunction.scratch_line_sigma = Convert.ToDouble(tb_scratch_line_sigma.Text.Trim());//=1;//划痕检测图像滤波系数，[0.4, 9],1
                Program.visionFunction.scratch_line_low = Convert.ToDouble(tb_scratch_line_low.Text.Trim());//=1;//划痕检测二阶导低阈值，Double，[0,5],1
                Program.visionFunction.scratch_line_high = Convert.ToDouble(tb_scratch_line_high.Text.Trim());//=2;//划痕检测二阶导低阈值，Double，[0,10],2
                Program.visionFunction.scratch_light_dark = tb_scratch_light_dark.Text.Trim();//="light";//划痕是亮还是暗，string， ['light','dark'],'light'
                Program.visionFunction.scratch_length_thresh = Convert.ToInt32(tb_scratch_length_thresh.Text.Trim());//=50;//,划痕的长度阈值(pix)，Int32，[1,200], 50
                Program.visionFunction.scratch_mask_size = Convert.ToInt32(tb_scratch_mask_size.Text.Trim());//=31;//划痕块检测的背景区大小(pix)，Int32，[3, 101],基数，31
                Program.visionFunction.scratch_sigma_thresh = Convert.ToDouble(tb_scratch_sigma_thresh.Text.Trim());//=3;//划痕块检测阈值大小，Double，[0,9],3.0
                Program.visionFunction.scratch_gray_thresh = Convert.ToInt32(tb_scratch_gray_thresh.Text.Trim());//=20;//划痕块检测灰度阈值，Int32，[0,255],20
                Program.visionFunction.scratch_area_thresh = Convert.ToInt32(tb_scratch_area_thresh.Text.Trim());//=20;//划痕块的最小面积阈值(pix)，Int32, [1,200],20
                Program.visionFunction.scratch_len1_thresh = Convert.ToInt32(tb_scratch_len1_thresh.Text.Trim());//=2;//划痕块长阈值(pix)，Int32，[0,100],2
                Program.visionFunction.scratch_len2_thresh = Convert.ToDouble(tb_scratch_len2_thresh.Text.Trim());//=2;//划痕块宽阈值(pix)，Int32，[0,100],2

                //hvec_vec_inspect_param[6]:模板比对外观检测
                Program.visionFunction.match_thresh_num = Convert.ToInt32(tb_match_num.Text.Trim());
                Program.visionFunction.match_angle_start = Convert.ToDouble(tb_match_angle_start.Text.Trim());//=-0.1;//匹配搜索起始角度(弧度)，Double,[-0.5,0]，-0.1
                Program.visionFunction.match_angle_extent = Convert.ToDouble(tb_match_angle_extent.Text.Trim());//=0.2;//匹配搜索角度范围(弧度)，Double，[0,1],0.2
                if (Program.visionFunction.match_thresh_num > 0)
                    Program.visionFunction.match_thresh[0] = Convert.ToDouble(tb_match_thresh1.Text.Trim());
                if (Program.visionFunction.match_thresh_num > 1)
                    Program.visionFunction.match_thresh[1] = Convert.ToDouble(tb_match_thresh2.Text.Trim());
                if (Program.visionFunction.match_thresh_num > 2)
                    Program.visionFunction.match_thresh[2] = Convert.ToDouble(tb_match_thresh3.Text.Trim());
                if (Program.visionFunction.match_thresh_num > 3)
                    Program.visionFunction.match_thresh[3] = Convert.ToDouble(tb_match_thresh4.Text.Trim());
                if (Program.visionFunction.match_thresh_num > 4)
                    Program.visionFunction.match_thresh[4] = Convert.ToDouble(tb_match_thresh5.Text.Trim());
                if (Program.visionFunction.match_thresh_num > 5)
                    Program.visionFunction.match_thresh[5] = Convert.ToDouble(tb_match_thresh6.Text.Trim());
                if (Program.visionFunction.match_thresh_num > 6)
                    Program.visionFunction.match_thresh[6] = Convert.ToDouble(tb_match_thresh7.Text.Trim());
                if (Program.visionFunction.match_thresh_num > 7)
                    Program.visionFunction.match_thresh[7] = Convert.ToDouble(tb_match_thresh8.Text.Trim());
                if (Program.visionFunction.match_thresh_num > 8)
                    Program.visionFunction.match_thresh[8] = Convert.ToDouble(tb_match_thresh9.Text.Trim());
                //if (Program.visionFunction.match_thresh_num > 9)
                //    Program.visionFunction.match_thresh[9] = Convert.ToDouble(tb_match_thresh10.Text.Trim());
                Program.visionFunction.SetValue();
                Program.visionFunction.Save();
            }
            catch(Exception ex)
            {
                MSG.Error("保存模板参数失败！\n"+ex.Message);
            }
        }

        private void VisionFunctionConfig_UI_Load(object sender, EventArgs e)
        {
            try
            {
                Program.visionFunction.Read();
                //模板参数
                //hvec_vec_model_param[0]:模板存放目录
                tb_model_path.Text = Program.visionFunction.model_path;//模板存放的目录
                //hvec_vec_model_param[1]:PCB黄金模板比对参数
                tb_pcb_sub_reg_num.Text = Convert.ToString(Program.visionFunction.pcb_sub_reg_num);//0;//子区域个数，Int32, [0,9],default:0
                tb_pcb_sobel_scale.Text = Convert.ToString(Program.visionFunction.pcb_sobel_scale);//0.2;//梯度图系数,Double,[0,2.0], 0.2
                tb_pcb_dark_thresh.Text = Convert.ToString(Program.visionFunction.pcb_dark_thresh);//4.0;//暗缺陷阈值,Double, [0.0, 10.0],4.0
                tb_pcb_light_thresh.Text = Convert.ToString(Program.visionFunction.pcb_light_thresh);//8;//亮缺陷阈值，Double，[0.0,10.0],4.0

                //hvec_vec_model_param[2]:IC黄金模板比对参数
                tb_ic_sub_reg_num.Text = Convert.ToString(Program.visionFunction.ic_sub_reg_num);//0;
                tb_ic_sobel_scale.Text = Convert.ToString(Program.visionFunction.ic_sobel_scale);//0.5;
                tb_ic_dark_thresh.Text = Convert.ToString(Program.visionFunction.ic_dark_thresh);//7;
                tb_ic_light_thresh.Text = Convert.ToString(Program.visionFunction.ic_light_thresh);//7;

                //检测参数

                //hvec_vec_inspect_param[0]:PCB外观检测参数
                tb_pcb_score_thresh.Text = Convert.ToString(Program.visionFunction.pcb_score_thresh);//0.6;// 匹配分数阈值，Double，[0,0,1.0], 0.8
                tb_pcb_angle_start.Text = Convert.ToString(Program.visionFunction.pcb_angle_start);//-0.1;//匹配搜索起始角度(弧度)，Double,[-0.5,0]，-0.1
                tb_pcb_angle_extent.Text = Convert.ToString(Program.visionFunction.pcb_angle_extent);//0.2;//匹配搜索角度范围(弧度)，Double，[0,1],0.2
                tb_pcb_search_size.Text = Convert.ToString(Program.visionFunction.pcb_search_size);//1001;//匹配搜索区域大小(pix)，Int32，[0,1500],500
                tb_pcb_closing_size.Text = Convert.ToString(Program.visionFunction.pcb_closing_size);//3;//闭运算核大小(pix),Int32,[1,9],3
                tb_pcb_select_operation.Text = Convert.ToString(Program.visionFunction.pcb_select_operation);//"and";//缺陷区域选择逻辑关系,string,['and','or'], 'and'
                tb_pcb_width_thresh.Text = Convert.ToString(Program.visionFunction.pcb_width_thresh);//50;//缺陷宽度阈值(pix),Double, [0,2000], 50
                tb_pcb_height_thresh.Text = Convert.ToString(Program.visionFunction.pcb_height_thresh);//50;//缺陷高度阈值(pix),Double, [0,2000], 50
                tb_pcb_area_thresh.Text = Convert.ToString(Program.visionFunction.pcb_area_thresh);//4000;//缺陷面积阈值(pix),Double, [0,4000000], 4000

                //hvec_vec_inspect_param[1]:IC外观检测参数
                tb_ic_score_thresh.Text = Convert.ToString(Program.visionFunction.ic_score_thresh);//0.6;
                tb_ic_angle_start.Text = Convert.ToString(Program.visionFunction.ic_angle_start);//-0.1;
                tb_ic_angle_extent.Text = Convert.ToString(Program.visionFunction.ic_angle_extent);//0.2;
                tb_ic_search_size.Text = Convert.ToString(Program.visionFunction.ic_search_size);//1001;
                tb_ic_closing_size.Text = Convert.ToString(Program.visionFunction.ic_closing_size);//1;
                tb_ic_select_operation.Text = Convert.ToString(Program.visionFunction.ic_select_operation);//"and";
                tb_ic_width_thresh.Text = Convert.ToString(Program.visionFunction.ic_width_thresh);//20;
                tb_ic_height_thresh.Text = Convert.ToString(Program.visionFunction.ic_height_thresh);//20;
                tb_ic_area_thresh.Text = Convert.ToString(Program.visionFunction.ic_area_thresh);//225;

                //hvec_vec_inspect_param[2]:IC偏移检测参数

                tb_pos_row_thresh.Text = Convert.ToString(Program.visionFunction.pos_row_thresh);//50;//IC允许偏移的行坐标阈值(pix),Double, [0, 1000], 50
                tb_pos_col_thresh.Text = Convert.ToString(Program.visionFunction.pos_col_thresh);//50;//IC允许偏移的列坐标阈值(pix),Double, [0, 1000], 50
                tb_pos_angle_thresh.Text = Convert.ToString(Program.visionFunction.pos_angle_thresh);//0.1;//IC允许的偏移角度阈值(弧度),Double, [0, 1], 0.1

                //hvec_vec_inspect_param[3]:金线外观检测
                tb_line_ic_radius_low.Text = Convert.ToString(Program.visionFunction.line_ic_radius_low);//8;// IC焊球半径下限，Double，[1,20],8
                tb_line_ic_radius_high.Text = Convert.ToString(Program.visionFunction.line_ic_radius_high);//13;//IC焊球半径上限，Double，[1,30],13
                tb_line_pcb_radius_low.Text = Convert.ToString(Program.visionFunction.line_pcb_radius_low);//9;//PCB焊球半径下限，Double，[1,20],9
                tb_line_pcb_radius_high.Text = Convert.ToString(Program.visionFunction.line_pcb_radius_high);//16;//PCB焊球半径上限，Double，[1,30],16
                tb_line_num.Text = Convert.ToString(Program.visionFunction.line_num);//69;//金线条数，Int32，[1,100],根据产品定,69
                tb_line_search_len1.Text = Convert.ToString(Program.visionFunction.line_search_len1);//20;//金线搜索范围(pix)，Int32，[5,50]
                tb_line_thresh.Text = Convert.ToString(Program.visionFunction.line_thresh);//1;//金线提取二阶导阈值，Double，[0.4, 5.0], 1
                tb_line_width.Text = Convert.ToString(Program.visionFunction.line_width);//6;//金线宽度(pix),Int32 [2,15]，6
                tb_line_min_seg_length.Text = Convert.ToString(Program.visionFunction.line_min_seg_length);//7;//金线线段最小长度阈值(pix)，Int32，[1,20],7



                //hvec_vec_inspect_param[4]:崩边检测
                tb_chipping_inspect_size.Text = Convert.ToString(Program.visionFunction.chipping_inspect_size);//30;//崩边检测范围(pix)，Int32，[1,50],30
                tb_chipping_low_thresh.Text = Convert.ToString(Program.visionFunction.chipping_low_thresh);//50;//崩边灰度下限值，Int32，[1,100]，50
                tb_chipping_high_thresh.Text = Convert.ToString(Program.visionFunction.chipping_high_thresh);//255;//崩边灰度上限值，Int32， [100,255],255
                tb_chipping_opening_size.Text = Convert.ToString(Program.visionFunction.chipping_opening_size);//5;//开运算核大小,Int32,[1,9],5
                tb_chipping_area_thresh.Text = Convert.ToString(Program.visionFunction.chipping_area_thresh);//100;//崩边区域最小面积阈值，Int32，[10,500]
                tb_chipping_len1_thresh.Text = Convert.ToString(Program.visionFunction.chipping_len1_thresh);//5;//崩边的长阈值(pix)，Int32，[0,100],5
                tb_chipping_len2_thresh.Text = Convert.ToString(Program.visionFunction.chipping_len2_thresh);//5;//崩边的宽阈值(pix)，Int32，[0,100],5
                tb_chipping_select_operation.Text = Convert.ToString(Program.visionFunction.chipping_select_operation);//"and";//缺陷区域选择逻辑关系,string,['and','or'], 'and'

                //hvec_vec_inspect_param[5]:划痕检测
                tb_scratch_is_gauss.Text = Convert.ToString(Program.visionFunction.scratch_is_gauss);//1;//是否采用高斯法检测，Int32，[0,1],1
                tb_scratch_line_sigma.Text = Convert.ToString(Program.visionFunction.scratch_line_sigma);//1;//划痕检测图像滤波系数，[0.4, 9],1
                tb_scratch_line_low.Text = Convert.ToString(Program.visionFunction.scratch_line_low);//1;//划痕检测二阶导低阈值，Double，[0,5],1
                tb_scratch_line_high.Text = Convert.ToString(Program.visionFunction.scratch_line_high);//2;//划痕检测二阶导低阈值，Double，[0,10],2
                tb_scratch_light_dark.Text = Convert.ToString(Program.visionFunction.scratch_light_dark);//"light";//划痕是亮还是暗，string， ['light','dark'],'light'
                tb_scratch_length_thresh.Text = Convert.ToString(Program.visionFunction.scratch_length_thresh);//50;//,划痕的长度阈值(pix)，Int32，[1,200], 50
                tb_scratch_mask_size.Text = Convert.ToString(Program.visionFunction.scratch_mask_size);//31;//划痕块检测的背景区大小(pix)，Int32，[3, 101],基数，31
                tb_scratch_sigma_thresh.Text = Convert.ToString(Program.visionFunction.scratch_sigma_thresh);//3;//划痕块检测阈值大小，Double，[0,9],3.0
                tb_scratch_gray_thresh.Text = Convert.ToString(Program.visionFunction.scratch_gray_thresh);//20;//划痕块检测灰度阈值，Int32，[0,255],20
                tb_scratch_area_thresh.Text = Convert.ToString(Program.visionFunction.scratch_area_thresh);//20;//划痕块的最小面积阈值(pix)，Int32, [1,200],20
                tb_scratch_len1_thresh.Text = Convert.ToString(Program.visionFunction.scratch_len1_thresh);//2;//划痕块长阈值(pix)，Int32，[0,100],2
                tb_scratch_len2_thresh.Text = Convert.ToString(Program.visionFunction.scratch_len2_thresh);//2;//划痕块宽阈值(pix)，Int32，[0,100],2

                //hvec_vec_inspect_param[6]:模板比对外观检测
                tb_match_num.Text = Convert.ToString(Program.visionFunction.match_thresh_num);
                tb_match_angle_start.Text = Convert.ToString(Program.visionFunction.match_angle_start);//-0.1;//匹配搜索起始角度(弧度)，Double,[-0.5,0]，-0.1
                tb_match_angle_extent.Text = Convert.ToString(Program.visionFunction.match_angle_extent);//0.2;//匹配搜索角度范围(弧度)，Double，[0,1],0.2
                if (Program.visionFunction.match_thresh_num > 0)
                {
                    tb_match_thresh1.Text = Convert.ToString(Program.visionFunction.match_thresh[0].D);//new HTuple(0.45, 0.4, 0.4, 0.4);//匹配分数，有几个检测模板就有几个匹配分数阈值，这边有4个模板，所以设置了4个分数阈值，Double，[0,1],0.5
                    lb_match_thresh1.Enabled = true;
                    tb_match_thresh1.Enabled = true;
                }
                if (Program.visionFunction.match_thresh_num > 1)
                {
                    tb_match_thresh2.Text = Convert.ToString(Program.visionFunction.match_thresh[1].D);//new HTuple(0.45, 0.4, 0.4, 0.4);//匹配分数，有几个检测模板就有几个匹配分数阈值，这边有4个模板，所以设置了4个分数阈值，Double，[0,1],0.5
                    lb_match_thresh2.Enabled = true;
                    tb_match_thresh2.Enabled = true;
                }
                if (Program.visionFunction.match_thresh_num > 2)
                {
                    tb_match_thresh3.Text = Convert.ToString(Program.visionFunction.match_thresh[2].D);//new HTuple(0.45, 0.4, 0.4, 0.4);//匹配分数，有几个检测模板就有几个匹配分数阈值，这边有4个模板，所以设置了4个分数阈值，Double，[0,1],0.5
                    lb_match_thresh3.Enabled = true;
                    tb_match_thresh3.Enabled = true;
                }
                if (Program.visionFunction.match_thresh_num > 3)
                {
                    tb_match_thresh4.Text = Convert.ToString(Program.visionFunction.match_thresh[3].D);//new HTuple(0.45, 0.4, 0.4, 0.4);//匹配分数，有几个检测模板就有几个匹配分数阈值，这边有4个模板，所以设置了4个分数阈值，Double，[0,1],0.5
                    lb_match_thresh4.Enabled = true;
                    tb_match_thresh4.Enabled = true;
                }
                if (Program.visionFunction.match_thresh_num > 4)
                {
                    tb_match_thresh5.Text = Convert.ToString(Program.visionFunction.match_thresh[4].D);//new HTuple(0.45, 0.4, 0.4, 0.4);//匹配分数，有几个检测模板就有几个匹配分数阈值，这边有4个模板，所以设置了4个分数阈值，Double，[0,1],0.5
                    lb_match_thresh5.Enabled = true;
                    tb_match_thresh5.Enabled = true;
                }
                if (Program.visionFunction.match_thresh_num > 5)
                {
                    tb_match_thresh6.Text = Convert.ToString(Program.visionFunction.match_thresh[5].D);//new HTuple(0.45, 0.4, 0.4, 0.4);//匹配分数，有几个检测模板就有几个匹配分数阈值，这边有4个模板，所以设置了4个分数阈值，Double，[0,1],0.5
                    lb_match_thresh6.Enabled = true;
                    tb_match_thresh6.Enabled = true;
                }
                if (Program.visionFunction.match_thresh_num > 6)
                {
                    tb_match_thresh7.Text = Convert.ToString(Program.visionFunction.match_thresh[6].D);//new HTuple(0.45, 0.4, 0.4, 0.4);//匹配分数，有几个检测模板就有几个匹配分数阈值，这边有4个模板，所以设置了4个分数阈值，Double，[0,1],0.5
                    lb_match_thresh7.Enabled = true;
                    tb_match_thresh7.Enabled = true;
                }
                if (Program.visionFunction.match_thresh_num > 7)
                {
                    tb_match_thresh8.Text = Convert.ToString(Program.visionFunction.match_thresh[7].D);//new HTuple(0.45, 0.4, 0.4, 0.4);//匹配分数，有几个检测模板就有几个匹配分数阈值，这边有4个模板，所以设置了4个分数阈值，Double，[0,1],0.5
                    lb_match_thresh8.Enabled = true;
                    tb_match_thresh8.Enabled = true;
                }
                if (Program.visionFunction.match_thresh_num > 8)
                {
                    tb_match_thresh9.Text = Convert.ToString(Program.visionFunction.match_thresh[8].D);//new HTuple(0.45, 0.4, 0.4, 0.4);//匹配分数，有几个检测模板就有几个匹配分数阈值，这边有4个模板，所以设置了4个分数阈值，Double，[0,1],0.5
                    lb_match_thresh9.Enabled = true;
                    tb_match_thresh9.Enabled = true;
                }
                //if (Program.visionFunction.match_thresh_num > 9)
                //{
                //    tb_match_thresh10.Text = Convert.ToString(Program.visionFunction.match_thresh[9].D);//new HTuple(0.45, 0.4, 0.4, 0.4);//匹配分数，有几个检测模板就有几个匹配分数阈值，这边有4个模板，所以设置了4个分数阈值，Double，[0,1],0.5
                //    lb_match_thresh10.Enabled = true;
                //    tb_match_thresh10.Enabled = true;
                //}
            }
            catch (Exception ex)
            {
                MSG.Error("加载模板参数失败！\n" + ex.Message);
            }
        }

        private void btnModelFile_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog();
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    tb_model_path.Text = ofd.FileName;
                }
            }
            catch(Exception ex)
            {
                MSG.Error("更改模板目录失败！\n"+ex.Message);
            }
        }
    }
}
