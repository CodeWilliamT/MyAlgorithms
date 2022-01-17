using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HalconDotNet;

namespace ToolKits.FunctionModule
{
    public class Common
    {
        /// <summary>
        /// 计算耗时
        /// </summary>
        /// <param name="previousTime">起始时间</param>
        /// <returns>返回时间（单位：毫秒）</returns>
        public static Double timeSpan(DateTime startTime)
        {
            DateTime now = DateTime.Now;
            TimeSpan ts = now.Subtract(startTime);
            return ts.TotalMilliseconds;
        }
        /// <summary>
        /// 字符数组转字符串
        /// </summary>
        /// <param name="chars"></param>
        /// <returns></returns>
        public static string CharToString(char[] chars)
        {
            string str = "";
            Int32 len = 0;
            for (Int32 i = 0; i < chars.Length; i++)
            {
                if (chars[i] == '\0')
                {
                    len = i;
                    break;
                }
            }
            //len = (len == 0) ? 0 : len;

            string msg = new string(chars);
            str = msg.Substring(0, len);
            return str;
        }
        /// <summary>
        /// 十进制转二进制，这个主要用于错误码解析，解析出的二进制顺序与正常顺序恰好相反
        /// </summary>
        /// <param name="value"></param>
        /// <param name="binValue"></param>
        public static void IntToBinary(Int32 value, ref Int32[] binValue)
        {
            string bin = Convert.ToString(value, 2);
            Int32 len = bin.Length;
            if (len < 1) return;
            for (Int32 i = 0; i < len; i++)
            {
                binValue[i] = Convert.ToInt32(bin[len - 1 - i].ToString());
            }
        }
        /// <summary>
        /// 根据图像大小等比例缩放图像控件
        /// </summary>
        /// <param name="parentCtrl">父级控件</param>
        /// <param name="imageCtrl">图像控件</param>
        /// <param name="imageWidth">图像宽</param>
        /// <param name="imageHeight">图像高</param>
        /// <param name="widthBlank">父级控件右侧至少保留的宽度</param>
        /// <param name="heightBlank">父级控件下侧至少保留的高度</param>
        public static void ControlResize(Control parentCtrl, Control imageCtrl, Int32 imageWidth, Int32 imageHeight, Int32 widthBlank, Int32 heightBlank)
        {
            if (imageWidth > imageHeight)
            {
                Double factor = imageWidth / (imageHeight * 1.0);
                imageCtrl.Width = ((parentCtrl.Height - heightBlank) * factor > parentCtrl.Width - widthBlank) ? (parentCtrl.Width - widthBlank) : (Convert.ToInt32((parentCtrl.Height - heightBlank) * factor));
                imageCtrl.Height = (parentCtrl.Height - heightBlank > imageCtrl.Width / factor) ? (Convert.ToInt32(imageCtrl.Width / factor)) : (parentCtrl.Height - heightBlank);
            }
            else
            {
                Double factor = imageHeight / (imageWidth * 1.0);
                imageCtrl.Height = ((parentCtrl.Width - widthBlank) * factor > parentCtrl.Height - heightBlank) ? (parentCtrl.Height - heightBlank) : (Convert.ToInt32((parentCtrl.Width - widthBlank) * factor));
                imageCtrl.Width = (parentCtrl.Width - widthBlank > imageCtrl.Height / factor) ? (Convert.ToInt32(imageCtrl.Height / factor)) : (parentCtrl.Width - widthBlank);
            }
        }
    }
    public class Vision
    {
        public struct HalconSystemPrm
        {
            private HTuple imgW;
            private HTuple imgH;
            private HTuple clipRgn;
            public HTuple ImgWidth { get { return this.imgW; } }
            public HTuple ImgHeight { get { return this.imgH; } }
            public HTuple ClipRegion { get { return this.clipRgn; } }
            public HalconSystemPrm(Int32 imgWidth, Int32 imgHeight, string clipRegion)
            {
                this.imgW = imgWidth;
                this.imgH = imgHeight;
                this.clipRgn = clipRegion;
            }
        }
        ///////////////////////////静态方法
        public static void InitHalcon(HalconSystemPrm halconSysPrm)
        {
            if (halconSysPrm.ImgWidth > 0) HOperatorSet.SetSystem("width", halconSysPrm.ImgWidth);
            if (halconSysPrm.ImgHeight > 0) HOperatorSet.SetSystem("height", halconSysPrm.ImgHeight);
            if (halconSysPrm.ClipRegion.Length > 0)
            {
                if (halconSysPrm.ClipRegion == "false" || halconSysPrm.ClipRegion == "true")
                    HOperatorSet.SetSystem("clip_region", halconSysPrm.ClipRegion);
            }
        }
        public static void DrawCross(HTuple rows, HTuple cols, HTuple size, ref HObject cross)
        {
            if (rows == null || cols == null)
            {
                cross.Dispose();
                HOperatorSet.GenEmptyObj(out cross);
                return;
            }
            if (rows.Length != cols.Length)
            {
                cross.Dispose();
                HOperatorSet.GenEmptyObj(out cross);
                return;
            }
            cross.Dispose();
            HOperatorSet.GenCrossContourXld(out cross, rows, cols, size, 0);
        }
        public static void read_image_fast(out HObject ho_Image, HTuple hv_FileName)
        {


            // Local control variables 

            HTuple hv_FileHandle = null, hv_SerializedItemHandle = null;

            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_Image);
            //序列化快速读取不稳定
            HOperatorSet.OpenFile(hv_FileName, "input_binary", out hv_FileHandle);
            HOperatorSet.FreadSerializedItem(hv_FileHandle, out hv_SerializedItemHandle);
            ho_Image.Dispose();
            HOperatorSet.DeserializeImage(out ho_Image, hv_SerializedItemHandle);
            HOperatorSet.CloseFile(hv_FileHandle);

            return;
        }
        public static void write_image_fast(HObject ho_Image, HTuple hv_FileName)
        {



            // Local control variables 

            HTuple hv_FileHandle = null, hv_SerializedItemHandle = null;

            // Initialize local and output iconic variables 

            HOperatorSet.OpenFile(hv_FileName, "output_binary", out hv_FileHandle);
            HOperatorSet.SerializeImage(ho_Image, out hv_SerializedItemHandle);
            HOperatorSet.FwriteSerializedItem(hv_FileHandle, hv_SerializedItemHandle);
            HOperatorSet.CloseFile(hv_FileHandle);

            return;
        }
        /// <summary>
        /// 图像镜像
        /// </summary>
        /// <param name="sourceImage">原始图像</param>
        /// <param name="destImage">镜像后图像</param>
        /// <param name="mirrorMode">镜像模式("row"/"column"/"diagonal")</param>
        public static void MirrorImage(HObject sourceImage, ref HObject destImage, string mirrorMode)
        {
            destImage.Dispose();
            HOperatorSet.MirrorImage(sourceImage, out destImage, mirrorMode);
        }
        /// <summary>
        /// 图像旋转
        /// </summary>
        /// <param name="sourceImage">原始图像</param>
        /// <param name="destImage">镜像后图像</param>
        /// <param name="angle">旋转角度(degree)</param>
        public static void RotateImage(HObject sourceImage, ref HObject destImage, Double angle)
        {
            destImage.Dispose();
            HOperatorSet.RotateImage(sourceImage, out destImage, angle, "bilinear");
        }
        public static void RotateImage(HObject sourceImage, ref HObject destImage, HTuple row1, HTuple col1, HTuple row2, HTuple col2)
        {
            HTuple linePhi = null;
            HOperatorSet.LineOrientation(row1, col1, row2, col2, out linePhi);
            HTuple lineCenterRow = (row1 + row2) / 2.0;
            HTuple lineCenterCol = (col1 + col2) / 2.0;
            HTuple hom2d = null, hom2dRotate = null;
            HOperatorSet.HomMat2dIdentity(out hom2d);
            HOperatorSet.HomMat2dRotate(hom2d, -linePhi, lineCenterRow, lineCenterCol, out hom2dRotate);
            destImage.Dispose();
            HOperatorSet.AffineTransImage(sourceImage, out destImage, hom2dRotate, "bilinear", "false");
        }
        /// <summary>
        /// 读取模板类型和模板文件（模板类型必须是存储在一个文件）
        /// </summary>
        /// <param name="modelPath">模板所在的目录</param>
        /// <param name="modelTypePattern">与模板类型文件匹配的字符串</param>
        /// <param name="modelPattern">与模板文件匹配的字符串</param>
        /// <param name="modelType"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public static bool ReadModel(string modelPath,string modelPattern, string modelTypePattern, out HTuple model,out HTuple modelType)
        {
            modelType = new HTuple();
            model = new HTuple();

            string[] modelTypeFile = System.IO.Directory.GetFiles(modelPath, modelTypePattern);
            if (modelTypeFile.Length != 1) return false;
            else
            {
                HOperatorSet.ReadTuple(modelTypeFile, out modelType);
                return ReadModel(modelPath, modelPattern, modelType, out model);
            }
        }
        /// <summary>
        /// 根据已知模板类型读取模板文件，支持读取多个模板文件
        /// </summary>
        /// <param name="modelPath">模板路径</param>
        /// <param name="modelPattern">与模板文件匹配的字符串信息</param>
        /// <param name="modelType">模板类型</param>
        /// <param name="model"></param>
        /// <returns></returns>
        public static bool ReadModel(string modelPath,string modelPattern, HTuple modelType, out HTuple model)
        {
            model = new HTuple();
            string[] modelFiles = System.IO.Directory.GetFiles(modelPath, modelPattern);
            if (modelFiles.Length != modelType.Length)
            {
                return false;
            }
            else
            {
                for (Int32 i = 0; i < modelType.Length; i++)
                {
                    HTuple _model = new HTuple();
                    switch (modelType[i].I)
                    {
                        case 0:
                            HOperatorSet.ReadNccModel(modelFiles[i], out _model);
                            break;
                        case 1:
                            HOperatorSet.ReadShapeModel(modelFiles[i], out _model);
                            break;
                        default:
                            break;
                    }
                    model.Append(_model);
                }
                return true;
            }
        }
        /// <summary>
        /// 保存一个模板文件
        /// </summary>
        /// <param name="modelFilePath"></param>
        /// <param name="modelType">模板类型长度为1</param>
        /// <param name="model">模板长度为1</param>
        /// <returns></returns>
        public static bool WriteModel(string modelFilePath, HTuple modelType, HTuple model)
        {
            if (model.Length != 1 || modelType.Length != 1) return false;
            switch (modelType.I)
            {
                case 0:
                    HOperatorSet.WriteNccModel(model, modelFilePath);
                    break;
                case 1:
                    HOperatorSet.WriteShapeModel(model, modelFilePath);
                    break;
                default:
                    return false;
            }
            return true;           
        }
        /// <summary>
        /// 清除模板信息和模板类型
        /// </summary>
        /// <param name="modelType"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public static bool ClearModel(HTuple modelType, HTuple model)
        {
            if (model.Length != modelType.Length) return false;
            for (Int32 i = 0; i < model.Length; i++)
            {
                switch (modelType[i].I)
                {
                    case 0:
                        HOperatorSet.ClearNccModel(model[i]);
                        break;
                    case 1:
                        HOperatorSet.ClearShapeModel(model[i]);
                        break;
                    default:
                        return false;
                }
            }
            model = new HTuple();
            modelType = new HTuple();
            return true;
        }
        /// <summary>
        /// 释放模板资源通用函数
        /// </summary>
        /// <param name="hv_model_type"></param>
        /// <param name="hv_model_id"></param>
        /// <param name="hv_iFlag"></param>
        public static void clear_model(HTuple hv_model_type, HTuple hv_model_id, out HTuple hv_iFlag)
        {


            // Local control variables 

            HTuple hv_i = null;

            // Initialize local and output iconic variables 

            hv_iFlag = 0;

            if ((Int32)(new HTuple((new HTuple(hv_model_type.TupleLength())).TupleNotEqual(
                new HTuple(hv_model_id.TupleLength())))) != 0)
            {
                hv_iFlag = -1;

                return;
            }
            for (hv_i = 0; (Int32)hv_i <= (Int32)((new HTuple(hv_model_id.TupleLength())) - 1); hv_i = (Int32)hv_i + 1)
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
            hv_model_id = new HTuple();
            hv_model_type = new HTuple();

            return;
        }
        /// <summary>
        /// 创建模板通用函数
        /// </summary>
        /// <param name="ho_image"></param>
        /// <param name="ho_model_region"></param>
        /// <param name="ho_fore_region"></param>
        /// <param name="hv_angle_start"></param>
        /// <param name="hv_angle_extent"></param>
        /// <param name="hv_model_type"></param>
        /// <param name="hv_model_id"></param>
        /// <param name="hv_iFlag"></param>
        public static void create_model(HObject ho_image, HObject ho_model_region, HObject ho_fore_region,
                                        HTuple hv_angle_start, HTuple hv_angle_extent, HTuple hv_model_type, out HTuple hv_model_id,
                                        out HTuple hv_iFlag)
        {



            // Local iconic variables 

            HObject ho_union_model_region = null, ho_model_image = null;
            HObject ho_select_model_cont = null, ho_modify_model_conts = null;
            HObject ho_binary_image = null;


            // Local control variables 

            HTuple hv_model_area = new HTuple(), hv_model_row = new HTuple();
            HTuple hv_model_col = new HTuple(), hv_model_cont_num = new HTuple();
            HTuple hv_point_rows = new HTuple(), hv_point_cols = new HTuple();
            HTuple hv_i = new HTuple(), hv_point_row = new HTuple();
            HTuple hv_point_col = new HTuple(), hv_model_row1 = new HTuple();
            HTuple hv_model_col1 = new HTuple(), hv_model_row2 = new HTuple();
            HTuple hv_model_col2 = new HTuple(), hv_image_width = new HTuple();
            HTuple hv_image_height = new HTuple();

            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_union_model_region);
            HOperatorSet.GenEmptyObj(out ho_model_image);
            HOperatorSet.GenEmptyObj(out ho_select_model_cont);
            HOperatorSet.GenEmptyObj(out ho_modify_model_conts);
            HOperatorSet.GenEmptyObj(out ho_binary_image);

            hv_model_id = new HTuple();
            try
            {
                hv_iFlag = 0;

                switch (hv_model_type.I)
                {
                    //***NCC
                    case 0:
                        ho_union_model_region.Dispose();
                        HOperatorSet.Union1(ho_model_region, out ho_union_model_region);
                        ho_model_image.Dispose();
                        HOperatorSet.ReduceDomain(ho_image, ho_union_model_region, out ho_model_image
                            );
                        HOperatorSet.CreateNccModel(ho_model_image, "auto", hv_angle_start.TupleRad()
                            , hv_angle_extent.TupleRad(), "auto", "use_polarity", out hv_model_id);
                        HOperatorSet.AreaCenter(ho_union_model_region, out hv_model_area, out hv_model_row,
                            out hv_model_col);
                        HOperatorSet.SetNccModelOrigin(hv_model_id, -hv_model_row, -hv_model_col);
                        break;
                    //***Shape
                    case 1:
                        ho_union_model_region.Dispose();
                        HOperatorSet.Union1(ho_model_region, out ho_union_model_region);
                        ho_model_image.Dispose();
                        HOperatorSet.ReduceDomain(ho_image, ho_union_model_region, out ho_model_image
                            );
                        HOperatorSet.CreateShapeModel(ho_model_image, "auto", hv_angle_start.TupleRad()
                            , hv_angle_extent.TupleRad(), "auto", "auto", "use_polarity", "auto",
                            "auto", out hv_model_id);
                        HOperatorSet.AreaCenter(ho_union_model_region, out hv_model_area, out hv_model_row,
                            out hv_model_col);
                        HOperatorSet.SetShapeModelOrigin(hv_model_id, -hv_model_row, -hv_model_col);
                        break;
                    //***Shape Xld
                    case 2:
                        HOperatorSet.CreateShapeModelXld(ho_model_region, "auto", hv_angle_start.TupleRad()
                            , hv_angle_extent.TupleRad(), "auto", "auto", "ignore_local_polarity",
                            5, out hv_model_id);
                        HOperatorSet.CountObj(ho_model_region, out hv_model_cont_num);
                        hv_point_rows = new HTuple();
                        hv_point_cols = new HTuple();
                        HTuple end_val25 = hv_model_cont_num;
                        HTuple step_val25 = 1;
                        for (hv_i = 1; hv_i.Continue(end_val25, step_val25); hv_i = hv_i.TupleAdd(step_val25))
                        {
                            ho_select_model_cont.Dispose();
                            HOperatorSet.SelectObj(ho_model_region, out ho_select_model_cont, hv_i);
                            HOperatorSet.GetContourXld(ho_select_model_cont, out hv_point_row, out hv_point_col);
                            hv_point_rows = hv_point_rows.TupleConcat(hv_point_row);
                            hv_point_cols = hv_point_cols.TupleConcat(hv_point_col);
                        }
                        ho_modify_model_conts.Dispose();
                        HOperatorSet.GenContourPolygonXld(out ho_modify_model_conts, hv_point_rows,
                            hv_point_cols);
                        HOperatorSet.SmallestRectangle1Xld(ho_modify_model_conts, out hv_model_row1,
                            out hv_model_col1, out hv_model_row2, out hv_model_col2);
                        hv_model_row = ((hv_model_row1.TupleConcat(hv_model_row2))).TupleMean();
                        hv_model_col = ((hv_model_col1.TupleConcat(hv_model_col2))).TupleMean();
                        HOperatorSet.SetShapeModelOrigin(hv_model_id, -hv_model_row, -hv_model_col);
                        break;
                    //***二值图Ncc
                    case 3:
                        HOperatorSet.GetImageSize(ho_image, out hv_image_width, out hv_image_height);
                        ho_binary_image.Dispose();
                        HOperatorSet.RegionToBin(ho_fore_region, out ho_binary_image, 255, 0, hv_image_width,
                            hv_image_height);
                        ho_union_model_region.Dispose();
                        HOperatorSet.Union1(ho_model_region, out ho_union_model_region);
                        ho_model_image.Dispose();
                        HOperatorSet.ReduceDomain(ho_binary_image, ho_union_model_region, out ho_model_image
                            );
                        HOperatorSet.CreateNccModel(ho_model_image, "auto", hv_angle_start.TupleRad()
                            , hv_angle_extent.TupleRad(), "auto", "use_polarity", out hv_model_id);
                        HOperatorSet.AreaCenter(ho_union_model_region, out hv_model_area, out hv_model_row,
                            out hv_model_col);
                        HOperatorSet.SetNccModelOrigin(hv_model_id, -hv_model_row, -hv_model_col);
                        break;
                    default:
                        hv_iFlag = -1;
                        break;
                }

                ho_union_model_region.Dispose();
                ho_model_image.Dispose();
                ho_select_model_cont.Dispose();
                ho_modify_model_conts.Dispose();
                ho_binary_image.Dispose();

                return;
            }
            catch (HalconException HDevExpDefaultException)
            {
                ho_union_model_region.Dispose();
                ho_model_image.Dispose();
                ho_select_model_cont.Dispose();
                ho_modify_model_conts.Dispose();
                ho_binary_image.Dispose();

                throw HDevExpDefaultException;
            }
        }
        public static void create_model(HObject ho_image, HObject ho_model_region, HObject ho_fore_region,
                                HTuple hv_angle_start, HTuple hv_angle_extent,HTuple hv_num_level, HTuple hv_model_type, out HTuple hv_model_id,
                                out HTuple hv_iFlag)
        {



            // Local iconic variables 

            HObject ho_union_model_region = null, ho_model_image = null;
            HObject ho_select_model_cont = null, ho_modify_model_conts = null;
            HObject ho_binary_image = null;


            // Local control variables 

            HTuple hv_model_area = new HTuple(), hv_model_row = new HTuple();
            HTuple hv_model_col = new HTuple(), hv_model_cont_num = new HTuple();
            HTuple hv_point_rows = new HTuple(), hv_point_cols = new HTuple();
            HTuple hv_i = new HTuple(), hv_point_row = new HTuple();
            HTuple hv_point_col = new HTuple(), hv_model_row1 = new HTuple();
            HTuple hv_model_col1 = new HTuple(), hv_model_row2 = new HTuple();
            HTuple hv_model_col2 = new HTuple(), hv_image_width = new HTuple();
            HTuple hv_image_height = new HTuple();

            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_union_model_region);
            HOperatorSet.GenEmptyObj(out ho_model_image);
            HOperatorSet.GenEmptyObj(out ho_select_model_cont);
            HOperatorSet.GenEmptyObj(out ho_modify_model_conts);
            HOperatorSet.GenEmptyObj(out ho_binary_image);

            hv_model_id = new HTuple();
            try
            {
                hv_iFlag = 0;

                switch (hv_model_type.I)
                {
                    //***NCC
                    case 0:
                        ho_union_model_region.Dispose();
                        HOperatorSet.Union1(ho_model_region, out ho_union_model_region);
                        ho_model_image.Dispose();
                        HOperatorSet.ReduceDomain(ho_image, ho_union_model_region, out ho_model_image
                            );
                        HOperatorSet.CreateNccModel(ho_model_image, hv_num_level, hv_angle_start.TupleRad()
                            , hv_angle_extent.TupleRad(), "auto", "use_polarity", out hv_model_id);
                        HOperatorSet.AreaCenter(ho_union_model_region, out hv_model_area, out hv_model_row,
                            out hv_model_col);
                        HOperatorSet.SetNccModelOrigin(hv_model_id, -hv_model_row, -hv_model_col);
                        break;
                    //***Shape
                    case 1:
                        ho_union_model_region.Dispose();
                        HOperatorSet.Union1(ho_model_region, out ho_union_model_region);
                        ho_model_image.Dispose();
                        HOperatorSet.ReduceDomain(ho_image, ho_union_model_region, out ho_model_image
                            );
                        HOperatorSet.CreateShapeModel(ho_model_image, hv_num_level, hv_angle_start.TupleRad()
                            , hv_angle_extent.TupleRad(), "auto", "auto", "use_polarity", "auto",
                            "auto", out hv_model_id);
                        HOperatorSet.AreaCenter(ho_union_model_region, out hv_model_area, out hv_model_row,
                            out hv_model_col);
                        HOperatorSet.SetShapeModelOrigin(hv_model_id, -hv_model_row, -hv_model_col);
                        break;
                    //***Shape Xld
                    case 2:
                        HOperatorSet.CreateShapeModelXld(ho_model_region, hv_num_level, hv_angle_start.TupleRad()
                            , hv_angle_extent.TupleRad(), "auto", "auto", "ignore_local_polarity",
                            5, out hv_model_id);
                        HOperatorSet.CountObj(ho_model_region, out hv_model_cont_num);
                        hv_point_rows = new HTuple();
                        hv_point_cols = new HTuple();
                        HTuple end_val25 = hv_model_cont_num;
                        HTuple step_val25 = 1;
                        for (hv_i = 1; hv_i.Continue(end_val25, step_val25); hv_i = hv_i.TupleAdd(step_val25))
                        {
                            ho_select_model_cont.Dispose();
                            HOperatorSet.SelectObj(ho_model_region, out ho_select_model_cont, hv_i);
                            HOperatorSet.GetContourXld(ho_select_model_cont, out hv_point_row, out hv_point_col);
                            hv_point_rows = hv_point_rows.TupleConcat(hv_point_row);
                            hv_point_cols = hv_point_cols.TupleConcat(hv_point_col);
                        }
                        ho_modify_model_conts.Dispose();
                        HOperatorSet.GenContourPolygonXld(out ho_modify_model_conts, hv_point_rows,
                            hv_point_cols);
                        HOperatorSet.SmallestRectangle1Xld(ho_modify_model_conts, out hv_model_row1,
                            out hv_model_col1, out hv_model_row2, out hv_model_col2);
                        hv_model_row = ((hv_model_row1.TupleConcat(hv_model_row2))).TupleMean();
                        hv_model_col = ((hv_model_col1.TupleConcat(hv_model_col2))).TupleMean();
                        HOperatorSet.SetShapeModelOrigin(hv_model_id, -hv_model_row, -hv_model_col);
                        break;
                    //***二值图Ncc
                    case 3:
                        HOperatorSet.GetImageSize(ho_image, out hv_image_width, out hv_image_height);
                        ho_binary_image.Dispose();
                        HOperatorSet.RegionToBin(ho_fore_region, out ho_binary_image, 255, 0, hv_image_width,
                            hv_image_height);
                        ho_union_model_region.Dispose();
                        HOperatorSet.Union1(ho_model_region, out ho_union_model_region);
                        ho_model_image.Dispose();
                        HOperatorSet.ReduceDomain(ho_binary_image, ho_union_model_region, out ho_model_image
                            );
                        HOperatorSet.CreateNccModel(ho_model_image, hv_num_level, hv_angle_start.TupleRad()
                            , hv_angle_extent.TupleRad(), "auto", "use_polarity", out hv_model_id);
                        HOperatorSet.AreaCenter(ho_union_model_region, out hv_model_area, out hv_model_row,
                            out hv_model_col);
                        HOperatorSet.SetNccModelOrigin(hv_model_id, -hv_model_row, -hv_model_col);
                        break;
                    default:
                        hv_iFlag = -1;
                        break;
                }

                ho_union_model_region.Dispose();
                ho_model_image.Dispose();
                ho_select_model_cont.Dispose();
                ho_modify_model_conts.Dispose();
                ho_binary_image.Dispose();

                return;
            }
            catch (HalconException HDevExpDefaultException)
            {
                ho_union_model_region.Dispose();
                ho_model_image.Dispose();
                ho_select_model_cont.Dispose();
                ho_modify_model_conts.Dispose();
                ho_binary_image.Dispose();

                throw HDevExpDefaultException;
            }
        }
        /// <summary>
        /// 读取模板通用函数
        /// </summary>
        /// <param name="ho_show_contour"></param>
        /// <param name="hv_model_path"></param>
        /// <param name="hv_model_type"></param>
        /// <param name="hv_model_id"></param>
        /// <param name="hv_def_row"></param>
        /// <param name="hv_def_col"></param>
        /// <param name="hv_iFlag"></param>
        public static void read_model(out HObject ho_show_contour, HTuple hv_model_path, out HTuple hv_model_type,
                                      out HTuple hv_model_id, out HTuple hv_def_row, out HTuple hv_def_col, out HTuple hv_iFlag)
        {


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
            if ((Int32)(new HTuple(hv_file_exist.TupleNotEqual(1))) != 0)
            {
                hv_iFlag = -1;

                return;
            }
            HOperatorSet.ReadTuple(hv_model_path + "/modelType.tup", out hv_model_type);
            HOperatorSet.FileExists(hv_model_path + "/modelID.dat", out hv_file_exist);
            if ((Int32)(new HTuple(hv_file_exist.TupleNotEqual(1))) != 0)
            {
                hv_iFlag = -1;

                return;
            }
            if ((Int32)(new HTuple(hv_model_type.TupleEqual(0))) != 0)
            {
                HOperatorSet.ReadNccModel(hv_model_path + "/modelID.dat", out hv_model_id);
            }
            else
            {
                HOperatorSet.ReadShapeModel(hv_model_path + "/modelID.dat", out hv_model_id);
            }
            HOperatorSet.FileExists(hv_model_path + "/showContour.dxf", out hv_file_exist);
            if ((Int32)(new HTuple(hv_file_exist.TupleNotEqual(1))) != 0)
            {
                hv_iFlag = -1;

                return;
            }
            ho_show_contour.Dispose();
            HOperatorSet.ReadContourXldDxf(out ho_show_contour, hv_model_path + "/showContour.dxf",
                new HTuple(), new HTuple(), out hv_DxfStatus);
            HOperatorSet.FileExists(hv_model_path + "/defRow.tup", out hv_file_exist);
            if ((Int32)(new HTuple(hv_file_exist.TupleNotEqual(1))) != 0)
            {
                hv_iFlag = -1;

                return;
            }
            HOperatorSet.ReadTuple(hv_model_path + "/defRow.tup", out hv_def_row);
            HOperatorSet.FileExists(hv_model_path + "/defCol.tup", out hv_file_exist);
            if ((Int32)(new HTuple(hv_file_exist.TupleNotEqual(1))) != 0)
            {
                hv_iFlag = -1;

                return;
            }
            HOperatorSet.ReadTuple(hv_model_path + "/defCol.tup", out hv_def_col);


            return;
        }
        /// <summary>
        /// 保存模板通用函数
        /// </summary>
        /// <param name="ho_show_contour"></param>
        /// <param name="hv_model_path"></param>
        /// <param name="hv_model_type"></param>
        /// <param name="hv_model_id"></param>
        /// <param name="hv_def_row"></param>
        /// <param name="hv_def_col"></param>
        /// <param name="hv_iFlag"></param>
        public static void write_model(HObject ho_show_contour, HTuple hv_model_path, HTuple hv_model_type,
                                       HTuple hv_model_id, HTuple hv_def_row, HTuple hv_def_col, out HTuple hv_iFlag)
        {


            // Initialize local and output iconic variables 

            hv_iFlag = 0;

            if ((Int32)(new HTuple((new HTuple(hv_model_type.TupleLength())).TupleEqual(0))) != 0)
            {
                hv_iFlag = -1;

                return;
            }
            HOperatorSet.WriteTuple(hv_model_type, hv_model_path + "/modelType.tup");
            if ((Int32)(new HTuple((new HTuple(hv_model_id.TupleLength())).TupleEqual(0))) != 0)
            {
                hv_iFlag = -1;

                return;
            }
            if ((Int32)(new HTuple(hv_model_type.TupleEqual(0))) != 0)
            {
                HOperatorSet.WriteNccModel(hv_model_id, hv_model_path + "/modelID.dat");
            }
            else
            {
                HOperatorSet.WriteShapeModel(hv_model_id, hv_model_path + "/modelID.dat");
            }
            HOperatorSet.WriteContourXldDxf(ho_show_contour, hv_model_path + "/showContour.dxf");
            HOperatorSet.WriteTuple(hv_def_row, hv_model_path + "/defRow.tup");
            HOperatorSet.WriteTuple(hv_def_col, hv_model_path + "/defCol.tup");


            return;
        }
        /// <summary>
        /// 模板匹配通用函数
        /// </summary>
        /// <param name="ho_image"></param>
        /// <param name="ho_roi"></param>
        /// <param name="ho_show_contour"></param>
        /// <param name="ho_update_show_contour"></param>
        /// <param name="hv_model_type"></param>
        /// <param name="hv_angle_start"></param>
        /// <param name="hv_angle_extent"></param>
        /// <param name="hv_score_thresh"></param>
        /// <param name="hv_match_num"></param>
        /// <param name="hv_model_id"></param>
        /// <param name="hv_ref_row"></param>
        /// <param name="hv_ref_col"></param>
        /// <param name="hv_def_row"></param>
        /// <param name="hv_def_col"></param>
        /// <param name="hv_comparison_type">区域特征比对方式。-1不必对，0区域平均灰度，1区域灰度方差</param>
        /// <param name="hv_comparison_val">【最小值，最大值】</param>
        /// <param name="hv_found_row"></param>
        /// <param name="hv_found_col"></param>
        /// <param name="hv_found_angle"></param>
        /// <param name="hv_update_def_row"></param>
        /// <param name="hv_update_def_col"></param>
        /// <param name="hv_iFlag">0匹配检测成功；-1匹配成功但区域特征不符合批判标准，-2匹配失败，-3函数执行异常</param>
        public static void find_model(HObject ho_image, HObject ho_roi, HObject ho_show_contour,
                                        out HObject ho_update_show_contour, HTuple hv_model_type, HTuple hv_angle_start,
                                        HTuple hv_angle_extent, HTuple hv_score_thresh, HTuple hv_match_num, HTuple hv_model_id,
                                        HTuple hv_ref_row, HTuple hv_ref_col, HTuple hv_def_row, HTuple hv_def_col,
                                        HTuple hv_comparison_type, HTuple hv_comparison_val, out HTuple hv_found_row,
                                        out HTuple hv_found_col, out HTuple hv_found_angle, out HTuple hv_update_def_row,
                                        out HTuple hv_update_def_col, out HTuple hv_iFlag)
        {



            // Stack for temporary objects 
            HObject[] OTemp = new HObject[20];

            // Local iconic variables 

            HObject ho_complement_region, ho_paint_image;
            HObject ho_reduced_image, ho_affine_show_contour = null, ho_affine_show_region = null;
            HObject ho_affine_cont = null;


            // Local control variables 

            HTuple hv_inv_model_row = new HTuple(), hv_inv_model_col = new HTuple();
            HTuple hv_ncc_numlevs = new HTuple(), hv_angle_step = new HTuple();
            HTuple hv_ncc_metric = new HTuple(), hv_local_row = new HTuple();
            HTuple hv_local_col = new HTuple(), hv_local_angle = new HTuple();
            HTuple hv_local_score = new HTuple(), hv_shape_numlevs = new HTuple();
            HTuple hv_shape_scale_min = new HTuple(), hv_shape_scale_max = new HTuple();
            HTuple hv_shape_scale_step = new HTuple(), hv_shape_metric = new HTuple();
            HTuple hv_shape_min_contrast = new HTuple(), hv_Greatereq = null;
            HTuple hv_GreaterInd = null, hv_model_row = new HTuple();
            HTuple hv_model_col = new HTuple(), hv_update_model_rows = new HTuple();
            HTuple hv_update_model_cols = new HTuple(), hv_update_model_angles = new HTuple();
            HTuple hv_i = new HTuple(), hv_model_H_any = new HTuple();
            HTuple hv_update_model_row = new HTuple(), hv_update_model_col = new HTuple();
            HTuple hv_dists = new HTuple(), hv_dists_indices = new HTuple();
            HTuple hv_min_dist_ind = new HTuple(), hv_mean_val = new HTuple();
            HTuple hv_dev_val = new HTuple(), hv_def_row_ind = new HTuple();
            HTuple hv_def_col_ind = new HTuple(), hv__update_def_row = new HTuple();
            HTuple hv__update_def_col = new HTuple();

            HTuple hv_angle_extent_COPY_INP_TMP = hv_angle_extent.Clone();
            HTuple hv_angle_start_COPY_INP_TMP = hv_angle_start.Clone();

            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_update_show_contour);
            HOperatorSet.GenEmptyObj(out ho_complement_region);
            HOperatorSet.GenEmptyObj(out ho_paint_image);
            HOperatorSet.GenEmptyObj(out ho_reduced_image);
            HOperatorSet.GenEmptyObj(out ho_affine_show_contour);
            HOperatorSet.GenEmptyObj(out ho_affine_show_region);
            HOperatorSet.GenEmptyObj(out ho_affine_cont);

            try
            {
                hv_update_def_row = new HTuple();
                hv_update_def_col = new HTuple();
                hv_found_row = new HTuple();
                hv_found_col = new HTuple();
                hv_found_angle = new HTuple();
                hv_iFlag = 0;
                ho_update_show_contour.Dispose();
                HOperatorSet.GenEmptyObj(out ho_update_show_contour);

                HTuple channel;
                HOperatorSet.CountChannels(ho_image, out channel);
                if (channel.I != 1)
                {
                    HOperatorSet.Rgb1ToGray(ho_image, out ho_image);
                }

                ho_complement_region.Dispose();
                HOperatorSet.Complement(ho_roi, out ho_complement_region);
                ho_paint_image.Dispose();
                HOperatorSet.PaintRegion(ho_complement_region, ho_image, out ho_paint_image,
                    255, "fill");
                ho_reduced_image.Dispose();
                HOperatorSet.ReduceDomain(ho_paint_image, ho_roi, out ho_reduced_image);
                //******NCC/bin Ncc
                if ((Int32)(new HTuple(hv_model_type.TupleEqual(0))) != 0)
                {
                    HOperatorSet.GetNccModelOrigin(hv_model_id, out hv_inv_model_row, out hv_inv_model_col);
                    if ((Int32)((new HTuple(hv_angle_start_COPY_INP_TMP.TupleEqual(-1))).TupleOr(
                        new HTuple(hv_angle_extent_COPY_INP_TMP.TupleEqual(-1)))) != 0)
                    {
                        HOperatorSet.GetNccModelParams(hv_model_id, out hv_ncc_numlevs, out hv_angle_start_COPY_INP_TMP,
                            out hv_angle_extent_COPY_INP_TMP, out hv_angle_step, out hv_ncc_metric);
                    }
                    else
                    {
                        hv_angle_start_COPY_INP_TMP = hv_angle_start_COPY_INP_TMP.TupleRad();
                        hv_angle_extent_COPY_INP_TMP = hv_angle_extent_COPY_INP_TMP.TupleRad();
                    }
                    HOperatorSet.FindNccModel(ho_reduced_image, hv_model_id, hv_angle_start_COPY_INP_TMP,
                        hv_angle_extent_COPY_INP_TMP, 0.5, hv_match_num, 0.5, "true", 0, out hv_local_row,
                        out hv_local_col, out hv_local_angle, out hv_local_score);
                    //******Shape/Shape Xld
                }
                else
                {
                    HOperatorSet.GetShapeModelOrigin(hv_model_id, out hv_inv_model_row, out hv_inv_model_col);
                    if ((Int32)((new HTuple(hv_angle_start_COPY_INP_TMP.TupleEqual(-1))).TupleOr(
                        new HTuple(hv_angle_extent_COPY_INP_TMP.TupleEqual(-1)))) != 0)
                    {
                        HOperatorSet.GetShapeModelParams(hv_model_id, out hv_shape_numlevs, out hv_angle_start_COPY_INP_TMP,
                            out hv_angle_extent_COPY_INP_TMP, out hv_angle_step, out hv_shape_scale_min,
                            out hv_shape_scale_max, out hv_shape_scale_step, out hv_shape_metric,
                            out hv_shape_min_contrast);
                    }
                    else
                    {
                        hv_angle_start_COPY_INP_TMP = hv_angle_start_COPY_INP_TMP.TupleRad();
                        hv_angle_extent_COPY_INP_TMP = hv_angle_extent_COPY_INP_TMP.TupleRad();
                    }
                    HOperatorSet.FindShapeModel(ho_reduced_image, hv_model_id, hv_angle_start_COPY_INP_TMP,
                        hv_angle_extent_COPY_INP_TMP, 0.5, hv_match_num, 0.5, "least_squares",
                        0, 0.9, out hv_local_row, out hv_local_col, out hv_local_angle, out hv_local_score);
                }

                if ((Int32)(new HTuple((new HTuple(hv_local_score.TupleLength())).TupleLess(1))) != 0)
                {
                    hv_iFlag = -2;
                    ho_complement_region.Dispose();
                    ho_paint_image.Dispose();
                    ho_reduced_image.Dispose();
                    ho_affine_show_contour.Dispose();
                    ho_affine_show_region.Dispose();
                    ho_affine_cont.Dispose();

                    return;
                }
                HOperatorSet.TupleGreaterEqualElem(hv_local_score, hv_score_thresh, out hv_Greatereq);
                HOperatorSet.TupleFind(hv_Greatereq, 1, out hv_GreaterInd);
                if ((Int32)(new HTuple(hv_GreaterInd.TupleEqual(-1))) != 0)
                {
                    hv_iFlag = -2;
                    ho_complement_region.Dispose();
                    ho_paint_image.Dispose();
                    ho_reduced_image.Dispose();
                    ho_affine_show_contour.Dispose();
                    ho_affine_show_region.Dispose();
                    ho_affine_cont.Dispose();

                    return;
                }
                else
                {
                    //***获取模板中心坐标
                    hv_model_row = -hv_inv_model_row;
                    hv_model_col = -hv_inv_model_col;
                    //****如果ref_row/ref_col均不为-1，则需要计算距离这个点最近的一个点
                    if ((Int32)((new HTuple(hv_ref_row.TupleNotEqual(-1))).TupleAnd(new HTuple(hv_ref_col.TupleNotEqual(
                        -1)))) != 0)
                    {
                        //***更新实际的模板坐标
                        hv_update_model_rows = new HTuple();
                        hv_update_model_cols = new HTuple();
                        hv_update_model_angles = new HTuple();
                        for (hv_i = 0; (Int32)hv_i <= (Int32)((new HTuple(hv_GreaterInd.TupleLength())) - 1); hv_i = (Int32)hv_i + 1)
                        {
                            HOperatorSet.VectorAngleToRigid(0, 0, 0, hv_local_row.TupleSelect(hv_GreaterInd.TupleSelect(
                                hv_i)), hv_local_col.TupleSelect(hv_GreaterInd.TupleSelect(hv_i)),
                                hv_local_angle.TupleSelect(hv_GreaterInd.TupleSelect(hv_i)), out hv_model_H_any);
                            HOperatorSet.AffineTransPoint2d(hv_model_H_any, hv_model_row, hv_model_col,
                                out hv_update_model_row, out hv_update_model_col);
                            //affine_trans_contour_xld (show_contour, ContoursAffinTrans, model_H_any)
                            hv_update_model_rows = hv_update_model_rows.TupleConcat(hv_update_model_row);
                            hv_update_model_cols = hv_update_model_cols.TupleConcat(hv_update_model_col);
                            hv_update_model_angles = hv_update_model_angles.TupleConcat(hv_local_angle.TupleSelect(
                                hv_GreaterInd.TupleSelect(hv_i)));
                        }

                        if (HDevWindowStack.IsOpen())
                        {
                            //dev_set_color ('blue')
                        }
                        //gen_cross_contour_xld (Cross, update_model_rows, update_model_cols, 36, 0)
                        //stop ()

                        HOperatorSet.DistancePp(hv_update_model_rows, hv_update_model_cols, HTuple.TupleGenConst(
                            new HTuple(hv_update_model_rows.TupleLength()), hv_ref_row), HTuple.TupleGenConst(
                            new HTuple(hv_update_model_rows.TupleLength()), hv_ref_col), out hv_dists);
                        HOperatorSet.TupleSortIndex(hv_dists, out hv_dists_indices);
                        hv_min_dist_ind = hv_GreaterInd.TupleSelect(hv_dists_indices.TupleSelect(
                            0));
                        HOperatorSet.VectorAngleToRigid(0, 0, 0, hv_local_row.TupleSelect(hv_min_dist_ind),
                            hv_local_col.TupleSelect(hv_min_dist_ind), hv_local_angle.TupleSelect(
                            hv_min_dist_ind), out hv_model_H_any);
                        ho_affine_show_contour.Dispose();
                        HOperatorSet.AffineTransContourXld(ho_show_contour, out ho_affine_show_contour,
                            hv_model_H_any);

                        //****根据灰度信息来判断是否为空
                        if ((Int32)(new HTuple(hv_comparison_type.TupleEqual(-1))) != 0)
                        {
                            ho_update_show_contour.Dispose();
                            HOperatorSet.CopyObj(ho_affine_show_contour, out ho_update_show_contour,
                                1, -1);
                        }
                        else
                        {
                            ho_affine_show_region.Dispose();
                            HOperatorSet.GenRegionContourXld(ho_affine_show_contour, out ho_affine_show_region,
                                "filled");
                            HOperatorSet.Intensity(ho_affine_show_region, ho_reduced_image, out hv_mean_val,
                                out hv_dev_val);
                            //****平均灰度
                            if ((Int32)(new HTuple(hv_comparison_type.TupleEqual(0))) != 0)
                            {
                                if ((Int32)((new HTuple(hv_mean_val.TupleLess(hv_comparison_val.TupleSelect(
                                    0)))).TupleOr(new HTuple(hv_mean_val.TupleGreater(hv_comparison_val.TupleSelect(
                                    1))))) != 0)
                                {
                                    hv_iFlag = -1;
                                    ho_complement_region.Dispose();
                                    ho_paint_image.Dispose();
                                    ho_reduced_image.Dispose();
                                    ho_affine_show_contour.Dispose();
                                    ho_affine_show_region.Dispose();
                                    ho_affine_cont.Dispose();

                                    return;
                                }
                                //****灰度方差
                            }
                            else if ((Int32)(new HTuple(hv_comparison_type.TupleEqual(
                                1))) != 0)
                            {
                                if ((Int32)((new HTuple(hv_dev_val.TupleLess(hv_comparison_val.TupleSelect(
                                    0)))).TupleOr(new HTuple(hv_dev_val.TupleGreater(hv_comparison_val.TupleSelect(
                                    1))))) != 0)
                                {
                                    hv_iFlag = -1;
                                    ho_complement_region.Dispose();
                                    ho_paint_image.Dispose();
                                    ho_reduced_image.Dispose();
                                    ho_affine_show_contour.Dispose();
                                    ho_affine_show_region.Dispose();
                                    ho_affine_cont.Dispose();

                                    return;
                                }
                            }
                            ho_update_show_contour.Dispose();
                            HOperatorSet.CopyObj(ho_affine_show_contour, out ho_update_show_contour,
                                1, -1);
                        }

                        hv_found_row = hv_update_model_rows.TupleSelect(hv_min_dist_ind);
                        hv_found_col = hv_update_model_cols.TupleSelect(hv_min_dist_ind);
                        hv_found_angle = ((hv_update_model_angles.TupleSelect(hv_min_dist_ind))).TupleDeg()
                            ;
                        //*****若def_row/def_col均不为-1，则需要将这些点更新
                        HOperatorSet.TupleFind(hv_def_row, -1, out hv_def_row_ind);
                        HOperatorSet.TupleFind(hv_def_col, -1, out hv_def_col_ind);
                        if ((Int32)((new HTuple(hv_def_row_ind.TupleEqual(-1))).TupleAnd(new HTuple(hv_def_col_ind.TupleEqual(
                            -1)))) != 0)
                        {
                            HOperatorSet.AffineTransPoint2d(hv_model_H_any, hv_def_row, hv_def_col,
                                out hv_update_def_row, out hv_update_def_col);
                        }
                        //*****没有ref_row/ref_col参考点，需要找出所有大于匹配分数阈值的目标，并将show_contour依次映射过去
                    }
                    else
                    {
                        HOperatorSet.TupleFind(hv_def_row, -1, out hv_def_row_ind);
                        HOperatorSet.TupleFind(hv_def_col, -1, out hv_def_col_ind);
                        for (hv_i = 0; (Int32)hv_i <= (Int32)((new HTuple(hv_GreaterInd.TupleLength())) - 1); hv_i = (Int32)hv_i + 1)
                        {
                            HOperatorSet.VectorAngleToRigid(0, 0, 0, hv_local_row.TupleSelect(hv_GreaterInd.TupleSelect(
                                hv_i)), hv_local_col.TupleSelect(hv_GreaterInd.TupleSelect(hv_i)),
                                hv_local_angle.TupleSelect(hv_GreaterInd.TupleSelect(hv_i)), out hv_model_H_any);
                            ho_affine_show_contour.Dispose();
                            HOperatorSet.AffineTransContourXld(ho_show_contour, out ho_affine_show_contour,
                                hv_model_H_any);

                            //****根据灰度信息来判断是否为空
                            if ((Int32)(new HTuple(hv_comparison_type.TupleEqual(-1))) != 0)
                            {
                                ho_affine_cont.Dispose();
                                HOperatorSet.CopyObj(ho_affine_show_contour, out ho_affine_cont, 1,
                                    -1);
                            }
                            else
                            {
                                ho_affine_show_region.Dispose();
                                HOperatorSet.GenRegionContourXld(ho_affine_show_contour, out ho_affine_show_region,
                                    "filled");
                                HOperatorSet.Intensity(ho_affine_show_region, ho_reduced_image, out hv_mean_val,
                                    out hv_dev_val);
                                //****平均灰度
                                if ((Int32)(new HTuple(hv_comparison_type.TupleEqual(0))) != 0)
                                {
                                    if ((Int32)((new HTuple(hv_mean_val.TupleLess(hv_comparison_val.TupleSelect(
                                        0)))).TupleOr(new HTuple(hv_mean_val.TupleGreater(hv_comparison_val.TupleSelect(
                                        1))))) != 0)
                                    {
                                        hv_iFlag = -1;
                                        continue;
                                    }
                                    //****灰度方差
                                }
                                else if ((Int32)(new HTuple(hv_comparison_type.TupleEqual(
                                    1))) != 0)
                                {
                                    if ((Int32)((new HTuple(hv_dev_val.TupleLess(hv_comparison_val.TupleSelect(
                                        0)))).TupleOr(new HTuple(hv_dev_val.TupleGreater(hv_comparison_val.TupleSelect(
                                        1))))) != 0)
                                    {
                                        hv_iFlag = -1;
                                        continue;
                                    }
                                }
                                ho_affine_cont.Dispose();
                                HOperatorSet.CopyObj(ho_affine_show_contour, out ho_affine_cont, 1,
                                    -1);
                            }

                            if ((Int32)((new HTuple(hv_def_row_ind.TupleEqual(-1))).TupleAnd(new HTuple(hv_def_col_ind.TupleEqual(
                                -1)))) != 0)
                            {
                                HOperatorSet.AffineTransPoint2d(hv_model_H_any, hv_def_row, hv_def_col,
                                    out hv__update_def_row, out hv__update_def_col);
                            }
                            else
                            {
                                hv__update_def_row = new HTuple();
                                hv__update_def_col = new HTuple();
                            }
                            HOperatorSet.ConcatObj(ho_update_show_contour, ho_affine_cont, out OTemp[0]
                                );
                            ho_update_show_contour.Dispose();
                            ho_update_show_contour = OTemp[0];
                            //*****更新实际的模板坐标
                            HOperatorSet.AffineTransPoint2d(hv_model_H_any, hv_model_row, hv_model_col,
                                out hv_update_model_row, out hv_update_model_col);
                            hv_found_row = hv_found_row.TupleConcat(hv_update_model_row);
                            hv_found_col = hv_found_col.TupleConcat(hv_update_model_col);
                            hv_found_angle = hv_found_angle.TupleConcat(((hv_local_angle.TupleSelect(
                                hv_GreaterInd.TupleSelect(hv_i)))).TupleDeg());
                            hv_update_def_row = hv_update_def_row.TupleConcat(hv__update_def_row);
                            hv_update_def_col = hv_update_def_col.TupleConcat(hv__update_def_col);
                        }
                        if ((Int32)(new HTuple((new HTuple(hv_found_row.TupleLength())).TupleNotEqual(
                            0))) != 0)
                        {
                            hv_iFlag = 0;
                        }
                    }
                }

                ho_complement_region.Dispose();
                ho_paint_image.Dispose();
                ho_reduced_image.Dispose();
                ho_affine_show_contour.Dispose();
                ho_affine_show_region.Dispose();
                ho_affine_cont.Dispose();

                return;
            }
            catch (HalconException)
            {
                hv_update_def_row = new HTuple();
                hv_update_def_col = new HTuple();
                hv_found_row = new HTuple();
                hv_found_col = new HTuple();
                hv_found_angle = new HTuple();
                hv_iFlag = -3;

                ho_complement_region.Dispose();
                ho_paint_image.Dispose();
                ho_reduced_image.Dispose();
                ho_affine_show_contour.Dispose();
                ho_affine_show_region.Dispose();
                ho_affine_cont.Dispose();

                //throw HDevExpDefaultException;
            }
        }
        /// <summary>
        /// 模板匹配通用函数，返回匹配分数
        /// </summary>
        /// <param name="ho_image"></param>
        /// <param name="ho_roi"></param>
        /// <param name="ho_show_contour"></param>
        /// <param name="ho_update_show_contour"></param>
        /// <param name="hv_model_type"></param>
        /// <param name="hv_angle_start"></param>
        /// <param name="hv_angle_extent"></param>
        /// <param name="hv_score_thresh"></param>
        /// <param name="hv_match_num"></param>
        /// <param name="hv_model_id"></param>
        /// <param name="hv_ref_row"></param>
        /// <param name="hv_ref_col"></param>
        /// <param name="hv_def_row"></param>
        /// <param name="hv_def_col"></param>
        /// <param name="hv_comparison_type"></param>
        /// <param name="hv_comparison_val"></param>
        /// <param name="hv_found_row"></param>
        /// <param name="hv_found_col"></param>
        /// <param name="hv_found_angle"></param>
        /// <param name="hv_found_score"></param>
        /// <param name="hv_update_def_row"></param>
        /// <param name="hv_update_def_col"></param>
        /// <param name="hv_iFlag">0匹配成功；-1匹配成功，但区域特征不满足标准，-2匹配失败，-3算法异常</param>
        public static void find_model(HObject ho_image, HObject ho_roi, HObject ho_show_contour,
                                        out HObject ho_update_show_contour, HTuple hv_model_type, HTuple hv_angle_start,
                                        HTuple hv_angle_extent, HTuple hv_score_thresh, HTuple hv_match_num, HTuple hv_model_id,
                                        HTuple hv_ref_row, HTuple hv_ref_col, HTuple hv_def_row, HTuple hv_def_col,
                                        HTuple hv_comparison_type, HTuple hv_comparison_val, out HTuple hv_found_row,
                                        out HTuple hv_found_col, out HTuple hv_found_angle, out HTuple hv_found_score,
                                        out HTuple hv_update_def_row, out HTuple hv_update_def_col, out HTuple hv_iFlag)
        {



            // Stack for temporary objects 
            HObject[] OTemp = new HObject[20];

            // Local iconic variables 

            HObject ho_complement_region, ho_paint_image;
            HObject ho_reduced_image, ho_affine_show_contour = null, ho_affine_show_region = null;
            HObject ho_affine_cont = null;


            // Local control variables 

            HTuple hv_inv_model_row = new HTuple(), hv_inv_model_col = new HTuple();
            HTuple hv_ncc_numlevs = new HTuple(), hv_angle_step = new HTuple();
            HTuple hv_ncc_metric = new HTuple(), hv_local_row = new HTuple();
            HTuple hv_local_col = new HTuple(), hv_local_angle = new HTuple();
            HTuple hv_local_score = new HTuple(), hv_shape_numlevs = new HTuple();
            HTuple hv_shape_scale_min = new HTuple(), hv_shape_scale_max = new HTuple();
            HTuple hv_shape_scale_step = new HTuple(), hv_shape_metric = new HTuple();
            HTuple hv_shape_min_contrast = new HTuple(), hv_Greatereq = null;
            HTuple hv_GreaterInd = null, hv_model_row = new HTuple();
            HTuple hv_model_col = new HTuple(), hv_update_model_rows = new HTuple();
            HTuple hv_update_model_cols = new HTuple(), hv_update_model_angles = new HTuple();
            HTuple hv_update_model_scores = new HTuple(), hv_i = new HTuple();
            HTuple hv_model_H_any = new HTuple(), hv_update_model_row = new HTuple();
            HTuple hv_update_model_col = new HTuple(), hv_dists = new HTuple();
            HTuple hv_dists_indices = new HTuple(), hv_min_dist_ind = new HTuple();
            HTuple hv_mean_val = new HTuple(), hv_dev_val = new HTuple();
            HTuple hv_def_row_ind = new HTuple(), hv_def_col_ind = new HTuple();
            HTuple hv__update_def_row = new HTuple(), hv__update_def_col = new HTuple();

            HTuple hv_angle_extent_COPY_INP_TMP = hv_angle_extent.Clone();
            HTuple hv_angle_start_COPY_INP_TMP = hv_angle_start.Clone();

            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_update_show_contour);
            HOperatorSet.GenEmptyObj(out ho_complement_region);
            HOperatorSet.GenEmptyObj(out ho_paint_image);
            HOperatorSet.GenEmptyObj(out ho_reduced_image);
            HOperatorSet.GenEmptyObj(out ho_affine_show_contour);
            HOperatorSet.GenEmptyObj(out ho_affine_show_region);
            HOperatorSet.GenEmptyObj(out ho_affine_cont);

            hv_found_score = new HTuple();
            try
            {
                hv_update_def_row = new HTuple();
                hv_update_def_col = new HTuple();
                hv_found_row = new HTuple();
                hv_found_col = new HTuple();
                hv_found_angle = new HTuple();
                hv_iFlag = 0;
                ho_update_show_contour.Dispose();
                HOperatorSet.GenEmptyObj(out ho_update_show_contour);

                HTuple channel;
                HOperatorSet.CountChannels(ho_image, out channel);
                if (channel.I != 1)
                {
                    HOperatorSet.Rgb1ToGray(ho_image, out ho_image);
                }

                ho_complement_region.Dispose();
                HOperatorSet.Complement(ho_roi, out ho_complement_region);
                ho_paint_image.Dispose();
                HOperatorSet.PaintRegion(ho_complement_region, ho_image, out ho_paint_image,
                    255, "fill");
                ho_reduced_image.Dispose();
                HOperatorSet.ReduceDomain(ho_paint_image, ho_roi, out ho_reduced_image);
                //******NCC/bin Ncc
                if ((Int32)(new HTuple(hv_model_type.TupleEqual(0))) != 0)
                {
                    HOperatorSet.GetNccModelOrigin(hv_model_id, out hv_inv_model_row, out hv_inv_model_col);
                    if ((Int32)((new HTuple(hv_angle_start_COPY_INP_TMP.TupleEqual(-1))).TupleOr(
                        new HTuple(hv_angle_extent_COPY_INP_TMP.TupleEqual(-1)))) != 0)
                    {
                        HOperatorSet.GetNccModelParams(hv_model_id, out hv_ncc_numlevs, out hv_angle_start_COPY_INP_TMP,
                            out hv_angle_extent_COPY_INP_TMP, out hv_angle_step, out hv_ncc_metric);
                    }
                    else
                    {
                        hv_angle_start_COPY_INP_TMP = hv_angle_start_COPY_INP_TMP.TupleRad();
                        hv_angle_extent_COPY_INP_TMP = hv_angle_extent_COPY_INP_TMP.TupleRad();
                    }
                    HOperatorSet.FindNccModel(ho_reduced_image, hv_model_id, hv_angle_start_COPY_INP_TMP,
                        hv_angle_extent_COPY_INP_TMP, 0.5, hv_match_num, 0.5, "true", 0, out hv_local_row,
                        out hv_local_col, out hv_local_angle, out hv_local_score);
                    //******Shape/Shape Xld
                }
                else
                {
                    HOperatorSet.GetShapeModelOrigin(hv_model_id, out hv_inv_model_row, out hv_inv_model_col);
                    if ((Int32)((new HTuple(hv_angle_start_COPY_INP_TMP.TupleEqual(-1))).TupleOr(
                        new HTuple(hv_angle_extent_COPY_INP_TMP.TupleEqual(-1)))) != 0)
                    {
                        HOperatorSet.GetShapeModelParams(hv_model_id, out hv_shape_numlevs, out hv_angle_start_COPY_INP_TMP,
                            out hv_angle_extent_COPY_INP_TMP, out hv_angle_step, out hv_shape_scale_min,
                            out hv_shape_scale_max, out hv_shape_scale_step, out hv_shape_metric,
                            out hv_shape_min_contrast);
                    }
                    else
                    {
                        hv_angle_start_COPY_INP_TMP = hv_angle_start_COPY_INP_TMP.TupleRad();
                        hv_angle_extent_COPY_INP_TMP = hv_angle_extent_COPY_INP_TMP.TupleRad();
                    }
                    HOperatorSet.FindShapeModel(ho_reduced_image, hv_model_id, hv_angle_start_COPY_INP_TMP,
                        hv_angle_extent_COPY_INP_TMP, 0.5, hv_match_num, 0.5, "least_squares",
                        0, 0.9, out hv_local_row, out hv_local_col, out hv_local_angle, out hv_local_score);
                }

                if ((Int32)(new HTuple((new HTuple(hv_local_score.TupleLength())).TupleLess(1))) != 0)
                {
                    hv_iFlag = -2;
                    ho_complement_region.Dispose();
                    ho_paint_image.Dispose();
                    ho_reduced_image.Dispose();
                    ho_affine_show_contour.Dispose();
                    ho_affine_show_region.Dispose();
                    ho_affine_cont.Dispose();

                    return;
                }
                HOperatorSet.TupleGreaterEqualElem(hv_local_score, hv_score_thresh, out hv_Greatereq);
                HOperatorSet.TupleFind(hv_Greatereq, 1, out hv_GreaterInd);
                if ((Int32)(new HTuple(hv_GreaterInd.TupleEqual(-1))) != 0)
                {
                    hv_iFlag = -2;
                    ho_complement_region.Dispose();
                    ho_paint_image.Dispose();
                    ho_reduced_image.Dispose();
                    ho_affine_show_contour.Dispose();
                    ho_affine_show_region.Dispose();
                    ho_affine_cont.Dispose();

                    return;
                }
                else
                {
                    //***获取模板中心坐标
                    hv_model_row = -hv_inv_model_row;
                    hv_model_col = -hv_inv_model_col;
                    //****如果ref_row/ref_col均不为-1，则需要计算距离这个点最近的一个点
                    if ((Int32)((new HTuple(hv_ref_row.TupleNotEqual(-1))).TupleAnd(new HTuple(hv_ref_col.TupleNotEqual(
                        -1)))) != 0)
                    {
                        //***更新实际的模板坐标
                        hv_update_model_rows = new HTuple();
                        hv_update_model_cols = new HTuple();
                        hv_update_model_angles = new HTuple();
                        hv_update_model_scores = new HTuple();
                        for (hv_i = 0; (Int32)hv_i <= (Int32)((new HTuple(hv_GreaterInd.TupleLength())) - 1); hv_i = (Int32)hv_i + 1)
                        {
                            HOperatorSet.VectorAngleToRigid(0, 0, 0, hv_local_row.TupleSelect(hv_GreaterInd.TupleSelect(
                                hv_i)), hv_local_col.TupleSelect(hv_GreaterInd.TupleSelect(hv_i)),
                                hv_local_angle.TupleSelect(hv_GreaterInd.TupleSelect(hv_i)), out hv_model_H_any);
                            HOperatorSet.AffineTransPoint2d(hv_model_H_any, hv_model_row, hv_model_col,
                                out hv_update_model_row, out hv_update_model_col);
                            //affine_trans_contour_xld (show_contour, ContoursAffinTrans, model_H_any)
                            hv_update_model_rows = hv_update_model_rows.TupleConcat(hv_update_model_row);
                            hv_update_model_cols = hv_update_model_cols.TupleConcat(hv_update_model_col);
                            hv_update_model_angles = hv_update_model_angles.TupleConcat(hv_local_angle.TupleSelect(
                                hv_GreaterInd.TupleSelect(hv_i)));
                            hv_update_model_scores = hv_update_model_scores.TupleConcat(hv_local_score.TupleSelect(
                                hv_GreaterInd.TupleSelect(hv_i)));
                        }

                        //dev_set_color ('blue')
                        //gen_cross_contour_xld (Cross, update_model_rows, update_model_cols, 36, 0)
                        //stop ()

                        HOperatorSet.DistancePp(hv_update_model_rows, hv_update_model_cols, HTuple.TupleGenConst(
                            new HTuple(hv_update_model_rows.TupleLength()), hv_ref_row), HTuple.TupleGenConst(
                            new HTuple(hv_update_model_rows.TupleLength()), hv_ref_col), out hv_dists);
                        HOperatorSet.TupleSortIndex(hv_dists, out hv_dists_indices);
                        hv_min_dist_ind = hv_GreaterInd.TupleSelect(hv_dists_indices.TupleSelect(
                            0));
                        HOperatorSet.VectorAngleToRigid(0, 0, 0, hv_local_row.TupleSelect(hv_min_dist_ind),
                            hv_local_col.TupleSelect(hv_min_dist_ind), hv_local_angle.TupleSelect(
                            hv_min_dist_ind), out hv_model_H_any);
                        ho_affine_show_contour.Dispose();
                        HOperatorSet.AffineTransContourXld(ho_show_contour, out ho_affine_show_contour,
                            hv_model_H_any);

                        //****根据灰度信息来判断是否为空
                        if ((Int32)(new HTuple(hv_comparison_type.TupleEqual(-1))) != 0)
                        {
                            ho_update_show_contour.Dispose();
                            HOperatorSet.CopyObj(ho_affine_show_contour, out ho_update_show_contour,
                                1, -1);
                        }
                        else
                        {
                            ho_affine_show_region.Dispose();
                            HOperatorSet.GenRegionContourXld(ho_affine_show_contour, out ho_affine_show_region,
                                "filled");
                            HOperatorSet.Intensity(ho_affine_show_region, ho_reduced_image, out hv_mean_val,
                                out hv_dev_val);
                            //****平均灰度
                            if ((Int32)(new HTuple(hv_comparison_type.TupleEqual(0))) != 0)
                            {
                                if ((Int32)((new HTuple(hv_mean_val.TupleLess(hv_comparison_val.TupleSelect(
                                    0)))).TupleOr(new HTuple(hv_mean_val.TupleGreater(hv_comparison_val.TupleSelect(
                                    1))))) != 0)
                                {
                                    hv_iFlag = -1;
                                    ho_complement_region.Dispose();
                                    ho_paint_image.Dispose();
                                    ho_reduced_image.Dispose();
                                    ho_affine_show_contour.Dispose();
                                    ho_affine_show_region.Dispose();
                                    ho_affine_cont.Dispose();

                                    return;
                                }
                                //****灰度方差
                            }
                            else if ((Int32)(new HTuple(hv_comparison_type.TupleEqual(
                                1))) != 0)
                            {
                                if ((Int32)((new HTuple(hv_dev_val.TupleLess(hv_comparison_val.TupleSelect(
                                    0)))).TupleOr(new HTuple(hv_dev_val.TupleGreater(hv_comparison_val.TupleSelect(
                                    1))))) != 0)
                                {
                                    hv_iFlag = -1;
                                    ho_complement_region.Dispose();
                                    ho_paint_image.Dispose();
                                    ho_reduced_image.Dispose();
                                    ho_affine_show_contour.Dispose();
                                    ho_affine_show_region.Dispose();
                                    ho_affine_cont.Dispose();

                                    return;
                                }
                            }
                            ho_update_show_contour.Dispose();
                            HOperatorSet.CopyObj(ho_affine_show_contour, out ho_update_show_contour,
                                1, -1);
                        }

                        hv_found_row = hv_update_model_rows.TupleSelect(hv_min_dist_ind);
                        hv_found_col = hv_update_model_cols.TupleSelect(hv_min_dist_ind);
                        hv_found_angle = ((hv_update_model_angles.TupleSelect(hv_min_dist_ind))).TupleDeg()
                            ;
                        hv_found_score = hv_update_model_scores.TupleSelect(hv_min_dist_ind);
                        //*****若def_row/def_col均不为-1，则需要将这些点更新
                        HOperatorSet.TupleFind(hv_def_row, -1, out hv_def_row_ind);
                        HOperatorSet.TupleFind(hv_def_col, -1, out hv_def_col_ind);
                        if ((Int32)((new HTuple(hv_def_row_ind.TupleEqual(-1))).TupleAnd(new HTuple(hv_def_col_ind.TupleEqual(
                            -1)))) != 0)
                        {
                            HOperatorSet.AffineTransPoint2d(hv_model_H_any, hv_def_row, hv_def_col,
                                out hv_update_def_row, out hv_update_def_col);
                        }
                        //*****没有ref_row/ref_col参考点，需要找出所有大于匹配分数阈值的目标，并将show_contour依次映射过去
                    }
                    else
                    {
                        HOperatorSet.TupleFind(hv_def_row, -1, out hv_def_row_ind);
                        HOperatorSet.TupleFind(hv_def_col, -1, out hv_def_col_ind);
                        for (hv_i = 0; (Int32)hv_i <= (Int32)((new HTuple(hv_GreaterInd.TupleLength())) - 1); hv_i = (Int32)hv_i + 1)
                        {
                            HOperatorSet.VectorAngleToRigid(0, 0, 0, hv_local_row.TupleSelect(hv_GreaterInd.TupleSelect(
                                hv_i)), hv_local_col.TupleSelect(hv_GreaterInd.TupleSelect(hv_i)),
                                hv_local_angle.TupleSelect(hv_GreaterInd.TupleSelect(hv_i)), out hv_model_H_any);
                            ho_affine_show_contour.Dispose();
                            HOperatorSet.AffineTransContourXld(ho_show_contour, out ho_affine_show_contour,
                                hv_model_H_any);

                            //****根据灰度信息来判断是否为空
                            if ((Int32)(new HTuple(hv_comparison_type.TupleEqual(-1))) != 0)
                            {
                                ho_affine_cont.Dispose();
                                HOperatorSet.CopyObj(ho_affine_show_contour, out ho_affine_cont, 1,
                                    -1);
                            }
                            else
                            {
                                ho_affine_show_region.Dispose();
                                HOperatorSet.GenRegionContourXld(ho_affine_show_contour, out ho_affine_show_region,
                                    "filled");
                                HOperatorSet.Intensity(ho_affine_show_region, ho_reduced_image, out hv_mean_val,
                                    out hv_dev_val);
                                //****平均灰度
                                if ((Int32)(new HTuple(hv_comparison_type.TupleEqual(0))) != 0)
                                {
                                    if ((Int32)((new HTuple(hv_mean_val.TupleLess(hv_comparison_val.TupleSelect(
                                        0)))).TupleOr(new HTuple(hv_mean_val.TupleGreater(hv_comparison_val.TupleSelect(
                                        1))))) != 0)
                                    {
                                        hv_iFlag = -1;
                                        continue;
                                    }
                                    //****灰度方差
                                }
                                else if ((Int32)(new HTuple(hv_comparison_type.TupleEqual(
                                    1))) != 0)
                                {
                                    if ((Int32)((new HTuple(hv_dev_val.TupleLess(hv_comparison_val.TupleSelect(
                                        0)))).TupleOr(new HTuple(hv_dev_val.TupleGreater(hv_comparison_val.TupleSelect(
                                        1))))) != 0)
                                    {
                                        hv_iFlag = -1;
                                        continue;
                                    }
                                }
                                ho_affine_cont.Dispose();
                                HOperatorSet.CopyObj(ho_affine_show_contour, out ho_affine_cont, 1,
                                    -1);
                            }

                            if ((Int32)((new HTuple(hv_def_row_ind.TupleEqual(-1))).TupleAnd(new HTuple(hv_def_col_ind.TupleEqual(
                                -1)))) != 0)
                            {
                                HOperatorSet.AffineTransPoint2d(hv_model_H_any, hv_def_row, hv_def_col,
                                    out hv__update_def_row, out hv__update_def_col);
                            }
                            else
                            {
                                hv__update_def_row = new HTuple();
                                hv__update_def_col = new HTuple();
                            }
                            HOperatorSet.ConcatObj(ho_update_show_contour, ho_affine_cont, out OTemp[0]
                                );
                            ho_update_show_contour.Dispose();
                            ho_update_show_contour = OTemp[0];
                            //*****更新实际的模板坐标
                            HOperatorSet.AffineTransPoint2d(hv_model_H_any, hv_model_row, hv_model_col,
                                out hv_update_model_row, out hv_update_model_col);
                            hv_found_row = hv_found_row.TupleConcat(hv_update_model_row);
                            hv_found_col = hv_found_col.TupleConcat(hv_update_model_col);
                            hv_found_angle = hv_found_angle.TupleConcat(((hv_local_angle.TupleSelect(
                                hv_GreaterInd.TupleSelect(hv_i)))).TupleDeg());
                            hv_found_score = hv_found_score.TupleConcat(hv_local_score.TupleSelect(
                                hv_GreaterInd.TupleSelect(hv_i)));
                            hv_update_def_row = hv_update_def_row.TupleConcat(hv__update_def_row);
                            hv_update_def_col = hv_update_def_col.TupleConcat(hv__update_def_col);
                        }
                        if ((Int32)(new HTuple((new HTuple(hv_found_row.TupleLength())).TupleNotEqual(
                            0))) != 0)
                        {
                            hv_iFlag = 0;
                        }
                    }
                }

                ho_complement_region.Dispose();
                ho_paint_image.Dispose();
                ho_reduced_image.Dispose();
                ho_affine_show_contour.Dispose();
                ho_affine_show_region.Dispose();
                ho_affine_cont.Dispose();

                return;
            }
            catch (HalconException)
            {
                hv_update_def_row = new HTuple();
                hv_update_def_col = new HTuple();
                hv_found_row = new HTuple();
                hv_found_col = new HTuple();
                hv_found_angle = new HTuple();
                hv_iFlag = -3;

                ho_complement_region.Dispose();
                ho_paint_image.Dispose();
                ho_reduced_image.Dispose();
                ho_affine_show_contour.Dispose();
                ho_affine_show_region.Dispose();
                ho_affine_cont.Dispose();

                //throw HDevExpDefaultException;
            }
        }
        /// <summary>
        /// 模板匹配通用函数，仅匹配
        /// </summary>
        /// <param name="ho_image"></param>
        /// <param name="ho_roi"></param>
        /// <param name="ho_show_contour"></param>
        /// <param name="ho_update_show_contour"></param>
        /// <param name="hv_model_type"></param>
        /// <param name="hv_model_id"></param>
        /// <param name="hv_angle_start"></param>
        /// <param name="hv_angle_extent"></param>
        /// <param name="hv_score_thresh"></param>
        /// <param name="hv_match_num"></param>
        /// <param name="hv_def_row"></param>
        /// <param name="hv_def_col"></param>
        /// <param name="hv_found_row"></param>
        /// <param name="hv_found_col"></param>
        /// <param name="hv_found_angle"></param>
        /// <param name="hv_found_score"></param>
        /// <param name="hv_update_def_row"></param>
        /// <param name="hv_update_def_col"></param>
        /// <param name="hv_model_H_new"></param>
        /// <param name="hv_iFlag"></param>
        public static void find_model(HObject ho_image, HObject ho_roi, HObject ho_show_contour,
                                    out HObject ho_update_show_contour, HTuple hv_model_type, HTuple hv_model_id,
                                    HTuple hv_angle_start, HTuple hv_angle_extent, HTuple hv_score_thresh, HTuple hv_match_num,
                                    HTuple hv_def_row, HTuple hv_def_col, out HTuple hv_found_row, out HTuple hv_found_col,
                                    out HTuple hv_found_angle, out HTuple hv_found_score, out HTuple hv_update_def_row,
                                    out HTuple hv_update_def_col, out HTuple hv_model_H_new, out HTuple hv_iFlag)
        {



            // Stack for temporary objects 
            HObject[] OTemp = new HObject[20];

            // Local iconic variables 

            HObject ho_complement_region, ho_paint_image;
            HObject ho_reduced_image, ho_affine_show_contour = null;


            // Local control variables 

            HTuple hv_inv_model_row = new HTuple(), hv_inv_model_col = new HTuple();
            HTuple hv_ncc_numlevs = new HTuple(), hv_angle_step = new HTuple();
            HTuple hv_ncc_metric = new HTuple(), hv_local_row = new HTuple();
            HTuple hv_local_col = new HTuple(), hv_local_angle = new HTuple();
            HTuple hv_local_score = new HTuple(), hv_shape_numlevs = new HTuple();
            HTuple hv_shape_scale_min = new HTuple(), hv_shape_scale_max = new HTuple();
            HTuple hv_shape_scale_step = new HTuple(), hv_shape_metric = new HTuple();
            HTuple hv_shape_min_contrast = new HTuple(), hv_Greatereq = null;
            HTuple hv_GreaterInd = null, hv_model_row = new HTuple();
            HTuple hv_model_col = new HTuple(), hv_def_row_ind = new HTuple();
            HTuple hv_def_col_ind = new HTuple(), hv_i = new HTuple();
            HTuple hv_model_H_any = new HTuple(), hv__update_def_row = new HTuple();
            HTuple hv__update_def_col = new HTuple(), hv_update_model_row = new HTuple();
            HTuple hv_update_model_col = new HTuple();

            HTuple hv_angle_extent_COPY_INP_TMP = hv_angle_extent.Clone();
            HTuple hv_angle_start_COPY_INP_TMP = hv_angle_start.Clone();

            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_update_show_contour);
            HOperatorSet.GenEmptyObj(out ho_complement_region);
            HOperatorSet.GenEmptyObj(out ho_paint_image);
            HOperatorSet.GenEmptyObj(out ho_reduced_image);
            HOperatorSet.GenEmptyObj(out ho_affine_show_contour);

            hv_update_def_row = new HTuple();
            hv_update_def_col = new HTuple();
            hv_found_row = new HTuple();
            hv_found_col = new HTuple();
            hv_found_angle = new HTuple();
            hv_found_score = new HTuple();
            hv_model_H_new = new HTuple();
            hv_iFlag = 0;
            ho_update_show_contour.Dispose();
            HOperatorSet.GenEmptyObj(out ho_update_show_contour);

            HTuple channel;
            HOperatorSet.CountChannels(ho_image, out channel);
            if (channel.I != 1)
            {
                HOperatorSet.Rgb1ToGray(ho_image, out ho_image);
            }

            ho_complement_region.Dispose();
            HOperatorSet.Complement(ho_roi, out ho_complement_region);
            ho_paint_image.Dispose();
            HOperatorSet.PaintRegion(ho_complement_region, ho_image, out ho_paint_image,
                255, "fill");
            ho_reduced_image.Dispose();
            HOperatorSet.ReduceDomain(ho_paint_image, ho_roi, out ho_reduced_image);
            //******NCC/bin Ncc
            if ((Int32)(new HTuple(hv_model_type.TupleEqual(0))) != 0)
            {
                HOperatorSet.GetNccModelOrigin(hv_model_id, out hv_inv_model_row, out hv_inv_model_col);
                if ((Int32)((new HTuple(hv_angle_start_COPY_INP_TMP.TupleEqual(-1))).TupleOr(
                    new HTuple(hv_angle_extent_COPY_INP_TMP.TupleEqual(-1)))) != 0)
                {
                    HOperatorSet.GetNccModelParams(hv_model_id, out hv_ncc_numlevs, out hv_angle_start_COPY_INP_TMP,
                        out hv_angle_extent_COPY_INP_TMP, out hv_angle_step, out hv_ncc_metric);
                }
                else
                {
                    hv_angle_start_COPY_INP_TMP = hv_angle_start_COPY_INP_TMP.TupleRad();
                    hv_angle_extent_COPY_INP_TMP = hv_angle_extent_COPY_INP_TMP.TupleRad();
                }
                HOperatorSet.FindNccModel(ho_reduced_image, hv_model_id, hv_angle_start_COPY_INP_TMP,
                    hv_angle_extent_COPY_INP_TMP, 0.5, hv_match_num, 0.5, "true", 0, out hv_local_row,
                    out hv_local_col, out hv_local_angle, out hv_local_score);
                //******Shape/Shape Xld
            }
            else
            {
                HOperatorSet.GetShapeModelOrigin(hv_model_id, out hv_inv_model_row, out hv_inv_model_col);
                if ((Int32)((new HTuple(hv_angle_start_COPY_INP_TMP.TupleEqual(-1))).TupleOr(
                    new HTuple(hv_angle_extent_COPY_INP_TMP.TupleEqual(-1)))) != 0)
                {
                    HOperatorSet.GetShapeModelParams(hv_model_id, out hv_shape_numlevs, out hv_angle_start_COPY_INP_TMP,
                        out hv_angle_extent_COPY_INP_TMP, out hv_angle_step, out hv_shape_scale_min,
                        out hv_shape_scale_max, out hv_shape_scale_step, out hv_shape_metric,
                        out hv_shape_min_contrast);
                }
                else
                {
                    hv_angle_start_COPY_INP_TMP = hv_angle_start_COPY_INP_TMP.TupleRad();
                    hv_angle_extent_COPY_INP_TMP = hv_angle_extent_COPY_INP_TMP.TupleRad();
                }
                HOperatorSet.FindShapeModel(ho_reduced_image, hv_model_id, hv_angle_start_COPY_INP_TMP,
                    hv_angle_extent_COPY_INP_TMP, 0.5, hv_match_num, 0.5, "least_squares",
                    0, 0.9, out hv_local_row, out hv_local_col, out hv_local_angle, out hv_local_score);
            }

            if ((Int32)(new HTuple((new HTuple(hv_local_score.TupleLength())).TupleLess(1))) != 0)
            {
                hv_iFlag = -2;
                ho_complement_region.Dispose();
                ho_paint_image.Dispose();
                ho_reduced_image.Dispose();
                ho_affine_show_contour.Dispose();

                return;
            }
            HOperatorSet.TupleGreaterEqualElem(hv_local_score, hv_score_thresh, out hv_Greatereq);
            HOperatorSet.TupleFind(hv_Greatereq, 1, out hv_GreaterInd);
            if ((Int32)(new HTuple(hv_GreaterInd.TupleEqual(-1))) != 0)
            {
                hv_iFlag = -2;
                ho_complement_region.Dispose();
                ho_paint_image.Dispose();
                ho_reduced_image.Dispose();
                ho_affine_show_contour.Dispose();

                return;
            }
            else
            {
                //***获取模板中心坐标
                hv_model_row = -hv_inv_model_row;
                hv_model_col = -hv_inv_model_col;
                HOperatorSet.TupleFind(hv_def_row, -1, out hv_def_row_ind);
                HOperatorSet.TupleFind(hv_def_col, -1, out hv_def_col_ind);
                for (hv_i = 0; (Int32)hv_i <= (Int32)((new HTuple(hv_GreaterInd.TupleLength())) - 1); hv_i = (Int32)hv_i + 1)
                {
                    HOperatorSet.VectorAngleToRigid(0, 0, 0, hv_local_row.TupleSelect(hv_GreaterInd.TupleSelect(
                        hv_i)), hv_local_col.TupleSelect(hv_GreaterInd.TupleSelect(hv_i)), hv_local_angle.TupleSelect(
                        hv_GreaterInd.TupleSelect(hv_i)), out hv_model_H_any);
                    ho_affine_show_contour.Dispose();
                    HOperatorSet.AffineTransContourXld(ho_show_contour, out ho_affine_show_contour,
                        hv_model_H_any);

                    if ((Int32)((new HTuple(hv_def_row_ind.TupleEqual(-1))).TupleAnd(new HTuple(hv_def_col_ind.TupleEqual(
                        -1)))) != 0)
                    {
                        HOperatorSet.AffineTransPoint2d(hv_model_H_any, hv_def_row, hv_def_col,
                            out hv__update_def_row, out hv__update_def_col);
                    }
                    else
                    {
                        hv__update_def_row = new HTuple();
                        hv__update_def_col = new HTuple();
                    }
                    HOperatorSet.ConcatObj(ho_update_show_contour, ho_affine_show_contour, out OTemp[0]
                        );
                    ho_update_show_contour.Dispose();
                    ho_update_show_contour = OTemp[0];
                    //*****更新实际的模板坐标
                    HOperatorSet.AffineTransPoint2d(hv_model_H_any, hv_model_row, hv_model_col,
                        out hv_update_model_row, out hv_update_model_col);
                    hv_found_row = hv_found_row.TupleConcat(hv_update_model_row);
                    hv_found_col = hv_found_col.TupleConcat(hv_update_model_col);
                    hv_found_angle = hv_found_angle.TupleConcat(((hv_local_angle.TupleSelect(
                        hv_GreaterInd.TupleSelect(hv_i)))).TupleDeg());
                    hv_found_score = hv_found_score.TupleConcat(hv_local_score.TupleSelect(hv_GreaterInd.TupleSelect(
                        hv_i)));
                    hv_update_def_row = hv_update_def_row.TupleConcat(hv__update_def_row);
                    hv_update_def_col = hv_update_def_col.TupleConcat(hv__update_def_col);
                    hv_model_H_new = hv_model_H_new.TupleConcat(hv_model_H_any);
                }
            }

            ho_complement_region.Dispose();
            ho_paint_image.Dispose();
            ho_reduced_image.Dispose();
            ho_affine_show_contour.Dispose();

            return;
        }
        
        /// <summary>
        /// 0:Ncc;1:Shape
        /// </summary>
        /// <param name="modelId"></param>
        /// <param name="modelType"></param>
        /// <returns></returns>
        public static HTuple CopyModel(HTuple modelId, HTuple modelType)
        {
            HTuple modelID = null;
            switch (modelType.I)
            {
                case 0:
                    modelID = CopyNccModel(modelId);
                    break;
                case 1:
                    modelID = CopyShapeModel(modelId);
                        break;
            }
            return modelID;
        }
        //model类Copy,需要序列化和反序列化的过程
        public static HTuple CopyNccModel(HTuple nccModelId)
        {
            HTuple copyNccModeId = new HTuple();
            HTuple serializedItemHandle = new HTuple();
            HOperatorSet.SerializeNccModel(nccModelId, out serializedItemHandle);
            HOperatorSet.DeserializeNccModel(serializedItemHandle, out copyNccModeId);
            return copyNccModeId;
        }
        public static HTuple CopyShapeModel(HTuple shapeModelId)
        {
            HTuple copyShapeModeId = new HTuple();
            HTuple serializedItemHandle = new HTuple();
            HOperatorSet.SerializeShapeModel(shapeModelId, out serializedItemHandle);
            HOperatorSet.DeserializeShapeModel(serializedItemHandle, out copyShapeModeId);
            return copyShapeModeId;
        }
        public static HTuple CopyNccModels(HTuple nccModelIds)
        {
            HTuple copyNccModels = new HTuple();
            for (Int32 i = 0; i < nccModelIds.Length; i++)
            {
                copyNccModels.Append(CopyNccModel(nccModelIds[i]));
            }
            return copyNccModels;
        }
        public static HTuple CopyShapeModels(HTuple shapeModelIds)
        {
            HTuple copyShapeModels = new HTuple();
            for (Int32 i = 0; i < shapeModelIds.Length; i++)
            {
                copyShapeModels.Append(CopyShapeModel(shapeModelIds[i]));
            }
            return copyShapeModels;
        }
        public static void int_to_bin(HTuple hv_err_code, out HTuple hv_bin_err_code)
        {


            // Local control variables 

            HTuple hv_remainder = new HTuple();

            HTuple hv_err_code_COPY_INP_TMP = hv_err_code.Clone();

            // Initialize local and output iconic variables 

            hv_bin_err_code = new HTuple();

            while ((Int32)(1) != 0)
            {
                HOperatorSet.TupleMod(hv_err_code_COPY_INP_TMP, 2, out hv_remainder);
                hv_bin_err_code = hv_bin_err_code.TupleConcat(hv_remainder);
                if ((Int32)(new HTuple(hv_err_code_COPY_INP_TMP.TupleEqual(1))) != 0)
                {
                    break;
                }
                hv_err_code_COPY_INP_TMP = hv_err_code_COPY_INP_TMP / 2;
            }

            return;
        }
        /// <summary>
        /// 从HTWindowControl窗口获取交互操作ROI
        /// </summary>
        /// <param name="htWindow"></param>
        /// <param name="mode">获取ROI的类型：contour/region</param>
        /// <param name="ROI"></param>
        public static void GenROI(HTHalControl.HTWindowControl htWindow, string mode, ref HObject ROI)
        {
            ROI.Dispose();
            switch (mode)
            {
                case "contour":
                    switch (htWindow.RegionType)
                    {
                        case "Point":
                            HOperatorSet.GenCrossContourXld(out ROI, htWindow.Row, htWindow.Column, 16, 0);
                            break;
                        case "Line":
                            HOperatorSet.GenContourPolygonXld(out ROI, htWindow.Row1.TupleConcat(htWindow.Row2), 
                                                                       htWindow.Column1.TupleConcat(htWindow.Column2));
                            break;
                        case "Rectangle1":
                            HOperatorSet.GenRectangle2ContourXld(out ROI, (htWindow.Row1 + htWindow.Row2) / 2.0,
                                                                         (htWindow.Column1 + htWindow.Column2) / 2.0,
                                                                         0,
                                                                         (htWindow.Column2 - htWindow.Column1) / 2.0,
                                                                         (htWindow.Row2 - htWindow.Row1) / 2.0);
                            break;
                        case "Rectangle2":
                            HOperatorSet.GenRectangle2ContourXld(out ROI, htWindow.Row, htWindow.Column, htWindow.Phi,
                                                                         htWindow.Length1, htWindow.Length2);
                            break;
                        case "Circle":
                            HOperatorSet.GenCircleContourXld(out ROI, htWindow.Row, htWindow.Column, htWindow.Radius, 0, Math.PI * 2,
                                                                     "positive", 1);
                            break;
                        case "Ellipse":
                            HOperatorSet.GenEllipseContourXld(out ROI, htWindow.Row, htWindow.Column, htWindow.Phi, htWindow.Radius1,
                                                                      htWindow.Radius2, 0, Math.PI * 2, "positive", 1);
                            break;
                        case "Region":
                            HOperatorSet.GenContourRegionXld(htWindow.Region, out ROI, "border_holes");
                            break;
                        case "Xld":
                            HOperatorSet.CopyObj(htWindow.Region, out ROI, 1, -1);
                            break;
                        default:
                            HOperatorSet.GenEmptyObj(out ROI);
                            break;
                    }
                    break;
                case "region":
                    switch (htWindow.RegionType)
                    {
                        case "Point":
                            HOperatorSet.GenRegionPoints(out ROI, htWindow.Row, htWindow.Column);
                            break;
                        case "Line":
                            HOperatorSet.GenRegionLine(out ROI, htWindow.Row1, htWindow.Column1, htWindow.Row2, htWindow.Column2);
                            break;
                        case "Rectangle1":
                            HOperatorSet.GenRectangle1(out ROI, htWindow.Row1, htWindow.Column1, htWindow.Row2, htWindow.Column2);
                            break;
                        case "Rectangle2":
                            HOperatorSet.GenRectangle2(out ROI, htWindow.Row, htWindow.Column, htWindow.Phi, htWindow.Length1, htWindow.Length2);
                            break;
                        case "Circle":
                            HOperatorSet.GenCircle(out ROI, htWindow.Row, htWindow.Column, htWindow.Radius);
                            break;
                        case "Ellipse":
                            HOperatorSet.GenEllipse(out ROI, htWindow.Row, htWindow.Column, 0, htWindow.Radius1, htWindow.Radius2);
                            break;
                        case "Region":
                            HOperatorSet.CopyObj(htWindow.Region, out ROI, 1, -1);
                            break;
                        case "Xld":
                            HOperatorSet.GenRegionContourXld(htWindow.Region, out ROI, "filled");
                            break;
                        default:
                            HOperatorSet.GenEmptyObj(out ROI);
                            break;
                    }
                    break;
                default:
                    HOperatorSet.GenEmptyObj(out ROI);
                    break;
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////非静态方法

        /**********相机标定**********/
        public bool FindCalibObject(HObject image, HTuple calibID, HTuple camIndex, HTuple poseIndex)
        {
            try
            {
                HOperatorSet.FindCalibObject(image, calibID, camIndex, 0, poseIndex, new HTuple(), new HTuple());
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public void VisualizeCalibResults(HObject image, HTuple calibID, HTuple camIndex, HTuple poseIndex, HTHalControl.HTWindowControl htWindow, ref HObject disp_conts)
        {
            HTuple mark_rows, mark_cols, arrow_rows, arrow_cols;
            HObject caltab = new HObject(), marks = new HObject(), center_cross = new HObject(), arrow_x = new HObject(), arrow_y = new HObject();
            HObject conts1 = new HObject(), conts2 = new HObject(), conts3 = new HObject();

            HOperatorSet.GenEmptyObj(out caltab);
            HOperatorSet.GenEmptyObj(out marks);
            HOperatorSet.GenEmptyObj(out center_cross);
            HOperatorSet.GenEmptyObj(out arrow_x);
            HOperatorSet.GenEmptyObj(out arrow_y);
            HOperatorSet.GenEmptyObj(out conts1);
            HOperatorSet.GenEmptyObj(out conts2);
            HOperatorSet.GenEmptyObj(out conts3);

            caltab.Dispose();
            marks.Dispose();
            get_calib_results(image, out caltab, out marks, calibID, camIndex, poseIndex, out mark_rows, out mark_cols,
                              out arrow_rows, out arrow_cols);
            center_cross.Dispose();
            DrawCross(mark_rows, mark_cols, 6, ref center_cross);
            arrow_x.Dispose();
            gen_arrow_contour_xld(out arrow_x, arrow_rows[0], arrow_cols[0], arrow_rows[1], arrow_cols[1], 5, 5);
            arrow_y.Dispose();
            gen_arrow_contour_xld(out arrow_y, arrow_rows[0], arrow_cols[0], arrow_rows[2], arrow_cols[2], 5, 5);

            conts1.Dispose();
            HOperatorSet.ConcatObj(caltab, marks, out conts1);
            conts2.Dispose();
            HOperatorSet.ConcatObj(arrow_x, arrow_y, out conts2);
            conts3.Dispose();
            HOperatorSet.ConcatObj(conts1, conts2, out conts3);
            disp_conts.Dispose();
            HOperatorSet.ConcatObj(conts3, center_cross, out disp_conts);
            htWindow.RefreshWindow(image, disp_conts, "");
            HOperatorSet.SetTposition(htWindow.HTWindow.HalconWindow, arrow_rows[1], arrow_cols[1]);
            HOperatorSet.WriteString(htWindow.HTWindow.HalconWindow, "x");
            HOperatorSet.SetTposition(htWindow.HTWindow.HalconWindow, arrow_rows[2], arrow_cols[2]);
            HOperatorSet.WriteString(htWindow.HTWindow.HalconWindow, "y");

            caltab.Dispose();
            marks.Dispose();
            center_cross.Dispose();
            arrow_x.Dispose();
            arrow_y.Dispose();
            conts1.Dispose();
            conts2.Dispose();
            conts3.Dispose();
        }
        public void get_calib_precision(HTuple hv_CalibDataID, HTuple hv_CameraIdx, HTuple hv_PoseIdx,
                                        out HTuple hv_PixelPrecision)
        {


            // Local iconic variables 

            //HObject ho_Cross;


            // Local control variables 

            HTuple hv_X = null, hv_Y = null, hv_RCoord = null;
            HTuple hv_CCoord = null, hv_Index = null, hv_Pose = null;
            HTuple hv_real_distance = null, hv_pix_distance = null;

            // Initialize local and output iconic variables 
            //HOperatorSet.GenEmptyObj(out ho_Cross);

            hv_PixelPrecision = new HTuple();
            HOperatorSet.GetCalibData(hv_CalibDataID, "calib_obj", 0, "x", out hv_X);
            HOperatorSet.GetCalibData(hv_CalibDataID, "calib_obj", 0, "y", out hv_Y);
            HOperatorSet.GetCalibDataObservPoints(hv_CalibDataID, hv_CameraIdx, 0, hv_PoseIdx,
                out hv_RCoord, out hv_CCoord, out hv_Index, out hv_Pose);
            HOperatorSet.DistancePp(hv_X.TupleSelect(0), hv_Y.TupleSelect(0), hv_X.TupleSelect(
                6), hv_Y.TupleSelect(6), out hv_real_distance);
            HOperatorSet.DistancePp(hv_RCoord.TupleSelect(0), hv_CCoord.TupleSelect(0), hv_RCoord.TupleSelect(
                6), hv_CCoord.TupleSelect(6), out hv_pix_distance);
            //ho_Cross.Dispose();
            //HOperatorSet.GenCrossContourXld(out ho_Cross, ((hv_RCoord.TupleSelect(0))).TupleConcat(
            //hv_RCoord.TupleSelect(6)), ((hv_CCoord.TupleSelect(0))).TupleConcat(hv_CCoord.TupleSelect(
            //6)), 6, 0.785398);
            hv_PixelPrecision = hv_real_distance / hv_pix_distance;
            //ho_Cross.Dispose();

            return;
        }
        public void gen_arrow_contour_xld(out HObject ho_Arrow, HTuple hv_Row1, HTuple hv_Column1,
                                          HTuple hv_Row2, HTuple hv_Column2, HTuple hv_HeadLength, HTuple hv_HeadWidth)
        {


            // Stack for temporary objects 
            HObject[] OTemp = new HObject[20];

            // Local iconic variables 

            HObject ho_TempArrow = null;


            // Local control variables 

            HTuple hv_Length = null, hv_ZeroLengthIndices = null;
            HTuple hv_DR = null, hv_DC = null, hv_HalfHeadWidth = null;
            HTuple hv_RowP1 = null, hv_ColP1 = null, hv_RowP2 = null;
            HTuple hv_ColP2 = null, hv_Index = null;

            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_Arrow);
            HOperatorSet.GenEmptyObj(out ho_TempArrow);

            //This procedure generates arrow shaped XLD contours,
            //pointing from (Row1, Column1) to (Row2, Column2).
            //If starting and end point are identical, a contour consisting
            //of a single point is returned.
            //
            //input parameteres:
            //Row1, Column1: Coordinates of the arrows' starting points
            //Row2, Column2: Coordinates of the arrows' end points
            //HeadLength, HeadWidth: Size of the arrow heads in pixels
            //
            //output parameter:
            //Arrow: The resulting XLD contour
            //
            //The input tuples Row1, Column1, Row2, and Column2 have to be of
            //the same length.
            //HeadLength and HeadWidth either have to be of the same length as
            //Row1, Column1, Row2, and Column2 or have to be a single element.
            //If one of the above restrictions is violated, an error will occur.
            //
            //
            //Init
            ho_Arrow.Dispose();
            HOperatorSet.GenEmptyObj(out ho_Arrow);
            //
            //Calculate the arrow length
            HOperatorSet.DistancePp(hv_Row1, hv_Column1, hv_Row2, hv_Column2, out hv_Length);
            //
            //Mark arrows with identical start and end point
            //(set Length to -1 to avoid division-by-zero exception)
            hv_ZeroLengthIndices = hv_Length.TupleFind(0);
            if ((Int32)(new HTuple(hv_ZeroLengthIndices.TupleNotEqual(-1))) != 0)
            {
                if (hv_Length == null)
                    hv_Length = new HTuple();
                hv_Length[hv_ZeroLengthIndices] = -1;
            }
            //
            //Calculate auxiliary variables.
            hv_DR = (1.0 * (hv_Row2 - hv_Row1)) / hv_Length;
            hv_DC = (1.0 * (hv_Column2 - hv_Column1)) / hv_Length;
            hv_HalfHeadWidth = hv_HeadWidth / 2.0;
            //
            //Calculate end points of the arrow head.
            hv_RowP1 = (hv_Row1 + ((hv_Length - hv_HeadLength) * hv_DR)) + (hv_HalfHeadWidth * hv_DC);
            hv_ColP1 = (hv_Column1 + ((hv_Length - hv_HeadLength) * hv_DC)) - (hv_HalfHeadWidth * hv_DR);
            hv_RowP2 = (hv_Row1 + ((hv_Length - hv_HeadLength) * hv_DR)) - (hv_HalfHeadWidth * hv_DC);
            hv_ColP2 = (hv_Column1 + ((hv_Length - hv_HeadLength) * hv_DC)) + (hv_HalfHeadWidth * hv_DR);
            //
            //Finally create output XLD contour for each input point pair
            for (hv_Index = 0; (Int32)hv_Index <= (Int32)((new HTuple(hv_Length.TupleLength())) - 1); hv_Index = (Int32)hv_Index + 1)
            {
                if ((Int32)(new HTuple(((hv_Length.TupleSelect(hv_Index))).TupleEqual(-1))) != 0)
                {
                    //Create_ single points for arrows with identical start and end point
                    ho_TempArrow.Dispose();
                    HOperatorSet.GenContourPolygonXld(out ho_TempArrow, hv_Row1.TupleSelect(hv_Index),
                        hv_Column1.TupleSelect(hv_Index));
                }
                else
                {
                    //Create arrow contour
                    ho_TempArrow.Dispose();
                    HOperatorSet.GenContourPolygonXld(out ho_TempArrow, ((((((((((hv_Row1.TupleSelect(
                        hv_Index))).TupleConcat(hv_Row2.TupleSelect(hv_Index)))).TupleConcat(
                        hv_RowP1.TupleSelect(hv_Index)))).TupleConcat(hv_Row2.TupleSelect(hv_Index)))).TupleConcat(
                        hv_RowP2.TupleSelect(hv_Index)))).TupleConcat(hv_Row2.TupleSelect(hv_Index)),
                        ((((((((((hv_Column1.TupleSelect(hv_Index))).TupleConcat(hv_Column2.TupleSelect(
                        hv_Index)))).TupleConcat(hv_ColP1.TupleSelect(hv_Index)))).TupleConcat(
                        hv_Column2.TupleSelect(hv_Index)))).TupleConcat(hv_ColP2.TupleSelect(
                        hv_Index)))).TupleConcat(hv_Column2.TupleSelect(hv_Index)));
                }
                HOperatorSet.ConcatObj(ho_Arrow, ho_TempArrow, out OTemp[0]);
                ho_Arrow.Dispose();
                ho_Arrow = OTemp[0];
            }
            ho_TempArrow.Dispose();

            return;
        }
        public void visualize_calib_results(HObject ho_Image, HTuple hv_CalibDataID, HTuple hv_CameraIdx,
                                            HTuple hv_PoseIdx, HTuple hv_WindowHandle)
        {



            // Local iconic variables 

            HObject ho_CalTab, ho_Marks, ho_Cross, ho_ArrowX;
            HObject ho_ArrowY;


            // Local control variables 

            HTuple hv_RCoord = null, hv_CCoord = null;
            HTuple hv_Index = null, hv_Pose = null, hv_CamPar = null;
            HTuple hv_X = null, hv_ArrowLength = null, hv_ArrowX_CPCS = null;
            HTuple hv_ArrowY_CPCS = null, hv_ArrowZ_CPCS = null, hv_HomMat_CPCS_CCS = null;
            HTuple hv_ArrowX_CCS = null, hv_ArrowY_CCS = null, hv_ArrowZ_CCS = null;
            HTuple hv_ArrowRow = null, hv_ArrowColumn = null;

            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_CalTab);
            HOperatorSet.GenEmptyObj(out ho_Marks);
            HOperatorSet.GenEmptyObj(out ho_Cross);
            HOperatorSet.GenEmptyObj(out ho_ArrowX);
            HOperatorSet.GenEmptyObj(out ho_ArrowY);

            HOperatorSet.GetCalibDataObservPoints(hv_CalibDataID, hv_CameraIdx, 0, hv_PoseIdx,
                out hv_RCoord, out hv_CCoord, out hv_Index, out hv_Pose);
            HOperatorSet.GetCalibData(hv_CalibDataID, "camera", hv_CameraIdx, "init_params",
                out hv_CamPar);
            HOperatorSet.GetCalibData(hv_CalibDataID, "calib_obj", 0, "x", out hv_X);
            ho_CalTab.Dispose();
            HOperatorSet.GetCalibDataObservContours(out ho_CalTab, hv_CalibDataID, "caltab",
                hv_CameraIdx, 0, hv_PoseIdx);
            ho_Marks.Dispose();
            HOperatorSet.GetCalibDataObservContours(out ho_Marks, hv_CalibDataID, "marks",
                hv_CameraIdx, 0, hv_PoseIdx);
            //
            //dev_set_window(...);
            HOperatorSet.DispObj(ho_Image, hv_WindowHandle);
            HOperatorSet.SetColor(hv_WindowHandle, "yellow");
            ho_Cross.Dispose();
            HOperatorSet.GenCrossContourXld(out ho_Cross, hv_RCoord, hv_CCoord, 6, 0.785398);
            HOperatorSet.DispObj(ho_Cross, hv_WindowHandle);
            hv_ArrowLength = ((hv_X.TupleSelect(6)) - (hv_X.TupleSelect(0))) * 0.8;
            hv_ArrowX_CPCS = new HTuple();
            hv_ArrowX_CPCS[0] = 0;
            hv_ArrowX_CPCS = hv_ArrowX_CPCS.TupleConcat(hv_ArrowLength);
            hv_ArrowX_CPCS = hv_ArrowX_CPCS.TupleConcat(0);
            hv_ArrowY_CPCS = new HTuple();
            hv_ArrowY_CPCS[0] = 0;
            hv_ArrowY_CPCS[1] = 0;
            hv_ArrowY_CPCS = hv_ArrowY_CPCS.TupleConcat(hv_ArrowLength);
            hv_ArrowZ_CPCS = new HTuple();
            hv_ArrowZ_CPCS[0] = 0;
            hv_ArrowZ_CPCS[1] = 0;
            hv_ArrowZ_CPCS[2] = 0;
            HOperatorSet.PoseToHomMat3d(hv_Pose, out hv_HomMat_CPCS_CCS);
            HOperatorSet.AffineTransPoint3d(hv_HomMat_CPCS_CCS, hv_ArrowX_CPCS, hv_ArrowY_CPCS,
                hv_ArrowZ_CPCS, out hv_ArrowX_CCS, out hv_ArrowY_CCS, out hv_ArrowZ_CCS);
            HOperatorSet.Project3dPoint(hv_ArrowX_CCS, hv_ArrowY_CCS, hv_ArrowZ_CCS, hv_CamPar,
                out hv_ArrowRow, out hv_ArrowColumn);
            HOperatorSet.SetColor(hv_WindowHandle, "green");
            ho_ArrowX.Dispose();
            gen_arrow_contour_xld(out ho_ArrowX, hv_ArrowRow.TupleSelect(0), hv_ArrowColumn.TupleSelect(
                0), hv_ArrowRow.TupleSelect(1), hv_ArrowColumn.TupleSelect(1), 5, 5);
            ho_ArrowY.Dispose();
            gen_arrow_contour_xld(out ho_ArrowY, hv_ArrowRow.TupleSelect(0), hv_ArrowColumn.TupleSelect(
                0), hv_ArrowRow.TupleSelect(2), hv_ArrowColumn.TupleSelect(2), 5, 5);
            HOperatorSet.DispObj(ho_ArrowX, hv_WindowHandle);
            HOperatorSet.DispObj(ho_ArrowY, hv_WindowHandle);
            HOperatorSet.DispObj(ho_CalTab, hv_WindowHandle);
            HOperatorSet.DispObj(ho_Marks, hv_WindowHandle);
            HOperatorSet.SetTposition(hv_WindowHandle, hv_ArrowRow.TupleSelect(1),
                hv_ArrowColumn.TupleSelect(1));
            HOperatorSet.WriteString(hv_WindowHandle, "x");
            HOperatorSet.SetTposition(hv_WindowHandle, hv_ArrowRow.TupleSelect(2),
                hv_ArrowColumn.TupleSelect(2));
            HOperatorSet.WriteString(hv_WindowHandle, "y");
            //dev_set_color ('white')
            ho_CalTab.Dispose();
            ho_Marks.Dispose();
            ho_Cross.Dispose();
            ho_ArrowX.Dispose();
            ho_ArrowY.Dispose();

            return;
        }
        public void get_calib_results(HObject ho_Image, out HObject ho_Caltab, out HObject ho_Marks,
                                    HTuple hv_CalibDataID, HTuple hv_CameraIdx, HTuple hv_PoseIdx, out HTuple hv_Mark_Rows,
                                    out HTuple hv_Mark_Cols, out HTuple hv_Arrow_Rows, out HTuple hv_Arrow_Cols)
        {



            // Local iconic variables 

            HObject ho_ArrowX, ho_ArrowY;


            // Local control variables 

            HTuple hv_RCoord = null, hv_CCoord = null;
            HTuple hv_Index = null, hv_Pose = null, hv_CamPar = null;
            HTuple hv_X = null, hv_ArrowLength = null, hv_ArrowX_CPCS = null;
            HTuple hv_ArrowY_CPCS = null, hv_ArrowZ_CPCS = null, hv_HomMat_CPCS_CCS = null;
            HTuple hv_ArrowX_CCS = null, hv_ArrowY_CCS = null, hv_ArrowZ_CCS = null;
            HTuple hv_ArrowRow = null, hv_ArrowColumn = null;

            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_Caltab);
            HOperatorSet.GenEmptyObj(out ho_Marks);
            HOperatorSet.GenEmptyObj(out ho_ArrowX);
            HOperatorSet.GenEmptyObj(out ho_ArrowY);

            try
            {
                HOperatorSet.GetCalibDataObservPoints(hv_CalibDataID, hv_CameraIdx, 0, hv_PoseIdx,
                    out hv_RCoord, out hv_CCoord, out hv_Index, out hv_Pose);
                HOperatorSet.GetCalibData(hv_CalibDataID, "camera", hv_CameraIdx, "init_params",
                    out hv_CamPar);
                HOperatorSet.GetCalibData(hv_CalibDataID, "calib_obj", 0, "x", out hv_X);
                ho_Caltab.Dispose();
                HOperatorSet.GetCalibDataObservContours(out ho_Caltab, hv_CalibDataID, "caltab",
                    hv_CameraIdx, 0, hv_PoseIdx);
                ho_Marks.Dispose();
                HOperatorSet.GetCalibDataObservContours(out ho_Marks, hv_CalibDataID, "marks",
                    hv_CameraIdx, 0, hv_PoseIdx);
                //
                //dev_set_window (WindowHandle)
                //dev_display (Image)
                //dev_set_color ('yellow')
                //gen_cross_contour_xld (Cross, RCoord, CCoord, 6, 0.785398)
                //dev_display (Cross)
                hv_ArrowLength = ((hv_X.TupleSelect(6)) - (hv_X.TupleSelect(0))) * 0.8;
                hv_ArrowX_CPCS = new HTuple();
                hv_ArrowX_CPCS[0] = 0;
                hv_ArrowX_CPCS = hv_ArrowX_CPCS.TupleConcat(hv_ArrowLength);
                hv_ArrowX_CPCS = hv_ArrowX_CPCS.TupleConcat(0);
                hv_ArrowY_CPCS = new HTuple();
                hv_ArrowY_CPCS[0] = 0;
                hv_ArrowY_CPCS[1] = 0;
                hv_ArrowY_CPCS = hv_ArrowY_CPCS.TupleConcat(hv_ArrowLength);
                hv_ArrowZ_CPCS = new HTuple();
                hv_ArrowZ_CPCS[0] = 0;
                hv_ArrowZ_CPCS[1] = 0;
                hv_ArrowZ_CPCS[2] = 0;
                HOperatorSet.PoseToHomMat3d(hv_Pose, out hv_HomMat_CPCS_CCS);
                HOperatorSet.AffineTransPoint3d(hv_HomMat_CPCS_CCS, hv_ArrowX_CPCS, hv_ArrowY_CPCS,
                    hv_ArrowZ_CPCS, out hv_ArrowX_CCS, out hv_ArrowY_CCS, out hv_ArrowZ_CCS);
                HOperatorSet.Project3dPoint(hv_ArrowX_CCS, hv_ArrowY_CCS, hv_ArrowZ_CCS, hv_CamPar,
                    out hv_ArrowRow, out hv_ArrowColumn);
                //dev_set_color ('green')
                ho_ArrowX.Dispose();
                gen_arrow_contour_xld(out ho_ArrowX, hv_ArrowRow.TupleSelect(0), hv_ArrowColumn.TupleSelect(
                    0), hv_ArrowRow.TupleSelect(1), hv_ArrowColumn.TupleSelect(1), 5, 5);
                ho_ArrowY.Dispose();
                gen_arrow_contour_xld(out ho_ArrowY, hv_ArrowRow.TupleSelect(0), hv_ArrowColumn.TupleSelect(
                    0), hv_ArrowRow.TupleSelect(2), hv_ArrowColumn.TupleSelect(2), 5, 5);
                //dev_display (ArrowX)
                //dev_display (ArrowY)
                //dev_display (CalTab)
                //dev_display (Marks)
                //set_tposition (WindowHandle, ArrowRow[1], ArrowColumn[1])
                //write_string (WindowHandle, 'x')
                //set_tposition (WindowHandle, ArrowRow[2], ArrowColumn[2])
                //write_string (WindowHandle, 'y')
                hv_Mark_Rows = hv_RCoord.Clone();
                hv_Mark_Cols = hv_CCoord.Clone();
                hv_Arrow_Rows = hv_ArrowRow.Clone();
                hv_Arrow_Cols = hv_ArrowColumn.Clone();
                ho_ArrowX.Dispose();
                ho_ArrowY.Dispose();

                return;
            }
            catch (HalconException HDevExpDefaultException)
            {
                ho_ArrowX.Dispose();
                ho_ArrowY.Dispose();

                throw HDevExpDefaultException;
            }
        }
        /***************************/

        public void read_hom2d(HTuple hv_filePath, out HTuple hv_HomMat2D)
        {


            // Local control variables 

            HTuple hv_FileHandle1 = null, hv_SerializedItemHandle1 = null;

            // Initialize local and output iconic variables 

            //dev_set_check ('~give_error')
            HOperatorSet.OpenFile(hv_filePath, "input_binary", out hv_FileHandle1);
            HOperatorSet.FreadSerializedItem(hv_FileHandle1, out hv_SerializedItemHandle1);
            HOperatorSet.CloseFile(hv_FileHandle1);
            HOperatorSet.DeserializeHomMat2d(hv_SerializedItemHandle1, out hv_HomMat2D);

            return;
        }
        public void save_hom2d(HTuple hv_filePath, HTuple hv_model_H_bga2D)
        {


            // Local control variables 

            HTuple hv_FileHandle = null, hv_SerializedItemHandle = null;

            // Initialize local and output iconic variables 

            //dev_set_check ('~give_error')
            HOperatorSet.OpenFile(hv_filePath, "output_binary", out hv_FileHandle);
            HOperatorSet.SerializeHomMat2d(hv_model_H_bga2D, out hv_SerializedItemHandle);
            HOperatorSet.FwriteSerializedItem(hv_FileHandle, hv_SerializedItemHandle);
            HOperatorSet.CloseFile(hv_FileHandle);

            return;
        }
        public void get_contour_corners(HObject ho_Rectangle, out HTuple hv_corner_rows,
                                        out HTuple hv_corner_cols)
        {


            // Local control variables 

            HTuple hv_Row3 = null, hv_Col = null, hv_RowCenter = null;
            HTuple hv_ColCenter = null, hv_Index = null;

            // Initialize local and output iconic variables 

            HOperatorSet.GetContourXld(ho_Rectangle, out hv_Row3, out hv_Col);
            hv_corner_rows = new HTuple();
            hv_corner_cols = new HTuple();
            HOperatorSet.TupleMean(hv_Row3, out hv_RowCenter);
            HOperatorSet.TupleMean(hv_Col, out hv_ColCenter);
            for (hv_Index = 0; (Int32)hv_Index <= 3; hv_Index = (Int32)hv_Index + 1)
            {
                if ((Int32)(new HTuple(((hv_Row3.TupleSelect(hv_Index))).TupleLess(hv_RowCenter))) != 0)
                {
                    if ((Int32)(new HTuple(((hv_Col.TupleSelect(hv_Index))).TupleLess(hv_ColCenter))) != 0)
                    {
                        if (hv_corner_rows == null)
                            hv_corner_rows = new HTuple();
                        hv_corner_rows[0] = hv_Row3.TupleSelect(hv_Index);
                        if (hv_corner_cols == null)
                            hv_corner_cols = new HTuple();
                        hv_corner_cols[0] = hv_Col.TupleSelect(hv_Index);
                    }
                    else
                    {
                        if (hv_corner_rows == null)
                            hv_corner_rows = new HTuple();
                        hv_corner_rows[1] = hv_Row3.TupleSelect(hv_Index);
                        if (hv_corner_cols == null)
                            hv_corner_cols = new HTuple();
                        hv_corner_cols[1] = hv_Col.TupleSelect(hv_Index);
                    }
                }
                else
                {
                    if ((Int32)(new HTuple(((hv_Col.TupleSelect(hv_Index))).TupleGreater(hv_ColCenter))) != 0)
                    {
                        if (hv_corner_rows == null)
                            hv_corner_rows = new HTuple();
                        hv_corner_rows[2] = hv_Row3.TupleSelect(hv_Index);
                        if (hv_corner_cols == null)
                            hv_corner_cols = new HTuple();
                        hv_corner_cols[2] = hv_Col.TupleSelect(hv_Index);
                    }
                    else
                    {
                        if (hv_corner_rows == null)
                            hv_corner_rows = new HTuple();
                        hv_corner_rows[3] = hv_Row3.TupleSelect(hv_Index);
                        if (hv_corner_cols == null)
                            hv_corner_cols = new HTuple();
                        hv_corner_cols[3] = hv_Col.TupleSelect(hv_Index);
                    }
                }
            }

            return;
        }
        public void projective_trans_point_2d_sub(HTuple hv_Px, HTuple hv_Py, HTuple hv_Homat2D,
                                                    out HTuple hv_Qx, out HTuple hv_Qy)
        {


            // Local control variables 

            HTuple hv_ones = null, hv_Qx1 = null, hv_Qy1 = null;
            HTuple hv_Qw1 = null;

            // Initialize local and output iconic variables 

            //dev_set_check ('~give_error')
            hv_Qx = new HTuple();
            hv_Qy = new HTuple();
            HOperatorSet.TupleGenConst(new HTuple(hv_Px.TupleLength()), 1, out hv_ones);
            HOperatorSet.ProjectiveTransPoint2d(hv_Homat2D, hv_Px, hv_Py, hv_ones, out hv_Qx1,
                out hv_Qy1, out hv_Qw1);
            HOperatorSet.TupleDiv(hv_Qx1, hv_Qw1, out hv_Qx);
            HOperatorSet.TupleDiv(hv_Qy1, hv_Qw1, out hv_Qy);

            return;
        }
        public void vector_to_proj_hom_mat2dNew(HTuple hv_rowsL, HTuple hv_colsL, HTuple hv_rowsR,
                                                HTuple hv_colsR, out HTuple hv_L_H_R)
        {


            // Local control variables 

            HTuple hv_Covariance = null;

            // Initialize local and output iconic variables 

            //dev_set_check ('~give_error')
            hv_L_H_R = new HTuple();
            HOperatorSet.VectorToProjHomMat2d(hv_rowsL, hv_colsL, hv_rowsR, hv_colsR, "gold_standard",
                new HTuple(), new HTuple(), new HTuple(), new HTuple(), new HTuple(), new HTuple(),
                out hv_L_H_R, out hv_Covariance);

            return;
        }
        public void fit_lines_metrology1(HObject ho_Image, HTuple hv_line_rows1, HTuple hv_line_cols1,
                                        HTuple hv_line_rows2, HTuple hv_line_cols2, HTuple hv_edge_threshold, HTuple hv_edge_transition,
                                        HTuple hv_edge_select, HTuple hv_rect_num, HTuple hv_rect_length1, HTuple hv_rect_length2,
                                        HTuple hv_sigma, HTuple hv_score, out HTuple hv_begin_rows, out HTuple hv_begin_cols,
                                        out HTuple hv_end_rows, out HTuple hv_end_cols, out HTuple hv_scores)
        {



            // Local iconic variables 

            HObject ho_line = null, ho_Contour = null, ho_Cross = null;
            HObject ho_Contour1 = null, ho_Contour2 = null;


            // Local control variables 

            HTuple hv_MetrologyIndices = null, hv_Width = null;
            HTuple hv_Height = null, hv_MetrologyHandle = null, hv_i = null;
            HTuple hv_Index = new HTuple(), hv_Row = new HTuple();
            HTuple hv_Column = new HTuple(), hv_Score = new HTuple();
            HTuple hv_RowBegin = new HTuple(), hv_ColBegin = new HTuple();
            HTuple hv_RowEnd = new HTuple(), hv_ColEnd = new HTuple();
            HTuple hv_Nr = new HTuple(), hv_Nc = new HTuple(), hv_Dist = new HTuple();

            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_line);
            HOperatorSet.GenEmptyObj(out ho_Contour);
            HOperatorSet.GenEmptyObj(out ho_Cross);
            HOperatorSet.GenEmptyObj(out ho_Contour1);
            HOperatorSet.GenEmptyObj(out ho_Contour2);

            //***********函数内部
            hv_MetrologyIndices = new HTuple();
            hv_begin_rows = new HTuple();
            hv_begin_cols = new HTuple();
            hv_end_rows = new HTuple();
            hv_end_cols = new HTuple();
            hv_scores = new HTuple();
            HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height);
            HOperatorSet.CreateMetrologyModel(out hv_MetrologyHandle);
            HOperatorSet.SetMetrologyModelImageSize(hv_MetrologyHandle, hv_Width, hv_Height);
            for (hv_i = 0; (Int32)hv_i <= (Int32)((new HTuple(hv_line_rows1.TupleLength())) - 1); hv_i = (Int32)hv_i + 1)
            {
                //ho_line.Dispose();
                //HOperatorSet.GenContourPolygonXld(out ho_line, ((hv_line_rows1.TupleSelect(
                //    hv_i))).TupleConcat(hv_line_rows2.TupleSelect(hv_i)), ((hv_line_cols1.TupleSelect(
                //    hv_i))).TupleConcat(hv_line_cols2.TupleSelect(hv_i)));
                HOperatorSet.AddMetrologyObjectLineMeasure(hv_MetrologyHandle, hv_line_rows1.TupleSelect(
                    hv_i), hv_line_cols1.TupleSelect(hv_i), hv_line_rows2.TupleSelect(hv_i),
                    hv_line_cols2.TupleSelect(hv_i), hv_rect_length1.TupleSelect(hv_i), hv_rect_length2.TupleSelect(
                    hv_i), hv_sigma.TupleSelect(hv_i), hv_edge_threshold.TupleSelect(hv_i),
                    new HTuple(), new HTuple(), out hv_Index);
                if ((Int32)(new HTuple(((hv_rect_num.TupleSelect(hv_i))).TupleNotEqual(-1))) != 0)
                {
                    HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandle, hv_Index, "num_measures",
                        hv_rect_num.TupleSelect(hv_i));
                }
                HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandle, hv_Index, "measure_select",
                    hv_edge_select.TupleSelect(hv_i));
                //set_metrology_object_param (MetrologyHandle, Index, 'measure_threshold', threshold)
                HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandle, hv_Index, "measure_transition",
                    hv_edge_transition.TupleSelect(hv_i));
                //set_metrology_object_param (MetrologyHandle, Index, 'measure_length1', length1)
                //set_metrology_object_param (MetrologyHandle, Index, 'measure_length2', length2)
                HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandle, hv_Index, "measure_sigma",
                    hv_sigma.TupleSelect(hv_i));
                //set_metrology_object_param (MetrologyHandle, Index, 'num_instances', instances)
                HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandle, hv_Index, "min_score",
                    hv_score.TupleSelect(hv_i));
                //set_metrology_object_param (MetrologyHandle, Index, 'measure_distance', distance)
                hv_MetrologyIndices = hv_MetrologyIndices.TupleConcat(hv_Index);
            }

            //将设置的Handle应用与指定图像
            HOperatorSet.ApplyMetrologyModel(ho_Image, hv_MetrologyHandle);
            for (hv_i = 0; (Int32)hv_i <= (Int32)((new HTuple(hv_line_rows1.TupleLength())) - 1); hv_i = (Int32)hv_i + 1)
            {
                ho_Contour.Dispose();
                HOperatorSet.GetMetrologyObjectMeasures(out ho_Contour, hv_MetrologyHandle,
                    hv_MetrologyIndices.TupleSelect(hv_i), hv_edge_transition.TupleSelect(hv_i),
                    out hv_Row, out hv_Column);
                HOperatorSet.GetMetrologyObjectResult(hv_MetrologyHandle, hv_MetrologyIndices.TupleSelect(
                    hv_i), "all", "result_type", "score", out hv_Score);
                if ((Int32)(new HTuple((new HTuple(hv_Score.TupleLength())).TupleGreater(0))) != 0)
                {
                    //ho_Cross.Dispose();
                    //HOperatorSet.GenCrossContourXld(out ho_Cross, hv_Row, hv_Column, 4, 0);
                    ho_Contour1.Dispose();
                    HOperatorSet.GenContourPolygonXld(out ho_Contour1, hv_Row, hv_Column);
                    HOperatorSet.FitLineContourXld(ho_Contour1, "tukey", -1, 0, 8, 2, out hv_RowBegin,
                        out hv_ColBegin, out hv_RowEnd, out hv_ColEnd, out hv_Nr, out hv_Nc,
                        out hv_Dist);
                    //ho_Contour2.Dispose();
                    //HOperatorSet.GenContourPolygonXld(out ho_Contour2, hv_RowBegin.TupleConcat(
                    //    hv_RowEnd), hv_ColBegin.TupleConcat(hv_ColEnd));
                    hv_begin_rows = hv_begin_rows.TupleConcat(hv_RowBegin);
                    hv_begin_cols = hv_begin_cols.TupleConcat(hv_ColBegin);
                    hv_end_rows = hv_end_rows.TupleConcat(hv_RowEnd);
                    hv_end_cols = hv_end_cols.TupleConcat(hv_ColEnd);
                    hv_scores = hv_scores.TupleConcat(hv_Score);
                }
                else
                {
                    hv_begin_rows = hv_begin_rows.TupleConcat(hv_line_rows1.TupleSelect(hv_i));
                    hv_begin_cols = hv_begin_cols.TupleConcat(hv_line_cols1.TupleSelect(hv_i));
                    hv_end_rows = hv_end_rows.TupleConcat(hv_line_rows2.TupleSelect(hv_i));
                    hv_end_cols = hv_end_cols.TupleConcat(hv_line_cols2.TupleSelect(hv_i));
                    hv_scores = hv_scores.TupleConcat(-1);
                }
            }

            //将设置的Handle清除，否则会占用内存
            HOperatorSet.ClearMetrologyModel(hv_MetrologyHandle);
            ho_line.Dispose();
            ho_Contour.Dispose();
            ho_Cross.Dispose();
            ho_Contour1.Dispose();
            ho_Contour2.Dispose();

            return;
        }
        public void fit_lines_metrology(HObject ho_Image, HTuple hv_line_rows1, HTuple hv_line_cols1,
                                        HTuple hv_line_rows2, HTuple hv_line_cols2, HTuple hv_edge_threshold, HTuple hv_edge_transition,
                                        HTuple hv_edge_select, HTuple hv_rect_num, HTuple hv_rect_length1, HTuple hv_rect_length2,
                                        HTuple hv_sigma, HTuple hv_score, out HTuple hv_begin_rows, out HTuple hv_begin_cols,
                                        out HTuple hv_end_rows, out HTuple hv_end_cols, out HTuple hv_scores)
        {



            // Local iconic variables 

            HObject ho_line = null, ho_Cross = null;
            HObject ho_Contour2 = null;


            // Local control variables 

            HTuple hv_MetrologyIndices = null, hv_Width = null;
            HTuple hv_Height = null, hv_MetrologyHandle = null;

            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_line);
            HOperatorSet.GenEmptyObj(out ho_Cross);
            HOperatorSet.GenEmptyObj(out ho_Contour2);

            //***********函数内部
            hv_MetrologyIndices = new HTuple();
            hv_begin_rows = new HTuple();
            hv_begin_cols = new HTuple();
            hv_end_rows = new HTuple();
            hv_end_cols = new HTuple();
            hv_scores = new HTuple();
            HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height);
            HOperatorSet.CreateMetrologyModel(out hv_MetrologyHandle);
            HOperatorSet.SetMetrologyModelImageSize(hv_MetrologyHandle, hv_Width, hv_Height);

            Int32[] MetrologyIndices = new Int32[hv_line_rows1.Length];
            Int32 max_thread_num = 3;
            ParallelOptions pOptions = new ParallelOptions();
            pOptions.MaxDegreeOfParallelism = max_thread_num;
            ParallelLoopResult result;
            result = Parallel.For(0, hv_line_rows1.Length, pOptions, (hv_i, pLoopState) =>

            //for (hv_i = 0; (Int32)hv_i <= (Int32)((new HTuple(hv_line_rows1.TupleLength())) - 1); hv_i = (Int32)hv_i + 1)
            {
                HTuple hv_Index = new HTuple();

                //ho_line.Dispose();
                //HOperatorSet.GenContourPolygonXld(out ho_line, ((hv_line_rows1.TupleSelect(
                //    hv_i))).TupleConcat(hv_line_rows2.TupleSelect(hv_i)), ((hv_line_cols1.TupleSelect(
                //    hv_i))).TupleConcat(hv_line_cols2.TupleSelect(hv_i)));
                HOperatorSet.AddMetrologyObjectLineMeasure(hv_MetrologyHandle, hv_line_rows1.TupleSelect(
                    hv_i), hv_line_cols1.TupleSelect(hv_i), hv_line_rows2.TupleSelect(hv_i),
                    hv_line_cols2.TupleSelect(hv_i), hv_rect_length1.TupleSelect(hv_i), hv_rect_length2.TupleSelect(
                    hv_i), hv_sigma.TupleSelect(hv_i), hv_edge_threshold.TupleSelect(hv_i),
                    new HTuple(), new HTuple(), out hv_Index);
                if ((Int32)(new HTuple(((hv_rect_num.TupleSelect(hv_i))).TupleNotEqual(-1))) != 0)
                {
                    HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandle, hv_Index, "num_measures",
                        hv_rect_num.TupleSelect(hv_i));
                }
                HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandle, hv_Index, "measure_select",
                    hv_edge_select.TupleSelect(hv_i));
                //set_metrology_object_param (MetrologyHandle, Index, 'measure_threshold', threshold)
                HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandle, hv_Index, "measure_transition",
                    hv_edge_transition.TupleSelect(hv_i));
                //set_metrology_object_param (MetrologyHandle, Index, 'measure_length1', length1)
                //set_metrology_object_param (MetrologyHandle, Index, 'measure_length2', length2)
                HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandle, hv_Index, "measure_sigma",
                    hv_sigma.TupleSelect(hv_i));
                //set_metrology_object_param (MetrologyHandle, Index, 'num_instances', instances)
                HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandle, hv_Index, "min_score",
                    hv_score.TupleSelect(hv_i));
                //set_metrology_object_param (MetrologyHandle, Index, 'measure_distance', distance)
                MetrologyIndices[hv_i] = hv_Index.I;
                //hv_MetrologyIndices = hv_MetrologyIndices.TupleConcat(hv_Index);
            });
            if (result.IsCompleted)
            {
                hv_MetrologyIndices = MetrologyIndices;
            }

            //将设置的Handle应用与指定图像
            HOperatorSet.ApplyMetrologyModel(ho_Image, hv_MetrologyHandle);

            Double[] begin_rows = new Double[hv_line_rows1.Length];
            Double[] begin_cols = new Double[hv_line_rows1.Length];
            Double[] end_rows = new Double[hv_line_rows1.Length];
            Double[] end_cols = new Double[hv_line_rows1.Length];
            Double[] scores = new Double[hv_line_rows1.Length];

            ParallelOptions pOptions_ex = new ParallelOptions();
            pOptions_ex.MaxDegreeOfParallelism = max_thread_num;
            ParallelLoopResult result_ex;
            result_ex = Parallel.For(0, hv_line_rows1.Length, pOptions_ex, (hv_i, pLoopState_ex) =>

            //for (hv_i = 0; (Int32)hv_i <= (Int32)((new HTuple(hv_line_rows1.TupleLength())) - 1); hv_i = (Int32)hv_i + 1)
            {
                HTuple hv_Row = new HTuple();
                HTuple hv_Column = new HTuple(), hv_Score = new HTuple();
                HTuple hv_RowBegin = new HTuple(), hv_ColBegin = new HTuple();
                HTuple hv_RowEnd = new HTuple(), hv_ColEnd = new HTuple();
                HTuple hv_Nr = new HTuple(), hv_Nc = new HTuple(), hv_Dist = new HTuple();
                HObject ho_Contour = new HObject(), ho_Contour1 = new HObject();
                HOperatorSet.GenEmptyObj(out ho_Contour);
                HOperatorSet.GenEmptyObj(out ho_Contour1);

                ho_Contour.Dispose();
                HOperatorSet.GetMetrologyObjectMeasures(out ho_Contour, hv_MetrologyHandle,
                    hv_MetrologyIndices.TupleSelect(hv_i), hv_edge_transition.TupleSelect(hv_i),
                    out hv_Row, out hv_Column);
                HOperatorSet.GetMetrologyObjectResult(hv_MetrologyHandle, hv_MetrologyIndices.TupleSelect(
                    hv_i), "all", "result_type", "score", out hv_Score);
                if ((Int32)(new HTuple((new HTuple(hv_Score.TupleLength())).TupleGreater(0))) != 0)
                {
                    //ho_Cross.Dispose();
                    //HOperatorSet.GenCrossContourXld(out ho_Cross, hv_Row, hv_Column, 4, 0);
                    ho_Contour1.Dispose();
                    HOperatorSet.GenContourPolygonXld(out ho_Contour1, hv_Row, hv_Column);
                    HOperatorSet.FitLineContourXld(ho_Contour1, "tukey", -1, 0, 8, 2, out hv_RowBegin,
                        out hv_ColBegin, out hv_RowEnd, out hv_ColEnd, out hv_Nr, out hv_Nc,
                        out hv_Dist);
                    //ho_Contour2.Dispose();
                    //HOperatorSet.GenContourPolygonXld(out ho_Contour2, hv_RowBegin.TupleConcat(
                    //    hv_RowEnd), hv_ColBegin.TupleConcat(hv_ColEnd));
                    begin_rows[hv_i] = hv_RowBegin.D;
                    begin_cols[hv_i] = hv_ColBegin.D;
                    end_rows[hv_i] = hv_RowEnd.D;
                    end_cols[hv_i] = hv_ColEnd.D;
                    scores[hv_i] = hv_Score.D;

                    //hv_begin_rows = hv_begin_rows.TupleConcat(hv_RowBegin);
                    //hv_begin_cols = hv_begin_cols.TupleConcat(hv_ColBegin);
                    //hv_end_rows = hv_end_rows.TupleConcat(hv_RowEnd);
                    //hv_end_cols = hv_end_cols.TupleConcat(hv_ColEnd);
                    //hv_scores = hv_scores.TupleConcat(hv_Score);
                }
                else
                {
                    begin_rows[hv_i] = hv_line_rows1[hv_i].D;
                    begin_cols[hv_i] = hv_line_cols1[hv_i].D;
                    end_rows[hv_i] = hv_line_rows2[hv_i].D;
                    end_cols[hv_i] = hv_line_cols2[hv_i].D;
                    scores[hv_i] = -1;
                    //hv_begin_rows = hv_begin_rows.TupleConcat(hv_line_rows1.TupleSelect(hv_i));
                    //hv_begin_cols = hv_begin_cols.TupleConcat(hv_line_cols1.TupleSelect(hv_i));
                    //hv_end_rows = hv_end_rows.TupleConcat(hv_line_rows2.TupleSelect(hv_i));
                    //hv_end_cols = hv_end_cols.TupleConcat(hv_line_cols2.TupleSelect(hv_i));
                    //hv_scores = hv_scores.TupleConcat(-1);
                }
                ho_Contour.Dispose();
                ho_Contour1.Dispose();
            });
            if (result_ex.IsCompleted)
            {
                hv_begin_rows = begin_rows;
                hv_begin_cols = begin_cols;
                hv_end_rows = end_rows;
                hv_end_cols = end_cols;
                hv_scores = scores;
            }
            //将设置的Handle清除，否则会占用内存
            HOperatorSet.ClearMetrologyModel(hv_MetrologyHandle);
            ho_line.Dispose();
            ho_Cross.Dispose();
            ho_Contour2.Dispose();

            return;
        }
        public void fit_rects_metrology1(HObject ho_Image, HTuple hv_RectangleRows, HTuple hv_RectangleColumns,
                                        HTuple hv_RectanglePhis, HTuple hv_RectangleLength1s, HTuple hv_RectangleLength2s,
                                        HTuple hv_MeasureNum, HTuple hv_MeasureSelect, HTuple hv_MeasureTransition,
                                        HTuple hv_MeasureScore, HTuple hv_MeasureSigma, HTuple hv_MeasureLength1, HTuple hv_MeasureLength2,
                                        HTuple hv_MeasureThreshold, out HTuple hv_rectRow, out HTuple hv_rectColumn,
                                        out HTuple hv_rectPhi, out HTuple hv_rectLength1, out HTuple hv_rectLength2,
                                        out HTuple hv_rectScore)
        {



            // Local iconic variables 

            HObject ho_Rectangle = null, ho_Contour = null;
            HObject ho_Cross = null;


            // Local control variables 

            HTuple hv_MetrologyRectangleIndices = null;
            HTuple hv_MetrologyHandle = null, hv_Width = null, hv_Height = null;
            HTuple hv_i = null, hv_Index = new HTuple(), hv_RectangleParameter = new HTuple();
            HTuple hv_RectangleScore = new HTuple(), hv_Row = new HTuple();
            HTuple hv_Column = new HTuple();

            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_Rectangle);
            HOperatorSet.GenEmptyObj(out ho_Contour);
            HOperatorSet.GenEmptyObj(out ho_Cross);

            hv_rectRow = new HTuple();
            hv_rectColumn = new HTuple();
            hv_rectPhi = new HTuple();
            hv_rectLength1 = new HTuple();
            hv_rectLength2 = new HTuple();
            hv_rectScore = new HTuple();

            hv_MetrologyRectangleIndices = new HTuple();
            //
            //Prepare the metrology model data structure
            HOperatorSet.CreateMetrologyModel(out hv_MetrologyHandle);
            //Setting the image width in advance is not
            //necessary, but improves the runtime of the
            //first measurement.
            HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height);
            HOperatorSet.SetMetrologyModelImageSize(hv_MetrologyHandle, hv_Width, hv_Height);
            //Add the metrology rectangle objects to the model
            //as defined above
            for (hv_i = 0; (Int32)hv_i <= (Int32)((new HTuple(hv_RectangleRows.TupleLength())) - 1); hv_i = (Int32)hv_i + 1)
            {
                ho_Rectangle.Dispose();
                //HOperatorSet.GenRectangle2ContourXld(out ho_Rectangle, hv_RectangleRows.TupleSelect(
                //    hv_i), hv_RectangleColumns.TupleSelect(hv_i), hv_RectanglePhis.TupleSelect(
                //    hv_i), hv_RectangleLength1s.TupleSelect(hv_i), hv_RectangleLength2s.TupleSelect(
                //    hv_i));
                HOperatorSet.AddMetrologyObjectRectangle2Measure(hv_MetrologyHandle, hv_RectangleRows.TupleSelect(
                    hv_i), hv_RectangleColumns.TupleSelect(hv_i), hv_RectanglePhis.TupleSelect(
                    hv_i), hv_RectangleLength1s.TupleSelect(hv_i), hv_RectangleLength2s.TupleSelect(
                    hv_i), hv_MeasureLength1, hv_MeasureLength2, hv_MeasureSigma, hv_MeasureThreshold,
                    new HTuple(), new HTuple(), out hv_Index);
                if ((Int32)(new HTuple(((hv_MeasureNum.TupleSelect(hv_i))).TupleNotEqual(-1))) != 0)
                {
                    HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandle, hv_Index, "num_measures",
                        hv_MeasureNum.TupleSelect(hv_i));
                }
                HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandle, hv_Index, "measure_select",
                    hv_MeasureSelect.TupleSelect(hv_i));
                HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandle, hv_Index, "measure_transition",
                    hv_MeasureTransition.TupleSelect(hv_i));
                HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandle, hv_Index, "measure_sigma",
                    hv_MeasureSigma.TupleSelect(hv_i));
                HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandle, hv_Index, "min_score",
                    hv_MeasureScore.TupleSelect(hv_i));
                if (hv_MetrologyRectangleIndices == null)
                    hv_MetrologyRectangleIndices = new HTuple();
                hv_MetrologyRectangleIndices[hv_i] = hv_Index;
            }
            //
            //Perform the measurement
            //
            HOperatorSet.ApplyMetrologyModel(ho_Image, hv_MetrologyHandle);
            //
            for (hv_i = 0; (Int32)hv_i <= (Int32)((new HTuple(hv_MetrologyRectangleIndices.TupleLength()
                )) - 1); hv_i = (Int32)hv_i + 1)
            {
                HOperatorSet.GetMetrologyObjectResult(hv_MetrologyHandle, hv_MetrologyRectangleIndices.TupleSelect(
                    hv_i), "all", "result_type", "param", out hv_RectangleParameter);
                HOperatorSet.GetMetrologyObjectResult(hv_MetrologyHandle, hv_MetrologyRectangleIndices.TupleSelect(
                    hv_i), "all", "result_type", "score", out hv_RectangleScore);

                ho_Contour.Dispose();
                HOperatorSet.GetMetrologyObjectMeasures(out ho_Contour, hv_MetrologyHandle,
                    hv_MetrologyRectangleIndices.TupleSelect(hv_i), hv_MeasureTransition.TupleSelect(
                    hv_i), out hv_Row, out hv_Column);
                ho_Cross.Dispose();
                //HOperatorSet.GenCrossContourXld(out ho_Cross, hv_Row, hv_Column, 6, 0.785398);


                if ((Int32)(new HTuple((new HTuple(hv_RectangleParameter.TupleLength())).TupleGreater(
                    0))) != 0)
                {
                    hv_rectRow = hv_rectRow.TupleConcat(hv_RectangleParameter.TupleSelect(0));
                    hv_rectColumn = hv_rectColumn.TupleConcat(hv_RectangleParameter.TupleSelect(
                        1));
                    hv_rectPhi = hv_rectPhi.TupleConcat(hv_RectangleParameter.TupleSelect(2));
                    hv_rectLength1 = hv_rectLength1.TupleConcat(hv_RectangleParameter.TupleSelect(
                        3));
                    hv_rectLength2 = hv_rectLength2.TupleConcat(hv_RectangleParameter.TupleSelect(
                        4));
                    hv_rectScore = hv_rectScore.TupleConcat(hv_RectangleScore);
                }
                else
                {
                    hv_rectRow = hv_rectRow.TupleConcat(hv_RectangleRows.TupleSelect(hv_i));
                    hv_rectColumn = hv_rectColumn.TupleConcat(hv_RectangleColumns.TupleSelect(
                        hv_i));
                    hv_rectPhi = hv_rectPhi.TupleConcat(hv_RectanglePhis.TupleSelect(hv_i));
                    hv_rectLength1 = hv_rectLength1.TupleConcat(hv_RectangleLength1s.TupleSelect(
                        hv_i));
                    hv_rectLength2 = hv_rectLength2.TupleConcat(hv_RectangleLength2s.TupleSelect(
                        hv_i));
                    hv_rectScore = hv_rectScore.TupleConcat(-1);
                }
            }
            //Clean up memory
            HOperatorSet.ClearMetrologyModel(hv_MetrologyHandle);
            ho_Rectangle.Dispose();
            ho_Contour.Dispose();
            ho_Cross.Dispose();

            return;
        }
        public void fit_rects_metrology(HObject ho_Image, HTuple hv_RectangleRows, HTuple hv_RectangleColumns,
                                        HTuple hv_RectanglePhis, HTuple hv_RectangleLength1s, HTuple hv_RectangleLength2s,
                                        HTuple hv_MeasureNum, HTuple hv_MeasureSelect, HTuple hv_MeasureTransition,
                                        HTuple hv_MeasureScore, HTuple hv_MeasureSigma, HTuple hv_MeasureLength1, HTuple hv_MeasureLength2,
                                        HTuple hv_MeasureThreshold, out HTuple hv_rectRow, out HTuple hv_rectColumn,
                                        out HTuple hv_rectPhi, out HTuple hv_rectLength1, out HTuple hv_rectLength2,
                                        out HTuple hv_rectScore)
        {



            // Local iconic variables 

            HObject ho_Rectangle = null, ho_Contour = null;
            HObject ho_Cross = null;


            // Local control variables 

            HTuple hv_MetrologyRectangleIndices = null;
            HTuple hv_MetrologyHandle = null, hv_Width = null, hv_Height = null;
            HTuple hv_Index = new HTuple(), hv_RectangleParameter = new HTuple();
            HTuple hv_RectangleScore = new HTuple(), hv_Row = new HTuple();
            HTuple hv_Column = new HTuple();

            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_Rectangle);
            HOperatorSet.GenEmptyObj(out ho_Contour);
            HOperatorSet.GenEmptyObj(out ho_Cross);

            hv_rectRow = new HTuple();
            hv_rectColumn = new HTuple();
            hv_rectPhi = new HTuple();
            hv_rectLength1 = new HTuple();
            hv_rectLength2 = new HTuple();
            hv_rectScore = new HTuple();

            hv_MetrologyRectangleIndices = new HTuple();
            //
            //Prepare the metrology model data structure
            HOperatorSet.CreateMetrologyModel(out hv_MetrologyHandle);
            //Setting the image width in advance is not
            //necessary, but improves the runtime of the
            //first measurement.
            HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height);
            HOperatorSet.SetMetrologyModelImageSize(hv_MetrologyHandle, hv_Width, hv_Height);
            //Add the metrology rectangle objects to the model
            //as defined above
            Int32[] MetrologyIndices = new Int32[hv_RectangleRows.Length];
            Int32 max_thread_num = 3;
            ParallelOptions pOptions = new ParallelOptions();
            pOptions.MaxDegreeOfParallelism = max_thread_num;
            ParallelLoopResult result;
            result = Parallel.For(0, hv_RectangleRows.Length, pOptions, (hv_i, pLoopState) =>

            //for (hv_i = 0; (Int32)hv_i <= (Int32)((new HTuple(hv_RectangleRows.TupleLength())) - 1); hv_i = (Int32)hv_i + 1)
            {
                ho_Rectangle.Dispose();
                //HOperatorSet.GenRectangle2ContourXld(out ho_Rectangle, hv_RectangleRows.TupleSelect(
                //    hv_i), hv_RectangleColumns.TupleSelect(hv_i), hv_RectanglePhis.TupleSelect(
                //    hv_i), hv_RectangleLength1s.TupleSelect(hv_i), hv_RectangleLength2s.TupleSelect(
                //    hv_i));
                HOperatorSet.AddMetrologyObjectRectangle2Measure(hv_MetrologyHandle, hv_RectangleRows.TupleSelect(
                    hv_i), hv_RectangleColumns.TupleSelect(hv_i), hv_RectanglePhis.TupleSelect(
                    hv_i), hv_RectangleLength1s.TupleSelect(hv_i), hv_RectangleLength2s.TupleSelect(
                    hv_i), hv_MeasureLength1, hv_MeasureLength2, hv_MeasureSigma, hv_MeasureThreshold,
                    new HTuple(), new HTuple(), out hv_Index);
                if ((Int32)(new HTuple(((hv_MeasureNum.TupleSelect(hv_i))).TupleNotEqual(-1))) != 0)
                {
                    HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandle, hv_Index, "num_measures",
                        hv_MeasureNum.TupleSelect(hv_i));
                }
                HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandle, hv_Index, "measure_select",
                    hv_MeasureSelect.TupleSelect(hv_i));
                HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandle, hv_Index, "measure_transition",
                    hv_MeasureTransition.TupleSelect(hv_i));
                HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandle, hv_Index, "measure_sigma",
                    hv_MeasureSigma.TupleSelect(hv_i));
                HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandle, hv_Index, "min_score",
                    hv_MeasureScore.TupleSelect(hv_i));
                if (hv_MetrologyRectangleIndices == null)
                    hv_MetrologyRectangleIndices = new HTuple();
                //hv_MetrologyRectangleIndices[hv_i] = hv_Index;
                MetrologyIndices[hv_i] = hv_Index;
            });
            if (result.IsCompleted)
            {
                hv_MetrologyRectangleIndices = MetrologyIndices;
            }
            //
            //Perform the measurement
            //
            HOperatorSet.ApplyMetrologyModel(ho_Image, hv_MetrologyHandle);

            Double[] rectRows = new Double[hv_RectangleRows.Length];
            Double[] rectCols = new Double[hv_RectangleRows.Length];
            Double[] rectLen1s = new Double[hv_RectangleRows.Length];
            Double[] rectLen2s = new Double[hv_RectangleRows.Length];
            Double[] rectPhis = new Double[hv_RectangleRows.Length];
            Double[] rectScores = new Double[hv_RectangleRows.Length];
            //
            ParallelOptions pOptions_ex = new ParallelOptions();
            pOptions_ex.MaxDegreeOfParallelism = max_thread_num;
            ParallelLoopResult result_ex;
            result_ex = Parallel.For(0, hv_RectangleRows.Length, pOptions_ex, (hv_i, pLoopState_ex) =>

            //for (hv_i = 0; (Int32)hv_i <= (Int32)((new HTuple(hv_MetrologyRectangleIndices.TupleLength()
            //    )) - 1); hv_i = (Int32)hv_i + 1)
            {
                HOperatorSet.GetMetrologyObjectResult(hv_MetrologyHandle, hv_MetrologyRectangleIndices.TupleSelect(
                    hv_i), "all", "result_type", "param", out hv_RectangleParameter);
                HOperatorSet.GetMetrologyObjectResult(hv_MetrologyHandle, hv_MetrologyRectangleIndices.TupleSelect(
                    hv_i), "all", "result_type", "score", out hv_RectangleScore);

                ho_Contour.Dispose();
                HOperatorSet.GetMetrologyObjectMeasures(out ho_Contour, hv_MetrologyHandle,
                    hv_MetrologyRectangleIndices.TupleSelect(hv_i), hv_MeasureTransition.TupleSelect(
                    hv_i), out hv_Row, out hv_Column);
                ho_Cross.Dispose();
                //HOperatorSet.GenCrossContourXld(out ho_Cross, hv_Row, hv_Column, 6, 0.785398);


                if ((Int32)(new HTuple((new HTuple(hv_RectangleParameter.TupleLength())).TupleGreater(
                    0))) != 0)
                {
                    rectRows[hv_i] = hv_RectangleParameter.TupleSelect(0);
                    rectCols[hv_i] = hv_RectangleParameter.TupleSelect(1);
                    rectPhis[hv_i] = hv_RectangleParameter.TupleSelect(2);
                    rectLen1s[hv_i] = hv_RectangleParameter.TupleSelect(3);
                    rectLen2s[hv_i] = hv_RectangleParameter.TupleSelect(4);
                    rectScores[hv_i] = hv_RectangleScore;
                    //hv_rectRow = hv_rectRow.TupleConcat(hv_RectangleParameter.TupleSelect(0));
                    //hv_rectColumn = hv_rectColumn.TupleConcat(hv_RectangleParameter.TupleSelect(
                    //    1));
                    //hv_rectPhi = hv_rectPhi.TupleConcat(hv_RectangleParameter.TupleSelect(2));
                    //hv_rectLength1 = hv_rectLength1.TupleConcat(hv_RectangleParameter.TupleSelect(
                    //    3));
                    //hv_rectLength2 = hv_rectLength2.TupleConcat(hv_RectangleParameter.TupleSelect(
                    //    4));
                    //hv_rectScore = hv_rectScore.TupleConcat(hv_RectangleScore);
                }
                else
                {
                    rectRows[hv_i] = hv_RectangleRows.TupleSelect(hv_i);
                    rectCols[hv_i] = hv_RectangleColumns.TupleSelect(hv_i);
                    rectPhis[hv_i] = hv_RectanglePhis.TupleSelect(hv_i);
                    rectLen1s[hv_i] = hv_RectangleLength1s.TupleSelect(hv_i);
                    rectLen2s[hv_i] = hv_RectangleLength2s.TupleSelect(hv_i);
                    rectScores[hv_i] = -1;
                    //hv_rectRow = hv_rectRow.TupleConcat(hv_RectangleRows.TupleSelect(hv_i));
                    //hv_rectColumn = hv_rectColumn.TupleConcat(hv_RectangleColumns.TupleSelect(
                    //    hv_i));
                    //hv_rectPhi = hv_rectPhi.TupleConcat(hv_RectanglePhis.TupleSelect(hv_i));
                    //hv_rectLength1 = hv_rectLength1.TupleConcat(hv_RectangleLength1s.TupleSelect(
                    //    hv_i));
                    //hv_rectLength2 = hv_rectLength2.TupleConcat(hv_RectangleLength2s.TupleSelect(
                    //    hv_i));
                    //hv_rectScore = hv_rectScore.TupleConcat(-1);
                }
            });
            if (result_ex.IsCompleted)
            {
                //hv_circle_rows = circle_rows;
                //hv_circle_cols = circle_cols;
                //hv_circle_radius = circle_radius;
                //hv_circle_scores = circle_scores;
                hv_rectRow = rectRows;
                hv_rectColumn = rectCols;
                hv_rectPhi = rectPhis;
                hv_rectLength1 = rectLen1s;
                hv_rectLength2 = rectLen2s;
                hv_rectScore = rectScores;
            }
            //Clean up memory
            HOperatorSet.ClearMetrologyModel(hv_MetrologyHandle);
            ho_Rectangle.Dispose();
            ho_Contour.Dispose();
            ho_Cross.Dispose();

            return;
        }
        public void fit_circles_metrology1(HObject ho_Image, HTuple hv_init_rows, HTuple hv_init_cols,
                                            HTuple hv_init_radius, HTuple hv_edge_threshold, HTuple hv_edge_transition,
                                            HTuple hv_edge_select, HTuple hv_rect_num, HTuple hv_rect_len1, HTuple hv_rect_len2,
                                            HTuple hv_sigma, HTuple hv_score, out HTuple hv_circle_rows, out HTuple hv_circle_cols,
                                            out HTuple hv_circle_radius, out HTuple hv_circle_scores)
        {



            // Local iconic variables 

            HObject ho_ContCircle1 = null;


            // Local control variables 

            HTuple hv_MetrologyIndices = null, hv_Width = null;
            HTuple hv_Height = null, hv_MetrologyHandle = null, hv_i = null;
            HTuple hv_Index = new HTuple(), hv_CircleParameter = new HTuple();
            HTuple hv_CircleScore = new HTuple(), hv_Sequence = new HTuple();
            HTuple hv_CircleRow = new HTuple(), hv_CircleColumn = new HTuple();
            HTuple hv_CircleRadius = new HTuple();

            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_ContCircle1);

            //***********函数内部
            hv_MetrologyIndices = new HTuple();
            hv_circle_rows = new HTuple();
            hv_circle_cols = new HTuple();
            hv_circle_radius = new HTuple();
            hv_circle_scores = new HTuple();
            HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height);
            HOperatorSet.CreateMetrologyModel(out hv_MetrologyHandle);
            HOperatorSet.SetMetrologyModelImageSize(hv_MetrologyHandle, hv_Width, hv_Height);
            for (hv_i = 0; (Int32)hv_i <= (Int32)((new HTuple(hv_init_rows.TupleLength())) - 1); hv_i = (Int32)hv_i + 1)
            {
                ho_ContCircle1.Dispose();
                HOperatorSet.GenCircleContourXld(out ho_ContCircle1, hv_init_rows.TupleSelect(
                    hv_i), hv_init_cols.TupleSelect(hv_i), hv_init_radius.TupleSelect(hv_i),
                    0, 6.28318, "positive", 1);
                HOperatorSet.AddMetrologyObjectCircleMeasure(hv_MetrologyHandle, hv_init_rows.TupleSelect(
                    hv_i), hv_init_cols.TupleSelect(hv_i), hv_init_radius.TupleSelect(hv_i),
                    hv_rect_len1.TupleSelect(hv_i), hv_rect_len2.TupleSelect(hv_i), hv_sigma.TupleSelect(
                    hv_i), (hv_edge_threshold.TupleSelect(hv_i)) - 15, new HTuple(), new HTuple(),
                    out hv_Index);
                if ((Int32)(new HTuple(((hv_rect_num.TupleSelect(hv_i))).TupleNotEqual(-1))) != 0)
                {
                    HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandle, hv_Index, "num_measures",
                        hv_rect_num.TupleSelect(hv_i));
                }
                HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandle, hv_Index, "measure_select",
                    hv_edge_select.TupleSelect(hv_i));
                HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandle, hv_Index, "measure_transition",
                    hv_edge_transition.TupleSelect(hv_i));
                HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandle, hv_Index, "measure_sigma",
                    hv_sigma.TupleSelect(hv_i));
                HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandle, hv_Index, "min_score",
                    hv_score.TupleSelect(hv_i));
                hv_MetrologyIndices = hv_MetrologyIndices.TupleConcat(hv_Index);
            }

            //将设置的Handle应用与指定图像
            HOperatorSet.ApplyMetrologyModel(ho_Image, hv_MetrologyHandle);
            //get_metrology_object_result (MetrologyHandle, MetrologyIndices, 'all', 'result_type', 'param', CircleParameter)
            //Extract the parameters for better readability
            hv_circle_scores = new HTuple();
            for (hv_i = 0; (Int32)hv_i <= (Int32)((new HTuple(hv_init_rows.TupleLength())) - 1); hv_i = (Int32)hv_i + 1)
            {
                //Access the results of the circle measurement
                HOperatorSet.GetMetrologyObjectResult(hv_MetrologyHandle, hv_MetrologyIndices.TupleSelect(
                    hv_i), "all", "result_type", "param", out hv_CircleParameter);

                HOperatorSet.GetMetrologyObjectResult(hv_MetrologyHandle, hv_MetrologyIndices.TupleSelect(
                    hv_i), "all", "result_type", "score", out hv_CircleScore);

                //get_metrology_object_measures (Contour, MetrologyHandle, MetrologyIndices[i], edge_transition[i], Row, Column)
                //gen_cross_contour_xld (Cross, Row, Column, 6, 0.785398)
                //Extract the parameters for better readability
                if ((Int32)(new HTuple((new HTuple(hv_CircleParameter.TupleLength())).TupleEqual(
                    0))) != 0)
                {
                    hv_circle_rows = hv_circle_rows.TupleConcat(hv_init_rows.TupleSelect(hv_i));
                    hv_circle_cols = hv_circle_cols.TupleConcat(hv_init_cols.TupleSelect(hv_i));
                    hv_circle_radius = hv_circle_radius.TupleConcat(500);
                    hv_circle_scores = hv_circle_scores.TupleConcat(-1);
                }
                else
                {
                    hv_Sequence = HTuple.TupleGenSequence(0, (new HTuple(hv_CircleParameter.TupleLength()
                        )) - 1, 3);
                    hv_CircleRow = hv_CircleParameter.TupleSelect(hv_Sequence);
                    hv_CircleColumn = hv_CircleParameter.TupleSelect(hv_Sequence + 1);
                    hv_CircleRadius = hv_CircleParameter.TupleSelect(hv_Sequence + 2);
                    hv_circle_rows = hv_circle_rows.TupleConcat(hv_CircleRow);
                    hv_circle_cols = hv_circle_cols.TupleConcat(hv_CircleColumn);
                    hv_circle_radius = hv_circle_radius.TupleConcat(hv_CircleRadius);
                    hv_circle_scores = hv_circle_scores.TupleConcat(hv_CircleScore);
                }
            }
            //将设置的Handle清除，否则会占用内存
            HOperatorSet.ClearMetrologyModel(hv_MetrologyHandle);
            ho_ContCircle1.Dispose();

            return;
        }

        public void fit_circles_metrology(HObject ho_Image, HTuple hv_init_rows, HTuple hv_init_cols,
                                        HTuple hv_init_radius, HTuple hv_edge_threshold, HTuple hv_edge_transition,
                                        HTuple hv_edge_select, HTuple hv_rect_num, HTuple hv_rect_len1, HTuple hv_rect_len2,
                                        HTuple hv_sigma, HTuple hv_score, out HTuple hv_circle_rows, out HTuple hv_circle_cols,
                                        out HTuple hv_circle_radius, out HTuple hv_circle_scores)
        {



            // Local iconic variables 

            HObject ho_ContCircle1 = null;


            // Local control variables 

            HTuple hv_MetrologyIndices = null, hv_Width = null;
            HTuple hv_Height = null, hv_MetrologyHandle = null;

            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_ContCircle1);

            //***********函数内部
            hv_MetrologyIndices = new HTuple();
            hv_circle_rows = new HTuple();
            hv_circle_cols = new HTuple();
            hv_circle_radius = new HTuple();
            hv_circle_scores = new HTuple();
            HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height);
            HOperatorSet.CreateMetrologyModel(out hv_MetrologyHandle);
            HOperatorSet.SetMetrologyModelImageSize(hv_MetrologyHandle, hv_Width, hv_Height);

            Int32[] MetrologyIndices = new Int32[hv_init_rows.Length];
            Int32 max_thread_num = 3;
            ParallelOptions pOptions = new ParallelOptions();
            pOptions.MaxDegreeOfParallelism = max_thread_num;
            ParallelLoopResult result;
            result = Parallel.For(0, hv_init_rows.Length, pOptions, (hv_i, pLoopState) =>

            //for (hv_i = 0; (Int32)hv_i <= (Int32)((new HTuple(hv_init_rows.TupleLength())) - 1); hv_i = (Int32)hv_i + 1)
            {
                HTuple hv_Index = new HTuple();
                //ho_ContCircle1.Dispose();
                //HOperatorSet.GenCircleContourXld(out ho_ContCircle1, hv_init_rows.TupleSelect(
                //    hv_i), hv_init_cols.TupleSelect(hv_i), hv_init_radius.TupleSelect(hv_i),
                //    0, 6.28318, "positive", 1);
                HOperatorSet.AddMetrologyObjectCircleMeasure(hv_MetrologyHandle, hv_init_rows.TupleSelect(
                    hv_i), hv_init_cols.TupleSelect(hv_i), hv_init_radius.TupleSelect(hv_i),
                    hv_rect_len1.TupleSelect(hv_i), hv_rect_len2.TupleSelect(hv_i), hv_sigma.TupleSelect(
                    hv_i), (hv_edge_threshold.TupleSelect(hv_i)), new HTuple(), new HTuple(),
                    out hv_Index);
                if ((Int32)(new HTuple(((hv_rect_num.TupleSelect(hv_i))).TupleNotEqual(-1))) != 0)
                {
                    HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandle, hv_Index, "num_measures",
                        hv_rect_num.TupleSelect(hv_i));
                }
                HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandle, hv_Index, "measure_select",
                    hv_edge_select.TupleSelect(hv_i));
                HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandle, hv_Index, "measure_transition",
                    hv_edge_transition.TupleSelect(hv_i));
                HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandle, hv_Index, "measure_sigma",
                    hv_sigma.TupleSelect(hv_i));
                HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandle, hv_Index, "min_score",
                    hv_score.TupleSelect(hv_i));
                MetrologyIndices[hv_i] = hv_Index.I;
                //hv_MetrologyIndices = hv_MetrologyIndices.TupleConcat(hv_Index);
            });
            if (result.IsCompleted)
            {
                hv_MetrologyIndices = MetrologyIndices;
            }

            //将设置的Handle应用与指定图像
            HOperatorSet.ApplyMetrologyModel(ho_Image, hv_MetrologyHandle);
            //get_metrology_object_result (MetrologyHandle, MetrologyIndices, 'all', 'result_type', 'param', CircleParameter)
            //Extract the parameters for better readability

            Double[] circle_rows = new Double[hv_init_rows.Length];
            Double[] circle_cols = new Double[hv_init_rows.Length];
            Double[] circle_radius = new Double[hv_init_rows.Length];
            Double[] circle_scores = new Double[hv_init_rows.Length];

            ParallelOptions pOptions_ex = new ParallelOptions();
            pOptions_ex.MaxDegreeOfParallelism = max_thread_num;
            ParallelLoopResult result_ex;
            result_ex = Parallel.For(0, hv_init_rows.Length, pOptions_ex, (hv_i, pLoopState_ex) =>

            //for (hv_i = 0; (Int32)hv_i <= (Int32)((new HTuple(hv_init_rows.TupleLength())) - 1); hv_i = (Int32)hv_i + 1)
            {
                HTuple hv_CircleParameter = new HTuple();
                HTuple hv_CircleScore = new HTuple(), hv_Sequence = new HTuple();
                HTuple hv_CircleRow = new HTuple(), hv_CircleColumn = new HTuple();
                HTuple hv_CircleRadius = new HTuple();

                //Access the results of the circle measurement
                HOperatorSet.GetMetrologyObjectResult(hv_MetrologyHandle, hv_MetrologyIndices.TupleSelect(
                    hv_i), "all", "result_type", "param", out hv_CircleParameter);

                HOperatorSet.GetMetrologyObjectResult(hv_MetrologyHandle, hv_MetrologyIndices.TupleSelect(
                    hv_i), "all", "result_type", "score", out hv_CircleScore);

                //get_metrology_object_measures (Contour, MetrologyHandle, MetrologyIndices[i], edge_transition[i], Row, Column)
                //gen_cross_contour_xld (Cross, Row, Column, 6, 0.785398)
                //Extract the parameters for better readability
                if ((Int32)(new HTuple((new HTuple(hv_CircleParameter.TupleLength())).TupleEqual(
                    0))) != 0)
                {
                    circle_rows[hv_i] = hv_init_rows[hv_i].D;
                    circle_cols[hv_i] = hv_init_cols[hv_i].D;
                    circle_radius[hv_i] = 500;
                    circle_scores[hv_i] = -1;
                    //hv_circle_rows = hv_circle_rows.TupleConcat(hv_init_rows.TupleSelect(hv_i));
                    //hv_circle_cols = hv_circle_cols.TupleConcat(hv_init_cols.TupleSelect(hv_i));
                    //hv_circle_radius = hv_circle_radius.TupleConcat(500);
                    //hv_circle_scores = hv_circle_scores.TupleConcat(-1);
                }
                else
                {
                    hv_Sequence = HTuple.TupleGenSequence(0, (new HTuple(hv_CircleParameter.TupleLength()
                        )) - 1, 3);
                    hv_CircleRow = hv_CircleParameter.TupleSelect(hv_Sequence);
                    hv_CircleColumn = hv_CircleParameter.TupleSelect(hv_Sequence + 1);
                    hv_CircleRadius = hv_CircleParameter.TupleSelect(hv_Sequence + 2);

                    circle_rows[hv_i] = hv_CircleRow.D;
                    circle_cols[hv_i] = hv_CircleColumn.D;
                    circle_radius[hv_i] = hv_CircleRadius.D;
                    circle_scores[hv_i] = hv_CircleScore.D;
                    //hv_circle_rows = hv_circle_rows.TupleConcat(hv_CircleRow);
                    //hv_circle_cols = hv_circle_cols.TupleConcat(hv_CircleColumn);
                    //hv_circle_radius = hv_circle_radius.TupleConcat(hv_CircleRadius);
                    //hv_circle_scores = hv_circle_scores.TupleConcat(hv_CircleScore);
                }
            });
            if (result_ex.IsCompleted)
            {
                hv_circle_rows = circle_rows;
                hv_circle_cols = circle_cols;
                hv_circle_radius = circle_radius;
                hv_circle_scores = circle_scores;
            }
            //将设置的Handle清除，否则会占用内存
            HOperatorSet.ClearMetrologyModel(hv_MetrologyHandle);
            ho_ContCircle1.Dispose();

            return;
        }

        public void fit_ellipses_metrology1(HObject ho_Image, HTuple hv_init_rows, HTuple hv_init_cols,
                                            HTuple hv_init_radius1, HTuple hv_init_radius2, HTuple hv_init_phis, HTuple hv_edge_threshold,
                                            HTuple hv_edge_transition, HTuple hv_edge_select, HTuple hv_rect_num, HTuple hv_rect_len1,
                                            HTuple hv_rect_len2, HTuple hv_sigma, HTuple hv_score, out HTuple hv_ellipse_rows,
                                            out HTuple hv_ellipse_cols, out HTuple hv_ellipse_phis, out HTuple hv_ellipse_radius1,
                                            out HTuple hv_ellipse_radius2, out HTuple hv_ellipse_scores)
        {



            // Local iconic variables 


            // Local control variables 

            HTuple hv_MetrologyIndices = null, hv_Width = null;
            HTuple hv_Height = null, hv_MetrologyHandle = null, hv_i = null;
            HTuple hv_Index = new HTuple(), hv_EllipseParameter = new HTuple();
            HTuple hv_EllipseScore = new HTuple(), hv_Sequence = new HTuple();
            HTuple hv_EllipseRow = new HTuple(), hv_EllipseColumn = new HTuple();
            HTuple hv_EllipsePhi = new HTuple(), hv_EllipseRadius1 = new HTuple();
            HTuple hv_EllipseRadius2 = new HTuple();

            // Initialize local and output iconic variables 

            //***********函数内部
            hv_MetrologyIndices = new HTuple();
            hv_ellipse_rows = new HTuple();
            hv_ellipse_cols = new HTuple();
            hv_ellipse_phis = new HTuple();
            hv_ellipse_radius1 = new HTuple();
            hv_ellipse_radius2 = new HTuple();
            hv_ellipse_scores = new HTuple();
            HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height);
            HOperatorSet.CreateMetrologyModel(out hv_MetrologyHandle);
            HOperatorSet.SetMetrologyModelImageSize(hv_MetrologyHandle, hv_Width, hv_Height);
            for (hv_i = 0; (Int32)hv_i <= (Int32)((new HTuple(hv_init_rows.TupleLength())) - 1); hv_i = (Int32)hv_i + 1)
            {
                //gen_ellipse_contour_xld (ContEllipse, init_rows[i], init_cols[i], init_phis[i], init_radius1[i], init_radius2[i], 0, 6.28318, 'positive', 1.5)
                HOperatorSet.AddMetrologyObjectEllipseMeasure(hv_MetrologyHandle, hv_init_rows.TupleSelect(
                    hv_i), hv_init_cols.TupleSelect(hv_i), hv_init_phis.TupleSelect(hv_i),
                    hv_init_radius1.TupleSelect(hv_i), hv_init_radius2.TupleSelect(2), hv_rect_len1.TupleSelect(
                    hv_i), hv_rect_len2.TupleSelect(hv_i), hv_sigma.TupleSelect(hv_i), (hv_edge_threshold.TupleSelect(
                    hv_i)) - 15, new HTuple(), new HTuple(), out hv_Index);
                if ((Int32)(new HTuple(((hv_rect_num.TupleSelect(hv_i))).TupleNotEqual(-1))) != 0)
                {
                    HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandle, hv_Index, "num_measures",
                        hv_rect_num.TupleSelect(hv_i));
                }
                HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandle, hv_Index, "measure_select",
                    hv_edge_select.TupleSelect(hv_i));
                HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandle, hv_Index, "measure_transition",
                    hv_edge_transition.TupleSelect(hv_i));
                HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandle, hv_Index, "measure_sigma",
                    hv_sigma.TupleSelect(hv_i));
                HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandle, hv_Index, "min_score",
                    hv_score.TupleSelect(hv_i));
                hv_MetrologyIndices = hv_MetrologyIndices.TupleConcat(hv_Index);
            }

            //将设置的Handle应用与指定图像
            HOperatorSet.ApplyMetrologyModel(ho_Image, hv_MetrologyHandle);
            //get_metrology_object_result (MetrologyHandle, MetrologyIndices, 'all', 'result_type', 'param', EllipseParameter)
            //Extract the parameters for better readability
            hv_ellipse_scores = new HTuple();
            for (hv_i = 0; (Int32)hv_i <= (Int32)((new HTuple(hv_init_rows.TupleLength())) - 1); hv_i = (Int32)hv_i + 1)
            {
                //Access the results of the circle measurement
                HOperatorSet.GetMetrologyObjectResult(hv_MetrologyHandle, hv_MetrologyIndices.TupleSelect(
                    hv_i), "all", "result_type", "param", out hv_EllipseParameter);

                HOperatorSet.GetMetrologyObjectResult(hv_MetrologyHandle, hv_MetrologyIndices.TupleSelect(
                    hv_i), "all", "result_type", "score", out hv_EllipseScore);

                //Extract the parameters for better readability
                if ((Int32)(new HTuple((new HTuple(hv_EllipseParameter.TupleLength())).TupleEqual(
                    0))) != 0)
                {
                    hv_ellipse_rows = hv_ellipse_rows.TupleConcat(hv_init_rows.TupleSelect(hv_i));
                    hv_ellipse_cols = hv_ellipse_cols.TupleConcat(hv_init_cols.TupleSelect(hv_i));
                    hv_ellipse_phis = hv_ellipse_phis.TupleConcat(hv_init_phis.TupleSelect(hv_i));
                    hv_ellipse_radius1 = hv_ellipse_radius1.TupleConcat(500);
                    hv_ellipse_radius2 = hv_ellipse_radius2.TupleConcat(500);
                    hv_ellipse_scores = hv_ellipse_scores.TupleConcat(-1);
                }
                else
                {
                    hv_Sequence = HTuple.TupleGenSequence(0, (new HTuple(hv_EllipseParameter.TupleLength()
                        )) - 1, 5);
                    hv_EllipseRow = hv_EllipseParameter.TupleSelect(hv_Sequence);
                    hv_EllipseColumn = hv_EllipseParameter.TupleSelect(hv_Sequence + 1);
                    hv_EllipsePhi = hv_EllipseParameter.TupleSelect(hv_Sequence + 2);
                    hv_EllipseRadius1 = hv_EllipseParameter.TupleSelect(hv_Sequence + 3);
                    hv_EllipseRadius2 = hv_EllipseParameter.TupleSelect(hv_Sequence + 4);
                    hv_ellipse_rows = hv_ellipse_rows.TupleConcat(hv_EllipseRow);
                    hv_ellipse_cols = hv_ellipse_cols.TupleConcat(hv_EllipseColumn);
                    hv_ellipse_phis = hv_ellipse_phis.TupleConcat(hv_EllipsePhi);
                    hv_ellipse_radius1 = hv_ellipse_radius1.TupleConcat(hv_EllipseRadius1);
                    hv_ellipse_radius2 = hv_ellipse_radius2.TupleConcat(hv_EllipseRadius2);
                    hv_ellipse_scores = hv_ellipse_scores.TupleConcat(hv_EllipseScore);
                }
            }
            //将设置的Handle清除，否则会占用内存
            HOperatorSet.ClearMetrologyModel(hv_MetrologyHandle);

            return;
        }
        public void fit_ellipses_metrology(HObject ho_Image, HTuple hv_init_rows, HTuple hv_init_cols,
                                            HTuple hv_init_radius1, HTuple hv_init_radius2, HTuple hv_init_phis, HTuple hv_edge_threshold,
                                            HTuple hv_edge_transition, HTuple hv_edge_select, HTuple hv_rect_num, HTuple hv_rect_len1,
                                            HTuple hv_rect_len2, HTuple hv_sigma, HTuple hv_score, out HTuple hv_ellipse_rows,
                                            out HTuple hv_ellipse_cols, out HTuple hv_ellipse_phis, out HTuple hv_ellipse_radius1,
                                            out HTuple hv_ellipse_radius2, out HTuple hv_ellipse_scores)
        {



            // Local iconic variables 


            // Local control variables 

            HTuple hv_MetrologyIndices = null, hv_Width = null;
            HTuple hv_Height = null, hv_MetrologyHandle = null;

            // Initialize local and output iconic variables 

            //***********函数内部
            hv_MetrologyIndices = new HTuple();
            hv_ellipse_rows = new HTuple();
            hv_ellipse_cols = new HTuple();
            hv_ellipse_phis = new HTuple();
            hv_ellipse_radius1 = new HTuple();
            hv_ellipse_radius2 = new HTuple();
            hv_ellipse_scores = new HTuple();
            HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height);
            HOperatorSet.CreateMetrologyModel(out hv_MetrologyHandle);
            HOperatorSet.SetMetrologyModelImageSize(hv_MetrologyHandle, hv_Width, hv_Height);

            Int32[] MetrologyIndices = new Int32[hv_init_rows.Length];
            Int32 max_thread_num = 3;
            ParallelOptions pOptions = new ParallelOptions();
            pOptions.MaxDegreeOfParallelism = max_thread_num;
            ParallelLoopResult result;
            result = Parallel.For(0, hv_init_rows.Length, pOptions, (hv_i, pLoopState) =>

            //for (hv_i = 0; (Int32)hv_i <= (Int32)((new HTuple(hv_init_rows.TupleLength())) - 1); hv_i = (Int32)hv_i + 1)
            {
                HTuple hv_Index = new HTuple();
                //gen_ellipse_contour_xld (ContEllipse, init_rows[i], init_cols[i], init_phis[i], init_radius1[i], init_radius2[i], 0, 6.28318, 'positive', 1.5)
                HOperatorSet.AddMetrologyObjectEllipseMeasure(hv_MetrologyHandle, hv_init_rows.TupleSelect(
                    hv_i), hv_init_cols.TupleSelect(hv_i), hv_init_phis.TupleSelect(hv_i),
                    hv_init_radius1.TupleSelect(hv_i), hv_init_radius2.TupleSelect(hv_i), hv_rect_len1.TupleSelect(
                    hv_i), hv_rect_len2.TupleSelect(hv_i), hv_sigma.TupleSelect(hv_i), (hv_edge_threshold.TupleSelect(
                    hv_i)), new HTuple(), new HTuple(), out hv_Index);
                if ((Int32)(new HTuple(((hv_rect_num.TupleSelect(hv_i))).TupleNotEqual(-1))) != 0)
                {
                    HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandle, hv_Index, "num_measures",
                        hv_rect_num.TupleSelect(hv_i));
                }
                HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandle, hv_Index, "measure_select",
                    hv_edge_select.TupleSelect(hv_i));
                HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandle, hv_Index, "measure_transition",
                    hv_edge_transition.TupleSelect(hv_i));
                HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandle, hv_Index, "measure_sigma",
                    hv_sigma.TupleSelect(hv_i));
                HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandle, hv_Index, "min_score",
                    hv_score.TupleSelect(hv_i));
                MetrologyIndices[hv_i] = hv_Index.I;
                //hv_MetrologyIndices = hv_MetrologyIndices.TupleConcat(hv_Index);
            });
            if (result.IsCompleted)
            {
                hv_MetrologyIndices = MetrologyIndices;
            }

            //将设置的Handle应用与指定图像
            HOperatorSet.ApplyMetrologyModel(ho_Image, hv_MetrologyHandle);
            //get_metrology_object_result (MetrologyHandle, MetrologyIndices, 'all', 'result_type', 'param', EllipseParameter)
            //Extract the parameters for better readability

            Double[] ellipse_rows = new Double[hv_init_rows.Length];
            Double[] ellipse_cols = new Double[hv_init_rows.Length];
            Double[] ellipse_phis = new Double[hv_init_rows.Length];
            Double[] ellipse_radius1 = new Double[hv_init_rows.Length];
            Double[] ellipse_radius2 = new Double[hv_init_rows.Length];
            Double[] ellipse_scores = new Double[hv_init_rows.Length];

            ParallelOptions pOptions_ex = new ParallelOptions();
            pOptions_ex.MaxDegreeOfParallelism = max_thread_num;
            ParallelLoopResult result_ex;
            result_ex = Parallel.For(0, hv_init_rows.Length, pOptions_ex, (hv_i, pLoopState_ex) =>

            //for (hv_i = 0; (Int32)hv_i <= (Int32)((new HTuple(hv_init_rows.TupleLength())) - 1); hv_i = (Int32)hv_i + 1)
            {
                HTuple hv_EllipseParameter = new HTuple();
                HTuple hv_EllipseScore = new HTuple(), hv_Sequence = new HTuple();
                HTuple hv_EllipseRow = new HTuple(), hv_EllipseColumn = new HTuple();
                HTuple hv_EllipsePhi = new HTuple(), hv_EllipseRadius1 = new HTuple();
                HTuple hv_EllipseRadius2 = new HTuple();
                //Access the results of the circle measurement
                HOperatorSet.GetMetrologyObjectResult(hv_MetrologyHandle, hv_MetrologyIndices.TupleSelect(
                    hv_i), "all", "result_type", "param", out hv_EllipseParameter);

                HOperatorSet.GetMetrologyObjectResult(hv_MetrologyHandle, hv_MetrologyIndices.TupleSelect(
                    hv_i), "all", "result_type", "score", out hv_EllipseScore);

                //Extract the parameters for better readability
                if ((Int32)(new HTuple((new HTuple(hv_EllipseParameter.TupleLength())).TupleEqual(
                    0))) != 0)
                {
                    ellipse_rows[hv_i] = hv_init_rows[hv_i].D;
                    ellipse_cols[hv_i] = hv_init_cols[hv_i].D;
                    ellipse_phis[hv_i] = hv_init_phis[hv_i].D;
                    ellipse_radius1[hv_i] = 500;
                    ellipse_radius2[hv_i] = 500;
                    ellipse_scores[hv_i] = -1;

                    //hv_ellipse_rows = hv_ellipse_rows.TupleConcat(hv_init_rows.TupleSelect(hv_i));
                    //hv_ellipse_cols = hv_ellipse_cols.TupleConcat(hv_init_cols.TupleSelect(hv_i));
                    //hv_ellipse_phis = hv_ellipse_phis.TupleConcat(hv_init_phis.TupleSelect(hv_i));
                    //hv_ellipse_radius1 = hv_ellipse_radius1.TupleConcat(500);
                    //hv_ellipse_radius2 = hv_ellipse_radius2.TupleConcat(500);
                    //hv_ellipse_scores = hv_ellipse_scores.TupleConcat(-1);
                }
                else
                {
                    hv_Sequence = HTuple.TupleGenSequence(0, (new HTuple(hv_EllipseParameter.TupleLength()
                        )) - 1, 5);
                    hv_EllipseRow = hv_EllipseParameter.TupleSelect(hv_Sequence);
                    hv_EllipseColumn = hv_EllipseParameter.TupleSelect(hv_Sequence + 1);
                    hv_EllipsePhi = hv_EllipseParameter.TupleSelect(hv_Sequence + 2);
                    hv_EllipseRadius1 = hv_EllipseParameter.TupleSelect(hv_Sequence + 3);
                    hv_EllipseRadius2 = hv_EllipseParameter.TupleSelect(hv_Sequence + 4);

                    ellipse_rows[hv_i] = hv_EllipseRow.D;
                    ellipse_cols[hv_i] = hv_EllipseColumn.D;
                    ellipse_phis[hv_i] = hv_EllipsePhi.D;
                    ellipse_radius1[hv_i] = hv_EllipseRadius1.D;
                    ellipse_radius2[hv_i] = hv_EllipseRadius2.D;
                    ellipse_scores[hv_i] = hv_EllipseScore.D;
                    //hv_ellipse_rows = hv_ellipse_rows.TupleConcat(hv_EllipseRow);
                    //hv_ellipse_cols = hv_ellipse_cols.TupleConcat(hv_EllipseColumn);
                    //hv_ellipse_phis = hv_ellipse_phis.TupleConcat(hv_EllipsePhi);
                    //hv_ellipse_radius1 = hv_ellipse_radius1.TupleConcat(hv_EllipseRadius1);
                    //hv_ellipse_radius2 = hv_ellipse_radius2.TupleConcat(hv_EllipseRadius2);
                    //hv_ellipse_scores = hv_ellipse_scores.TupleConcat(hv_EllipseScore);
                }
            });
            if (result_ex.IsCompleted)
            {
                hv_ellipse_rows = ellipse_rows;
                hv_ellipse_cols = ellipse_cols;
                hv_ellipse_phis = ellipse_phis;
                hv_ellipse_radius1 = ellipse_radius1;
                hv_ellipse_radius2 = ellipse_radius2;
                hv_ellipse_scores = ellipse_scores;
            }
            //将设置的Handle清除，否则会占用内存
            HOperatorSet.ClearMetrologyModel(hv_MetrologyHandle);

            return;
        }
        public void tuple_mean_ext(HTuple hv_tuple, HTuple hv_alpha, out HTuple hv_mean_tuple)
        {


            // Local control variables 

            HTuple hv_mean_vals = null;

            // Initialize local and output iconic variables 

            hv_mean_tuple = new HTuple();
            HOperatorSet.TupleMean(hv_tuple, out hv_mean_vals);
            hv_mean_tuple = (hv_alpha * hv_tuple) + ((1 - hv_alpha) * hv_mean_vals);

            return;
        }
        public void tuple_mean_outof_local(HTuple hv_tuple, HTuple hv_alpha, out HTuple hv_mean_tuple)
        {


            // Local control variables 

            HTuple hv_Sorted = null, hv_Median = null;
            HTuple hv_offset_percent = null, hv_Less1 = null, hv_Greater = null;
            HTuple hv_Prod = null, hv_Indices1 = null, hv_selected_tuple = null;
            HTuple hv_mean_vals = null, hv_Deviation = null, hv_offset = null;
            HTuple hv_Less = null, hv_Indices = null, hv__tup = null;
            HTuple hv_mean_tuple1 = null;

            // Initialize local and output iconic variables 

            hv_mean_tuple = new HTuple();

            HOperatorSet.TupleSort(hv_tuple, out hv_Sorted);
            HOperatorSet.TupleMedian(hv_Sorted, out hv_Median);
            hv_offset_percent = 0.25;
            HOperatorSet.TupleLessElem(hv_Median * (1 - hv_offset_percent), hv_tuple, out hv_Less1);
            HOperatorSet.TupleGreaterElem(hv_Median * (1 + hv_offset_percent), hv_tuple, out hv_Greater);
            HOperatorSet.TupleMult(hv_Less1, hv_Greater, out hv_Prod);
            HOperatorSet.TupleFind(hv_Prod, 1, out hv_Indices1);
            hv_selected_tuple = hv_tuple.TupleSelect(hv_Indices1);
            HOperatorSet.TupleMean(hv_selected_tuple, out hv_mean_vals);

            HOperatorSet.TupleDeviation(hv_tuple, out hv_Deviation);
            hv_offset = ((hv_tuple - hv_mean_vals)).TupleAbs();
            HOperatorSet.TupleLessElem(hv_offset, hv_Deviation * 4.0, out hv_Less);
            HOperatorSet.TupleFind(hv_Less, 1, out hv_Indices);
            hv__tup = hv_tuple.TupleSelect(hv_Indices);
            hv_mean_tuple1 = (hv_alpha * hv__tup) + ((1 - hv_alpha) * hv_mean_vals);
            HOperatorSet.TupleReplace(hv_tuple, hv_Indices, hv_mean_tuple1, out hv_mean_tuple);

            return;

        }
        public void fit_first_surface(HTuple hv_X, HTuple hv_Y, HTuple hv_Z, out HTuple hv_dZ)
        {


            // Local control variables 

            HTuple hv_MatA = null, hv_ind_row = null, hv_ind_col0 = null;
            HTuple hv_ind_col1 = null, hv_ind_col2 = null, hv_Values = null;
            HTuple hv_vecB = null, hv_vecPara = null, hv_vecB_ = null;
            HTuple hv_Z1_ = null;

            // Initialize local and output iconic variables 

            hv_dZ = new HTuple();

            HOperatorSet.CreateMatrix(new HTuple(hv_X.TupleLength()), 3, 0, out hv_MatA);
            hv_ind_row = HTuple.TupleGenSequence(0, (new HTuple(hv_X.TupleLength())) - 1, 1);
            HOperatorSet.TupleGenConst(new HTuple(hv_X.TupleLength()), 0, out hv_ind_col0);
            HOperatorSet.TupleGenConst(new HTuple(hv_X.TupleLength()), 1, out hv_ind_col1);
            HOperatorSet.TupleGenConst(new HTuple(hv_X.TupleLength()), 2, out hv_ind_col2);
            HOperatorSet.SetValueMatrix(hv_MatA, hv_ind_row, hv_ind_col0, hv_X);
            HOperatorSet.SetValueMatrix(hv_MatA, hv_ind_row, hv_ind_col1, hv_Y);
            HOperatorSet.SetValueMatrix(hv_MatA, hv_ind_row, hv_ind_col2, hv_ind_col1);
            HOperatorSet.GetFullMatrix(hv_MatA, out hv_Values);

            HOperatorSet.CreateMatrix(new HTuple(hv_Z.TupleLength()), 1, 0, out hv_vecB);
            HOperatorSet.SetValueMatrix(hv_vecB, hv_ind_row, hv_ind_col0, hv_Z);
            HOperatorSet.SolveMatrix(hv_MatA, "general", 0, hv_vecB, out hv_vecPara);
            HOperatorSet.MultMatrix(hv_MatA, hv_vecPara, "AB", out hv_vecB_);
            HOperatorSet.GetValueMatrix(hv_vecB_, hv_ind_row, hv_ind_col0, out hv_Z1_);
            hv_dZ = hv_Z - hv_Z1_;

            return;
        }
        public void fit_third_surface(HTuple hv_X, HTuple hv_Y, HTuple hv_Z, out HTuple hv_dZ)
        {


            // Local control variables 

            HTuple hv_A = null, hv_ind_row = null, hv_c0 = null;
            HTuple hv_c1 = null, hv_c2 = null, hv_c3 = null, hv_c4 = null;
            HTuple hv_c5 = null, hv_c6 = null, hv_c7 = null, hv_c8 = null;
            HTuple hv_c9 = null, hv_r = null, hv_B = null, hv_Para = null;
            HTuple hv_B_ = null, hv_Z_ = null, hv_P7 = null, hv_P8 = null;
            HTuple hv_Sqrt = null;

            // Initialize local and output iconic variables 

            //三次曲面拟合
            HOperatorSet.CreateMatrix(new HTuple(hv_X.TupleLength()), 10, 0, out hv_A);
            hv_ind_row = HTuple.TupleGenSequence(0, (new HTuple(hv_X.TupleLength())) - 1, 1);
            HOperatorSet.TupleGenConst(new HTuple(hv_X.TupleLength()), 0, out hv_c0);
            HOperatorSet.TupleGenConst(new HTuple(hv_X.TupleLength()), 1, out hv_c1);
            HOperatorSet.TupleGenConst(new HTuple(hv_X.TupleLength()), 2, out hv_c2);
            HOperatorSet.TupleGenConst(new HTuple(hv_X.TupleLength()), 3, out hv_c3);
            HOperatorSet.TupleGenConst(new HTuple(hv_X.TupleLength()), 4, out hv_c4);
            HOperatorSet.TupleGenConst(new HTuple(hv_X.TupleLength()), 5, out hv_c5);
            HOperatorSet.TupleGenConst(new HTuple(hv_X.TupleLength()), 6, out hv_c6);
            HOperatorSet.TupleGenConst(new HTuple(hv_X.TupleLength()), 7, out hv_c7);
            HOperatorSet.TupleGenConst(new HTuple(hv_X.TupleLength()), 8, out hv_c8);
            HOperatorSet.TupleGenConst(new HTuple(hv_X.TupleLength()), 9, out hv_c9);
            hv_r = HTuple.TupleGenSequence(0, (new HTuple(hv_X.TupleLength())) - 1, 1);
            HOperatorSet.SetValueMatrix(hv_A, hv_r, hv_c0, (hv_X * hv_X) * hv_X);
            HOperatorSet.SetValueMatrix(hv_A, hv_r, hv_c1, (hv_X * hv_X) * hv_Y);
            HOperatorSet.SetValueMatrix(hv_A, hv_r, hv_c2, (hv_X * hv_Y) * hv_Y);
            HOperatorSet.SetValueMatrix(hv_A, hv_r, hv_c3, (hv_Y * hv_Y) * hv_Y);
            HOperatorSet.SetValueMatrix(hv_A, hv_r, hv_c4, hv_X * hv_X);
            HOperatorSet.SetValueMatrix(hv_A, hv_r, hv_c5, hv_X * hv_Y);
            HOperatorSet.SetValueMatrix(hv_A, hv_r, hv_c6, hv_Y * hv_Y);
            HOperatorSet.SetValueMatrix(hv_A, hv_r, hv_c7, hv_X);
            HOperatorSet.SetValueMatrix(hv_A, hv_r, hv_c8, hv_Y);
            HOperatorSet.SetValueMatrix(hv_A, hv_r, hv_c9, hv_c1);
            HOperatorSet.CreateMatrix(new HTuple(hv_Z.TupleLength()), 1, 0, out hv_B);
            HOperatorSet.SetValueMatrix(hv_B, hv_r, hv_c0, hv_Z);
            HOperatorSet.SolveMatrix(hv_A, "general", 0, hv_B, out hv_Para);
            HOperatorSet.MultMatrix(hv_A, hv_Para, "AB", out hv_B_);
            HOperatorSet.GetValueMatrix(hv_B_, hv_r, hv_c0, out hv_Z_);
            HOperatorSet.GetValueMatrix(hv_Para, 7, 0, out hv_P7);
            HOperatorSet.GetValueMatrix(hv_Para, 8, 0, out hv_P8);
            HOperatorSet.TupleSqrt(((hv_P7 * hv_P7) + (hv_P8 * hv_P8)) + 1, out hv_Sqrt);
            hv_dZ = (hv_Z - hv_Z_) / hv_Sqrt;

            return;
        }
        public void fit_third_surface_ext(HTuple hv_X, HTuple hv_Y, HTuple hv_Z, out HTuple hv_surface)
        {


            // Local control variables 

            HTuple hv_A = null, hv_ind_row = null, hv_c0 = null;
            HTuple hv_c1 = null, hv_c2 = null, hv_c3 = null, hv_c4 = null;
            HTuple hv_c5 = null, hv_c6 = null, hv_c7 = null, hv_c8 = null;
            HTuple hv_c9 = null, hv_r = null, hv_B = null;

            // Initialize local and output iconic variables 

            //三次曲面拟合
            HOperatorSet.CreateMatrix(new HTuple(hv_X.TupleLength()), 10, 0, out hv_A);
            hv_ind_row = HTuple.TupleGenSequence(0, (new HTuple(hv_X.TupleLength())) - 1, 1);
            HOperatorSet.TupleGenConst(new HTuple(hv_X.TupleLength()), 0, out hv_c0);
            HOperatorSet.TupleGenConst(new HTuple(hv_X.TupleLength()), 1, out hv_c1);
            HOperatorSet.TupleGenConst(new HTuple(hv_X.TupleLength()), 2, out hv_c2);
            HOperatorSet.TupleGenConst(new HTuple(hv_X.TupleLength()), 3, out hv_c3);
            HOperatorSet.TupleGenConst(new HTuple(hv_X.TupleLength()), 4, out hv_c4);
            HOperatorSet.TupleGenConst(new HTuple(hv_X.TupleLength()), 5, out hv_c5);
            HOperatorSet.TupleGenConst(new HTuple(hv_X.TupleLength()), 6, out hv_c6);
            HOperatorSet.TupleGenConst(new HTuple(hv_X.TupleLength()), 7, out hv_c7);
            HOperatorSet.TupleGenConst(new HTuple(hv_X.TupleLength()), 8, out hv_c8);
            HOperatorSet.TupleGenConst(new HTuple(hv_X.TupleLength()), 9, out hv_c9);
            hv_r = HTuple.TupleGenSequence(0, (new HTuple(hv_X.TupleLength())) - 1, 1);
            HOperatorSet.SetValueMatrix(hv_A, hv_r, hv_c0, (hv_X * hv_X) * hv_X);
            HOperatorSet.SetValueMatrix(hv_A, hv_r, hv_c1, (hv_X * hv_X) * hv_Y);
            HOperatorSet.SetValueMatrix(hv_A, hv_r, hv_c2, (hv_X * hv_Y) * hv_Y);
            HOperatorSet.SetValueMatrix(hv_A, hv_r, hv_c3, (hv_Y * hv_Y) * hv_Y);
            HOperatorSet.SetValueMatrix(hv_A, hv_r, hv_c4, hv_X * hv_X);
            HOperatorSet.SetValueMatrix(hv_A, hv_r, hv_c5, hv_X * hv_Y);
            HOperatorSet.SetValueMatrix(hv_A, hv_r, hv_c6, hv_Y * hv_Y);
            HOperatorSet.SetValueMatrix(hv_A, hv_r, hv_c7, hv_X);
            HOperatorSet.SetValueMatrix(hv_A, hv_r, hv_c8, hv_Y);
            HOperatorSet.SetValueMatrix(hv_A, hv_r, hv_c9, hv_c1);
            HOperatorSet.CreateMatrix(new HTuple(hv_Z.TupleLength()), 1, 0, out hv_B);
            HOperatorSet.SetValueMatrix(hv_B, hv_r, hv_c0, hv_Z);
            HOperatorSet.SolveMatrix(hv_A, "general", 0, hv_B, out hv_surface);

            return;
        }
        public void get_z_from_third_surface(HTuple hv_X, HTuple hv_Y, HTuple hv_surface, out HTuple hv_Z)
        {


            // Local control variables 

            HTuple hv_A = null, hv_ind_row = null, hv_c0 = null;
            HTuple hv_c1 = null, hv_c2 = null, hv_c3 = null, hv_c4 = null;
            HTuple hv_c5 = null, hv_c6 = null, hv_c7 = null, hv_c8 = null;
            HTuple hv_c9 = null, hv_r = null, hv_B_ = null;

            // Initialize local and output iconic variables 

            //三次曲面拟合
            HOperatorSet.CreateMatrix(new HTuple(hv_X.TupleLength()), 10, 0, out hv_A);
            hv_ind_row = HTuple.TupleGenSequence(0, (new HTuple(hv_X.TupleLength())) - 1, 1);
            HOperatorSet.TupleGenConst(new HTuple(hv_X.TupleLength()), 0, out hv_c0);
            HOperatorSet.TupleGenConst(new HTuple(hv_X.TupleLength()), 1, out hv_c1);
            HOperatorSet.TupleGenConst(new HTuple(hv_X.TupleLength()), 2, out hv_c2);
            HOperatorSet.TupleGenConst(new HTuple(hv_X.TupleLength()), 3, out hv_c3);
            HOperatorSet.TupleGenConst(new HTuple(hv_X.TupleLength()), 4, out hv_c4);
            HOperatorSet.TupleGenConst(new HTuple(hv_X.TupleLength()), 5, out hv_c5);
            HOperatorSet.TupleGenConst(new HTuple(hv_X.TupleLength()), 6, out hv_c6);
            HOperatorSet.TupleGenConst(new HTuple(hv_X.TupleLength()), 7, out hv_c7);
            HOperatorSet.TupleGenConst(new HTuple(hv_X.TupleLength()), 8, out hv_c8);
            HOperatorSet.TupleGenConst(new HTuple(hv_X.TupleLength()), 9, out hv_c9);
            hv_r = HTuple.TupleGenSequence(0, (new HTuple(hv_X.TupleLength())) - 1, 1);
            HOperatorSet.SetValueMatrix(hv_A, hv_r, hv_c0, (hv_X * hv_X) * hv_X);
            HOperatorSet.SetValueMatrix(hv_A, hv_r, hv_c1, (hv_X * hv_X) * hv_Y);
            HOperatorSet.SetValueMatrix(hv_A, hv_r, hv_c2, (hv_X * hv_Y) * hv_Y);
            HOperatorSet.SetValueMatrix(hv_A, hv_r, hv_c3, (hv_Y * hv_Y) * hv_Y);
            HOperatorSet.SetValueMatrix(hv_A, hv_r, hv_c4, hv_X * hv_X);
            HOperatorSet.SetValueMatrix(hv_A, hv_r, hv_c5, hv_X * hv_Y);
            HOperatorSet.SetValueMatrix(hv_A, hv_r, hv_c6, hv_Y * hv_Y);
            HOperatorSet.SetValueMatrix(hv_A, hv_r, hv_c7, hv_X);
            HOperatorSet.SetValueMatrix(hv_A, hv_r, hv_c8, hv_Y);
            HOperatorSet.SetValueMatrix(hv_A, hv_r, hv_c9, hv_c1);

            HOperatorSet.MultMatrix(hv_A, hv_surface, "AB", out hv_B_);
            HOperatorSet.GetValueMatrix(hv_B_, hv_r, hv_c0, out hv_Z);

            return;
        }
        public void get_between_ball_distance(HTuple hv_rows, HTuple hv_cols, out HTuple hv_betweenBallDistance)
        {


            // Local control variables 

            HTuple hv_tmp_ids = null, hv_i = null, hv_Reduced = new HTuple();
            HTuple hv_tmp_array = new HTuple();

            // Initialize local and output iconic variables 

            hv_betweenBallDistance = new HTuple();
            if ((Int32)(new HTuple((new HTuple(hv_rows.TupleLength())).TupleLess(1))) != 0)
            {

                return;
            }
            hv_tmp_ids = HTuple.TupleGenSequence(0, (new HTuple(hv_rows.TupleLength())) - 1,
                1);
            for (hv_i = 0; (Int32)hv_i <= (Int32)((new HTuple(hv_rows.TupleLength())) - 1); hv_i = (Int32)hv_i + 1)
            {
                HOperatorSet.TupleRemove(hv_tmp_ids, hv_i, out hv_Reduced);
                hv_tmp_array = (((((hv_rows.TupleSelect(hv_i)) - (hv_rows.TupleSelect(hv_Reduced))) * ((hv_rows.TupleSelect(
                    hv_i)) - (hv_rows.TupleSelect(hv_Reduced)))) + (((hv_cols.TupleSelect(hv_i)) - (hv_cols.TupleSelect(
                    hv_Reduced))) * ((hv_cols.TupleSelect(hv_i)) - (hv_cols.TupleSelect(hv_Reduced)))))).TupleSqrt()
                    ;
                if (hv_betweenBallDistance == null)
                    hv_betweenBallDistance = new HTuple();
                hv_betweenBallDistance[hv_i] = hv_tmp_array.TupleMin();
            }

            return;
        }
        public void calculate_ball_line(HTuple hv_BallRows, HTuple hv_BallColumns, HTuple hv_RectRows,
                                        HTuple hv_RectColumns, out HTuple hv_BL_top, out HTuple hv_BL_left, out HTuple hv_BL_right,
                                        out HTuple hv_BL_bottom, out HTuple hv_BL_min)
        {


            // Local control variables 

            HTuple hv_Index = null, hv_Distance = new HTuple();
            HTuple hv_Min2 = null, hv_Min21 = null;

            HTuple hv_RectColumns_COPY_INP_TMP = hv_RectColumns.Clone();
            HTuple hv_RectRows_COPY_INP_TMP = hv_RectRows.Clone();

            // Initialize local and output iconic variables 

            hv_BL_top = new HTuple();
            hv_BL_left = new HTuple();
            hv_BL_right = new HTuple();
            hv_BL_bottom = new HTuple();
            hv_BL_min = new HTuple();

            hv_RectRows_COPY_INP_TMP = hv_RectRows_COPY_INP_TMP.TupleConcat(hv_RectRows_COPY_INP_TMP.TupleSelect(
                0));
            hv_RectColumns_COPY_INP_TMP = hv_RectColumns_COPY_INP_TMP.TupleConcat(hv_RectColumns_COPY_INP_TMP.TupleSelect(
                0));

            for (hv_Index = 0; (Int32)hv_Index <= (Int32)((new HTuple(hv_RectRows_COPY_INP_TMP.TupleLength()
                )) - 2); hv_Index = (Int32)hv_Index + 1)
            {
                hv_Distance = new HTuple();
                HOperatorSet.DistancePl(hv_BallRows, hv_BallColumns, hv_RectRows_COPY_INP_TMP.TupleSelect(
                    hv_Index), hv_RectColumns_COPY_INP_TMP.TupleSelect(hv_Index), hv_RectRows_COPY_INP_TMP.TupleSelect(
                    hv_Index + 1), hv_RectColumns_COPY_INP_TMP.TupleSelect(hv_Index + 1), out hv_Distance);
                //ball_distance := [ball_distance,Distance]
                switch (hv_Index.I)
                {
                    case 0:
                        hv_BL_top = hv_Distance.Clone();
                        break;
                    case 1:
                        hv_BL_right = hv_Distance.Clone();
                        break;
                    case 2:
                        hv_BL_bottom = hv_Distance.Clone();
                        break;
                    case 3:
                        hv_BL_left = hv_Distance.Clone();
                        break;
                }
            }
            HOperatorSet.TupleMin2(hv_BL_top, hv_BL_left, out hv_Min2);
            HOperatorSet.TupleMin2(hv_BL_right, hv_Min2, out hv_Min21);
            HOperatorSet.TupleMin2(hv_BL_bottom, hv_Min21, out hv_BL_min);
            //tuple_sort_index (ball_distance, Indices)
            //t := (Indices[0]+1)/(|BallRows|*1.0)
            //BL_left := Int32((t-floor(t))*|BallRows|-1)
            //BL_top := ball_distance[Indices[0]]

            return;
        }
        public void get_rect_median_coords(HTuple hv_rect_row, HTuple hv_rect_col, HTuple hv_rect_phi,
                                            HTuple hv_rect_length1, HTuple hv_rect_length2, out HTuple hv_rect_median_rows,
                                            out HTuple hv_rect_median_cols)
        {


            // Local control variables 

            HTuple hv_rect_len = null, hv_i = null, hv_phi = new HTuple();

            // Initialize local and output iconic variables 

            hv_rect_median_rows = new HTuple();
            hv_rect_median_cols = new HTuple();

            if ((Int32)((new HTuple((new HTuple((new HTuple((new HTuple((new HTuple(hv_rect_row.TupleLength()
                )).TupleNotEqual(1))).TupleOr(new HTuple((new HTuple(hv_rect_col.TupleLength()
                )).TupleNotEqual(1))))).TupleOr(new HTuple((new HTuple(hv_rect_phi.TupleLength()
                )).TupleNotEqual(1))))).TupleOr(new HTuple((new HTuple(hv_rect_length1.TupleLength()
                )).TupleNotEqual(1))))).TupleOr(new HTuple((new HTuple(hv_rect_length2.TupleLength()
                )).TupleNotEqual(1)))) != 0)
            {

                return;
            }
            //*******点的顺序按照矩形方向的相反方向起始，顺时针计算
            hv_rect_len = new HTuple();
            hv_rect_len = hv_rect_len.TupleConcat(hv_rect_length1);
            hv_rect_len = hv_rect_len.TupleConcat(hv_rect_length2);
            hv_rect_len = hv_rect_len.TupleConcat(hv_rect_length1);
            hv_rect_len = hv_rect_len.TupleConcat(hv_rect_length2);
            for (hv_i = 0; (Int32)hv_i <= 3; hv_i = (Int32)hv_i + 1)
            {
                hv_phi = hv_rect_phi - (hv_i * ((new HTuple(90)).TupleRad()));
                hv_rect_median_rows = hv_rect_median_rows.TupleConcat(hv_rect_row + ((hv_rect_len.TupleSelect(
                    hv_i)) * (hv_phi.TupleSin())));
                hv_rect_median_cols = hv_rect_median_cols.TupleConcat(hv_rect_col - ((hv_rect_len.TupleSelect(
                    hv_i)) * (hv_phi.TupleCos())));
            }


            return;
        }
        public void create_grid_points_and_corner_ind(HTuple hv_local_row_point_num, HTuple hv_local_col_point_num,
                                              HTuple hv_expand_point_num, out HTuple hv_model_rows, out HTuple hv_model_cols,
                                              out HTuple hv_corner_index)
        {


            // Local control variables 

            HTuple hv_ind0 = null, hv_ind1 = null, hv_ind2 = null;
            HTuple hv_ind3 = null;

            // Initialize local and output iconic variables 

            hv_model_rows = new HTuple();
            hv_model_cols = new HTuple();
            hv_corner_index = new HTuple();

            create_grid_points(hv_local_row_point_num + (hv_expand_point_num * 2), hv_local_col_point_num + (hv_expand_point_num * 2),
                out hv_model_rows, out hv_model_cols);
            hv_ind0 = (hv_expand_point_num * ((hv_expand_point_num * 2) + hv_local_col_point_num)) + hv_expand_point_num;
            hv_ind1 = (hv_ind0 + hv_local_col_point_num) - 1;
            hv_ind2 = ((new HTuple(hv_model_rows.TupleLength())) - hv_ind0) - 1;
            hv_ind3 = (hv_ind2 - hv_local_col_point_num) + 1;
            hv_corner_index = new HTuple();
            hv_corner_index = hv_corner_index.TupleConcat(hv_ind0);
            hv_corner_index = hv_corner_index.TupleConcat(hv_ind1);
            hv_corner_index = hv_corner_index.TupleConcat(hv_ind2);
            hv_corner_index = hv_corner_index.TupleConcat(hv_ind3);

            return;
        }
        public void create_grid_points(HTuple hv_row_point_num, HTuple hv_col_point_num,
                                       out HTuple hv_model_rows, out HTuple hv_model_cols)
        {


            // Local control variables 

            HTuple hv_i = null;

            // Initialize local and output iconic variables 

            hv_model_cols = new HTuple();
            hv_model_rows = new HTuple();
            HTuple end_val2 = hv_row_point_num - 1;
            HTuple step_val2 = 1;
            for (hv_i = 0; hv_i.Continue(end_val2, step_val2); hv_i = hv_i.TupleAdd(step_val2))
            {
                hv_model_rows = hv_model_rows.TupleConcat(HTuple.TupleGenConst(hv_col_point_num,
                    hv_i + 1));
            }

            HTuple end_val6 = hv_row_point_num - 1;
            HTuple step_val6 = 1;
            for (hv_i = 0; hv_i.Continue(end_val6, step_val6); hv_i = hv_i.TupleAdd(step_val6))
            {
                hv_model_cols = hv_model_cols.TupleConcat(HTuple.TupleGenSequence(1, hv_col_point_num,
                    1));
            }

            return;
        }

    }
}
