﻿//2018/2/6 MATCH STRUCT
//2/8 针对信利PCB_BALL有轮廓 单独更新
//2/9 新产品不检 chipping
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;
using HalconDotNet;
using System.IO;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using CNTK;

namespace LeadframeAOI
{
    class VisionFunction : Base
    {
        public VisionFunction(String para_file, String para_table) : base(para_file, para_table) { }
        #region 算法开放接口参数
        //模板参数

        //hvec_vec_model_param[0]:模板存放目录
        public String model_path = Application.StartupPath + "\\model";//模板存放的目录

        //hvec_vec_model_param[1]:PCB黄金模板比对参数
        public Double pcb_sub_reg_num = 0;//子区域个数，int, [0,9],default:0
        public Double pcb_sobel_scale = 0.5;//梯度图系数,Double,[0,2.0], 0.2
        public Double pcb_dark_thresh = 10;//暗缺陷阈值,Double, [0.0, 10.0],4.0
        public Double pcb_light_thresh = 15;//亮缺陷阈值，Double，[0.0,10.0],4.0

        //hvec_vec_model_param[2]:IC黄金模板比对参数
        public Double ic_sub_reg_num = 0;
        public Double ic_sobel_scale = 0.5;
        public Double ic_dark_thresh = 7;
        public Double ic_light_thresh = 20;

        //检测参数

        //hvec_vec_inspect_param[0]:PCB外观检测参数
        public Double pcb_score_thresh = 0.6;// 匹配分数阈值，Double，[0,0,1.0], 0.8
        public Double pcb_angle_start = -0.1;//匹配搜索起始角度(弧度)，Double,[-0.5,0]，-0.1
        public Double pcb_angle_extent = 0.2;//匹配搜索角度范围(弧度)，Double，[0,1],0.2
        public Double pcb_search_size = 1001;//匹配搜索区域大小(pix)，int，[0,1500],500
        public Double pcb_closing_size = 3;//闭运算核大小(pix),int,[1,9],3
        public String pcb_select_operation = "and";// 缺陷区域选择逻辑关系,string,['and','or'], 'and'
        public Double pcb_width_thresh = 50;//缺陷宽度阈值(pix),Double, [0,2000], 50
        public Double pcb_height_thresh = 50;//缺陷高度阈值(pix),Double, [0,2000], 50
        public Double pcb_area_thresh = 4000;//缺陷面积阈值(pix),Double, [0,4000000], 4000

        //hvec_vec_inspect_param[1]:IC外观检测参数
        public Double ic_score_thresh = 0.6;
        public Double ic_angle_start = -0.1;
        public Double ic_angle_extent = 0.2;
        public Double ic_search_size = 1001;
        public Double ic_closing_size = 1;
        public String ic_select_operation = "and";
        public Double ic_width_thresh = 20;
        public Double ic_height_thresh = 20;
        public Double ic_area_thresh = 225;

        //hvec_vec_inspect_param[2]:IC偏移检测参数

        public Double pos_row_thresh = 50;//IC允许偏移的行坐标阈值(pix),Double, [0, 1000], 50
        public Double pos_col_thresh = 50;//IC允许偏移的列坐标阈值(pix),Double, [0, 1000], 50
        public Double pos_angle_thresh = 0.1;//IC允许的偏移角度阈值(弧度),Double, [0, 1], 0.1

        //hvec_vec_inspect_param[3]:金线外观检测
        public Double line_ic_radius_low = 3;// IC焊球半径下限，Double，[1,20],8
        public Double line_ic_radius_high = 7;//IC焊球半径上限，Double，[1,30],13
        public Double line_pcb_radius_low = 5;//PCB焊球半径下限，Double，[1,20],9
        public Double line_pcb_radius_high = 9;//PCB焊球半径上限，Double，[1,30],16
        public Double line_num = 69;//金线条数，int，[1,100],根据产品定,69
        public Double line_search_len1 = 10;//金线搜索范围(pix)，int，[5,50]
        public Double line_thresh = 1;//金线提取二阶导阈值，Double，[0.4, 5.0], 1
        public Double line_width = 3;//金线宽度(pix),int [2,15]，6
        public Double line_min_seg_length = 3;//金线线段最小长度阈值(pix)，int，[1,20],7
        public Double line_clip_length = 6;
        public Double line_angle_extent = 0.55;
        public Double line_max_gap = 30;
        public Double lead_wire_inspect_size = 11;
        public Double lead_wire_distance_thresh = 3;
        public Double lead_wire_angle_thresh = 0.4;
        public Double lead_wire_contrast = 120;
        public Double lead_wire_length_thresh = 3;
        public Double line_pcb_ball_gray_thresh = 80;
        public Int32  is_cntk = 0;
        public Double line_pcb_clsoing_rasius = 2;
        public Double line_pcb_fill_max = 20;


        //hvec_vec_inspect_param[4]:崩边检测
        public Double chipping_inspect_size = 40;//崩边检测范围(pix)，int，[1,50],30
        public Double chipping_low_thresh = 5;//崩边灰度下限值，int，[1,100]，50
        public Double chipping_high_thresh = 5;//崩边灰度上限值，int， [100,255],255
        public Double chipping_opening_size = 3;//开运算核大小,int,[1,9],5
        public Double chipping_area_thresh = 10;//崩边区域最小面积阈值，int，[10,500]
        public Double chipping_len1_thresh = 2;//崩边的长阈值(pix)，int，[0,100],5
        public Double chipping_len2_thresh = 2;//崩边的宽阈值(pix)，int，[0,100],5
        public String chipping_select_operation = "and";//缺陷区域选择逻辑关系,string,['and','or'], 'and'
        public Double chipping_search_size = 60;
        public Double chipping_score_thresh = 0.78;
        public Double chipping_measure_length1 = 10;
        public Double chipping_measure_length2 = 5;
        public Double chipping_measure_threshold = 40;

        //hvec_vec_inspect_param[5]:划痕检测
        public Int32 scratch_is_gauss = 1;//是否采用高斯法检测，int，[0,1],1
        public Double scratch_line_sigma = 1;//划痕检测图像滤波系数，[0.4, 9],1
        public Double scratch_line_low = 1;//划痕检测二阶导低阈值，Double，[0,5],1
        public Double scratch_line_high = 2;//划痕检测二阶导低阈值，Double，[0,10],2
        public String scratch_light_dark = "light";//划痕是亮还是暗，string， ['light','dark'],'light'
        public Int32 scratch_length_thresh = 30;//,划痕的长度阈值(pix)，int，[1,200], 50
        public Int32 scratch_mask_size = 51;//划痕块检测的背景区大小(pix)，int，[3, 101],基数，31
        public Double scratch_sigma_thresh = 2;//划痕块检测阈值大小，Double，[0,9],3.0
        public Int32 scratch_gray_thresh = 20;//划痕块检测灰度阈值，int，[0,255],20
        public Int32 scratch_area_thresh = 20;//划痕块的最小面积阈值(pix)，int, [1,200],20
        public Int32 scratch_len1_thresh = 2;//划痕块长阈值(pix)，int，[0,100],2
        public Double scratch_len2_thresh = 2;//划痕块宽阈值(pix)，int，[0,100],2

        //hvec_vec_inspect_param[6]:模板比对外观检测

        public Double match_angle_start = -0.1;//匹配搜索起始角度(弧度)，Double,[-0.5,0]，-0.1
        public Double match_angle_extent = 0.2;//匹配搜索角度范围(弧度)，Double，[0,1],0.2
        public Int32 match_thresh_num = 8;//匹配模板数
        public Double[] match_thresh = new Double[] { 0.9, 0.7, 0.9, 0.75, 0.9, 0.75, 0.9, 0.7 };//匹配分数，有几个检测模板就有几个匹配分数阈值，这边有4个模板，所以设置了4个分数阈值，Double，[0,1],0.5


        #endregion
        #region 私有接口控制参数
        //模板参数
        //hvec_vec_model_param[0]:模板存放目录
        private HTuple hv_model_path;//模板存放的目录

        //hvec_vec_model_param[1]:PCB黄金模板比对参数
        private HTuple hv_pcb_sub_reg_num = 0;//子区域个数，int, [0,9],default:0
        private HTuple hv_pcb_sobel_scale = 0.5;//梯度图系数,Double,[0,2.0], 0.2
        private HTuple hv_pcb_dark_thresh = 4.0;//暗缺陷阈值,Double, [0.0, 10.0],4.0
        private HTuple hv_pcb_light_thresh = 3;//亮缺陷阈值，Double，[0.0,10.0],4.0

        //hvec_vec_model_param[2]:IC黄金模板比对参数
        private HTuple hv_ic_sub_reg_num = 0;
        private HTuple hv_ic_sobel_scale = 0.5;
        private HTuple hv_ic_dark_thresh = 5;
        private HTuple hv_ic_light_thresh = 3;

        //检测参数

        //hvec_vec_inspect_param[0]:PCB外观检测参数
        private HTuple hv_pcb_score_thresh = 0.6;// 匹配分数阈值，Double，[0,0,1.0], 0.8
        private HTuple hv_pcb_angle_start = -0.1;//匹配搜索起始角度(弧度)，Double,[-0.5,0]，-0.1
        private HTuple hv_pcb_angle_extent = 0.2;//匹配搜索角度范围(弧度)，Double，[0,1],0.2
        private HTuple hv_pcb_search_size = 1001;//匹配搜索区域大小(pix)，int，[0,1500],500
        private HTuple hv_pcb_closing_size = 3;//闭运算核大小(pix),int,[1,9],3
        private HTuple hv_pcb_select_operation = "and";//缺陷区域选择逻辑关系,string,['and','or'], 'and'
        private HTuple hv_pcb_width_thresh = 50;//缺陷宽度阈值(pix),Double, [0,2000], 50
        private HTuple hv_pcb_height_thresh = 50;//缺陷高度阈值(pix),Double, [0,2000], 50
        private HTuple hv_pcb_area_thresh = 4000;//缺陷面积阈值(pix),Double, [0,4000000], 4000

        //hvec_vec_inspect_param[1]:IC外观检测参数
        private HTuple hv_ic_score_thresh = 0.6;
        private HTuple hv_ic_angle_start = -0.1;
        private HTuple hv_ic_angle_extent = 0.2;
        private HTuple hv_ic_search_size = 1001;
        private HTuple hv_ic_closing_size = 1;
        private HTuple hv_ic_select_operation = "and";
        private HTuple hv_ic_width_thresh = 20;
        private HTuple hv_ic_height_thresh = 20;
        private HTuple hv_ic_area_thresh = 225;

        //hvec_vec_inspect_param[2]:IC偏移检测参数

        private HTuple hv_pos_row_thresh = 50;//IC允许偏移的行坐标阈值(pix),Double, [0, 1000], 50
        private HTuple hv_pos_col_thresh = 50;//IC允许偏移的列坐标阈值(pix),Double, [0, 1000], 50
        private HTuple hv_pos_angle_thresh = 0.1;//IC允许的偏移角度阈值(弧度),Double, [0, 1], 0.1

        //hvec_vec_inspect_param[3]:金线外观检测
        private HTuple hv_line_ic_radius_low = 8;// IC焊球半径下限，Double，[1,20],8
        private HTuple hv_line_ic_radius_high = 13;//IC焊球半径上限，Double，[1,30],13
        private HTuple hv_line_pcb_radius_low = 9;//PCB焊球半径下限，Double，[1,20],9
        private HTuple hv_line_pcb_radius_high = 16;//PCB焊球半径上限，Double，[1,30],16
        private HTuple hv_line_num = 69;//金线条数，int，[1,100],根据产品定,69
        private HTuple hv_line_search_len1 = 20;//金线搜索范围(pix)，int，[5,50]
        private HTuple hv_line_thresh = 1;//金线提取二阶导阈值，Double，[0.4, 5.0], 1
        private HTuple hv_line_width = 6;//金线宽度(pix),int [2,15]，6
        private HTuple hv_line_min_seg_length = 7;//金线线段最小长度阈值(pix)，int，[1,20],7
        private HTuple hv_line_clip_length = 6;
        private HTuple hv_line_angle_extent = 0.55;
        private HTuple hv_line_max_gap = 30;
        private HTuple hv_lead_wire_inspect_size = 11;
        private HTuple hv_lead_wire_distance_thresh = 3;
        private HTuple hv_lead_wire_angle_thresh = 0.4;
        private HTuple hv_lead_wire_contrast = 120;
        private HTuple hv_lead_wire_length_thresh = 3;
        private HTuple hv_line_pcb_ball_gray_thresh = 80;
        private HTuple hv_is_cntk = 0;
        private HTuple hv_line_pcb_clsoing_rasius = 2;
        private HTuple hv_line_pcb_fill_max = 20;


        //hvec_vec_inspect_param[4]:崩边检测
        private HTuple hv_chipping_inspect_size = 30;//崩边检测范围(pix)，int，[1,50],30
        private HTuple hv_chipping_low_thresh = 50;//崩边灰度下限值，int，[1,100]，50
        private HTuple hv_chipping_high_thresh = 255;//崩边灰度上限值，int， [100,255],255
        private HTuple hv_chipping_opening_size = 5;//开运算核大小,int,[1,9],5
        private HTuple hv_chipping_area_thresh = 100;//崩边区域最小面积阈值，int，[10,500]
        private HTuple hv_chipping_len1_thresh = 5;//崩边的长阈值(pix)，int，[0,100],5
        private HTuple hv_chipping_len2_thresh = 5;//崩边的宽阈值(pix)，int，[0,100],5
        private HTuple hv_chipping_select_operation = "and";//缺陷区域选择逻辑关系,string,['and','or'], 'and'
        private HTuple hv_chipping_search_size = 60;
        private HTuple hv_chipping_score_thresh = 0.78;
        private HTuple hv_chipping_measure_length1 = 10;
        private HTuple hv_chipping_measure_length2 = 5;
        private HTuple hv_chipping_measure_threshold = 40;
       

        //hvec_vec_inspect_param[5]:划痕检测
        private HTuple hv_scratch_is_gauss = 1;//是否采用高斯法检测，int，[0,1],1
        private HTuple hv_scratch_line_sigma = 1;//划痕检测图像滤波系数，[0.4, 9],1
        private HTuple hv_scratch_line_low = 1;//划痕检测二阶导低阈值，Double，[0,5],1
        private HTuple hv_scratch_line_high = 2;//划痕检测二阶导低阈值，Double，[0,10],2
        private HTuple hv_scratch_light_dark = "light";//划痕是亮还是暗，string， ['light','dark'],'light'
        private HTuple hv_scratch_length_thresh = 50;//,划痕的长度阈值(pix)，int，[1,200], 50
        private HTuple hv_scratch_mask_size = 31;//划痕块检测的背景区大小(pix)，int，[3, 101],基数，31
        private HTuple hv_scratch_sigma_thresh = 3;//划痕块检测阈值大小，Double，[0,9],3.0
        private HTuple hv_scratch_gray_thresh = 20;//划痕块检测灰度阈值，int，[0,255],20
        private HTuple hv_scratch_area_thresh = 20;//划痕块的最小面积阈值(pix)，int, [1,200],20
        private HTuple hv_scratch_len1_thresh = 2;//划痕块长阈值(pix)，int，[0,100],2
        private HTuple hv_scratch_len2_thresh = 2;//划痕块宽阈值(pix)，int，[0,100],2

        //hvec_vec_inspect_param[6]:模板比对外观检测

        private HTuple hv_match_angle_start = -0.1;//匹配搜索起始角度(弧度)，Double,[-0.5,0]，-0.1
        private HTuple hv_match_angle_extent = 0.2;//匹配搜索角度范围(弧度)，Double，[0,1],0.2
        private HTuple hv_match_thresh = new HTuple();//匹配分数，有几个检测模板就有几个匹配分数阈值，这边有4个模板，所以设置了4个分数阈值，Double，[0,1],0.5



        #endregion

        #region 变量存储模块"声明变量"

        /// <summary>
        /// 写入INI文件
        /// </summary>
        /// <param name="section">节点名称[如[TypeName]]</param>
        /// <param name="key">键</param>
        /// <param name="val">值</param>
        /// <param name="filepath">文件路径</param>
        /// <returns></returns>
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filepath);
        /// <summary>
        /// 读取INI文件
        /// </summary>
        /// <param name="section">节点名称</param>
        /// <param name="key">键</param>
        /// <param name="def">值</param>
        /// <param name="retval">stringbulider对象</param>
        /// <param name="size">字节大小</param>
        /// <param name="filePath">文件路径</param>
        /// <returns></returns>
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retval, int size, string filePath);


        /// <summary>
        /// 自定义读取INI文件中的内容方法
        /// </summary>
        /// <param name="Section">键</param>
        /// <param name="key">值</param>
        /// <returns></returns>
        private string ContentValue(string strFilePath, string Section, string key)
        {

            StringBuilder temp = new StringBuilder(1024);
            GetPrivateProfileString(Section, key, "", temp, 1024, strFilePath);
            return temp.ToString();
        }
        #endregion
        #region ini存储方法
        public Boolean WriteData(string strFilePath, string strSec, string StepName, string value)
        {
            try
            {
                WritePrivateProfileString(strSec, StepName, value, strFilePath);
                return true;

            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message.ToString());
                return false;
            }
        }
        public Boolean ReadData(string strFilePath, string strSec, string StepName, out string value)
        {
            if (File.Exists(strFilePath))//读取时先要判读INI文件是否存在
            {
                value = ContentValue(strFilePath, strSec, StepName);
                return true;
            }
            else
            {
                value = "";
                return false;
            }
        }
        #endregion
        //参数存储封装参数
        HTupleVector hvec_vec_model_param = new HTupleVector(1);
        HTupleVector hvec_vec_inspect_param = new HTupleVector(1), hvec_vec_model_tuple = new HTupleVector(1);
        HObjectVector hvec_vec_model_object = new HObjectVector(1);
        Function CNNModel;
        /// <summary>
        /// 读取Model文件
        /// </summary>
        /// <returns>读取Model文件是否成功</returns>
        public bool LoadAllModels()
        {
            SetValue();
            //输入参数
            HTuple hv_iFlag_model = new HTuple(), hv_err_msg = new HTuple();
            try
            {
                string CNNModelPath = hv_model_path.S + "\\CNTK\\model5_cntk";
                LFAOI_load_all_model(out hvec_vec_model_object, hvec_vec_model_param, out hvec_vec_model_tuple,
                out hv_iFlag_model, out hv_err_msg);
                if (hv_iFlag_model.I == -1) return false;
                hv_model_path = new HTuple();
                hv_model_path = hvec_vec_model_param[0].T.Clone();                
                //CNNModel = LoadModel(CNNModelPath, DeviceDescriptor.GPUDevice(0));
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 检测算法
        /// </summary>
        /// <param name="rowIndex">芯片行坐标</param>
        /// <param name="columnIndex">芯片列坐标</param>
        /// <param name="imagefile">图像文件夹目录</param>
        /// <param name="ho_ImageIC">IC图像</param>
        /// <param name="ho_ImagePCB">PCB图像</param>
        /// <param name="ho_ImageLine">金线图像</param>
        /// <param name="ho_Defect_region">缺陷ROI</param>
        /// <param name="result">1为检测OK,0为检测含缺陷</param>
        /// <returns>true为检测成功,false为检测失败</returns>
        public bool Inspection(int rowIndex, int columnIndex, string imagefile, HObject ho_ImageIC, HObject ho_ImagePCB, HObject ho_ImageLine, out HObject ho_Defect_region, out int result)
        {
            try
            {
                string sPath = "";
                //sw = new StreamWriter(imagefile + "\\inspectionResult.txt", true);
                //输入参数
                HTuple hv_iFlag = new HTuple();
                HTuple hv_err_msg1 = new HTuple();
                HTuple hv_defect_type = new HTuple();
                //输出参数
                HObject ho_defect_region = null, ho_gold_wire = null;
                LFAOI_inspection(ho_ImageIC, ho_ImagePCB, ho_ImageLine, hvec_vec_model_object,
                out ho_defect_region, out ho_gold_wire, hvec_vec_model_tuple, hvec_vec_inspect_param,
                out hv_iFlag,out hv_defect_type, out hv_err_msg1);
                if (hv_iFlag.I == -1)
                {
                    WriteData(imagefile + "\\inspectionResult.ini", "inspectionResult", rowIndex.ToString() + "-" + columnIndex.ToString(), "NG");
                    //sw.WriteLine(rowIndex + "-" + columnIndex + "-" + "NG");
                    ho_Defect_region = ho_defect_region.CopyObj(1, -1);
                    sPath = imagefile + "\\" + rowIndex.ToString() + "-" + columnIndex.ToString();
                    if (!Directory.Exists(sPath))
                    {
                        Directory.CreateDirectory(sPath);
                    }
                    HOperatorSet.WriteRegion(ho_defect_region, sPath + "\\" + "defect.reg");
                    HOperatorSet.WriteTuple(hv_defect_type, sPath + "\\" + "defectType.tup");
                    result = 0;
                    ho_defect_region.Dispose();
                    ho_gold_wire.Dispose();
                    return true;
                }
                WriteData(imagefile + "\\inspectionResult.ini", "inspectionResult", rowIndex.ToString() + "-" + columnIndex.ToString(), "OK");
                //sw.WriteLine(rowIndex + "-" + columnIndex + "-" + "OK");
                ho_Defect_region = ho_defect_region.CopyObj(1, -1);
                result = 1;
                ho_defect_region.Dispose();
                ho_gold_wire.Dispose();
                return true;
            }
            catch
            {
                result = -1;
                HOperatorSet.GenEmptyRegion(out ho_Defect_region);
                return false;
            }
        }
        /// <summary>
        /// 检测
        /// </summary>
        /// <param name="index"></param>
        /// <param name="imagePath"></param>
        /// <param name="imagefile"></param>
        /// <param name="ho_ImageIC"></param>
        /// <param name="ho_ImagePCB"></param>
        /// <param name="ho_ImageLine"></param>
        /// <param name="ho_Defect_region"></param>
        /// <param name="ho_Gold_Wire"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public bool Inspection(int index, string imagePath, string imagefile, HObject ho_ImageIC, HObject ho_ImagePCB, HObject ho_ImageLine, out HObject ho_Defect_region, out HObject ho_Gold_Wire, out int result)
        {
            try
            {
                string sPath = "";
                //sw = new StreamWriter(imagefile + "\\inspectionResult.txt", true);
                //输入参数
                HTuple hv_iFlag = new HTuple();
                HTuple hv_err_msg1 = new HTuple();
                HTuple hv_defect_type = new HTuple();
                //输出参数
                HObject ho_defect_region = null, ho_gold_wire = null;
                LFAOI_inspection(ho_ImageIC, ho_ImagePCB, ho_ImageLine, hvec_vec_model_object,
                out ho_defect_region, out ho_gold_wire, hvec_vec_model_tuple, hvec_vec_inspect_param,
                out hv_iFlag,out hv_defect_type, out hv_err_msg1);
                if (hv_iFlag.I == -1)
                {
                    WriteData(imagefile + "\\inspectionResult2.ini", "inspectionResult2", index.ToString(), "NG");
                    //sw.WriteLine(rowIndex + "-" + columnIndex + "-" + "NG");
                    ho_Defect_region = ho_defect_region.CopyObj(1, -1);
                    ho_Gold_Wire = ho_gold_wire.CopyObj(1, -1);
                    //sPath = imagefile + "\\" + rowIndex.ToString() + "-" + columnIndex.ToString();
                    if (!Directory.Exists(imagePath))
                    {
                        Directory.CreateDirectory(imagePath);
                    }
                    HOperatorSet.WriteRegion(ho_defect_region, sPath + "\\" + "defect.reg");
                    HOperatorSet.WriteTuple(hv_defect_type, sPath + "\\" + "defectType.tup");
                    result = 0;
                    ho_defect_region.Dispose();
                    ho_gold_wire.Dispose();
                    return true;
                }
                WriteData(imagefile + "\\inspectionResult2.ini", "inspectionResult2", index.ToString(), "OK");
                //sw.WriteLine(rowIndex + "-" + columnIndex + "-" + "OK");
                ho_Defect_region = ho_defect_region.CopyObj(1, -1);
                ho_Gold_Wire = ho_gold_wire.CopyObj(1, -1);
                result = 1;
                ho_defect_region.Dispose();
                ho_gold_wire.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                result = -1;
                HOperatorSet.GenEmptyRegion(out ho_Defect_region);
                HOperatorSet.GenEmptyRegion(out ho_Gold_Wire);
                return false;
            }
        }
        /// <summary>
        /// 配置接口值到私有值上
        /// </summary>
        public void SetValue()
        {
            hvec_vec_model_param = (new HTupleVector(1).Insert(0, new HTupleVector(new HTuple())));
            hvec_vec_model_param.Clear();
            model_path = App.ProductFile + "\\" + ProductMagzine.ActivePdt +"\\Models";

            hv_model_path = model_path;
            hvec_vec_model_param[0] = new HTupleVector(hv_model_path).Clone();
            hvec_vec_inspect_param = (new HTupleVector(1).Insert(0, new HTupleVector(new HTuple())));
            hvec_vec_inspect_param.Clear();
            //pcb黄金模板比对
            hv_pcb_sub_reg_num = pcb_sub_reg_num;
            hv_pcb_sobel_scale = pcb_sobel_scale;
            hv_pcb_dark_thresh = pcb_dark_thresh;
            hv_pcb_light_thresh = pcb_light_thresh;
            hvec_vec_model_param[1] = new HTupleVector(((((hv_pcb_sub_reg_num.TupleConcat(
                hv_pcb_sobel_scale))).TupleConcat(hv_pcb_dark_thresh))).TupleConcat(hv_pcb_light_thresh));
            hv_pcb_score_thresh = pcb_score_thresh;
            hv_pcb_angle_start = pcb_angle_start;
            hv_pcb_angle_extent = pcb_angle_extent;
            hv_pcb_search_size = pcb_search_size;
            hv_pcb_closing_size = pcb_closing_size;
            hv_pcb_select_operation = pcb_select_operation;
            hv_pcb_width_thresh = pcb_width_thresh;
            hv_pcb_height_thresh = pcb_height_thresh;
            hv_pcb_area_thresh = pcb_area_thresh;
            hvec_vec_inspect_param[0] = new HTupleVector(((((((((((((((((hv_pcb_score_thresh.TupleConcat(
                hv_pcb_angle_start))).TupleConcat(hv_pcb_angle_extent))).TupleConcat(hv_pcb_search_size))).TupleConcat(
                hv_pcb_closing_size))).TupleConcat(hv_pcb_select_operation))).TupleConcat(
                hv_pcb_width_thresh))).TupleConcat(hv_pcb_height_thresh))).TupleConcat(
                hv_pcb_area_thresh))).TupleConcat(hv_pcb_sub_reg_num));
            //ic黄金模板比对
            hv_ic_sub_reg_num = ic_sub_reg_num;
            hv_ic_sobel_scale = ic_sobel_scale;
            hv_ic_dark_thresh = ic_dark_thresh;
            hv_ic_light_thresh = ic_light_thresh;
            hvec_vec_model_param[2] = new HTupleVector(((((hv_ic_sub_reg_num.TupleConcat(
                hv_ic_sobel_scale))).TupleConcat(hv_ic_dark_thresh))).TupleConcat(hv_ic_light_thresh));
            hv_ic_score_thresh = ic_score_thresh;
            hv_ic_angle_start = ic_angle_start;
            hv_ic_angle_extent = ic_angle_extent;
            hv_ic_search_size = ic_search_size;
            hv_ic_closing_size = ic_closing_size;
            hv_ic_select_operation = ic_select_operation;
            hv_ic_width_thresh = ic_width_thresh;
            hv_ic_height_thresh = ic_height_thresh;
            hv_ic_area_thresh = ic_area_thresh;

            hvec_vec_inspect_param[1] = new HTupleVector(((((((((((((((((hv_ic_score_thresh.TupleConcat(
                hv_ic_angle_start))).TupleConcat(hv_ic_angle_extent))).TupleConcat(hv_ic_search_size))).TupleConcat(
                hv_ic_closing_size))).TupleConcat(hv_ic_select_operation))).TupleConcat(
                hv_ic_width_thresh))).TupleConcat(hv_ic_height_thresh))).TupleConcat(hv_ic_area_thresh))).TupleConcat(
                hv_ic_sub_reg_num));
            //ic偏移检测
            hv_pos_row_thresh = pos_row_thresh;
            hv_pos_col_thresh = pos_col_thresh;
            hv_pos_angle_thresh = pos_angle_thresh;
            hvec_vec_inspect_param[2] = new HTupleVector(((hv_pos_row_thresh.TupleConcat(
                hv_pos_col_thresh))).TupleConcat(hv_pos_angle_thresh));
            //金线检测
            hv_line_ic_radius_low = line_ic_radius_low;
            hv_line_ic_radius_high = line_ic_radius_high;
            hv_line_pcb_radius_low = line_pcb_radius_low;
            hv_line_pcb_radius_high = line_pcb_radius_high;
            hv_line_num = line_num;
            hv_line_search_len1 = line_search_len1;
            hv_line_thresh = line_thresh;
            hv_line_width = line_width;
            hv_line_min_seg_length = line_min_seg_length;
            hv_line_clip_length = line_clip_length;
            hv_line_angle_extent = line_angle_extent;
            hv_line_max_gap = line_max_gap;
            hv_lead_wire_inspect_size = lead_wire_inspect_size;
            hv_lead_wire_distance_thresh = lead_wire_distance_thresh;
            hv_lead_wire_angle_thresh = lead_wire_angle_thresh;
            hv_lead_wire_contrast = lead_wire_contrast;
            hv_lead_wire_length_thresh = lead_wire_length_thresh;
            hv_line_pcb_ball_gray_thresh = line_pcb_ball_gray_thresh;
            hv_is_cntk = is_cntk;
            hv_line_pcb_clsoing_rasius = line_pcb_clsoing_rasius;
            hv_line_pcb_fill_max = line_pcb_fill_max;
                        hvec_vec_inspect_param[3] = new HTupleVector(((((((((((((((hv_line_ic_radius_low.TupleConcat(
                hv_line_ic_radius_high))).TupleConcat(hv_line_pcb_radius_low))).TupleConcat(
                hv_line_pcb_radius_high))).TupleConcat(hv_line_num))).TupleConcat(hv_line_search_len1))).TupleConcat(
                hv_line_thresh))).TupleConcat(hv_line_width))).TupleConcat(hv_line_min_seg_length).TupleConcat(hv_line_clip_length)
                .TupleConcat(hv_line_angle_extent).TupleConcat(hv_line_max_gap).TupleConcat(hv_lead_wire_inspect_size).TupleConcat(hv_lead_wire_distance_thresh)
                .TupleConcat(hv_lead_wire_angle_thresh).TupleConcat(hv_lead_wire_contrast).TupleConcat(hv_lead_wire_length_thresh).
                TupleConcat(hv_line_pcb_ball_gray_thresh).TupleConcat(hv_is_cntk)
                .TupleConcat(hv_line_pcb_clsoing_rasius).TupleConcat(hv_line_pcb_fill_max));
            //崩边检测
            hv_chipping_inspect_size = chipping_inspect_size;
            hv_chipping_low_thresh = chipping_low_thresh;
            hv_chipping_high_thresh = chipping_high_thresh;
            hv_chipping_opening_size = chipping_opening_size;
            hv_chipping_area_thresh = chipping_area_thresh;
            hv_chipping_len1_thresh = chipping_len1_thresh;
            hv_chipping_len2_thresh = chipping_len2_thresh;
            hv_chipping_select_operation = "and";
            hv_chipping_search_size = chipping_search_size;
            hv_chipping_score_thresh = chipping_score_thresh;
            hv_chipping_measure_length1 = chipping_measure_length1;
            hv_chipping_measure_length2 = chipping_measure_length2;
            hv_chipping_measure_threshold = chipping_measure_threshold;
            hvec_vec_inspect_param[4] = new HTupleVector(((((((((((((hv_chipping_inspect_size.TupleConcat(
                hv_chipping_low_thresh))).TupleConcat(hv_chipping_high_thresh))).TupleConcat(
                hv_chipping_opening_size))).TupleConcat(hv_chipping_area_thresh))).TupleConcat(
                hv_chipping_len1_thresh))).TupleConcat(hv_chipping_len2_thresh))).TupleConcat(
                hv_chipping_select_operation).TupleConcat(hv_chipping_search_size).TupleConcat(
                hv_chipping_score_thresh).TupleConcat(hv_chipping_measure_length1).TupleConcat(
                hv_chipping_measure_length2).TupleConcat(hv_chipping_measure_threshold));
            //划痕检测
            hv_scratch_is_gauss = scratch_is_gauss;
            hv_scratch_line_sigma = scratch_line_sigma;
            hv_scratch_line_low = scratch_line_low;
            hv_scratch_line_high = scratch_line_high;
            hv_scratch_light_dark = scratch_light_dark;
            hv_scratch_length_thresh = scratch_length_thresh;
            hv_scratch_mask_size = scratch_mask_size;
            hv_scratch_sigma_thresh = scratch_sigma_thresh;
            hv_scratch_gray_thresh = scratch_gray_thresh;
            hv_scratch_area_thresh = scratch_area_thresh;
            hv_scratch_len1_thresh = scratch_len1_thresh;
            hv_scratch_len2_thresh = scratch_len2_thresh;
            hvec_vec_inspect_param[5] = new HTupleVector(((((((((((((((((((((hv_scratch_is_gauss.TupleConcat(
                hv_scratch_line_sigma))).TupleConcat(hv_scratch_line_low))).TupleConcat(
                hv_scratch_line_high))).TupleConcat(hv_scratch_light_dark))).TupleConcat(
                hv_scratch_length_thresh))).TupleConcat(hv_scratch_mask_size))).TupleConcat(
                hv_scratch_sigma_thresh))).TupleConcat(hv_scratch_gray_thresh))).TupleConcat(
                hv_scratch_area_thresh))).TupleConcat(hv_scratch_len1_thresh))).TupleConcat(
                hv_scratch_len2_thresh));
            //模板比对外观检测
            hv_match_angle_start = match_angle_start;
            hv_match_angle_extent = match_angle_extent;
            hv_match_thresh = new HTuple();
            hv_match_thresh[0] = match_thresh[0];
            hv_match_thresh[1] = match_thresh[1];
            hv_match_thresh[2] = match_thresh[2];
            hv_match_thresh[3] = match_thresh[3];
            hv_match_thresh[4] = match_thresh[4];
            hv_match_thresh[5] = match_thresh[5];
            hv_match_thresh[6] = match_thresh[6];
            hv_match_thresh[7] = match_thresh[7];
            hvec_vec_inspect_param[6] = new HTupleVector(((hv_match_angle_start.TupleConcat(
                hv_match_angle_extent))).TupleConcat(hv_match_thresh));
        }

        #region halcon2C#
        public void LFAOI_clear_all_model(HTupleVector/*{eTupleVector,Dim=1}*/ hvec_vec_model_tuple,
            out HTuple hv_iFlag)
        {



            // Local iconic variables 

            // Local control variables 

            HTuple hv_model_len = null, hv_Index = null;
            HTuple hv_model_tuple = new HTuple(), hv_model_num = new HTuple();
            HTuple hv_model_id = new HTuple(), hv_model_type = new HTuple();
            HTuple hv_Exception = new HTuple();
            // Initialize local and output iconic variables 
            hv_iFlag = 0;
            hv_model_len = new HTuple(hvec_vec_model_tuple.Length);
            HTuple end_val2 = hv_model_len - 1;
            HTuple step_val2 = 1;
            for (hv_Index = 0; hv_Index.Continue(end_val2, step_val2); hv_Index = hv_Index.TupleAdd(step_val2))
            {
                if ((int)(new HTuple(hv_Index.TupleEqual(2))) != 0)
                {
                    continue;
                }
                hv_model_tuple = hvec_vec_model_tuple[hv_Index].T.Clone();
                hv_model_num = (new HTuple(hv_model_tuple.TupleLength())) / 2;
                if ((int)(hv_model_num) != 0)
                {
                    hv_model_id = hv_model_tuple.TupleSelectRange(0, hv_model_num - 1);
                    hv_model_type = hv_model_tuple.TupleSelectRange(hv_model_num, (hv_model_num * 2) - 1);
                    try
                    {
                        clear_model(hv_model_type, hv_model_id, out hv_iFlag);
                    }
                    // catch (Exception) 
                    catch (HalconException HDevExpDefaultException1)
                    {
                        HDevExpDefaultException1.ToHTuple(out hv_Exception);
                    }
                }
            }

            return;
        }

        //2.26 cfar
        //3.11 多个chipping
        //0312 cntk
        //0327 电阻*
        public void LFAOI_inspection(HObject ho_ImageIC, HObject ho_ImagePCB, HObject ho_ImageLine,
            HObjectVector/*{eObjectVector,Dim=1}*/ hvec_vec_model_object, out HObject ho_defect_region,
            out HObject ho_gold_wire, HTupleVector/*{eTupleVector,Dim=1}*/ hvec_vec_model_tuple,
            HTupleVector/*{eTupleVector,Dim=1}*/ hvec_vec_inspect_param, out HTuple hv_iFlag,
            out HTuple hv_defect_type, out HTuple hv_err_msg)
        {




            // Stack for temporary objects 
            HObject[] OTemp = new HObject[20];

            // Local iconic variables 

            HObject ho_pcb_model_object = null, ho_pcb_dark_thresh_image;
            HObject ho_pcb_light_thresh_image, ho_pcb_match_region;
            HObject ho_pcb_inspect_region, ho_pcb_reject_region, ho_pcb_sub_region;
            HObject ho_pcb_failure_regions, ho_pcb_inspect_region_affine;
            HObject ho_defect_region_union = null, ho_ic_model_object = null;
            HObject ho_ic_dark_thresh_image, ho_ic_light_thresh_image;
            HObject ho_ic_match_region, ho_ic_inspect_region, ho_ic_reject_region;
            HObject ho_ic_sub_region, ho_ic_failure_regions, ho_ic_inspect_region_affine;
            HObject ho_pcb_pad = null, ho_ic_pad = null, ho_Cross, ho_pcb_pad_affine;
            HObject ho_ic_pad_affine, ho_wire_defect_region, ho_chipping_region = null;
            HObject ho_chipping_defect_region = null, ho_aug_ic = null;
            HObject ho_aug_ic_select = null, ho_aug_ic_refine = null, ho_DefectRegionsPCB = null;
            HObject ho_DefectRegionsLine = null, ho_DefectRegionsIC = null;
            HObject ho_RegionUnion = null, ho_DefectRegion = null, ho_scratch_inspct_region = null;
            HObject ho_RegionAffineTrans, ho_scratch_defect_region;
            HObject ho_inspect_region = null, ho_DefectLinesLine = null;
            HObject ho_DefectRegionCFAR = null, ho_DefectLinesPCB = null;
            HObject ho_Rectangle = null, ho_ConnectedRegions = null, ho_SelectedRegions = null;
            HObject ho_match_inspect_region = null, ho_RegionDilation;
            HObject ho_match_defect_region, ho_ObjectSelected = null;
            HObject ho_DefectRegions0 = null, ho_DefectRegions1 = null;
            HObject ho_DefectRegions = null, ho_match_defect_region_;

            // Local control variables 

            HTuple hv_pcb_ModelID = null, hv_pcb_model_type = null;
            HTuple hv_pcb_params = null, hv_pcb_score_thresh = null;
            HTuple hv_pcb_angle_start = null, hv_pcb_angle_extent = null;
            HTuple hv_pcb_search_size = null, hv_pcb_closing_size = null;
            HTuple hv_pcb_select_operation = null, hv_pcb_width_thresh = null;
            HTuple hv_pcb_height_thresh = null, hv_pcb_area_thresh = null;
            HTuple hv_pcb_sub_reg_num = null, hv_pcb_hom_mat2d = null;
            HTuple hv_pcb_row = null, hv_pcb_col = null, hv_pcb_angle = null;
            HTuple hv_pcb_iFlag = null, hv_pcb_ErrMsg = null, hv_Number = new HTuple();
            HTuple hv_ic_ModelID = null, hv_ic_model_type = null, hv_ic_params = null;
            HTuple hv_ic_score_thresh = null, hv_ic_angle_start = null;
            HTuple hv_ic_angle_extent = null, hv_ic_search_size = null;
            HTuple hv_ic_closing_size = null, hv_ic_select_operation = null;
            HTuple hv_ic_width_thresh = null, hv_ic_height_thresh = null;
            HTuple hv_ic_area_thresh = null, hv_ic_sub_reg_num = null;
            HTuple hv_ic_hom_mat2d = null, hv_ic_row = null, hv_ic_col = null;
            HTuple hv_ic_angle = null, hv_ic_iFlag = null, hv_ic_ErrMsg = null;
            HTuple hv_postion_params = null, hv_row_thresh = null;
            HTuple hv_col_thresh = null, hv_angle_thresh = null, hv_row_diff = null;
            HTuple hv_col_diff = null, hv_angle_diff = null, hv_ic_pos_iFlag = null;
            HTuple hv_pcb_ball_num = null, hv_inspect_block_num = null;
            HTuple hv_inspect_wire_num = null, hv_inspect_row_ = null;
            HTuple hv_inspect_col_ = null, hv_inspect_row = null, hv_inspect_col = null;
            HTuple hv_line_params = null, hv_ic_radius_low = null;
            HTuple hv_ic_radius_high = null, hv_pcb_radius_low = null;
            HTuple hv_pcb_radius_high = null, hv_line_num = null, hv_search_len1 = null;
            HTuple hv_line_thresh = null, hv_line_width = null, hv_min_seg_length = null;
            HTuple hv_line_clip_length = null, hv_line_angle_extent = null;
            HTuple hv_line_max_gap = null, hv_lead_wire_inspect_size = null;
            HTuple hv_lead_wire_distance_thresh = null, hv_lead_wire_angle_thresh = null;
            HTuple hv_lead_wire_contrast = null, hv_lead_wire_length_thresh = null;
            HTuple hv_pcb_ball_gray_thresh = null, hv_is_cntk = null;
            HTuple hv_pcb_closing_radius = null, hv_pcb_fill_up_max = null;
            HTuple hv_gold_wire_iFlag = null, hv_chipping_model_tuple = null;
            HTuple hv_chipping_params = new HTuple(), hv_inspect_size = new HTuple();
            HTuple hv_low_thresh = new HTuple(), hv_high_thresh = new HTuple();
            HTuple hv_opening_size = new HTuple(), hv_area_thresh = new HTuple();
            HTuple hv_len1_thresh = new HTuple(), hv_len2_thresh = new HTuple();
            HTuple hv_select_operation = new HTuple(), hv_search_size = new HTuple();
            HTuple hv_find_shape_score1 = new HTuple(), hv_measure_length1 = new HTuple();
            HTuple hv_measure_length2 = new HTuple(), hv_measure_threshold = new HTuple();
            HTuple hv_chipping_num = new HTuple(), hv_chipping_model_id = new HTuple();
            HTuple hv_chipping_model_type = new HTuple(), hv_Index = new HTuple();
            HTuple hv_model_id = new HTuple(), hv_model_type = new HTuple();
            HTuple hv_iFlag_aux_ic = new HTuple(), hv_iFlag2 = new HTuple();
            HTuple hv_ErrMsg = new HTuple(), hv_scratc_params = null;
            HTuple hv_is_gauss = null, hv_line_sigma = null, hv_line_low = null;
            HTuple hv_line_high = null, hv_light_dark = null, hv_length_thresh = null;
            HTuple hv_mask_size = null, hv_sigma_thresh = null, hv_gray_thresh = null;
            HTuple hv_Row = new HTuple(), hv_Column = new HTuple();
            HTuple hv_Phi = new HTuple(), hv_Length1 = new HTuple();
            HTuple hv_Length2 = new HTuple(), hv_match_model_tuple = null;
            HTuple hv_match_num = null, hv_match_model_id = null, hv_match_model_type = null;
            HTuple hv_match_params = null, hv_angle_start = null, hv_angle_extent = null;
            HTuple hv_match_thresh = null, hv_match_thresh_ = new HTuple();
            HTuple hv_MatchScore0 = new HTuple(), hv_iFlag1 = new HTuple();
            HTuple hv_err_msg1 = new HTuple(), hv_MatchScore1 = new HTuple();
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_defect_region);
            HOperatorSet.GenEmptyObj(out ho_gold_wire);
            HOperatorSet.GenEmptyObj(out ho_pcb_model_object);
            HOperatorSet.GenEmptyObj(out ho_pcb_dark_thresh_image);
            HOperatorSet.GenEmptyObj(out ho_pcb_light_thresh_image);
            HOperatorSet.GenEmptyObj(out ho_pcb_match_region);
            HOperatorSet.GenEmptyObj(out ho_pcb_inspect_region);
            HOperatorSet.GenEmptyObj(out ho_pcb_reject_region);
            HOperatorSet.GenEmptyObj(out ho_pcb_sub_region);
            HOperatorSet.GenEmptyObj(out ho_pcb_failure_regions);
            HOperatorSet.GenEmptyObj(out ho_pcb_inspect_region_affine);
            HOperatorSet.GenEmptyObj(out ho_defect_region_union);
            HOperatorSet.GenEmptyObj(out ho_ic_model_object);
            HOperatorSet.GenEmptyObj(out ho_ic_dark_thresh_image);
            HOperatorSet.GenEmptyObj(out ho_ic_light_thresh_image);
            HOperatorSet.GenEmptyObj(out ho_ic_match_region);
            HOperatorSet.GenEmptyObj(out ho_ic_inspect_region);
            HOperatorSet.GenEmptyObj(out ho_ic_reject_region);
            HOperatorSet.GenEmptyObj(out ho_ic_sub_region);
            HOperatorSet.GenEmptyObj(out ho_ic_failure_regions);
            HOperatorSet.GenEmptyObj(out ho_ic_inspect_region_affine);
            HOperatorSet.GenEmptyObj(out ho_pcb_pad);
            HOperatorSet.GenEmptyObj(out ho_ic_pad);
            HOperatorSet.GenEmptyObj(out ho_Cross);
            HOperatorSet.GenEmptyObj(out ho_pcb_pad_affine);
            HOperatorSet.GenEmptyObj(out ho_ic_pad_affine);
            HOperatorSet.GenEmptyObj(out ho_wire_defect_region);
            HOperatorSet.GenEmptyObj(out ho_chipping_region);
            HOperatorSet.GenEmptyObj(out ho_chipping_defect_region);
            HOperatorSet.GenEmptyObj(out ho_aug_ic);
            HOperatorSet.GenEmptyObj(out ho_aug_ic_select);
            HOperatorSet.GenEmptyObj(out ho_aug_ic_refine);
            HOperatorSet.GenEmptyObj(out ho_DefectRegionsPCB);
            HOperatorSet.GenEmptyObj(out ho_DefectRegionsLine);
            HOperatorSet.GenEmptyObj(out ho_DefectRegionsIC);
            HOperatorSet.GenEmptyObj(out ho_RegionUnion);
            HOperatorSet.GenEmptyObj(out ho_DefectRegion);
            HOperatorSet.GenEmptyObj(out ho_scratch_inspct_region);
            HOperatorSet.GenEmptyObj(out ho_RegionAffineTrans);
            HOperatorSet.GenEmptyObj(out ho_scratch_defect_region);
            HOperatorSet.GenEmptyObj(out ho_inspect_region);
            HOperatorSet.GenEmptyObj(out ho_DefectLinesLine);
            HOperatorSet.GenEmptyObj(out ho_DefectRegionCFAR);
            HOperatorSet.GenEmptyObj(out ho_DefectLinesPCB);
            HOperatorSet.GenEmptyObj(out ho_Rectangle);
            HOperatorSet.GenEmptyObj(out ho_ConnectedRegions);
            HOperatorSet.GenEmptyObj(out ho_SelectedRegions);
            HOperatorSet.GenEmptyObj(out ho_match_inspect_region);
            HOperatorSet.GenEmptyObj(out ho_RegionDilation);
            HOperatorSet.GenEmptyObj(out ho_match_defect_region);
            HOperatorSet.GenEmptyObj(out ho_ObjectSelected);
            HOperatorSet.GenEmptyObj(out ho_DefectRegions0);
            HOperatorSet.GenEmptyObj(out ho_DefectRegions1);
            HOperatorSet.GenEmptyObj(out ho_DefectRegions);
            HOperatorSet.GenEmptyObj(out ho_match_defect_region_);
            try
            {
                //*****************************************************************************
                //defect type: 1(pcb match failed), 2(pcb surface defect), 3(ic match failed)  *              4(ic surface defect), 5(ic position shift), 6(gold wire defect)  *              7(auxiliary ic chipping), 8(comos scratch), 9(capacitor defect)
                //*****************************************************************************
                hv_iFlag = 0;
                hv_err_msg = "";
                hv_defect_type = new HTuple();
                ho_defect_region.Dispose();
                HOperatorSet.GenEmptyObj(out ho_defect_region);
                //******************************************** pcb golden inspect
                //model
                ho_pcb_model_object.Dispose();
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    ho_pcb_model_object = hvec_vec_model_object[0].O.CopyObj(1, -1);
                }
                ho_pcb_dark_thresh_image.Dispose();
                HOperatorSet.SelectObj(ho_pcb_model_object, out ho_pcb_dark_thresh_image, 7);
                ho_pcb_light_thresh_image.Dispose();
                HOperatorSet.SelectObj(ho_pcb_model_object, out ho_pcb_light_thresh_image,
                    8);
                ho_pcb_match_region.Dispose();
                HOperatorSet.SelectObj(ho_pcb_model_object, out ho_pcb_match_region, 3);
                ho_pcb_inspect_region.Dispose();
                HOperatorSet.SelectObj(ho_pcb_model_object, out ho_pcb_inspect_region, 4);
                ho_pcb_reject_region.Dispose();
                HOperatorSet.SelectObj(ho_pcb_model_object, out ho_pcb_reject_region, 5);
                ho_pcb_sub_region.Dispose();
                HOperatorSet.SelectObj(ho_pcb_model_object, out ho_pcb_sub_region, 6);
                hv_pcb_ModelID = hvec_vec_model_tuple[0].T[0];
                hv_pcb_model_type = hvec_vec_model_tuple[0].T[1];
                //parameters
                hv_pcb_params = hvec_vec_inspect_param[0].T.Clone();
                hv_pcb_score_thresh = hv_pcb_params[0];
                hv_pcb_angle_start = hv_pcb_params[1];
                hv_pcb_angle_extent = hv_pcb_params[2];
                hv_pcb_search_size = hv_pcb_params[3];
                hv_pcb_closing_size = hv_pcb_params[4];
                hv_pcb_select_operation = hv_pcb_params[5];
                hv_pcb_width_thresh = hv_pcb_params[6];
                hv_pcb_height_thresh = hv_pcb_params[7];
                hv_pcb_area_thresh = hv_pcb_params[8];
                hv_pcb_sub_reg_num = hv_pcb_params[9];
                ho_pcb_failure_regions.Dispose(); ho_pcb_inspect_region_affine.Dispose();
                inspect_golden_model(ho_ImagePCB, ho_pcb_dark_thresh_image, ho_pcb_light_thresh_image,
                    ho_pcb_match_region, ho_pcb_inspect_region, ho_pcb_reject_region, ho_pcb_sub_region,
                    out ho_pcb_failure_regions, out ho_pcb_inspect_region_affine, hv_pcb_ModelID,
                    hv_pcb_model_type, hv_pcb_score_thresh, hv_pcb_angle_start, hv_pcb_angle_extent,
                    hv_pcb_sub_reg_num, hv_pcb_select_operation, hv_pcb_width_thresh, hv_pcb_height_thresh,
                    hv_pcb_area_thresh, hv_pcb_closing_size, hv_pcb_search_size, out hv_pcb_hom_mat2d,
                    out hv_pcb_row, out hv_pcb_col, out hv_pcb_angle, out hv_pcb_iFlag, out hv_pcb_ErrMsg);
                if ((int)(new HTuple(hv_pcb_iFlag.TupleNotEqual(0))) != 0)
                {
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.ConcatObj(ho_defect_region, ho_pcb_failure_regions, out ExpTmpOutVar_0
                            );
                        ho_defect_region.Dispose();
                        ho_defect_region = ExpTmpOutVar_0;
                    }
                    hv_defect_type = hv_defect_type.TupleConcat(1);
                    hv_iFlag = -1;
                    ho_pcb_model_object.Dispose();
                    ho_pcb_dark_thresh_image.Dispose();
                    ho_pcb_light_thresh_image.Dispose();
                    ho_pcb_match_region.Dispose();
                    ho_pcb_inspect_region.Dispose();
                    ho_pcb_reject_region.Dispose();
                    ho_pcb_sub_region.Dispose();
                    ho_pcb_failure_regions.Dispose();
                    ho_pcb_inspect_region_affine.Dispose();
                    ho_defect_region_union.Dispose();
                    ho_ic_model_object.Dispose();
                    ho_ic_dark_thresh_image.Dispose();
                    ho_ic_light_thresh_image.Dispose();
                    ho_ic_match_region.Dispose();
                    ho_ic_inspect_region.Dispose();
                    ho_ic_reject_region.Dispose();
                    ho_ic_sub_region.Dispose();
                    ho_ic_failure_regions.Dispose();
                    ho_ic_inspect_region_affine.Dispose();
                    ho_pcb_pad.Dispose();
                    ho_ic_pad.Dispose();
                    ho_Cross.Dispose();
                    ho_pcb_pad_affine.Dispose();
                    ho_ic_pad_affine.Dispose();
                    ho_wire_defect_region.Dispose();
                    ho_chipping_region.Dispose();
                    ho_chipping_defect_region.Dispose();
                    ho_aug_ic.Dispose();
                    ho_aug_ic_select.Dispose();
                    ho_aug_ic_refine.Dispose();
                    ho_DefectRegionsPCB.Dispose();
                    ho_DefectRegionsLine.Dispose();
                    ho_DefectRegionsIC.Dispose();
                    ho_RegionUnion.Dispose();
                    ho_DefectRegion.Dispose();
                    ho_scratch_inspct_region.Dispose();
                    ho_RegionAffineTrans.Dispose();
                    ho_scratch_defect_region.Dispose();
                    ho_inspect_region.Dispose();
                    ho_DefectLinesLine.Dispose();
                    ho_DefectRegionCFAR.Dispose();
                    ho_DefectLinesPCB.Dispose();
                    ho_Rectangle.Dispose();
                    ho_ConnectedRegions.Dispose();
                    ho_SelectedRegions.Dispose();
                    ho_match_inspect_region.Dispose();
                    ho_RegionDilation.Dispose();
                    ho_match_defect_region.Dispose();
                    ho_ObjectSelected.Dispose();
                    ho_DefectRegions0.Dispose();
                    ho_DefectRegions1.Dispose();
                    ho_DefectRegions.Dispose();
                    ho_match_defect_region_.Dispose();

                    return;
                }
                else
                {
                    HOperatorSet.CountObj(ho_pcb_failure_regions, out hv_Number);
                    if ((int)(hv_Number) != 0)
                    {
                        ho_defect_region_union.Dispose();
                        HOperatorSet.Union1(ho_pcb_failure_regions, out ho_defect_region_union);
                        {
                            HObject ExpTmpOutVar_0;
                            HOperatorSet.ConcatObj(ho_defect_region, ho_defect_region_union, out ExpTmpOutVar_0
                                );
                            ho_defect_region.Dispose();
                            ho_defect_region = ExpTmpOutVar_0;
                        }
                        hv_defect_type = hv_defect_type.TupleConcat(2);
                    }
                }
                //******************************************** ic golden inspect
                //model
                ho_ic_model_object.Dispose();
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    ho_ic_model_object = hvec_vec_model_object[1].O.CopyObj(1, -1);
                }
                ho_ic_dark_thresh_image.Dispose();
                HOperatorSet.SelectObj(ho_ic_model_object, out ho_ic_dark_thresh_image, 7);
                ho_ic_light_thresh_image.Dispose();
                HOperatorSet.SelectObj(ho_ic_model_object, out ho_ic_light_thresh_image, 8);
                ho_ic_match_region.Dispose();
                HOperatorSet.SelectObj(ho_ic_model_object, out ho_ic_match_region, 3);
                ho_ic_inspect_region.Dispose();
                HOperatorSet.SelectObj(ho_ic_model_object, out ho_ic_inspect_region, 4);
                ho_ic_reject_region.Dispose();
                HOperatorSet.SelectObj(ho_ic_model_object, out ho_ic_reject_region, 5);
                ho_ic_sub_region.Dispose();
                HOperatorSet.SelectObj(ho_ic_model_object, out ho_ic_sub_region, 6);
                hv_ic_ModelID = hvec_vec_model_tuple[1].T[0];
                hv_ic_model_type = hvec_vec_model_tuple[1].T[1];
                //parameters
                hv_ic_params = hvec_vec_inspect_param[1].T.Clone();
                hv_ic_score_thresh = hv_ic_params[0];
                hv_ic_angle_start = hv_ic_params[1];
                hv_ic_angle_extent = hv_ic_params[2];
                hv_ic_search_size = hv_ic_params[3];
                hv_ic_closing_size = hv_ic_params[4];
                hv_ic_select_operation = hv_ic_params[5];
                hv_ic_width_thresh = hv_ic_params[6];
                hv_ic_height_thresh = hv_ic_params[7];
                hv_ic_area_thresh = hv_ic_params[8];
                hv_ic_sub_reg_num = hv_ic_params[9];
                ho_ic_failure_regions.Dispose(); ho_ic_inspect_region_affine.Dispose();
                inspect_golden_model(ho_ImageIC, ho_ic_dark_thresh_image, ho_ic_light_thresh_image,
                    ho_ic_match_region, ho_ic_inspect_region, ho_ic_reject_region, ho_ic_sub_region,
                    out ho_ic_failure_regions, out ho_ic_inspect_region_affine, hv_ic_ModelID,
                    hv_ic_model_type, hv_ic_score_thresh, hv_ic_angle_start, hv_ic_angle_extent,
                    hv_ic_sub_reg_num, hv_ic_select_operation, hv_ic_width_thresh, hv_ic_height_thresh,
                    hv_ic_area_thresh, hv_ic_closing_size, hv_ic_search_size, out hv_ic_hom_mat2d,
                    out hv_ic_row, out hv_ic_col, out hv_ic_angle, out hv_ic_iFlag, out hv_ic_ErrMsg);
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.ConcatObj(ho_defect_region, ho_ic_failure_regions, out ExpTmpOutVar_0
                        );
                    ho_defect_region.Dispose();
                    ho_defect_region = ExpTmpOutVar_0;
                }
                if ((int)(new HTuple(hv_ic_iFlag.TupleNotEqual(0))) != 0)
                {
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.ConcatObj(ho_defect_region, ho_ic_failure_regions, out ExpTmpOutVar_0
                            );
                        ho_defect_region.Dispose();
                        ho_defect_region = ExpTmpOutVar_0;
                    }
                    hv_defect_type = hv_defect_type.TupleConcat(3);
                    hv_iFlag = -1;
                    ho_pcb_model_object.Dispose();
                    ho_pcb_dark_thresh_image.Dispose();
                    ho_pcb_light_thresh_image.Dispose();
                    ho_pcb_match_region.Dispose();
                    ho_pcb_inspect_region.Dispose();
                    ho_pcb_reject_region.Dispose();
                    ho_pcb_sub_region.Dispose();
                    ho_pcb_failure_regions.Dispose();
                    ho_pcb_inspect_region_affine.Dispose();
                    ho_defect_region_union.Dispose();
                    ho_ic_model_object.Dispose();
                    ho_ic_dark_thresh_image.Dispose();
                    ho_ic_light_thresh_image.Dispose();
                    ho_ic_match_region.Dispose();
                    ho_ic_inspect_region.Dispose();
                    ho_ic_reject_region.Dispose();
                    ho_ic_sub_region.Dispose();
                    ho_ic_failure_regions.Dispose();
                    ho_ic_inspect_region_affine.Dispose();
                    ho_pcb_pad.Dispose();
                    ho_ic_pad.Dispose();
                    ho_Cross.Dispose();
                    ho_pcb_pad_affine.Dispose();
                    ho_ic_pad_affine.Dispose();
                    ho_wire_defect_region.Dispose();
                    ho_chipping_region.Dispose();
                    ho_chipping_defect_region.Dispose();
                    ho_aug_ic.Dispose();
                    ho_aug_ic_select.Dispose();
                    ho_aug_ic_refine.Dispose();
                    ho_DefectRegionsPCB.Dispose();
                    ho_DefectRegionsLine.Dispose();
                    ho_DefectRegionsIC.Dispose();
                    ho_RegionUnion.Dispose();
                    ho_DefectRegion.Dispose();
                    ho_scratch_inspct_region.Dispose();
                    ho_RegionAffineTrans.Dispose();
                    ho_scratch_defect_region.Dispose();
                    ho_inspect_region.Dispose();
                    ho_DefectLinesLine.Dispose();
                    ho_DefectRegionCFAR.Dispose();
                    ho_DefectLinesPCB.Dispose();
                    ho_Rectangle.Dispose();
                    ho_ConnectedRegions.Dispose();
                    ho_SelectedRegions.Dispose();
                    ho_match_inspect_region.Dispose();
                    ho_RegionDilation.Dispose();
                    ho_match_defect_region.Dispose();
                    ho_ObjectSelected.Dispose();
                    ho_DefectRegions0.Dispose();
                    ho_DefectRegions1.Dispose();
                    ho_DefectRegions.Dispose();
                    ho_match_defect_region_.Dispose();

                    return;
                }
                else
                {
                    HOperatorSet.CountObj(ho_ic_failure_regions, out hv_Number);
                    if ((int)(hv_Number) != 0)
                    {
                        ho_defect_region_union.Dispose();
                        HOperatorSet.Union1(ho_ic_failure_regions, out ho_defect_region_union);
                        {
                            HObject ExpTmpOutVar_0;
                            HOperatorSet.ConcatObj(ho_defect_region, ho_defect_region_union, out ExpTmpOutVar_0
                                );
                            ho_defect_region.Dispose();
                            ho_defect_region = ExpTmpOutVar_0;
                        }
                        hv_defect_type = hv_defect_type.TupleConcat(4);
                    }
                }
                //******************************************** ic position inspect
                hv_postion_params = hvec_vec_inspect_param[2].T.Clone();
                hv_row_thresh = hv_postion_params[0];
                hv_col_thresh = hv_postion_params[1];
                hv_angle_thresh = hv_postion_params[2];
                compare_position(hv_ic_row, hv_ic_col, hv_ic_angle, hv_pcb_row, hv_pcb_col,
                    hv_pcb_angle, hv_row_thresh, hv_col_thresh, hv_angle_thresh, out hv_row_diff,
                    out hv_col_diff, out hv_angle_diff, out hv_ic_pos_iFlag);
                if ((int)(new HTuple(hv_ic_pos_iFlag.TupleNotEqual(0))) != 0)
                {
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.ConcatObj(ho_defect_region, ho_ic_inspect_region_affine, out ExpTmpOutVar_0
                            );
                        ho_defect_region.Dispose();
                        ho_defect_region = ExpTmpOutVar_0;
                    }
                    hv_defect_type = hv_defect_type.TupleConcat(5);
                    hv_iFlag = -1;
                    ho_pcb_model_object.Dispose();
                    ho_pcb_dark_thresh_image.Dispose();
                    ho_pcb_light_thresh_image.Dispose();
                    ho_pcb_match_region.Dispose();
                    ho_pcb_inspect_region.Dispose();
                    ho_pcb_reject_region.Dispose();
                    ho_pcb_sub_region.Dispose();
                    ho_pcb_failure_regions.Dispose();
                    ho_pcb_inspect_region_affine.Dispose();
                    ho_defect_region_union.Dispose();
                    ho_ic_model_object.Dispose();
                    ho_ic_dark_thresh_image.Dispose();
                    ho_ic_light_thresh_image.Dispose();
                    ho_ic_match_region.Dispose();
                    ho_ic_inspect_region.Dispose();
                    ho_ic_reject_region.Dispose();
                    ho_ic_sub_region.Dispose();
                    ho_ic_failure_regions.Dispose();
                    ho_ic_inspect_region_affine.Dispose();
                    ho_pcb_pad.Dispose();
                    ho_ic_pad.Dispose();
                    ho_Cross.Dispose();
                    ho_pcb_pad_affine.Dispose();
                    ho_ic_pad_affine.Dispose();
                    ho_wire_defect_region.Dispose();
                    ho_chipping_region.Dispose();
                    ho_chipping_defect_region.Dispose();
                    ho_aug_ic.Dispose();
                    ho_aug_ic_select.Dispose();
                    ho_aug_ic_refine.Dispose();
                    ho_DefectRegionsPCB.Dispose();
                    ho_DefectRegionsLine.Dispose();
                    ho_DefectRegionsIC.Dispose();
                    ho_RegionUnion.Dispose();
                    ho_DefectRegion.Dispose();
                    ho_scratch_inspct_region.Dispose();
                    ho_RegionAffineTrans.Dispose();
                    ho_scratch_defect_region.Dispose();
                    ho_inspect_region.Dispose();
                    ho_DefectLinesLine.Dispose();
                    ho_DefectRegionCFAR.Dispose();
                    ho_DefectLinesPCB.Dispose();
                    ho_Rectangle.Dispose();
                    ho_ConnectedRegions.Dispose();
                    ho_SelectedRegions.Dispose();
                    ho_match_inspect_region.Dispose();
                    ho_RegionDilation.Dispose();
                    ho_match_defect_region.Dispose();
                    ho_ObjectSelected.Dispose();
                    ho_DefectRegions0.Dispose();
                    ho_DefectRegions1.Dispose();
                    ho_DefectRegions.Dispose();
                    ho_match_defect_region_.Dispose();

                    return;
                }
                //******************************************** gold line inspect
                ho_pcb_pad.Dispose();
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    ho_pcb_pad = hvec_vec_model_object[2].O.CopyObj(1, -1);
                }
                ho_ic_pad.Dispose();
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    ho_ic_pad = hvec_vec_model_object[3].O.CopyObj(1, -1);
                }
                hv_pcb_ball_num = hvec_vec_model_tuple[2].T.Clone();
                //** cntk test **************************************
                hv_inspect_block_num = (new HTuple((hvec_vec_model_tuple[3]).T.TupleLength())) / 3;
                hv_inspect_wire_num = (hvec_vec_model_tuple[3].T).TupleSelectRange(0, hv_inspect_block_num - 1);
                hv_inspect_row_ = (hvec_vec_model_tuple[3].T).TupleSelectRange(hv_inspect_block_num,
                    (hv_inspect_block_num * 2) - 1);
                hv_inspect_col_ = (hvec_vec_model_tuple[3].T).TupleSelectRange(hv_inspect_block_num * 2,
                    (hv_inspect_block_num * 3) - 1);
                HOperatorSet.AffineTransPoint2d(hv_pcb_hom_mat2d, hv_inspect_row_, hv_inspect_col_,
                    out hv_inspect_row, out hv_inspect_col);
                ho_Cross.Dispose();
                HOperatorSet.GenCrossContourXld(out ho_Cross, hv_inspect_row, hv_inspect_col,
                    6, hv_ic_angle);
                //***************************************************
                ho_pcb_pad_affine.Dispose();
                HOperatorSet.AffineTransRegion(ho_pcb_pad, out ho_pcb_pad_affine, hv_pcb_hom_mat2d,
                    "nearest_neighbor");
                ho_ic_pad_affine.Dispose();
                HOperatorSet.AffineTransRegion(ho_ic_pad, out ho_ic_pad_affine, hv_ic_hom_mat2d,
                    "nearest_neighbor");
                //params
                hv_line_params = hvec_vec_inspect_param[3].T.Clone();
                hv_ic_radius_low = hv_line_params[0];
                hv_ic_radius_high = hv_line_params[1];
                hv_pcb_radius_low = hv_line_params[2];
                hv_pcb_radius_high = hv_line_params[3];
                hv_line_num = hv_line_params[4];
                hv_search_len1 = hv_line_params[5];
                hv_line_thresh = hv_line_params[6];
                hv_line_width = hv_line_params[7];
                hv_min_seg_length = hv_line_params[8];
                hv_line_clip_length = hv_line_params[9];
                hv_line_angle_extent = hv_line_params[10];
                hv_line_max_gap = hv_line_params[11];
                hv_lead_wire_inspect_size = hv_line_params[12];
                hv_lead_wire_distance_thresh = hv_line_params[13];
                hv_lead_wire_angle_thresh = hv_line_params[14];
                hv_lead_wire_contrast = hv_line_params[15];
                hv_lead_wire_length_thresh = hv_line_params[16];
                hv_pcb_ball_gray_thresh = hv_line_params[17];
                hv_is_cntk = hv_line_params[18];
                hv_pcb_closing_radius = hv_line_params[19];
                hv_pcb_fill_up_max = hv_line_params[20];
                ho_wire_defect_region.Dispose(); ho_gold_wire.Dispose();
                gold_wire_detect(ho_ImageIC, ho_ImagePCB, ho_ImageLine, ho_ic_pad_affine, ho_pcb_pad_affine,
                    out ho_wire_defect_region, out ho_gold_wire, hv_line_num, hv_pcb_ball_num,
                    hv_pcb_radius_low, hv_pcb_radius_high, hv_ic_radius_low, hv_ic_radius_high,
                    hv_search_len1, hv_line_width, hv_line_thresh, hv_min_seg_length, hv_inspect_wire_num,
                    hv_inspect_row, hv_inspect_col, hv_line_clip_length, hv_line_angle_extent,
                    hv_line_max_gap, hv_lead_wire_inspect_size, hv_lead_wire_distance_thresh,
                    hv_lead_wire_angle_thresh, hv_lead_wire_contrast, hv_lead_wire_length_thresh,
                    hv_pcb_ball_gray_thresh, hv_is_cntk, hv_pcb_closing_radius, hv_pcb_fill_up_max,
                    out hv_gold_wire_iFlag);
                if ((int)(new HTuple(hv_gold_wire_iFlag.TupleNotEqual(0))) != 0)
                {
                    ho_defect_region_union.Dispose();
                    HOperatorSet.Union1(ho_wire_defect_region, out ho_defect_region_union);
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.ConcatObj(ho_defect_region, ho_defect_region_union, out ExpTmpOutVar_0
                            );
                        ho_defect_region.Dispose();
                        ho_defect_region = ExpTmpOutVar_0;
                    }
                    hv_defect_type = hv_defect_type.TupleConcat(6);
                }
                //******************************************** chipping inspect
                ho_chipping_region.Dispose();
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    ho_chipping_region = hvec_vec_model_object[4].O.CopyObj(1, -1);
                }
                hv_chipping_model_tuple = hvec_vec_model_tuple[4].T.Clone();

                HOperatorSet.CountObj(ho_chipping_region, out hv_Number);
                if ((int)(new HTuple(hv_Number.TupleGreaterEqual(1))) != 0)
                {
                    hv_chipping_params = hvec_vec_inspect_param[4].T.Clone();
                    hv_inspect_size = hv_chipping_params[0];
                    hv_low_thresh = hv_chipping_params[1];
                    hv_high_thresh = hv_chipping_params[2];
                    hv_opening_size = hv_chipping_params[3];
                    hv_area_thresh = hv_chipping_params[4];
                    hv_len1_thresh = hv_chipping_params[5];
                    hv_len2_thresh = hv_chipping_params[6];
                    hv_select_operation = hv_chipping_params[7];
                    hv_search_size = hv_chipping_params[8];
                    hv_find_shape_score1 = hv_chipping_params[9];
                    hv_measure_length1 = hv_chipping_params[10];
                    hv_measure_length2 = hv_chipping_params[11];
                    hv_measure_threshold = hv_chipping_params[12];
                    hv_chipping_num = (new HTuple(hv_chipping_model_tuple.TupleLength())) / 2;
                    hv_chipping_model_id = hv_chipping_model_tuple.TupleSelectRange(0, hv_chipping_num - 1);
                    hv_chipping_model_type = hv_chipping_model_tuple.TupleSelectRange(hv_chipping_num,
                        (hv_chipping_num * 2) - 1);
                    ho_chipping_defect_region.Dispose();
                    HOperatorSet.GenEmptyObj(out ho_chipping_defect_region);
                    ho_aug_ic.Dispose();
                    HOperatorSet.AffineTransRegion(ho_chipping_region, out ho_aug_ic, hv_pcb_hom_mat2d,
                        "nearest_neighbor");
                    HTuple end_val162 = hv_Number;
                    HTuple step_val162 = 1;
                    for (hv_Index = 1; hv_Index.Continue(end_val162, step_val162); hv_Index = hv_Index.TupleAdd(step_val162))
                    {
                        ho_aug_ic_select.Dispose();
                        HOperatorSet.SelectObj(ho_aug_ic, out ho_aug_ic_select, hv_Index);
                        hv_model_id = hv_chipping_model_id.TupleSelect(hv_Index - 1);
                        hv_model_type = hv_chipping_model_type.TupleSelect(hv_Index - 1);
                        ho_aug_ic_refine.Dispose();
                        find_auxiliary_ic(ho_ImageLine, ho_ImagePCB, ho_aug_ic_select, out ho_aug_ic_refine,
                            hv_search_size, hv_model_id, hv_model_type, hv_find_shape_score1, hv_measure_length1,
                            hv_measure_length2, hv_measure_threshold, out hv_iFlag_aux_ic);
                        if ((int)(new HTuple(hv_iFlag_aux_ic.TupleNotEqual(0))) != 0)
                        {
                            {
                                HObject ExpTmpOutVar_0;
                                HOperatorSet.ConcatObj(ho_defect_region, ho_aug_ic_select, out ExpTmpOutVar_0
                                    );
                                ho_defect_region.Dispose();
                                ho_defect_region = ExpTmpOutVar_0;
                            }
                            continue;
                        }
                        ho_DefectRegionsPCB.Dispose();
                        inspect_chipping_threshold(ho_ImagePCB, ho_aug_ic_refine, out ho_DefectRegionsPCB,
                            hv_inspect_size, 40, 8, hv_low_thresh, hv_opening_size, hv_area_thresh,
                            hv_len1_thresh, hv_len2_thresh, hv_select_operation, out hv_iFlag2,
                            out hv_ErrMsg);
                        ho_DefectRegionsLine.Dispose();
                        inspect_chipping_threshold(ho_ImageLine, ho_aug_ic_refine, out ho_DefectRegionsLine,
                            hv_inspect_size, 5, hv_high_thresh+7, 15, hv_opening_size, hv_area_thresh,
                            hv_len1_thresh, hv_len2_thresh, hv_select_operation, out hv_iFlag2,
                            out hv_ErrMsg);
                        ho_DefectRegionsIC.Dispose();
                        inspect_chipping_threshold(ho_ImageIC, ho_aug_ic_refine, out ho_DefectRegionsIC,
                            hv_inspect_size, 10, hv_high_thresh+22, 15, hv_opening_size, hv_area_thresh,
                            hv_len1_thresh, hv_len2_thresh, hv_select_operation, out hv_iFlag2,
                            out hv_ErrMsg);
                        ho_RegionUnion.Dispose();
                        HOperatorSet.Union2(ho_DefectRegionsPCB, ho_DefectRegionsLine, out ho_RegionUnion
                            );
                        {
                            HObject ExpTmpOutVar_0;
                            HOperatorSet.Union2(ho_RegionUnion, ho_DefectRegionsIC, out ExpTmpOutVar_0
                                );
                            ho_RegionUnion.Dispose();
                            ho_RegionUnion = ExpTmpOutVar_0;
                        }
                        ho_DefectRegion.Dispose();
                        HOperatorSet.Union1(ho_RegionUnion, out ho_DefectRegion);
                        {
                            HObject ExpTmpOutVar_0;
                            HOperatorSet.ConcatObj(ho_chipping_defect_region, ho_DefectRegion, out ExpTmpOutVar_0
                                );
                            ho_chipping_defect_region.Dispose();
                            ho_chipping_defect_region = ExpTmpOutVar_0;
                        }
                    }
                    HOperatorSet.CountObj(ho_chipping_defect_region, out hv_Number);
                    if ((int)(hv_Number) != 0)
                    {
                        ho_defect_region_union.Dispose();
                        HOperatorSet.Union1(ho_chipping_defect_region, out ho_defect_region_union
                            );
                        {
                            HObject ExpTmpOutVar_0;
                            HOperatorSet.ConcatObj(ho_defect_region, ho_defect_region_union, out ExpTmpOutVar_0
                                );
                            ho_defect_region.Dispose();
                            ho_defect_region = ExpTmpOutVar_0;
                        }
                        hv_defect_type = hv_defect_type.TupleConcat(7);
                    }
                }

                //******************************************** scratch inspect
                ho_scratch_inspct_region.Dispose();
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    ho_scratch_inspct_region = hvec_vec_model_object[5].O.CopyObj(1, -1);
                }
                ho_RegionAffineTrans.Dispose();
                HOperatorSet.AffineTransRegion(ho_scratch_inspct_region, out ho_RegionAffineTrans,
                    hv_ic_hom_mat2d, "nearest_neighbor");
                HOperatorSet.CountObj(ho_RegionAffineTrans, out hv_Number);
                //params
                hv_scratc_params = hvec_vec_inspect_param[5].T.Clone();
                hv_is_gauss = hv_scratc_params[0];
                hv_line_sigma = hv_scratc_params[1];
                hv_line_low = hv_scratc_params[2];
                hv_line_high = hv_scratc_params[3];
                hv_light_dark = hv_scratc_params[4];
                hv_length_thresh = hv_scratc_params[5];
                hv_mask_size = hv_scratc_params[6];
                hv_sigma_thresh = hv_scratc_params[7];
                hv_gray_thresh = hv_scratc_params[8];
                hv_area_thresh = hv_scratc_params[9];
                hv_len1_thresh = hv_scratc_params[10];
                hv_len2_thresh = hv_scratc_params[11];
                ho_scratch_defect_region.Dispose();
                HOperatorSet.GenEmptyObj(out ho_scratch_defect_region);
                HTuple end_val206 = hv_Number;
                HTuple step_val206 = 1;
                for (hv_Index = 1; hv_Index.Continue(end_val206, step_val206); hv_Index = hv_Index.TupleAdd(step_val206))
                {
                    ho_inspect_region.Dispose();
                    HOperatorSet.SelectObj(ho_RegionAffineTrans, out ho_inspect_region, hv_Index);
                    ho_DefectLinesLine.Dispose();
                    inspect_scratch(ho_ImageLine, ho_inspect_region, out ho_DefectLinesLine,
                        hv_is_gauss, hv_line_sigma, hv_line_low, hv_line_high, hv_light_dark,
                        hv_length_thresh);
                    ho_DefectRegionCFAR.Dispose();
                    inspect_CFAR(ho_ImagePCB, ho_inspect_region, out ho_DefectRegionCFAR, 21,
                        11, 3, (new HTuple(3)).TupleConcat(3), "not_equal", 1);
                    ho_DefectLinesPCB.Dispose();
                    inspect_scratch(ho_ImagePCB, ho_inspect_region, out ho_DefectLinesPCB, hv_is_gauss,
                        hv_line_sigma, hv_line_low, hv_line_high, hv_light_dark, hv_length_thresh);
                    HOperatorSet.CountObj(ho_DefectLinesLine, out hv_line_num);
                    if ((int)(hv_line_num) != 0)
                    {
                        HOperatorSet.SmallestRectangle2Xld(ho_DefectLinesLine, out hv_Row, out hv_Column,
                            out hv_Phi, out hv_Length1, out hv_Length2);
                        ho_Rectangle.Dispose();
                        HOperatorSet.GenRectangle2(out ho_Rectangle, hv_Row, hv_Column, hv_Phi,
                            hv_Length1 + 5, hv_Length2 + 5);
                        {
                            HObject ExpTmpOutVar_0;
                            HOperatorSet.ConcatObj(ho_scratch_defect_region, ho_Rectangle, out ExpTmpOutVar_0
                                );
                            ho_scratch_defect_region.Dispose();
                            ho_scratch_defect_region = ExpTmpOutVar_0;
                        }
                    }
                    HOperatorSet.CountObj(ho_DefectLinesPCB, out hv_line_num);
                    if ((int)(hv_line_num) != 0)
                    {
                        HOperatorSet.SmallestRectangle2Xld(ho_DefectLinesPCB, out hv_Row, out hv_Column,
                            out hv_Phi, out hv_Length1, out hv_Length2);
                        ho_Rectangle.Dispose();
                        HOperatorSet.GenRectangle2(out ho_Rectangle, hv_Row, hv_Column, hv_Phi,
                            hv_Length1 + 5, hv_Length2 + 5);
                        {
                            HObject ExpTmpOutVar_0;
                            HOperatorSet.ConcatObj(ho_scratch_defect_region, ho_Rectangle, out ExpTmpOutVar_0
                                );
                            ho_scratch_defect_region.Dispose();
                            ho_scratch_defect_region = ExpTmpOutVar_0;
                        }
                    }
                    ho_ConnectedRegions.Dispose();
                    HOperatorSet.Connection(ho_DefectRegionCFAR, out ho_ConnectedRegions);
                    ho_SelectedRegions.Dispose();
                    HOperatorSet.SelectShape(ho_ConnectedRegions, out ho_SelectedRegions, "area",
                        "and", 5, 99999);
                    HOperatorSet.CountObj(ho_SelectedRegions, out hv_Number);
                    if ((int)(hv_Number) != 0)
                    {
                        {
                            HObject ExpTmpOutVar_0;
                            HOperatorSet.ConcatObj(ho_scratch_defect_region, ho_SelectedRegions, out ExpTmpOutVar_0
                                );
                            ho_scratch_defect_region.Dispose();
                            ho_scratch_defect_region = ExpTmpOutVar_0;
                        }
                    }
                }
                HOperatorSet.CountObj(ho_scratch_defect_region, out hv_Number);
                if ((int)(hv_Number) != 0)
                {
                    ho_defect_region_union.Dispose();
                    HOperatorSet.Union1(ho_scratch_defect_region, out ho_defect_region_union);
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.ConcatObj(ho_defect_region, ho_defect_region_union, out ExpTmpOutVar_0
                            );
                        ho_defect_region.Dispose();
                        ho_defect_region = ExpTmpOutVar_0;
                    }
                    hv_defect_type = hv_defect_type.TupleConcat(8);
                }
                //******************************************** match inspect
                //model
                ho_match_inspect_region.Dispose();
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    ho_match_inspect_region = hvec_vec_model_object[6].O.CopyObj(1, -1);
                }
                //[modelID1,modelIDn,modeltype1,modeltypen]
                hv_match_model_tuple = hvec_vec_model_tuple[6].T.Clone();
                //几种match
                hv_match_num = (new HTuple(hv_match_model_tuple.TupleLength())) / 4;
                hv_match_model_id = hv_match_model_tuple.TupleSelectRange(0, (hv_match_num * 2) - 1);
                hv_match_model_type = hv_match_model_tuple.TupleSelectRange(hv_match_num * 2,
                    (hv_match_num * 4) - 1);
                //params
                hv_match_params = hvec_vec_inspect_param[6].T.Clone();
                hv_angle_start = hv_match_params[0];
                hv_angle_extent = hv_match_params[1];
                hv_match_thresh = hv_match_params.TupleSelectRange(2, (new HTuple(hv_match_params.TupleLength()
                    )) - 1);
                ho_RegionAffineTrans.Dispose();
                HOperatorSet.AffineTransRegion(ho_match_inspect_region, out ho_RegionAffineTrans,
                    hv_pcb_hom_mat2d, "nearest_neighbor");
                ho_RegionDilation.Dispose();
                HOperatorSet.DilationRectangle1(ho_RegionAffineTrans, out ho_RegionDilation,
                    11, 11);
                //一个区域做两次
                //1/2/6
                ho_match_defect_region.Dispose();
                HOperatorSet.GenEmptyObj(out ho_match_defect_region);
                HTuple end_val255 = hv_match_num - 1;
                HTuple step_val255 = 1;
                for (hv_Index = 0; hv_Index.Continue(end_val255, step_val255); hv_Index = hv_Index.TupleAdd(step_val255))
                {
                    ho_ObjectSelected.Dispose();
                    HOperatorSet.SelectObj(ho_RegionDilation, out ho_ObjectSelected, hv_Index + 1);
                    ho_inspect_region.Dispose();
                    HOperatorSet.Connection(ho_ObjectSelected, out ho_inspect_region);
                    hv_model_id = hv_match_model_id.TupleSelect(hv_Index * 2);
                    hv_model_type = hv_match_model_type.TupleSelect(hv_Index * 2);
                    hv_match_thresh_ = hv_match_thresh.TupleSelect(hv_Index * 2);
                    ho_DefectRegions0.Dispose();
                    inspect_matching(ho_ImagePCB, ho_inspect_region, out ho_DefectRegions0, hv_model_type,
                        hv_model_id, hv_angle_start, hv_angle_extent, hv_match_thresh_, out hv_MatchScore0,
                        out hv_iFlag1, out hv_err_msg1);
                    hv_model_id = hv_match_model_id.TupleSelect((hv_Index * 2) + 1);
                    hv_model_type = hv_match_model_type.TupleSelect((hv_Index * 2) + 1);
                    hv_match_thresh_ = hv_match_thresh.TupleSelect((hv_Index * 2) + 1);
                    ho_DefectRegions1.Dispose();
                    inspect_matching(ho_ImageLine, ho_inspect_region, out ho_DefectRegions1,
                        hv_model_type, hv_model_id, hv_angle_start, hv_angle_extent, hv_match_thresh_,
                        out hv_MatchScore1, out hv_iFlag1, out hv_err_msg1);
                    ho_DefectRegions.Dispose();
                    HOperatorSet.Intersection(ho_DefectRegions0, ho_DefectRegions1, out ho_DefectRegions
                        );
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.ConcatObj(ho_match_defect_region, ho_DefectRegions, out ExpTmpOutVar_0
                            );
                        ho_match_defect_region.Dispose();
                        ho_match_defect_region = ExpTmpOutVar_0;
                    }
                }
                ho_match_defect_region_.Dispose();
                //HOperatorSet.SelectShape(ho_match_defect_region, out ho_match_defect_region_,
                //    "area", "and", 1, 99999);
                //HOperatorSet.CountObj(ho_match_defect_region_, out hv_Number);
                //if ((int)(hv_Number) != 0)
                //{
                //    ho_defect_region_union.Dispose();
                //    HOperatorSet.Union1(ho_match_defect_region_, out ho_defect_region_union);
                //    {
                //        HObject ExpTmpOutVar_0;
                //        HOperatorSet.ConcatObj(ho_defect_region, ho_defect_region_union, out ExpTmpOutVar_0
                //            );
                //        ho_defect_region.Dispose();
                //        ho_defect_region = ExpTmpOutVar_0;
                //    }
                //    hv_defect_type = hv_defect_type.TupleConcat(9);
                //}
                //if ((int)(new HTuple(hv_defect_type.TupleLength())) != 0)
                //{
                //    hv_iFlag = -1;
                //}
                ho_pcb_model_object.Dispose();
                ho_pcb_dark_thresh_image.Dispose();
                ho_pcb_light_thresh_image.Dispose();
                ho_pcb_match_region.Dispose();
                ho_pcb_inspect_region.Dispose();
                ho_pcb_reject_region.Dispose();
                ho_pcb_sub_region.Dispose();
                ho_pcb_failure_regions.Dispose();
                ho_pcb_inspect_region_affine.Dispose();
                ho_defect_region_union.Dispose();
                ho_ic_model_object.Dispose();
                ho_ic_dark_thresh_image.Dispose();
                ho_ic_light_thresh_image.Dispose();
                ho_ic_match_region.Dispose();
                ho_ic_inspect_region.Dispose();
                ho_ic_reject_region.Dispose();
                ho_ic_sub_region.Dispose();
                ho_ic_failure_regions.Dispose();
                ho_ic_inspect_region_affine.Dispose();
                ho_pcb_pad.Dispose();
                ho_ic_pad.Dispose();
                ho_Cross.Dispose();
                ho_pcb_pad_affine.Dispose();
                ho_ic_pad_affine.Dispose();
                ho_wire_defect_region.Dispose();
                ho_chipping_region.Dispose();
                ho_chipping_defect_region.Dispose();
                ho_aug_ic.Dispose();
                ho_aug_ic_select.Dispose();
                ho_aug_ic_refine.Dispose();
                ho_DefectRegionsPCB.Dispose();
                ho_DefectRegionsLine.Dispose();
                ho_DefectRegionsIC.Dispose();
                ho_RegionUnion.Dispose();
                ho_DefectRegion.Dispose();
                ho_scratch_inspct_region.Dispose();
                ho_RegionAffineTrans.Dispose();
                ho_scratch_defect_region.Dispose();
                ho_inspect_region.Dispose();
                ho_DefectLinesLine.Dispose();
                ho_DefectRegionCFAR.Dispose();
                ho_DefectLinesPCB.Dispose();
                ho_Rectangle.Dispose();
                ho_ConnectedRegions.Dispose();
                ho_SelectedRegions.Dispose();
                ho_match_inspect_region.Dispose();
                ho_RegionDilation.Dispose();
                ho_match_defect_region.Dispose();
                ho_ObjectSelected.Dispose();
                ho_DefectRegions0.Dispose();
                ho_DefectRegions1.Dispose();
                ho_DefectRegions.Dispose();
                ho_match_defect_region_.Dispose();

                return;
            }
            catch (HalconException HDevExpDefaultException)
            {
                ho_pcb_model_object.Dispose();
                ho_pcb_dark_thresh_image.Dispose();
                ho_pcb_light_thresh_image.Dispose();
                ho_pcb_match_region.Dispose();
                ho_pcb_inspect_region.Dispose();
                ho_pcb_reject_region.Dispose();
                ho_pcb_sub_region.Dispose();
                ho_pcb_failure_regions.Dispose();
                ho_pcb_inspect_region_affine.Dispose();
                ho_defect_region_union.Dispose();
                ho_ic_model_object.Dispose();
                ho_ic_dark_thresh_image.Dispose();
                ho_ic_light_thresh_image.Dispose();
                ho_ic_match_region.Dispose();
                ho_ic_inspect_region.Dispose();
                ho_ic_reject_region.Dispose();
                ho_ic_sub_region.Dispose();
                ho_ic_failure_regions.Dispose();
                ho_ic_inspect_region_affine.Dispose();
                ho_pcb_pad.Dispose();
                ho_ic_pad.Dispose();
                ho_Cross.Dispose();
                ho_pcb_pad_affine.Dispose();
                ho_ic_pad_affine.Dispose();
                ho_wire_defect_region.Dispose();
                ho_chipping_region.Dispose();
                ho_chipping_defect_region.Dispose();
                ho_aug_ic.Dispose();
                ho_aug_ic_select.Dispose();
                ho_aug_ic_refine.Dispose();
                ho_DefectRegionsPCB.Dispose();
                ho_DefectRegionsLine.Dispose();
                ho_DefectRegionsIC.Dispose();
                ho_RegionUnion.Dispose();
                ho_DefectRegion.Dispose();
                ho_scratch_inspct_region.Dispose();
                ho_RegionAffineTrans.Dispose();
                ho_scratch_defect_region.Dispose();
                ho_inspect_region.Dispose();
                ho_DefectLinesLine.Dispose();
                ho_DefectRegionCFAR.Dispose();
                ho_DefectLinesPCB.Dispose();
                ho_Rectangle.Dispose();
                ho_ConnectedRegions.Dispose();
                ho_SelectedRegions.Dispose();
                ho_match_inspect_region.Dispose();
                ho_RegionDilation.Dispose();
                ho_match_defect_region.Dispose();
                ho_ObjectSelected.Dispose();
                ho_DefectRegions0.Dispose();
                ho_DefectRegions1.Dispose();
                ho_DefectRegions.Dispose();
                ho_match_defect_region_.Dispose();

                throw HDevExpDefaultException;
            }
        }

        public void inspect_golden_model(HObject ho_Image, HObject ho_dark_thresh_image,
            HObject ho_light_thresh_image, HObject ho_match_region, HObject ho_inspect_region,
            HObject ho_reject_region, HObject ho_sub_region, out HObject ho_failure_regions,
            out HObject ho_inspect_region_affine, HTuple hv_ModelID, HTuple hv_model_type,
            HTuple hv_score_thresh, HTuple hv_angle_start, HTuple hv_angle_extent, HTuple hv_sub_reg_num,
            HTuple hv_select_operation, HTuple hv_width_thresh, HTuple hv_height_thresh,
            HTuple hv_area_thresh, HTuple hv_closing_size, HTuple hv_match_dilation_size,
            out HTuple hv_hom_temp2image, out HTuple hv_row, out HTuple hv_col, out HTuple hv_angle,
            out HTuple hv_iFlag, out HTuple hv_ErrMsg)
        {




            // Stack for temporary objects 
            HObject[] OTemp = new HObject[20];

            // Local iconic variables 

            HObject ho_MatchRegionDilation = null, ho_ImageAffinTrans = null;
            HObject ho_Region_dark = null, ho_Region_light = null, ho_RegionUnion = null;
            HObject ho_RegionDifference = null, ho_RegionClosing = null;
            HObject ho_ConnectedRegions = null, ho_SelectedRegions = null;
            HObject ho_RegionClosing1 = null, ho_ConnectedRegions1 = null;
            HObject ho_EmptyRegion = null, ho_ObjectSelected = null, ho_RegionReduced = null;
            HObject ho_ObjectSelected1 = null, ho_RegionIntersection = null;
            HObject ho_SelectedRegions_ = null, ho_RegionAffineTrans = null;
            HObject ho__failure_regions = null;

            // Local control variables 

            HTuple hv_Number = new HTuple(), hv_Index = new HTuple();
            HTuple hv_Area1 = new HTuple(), hv_Row1 = new HTuple();
            HTuple hv_Column1 = new HTuple(), hv_Index1 = new HTuple();
            HTuple hv_Area = new HTuple(), hv_Row = new HTuple(), hv_Column = new HTuple();
            HTuple hv_Exception = null;
            HTuple hv_height_thresh_COPY_INP_TMP = hv_height_thresh.Clone();
            HTuple hv_width_thresh_COPY_INP_TMP = hv_width_thresh.Clone();

            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_failure_regions);
            HOperatorSet.GenEmptyObj(out ho_inspect_region_affine);
            HOperatorSet.GenEmptyObj(out ho_MatchRegionDilation);
            HOperatorSet.GenEmptyObj(out ho_ImageAffinTrans);
            HOperatorSet.GenEmptyObj(out ho_Region_dark);
            HOperatorSet.GenEmptyObj(out ho_Region_light);
            HOperatorSet.GenEmptyObj(out ho_RegionUnion);
            HOperatorSet.GenEmptyObj(out ho_RegionDifference);
            HOperatorSet.GenEmptyObj(out ho_RegionClosing);
            HOperatorSet.GenEmptyObj(out ho_ConnectedRegions);
            HOperatorSet.GenEmptyObj(out ho_SelectedRegions);
            HOperatorSet.GenEmptyObj(out ho_RegionClosing1);
            HOperatorSet.GenEmptyObj(out ho_ConnectedRegions1);
            HOperatorSet.GenEmptyObj(out ho_EmptyRegion);
            HOperatorSet.GenEmptyObj(out ho_ObjectSelected);
            HOperatorSet.GenEmptyObj(out ho_RegionReduced);
            HOperatorSet.GenEmptyObj(out ho_ObjectSelected1);
            HOperatorSet.GenEmptyObj(out ho_RegionIntersection);
            HOperatorSet.GenEmptyObj(out ho_SelectedRegions_);
            HOperatorSet.GenEmptyObj(out ho_RegionAffineTrans);
            HOperatorSet.GenEmptyObj(out ho__failure_regions);
            hv_hom_temp2image = new HTuple();
            hv_row = new HTuple();
            hv_col = new HTuple();
            hv_angle = new HTuple();
            try
            {
                //*************************************************************************************
                //       align Image to mean_image, and inspect the Image, get the failure_regions
                //       in:  Image, match_regions, mean_image, std_image, common_region, reject_region
                //            ModelID, model_type, score_thresh, angle_start, angle_extent, thresh_sigma
                //       out: failure_region, inspect_region, iFlag
                //       pre modification:  yongbang zhou, @15/12/2016
                //       pre modification:  yongbang zhou, @20/12/2016, thresh_image
                //       last modification: yongbang zhou, @01/03/2017, multiple regions and parameters
                //       next modification: delete parameters: dark_thresh, light_thesh
                //************************************************************************************
                hv_iFlag = 0;
                hv_ErrMsg = "";
                ho_failure_regions.Dispose();
                HOperatorSet.GenEmptyObj(out ho_failure_regions);
                try
                {
                    //align image
                    ho_MatchRegionDilation.Dispose();
                    match_region_dilation(ho_match_region, out ho_MatchRegionDilation, hv_match_dilation_size);
                    ho_ImageAffinTrans.Dispose();
                    align_image(ho_Image, ho_MatchRegionDilation, out ho_ImageAffinTrans, hv_model_type,
                        hv_ModelID, hv_angle_start, hv_angle_extent, hv_score_thresh, out hv_iFlag,
                        out hv_hom_temp2image, out hv_row, out hv_col, out hv_angle);

                    //inspect image
                    if ((int)(new HTuple(hv_iFlag.TupleEqual(-1))) != 0)
                    {
                        ho_failure_regions.Dispose();
                        HOperatorSet.CopyObj(ho_MatchRegionDilation, out ho_failure_regions, 1,
                            -1);
                        hv_ErrMsg = "match failed";
                        ho_MatchRegionDilation.Dispose();
                        ho_ImageAffinTrans.Dispose();
                        ho_Region_dark.Dispose();
                        ho_Region_light.Dispose();
                        ho_RegionUnion.Dispose();
                        ho_RegionDifference.Dispose();
                        ho_RegionClosing.Dispose();
                        ho_ConnectedRegions.Dispose();
                        ho_SelectedRegions.Dispose();
                        ho_RegionClosing1.Dispose();
                        ho_ConnectedRegions1.Dispose();
                        ho_EmptyRegion.Dispose();
                        ho_ObjectSelected.Dispose();
                        ho_RegionReduced.Dispose();
                        ho_ObjectSelected1.Dispose();
                        ho_RegionIntersection.Dispose();
                        ho_SelectedRegions_.Dispose();
                        ho_RegionAffineTrans.Dispose();
                        ho__failure_regions.Dispose();

                        return;
                    }
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.ReduceDomain(ho_ImageAffinTrans, ho_inspect_region, out ExpTmpOutVar_0
                            );
                        ho_ImageAffinTrans.Dispose();
                        ho_ImageAffinTrans = ExpTmpOutVar_0;
                    }
                    ho_inspect_region_affine.Dispose();
                    HOperatorSet.AffineTransRegion(ho_inspect_region, out ho_inspect_region_affine,
                        hv_hom_temp2image, "nearest_neighbor");
                    ho_Region_dark.Dispose();
                    HOperatorSet.DynThreshold(ho_ImageAffinTrans, ho_dark_thresh_image, out ho_Region_dark,
                        1, "dark");
                    ho_Region_light.Dispose();
                    HOperatorSet.DynThreshold(ho_ImageAffinTrans, ho_light_thresh_image, out ho_Region_light,
                        1, "light");
                    ho_RegionUnion.Dispose();
                    HOperatorSet.Union2(ho_Region_dark, ho_Region_light, out ho_RegionUnion);
                    ho_RegionDifference.Dispose();
                    HOperatorSet.Difference(ho_RegionUnion, ho_reject_region, out ho_RegionDifference
                        );
                    hv_width_thresh_COPY_INP_TMP = hv_width_thresh_COPY_INP_TMP * 0.5;
                    hv_height_thresh_COPY_INP_TMP = hv_height_thresh_COPY_INP_TMP * 0.5;
                    if ((int)(new HTuple(hv_sub_reg_num.TupleEqual(0))) != 0)
                    {
                        ho_RegionClosing.Dispose();
                        HOperatorSet.ClosingRectangle1(ho_RegionDifference, out ho_RegionClosing,
                            hv_closing_size, hv_closing_size);
                        ho_ConnectedRegions.Dispose();
                        HOperatorSet.Connection(ho_RegionClosing, out ho_ConnectedRegions);
                        ho_SelectedRegions.Dispose();
                        HOperatorSet.SelectShape(ho_ConnectedRegions, out ho_SelectedRegions, (
                            (new HTuple("rect2_len1")).TupleConcat("rect2_len2")).TupleConcat("area"),
                            hv_select_operation, ((hv_width_thresh_COPY_INP_TMP.TupleConcat(hv_height_thresh_COPY_INP_TMP))).TupleConcat(
                            hv_area_thresh), ((new HTuple(999999)).TupleConcat(999999)).TupleConcat(
                            9999999));
                        ho_failure_regions.Dispose();
                        HOperatorSet.AffineTransRegion(ho_SelectedRegions, out ho_failure_regions,
                            hv_hom_temp2image, "nearest_neighbor");
                    }
                    else
                    {
                        ho_RegionClosing1.Dispose();
                        HOperatorSet.ClosingRectangle1(ho_RegionDifference, out ho_RegionClosing1,
                            hv_closing_size.TupleMin(), hv_closing_size.TupleMin());
                        ho_ConnectedRegions1.Dispose();
                        HOperatorSet.Connection(ho_RegionClosing1, out ho_ConnectedRegions1);
                        ho_SelectedRegions.Dispose();
                        HOperatorSet.SelectShape(ho_ConnectedRegions1, out ho_SelectedRegions,
                            ((new HTuple("rect2_len1")).TupleConcat("rect2_len2")).TupleConcat(
                            "area"), hv_select_operation, ((((hv_width_thresh_COPY_INP_TMP.TupleMin()
                            )).TupleConcat(hv_height_thresh_COPY_INP_TMP.TupleMin()))).TupleConcat(
                            hv_area_thresh.TupleMin()), ((new HTuple(999999)).TupleConcat(999999)).TupleConcat(
                            9999999));
                        HOperatorSet.CountObj(ho_SelectedRegions, out hv_Number);
                        ho_EmptyRegion.Dispose();
                        HOperatorSet.GenEmptyRegion(out ho_EmptyRegion);
                        HTuple end_val43 = hv_sub_reg_num;
                        HTuple step_val43 = 1;
                        for (hv_Index = 0; hv_Index.Continue(end_val43, step_val43); hv_Index = hv_Index.TupleAdd(step_val43))
                        {
                            ho_ObjectSelected.Dispose();
                            HOperatorSet.SelectObj(ho_sub_region, out ho_ObjectSelected, hv_Index + 1);
                            HOperatorSet.AreaCenter(ho_ObjectSelected, out hv_Area1, out hv_Row1,
                                out hv_Column1);
                            if ((int)(new HTuple(hv_Area1.TupleLess(1))) != 0)
                            {
                                {
                                    HObject ExpTmpOutVar_0;
                                    HOperatorSet.ConcatObj(ho_failure_regions, ho_EmptyRegion, out ExpTmpOutVar_0
                                        );
                                    ho_failure_regions.Dispose();
                                    ho_failure_regions = ExpTmpOutVar_0;
                                }
                                continue;
                            }
                            ho_RegionReduced.Dispose();
                            HOperatorSet.GenEmptyObj(out ho_RegionReduced);
                            HTuple end_val51 = hv_Number;
                            HTuple step_val51 = 1;
                            for (hv_Index1 = 1; hv_Index1.Continue(end_val51, step_val51); hv_Index1 = hv_Index1.TupleAdd(step_val51))
                            {
                                ho_ObjectSelected1.Dispose();
                                HOperatorSet.SelectObj(ho_SelectedRegions, out ho_ObjectSelected1,
                                    hv_Index1);
                                ho_RegionIntersection.Dispose();
                                HOperatorSet.Intersection(ho_ObjectSelected, ho_ObjectSelected1, out ho_RegionIntersection
                                    );
                                HOperatorSet.AreaCenter(ho_RegionIntersection, out hv_Area, out hv_Row,
                                    out hv_Column);
                                if ((int)(new HTuple(hv_Area.TupleGreater(0))) != 0)
                                {
                                    {
                                        HObject ExpTmpOutVar_0;
                                        HOperatorSet.Union2(ho_RegionReduced, ho_ObjectSelected1, out ExpTmpOutVar_0
                                            );
                                        ho_RegionReduced.Dispose();
                                        ho_RegionReduced = ExpTmpOutVar_0;
                                    }
                                }
                            }
                            ho_RegionClosing.Dispose();
                            HOperatorSet.ClosingRectangle1(ho_RegionReduced, out ho_RegionClosing,
                                hv_closing_size.TupleSelect(hv_Index), hv_closing_size.TupleSelect(
                                hv_Index));
                            ho_ConnectedRegions.Dispose();
                            HOperatorSet.Connection(ho_RegionClosing, out ho_ConnectedRegions);
                            ho_SelectedRegions_.Dispose();
                            HOperatorSet.SelectShape(ho_ConnectedRegions, out ho_SelectedRegions_,
                                ((new HTuple("rect2_len1")).TupleConcat("rect2_len2")).TupleConcat(
                                "area"), hv_select_operation, ((((hv_width_thresh_COPY_INP_TMP.TupleSelect(
                                hv_Index))).TupleConcat(hv_height_thresh_COPY_INP_TMP.TupleSelect(
                                hv_Index)))).TupleConcat(hv_area_thresh.TupleSelect(hv_Index)), (
                                (new HTuple(999999)).TupleConcat(999999)).TupleConcat(999999));
                            ho_RegionAffineTrans.Dispose();
                            HOperatorSet.AffineTransRegion(ho_SelectedRegions_, out ho_RegionAffineTrans,
                                hv_hom_temp2image, "nearest_neighbor");
                            ho__failure_regions.Dispose();
                            HOperatorSet.Union2(ho_RegionAffineTrans, ho_EmptyRegion, out ho__failure_regions
                                );
                            {
                                HObject ExpTmpOutVar_0;
                                HOperatorSet.ConcatObj(ho_failure_regions, ho__failure_regions, out ExpTmpOutVar_0
                                    );
                                ho_failure_regions.Dispose();
                                ho_failure_regions = ExpTmpOutVar_0;
                            }
                        }
                    }
                }
                // catch (Exception) 
                catch (HalconException HDevExpDefaultException1)
                {
                    HDevExpDefaultException1.ToHTuple(out hv_Exception);
                    hv_iFlag = -1;
                    ho_failure_regions.Dispose();
                    HOperatorSet.CopyObj(ho_MatchRegionDilation, out ho_failure_regions, 1, -1);
                    hv_ErrMsg = "other halcon exception";
                }
                ho_MatchRegionDilation.Dispose();
                ho_ImageAffinTrans.Dispose();
                ho_Region_dark.Dispose();
                ho_Region_light.Dispose();
                ho_RegionUnion.Dispose();
                ho_RegionDifference.Dispose();
                ho_RegionClosing.Dispose();
                ho_ConnectedRegions.Dispose();
                ho_SelectedRegions.Dispose();
                ho_RegionClosing1.Dispose();
                ho_ConnectedRegions1.Dispose();
                ho_EmptyRegion.Dispose();
                ho_ObjectSelected.Dispose();
                ho_RegionReduced.Dispose();
                ho_ObjectSelected1.Dispose();
                ho_RegionIntersection.Dispose();
                ho_SelectedRegions_.Dispose();
                ho_RegionAffineTrans.Dispose();
                ho__failure_regions.Dispose();

                return;
            }
            catch (HalconException HDevExpDefaultException)
            {
                ho_MatchRegionDilation.Dispose();
                ho_ImageAffinTrans.Dispose();
                ho_Region_dark.Dispose();
                ho_Region_light.Dispose();
                ho_RegionUnion.Dispose();
                ho_RegionDifference.Dispose();
                ho_RegionClosing.Dispose();
                ho_ConnectedRegions.Dispose();
                ho_SelectedRegions.Dispose();
                ho_RegionClosing1.Dispose();
                ho_ConnectedRegions1.Dispose();
                ho_EmptyRegion.Dispose();
                ho_ObjectSelected.Dispose();
                ho_RegionReduced.Dispose();
                ho_ObjectSelected1.Dispose();
                ho_RegionIntersection.Dispose();
                ho_SelectedRegions_.Dispose();
                ho_RegionAffineTrans.Dispose();
                ho__failure_regions.Dispose();

                throw HDevExpDefaultException;
            }
        }

        public void load_golden_model(out HObject ho_mean_image, out HObject ho_std_image,
            out HObject ho_match_region, out HObject ho_inspect_region, out HObject ho_reject_region,
            out HObject ho_sub_region, out HObject ho_dark_thresh_image, out HObject ho_light_thresh_image,
            HTuple hv_folder_name, HTuple hv_sub_reg_num, HTuple hv_sobel_scale, HTuple hv_thresh_dark,
            HTuple hv_thresh_light, out HTuple hv_ModelID, out HTuple hv_model_type, out HTuple hv_iFlag)
        {



            // Stack for temporary objects 
            HObject[] OTemp = new HObject[20];

            // Local iconic variables 

            HObject ho_show_contour, ho_sub_insp_reg = null;
            HObject ho_RegionDifference, ho_RegionUnion;

            // Local control variables 

            HTuple hv_def_row = null, hv_def_col = null;
            HTuple hv_FileExists = null, hv_Number = null, hv_Index = null;
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_mean_image);
            HOperatorSet.GenEmptyObj(out ho_std_image);
            HOperatorSet.GenEmptyObj(out ho_match_region);
            HOperatorSet.GenEmptyObj(out ho_inspect_region);
            HOperatorSet.GenEmptyObj(out ho_reject_region);
            HOperatorSet.GenEmptyObj(out ho_sub_region);
            HOperatorSet.GenEmptyObj(out ho_dark_thresh_image);
            HOperatorSet.GenEmptyObj(out ho_light_thresh_image);
            HOperatorSet.GenEmptyObj(out ho_show_contour);
            HOperatorSet.GenEmptyObj(out ho_sub_insp_reg);
            HOperatorSet.GenEmptyObj(out ho_RegionDifference);
            HOperatorSet.GenEmptyObj(out ho_RegionUnion);
            try
            {
                hv_iFlag = 0;
                hv_ModelID = new HTuple();
                hv_model_type = new HTuple();
                ho_std_image.Dispose();
                HOperatorSet.GenEmptyObj(out ho_std_image);
                ho_mean_image.Dispose();
                HOperatorSet.GenEmptyObj(out ho_mean_image);
                ho_match_region.Dispose();
                HOperatorSet.GenEmptyObj(out ho_match_region);
                ho_inspect_region.Dispose();
                HOperatorSet.GenEmptyObj(out ho_inspect_region);
                ho_reject_region.Dispose();
                HOperatorSet.GenEmptyObj(out ho_reject_region);
                ho_sub_region.Dispose();
                HOperatorSet.GenEmptyObj(out ho_sub_region);
                ho_show_contour.Dispose();
                read_model(out ho_show_contour, hv_folder_name, out hv_model_type, out hv_ModelID,
                    out hv_def_row, out hv_def_col, out hv_iFlag);
                if ((int)(new HTuple(hv_iFlag.TupleEqual(-1))) != 0)
                {
                    ho_show_contour.Dispose();
                    ho_sub_insp_reg.Dispose();
                    ho_RegionDifference.Dispose();
                    ho_RegionUnion.Dispose();

                    return;
                }
                HOperatorSet.FileExists(hv_folder_name + "/std_image.ima", out hv_FileExists);
                if ((int)(hv_FileExists) != 0)
                {
                    ho_std_image.Dispose();
                    HOperatorSet.ReadImage(out ho_std_image, hv_folder_name + "/std_image.ima");
                }
                HOperatorSet.CountObj(ho_std_image, out hv_Number);
                if ((int)(new HTuple(hv_Number.TupleEqual(0))) != 0)
                {
                    hv_iFlag = -1;
                    ho_show_contour.Dispose();
                    ho_sub_insp_reg.Dispose();
                    ho_RegionDifference.Dispose();
                    ho_RegionUnion.Dispose();

                    return;
                }
                HOperatorSet.FileExists(hv_folder_name + "/mean_image.ima", out hv_FileExists);
                if ((int)(hv_FileExists) != 0)
                {
                    ho_mean_image.Dispose();
                    HOperatorSet.ReadImage(out ho_mean_image, hv_folder_name + "/mean_image.ima");
                }
                HOperatorSet.CountObj(ho_mean_image, out hv_Number);
                if ((int)(new HTuple(hv_Number.TupleEqual(0))) != 0)
                {
                    hv_iFlag = -1;
                    ho_show_contour.Dispose();
                    ho_sub_insp_reg.Dispose();
                    ho_RegionDifference.Dispose();
                    ho_RegionUnion.Dispose();

                    return;
                }
                HOperatorSet.FileExists(hv_folder_name + "/match_region.reg", out hv_FileExists);
                if ((int)(hv_FileExists) != 0)
                {
                    ho_match_region.Dispose();
                    HOperatorSet.ReadRegion(out ho_match_region, hv_folder_name + "/match_region.reg");
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.Union1(ho_match_region, out ExpTmpOutVar_0);
                        ho_match_region.Dispose();
                        ho_match_region = ExpTmpOutVar_0;
                    }
                }
                HOperatorSet.CountObj(ho_match_region, out hv_Number);
                if ((int)(new HTuple(hv_Number.TupleEqual(0))) != 0)
                {
                    hv_iFlag = -1;
                    ho_show_contour.Dispose();
                    ho_sub_insp_reg.Dispose();
                    ho_RegionDifference.Dispose();
                    ho_RegionUnion.Dispose();

                    return;
                }
                HOperatorSet.FileExists(hv_folder_name + "/inspect_region.reg", out hv_FileExists);
                if ((int)(hv_FileExists) != 0)
                {
                    ho_inspect_region.Dispose();
                    HOperatorSet.ReadRegion(out ho_inspect_region, hv_folder_name + "/inspect_region.reg");
                }
                HOperatorSet.CountObj(ho_inspect_region, out hv_Number);
                if ((int)(new HTuple(hv_Number.TupleEqual(0))) != 0)
                {
                    hv_iFlag = -1;
                    ho_show_contour.Dispose();
                    ho_sub_insp_reg.Dispose();
                    ho_RegionDifference.Dispose();
                    ho_RegionUnion.Dispose();

                    return;
                }
                HOperatorSet.FileExists(hv_folder_name + "/reject_region.reg", out hv_FileExists);
                if ((int)(hv_FileExists) != 0)
                {
                    ho_reject_region.Dispose();
                    HOperatorSet.ReadRegion(out ho_reject_region, hv_folder_name + "/reject_region.reg");
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.Union1(ho_reject_region, out ExpTmpOutVar_0);
                        ho_reject_region.Dispose();
                        ho_reject_region = ExpTmpOutVar_0;
                    }
                }
                HOperatorSet.CountObj(ho_reject_region, out hv_Number);
                if ((int)(new HTuple(hv_Number.TupleEqual(0))) != 0)
                {
                    ho_reject_region.Dispose();
                    HOperatorSet.GenEmptyRegion(out ho_reject_region);
                }
                HTuple end_val59 = hv_sub_reg_num - 1;
                HTuple step_val59 = 1;
                for (hv_Index = 0; hv_Index.Continue(end_val59, step_val59); hv_Index = hv_Index.TupleAdd(step_val59))
                {
                    HOperatorSet.FileExists(((hv_folder_name + "/sub_region_") + hv_Index) + ".reg",
                        out hv_FileExists);
                    if ((int)(hv_FileExists) != 0)
                    {
                        ho_sub_insp_reg.Dispose();
                        HOperatorSet.ReadRegion(out ho_sub_insp_reg, ((hv_folder_name + "/sub_region_") + hv_Index) + ".reg");
                        {
                            HObject ExpTmpOutVar_0;
                            HOperatorSet.Union1(ho_sub_insp_reg, out ExpTmpOutVar_0);
                            ho_sub_insp_reg.Dispose();
                            ho_sub_insp_reg = ExpTmpOutVar_0;
                        }
                    }
                    else
                    {
                        hv_iFlag = -1;
                        ho_show_contour.Dispose();
                        ho_sub_insp_reg.Dispose();
                        ho_RegionDifference.Dispose();
                        ho_RegionUnion.Dispose();

                        return;
                    }
                    HOperatorSet.CountObj(ho_sub_insp_reg, out hv_Number);
                    if ((int)(new HTuple(hv_Number.TupleEqual(0))) != 0)
                    {
                        ho_sub_insp_reg.Dispose();
                        HOperatorSet.GenEmptyRegion(out ho_sub_insp_reg);
                    }
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.ConcatObj(ho_sub_region, ho_sub_insp_reg, out ExpTmpOutVar_0
                            );
                        ho_sub_region.Dispose();
                        ho_sub_region = ExpTmpOutVar_0;
                    }
                }
                ho_RegionDifference.Dispose();
                HOperatorSet.Difference(ho_inspect_region, ho_sub_region, out ho_RegionDifference
                    );
                ho_RegionUnion.Dispose();
                HOperatorSet.Union1(ho_RegionDifference, out ho_RegionUnion);
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.ConcatObj(ho_RegionUnion, ho_sub_region, out ExpTmpOutVar_0);
                    ho_sub_region.Dispose();
                    ho_sub_region = ExpTmpOutVar_0;
                }
                //gen thresh image
                ho_dark_thresh_image.Dispose(); ho_light_thresh_image.Dispose();
                gen_thresh_image(ho_mean_image, ho_std_image, ho_sub_region, out ho_dark_thresh_image,
                    out ho_light_thresh_image, hv_sub_reg_num, hv_thresh_dark, hv_thresh_light,
                    hv_sobel_scale, out hv_iFlag);
                ho_show_contour.Dispose();
                ho_sub_insp_reg.Dispose();
                ho_RegionDifference.Dispose();
                ho_RegionUnion.Dispose();

                return;
            }
            catch (HalconException HDevExpDefaultException)
            {
                ho_show_contour.Dispose();
                ho_sub_insp_reg.Dispose();
                ho_RegionDifference.Dispose();
                ho_RegionUnion.Dispose();

                throw HDevExpDefaultException;
            }
        }

        public void match_region_dilation(HObject ho_match_region, out HObject ho_RegionDilation,
            HTuple hv_match_dilation_size)
        {




            // Local iconic variables 

            HObject ho_RegionUnion, ho_RegionTrans;
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_RegionDilation);
            HOperatorSet.GenEmptyObj(out ho_RegionUnion);
            HOperatorSet.GenEmptyObj(out ho_RegionTrans);
            try
            {
                ho_RegionUnion.Dispose();
                HOperatorSet.Union1(ho_match_region, out ho_RegionUnion);
                ho_RegionTrans.Dispose();
                HOperatorSet.ShapeTrans(ho_RegionUnion, out ho_RegionTrans, "rectangle1");
                ho_RegionDilation.Dispose();
                HOperatorSet.DilationRectangle1(ho_RegionTrans, out ho_RegionDilation, hv_match_dilation_size,
                    hv_match_dilation_size);
                ho_RegionUnion.Dispose();
                ho_RegionTrans.Dispose();

                return;
            }
            catch (HalconException HDevExpDefaultException)
            {
                ho_RegionUnion.Dispose();
                ho_RegionTrans.Dispose();

                throw HDevExpDefaultException;
            }
        }

        public void distance_cc_angle(HObject ho_Seg1, HObject ho_Seg2, HTuple hv_Angle,
            HTuple hv_row0, HTuple hv_col0, HTuple hv_row1, HTuple hv_col1, out HTuple hv_Distance)
        {




            // Local iconic variables 

            // Local control variables 

            HTuple hv_Row = null, hv_Col = null, hv_idxEnd0 = null;
            HTuple hv_Row1 = new HTuple(), hv_Col1 = new HTuple();
            HTuple hv_Angle2 = new HTuple(), hv_idxEnd1 = null, hv_Row2 = new HTuple();
            HTuple hv_Col2 = new HTuple(), hv_dAll = null, hv_dMin = null;
            HTuple hv_AngleDist_ = null, hv_Index = null, hv_angle_ = null;
            HTuple hv_AngleDist = null, hv_Angle1 = new HTuple(), hv_AngleAbs = null;
            HTuple hv_AngleMax = null, hv_angle_thresh = new HTuple();
            // Initialize local and output iconic variables 
            //xubo
            HOperatorSet.GetContourXld(ho_Seg1, out hv_Row, out hv_Col);
            hv_idxEnd0 = (new HTuple(hv_Row.TupleLength())) - 1;
            if ((int)(new HTuple(hv_idxEnd0.TupleEqual(1))) != 0)
            {
                HOperatorSet.TupleSelect(hv_Row, (new HTuple(0)).TupleConcat(hv_idxEnd0), out hv_Row1);
                HOperatorSet.TupleSelect(hv_Col, (new HTuple(0)).TupleConcat(hv_idxEnd0), out hv_Col1);
            }
            else
            {
                HOperatorSet.AngleLl(hv_Row.TupleSelect(0), hv_Col.TupleSelect(0), hv_Row.TupleSelect(
                    hv_idxEnd0), hv_Col.TupleSelect(hv_idxEnd0), hv_row0, hv_col0, hv_row1,
                    hv_col1, out hv_Angle2);
                if ((int)(new HTuple(((hv_Angle2.TupleAbs())).TupleLess(1))) != 0)
                {
                    HOperatorSet.TupleSelect(hv_Row, (new HTuple(0)).TupleConcat(hv_idxEnd0),
                        out hv_Row1);
                    HOperatorSet.TupleSelect(hv_Col, (new HTuple(0)).TupleConcat(hv_idxEnd0),
                        out hv_Col1);
                }
                else
                {
                    HOperatorSet.TupleSelect(hv_Row, hv_idxEnd0.TupleConcat(0), out hv_Row1);
                    HOperatorSet.TupleSelect(hv_Col, hv_idxEnd0.TupleConcat(0), out hv_Col1);
                }

            }
            HOperatorSet.GetContourXld(ho_Seg2, out hv_Row, out hv_Col);
            hv_idxEnd1 = (new HTuple(hv_Row.TupleLength())) - 1;
            if ((int)(new HTuple(hv_idxEnd1.TupleEqual(1))) != 0)
            {
                HOperatorSet.TupleSelect(hv_Row, (new HTuple(0)).TupleConcat(hv_idxEnd1), out hv_Row2);
                HOperatorSet.TupleSelect(hv_Col, (new HTuple(0)).TupleConcat(hv_idxEnd1), out hv_Col2);
            }
            else
            {
                HOperatorSet.AngleLl(hv_Row.TupleSelect(0), hv_Col.TupleSelect(0), hv_Row.TupleSelect(
                    hv_idxEnd1), hv_Col.TupleSelect(hv_idxEnd1), hv_row0, hv_col0, hv_row1,
                    hv_col1, out hv_Angle2);
                if ((int)(new HTuple(((hv_Angle2.TupleAbs())).TupleLess(1))) != 0)
                {
                    HOperatorSet.TupleSelect(hv_Row, (new HTuple(0)).TupleConcat(hv_idxEnd1),
                        out hv_Row2);
                    HOperatorSet.TupleSelect(hv_Col, (new HTuple(0)).TupleConcat(hv_idxEnd1),
                        out hv_Col2);
                }
                else
                {
                    HOperatorSet.TupleSelect(hv_Row, hv_idxEnd1.TupleConcat(0), out hv_Row2);
                    HOperatorSet.TupleSelect(hv_Col, hv_idxEnd1.TupleConcat(0), out hv_Col2);
                }
            }
            HOperatorSet.DistancePp(hv_Row1.TupleSelect((new HTuple(1)).TupleConcat(0)),
                hv_Col1.TupleSelect((new HTuple(1)).TupleConcat(0)), hv_Row2.TupleSelect(
                (new HTuple(0)).TupleConcat(1)), hv_Col2.TupleSelect((new HTuple(0)).TupleConcat(
                1)), out hv_dAll);
            HOperatorSet.TupleMin(hv_dAll, out hv_dMin);
            if ((int)(new HTuple(hv_dMin.TupleLess(1))) != 0)
            {
                hv_AngleDist_ = 0;
            }
            HOperatorSet.TupleFindFirst(hv_dAll, hv_dMin, out hv_Index);
            if ((int)(hv_Index) != 0)
            {
                hv_Row1 = hv_Row1.TupleInverse();
                hv_Col1 = hv_Col1.TupleInverse();
                hv_Row2 = hv_Row2.TupleInverse();
                hv_Col2 = hv_Col2.TupleInverse();
            }
            //gen_cross_contour_xld (Cross, Row1[1], Col1[1], 6, 0.785398)
            //gen_cross_contour_xld (Cross1, Row2[0], Col2[0], 6, 0.785398)
            HOperatorSet.TupleAtan2(-((hv_Row1.TupleSelect(1)) - (hv_Row2.TupleSelect(0))),
                (hv_Col1.TupleSelect(1)) - (hv_Col2.TupleSelect(0)), out hv_angle_);
            if ((int)(new HTuple(hv_angle_.TupleLess(0))) != 0)
            {
                hv_angle_ = ((new HTuple(180)).TupleRad()) + hv_angle_;
            }
            hv_AngleDist = ((hv_angle_ - hv_Angle)).TupleAbs();
            HOperatorSet.TupleMin(hv_AngleDist.TupleConcat(((new HTuple(180)).TupleRad()) - hv_AngleDist),
                out hv_AngleDist_);
            if ((int)(new HTuple(hv_dMin.TupleLess(5))) != 0)
            {
                hv_AngleDist_ = 0;
            }
            //distance_pp (Row1[Index], Col1[Index], Row2[Index], Col2[Index], Distance1)
            if ((int)(new HTuple(hv_dMin.TupleLess(5))) != 0)
            {
                //ind1 := abs(Index/2*2-2)
                //ind2 := abs(Index%2-1)
                HOperatorSet.AngleLl(hv_Row1.TupleSelect(0), hv_Col1.TupleSelect(0), hv_Row2.TupleSelect(
                    0), hv_Col2.TupleSelect(0), hv_Row1.TupleSelect(0), hv_Col1.TupleSelect(
                    0), hv_Row2.TupleSelect(1), hv_Col2.TupleSelect(1), out hv_Angle1);
            }
            else
            {
                HOperatorSet.AngleLl(hv_Row1.TupleSelect((new HTuple(0)).TupleConcat(1)), hv_Col1.TupleSelect(
                    (new HTuple(0)).TupleConcat(1)), hv_Row2.TupleSelect((new HTuple(0)).TupleConcat(
                    0)), hv_Col2.TupleSelect((new HTuple(0)).TupleConcat(0)), hv_Row1.TupleSelect(
                    (new HTuple(0)).TupleConcat(0)), hv_Col1.TupleSelect((new HTuple(0)).TupleConcat(
                    0)), hv_Row2.TupleSelect((new HTuple(1)).TupleConcat(1)), hv_Col2.TupleSelect(
                    (new HTuple(1)).TupleConcat(1)), out hv_Angle1);
            }
            HOperatorSet.TupleAbs(hv_Angle1, out hv_AngleAbs);
            //tuple_deg (AngleAbs, Deg)
            HOperatorSet.TupleMax(hv_AngleAbs.TupleConcat(hv_AngleDist_), out hv_AngleMax);
            if ((int)(new HTuple(((((hv_idxEnd0.TupleConcat(hv_idxEnd1))).TupleMin())).TupleEqual(
                1))) != 0)
            {
                hv_angle_thresh = 1;
            }
            else
            {
                hv_angle_thresh = 0.7;
            }
            if ((int)(new HTuple(hv_AngleMax.TupleGreater(hv_angle_thresh))) != 0)
            {
                hv_AngleMax = 10000;
            }
            else
            {
                hv_AngleMax = 0;
            }
            hv_Distance = (hv_dMin + 0.001) * (1 + ((4 * hv_AngleMax) * hv_AngleMax));

            return;
        }
        //0312 cntk
        //0327 塌线
        public void gold_wire_detect(HObject ho_ImageIC, HObject ho_ImagePCB, HObject ho_ImageLine,
            HObject ho_ic_pad_region, HObject ho_pcb_pad_region, out HObject ho_defect_region,
            out HObject ho_gold_wire, HTuple hv_line_num, HTuple hv_pcb_ball_num, HTuple hv_pcb_radius_low,
            HTuple hv_pcb_radius_high, HTuple hv_ic_radius_low, HTuple hv_ic_radius_high,
            HTuple hv_search_len1, HTuple hv_line_width, HTuple hv_line_contrast, HTuple hv_min_seg_length,
            HTuple hv_inspect_wire_num, HTuple hv_inspect_row, HTuple hv_inspect_col, HTuple hv_line_clip_length,
            HTuple hv_line_angle_extent, HTuple hv_line_max_gap, HTuple hv_lead_wire_inspect_size,
            HTuple hv_lead_wire_distance_thresh, HTuple hv_lead_wire_angle_thresh, HTuple hv_lead_wire_contrast,
            HTuple hv_lead_wire_length_thresh, HTuple hv_pcb_ball_gray_thresh, HTuple hv_is_cntk,
            HTuple hv_pcb_closing_radius, HTuple hv_pcb_fill_up_max, out HTuple hv_iFlag)
        {




            // Stack for temporary objects 
            HObject[] OTemp = new HObject[20];

            // Local iconic variables 

            HObject ho_ImageExtended = null, ho_ImagePart0 = null;
            HObject ho_ImageRotate0 = null, ho_ImagePart1 = null, ho_ImageRotate1 = null;
            HObject ho_ImagePart2 = null, ho_ImagePartsIn = null, ho_ImagePartsOut = null;
            HObject ho_ObjectSelected1 = null, ho_ObjectSelected = null;
            HObject ho_ImageAffinTrans = null, ho_image_ic = null, ho_image_pcb = null;
            HObject ho_image_wire = null, ho_pcb_pad = null, ho_ic_pad = null;
            HObject ho_ic_pad_ = null, ho_TrackRegion = null, ho_Wire = null;
            HObject ho_ImageSub = null, ho_pcb_ball = null, ho_pcb_ball_ = null;
            HObject ho_ic_ball = null, ho_defect_ic_pad = null, ho_ic_ball_ = null;
            HObject ho_defect_region_ = null, ho_wire = null;

            // Local control variables 

            HTuple hv_ic_measure_radius = null, hv_pcb_measure_radius = null;
            HTuple hv_crop_len1 = new HTuple(), hv_crop_len2 = new HTuple();
            HTuple hv_data_path = new HTuple(), hv_Area = new HTuple();
            HTuple hv_Row = new HTuple(), hv_Column = new HTuple();
            HTuple hv_Angle = new HTuple(), hv_inspect_wire_num_ = new HTuple();
            HTuple hv_Index = new HTuple(), hv_HomMat2D = new HTuple();
            HTuple hv_wire_num = new HTuple(), hv_i = new HTuple();
            HTuple hv_pad_ind = new HTuple(), hv_ball_num = new HTuple();
            HTuple hv_ball_index = new HTuple(), hv_ICBallRow = new HTuple();
            HTuple hv_ICBallCol = new HTuple(), hv_ICBallRadius = new HTuple();
            HTuple hv_iFlag_IC = new HTuple(), hv_Indices = new HTuple();
            HTuple hv_PCBBallRow = new HTuple(), hv_PCBBallCol = new HTuple();
            HTuple hv_PCBBallRadius = new HTuple(), hv_iFlag_PCB = new HTuple();
            HTuple hv_j = new HTuple(), hv_ic_ball_row = new HTuple();
            HTuple hv_ic_ball_col = new HTuple(), hv_ic_ball_radius = new HTuple();
            HTuple hv_pcb_ball_row = new HTuple(), hv_pcb_ball_col = new HTuple();
            HTuple hv_pcb_ball_radius = new HTuple(), hv_iFlag_lead_wire = new HTuple();
            HTuple hv_iFlag_wire = new HTuple(), hv_Min = new HTuple();
            HTuple hv_Max = new HTuple(), hv_Range = new HTuple();
            HTuple hv_Min1 = new HTuple(), hv_Max1 = new HTuple();
            HTuple hv_Range1 = new HTuple(), hv_PCB_pad_num = new HTuple();
            HTuple hv_ic_ball_iFlag = new HTuple(), hv_Area1 = new HTuple();
            HTuple hv_Distance = new HTuple(), hv_Number = null;
            HTuple hv_line_max_gap_COPY_INP_TMP = hv_line_max_gap.Clone();

            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_defect_region);
            HOperatorSet.GenEmptyObj(out ho_gold_wire);
            HOperatorSet.GenEmptyObj(out ho_ImageExtended);
            HOperatorSet.GenEmptyObj(out ho_ImagePart0);
            HOperatorSet.GenEmptyObj(out ho_ImageRotate0);
            HOperatorSet.GenEmptyObj(out ho_ImagePart1);
            HOperatorSet.GenEmptyObj(out ho_ImageRotate1);
            HOperatorSet.GenEmptyObj(out ho_ImagePart2);
            HOperatorSet.GenEmptyObj(out ho_ImagePartsIn);
            HOperatorSet.GenEmptyObj(out ho_ImagePartsOut);
            HOperatorSet.GenEmptyObj(out ho_ObjectSelected1);
            HOperatorSet.GenEmptyObj(out ho_ObjectSelected);
            HOperatorSet.GenEmptyObj(out ho_ImageAffinTrans);
            HOperatorSet.GenEmptyObj(out ho_image_ic);
            HOperatorSet.GenEmptyObj(out ho_image_pcb);
            HOperatorSet.GenEmptyObj(out ho_image_wire);
            HOperatorSet.GenEmptyObj(out ho_pcb_pad);
            HOperatorSet.GenEmptyObj(out ho_ic_pad);
            HOperatorSet.GenEmptyObj(out ho_ic_pad_);
            HOperatorSet.GenEmptyObj(out ho_TrackRegion);
            HOperatorSet.GenEmptyObj(out ho_Wire);
            HOperatorSet.GenEmptyObj(out ho_ImageSub);
            HOperatorSet.GenEmptyObj(out ho_pcb_ball);
            HOperatorSet.GenEmptyObj(out ho_pcb_ball_);
            HOperatorSet.GenEmptyObj(out ho_ic_ball);
            HOperatorSet.GenEmptyObj(out ho_defect_ic_pad);
            HOperatorSet.GenEmptyObj(out ho_ic_ball_);
            HOperatorSet.GenEmptyObj(out ho_defect_region_);
            HOperatorSet.GenEmptyObj(out ho_wire);
            try
            {
                hv_iFlag = 0;
                ho_defect_region.Dispose();
                HOperatorSet.GenEmptyObj(out ho_defect_region);
                ho_gold_wire.Dispose();
                HOperatorSet.GenEmptyObj(out ho_gold_wire);
                //***************************************************************
                //line_clip_length := 6
                //line_angle_extent := 0.55
                //line_max_gap := 30
                //lead_wire
                //lead_wire_inspect_size := 11
                //lead_wire_distance_thresh := 3
                //lead_wire_angle_thresh := 0.4
                //lead_wire_contrast := 120
                //lead_wire_length_thresh := 3
                //pcb_ball_gray_thresh := 80
                //***************************************************************
                hv_ic_measure_radius = (hv_ic_radius_low + hv_ic_radius_high) / 2;
                hv_pcb_measure_radius = (hv_pcb_radius_low + hv_pcb_radius_high) / 2;
                //is_cntk := 0
                if ((int)(hv_is_cntk) != 0)
                {
                    hv_crop_len1 = 80;
                    hv_crop_len2 = 480;
                    if ((int)(1) != 0)
                    {
                        ho_ImageExtended.Dispose();
                        HOperatorSet.AppendChannel(ho_ImageLine, ho_ImagePCB, out ho_ImageExtended
                            );
                        {
                            HObject ExpTmpOutVar_0;
                            HOperatorSet.AppendChannel(ho_ImageExtended, ho_ImageIC, out ExpTmpOutVar_0
                                );
                            ho_ImageExtended.Dispose();
                            ho_ImageExtended = ExpTmpOutVar_0;
                        }
                        ho_ImagePart0.Dispose();
                        HOperatorSet.CropRectangle1(ho_ImageExtended, out ho_ImagePart0, ((((hv_inspect_row.TupleSelect(
                            0))).TupleInt()) - hv_crop_len2) + 1, ((((hv_inspect_col.TupleSelect(0))).TupleInt()
                            ) - hv_crop_len1) + 1, (((hv_inspect_row.TupleSelect(0))).TupleInt()) + hv_crop_len2,
                            (((hv_inspect_col.TupleSelect(0))).TupleInt()) + hv_crop_len1);
                        ho_ImageRotate0.Dispose();
                        HOperatorSet.RotateImage(ho_ImagePart0, out ho_ImageRotate0, -90, "nearest_neighbor");
                        ho_ImagePart1.Dispose();
                        HOperatorSet.CropRectangle1(ho_ImageExtended, out ho_ImagePart1, ((((hv_inspect_row.TupleSelect(
                            1))).TupleInt()) - hv_crop_len2) + 1, ((((hv_inspect_col.TupleSelect(1))).TupleInt()
                            ) - hv_crop_len1) + 1, (((hv_inspect_row.TupleSelect(1))).TupleInt()) + hv_crop_len2,
                            (((hv_inspect_col.TupleSelect(1))).TupleInt()) + hv_crop_len1);
                        ho_ImageRotate1.Dispose();
                        HOperatorSet.RotateImage(ho_ImagePart1, out ho_ImageRotate1, 90, "nearest_neighbor");
                        ho_ImagePart2.Dispose();
                        HOperatorSet.CropRectangle1(ho_ImageExtended, out ho_ImagePart2, ((((hv_inspect_row.TupleSelect(
                            2))).TupleInt()) - hv_crop_len1) + 1, ((((hv_inspect_col.TupleSelect(2))).TupleInt()
                            ) - hv_crop_len2) + 1, (((hv_inspect_row.TupleSelect(2))).TupleInt()) + hv_crop_len1,
                            (((hv_inspect_col.TupleSelect(2))).TupleInt()) + hv_crop_len2);
                        ho_ImagePartsIn.Dispose();
                        HOperatorSet.ConcatObj(ho_ImageRotate0, ho_ImageRotate1, out ho_ImagePartsIn
                            );
                        {
                            HObject ExpTmpOutVar_0;
                            HOperatorSet.ConcatObj(ho_ImagePartsIn, ho_ImagePart2, out ExpTmpOutVar_0
                                );
                            ho_ImagePartsIn.Dispose();
                            ho_ImagePartsIn = ExpTmpOutVar_0;
                        }
                        //cntk predict
                        ho_ImagePartsOut.Dispose();
                        HOperatorSet.CopyImage(ho_ImagePartsIn, out ho_ImagePartsOut);
                    }
                    else
                    {
                        hv_data_path = "H:/LF_AOI/cntk test/20180321/data/result1.tiff";
                        ho_ImagePartsOut.Dispose();
                        HOperatorSet.ReadImage(out ho_ImagePartsOut, hv_data_path);
                    }
                    //********
                    ho_ObjectSelected1.Dispose();
                    HOperatorSet.SelectObj(ho_ImagePartsOut, out ho_ObjectSelected1, 1);
                    HOperatorSet.AreaCenter(ho_ObjectSelected1, out hv_Area, out hv_Row, out hv_Column);
                    hv_Angle = new HTuple();
                    hv_Angle[0] = 1.5708;
                    hv_Angle[1] = -1.5708;
                    hv_Angle[2] = 0;
                    hv_inspect_wire_num_ = new HTuple();
                    hv_inspect_wire_num_[0] = 0;
                    hv_inspect_wire_num_ = hv_inspect_wire_num_.TupleConcat(hv_inspect_wire_num);
                    for (hv_Index = 0; (int)hv_Index <= (int)((new HTuple(hv_inspect_wire_num.TupleLength()
                        )) - 1); hv_Index = (int)hv_Index + 1)
                    {
                        ho_ObjectSelected.Dispose();
                        HOperatorSet.SelectObj(ho_ImagePartsOut, out ho_ObjectSelected, hv_Index + 1);
                        HOperatorSet.VectorAngleToRigid(hv_Row, hv_Column, 0, hv_inspect_row.TupleSelect(
                            hv_Index), hv_inspect_col.TupleSelect(hv_Index), hv_Angle.TupleSelect(
                            hv_Index), out hv_HomMat2D);
                        ho_ImageAffinTrans.Dispose();
                        HOperatorSet.AffineTransImage(ho_ObjectSelected, out ho_ImageAffinTrans,
                            hv_HomMat2D, "nearest_neighbor", "true");
                        ho_image_ic.Dispose();
                        HOperatorSet.AccessChannel(ho_ImageAffinTrans, out ho_image_ic, 3);
                        ho_image_pcb.Dispose();
                        HOperatorSet.AccessChannel(ho_ImageAffinTrans, out ho_image_pcb, 2);
                        ho_image_wire.Dispose();
                        HOperatorSet.AccessChannel(ho_ImageAffinTrans, out ho_image_wire, 1);
                        hv_wire_num = hv_inspect_wire_num.TupleSelect(hv_Index);
                        HTuple end_val50 = hv_wire_num - 1;
                        HTuple step_val50 = 1;
                        for (hv_i = 0; hv_i.Continue(end_val50, step_val50); hv_i = hv_i.TupleAdd(step_val50))
                        {
                            hv_pad_ind = hv_i + (((hv_inspect_wire_num_.TupleSelectRange(0, hv_Index))).TupleSum()
                                );
                            ho_pcb_pad.Dispose();
                            HOperatorSet.SelectObj(ho_pcb_pad_region, out ho_pcb_pad, hv_pad_ind + 1);
                            hv_ball_num = hv_pcb_ball_num.TupleSelect(hv_pad_ind);
                            hv_ball_index = ((hv_pcb_ball_num.TupleSelectRange(0, hv_pad_ind))).TupleSum()
                                ;
                            ho_ic_pad.Dispose();
                            HOperatorSet.SelectObj(ho_ic_pad_region, out ho_ic_pad, HTuple.TupleGenSequence(
                                (hv_ball_index - hv_ball_num) + 1, hv_ball_index, 1));
                            //ic ball detect
                            ic_ball_detect_unit(ho_image_ic, ho_ic_pad, 1, hv_ball_num, hv_ic_radius_low,
                                hv_ic_radius_high, hv_ic_measure_radius, 3, 2, out hv_ICBallRow,
                                out hv_ICBallCol, out hv_ICBallRadius, out hv_iFlag_IC);
                            HOperatorSet.TupleFind(hv_ICBallRow, 0, out hv_Indices);
                            if ((int)(new HTuple(hv_Indices.TupleNotEqual(-1))) != 0)
                            {
                                ho_ic_pad_.Dispose();
                                HOperatorSet.SelectObj(ho_ic_pad, out ho_ic_pad_, hv_Indices + 1);
                                {
                                    HObject ExpTmpOutVar_0;
                                    HOperatorSet.ConcatObj(ho_defect_region, ho_ic_pad_, out ExpTmpOutVar_0
                                        );
                                    ho_defect_region.Dispose();
                                    ho_defect_region = ExpTmpOutVar_0;
                                }
                            }
                            //pcb ball detect
                            pcb_ball_detect_unit(ho_image_pcb, ho_ic_pad, ho_pcb_pad, hv_ball_num,
                                hv_pcb_radius_low, hv_pcb_radius_high, 128, hv_pcb_measure_radius,
                                3, 2, hv_pcb_closing_radius, hv_pcb_fill_up_max, out hv_PCBBallRow,
                                out hv_PCBBallCol, out hv_PCBBallRadius, out hv_iFlag_PCB);
                            HOperatorSet.TupleFind(hv_PCBBallRow, 0, out hv_Indices);
                            if ((int)(new HTuple(hv_Indices.TupleNotEqual(-1))) != 0)
                            {
                                {
                                    HObject ExpTmpOutVar_0;
                                    HOperatorSet.ConcatObj(ho_defect_region, ho_pcb_pad, out ExpTmpOutVar_0
                                        );
                                    ho_defect_region.Dispose();
                                    ho_defect_region = ExpTmpOutVar_0;
                                }
                            }
                            //ic lead wire detect
                            HTuple end_val70 = hv_ball_num - 1;
                            HTuple step_val70 = 1;
                            for (hv_j = 0; hv_j.Continue(end_val70, step_val70); hv_j = hv_j.TupleAdd(step_val70))
                            {
                                ho_ic_pad_.Dispose();
                                HOperatorSet.SelectObj(ho_ic_pad, out ho_ic_pad_, hv_j + 1);
                                hv_ic_ball_row = hv_ICBallRow.TupleSelect(hv_j);
                                hv_ic_ball_col = hv_ICBallCol.TupleSelect(hv_j);
                                hv_ic_ball_radius = (hv_ICBallRadius.TupleSelect(hv_j)) + 1;
                                hv_pcb_ball_row = hv_PCBBallRow.TupleSelect(hv_j);
                                hv_pcb_ball_col = hv_PCBBallCol.TupleSelect(hv_j);
                                hv_pcb_ball_radius = hv_PCBBallRow.TupleSelect(hv_j);
                                if ((int)((new HTuple(((hv_ICBallRadius.TupleSelect(hv_j))).TupleLess(
                                    1))).TupleOr(new HTuple(hv_pcb_ball_radius.TupleLess(1)))) != 0)
                                {
                                    continue;
                                }
                                lead_wire_detect(ho_ImageLine, ho_ic_pad_, hv_lead_wire_inspect_size,
                                    hv_ic_ball_row, hv_ic_ball_col, hv_ic_ball_radius, hv_pcb_ball_row,
                                    hv_pcb_ball_col, hv_line_width, hv_lead_wire_contrast, hv_lead_wire_length_thresh,
                                    hv_lead_wire_angle_thresh, hv_lead_wire_distance_thresh, out hv_iFlag_lead_wire);
                                if ((int)(new HTuple(hv_iFlag_lead_wire.TupleNotEqual(0))) != 0)
                                {
                                    HOperatorSet.TupleReplace(hv_ICBallRow, hv_j, 0, out hv_ICBallRow);
                                    {
                                        HObject ExpTmpOutVar_0;
                                        HOperatorSet.ConcatObj(ho_defect_region, ho_ic_pad_, out ExpTmpOutVar_0
                                            );
                                        ho_defect_region.Dispose();
                                        ho_defect_region = ExpTmpOutVar_0;
                                    }
                                }
                            }
                            //bond wire track
                            HTuple end_val88 = hv_ball_num - 1;
                            HTuple step_val88 = 1;
                            for (hv_j = 0; hv_j.Continue(end_val88, step_val88); hv_j = hv_j.TupleAdd(step_val88))
                            {
                                if ((int)((new HTuple(((hv_PCBBallRow.TupleSelect(hv_j))).TupleEqual(
                                    0))).TupleOr(new HTuple(((hv_ICBallRow.TupleSelect(hv_j))).TupleEqual(
                                    0)))) != 0)
                                {
                                    continue;
                                }
                                ho_TrackRegion.Dispose(); ho_Wire.Dispose();
                                track_wire_unit(ho_image_wire, out ho_TrackRegion, out ho_Wire, hv_PCBBallRow.TupleSelect(
                                    hv_j), hv_PCBBallCol.TupleSelect(hv_j), hv_PCBBallRadius.TupleSelect(
                                    hv_j), hv_ICBallRow.TupleSelect(hv_j), hv_ICBallCol.TupleSelect(
                                    hv_j), hv_ICBallRadius.TupleSelect(hv_j), hv_search_len1, hv_line_clip_length,
                                    hv_line_width, hv_line_contrast, hv_min_seg_length, hv_line_angle_extent,
                                    hv_line_max_gap_COPY_INP_TMP, out hv_iFlag_wire);
                                {
                                    HObject ExpTmpOutVar_0;
                                    HOperatorSet.ConcatObj(ho_gold_wire, ho_Wire, out ExpTmpOutVar_0);
                                    ho_gold_wire.Dispose();
                                    ho_gold_wire = ExpTmpOutVar_0;
                                }
                                if ((int)(new HTuple(hv_iFlag_wire.TupleNotEqual(0))) != 0)
                                {
                                    {
                                        HObject ExpTmpOutVar_0;
                                        HOperatorSet.ConcatObj(ho_defect_region, ho_TrackRegion, out ExpTmpOutVar_0
                                            );
                                        ho_defect_region.Dispose();
                                        ho_defect_region = ExpTmpOutVar_0;
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    ho_pcb_pad.Dispose();
                    HOperatorSet.Union1(ho_pcb_pad_region, out ho_pcb_pad);
                    HOperatorSet.MinMaxGray(ho_pcb_pad, ho_ImageLine, 20, out hv_Min, out hv_Max,
                        out hv_Range);
                    HOperatorSet.MinMaxGray(ho_pcb_pad, ho_ImageIC, 20, out hv_Min1, out hv_Max1,
                        out hv_Range1);
                    ho_ImageSub.Dispose();
                    HOperatorSet.SubImage(ho_ImageLine, ho_ImageIC, out ho_ImageSub, 1, 0);
                    HOperatorSet.CountObj(ho_pcb_pad_region, out hv_PCB_pad_num);
                    ho_pcb_ball.Dispose();
                    HOperatorSet.GenEmptyObj(out ho_pcb_ball);
                    HTuple end_val107 = hv_PCB_pad_num - 1;
                    HTuple step_val107 = 1;
                    for (hv_Index = 0; hv_Index.Continue(end_val107, step_val107); hv_Index = hv_Index.TupleAdd(step_val107))
                    {
                        ho_pcb_pad.Dispose();
                        HOperatorSet.SelectObj(ho_pcb_pad_region, out ho_pcb_pad, hv_Index + 1);
                        hv_ball_num = hv_pcb_ball_num.TupleSelect(hv_Index);
                        hv_ball_index = ((hv_pcb_ball_num.TupleSelectRange(0, hv_Index))).TupleSum()
                            ;
                        ho_ic_pad.Dispose();
                        HOperatorSet.SelectObj(ho_ic_pad_region, out ho_ic_pad, HTuple.TupleGenSequence(
                            (hv_ball_index - hv_ball_num) + 1, hv_ball_index, 1));
                        pcb_ball_detect_unit(ho_ImageSub, ho_ic_pad, ho_pcb_pad, hv_ball_num, hv_pcb_radius_low,
                            hv_pcb_radius_high, hv_pcb_ball_gray_thresh, hv_pcb_measure_radius,
                            4, 2, hv_pcb_closing_radius, hv_pcb_fill_up_max, out hv_PCBBallRow,
                            out hv_PCBBallCol, out hv_PCBBallRadius, out hv_iFlag_PCB);
                        HOperatorSet.TupleFind(hv_PCBBallRow, 0, out hv_Indices);
                        if ((int)(new HTuple(hv_Indices.TupleNotEqual(-1))) != 0)
                        {
                            {
                                HObject ExpTmpOutVar_0;
                                HOperatorSet.ConcatObj(ho_defect_region, ho_pcb_pad, out ExpTmpOutVar_0
                                    );
                                ho_defect_region.Dispose();
                                ho_defect_region = ExpTmpOutVar_0;
                            }
                        }
                        ho_pcb_ball_.Dispose();
                        HOperatorSet.GenCircle(out ho_pcb_ball_, hv_PCBBallRow, hv_PCBBallCol,
                            hv_PCBBallRadius);
                        {
                            HObject ExpTmpOutVar_0;
                            HOperatorSet.ConcatObj(ho_pcb_ball, ho_pcb_ball_, out ExpTmpOutVar_0);
                            ho_pcb_ball.Dispose();
                            ho_pcb_ball = ExpTmpOutVar_0;
                        }
                    }
                    //pcb_ball_detect (ImageLine, ImageIC, pcb_pad_region, ic_pad_region, defect_pcb_pad, pcb_ball, pcb_ball_num, line_num, pcb_radius_low, pcb_radius_high, pcb_ball_iFlag)
                    //if (pcb_ball_iFlag # 0)
                    //concat_obj (defect_region, defect_pcb_pad, defect_region)
                    //endif
                    ho_ic_ball.Dispose(); ho_defect_ic_pad.Dispose();
                    ic_ball_detect(ho_ImageIC, ho_ic_pad_region, out ho_ic_ball, out ho_defect_ic_pad,
                        hv_line_num, hv_ic_radius_low, hv_ic_radius_high, out hv_ic_ball_iFlag);
                    if ((int)(new HTuple(hv_ic_ball_iFlag.TupleNotEqual(0))) != 0)
                    {
                        {
                            HObject ExpTmpOutVar_0;
                            HOperatorSet.ConcatObj(ho_defect_region, ho_defect_ic_pad, out ExpTmpOutVar_0
                                );
                            ho_defect_region.Dispose();
                            ho_defect_region = ExpTmpOutVar_0;
                        }
                    }
                    ho_gold_wire.Dispose();
                    HOperatorSet.GenEmptyObj(out ho_gold_wire);
                    HTuple end_val129 = hv_line_num - 1;
                    HTuple step_val129 = 1;
                    for (hv_Index = 0; hv_Index.Continue(end_val129, step_val129); hv_Index = hv_Index.TupleAdd(step_val129))
                    {
                        ho_ic_pad_.Dispose();
                        HOperatorSet.SelectObj(ho_ic_pad_region, out ho_ic_pad_, hv_Index + 1);
                        ho_pcb_ball_.Dispose();
                        HOperatorSet.SelectObj(ho_pcb_ball, out ho_pcb_ball_, hv_Index + 1);
                        ho_ic_ball_.Dispose();
                        HOperatorSet.SelectObj(ho_ic_ball, out ho_ic_ball_, hv_Index + 1);
                        HOperatorSet.SmallestCircle(ho_ic_ball_, out hv_ic_ball_row, out hv_ic_ball_col,
                            out hv_ic_ball_radius);
                        HOperatorSet.AreaCenter(ho_pcb_ball_, out hv_Area1, out hv_pcb_ball_row,
                            out hv_pcb_ball_col);
                        if ((int)((new HTuple(hv_ic_ball_radius.TupleLess(1))).TupleOr(new HTuple(hv_Area1.TupleLess(
                            1)))) != 0)
                        {
                            continue;
                        }
                        lead_wire_detect(ho_ImageLine, ho_ic_pad_, hv_lead_wire_inspect_size, hv_ic_ball_row,
                            hv_ic_ball_col, hv_ic_ball_radius + 2, hv_pcb_ball_row, hv_pcb_ball_col,
                            hv_line_width, hv_lead_wire_contrast, hv_lead_wire_length_thresh, hv_lead_wire_angle_thresh,
                            hv_lead_wire_distance_thresh, out hv_iFlag_lead_wire);
                        if ((int)(new HTuple(hv_iFlag_lead_wire.TupleNotEqual(0))) != 0)
                        {
                            {
                                HObject ExpTmpOutVar_0;
                                HOperatorSet.ConcatObj(ho_defect_region, ho_ic_pad_, out ExpTmpOutVar_0
                                    );
                                ho_defect_region.Dispose();
                                ho_defect_region = ExpTmpOutVar_0;
                            }
                            continue;
                        }
                        hv_pcb_ball_radius = (hv_pcb_radius_low + hv_pcb_radius_high) / 2;
                        HOperatorSet.DistancePp(hv_pcb_ball_row, hv_pcb_ball_col, hv_ic_ball_row,
                            hv_ic_ball_col, out hv_Distance);
                        hv_line_max_gap_COPY_INP_TMP = hv_Distance * 0.4;
                        ho_defect_region_.Dispose(); ho_wire.Dispose();
                        track_wire_unit(ho_ImageSub, out ho_defect_region_, out ho_wire, hv_pcb_ball_row,
                            hv_pcb_ball_col, hv_pcb_ball_radius, hv_ic_ball_row, hv_ic_ball_col,
                            hv_ic_ball_radius, hv_search_len1, hv_line_clip_length, hv_line_width,
                            hv_line_contrast, hv_min_seg_length, hv_line_angle_extent, hv_line_max_gap_COPY_INP_TMP,
                            out hv_iFlag_wire);
                        //track_wire_DP (ImageSub, ic_ball_, pcb_ball_, wire, defect_region_, search_len1, line_contrast, line_width, min_seg_length, iFlag1)
                        if ((int)(new HTuple(hv_iFlag_wire.TupleNotEqual(0))) != 0)
                        {
                            {
                                HObject ExpTmpOutVar_0;
                                HOperatorSet.ConcatObj(ho_defect_region, ho_defect_region_, out ExpTmpOutVar_0
                                    );
                                ho_defect_region.Dispose();
                                ho_defect_region = ExpTmpOutVar_0;
                            }
                        }
                        {
                            HObject ExpTmpOutVar_0;
                            HOperatorSet.ConcatObj(ho_gold_wire, ho_wire, out ExpTmpOutVar_0);
                            ho_gold_wire.Dispose();
                            ho_gold_wire = ExpTmpOutVar_0;
                        }
                    }
                }
                HOperatorSet.CountObj(ho_defect_region, out hv_Number);
                if ((int)(hv_Number) != 0)
                {
                    hv_iFlag = -1;
                }
                ho_ImageExtended.Dispose();
                ho_ImagePart0.Dispose();
                ho_ImageRotate0.Dispose();
                ho_ImagePart1.Dispose();
                ho_ImageRotate1.Dispose();
                ho_ImagePart2.Dispose();
                ho_ImagePartsIn.Dispose();
                ho_ImagePartsOut.Dispose();
                ho_ObjectSelected1.Dispose();
                ho_ObjectSelected.Dispose();
                ho_ImageAffinTrans.Dispose();
                ho_image_ic.Dispose();
                ho_image_pcb.Dispose();
                ho_image_wire.Dispose();
                ho_pcb_pad.Dispose();
                ho_ic_pad.Dispose();
                ho_ic_pad_.Dispose();
                ho_TrackRegion.Dispose();
                ho_Wire.Dispose();
                ho_ImageSub.Dispose();
                ho_pcb_ball.Dispose();
                ho_pcb_ball_.Dispose();
                ho_ic_ball.Dispose();
                ho_defect_ic_pad.Dispose();
                ho_ic_ball_.Dispose();
                ho_defect_region_.Dispose();
                ho_wire.Dispose();

                return;
            }
            catch (HalconException HDevExpDefaultException)
            {
                ho_ImageExtended.Dispose();
                ho_ImagePart0.Dispose();
                ho_ImageRotate0.Dispose();
                ho_ImagePart1.Dispose();
                ho_ImageRotate1.Dispose();
                ho_ImagePart2.Dispose();
                ho_ImagePartsIn.Dispose();
                ho_ImagePartsOut.Dispose();
                ho_ObjectSelected1.Dispose();
                ho_ObjectSelected.Dispose();
                ho_ImageAffinTrans.Dispose();
                ho_image_ic.Dispose();
                ho_image_pcb.Dispose();
                ho_image_wire.Dispose();
                ho_pcb_pad.Dispose();
                ho_ic_pad.Dispose();
                ho_ic_pad_.Dispose();
                ho_TrackRegion.Dispose();
                ho_Wire.Dispose();
                ho_ImageSub.Dispose();
                ho_pcb_ball.Dispose();
                ho_pcb_ball_.Dispose();
                ho_ic_ball.Dispose();
                ho_defect_ic_pad.Dispose();
                ho_ic_ball_.Dispose();
                ho_defect_region_.Dispose();
                ho_wire.Dispose();

                throw HDevExpDefaultException;
            }
        }


        //3.11 测量园长半径 3-4
        public void ic_ball_detect(HObject ho_ImageIC, HObject ho_ic_pad_affine, out HObject ho_ic_ball,
            out HObject ho_defect_ic_pad, HTuple hv_line_num, HTuple hv_ic_radius_low, HTuple hv_ic_radius_high,
            out HTuple hv_iFlag)
        {




            // Stack for temporary objects 
            HObject[] OTemp = new HObject[20];

            // Local iconic variables 

            HObject ho_ObjectSelected = null, ho_ImageReduced1 = null;
            HObject ho_Region1 = null, ho_RegionClosing = null, ho_RegionFillUp = null;
            HObject ho_Contours = null, ho_Cross = null, ho_Contour = null;
            HObject ho_SelectedRegion = null, ho_ImageReduced = null, ho_Region = null;
            HObject ho_RegionOpening = null, ho_ConnectedRegions = null;

            // Local control variables 

            HTuple hv_ICBallRadius = null, hv_is_measure_circle = null;
            HTuple hv_Width = null, hv_Height = null, hv_Index = null;
            HTuple hv_Area = new HTuple(), hv_Row = new HTuple(), hv_Column = new HTuple();
            HTuple hv_MetrologyHandle = new HTuple(), hv_Index_ = new HTuple();
            HTuple hv_Circle = new HTuple(), hv_Row1 = new HTuple();
            HTuple hv_Column1 = new HTuple(), hv_Mean = new HTuple();
            HTuple hv_Deviation = new HTuple(), hv_num = new HTuple();
            HTuple hv_opening_size = new HTuple(), hv_Row2 = new HTuple();
            HTuple hv_Column2 = new HTuple(), hv_Radius = new HTuple();
            HTuple hv_Number = null;
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_ic_ball);
            HOperatorSet.GenEmptyObj(out ho_defect_ic_pad);
            HOperatorSet.GenEmptyObj(out ho_ObjectSelected);
            HOperatorSet.GenEmptyObj(out ho_ImageReduced1);
            HOperatorSet.GenEmptyObj(out ho_Region1);
            HOperatorSet.GenEmptyObj(out ho_RegionClosing);
            HOperatorSet.GenEmptyObj(out ho_RegionFillUp);
            HOperatorSet.GenEmptyObj(out ho_Contours);
            HOperatorSet.GenEmptyObj(out ho_Cross);
            HOperatorSet.GenEmptyObj(out ho_Contour);
            HOperatorSet.GenEmptyObj(out ho_SelectedRegion);
            HOperatorSet.GenEmptyObj(out ho_ImageReduced);
            HOperatorSet.GenEmptyObj(out ho_Region);
            HOperatorSet.GenEmptyObj(out ho_RegionOpening);
            HOperatorSet.GenEmptyObj(out ho_ConnectedRegions);
            try
            {
                hv_iFlag = 0;
                hv_ICBallRadius = new HTuple();
                ho_ic_ball.Dispose();
                HOperatorSet.GenEmptyObj(out ho_ic_ball);
                ho_defect_ic_pad.Dispose();
                HOperatorSet.GenEmptyObj(out ho_defect_ic_pad);

                hv_is_measure_circle = 1;
                HOperatorSet.GetImageSize(ho_ImageIC, out hv_Width, out hv_Height);
                HTuple end_val7 = hv_line_num;
                HTuple step_val7 = 1;
                for (hv_Index = 1; hv_Index.Continue(end_val7, step_val7); hv_Index = hv_Index.TupleAdd(step_val7))
                {
                    ho_ObjectSelected.Dispose();
                    HOperatorSet.SelectObj(ho_ic_pad_affine, out ho_ObjectSelected, hv_Index);
                    if ((int)(hv_is_measure_circle) != 0)
                    {
                        ho_ImageReduced1.Dispose();
                        HOperatorSet.ReduceDomain(ho_ImageIC, ho_ObjectSelected, out ho_ImageReduced1
                            );
                        ho_Region1.Dispose();
                        HOperatorSet.Threshold(ho_ImageReduced1, out ho_Region1, 0, 90);
                        ho_RegionClosing.Dispose();
                        HOperatorSet.OpeningCircle(ho_Region1, out ho_RegionClosing, 3);
                        ho_RegionFillUp.Dispose();
                        HOperatorSet.FillUpShape(ho_RegionClosing, out ho_RegionFillUp, "area",
                            1, 3);
                        HOperatorSet.AreaCenter(ho_RegionFillUp, out hv_Area, out hv_Row, out hv_Column);
                        HOperatorSet.CreateMetrologyModel(out hv_MetrologyHandle);
                        HOperatorSet.SetMetrologyModelImageSize(hv_MetrologyHandle, hv_Width, hv_Height);
                        HOperatorSet.AddMetrologyObjectCircleMeasure(hv_MetrologyHandle, hv_Row,
                            hv_Column, 5, 4, 2, 1, 30, ((new HTuple("measure_transition")).TupleConcat(
                            "measure_distance")).TupleConcat("min_score"), ((new HTuple("positive")).TupleConcat(
                            3)).TupleConcat(0.6), out hv_Index_);
                        HOperatorSet.ApplyMetrologyModel(ho_ImageIC, hv_MetrologyHandle);
                        HOperatorSet.GetMetrologyObjectResult(hv_MetrologyHandle, hv_Index_, "all",
                            "result_type", "all_param", out hv_Circle);
                        ho_Contours.Dispose();
                        HOperatorSet.GetMetrologyObjectMeasures(out ho_Contours, hv_MetrologyHandle,
                            "all", "all", out hv_Row1, out hv_Column1);
                        ho_Cross.Dispose();
                        HOperatorSet.GenCrossContourXld(out ho_Cross, hv_Row1, hv_Column1, 6, 0.785398);
                        ho_Contour.Dispose();
                        HOperatorSet.GetMetrologyObjectResultContour(out ho_Contour, hv_MetrologyHandle,
                            hv_Index_, "all", 1.5);
                        HOperatorSet.ClearMetrologyModel(hv_MetrologyHandle);
                        if ((int)(new HTuple((new HTuple(hv_Circle.TupleLength())).TupleEqual(3))) != 0)
                        {
                            hv_ICBallRadius = hv_ICBallRadius.TupleConcat(hv_Circle.TupleSelect(2));
                            if ((int)((new HTuple(((hv_Circle.TupleSelect(2))).TupleGreater(hv_ic_radius_low))).TupleAnd(
                                new HTuple(((hv_Circle.TupleSelect(2))).TupleLess(hv_ic_radius_high)))) != 0)
                            {
                                ho_SelectedRegion.Dispose();
                                HOperatorSet.GenCircle(out ho_SelectedRegion, hv_Circle.TupleSelect(
                                    0), hv_Circle.TupleSelect(1), hv_Circle.TupleSelect(2));
                            }
                            else
                            {
                                {
                                    HObject ExpTmpOutVar_0;
                                    HOperatorSet.ConcatObj(ho_defect_ic_pad, ho_ObjectSelected, out ExpTmpOutVar_0
                                        );
                                    ho_defect_ic_pad.Dispose();
                                    ho_defect_ic_pad = ExpTmpOutVar_0;
                                }
                                ho_SelectedRegion.Dispose();
                                HOperatorSet.GenEmptyRegion(out ho_SelectedRegion);
                            }
                        }
                        else
                        {
                            hv_ICBallRadius = hv_ICBallRadius.TupleConcat(0);
                            {
                                HObject ExpTmpOutVar_0;
                                HOperatorSet.ConcatObj(ho_defect_ic_pad, ho_ObjectSelected, out ExpTmpOutVar_0
                                    );
                                ho_defect_ic_pad.Dispose();
                                ho_defect_ic_pad = ExpTmpOutVar_0;
                            }
                            ho_SelectedRegion.Dispose();
                            HOperatorSet.GenEmptyRegion(out ho_SelectedRegion);
                        }
                        {
                            HObject ExpTmpOutVar_0;
                            HOperatorSet.ConcatObj(ho_ic_ball, ho_SelectedRegion, out ExpTmpOutVar_0
                                );
                            ho_ic_ball.Dispose();
                            ho_ic_ball = ExpTmpOutVar_0;
                        }
                    }
                    else
                    {
                        ho_ImageReduced.Dispose();
                        HOperatorSet.ReduceDomain(ho_ImageIC, ho_ObjectSelected, out ho_ImageReduced
                            );
                        HOperatorSet.Intensity(ho_ObjectSelected, ho_ImageReduced, out hv_Mean,
                            out hv_Deviation);
                        ho_Region.Dispose();
                        HOperatorSet.Threshold(ho_ImageReduced, out ho_Region, 0, hv_Mean);
                        ho_RegionFillUp.Dispose();
                        HOperatorSet.FillUpShape(ho_Region, out ho_RegionFillUp, "area", 1, 100);
                        hv_num = 0;
                        hv_opening_size = hv_ic_radius_high.Clone();
                        while ((int)((new HTuple(hv_num.TupleNotEqual(1))).TupleAnd(new HTuple(hv_opening_size.TupleGreaterEqual(
                            hv_ic_radius_low)))) != 0)
                        {
                            ho_RegionOpening.Dispose();
                            HOperatorSet.OpeningCircle(ho_RegionFillUp, out ho_RegionOpening, hv_opening_size);
                            hv_opening_size = hv_opening_size - 1;
                            ho_ConnectedRegions.Dispose();
                            HOperatorSet.Connection(ho_RegionOpening, out ho_ConnectedRegions);
                            ho_SelectedRegion.Dispose();
                            HOperatorSet.SelectShape(ho_ConnectedRegions, out ho_SelectedRegion,
                                "area", "and", 1, 99999);
                            HOperatorSet.CountObj(ho_SelectedRegion, out hv_num);
                        }
                        if ((int)(new HTuple(hv_num.TupleEqual(0))) != 0)
                        {
                            hv_ICBallRadius = hv_ICBallRadius.TupleConcat(0);
                            ho_SelectedRegion.Dispose();
                            HOperatorSet.GenEmptyObj(out ho_SelectedRegion);
                            {
                                HObject ExpTmpOutVar_0;
                                HOperatorSet.ConcatObj(ho_defect_ic_pad, ho_ObjectSelected, out ExpTmpOutVar_0
                                    );
                                ho_defect_ic_pad.Dispose();
                                ho_defect_ic_pad = ExpTmpOutVar_0;
                            }
                            ho_SelectedRegion.Dispose();
                            HOperatorSet.GenEmptyRegion(out ho_SelectedRegion);
                        }
                        HOperatorSet.SmallestCircle(ho_SelectedRegion, out hv_Row2, out hv_Column2,
                            out hv_Radius);
                        hv_ICBallRadius = hv_ICBallRadius.TupleConcat(hv_Radius);
                        {
                            HObject ExpTmpOutVar_0;
                            HOperatorSet.ConcatObj(ho_ic_ball, ho_SelectedRegion, out ExpTmpOutVar_0
                                );
                            ho_ic_ball.Dispose();
                            ho_ic_ball = ExpTmpOutVar_0;
                        }
                    }
                }
                HOperatorSet.CountObj(ho_defect_ic_pad, out hv_Number);
                if ((int)(hv_Number) != 0)
                {
                    hv_iFlag = -1;
                }
                ho_ObjectSelected.Dispose();
                ho_ImageReduced1.Dispose();
                ho_Region1.Dispose();
                ho_RegionClosing.Dispose();
                ho_RegionFillUp.Dispose();
                ho_Contours.Dispose();
                ho_Cross.Dispose();
                ho_Contour.Dispose();
                ho_SelectedRegion.Dispose();
                ho_ImageReduced.Dispose();
                ho_Region.Dispose();
                ho_RegionOpening.Dispose();
                ho_ConnectedRegions.Dispose();

                return;
            }
            catch (HalconException HDevExpDefaultException)
            {
                ho_ObjectSelected.Dispose();
                ho_ImageReduced1.Dispose();
                ho_Region1.Dispose();
                ho_RegionClosing.Dispose();
                ho_RegionFillUp.Dispose();
                ho_Contours.Dispose();
                ho_Cross.Dispose();
                ho_Contour.Dispose();
                ho_SelectedRegion.Dispose();
                ho_ImageReduced.Dispose();
                ho_Region.Dispose();
                ho_RegionOpening.Dispose();
                ho_ConnectedRegions.Dispose();

                throw HDevExpDefaultException;
            }
        }

        //3/11 针对多个chipping
        //0321 cntk
        public void LFAOI_load_all_model(out HObjectVector/*{eObjectVector,Dim=1}*/ hvec_vec_model_object,
            HTupleVector/*{eTupleVector,Dim=1}*/ hvec_vec_model_param, out HTupleVector/*{eTupleVector,Dim=1}*/ hvec_vec_model_tuple,
            out HTuple hv_iFlag, out HTuple hv_err_msg)
        {



            // Stack for temporary objects 
            HObject[] OTemp = new HObject[20];

            // Local iconic variables 

            HObject ho_EmptyObject, ho_pcb_mean_image;
            HObject ho_pcb_std_image, ho_pcb_match_region, ho_pcb_inspect_region;
            HObject ho_pcb_reject_region, ho_pcb_sub_region, ho_pcb_dark_thresh_image;
            HObject ho_pcb_light_thresh_image, ho_ic_mean_image, ho_ic_std_image;
            HObject ho_ic_match_region, ho_ic_inspect_region, ho_ic_reject_region;
            HObject ho_ic_sub_region, ho_ic_dark_thresh_image, ho_ic_light_thresh_image;
            HObject ho_pcb_pad, ho_ic_pad, ho_chippingInspectReg, ho_region = null;
            HObject ho_scratch_region, ho_match_inspect_region, ho_RegionUnion = null;
            HObject ho_show_contour1 = null, ho_show_contour = null;

            // Local control variables 

            HTuple hv_model_path = null, hv_pcb_sub_reg_num = null;
            HTuple hv_pcb_sobel_scale = null, hv_pcb_dark_thresh = null;
            HTuple hv_pcb_light_thresh = null, hv_ic_sub_reg_num = null;
            HTuple hv_ic_sobel_scale = null, hv_ic_dark_thresh = null;
            HTuple hv_ic_light_thresh = null, hv_pcb_model_path = null;
            HTuple hv_pcb_iFlag1 = null, hv_pcb_ModelID = null, hv_pcb_model_type = null;
            HTuple hv_ic_model_path = null, hv_ic_iFlag1 = null, hv_ic_ModelID = null;
            HTuple hv_ic_model_type = null, hv_wire_model_path = null;
            HTuple hv_ball_num_tup = null, hv_pcb_Number = null, hv_ic_Number = null;
            HTuple hv_inspect_row = null, hv_inspect_col = null, hv_inspect_wire_num = null;
            HTuple hv_chipping_model_path = null, hv_Dres = null, hv_chipping_modelID = null;
            HTuple hv_chipping_model_type = null, hv_Index = null;
            HTuple hv_model_id = new HTuple(), hv_model_type = new HTuple();
            HTuple hv_scratch_model_path = null, hv_match_model_path = null;
            HTuple hv_match_modelID = null, hv_match_model_type = null;
            HTuple hv_iFlag2 = new HTuple(), hv_model_id0 = new HTuple();
            HTuple hv_model_id1 = new HTuple(), hv_model_type0 = new HTuple();
            HTuple hv_model_type1 = new HTuple(), hv_def_row1 = new HTuple();
            HTuple hv_def_col1 = new HTuple(), hv_iFlag1 = new HTuple();
            HTuple hv_def_row = new HTuple(), hv_def_col = new HTuple();
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_EmptyObject);
            HOperatorSet.GenEmptyObj(out ho_pcb_mean_image);
            HOperatorSet.GenEmptyObj(out ho_pcb_std_image);
            HOperatorSet.GenEmptyObj(out ho_pcb_match_region);
            HOperatorSet.GenEmptyObj(out ho_pcb_inspect_region);
            HOperatorSet.GenEmptyObj(out ho_pcb_reject_region);
            HOperatorSet.GenEmptyObj(out ho_pcb_sub_region);
            HOperatorSet.GenEmptyObj(out ho_pcb_dark_thresh_image);
            HOperatorSet.GenEmptyObj(out ho_pcb_light_thresh_image);
            HOperatorSet.GenEmptyObj(out ho_ic_mean_image);
            HOperatorSet.GenEmptyObj(out ho_ic_std_image);
            HOperatorSet.GenEmptyObj(out ho_ic_match_region);
            HOperatorSet.GenEmptyObj(out ho_ic_inspect_region);
            HOperatorSet.GenEmptyObj(out ho_ic_reject_region);
            HOperatorSet.GenEmptyObj(out ho_ic_sub_region);
            HOperatorSet.GenEmptyObj(out ho_ic_dark_thresh_image);
            HOperatorSet.GenEmptyObj(out ho_ic_light_thresh_image);
            HOperatorSet.GenEmptyObj(out ho_pcb_pad);
            HOperatorSet.GenEmptyObj(out ho_ic_pad);
            HOperatorSet.GenEmptyObj(out ho_chippingInspectReg);
            HOperatorSet.GenEmptyObj(out ho_region);
            HOperatorSet.GenEmptyObj(out ho_scratch_region);
            HOperatorSet.GenEmptyObj(out ho_match_inspect_region);
            HOperatorSet.GenEmptyObj(out ho_RegionUnion);
            HOperatorSet.GenEmptyObj(out ho_show_contour1);
            HOperatorSet.GenEmptyObj(out ho_show_contour);
            hvec_vec_model_object = new HObjectVector(1);
            try
            {
                //*******************************************************************************
                //0:pcb golden model, 1:ic golden model, 2:pcb pad, 3:ic pad, 4:chipping, 5: scratch, 6: match
                //golden model [dark, light, match, inspect, reject, sub]
                //*******************************************************************************
                hv_iFlag = 0;
                hv_err_msg = "";
                hv_model_path = hvec_vec_model_param[0].T.Clone();
                hv_pcb_sub_reg_num = hvec_vec_model_param[1].T[0];
                hv_pcb_sobel_scale = hvec_vec_model_param[1].T[1];
                hv_pcb_dark_thresh = hvec_vec_model_param[1].T[2];
                hv_pcb_light_thresh = hvec_vec_model_param[1].T[3];
                hv_ic_sub_reg_num = hvec_vec_model_param[2].T[0];
                hv_ic_sobel_scale = hvec_vec_model_param[2].T[1];
                hv_ic_dark_thresh = hvec_vec_model_param[2].T[2];
                hv_ic_light_thresh = hvec_vec_model_param[2].T[3];
                ho_EmptyObject.Dispose();
                HOperatorSet.GenEmptyObj(out ho_EmptyObject);
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    hvec_vec_model_object = dh.Take((
                        dh.Add(new HObjectVector(1)).Insert(0, dh.Add(new HObjectVector(ho_EmptyObject)))));
                }
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    hvec_vec_model_object.Clear();
                }
                hvec_vec_model_tuple = (new HTupleVector(1).Insert(0, new HTupleVector(new HTuple())));
                hvec_vec_model_tuple.Clear();
                //pcb golden model
                hv_pcb_model_path = hv_model_path + "/golden_pcb";
                ho_pcb_mean_image.Dispose(); ho_pcb_std_image.Dispose(); ho_pcb_match_region.Dispose(); ho_pcb_inspect_region.Dispose(); ho_pcb_reject_region.Dispose(); ho_pcb_sub_region.Dispose(); ho_pcb_dark_thresh_image.Dispose(); ho_pcb_light_thresh_image.Dispose();
                load_golden_model(out ho_pcb_mean_image, out ho_pcb_std_image, out ho_pcb_match_region,
                    out ho_pcb_inspect_region, out ho_pcb_reject_region, out ho_pcb_sub_region,
                    out ho_pcb_dark_thresh_image, out ho_pcb_light_thresh_image, hv_pcb_model_path,
                    hv_pcb_sub_reg_num, hv_pcb_sobel_scale, hv_pcb_dark_thresh, hv_pcb_light_thresh,
                    out hv_pcb_ModelID, out hv_pcb_model_type, out hv_pcb_iFlag1);
                if ((int)(new HTuple(hv_pcb_iFlag1.TupleNotEqual(0))) != 0)
                {
                    hv_iFlag = -1;
                    hv_err_msg = "load pcb golden model failed";
                    ho_EmptyObject.Dispose();
                    ho_pcb_mean_image.Dispose();
                    ho_pcb_std_image.Dispose();
                    ho_pcb_match_region.Dispose();
                    ho_pcb_inspect_region.Dispose();
                    ho_pcb_reject_region.Dispose();
                    ho_pcb_sub_region.Dispose();
                    ho_pcb_dark_thresh_image.Dispose();
                    ho_pcb_light_thresh_image.Dispose();
                    ho_ic_mean_image.Dispose();
                    ho_ic_std_image.Dispose();
                    ho_ic_match_region.Dispose();
                    ho_ic_inspect_region.Dispose();
                    ho_ic_reject_region.Dispose();
                    ho_ic_sub_region.Dispose();
                    ho_ic_dark_thresh_image.Dispose();
                    ho_ic_light_thresh_image.Dispose();
                    ho_pcb_pad.Dispose();
                    ho_ic_pad.Dispose();
                    ho_chippingInspectReg.Dispose();
                    ho_region.Dispose();
                    ho_scratch_region.Dispose();
                    ho_match_inspect_region.Dispose();
                    ho_RegionUnion.Dispose();
                    ho_show_contour1.Dispose();
                    ho_show_contour.Dispose();

                    return;
                }
                ho_EmptyObject.Dispose();
                HOperatorSet.GenEmptyObj(out ho_EmptyObject);
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.ConcatObj(ho_EmptyObject, ho_pcb_mean_image, out ExpTmpOutVar_0
                        );
                    ho_EmptyObject.Dispose();
                    ho_EmptyObject = ExpTmpOutVar_0;
                }
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.ConcatObj(ho_EmptyObject, ho_pcb_std_image, out ExpTmpOutVar_0
                        );
                    ho_EmptyObject.Dispose();
                    ho_EmptyObject = ExpTmpOutVar_0;
                }
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.ConcatObj(ho_EmptyObject, ho_pcb_match_region, out ExpTmpOutVar_0
                        );
                    ho_EmptyObject.Dispose();
                    ho_EmptyObject = ExpTmpOutVar_0;
                }
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.ConcatObj(ho_EmptyObject, ho_pcb_inspect_region, out ExpTmpOutVar_0
                        );
                    ho_EmptyObject.Dispose();
                    ho_EmptyObject = ExpTmpOutVar_0;
                }
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.ConcatObj(ho_EmptyObject, ho_pcb_reject_region, out ExpTmpOutVar_0
                        );
                    ho_EmptyObject.Dispose();
                    ho_EmptyObject = ExpTmpOutVar_0;
                }
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.ConcatObj(ho_EmptyObject, ho_pcb_sub_region, out ExpTmpOutVar_0
                        );
                    ho_EmptyObject.Dispose();
                    ho_EmptyObject = ExpTmpOutVar_0;
                }
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.ConcatObj(ho_EmptyObject, ho_pcb_dark_thresh_image, out ExpTmpOutVar_0
                        );
                    ho_EmptyObject.Dispose();
                    ho_EmptyObject = ExpTmpOutVar_0;
                }
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.ConcatObj(ho_EmptyObject, ho_pcb_light_thresh_image, out ExpTmpOutVar_0
                        );
                    ho_EmptyObject.Dispose();
                    ho_EmptyObject = ExpTmpOutVar_0;
                }
                hvec_vec_model_object[0] = new HObjectVector((ho_EmptyObject).CopyObj(1, -1));
                hvec_vec_model_tuple[0] = new HTupleVector(hv_pcb_ModelID.TupleConcat(hv_pcb_model_type));
                //ic golden model
                hv_ic_model_path = hv_model_path + "/golden_ic";
                ho_ic_mean_image.Dispose(); ho_ic_std_image.Dispose(); ho_ic_match_region.Dispose(); ho_ic_inspect_region.Dispose(); ho_ic_reject_region.Dispose(); ho_ic_sub_region.Dispose(); ho_ic_dark_thresh_image.Dispose(); ho_ic_light_thresh_image.Dispose();
                load_golden_model(out ho_ic_mean_image, out ho_ic_std_image, out ho_ic_match_region,
                    out ho_ic_inspect_region, out ho_ic_reject_region, out ho_ic_sub_region,
                    out ho_ic_dark_thresh_image, out ho_ic_light_thresh_image, hv_ic_model_path,
                    hv_ic_sub_reg_num, hv_ic_sobel_scale, hv_ic_dark_thresh, hv_ic_light_thresh,
                    out hv_ic_ModelID, out hv_ic_model_type, out hv_ic_iFlag1);
                if ((int)(new HTuple(hv_ic_iFlag1.TupleNotEqual(0))) != 0)
                {
                    hv_iFlag = -1;
                    hv_err_msg = "load ic golden model failed";
                    ho_EmptyObject.Dispose();
                    ho_pcb_mean_image.Dispose();
                    ho_pcb_std_image.Dispose();
                    ho_pcb_match_region.Dispose();
                    ho_pcb_inspect_region.Dispose();
                    ho_pcb_reject_region.Dispose();
                    ho_pcb_sub_region.Dispose();
                    ho_pcb_dark_thresh_image.Dispose();
                    ho_pcb_light_thresh_image.Dispose();
                    ho_ic_mean_image.Dispose();
                    ho_ic_std_image.Dispose();
                    ho_ic_match_region.Dispose();
                    ho_ic_inspect_region.Dispose();
                    ho_ic_reject_region.Dispose();
                    ho_ic_sub_region.Dispose();
                    ho_ic_dark_thresh_image.Dispose();
                    ho_ic_light_thresh_image.Dispose();
                    ho_pcb_pad.Dispose();
                    ho_ic_pad.Dispose();
                    ho_chippingInspectReg.Dispose();
                    ho_region.Dispose();
                    ho_scratch_region.Dispose();
                    ho_match_inspect_region.Dispose();
                    ho_RegionUnion.Dispose();
                    ho_show_contour1.Dispose();
                    ho_show_contour.Dispose();

                    return;
                }
                ho_EmptyObject.Dispose();
                HOperatorSet.GenEmptyObj(out ho_EmptyObject);
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.ConcatObj(ho_EmptyObject, ho_ic_mean_image, out ExpTmpOutVar_0
                        );
                    ho_EmptyObject.Dispose();
                    ho_EmptyObject = ExpTmpOutVar_0;
                }
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.ConcatObj(ho_EmptyObject, ho_ic_std_image, out ExpTmpOutVar_0);
                    ho_EmptyObject.Dispose();
                    ho_EmptyObject = ExpTmpOutVar_0;
                }
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.ConcatObj(ho_EmptyObject, ho_ic_match_region, out ExpTmpOutVar_0
                        );
                    ho_EmptyObject.Dispose();
                    ho_EmptyObject = ExpTmpOutVar_0;
                }
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.ConcatObj(ho_EmptyObject, ho_ic_inspect_region, out ExpTmpOutVar_0
                        );
                    ho_EmptyObject.Dispose();
                    ho_EmptyObject = ExpTmpOutVar_0;
                }
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.ConcatObj(ho_EmptyObject, ho_ic_reject_region, out ExpTmpOutVar_0
                        );
                    ho_EmptyObject.Dispose();
                    ho_EmptyObject = ExpTmpOutVar_0;
                }
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.ConcatObj(ho_EmptyObject, ho_ic_sub_region, out ExpTmpOutVar_0
                        );
                    ho_EmptyObject.Dispose();
                    ho_EmptyObject = ExpTmpOutVar_0;
                }
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.ConcatObj(ho_EmptyObject, ho_ic_dark_thresh_image, out ExpTmpOutVar_0
                        );
                    ho_EmptyObject.Dispose();
                    ho_EmptyObject = ExpTmpOutVar_0;
                }
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.ConcatObj(ho_EmptyObject, ho_ic_light_thresh_image, out ExpTmpOutVar_0
                        );
                    ho_EmptyObject.Dispose();
                    ho_EmptyObject = ExpTmpOutVar_0;
                }
                hvec_vec_model_object[1] = new HObjectVector((ho_EmptyObject).CopyObj(1, -1));
                hvec_vec_model_tuple[1] = new HTupleVector(hv_ic_ModelID.TupleConcat(hv_ic_model_type));
                //pad and wire line
                hv_wire_model_path = hv_model_path + "/gold_line";
                ho_pcb_pad.Dispose();
                HOperatorSet.ReadRegion(out ho_pcb_pad, hv_wire_model_path + "/pcb_pad_region.reg");
                ho_ic_pad.Dispose();
                HOperatorSet.ReadRegion(out ho_ic_pad, hv_wire_model_path + "/ic_pad_region.reg");
                HOperatorSet.ReadTuple(hv_wire_model_path + "/ball_num_list.tup", out hv_ball_num_tup);
                HOperatorSet.CountObj(ho_pcb_pad, out hv_pcb_Number);
                HOperatorSet.CountObj(ho_ic_pad, out hv_ic_Number);
                if ((int)((new HTuple(hv_pcb_Number.TupleEqual(0))).TupleOr(new HTuple(hv_ic_Number.TupleEqual(
                    0)))) != 0)
                {
                    hv_iFlag = -1;
                    hv_err_msg = "load pad region failed";
                    ho_EmptyObject.Dispose();
                    ho_pcb_mean_image.Dispose();
                    ho_pcb_std_image.Dispose();
                    ho_pcb_match_region.Dispose();
                    ho_pcb_inspect_region.Dispose();
                    ho_pcb_reject_region.Dispose();
                    ho_pcb_sub_region.Dispose();
                    ho_pcb_dark_thresh_image.Dispose();
                    ho_pcb_light_thresh_image.Dispose();
                    ho_ic_mean_image.Dispose();
                    ho_ic_std_image.Dispose();
                    ho_ic_match_region.Dispose();
                    ho_ic_inspect_region.Dispose();
                    ho_ic_reject_region.Dispose();
                    ho_ic_sub_region.Dispose();
                    ho_ic_dark_thresh_image.Dispose();
                    ho_ic_light_thresh_image.Dispose();
                    ho_pcb_pad.Dispose();
                    ho_ic_pad.Dispose();
                    ho_chippingInspectReg.Dispose();
                    ho_region.Dispose();
                    ho_scratch_region.Dispose();
                    ho_match_inspect_region.Dispose();
                    ho_RegionUnion.Dispose();
                    ho_show_contour1.Dispose();
                    ho_show_contour.Dispose();

                    return;
                }
                hvec_vec_model_object[2] = new HObjectVector((ho_pcb_pad).CopyObj(1, -1));
                hvec_vec_model_object[3] = new HObjectVector((ho_ic_pad).CopyObj(1, -1));
                //ball_num_tup ?
                hvec_vec_model_tuple[2] = new HTupleVector(hv_ball_num_tup).Clone();
                //cntk test ******************************************
                HOperatorSet.ReadTuple(hv_wire_model_path + "/inspect_row.tup", out hv_inspect_row);
                HOperatorSet.ReadTuple(hv_wire_model_path + "/inspect_col.tup", out hv_inspect_col);
                HOperatorSet.ReadTuple(hv_wire_model_path + "/inspect_wire_num.tup", out hv_inspect_wire_num);
                hvec_vec_model_tuple[3] = new HTupleVector(((hv_inspect_wire_num.TupleConcat(
                    hv_inspect_row))).TupleConcat(hv_inspect_col));
                //*****************************************************
                //chipping model
                hv_chipping_model_path = hv_model_path + "/chipping";
                HOperatorSet.ListFiles(hv_chipping_model_path, "directories", out hv_Dres);
                ho_chippingInspectReg.Dispose();
                HOperatorSet.GenEmptyObj(out ho_chippingInspectReg);
                hv_chipping_modelID = new HTuple();
                hv_chipping_model_type = new HTuple();
                for (hv_Index = 0; (int)hv_Index <= (int)((new HTuple(hv_Dres.TupleLength())) - 1); hv_Index = (int)hv_Index + 1)
                {
                    ho_show_contour1.Dispose();
                    read_model(out ho_show_contour1, hv_Dres.TupleSelect(hv_Index), out hv_model_type,
                        out hv_model_id, out hv_def_row1, out hv_def_col1, out hv_iFlag1);
                    hv_chipping_modelID = hv_chipping_modelID.TupleConcat(hv_model_id);
                    hv_chipping_model_type = hv_chipping_model_type.TupleConcat(hv_model_type);
                    ho_region.Dispose();
                    HOperatorSet.ReadRegion(out ho_region, (hv_Dres.TupleSelect(hv_Index)) + "/inspectRegion.reg");
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.ConcatObj(ho_chippingInspectReg, ho_region, out ExpTmpOutVar_0
                            );
                        ho_chippingInspectReg.Dispose();
                        ho_chippingInspectReg = ExpTmpOutVar_0;
                    }
                }
                hvec_vec_model_object[4] = new HObjectVector((ho_chippingInspectReg).CopyObj(1, -1));
                hvec_vec_model_tuple[4] = new HTupleVector(hv_chipping_modelID.TupleConcat(
                    hv_chipping_model_type));
                //scratch model
                hv_scratch_model_path = hv_model_path + "/scratch";
                HOperatorSet.ListFiles(hv_scratch_model_path, "directories", out hv_Dres);
                ho_scratch_region.Dispose();
                HOperatorSet.GenEmptyObj(out ho_scratch_region);
                for (hv_Index = 0; (int)hv_Index <= (int)((new HTuple(hv_Dres.TupleLength())) - 1); hv_Index = (int)hv_Index + 1)
                {
                    ho_region.Dispose();
                    HOperatorSet.ReadRegion(out ho_region, (hv_Dres.TupleSelect(hv_Index)) + "/inspect_region.reg");

                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.ConcatObj(ho_scratch_region, ho_region, out ExpTmpOutVar_0);
                        ho_scratch_region.Dispose();
                        ho_scratch_region = ExpTmpOutVar_0;
                    }
                }
                hvec_vec_model_object[5] = new HObjectVector((ho_scratch_region).CopyObj(1, -1));
                //match model
                hv_match_model_path = hv_model_path + "/match";
                HOperatorSet.ListFiles(hv_match_model_path, "directories", out hv_Dres);
                ho_match_inspect_region.Dispose();
                HOperatorSet.GenEmptyObj(out ho_match_inspect_region);
                hv_match_modelID = new HTuple();
                hv_match_model_type = new HTuple();
                for (hv_Index = 0; (int)hv_Index <= (int)((new HTuple(hv_Dres.TupleLength())) - 1); hv_Index = (int)hv_Index + 1)
                {

                    ho_region.Dispose();
                    HOperatorSet.ReadRegion(out ho_region, (hv_Dres.TupleSelect(hv_Index)) + "/inspect_region.reg");
                    ho_RegionUnion.Dispose();
                    HOperatorSet.Union1(ho_region, out ho_RegionUnion);
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.ConcatObj(ho_match_inspect_region, ho_RegionUnion, out ExpTmpOutVar_0
                            );
                        ho_match_inspect_region.Dispose();
                        ho_match_inspect_region = ExpTmpOutVar_0;
                    }
                    ho_show_contour.Dispose();
                    read_model(out ho_show_contour, (hv_Dres.TupleSelect(hv_Index)) + "1", out hv_model_type0,
                        out hv_model_id0, out hv_def_row, out hv_def_col, out hv_iFlag2);
                    ho_show_contour.Dispose();
                    read_model(out ho_show_contour, (hv_Dres.TupleSelect(hv_Index)) + "2", out hv_model_type1,
                        out hv_model_id1, out hv_def_row, out hv_def_col, out hv_iFlag2);
                    if ((int)(new HTuple(hv_iFlag2.TupleNotEqual(0))) != 0)
                    {
                        hv_iFlag = -1;
                        hv_err_msg = "load match model failed";
                        ho_EmptyObject.Dispose();
                        ho_pcb_mean_image.Dispose();
                        ho_pcb_std_image.Dispose();
                        ho_pcb_match_region.Dispose();
                        ho_pcb_inspect_region.Dispose();
                        ho_pcb_reject_region.Dispose();
                        ho_pcb_sub_region.Dispose();
                        ho_pcb_dark_thresh_image.Dispose();
                        ho_pcb_light_thresh_image.Dispose();
                        ho_ic_mean_image.Dispose();
                        ho_ic_std_image.Dispose();
                        ho_ic_match_region.Dispose();
                        ho_ic_inspect_region.Dispose();
                        ho_ic_reject_region.Dispose();
                        ho_ic_sub_region.Dispose();
                        ho_ic_dark_thresh_image.Dispose();
                        ho_ic_light_thresh_image.Dispose();
                        ho_pcb_pad.Dispose();
                        ho_ic_pad.Dispose();
                        ho_chippingInspectReg.Dispose();
                        ho_region.Dispose();
                        ho_scratch_region.Dispose();
                        ho_match_inspect_region.Dispose();
                        ho_RegionUnion.Dispose();
                        ho_show_contour1.Dispose();
                        ho_show_contour.Dispose();

                        return;
                    }
                    hv_match_modelID = hv_match_modelID.TupleConcat(hv_model_id0.TupleConcat(
                        hv_model_id1));
                    hv_match_model_type = hv_match_model_type.TupleConcat(hv_model_type0.TupleConcat(
                        hv_model_type1));
                }
                hvec_vec_model_object[6] = new HObjectVector((ho_match_inspect_region).CopyObj(1, -1));
                hvec_vec_model_tuple[6] = new HTupleVector(hv_match_modelID.TupleConcat(hv_match_model_type));
                hv_err_msg = "load model completed";
                ho_EmptyObject.Dispose();
                ho_pcb_mean_image.Dispose();
                ho_pcb_std_image.Dispose();
                ho_pcb_match_region.Dispose();
                ho_pcb_inspect_region.Dispose();
                ho_pcb_reject_region.Dispose();
                ho_pcb_sub_region.Dispose();
                ho_pcb_dark_thresh_image.Dispose();
                ho_pcb_light_thresh_image.Dispose();
                ho_ic_mean_image.Dispose();
                ho_ic_std_image.Dispose();
                ho_ic_match_region.Dispose();
                ho_ic_inspect_region.Dispose();
                ho_ic_reject_region.Dispose();
                ho_ic_sub_region.Dispose();
                ho_ic_dark_thresh_image.Dispose();
                ho_ic_light_thresh_image.Dispose();
                ho_pcb_pad.Dispose();
                ho_ic_pad.Dispose();
                ho_chippingInspectReg.Dispose();
                ho_region.Dispose();
                ho_scratch_region.Dispose();
                ho_match_inspect_region.Dispose();
                ho_RegionUnion.Dispose();
                ho_show_contour1.Dispose();
                ho_show_contour.Dispose();

                return;
            }
            catch (HalconException HDevExpDefaultException)
            {
                ho_EmptyObject.Dispose();
                ho_pcb_mean_image.Dispose();
                ho_pcb_std_image.Dispose();
                ho_pcb_match_region.Dispose();
                ho_pcb_inspect_region.Dispose();
                ho_pcb_reject_region.Dispose();
                ho_pcb_sub_region.Dispose();
                ho_pcb_dark_thresh_image.Dispose();
                ho_pcb_light_thresh_image.Dispose();
                ho_ic_mean_image.Dispose();
                ho_ic_std_image.Dispose();
                ho_ic_match_region.Dispose();
                ho_ic_inspect_region.Dispose();
                ho_ic_reject_region.Dispose();
                ho_ic_sub_region.Dispose();
                ho_ic_dark_thresh_image.Dispose();
                ho_ic_light_thresh_image.Dispose();
                ho_pcb_pad.Dispose();
                ho_ic_pad.Dispose();
                ho_chippingInspectReg.Dispose();
                ho_region.Dispose();
                ho_scratch_region.Dispose();
                ho_match_inspect_region.Dispose();
                ho_RegionUnion.Dispose();
                ho_show_contour1.Dispose();
                ho_show_contour.Dispose();

                throw HDevExpDefaultException;
            }
        }
        public void LFAOI_update_thresh_image(HObjectVector/*{eObjectVector,Dim=1}*/ hvec_vec_model_object,
            out HObject ho_pcb_golden_model, out HObject ho_ic_golden_model, HTupleVector/*{eTupleVector,Dim=1}*/ hvec_vec_model_param,
            out HTuple hv_iFlag)
        {




            // Stack for temporary objects 
            HObject[] OTemp = new HObject[20];

            // Local iconic variables 

            HObject ho_pcb_model_object = null, ho_pcb_mean_image;
            HObject ho_pcb_std_image, ho_pcb_sub_region, ho_pcb_dark_thresh_image;
            HObject ho_pcb_light_thresh_image, ho_ic_model_object = null;
            HObject ho_ic_mean_image, ho_ic_std_image, ho_ic_sub_region;
            HObject ho_ic_dark_thresh_image, ho_ic_light_thresh_image;

            // Local control variables 

            HTuple hv_pcb_sub_reg_num = null, hv_pcb_sobel_scale = null;
            HTuple hv_pcb_dark_thresh = null, hv_pcb_light_thresh = null;
            HTuple hv_pcb_iFlag = null, hv_ic_sub_reg_num = null, hv_ic_sobel_scale = null;
            HTuple hv_ic_dark_thresh = null, hv_ic_light_thresh = null;
            HTuple hv_ic_iFlag = null;
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_pcb_golden_model);
            HOperatorSet.GenEmptyObj(out ho_ic_golden_model);
            HOperatorSet.GenEmptyObj(out ho_pcb_model_object);
            HOperatorSet.GenEmptyObj(out ho_pcb_mean_image);
            HOperatorSet.GenEmptyObj(out ho_pcb_std_image);
            HOperatorSet.GenEmptyObj(out ho_pcb_sub_region);
            HOperatorSet.GenEmptyObj(out ho_pcb_dark_thresh_image);
            HOperatorSet.GenEmptyObj(out ho_pcb_light_thresh_image);
            HOperatorSet.GenEmptyObj(out ho_ic_model_object);
            HOperatorSet.GenEmptyObj(out ho_ic_mean_image);
            HOperatorSet.GenEmptyObj(out ho_ic_std_image);
            HOperatorSet.GenEmptyObj(out ho_ic_sub_region);
            HOperatorSet.GenEmptyObj(out ho_ic_dark_thresh_image);
            HOperatorSet.GenEmptyObj(out ho_ic_light_thresh_image);
            try
            {
                hv_iFlag = 0;
                ho_pcb_golden_model.Dispose();
                HOperatorSet.GenEmptyObj(out ho_pcb_golden_model);
                ho_ic_golden_model.Dispose();
                HOperatorSet.GenEmptyObj(out ho_ic_golden_model);
                //pcb
                hv_pcb_sub_reg_num = hvec_vec_model_param[1].T[0];
                hv_pcb_sobel_scale = hvec_vec_model_param[1].T[1];
                hv_pcb_dark_thresh = hvec_vec_model_param[1].T[2];
                hv_pcb_light_thresh = hvec_vec_model_param[1].T[3];
                ho_pcb_model_object.Dispose();
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    ho_pcb_model_object = hvec_vec_model_object[0].O.CopyObj(1, -1);
                }
                ho_pcb_mean_image.Dispose();
                HOperatorSet.SelectObj(ho_pcb_model_object, out ho_pcb_mean_image, 1);
                ho_pcb_std_image.Dispose();
                HOperatorSet.SelectObj(ho_pcb_model_object, out ho_pcb_std_image, 2);
                ho_pcb_sub_region.Dispose();
                HOperatorSet.SelectObj(ho_pcb_model_object, out ho_pcb_sub_region, 6);
                ho_pcb_dark_thresh_image.Dispose(); ho_pcb_light_thresh_image.Dispose();
                gen_thresh_image(ho_pcb_mean_image, ho_pcb_std_image, ho_pcb_sub_region, out ho_pcb_dark_thresh_image,
                    out ho_pcb_light_thresh_image, hv_pcb_sub_reg_num, hv_pcb_dark_thresh,
                    hv_pcb_light_thresh, hv_pcb_sobel_scale, out hv_pcb_iFlag);
                if ((int)(new HTuple(hv_pcb_iFlag.TupleNotEqual(0))) != 0)
                {
                    hv_iFlag = -1;
                    ho_pcb_model_object.Dispose();
                    ho_pcb_mean_image.Dispose();
                    ho_pcb_std_image.Dispose();
                    ho_pcb_sub_region.Dispose();
                    ho_pcb_dark_thresh_image.Dispose();
                    ho_pcb_light_thresh_image.Dispose();
                    ho_ic_model_object.Dispose();
                    ho_ic_mean_image.Dispose();
                    ho_ic_std_image.Dispose();
                    ho_ic_sub_region.Dispose();
                    ho_ic_dark_thresh_image.Dispose();
                    ho_ic_light_thresh_image.Dispose();

                    return;
                }
                ho_pcb_golden_model.Dispose();
                HOperatorSet.CopyObj(ho_pcb_model_object, out ho_pcb_golden_model, 1, 6);
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.ConcatObj(ho_pcb_golden_model, ho_pcb_dark_thresh_image, out ExpTmpOutVar_0
                        );
                    ho_pcb_golden_model.Dispose();
                    ho_pcb_golden_model = ExpTmpOutVar_0;
                }
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.ConcatObj(ho_pcb_golden_model, ho_pcb_light_thresh_image, out ExpTmpOutVar_0
                        );
                    ho_pcb_golden_model.Dispose();
                    ho_pcb_golden_model = ExpTmpOutVar_0;
                }
                //ic
                hv_ic_sub_reg_num = hvec_vec_model_param[2].T[0];
                hv_ic_sobel_scale = hvec_vec_model_param[2].T[1];
                hv_ic_dark_thresh = hvec_vec_model_param[2].T[2];
                hv_ic_light_thresh = hvec_vec_model_param[2].T[3];
                ho_ic_model_object.Dispose();
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    ho_ic_model_object = hvec_vec_model_object[1].O.CopyObj(1, -1);
                }
                ho_ic_mean_image.Dispose();
                HOperatorSet.SelectObj(ho_ic_model_object, out ho_ic_mean_image, 1);
                ho_ic_std_image.Dispose();
                HOperatorSet.SelectObj(ho_ic_model_object, out ho_ic_std_image, 2);
                ho_ic_sub_region.Dispose();
                HOperatorSet.SelectObj(ho_ic_model_object, out ho_ic_sub_region, 6);
                ho_ic_dark_thresh_image.Dispose(); ho_ic_light_thresh_image.Dispose();
                gen_thresh_image(ho_ic_mean_image, ho_ic_std_image, ho_ic_sub_region, out ho_ic_dark_thresh_image,
                    out ho_ic_light_thresh_image, hv_ic_sub_reg_num, hv_ic_dark_thresh, hv_ic_light_thresh,
                    hv_ic_sobel_scale, out hv_ic_iFlag);
                if ((int)(new HTuple(hv_ic_iFlag.TupleNotEqual(0))) != 0)
                {
                    hv_iFlag = -1;
                    ho_pcb_model_object.Dispose();
                    ho_pcb_mean_image.Dispose();
                    ho_pcb_std_image.Dispose();
                    ho_pcb_sub_region.Dispose();
                    ho_pcb_dark_thresh_image.Dispose();
                    ho_pcb_light_thresh_image.Dispose();
                    ho_ic_model_object.Dispose();
                    ho_ic_mean_image.Dispose();
                    ho_ic_std_image.Dispose();
                    ho_ic_sub_region.Dispose();
                    ho_ic_dark_thresh_image.Dispose();
                    ho_ic_light_thresh_image.Dispose();

                    return;
                }
                ho_ic_golden_model.Dispose();
                HOperatorSet.CopyObj(ho_pcb_model_object, out ho_ic_golden_model, 1, 6);
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.ConcatObj(ho_ic_golden_model, ho_ic_dark_thresh_image, out ExpTmpOutVar_0
                        );
                    ho_ic_golden_model.Dispose();
                    ho_ic_golden_model = ExpTmpOutVar_0;
                }
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.ConcatObj(ho_ic_golden_model, ho_ic_light_thresh_image, out ExpTmpOutVar_0
                        );
                    ho_ic_golden_model.Dispose();
                    ho_ic_golden_model = ExpTmpOutVar_0;
                }
                ho_pcb_model_object.Dispose();
                ho_pcb_mean_image.Dispose();
                ho_pcb_std_image.Dispose();
                ho_pcb_sub_region.Dispose();
                ho_pcb_dark_thresh_image.Dispose();
                ho_pcb_light_thresh_image.Dispose();
                ho_ic_model_object.Dispose();
                ho_ic_mean_image.Dispose();
                ho_ic_std_image.Dispose();
                ho_ic_sub_region.Dispose();
                ho_ic_dark_thresh_image.Dispose();
                ho_ic_light_thresh_image.Dispose();

                return;
            }
            catch (HalconException HDevExpDefaultException)
            {
                ho_pcb_model_object.Dispose();
                ho_pcb_mean_image.Dispose();
                ho_pcb_std_image.Dispose();
                ho_pcb_sub_region.Dispose();
                ho_pcb_dark_thresh_image.Dispose();
                ho_pcb_light_thresh_image.Dispose();
                ho_ic_model_object.Dispose();
                ho_ic_mean_image.Dispose();
                ho_ic_std_image.Dispose();
                ho_ic_sub_region.Dispose();
                ho_ic_dark_thresh_image.Dispose();
                ho_ic_light_thresh_image.Dispose();

                throw HDevExpDefaultException;
            }
        }
        public void pcb_ball_detect_unit(HObject ho_Image, HObject ho_ICPad, HObject ho_PCBPad,
            HTuple hv_BallNum, HTuple hv_PCBRadiusLow, HTuple hv_PCBRadiusHigh, HTuple hv_GrayThresh,
            HTuple hv_MeasureRadius, HTuple hv_MeasureLen1, HTuple hv_MeasureLen2, HTuple hv_pcb_closing_radius,
            HTuple hv_pcb_fill_up_max, out HTuple hv_PCBBallRow, out HTuple hv_PCBBallCol,
            out HTuple hv_PCBBallRadius, out HTuple hv_iFlag)
        {




            // Local iconic variables 

            HObject ho_ImageReduced, ho_Region, ho_RegionClosing;
            HObject ho_RegionFillUp, ho_RegionOpening = null, ho_ConnectedRegions = null;
            HObject ho_SelectedRegions = null, ho_Rectangle = null, ho_RegionIntersection = null;
            HObject ho_RegionUnion = null;

            // Local control variables 

            HTuple hv_Min = null, hv_Max = null, hv_Range = null;
            HTuple hv_radius = null, hv_Area = null, hv_PCB_row = new HTuple();
            HTuple hv_PCB_col = new HTuple(), hv_Row = new HTuple();
            HTuple hv_Column = new HTuple(), hv_Phi = new HTuple();
            HTuple hv_len2 = new HTuple(), hv_Area1 = new HTuple();
            HTuple hv_ic_row = new HTuple(), hv_ic_col = new HTuple();
            HTuple hv_pcb_row = new HTuple(), hv_pcb_col = new HTuple();
            HTuple hv_HomMat2DIdentity = new HTuple(), hv_HomMat2DRotate = new HTuple();
            HTuple hv_pcb_row_proj = new HTuple(), hv_pcb_col_proj = new HTuple();
            HTuple hv_Indices = new HTuple(), hv_is_refine = null;
            HTuple hv_Width = null, hv_Height = null, hv_ind = null;
            HTuple hv_MetrologyHandle = new HTuple(), hv_Index = new HTuple();
            HTuple hv_Circle = new HTuple();
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_ImageReduced);
            HOperatorSet.GenEmptyObj(out ho_Region);
            HOperatorSet.GenEmptyObj(out ho_RegionClosing);
            HOperatorSet.GenEmptyObj(out ho_RegionFillUp);
            HOperatorSet.GenEmptyObj(out ho_RegionOpening);
            HOperatorSet.GenEmptyObj(out ho_ConnectedRegions);
            HOperatorSet.GenEmptyObj(out ho_SelectedRegions);
            HOperatorSet.GenEmptyObj(out ho_Rectangle);
            HOperatorSet.GenEmptyObj(out ho_RegionIntersection);
            HOperatorSet.GenEmptyObj(out ho_RegionUnion);
            try
            {
                //*********************************************************************
                //         detect and measure the ic ball
                //input:    Image, ic_pad,   *          ic_radius_low, ic_radius_high
                //output：  ic_ball,   *          iFlag(0:ic ball detect successed, -1:ic ball not found,                            -2:small ic ball , -3: big ic ball)
                //*********************************************************************
                hv_iFlag = new HTuple();
                hv_PCBBallRow = new HTuple();
                hv_PCBBallCol = new HTuple();
                hv_PCBBallRadius = new HTuple();
                //********* coarse measure***************
                ho_ImageReduced.Dispose();
                HOperatorSet.ReduceDomain(ho_Image, ho_PCBPad, out ho_ImageReduced);
                HOperatorSet.MinMaxGray(ho_PCBPad, ho_ImageReduced, 10, out hv_Min, out hv_Max,
                    out hv_Range);
                ho_Region.Dispose();
                HOperatorSet.Threshold(ho_ImageReduced, out ho_Region, ((hv_GrayThresh.TupleConcat(
                    (hv_Min + hv_Max) * 0.5))).TupleMax(), 255);
                ho_RegionClosing.Dispose();
                HOperatorSet.ClosingCircle(ho_Region, out ho_RegionClosing, hv_pcb_closing_radius);
                ho_RegionFillUp.Dispose();
                HOperatorSet.FillUpShape(ho_RegionClosing, out ho_RegionFillUp, "area", 1,
                    hv_pcb_fill_up_max);
                hv_radius = hv_PCBRadiusLow - 1;
                hv_Area = new HTuple();
                if ((int)(new HTuple(hv_BallNum.TupleEqual(1))) != 0)
                {
                    while ((int)((new HTuple(hv_radius.TupleLess(hv_PCBRadiusHigh))).TupleAnd(
                        (new HTuple((new HTuple(hv_Area.TupleLength())).TupleNotEqual(1))).TupleOr(
                        new HTuple(hv_Area.TupleEqual(0))))) != 0)
                    {
                        ho_RegionOpening.Dispose();
                        HOperatorSet.OpeningCircle(ho_RegionFillUp, out ho_RegionOpening, hv_radius);
                        ho_ConnectedRegions.Dispose();
                        HOperatorSet.Connection(ho_RegionOpening, out ho_ConnectedRegions);
                        HOperatorSet.AreaCenter(ho_ConnectedRegions, out hv_Area, out hv_PCB_row,
                            out hv_PCB_col);
                        hv_radius = hv_radius + 1;
                    }
                    if ((int)(new HTuple(hv_Area.TupleEqual(0))) != 0)
                    {
                        hv_iFlag = -1;
                        hv_PCBBallRow = 0;
                        hv_PCBBallCol = 0;
                        hv_PCBBallRadius = 0;
                        ho_ImageReduced.Dispose();
                        ho_Region.Dispose();
                        ho_RegionClosing.Dispose();
                        ho_RegionFillUp.Dispose();
                        ho_RegionOpening.Dispose();
                        ho_ConnectedRegions.Dispose();
                        ho_SelectedRegions.Dispose();
                        ho_Rectangle.Dispose();
                        ho_RegionIntersection.Dispose();
                        ho_RegionUnion.Dispose();

                        return;
                    }
                }
                else
                {
                    //get ball center and split balls
                    while ((int)((new HTuple(hv_radius.TupleLess(hv_PCBRadiusHigh))).TupleAnd(
                        (new HTuple((new HTuple(hv_Area.TupleLength())).TupleNotEqual(hv_BallNum))).TupleOr(
                        new HTuple(hv_Area.TupleEqual(0))))) != 0)
                    {
                        ho_RegionOpening.Dispose();
                        HOperatorSet.OpeningCircle(ho_RegionFillUp, out ho_RegionOpening, hv_radius);
                        ho_ConnectedRegions.Dispose();
                        HOperatorSet.Connection(ho_RegionOpening, out ho_ConnectedRegions);
                        ho_SelectedRegions.Dispose();
                        HOperatorSet.SelectShape(ho_ConnectedRegions, out ho_SelectedRegions, "area",
                            "and", (hv_PCBRadiusLow * 3.14) * hv_PCBRadiusLow, 99999);
                        HOperatorSet.AreaCenter(ho_SelectedRegions, out hv_Area, out hv_Row, out hv_Column);
                        if ((int)((new HTuple(hv_Area.TupleGreater(0))).TupleAnd(new HTuple((new HTuple(hv_Area.TupleLength()
                            )).TupleLess(hv_BallNum)))) != 0)
                        {
                            HOperatorSet.OrientationRegion(ho_RegionOpening, out hv_Phi);
                            hv_len2 = hv_PCBRadiusLow - 1;
                            while ((int)((new HTuple(hv_len2.TupleLess(hv_PCBRadiusHigh))).TupleAnd(
                                new HTuple((new HTuple(hv_Area.TupleLength())).TupleNotEqual(hv_BallNum)))) != 0)
                            {
                                ho_Rectangle.Dispose();
                                HOperatorSet.GenRectangle2(out ho_Rectangle, 50, 50, hv_Phi, 1, hv_len2);
                                ho_RegionIntersection.Dispose();
                                HOperatorSet.Opening(ho_RegionOpening, ho_Rectangle, out ho_RegionIntersection
                                    );
                                ho_ConnectedRegions.Dispose();
                                HOperatorSet.Connection(ho_RegionIntersection, out ho_ConnectedRegions
                                    );
                                HOperatorSet.AreaCenter(ho_ConnectedRegions, out hv_Area, out hv_Row,
                                    out hv_Column);
                                hv_len2 = hv_len2 + 0.5;
                            }
                        }
                        hv_radius = hv_radius + 1;
                    }
                    if ((int)((new HTuple((new HTuple(hv_Area.TupleLength())).TupleLess(hv_BallNum))).TupleOr(
                        new HTuple(hv_Area.TupleEqual(0)))) != 0)
                    {
                        hv_iFlag = -1;
                        HOperatorSet.TupleGenConst(hv_BallNum, 0, out hv_PCBBallRow);
                        HOperatorSet.TupleGenConst(hv_BallNum, 0, out hv_PCBBallCol);
                        HOperatorSet.TupleGenConst(hv_BallNum, 0, out hv_PCBBallRadius);
                        ho_ImageReduced.Dispose();
                        ho_Region.Dispose();
                        ho_RegionClosing.Dispose();
                        ho_RegionFillUp.Dispose();
                        ho_RegionOpening.Dispose();
                        ho_ConnectedRegions.Dispose();
                        ho_SelectedRegions.Dispose();
                        ho_Rectangle.Dispose();
                        ho_RegionIntersection.Dispose();
                        ho_RegionUnion.Dispose();

                        return;
                    }
                    //sort ball
                    ho_RegionUnion.Dispose();
                    HOperatorSet.Union1(ho_ICPad, out ho_RegionUnion);
                    HOperatorSet.AreaCenter(ho_RegionUnion, out hv_Area1, out hv_ic_row, out hv_ic_col);
                    HOperatorSet.AreaCenter(ho_PCBPad, out hv_Area1, out hv_pcb_row, out hv_pcb_col);
                    HOperatorSet.AngleLl(hv_ic_row, hv_ic_col, hv_pcb_row, hv_pcb_col, 0, 1,
                        0, 0, out hv_Phi);
                    HOperatorSet.HomMat2dIdentity(out hv_HomMat2DIdentity);
                    HOperatorSet.HomMat2dRotate(hv_HomMat2DIdentity, hv_Phi, hv_ic_row, hv_ic_col,
                        out hv_HomMat2DRotate);
                    //affine_trans_point_2d (HomMat2DRotate, ic_row, ic_col, ic_row_proj, ic_col_proj)
                    HOperatorSet.AffineTransPoint2d(hv_HomMat2DRotate, hv_Row, hv_Column, out hv_pcb_row_proj,
                        out hv_pcb_col_proj);
                    HOperatorSet.TupleSortIndex(hv_pcb_row_proj, out hv_Indices);
                    hv_PCB_row = hv_Row.TupleSelect(hv_Indices);
                    hv_PCB_col = hv_Column.TupleSelect(hv_Indices);
                }
                //********** refine by measure circle ********************
                hv_is_refine = 1;
                HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height);
                HTuple end_val74 = hv_BallNum - 1;
                HTuple step_val74 = 1;
                for (hv_ind = 0; hv_ind.Continue(end_val74, step_val74); hv_ind = hv_ind.TupleAdd(step_val74))
                {
                    HOperatorSet.CreateMetrologyModel(out hv_MetrologyHandle);
                    HOperatorSet.SetMetrologyModelImageSize(hv_MetrologyHandle, hv_Width, hv_Height);
                    HOperatorSet.AddMetrologyObjectCircleMeasure(hv_MetrologyHandle, hv_PCB_row.TupleSelect(
                        hv_ind), hv_PCB_col.TupleSelect(hv_ind), hv_MeasureRadius, hv_MeasureLen1,
                        hv_MeasureLen2, 1, 30, ((new HTuple("measure_transition")).TupleConcat(
                        "measure_distance")).TupleConcat("min_score"), ((new HTuple("negative")).TupleConcat(
                        3)).TupleConcat(0.5), out hv_Index);
                    HOperatorSet.ApplyMetrologyModel(ho_Image, hv_MetrologyHandle);
                    HOperatorSet.GetMetrologyObjectResult(hv_MetrologyHandle, hv_Index, "all",
                        "result_type", "all_param", out hv_Circle);
                    //get_metrology_object_measures (Contours, MetrologyHandle, 'all', 'all', Row1, Column1)
                    //gen_cross_contour_xld (Cross, Row1, Column1, 6, 0.785398)
                    HOperatorSet.ClearMetrologyModel(hv_MetrologyHandle);
                    if ((int)(new HTuple((new HTuple(hv_Circle.TupleLength())).TupleEqual(3))) != 0)
                    {
                        if ((int)(new HTuple(((hv_Circle.TupleSelect(2))).TupleLess(hv_PCBRadiusLow))) != 0)
                        {
                            hv_iFlag = hv_iFlag.TupleConcat(-2);
                            hv_PCBBallRow = hv_PCBBallRow.TupleConcat(0);
                            hv_PCBBallCol = hv_PCBBallCol.TupleConcat(0);
                            hv_PCBBallRadius = hv_PCBBallRadius.TupleConcat(0);
                        }
                        else if ((int)(new HTuple(((hv_Circle.TupleSelect(2))).TupleGreater(
                            hv_PCBRadiusHigh))) != 0)
                        {
                            hv_iFlag = hv_iFlag.TupleConcat(-3);
                            hv_PCBBallRow = hv_PCBBallRow.TupleConcat(0);
                            hv_PCBBallCol = hv_PCBBallCol.TupleConcat(0);
                            hv_PCBBallRadius = hv_PCBBallRadius.TupleConcat(0);
                        }
                        else
                        {
                            hv_iFlag = hv_iFlag.TupleConcat(0);
                            hv_PCBBallRow = hv_PCBBallRow.TupleConcat(hv_Circle.TupleSelect(0));
                            hv_PCBBallCol = hv_PCBBallCol.TupleConcat(hv_Circle.TupleSelect(1));
                            hv_PCBBallRadius = hv_PCBBallRadius.TupleConcat(hv_Circle.TupleSelect(
                                2));
                            //gen_circle (Circle1, Circle[0], Circle[1], Circle[2])
                        }
                    }
                    else
                    {
                        hv_iFlag = hv_iFlag.TupleConcat(-1);
                        hv_PCBBallRow = hv_PCBBallRow.TupleConcat(0);
                        hv_PCBBallCol = hv_PCBBallCol.TupleConcat(0);
                        hv_PCBBallRadius = hv_PCBBallRadius.TupleConcat(0);
                    }
                }
                ho_ImageReduced.Dispose();
                ho_Region.Dispose();
                ho_RegionClosing.Dispose();
                ho_RegionFillUp.Dispose();
                ho_RegionOpening.Dispose();
                ho_ConnectedRegions.Dispose();
                ho_SelectedRegions.Dispose();
                ho_Rectangle.Dispose();
                ho_RegionIntersection.Dispose();
                ho_RegionUnion.Dispose();

                return;
            }
            catch (HalconException HDevExpDefaultException)
            {
                ho_ImageReduced.Dispose();
                ho_Region.Dispose();
                ho_RegionClosing.Dispose();
                ho_RegionFillUp.Dispose();
                ho_RegionOpening.Dispose();
                ho_ConnectedRegions.Dispose();
                ho_SelectedRegions.Dispose();
                ho_Rectangle.Dispose();
                ho_RegionIntersection.Dispose();
                ho_RegionUnion.Dispose();

                throw HDevExpDefaultException;
            }
        }

        public void lead_wire_detect(HObject ho_Image, HObject ho_ic_pad, HTuple hv_dilation_size,
            HTuple hv_ic_ball_row, HTuple hv_ic_ball_col, HTuple hv_ic_ball_radius, HTuple hv_pcb_ball_row,
            HTuple hv_pcb_ball_col, HTuple hv_line_width, HTuple hv_line_contrast, HTuple hv_length_thresh,
            HTuple hv_angle_thresh, HTuple hv_distance_thresh, out HTuple hv_iFlag)
        {




            // Stack for temporary objects 
            HObject[] OTemp = new HObject[20];

            // Local iconic variables 

            HObject ho_Rectangle, ho_Circle, ho_RegionDifference;
            HObject ho_ImageReduced, ho_Lines, ho_ContoursSplit, ho_SelectedContours;
            HObject ho_AdjacentContours = null, ho_ObjectSelected = null;
            HObject ho_SelectedContours_;

            // Local control variables 

            HTuple hv_Sigma = null, hv_Low = null, hv_High = null;
            HTuple hv_Phi = null, hv_Number = null, hv_Index = new HTuple();
            HTuple hv_DistanceMin = new HTuple(), hv_DistanceMax = new HTuple();
            HTuple hv_Length = new HTuple(), hv_distance_min = null;
            HTuple hv_distance_max = null, hv_RowBegin = new HTuple();
            HTuple hv_ColBegin = new HTuple(), hv_RowEnd = new HTuple();
            HTuple hv_ColEnd = new HTuple(), hv_Nr = new HTuple();
            HTuple hv_Nc = new HTuple(), hv_Dist = new HTuple(), hv_Distance1 = new HTuple();
            HTuple hv_Distance2 = new HTuple(), hv_Angle1 = new HTuple();
            HTuple hv_Angle2 = new HTuple(), hv_DistanceMinLine = new HTuple();
            HTuple hv_DistanceMax1 = new HTuple();
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_Rectangle);
            HOperatorSet.GenEmptyObj(out ho_Circle);
            HOperatorSet.GenEmptyObj(out ho_RegionDifference);
            HOperatorSet.GenEmptyObj(out ho_ImageReduced);
            HOperatorSet.GenEmptyObj(out ho_Lines);
            HOperatorSet.GenEmptyObj(out ho_ContoursSplit);
            HOperatorSet.GenEmptyObj(out ho_SelectedContours);
            HOperatorSet.GenEmptyObj(out ho_AdjacentContours);
            HOperatorSet.GenEmptyObj(out ho_ObjectSelected);
            HOperatorSet.GenEmptyObj(out ho_SelectedContours_);
            try
            {
                hv_iFlag = 0;
                //dilation_rectangle1 (ic_pad, RegionDilation, dilation_size, dilation_size)
                ho_Rectangle.Dispose();
                HOperatorSet.GenRectangle2(out ho_Rectangle, hv_ic_ball_row, hv_ic_ball_col,
                    0, hv_dilation_size, hv_dilation_size);
                ho_Circle.Dispose();
                HOperatorSet.GenCircle(out ho_Circle, hv_ic_ball_row, hv_ic_ball_col, hv_ic_ball_radius);
                ho_RegionDifference.Dispose();
                HOperatorSet.Difference(ho_Rectangle, ho_Circle, out ho_RegionDifference);
                ho_ImageReduced.Dispose();
                HOperatorSet.ReduceDomain(ho_Image, ho_RegionDifference, out ho_ImageReduced
                    );
                calculate_lines_gauss_parameters(hv_line_width, hv_line_contrast, out hv_Sigma,
                    out hv_Low, out hv_High);
                ho_Lines.Dispose();
                HOperatorSet.LinesGauss(ho_ImageReduced, out ho_Lines, hv_Sigma, hv_Low, hv_High,
                    "light", "true", "bar-shaped", "true");
                ho_ContoursSplit.Dispose();
                HOperatorSet.SegmentContoursXld(ho_Lines, out ho_ContoursSplit, "lines", 3,
                    1, 1);
                HOperatorSet.LineOrientation(hv_pcb_ball_row, hv_pcb_ball_col, hv_ic_ball_row,
                    hv_ic_ball_col, out hv_Phi);
                ho_SelectedContours.Dispose();
                HOperatorSet.SelectContoursXld(ho_ContoursSplit, out ho_SelectedContours, "contour_length",
                    hv_length_thresh, 999, -0.5, 0.5);
                HOperatorSet.CountObj(ho_SelectedContours, out hv_Number);
                if ((int)(new HTuple(hv_Number.TupleGreater(1))) != 0)
                {
                    ho_AdjacentContours.Dispose();
                    HOperatorSet.GenEmptyObj(out ho_AdjacentContours);
                    HTuple end_val14 = hv_Number;
                    HTuple step_val14 = 1;
                    for (hv_Index = 1; hv_Index.Continue(end_val14, step_val14); hv_Index = hv_Index.TupleAdd(step_val14))
                    {
                        ho_ObjectSelected.Dispose();
                        HOperatorSet.SelectObj(ho_SelectedContours, out ho_ObjectSelected, hv_Index);
                        HOperatorSet.DistancePc(ho_ObjectSelected, hv_ic_ball_row, hv_ic_ball_col,
                            out hv_DistanceMin, out hv_DistanceMax);
                        HOperatorSet.LengthXld(ho_ObjectSelected, out hv_Length);
                        if ((int)((new HTuple(hv_DistanceMin.TupleLess(hv_ic_ball_radius - 0.5))).TupleAnd(
                            new HTuple(hv_Length.TupleGreater(4)))) != 0)
                        {
                            {
                                HObject ExpTmpOutVar_0;
                                HOperatorSet.ConcatObj(ho_AdjacentContours, ho_ObjectSelected, out ExpTmpOutVar_0
                                    );
                                ho_AdjacentContours.Dispose();
                                ho_AdjacentContours = ExpTmpOutVar_0;
                            }
                        }
                    }
                    HOperatorSet.CountObj(ho_AdjacentContours, out hv_Number);
                    if ((int)(new HTuple(hv_Number.TupleGreater(1))) != 0)
                    {
                        hv_iFlag = -1;
                        ho_Rectangle.Dispose();
                        ho_Circle.Dispose();
                        ho_RegionDifference.Dispose();
                        ho_ImageReduced.Dispose();
                        ho_Lines.Dispose();
                        ho_ContoursSplit.Dispose();
                        ho_SelectedContours.Dispose();
                        ho_AdjacentContours.Dispose();
                        ho_ObjectSelected.Dispose();
                        ho_SelectedContours_.Dispose();

                        return;
                    }
                }
                ho_SelectedContours_.Dispose();
                HOperatorSet.SelectContoursXld(ho_SelectedContours, out ho_SelectedContours_,
                    "direction", hv_Phi - hv_angle_thresh, hv_Phi + hv_angle_thresh, -0.5, 0.5);
                HOperatorSet.CountObj(ho_SelectedContours_, out hv_Number);
                if ((int)(new HTuple(hv_Number.TupleEqual(0))) != 0)
                {
                    hv_iFlag = -1;
                    ho_Rectangle.Dispose();
                    ho_Circle.Dispose();
                    ho_RegionDifference.Dispose();
                    ho_ImageReduced.Dispose();
                    ho_Lines.Dispose();
                    ho_ContoursSplit.Dispose();
                    ho_SelectedContours.Dispose();
                    ho_AdjacentContours.Dispose();
                    ho_ObjectSelected.Dispose();
                    ho_SelectedContours_.Dispose();

                    return;
                }
                hv_distance_min = new HTuple();
                hv_distance_max = new HTuple();

                HTuple end_val37 = hv_Number;
                HTuple step_val37 = 1;
                for (hv_Index = 1; hv_Index.Continue(end_val37, step_val37); hv_Index = hv_Index.TupleAdd(step_val37))
                {
                    ho_ObjectSelected.Dispose();
                    HOperatorSet.SelectObj(ho_SelectedContours_, out ho_ObjectSelected, hv_Index);
                    //
                    HOperatorSet.FitLineContourXld(ho_ObjectSelected, "tukey", -1, 0, 5, 2, out hv_RowBegin,
                        out hv_ColBegin, out hv_RowEnd, out hv_ColEnd, out hv_Nr, out hv_Nc,
                        out hv_Dist);
                    HOperatorSet.DistancePp(hv_RowBegin, hv_ColBegin, hv_ic_ball_row, hv_ic_ball_col,
                        out hv_Distance1);
                    HOperatorSet.DistancePp(hv_RowEnd, hv_ColEnd, hv_ic_ball_row, hv_ic_ball_col,
                        out hv_Distance2);
                    hv_DistanceMin = ((hv_Distance1.TupleConcat(hv_Distance2))).TupleMin();
                    HOperatorSet.AngleLl(hv_ic_ball_row, hv_ic_ball_col, hv_pcb_ball_row, hv_pcb_ball_col,
                        hv_ic_ball_row, hv_ic_ball_col, hv_RowBegin, hv_ColBegin, out hv_Angle1);
                    HOperatorSet.AngleLl(hv_ic_ball_row, hv_ic_ball_col, hv_pcb_ball_row, hv_pcb_ball_col,
                        hv_ic_ball_row, hv_ic_ball_col, hv_RowEnd, hv_ColEnd, out hv_Angle2);
                    if ((int)(new HTuple(((((((hv_Angle1.TupleAbs())).TupleConcat(hv_Angle2.TupleAbs()
                        ))).TupleMax())).TupleGreater(1.5709))) != 0)
                    {
                        hv_DistanceMin = 999;
                        continue;
                    }
                    HOperatorSet.DistanceLc(ho_ObjectSelected, hv_ic_ball_row, hv_ic_ball_col,
                        hv_pcb_ball_row, hv_pcb_ball_col, out hv_DistanceMinLine, out hv_DistanceMax1);
                    if ((int)(new HTuple(hv_DistanceMinLine.TupleGreater(hv_distance_thresh + 1))) != 0)
                    {
                        hv_DistanceMin = 999;
                        continue;
                    }
                    //
                    hv_distance_min = hv_distance_min.TupleConcat(hv_DistanceMin);
                }
                if ((int)(new HTuple((new HTuple(hv_distance_min.TupleLength())).TupleEqual(
                    0))) != 0)
                {
                    hv_iFlag = -1;
                    ho_Rectangle.Dispose();
                    ho_Circle.Dispose();
                    ho_RegionDifference.Dispose();
                    ho_ImageReduced.Dispose();
                    ho_Lines.Dispose();
                    ho_ContoursSplit.Dispose();
                    ho_SelectedContours.Dispose();
                    ho_AdjacentContours.Dispose();
                    ho_ObjectSelected.Dispose();
                    ho_SelectedContours_.Dispose();

                    return;
                }
                if ((int)(new HTuple(((hv_distance_min.TupleMin())).TupleGreater(hv_distance_thresh + hv_ic_ball_radius))) != 0)
                {
                    hv_iFlag = -1;
                }
                ho_Rectangle.Dispose();
                ho_Circle.Dispose();
                ho_RegionDifference.Dispose();
                ho_ImageReduced.Dispose();
                ho_Lines.Dispose();
                ho_ContoursSplit.Dispose();
                ho_SelectedContours.Dispose();
                ho_AdjacentContours.Dispose();
                ho_ObjectSelected.Dispose();
                ho_SelectedContours_.Dispose();

                return;
            }
            catch (HalconException HDevExpDefaultException)
            {
                ho_Rectangle.Dispose();
                ho_Circle.Dispose();
                ho_RegionDifference.Dispose();
                ho_ImageReduced.Dispose();
                ho_Lines.Dispose();
                ho_ContoursSplit.Dispose();
                ho_SelectedContours.Dispose();
                ho_AdjacentContours.Dispose();
                ho_ObjectSelected.Dispose();
                ho_SelectedContours_.Dispose();

                throw HDevExpDefaultException;
            }
        }
        public void track_wire_unit(HObject ho_Image, out HObject ho_TrackRegion, out HObject ho_Wire,
            HTuple hv_LPBallRow, HTuple hv_LPBallCol, HTuple hv_LPBallRadius, HTuple hv_ICBallRow,
            HTuple hv_ICBallCol, HTuple hv_ICBallRadius, HTuple hv_SearchLen, HTuple hv_ClipLen,
            HTuple hv_LineWidth, HTuple hv_LineContrast, HTuple hv_MinSegLen, HTuple hv_AngleExtent,
            HTuple hv_MaxGap, out HTuple hv_iFlag)
        {




            // Stack for temporary objects 
            HObject[] OTemp = new HObject[20];

            // Local iconic variables 

            HObject ho_ImageReduced1, ho_Lines, ho_LinesDark = null;
            HObject ho_ContoursSplit, ho_SelectedContoursLength, ho_SelectedContours;
            HObject ho_ContoursSort, ho_ObjectSelected, ho_ContourLP;
            HObject ho_ContourIC, ho_ObjectConcat, ho_ObjectsConcat;
            HObject ho_UnionContour;

            // Local control variables 

            HTuple hv_Mode = null, hv_IMAX = null, hv_DistanceL = null;
            HTuple hv_Phi = null, hv_Sigma = null, hv_Low = null, hv_High = null;
            HTuple hv_is_dark_line_extent = null, hv_Number = null;
            HTuple hv_RowBegin_ = null, hv_ColBegin_ = null, hv_RowEnd_ = null;
            HTuple hv_ColEnd_ = null, hv_Indices = null, hv_Length = null;
            HTuple hv_SegLength = null, hv_RowBegin = null, hv_ColBegin = null;
            HTuple hv_RowEnd = null, hv_ColEnd = null, hv_SegNum = null;
            HTuple hv_DistanceMatrix = null, hv_i = null, hv_Row0 = new HTuple();
            HTuple hv_Col0 = new HTuple(), hv_Row1 = new HTuple();
            HTuple hv_Col1 = new HTuple(), hv_j = new HTuple(), hv_DistanceMin = new HTuple();
            HTuple hv_Angle = new HTuple(), hv_Distance = null, hv_VisitedInd = null;
            HTuple hv_Less = null, hv_Previous = null, hv_DistMin = new HTuple();
            HTuple hv_Prev = new HTuple(), hv_DistanceCurrent = new HTuple();
            HTuple hv_Road = null, hv_ind = null, hv_WireLength = null;
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_TrackRegion);
            HOperatorSet.GenEmptyObj(out ho_Wire);
            HOperatorSet.GenEmptyObj(out ho_ImageReduced1);
            HOperatorSet.GenEmptyObj(out ho_Lines);
            HOperatorSet.GenEmptyObj(out ho_LinesDark);
            HOperatorSet.GenEmptyObj(out ho_ContoursSplit);
            HOperatorSet.GenEmptyObj(out ho_SelectedContoursLength);
            HOperatorSet.GenEmptyObj(out ho_SelectedContours);
            HOperatorSet.GenEmptyObj(out ho_ContoursSort);
            HOperatorSet.GenEmptyObj(out ho_ObjectSelected);
            HOperatorSet.GenEmptyObj(out ho_ContourLP);
            HOperatorSet.GenEmptyObj(out ho_ContourIC);
            HOperatorSet.GenEmptyObj(out ho_ObjectConcat);
            HOperatorSet.GenEmptyObj(out ho_ObjectsConcat);
            HOperatorSet.GenEmptyObj(out ho_UnionContour);
            try
            {
                hv_iFlag = 0;
                hv_Mode = "distance";
                ho_Wire.Dispose();
                HOperatorSet.GenEmptyObj(out ho_Wire);
                ho_TrackRegion.Dispose();
                HOperatorSet.GenEmptyObj(out ho_TrackRegion);
                hv_IMAX = 9999;
                //gen bond wire detect region
                HOperatorSet.DistancePp(hv_LPBallRow, hv_LPBallCol, hv_ICBallRow, hv_ICBallCol,
                    out hv_DistanceL);
                HOperatorSet.LineOrientation(hv_LPBallRow, hv_LPBallCol, hv_ICBallRow, hv_ICBallCol,
                    out hv_Phi);
                ho_TrackRegion.Dispose();
                HOperatorSet.GenRectangle2(out ho_TrackRegion, ((hv_LPBallRow.TupleConcat(hv_ICBallRow))).TupleMean()
                    , ((hv_LPBallCol.TupleConcat(hv_ICBallCol))).TupleMean(), hv_Phi, (hv_DistanceL / 2.0) - hv_ClipLen,
                    hv_SearchLen);
                ho_ImageReduced1.Dispose();
                HOperatorSet.ReduceDomain(ho_Image, ho_TrackRegion, out ho_ImageReduced1);
                //extract lines
                calculate_lines_gauss_parameters(hv_LineWidth, hv_LineContrast, out hv_Sigma,
                    out hv_Low, out hv_High);
                ho_Lines.Dispose();
                HOperatorSet.LinesGauss(ho_ImageReduced1, out ho_Lines, hv_Sigma, hv_Low, hv_High,
                    "light", "false", "none", "false");
                //
                hv_is_dark_line_extent = 0;
                if ((int)(hv_is_dark_line_extent) != 0)
                {
                    calculate_lines_gauss_parameters(hv_LineWidth - 2, hv_LineContrast, out hv_Sigma,
                        out hv_Low, out hv_High);
                    ho_LinesDark.Dispose();
                    HOperatorSet.LinesGauss(ho_ImageReduced1, out ho_LinesDark, hv_Sigma, hv_Low,
                        hv_High, "dark", "false", "none", "false");
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.ConcatObj(ho_Lines, ho_LinesDark, out ExpTmpOutVar_0);
                        ho_Lines.Dispose();
                        ho_Lines = ExpTmpOutVar_0;
                    }
                }
                //
                ho_ContoursSplit.Dispose();
                HOperatorSet.SegmentContoursXld(ho_Lines, out ho_ContoursSplit, "lines", 0,
                    1.5, 1);
                ho_SelectedContoursLength.Dispose();
                HOperatorSet.SelectContoursXld(ho_ContoursSplit, out ho_SelectedContoursLength,
                    "contour_length", hv_MinSegLen, 9999, -0.5, 0.5);
                ho_SelectedContours.Dispose();
                HOperatorSet.SelectContoursXld(ho_SelectedContoursLength, out ho_SelectedContours,
                    "direction", hv_Phi - hv_AngleExtent, hv_Phi + hv_AngleExtent, -0.5, 0.5);
                HOperatorSet.CountObj(ho_SelectedContours, out hv_Number);
                if ((int)(new HTuple(hv_Number.TupleLess(1))) != 0)
                {
                    hv_iFlag = -1;
                    ho_ImageReduced1.Dispose();
                    ho_Lines.Dispose();
                    ho_LinesDark.Dispose();
                    ho_ContoursSplit.Dispose();
                    ho_SelectedContoursLength.Dispose();
                    ho_SelectedContours.Dispose();
                    ho_ContoursSort.Dispose();
                    ho_ObjectSelected.Dispose();
                    ho_ContourLP.Dispose();
                    ho_ContourIC.Dispose();
                    ho_ObjectConcat.Dispose();
                    ho_ObjectsConcat.Dispose();
                    ho_UnionContour.Dispose();

                    return;
                }
                //track bond wire
                fit_line_contour_sort(ho_SelectedContours, hv_LPBallRow, hv_LPBallCol, out hv_RowBegin_,
                    out hv_ColBegin_, out hv_RowEnd_, out hv_ColEnd_, out hv_Indices);
                ho_ContoursSort.Dispose();
                HOperatorSet.SelectObj(ho_SelectedContours, out ho_ContoursSort, hv_Indices + 1);
                HOperatorSet.LengthXld(ho_ContoursSort, out hv_Length);
                hv_SegLength = new HTuple();
                hv_SegLength[0] = 0;
                hv_SegLength = hv_SegLength.TupleConcat(hv_Length);
                hv_SegLength = hv_SegLength.TupleConcat(0);
                hv_RowBegin = new HTuple();
                hv_RowBegin = hv_RowBegin.TupleConcat(hv_LPBallRow);
                hv_RowBegin = hv_RowBegin.TupleConcat(hv_RowBegin_);
                hv_RowBegin = hv_RowBegin.TupleConcat(hv_ICBallRow);
                hv_ColBegin = new HTuple();
                hv_ColBegin = hv_ColBegin.TupleConcat(hv_LPBallCol);
                hv_ColBegin = hv_ColBegin.TupleConcat(hv_ColBegin_);
                hv_ColBegin = hv_ColBegin.TupleConcat(hv_ICBallCol);
                hv_RowEnd = new HTuple();
                hv_RowEnd = hv_RowEnd.TupleConcat(hv_LPBallRow);
                hv_RowEnd = hv_RowEnd.TupleConcat(hv_RowEnd_);
                hv_RowEnd = hv_RowEnd.TupleConcat(hv_ICBallRow);
                hv_ColEnd = new HTuple();
                hv_ColEnd = hv_ColEnd.TupleConcat(hv_LPBallCol);
                hv_ColEnd = hv_ColEnd.TupleConcat(hv_ColEnd_);
                hv_ColEnd = hv_ColEnd.TupleConcat(hv_ICBallCol);
                hv_SegNum = new HTuple(hv_RowBegin.TupleLength());
                HOperatorSet.TupleGenConst(hv_SegNum * hv_SegNum, hv_IMAX, out hv_DistanceMatrix);
                HTuple end_val40 = hv_SegNum - 1;
                HTuple step_val40 = 1;
                for (hv_i = 0; hv_i.Continue(end_val40, step_val40); hv_i = hv_i.TupleAdd(step_val40))
                {
                    hv_Row0 = hv_RowBegin.TupleSelect(hv_i);
                    hv_Col0 = hv_ColBegin.TupleSelect(hv_i);
                    hv_Row1 = hv_RowEnd.TupleSelect(hv_i);
                    hv_Col1 = hv_ColEnd.TupleSelect(hv_i);
                    if (hv_DistanceMatrix == null)
                        hv_DistanceMatrix = new HTuple();
                    hv_DistanceMatrix[(hv_i * hv_SegNum) + hv_i] = 0;
                    HTuple end_val46 = hv_SegNum - 1;
                    HTuple step_val46 = 1;
                    for (hv_j = hv_i + 1; hv_j.Continue(end_val46, step_val46); hv_j = hv_j.TupleAdd(step_val46))
                    {
                        HOperatorSet.DistancePp(hv_Row1, hv_Col1, hv_RowBegin.TupleSelect(hv_j),
                            hv_ColBegin.TupleSelect(hv_j), out hv_DistanceMin);
                        if ((int)(new HTuple(hv_i.TupleEqual(0))) != 0)
                        {
                            hv_DistanceMin = ((((hv_DistanceMin - hv_LPBallRadius)).TupleConcat(0))).TupleMax()
                                ;
                        }
                        else if ((int)(new HTuple(hv_j.TupleEqual(hv_SegNum - 1))) != 0)
                        {
                            hv_DistanceMin = ((((hv_DistanceMin - hv_ICBallRadius)).TupleConcat(0))).TupleMax()
                                ;
                        }
                        HOperatorSet.AngleLl(hv_Row1, hv_Col1, hv_RowBegin.TupleSelect(hv_j), hv_ColBegin.TupleSelect(
                            hv_j), hv_LPBallRow, hv_LPBallCol, hv_ICBallRow, hv_ICBallCol, out hv_Angle);
                        if ((int)((new HTuple(hv_DistanceMin.TupleLess(hv_MaxGap))).TupleAnd((new HTuple(((hv_Angle.TupleAbs()
                            )).TupleLess(1.0))).TupleOr(new HTuple(hv_DistanceMin.TupleLess(hv_LineWidth))))) != 0)
                        {
                            if ((int)(new HTuple(hv_Mode.TupleEqual("gap"))) != 0)
                            {
                                if (hv_DistanceMatrix == null)
                                    hv_DistanceMatrix = new HTuple();
                                hv_DistanceMatrix[(hv_i * hv_SegNum) + hv_j] = hv_DistanceMin;
                            }
                            else
                            {
                                if (hv_DistanceMatrix == null)
                                    hv_DistanceMatrix = new HTuple();
                                hv_DistanceMatrix[(hv_i * hv_SegNum) + hv_j] = (hv_DistanceMin * 1.2) + (hv_SegLength.TupleSelect(
                                    hv_i));
                            }
                        }
                    }
                }
                //****************************
                hv_Distance = hv_DistanceMatrix.TupleSelectRange(0, hv_SegNum - 1);
                HOperatorSet.TupleGenConst(hv_SegNum, 0, out hv_VisitedInd);
                HOperatorSet.TupleLessElem(hv_Distance, hv_IMAX, out hv_Less);
                hv_Previous = hv_Less - 1;
                if (hv_VisitedInd == null)
                    hv_VisitedInd = new HTuple();
                hv_VisitedInd[0] = 1;
                HTuple end_val69 = hv_SegNum - 1;
                HTuple step_val69 = 1;
                for (hv_i = 1; hv_i.Continue(end_val69, step_val69); hv_i = hv_i.TupleAdd(step_val69))
                {
                    hv_DistMin = hv_IMAX.Clone();
                    hv_Prev = 0;
                    HTuple end_val72 = hv_SegNum - 1;
                    HTuple step_val72 = 1;
                    for (hv_j = 0; hv_j.Continue(end_val72, step_val72); hv_j = hv_j.TupleAdd(step_val72))
                    {
                        if ((int)((new HTuple(((hv_VisitedInd.TupleSelect(hv_j))).TupleEqual(0))).TupleAnd(
                            new HTuple(((hv_Distance.TupleSelect(hv_j))).TupleLess(hv_DistMin)))) != 0)
                        {
                            hv_DistMin = hv_Distance.TupleSelect(hv_j);
                            hv_Prev = hv_j.Clone();
                        }
                    }
                    if (hv_VisitedInd == null)
                        hv_VisitedInd = new HTuple();
                    hv_VisitedInd[hv_Prev] = 1;
                    HTuple end_val79 = hv_SegNum - 1;
                    HTuple step_val79 = 1;
                    for (hv_j = 0; hv_j.Continue(end_val79, step_val79); hv_j = hv_j.TupleAdd(step_val79))
                    {
                        if ((int)(new HTuple(((hv_VisitedInd.TupleSelect(hv_j))).TupleEqual(0))) != 0)
                        {
                            hv_DistanceCurrent = hv_DistanceMatrix.TupleSelect((hv_Prev * hv_SegNum) + hv_j);
                            if ((int)(new HTuple(hv_DistanceCurrent.TupleLess(hv_IMAX))) != 0)
                            {
                                if ((int)(new HTuple(((hv_Distance.TupleSelect(hv_j))).TupleGreater(
                                    (hv_Distance.TupleSelect(hv_Prev)) + hv_DistanceCurrent))) != 0)
                                {
                                    if (hv_Distance == null)
                                        hv_Distance = new HTuple();
                                    hv_Distance[hv_j] = (hv_Distance.TupleSelect(hv_Prev)) + hv_DistanceCurrent;
                                    if (hv_Previous == null)
                                        hv_Previous = new HTuple();
                                    hv_Previous[hv_j] = hv_Prev;
                                }
                            }
                        }
                    }
                }
                hv_Road = new HTuple();
                hv_ind = hv_SegNum - 1;
                if ((int)(new HTuple(((hv_Previous.TupleSelect(hv_ind))).TupleEqual(-1))) != 0)
                {
                    hv_iFlag = -1;
                    ho_ImageReduced1.Dispose();
                    ho_Lines.Dispose();
                    ho_LinesDark.Dispose();
                    ho_ContoursSplit.Dispose();
                    ho_SelectedContoursLength.Dispose();
                    ho_SelectedContours.Dispose();
                    ho_ContoursSort.Dispose();
                    ho_ObjectSelected.Dispose();
                    ho_ContourLP.Dispose();
                    ho_ContourIC.Dispose();
                    ho_ObjectConcat.Dispose();
                    ho_ObjectsConcat.Dispose();
                    ho_UnionContour.Dispose();

                    return;
                }
                while ((int)(1) != 0)
                {
                    hv_ind = hv_Previous.TupleSelect(hv_ind);
                    if ((int)(new HTuple(hv_ind.TupleEqual(0))) != 0)
                    {
                        break;
                    }
                    hv_Road = hv_Road.TupleConcat(hv_ind);
                }
                ho_ObjectSelected.Dispose();
                HOperatorSet.SelectObj(ho_ContoursSort, out ho_ObjectSelected, hv_Road);
                ho_ContourLP.Dispose();
                HOperatorSet.GenContourPolygonXld(out ho_ContourLP, hv_LPBallRow.TupleConcat(
                    hv_RowBegin.TupleSelect(hv_Road.TupleSelect((new HTuple(hv_Road.TupleLength()
                    )) - 1))), hv_LPBallCol.TupleConcat(hv_ColBegin.TupleSelect(hv_Road.TupleSelect(
                    (new HTuple(hv_Road.TupleLength())) - 1))));
                ho_ContourIC.Dispose();
                HOperatorSet.GenContourPolygonXld(out ho_ContourIC, hv_ICBallRow.TupleConcat(
                    hv_RowEnd.TupleSelect(hv_Road.TupleSelect(0))), hv_ICBallCol.TupleConcat(
                    hv_ColEnd.TupleSelect(hv_Road.TupleSelect(0))));
                ho_ObjectConcat.Dispose();
                HOperatorSet.ConcatObj(ho_ObjectSelected, ho_ContourLP, out ho_ObjectConcat
                    );
                ho_ObjectsConcat.Dispose();
                HOperatorSet.ConcatObj(ho_ObjectConcat, ho_ContourIC, out ho_ObjectsConcat);
                ho_UnionContour.Dispose();
                HOperatorSet.UnionAdjacentContoursXld(ho_ObjectsConcat, out ho_UnionContour,
                    hv_MaxGap * 1.5, 2, "attr_keep");
                ho_Wire.Dispose();
                HOperatorSet.SmoothContoursXld(ho_UnionContour, out ho_Wire, 21);
                HOperatorSet.CountObj(ho_Wire, out hv_Number);
                if ((int)(new HTuple(hv_Number.TupleNotEqual(1))) != 0)
                {
                    hv_iFlag = -1;
                    ho_ImageReduced1.Dispose();
                    ho_Lines.Dispose();
                    ho_LinesDark.Dispose();
                    ho_ContoursSplit.Dispose();
                    ho_SelectedContoursLength.Dispose();
                    ho_SelectedContours.Dispose();
                    ho_ContoursSort.Dispose();
                    ho_ObjectSelected.Dispose();
                    ho_ContourLP.Dispose();
                    ho_ContourIC.Dispose();
                    ho_ObjectConcat.Dispose();
                    ho_ObjectsConcat.Dispose();
                    ho_UnionContour.Dispose();

                    return;
                }
                HOperatorSet.LengthXld(ho_Wire, out hv_WireLength);
                if ((int)((new HTuple(hv_WireLength.TupleGreater(hv_DistanceL * 1.1))).TupleOr(
                    new HTuple(hv_WireLength.TupleLess(hv_DistanceL * 0.9)))) != 0)
                {
                    ho_Wire.Dispose();
                    HOperatorSet.GenEmptyObj(out ho_Wire);
                    hv_iFlag = -1;
                }
                ho_ImageReduced1.Dispose();
                ho_Lines.Dispose();
                ho_LinesDark.Dispose();
                ho_ContoursSplit.Dispose();
                ho_SelectedContoursLength.Dispose();
                ho_SelectedContours.Dispose();
                ho_ContoursSort.Dispose();
                ho_ObjectSelected.Dispose();
                ho_ContourLP.Dispose();
                ho_ContourIC.Dispose();
                ho_ObjectConcat.Dispose();
                ho_ObjectsConcat.Dispose();
                ho_UnionContour.Dispose();

                return;
            }
            catch (HalconException HDevExpDefaultException)
            {
                ho_ImageReduced1.Dispose();
                ho_Lines.Dispose();
                ho_LinesDark.Dispose();
                ho_ContoursSplit.Dispose();
                ho_SelectedContoursLength.Dispose();
                ho_SelectedContours.Dispose();
                ho_ContoursSort.Dispose();
                ho_ObjectSelected.Dispose();
                ho_ContourLP.Dispose();
                ho_ContourIC.Dispose();
                ho_ObjectConcat.Dispose();
                ho_ObjectsConcat.Dispose();
                ho_UnionContour.Dispose();

                throw HDevExpDefaultException;
            }
        }

        public void ic_ball_detect_unit(HObject ho_Image, HObject ho_IcPad, HTuple hv_OnIC,
            HTuple hv_ICBallNum, HTuple hv_ICRadiusLow, HTuple hv_ICRadiusHigh, HTuple hv_MeasureRadius,
            HTuple hv_MeasureLen1, HTuple hv_MeasureLen2, out HTuple hv_ICBallRow, out HTuple hv_ICBallCol,
            out HTuple hv_ICBallRadius, out HTuple hv_iFlag)
        {




            // Local iconic variables 

            HObject ho_ImageReduced = null, ho_Region = null;
            HObject ho_RegionClosing = null, ho_circle = null, ho_RegionOpening = null;
            HObject ho_ConnectedRegions = null, ho_Contours = null, ho_Cross = null;

            // Local control variables 

            HTuple hv_Area = new HTuple(), hv_Row = new HTuple();
            HTuple hv_Column = new HTuple(), hv_radius = new HTuple();
            HTuple hv_Width = null, hv_Height = null, hv_ind = null;
            HTuple hv_MetrologyHandle = new HTuple(), hv_Index = new HTuple();
            HTuple hv_Circle = new HTuple(), hv_Row1 = new HTuple();
            HTuple hv_Column1 = new HTuple();
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_ImageReduced);
            HOperatorSet.GenEmptyObj(out ho_Region);
            HOperatorSet.GenEmptyObj(out ho_RegionClosing);
            HOperatorSet.GenEmptyObj(out ho_circle);
            HOperatorSet.GenEmptyObj(out ho_RegionOpening);
            HOperatorSet.GenEmptyObj(out ho_ConnectedRegions);
            HOperatorSet.GenEmptyObj(out ho_Contours);
            HOperatorSet.GenEmptyObj(out ho_Cross);
            try
            {
                //*********************************************************************
                //         detect and measure the ic ball
                //input:    Image, ic_pad,   *          ic_radius_low, ic_radius_high
                //output：  ic_ball,   *          iFlag(0:ic ball detect successed, -1:ic ball not found,                            -2:small ic ball , -3: big ic ball)
                //*********************************************************************
                hv_iFlag = new HTuple();
                hv_ICBallRow = new HTuple();
                hv_ICBallCol = new HTuple();
                hv_ICBallRadius = new HTuple();
                //**************************
                if ((int)(hv_OnIC) != 0)
                {
                    HOperatorSet.AreaCenter(ho_IcPad, out hv_Area, out hv_Row, out hv_Column);
                }
                else
                {
                    ho_ImageReduced.Dispose();
                    HOperatorSet.ReduceDomain(ho_Image, ho_IcPad, out ho_ImageReduced);
                    ho_Region.Dispose();
                    HOperatorSet.Threshold(ho_ImageReduced, out ho_Region, 128, 255);
                    ho_RegionClosing.Dispose();
                    HOperatorSet.ClosingCircle(ho_Region, out ho_RegionClosing, 1);
                    hv_radius = hv_ICRadiusLow.Clone();
                    hv_Area = new HTuple();
                    while ((int)((new HTuple(hv_radius.TupleLess(hv_ICRadiusHigh))).TupleAnd(
                        (new HTuple((new HTuple(hv_Area.TupleLength())).TupleNotEqual(hv_ICBallNum))).TupleOr(
                        new HTuple(hv_Area.TupleLess(1))))) != 0)
                    {
                        ho_circle.Dispose();
                        HOperatorSet.GenCircle(out ho_circle, 50, 50, hv_radius);
                        ho_RegionOpening.Dispose();
                        HOperatorSet.Opening(ho_RegionClosing, ho_circle, out ho_RegionOpening);
                        ho_ConnectedRegions.Dispose();
                        HOperatorSet.Connection(ho_RegionOpening, out ho_ConnectedRegions);
                        HOperatorSet.AreaCenter(ho_ConnectedRegions, out hv_Area, out hv_Row, out hv_Column);
                        hv_radius = hv_radius + 1;
                    }
                    if ((int)((new HTuple((new HTuple(hv_Area.TupleLength())).TupleLess(hv_ICBallNum))).TupleOr(
                        new HTuple(hv_Area.TupleEqual(0)))) != 0)
                    {
                        hv_iFlag = -1;
                        HOperatorSet.TupleGenConst(hv_ICBallNum, 0, out hv_ICBallRow);
                        HOperatorSet.TupleGenConst(hv_ICBallNum, 0, out hv_ICBallCol);
                        HOperatorSet.TupleGenConst(hv_ICBallNum, 0, out hv_ICBallRadius);
                        ho_ImageReduced.Dispose();
                        ho_Region.Dispose();
                        ho_RegionClosing.Dispose();
                        ho_circle.Dispose();
                        ho_RegionOpening.Dispose();
                        ho_ConnectedRegions.Dispose();
                        ho_Contours.Dispose();
                        ho_Cross.Dispose();

                        return;
                    }
                }
                HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height);
                HTuple end_val34 = hv_ICBallNum - 1;
                HTuple step_val34 = 1;
                for (hv_ind = 0; hv_ind.Continue(end_val34, step_val34); hv_ind = hv_ind.TupleAdd(step_val34))
                {
                    HOperatorSet.CreateMetrologyModel(out hv_MetrologyHandle);
                    HOperatorSet.SetMetrologyModelImageSize(hv_MetrologyHandle, hv_Width, hv_Height);
                    HOperatorSet.AddMetrologyObjectCircleMeasure(hv_MetrologyHandle, hv_Row.TupleSelect(
                        hv_ind), hv_Column.TupleSelect(hv_ind), hv_MeasureRadius, hv_MeasureLen1,
                        hv_MeasureLen2, 1, 30, ((new HTuple("measure_transition")).TupleConcat(
                        "measure_distance")).TupleConcat("min_score"), ((new HTuple("positive")).TupleConcat(
                        3)).TupleConcat(0.6), out hv_Index);
                    HOperatorSet.ApplyMetrologyModel(ho_Image, hv_MetrologyHandle);
                    HOperatorSet.GetMetrologyObjectResult(hv_MetrologyHandle, hv_Index, "all",
                        "result_type", "all_param", out hv_Circle);
                    ho_Contours.Dispose();
                    HOperatorSet.GetMetrologyObjectMeasures(out ho_Contours, hv_MetrologyHandle,
                        "all", "all", out hv_Row1, out hv_Column1);
                    ho_Cross.Dispose();
                    HOperatorSet.GenCrossContourXld(out ho_Cross, hv_Row1, hv_Column1, 6, 0.785398);
                    HOperatorSet.ClearMetrologyModel(hv_MetrologyHandle);
                    if ((int)(new HTuple((new HTuple(hv_Circle.TupleLength())).TupleEqual(3))) != 0)
                    {
                        if ((int)(new HTuple(((hv_Circle.TupleSelect(2))).TupleLess(hv_ICRadiusLow))) != 0)
                        {
                            hv_iFlag = hv_iFlag.TupleConcat(-2);
                            hv_ICBallRow = hv_ICBallRow.TupleConcat(0);
                            hv_ICBallCol = hv_ICBallCol.TupleConcat(0);
                            hv_ICBallRadius = hv_ICBallRadius.TupleConcat(0);
                        }
                        else if ((int)(new HTuple(((hv_Circle.TupleSelect(2))).TupleGreater(
                            hv_ICRadiusHigh))) != 0)
                        {
                            hv_iFlag = hv_iFlag.TupleConcat(-3);
                            hv_ICBallRow = hv_ICBallRow.TupleConcat(0);
                            hv_ICBallCol = hv_ICBallCol.TupleConcat(0);
                            hv_ICBallRadius = hv_ICBallRadius.TupleConcat(0);
                        }
                        else
                        {
                            hv_iFlag = hv_iFlag.TupleConcat(0);
                            hv_ICBallRow = hv_ICBallRow.TupleConcat(hv_Circle.TupleSelect(0));
                            hv_ICBallCol = hv_ICBallCol.TupleConcat(hv_Circle.TupleSelect(1));
                            hv_ICBallRadius = hv_ICBallRadius.TupleConcat(hv_Circle.TupleSelect(2));
                        }
                    }
                    else
                    {
                        hv_iFlag = hv_iFlag.TupleConcat(-1);
                        hv_ICBallRow = hv_ICBallRow.TupleConcat(0);
                        hv_ICBallCol = hv_ICBallCol.TupleConcat(0);
                        hv_ICBallRadius = hv_ICBallRadius.TupleConcat(0);
                    }
                }
                ho_ImageReduced.Dispose();
                ho_Region.Dispose();
                ho_RegionClosing.Dispose();
                ho_circle.Dispose();
                ho_RegionOpening.Dispose();
                ho_ConnectedRegions.Dispose();
                ho_Contours.Dispose();
                ho_Cross.Dispose();

                return;
            }
            catch (HalconException HDevExpDefaultException)
            {
                ho_ImageReduced.Dispose();
                ho_Region.Dispose();
                ho_RegionClosing.Dispose();
                ho_circle.Dispose();
                ho_RegionOpening.Dispose();
                ho_ConnectedRegions.Dispose();
                ho_Contours.Dispose();
                ho_Cross.Dispose();

                throw HDevExpDefaultException;
            }
        }
        public void fit_line_contour_sort(HObject ho_Contours, HTuple hv_RefRow, HTuple hv_RefCol,
            out HTuple hv_RowBegin, out HTuple hv_ColBegin, out HTuple hv_RowEnd, out HTuple hv_ColEnd,
            out HTuple hv_Indices)
        {




            // Local iconic variables 

            // Local control variables 

            HTuple hv_RowBegin_ = null, hv_ColBegin_ = null;
            HTuple hv_RowEnd_ = null, hv_ColEnd_ = null, hv_Nr = null;
            HTuple hv_Nc = null, hv_Dist = null, hv_seg_num = null;
            HTuple hv_lp_row_tup = null, hv_lp_col_tup = null, hv_DistanceBegin = null;
            HTuple hv_DistanceEnd = null, hv_Greater_ = null, hv_Min2 = null;
            HTuple hv_Greater = null, hv_ind = null;
            // Initialize local and output iconic variables 
            hv_RowBegin = new HTuple();
            hv_ColBegin = new HTuple();
            hv_RowEnd = new HTuple();
            hv_ColEnd = new HTuple();
            HOperatorSet.FitLineContourXld(ho_Contours, "tukey", -1, 0, 5, 2, out hv_RowBegin_,
                out hv_ColBegin_, out hv_RowEnd_, out hv_ColEnd_, out hv_Nr, out hv_Nc, out hv_Dist);
            hv_seg_num = new HTuple(hv_RowBegin_.TupleLength());
            HOperatorSet.TupleGenConst(hv_seg_num, hv_RefRow, out hv_lp_row_tup);
            HOperatorSet.TupleGenConst(hv_seg_num, hv_RefCol, out hv_lp_col_tup);
            HOperatorSet.DistancePp(hv_lp_row_tup, hv_lp_col_tup, hv_RowBegin_, hv_ColBegin_,
                out hv_DistanceBegin);
            HOperatorSet.DistancePp(hv_lp_row_tup, hv_lp_col_tup, hv_RowEnd_, hv_ColEnd_,
                out hv_DistanceEnd);
            HOperatorSet.TupleGreaterElem(hv_DistanceBegin, hv_DistanceEnd, out hv_Greater_);
            HOperatorSet.TupleMin2(hv_DistanceBegin, hv_DistanceEnd, out hv_Min2);
            HOperatorSet.TupleSortIndex(hv_Min2, out hv_Indices);
            hv_RowBegin_ = hv_RowBegin_.TupleSelect(hv_Indices);
            hv_ColBegin_ = hv_ColBegin_.TupleSelect(hv_Indices);
            hv_RowEnd_ = hv_RowEnd_.TupleSelect(hv_Indices);
            hv_ColEnd_ = hv_ColEnd_.TupleSelect(hv_Indices);
            hv_Greater = hv_Greater_.TupleSelect(hv_Indices);
            HTuple end_val18 = hv_seg_num - 1;
            HTuple step_val18 = 1;
            for (hv_ind = 0; hv_ind.Continue(end_val18, step_val18); hv_ind = hv_ind.TupleAdd(step_val18))
            {
                if ((int)(new HTuple(((hv_Greater.TupleSelect(hv_ind))).TupleEqual(0))) != 0)
                {
                    hv_RowBegin = hv_RowBegin.TupleConcat(hv_RowBegin_.TupleSelect(hv_ind));
                    hv_ColBegin = hv_ColBegin.TupleConcat(hv_ColBegin_.TupleSelect(hv_ind));
                    hv_RowEnd = hv_RowEnd.TupleConcat(hv_RowEnd_.TupleSelect(hv_ind));
                    hv_ColEnd = hv_ColEnd.TupleConcat(hv_ColEnd_.TupleSelect(hv_ind));
                }
                else
                {
                    hv_RowBegin = hv_RowBegin.TupleConcat(hv_RowEnd_.TupleSelect(hv_ind));
                    hv_ColBegin = hv_ColBegin.TupleConcat(hv_ColEnd_.TupleSelect(hv_ind));
                    hv_RowEnd = hv_RowEnd.TupleConcat(hv_RowBegin_.TupleSelect(hv_ind));
                    hv_ColEnd = hv_ColEnd.TupleConcat(hv_ColBegin_.TupleSelect(hv_ind));
                }
            }

            return;
        }
        public void clear_all_matching_models()
        {

            // Initialize local and output iconic variables 
            HOperatorSet.ClearAllNccModels();
            HOperatorSet.ClearAllShapeModels();

            return;
        }

        public void clear_model(HTuple hv_model_type, HTuple hv_model_id, out HTuple hv_iFlag)
        {



            // Local iconic variables 

            // Local control variables 

            HTuple hv_i = null;
            // Initialize local and output iconic variables 
            hv_iFlag = 0;
            if ((int)(new HTuple((new HTuple(hv_model_type.TupleLength())).TupleNotEqual(
                new HTuple(hv_model_id.TupleLength())))) != 0)
            {
                hv_iFlag = -1;

                return;
            }
            for (hv_i = 0; (int)hv_i <= (int)((new HTuple(hv_model_id.TupleLength())) - 1); hv_i = (int)hv_i + 1)
            {
                switch ((hv_model_type.TupleSelect(
                    hv_i)).I)
                {
                    case 0:
                        HOperatorSet.ClearNccModel(hv_model_id.TupleSelect(hv_i));
                        break;
                    case 1:
                        HOperatorSet.ClearShapeModel(hv_model_id.TupleSelect(hv_i));
                        break;
                }
            }

            return;
        }

        public void compare_position(HTuple hv_Row1, HTuple hv_Col1, HTuple hv_Angle1,
            HTuple hv_Row2, HTuple hv_Col2, HTuple hv_Angle2, HTuple hv_RowThr, HTuple hv_ColThr,
            HTuple hv_AngleThr, out HTuple hv_RowDiff, out HTuple hv_ColDiff, out HTuple hv_AngleDiff,
            out HTuple hv_iFlag)
        {



            // Local iconic variables 
            // Initialize local and output iconic variables 
            hv_iFlag = 0;

            hv_RowDiff = ((hv_Row1 - hv_Row2)).TupleAbs();
            hv_ColDiff = ((hv_Col1 - hv_Col2)).TupleAbs();
            hv_AngleDiff = ((hv_Angle1 - hv_Angle2)).TupleAbs();
            if ((int)(new HTuple(hv_AngleDiff.TupleGreater((new HTuple(180)).TupleRad()))) != 0)
            {
                hv_AngleDiff = ((new HTuple(360)).TupleRad()) - hv_AngleDiff;
            }
            if ((int)(new HTuple(hv_RowDiff.TupleGreater(hv_RowThr))) != 0)
            {
                hv_iFlag = -1;
            }
            if ((int)(new HTuple(hv_ColDiff.TupleGreater(hv_ColThr))) != 0)
            {
                hv_iFlag = -2;
            }
            if ((int)(new HTuple(hv_AngleDiff.TupleGreater(hv_AngleThr))) != 0)
            {
                hv_iFlag = -3;
            }

            return;
        }

        public void gen_thresh_image(HObject ho_mean_image, HObject ho_std_image, HObject ho_sub_region,
            out HObject ho_dark_thresh_image, out HObject ho_light_thresh_image, HTuple hv_sub_reg_num,
            HTuple hv_thresh_dark, HTuple hv_thresh_light, HTuple hv_sobel_scale, out HTuple hv_iFlag)
        {




            // Stack for temporary objects 
            HObject[] OTemp = new HObject[20];

            // Local iconic variables 

            HObject ho_EdgeAmplitude = null, ho_ImageCleared = null;
            HObject ho_sub_region_ = null, ho_ImageScaled = null, ho_ImageResult = null;
            HObject ho_ImageScaled_D = null, ho_ImageSub_D = null, ho__sub_dark_thresh_image = null;
            HObject ho_ImageScaled_L = null, ho_ImageSub_L = null, ho__sub_light_thresh_image = null;
            HObject ho_dark_image_reduce = null, ho_light_image_reduce = null;

            // Local control variables 

            HTuple hv_index = new HTuple(), hv_Exception = null;
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_dark_thresh_image);
            HOperatorSet.GenEmptyObj(out ho_light_thresh_image);
            HOperatorSet.GenEmptyObj(out ho_EdgeAmplitude);
            HOperatorSet.GenEmptyObj(out ho_ImageCleared);
            HOperatorSet.GenEmptyObj(out ho_sub_region_);
            HOperatorSet.GenEmptyObj(out ho_ImageScaled);
            HOperatorSet.GenEmptyObj(out ho_ImageResult);
            HOperatorSet.GenEmptyObj(out ho_ImageScaled_D);
            HOperatorSet.GenEmptyObj(out ho_ImageSub_D);
            HOperatorSet.GenEmptyObj(out ho__sub_dark_thresh_image);
            HOperatorSet.GenEmptyObj(out ho_ImageScaled_L);
            HOperatorSet.GenEmptyObj(out ho_ImageSub_L);
            HOperatorSet.GenEmptyObj(out ho__sub_light_thresh_image);
            HOperatorSet.GenEmptyObj(out ho_dark_image_reduce);
            HOperatorSet.GenEmptyObj(out ho_light_image_reduce);
            try
            {
                hv_iFlag = 0;
                try
                {
                    ho_EdgeAmplitude.Dispose();
                    HOperatorSet.SobelAmp(ho_mean_image, out ho_EdgeAmplitude, "sum_abs", 5);
                    ho_ImageCleared.Dispose();
                    HOperatorSet.GenImageProto(ho_mean_image, out ho_ImageCleared, 0);
                    ho_dark_thresh_image.Dispose();
                    HOperatorSet.ConvertImageType(ho_ImageCleared, out ho_dark_thresh_image,
                        "byte");
                    ho_light_thresh_image.Dispose();
                    HOperatorSet.GenImageProto(ho_dark_thresh_image, out ho_light_thresh_image,
                        0);
                    HTuple end_val6 = hv_sub_reg_num;
                    HTuple step_val6 = 1;
                    for (hv_index = 0; hv_index.Continue(end_val6, step_val6); hv_index = hv_index.TupleAdd(step_val6))
                    {
                        ho_sub_region_.Dispose();
                        HOperatorSet.SelectObj(ho_sub_region, out ho_sub_region_, hv_index + 1);
                        ho_ImageScaled.Dispose();
                        HOperatorSet.ScaleImage(ho_EdgeAmplitude, out ho_ImageScaled, hv_sobel_scale.TupleSelect(
                            hv_index), 0);
                        ho_ImageResult.Dispose();
                        HOperatorSet.AddImage(ho_std_image, ho_ImageScaled, out ho_ImageResult,
                            1, 0);
                        ho_ImageScaled_D.Dispose();
                        HOperatorSet.ScaleImage(ho_ImageResult, out ho_ImageScaled_D, hv_thresh_dark.TupleSelect(
                            hv_index), 0);
                        ho_ImageSub_D.Dispose();
                        HOperatorSet.SubImage(ho_mean_image, ho_ImageScaled_D, out ho_ImageSub_D,
                            1, 0);
                        ho__sub_dark_thresh_image.Dispose();
                        HOperatorSet.ConvertImageType(ho_ImageSub_D, out ho__sub_dark_thresh_image,
                            "byte");
                        ho_ImageScaled_L.Dispose();
                        HOperatorSet.ScaleImage(ho_ImageResult, out ho_ImageScaled_L, hv_thresh_light.TupleSelect(
                            hv_index), 0);
                        ho_ImageSub_L.Dispose();
                        HOperatorSet.AddImage(ho_mean_image, ho_ImageScaled_L, out ho_ImageSub_L,
                            1, 0);
                        ho__sub_light_thresh_image.Dispose();
                        HOperatorSet.ConvertImageType(ho_ImageSub_L, out ho__sub_light_thresh_image,
                            "byte");
                        ho_dark_image_reduce.Dispose();
                        HOperatorSet.ReduceDomain(ho__sub_dark_thresh_image, ho_sub_region_, out ho_dark_image_reduce
                            );
                        {
                            HObject ExpTmpOutVar_0;
                            HOperatorSet.PaintGray(ho_dark_image_reduce, ho_dark_thresh_image, out ExpTmpOutVar_0
                                );
                            ho_dark_thresh_image.Dispose();
                            ho_dark_thresh_image = ExpTmpOutVar_0;
                        }
                        ho_light_image_reduce.Dispose();
                        HOperatorSet.ReduceDomain(ho__sub_light_thresh_image, ho_sub_region_, out ho_light_image_reduce
                            );
                        {
                            HObject ExpTmpOutVar_0;
                            HOperatorSet.PaintGray(ho_light_image_reduce, ho_light_thresh_image, out ExpTmpOutVar_0
                                );
                            ho_light_thresh_image.Dispose();
                            ho_light_thresh_image = ExpTmpOutVar_0;
                        }
                    }
                }
                // catch (Exception) 
                catch (HalconException HDevExpDefaultException1)
                {
                    HDevExpDefaultException1.ToHTuple(out hv_Exception);
                    hv_iFlag = -1;
                }
                ho_EdgeAmplitude.Dispose();
                ho_ImageCleared.Dispose();
                ho_sub_region_.Dispose();
                ho_ImageScaled.Dispose();
                ho_ImageResult.Dispose();
                ho_ImageScaled_D.Dispose();
                ho_ImageSub_D.Dispose();
                ho__sub_dark_thresh_image.Dispose();
                ho_ImageScaled_L.Dispose();
                ho_ImageSub_L.Dispose();
                ho__sub_light_thresh_image.Dispose();
                ho_dark_image_reduce.Dispose();
                ho_light_image_reduce.Dispose();

                return;
            }
            catch (HalconException HDevExpDefaultException)
            {
                ho_EdgeAmplitude.Dispose();
                ho_ImageCleared.Dispose();
                ho_sub_region_.Dispose();
                ho_ImageScaled.Dispose();
                ho_ImageResult.Dispose();
                ho_ImageScaled_D.Dispose();
                ho_ImageSub_D.Dispose();
                ho__sub_dark_thresh_image.Dispose();
                ho_ImageScaled_L.Dispose();
                ho_ImageSub_L.Dispose();
                ho__sub_light_thresh_image.Dispose();
                ho_dark_image_reduce.Dispose();
                ho_light_image_reduce.Dispose();

                throw HDevExpDefaultException;
            }
        }

        //2.26 启用
        public void inspect_CFAR(HObject ho_Image, HObject ho_InspectRegion, out HObject ho_DefectRegion,
            HTuple hv_ClutterSize, HTuple hv_ProtectSize, HTuple hv_TargetSize, HTuple hv_DevScale,
            HTuple hv_DarkLight, HTuple hv_UseReduce)
        {




            // Local iconic variables 

            HObject ho_ImageByte, ho_ImagePart, ho_RegionTrans;
            HObject ho_Domain, ho_RegionTrans1, ho_ImageReal, ho_ImageMeanClutter;
            HObject ho_ImageMeanClutterSquare, ho_ImageSquareReal, ho_ImageSquareMeanClutter;
            HObject ho_ImageVarClutterSquare, ho_ImageVarClutter, ho_ImageMeanTarget = null;
            HObject ho_ImageSub, ho_ImageSufficient, ho_RegionLight = null;
            HObject ho_RegionDark = null, ho_DefectRegion_ = null, ho_SelectedRegions;

            // Local control variables 

            HTuple hv_iFlag = null, hv_ErrMsg = null, hv_Area = null;
            HTuple hv_Row = null, hv_Column = null, hv_Area1 = null;
            HTuple hv_Row1 = null, hv_Column1 = null, hv_HomMat2D = null;
            HTuple hv_Number = null;
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_DefectRegion);
            HOperatorSet.GenEmptyObj(out ho_ImageByte);
            HOperatorSet.GenEmptyObj(out ho_ImagePart);
            HOperatorSet.GenEmptyObj(out ho_RegionTrans);
            HOperatorSet.GenEmptyObj(out ho_Domain);
            HOperatorSet.GenEmptyObj(out ho_RegionTrans1);
            HOperatorSet.GenEmptyObj(out ho_ImageReal);
            HOperatorSet.GenEmptyObj(out ho_ImageMeanClutter);
            HOperatorSet.GenEmptyObj(out ho_ImageMeanClutterSquare);
            HOperatorSet.GenEmptyObj(out ho_ImageSquareReal);
            HOperatorSet.GenEmptyObj(out ho_ImageSquareMeanClutter);
            HOperatorSet.GenEmptyObj(out ho_ImageVarClutterSquare);
            HOperatorSet.GenEmptyObj(out ho_ImageVarClutter);
            HOperatorSet.GenEmptyObj(out ho_ImageMeanTarget);
            HOperatorSet.GenEmptyObj(out ho_ImageSub);
            HOperatorSet.GenEmptyObj(out ho_ImageSufficient);
            HOperatorSet.GenEmptyObj(out ho_RegionLight);
            HOperatorSet.GenEmptyObj(out ho_RegionDark);
            HOperatorSet.GenEmptyObj(out ho_DefectRegion_);
            HOperatorSet.GenEmptyObj(out ho_SelectedRegions);
            try
            {
                hv_iFlag = 0;
                hv_ErrMsg = "";
                ho_DefectRegion.Dispose();
                HOperatorSet.GenEmptyObj(out ho_DefectRegion);
                ho_ImageByte.Dispose();
                HOperatorSet.ReduceDomain(ho_Image, ho_InspectRegion, out ho_ImageByte);
                //*****************
                ho_ImagePart.Dispose();
                HOperatorSet.CropDomain(ho_ImageByte, out ho_ImagePart);
                ho_RegionTrans.Dispose();
                HOperatorSet.ShapeTrans(ho_InspectRegion, out ho_RegionTrans, "rectangle1");
                HOperatorSet.AreaCenter(ho_RegionTrans, out hv_Area, out hv_Row, out hv_Column);
                ho_Domain.Dispose();
                HOperatorSet.GetDomain(ho_ImagePart, out ho_Domain);
                ho_RegionTrans1.Dispose();
                HOperatorSet.ShapeTrans(ho_Domain, out ho_RegionTrans1, "rectangle1");
                HOperatorSet.AreaCenter(ho_RegionTrans1, out hv_Area1, out hv_Row1, out hv_Column1);
                HOperatorSet.VectorAngleToRigid(hv_Row1, hv_Column1, 0, hv_Row, hv_Column,
                    0, out hv_HomMat2D);
                //*****************
                ho_ImageReal.Dispose();
                HOperatorSet.ConvertImageType(ho_ImagePart, out ho_ImageReal, "real");
                ho_ImageMeanClutter.Dispose();
                mean_image_clutter(ho_ImageReal, ho_Domain, out ho_ImageMeanClutter, hv_ClutterSize,
                    hv_ProtectSize, hv_UseReduce);
                ho_ImageMeanClutterSquare.Dispose();
                HOperatorSet.MultImage(ho_ImageMeanClutter, ho_ImageMeanClutter, out ho_ImageMeanClutterSquare,
                    1, 0);
                ho_ImageSquareReal.Dispose();
                HOperatorSet.MultImage(ho_ImageReal, ho_ImageReal, out ho_ImageSquareReal,
                    1, 0);
                ho_ImageSquareMeanClutter.Dispose();
                mean_image_clutter(ho_ImageSquareReal, ho_Domain, out ho_ImageSquareMeanClutter,
                    hv_ClutterSize, hv_ProtectSize, hv_UseReduce);
                ho_ImageVarClutterSquare.Dispose();
                HOperatorSet.SubImage(ho_ImageSquareMeanClutter, ho_ImageMeanClutterSquare,
                    out ho_ImageVarClutterSquare, 1, 0.0000001);
                ho_ImageVarClutter.Dispose();
                HOperatorSet.SqrtImage(ho_ImageVarClutterSquare, out ho_ImageVarClutter);
                if ((int)(new HTuple(hv_UseReduce.TupleEqual(0))) != 0)
                {
                    ho_ImageMeanTarget.Dispose();
                    HOperatorSet.MeanImage(ho_ImageReal, out ho_ImageMeanTarget, hv_TargetSize,
                        hv_TargetSize);
                }
                else
                {
                    ho_ImageMeanTarget.Dispose();
                    mean_image_reduce(ho_ImageReal, ho_Domain, out ho_ImageMeanTarget, hv_TargetSize);
                }
                ho_ImageSub.Dispose();
                HOperatorSet.SubImage(ho_ImageMeanTarget, ho_ImageMeanClutter, out ho_ImageSub,
                    1, 0);
                ho_ImageSufficient.Dispose();
                HOperatorSet.DivImage(ho_ImageSub, ho_ImageVarClutter, out ho_ImageSufficient,
                    1, 0);
                if ((int)(new HTuple(hv_DarkLight.TupleEqual("not_equal"))) != 0)
                {
                    if ((int)(new HTuple((new HTuple(hv_DevScale.TupleLength())).TupleNotEqual(
                        2))) != 0)
                    {
                        hv_iFlag = -1;
                        hv_ErrMsg = "wrong number of DevScale for not_equal";
                        ho_ImageByte.Dispose();
                        ho_ImagePart.Dispose();
                        ho_RegionTrans.Dispose();
                        ho_Domain.Dispose();
                        ho_RegionTrans1.Dispose();
                        ho_ImageReal.Dispose();
                        ho_ImageMeanClutter.Dispose();
                        ho_ImageMeanClutterSquare.Dispose();
                        ho_ImageSquareReal.Dispose();
                        ho_ImageSquareMeanClutter.Dispose();
                        ho_ImageVarClutterSquare.Dispose();
                        ho_ImageVarClutter.Dispose();
                        ho_ImageMeanTarget.Dispose();
                        ho_ImageSub.Dispose();
                        ho_ImageSufficient.Dispose();
                        ho_RegionLight.Dispose();
                        ho_RegionDark.Dispose();
                        ho_DefectRegion_.Dispose();
                        ho_SelectedRegions.Dispose();

                        return;
                    }
                    ho_RegionLight.Dispose();
                    HOperatorSet.Threshold(ho_ImageSufficient, out ho_RegionLight, hv_DevScale.TupleSelect(
                        0), 999);
                    ho_RegionDark.Dispose();
                    HOperatorSet.Threshold(ho_ImageSufficient, out ho_RegionDark, -999, -(hv_DevScale.TupleSelect(
                        1)));
                    ho_DefectRegion_.Dispose();
                    HOperatorSet.ConcatObj(ho_RegionLight, ho_RegionDark, out ho_DefectRegion_
                        );
                    //union2 (RegionLight, RegionDark, DefectRegion_)
                }
                else if ((int)(new HTuple(hv_DarkLight.TupleEqual("dark"))) != 0)
                {
                    ho_DefectRegion_.Dispose();
                    HOperatorSet.Threshold(ho_ImageSufficient, out ho_DefectRegion_, -999, -(hv_DevScale.TupleSelect(
                        0)));
                }
                else if ((int)(new HTuple(hv_DarkLight.TupleEqual("light"))) != 0)
                {
                    ho_DefectRegion_.Dispose();
                    HOperatorSet.Threshold(ho_ImageSufficient, out ho_DefectRegion_, hv_DevScale.TupleSelect(
                        0), 999);
                }
                else
                {
                    hv_iFlag = -1;
                    hv_ErrMsg = "wrong DarkLight value";
                    ho_ImageByte.Dispose();
                    ho_ImagePart.Dispose();
                    ho_RegionTrans.Dispose();
                    ho_Domain.Dispose();
                    ho_RegionTrans1.Dispose();
                    ho_ImageReal.Dispose();
                    ho_ImageMeanClutter.Dispose();
                    ho_ImageMeanClutterSquare.Dispose();
                    ho_ImageSquareReal.Dispose();
                    ho_ImageSquareMeanClutter.Dispose();
                    ho_ImageVarClutterSquare.Dispose();
                    ho_ImageVarClutter.Dispose();
                    ho_ImageMeanTarget.Dispose();
                    ho_ImageSub.Dispose();
                    ho_ImageSufficient.Dispose();
                    ho_RegionLight.Dispose();
                    ho_RegionDark.Dispose();
                    ho_DefectRegion_.Dispose();
                    ho_SelectedRegions.Dispose();

                    return;
                }
                ho_SelectedRegions.Dispose();
                HOperatorSet.SelectShape(ho_DefectRegion_, out ho_SelectedRegions, "area",
                    "and", 0.5, 99999);
                HOperatorSet.CountObj(ho_SelectedRegions, out hv_Number);
                if ((int)(hv_Number) != 0)
                {
                    ho_DefectRegion.Dispose();
                    HOperatorSet.AffineTransRegion(ho_DefectRegion_, out ho_DefectRegion, hv_HomMat2D,
                        "nearest_neighbor");
                }
                ho_ImageByte.Dispose();
                ho_ImagePart.Dispose();
                ho_RegionTrans.Dispose();
                ho_Domain.Dispose();
                ho_RegionTrans1.Dispose();
                ho_ImageReal.Dispose();
                ho_ImageMeanClutter.Dispose();
                ho_ImageMeanClutterSquare.Dispose();
                ho_ImageSquareReal.Dispose();
                ho_ImageSquareMeanClutter.Dispose();
                ho_ImageVarClutterSquare.Dispose();
                ho_ImageVarClutter.Dispose();
                ho_ImageMeanTarget.Dispose();
                ho_ImageSub.Dispose();
                ho_ImageSufficient.Dispose();
                ho_RegionLight.Dispose();
                ho_RegionDark.Dispose();
                ho_DefectRegion_.Dispose();
                ho_SelectedRegions.Dispose();

                return;
            }
            catch (HalconException HDevExpDefaultException)
            {
                ho_ImageByte.Dispose();
                ho_ImagePart.Dispose();
                ho_RegionTrans.Dispose();
                ho_Domain.Dispose();
                ho_RegionTrans1.Dispose();
                ho_ImageReal.Dispose();
                ho_ImageMeanClutter.Dispose();
                ho_ImageMeanClutterSquare.Dispose();
                ho_ImageSquareReal.Dispose();
                ho_ImageSquareMeanClutter.Dispose();
                ho_ImageVarClutterSquare.Dispose();
                ho_ImageVarClutter.Dispose();
                ho_ImageMeanTarget.Dispose();
                ho_ImageSub.Dispose();
                ho_ImageSufficient.Dispose();
                ho_RegionLight.Dispose();
                ho_RegionDark.Dispose();
                ho_DefectRegion_.Dispose();
                ho_SelectedRegions.Dispose();

                throw HDevExpDefaultException;
            }
        }

        public void inspect_chipping_threshold(HObject ho_Image, HObject ho_InspectRegion,
            out HObject ho_DefectRegion, HTuple hv_ErosionSize, HTuple hv_MaxDeviation,
            HTuple hv_LightThresh, HTuple hv_DarkThresh, HTuple hv_OpeningSize, HTuple hv_AreaThresh,
            HTuple hv_Len1Thresh, HTuple hv_Len2Thresh, HTuple hv_SelectOperation, out HTuple hv_iFlag,
            out HTuple hv_ErrMsg)
        {




            // Stack for temporary objects 
            HObject[] OTemp = new HObject[20];

            // Local iconic variables 

            HObject ho_RegionErosion, ho_RegionDifference;
            HObject ho_Region_, ho_ImageReduced = null, ho_RegionLight = null;
            HObject ho_RegionDark = null, ho_RegionUnion = null, ho_RegionOpening;
            HObject ho_ConnectedRegions, ho_SelectedRegions, ho_Contours;
            HObject ho_InspectContour, ho_ObjectSelected = null;

            // Local control variables 

            HTuple hv_Mean = null, hv_Deviation = null;
            HTuple hv_iteration = null, hv_LightGrayThresh = new HTuple();
            HTuple hv_DarkGrayThresh = new HTuple(), hv_Number = null;
            HTuple hv_DefectIndex = null, hv_Index = null, hv_DistanceMin = new HTuple();
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_DefectRegion);
            HOperatorSet.GenEmptyObj(out ho_RegionErosion);
            HOperatorSet.GenEmptyObj(out ho_RegionDifference);
            HOperatorSet.GenEmptyObj(out ho_Region_);
            HOperatorSet.GenEmptyObj(out ho_ImageReduced);
            HOperatorSet.GenEmptyObj(out ho_RegionLight);
            HOperatorSet.GenEmptyObj(out ho_RegionDark);
            HOperatorSet.GenEmptyObj(out ho_RegionUnion);
            HOperatorSet.GenEmptyObj(out ho_RegionOpening);
            HOperatorSet.GenEmptyObj(out ho_ConnectedRegions);
            HOperatorSet.GenEmptyObj(out ho_SelectedRegions);
            HOperatorSet.GenEmptyObj(out ho_Contours);
            HOperatorSet.GenEmptyObj(out ho_InspectContour);
            HOperatorSet.GenEmptyObj(out ho_ObjectSelected);
            try
            {
                hv_iFlag = 0;
                hv_ErrMsg = "";
                ho_DefectRegion.Dispose();
                HOperatorSet.GenEmptyObj(out ho_DefectRegion);
                //get round region
                ho_RegionErosion.Dispose();
                HOperatorSet.ErosionRectangle1(ho_InspectRegion, out ho_RegionErosion, hv_ErosionSize,
                    hv_ErosionSize);
                ho_RegionDifference.Dispose();
                HOperatorSet.Difference(ho_InspectRegion, ho_RegionErosion, out ho_RegionDifference
                    );
                //get region meanGray and deviation
                HOperatorSet.Intensity(ho_RegionDifference, ho_Image, out hv_Mean, out hv_Deviation);
                ho_Region_.Dispose();
                HOperatorSet.CopyObj(ho_RegionDifference, out ho_Region_, 1, 1);
                //avoid unexpected breakdown
                //find region light dark
                hv_iteration = 0;
                while ((int)((new HTuple(hv_Deviation.TupleGreater(hv_MaxDeviation))).TupleAnd(
                    new HTuple(hv_iteration.TupleLess(5)))) != 0)
                {
                    ho_ImageReduced.Dispose();
                    HOperatorSet.ReduceDomain(ho_Image, ho_Region_, out ho_ImageReduced);
                    hv_LightGrayThresh = hv_Mean + (3 * hv_Deviation);
                    if ((int)(new HTuple(hv_LightGrayThresh.TupleLess(255))) != 0)
                    {
                        ho_RegionLight.Dispose();
                        HOperatorSet.Threshold(ho_ImageReduced, out ho_RegionLight, hv_LightGrayThresh,
                            255);
                    }
                    else
                    {
                        ho_RegionLight.Dispose();
                        HOperatorSet.GenEmptyObj(out ho_RegionLight);
                    }
                    hv_DarkGrayThresh = hv_Mean - (3 * hv_Deviation);
                    if ((int)(new HTuple(hv_DarkGrayThresh.TupleGreater(0))) != 0)
                    {
                        ho_RegionDark.Dispose();
                        HOperatorSet.Threshold(ho_ImageReduced, out ho_RegionDark, 0, hv_DarkGrayThresh);
                    }
                    else
                    {
                        ho_RegionDark.Dispose();
                        HOperatorSet.GenEmptyObj(out ho_RegionDark);
                    }
                    ho_RegionUnion.Dispose();
                    HOperatorSet.Union2(ho_RegionDark, ho_RegionLight, out ho_RegionUnion);
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.Difference(ho_Region_, ho_RegionUnion, out ExpTmpOutVar_0);
                        ho_Region_.Dispose();
                        ho_Region_ = ExpTmpOutVar_0;
                    }
                    HOperatorSet.Intensity(ho_Region_, ho_Image, out hv_Mean, out hv_Deviation);
                    hv_iteration = hv_iteration + 1;
                }
                ho_ImageReduced.Dispose();
                HOperatorSet.ReduceDomain(ho_Image, ho_RegionDifference, out ho_ImageReduced
                    );
                hv_LightGrayThresh = hv_Mean + (hv_LightThresh * hv_Deviation);
                if ((int)(new HTuple(hv_LightGrayThresh.TupleLess(255))) != 0)
                {
                    ho_RegionLight.Dispose();
                    HOperatorSet.Threshold(ho_ImageReduced, out ho_RegionLight, hv_LightGrayThresh,
                        255);
                }
                else
                {
                    ho_RegionLight.Dispose();
                    HOperatorSet.GenEmptyObj(out ho_RegionLight);
                }
                hv_DarkGrayThresh = hv_Mean - (hv_DarkThresh * hv_Deviation);
                if ((int)(new HTuple(hv_DarkGrayThresh.TupleGreater(0))) != 0)
                {
                    ho_RegionDark.Dispose();
                    HOperatorSet.Threshold(ho_ImageReduced, out ho_RegionDark, 0, hv_DarkGrayThresh);
                }
                else
                {
                    ho_RegionDark.Dispose();
                    HOperatorSet.GenEmptyObj(out ho_RegionDark);
                }
                ho_RegionUnion.Dispose();
                HOperatorSet.Union2(ho_RegionDark, ho_RegionLight, out ho_RegionUnion);
                ho_RegionOpening.Dispose();
                HOperatorSet.OpeningRectangle1(ho_RegionUnion, out ho_RegionOpening, hv_OpeningSize,
                    hv_OpeningSize);
                ho_ConnectedRegions.Dispose();
                HOperatorSet.Connection(ho_RegionOpening, out ho_ConnectedRegions);
                ho_SelectedRegions.Dispose();
                HOperatorSet.SelectShape(ho_ConnectedRegions, out ho_SelectedRegions, ((new HTuple("area")).TupleConcat(
                    "rect2_len1")).TupleConcat("rect2_len2"), hv_SelectOperation, ((hv_AreaThresh.TupleConcat(
                    hv_Len1Thresh))).TupleConcat(hv_Len2Thresh), ((new HTuple(9999999)).TupleConcat(
                    10000)).TupleConcat(10000));
                ho_Contours.Dispose();
                HOperatorSet.GenContourRegionXld(ho_SelectedRegions, out ho_Contours, "border");
                ho_InspectContour.Dispose();
                HOperatorSet.GenContourRegionXld(ho_InspectRegion, out ho_InspectContour, "border");
                HOperatorSet.CountObj(ho_Contours, out hv_Number);
                hv_DefectIndex = new HTuple();
                HTuple end_val52 = hv_Number;
                HTuple step_val52 = 1;
                for (hv_Index = 1; hv_Index.Continue(end_val52, step_val52); hv_Index = hv_Index.TupleAdd(step_val52))
                {
                    ho_ObjectSelected.Dispose();
                    HOperatorSet.SelectObj(ho_Contours, out ho_ObjectSelected, hv_Index);
                    HOperatorSet.DistanceCcMin(ho_ObjectSelected, ho_InspectContour, "point_to_segment",
                        out hv_DistanceMin);
                    if ((int)(new HTuple(hv_DistanceMin.TupleLess(1))) != 0)
                    {
                        hv_DefectIndex = hv_DefectIndex.TupleConcat(hv_Index);
                        {
                            HObject ExpTmpOutVar_0;
                            HOperatorSet.ConcatObj(ho_DefectRegion, ho_ObjectSelected, out ExpTmpOutVar_0
                                );
                            ho_DefectRegion.Dispose();
                            ho_DefectRegion = ExpTmpOutVar_0;
                        }
                    }
                }
                ho_DefectRegion.Dispose();
                HOperatorSet.SelectObj(ho_SelectedRegions, out ho_DefectRegion, hv_DefectIndex);
                HOperatorSet.CountObj(ho_DefectRegion, out hv_Number);
                if ((int)(hv_Number) != 0)
                {
                    hv_iFlag = -1;
                    hv_ErrMsg = ("NG, there are " + hv_Number) + " defect regions";
                }
                ho_RegionErosion.Dispose();
                ho_RegionDifference.Dispose();
                ho_Region_.Dispose();
                ho_ImageReduced.Dispose();
                ho_RegionLight.Dispose();
                ho_RegionDark.Dispose();
                ho_RegionUnion.Dispose();
                ho_RegionOpening.Dispose();
                ho_ConnectedRegions.Dispose();
                ho_SelectedRegions.Dispose();
                ho_Contours.Dispose();
                ho_InspectContour.Dispose();
                ho_ObjectSelected.Dispose();

                return;
            }
            catch (HalconException HDevExpDefaultException)
            {
                ho_RegionErosion.Dispose();
                ho_RegionDifference.Dispose();
                ho_Region_.Dispose();
                ho_ImageReduced.Dispose();
                ho_RegionLight.Dispose();
                ho_RegionDark.Dispose();
                ho_RegionUnion.Dispose();
                ho_RegionOpening.Dispose();
                ho_ConnectedRegions.Dispose();
                ho_SelectedRegions.Dispose();
                ho_Contours.Dispose();
                ho_InspectContour.Dispose();
                ho_ObjectSelected.Dispose();

                throw HDevExpDefaultException;
            }
        }

        public void inspect_matching(HObject ho_Image, HObject ho_InspectRegions, out HObject ho_DefectRegions,
            HTuple hv_ModelType, HTuple hv_ModelID, HTuple hv_AngleStart, HTuple hv_AngleExtent,
            HTuple hv_MatchThr, out HTuple hv_MatchScore, out HTuple hv_iFlag, out HTuple hv_err_msg)
        {




            // Stack for temporary objects 
            HObject[] OTemp = new HObject[20];

            // Local iconic variables 

            HObject ho_ObjectSelected = null, ho_ImageReduced = null;
            HObject ho_ModelContours = null, ho_ObjectXLD = null;

            // Local control variables 

            HTuple hv_Number = null, hv_Index = null, hv_Row = new HTuple();
            HTuple hv_Column = new HTuple(), hv_Angle = new HTuple();
            HTuple hv_Score = new HTuple(), hv_ScaleR = new HTuple();
            HTuple hv_ScaleC = new HTuple(), hv_HomMat2DIdentity = new HTuple();
            HTuple hv_HomMat2DScale = new HTuple(), hv_HomMat2DRotate = new HTuple();
            HTuple hv_HomMat2DObject = new HTuple();
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_DefectRegions);
            HOperatorSet.GenEmptyObj(out ho_ObjectSelected);
            HOperatorSet.GenEmptyObj(out ho_ImageReduced);
            HOperatorSet.GenEmptyObj(out ho_ModelContours);
            HOperatorSet.GenEmptyObj(out ho_ObjectXLD);
            try
            {
                hv_iFlag = 0;
                hv_err_msg = "";
                hv_MatchScore = new HTuple();
                ho_DefectRegions.Dispose();
                HOperatorSet.GenEmptyObj(out ho_DefectRegions);
                //********************* match inspect ***************************
                //union1 (InspectRegions, RegionUnion)
                //取反
                //complement (RegionUnion, RegionComplement)
                //paint_region (RegionComplement, Image, ImageResult, 0, 'fill')
                HOperatorSet.CountObj(ho_InspectRegions, out hv_Number);
                HTuple end_val10 = hv_Number;
                HTuple step_val10 = 1;
                for (hv_Index = 1; hv_Index.Continue(end_val10, step_val10); hv_Index = hv_Index.TupleAdd(step_val10))
                {
                    ho_ObjectSelected.Dispose();
                    HOperatorSet.SelectObj(ho_InspectRegions, out ho_ObjectSelected, hv_Index);
                    ho_ImageReduced.Dispose();
                    HOperatorSet.ReduceDomain(ho_Image, ho_ObjectSelected, out ho_ImageReduced
                        );
                    if ((int)(new HTuple(hv_ModelType.TupleEqual(0))) != 0)
                    {
                        HOperatorSet.FindNccModel(ho_ImageReduced, hv_ModelID, hv_AngleStart, hv_AngleExtent,
                            0.1, 1, 0.5, "true", 0, out hv_Row, out hv_Column, out hv_Angle, out hv_Score);
                    }
                    else if ((int)(new HTuple(hv_ModelType.TupleEqual(1))) != 0)
                    {
                        //find_shape_model (ImageReduced, ModelID, AngleStart, AngleExtent, 0.1, 1, 0.5, 'least_squares', 0, 0.9, Row, Column, Angle, Score)
                        HOperatorSet.FindAnisoShapeModel(ho_ImageReduced, hv_ModelID, hv_AngleStart,
                            hv_AngleExtent, 0.95, 1.05, 0.95, 1.05, 0.2, 1, 0.5, "least_squares",
                            3, 0.9, out hv_Row, out hv_Column, out hv_Angle, out hv_ScaleR, out hv_ScaleC,
                            out hv_Score);
                        //******test***
                        ho_ModelContours.Dispose();
                        HOperatorSet.GetShapeModelContours(out ho_ModelContours, hv_ModelID, 1);
                        HOperatorSet.HomMat2dIdentity(out hv_HomMat2DIdentity);
                        HOperatorSet.HomMat2dScale(hv_HomMat2DIdentity, hv_ScaleR, hv_ScaleC, 0,
                            0, out hv_HomMat2DScale);
                        HOperatorSet.HomMat2dRotate(hv_HomMat2DScale, hv_Angle, 0, 0, out hv_HomMat2DRotate);
                        HOperatorSet.HomMat2dTranslate(hv_HomMat2DRotate, hv_Row, hv_Column, out hv_HomMat2DObject);
                        ho_ObjectXLD.Dispose();
                        HOperatorSet.AffineTransContourXld(ho_ModelContours, out ho_ObjectXLD,
                            hv_HomMat2DObject);
                        //******
                    }
                    else
                    {
                        hv_iFlag = -2;
                        hv_err_msg = "wrong model type";
                        ho_ObjectSelected.Dispose();
                        ho_ImageReduced.Dispose();
                        ho_ModelContours.Dispose();
                        ho_ObjectXLD.Dispose();

                        return;
                    }
                    if ((int)(new HTuple((new HTuple(hv_Score.TupleLength())).TupleEqual(0))) != 0)
                    {
                        hv_MatchScore = hv_MatchScore.TupleConcat(0);
                    }
                    else
                    {
                        hv_MatchScore = hv_MatchScore.TupleConcat(hv_Score);
                    }
                    if ((int)((new HTuple((new HTuple(hv_Score.TupleLength())).TupleEqual(0))).TupleOr(
                        new HTuple(hv_Score.TupleLess(hv_MatchThr)))) != 0)
                    {
                        {
                            HObject ExpTmpOutVar_0;
                            HOperatorSet.ConcatObj(ho_DefectRegions, ho_ObjectSelected, out ExpTmpOutVar_0
                                );
                            ho_DefectRegions.Dispose();
                            ho_DefectRegions = ExpTmpOutVar_0;
                        }
                    }
                }
                HOperatorSet.CountObj(ho_DefectRegions, out hv_Number);
                if ((int)(new HTuple(hv_Number.TupleGreater(0))) != 0)
                {
                    hv_iFlag = -1;
                    hv_err_msg = ("NG, there are " + hv_Number) + " defect regions";
                }
                ho_ObjectSelected.Dispose();
                ho_ImageReduced.Dispose();
                ho_ModelContours.Dispose();
                ho_ObjectXLD.Dispose();

                return;
            }
            catch (HalconException HDevExpDefaultException)
            {
                ho_ObjectSelected.Dispose();
                ho_ImageReduced.Dispose();
                ho_ModelContours.Dispose();
                ho_ObjectXLD.Dispose();

                throw HDevExpDefaultException;
            }
        }

        public void inspect_scratch(HObject ho_Image, HObject ho_InspectRegion, out HObject ho_DefectLines,
            HTuple hv_IsGauss, HTuple hv_LineSigma, HTuple hv_LineLow, HTuple hv_LineHigh,
            HTuple hv_LightDark, HTuple hv_LengthThresh)
        {




            // Local iconic variables 

            HObject ho_ImageReduced, ho_Lines = null, ho_SelectedXLD;
            HObject ho_UnionContours;
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_DefectLines);
            HOperatorSet.GenEmptyObj(out ho_ImageReduced);
            HOperatorSet.GenEmptyObj(out ho_Lines);
            HOperatorSet.GenEmptyObj(out ho_SelectedXLD);
            HOperatorSet.GenEmptyObj(out ho_UnionContours);
            try
            {

                ho_DefectLines.Dispose();
                HOperatorSet.GenEmptyObj(out ho_DefectLines);
                ho_ImageReduced.Dispose();
                HOperatorSet.ReduceDomain(ho_Image, ho_InspectRegion, out ho_ImageReduced);
                if ((int)(hv_IsGauss) != 0)
                {
                    //lines_gauss (ImageReduced, Lines, 1, 1, 1.5, 'light', 'true', 'bar-shaped', 'true')
                    ho_Lines.Dispose();
                    HOperatorSet.LinesGauss(ho_ImageReduced, out ho_Lines, hv_LineSigma, hv_LineLow,
                        hv_LineHigh, hv_LightDark, "true", "bar-shaped", "true");
                }
                else
                {
                    ho_Lines.Dispose();
                    HOperatorSet.LinesFacet(ho_ImageReduced, out ho_Lines, hv_LineSigma, hv_LineLow,
                        hv_LineHigh, hv_LightDark);
                }
                ho_SelectedXLD.Dispose();
                HOperatorSet.SelectShapeXld(ho_Lines, out ho_SelectedXLD, "contlength", "and",
                    3, 99999);
                //union_collinear_contours_xld (Lines, UnionContours, LengthThresh, 1, 2, 0.1, 'attr_keep')
                ho_UnionContours.Dispose();
                HOperatorSet.UnionAdjacentContoursXld(ho_SelectedXLD, out ho_UnionContours,
                    hv_LengthThresh, 1, "attr_keep");

                ho_DefectLines.Dispose();
                HOperatorSet.SelectShapeXld(ho_UnionContours, out ho_DefectLines, "contlength",
                    "and", hv_LengthThresh, 99999);

                ho_ImageReduced.Dispose();
                ho_Lines.Dispose();
                ho_SelectedXLD.Dispose();
                ho_UnionContours.Dispose();

                return;
            }
            catch (HalconException HDevExpDefaultException)
            {
                ho_ImageReduced.Dispose();
                ho_Lines.Dispose();
                ho_SelectedXLD.Dispose();
                ho_UnionContours.Dispose();

                throw HDevExpDefaultException;
            }
        }

        public void mean_image_clutter(HObject ho_ImageReal, HObject ho_Region, out HObject ho_ImageMeanClutter,
            HTuple hv_SizeClutter, HTuple hv_SizeProtect, HTuple hv_UseReduce)
        {




            // Stack for temporary objects 
            HObject[] OTemp = new HObject[20];

            // Local iconic variables 

            HObject ho_ImageMeanAll = null, ho_ImageMeanProtect = null;
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_ImageMeanClutter);
            HOperatorSet.GenEmptyObj(out ho_ImageMeanAll);
            HOperatorSet.GenEmptyObj(out ho_ImageMeanProtect);
            try
            {
                if ((int)(new HTuple(hv_UseReduce.TupleEqual(0))) != 0)
                {
                    ho_ImageMeanAll.Dispose();
                    HOperatorSet.MeanImage(ho_ImageReal, out ho_ImageMeanAll, hv_SizeClutter,
                        hv_SizeClutter);
                    ho_ImageMeanProtect.Dispose();
                    HOperatorSet.MeanImage(ho_ImageReal, out ho_ImageMeanProtect, hv_SizeProtect,
                        hv_SizeProtect);
                }
                else
                {
                    ho_ImageMeanAll.Dispose();
                    mean_image_reduce(ho_ImageReal, ho_Region, out ho_ImageMeanAll, hv_SizeClutter);
                    ho_ImageMeanProtect.Dispose();
                    mean_image_reduce(ho_ImageReal, ho_Region, out ho_ImageMeanProtect, hv_SizeProtect);
                }
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.ScaleImage(ho_ImageMeanAll, out ExpTmpOutVar_0, hv_SizeClutter * hv_SizeClutter,
                        0);
                    ho_ImageMeanAll.Dispose();
                    ho_ImageMeanAll = ExpTmpOutVar_0;
                }
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.ScaleImage(ho_ImageMeanProtect, out ExpTmpOutVar_0, hv_SizeProtect * hv_SizeProtect,
                        0);
                    ho_ImageMeanProtect.Dispose();
                    ho_ImageMeanProtect = ExpTmpOutVar_0;
                }
                ho_ImageMeanClutter.Dispose();
                HOperatorSet.SubImage(ho_ImageMeanAll, ho_ImageMeanProtect, out ho_ImageMeanClutter,
                    1.0 / ((hv_SizeClutter * hv_SizeClutter) - (hv_SizeProtect * hv_SizeProtect)),
                    0);
                ho_ImageMeanAll.Dispose();
                ho_ImageMeanProtect.Dispose();

                return;
            }
            catch (HalconException HDevExpDefaultException)
            {
                ho_ImageMeanAll.Dispose();
                ho_ImageMeanProtect.Dispose();

                throw HDevExpDefaultException;
            }
        }

        public void mean_image_reduce(HObject ho_Image, HObject ho_Region, out HObject ho_ImageMean,
            HTuple hv_MeanSize)
        {




            // Stack for temporary objects 
            HObject[] OTemp = new HObject[20];

            // Local iconic variables 

            HObject ho_Image2, ho_ImageMean1, ho_ImageMean2;

            // Local control variables 

            HTuple hv_Width = null, hv_Height = null, hv_Type = null;
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_ImageMean);
            HOperatorSet.GenEmptyObj(out ho_Image2);
            HOperatorSet.GenEmptyObj(out ho_ImageMean1);
            HOperatorSet.GenEmptyObj(out ho_ImageMean2);
            try
            {
                HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height);
                HOperatorSet.GetImageType(ho_Image, out hv_Type);
                ho_Image2.Dispose();
                HOperatorSet.RegionToBin(ho_Region, out ho_Image2, 255, 0, hv_Width, hv_Height);
                if ((int)(new HTuple(hv_Type.TupleNotEqual("byte"))) != 0)
                {
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.ConvertImageType(ho_Image2, out ExpTmpOutVar_0, hv_Type);
                        ho_Image2.Dispose();
                        ho_Image2 = ExpTmpOutVar_0;
                    }
                }
                //mult_image (Image, Image2, Image1, 1.0/255, 0)
                ho_ImageMean1.Dispose();
                HOperatorSet.MeanImage(ho_Image, out ho_ImageMean1, hv_MeanSize, hv_MeanSize);
                ho_ImageMean2.Dispose();
                HOperatorSet.MeanImage(ho_Image2, out ho_ImageMean2, hv_MeanSize, hv_MeanSize);
                ho_ImageMean.Dispose();
                HOperatorSet.DivImage(ho_ImageMean1, ho_ImageMean2, out ho_ImageMean, 255,
                    0);
                ho_Image2.Dispose();
                ho_ImageMean1.Dispose();
                ho_ImageMean2.Dispose();

                return;
            }
            catch (HalconException HDevExpDefaultException)
            {
                ho_Image2.Dispose();
                ho_ImageMean1.Dispose();
                ho_ImageMean2.Dispose();

                throw HDevExpDefaultException;
            }
        }
     //measure_specify_rectanfle2该方法不用
        public void measure_specify_rectangle2(HObject ho_Image, HObject ho_InitRectangle,
      out HObject ho_Rectangle, HTuple hv_DistanceThresh, HTuple hv_MinScore, out HTuple hv_iFlag)
        {




            // Stack for temporary objects 
            HObject[] OTemp = new HObject[20];

            // Local iconic variables 

            HObject ho_ImageReduced, ho_ImageGauss, ho_Region;
            HObject ho_RegionOpening, ho_RegionClosing, ho_RegionOpening1;

            // Local copy input parameter variables 
            HObject ho_InitRectangle_COPY_INP_TMP;
            ho_InitRectangle_COPY_INP_TMP = ho_InitRectangle.CopyObj(1, -1);



            // Local control variables 

            HTuple hv_Width = null, hv_Height = null, hv_InitRow = null;
            HTuple hv_InitColumn = null, hv_InitPhi = null, hv_InitLength1 = null;
            HTuple hv_InitLength2 = null, hv_Row2 = null, hv_Column2 = null;
            HTuple hv_Phi1 = null, hv_Length11 = null, hv_Length21 = null;
            HTuple hv_MetrologyHandle = null, hv_MetrologyRectangleIndices = null;
            HTuple hv_RectangleParameter = null, hv_Sequence = new HTuple();
            HTuple hv_Row = new HTuple(), hv_Column = new HTuple();
            HTuple hv_Phi = new HTuple(), hv_Length1 = new HTuple();
            HTuple hv_Length2 = new HTuple(), hv_LengthOffset = null;
            HTuple hv_Indices = null;
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_Rectangle);
            HOperatorSet.GenEmptyObj(out ho_ImageReduced);
            HOperatorSet.GenEmptyObj(out ho_ImageGauss);
            HOperatorSet.GenEmptyObj(out ho_Region);
            HOperatorSet.GenEmptyObj(out ho_RegionOpening);
            HOperatorSet.GenEmptyObj(out ho_RegionClosing);
            HOperatorSet.GenEmptyObj(out ho_RegionOpening1);
            try
            {
                hv_iFlag = 0;
                ho_Rectangle.Dispose();
                HOperatorSet.GenEmptyObj(out ho_Rectangle);
                //*****************************************
                HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height);
                HOperatorSet.SmallestRectangle2(ho_InitRectangle_COPY_INP_TMP, out hv_InitRow,
                    out hv_InitColumn, out hv_InitPhi, out hv_InitLength1, out hv_InitLength2);
                //*
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.DilationRectangle1(ho_InitRectangle_COPY_INP_TMP, out ExpTmpOutVar_0,
                        35, 30);
                    ho_InitRectangle_COPY_INP_TMP.Dispose();
                    ho_InitRectangle_COPY_INP_TMP = ExpTmpOutVar_0;
                }
                ho_ImageReduced.Dispose();
                HOperatorSet.ReduceDomain(ho_Image, ho_InitRectangle_COPY_INP_TMP, out ho_ImageReduced
                    );
                ho_ImageGauss.Dispose();
                HOperatorSet.GaussFilter(ho_ImageReduced, out ho_ImageGauss, 11);
                ho_Region.Dispose();
                HOperatorSet.Threshold(ho_ImageGauss, out ho_Region, 150, 255);
                ho_RegionOpening.Dispose();
                HOperatorSet.OpeningCircle(ho_Region, out ho_RegionOpening, 7);
                ho_RegionClosing.Dispose();
                HOperatorSet.ClosingRectangle1(ho_RegionOpening, out ho_RegionClosing, 81,
                    81);
                ho_RegionOpening1.Dispose();
                HOperatorSet.OpeningRectangle1(ho_RegionClosing, out ho_RegionOpening1, 6,
                    6);
                HOperatorSet.SmallestRectangle2(ho_RegionClosing, out hv_Row2, out hv_Column2,
                    out hv_Phi1, out hv_Length11, out hv_Length21);
                //**
                HOperatorSet.CreateMetrologyModel(out hv_MetrologyHandle);

                HOperatorSet.SetMetrologyModelImageSize(hv_MetrologyHandle, hv_Width, hv_Height);
                HOperatorSet.AddMetrologyObjectRectangle2Measure(hv_MetrologyHandle, hv_Row2,
                    hv_Column2, hv_Phi1, hv_Length11, hv_Length21, 8, 5, 1.0, 5, (((((new HTuple("measure_select")).TupleConcat(
                    "distance_threshold")).TupleConcat("min_score")).TupleConcat("num_instances")).TupleConcat(
                    "measure_distance")).TupleConcat("measure_transition"), (((((new HTuple("first")).TupleConcat(
                    hv_DistanceThresh))).TupleConcat(hv_MinScore))).TupleConcat(((new HTuple(2)).TupleConcat(
                    6)).TupleConcat("negative")), out hv_MetrologyRectangleIndices);
                HOperatorSet.ApplyMetrologyModel(ho_Image, hv_MetrologyHandle);
                //get_metrology_object_measures (Contours1, MetrologyHandle, 'all', 'all', Row1, Column1)
                //gen_cross_contour_xld (Cross, Row1, Column1, 6, 0)
                HOperatorSet.GetMetrologyObjectResult(hv_MetrologyHandle, hv_MetrologyRectangleIndices,
                    "all", "result_type", "all_param", out hv_RectangleParameter);
                HOperatorSet.ClearMetrologyModel(hv_MetrologyHandle);
                if ((int)(new HTuple((new HTuple(hv_RectangleParameter.TupleLength())).TupleGreater(
                    0))) != 0)
                {
                    hv_Sequence = HTuple.TupleGenSequence(0, (new HTuple(hv_RectangleParameter.TupleLength()
                        )) - 1, 5);
                    hv_Row = hv_RectangleParameter.TupleSelect(hv_Sequence);
                    hv_Column = hv_RectangleParameter.TupleSelect(hv_Sequence + 1);
                    hv_Phi = hv_RectangleParameter.TupleSelect(hv_Sequence + 2);
                    hv_Length1 = hv_RectangleParameter.TupleSelect(hv_Sequence + 3);
                    hv_Length2 = hv_RectangleParameter.TupleSelect(hv_Sequence + 4);
                }
                else
                {
                    hv_iFlag = -1;
                    ho_InitRectangle_COPY_INP_TMP.Dispose();
                    ho_ImageReduced.Dispose();
                    ho_ImageGauss.Dispose();
                    ho_Region.Dispose();
                    ho_RegionOpening.Dispose();
                    ho_RegionClosing.Dispose();
                    ho_RegionOpening1.Dispose();

                    return;
                }
                hv_LengthOffset = (((hv_Length1 - hv_InitLength1)).TupleAbs()) + (((hv_Length2 - hv_InitLength2)).TupleAbs()
                    );
                if ((int)(new HTuple(((hv_LengthOffset.TupleMin())).TupleGreater(8))) != 0)
                {
                    hv_iFlag = -1;
                    ho_InitRectangle_COPY_INP_TMP.Dispose();
                    ho_ImageReduced.Dispose();
                    ho_ImageGauss.Dispose();
                    ho_Region.Dispose();
                    ho_RegionOpening.Dispose();
                    ho_RegionClosing.Dispose();
                    ho_RegionOpening1.Dispose();

                    return;
                }
                HOperatorSet.TupleFind(hv_LengthOffset, hv_LengthOffset.TupleMin(), out hv_Indices);
                ho_Rectangle.Dispose();
                HOperatorSet.GenRectangle2(out ho_Rectangle, hv_Row.TupleSelect(hv_Indices),
                    hv_Column.TupleSelect(hv_Indices), hv_Phi.TupleSelect(hv_Indices), hv_Length1.TupleSelect(
                    hv_Indices), hv_Length2.TupleSelect(hv_Indices));
                //gen_rectangle2 (Rectangle, Row, Column, Phi, Length1, Length2)
                ho_InitRectangle_COPY_INP_TMP.Dispose();
                ho_ImageReduced.Dispose();
                ho_ImageGauss.Dispose();
                ho_Region.Dispose();
                ho_RegionOpening.Dispose();
                ho_RegionClosing.Dispose();
                ho_RegionOpening1.Dispose();

                return;
            }
            catch (HalconException HDevExpDefaultException)
            {
                ho_InitRectangle_COPY_INP_TMP.Dispose();
                ho_ImageReduced.Dispose();
                ho_ImageGauss.Dispose();
                ho_Region.Dispose();
                ho_RegionOpening.Dispose();
                ho_RegionClosing.Dispose();
                ho_RegionOpening1.Dispose();

                throw HDevExpDefaultException;
            }
        }

        public void read_model(out HObject ho_show_contour, HTuple hv_model_path, out HTuple hv_model_type,
            out HTuple hv_model_id, out HTuple hv_def_row, out HTuple hv_def_col, out HTuple hv_iFlag)
        {



            // Local iconic variables 

            // Local control variables 

            HTuple hv_file_exist = null, hv_DxfStatus = null;
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_show_contour);
            hv_model_type = new HTuple();
            hv_model_id = new HTuple();
            hv_def_row = new HTuple();
            hv_def_col = new HTuple();
            hv_iFlag = 0;
            HOperatorSet.FileExists(hv_model_path + "/modelType.tup", out hv_file_exist);
            if ((int)(new HTuple(hv_file_exist.TupleNotEqual(1))) != 0)
            {
                hv_iFlag = -1;

                return;
            }
            HOperatorSet.ReadTuple(hv_model_path + "/modelType.tup", out hv_model_type);
            HOperatorSet.FileExists(hv_model_path + "/modelID.dat", out hv_file_exist);
            if ((int)(new HTuple(hv_file_exist.TupleNotEqual(1))) != 0)
            {
                hv_iFlag = -1;

                return;
            }
            if ((int)(new HTuple(hv_model_type.TupleEqual(0))) != 0)
            {
                HOperatorSet.ReadNccModel(hv_model_path + "/modelID.dat", out hv_model_id);
            }
            else
            {
                HOperatorSet.ReadShapeModel(hv_model_path + "/modelID.dat", out hv_model_id);
            }
            HOperatorSet.FileExists(hv_model_path + "/showContour.dxf", out hv_file_exist);
            if ((int)(new HTuple(hv_file_exist.TupleNotEqual(1))) != 0)
            {
                hv_iFlag = -1;

                return;
            }
            ho_show_contour.Dispose();
            HOperatorSet.ReadContourXldDxf(out ho_show_contour, hv_model_path + "/showContour.dxf",
                new HTuple(), new HTuple(), out hv_DxfStatus);
            HOperatorSet.FileExists(hv_model_path + "/defRow.tup", out hv_file_exist);
            if ((int)(new HTuple(hv_file_exist.TupleNotEqual(1))) != 0)
            {
                hv_iFlag = -1;

                return;
            }
            HOperatorSet.ReadTuple(hv_model_path + "/defRow.tup", out hv_def_row);
            HOperatorSet.FileExists(hv_model_path + "/defCol.tup", out hv_file_exist);
            if ((int)(new HTuple(hv_file_exist.TupleNotEqual(1))) != 0)
            {
                hv_iFlag = -1;

                return;
            }
            HOperatorSet.ReadTuple(hv_model_path + "/defCol.tup", out hv_def_col);


            return;
        }

        public void align_image(HObject ho_Image, HObject ho_match_region, out HObject ho_ImageAffinTrans,
            HTuple hv_model_type, HTuple hv_ModelID, HTuple hv_angle_start, HTuple hv_angle_extent,
            HTuple hv_score_thresh, out HTuple hv_iFlag, out HTuple hv_hom_temp2image, out HTuple hv_row,
            out HTuple hv_col, out HTuple hv_angle)
        {




            // Local iconic variables 

            // Local control variables 

            HTuple hv_score = null, hv_ErrMsg = null, hv_hom_image2temp = null;
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_ImageAffinTrans);
            //*******************************************************************************
            //       align image to golden model image, init matching
            //       in:  score_thresh, angle_start, angle_extent, ModelID
            //       out: ImageAffinTrans, hom_temp2image, iFlag, ErrMsg
            //       pre modification:  yilun chen, 05/05/16
            //       last modification: yongbang zhou, @12/11/2016
            //*******************************************************************************
            hv_iFlag = 0;
            coarse_matching(ho_Image, ho_match_region, hv_model_type, hv_ModelID, hv_angle_start,
                hv_angle_extent, 0.3, hv_score_thresh, out hv_row, out hv_col, out hv_angle,
                out hv_score, out hv_hom_temp2image, out hv_iFlag, out hv_ErrMsg);
            if ((int)(new HTuple(hv_iFlag.TupleNotEqual(0))) != 0)
            {

                return;
            }
            HOperatorSet.HomMat2dInvert(hv_hom_temp2image, out hv_hom_image2temp);
            ho_ImageAffinTrans.Dispose();
            HOperatorSet.AffineTransImage(ho_Image, out ho_ImageAffinTrans, hv_hom_image2temp,
                "nearest_neighbor", "false");

            return;
        }

        public void coarse_matching(HObject ho_Image, HObject ho_MatchRegion, HTuple hv_ModelType,
            HTuple hv_ModelID, HTuple hv_AngleStart, HTuple hv_AngleExtent, HTuple hv_MinScore,
            HTuple hv_MatchThresh, out HTuple hv_Row, out HTuple hv_Column, out HTuple hv_Angle,
            out HTuple hv_Score, out HTuple hv_HomMat2D, out HTuple hv_iFlag, out HTuple hv_err_msg)
        {




            // Stack for temporary objects 
            HObject[] OTemp = new HObject[20];

            // Local iconic variables 

            HObject ho_RegionUnion = null;

            // Local copy input parameter variables 
            HObject ho_Image_COPY_INP_TMP;
            ho_Image_COPY_INP_TMP = ho_Image.CopyObj(1, -1);



            // Local control variables 

            HTuple hv_Number = null;
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_RegionUnion);
            hv_Row = new HTuple();
            hv_Column = new HTuple();
            hv_Angle = new HTuple();
            hv_Score = new HTuple();
            hv_HomMat2D = new HTuple();
            try
            {
                hv_iFlag = 0;
                hv_err_msg = "";

                HOperatorSet.CountObj(ho_MatchRegion, out hv_Number);
                if ((int)(new HTuple(hv_Number.TupleGreater(0))) != 0)
                {
                    ho_RegionUnion.Dispose();
                    HOperatorSet.Union1(ho_MatchRegion, out ho_RegionUnion);
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.ReduceDomain(ho_Image_COPY_INP_TMP, ho_RegionUnion, out ExpTmpOutVar_0
                            );
                        ho_Image_COPY_INP_TMP.Dispose();
                        ho_Image_COPY_INP_TMP = ExpTmpOutVar_0;
                    }
                }

                if ((int)(new HTuple(hv_ModelType.TupleEqual(0))) != 0)
                {
                    HOperatorSet.FindNccModel(ho_Image_COPY_INP_TMP, hv_ModelID, hv_AngleStart,
                        hv_AngleExtent, hv_MinScore, 1, 0.5, "true", 0, out hv_Row, out hv_Column,
                        out hv_Angle, out hv_Score);
                }
                else if ((int)(new HTuple(hv_ModelType.TupleEqual(1))) != 0)
                {
                    HOperatorSet.FindShapeModel(ho_Image_COPY_INP_TMP, hv_ModelID, hv_AngleStart,
                        hv_AngleExtent, hv_MinScore, 1, 0.5, "least_squares", 0, 0.9, out hv_Row,
                        out hv_Column, out hv_Angle, out hv_Score);
                }
                else
                {
                    hv_iFlag = -2;
                    hv_err_msg = "wrong model type";
                    ho_Image_COPY_INP_TMP.Dispose();
                    ho_RegionUnion.Dispose();

                    return;
                }

                if ((int)((new HTuple((new HTuple(hv_Score.TupleLength())).TupleEqual(0))).TupleOr(
                    new HTuple(hv_Score.TupleLess(hv_MatchThresh)))) != 0)
                {
                    hv_iFlag = -1;
                    hv_err_msg = "coarse match failed";
                    ho_Image_COPY_INP_TMP.Dispose();
                    ho_RegionUnion.Dispose();

                    return;
                }

                HOperatorSet.VectorAngleToRigid(0, 0, 0, hv_Row, hv_Column, hv_Angle, out hv_HomMat2D);

                ho_Image_COPY_INP_TMP.Dispose();
                ho_RegionUnion.Dispose();

                return;
            }
            catch (HalconException HDevExpDefaultException)
            {
                ho_Image_COPY_INP_TMP.Dispose();
                ho_RegionUnion.Dispose();

                throw HDevExpDefaultException;
            }
        }

        public void calculate_lines_gauss_parameters(HTuple hv_MaxLineWidth, HTuple hv_Contrast,
             out HTuple hv_Sigma, out HTuple hv_Low, out HTuple hv_High)
        {



            // Local iconic variables 

            // Local control variables 

            HTuple hv_ContrastHigh = null, hv_ContrastLow = new HTuple();
            HTuple hv_HalfWidth = null, hv_Help = null;
            HTuple hv_MaxLineWidth_COPY_INP_TMP = hv_MaxLineWidth.Clone();

            // Initialize local and output iconic variables 
            //Check control parameters
            if ((int)(new HTuple((new HTuple(hv_MaxLineWidth_COPY_INP_TMP.TupleLength())).TupleNotEqual(
                1))) != 0)
            {
                throw new HalconException("Wrong number of values of control parameter: 1");
            }
            if ((int)(((hv_MaxLineWidth_COPY_INP_TMP.TupleIsNumber())).TupleNot()) != 0)
            {
                throw new HalconException("Wrong type of control parameter: 1");
            }
            if ((int)(new HTuple(hv_MaxLineWidth_COPY_INP_TMP.TupleLessEqual(0))) != 0)
            {
                throw new HalconException("Wrong value of control parameter: 1");
            }
            if ((int)((new HTuple((new HTuple(hv_Contrast.TupleLength())).TupleNotEqual(1))).TupleAnd(
                new HTuple((new HTuple(hv_Contrast.TupleLength())).TupleNotEqual(2)))) != 0)
            {
                throw new HalconException("Wrong number of values of control parameter: 2");
            }
            if ((int)(new HTuple(((((hv_Contrast.TupleIsNumber())).TupleMin())).TupleEqual(
                0))) != 0)
            {
                throw new HalconException("Wrong type of control parameter: 2");
            }
            //Set and check ContrastHigh
            hv_ContrastHigh = hv_Contrast[0];
            if ((int)(new HTuple(hv_ContrastHigh.TupleLess(0))) != 0)
            {
                throw new HalconException("Wrong value of control parameter: 2");
            }
            //Set or derive ContrastLow
            if ((int)(new HTuple((new HTuple(hv_Contrast.TupleLength())).TupleEqual(2))) != 0)
            {
                hv_ContrastLow = hv_Contrast[1];
            }
            else
            {
                hv_ContrastLow = hv_ContrastHigh / 3.0;
            }
            //Check ContrastLow
            if ((int)(new HTuple(hv_ContrastLow.TupleLess(0))) != 0)
            {
                throw new HalconException("Wrong value of control parameter: 2");
            }
            if ((int)(new HTuple(hv_ContrastLow.TupleGreater(hv_ContrastHigh))) != 0)
            {
                throw new HalconException("Wrong value of control parameter: 2");
            }
            //
            //Calculate the parameters Sigma, Low, and High for lines_gauss
            if ((int)(new HTuple(hv_MaxLineWidth_COPY_INP_TMP.TupleLess((new HTuple(3.0)).TupleSqrt()
                ))) != 0)
            {
                //Note that LineWidthMax < sqrt(3.0) would result in a Sigma < 0.5,
                //which does not make any sense, because the corresponding smoothing
                //filter mask would be of size 1x1.
                //To avoid this, LineWidthMax is restricted to values greater or equal
                //to sqrt(3.0) and the contrast values are adapted to reflect the fact
                //that lines that are thinner than sqrt(3.0) pixels have a lower contrast
                //in the smoothed image (compared to lines that are sqrt(3.0) pixels wide).
                hv_ContrastLow = (hv_ContrastLow * hv_MaxLineWidth_COPY_INP_TMP) / ((new HTuple(3.0)).TupleSqrt()
                    );
                hv_ContrastHigh = (hv_ContrastHigh * hv_MaxLineWidth_COPY_INP_TMP) / ((new HTuple(3.0)).TupleSqrt()
                    );
                hv_MaxLineWidth_COPY_INP_TMP = (new HTuple(3.0)).TupleSqrt();
            }
            //Convert LineWidthMax and the given contrast values into the input parameters
            //Sigma, Low, and High required by lines_gauss
            hv_HalfWidth = hv_MaxLineWidth_COPY_INP_TMP / 2.0;
            hv_Sigma = hv_HalfWidth / ((new HTuple(3.0)).TupleSqrt());
            hv_Help = ((-2.0 * hv_HalfWidth) / (((new HTuple(6.283185307178)).TupleSqrt()) * (hv_Sigma.TuplePow(
                3.0)))) * (((-0.5 * (((hv_HalfWidth / hv_Sigma)).TuplePow(2.0)))).TupleExp());
            hv_High = ((hv_ContrastHigh * hv_Help)).TupleFabs();
            hv_Low = ((hv_ContrastLow * hv_Help)).TupleFabs();

            return;
        }
        
        public void lines_gauss_iter_1(HObject ho_ImageReduced, out HObject ho_Lines,
            HTuple hv_HighThreshold, HTuple hv_LineWidth, HTuple hv_MinSegLength, HTuple hv_LineType)
        {




            // Stack for temporary objects 
            HObject[] OTemp = new HObject[20];

            // Local iconic variables 

            // Local control variables 

            HTuple hv_NXLDs = null, hv_Sigma = null, hv_Low = null;
            HTuple hv_High = null;
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_Lines);
            hv_NXLDs = 0;
            calculate_lines_gauss_parameters(hv_LineWidth, hv_HighThreshold, out hv_Sigma,
                out hv_Low, out hv_High);
            ho_Lines.Dispose();
            HOperatorSet.LinesGauss(ho_ImageReduced, out ho_Lines, hv_Sigma, hv_Low, hv_High,
                hv_LineType, "false", "true", "false");
            {
                HObject ExpTmpOutVar_0;
                HOperatorSet.SelectContoursXld(ho_Lines, out ExpTmpOutVar_0, "contour_length",
                    hv_MinSegLength, 8000, -0.5, 0.5);
                ho_Lines.Dispose();
                ho_Lines = ExpTmpOutVar_0;
            }
            //**********************
            //while (NXLDs == 0 and HighThreshold >= 0.1)
            //lines_gauss (ImageReduced, Lines, LineWidth / sqrt(3), 0.05, HighThreshold, LineType, 'true', 'true', 'true')
            //select_contours_xld (Lines, Lines, 'contour_length', MinSegLength, 8000, -0.5, 0.5)
            //count_obj (Lines, NXLDs)
            //HighThreshold := 0.5 * HighThreshold
            //endwhile

            return;
        }

        public void contours_neighborhood_regions(HObject ho_Contours, out HObject ho_NeighborRegions,
            HTuple hv_Radius)
        {




            // Stack for temporary objects 
            HObject[] OTemp = new HObject[20];

            // Local iconic variables 

            HObject ho_ClippedContours, ho_EmptyObject;
            HObject ho_ObjectSelected = null, ho_Region = null, ho_RegionUnion;
            HObject ho_RegionDilation;

            // Local control variables 

            HTuple hv_Number = null, hv_I = null, hv_Rows = new HTuple();
            HTuple hv_Columns = new HTuple();
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_NeighborRegions);
            HOperatorSet.GenEmptyObj(out ho_ClippedContours);
            HOperatorSet.GenEmptyObj(out ho_EmptyObject);
            HOperatorSet.GenEmptyObj(out ho_ObjectSelected);
            HOperatorSet.GenEmptyObj(out ho_Region);
            HOperatorSet.GenEmptyObj(out ho_RegionUnion);
            HOperatorSet.GenEmptyObj(out ho_RegionDilation);
            try
            {
                ho_NeighborRegions.Dispose();
                HOperatorSet.GenEmptyObj(out ho_NeighborRegions);
                ho_ClippedContours.Dispose();
                HOperatorSet.ClipEndPointsContoursXld(ho_Contours, out ho_ClippedContours,
                    "num_points", 0);
                HOperatorSet.CountObj(ho_ClippedContours, out hv_Number);
                ho_EmptyObject.Dispose();
                HOperatorSet.GenEmptyObj(out ho_EmptyObject);
                HTuple end_val4 = hv_Number;
                HTuple step_val4 = 1;
                for (hv_I = 1; hv_I.Continue(end_val4, step_val4); hv_I = hv_I.TupleAdd(step_val4))
                {
                    ho_ObjectSelected.Dispose();
                    HOperatorSet.SelectObj(ho_ClippedContours, out ho_ObjectSelected, hv_I);
                    HOperatorSet.GetContourXld(ho_ObjectSelected, out hv_Rows, out hv_Columns);
                    ho_Region.Dispose();
                    HOperatorSet.GenRegionPolygon(out ho_Region, hv_Rows, hv_Columns);
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.ConcatObj(ho_EmptyObject, ho_Region, out ExpTmpOutVar_0);
                        ho_EmptyObject.Dispose();
                        ho_EmptyObject = ExpTmpOutVar_0;
                    }
                }
                ho_RegionUnion.Dispose();
                HOperatorSet.Union1(ho_EmptyObject, out ho_RegionUnion);
                ho_RegionDilation.Dispose();
                HOperatorSet.DilationCircle(ho_RegionUnion, out ho_RegionDilation, hv_Radius);
                ho_NeighborRegions.Dispose();
                HOperatorSet.Union1(ho_RegionDilation, out ho_NeighborRegions);
                ho_ClippedContours.Dispose();
                ho_EmptyObject.Dispose();
                ho_ObjectSelected.Dispose();
                ho_Region.Dispose();
                ho_RegionUnion.Dispose();
                ho_RegionDilation.Dispose();

                return;
            }
            catch (HalconException HDevExpDefaultException)
            {
                ho_ClippedContours.Dispose();
                ho_EmptyObject.Dispose();
                ho_ObjectSelected.Dispose();
                ho_Region.Dispose();
                ho_RegionUnion.Dispose();
                ho_RegionDilation.Dispose();

                throw HDevExpDefaultException;
            }
        }
        public void pcb_ball_detect(HObject ho_ImageLine, HObject ho_ImageIC, HObject ho_pcb_pad_affine,
            HObject ho_ic_pad_affine, out HObject ho_defect_pcb_pad, out HObject ho_pcb_ball,
            HTuple hv_pcb_ball_num, HTuple hv_line_num, HTuple hv_pcb_radius_low, HTuple hv_pcb_radius_high,
            out HTuple hv_iFlag)
        {




            // Stack for temporary objects 
            HObject[] OTemp = new HObject[20];

            // Local iconic variables 

            HObject ho_pcb_pad_dilation, ho_RegionUnion1;
            HObject ho_ImageReduced1, ho_Region1, ho_ImageResult, ho_ImageSub;
            HObject ho_Region3, ho_ImageReduced2, ho_Edges, ho_NeighborRegions;
            HObject ho_RegionUnion2, ho_BinImage, ho_ImageGauss, ho_ObjectSelected = null;
            HObject ho_ObjectSelected1 = null, ho_ic_pad = null, ho_ImageReduced = null;
            HObject ho_Region = null, ho_RegionFillUp = null, ho_RegionOpening = null;
            HObject ho_ConnectedRegions3 = null, ho_RegionUnion = null;
            HObject ho_Rectangle = null, ho_RegionIntersection = null, ho_SelectedRegion = null;
            HObject ho_SelectedRegions2 = null, ho_Circle1 = null, ho_Region2 = null;
            HObject ho_ConnectedRegions = null;

            // Local control variables 

            HTuple hv_Width = null, hv_Height = null, hv_pad_num = null;
            HTuple hv_ball_index = null, hv_Min = null, hv_Max = null;
            HTuple hv_Range = null, hv_Min1 = null, hv_Max1 = null;
            HTuple hv_Range1 = null, hv_Index = null, hv_ball_num = new HTuple();
            HTuple hv_Area3 = new HTuple(), hv_Row2 = new HTuple();
            HTuple hv_Column2 = new HTuple(), hv_radius_mean = new HTuple();
            HTuple hv_area_mean = new HTuple(), hv_area_mean_ = new HTuple();
            HTuple hv_Area4 = new HTuple(), hv_Row4 = new HTuple();
            HTuple hv_Column4 = new HTuple(), hv_area_ratio = new HTuple();
            HTuple hv_Mean = new HTuple(), hv_Deviation = new HTuple();
            HTuple hv_AbsoluteHisto = new HTuple(), hv_RelativeHisto = new HTuple();
            HTuple hv_ratio_ = new HTuple(), hv_gray_threshold = new HTuple();
            HTuple hv_gray_threshold_max = new HTuple(), hv_Index2 = new HTuple();
            HTuple hv_Area1 = new HTuple(), hv_Row = new HTuple();
            HTuple hv_Column = new HTuple(), hv_Phi2 = new HTuple();
            HTuple hv_Number1 = new HTuple(), hv_ind = new HTuple();
            HTuple hv_Index1 = new HTuple(), hv_Row3 = new HTuple();
            HTuple hv_Column3 = new HTuple(), hv_Phi1 = new HTuple();
            HTuple hv_Length1 = new HTuple(), hv_Length2 = new HTuple();
            HTuple hv_Sorted = new HTuple(), hv_Inverted = new HTuple();
            HTuple hv_Area = new HTuple(), hv_pcb_row = new HTuple();
            HTuple hv_pcb_col = new HTuple(), hv_Area2 = new HTuple();
            HTuple hv_ic_row = new HTuple(), hv_ic_col = new HTuple();
            HTuple hv_Phi = new HTuple(), hv_HomMat2DIdentity = new HTuple();
            HTuple hv_HomMat2DRotate = new HTuple(), hv_ic_row_proj = new HTuple();
            HTuple hv_ic_col_proj = new HTuple(), hv_pcb_row_proj = new HTuple();
            HTuple hv_pcb_col_proj = new HTuple(), hv_Indices = new HTuple();
            HTuple hv_ball_ind = new HTuple(), hv_num = new HTuple();
            HTuple hv_opening_size = new HTuple(), hv_Number = null;
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_defect_pcb_pad);
            HOperatorSet.GenEmptyObj(out ho_pcb_ball);
            HOperatorSet.GenEmptyObj(out ho_pcb_pad_dilation);
            HOperatorSet.GenEmptyObj(out ho_RegionUnion1);
            HOperatorSet.GenEmptyObj(out ho_ImageReduced1);
            HOperatorSet.GenEmptyObj(out ho_Region1);
            HOperatorSet.GenEmptyObj(out ho_ImageResult);
            HOperatorSet.GenEmptyObj(out ho_ImageSub);
            HOperatorSet.GenEmptyObj(out ho_Region3);
            HOperatorSet.GenEmptyObj(out ho_ImageReduced2);
            HOperatorSet.GenEmptyObj(out ho_Edges);
            HOperatorSet.GenEmptyObj(out ho_NeighborRegions);
            HOperatorSet.GenEmptyObj(out ho_RegionUnion2);
            HOperatorSet.GenEmptyObj(out ho_BinImage);
            HOperatorSet.GenEmptyObj(out ho_ImageGauss);
            HOperatorSet.GenEmptyObj(out ho_ObjectSelected);
            HOperatorSet.GenEmptyObj(out ho_ObjectSelected1);
            HOperatorSet.GenEmptyObj(out ho_ic_pad);
            HOperatorSet.GenEmptyObj(out ho_ImageReduced);
            HOperatorSet.GenEmptyObj(out ho_Region);
            HOperatorSet.GenEmptyObj(out ho_RegionFillUp);
            HOperatorSet.GenEmptyObj(out ho_RegionOpening);
            HOperatorSet.GenEmptyObj(out ho_ConnectedRegions3);
            HOperatorSet.GenEmptyObj(out ho_RegionUnion);
            HOperatorSet.GenEmptyObj(out ho_Rectangle);
            HOperatorSet.GenEmptyObj(out ho_RegionIntersection);
            HOperatorSet.GenEmptyObj(out ho_SelectedRegion);
            HOperatorSet.GenEmptyObj(out ho_SelectedRegions2);
            HOperatorSet.GenEmptyObj(out ho_Circle1);
            HOperatorSet.GenEmptyObj(out ho_Region2);
            HOperatorSet.GenEmptyObj(out ho_ConnectedRegions);
            try
            {
                hv_iFlag = 0;
                ho_defect_pcb_pad.Dispose();
                HOperatorSet.GenEmptyObj(out ho_defect_pcb_pad);
                ho_pcb_ball.Dispose();
                HOperatorSet.GenEmptyObj(out ho_pcb_ball);
                HOperatorSet.GetImageSize(ho_ImageLine, out hv_Width, out hv_Height);
                ho_pcb_pad_dilation.Dispose();
                HOperatorSet.DilationCircle(ho_pcb_pad_affine, out ho_pcb_pad_dilation, 3.5);
                HOperatorSet.GetImageSize(ho_ImageLine, out hv_Width, out hv_Height);
                hv_pad_num = new HTuple(hv_pcb_ball_num.TupleLength());
                hv_ball_index = 0;
                ho_RegionUnion1.Dispose();
                HOperatorSet.Union1(ho_pcb_pad_affine, out ho_RegionUnion1);
                ho_ImageReduced1.Dispose();
                HOperatorSet.ReduceDomain(ho_ImageLine, ho_RegionUnion1, out ho_ImageReduced1
                    );
                HOperatorSet.MinMaxGray(ho_RegionUnion1, ho_ImageLine, 10, out hv_Min, out hv_Max,
                    out hv_Range);
                ho_Region1.Dispose();
                HOperatorSet.Threshold(ho_ImageReduced1, out ho_Region1, hv_Max, 255);
                ho_ImageResult.Dispose();
                HOperatorSet.PaintRegion(ho_Region1, ho_ImageLine, out ho_ImageResult, hv_Max,
                    "fill");
                HOperatorSet.MinMaxGray(ho_RegionUnion1, ho_ImageResult, 25, out hv_Min, out hv_Max,
                    out hv_Range);
                HOperatorSet.MinMaxGray(ho_RegionUnion1, ho_ImageIC, 10, out hv_Min1, out hv_Max1,
                    out hv_Range1);
                ho_ImageSub.Dispose();
                HOperatorSet.SubImage(ho_ImageResult, ho_ImageIC, out ho_ImageSub, 1, ((((hv_Max1 - hv_Max)).TupleConcat(
                    40))).TupleMin());
                //*********2/26 by yongbang Zhou
                //*
                //reduce_domain (ImageSub, RegionUnion1, ImageReduced3)
                //threshold (ImageReduced3, Region3, 20, 255)
                //fill_up_shape (Region3, RegionFillUp1, 'area', 1, 5)
                //opening_circle (RegionFillUp1, RegionOpening1, pcb_radius_low)
                //***

                ho_ImageReduced2.Dispose();
                HOperatorSet.ReduceDomain(ho_ImageLine, ho_RegionUnion1, out ho_ImageReduced2
                    );
                ho_Edges.Dispose();
                HOperatorSet.EdgesSubPix(ho_ImageReduced2, out ho_Edges, "canny", 1, 30, 50);
                ho_Region3.Dispose();
                HOperatorSet.Threshold(ho_ImageReduced2, out ho_Region3, 128, 255);
                ho_NeighborRegions.Dispose();
                contours_neighborhood_regions(ho_Edges, out ho_NeighborRegions, 0.5);
                ho_RegionUnion2.Dispose();
                HOperatorSet.Union2(ho_NeighborRegions, ho_Region3, out ho_RegionUnion2);
                //difference (NeighborRegions, NeighborRegions_, RegionDifference)
                ho_BinImage.Dispose();
                HOperatorSet.RegionToBin(ho_RegionUnion2, out ho_BinImage, 255, 0, hv_Width,
                    hv_Height);
                ho_ImageGauss.Dispose();
                HOperatorSet.GaussImage(ho_BinImage, out ho_ImageGauss, 7);
                //***********
                HTuple end_val33 = hv_pad_num;
                HTuple step_val33 = 1;
                for (hv_Index = 1; hv_Index.Continue(end_val33, step_val33); hv_Index = hv_Index.TupleAdd(step_val33))
                {
                    ho_ObjectSelected.Dispose();
                    HOperatorSet.SelectObj(ho_pcb_pad_dilation, out ho_ObjectSelected, hv_Index);
                    ho_ObjectSelected1.Dispose();
                    HOperatorSet.SelectObj(ho_pcb_pad_affine, out ho_ObjectSelected1, hv_Index);
                    //**************
                    hv_ball_num = hv_pcb_ball_num.TupleSelect(hv_Index - 1);
                    hv_ball_index = ((hv_pcb_ball_num.TupleSelectRange(0, hv_Index - 1))).TupleSum()
                        ;
                    ho_ic_pad.Dispose();
                    HOperatorSet.SelectObj(ho_ic_pad_affine, out ho_ic_pad, HTuple.TupleGenSequence(
                        (hv_ball_index - hv_ball_num) + 1, hv_ball_index, 1));
                    //******test*******
                    if ((int)(0) != 0)
                    {
                        //none
                    }
                    else
                    {
                        HOperatorSet.AreaCenter(ho_ObjectSelected1, out hv_Area3, out hv_Row2,
                            out hv_Column2);
                        hv_radius_mean = (hv_pcb_radius_low + hv_pcb_radius_high) / 2.0;
                        hv_area_mean = ((hv_radius_mean * hv_radius_mean) * 3.14159) * hv_ball_num;
                        hv_area_mean_ = ((hv_pcb_radius_high * hv_pcb_radius_high) * 3.14159) * hv_ball_num;
                        HOperatorSet.AreaCenter(ho_ObjectSelected1, out hv_Area4, out hv_Row4,
                            out hv_Column4);
                        hv_area_ratio = (hv_area_mean_ * 1.5) / hv_Area4;
                        ho_ImageReduced.Dispose();
                        HOperatorSet.ReduceDomain(ho_ImageGauss, ho_ObjectSelected1, out ho_ImageReduced
                            );
                        HOperatorSet.MinMaxGray(ho_ObjectSelected1, ho_ImageReduced, 10, out hv_Min,
                            out hv_Max, out hv_Range);
                        //threshold (ImageReduced, Region1, Max, 255)
                        //paint_region (Region1, ImageReduced, ImageResult, Max, 'fill')
                        HOperatorSet.Intensity(ho_ObjectSelected1, ho_ImageReduced, out hv_Mean,
                            out hv_Deviation);
                        HOperatorSet.GrayHisto(ho_ObjectSelected1, ho_ImageReduced, out hv_AbsoluteHisto,
                            out hv_RelativeHisto);
                        hv_ratio_ = 0;
                        hv_gray_threshold = hv_Mean / 2;
                        //for tup_ind := |RelativeHisto|-1 to 0 by -1
                        //ratio_ := ratio_+RelativeHisto[tup_ind]
                        //if (ratio_ > area_ratio or tup_ind < 30)
                        //gray_threshold := tup_ind
                        //break
                        //endif
                        //endfor
                        hv_gray_threshold_max = (((((hv_Mean + hv_Max) / 2)).TupleConcat(hv_gray_threshold + 20))).TupleMax()
                            ;
                        //gray_threshold := (Mean+Min)/2
                        while ((int)(new HTuple(hv_gray_threshold.TupleLess(hv_gray_threshold_max))) != 0)
                        {
                            ho_Region.Dispose();
                            HOperatorSet.Threshold(ho_ImageReduced, out ho_Region, hv_gray_threshold,
                                255);
                            hv_gray_threshold = hv_gray_threshold + 5;
                            ho_RegionFillUp.Dispose();
                            HOperatorSet.FillUpShape(ho_Region, out ho_RegionFillUp, "area", 1, 10);
                            for (hv_Index2 = 0; (int)hv_Index2 <= 6; hv_Index2 = (int)hv_Index2 + 1)
                            {
                                ho_RegionOpening.Dispose();
                                HOperatorSet.OpeningCircle(ho_RegionFillUp, out ho_RegionOpening, hv_pcb_radius_low - (hv_Index2 * 0.5));
                                ho_ConnectedRegions3.Dispose();
                                HOperatorSet.Connection(ho_RegionOpening, out ho_ConnectedRegions3);
                                HOperatorSet.AreaCenter(ho_ConnectedRegions3, out hv_Area1, out hv_Row,
                                    out hv_Column);
                                if ((int)((new HTuple((new HTuple(hv_Area1.TupleLength())).TupleLess(
                                    hv_ball_num))).TupleAnd(new HTuple(hv_ball_num.TupleGreaterEqual(
                                    2)))) != 0)
                                {
                                    ho_RegionUnion.Dispose();
                                    HOperatorSet.Union1(ho_ConnectedRegions3, out ho_RegionUnion);
                                    HOperatorSet.OrientationRegion(ho_RegionUnion, out hv_Phi2);
                                    ho_Rectangle.Dispose();
                                    HOperatorSet.GenRectangle2(out ho_Rectangle, hv_Row.TupleSelect(0),
                                        hv_Column.TupleSelect(0), hv_Phi2, 2, hv_pcb_radius_low);
                                    {
                                        HObject ExpTmpOutVar_0;
                                        HOperatorSet.Opening(ho_ConnectedRegions3, ho_Rectangle, out ExpTmpOutVar_0
                                            );
                                        ho_ConnectedRegions3.Dispose();
                                        ho_ConnectedRegions3 = ExpTmpOutVar_0;
                                    }
                                    {
                                        HObject ExpTmpOutVar_0;
                                        HOperatorSet.Connection(ho_ConnectedRegions3, out ExpTmpOutVar_0);
                                        ho_ConnectedRegions3.Dispose();
                                        ho_ConnectedRegions3 = ExpTmpOutVar_0;
                                    }
                                    HOperatorSet.AreaCenter(ho_ConnectedRegions3, out hv_Area1, out hv_Row,
                                        out hv_Column);
                                }
                                if ((int)((new HTuple((new HTuple((new HTuple(hv_Area1.TupleLength()
                                    )).TupleGreaterEqual(hv_ball_num))).TupleAnd(new HTuple(hv_Area1.TupleGreater(
                                    0))))).TupleOr(new HTuple(((hv_Area1.TupleSum())).TupleGreater(
                                    hv_area_mean * 0.9)))) != 0)
                                {
                                    break;
                                }
                            }
                            HOperatorSet.CountObj(ho_ConnectedRegions3, out hv_Number1);
                            if ((int)(new HTuple((new HTuple(hv_Area1.TupleLength())).TupleLess(hv_ball_num))) != 0)
                            {
                                HOperatorSet.OrientationRegion(ho_RegionOpening, out hv_Phi2);
                                for (hv_ind = 0; (int)hv_ind <= 8; hv_ind = (int)hv_ind + 1)
                                {
                                    ho_Rectangle.Dispose();
                                    HOperatorSet.GenRectangle2(out ho_Rectangle, hv_Row.TupleSelect(0),
                                        hv_Column.TupleSelect(0), hv_Phi2, 1, hv_pcb_radius_low + (hv_ind * 0.5));
                                    ho_RegionIntersection.Dispose();
                                    HOperatorSet.Opening(ho_RegionOpening, ho_Rectangle, out ho_RegionIntersection
                                        );
                                    ho_ConnectedRegions3.Dispose();
                                    HOperatorSet.Connection(ho_RegionIntersection, out ho_ConnectedRegions3
                                        );
                                    HOperatorSet.CountObj(ho_ConnectedRegions3, out hv_Number1);
                                    if ((int)(new HTuple(hv_Number1.TupleEqual(hv_ball_num))) != 0)
                                    {
                                        break;
                                    }
                                }
                            }
                            if ((int)(new HTuple(hv_Number1.TupleGreaterEqual(hv_ball_num))) != 0)
                            {
                                break;
                            }
                        }
                        HOperatorSet.AreaCenter(ho_ConnectedRegions3, out hv_Area1, out hv_Row,
                            out hv_Column);
                        if ((int)(new HTuple((new HTuple(hv_Area1.TupleLength())).TupleLess(hv_ball_num))) != 0)
                        {
                            HOperatorSet.AreaCenter(ho_ConnectedRegions3, out hv_Area1, out hv_Row,
                                out hv_Column);
                            {
                                HObject ExpTmpOutVar_0;
                                HOperatorSet.ConcatObj(ho_defect_pcb_pad, ho_ObjectSelected, out ExpTmpOutVar_0
                                    );
                                ho_defect_pcb_pad.Dispose();
                                ho_defect_pcb_pad = ExpTmpOutVar_0;
                            }
                            ho_SelectedRegion.Dispose();
                            HOperatorSet.GenEmptyRegion(out ho_SelectedRegion);
                            HTuple end_val109 = hv_ball_num;
                            HTuple step_val109 = 1;
                            for (hv_Index1 = 1; hv_Index1.Continue(end_val109, step_val109); hv_Index1 = hv_Index1.TupleAdd(step_val109))
                            {
                                {
                                    HObject ExpTmpOutVar_0;
                                    HOperatorSet.ConcatObj(ho_pcb_ball, ho_SelectedRegion, out ExpTmpOutVar_0
                                        );
                                    ho_pcb_ball.Dispose();
                                    ho_pcb_ball = ExpTmpOutVar_0;
                                }
                            }
                            continue;
                        }
                        HOperatorSet.SmallestRectangle2(ho_ConnectedRegions3, out hv_Row3, out hv_Column3,
                            out hv_Phi1, out hv_Length1, out hv_Length2);
                        HOperatorSet.TupleSort(hv_Length2, out hv_Sorted);
                        HOperatorSet.TupleInverse(hv_Sorted, out hv_Inverted);
                        ho_SelectedRegions2.Dispose();
                        HOperatorSet.SelectShape(ho_ConnectedRegions3, out ho_SelectedRegions2,
                            "rect2_len2", "and", hv_Inverted.TupleSelect(hv_ball_num - 1), 99999);
                        HOperatorSet.AreaCenter(ho_SelectedRegions2, out hv_Area, out hv_pcb_row,
                            out hv_pcb_col);
                        if ((int)(new HTuple(hv_ball_num.TupleGreater(1))) != 0)
                        {
                            HOperatorSet.AreaCenter(ho_ic_pad, out hv_Area2, out hv_ic_row, out hv_ic_col);
                            HOperatorSet.AngleLl(hv_ic_row.TupleMean(), hv_ic_col.TupleMean(), hv_pcb_row.TupleMean()
                                , hv_pcb_col.TupleMean(), 0, 1, 0, 0, out hv_Phi);
                            HOperatorSet.HomMat2dIdentity(out hv_HomMat2DIdentity);
                            HOperatorSet.HomMat2dRotate(hv_HomMat2DIdentity, hv_Phi, hv_ic_row.TupleMean()
                                , hv_ic_col.TupleMean(), out hv_HomMat2DRotate);
                            HOperatorSet.AffineTransPoint2d(hv_HomMat2DRotate, hv_ic_row, hv_ic_col,
                                out hv_ic_row_proj, out hv_ic_col_proj);
                            HOperatorSet.AffineTransPoint2d(hv_HomMat2DRotate, hv_pcb_row, hv_pcb_col,
                                out hv_pcb_row_proj, out hv_pcb_col_proj);
                            //gen_cross_contour_xld (Cross1, pcb_row_proj, pcb_col_proj, 6, Phi)
                            //gen_cross_contour_xld (Cross2, ic_row_proj, ic_col_proj, 6, Phi)
                            HOperatorSet.TupleSortIndex(hv_pcb_row_proj, out hv_Indices);
                        }
                        else
                        {
                            hv_Indices = 0;
                        }
                        HTuple end_val132 = hv_ball_num - 1;
                        HTuple step_val132 = 1;
                        for (hv_ball_ind = 0; hv_ball_ind.Continue(end_val132, step_val132); hv_ball_ind = hv_ball_ind.TupleAdd(step_val132))
                        {
                            ho_Circle1.Dispose();
                            HOperatorSet.GenCircle(out ho_Circle1, hv_pcb_row.TupleSelect(hv_Indices.TupleSelect(
                                hv_ball_ind)), hv_pcb_col.TupleSelect(hv_Indices.TupleSelect(hv_ball_ind)),
                                hv_pcb_radius_high + 2);
                            {
                                HObject ExpTmpOutVar_0;
                                HOperatorSet.Intersection(ho_Circle1, ho_ObjectSelected1, out ExpTmpOutVar_0
                                    );
                                ho_Circle1.Dispose();
                                ho_Circle1 = ExpTmpOutVar_0;
                            }
                            ho_ImageReduced.Dispose();
                            HOperatorSet.ReduceDomain(ho_ImageGauss, ho_Circle1, out ho_ImageReduced
                                );
                            HOperatorSet.MinMaxGray(ho_Circle1, ho_ImageReduced, 10, out hv_Min,
                                out hv_Max, out hv_Range);
                            ho_Region2.Dispose();
                            HOperatorSet.Threshold(ho_ImageReduced, out ho_Region2, (hv_Min + hv_Max) * 0.5,
                                255);
                            ho_RegionFillUp.Dispose();
                            HOperatorSet.ClosingCircle(ho_Region2, out ho_RegionFillUp, 3);
                            {
                                HObject ExpTmpOutVar_0;
                                HOperatorSet.FillUpShape(ho_RegionFillUp, out ExpTmpOutVar_0, "area",
                                    1, 20);
                                ho_RegionFillUp.Dispose();
                                ho_RegionFillUp = ExpTmpOutVar_0;
                            }
                            hv_num = 0;
                            hv_opening_size = hv_pcb_radius_high.Clone();
                            while ((int)((new HTuple(hv_num.TupleNotEqual(1))).TupleAnd(new HTuple(hv_opening_size.TupleGreaterEqual(
                                hv_pcb_radius_low)))) != 0)
                            {
                                ho_RegionOpening.Dispose();
                                HOperatorSet.OpeningCircle(ho_RegionFillUp, out ho_RegionOpening, hv_opening_size);
                                hv_opening_size = hv_opening_size - 1;
                                ho_ConnectedRegions.Dispose();
                                HOperatorSet.Connection(ho_RegionOpening, out ho_ConnectedRegions);
                                ho_SelectedRegion.Dispose();
                                HOperatorSet.SelectShape(ho_ConnectedRegions, out ho_SelectedRegion,
                                    "area", "and", 1, 99999);
                                HOperatorSet.CountObj(ho_SelectedRegion, out hv_num);
                            }
                            if ((int)(new HTuple(hv_num.TupleNotEqual(1))) != 0)
                            {
                                {
                                    HObject ExpTmpOutVar_0;
                                    HOperatorSet.ConcatObj(ho_defect_pcb_pad, ho_ObjectSelected, out ExpTmpOutVar_0
                                        );
                                    ho_defect_pcb_pad.Dispose();
                                    ho_defect_pcb_pad = ExpTmpOutVar_0;
                                }
                                ho_SelectedRegion.Dispose();
                                HOperatorSet.GenEmptyRegion(out ho_SelectedRegion);
                            }
                            {
                                HObject ExpTmpOutVar_0;
                                HOperatorSet.ConcatObj(ho_pcb_ball, ho_SelectedRegion, out ExpTmpOutVar_0
                                    );
                                ho_pcb_ball.Dispose();
                                ho_pcb_ball = ExpTmpOutVar_0;
                            }
                        }
                    }
                }
                HOperatorSet.CountObj(ho_defect_pcb_pad, out hv_Number);
                if ((int)(hv_Number) != 0)
                {
                    hv_iFlag = -1;
                }
                ho_pcb_pad_dilation.Dispose();
                ho_RegionUnion1.Dispose();
                ho_ImageReduced1.Dispose();
                ho_Region1.Dispose();
                ho_ImageResult.Dispose();
                ho_ImageSub.Dispose();
                ho_Region3.Dispose();
                ho_ImageReduced2.Dispose();
                ho_Edges.Dispose();
                ho_NeighborRegions.Dispose();
                ho_RegionUnion2.Dispose();
                ho_BinImage.Dispose();
                ho_ImageGauss.Dispose();
                ho_ObjectSelected.Dispose();
                ho_ObjectSelected1.Dispose();
                ho_ic_pad.Dispose();
                ho_ImageReduced.Dispose();
                ho_Region.Dispose();
                ho_RegionFillUp.Dispose();
                ho_RegionOpening.Dispose();
                ho_ConnectedRegions3.Dispose();
                ho_RegionUnion.Dispose();
                ho_Rectangle.Dispose();
                ho_RegionIntersection.Dispose();
                ho_SelectedRegion.Dispose();
                ho_SelectedRegions2.Dispose();
                ho_Circle1.Dispose();
                ho_Region2.Dispose();
                ho_ConnectedRegions.Dispose();

                return;
            }
            catch (HalconException HDevExpDefaultException)
            {
                ho_pcb_pad_dilation.Dispose();
                ho_RegionUnion1.Dispose();
                ho_ImageReduced1.Dispose();
                ho_Region1.Dispose();
                ho_ImageResult.Dispose();
                ho_ImageSub.Dispose();
                ho_Region3.Dispose();
                ho_ImageReduced2.Dispose();
                ho_Edges.Dispose();
                ho_NeighborRegions.Dispose();
                ho_RegionUnion2.Dispose();
                ho_BinImage.Dispose();
                ho_ImageGauss.Dispose();
                ho_ObjectSelected.Dispose();
                ho_ObjectSelected1.Dispose();
                ho_ic_pad.Dispose();
                ho_ImageReduced.Dispose();
                ho_Region.Dispose();
                ho_RegionFillUp.Dispose();
                ho_RegionOpening.Dispose();
                ho_ConnectedRegions3.Dispose();
                ho_RegionUnion.Dispose();
                ho_Rectangle.Dispose();
                ho_RegionIntersection.Dispose();
                ho_SelectedRegion.Dispose();
                ho_SelectedRegions2.Dispose();
                ho_Circle1.Dispose();
                ho_Region2.Dispose();
                ho_ConnectedRegions.Dispose();

                throw HDevExpDefaultException;
            }
        }

        public void segments_concat_DP(HObject ho_Segments, out HObject ho_Wire, HTuple hv_angle,
            HTuple hv_MaxDist)
        {




            // Stack for temporary objects 
            HObject[] OTemp = new HObject[20];

            // Local iconic variables 

            HObject ho_ObjectSelected, ho_ObjectSelected1;
            HObject ho_Seg1 = null, ho_Seg2 = null, ho_Wires;

            // Local control variables 

            HTuple hv_Dist = null, hv_Num_seg = null, hv_Row0 = null;
            HTuple hv_Col0 = null, hv_row0 = null, hv_col0 = null;
            HTuple hv_Row1 = null, hv_Col1 = null, hv_row1 = null;
            HTuple hv_col1 = null, hv_Distance1 = null, hv_flag = null;
            HTuple hv_Num_seg_1 = null, hv_idx_temp_1 = null, hv_idx_temp_2 = null;
            HTuple hv_idx_k = null, hv_idx_l = new HTuple(), hv_Distance = new HTuple();
            HTuple hv_road = null, hv_distSmall = null, hv_pos = null;
            HTuple hv_road_ = null, hv_dist_min = new HTuple(), hv_pre = null;
            HTuple hv_road_now = null, hv_road_out = null, hv_roud_len = null;
            HTuple hv_dist_thresh = null, hv_Length = new HTuple();
            HTuple hv_Row_ = new HTuple(), hv_Col_ = new HTuple();
            HTuple hv_Row = new HTuple(), hv_Col = new HTuple(), hv_idxEnd = new HTuple();
            HTuple hv_Row2 = new HTuple(), hv_Col2 = new HTuple();
            HTuple hv_dAll = new HTuple(), hv_dMin = new HTuple();
            HTuple hv_Index = new HTuple(), hv_DistanceMin = new HTuple();
            HTuple hv_Angle = new HTuple(), hv_row_ = new HTuple();
            HTuple hv_col_ = new HTuple();
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_Wire);
            HOperatorSet.GenEmptyObj(out ho_ObjectSelected);
            HOperatorSet.GenEmptyObj(out ho_ObjectSelected1);
            HOperatorSet.GenEmptyObj(out ho_Seg1);
            HOperatorSet.GenEmptyObj(out ho_Seg2);
            HOperatorSet.GenEmptyObj(out ho_Wires);
            try
            {
                //IMAX := 10000
                hv_Dist = new HTuple();
                ho_Wire.Dispose();
                HOperatorSet.GenEmptyObj(out ho_Wire);
                HOperatorSet.CountObj(ho_Segments, out hv_Num_seg);
                //*****
                ho_ObjectSelected.Dispose();
                HOperatorSet.SelectObj(ho_Segments, out ho_ObjectSelected, 1);
                HOperatorSet.GetContourXld(ho_ObjectSelected, out hv_Row0, out hv_Col0);
                hv_row0 = hv_Row0[0];
                hv_col0 = hv_Col0[0];
                ho_ObjectSelected1.Dispose();
                HOperatorSet.SelectObj(ho_Segments, out ho_ObjectSelected1, hv_Num_seg);
                HOperatorSet.GetContourXld(ho_ObjectSelected1, out hv_Row1, out hv_Col1);
                hv_row1 = hv_Row1[0];
                hv_col1 = hv_Col1[0];
                HOperatorSet.DistancePp(hv_row0, hv_col0, hv_row1, hv_col1, out hv_Distance1);
                //*****
                HOperatorSet.TupleGenConst(hv_Num_seg * hv_Num_seg, 0, out hv_Dist);
                HOperatorSet.TupleGenConst(hv_Num_seg, 0, out hv_flag);
                hv_Num_seg_1 = hv_Num_seg - 1;

                hv_idx_temp_1 = 0;
                hv_idx_temp_2 = 0;
                HTuple end_val21 = hv_Num_seg_1;
                HTuple step_val21 = 1;
                for (hv_idx_k = 1; hv_idx_k.Continue(end_val21, step_val21); hv_idx_k = hv_idx_k.TupleAdd(step_val21))
                {
                    ho_Seg1.Dispose();
                    HOperatorSet.SelectObj(ho_Segments, out ho_Seg1, hv_idx_k);
                    hv_idx_temp_1 = ((hv_idx_k - 1) * hv_Num_seg) - 1;
                    hv_idx_temp_2 = (hv_idx_k - 1) - hv_Num_seg;
                    HTuple end_val25 = hv_Num_seg;
                    HTuple step_val25 = 1;
                    for (hv_idx_l = hv_idx_k + 1; hv_idx_l.Continue(end_val25, step_val25); hv_idx_l = hv_idx_l.TupleAdd(step_val25))
                    {
                        ho_Seg2.Dispose();
                        HOperatorSet.SelectObj(ho_Segments, out ho_Seg2, hv_idx_l);
                        //distance_cc (Seg1, Seg2, 'point_to_point', Distance, DistanceMax)
                        distance_cc_angle(ho_Seg1, ho_Seg2, hv_angle, hv_row0, hv_col0, hv_row1,
                            hv_col1, out hv_Distance);
                        if (hv_Dist == null)
                            hv_Dist = new HTuple();
                        hv_Dist[hv_idx_temp_1 + hv_idx_l] = hv_Distance;
                        if (hv_Dist == null)
                            hv_Dist = new HTuple();
                        hv_Dist[hv_idx_temp_2 + (hv_idx_l * hv_Num_seg)] = hv_Distance;
                    }
                }

                HOperatorSet.TupleGenConst(hv_Num_seg * hv_Num_seg, 0, out hv_road);
                hv_distSmall = hv_Dist.TupleSelectRange(0, hv_Num_seg_1);
                if (hv_flag == null)
                    hv_flag = new HTuple();
                hv_flag[0] = 1;
                hv_pos = 0;
                HOperatorSet.TupleGenConst(hv_Num_seg, 0, out hv_road_);

                //dijkstra algorithm
                HTuple end_val41 = hv_Num_seg_1;
                HTuple step_val41 = 1;
                for (hv_idx_k = 1; hv_idx_k.Continue(end_val41, step_val41); hv_idx_k = hv_idx_k.TupleAdd(step_val41))
                {
                    hv_dist_min = 10000;
                    HTuple end_val43 = hv_Num_seg_1;
                    HTuple step_val43 = 1;
                    for (hv_idx_l = 1; hv_idx_l.Continue(end_val43, step_val43); hv_idx_l = hv_idx_l.TupleAdd(step_val43))
                    {
                        if ((int)((new HTuple(((hv_flag.TupleSelect(hv_idx_l))).TupleEqual(0))).TupleAnd(
                            new HTuple(((hv_distSmall.TupleSelect(hv_idx_l))).TupleLess(hv_dist_min)))) != 0)
                        {
                            hv_dist_min = hv_distSmall.TupleSelect(hv_idx_l);
                            hv_pos = hv_idx_l.Clone();
                        }
                    }
                    if (hv_flag == null)
                        hv_flag = new HTuple();
                    hv_flag[hv_pos] = 1;
                    if (hv_road_ == null)
                        hv_road_ = new HTuple();
                    hv_road_[hv_idx_k] = hv_pos;
                    HTuple end_val51 = hv_Num_seg_1;
                    HTuple step_val51 = 1;
                    for (hv_idx_l = 1; hv_idx_l.Continue(end_val51, step_val51); hv_idx_l = hv_idx_l.TupleAdd(step_val51))
                    {
                        if ((int)((new HTuple(((hv_flag.TupleSelect(hv_idx_l))).TupleEqual(0))).TupleAnd(
                            new HTuple((((hv_distSmall.TupleSelect(hv_pos)) + (hv_Dist.TupleSelect(
                            (hv_pos * hv_Num_seg) + hv_idx_l)))).TupleLess(hv_distSmall.TupleSelect(
                            hv_idx_l))))) != 0)
                        {
                            if (hv_distSmall == null)
                                hv_distSmall = new HTuple();
                            hv_distSmall[hv_idx_l] = (hv_distSmall.TupleSelect(hv_pos)) + (hv_Dist.TupleSelect(
                                (hv_pos * hv_Num_seg) + hv_idx_l));
                            if (hv_road == null)
                                hv_road = new HTuple();
                            hv_road[(hv_pos * hv_Num_seg) + hv_idx_l] = 1;
                            if (hv_road == null)
                                hv_road = new HTuple();
                            hv_road[(hv_idx_l * hv_Num_seg) + hv_pos] = 1;
                        }
                    }
                }

                HOperatorSet.TupleGenConst(hv_Num_seg, 0, out hv_pre);
                HTuple end_val61 = 1;
                HTuple step_val61 = -1;
                for (hv_idx_k = hv_Num_seg_1; hv_idx_k.Continue(end_val61, step_val61); hv_idx_k = hv_idx_k.TupleAdd(step_val61))
                {
                    HTuple end_val62 = 1;
                    HTuple step_val62 = -1;
                    for (hv_idx_l = hv_idx_k - 1; hv_idx_l.Continue(end_val62, step_val62); hv_idx_l = hv_idx_l.TupleAdd(step_val62))
                    {
                        if ((int)(new HTuple(((hv_road.TupleSelect(((hv_road_.TupleSelect(hv_idx_k)) * hv_Num_seg) + (hv_road_.TupleSelect(
                            hv_idx_l))))).TupleEqual(1))) != 0)
                        {
                            if (hv_pre == null)
                                hv_pre = new HTuple();
                            hv_pre[hv_road_.TupleSelect(hv_idx_k)] = hv_road_.TupleSelect(hv_idx_l);
                            break;
                        }
                    }
                }

                hv_road_now = hv_Num_seg_1.Clone();
                hv_road_out = hv_road_now.Clone();
                while ((int)(new HTuple(hv_road_now.TupleGreater(0))) != 0)
                {
                    hv_road_now = hv_pre.TupleSelect(hv_road_now);
                    hv_road_out = hv_road_out.TupleConcat(hv_road_now);
                }

                ho_Wires.Dispose();
                HOperatorSet.GenEmptyObj(out ho_Wires);
                hv_roud_len = new HTuple(hv_road_out.TupleLength());
                hv_dist_thresh = 10;
                HTuple end_val80 = 0;
                HTuple step_val80 = -1;
                for (hv_idx_k = hv_roud_len - 1; hv_idx_k.Continue(end_val80, step_val80); hv_idx_k = hv_idx_k.TupleAdd(step_val80))
                {
                    ho_Seg1.Dispose();
                    HOperatorSet.SelectObj(ho_Segments, out ho_Seg1, (hv_road_out.TupleSelect(
                        hv_idx_k)) + 1);
                    if ((int)(new HTuple(hv_idx_k.TupleEqual(hv_roud_len - 1))) != 0)
                    {
                        HOperatorSet.LengthXld(ho_Seg1, out hv_Length);
                        if ((int)(new HTuple(hv_Length.TupleEqual(0))) != 0)
                        {
                            ho_Seg2.Dispose();
                            HOperatorSet.SelectObj(ho_Segments, out ho_Seg2, (hv_road_out.TupleSelect(
                                hv_idx_k - 1)) + 1);
                            HOperatorSet.GetContourXld(ho_Seg1, out hv_Row_, out hv_Col_);
                            HOperatorSet.TupleSelect(hv_Row_, (((new HTuple(0)).TupleConcat(0)).TupleConcat(
                                0)).TupleConcat(0), out hv_Row1);
                            HOperatorSet.TupleSelect(hv_Col_, (((new HTuple(0)).TupleConcat(0)).TupleConcat(
                                0)).TupleConcat(0), out hv_Col1);
                            HOperatorSet.GetContourXld(ho_Seg2, out hv_Row, out hv_Col);
                            hv_idxEnd = (new HTuple(hv_Row.TupleLength())) - 1;
                            HOperatorSet.TupleSelect(hv_Row, (((((new HTuple(0)).TupleConcat(hv_idxEnd))).TupleConcat(
                                0))).TupleConcat(hv_idxEnd), out hv_Row2);
                            HOperatorSet.TupleSelect(hv_Col, (((((new HTuple(0)).TupleConcat(hv_idxEnd))).TupleConcat(
                                0))).TupleConcat(hv_idxEnd), out hv_Col2);
                            HOperatorSet.DistancePp(hv_Row1, hv_Col1, hv_Row2, hv_Col2, out hv_dAll);
                            HOperatorSet.TupleMin(hv_dAll, out hv_dMin);
                            HOperatorSet.TupleFindFirst(hv_dAll, hv_dMin, out hv_Index);
                            //**********
                            HOperatorSet.DistancePp(hv_Row_.TupleSelect(0), hv_Col_.TupleSelect(0),
                                hv_Row2.TupleSelect(hv_Index), hv_Col2.TupleSelect(hv_Index), out hv_DistanceMin);
                            HOperatorSet.AngleLl(hv_row0, hv_col0, hv_row1, hv_col1, hv_Row_.TupleSelect(
                                0), hv_Col_.TupleSelect(0), hv_Row2.TupleSelect(hv_Index), hv_Col2.TupleSelect(
                                hv_Index), out hv_Angle);
                            if ((int)(new HTuple(((hv_Angle.TupleAbs())).TupleGreater(0.5))) != 0)
                            {
                                ho_Seg1.Dispose();
                                HOperatorSet.GenEmptyObj(out ho_Seg1);
                                continue;
                            }
                            if ((int)(new HTuple(hv_DistanceMin.TupleGreater(hv_dist_thresh))) != 0)
                            {
                                hv_row_ = (hv_Row_.TupleSelect(0)) + ((((hv_Row2.TupleSelect(hv_Index)) - (hv_Row_.TupleSelect(
                                    0))) * hv_dist_thresh) / hv_DistanceMin);
                                hv_col_ = (hv_Col_.TupleSelect(0)) + ((((hv_Col2.TupleSelect(hv_Index)) - (hv_Col_.TupleSelect(
                                    0))) * hv_dist_thresh) / hv_DistanceMin);
                                ho_Seg1.Dispose();
                                HOperatorSet.GenContourPolygonXld(out ho_Seg1, ((hv_Row_.TupleSelect(
                                    0))).TupleConcat(hv_row_), ((hv_Col_.TupleSelect(0))).TupleConcat(
                                    hv_col_));
                            }
                            else
                            {
                                ho_Seg1.Dispose();
                                HOperatorSet.GenContourPolygonXld(out ho_Seg1, ((hv_Row_.TupleSelect(
                                    0))).TupleConcat(hv_Row2.TupleSelect(hv_Index)), ((hv_Col_.TupleSelect(
                                    0))).TupleConcat(hv_Col2.TupleSelect(hv_Index)));
                            }
                        }
                    }
                    if ((int)(new HTuple(hv_idx_k.TupleEqual(0))) != 0)
                    {
                        HOperatorSet.LengthXld(ho_Seg1, out hv_Length);
                        if ((int)(new HTuple(hv_Length.TupleEqual(0))) != 0)
                        {
                            ho_Seg2.Dispose();
                            HOperatorSet.SelectObj(ho_Segments, out ho_Seg2, (hv_road_out.TupleSelect(
                                1)) + 1);
                            HOperatorSet.GetContourXld(ho_Seg1, out hv_Row_, out hv_Col_);
                            HOperatorSet.TupleSelect(hv_Row_, (((new HTuple(0)).TupleConcat(0)).TupleConcat(
                                0)).TupleConcat(0), out hv_Row1);
                            HOperatorSet.TupleSelect(hv_Col_, (((new HTuple(0)).TupleConcat(0)).TupleConcat(
                                0)).TupleConcat(0), out hv_Col1);
                            HOperatorSet.GetContourXld(ho_Seg2, out hv_Row, out hv_Col);
                            hv_idxEnd = (new HTuple(hv_Row.TupleLength())) - 1;
                            HOperatorSet.TupleSelect(hv_Row, (((((new HTuple(0)).TupleConcat(hv_idxEnd))).TupleConcat(
                                0))).TupleConcat(hv_idxEnd), out hv_Row2);
                            HOperatorSet.TupleSelect(hv_Col, (((((new HTuple(0)).TupleConcat(hv_idxEnd))).TupleConcat(
                                0))).TupleConcat(hv_idxEnd), out hv_Col2);
                            HOperatorSet.DistancePp(hv_Row1, hv_Col1, hv_Row2, hv_Col2, out hv_dAll);
                            HOperatorSet.TupleMin(hv_dAll, out hv_dMin);
                            HOperatorSet.TupleFindFirst(hv_dAll, hv_dMin, out hv_Index);
                            HOperatorSet.DistancePp(hv_Row_.TupleSelect(0), hv_Col_.TupleSelect(0),
                                hv_Row2.TupleSelect(hv_Index), hv_Col2.TupleSelect(hv_Index), out hv_DistanceMin);
                            if ((int)(new HTuple(hv_DistanceMin.TupleGreater(hv_dist_thresh))) != 0)
                            {
                                hv_row_ = (hv_Row_.TupleSelect(0)) + ((((hv_Row2.TupleSelect(hv_Index)) - (hv_Row_.TupleSelect(
                                    0))) * hv_dist_thresh) / hv_DistanceMin);
                                hv_col_ = (hv_Col_.TupleSelect(0)) + ((((hv_Col2.TupleSelect(hv_Index)) - (hv_Col_.TupleSelect(
                                    0))) * hv_dist_thresh) / hv_DistanceMin);
                                ho_Seg1.Dispose();
                                HOperatorSet.GenContourPolygonXld(out ho_Seg1, ((hv_Row_.TupleSelect(
                                    0))).TupleConcat(hv_row_), ((hv_Col_.TupleSelect(0))).TupleConcat(
                                    hv_col_));
                            }
                            else
                            {
                                ho_Seg1.Dispose();
                                HOperatorSet.GenContourPolygonXld(out ho_Seg1, ((hv_Row_.TupleSelect(
                                    0))).TupleConcat(hv_Row2.TupleSelect(hv_Index)), ((hv_Col_.TupleSelect(
                                    0))).TupleConcat(hv_Col2.TupleSelect(hv_Index)));
                            }
                        }
                    }
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.ConcatObj(ho_Wires, ho_Seg1, out ExpTmpOutVar_0);
                        ho_Wires.Dispose();
                        ho_Wires = ExpTmpOutVar_0;
                    }
                }
                //select_shape_xld (Wires, SelectedXLD, 'area_points', 'and', 2, 999999)
                ho_Wire.Dispose();
                HOperatorSet.UnionAdjacentContoursXld(ho_Wires, out ho_Wire, hv_MaxDist, 1,
                    "attr_keep");
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.SelectShapeXld(ho_Wire, out ExpTmpOutVar_0, "contlength", "and",
                        hv_Distance1, 99999);
                    ho_Wire.Dispose();
                    ho_Wire = ExpTmpOutVar_0;
                }
                ho_ObjectSelected.Dispose();
                ho_ObjectSelected1.Dispose();
                ho_Seg1.Dispose();
                ho_Seg2.Dispose();
                ho_Wires.Dispose();

                return;
            }
            catch (HalconException HDevExpDefaultException)
            {
                ho_ObjectSelected.Dispose();
                ho_ObjectSelected1.Dispose();
                ho_Seg1.Dispose();
                ho_Seg2.Dispose();
                ho_Wires.Dispose();

                throw HDevExpDefaultException;
            }
        }

        public void track_wire_DP(HObject ho_ImageR, HObject ho_ic_ball, HObject ho_pcb_ball,
            out HObject ho_Wire, out HObject ho_defect_region, HTuple hv_SearchWidth, HTuple hv_LineThr,
            HTuple hv_LineWidth, HTuple hv_MinSegLength, out HTuple hv_iFlag)
        {




            // Stack for temporary objects 
            HObject[] OTemp = new HObject[20];

            // Local iconic variables 

            HObject ho_Rectangle, ho_RegionDifference_;
            HObject ho_RegionDifference, ho_Rectangle1, ho_RegionOpening;
            HObject ho_ImageReduced, ho_Line, ho_LineSplit, ho_SelectedXLD;
            HObject ho_Seg_bin, ho_ObjectSelected = null, ho_Point_start;
            HObject ho_Point_end, ho_Segments;

            // Local control variables 

            HTuple hv_line_length = null, hv_area_pcb = null;
            HTuple hv_RowStart = null, hv_ColStart = null, hv_area_ic = null;
            HTuple hv_RowEnd = null, hv_ColEnd = null, hv_Length = null;
            HTuple hv_angle = null, hv_Number = null, hv_Index = null;
            HTuple hv_RowBegin1 = new HTuple(), hv_ColBegin1 = new HTuple();
            HTuple hv_RowEnd1 = new HTuple(), hv_ColEnd1 = new HTuple();
            HTuple hv_Nr = new HTuple(), hv_Nc = new HTuple(), hv_Dist = new HTuple();
            HTuple hv_Angle = new HTuple(), hv_row_1 = null, hv_col_1 = null;
            HTuple hv_row_0 = null, hv_col_0 = null, hv_max_dist = null;
            HTuple hv_WireLength = null;
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_Wire);
            HOperatorSet.GenEmptyObj(out ho_defect_region);
            HOperatorSet.GenEmptyObj(out ho_Rectangle);
            HOperatorSet.GenEmptyObj(out ho_RegionDifference_);
            HOperatorSet.GenEmptyObj(out ho_RegionDifference);
            HOperatorSet.GenEmptyObj(out ho_Rectangle1);
            HOperatorSet.GenEmptyObj(out ho_RegionOpening);
            HOperatorSet.GenEmptyObj(out ho_ImageReduced);
            HOperatorSet.GenEmptyObj(out ho_Line);
            HOperatorSet.GenEmptyObj(out ho_LineSplit);
            HOperatorSet.GenEmptyObj(out ho_SelectedXLD);
            HOperatorSet.GenEmptyObj(out ho_Seg_bin);
            HOperatorSet.GenEmptyObj(out ho_ObjectSelected);
            HOperatorSet.GenEmptyObj(out ho_Point_start);
            HOperatorSet.GenEmptyObj(out ho_Point_end);
            HOperatorSet.GenEmptyObj(out ho_Segments);
            try
            {
                hv_iFlag = 0;
                //line_length := 6
                ho_defect_region.Dispose();
                HOperatorSet.GenEmptyObj(out ho_defect_region);
                ho_Wire.Dispose();
                HOperatorSet.GenEmptyObj(out ho_Wire);
                HOperatorSet.AreaCenter(ho_pcb_ball, out hv_area_pcb, out hv_RowStart, out hv_ColStart);
                HOperatorSet.AreaCenter(ho_ic_ball, out hv_area_ic, out hv_RowEnd, out hv_ColEnd);
                if ((int)((new HTuple(hv_area_ic.TupleLess(1))).TupleOr(new HTuple(hv_area_pcb.TupleLess(
                    1)))) != 0)
                {
                    hv_iFlag = -1;
                    ho_Rectangle.Dispose();
                    ho_RegionDifference_.Dispose();
                    ho_RegionDifference.Dispose();
                    ho_Rectangle1.Dispose();
                    ho_RegionOpening.Dispose();
                    ho_ImageReduced.Dispose();
                    ho_Line.Dispose();
                    ho_LineSplit.Dispose();
                    ho_SelectedXLD.Dispose();
                    ho_Seg_bin.Dispose();
                    ho_ObjectSelected.Dispose();
                    ho_Point_start.Dispose();
                    ho_Point_end.Dispose();
                    ho_Segments.Dispose();

                    return;
                }
                HOperatorSet.DistancePp(hv_RowStart, hv_ColStart, hv_RowEnd, hv_ColEnd, out hv_Length);
                HOperatorSet.TupleAtan2((-(hv_RowStart - hv_RowEnd)) * 1.0, (hv_ColStart - hv_ColEnd) * 1.0,
                    out hv_angle);
                if ((int)(new HTuple(hv_angle.TupleLess(0))) != 0)
                {
                    hv_angle = ((new HTuple(180)).TupleRad()) + hv_angle;
                }
                //detect lines in the SecondPadRegion of ImageS
                ho_Rectangle.Dispose();
                HOperatorSet.GenRectangle2(out ho_Rectangle, (hv_RowStart + hv_RowEnd) / 2.0, (hv_ColStart + hv_ColEnd) / 2.0,
                    hv_angle, hv_Length / 2.0, hv_SearchWidth);
                ho_RegionDifference_.Dispose();
                HOperatorSet.Difference(ho_Rectangle, ho_pcb_ball, out ho_RegionDifference_
                    );
                ho_RegionDifference.Dispose();
                HOperatorSet.Difference(ho_RegionDifference_, ho_ic_ball, out ho_RegionDifference
                    );
                ho_Rectangle1.Dispose();
                HOperatorSet.GenRectangle2(out ho_Rectangle1, hv_RowStart, hv_ColStart, hv_angle,
                    4, hv_SearchWidth - 2);
                ho_RegionOpening.Dispose();
                HOperatorSet.Opening(ho_RegionDifference, ho_Rectangle1, out ho_RegionOpening
                    );
                ho_ImageReduced.Dispose();
                HOperatorSet.ReduceDomain(ho_ImageR, ho_RegionOpening, out ho_ImageReduced);
                ho_Line.Dispose();
                lines_gauss_iter_1(ho_ImageReduced, out ho_Line, hv_LineThr, hv_LineWidth,
                    hv_MinSegLength, "light");
                ho_LineSplit.Dispose();
                HOperatorSet.SegmentContoursXld(ho_Line, out ho_LineSplit, "lines", 3, 1, 1);
                ho_SelectedXLD.Dispose();
                HOperatorSet.SelectContoursXld(ho_LineSplit, out ho_SelectedXLD, "contour_length",
                    hv_MinSegLength, 8000, -0.5, 0.5);
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.SelectContoursXld(ho_SelectedXLD, out ExpTmpOutVar_0, "open",
                        1, 10000, 0, 0);
                    ho_SelectedXLD.Dispose();
                    ho_SelectedXLD = ExpTmpOutVar_0;
                }
                HOperatorSet.CountObj(ho_SelectedXLD, out hv_Number);
                ho_Seg_bin.Dispose();
                HOperatorSet.GenEmptyObj(out ho_Seg_bin);
                HTuple end_val28 = hv_Number;
                HTuple step_val28 = 1;
                for (hv_Index = 1; hv_Index.Continue(end_val28, step_val28); hv_Index = hv_Index.TupleAdd(step_val28))
                {
                    ho_ObjectSelected.Dispose();
                    HOperatorSet.SelectObj(ho_SelectedXLD, out ho_ObjectSelected, hv_Index);
                    HOperatorSet.FitLineContourXld(ho_ObjectSelected, "tukey", -1, 0, 5, 2, out hv_RowBegin1,
                        out hv_ColBegin1, out hv_RowEnd1, out hv_ColEnd1, out hv_Nr, out hv_Nc,
                        out hv_Dist);
                    if ((int)(new HTuple((new HTuple(hv_RowBegin1.TupleLength())).TupleNotEqual(
                        1))) != 0)
                    {
                        continue;
                    }
                    HOperatorSet.AngleLl(hv_RowBegin1, hv_ColBegin1, hv_RowEnd1, hv_ColEnd1,
                        hv_RowStart, hv_ColStart, hv_RowEnd, hv_ColEnd, out hv_Angle);
                    //tuple_deg (Angle, Deg)
                    if ((int)(new HTuple(hv_Angle.TupleGreater((new HTuple(90)).TupleRad()))) != 0)
                    {
                        hv_Angle = hv_Angle - ((new HTuple(180)).TupleRad());
                    }
                    if ((int)(new HTuple(hv_Angle.TupleLess((new HTuple(-90)).TupleRad()))) != 0)
                    {
                        hv_Angle = hv_Angle + ((new HTuple(180)).TupleRad());
                    }
                    if ((int)(new HTuple(((hv_Angle.TupleAbs())).TupleGreater((new HTuple(30)).TupleRad()
                        ))) != 0)
                    {
                        continue;
                    }
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.ConcatObj(ho_Seg_bin, ho_ObjectSelected, out ExpTmpOutVar_0);
                        ho_Seg_bin.Dispose();
                        ho_Seg_bin = ExpTmpOutVar_0;
                    }
                }
                hv_line_length = 0;
                hv_row_1 = hv_RowStart - ((hv_line_length / hv_Length) * (hv_RowStart - hv_RowEnd));
                hv_col_1 = hv_ColStart - ((hv_line_length / hv_Length) * (hv_ColStart - hv_ColEnd));
                hv_row_0 = hv_RowEnd + ((hv_line_length / hv_Length) * (hv_RowStart - hv_RowEnd));
                hv_col_0 = hv_ColEnd + ((hv_line_length / hv_Length) * (hv_ColStart - hv_ColEnd));
                ho_Point_start.Dispose();
                HOperatorSet.GenContourPolygonXld(out ho_Point_start, hv_RowStart.TupleConcat(
                    hv_row_1), hv_ColStart.TupleConcat(hv_col_1));
                ho_Point_end.Dispose();
                HOperatorSet.GenContourPolygonXld(out ho_Point_end, hv_RowEnd.TupleConcat(hv_row_0),
                    hv_ColEnd.TupleConcat(hv_col_0));
                ho_Segments.Dispose();
                HOperatorSet.ConcatObj(ho_Point_start, ho_Seg_bin, out ho_Segments);
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.ConcatObj(ho_Segments, ho_Point_end, out ExpTmpOutVar_0);
                    ho_Segments.Dispose();
                    ho_Segments = ExpTmpOutVar_0;
                }

                hv_max_dist = 30;
                ho_Wire.Dispose();
                segments_concat_DP(ho_Segments, out ho_Wire, hv_angle, hv_max_dist);
                if (HDevWindowStack.IsOpen())
                {
                    //dev_clear_window ()
                }
                if (HDevWindowStack.IsOpen())
                {
                    //dev_display (ImageR)
                }
                if (HDevWindowStack.IsOpen())
                {
                    //dev_display (Wire)
                }
                HOperatorSet.CountObj(ho_Wire, out hv_Number);
                if ((int)(new HTuple(hv_Number.TupleNotEqual(1))) != 0)
                {
                    ho_Wire.Dispose();
                    HOperatorSet.GenEmptyObj(out ho_Wire);
                    ho_defect_region.Dispose();
                    HOperatorSet.CopyObj(ho_Rectangle, out ho_defect_region, 1, 1);
                    hv_iFlag = -1;
                    ho_Rectangle.Dispose();
                    ho_RegionDifference_.Dispose();
                    ho_RegionDifference.Dispose();
                    ho_Rectangle1.Dispose();
                    ho_RegionOpening.Dispose();
                    ho_ImageReduced.Dispose();
                    ho_Line.Dispose();
                    ho_LineSplit.Dispose();
                    ho_SelectedXLD.Dispose();
                    ho_Seg_bin.Dispose();
                    ho_ObjectSelected.Dispose();
                    ho_Point_start.Dispose();
                    ho_Point_end.Dispose();
                    ho_Segments.Dispose();

                    return;
                }
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.SmoothContoursXld(ho_Wire, out ExpTmpOutVar_0, 11);
                    ho_Wire.Dispose();
                    ho_Wire = ExpTmpOutVar_0;
                }
                HOperatorSet.LengthXld(ho_Wire, out hv_WireLength);
                if ((int)((new HTuple(hv_WireLength.TupleGreater(hv_Length * 1.2))).TupleOr(new HTuple(hv_WireLength.TupleLess(
                    hv_Length * 0.8)))) != 0)
                {
                    ho_Wire.Dispose();
                    HOperatorSet.GenEmptyObj(out ho_Wire);
                    ho_defect_region.Dispose();
                    HOperatorSet.CopyObj(ho_Rectangle, out ho_defect_region, 1, 1);
                    hv_iFlag = -1;
                }
                ho_Rectangle.Dispose();
                ho_RegionDifference_.Dispose();
                ho_RegionDifference.Dispose();
                ho_Rectangle1.Dispose();
                ho_RegionOpening.Dispose();
                ho_ImageReduced.Dispose();
                ho_Line.Dispose();
                ho_LineSplit.Dispose();
                ho_SelectedXLD.Dispose();
                ho_Seg_bin.Dispose();
                ho_ObjectSelected.Dispose();
                ho_Point_start.Dispose();
                ho_Point_end.Dispose();
                ho_Segments.Dispose();

                return;
            }
            catch (HalconException HDevExpDefaultException)
            {
                ho_Rectangle.Dispose();
                ho_RegionDifference_.Dispose();
                ho_RegionDifference.Dispose();
                ho_Rectangle1.Dispose();
                ho_RegionOpening.Dispose();
                ho_ImageReduced.Dispose();
                ho_Line.Dispose();
                ho_LineSplit.Dispose();
                ho_SelectedXLD.Dispose();
                ho_Seg_bin.Dispose();
                ho_ObjectSelected.Dispose();
                ho_Point_start.Dispose();
                ho_Point_end.Dispose();
                ho_Segments.Dispose();

                throw HDevExpDefaultException;
            }
        }

        // Local procedures 
        public void extract_balls(HObject ho_Image, out HObject ho_Balls, HTuple hv_thr,
            HTuple hv_open_radius)
        {




            // Local iconic variables 

            HObject ho_Region, ho_SingleBalls;
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_Balls);
            HOperatorSet.GenEmptyObj(out ho_Region);
            HOperatorSet.GenEmptyObj(out ho_SingleBalls);
            try
            {
                ho_Region.Dispose();
                HOperatorSet.Threshold(ho_Image, out ho_Region, 0, hv_thr);
                ho_Balls.Dispose();
                HOperatorSet.OpeningCircle(ho_Region, out ho_Balls, hv_open_radius);
                ho_SingleBalls.Dispose();
                HOperatorSet.Connection(ho_Balls, out ho_SingleBalls);
                ho_Balls.Dispose();
                HOperatorSet.SelectShape(ho_SingleBalls, out ho_Balls, "area", "and", 30, 200);

                ho_Region.Dispose();
                ho_SingleBalls.Dispose();

                return;
            }
            catch (HalconException HDevExpDefaultException)
            {
                ho_Region.Dispose();
                ho_SingleBalls.Dispose();

                throw HDevExpDefaultException;
            }
        }

        public void union_collinear_wire_segments(HObject ho_Segments, out HObject ho_UnionContours,
            HTuple hv_MaxIterations, HTuple hv_MaxAngle)
        {




            // Stack for temporary objects 
            HObject[] OTemp = new HObject[20];

            // Local iconic variables 

            // Local control variables 

            HTuple hv_Number = null, hv_Lengths = null;
            HTuple hv_MaxDistAbs = null, hv_MaxDistRel = null, hv_Iter = null;
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_UnionContours);
            //Usually there are dark areas around the chip corresponding to shadows
            //which give rise to gaps when tracking the wire. We need to overcome such
            //gaps by joining separate segments.
            //
            //The idea of the lines below is the following: the longer the segments are,
            //the higher are the chances that those segments actually correspond to a
            //wire and, therefore, we decide to join them even when the separation among
            //them is bigger

            HOperatorSet.CountObj(ho_Segments, out hv_Number);
            if ((int)(new HTuple(hv_Number.TupleLess(2))) != 0)
            {
                ho_UnionContours.Dispose();
                HOperatorSet.CopyObj(ho_Segments, out ho_UnionContours, 1, -1);

                return;
            }

            HOperatorSet.LengthXld(ho_Segments, out hv_Lengths);
            hv_MaxDistAbs = hv_Lengths.TupleMax();
            hv_MaxDistRel = (hv_Lengths.TupleMax()) / (hv_Lengths.TupleMin());
            ho_UnionContours.Dispose();
            HOperatorSet.UnionCollinearContoursExtXld(ho_Segments, out ho_UnionContours,
                hv_MaxDistAbs, hv_MaxDistRel, 5, hv_MaxAngle, 0, -1, 1, 1, 1, 1, 1, 0, "attr_keep");
            //
            hv_Iter = 0;
            HOperatorSet.LengthXld(ho_UnionContours, out hv_Lengths);
            while ((int)((new HTuple(hv_Iter.TupleLess(hv_MaxIterations))).TupleAnd(new HTuple((new HTuple(hv_Lengths.TupleLength()
                )).TupleGreater(1)))) != 0)
            {
                hv_MaxDistAbs = 0.4 * (hv_Lengths.TupleMax());
                hv_MaxDistRel = (0.1 * (hv_Lengths.TupleMax())) / (hv_Lengths.TupleMin());
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.UnionCollinearContoursExtXld(ho_UnionContours, out ExpTmpOutVar_0,
                        hv_MaxDistAbs, hv_MaxDistRel, 5, hv_MaxAngle, 0, -1, 1, 1, 1, 1, 1, 0,
                        "attr_keep");
                    ho_UnionContours.Dispose();
                    ho_UnionContours = ExpTmpOutVar_0;
                }
                hv_Iter = hv_Iter + 1;
            }

            return;
        }

        public void lines_gauss_iter(HObject ho_ImageReduced, out HObject ho_SelectedContours,
            HTuple hv_HighThreshold, HTuple hv_LineWidth, HTuple hv_MinSegLength)
        {




            // Local iconic variables 

            HObject ho_Lines = null;

            // Local control variables 

            HTuple hv_NXLDs = null;
            HTuple hv_HighThreshold_COPY_INP_TMP = hv_HighThreshold.Clone();

            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_SelectedContours);
            HOperatorSet.GenEmptyObj(out ho_Lines);
            try
            {
                hv_NXLDs = 0;
                while ((int)((new HTuple(hv_NXLDs.TupleEqual(0))).TupleAnd(new HTuple(hv_HighThreshold_COPY_INP_TMP.TupleGreaterEqual(
                    0.1)))) != 0)
                {
                    ho_Lines.Dispose();
                    HOperatorSet.LinesGauss(ho_ImageReduced, out ho_Lines, hv_LineWidth / ((new HTuple(3)).TupleSqrt()
                        ), 0.05, hv_HighThreshold_COPY_INP_TMP, "dark", "true", "true", "true");
                    ho_SelectedContours.Dispose();
                    HOperatorSet.SelectContoursXld(ho_Lines, out ho_SelectedContours, "contour_length",
                        hv_MinSegLength, 800, -0.5, 0.5);
                    HOperatorSet.CountObj(ho_SelectedContours, out hv_NXLDs);
                    hv_HighThreshold_COPY_INP_TMP = 0.5 * hv_HighThreshold_COPY_INP_TMP;
                }
                ho_Lines.Dispose();

                return;
            }
            catch (HalconException HDevExpDefaultException)
            {
                ho_Lines.Dispose();

                throw HDevExpDefaultException;
            }
        }
        public void scale_image_range(HObject ho_Image, out HObject ho_ImageScaled, HTuple hv_Min,
    HTuple hv_Max)
        {




            // Stack for temporary objects 
            HObject[] OTemp = new HObject[20];

            // Local iconic variables 

            HObject ho_SelectedChannel = null, ho_LowerRegion = null;
            HObject ho_UpperRegion = null;

            // Local copy input parameter variables 
            HObject ho_Image_COPY_INP_TMP;
            ho_Image_COPY_INP_TMP = ho_Image.CopyObj(1, -1);



            // Local control variables 

            HTuple hv_LowerLimit = new HTuple(), hv_UpperLimit = new HTuple();
            HTuple hv_Mult = null, hv_Add = null, hv_Channels = null;
            HTuple hv_Index = null, hv_MinGray = new HTuple(), hv_MaxGray = new HTuple();
            HTuple hv_Range = new HTuple();
            HTuple hv_Max_COPY_INP_TMP = hv_Max.Clone();
            HTuple hv_Min_COPY_INP_TMP = hv_Min.Clone();

            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_ImageScaled);
            HOperatorSet.GenEmptyObj(out ho_SelectedChannel);
            HOperatorSet.GenEmptyObj(out ho_LowerRegion);
            HOperatorSet.GenEmptyObj(out ho_UpperRegion);
            try
            {
                //Convenience procedure to scale the gray values of the
                //input image Image from the interval [Min,Max]
                //to the interval [0,255] (default).
                //Gray values < 0 or > 255 (after scaling) are clipped.
                //
                //If the image shall be scaled to an interval different from [0,255],
                //this can be achieved by passing tuples with 2 values [From, To]
                //as Min and Max.
                //Example:
                //scale_image_range(Image:ImageScaled:[100,50],[200,250])
                //maps the gray values of Image from the interval [100,200] to [50,250].
                //All other gray values will be clipped.
                //
                //input parameters:
                //Image: the input image
                //Min: the minimum gray value which will be mapped to 0
                //     If a tuple with two values is given, the first value will
                //     be mapped to the second value.
                //Max: The maximum gray value which will be mapped to 255
                //     If a tuple with two values is given, the first value will
                //     be mapped to the second value.
                //
                //output parameter:
                //ImageScale: the resulting scaled image
                //
                if ((int)(new HTuple((new HTuple(hv_Min_COPY_INP_TMP.TupleLength())).TupleEqual(
                    2))) != 0)
                {
                    hv_LowerLimit = hv_Min_COPY_INP_TMP[1];
                    hv_Min_COPY_INP_TMP = hv_Min_COPY_INP_TMP[0];
                }
                else
                {
                    hv_LowerLimit = 0.0;
                }
                if ((int)(new HTuple((new HTuple(hv_Max_COPY_INP_TMP.TupleLength())).TupleEqual(
                    2))) != 0)
                {
                    hv_UpperLimit = hv_Max_COPY_INP_TMP[1];
                    hv_Max_COPY_INP_TMP = hv_Max_COPY_INP_TMP[0];
                }
                else
                {
                    hv_UpperLimit = 255.0;
                }
                //
                //Calculate scaling parameters
                hv_Mult = (((hv_UpperLimit - hv_LowerLimit)).TupleReal()) / (hv_Max_COPY_INP_TMP - hv_Min_COPY_INP_TMP);
                hv_Add = ((-hv_Mult) * hv_Min_COPY_INP_TMP) + hv_LowerLimit;
                //
                //Scale image
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.ScaleImage(ho_Image_COPY_INP_TMP, out ExpTmpOutVar_0, hv_Mult,
                        hv_Add);
                    ho_Image_COPY_INP_TMP.Dispose();
                    ho_Image_COPY_INP_TMP = ExpTmpOutVar_0;
                }
                //
                //Clip gray values if necessary
                //This must be done for each channel separately
                HOperatorSet.CountChannels(ho_Image_COPY_INP_TMP, out hv_Channels);
                HTuple end_val48 = hv_Channels;
                HTuple step_val48 = 1;
                for (hv_Index = 1; hv_Index.Continue(end_val48, step_val48); hv_Index = hv_Index.TupleAdd(step_val48))
                {
                    ho_SelectedChannel.Dispose();
                    HOperatorSet.AccessChannel(ho_Image_COPY_INP_TMP, out ho_SelectedChannel,
                        hv_Index);
                    HOperatorSet.MinMaxGray(ho_SelectedChannel, ho_SelectedChannel, 0, out hv_MinGray,
                        out hv_MaxGray, out hv_Range);
                    ho_LowerRegion.Dispose();
                    HOperatorSet.Threshold(ho_SelectedChannel, out ho_LowerRegion, ((hv_MinGray.TupleConcat(
                        hv_LowerLimit))).TupleMin(), hv_LowerLimit);
                    ho_UpperRegion.Dispose();
                    HOperatorSet.Threshold(ho_SelectedChannel, out ho_UpperRegion, hv_UpperLimit,
                        ((hv_UpperLimit.TupleConcat(hv_MaxGray))).TupleMax());
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.PaintRegion(ho_LowerRegion, ho_SelectedChannel, out ExpTmpOutVar_0,
                            hv_LowerLimit, "fill");
                        ho_SelectedChannel.Dispose();
                        ho_SelectedChannel = ExpTmpOutVar_0;
                    }
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.PaintRegion(ho_UpperRegion, ho_SelectedChannel, out ExpTmpOutVar_0,
                            hv_UpperLimit, "fill");
                        ho_SelectedChannel.Dispose();
                        ho_SelectedChannel = ExpTmpOutVar_0;
                    }
                    if ((int)(new HTuple(hv_Index.TupleEqual(1))) != 0)
                    {
                        ho_ImageScaled.Dispose();
                        HOperatorSet.CopyObj(ho_SelectedChannel, out ho_ImageScaled, 1, 1);
                    }
                    else
                    {
                        {
                            HObject ExpTmpOutVar_0;
                            HOperatorSet.AppendChannel(ho_ImageScaled, ho_SelectedChannel, out ExpTmpOutVar_0
                                );
                            ho_ImageScaled.Dispose();
                            ho_ImageScaled = ExpTmpOutVar_0;
                        }
                    }
                }
                ho_Image_COPY_INP_TMP.Dispose();
                ho_SelectedChannel.Dispose();
                ho_LowerRegion.Dispose();
                ho_UpperRegion.Dispose();

                return;
            }
            catch (HalconException HDevExpDefaultException)
            {
                ho_Image_COPY_INP_TMP.Dispose();
                ho_SelectedChannel.Dispose();
                ho_LowerRegion.Dispose();
                ho_UpperRegion.Dispose();

                throw HDevExpDefaultException;
            }
        }
        public void find_auxiliary_ic(HObject ho_ImageLine, HObject ho_ImagePCB, HObject ho_aug_ic,
    out HObject ho_aug_ic_refine, HTuple hv_search_size, HTuple hv_model_id, HTuple hv_model_type,
    HTuple hv_score_thresh, HTuple hv_measure_length1, HTuple hv_measure_length2,
    HTuple hv_measure_threshold, out HTuple hv_iFlag)
        {




            // Local iconic variables 

            HObject ho_RegionDilation, ho_ImagePart, ho_ImagePart1;
            HObject ho_ImageSub, ho_ImageScaled, ho_RegionAffineTrans = null;
            HObject ho_ModelContours = null, ho_ContoursAffinTrans = null;

            // Local control variables 

            HTuple hv_Row1 = null, hv_Column1 = null, hv_Row21 = null;
            HTuple hv_Column21 = null, hv_Min = null, hv_Max = null;
            HTuple hv_Range = null, hv_Row = new HTuple(), hv_Column = new HTuple();
            HTuple hv_Angle = new HTuple(), hv_Score = new HTuple();
            HTuple hv_HomMat2D = new HTuple(), hv_Row3 = new HTuple();
            HTuple hv_Column3 = new HTuple(), hv_Phi1 = new HTuple();
            HTuple hv_Length11 = new HTuple(), hv_Length21 = new HTuple();
            HTuple hv_Width = new HTuple(), hv_Height = new HTuple();
            HTuple hv_MetrologyHandle = new HTuple(), hv_Index1 = new HTuple();
            HTuple hv_Parameter = new HTuple();
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_aug_ic_refine);
            HOperatorSet.GenEmptyObj(out ho_RegionDilation);
            HOperatorSet.GenEmptyObj(out ho_ImagePart);
            HOperatorSet.GenEmptyObj(out ho_ImagePart1);
            HOperatorSet.GenEmptyObj(out ho_ImageSub);
            HOperatorSet.GenEmptyObj(out ho_ImageScaled);
            HOperatorSet.GenEmptyObj(out ho_RegionAffineTrans);
            HOperatorSet.GenEmptyObj(out ho_ModelContours);
            HOperatorSet.GenEmptyObj(out ho_ContoursAffinTrans);
            try
            {
                hv_iFlag = 0;
                ho_aug_ic_refine.Dispose();
                HOperatorSet.GenEmptyObj(out ho_aug_ic_refine);
                //*****************************
                ho_RegionDilation.Dispose();
                HOperatorSet.DilationRectangle1(ho_aug_ic, out ho_RegionDilation, hv_search_size,
                    hv_search_size);
                HOperatorSet.SmallestRectangle1(ho_RegionDilation, out hv_Row1, out hv_Column1,
                    out hv_Row21, out hv_Column21);
                ho_ImagePart.Dispose();
                HOperatorSet.CropRectangle1(ho_ImageLine, out ho_ImagePart, hv_Row1, hv_Column1,
                    hv_Row21, hv_Column21);
                ho_ImagePart1.Dispose();
                HOperatorSet.CropRectangle1(ho_ImagePCB, out ho_ImagePart1, hv_Row1, hv_Column1,
                    hv_Row21, hv_Column21);
                ho_ImageSub.Dispose();
                HOperatorSet.SubImage(ho_ImagePart1, ho_ImagePart, out ho_ImageSub, 1, 0);
                HOperatorSet.MinMaxGray(ho_ImageSub, ho_ImageSub, 10, out hv_Min, out hv_Max,
                    out hv_Range);
                ho_ImageScaled.Dispose();
                scale_image_range(ho_ImageSub, out ho_ImageScaled, hv_Min, hv_Max);
                if ((int)(new HTuple(hv_model_type.TupleEqual(0))) != 0)
                {
                    //none
                }
                else
                {
                    HOperatorSet.FindShapeModel(ho_ImageScaled, hv_model_id, -0.1, 0.2, 0.0,
                        1, 0.5, "least_squares", 0, 0.9, out hv_Row, out hv_Column, out hv_Angle,
                        out hv_Score);
                    if ((int)((new HTuple((new HTuple(hv_Score.TupleLength())).TupleEqual(0))).TupleOr(
                        new HTuple(hv_Score.TupleLess(hv_score_thresh)))) != 0)
                    {
                        HOperatorSet.VectorAngleToRigid(hv_Row1, hv_Column1, 0, 0, 0, 0, out hv_HomMat2D);
                        ho_RegionAffineTrans.Dispose();
                        HOperatorSet.AffineTransRegion(ho_aug_ic, out ho_RegionAffineTrans, hv_HomMat2D,
                            "nearest_neighbor");
                        HOperatorSet.SmallestRectangle2(ho_RegionAffineTrans, out hv_Row3, out hv_Column3,
                            out hv_Phi1, out hv_Length11, out hv_Length21);
                        HOperatorSet.GetImageSize(ho_ImageScaled, out hv_Width, out hv_Height);
                        HOperatorSet.CreateMetrologyModel(out hv_MetrologyHandle);
                        HOperatorSet.SetMetrologyModelImageSize(hv_MetrologyHandle, hv_Width, hv_Height);
                        HOperatorSet.AddMetrologyObjectRectangle2Measure(hv_MetrologyHandle, hv_Row3,
                            hv_Column3, hv_Phi1, hv_Length11, hv_Length21, hv_measure_length1,
                            hv_measure_length2, 1, hv_measure_threshold, "measure_transition",
                            "negative", out hv_Index1);
                        HOperatorSet.ApplyMetrologyModel(ho_ImageScaled, hv_MetrologyHandle);
                        //get_metrology_object_measures (Contours, MetrologyHandle, 'all', 'all', Row4, Column4)
                        //gen_cross_contour_xld (Cross1, Row4, Column4, 6, Angle)
                        HOperatorSet.GetMetrologyObjectResult(hv_MetrologyHandle, "all", "all",
                            "result_type", "all_param", out hv_Parameter);
                        HOperatorSet.ClearMetrologyModel(hv_MetrologyHandle);
                        if ((int)(new HTuple((new HTuple(hv_Parameter.TupleLength())).TupleEqual(
                            5))) != 0)
                        {
                            ho_aug_ic_refine.Dispose();
                            HOperatorSet.GenRectangle2(out ho_aug_ic_refine, (hv_Parameter.TupleSelect(
                                0)) + (hv_Row1.TupleInt()), (hv_Parameter.TupleSelect(1)) + (hv_Column1.TupleInt()
                                ), hv_Parameter.TupleSelect(2), hv_Parameter.TupleSelect(3), hv_Parameter.TupleSelect(
                                4));
                        }
                        else
                        {
                            hv_iFlag = -1;
                            ho_RegionDilation.Dispose();
                            ho_ImagePart.Dispose();
                            ho_ImagePart1.Dispose();
                            ho_ImageSub.Dispose();
                            ho_ImageScaled.Dispose();
                            ho_RegionAffineTrans.Dispose();
                            ho_ModelContours.Dispose();
                            ho_ContoursAffinTrans.Dispose();

                            return;
                        }
                    }
                    else
                    {
                        ho_ModelContours.Dispose();
                        HOperatorSet.GetShapeModelContours(out ho_ModelContours, hv_model_id, 1);
                        HOperatorSet.VectorAngleToRigid(0, 0, 0, hv_Row + (hv_Row1.TupleInt()), hv_Column + (hv_Column1.TupleInt()
                            ), hv_Angle, out hv_HomMat2D);
                        ho_ContoursAffinTrans.Dispose();
                        HOperatorSet.AffineTransContourXld(ho_ModelContours, out ho_ContoursAffinTrans,
                            hv_HomMat2D);
                        ho_aug_ic_refine.Dispose();
                        HOperatorSet.GenRegionContourXld(ho_ContoursAffinTrans, out ho_aug_ic_refine,
                            "filled");
                    }
                }
                ho_RegionDilation.Dispose();
                ho_ImagePart.Dispose();
                ho_ImagePart1.Dispose();
                ho_ImageSub.Dispose();
                ho_ImageScaled.Dispose();
                ho_RegionAffineTrans.Dispose();
                ho_ModelContours.Dispose();
                ho_ContoursAffinTrans.Dispose();

                return;
            }
            catch (HalconException HDevExpDefaultException)
            {
                ho_RegionDilation.Dispose();
                ho_ImagePart.Dispose();
                ho_ImagePart1.Dispose();
                ho_ImageSub.Dispose();
                ho_ImageScaled.Dispose();
                ho_RegionAffineTrans.Dispose();
                ho_ModelContours.Dispose();
                ho_ContoursAffinTrans.Dispose();

                throw HDevExpDefaultException;
            }
        }
        #endregion

        #region CNNPredict
        public string errMsg = "Welcome! No error found.";

        public Function LoadModel(string modelFile, DeviceDescriptor device)
        {
            //errMsg = string.Format("Error: The model '{0}' does not exist.", modelFile);
            ThrowIfFileNotExist(modelFile, string.Format("Error: The model '{0}' does not exist.", modelFile));
            return Function.Load(modelFile, device);
        }

        public HObject Predict(HObject multiChannelImageIn, Function modelFunc, DeviceDescriptor device)
        {
            //input sanity check
            HTuple tupChannelNum;
            HOperatorSet.CountChannels(multiChannelImageIn, out tupChannelNum);
            int channel = tupChannelNum.I;
            if (channel < 1)
            {
                errMsg = "Input image error, channel = 0";
                return null;
            }

            //get image data
            var pointers = new List<IntPtr>();
            HTuple tupWidth = 0, tupHeight = 0;
            for (int i = 0; i < channel; i++)
            {
                HObject tempObj = new HObject();
                HOperatorSet.AccessChannel(multiChannelImageIn, out tempObj, i + 1);
                HTuple tempPointer, tempType;
                HOperatorSet.GetImagePointer1(tempObj, out tempPointer, out tempType, out tupWidth, out tupHeight);
                pointers.Add((IntPtr)tempPointer.L);
            }
            int width = tupWidth.I;
            int height = tupHeight.I;
            List<float> buffer = GetImageAsCHW(pointers, height, width);

            //model sanity check
            Variable inputVar = modelFunc.Arguments.Single();
            NDShape inputShape = inputVar.Shape;
            int imageChannel = inputShape[2];
            int imageWidth = inputShape[1];
            int imageHeight = inputShape[0];
            if (imageChannel != channel || imageWidth != width || imageHeight != height)
            {
                errMsg = "input image's shape does not match to the model's";
                return null;
            }

            //predict
            var inputDataMap = new Dictionary<Variable, Value>();
            var inputVal = Value.CreateBatch(inputShape, buffer, device);
            inputDataMap.Add(inputVar, inputVal);
            Variable outputVar = modelFunc.Output;
            var outputDataMap = new Dictionary<Variable, Value>();
            outputDataMap.Add(outputVar, null);
            modelFunc.Evaluate(inputDataMap, outputDataMap, device);

            //output collection
            var outputVal = outputDataMap[outputVar];
            var outputData = outputVal.GetDenseData<float>(outputVar);
            var outputBufferf = outputData[0].ToArray<float>();
            var multiChannelImageOut = new HObject();
            HOperatorSet.GenEmptyObj(out multiChannelImageOut);
            int channelStride = width * height;
            var bufferByte = new byte[channelStride];
            for (int c = 0; c < channel; c++)
            {
                for (int i = 0; i < channelStride; i++)
                {
                    float temp = (outputBufferf[i + c * channelStride] >= 0 ? outputBufferf[i
                        + c * channelStride] : 0);
                    temp = (temp <= 1.0f ? temp : 1.0f);
                    temp = (temp) * 255.0f;
                    bufferByte[i] = (byte)temp;
                }
                IntPtr unmanagedPointer = Marshal.AllocHGlobal(channelStride);
                Marshal.Copy(bufferByte, 0, unmanagedPointer, channelStride);
                HObject outImage = new HObject();
                HOperatorSet.GenImage1(out outImage, "byte", width, height, unmanagedPointer);
                Marshal.FreeHGlobal(unmanagedPointer);
                HOperatorSet.AppendChannel(multiChannelImageOut, outImage, out multiChannelImageOut);
            }
            return multiChannelImageOut;
        }

        public List<HObject> Predict(List<HObject> multiChannelImageInList, Function modelFunc, DeviceDescriptor device)
        {
            var multiChannelImageOutList = new List<HObject>();
            for (int i = 0; i < multiChannelImageInList.Count; i++)
            {
                multiChannelImageOutList.Add(
                    Predict(multiChannelImageInList[i], modelFunc, device)
                    );
            }
            return multiChannelImageOutList;
        }

        public List<HObject> PredictBatch(List<HObject> multiChannelImageInList, Function modelFunc, DeviceDescriptor device)
        {
            //input sanity check
            foreach (var image in multiChannelImageInList)
            {
                if (image == null)
                {
                    errMsg = "there is null in input image list. please check your input.";
                    return null;
                }
            }

            //get image data
            var bufferBatch = new List<float>();
            HTuple tupWidth = 0, tupHeight = 0;
            HTuple tupChannelNum;
            int channel = 0, width = 0, height = 0;
            foreach (var image in multiChannelImageInList)
            {
                var pointers = new List<IntPtr>();
                HOperatorSet.CountChannels(image, out tupChannelNum);
                channel = tupChannelNum.I;
                for (int i = 0; i < channel; i++)
                {
                    HObject tempObj = new HObject();
                    HOperatorSet.AccessChannel(image, out tempObj, i + 1);
                    HTuple tempPointer, tempType;
                    HOperatorSet.GetImagePointer1(tempObj, out tempPointer, out tempType, out tupWidth, out tupHeight);
                    pointers.Add((IntPtr)tempPointer.L);
                }
                width = tupWidth.I;
                height = tupHeight.I;
                bufferBatch.AddRange(GetImageAsCHW(pointers, height, width));
            }

            //model sanity check
            Variable inputVar = modelFunc.Arguments.Single();
            NDShape inputShape = inputVar.Shape;
            int imageChannel = inputShape[2];
            int imageWidth = inputShape[0];
            int imageHeight = inputShape[1];
            if (imageChannel != channel || imageWidth != width || imageHeight != height)
            {
                errMsg = "input image's shape does not match to the model's";
                return null;
            }

            //predict
            var inputDataMap = new Dictionary<Variable, Value>();
            var inputVal = Value.CreateBatch(inputShape, bufferBatch, device);
            inputDataMap.Add(inputVar, inputVal);
            Variable outputVar = modelFunc.Output;
            var outputDataMap = new Dictionary<Variable, Value>();
            outputDataMap.Add(outputVar, null);
            modelFunc.Evaluate(inputDataMap, outputDataMap, device);

            //output collection
            var multiChannelImageOutList = new List<HObject>();
            var outputVal = outputDataMap[outputVar];
            var outputData = outputVal.GetDenseData<float>(outputVar);
            for (int sample = 0; sample < multiChannelImageInList.Count; sample++)
            {
                var outputBufferf = outputData[sample].ToArray<float>();
                var multiChannelImageOut = new HObject();
                HOperatorSet.GenEmptyObj(out multiChannelImageOut);
                int channelStride = width * height;
                var bufferByte = new byte[channelStride];
                for (int c = 0; c < channel; c++)
                {
                    for (int i = 0; i < channelStride; i++)
                    {
                        float temp = (outputBufferf[i + c * channelStride] >= 0 ? outputBufferf[i
                            + c * channelStride] : 0);
                        temp = (temp <= 1.0f ? temp : 1.0f);
                        temp = (temp) * 255.0f;
                        bufferByte[i] = (byte)temp;
                    }
                    IntPtr unmanagedPointer = Marshal.AllocHGlobal(channelStride);
                    Marshal.Copy(bufferByte, 0, unmanagedPointer, channelStride);
                    HObject outImage = new HObject();
                    HOperatorSet.GenImage1(out outImage, "byte", width, height, unmanagedPointer);
                    Marshal.FreeHGlobal(unmanagedPointer);
                    HOperatorSet.AppendChannel(multiChannelImageOut, outImage, out multiChannelImageOut);
                }
                multiChannelImageOutList.Add(multiChannelImageOut);
            }
            return multiChannelImageOutList;
        }

        public HObject PredictBatch(HObject multiChannelImageInTuple, Function modelFunc, DeviceDescriptor device)
        {
            //input sanity check
            HTuple ImageNumber = new HTuple();
            HOperatorSet.CountObj(multiChannelImageInTuple, out ImageNumber);
            if (ImageNumber.I == 0)
            {
                errMsg = "there is null in input image list. please check your input.";
                return null;
            }
            //get image data
            var bufferBatch = new List<float>();
            HTuple tupWidth = 0, tupHeight = 0;
            HTuple tupChannelNum;
            int channel = 0, width = 0, height = 0;
            HObject image = new HObject();
            for (int index = 0; index < ImageNumber.I; index++)
            {
                var pointers = new List<IntPtr>();
                image.Dispose();
                HOperatorSet.SelectObj(multiChannelImageInTuple, out image, index + 1);
                HOperatorSet.CountChannels(image, out tupChannelNum);
                channel = tupChannelNum.I;
                for (int i = 0; i < channel; i++)
                {
                    HObject tempObj = new HObject();
                    HOperatorSet.AccessChannel(image, out tempObj, i + 1);
                    HTuple tempPointer, tempType;
                    HOperatorSet.GetImagePointer1(tempObj, out tempPointer, out tempType, out tupWidth, out tupHeight);
                    pointers.Add((IntPtr)tempPointer.L);
                }
                width = tupWidth.I;
                height = tupHeight.I;
                bufferBatch.AddRange(GetImageAsCHW(pointers, height, width));
            }

            //model sanity check
            Variable inputVar = modelFunc.Arguments.Single();
            NDShape inputShape = inputVar.Shape;
            int imageChannel = inputShape[2];
            int imageWidth = inputShape[0];
            int imageHeight = inputShape[1];
            if (imageChannel != channel || imageWidth != width || imageHeight != height)
            {
                errMsg = "input image's shape does not match to the model's";
                return null;
            }

            //predict
            var inputDataMap = new Dictionary<Variable, Value>();
            var inputVal = Value.CreateBatch(inputShape, bufferBatch, device);
            inputDataMap.Add(inputVar, inputVal);
            Variable outputVar = modelFunc.Output;
            var outputDataMap = new Dictionary<Variable, Value>();
            outputDataMap.Add(outputVar, null);
            modelFunc.Evaluate(inputDataMap, outputDataMap, device);

            //output collection
            HObject multiChannelImageOutTuple = new HObject();
            HOperatorSet.GenEmptyObj(out multiChannelImageOutTuple);
            var outputVal = outputDataMap[outputVar];
            var outputData = outputVal.GetDenseData<float>(outputVar);
            for (int sample = 0; sample < ImageNumber; sample++)
            {
                var outputBufferf = outputData[sample].ToArray<float>();
                var multiChannelImageOut = new HObject();
                HOperatorSet.GenEmptyObj(out multiChannelImageOut);
                int channelStride = width * height;
                var bufferByte = new byte[channelStride];
                for (int c = 0; c < channel; c++)
                {
                    for (int i = 0; i < channelStride; i++)
                    {
                        float temp = (outputBufferf[i + c * channelStride] >= 0 ? outputBufferf[i
                            + c * channelStride] : 0);
                        temp = (temp <= 1.0f ? temp : 1.0f);
                        temp = (temp) * 255.0f;
                        bufferByte[i] = (byte)temp;
                    }
                    IntPtr unmanagedPointer = Marshal.AllocHGlobal(channelStride);
                    Marshal.Copy(bufferByte, 0, unmanagedPointer, channelStride);
                    HObject outImage = new HObject();
                    HOperatorSet.GenImage1(out outImage, "byte", width, height, unmanagedPointer);
                    Marshal.FreeHGlobal(unmanagedPointer);
                    HOperatorSet.AppendChannel(multiChannelImageOut, outImage, out multiChannelImageOut);
                }
                HObject ExpTmpOutVar_0 = new HObject();
                HOperatorSet.ConcatObj(multiChannelImageOutTuple, multiChannelImageOut, out ExpTmpOutVar_0);
                //multiChannelImageOutTuple = ExpTmpOutVar_0;
                HOperatorSet.CopyObj(ExpTmpOutVar_0, out multiChannelImageOutTuple, 1, -1);
                ExpTmpOutVar_0.Dispose();
            }
            return multiChannelImageOutTuple;
        }

        internal List<float> GetImageAsCHW(List<IntPtr> images, int height, int width)
        {
            int channel = images.Count;
            int channelStride = height * width;
            var buffer = new byte[channel * channelStride];
            for (int c = 0; c < channel; c++)
            {
                System.Runtime.InteropServices.Marshal.Copy(images[c], buffer, c * channelStride, channelStride);
            }
            return buffer.Select(b => (float)(((float)b) / 255.0)).ToList();
        }

        internal void ThrowIfFileNotExist(string filePath, string errorMsg)

        {
            if (!File.Exists(filePath))
            {
                if (!string.IsNullOrEmpty(errorMsg))
                {
                    Console.WriteLine(errorMsg);
                }
                throw new FileNotFoundException(string.Format("File '{0}' not found.", filePath));
            }
        }
        #endregion

    }
}