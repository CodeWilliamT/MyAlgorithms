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
    class JSInspection
    {
        // Local iconic variables 
        HObject ho_Image = null, ho_defect_region = null;
        HObject ho_gold_wire = null, ho_map_rect, ho_defect_rect;

        //out HObjectVector/*{eObjectVector,Dim=1}*/ hvec_vec_pcb_golden_object,
        //out HObjectVector/*{eObjectVector,Dim=1}*/ hvec_vec_ic_golden_object,
        //out HObjectVector/*{eObjectVector,Dim=1}*/ hvec_vec_bond_wire_object,
        //out HTupleVector/*{eTupleVector,Dim=1}*/ hvec_vec_pcb_golden_param_out,
        //out HTupleVector/*{eTupleVector,Dim=1}*/ hvec_vec_ic_golden_param_out,
        //out HTupleVector/*{eTupleVector,Dim=1}*/ hvec_vec_bond_wire_param_out,

        HObjectVector hvec_vec_pcb_golden_object = null;
        HObjectVector hvec_vec_ic_golden_object = null;
        HObjectVector hvec_vec_bond_wire_object = null;
        HTupleVector hvec_vec_pcb_golden_param_out = null;
        HTupleVector hvec_vec_ic_golden_param_out = null;
        HTupleVector hvec_vec_bond_wire_param_out = null;

        HTupleVector hvec_vec_pcb_golden_param = new HTupleVector(1);
        HTupleVector hvec_vec_ic_golden_param = new HTupleVector(1), hvec_vec_pos_param = new HTupleVector(1);
        HTupleVector hvec_vec_bond_wire_param = new HTupleVector(1), hvec_vec_epoxy_param = new HTupleVector(1);




        // Local control variables 

        HTuple hv_model_path = null, hv_pcb_sub_reg_num = null;
        HTuple hv_pcb_sobel_scale = null, hv_pcb_dark_thresh = null;
        HTuple hv_pcb_light_thresh = null, hv_pcb_score_thresh = null;
        HTuple hv_pcb_angle_start = null, hv_pcb_angle_extent = null;
        HTuple hv_pcb_search_size = null, hv_pcb_closing_size = null;
        HTuple hv_pcb_select_operation = null, hv_pcb_width_thresh = null;
        HTuple hv_pcb_height_thresh = null, hv_pcb_area_thresh = null;
        HTuple hv_ic_sub_reg_num = null, hv_ic_sobel_scale = null;
        HTuple hv_ic_dark_thresh = null, hv_ic_light_thresh = null;
        HTuple hv_ic_score_thresh = null, hv_ic_angle_start = null;
        HTuple hv_ic_angle_extent = null, hv_ic_search_size = null;
        HTuple hv_ic_closing_size = null, hv_ic_select_operation = null;
        HTuple hv_ic_width_thresh = null, hv_ic_height_thresh = null;
        HTuple hv_ic_area_thresh = null, hv_pos_row_thresh = null;
        HTuple hv_pos_col_thresh = null, hv_pos_angle_thresh = null;
        HTuple hv_ic_radius_low = null, hv_ic_radius_high = null;
        HTuple hv_lp_match_thresh = null, hv_lp_angle_extent = null;
        HTuple hv_line_search_len = null, hv_line_clip_len = null;
        HTuple hv_line_width = null, hv_line_contrast = null, hv_line_min_seg_len = null;
        HTuple hv_line_angle_extent = null, hv_line_max_gap = null;
        HTuple hv_epoxy_inspect_size = null, hv_epoxy_dark_light = null;
        HTuple hv_epoxy_edge_sigma = null, hv_epoxy_edge_thresh = null;
        HTuple hv_epoxy_dist_thresh = null, hv_R = null, hv_C = null;
        HTuple hv_M = null, hv_N = null, hv_iFlag_model = null;
        HTuple hv_err_msg = null, hv_is_update = null, hv_data_path = null;
        HTuple hv_ImageFiles = null, hv_defect_rows = null, hv_defect_cols = null;
        HTuple hv_dir_ind = null, hv_image_path = new HTuple();
        HTuple hv_Matches = new HTuple(), hv_image_key = new HTuple();
        HTuple hv_Seconds = new HTuple(), hv_Index = new HTuple();
        HTuple hv_defect_r = new HTuple(), hv_defect_c = new HTuple();
        HTuple hv_defect_type = new HTuple(), hv_iFlag = new HTuple();
        HTuple hv_Seconds1 = new HTuple(), hv_dt = new HTuple();
        HTuple hv_map_row = null, hv_map_col = null, hv_r = null;
        HTuple hv_c = new HTuple(), hv_map_rect_size = null, hv_NewtuplePhi = null;
        HTuple hv_NewtupleLen = null, hv_iFlag1 = null;

      


        
        // Initialize local and output iconic variables 

        //public JSInspection()
        //{
        //    // Default settings used in HDevelop 
        //    HOperatorSet.SetSystem("width", 512);
        //    HOperatorSet.SetSystem("height", 512);
        //    if (HalconAPI.isWindows)
        //        HOperatorSet.SetSystem("use_window_thread", "true");
        //    //Action();

        //}


        //public partial class HDevelopExport
        //{
        //#if !(NO_EXPORT_MAIN || NO_EXPORT_APP_MAIN)
        //    public HDevelopExport()
        //    {
        //        // Default settings used in HDevelop 
        //        HOperatorSet.SetSystem("width", 512);
        //        HOperatorSet.SetSystem("height", 512);
        //        if (HalconAPI.isWindows)
        //            HOperatorSet.SetSystem("use_window_thread", "true");
        //        action();
        //    }
        //#endif

        // Procedures 
        // External procedures 
        public void JSLF_AOI_clear_all_model
            (//HTupleVector/*{eTupleVector,Dim=1}*/ hvec_vec_pcb_golden_param,
             //HTupleVector/*{eTupleVector,Dim=1}*/ hvec_vec_ic_golden_param, 
             //HTupleVector/*{eTupleVector,Dim=1}*/ hvec_vec_bond_wire_param,
            out HTuple hv_iFlag)
        {



            // Local iconic variables 

            // Local control variables 

            HTuple hv_Exception = null;
            // Initialize local and output iconic variables 
            hv_iFlag = 0;
            //clear pcb model
            if (hvec_vec_pcb_golden_param_out == null)
            {
                return;
            }

            try
            {
                clear_model(hvec_vec_pcb_golden_param_out[5].T, hvec_vec_pcb_golden_param_out[4].T,
                    out hv_iFlag);
            }
            // catch (Exception) 
            catch (HalconException HDevExpDefaultException1)
            {
                HDevExpDefaultException1.ToHTuple(out hv_Exception);
            }
            //clear ic model
            try
            {
                clear_model(hvec_vec_ic_golden_param_out[5].T, hvec_vec_ic_golden_param_out[4].T, out hv_iFlag);
            }
            // catch (Exception) 
            catch (HalconException HDevExpDefaultException1)
            {
                HDevExpDefaultException1.ToHTuple(out hv_Exception);
            }
            //clear bond wire model
            try
            {
                clear_model(hvec_vec_bond_wire_param_out[3].T, hvec_vec_bond_wire_param_out[2].T, out hv_iFlag);
            }
            // catch (Exception) 
            catch (HalconException HDevExpDefaultException1)
            {
                HDevExpDefaultException1.ToHTuple(out hv_Exception);
            }

            return;
        }

        public void JSLF_AOI_inspect_unit(HObject ho_Image, HObject ho_pcb_dark_thresh_image,
            HObject ho_pcb_light_thresh_image, HObject ho_pcb_match_region, HObject ho_pcb_inspect_region,
            HObject ho_pcb_reject_region, HObject ho_pcb_sub_region, HObject ho_ic_dark_thresh_image,
            HObject ho_ic_light_thresh_image, HObject ho_ic_match_region, HObject ho_ic_inspect_region,
            HObject ho_ic_reject_region, HObject ho_ic_sub_region, HObject ho_pcb_pad, HObject ho_ic_pad,
            out HObject ho_defect_region, out HObject ho_gold_wire, HTuple hv_pcb_ModelID,
            HTuple hv_pcb_model_type, HTuple hv_pcb_score_thresh, HTuple hv_pcb_angle_start,
            HTuple hv_pcb_angle_extent, HTuple hv_pcb_sub_reg_num, HTuple hv_pcb_select_operation,
            HTuple hv_pcb_width_thresh, HTuple hv_pcb_height_thresh, HTuple hv_pcb_area_thresh,
            HTuple hv_pcb_closing_size, HTuple hv_pcb_search_size, HTuple hv_ic_angle_start,
            HTuple hv_ic_ModelID, HTuple hv_ic_model_type, HTuple hv_ic_score_thresh, HTuple hv_ic_angle_extent,
            HTuple hv_ic_sub_reg_num, HTuple hv_ic_select_operation, HTuple hv_ic_width_thresh,
            HTuple hv_ic_height_thresh, HTuple hv_ic_area_thresh, HTuple hv_ic_closing_size,
            HTuple hv_ic_search_size, HTuple hv_pos_row_thresh, HTuple hv_pos_col_thresh,
            HTuple hv_pos_angle_thresh, HTuple hv_lp_ball_num, HTuple hv_on_ic_ind, HTuple hv_ic_radius_low,
            HTuple hv_ic_radius_high, HTuple hv_lp_ball_modelID, HTuple hv_lp_ball_model_type,
            HTuple hv_lp_match_thresh, HTuple hv_lp_angle_extent, HTuple hv_line_search_len,
            HTuple hv_line_clip_len, HTuple hv_line_width, HTuple hv_line_contrast, HTuple hv_line_min_seg_len,
            HTuple hv_line_angle_extent, HTuple hv_line_max_gap, HTuple hv_epoxy_inspect_size,
            HTuple hv_epoxy_dark_light, HTuple hv_epoxy_edge_sigma, HTuple hv_epoxy_edge_thresh,
            HTuple hv_epoxy_dist_thresh, out HTuple hv_iFlag, out HTuple hv_err_msg, out HTuple hv_defect_type)
        {




            // Stack for temporary objects 
            HObject[] OTemp = new HObject[20];

            // Local iconic variables 

            HObject ho_pcb_failure_regions, ho_pcb_inspect_region_affine;
            HObject ho_ic_match_region_, ho_ic_failure_regions, ho_ic_inspect_region_affine;
            HObject ho_lp_pad_affine, ho_ic_pad_affine, ho_wire_defect_region;
            HObject ho_LPBalls, ho_ICBalls, ho_RegionUnion = null, ho_gold_wire_region;
            HObject ho_ConnectedRegions, ho_pcb_defect_region, ho_ic_defect_region;
            HObject ho_epoxy_defect_region;

            // Local control variables 

            HTuple hv_pcb_hom_mat2d = null, hv_pcb_row = null;
            HTuple hv_pcb_col = null, hv_pcb_angle = null, hv_pcb_iFlag = null;
            HTuple hv_pcb_ErrMsg = null, hv_ic_angle_start_ = null;
            HTuple hv_ic_hom_mat2d = null, hv_ic_row = null, hv_ic_col = null;
            HTuple hv_ic_angle = null, hv_ic_iFlag = null, hv_ic_ErrMsg = null;
            HTuple hv_row_diff = null, hv_col_diff = null, hv_angle_diff = null;
            HTuple hv_ic_pos_iFlag = null, hv_bond_wire_iFlag = null;
            HTuple hv_Number = null, hv_EpoxyRow = null, hv_EpoxyCol = null;
            HTuple hv_EpoxyiFlag = null;
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_defect_region);
            HOperatorSet.GenEmptyObj(out ho_gold_wire);
            HOperatorSet.GenEmptyObj(out ho_pcb_failure_regions);
            HOperatorSet.GenEmptyObj(out ho_pcb_inspect_region_affine);
            HOperatorSet.GenEmptyObj(out ho_ic_match_region_);
            HOperatorSet.GenEmptyObj(out ho_ic_failure_regions);
            HOperatorSet.GenEmptyObj(out ho_ic_inspect_region_affine);
            HOperatorSet.GenEmptyObj(out ho_lp_pad_affine);
            HOperatorSet.GenEmptyObj(out ho_ic_pad_affine);
            HOperatorSet.GenEmptyObj(out ho_wire_defect_region);
            HOperatorSet.GenEmptyObj(out ho_LPBalls);
            HOperatorSet.GenEmptyObj(out ho_ICBalls);
            HOperatorSet.GenEmptyObj(out ho_RegionUnion);
            HOperatorSet.GenEmptyObj(out ho_gold_wire_region);
            HOperatorSet.GenEmptyObj(out ho_ConnectedRegions);
            HOperatorSet.GenEmptyObj(out ho_pcb_defect_region);
            HOperatorSet.GenEmptyObj(out ho_ic_defect_region);
            HOperatorSet.GenEmptyObj(out ho_epoxy_defect_region);
            try
            {
                //**********************************************************************
                //defect_type: 0(pcb match failed), 1(pcb surface defect), 2(ic match failed), 3(ic surface defect),                    4(ic bias), 5(bond wire defect), 6(epoxy defect)
                //***************************************
                hv_iFlag = 0;
                hv_err_msg = "";
                hv_defect_type = new HTuple();
                ho_defect_region.Dispose();
                HOperatorSet.GenEmptyObj(out ho_defect_region);
                ho_gold_wire.Dispose();
                HOperatorSet.GenEmptyObj(out ho_gold_wire);
                //******************************************** pcb golden inspect
                ho_pcb_failure_regions.Dispose(); ho_pcb_inspect_region_affine.Dispose();
                inspect_golden_model(ho_Image, ho_pcb_dark_thresh_image, ho_pcb_light_thresh_image,
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
                    hv_defect_type = hv_defect_type.TupleConcat(0);
                    hv_iFlag = -1;
                    ho_pcb_failure_regions.Dispose();
                    ho_pcb_inspect_region_affine.Dispose();
                    ho_ic_match_region_.Dispose();
                    ho_ic_failure_regions.Dispose();
                    ho_ic_inspect_region_affine.Dispose();
                    ho_lp_pad_affine.Dispose();
                    ho_ic_pad_affine.Dispose();
                    ho_wire_defect_region.Dispose();
                    ho_LPBalls.Dispose();
                    ho_ICBalls.Dispose();
                    ho_RegionUnion.Dispose();
                    ho_gold_wire_region.Dispose();
                    ho_ConnectedRegions.Dispose();
                    ho_pcb_defect_region.Dispose();
                    ho_ic_defect_region.Dispose();
                    ho_epoxy_defect_region.Dispose();

                    return;
                }
                //******************************************** ic golden inspect
                ho_ic_match_region_.Dispose();
                HOperatorSet.AffineTransRegion(ho_ic_match_region, out ho_ic_match_region_,
                    hv_pcb_hom_mat2d, "nearest_neighbor");
                hv_ic_angle_start_ = hv_ic_angle_start + hv_pcb_angle;
                ho_ic_failure_regions.Dispose(); ho_ic_inspect_region_affine.Dispose();
                inspect_golden_model(ho_Image, ho_ic_dark_thresh_image, ho_ic_light_thresh_image,
                    ho_ic_match_region_, ho_ic_inspect_region, ho_ic_reject_region, ho_ic_sub_region,
                    out ho_ic_failure_regions, out ho_ic_inspect_region_affine, hv_ic_ModelID,
                    hv_ic_model_type, hv_ic_score_thresh, hv_ic_angle_start_, hv_ic_angle_extent,
                    hv_ic_sub_reg_num, hv_ic_select_operation, hv_ic_width_thresh, hv_ic_height_thresh,
                    hv_ic_area_thresh, hv_ic_closing_size, hv_ic_search_size, out hv_ic_hom_mat2d,
                    out hv_ic_row, out hv_ic_col, out hv_ic_angle, out hv_ic_iFlag, out hv_ic_ErrMsg);
                if ((int)(new HTuple(hv_ic_iFlag.TupleNotEqual(0))) != 0)
                {
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.ConcatObj(ho_defect_region, ho_ic_failure_regions, out ExpTmpOutVar_0
                            );
                        ho_defect_region.Dispose();
                        ho_defect_region = ExpTmpOutVar_0;
                    }
                    hv_defect_type = hv_defect_type.TupleConcat(2);
                    hv_iFlag = -1;
                    ho_pcb_failure_regions.Dispose();
                    ho_pcb_inspect_region_affine.Dispose();
                    ho_ic_match_region_.Dispose();
                    ho_ic_failure_regions.Dispose();
                    ho_ic_inspect_region_affine.Dispose();
                    ho_lp_pad_affine.Dispose();
                    ho_ic_pad_affine.Dispose();
                    ho_wire_defect_region.Dispose();
                    ho_LPBalls.Dispose();
                    ho_ICBalls.Dispose();
                    ho_RegionUnion.Dispose();
                    ho_gold_wire_region.Dispose();
                    ho_ConnectedRegions.Dispose();
                    ho_pcb_defect_region.Dispose();
                    ho_ic_defect_region.Dispose();
                    ho_epoxy_defect_region.Dispose();

                    return;
                }
                //******************************************** ic position inspect
                compare_position(hv_ic_row, hv_ic_col, hv_ic_angle, hv_pcb_row, hv_pcb_col,
                    hv_pcb_angle, hv_pos_row_thresh, hv_pos_col_thresh, hv_pos_angle_thresh,
                    out hv_row_diff, out hv_col_diff, out hv_angle_diff, out hv_ic_pos_iFlag);
                if ((int)(new HTuple(hv_ic_pos_iFlag.TupleNotEqual(0))) != 0)
                {
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.ConcatObj(ho_defect_region, ho_ic_inspect_region_affine, out ExpTmpOutVar_0
                            );
                        ho_defect_region.Dispose();
                        ho_defect_region = ExpTmpOutVar_0;
                    }
                    hv_defect_type = hv_defect_type.TupleConcat(4);
                    hv_iFlag = -1;
                    ho_pcb_failure_regions.Dispose();
                    ho_pcb_inspect_region_affine.Dispose();
                    ho_ic_match_region_.Dispose();
                    ho_ic_failure_regions.Dispose();
                    ho_ic_inspect_region_affine.Dispose();
                    ho_lp_pad_affine.Dispose();
                    ho_ic_pad_affine.Dispose();
                    ho_wire_defect_region.Dispose();
                    ho_LPBalls.Dispose();
                    ho_ICBalls.Dispose();
                    ho_RegionUnion.Dispose();
                    ho_gold_wire_region.Dispose();
                    ho_ConnectedRegions.Dispose();
                    ho_pcb_defect_region.Dispose();
                    ho_ic_defect_region.Dispose();
                    ho_epoxy_defect_region.Dispose();

                    return;
                }
                //******************************************** bond wire inspect
                ho_lp_pad_affine.Dispose();
                HOperatorSet.AffineTransRegion(ho_pcb_pad, out ho_lp_pad_affine, hv_pcb_hom_mat2d,
                    "nearest_neighbor");
                ho_ic_pad_affine.Dispose();
                HOperatorSet.AffineTransRegion(ho_ic_pad, out ho_ic_pad_affine, hv_ic_hom_mat2d,
                    "nearest_neighbor");
                ho_wire_defect_region.Dispose(); ho_gold_wire.Dispose(); ho_LPBalls.Dispose(); ho_ICBalls.Dispose();
                bond_wire_detect(ho_Image, ho_lp_pad_affine, ho_ic_pad_affine, out ho_wire_defect_region,
                    out ho_gold_wire, out ho_LPBalls, out ho_ICBalls, hv_lp_ball_num, hv_on_ic_ind,
                    hv_ic_radius_low, hv_ic_radius_high, hv_lp_ball_modelID, hv_lp_ball_model_type,
                    hv_lp_match_thresh, hv_lp_angle_extent, hv_line_search_len, hv_line_clip_len,
                    hv_line_width, hv_line_contrast, hv_line_min_seg_len, hv_line_angle_extent,
                    hv_line_max_gap, out hv_bond_wire_iFlag);
                if ((int)(new HTuple(hv_bond_wire_iFlag.TupleNotEqual(0))) != 0)
                {
                    ho_RegionUnion.Dispose();
                    HOperatorSet.Union1(ho_wire_defect_region, out ho_RegionUnion);
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.ConcatObj(ho_defect_region, ho_RegionUnion, out ExpTmpOutVar_0
                            );
                        ho_defect_region.Dispose();
                        ho_defect_region = ExpTmpOutVar_0;
                    }
                    hv_defect_type = hv_defect_type.TupleConcat(5);
                    hv_iFlag = -1;
                    ho_pcb_failure_regions.Dispose();
                    ho_pcb_inspect_region_affine.Dispose();
                    ho_ic_match_region_.Dispose();
                    ho_ic_failure_regions.Dispose();
                    ho_ic_inspect_region_affine.Dispose();
                    ho_lp_pad_affine.Dispose();
                    ho_ic_pad_affine.Dispose();
                    ho_wire_defect_region.Dispose();
                    ho_LPBalls.Dispose();
                    ho_ICBalls.Dispose();
                    ho_RegionUnion.Dispose();
                    ho_gold_wire_region.Dispose();
                    ho_ConnectedRegions.Dispose();
                    ho_pcb_defect_region.Dispose();
                    ho_ic_defect_region.Dispose();
                    ho_epoxy_defect_region.Dispose();

                    return;
                }
                //select pcb defect region
                ho_gold_wire_region.Dispose();
                contours_neighborhood_regions(ho_gold_wire, out ho_gold_wire_region, hv_line_width + 2);
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.Difference(ho_pcb_failure_regions, ho_gold_wire_region, out ExpTmpOutVar_0
                        );
                    ho_pcb_failure_regions.Dispose();
                    ho_pcb_failure_regions = ExpTmpOutVar_0;
                }
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.Difference(ho_pcb_failure_regions, ho_LPBalls, out ExpTmpOutVar_0
                        );
                    ho_pcb_failure_regions.Dispose();
                    ho_pcb_failure_regions = ExpTmpOutVar_0;
                }
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.Difference(ho_pcb_failure_regions, ho_ICBalls, out ExpTmpOutVar_0
                        );
                    ho_pcb_failure_regions.Dispose();
                    ho_pcb_failure_regions = ExpTmpOutVar_0;
                }
                ho_ConnectedRegions.Dispose();
                HOperatorSet.Connection(ho_pcb_failure_regions, out ho_ConnectedRegions);
                ho_pcb_defect_region.Dispose();
                HOperatorSet.SelectShape(ho_ConnectedRegions, out ho_pcb_defect_region, ((new HTuple("rect2_len1")).TupleConcat(
                    "rect2_len2")).TupleConcat("area"), "and", ((((hv_pcb_width_thresh / 2.0)).TupleConcat(
                    hv_pcb_height_thresh / 2.0))).TupleConcat(hv_pcb_area_thresh), ((new HTuple(999999)).TupleConcat(
                    999999)).TupleConcat(9999999));
                HOperatorSet.CountObj(ho_pcb_defect_region, out hv_Number);
                if ((int)(hv_Number) != 0)
                {
                    ho_RegionUnion.Dispose();
                    HOperatorSet.Union1(ho_pcb_defect_region, out ho_RegionUnion);
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.ConcatObj(ho_defect_region, ho_RegionUnion, out ExpTmpOutVar_0
                            );
                        ho_defect_region.Dispose();
                        ho_defect_region = ExpTmpOutVar_0;
                    }
                    hv_defect_type = hv_defect_type.TupleConcat(1);
                    hv_iFlag = -1;
                    ho_pcb_failure_regions.Dispose();
                    ho_pcb_inspect_region_affine.Dispose();
                    ho_ic_match_region_.Dispose();
                    ho_ic_failure_regions.Dispose();
                    ho_ic_inspect_region_affine.Dispose();
                    ho_lp_pad_affine.Dispose();
                    ho_ic_pad_affine.Dispose();
                    ho_wire_defect_region.Dispose();
                    ho_LPBalls.Dispose();
                    ho_ICBalls.Dispose();
                    ho_RegionUnion.Dispose();
                    ho_gold_wire_region.Dispose();
                    ho_ConnectedRegions.Dispose();
                    ho_pcb_defect_region.Dispose();
                    ho_ic_defect_region.Dispose();
                    ho_epoxy_defect_region.Dispose();

                    return;
                }
                //select ic defect region
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.Difference(ho_ic_failure_regions, ho_gold_wire_region, out ExpTmpOutVar_0
                        );
                    ho_ic_failure_regions.Dispose();
                    ho_ic_failure_regions = ExpTmpOutVar_0;
                }
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.Difference(ho_ic_failure_regions, ho_ICBalls, out ExpTmpOutVar_0
                        );
                    ho_ic_failure_regions.Dispose();
                    ho_ic_failure_regions = ExpTmpOutVar_0;
                }
                ho_ConnectedRegions.Dispose();
                HOperatorSet.Connection(ho_ic_failure_regions, out ho_ConnectedRegions);
                ho_ic_defect_region.Dispose();
                HOperatorSet.SelectShape(ho_ConnectedRegions, out ho_ic_defect_region, ((new HTuple("rect2_len1")).TupleConcat(
                    "rect2_len2")).TupleConcat("area"), "and", ((((hv_ic_width_thresh / 2.0)).TupleConcat(
                    hv_ic_height_thresh / 2.0))).TupleConcat(hv_ic_area_thresh), ((new HTuple(999999)).TupleConcat(
                    999999)).TupleConcat(9999999));
                HOperatorSet.CountObj(ho_ic_defect_region, out hv_Number);
                if ((int)(hv_Number) != 0)
                {
                    ho_RegionUnion.Dispose();
                    HOperatorSet.Union1(ho_ic_defect_region, out ho_RegionUnion);
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.ConcatObj(ho_defect_region, ho_RegionUnion, out ExpTmpOutVar_0
                            );
                        ho_defect_region.Dispose();
                        ho_defect_region = ExpTmpOutVar_0;
                    }
                    hv_defect_type = hv_defect_type.TupleConcat(3);
                    hv_iFlag = -1;
                    ho_pcb_failure_regions.Dispose();
                    ho_pcb_inspect_region_affine.Dispose();
                    ho_ic_match_region_.Dispose();
                    ho_ic_failure_regions.Dispose();
                    ho_ic_inspect_region_affine.Dispose();
                    ho_lp_pad_affine.Dispose();
                    ho_ic_pad_affine.Dispose();
                    ho_wire_defect_region.Dispose();
                    ho_LPBalls.Dispose();
                    ho_ICBalls.Dispose();
                    ho_RegionUnion.Dispose();
                    ho_gold_wire_region.Dispose();
                    ho_ConnectedRegions.Dispose();
                    ho_pcb_defect_region.Dispose();
                    ho_ic_defect_region.Dispose();
                    ho_epoxy_defect_region.Dispose();

                    return;
                }
                //********************************************* expoxy inspect
                ho_epoxy_defect_region.Dispose();
                epoxy_distance_inspect(ho_Image, ho_ic_inspect_region_affine, out ho_epoxy_defect_region,
                    hv_epoxy_inspect_size, hv_epoxy_edge_thresh, hv_epoxy_edge_sigma, hv_epoxy_dark_light,
                    hv_epoxy_dist_thresh, out hv_EpoxyRow, out hv_EpoxyCol, out hv_EpoxyiFlag);
                if ((int)(new HTuple(hv_EpoxyiFlag.TupleEqual(-1))) != 0)
                {
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.ConcatObj(ho_defect_region, ho_epoxy_defect_region, out ExpTmpOutVar_0
                            );
                        ho_defect_region.Dispose();
                        ho_defect_region = ExpTmpOutVar_0;
                    }
                    hv_defect_type = hv_defect_type.TupleConcat(6);
                    hv_iFlag = -1;
                    ho_pcb_failure_regions.Dispose();
                    ho_pcb_inspect_region_affine.Dispose();
                    ho_ic_match_region_.Dispose();
                    ho_ic_failure_regions.Dispose();
                    ho_ic_inspect_region_affine.Dispose();
                    ho_lp_pad_affine.Dispose();
                    ho_ic_pad_affine.Dispose();
                    ho_wire_defect_region.Dispose();
                    ho_LPBalls.Dispose();
                    ho_ICBalls.Dispose();
                    ho_RegionUnion.Dispose();
                    ho_gold_wire_region.Dispose();
                    ho_ConnectedRegions.Dispose();
                    ho_pcb_defect_region.Dispose();
                    ho_ic_defect_region.Dispose();
                    ho_epoxy_defect_region.Dispose();

                    return;
                }
                ho_pcb_failure_regions.Dispose();
                ho_pcb_inspect_region_affine.Dispose();
                ho_ic_match_region_.Dispose();
                ho_ic_failure_regions.Dispose();
                ho_ic_inspect_region_affine.Dispose();
                ho_lp_pad_affine.Dispose();
                ho_ic_pad_affine.Dispose();
                ho_wire_defect_region.Dispose();
                ho_LPBalls.Dispose();
                ho_ICBalls.Dispose();
                ho_RegionUnion.Dispose();
                ho_gold_wire_region.Dispose();
                ho_ConnectedRegions.Dispose();
                ho_pcb_defect_region.Dispose();
                ho_ic_defect_region.Dispose();
                ho_epoxy_defect_region.Dispose();

                return;
            }
            catch (HalconException HDevExpDefaultException)
            {
                ho_pcb_failure_regions.Dispose();
                ho_pcb_inspect_region_affine.Dispose();
                ho_ic_match_region_.Dispose();
                ho_ic_failure_regions.Dispose();
                ho_ic_inspect_region_affine.Dispose();
                ho_lp_pad_affine.Dispose();
                ho_ic_pad_affine.Dispose();
                ho_wire_defect_region.Dispose();
                ho_LPBalls.Dispose();
                ho_ICBalls.Dispose();
                ho_RegionUnion.Dispose();
                ho_gold_wire_region.Dispose();
                ho_ConnectedRegions.Dispose();
                ho_pcb_defect_region.Dispose();
                ho_ic_defect_region.Dispose();
                ho_epoxy_defect_region.Dispose();

                throw HDevExpDefaultException;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ho_Image">一副图片</param>
        /// <param name="hv_image_key">数组 B-R-C</param>
        /// <param name="hv_R"></param>
        /// <param name="hv_C"></param>
        /// <param name="hv_M"></param>
        /// <param name="hv_N"></param>
        /// <param name="ho_defect_regions"></param>
        /// <param name="ho_bond_wires"></param>
        /// <param name="hv_defect_r"></param>
        /// <param name="hv_defect_c"></param>
        /// <param name="hv_defect_type"></param>
        /// <param name="hv_iFlag"></param>
        /// <param name="hv_err_msg"></param>

        public void JSLF_AOI_inspection(
         HObject ho_Image,
    HTuple hv_image_key,
    out HObject ho_defect_regions,
    out HObject ho_bond_wires,
    out HTuple hv_defect_r,
    out HTuple hv_defect_c,
    out HTuple hv_defect_type,
    out HTuple hv_iFlag,
    out HTuple hv_err_msg)
        {

            using (HDevThreadContext context = new HDevThreadContext())
            {
                // +++ Threading variables 
                HDevThread devThread;


                // Stack for temporary objects 
                HObject[] OTemp = new HObject[20];

                // Local iconic variables 

                HObject ho_pcb_dark_thresh_image = null, ho_pcb_light_thresh_image = null;
                HObject ho_pcb_match_region = null, ho_pcb_inspect_region = null;
                HObject ho_pcb_reject_region = null, ho_pcb_sub_region = null;
                HObject ho_ic_dark_thresh_image = null, ho_ic_light_thresh_image = null;
                HObject ho_ic_match_region = null, ho_ic_inspect_region = null;
                HObject ho_ic_reject_region = null, ho_ic_sub_region = null;
                HObject ho_lp_pad = null, ho_ic_pad = null, ho_pcb_match_region_unit = null;
                HObject ho_defect_region_unit = null, ho_bond_wire_unit = null;

                HObjectVector hvec_vec_defect_region = new HObjectVector(1);
                HObjectVector hvec_vec_bond_wire = new HObjectVector(1);

                // Local control variables 

                HTuple hv_iFlags = null, hv_block = null, hv_first_r = null;
                HTuple hv_first_c = null, hv_pcb_sub_reg_num = null, hv_pcb_ModelID = null;
                HTuple hv_pcb_model_type = null, hv_pcb_score_thresh = null;
                HTuple hv_pcb_angle_start = null, hv_pcb_angle_extent = null;
                HTuple hv_pcb_search_size = null, hv_pcb_closing_size = null;
                HTuple hv_pcb_select_operation = null, hv_pcb_width_thresh = null;
                HTuple hv_pcb_height_thresh = null, hv_pcb_area_thresh = null;
                HTuple hv_ic_sub_reg_num = null, hv_ic_ModelID = null;
                HTuple hv_ic_model_type = null, hv_ic_score_thresh = null;
                HTuple hv_ic_angle_start = null, hv_ic_angle_extent = null;
                HTuple hv_ic_search_size = null, hv_ic_closing_size = null;
                HTuple hv_ic_select_operation = null, hv_ic_width_thresh = null;
                HTuple hv_ic_height_thresh = null, hv_ic_area_thresh = null;
                HTuple hv_row_thresh = null, hv_col_thresh = null, hv_angle_thresh = null;
                HTuple hv_lp_ball_num = null, hv_on_ic_ind = null, hv_lp_ball_modelID = null;
                HTuple hv_lp_ball_model_type = null, hv_ic_radius_low = null;
                HTuple hv_ic_radius_high = null, hv_lp_match_thresh = null;
                HTuple hv_lp_angle_extent = null, hv_line_search_len = null;
                HTuple hv_line_clip_len = null, hv_line_width = null, hv_line_contrast = null;
                HTuple hv_line_min_seg_len = null, hv_line_angle_extent = null;
                HTuple hv_line_max_gap = null, hv_epoxy_inspect_size = null;
                HTuple hv_epoxy_dark_light = null, hv_epoxy_edge_sigma = null;
                HTuple hv_epoxy_edge_thresh = null, hv_epoxy_dist_thresh = null;
                HTuple hv_Number = null, hv_is_mutithread = null, hv_Index = new HTuple();
                HTuple hv_block_r = new HTuple(), hv_block_c = new HTuple();
                HTuple hv_Threads = new HTuple(), hv_thread_num = new HTuple();
                HTuple hv_I = new HTuple(), hv_iFlag_unit = new HTuple();
                HTuple hv_err_msg_unit = new HTuple(), hv_defect_type_unit = new HTuple();
                HTuple hv_Indices = null;

                HTupleVector hvec_vec_inspect_r = new HTupleVector(1);
                HTupleVector hvec_vec_inspect_c = new HTupleVector(1), hvec_vec_iFlag = new HTupleVector(1);
                HTupleVector hvec_vec_err_msg = new HTupleVector(1), hvec_vec_defect_type = new HTupleVector(1);
                HTupleVector hvec_VThreads = new HTupleVector(1);
                // Initialize local and output iconic variables 
                HOperatorSet.GenEmptyObj(out ho_defect_regions);
                HOperatorSet.GenEmptyObj(out ho_bond_wires);
                HOperatorSet.GenEmptyObj(out ho_pcb_dark_thresh_image);
                HOperatorSet.GenEmptyObj(out ho_pcb_light_thresh_image);
                HOperatorSet.GenEmptyObj(out ho_pcb_match_region);
                HOperatorSet.GenEmptyObj(out ho_pcb_inspect_region);
                HOperatorSet.GenEmptyObj(out ho_pcb_reject_region);
                HOperatorSet.GenEmptyObj(out ho_pcb_sub_region);
                HOperatorSet.GenEmptyObj(out ho_ic_dark_thresh_image);
                HOperatorSet.GenEmptyObj(out ho_ic_light_thresh_image);
                HOperatorSet.GenEmptyObj(out ho_ic_match_region);
                HOperatorSet.GenEmptyObj(out ho_ic_inspect_region);
                HOperatorSet.GenEmptyObj(out ho_ic_reject_region);
                HOperatorSet.GenEmptyObj(out ho_ic_sub_region);
                HOperatorSet.GenEmptyObj(out ho_lp_pad);
                HOperatorSet.GenEmptyObj(out ho_ic_pad);
                HOperatorSet.GenEmptyObj(out ho_pcb_match_region_unit);
                HOperatorSet.GenEmptyObj(out ho_defect_region_unit);
                HOperatorSet.GenEmptyObj(out ho_bond_wire_unit);
                try
                {
                    hv_iFlag = 0;
                    hv_iFlags = new HTuple();
                    hv_err_msg = "";
                    hv_defect_type = new HTuple();
                    hv_defect_r = new HTuple();
                    hv_defect_c = new HTuple();
                    ho_defect_regions.Dispose();
                    HOperatorSet.GenEmptyObj(out ho_defect_regions);
                    ho_bond_wires.Dispose();
                    HOperatorSet.GenEmptyObj(out ho_bond_wires);
                    hv_block = hv_image_key[0];
                    hv_first_r = hv_image_key[1];
                    hv_first_c = hv_image_key[2];
                    //******************************************** pcb golden inspect input
                    //model
                    ho_pcb_dark_thresh_image.Dispose();
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        ho_pcb_dark_thresh_image = hvec_vec_pcb_golden_object[2].O.CopyObj(1, -1);
                    }
                    ho_pcb_light_thresh_image.Dispose();
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        ho_pcb_light_thresh_image = hvec_vec_pcb_golden_object[3].O.CopyObj(1, -1);
                    }
                    ho_pcb_match_region.Dispose();
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        ho_pcb_match_region = hvec_vec_pcb_golden_object[4].O.CopyObj(1, -1);
                    }
                    ho_pcb_inspect_region.Dispose();
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        ho_pcb_inspect_region = hvec_vec_pcb_golden_object[5].O.CopyObj(1, -1);
                    }
                    ho_pcb_reject_region.Dispose();
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        ho_pcb_reject_region = hvec_vec_pcb_golden_object[6].O.CopyObj(1, -1);
                    }
                    ho_pcb_sub_region.Dispose();
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        ho_pcb_sub_region = hvec_vec_pcb_golden_object[7].O.CopyObj(1, -1);
                    }
                    //parameters
                    hv_pcb_sub_reg_num = hvec_vec_pcb_golden_param_out[0].T.Clone();
                    hv_pcb_ModelID = hvec_vec_pcb_golden_param_out[4].T.Clone();
                    hv_pcb_model_type = hvec_vec_pcb_golden_param_out[5].T.Clone();
                    hv_pcb_score_thresh = hvec_vec_pcb_golden_param_out[6].T.Clone();
                    hv_pcb_angle_start = hvec_vec_pcb_golden_param_out[7].T.Clone();
                    hv_pcb_angle_extent = hvec_vec_pcb_golden_param_out[8].T.Clone();
                    hv_pcb_search_size = hvec_vec_pcb_golden_param_out[9].T.Clone();
                    hv_pcb_closing_size = hvec_vec_pcb_golden_param_out[10].T.Clone();
                    hv_pcb_select_operation = hvec_vec_pcb_golden_param_out[11].T.Clone();
                    hv_pcb_width_thresh = hvec_vec_pcb_golden_param_out[12].T.Clone();
                    hv_pcb_height_thresh = hvec_vec_pcb_golden_param_out[13].T.Clone();
                    hv_pcb_area_thresh = hvec_vec_pcb_golden_param_out[14].T.Clone();
                    //******************************************** ic golden inspect input
                    //model
                    ho_ic_dark_thresh_image.Dispose();
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        ho_ic_dark_thresh_image = hvec_vec_ic_golden_object[2].O.CopyObj(1, -1);
                    }
                    ho_ic_light_thresh_image.Dispose();
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        ho_ic_light_thresh_image = hvec_vec_ic_golden_object[3].O.CopyObj(1, -1);
                    }
                    ho_ic_match_region.Dispose();
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        ho_ic_match_region = hvec_vec_ic_golden_object[4].O.CopyObj(1, -1);
                    }
                    ho_ic_inspect_region.Dispose();
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        ho_ic_inspect_region = hvec_vec_ic_golden_object[5].O.CopyObj(1, -1);
                    }
                    ho_ic_reject_region.Dispose();
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        ho_ic_reject_region = hvec_vec_ic_golden_object[6].O.CopyObj(1, -1);
                    }
                    ho_ic_sub_region.Dispose();
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        ho_ic_sub_region = hvec_vec_ic_golden_object[7].O.CopyObj(1, -1);
                    }
                    //parameters
                    hv_ic_sub_reg_num = hvec_vec_ic_golden_param_out[0].T.Clone();
                    hv_ic_ModelID = hvec_vec_ic_golden_param_out[4].T.Clone();
                    hv_ic_model_type = hvec_vec_ic_golden_param_out[5].T.Clone();
                    hv_ic_score_thresh = hvec_vec_ic_golden_param_out[6].T.Clone();
                    hv_ic_angle_start = hvec_vec_ic_golden_param_out[7].T.Clone();
                    hv_ic_angle_extent = hvec_vec_ic_golden_param_out[8].T.Clone();
                    hv_ic_search_size = hvec_vec_ic_golden_param_out[9].T.Clone();
                    hv_ic_closing_size = hvec_vec_ic_golden_param_out[10].T.Clone();
                    hv_ic_select_operation = hvec_vec_ic_golden_param_out[11].T.Clone();
                    hv_ic_width_thresh = hvec_vec_ic_golden_param_out[12].T.Clone();
                    hv_ic_height_thresh = hvec_vec_ic_golden_param_out[13].T.Clone();
                    hv_ic_area_thresh = hvec_vec_ic_golden_param_out[14].T.Clone();
                    //******************************************** ic position inspect input
                    hv_row_thresh = hvec_vec_pos_param[0].T.Clone();
                    hv_col_thresh = hvec_vec_pos_param[1].T.Clone();
                    hv_angle_thresh = hvec_vec_pos_param[2].T.Clone();
                    //******************************************** gold line inspect input
                    //model
                    ho_lp_pad.Dispose();
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        ho_lp_pad = hvec_vec_bond_wire_object[0].O.CopyObj(1, -1);
                    }
                    ho_ic_pad.Dispose();
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        ho_ic_pad = hvec_vec_bond_wire_object[1].O.CopyObj(1, -1);
                    }
                    hv_lp_ball_num = hvec_vec_bond_wire_param_out[0].T.Clone();
                    hv_on_ic_ind = hvec_vec_bond_wire_param_out[1].T.Clone();
                    hv_lp_ball_modelID = hvec_vec_bond_wire_param_out[2].T.Clone();
                    hv_lp_ball_model_type = hvec_vec_bond_wire_param_out[3].T.Clone();
                    //params
                    hv_ic_radius_low = hvec_vec_bond_wire_param_out[4].T.Clone();
                    hv_ic_radius_high = hvec_vec_bond_wire_param_out[5].T.Clone();
                    hv_lp_match_thresh = hvec_vec_bond_wire_param_out[6].T.Clone();
                    hv_lp_angle_extent = hvec_vec_bond_wire_param_out[7].T.Clone();
                    hv_line_search_len = hvec_vec_bond_wire_param_out[8].T.Clone();
                    hv_line_clip_len = hvec_vec_bond_wire_param_out[9].T.Clone();
                    hv_line_width = hvec_vec_bond_wire_param_out[10].T.Clone();
                    hv_line_contrast = hvec_vec_bond_wire_param_out[11].T.Clone();
                    hv_line_min_seg_len = hvec_vec_bond_wire_param_out[12].T.Clone();
                    hv_line_angle_extent = hvec_vec_bond_wire_param_out[13].T.Clone();
                    hv_line_max_gap = hvec_vec_bond_wire_param_out[14].T.Clone();
                    //********************************************* epoxy inspect input
                    hv_epoxy_inspect_size = hvec_vec_epoxy_param[0].T.Clone();
                    hv_epoxy_dark_light = hvec_vec_epoxy_param[1].T.Clone();
                    hv_epoxy_edge_sigma = hvec_vec_epoxy_param[2].T.Clone();
                    hv_epoxy_edge_thresh = hvec_vec_epoxy_param[3].T.Clone();
                    hv_epoxy_dist_thresh = hvec_vec_epoxy_param[4].T.Clone();
                    //*********************************************************************************************
                    //*********************************************************************************************
                    //inspection
                    //*********************************************************************************************
                    //*********************************************************************************************
                    HOperatorSet.CountObj(ho_pcb_match_region, out hv_Number);
                    hv_is_mutithread = 1;
                    if ((int)(hv_is_mutithread) != 0)
                    {
                        hvec_vec_inspect_r = (new HTupleVector(1).Insert(0, new HTupleVector(new HTuple())));
                        hvec_vec_inspect_c = (new HTupleVector(1).Insert(0, new HTupleVector(new HTuple())));
                        HTuple end_val93 = hv_Number - 1;
                        HTuple step_val93 = 1;
                        for (hv_Index = 0; hv_Index.Continue(end_val93, step_val93); hv_Index = hv_Index.TupleAdd(step_val93))
                        {
                            hv_block_r = (hv_Index / hv_N) + hv_first_r;
                            hv_block_c = (hv_Index % hv_N) + hv_first_c;
                            if ((int)((new HTuple(hv_block_r.TupleGreaterEqual(hv_R))).TupleOr(new HTuple(hv_block_c.TupleGreaterEqual(
                                hv_C)))) != 0)
                            {
                                continue;
                            }
                            hvec_vec_inspect_r[hv_Index] = new HTupleVector(hv_block_r).Clone();
                            hvec_vec_inspect_c[hv_Index] = new HTupleVector(hv_block_c + (hv_block * hv_C));
                            ho_pcb_match_region_unit.Dispose();
                            HOperatorSet.SelectObj(ho_pcb_match_region, out ho_pcb_match_region_unit,
                                hv_Index + 1);
                            devThread = new HDevThread(context,
                              (HDevThread.ProcCallback)delegate (HDevThread devThreadCB)
                              {
                                  try
                                  {
                    // Input parameters
                    HObject cbho_Image = devThreadCB.GetInputIconicParamObject(0);
                                      HObject cbho_pcb_dark_thresh_image = devThreadCB.GetInputIconicParamObject(1);
                                      HObject cbho_pcb_light_thresh_image = devThreadCB.GetInputIconicParamObject(2);
                                      HObject cbho_pcb_match_region = devThreadCB.GetInputIconicParamObject(3);
                                      HObject cbho_pcb_inspect_region = devThreadCB.GetInputIconicParamObject(4);
                                      HObject cbho_pcb_reject_region = devThreadCB.GetInputIconicParamObject(5);
                                      HObject cbho_pcb_sub_region = devThreadCB.GetInputIconicParamObject(6);
                                      HObject cbho_ic_dark_thresh_image = devThreadCB.GetInputIconicParamObject(7);
                                      HObject cbho_ic_light_thresh_image = devThreadCB.GetInputIconicParamObject(8);
                                      HObject cbho_ic_match_region = devThreadCB.GetInputIconicParamObject(9);
                                      HObject cbho_ic_inspect_region = devThreadCB.GetInputIconicParamObject(10);
                                      HObject cbho_ic_reject_region = devThreadCB.GetInputIconicParamObject(11);
                                      HObject cbho_ic_sub_region = devThreadCB.GetInputIconicParamObject(12);
                                      HObject cbho_pcb_pad = devThreadCB.GetInputIconicParamObject(13);
                                      HObject cbho_ic_pad = devThreadCB.GetInputIconicParamObject(14);
                                      HTuple cbhv_pcb_ModelID = devThreadCB.GetInputCtrlParamTuple(15);
                                      HTuple cbhv_pcb_model_type = devThreadCB.GetInputCtrlParamTuple(16);
                                      HTuple cbhv_pcb_score_thresh = devThreadCB.GetInputCtrlParamTuple(17);
                                      HTuple cbhv_pcb_angle_start = devThreadCB.GetInputCtrlParamTuple(18);
                                      HTuple cbhv_pcb_angle_extent = devThreadCB.GetInputCtrlParamTuple(19);
                                      HTuple cbhv_pcb_sub_reg_num = devThreadCB.GetInputCtrlParamTuple(20);
                                      HTuple cbhv_pcb_select_operation = devThreadCB.GetInputCtrlParamTuple(21);
                                      HTuple cbhv_pcb_width_thresh = devThreadCB.GetInputCtrlParamTuple(22);
                                      HTuple cbhv_pcb_height_thresh = devThreadCB.GetInputCtrlParamTuple(23);
                                      HTuple cbhv_pcb_area_thresh = devThreadCB.GetInputCtrlParamTuple(24);
                                      HTuple cbhv_pcb_closing_size = devThreadCB.GetInputCtrlParamTuple(25);
                                      HTuple cbhv_pcb_search_size = devThreadCB.GetInputCtrlParamTuple(26);
                                      HTuple cbhv_ic_angle_start = devThreadCB.GetInputCtrlParamTuple(27);
                                      HTuple cbhv_ic_ModelID = devThreadCB.GetInputCtrlParamTuple(28);
                                      HTuple cbhv_ic_model_type = devThreadCB.GetInputCtrlParamTuple(29);
                                      HTuple cbhv_ic_score_thresh = devThreadCB.GetInputCtrlParamTuple(30);
                                      HTuple cbhv_ic_angle_extent = devThreadCB.GetInputCtrlParamTuple(31);
                                      HTuple cbhv_ic_sub_reg_num = devThreadCB.GetInputCtrlParamTuple(32);
                                      HTuple cbhv_ic_select_operation = devThreadCB.GetInputCtrlParamTuple(33);
                                      HTuple cbhv_ic_width_thresh = devThreadCB.GetInputCtrlParamTuple(34);
                                      HTuple cbhv_ic_height_thresh = devThreadCB.GetInputCtrlParamTuple(35);
                                      HTuple cbhv_ic_area_thresh = devThreadCB.GetInputCtrlParamTuple(36);
                                      HTuple cbhv_ic_closing_size = devThreadCB.GetInputCtrlParamTuple(37);
                                      HTuple cbhv_ic_search_size = devThreadCB.GetInputCtrlParamTuple(38);
                                      HTuple cbhv_pos_row_thresh = devThreadCB.GetInputCtrlParamTuple(39);
                                      HTuple cbhv_pos_col_thresh = devThreadCB.GetInputCtrlParamTuple(40);
                                      HTuple cbhv_pos_angle_thresh = devThreadCB.GetInputCtrlParamTuple(41);
                                      HTuple cbhv_lp_ball_num = devThreadCB.GetInputCtrlParamTuple(42);
                                      HTuple cbhv_on_ic_ind = devThreadCB.GetInputCtrlParamTuple(43);
                                      HTuple cbhv_ic_radius_low = devThreadCB.GetInputCtrlParamTuple(44);
                                      HTuple cbhv_ic_radius_high = devThreadCB.GetInputCtrlParamTuple(45);
                                      HTuple cbhv_lp_ball_modelID = devThreadCB.GetInputCtrlParamTuple(46);
                                      HTuple cbhv_lp_ball_model_type = devThreadCB.GetInputCtrlParamTuple(47);
                                      HTuple cbhv_lp_match_thresh = devThreadCB.GetInputCtrlParamTuple(48);
                                      HTuple cbhv_lp_angle_extent = devThreadCB.GetInputCtrlParamTuple(49);
                                      HTuple cbhv_line_search_len = devThreadCB.GetInputCtrlParamTuple(50);
                                      HTuple cbhv_line_clip_len = devThreadCB.GetInputCtrlParamTuple(51);
                                      HTuple cbhv_line_width = devThreadCB.GetInputCtrlParamTuple(52);
                                      HTuple cbhv_line_contrast = devThreadCB.GetInputCtrlParamTuple(53);
                                      HTuple cbhv_line_min_seg_len = devThreadCB.GetInputCtrlParamTuple(54);
                                      HTuple cbhv_line_angle_extent = devThreadCB.GetInputCtrlParamTuple(55);
                                      HTuple cbhv_line_max_gap = devThreadCB.GetInputCtrlParamTuple(56);
                                      HTuple cbhv_epoxy_inspect_size = devThreadCB.GetInputCtrlParamTuple(57);
                                      HTuple cbhv_epoxy_dark_light = devThreadCB.GetInputCtrlParamTuple(58);
                                      HTuple cbhv_epoxy_edge_sigma = devThreadCB.GetInputCtrlParamTuple(59);
                                      HTuple cbhv_epoxy_edge_thresh = devThreadCB.GetInputCtrlParamTuple(60);
                                      HTuple cbhv_epoxy_dist_thresh = devThreadCB.GetInputCtrlParamTuple(61);

                    // Output parameters
                    HObject cbho_defect_region;
                                      HObject cbho_gold_wire;
                                      HTuple cbhv_iFlag;
                                      HTuple cbhv_err_msg;
                                      HTuple cbhv_defect_type;

                    // Call JSLF_AOI_inspect_unit
                    JSLF_AOI_inspect_unit(cbho_Image, cbho_pcb_dark_thresh_image, cbho_pcb_light_thresh_image,
                                  cbho_pcb_match_region, cbho_pcb_inspect_region, cbho_pcb_reject_region,
                                  cbho_pcb_sub_region, cbho_ic_dark_thresh_image, cbho_ic_light_thresh_image,
                                  cbho_ic_match_region, cbho_ic_inspect_region, cbho_ic_reject_region,
                                  cbho_ic_sub_region, cbho_pcb_pad, cbho_ic_pad, out cbho_defect_region,
                                  out cbho_gold_wire, cbhv_pcb_ModelID, cbhv_pcb_model_type,
                                  cbhv_pcb_score_thresh, cbhv_pcb_angle_start, cbhv_pcb_angle_extent,
                                  cbhv_pcb_sub_reg_num, cbhv_pcb_select_operation, cbhv_pcb_width_thresh,
                                  cbhv_pcb_height_thresh, cbhv_pcb_area_thresh, cbhv_pcb_closing_size,
                                  cbhv_pcb_search_size, cbhv_ic_angle_start, cbhv_ic_ModelID,
                                  cbhv_ic_model_type, cbhv_ic_score_thresh, cbhv_ic_angle_extent,
                                  cbhv_ic_sub_reg_num, cbhv_ic_select_operation, cbhv_ic_width_thresh,
                                  cbhv_ic_height_thresh, cbhv_ic_area_thresh, cbhv_ic_closing_size,
                                  cbhv_ic_search_size, cbhv_pos_row_thresh, cbhv_pos_col_thresh,
                                  cbhv_pos_angle_thresh, cbhv_lp_ball_num, cbhv_on_ic_ind,
                                  cbhv_ic_radius_low, cbhv_ic_radius_high, cbhv_lp_ball_modelID,
                                  cbhv_lp_ball_model_type, cbhv_lp_match_thresh, cbhv_lp_angle_extent,
                                  cbhv_line_search_len, cbhv_line_clip_len, cbhv_line_width,
                                  cbhv_line_contrast, cbhv_line_min_seg_len, cbhv_line_angle_extent,
                                  cbhv_line_max_gap, cbhv_epoxy_inspect_size, cbhv_epoxy_dark_light,
                                  cbhv_epoxy_edge_sigma, cbhv_epoxy_edge_thresh, cbhv_epoxy_dist_thresh,
                                  out cbhv_iFlag, out cbhv_err_msg, out cbhv_defect_type);

                    // Store output parameters in thread object
                    devThreadCB.StoreOutputIconicParamObject(0, cbho_defect_region);
                                      devThreadCB.StoreOutputIconicParamObject(1, cbho_gold_wire);
                                      devThreadCB.StoreOutputCtrlParamTuple(2, cbhv_iFlag);
                                      devThreadCB.StoreOutputCtrlParamTuple(3, cbhv_err_msg);
                                      devThreadCB.StoreOutputCtrlParamTuple(4, cbhv_defect_type);

                    // Reduce reference counter of thread object
                    devThreadCB.Exit();
                                      devThreadCB.Dispose();

                                  }
                                  catch (HalconException exc)
                                  {
                    // No exceptions may be raised from stub in parallel case,
                    // so we need to store this information prior to cleanup
                    bool is_direct_call = devThreadCB.IsDirectCall();
                    // Attempt to clean up in error case, too
                    devThreadCB.Exit();
                                      devThreadCB.Dispose();
                    // Propagate exception if called directly
                    if (is_direct_call)
                                          throw exc;
                                  }
                              }, 62, 5);
                            // Set thread procedure call arguments 
                            devThread.SetInputIconicParamObject(0, ho_Image);
                            devThread.SetInputIconicParamObject(1, ho_pcb_dark_thresh_image);
                            devThread.SetInputIconicParamObject(2, ho_pcb_light_thresh_image);
                            devThread.SetInputIconicParamObject(3, ho_pcb_match_region_unit);
                            devThread.SetInputIconicParamObject(4, ho_pcb_inspect_region);
                            devThread.SetInputIconicParamObject(5, ho_pcb_reject_region);
                            devThread.SetInputIconicParamObject(6, ho_pcb_sub_region);
                            devThread.SetInputIconicParamObject(7, ho_ic_dark_thresh_image);
                            devThread.SetInputIconicParamObject(8, ho_ic_light_thresh_image);
                            devThread.SetInputIconicParamObject(9, ho_ic_match_region);
                            devThread.SetInputIconicParamObject(10, ho_ic_inspect_region);
                            devThread.SetInputIconicParamObject(11, ho_ic_reject_region);
                            devThread.SetInputIconicParamObject(12, ho_ic_sub_region);
                            devThread.SetInputIconicParamObject(13, ho_lp_pad);
                            devThread.SetInputIconicParamObject(14, ho_ic_pad);
                            devThread.SetInputCtrlParamTuple(15, hv_pcb_ModelID);
                            devThread.SetInputCtrlParamTuple(16, hv_pcb_model_type);
                            devThread.SetInputCtrlParamTuple(17, hv_pcb_score_thresh);
                            devThread.SetInputCtrlParamTuple(18, hv_pcb_angle_start);
                            devThread.SetInputCtrlParamTuple(19, hv_pcb_angle_extent);
                            devThread.SetInputCtrlParamTuple(20, hv_pcb_sub_reg_num);
                            devThread.SetInputCtrlParamTuple(21, hv_pcb_select_operation);
                            devThread.SetInputCtrlParamTuple(22, hv_pcb_width_thresh);
                            devThread.SetInputCtrlParamTuple(23, hv_pcb_height_thresh);
                            devThread.SetInputCtrlParamTuple(24, hv_pcb_area_thresh);
                            devThread.SetInputCtrlParamTuple(25, hv_pcb_closing_size);
                            devThread.SetInputCtrlParamTuple(26, hv_pcb_search_size);
                            devThread.SetInputCtrlParamTuple(27, hv_ic_angle_start);
                            devThread.SetInputCtrlParamTuple(28, hv_ic_ModelID);
                            devThread.SetInputCtrlParamTuple(29, hv_ic_model_type);
                            devThread.SetInputCtrlParamTuple(30, hv_ic_score_thresh);
                            devThread.SetInputCtrlParamTuple(31, hv_ic_angle_extent);
                            devThread.SetInputCtrlParamTuple(32, hv_ic_sub_reg_num);
                            devThread.SetInputCtrlParamTuple(33, hv_ic_select_operation);
                            devThread.SetInputCtrlParamTuple(34, hv_ic_width_thresh);
                            devThread.SetInputCtrlParamTuple(35, hv_ic_height_thresh);
                            devThread.SetInputCtrlParamTuple(36, hv_ic_area_thresh);
                            devThread.SetInputCtrlParamTuple(37, hv_ic_closing_size);
                            devThread.SetInputCtrlParamTuple(38, hv_ic_search_size);
                            devThread.SetInputCtrlParamTuple(39, hv_row_thresh);
                            devThread.SetInputCtrlParamTuple(40, hv_col_thresh);
                            devThread.SetInputCtrlParamTuple(41, hv_angle_thresh);
                            devThread.SetInputCtrlParamTuple(42, hv_lp_ball_num);
                            devThread.SetInputCtrlParamTuple(43, hv_on_ic_ind);
                            devThread.SetInputCtrlParamTuple(44, hv_ic_radius_low);
                            devThread.SetInputCtrlParamTuple(45, hv_ic_radius_high);
                            devThread.SetInputCtrlParamTuple(46, hv_lp_ball_modelID);
                            devThread.SetInputCtrlParamTuple(47, hv_lp_ball_model_type);
                            devThread.SetInputCtrlParamTuple(48, hv_lp_match_thresh);
                            devThread.SetInputCtrlParamTuple(49, hv_lp_angle_extent);
                            devThread.SetInputCtrlParamTuple(50, hv_line_search_len);
                            devThread.SetInputCtrlParamTuple(51, hv_line_clip_len);
                            devThread.SetInputCtrlParamTuple(52, hv_line_width);
                            devThread.SetInputCtrlParamTuple(53, hv_line_contrast);
                            devThread.SetInputCtrlParamTuple(54, hv_line_min_seg_len);
                            devThread.SetInputCtrlParamTuple(55, hv_line_angle_extent);
                            devThread.SetInputCtrlParamTuple(56, hv_line_max_gap);
                            devThread.SetInputCtrlParamTuple(57, hv_epoxy_inspect_size);
                            devThread.SetInputCtrlParamTuple(58, hv_epoxy_dark_light);
                            devThread.SetInputCtrlParamTuple(59, hv_epoxy_edge_sigma);
                            devThread.SetInputCtrlParamTuple(60, hv_epoxy_edge_thresh);
                            devThread.SetInputCtrlParamTuple(61, hv_epoxy_dist_thresh);
                            {
                                HTuple at_idx = new HTuple();
                                at_idx[0] = hv_Index;
                                devThread.BindOutputIconicParamVector(0, false, hvec_vec_defect_region, at_idx);
                            }
                            {
                                HTuple at_idx = new HTuple();
                                at_idx[0] = hv_Index;
                                devThread.BindOutputIconicParamVector(1, false, hvec_vec_bond_wire, at_idx);
                            }
                            {
                                HTuple at_idx = new HTuple();
                                at_idx[0] = hv_Index;
                                devThread.BindOutputCtrlParamVector(2, false, hvec_vec_iFlag, at_idx);
                            }
                            {
                                HTuple at_idx = new HTuple();
                                at_idx[0] = hv_Index;
                                devThread.BindOutputCtrlParamVector(3, false, hvec_vec_err_msg, at_idx);
                            }
                            {
                                HTuple at_idx = new HTuple();
                                at_idx[0] = hv_Index;
                                devThread.BindOutputCtrlParamVector(4, false, hvec_vec_defect_type, at_idx);
                            }

                            // Start proc line in thread
                            {
                                HTuple TmpThreadId;
                                devThread.ParStart(out TmpThreadId);
                                hvec_VThreads[hv_Index].T = TmpThreadId;
                            }

                        }
                        hv_Threads = hvec_VThreads.ConvertVectorToTuple();
                        HDevThread.ParJoin(hv_Threads);
                        hv_iFlags = hvec_vec_iFlag.ConvertVectorToTuple();
                        hv_defect_type = hvec_vec_defect_type.ConvertVectorToTuple();
                        hv_thread_num = new HTuple(hvec_VThreads.Length);
                        HTuple end_val109 = hv_thread_num - 1;
                        HTuple step_val109 = 1;
                        for (hv_I = 0; hv_I.Continue(end_val109, step_val109); hv_I = hv_I.TupleAdd(step_val109))
                        {
                            if ((int)(new HTuple((hvec_VThreads[hv_I].T).TupleNotEqual(new HTuple()))) != 0)
                            {
                                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                                {
                                    HObject ExpTmpOutVar_0;
                                    HOperatorSet.ConcatObj(ho_defect_regions, hvec_vec_defect_region[hv_I].O,
                                        out ExpTmpOutVar_0);
                                    ho_defect_regions.Dispose();
                                    ho_defect_regions = ExpTmpOutVar_0;
                                }
                                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                                {
                                    HObject ExpTmpOutVar_0;
                                    HOperatorSet.ConcatObj(ho_bond_wires, hvec_vec_bond_wire[hv_I].O, out ExpTmpOutVar_0
                                        );
                                    ho_bond_wires.Dispose();
                                    ho_bond_wires = ExpTmpOutVar_0;
                                }
                                if ((int)(new HTuple((hvec_vec_iFlag[hv_I].T).TupleNotEqual(0))) != 0)
                                {
                                    hv_defect_r = hv_defect_r.TupleConcat(hvec_vec_inspect_r[hv_I].T);
                                    hv_defect_c = hv_defect_c.TupleConcat(hvec_vec_inspect_c[hv_I].T);
                                }
                            }
                        }
                    }
                    else
                    {
                        HTuple end_val120 = hv_Number - 1;
                        HTuple step_val120 = 1;
                        for (hv_Index = 0; hv_Index.Continue(end_val120, step_val120); hv_Index = hv_Index.TupleAdd(step_val120))
                        {
                            hv_block_r = (hv_Index / hv_N) + hv_first_r;
                            hv_block_c = (hv_Index % hv_N) + hv_first_c;
                            if ((int)((new HTuple(hv_block_r.TupleGreaterEqual(hv_R))).TupleOr(new HTuple(hv_block_c.TupleGreaterEqual(
                                hv_C)))) != 0)
                            {
                                continue;
                            }
                            ho_pcb_match_region_unit.Dispose();
                            HOperatorSet.SelectObj(ho_pcb_match_region, out ho_pcb_match_region_unit,
                                hv_Index + 1);
                            ho_defect_region_unit.Dispose(); ho_bond_wire_unit.Dispose();
                            JSLF_AOI_inspect_unit(ho_Image, ho_pcb_dark_thresh_image, ho_pcb_light_thresh_image,
                                ho_pcb_match_region_unit, ho_pcb_inspect_region, ho_pcb_reject_region,
                                ho_pcb_sub_region, ho_ic_dark_thresh_image, ho_ic_light_thresh_image,
                                ho_ic_match_region, ho_ic_inspect_region, ho_ic_reject_region, ho_ic_sub_region,
                                ho_lp_pad, ho_ic_pad, out ho_defect_region_unit, out ho_bond_wire_unit,
                                hv_pcb_ModelID, hv_pcb_model_type, hv_pcb_score_thresh, hv_pcb_angle_start,
                                hv_pcb_angle_extent, hv_pcb_sub_reg_num, hv_pcb_select_operation, hv_pcb_width_thresh,
                                hv_pcb_height_thresh, hv_pcb_area_thresh, hv_pcb_closing_size, hv_pcb_search_size,
                                hv_ic_angle_start, hv_ic_ModelID, hv_ic_model_type, hv_ic_score_thresh,
                                hv_ic_angle_extent, hv_ic_sub_reg_num, hv_ic_select_operation, hv_ic_width_thresh,
                                hv_ic_height_thresh, hv_ic_area_thresh, hv_ic_closing_size, hv_ic_search_size,
                                hv_row_thresh, hv_col_thresh, hv_angle_thresh, hv_lp_ball_num, hv_on_ic_ind,
                                hv_ic_radius_low, hv_ic_radius_high, hv_lp_ball_modelID, hv_lp_ball_model_type,
                                hv_lp_match_thresh, hv_lp_angle_extent, hv_line_search_len, hv_line_clip_len,
                                hv_line_width, hv_line_contrast, hv_line_min_seg_len, hv_line_angle_extent,
                                hv_line_max_gap, hv_epoxy_inspect_size, hv_epoxy_dark_light, hv_epoxy_edge_sigma,
                                hv_epoxy_edge_thresh, hv_epoxy_dist_thresh, out hv_iFlag_unit, out hv_err_msg_unit,
                                out hv_defect_type_unit);
                            {
                                HObject ExpTmpOutVar_0;
                                HOperatorSet.ConcatObj(ho_defect_regions, ho_defect_region_unit, out ExpTmpOutVar_0
                                    );
                                ho_defect_regions.Dispose();
                                ho_defect_regions = ExpTmpOutVar_0;
                            }
                            {
                                HObject ExpTmpOutVar_0;
                                HOperatorSet.ConcatObj(ho_bond_wires, ho_bond_wire_unit, out ExpTmpOutVar_0
                                    );
                                ho_bond_wires.Dispose();
                                ho_bond_wires = ExpTmpOutVar_0;
                            }
                            hv_iFlags = hv_iFlags.TupleConcat(hv_iFlag_unit);
                            if ((int)(new HTuple(hv_iFlag_unit.TupleNotEqual(0))) != 0)
                            {
                                hv_defect_r = hv_defect_r.TupleConcat(hv_block_r);
                                hv_defect_c = hv_defect_c.TupleConcat(hv_block_c + (hv_block * hv_C));
                                hv_defect_type = hv_defect_type.TupleConcat(hv_defect_type_unit);
                            }
                        }
                    }
                    HOperatorSet.TupleFindFirst(hv_iFlags, -1, out hv_Indices);
                    if ((int)(new HTuple(hv_Indices.TupleGreaterEqual(0))) != 0)
                    {
                        hv_iFlag = -1;
                    }
                    ho_pcb_dark_thresh_image.Dispose();
                    ho_pcb_light_thresh_image.Dispose();
                    ho_pcb_match_region.Dispose();
                    ho_pcb_inspect_region.Dispose();
                    ho_pcb_reject_region.Dispose();
                    ho_pcb_sub_region.Dispose();
                    ho_ic_dark_thresh_image.Dispose();
                    ho_ic_light_thresh_image.Dispose();
                    ho_ic_match_region.Dispose();
                    ho_ic_inspect_region.Dispose();
                    ho_ic_reject_region.Dispose();
                    ho_ic_sub_region.Dispose();
                    ho_lp_pad.Dispose();
                    ho_ic_pad.Dispose();
                    ho_pcb_match_region_unit.Dispose();
                    ho_defect_region_unit.Dispose();
                    ho_bond_wire_unit.Dispose();
                    hvec_vec_defect_region.Dispose();
                    hvec_vec_bond_wire.Dispose();

                    return;
                }
                catch (HalconException HDevExpDefaultException)
                {
                    ho_pcb_dark_thresh_image.Dispose();
                    ho_pcb_light_thresh_image.Dispose();
                    ho_pcb_match_region.Dispose();
                    ho_pcb_inspect_region.Dispose();
                    ho_pcb_reject_region.Dispose();
                    ho_pcb_sub_region.Dispose();
                    ho_ic_dark_thresh_image.Dispose();
                    ho_ic_light_thresh_image.Dispose();
                    ho_ic_match_region.Dispose();
                    ho_ic_inspect_region.Dispose();
                    ho_ic_reject_region.Dispose();
                    ho_ic_sub_region.Dispose();
                    ho_lp_pad.Dispose();
                    ho_ic_pad.Dispose();
                    ho_pcb_match_region_unit.Dispose();
                    ho_defect_region_unit.Dispose();
                    ho_bond_wire_unit.Dispose();
                    hvec_vec_defect_region.Dispose();
                    hvec_vec_bond_wire.Dispose();

                    throw HDevExpDefaultException;
                }
            }
        }

        public void JSLF_AOI_load_all_model(out HTuple hv_iFlag,
      out HTuple hv_err_msg)
        {



            // Local iconic variables 

            HObject ho_EmptyObject, ho_pcb_mean_image;
            HObject ho_pcb_std_image, ho_pcb_match_region, ho_pcb_inspect_region;
            HObject ho_pcb_reject_region, ho_pcb_sub_region, ho_pcb_dark_thresh_image;
            HObject ho_pcb_light_thresh_image, ho_ic_mean_image, ho_ic_std_image;
            HObject ho_ic_match_region, ho_ic_inspect_region, ho_ic_reject_region;
            HObject ho_ic_sub_region, ho_ic_dark_thresh_image, ho_ic_light_thresh_image;
            HObject ho_pcb_pad, ho_ic_pad, ho_show_contour;

            // Local control variables 

            HTuple hv_pcb_sub_reg_num = null, hv_pcb_sobel_scale = null;
            HTuple hv_pcb_dark_thresh = null, hv_pcb_light_thresh = null;
            HTuple hv_pcb_model_path = null, hv_pcb_ModelID = null;
            HTuple hv_pcb_model_type = null, hv_pcb_iFlag1 = null;
            HTuple hv_ic_sub_reg_num = null, hv_ic_sobel_scale = null;
            HTuple hv_ic_dark_thresh = null, hv_ic_light_thresh = null;
            HTuple hv_ic_model_path = null, hv_ic_ModelID = null, hv_ic_model_type = null;
            HTuple hv_ic_iFlag1 = null, hv_wire_model_path = null;
            HTuple hv_ball_num_tup = null, hv_on_ic_ind = null, hv_pcb_Number = null;
            HTuple hv_ic_Number = null, hv_model_type = null, hv_model_id = null;
            HTuple hv_def_row = null, hv_def_col = null, hv_iFlag1 = null;
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
            HOperatorSet.GenEmptyObj(out ho_show_contour);
            hvec_vec_pcb_golden_object = new HObjectVector(1);
            hvec_vec_ic_golden_object = new HObjectVector(1);
            hvec_vec_bond_wire_object = new HObjectVector(1);
            hvec_vec_ic_golden_param_out = new HTupleVector(1);
            hvec_vec_bond_wire_param_out = new HTupleVector(1);
            try
            {
                //*******************************************************************************
                //0:pcb golden model, 1:ic golden model, 2:pcb pad, 3:ic pad, 4:chipping, 5: scratch, 6: match
                //golden model [dark, light, match, inspect, reject, sub]
                //*******************************************************************************
             
                hv_iFlag = 0;
                hv_err_msg = "";
                ho_EmptyObject.Dispose();
                HOperatorSet.GenEmptyObj(out ho_EmptyObject);
                //pcb golden model
                hv_pcb_sub_reg_num = hvec_vec_pcb_golden_param[0].T.Clone();
                hv_pcb_sobel_scale = hvec_vec_pcb_golden_param[1].T.Clone();
                hv_pcb_dark_thresh = hvec_vec_pcb_golden_param[2].T.Clone();
                hv_pcb_light_thresh = hvec_vec_pcb_golden_param[3].T.Clone();
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    hvec_vec_pcb_golden_object = dh.Take((
                        dh.Add(new HObjectVector(1)).Insert(0, dh.Add(new HObjectVector(ho_EmptyObject)))));
                }
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    hvec_vec_pcb_golden_object.Clear();
                }
                hvec_vec_pcb_golden_param_out = hvec_vec_pcb_golden_param.Clone();
                hv_pcb_model_path = hv_model_path + "/golden_lp";
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
                    ho_show_contour.Dispose();

                    return;
                }
                hvec_vec_pcb_golden_object[0] = new HObjectVector(ho_pcb_mean_image.CopyObj(1, -1));
                hvec_vec_pcb_golden_object[1] = new HObjectVector(ho_pcb_std_image.CopyObj(1, -1));
                hvec_vec_pcb_golden_object[2] = new HObjectVector(ho_pcb_dark_thresh_image.CopyObj(1, -1));
                hvec_vec_pcb_golden_object[3] = new HObjectVector(ho_pcb_light_thresh_image.CopyObj(1, -1));
                hvec_vec_pcb_golden_object[4] = new HObjectVector(ho_pcb_match_region.CopyObj(1, -1));
                hvec_vec_pcb_golden_object[5] = new HObjectVector(ho_pcb_inspect_region.CopyObj(1, -1));
                hvec_vec_pcb_golden_object[6] = new HObjectVector(ho_pcb_reject_region.CopyObj(1, -1));
                hvec_vec_pcb_golden_object[7] = new HObjectVector(ho_pcb_sub_region.CopyObj(1, -1));
                hvec_vec_pcb_golden_param_out[4] = new HTupleVector(hv_pcb_ModelID).Clone();
                hvec_vec_pcb_golden_param_out[5] = new HTupleVector(hv_pcb_model_type).Clone();
                //ic golden model
                hv_ic_sub_reg_num = hvec_vec_ic_golden_param[0].T.Clone();
                hv_ic_sobel_scale = hvec_vec_ic_golden_param[1].T.Clone();
                hv_ic_dark_thresh = hvec_vec_ic_golden_param[2].T.Clone();
                hv_ic_light_thresh = hvec_vec_ic_golden_param[3].T.Clone();
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    hvec_vec_ic_golden_object = dh.Take((
                        dh.Add(new HObjectVector(1)).Insert(0, dh.Add(new HObjectVector(ho_EmptyObject)))));
                }
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    hvec_vec_ic_golden_object.Clear();
                }
                hvec_vec_ic_golden_param_out = hvec_vec_ic_golden_param.Clone();
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
                    ho_show_contour.Dispose();

                    return;
                }
                hvec_vec_ic_golden_object[0] = new HObjectVector(ho_ic_mean_image.CopyObj(1, -1));
                hvec_vec_ic_golden_object[1] = new HObjectVector(ho_ic_std_image.CopyObj(1, -1));
                hvec_vec_ic_golden_object[2] = new HObjectVector(ho_ic_dark_thresh_image.CopyObj(1, -1));
                hvec_vec_ic_golden_object[3] = new HObjectVector(ho_ic_light_thresh_image.CopyObj(1, -1));
                hvec_vec_ic_golden_object[4] = new HObjectVector(ho_ic_match_region.CopyObj(1, -1));
                hvec_vec_ic_golden_object[5] = new HObjectVector(ho_ic_inspect_region.CopyObj(1, -1));
                hvec_vec_ic_golden_object[6] = new HObjectVector(ho_ic_reject_region.CopyObj(1, -1));
                hvec_vec_ic_golden_object[7] = new HObjectVector(ho_ic_sub_region.CopyObj(1, -1));
                hvec_vec_ic_golden_param_out[4] = new HTupleVector(hv_ic_ModelID).Clone();
                hvec_vec_ic_golden_param_out[5] = new HTupleVector(hv_ic_model_type).Clone();
                //bond wire
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    hvec_vec_bond_wire_object = dh.Take((
                        dh.Add(new HObjectVector(1)).Insert(0, dh.Add(new HObjectVector(ho_EmptyObject)))));
                }
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    hvec_vec_bond_wire_object.Clear();
                }
                hvec_vec_bond_wire_param_out = hvec_vec_bond_wire_param.Clone();
                hv_wire_model_path = hv_model_path + "/gold_line";
                ho_pcb_pad.Dispose();
                HOperatorSet.ReadRegion(out ho_pcb_pad, hv_wire_model_path + "/lp_pad_region.reg");
                ho_ic_pad.Dispose();
                HOperatorSet.ReadRegion(out ho_ic_pad, hv_wire_model_path + "/ic_pad_region.reg");
                HOperatorSet.ReadTuple(hv_wire_model_path + "/ball_num_list.tup", out hv_ball_num_tup);
                HOperatorSet.ReadTuple(hv_wire_model_path + "/on_ic_ind.tup", out hv_on_ic_ind);
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
                    ho_show_contour.Dispose();

                    return;
                }
                ho_show_contour.Dispose();
                read_model(out ho_show_contour, hv_wire_model_path, out hv_model_type, out hv_model_id,
                    out hv_def_row, out hv_def_col, out hv_iFlag1);
                if ((int)(new HTuple(hv_iFlag1.TupleNotEqual(0))) != 0)
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
                    ho_show_contour.Dispose();

                    return;
                }
                hvec_vec_bond_wire_object[0] = new HObjectVector(ho_pcb_pad.CopyObj(1, -1));
                hvec_vec_bond_wire_object[1] = new HObjectVector(ho_ic_pad.CopyObj(1, -1));
                hvec_vec_bond_wire_param_out[0] = new HTupleVector(hv_ball_num_tup).Clone();
                hvec_vec_bond_wire_param_out[1] = new HTupleVector(hv_on_ic_ind).Clone();
                hvec_vec_bond_wire_param_out[2] = new HTupleVector(hv_model_id).Clone();
                hvec_vec_bond_wire_param_out[3] = new HTupleVector(hv_model_type).Clone();
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
                ho_show_contour.Dispose();

                throw HDevExpDefaultException;
            }
        }

        public void JSLF_AOI_update_thresh_image(HObjectVector/*{eObjectVector,Dim=1}*/ hvec_vec_model_object,
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
            hv_iFlag = -1;

            hv_RowDiff = ((hv_Row1 - hv_Row2)).TupleAbs();
            hv_ColDiff = ((hv_Col1 - hv_Col2)).TupleAbs();
            hv_AngleDiff = ((hv_Angle1 - hv_Angle2)).TupleAbs();
            if ((int)(new HTuple(hv_AngleDiff.TupleGreater((new HTuple(180)).TupleRad()))) != 0)
            {
                hv_AngleDiff = ((new HTuple(360)).TupleRad()) - hv_AngleDiff;
            }

            if ((int)((new HTuple((new HTuple(hv_RowDiff.TupleLess(hv_RowThr))).TupleAnd(
                new HTuple(hv_ColDiff.TupleLess(hv_ColThr))))).TupleAnd(new HTuple(hv_AngleDiff.TupleLess(
                hv_AngleThr)))) != 0)
            {
                hv_iFlag = 0;
            }

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

        public void epoxy_distance_inspect(HObject ho_Image, HObject ho_ICRegion, out HObject ho_DefectRegion,
            HTuple hv_EpoxyInspectSize, HTuple hv_EpoxyEdgeThresh, HTuple hv_EpoxyEdgeSigma,
            HTuple hv_EpoxyDarkLight, HTuple hv_EpoxyDistThresh, out HTuple hv_EpoxyRow,
            out HTuple hv_EpoxyCol, out HTuple hv_iFlag)
        {




            // Local iconic variables 

            // Local control variables 

            HTuple hv_Width = null, hv_Height = null, hv_Row = null;
            HTuple hv_Column = null, hv_Phi = null, hv_Length1 = null;
            HTuple hv_Length2 = null, hv_MeasureHandle = null, hv_RowEdge = null;
            HTuple hv_ColumnEdge = null, hv_Amplitude = null, hv_Distance = null;
            HTuple hv_Greater = null, hv_FirstIndex = new HTuple();
            HTuple hv_LastIndex = new HTuple(), hv_RowTuple = null;
            HTuple hv_ColTuple = null, hv_Distance1 = null, hv_Length = null;
            HTuple hv_EpoxyDist = null, hv_GreaterEpoxy = null, hv_Indices = null;
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_DefectRegion);
            hv_iFlag = 0;
            ho_DefectRegion.Dispose();
            HOperatorSet.GenEmptyObj(out ho_DefectRegion);
            hv_EpoxyRow = new HTuple();
            hv_EpoxyCol = new HTuple();
            //*****************************************
            HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height);
            HOperatorSet.SmallestRectangle2(ho_ICRegion, out hv_Row, out hv_Column, out hv_Phi,
                out hv_Length1, out hv_Length2);
            //gen_rectangle2 (Rectangle, Row, Column, Phi, Length1+EpoxyInspectSize, Length2)
            HOperatorSet.GenMeasureRectangle2(hv_Row, hv_Column, hv_Phi, hv_Length1 + hv_EpoxyInspectSize,
                hv_Length2, hv_Width, hv_Height, "nearest_neighbor", out hv_MeasureHandle);
            HOperatorSet.MeasurePos(ho_Image, hv_MeasureHandle, hv_EpoxyEdgeSigma, hv_EpoxyEdgeThresh,
                "all", "all", out hv_RowEdge, out hv_ColumnEdge, out hv_Amplitude, out hv_Distance);
            HOperatorSet.CloseMeasure(hv_MeasureHandle);
            HOperatorSet.TupleGreaterElem(hv_Amplitude, 0, out hv_Greater);
            if ((int)(hv_EpoxyDarkLight) != 0)
            {
                HOperatorSet.TupleFindFirst(hv_Greater, 1, out hv_FirstIndex);
                HOperatorSet.TupleFindLast(hv_Greater, 0, out hv_LastIndex);
            }
            else
            {
                HOperatorSet.TupleFindFirst(hv_Greater, 0, out hv_FirstIndex);
                HOperatorSet.TupleFindLast(hv_Greater, 1, out hv_LastIndex);
            }
            if ((int)((new HTuple(hv_FirstIndex.TupleEqual(-1))).TupleOr(new HTuple(hv_LastIndex.TupleEqual(
                -1)))) != 0)
            {
                ho_DefectRegion.Dispose();
                HOperatorSet.DilationRectangle1(ho_ICRegion, out ho_DefectRegion, hv_EpoxyInspectSize * 2,
                    hv_EpoxyInspectSize * 2);
                hv_iFlag = -1;

                return;
            }
            hv_EpoxyRow = hv_EpoxyRow.TupleConcat(hv_RowEdge.TupleSelect(hv_FirstIndex.TupleConcat(
                hv_LastIndex)));
            hv_EpoxyCol = hv_EpoxyCol.TupleConcat(hv_ColumnEdge.TupleSelect(hv_FirstIndex.TupleConcat(
                hv_LastIndex)));
            //gen_cross_contour_xld (Cross, RowEdge[[first_index,lat_index]], ColumnEdge[[first_index,lat_index]], 6, 0)
            //gen_rectangle2 (Rectangle, Row, Column, Phi+rad(90), Length2+EpoxyInspectSize, Length1)
            HOperatorSet.GenMeasureRectangle2(hv_Row, hv_Column, hv_Phi + 1.5708, hv_Length2 + hv_EpoxyInspectSize,
                hv_Length1, hv_Width, hv_Height, "nearest_neighbor", out hv_MeasureHandle);
            HOperatorSet.MeasurePos(ho_Image, hv_MeasureHandle, hv_EpoxyEdgeSigma, hv_EpoxyEdgeThresh,
                "all", "all", out hv_RowEdge, out hv_ColumnEdge, out hv_Amplitude, out hv_Distance);
            HOperatorSet.CloseMeasure(hv_MeasureHandle);
            HOperatorSet.TupleGreaterElem(hv_Amplitude, 0, out hv_Greater);
            if ((int)(hv_EpoxyDarkLight) != 0)
            {
                HOperatorSet.TupleFindFirst(hv_Greater, 1, out hv_FirstIndex);
                HOperatorSet.TupleFindLast(hv_Greater, 0, out hv_LastIndex);
            }
            else
            {
                HOperatorSet.TupleFindFirst(hv_Greater, 1, out hv_FirstIndex);
                HOperatorSet.TupleFindLast(hv_Greater, 0, out hv_LastIndex);
            }
            if ((int)((new HTuple(hv_FirstIndex.TupleEqual(-1))).TupleOr(new HTuple(hv_LastIndex.TupleEqual(
                -1)))) != 0)
            {
                ho_DefectRegion.Dispose();
                HOperatorSet.DilationRectangle1(ho_ICRegion, out ho_DefectRegion, hv_EpoxyInspectSize * 2,
                    hv_EpoxyInspectSize * 2);
                hv_iFlag = -1;

                return;
            }
            hv_EpoxyRow = hv_EpoxyRow.TupleConcat(hv_RowEdge.TupleSelect(hv_FirstIndex.TupleConcat(
                hv_LastIndex)));
            hv_EpoxyCol = hv_EpoxyCol.TupleConcat(hv_ColumnEdge.TupleSelect(hv_FirstIndex.TupleConcat(
                hv_LastIndex)));
            HOperatorSet.TupleGenConst(4, hv_Row, out hv_RowTuple);
            HOperatorSet.TupleGenConst(4, hv_Column, out hv_ColTuple);
            HOperatorSet.DistancePp(hv_EpoxyRow, hv_EpoxyCol, hv_RowTuple, hv_ColTuple, out hv_Distance1);
            hv_Length = new HTuple();
            hv_Length = hv_Length.TupleConcat(hv_Length2);
            hv_Length = hv_Length.TupleConcat(hv_Length2);
            hv_Length = hv_Length.TupleConcat(hv_Length1);
            hv_Length = hv_Length.TupleConcat(hv_Length1);
            hv_EpoxyDist = hv_Distance1 - hv_Length;
            HOperatorSet.TupleGreaterElem(hv_EpoxyDist, hv_EpoxyDistThresh, out hv_GreaterEpoxy);
            HOperatorSet.TupleFind(hv_GreaterEpoxy, 1, out hv_Indices);
            //gen_cross_contour_xld (Cross1, EpoxyRow, EpoxyCol, 6, Phi)
            if ((int)(new HTuple(hv_Indices.TupleGreaterEqual(0))) != 0)
            {
                ho_DefectRegion.Dispose();
                HOperatorSet.DilationRectangle1(ho_ICRegion, out ho_DefectRegion, (hv_EpoxyDist.TupleMax()
                    ) * 2, (hv_EpoxyDist.TupleMax()) * 2);
                hv_iFlag = -1;

                return;
            }

            return;
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

        public void lp_ball_detect(HObject ho_Image, HObject ho_LpPad, HTuple hv_ModelType,
            HTuple hv_ModelID, HTuple hv_MatchThresh, HTuple hv_AngleStart, HTuple hv_AngleExtent,
            out HTuple hv_BallRow, out HTuple hv_BallCol, out HTuple hv_BallAngle, out HTuple hv_iFlag)
        {




            // Local iconic variables 

            HObject ho_ImageReduced = null;

            // Local control variables 

            HTuple hv_is_DL = null, hv_Score = new HTuple();
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_ImageReduced);
            try
            {
                hv_iFlag = 0;
                hv_BallRow = new HTuple();
                hv_BallCol = new HTuple();
                hv_BallAngle = new HTuple();
                hv_is_DL = 0;
                if ((int)(hv_is_DL) != 0)
                {
                    //NONE
                }
                else
                {
                    ho_ImageReduced.Dispose();
                    HOperatorSet.ReduceDomain(ho_Image, ho_LpPad, out ho_ImageReduced);
                    if ((int)(new HTuple(hv_ModelType.TupleEqual(0))) != 0)
                    {
                        HOperatorSet.FindNccModel(ho_ImageReduced, hv_ModelID, hv_AngleStart, hv_AngleExtent,
                            0.5, 1, 0.5, "true", 0, out hv_BallRow, out hv_BallCol, out hv_BallAngle,
                            out hv_Score);
                    }
                    else if ((int)(new HTuple(hv_ModelType.TupleEqual(1))) != 0)
                    {
                        HOperatorSet.FindShapeModel(ho_ImageReduced, hv_ModelID, hv_AngleStart,
                            hv_AngleExtent, 0.5, 1, 0.5, "least_squares", 0, 0.9, out hv_BallRow,
                            out hv_BallCol, out hv_BallAngle, out hv_Score);
                    }
                    //gen_cross_contour_xld (Cross, BallRow, BallCol, BallAngle, Angle)
                    if ((int)((new HTuple((new HTuple(hv_Score.TupleLength())).TupleLess(0))).TupleOr(
                        new HTuple(hv_Score.TupleLess(hv_MatchThresh)))) != 0)
                    {
                        hv_iFlag = -1;
                        ho_ImageReduced.Dispose();

                        return;
                    }
                }
                ho_ImageReduced.Dispose();

                return;
            }
            catch (HalconException HDevExpDefaultException)
            {
                ho_ImageReduced.Dispose();

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

        public void track_wire_dp(HObject ho_Image, out HObject ho_TrackRegion, out HObject ho_Wire,
            HTuple hv_LPBallRow, HTuple hv_LPBallCol, HTuple hv_ICBallRow, HTuple hv_ICBallCol,
            HTuple hv_SearchLen, HTuple hv_ClipLen, HTuple hv_LineWidth, HTuple hv_LineContrast,
            HTuple hv_MinSegLen, HTuple hv_AngleExtent, HTuple hv_MaxGap, out HTuple hv_iFlag)
        {




            // Local iconic variables 

            HObject ho_ImageReduced1, ho_Lines, ho_ContoursSplit;
            HObject ho_SelectedContoursLength, ho_SelectedContours;
            HObject ho_ContoursSort, ho_ObjectSelected, ho_ContourLP;
            HObject ho_ContourIC, ho_ObjectConcat, ho_ObjectsConcat;
            HObject ho_UnionContour;

            // Local control variables 

            HTuple hv_IMAX = null, hv_Distance = null;
            HTuple hv_Phi = null, hv_Sigma = null, hv_Low = null, hv_High = null;
            HTuple hv_Number = null, hv_RowBegin_ = null, hv_ColBegin_ = null;
            HTuple hv_RowEnd_ = null, hv_ColEnd_ = null, hv_Indices = null;
            HTuple hv_Length = null, hv_SegLength = null, hv_RowBegin = null;
            HTuple hv_ColBegin = null, hv_RowEnd = null, hv_ColEnd = null;
            HTuple hv_SegNum = null, hv_DistanceMatrix = null, hv_i = null;
            HTuple hv_Row0 = new HTuple(), hv_Col0 = new HTuple();
            HTuple hv_Row1 = new HTuple(), hv_Col1 = new HTuple();
            HTuple hv_j = new HTuple(), hv_DistanceMin = new HTuple();
            HTuple hv_Angle = new HTuple(), hv_VisitedInd = null, hv_Less = null;
            HTuple hv_Previous = null, hv_DistMin = new HTuple(), hv_Prev = new HTuple();
            HTuple hv_DistanceCurrent = new HTuple(), hv_Road = null;
            HTuple hv_ind = null;
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_TrackRegion);
            HOperatorSet.GenEmptyObj(out ho_Wire);
            HOperatorSet.GenEmptyObj(out ho_ImageReduced1);
            HOperatorSet.GenEmptyObj(out ho_Lines);
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
                ho_Wire.Dispose();
                HOperatorSet.GenEmptyObj(out ho_Wire);
                ho_TrackRegion.Dispose();
                HOperatorSet.GenEmptyObj(out ho_TrackRegion);
                hv_IMAX = 9999;
                //gen bond wire detect region
                HOperatorSet.DistancePp(hv_LPBallRow, hv_LPBallCol, hv_ICBallRow, hv_ICBallCol,
                    out hv_Distance);
                HOperatorSet.LineOrientation(hv_LPBallRow, hv_LPBallCol, hv_ICBallRow, hv_ICBallCol,
                    out hv_Phi);
                ho_TrackRegion.Dispose();
                HOperatorSet.GenRectangle2(out ho_TrackRegion, ((hv_LPBallRow.TupleConcat(hv_ICBallRow))).TupleMean()
                    , ((hv_LPBallCol.TupleConcat(hv_ICBallCol))).TupleMean(), hv_Phi, (hv_Distance / 2.0) - hv_ClipLen,
                    hv_SearchLen);
                ho_ImageReduced1.Dispose();
                HOperatorSet.ReduceDomain(ho_Image, ho_TrackRegion, out ho_ImageReduced1);
                //extract lines
                calculate_lines_gauss_parameters(hv_LineWidth, hv_LineContrast, out hv_Sigma,
                    out hv_Low, out hv_High);
                ho_Lines.Dispose();
                HOperatorSet.LinesGauss(ho_ImageReduced1, out ho_Lines, hv_Sigma, hv_Low, hv_High,
                    "light", "false", "none", "false");
                ho_ContoursSplit.Dispose();
                HOperatorSet.SegmentContoursXld(ho_Lines, out ho_ContoursSplit, "lines", 7,
                    2, 1);
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
                HTuple end_val31 = hv_SegNum - 1;
                HTuple step_val31 = 1;
                for (hv_i = 0; hv_i.Continue(end_val31, step_val31); hv_i = hv_i.TupleAdd(step_val31))
                {
                    hv_Row0 = hv_RowBegin.TupleSelect(hv_i);
                    hv_Col0 = hv_ColBegin.TupleSelect(hv_i);
                    hv_Row1 = hv_RowEnd.TupleSelect(hv_i);
                    hv_Col1 = hv_ColEnd.TupleSelect(hv_i);
                    if (hv_DistanceMatrix == null)
                        hv_DistanceMatrix = new HTuple();
                    hv_DistanceMatrix[(hv_i * hv_SegNum) + hv_i] = 0;
                    HTuple end_val37 = hv_SegNum - 1;
                    HTuple step_val37 = 1;
                    for (hv_j = hv_i + 1; hv_j.Continue(end_val37, step_val37); hv_j = hv_j.TupleAdd(step_val37))
                    {
                        HOperatorSet.DistancePp(hv_Row1, hv_Col1, hv_RowBegin.TupleSelect(hv_j),
                            hv_ColBegin.TupleSelect(hv_j), out hv_DistanceMin);
                        HOperatorSet.AngleLl(hv_Row1, hv_Col1, hv_RowBegin.TupleSelect(hv_j), hv_ColBegin.TupleSelect(
                            hv_j), hv_LPBallRow, hv_LPBallCol, hv_ICBallRow, hv_ICBallCol, out hv_Angle);
                        if ((int)((new HTuple(hv_DistanceMin.TupleLess(hv_MaxGap))).TupleAnd((new HTuple(((hv_Angle.TupleAbs()
                            )).TupleLess(1.0))).TupleOr(new HTuple(hv_DistanceMin.TupleLess(hv_LineWidth))))) != 0)
                        {
                            if (hv_DistanceMatrix == null)
                                hv_DistanceMatrix = new HTuple();
                            hv_DistanceMatrix[(hv_i * hv_SegNum) + hv_j] = (hv_DistanceMin * 1.1) + (hv_SegLength.TupleSelect(
                                hv_i));
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
                HTuple end_val51 = hv_SegNum - 1;
                HTuple step_val51 = 1;
                for (hv_i = 1; hv_i.Continue(end_val51, step_val51); hv_i = hv_i.TupleAdd(step_val51))
                {
                    hv_DistMin = hv_IMAX.Clone();
                    hv_Prev = 0;
                    HTuple end_val54 = hv_SegNum - 1;
                    HTuple step_val54 = 1;
                    for (hv_j = 0; hv_j.Continue(end_val54, step_val54); hv_j = hv_j.TupleAdd(step_val54))
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
                    HTuple end_val61 = hv_SegNum - 1;
                    HTuple step_val61 = 1;
                    for (hv_j = 0; hv_j.Continue(end_val61, step_val61); hv_j = hv_j.TupleAdd(step_val61))
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
                    hv_MaxGap, 1, "attr_keep");
                ho_Wire.Dispose();
                HOperatorSet.SmoothContoursXld(ho_UnionContour, out ho_Wire, 21);
                ho_ImageReduced1.Dispose();
                ho_Lines.Dispose();
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
            HObject ho_RegionAffineTrans = null, ho_ObjectsConcat = null;
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
            HOperatorSet.GenEmptyObj(out ho_RegionAffineTrans);
            HOperatorSet.GenEmptyObj(out ho_ObjectsConcat);
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
                        ho_RegionAffineTrans.Dispose();
                        ho_ObjectsConcat.Dispose();
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
                        HTuple end_val42 = hv_sub_reg_num;
                        HTuple step_val42 = 1;
                        for (hv_Index = 0; hv_Index.Continue(end_val42, step_val42); hv_Index = hv_Index.TupleAdd(step_val42))
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
                            HTuple end_val50 = hv_Number;
                            HTuple step_val50 = 1;
                            for (hv_Index1 = 1; hv_Index1.Continue(end_val50, step_val50); hv_Index1 = hv_Index1.TupleAdd(step_val50))
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
                            ho_SelectedRegions.Dispose();
                            HOperatorSet.SelectShape(ho_ConnectedRegions, out ho_SelectedRegions,
                                ((new HTuple("rect2_len1")).TupleConcat("rect2_len2")).TupleConcat(
                                "area"), hv_select_operation, ((((hv_width_thresh_COPY_INP_TMP.TupleSelect(
                                hv_Index))).TupleConcat(hv_height_thresh_COPY_INP_TMP.TupleSelect(
                                hv_Index)))).TupleConcat(hv_area_thresh.TupleSelect(hv_Index)), (
                                (new HTuple(999999)).TupleConcat(999999)).TupleConcat(999999));
                            ho_RegionAffineTrans.Dispose();
                            HOperatorSet.AffineTransRegion(ho_SelectedRegions, out ho_RegionAffineTrans,
                                hv_hom_temp2image, "nearest_neighbor");
                            ho_ObjectsConcat.Dispose();
                            HOperatorSet.ConcatObj(ho_RegionAffineTrans, ho_EmptyRegion, out ho_ObjectsConcat
                                );
                            ho__failure_regions.Dispose();
                            HOperatorSet.Union1(ho_ObjectsConcat, out ho__failure_regions);
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
                ho_RegionAffineTrans.Dispose();
                ho_ObjectsConcat.Dispose();
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
                ho_RegionAffineTrans.Dispose();
                ho_ObjectsConcat.Dispose();
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
                    //union1 (match_region, match_region)
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

        public void bond_wire_detect(HObject ho_Image, HObject ho_LPPads, HObject ho_ICPads,
            out HObject ho_DefectRegions, out HObject ho_BondWires, out HObject ho_LPBalls,
            out HObject ho_ICBalls, HTuple hv_LPBallNum, HTuple hv_OnICInd, HTuple hv_ICRadiusLow,
            HTuple hv_ICRadiusHigh, HTuple hv_LPBallModelID, HTuple hv_LPBallModelType,
            HTuple hv_LPMatchThresh, HTuple hv_LPAngleExtent, HTuple hv_LineSearchLen, HTuple hv_LineClipLen,
            HTuple hv_LineWidth, HTuple hv_LineContrast, HTuple hv_LineMinSegLen, HTuple hv_LineAngleExtent,
            HTuple hv_LineMaxGap, out HTuple hv_iFlag)
        {




            // Stack for temporary objects 
            HObject[] OTemp = new HObject[20];

            // Local iconic variables 

            HObject ho_LpPad = null, ho_IcPad = null, ho_ICBall = null;
            HObject ho_LPBall = null, ho_TrackRegion = null, ho_Wire = null;

            // Local control variables 

            HTuple hv_Number = null, hv_Index = null, hv_BallNum = new HTuple();
            HTuple hv_OnIc = new HTuple(), hv_BallIndEnd = new HTuple();
            HTuple hv_ICBallRow = new HTuple(), hv_ICBallCol = new HTuple();
            HTuple hv_ICBallRadius = new HTuple(), hv_ICiFlag = new HTuple();
            HTuple hv_Area = new HTuple(), hv_Row = new HTuple(), hv_Column = new HTuple();
            HTuple hv_Angle = new HTuple(), hv_LPBallRow = new HTuple();
            HTuple hv_LPBallCol = new HTuple(), hv_LPBallAngel = new HTuple();
            HTuple hv_LPiFlag = new HTuple(), hv_WireiFlag = new HTuple();
            HTuple hv_DefectNum = null;
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_DefectRegions);
            HOperatorSet.GenEmptyObj(out ho_BondWires);
            HOperatorSet.GenEmptyObj(out ho_LPBalls);
            HOperatorSet.GenEmptyObj(out ho_ICBalls);
            HOperatorSet.GenEmptyObj(out ho_LpPad);
            HOperatorSet.GenEmptyObj(out ho_IcPad);
            HOperatorSet.GenEmptyObj(out ho_ICBall);
            HOperatorSet.GenEmptyObj(out ho_LPBall);
            HOperatorSet.GenEmptyObj(out ho_TrackRegion);
            HOperatorSet.GenEmptyObj(out ho_Wire);
            try
            {
                hv_iFlag = 0;
                ho_DefectRegions.Dispose();
                HOperatorSet.GenEmptyObj(out ho_DefectRegions);
                ho_BondWires.Dispose();
                HOperatorSet.GenEmptyObj(out ho_BondWires);
                ho_LPBalls.Dispose();
                HOperatorSet.GenEmptyObj(out ho_LPBalls);
                ho_ICBalls.Dispose();
                HOperatorSet.GenEmptyObj(out ho_ICBalls);
                //*************************************************************************************************
                HOperatorSet.CountObj(ho_LPPads, out hv_Number);
                HTuple end_val7 = hv_Number;
                HTuple step_val7 = 1;
                for (hv_Index = 1; hv_Index.Continue(end_val7, step_val7); hv_Index = hv_Index.TupleAdd(step_val7))
                {
                    hv_BallNum = hv_LPBallNum.TupleSelect(hv_Index - 1);
                    ho_LpPad.Dispose();
                    HOperatorSet.SelectObj(ho_LPPads, out ho_LpPad, hv_Index);
                    hv_OnIc = hv_OnICInd.TupleSelect(hv_Index - 1);
                    hv_BallIndEnd = ((hv_LPBallNum.TupleSelectRange(0, hv_Index - 1))).TupleSum()
                        ;
                    ho_IcPad.Dispose();
                    HOperatorSet.SelectObj(ho_ICPads, out ho_IcPad, HTuple.TupleGenSequence((hv_BallIndEnd - hv_BallNum) + 1,
                        hv_BallIndEnd, 1));
                    //ic ball detect
                    ic_ball_detect(ho_Image, ho_IcPad, hv_OnIc, hv_ICRadiusLow, hv_ICRadiusHigh,
                        out hv_ICBallRow, out hv_ICBallCol, out hv_ICBallRadius, out hv_ICiFlag);
                    if ((int)(new HTuple(hv_ICiFlag.TupleNotEqual(0))) != 0)
                    {
                        {
                            HObject ExpTmpOutVar_0;
                            HOperatorSet.ConcatObj(ho_DefectRegions, ho_IcPad, out ExpTmpOutVar_0);
                            ho_DefectRegions.Dispose();
                            ho_DefectRegions = ExpTmpOutVar_0;
                        }
                        continue;
                    }
                    else
                    {
                        ho_ICBall.Dispose();
                        HOperatorSet.GenCircle(out ho_ICBall, hv_ICBallRow, hv_ICBallCol, hv_ICBallRadius + 1);
                        {
                            HObject ExpTmpOutVar_0;
                            HOperatorSet.ConcatObj(ho_ICBalls, ho_ICBall, out ExpTmpOutVar_0);
                            ho_ICBalls.Dispose();
                            ho_ICBalls = ExpTmpOutVar_0;
                        }
                    }
                    //lead post ball detect
                    HOperatorSet.AreaCenter(ho_LpPad, out hv_Area, out hv_Row, out hv_Column);
                    HOperatorSet.AngleLl(0, 0, 0, 1, hv_Row, hv_Column, hv_ICBallRow, hv_ICBallCol,
                        out hv_Angle);
                    lp_ball_detect(ho_Image, ho_LpPad, hv_LPBallModelType, hv_LPBallModelID,
                        hv_LPMatchThresh, hv_Angle - hv_LPAngleExtent, hv_LPAngleExtent * 2, out hv_LPBallRow,
                        out hv_LPBallCol, out hv_LPBallAngel, out hv_LPiFlag);
                    if ((int)(new HTuple(hv_LPiFlag.TupleNotEqual(0))) != 0)
                    {
                        {
                            HObject ExpTmpOutVar_0;
                            HOperatorSet.ConcatObj(ho_DefectRegions, ho_LpPad, out ExpTmpOutVar_0);
                            ho_DefectRegions.Dispose();
                            ho_DefectRegions = ExpTmpOutVar_0;
                        }
                        continue;
                    }
                    else
                    {
                        ho_LPBall.Dispose();
                        HOperatorSet.GenCircle(out ho_LPBall, hv_LPBallRow, hv_LPBallCol, hv_ICBallRadius + 9);
                        {
                            HObject ExpTmpOutVar_0;
                            HOperatorSet.ConcatObj(ho_LPBalls, ho_LPBall, out ExpTmpOutVar_0);
                            ho_LPBalls.Dispose();
                            ho_LPBalls = ExpTmpOutVar_0;
                        }
                    }
                    //gold wire detect
                    ho_TrackRegion.Dispose(); ho_Wire.Dispose();
                    track_wire_dp(ho_Image, out ho_TrackRegion, out ho_Wire, hv_LPBallRow, hv_LPBallCol,
                        hv_ICBallRow, hv_ICBallCol, hv_LineSearchLen, hv_LineClipLen, hv_LineWidth,
                        hv_LineContrast, hv_LineMinSegLen, hv_LineAngleExtent, hv_LineMaxGap,
                        out hv_WireiFlag);
                    if ((int)(new HTuple(hv_WireiFlag.TupleEqual(0))) != 0)
                    {
                        {
                            HObject ExpTmpOutVar_0;
                            HOperatorSet.ConcatObj(ho_BondWires, ho_Wire, out ExpTmpOutVar_0);
                            ho_BondWires.Dispose();
                            ho_BondWires = ExpTmpOutVar_0;
                        }
                    }
                    else
                    {
                        {
                            HObject ExpTmpOutVar_0;
                            HOperatorSet.ConcatObj(ho_DefectRegions, ho_TrackRegion, out ExpTmpOutVar_0
                                );
                            ho_DefectRegions.Dispose();
                            ho_DefectRegions = ExpTmpOutVar_0;
                        }
                    }
                }
                HOperatorSet.CountObj(ho_DefectRegions, out hv_DefectNum);
                if ((int)(hv_DefectNum) != 0)
                {
                    hv_iFlag = -1;
                }
                ho_LpPad.Dispose();
                ho_IcPad.Dispose();
                ho_ICBall.Dispose();
                ho_LPBall.Dispose();
                ho_TrackRegion.Dispose();
                ho_Wire.Dispose();

                return;
            }
            catch (HalconException HDevExpDefaultException)
            {
                ho_LpPad.Dispose();
                ho_IcPad.Dispose();
                ho_ICBall.Dispose();
                ho_LPBall.Dispose();
                ho_TrackRegion.Dispose();
                ho_Wire.Dispose();

                throw HDevExpDefaultException;
            }
        }

        public void ic_ball_detect(HObject ho_Image, HObject ho_IcPad, HTuple hv_OnIC,
            HTuple hv_ICRadiusLow, HTuple hv_ICRadiusHigh, out HTuple hv_ICBallRow, out HTuple hv_ICBallCol,
            out HTuple hv_ICBallRadius, out HTuple hv_iFlag)
        {




            // Local iconic variables 

            HObject ho_ImageReduced = null, ho_Edges = null;
            HObject ho_Region = null, ho_RegionClosing = null, ho_RegionFillUp = null;
            HObject ho_RegionOpening = null;

            // Local control variables 

            HTuple hv_Area = new HTuple(), hv_Row = new HTuple();
            HTuple hv_Column = new HTuple(), hv_opening_size = new HTuple();
            HTuple hv_Width = null, hv_Height = null, hv_MetrologyHandle = null;
            HTuple hv_Index = null, hv_Circle = null;
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_ImageReduced);
            HOperatorSet.GenEmptyObj(out ho_Edges);
            HOperatorSet.GenEmptyObj(out ho_Region);
            HOperatorSet.GenEmptyObj(out ho_RegionClosing);
            HOperatorSet.GenEmptyObj(out ho_RegionFillUp);
            HOperatorSet.GenEmptyObj(out ho_RegionOpening);
            try
            {
                //*********************************************************************
                //         detect and measure the ic ball
                //input:    Image, ic_pad,   *          ic_radius_low, ic_radius_high
                //output：  ic_ball,   *          iFlag(0:ic ball detect successed, -1:ic ball not found,                            -2:small ic ball , -3: big ic ball)
                //*********************************************************************
                hv_iFlag = 0;
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
                    ho_Edges.Dispose();
                    HOperatorSet.EdgesSubPix(ho_ImageReduced, out ho_Edges, "canny", 1, 30, 60);
                    ho_Region.Dispose();
                    contours_neighborhood_regions(ho_Edges, out ho_Region, 0.5);
                    //threshold (ImageReduced, Region, 140, 255)
                    ho_RegionClosing.Dispose();
                    HOperatorSet.ClosingCircle(ho_Region, out ho_RegionClosing, hv_ICRadiusLow);
                    ho_RegionFillUp.Dispose();
                    HOperatorSet.FillUpShape(ho_RegionClosing, out ho_RegionFillUp, "area", 1,
                        100);
                    hv_opening_size = hv_ICRadiusHigh.Clone();
                    hv_Area = 0;
                    while ((int)((new HTuple(hv_opening_size.TupleGreater(hv_ICRadiusLow - 1))).TupleAnd(
                        new HTuple(hv_Area.TupleLess(1)))) != 0)
                    {
                        ho_RegionOpening.Dispose();
                        HOperatorSet.OpeningCircle(ho_RegionFillUp, out ho_RegionOpening, hv_opening_size);
                        HOperatorSet.AreaCenter(ho_RegionOpening, out hv_Area, out hv_Row, out hv_Column);
                        hv_opening_size = hv_opening_size - 0.5;
                    }
                    //area_center (RegionOpening, Area, Row, Column)
                    if ((int)(new HTuple(hv_Area.TupleLess(1))) != 0)
                    {
                        hv_iFlag = -1;
                        ho_ImageReduced.Dispose();
                        ho_Edges.Dispose();
                        ho_Region.Dispose();
                        ho_RegionClosing.Dispose();
                        ho_RegionFillUp.Dispose();
                        ho_RegionOpening.Dispose();

                        return;
                    }
                    //none
                }
                HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height);
                HOperatorSet.CreateMetrologyModel(out hv_MetrologyHandle);
                HOperatorSet.SetMetrologyModelImageSize(hv_MetrologyHandle, hv_Width, hv_Height);
                HOperatorSet.AddMetrologyObjectCircleMeasure(hv_MetrologyHandle, hv_Row, hv_Column,
                    5, 3, 2, 1, 30, ((new HTuple("measure_transition")).TupleConcat("measure_distance")).TupleConcat(
                    "min_score"), ((new HTuple("negative")).TupleConcat(3)).TupleConcat(0.6),
                    out hv_Index);
                HOperatorSet.ApplyMetrologyModel(ho_Image, hv_MetrologyHandle);
                HOperatorSet.GetMetrologyObjectResult(hv_MetrologyHandle, hv_Index, "all",
                    "result_type", "all_param", out hv_Circle);
                //get_metrology_object_measures (Contours, MetrologyHandle, 'all', 'all', Row1, Column1)
                //gen_cross_contour_xld (Cross, Row1, Column1, 6, 0.785398)
                HOperatorSet.ClearMetrologyModel(hv_MetrologyHandle);
                if ((int)(new HTuple((new HTuple(hv_Circle.TupleLength())).TupleEqual(3))) != 0)
                {
                    if ((int)(new HTuple(((hv_Circle.TupleSelect(2))).TupleLess(hv_ICRadiusLow))) != 0)
                    {
                        hv_iFlag = -2;
                    }
                    else if ((int)(new HTuple(((hv_Circle.TupleSelect(2))).TupleGreater(
                        hv_ICRadiusHigh))) != 0)
                    {
                        hv_iFlag = -3;
                    }
                    else
                    {
                        hv_ICBallRow = hv_Circle[0];
                        hv_ICBallCol = hv_Circle[1];
                        hv_ICBallRadius = hv_Circle[2];
                    }
                }
                else
                {
                    hv_iFlag = -1;
                }
                ho_ImageReduced.Dispose();
                ho_Edges.Dispose();
                ho_Region.Dispose();
                ho_RegionClosing.Dispose();
                ho_RegionFillUp.Dispose();
                ho_RegionOpening.Dispose();

                return;
            }
            catch (HalconException HDevExpDefaultException)
            {
                ho_ImageReduced.Dispose();
                ho_Edges.Dispose();
                ho_Region.Dispose();
                ho_RegionClosing.Dispose();
                ho_RegionFillUp.Dispose();
                ho_RegionOpening.Dispose();

                throw HDevExpDefaultException;
            }
        }

        // Chapter: Filters / Lines
        // Short Description: Calculates the parameters Sigma, Low, and High for lines_gauss from the maximum width and the contrast of the lines to be extracted. 
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

        // Chapter: Graphics / Text
        // Short Description: This procedure writes a text message. 
        public void disp_message(HTuple hv_WindowHandle, HTuple hv_String, HTuple hv_CoordSystem,
            HTuple hv_Row, HTuple hv_Column, HTuple hv_Color, HTuple hv_Box)
        {



            // Local iconic variables 

            // Local control variables 

            HTuple hv_Red = null, hv_Green = null, hv_Blue = null;
            HTuple hv_Row1Part = null, hv_Column1Part = null, hv_Row2Part = null;
            HTuple hv_Column2Part = null, hv_RowWin = null, hv_ColumnWin = null;
            HTuple hv_WidthWin = null, hv_HeightWin = null, hv_MaxAscent = null;
            HTuple hv_MaxDescent = null, hv_MaxWidth = null, hv_MaxHeight = null;
            HTuple hv_R1 = new HTuple(), hv_C1 = new HTuple(), hv_FactorRow = new HTuple();
            HTuple hv_FactorColumn = new HTuple(), hv_UseShadow = null;
            HTuple hv_ShadowColor = null, hv_Exception = new HTuple();
            HTuple hv_Width = new HTuple(), hv_Index = new HTuple();
            HTuple hv_Ascent = new HTuple(), hv_Descent = new HTuple();
            HTuple hv_W = new HTuple(), hv_H = new HTuple(), hv_FrameHeight = new HTuple();
            HTuple hv_FrameWidth = new HTuple(), hv_R2 = new HTuple();
            HTuple hv_C2 = new HTuple(), hv_DrawMode = new HTuple();
            HTuple hv_CurrentColor = new HTuple();
            HTuple hv_Box_COPY_INP_TMP = hv_Box.Clone();
            HTuple hv_Color_COPY_INP_TMP = hv_Color.Clone();
            HTuple hv_Column_COPY_INP_TMP = hv_Column.Clone();
            HTuple hv_Row_COPY_INP_TMP = hv_Row.Clone();
            HTuple hv_String_COPY_INP_TMP = hv_String.Clone();

            // Initialize local and output iconic variables 
            //This procedure displays text in a graphics window.
            //
            //Input parameters:
            //WindowHandle: The WindowHandle of the graphics window, where
            //   the message should be displayed
            //String: A tuple of strings containing the text message to be displayed
            //CoordSystem: If set to 'window', the text position is given
            //   with respect to the window coordinate system.
            //   If set to 'image', image coordinates are used.
            //   (This may be useful in zoomed images.)
            //Row: The row coordinate of the desired text position
            //   If set to -1, a default value of 12 is used.
            //Column: The column coordinate of the desired text position
            //   If set to -1, a default value of 12 is used.
            //Color: defines the color of the text as string.
            //   If set to [], '' or 'auto' the currently set color is used.
            //   If a tuple of strings is passed, the colors are used cyclically
            //   for each new textline.
            //Box: If Box[0] is set to 'true', the text is written within an orange box.
            //     If set to' false', no box is displayed.
            //     If set to a color string (e.g. 'white', '#FF00CC', etc.),
            //       the text is written in a box of that color.
            //     An optional second value for Box (Box[1]) controls if a shadow is displayed:
            //       'true' -> display a shadow in a default color
            //       'false' -> display no shadow (same as if no second value is given)
            //       otherwise -> use given string as color string for the shadow color
            //
            //Prepare window
            HOperatorSet.GetRgb(hv_WindowHandle, out hv_Red, out hv_Green, out hv_Blue);
            HOperatorSet.GetPart(hv_WindowHandle, out hv_Row1Part, out hv_Column1Part, out hv_Row2Part,
                out hv_Column2Part);
            HOperatorSet.GetWindowExtents(hv_WindowHandle, out hv_RowWin, out hv_ColumnWin,
                out hv_WidthWin, out hv_HeightWin);
            HOperatorSet.SetPart(hv_WindowHandle, 0, 0, hv_HeightWin - 1, hv_WidthWin - 1);
            //
            //default settings
            if ((int)(new HTuple(hv_Row_COPY_INP_TMP.TupleEqual(-1))) != 0)
            {
                hv_Row_COPY_INP_TMP = 12;
            }
            if ((int)(new HTuple(hv_Column_COPY_INP_TMP.TupleEqual(-1))) != 0)
            {
                hv_Column_COPY_INP_TMP = 12;
            }
            if ((int)(new HTuple(hv_Color_COPY_INP_TMP.TupleEqual(new HTuple()))) != 0)
            {
                hv_Color_COPY_INP_TMP = "";
            }
            //
            hv_String_COPY_INP_TMP = ((("" + hv_String_COPY_INP_TMP) + "")).TupleSplit("\n");
            //
            //Estimate extentions of text depending on font size.
            HOperatorSet.GetFontExtents(hv_WindowHandle, out hv_MaxAscent, out hv_MaxDescent,
                out hv_MaxWidth, out hv_MaxHeight);
            if ((int)(new HTuple(hv_CoordSystem.TupleEqual("window"))) != 0)
            {
                hv_R1 = hv_Row_COPY_INP_TMP.Clone();
                hv_C1 = hv_Column_COPY_INP_TMP.Clone();
            }
            else
            {
                //Transform image to window coordinates
                hv_FactorRow = (1.0 * hv_HeightWin) / ((hv_Row2Part - hv_Row1Part) + 1);
                hv_FactorColumn = (1.0 * hv_WidthWin) / ((hv_Column2Part - hv_Column1Part) + 1);
                hv_R1 = ((hv_Row_COPY_INP_TMP - hv_Row1Part) + 0.5) * hv_FactorRow;
                hv_C1 = ((hv_Column_COPY_INP_TMP - hv_Column1Part) + 0.5) * hv_FactorColumn;
            }
            //
            //Display text box depending on text size
            hv_UseShadow = 1;
            hv_ShadowColor = "gray";
            if ((int)(new HTuple(((hv_Box_COPY_INP_TMP.TupleSelect(0))).TupleEqual("true"))) != 0)
            {
                if (hv_Box_COPY_INP_TMP == null)
                    hv_Box_COPY_INP_TMP = new HTuple();
                hv_Box_COPY_INP_TMP[0] = "#fce9d4";
                hv_ShadowColor = "#f28d26";
            }
            if ((int)(new HTuple((new HTuple(hv_Box_COPY_INP_TMP.TupleLength())).TupleGreater(
                1))) != 0)
            {
                if ((int)(new HTuple(((hv_Box_COPY_INP_TMP.TupleSelect(1))).TupleEqual("true"))) != 0)
                {
                    //Use default ShadowColor set above
                }
                else if ((int)(new HTuple(((hv_Box_COPY_INP_TMP.TupleSelect(1))).TupleEqual(
                    "false"))) != 0)
                {
                    hv_UseShadow = 0;
                }
                else
                {
                    hv_ShadowColor = hv_Box_COPY_INP_TMP[1];
                    //Valid color?
                    try
                    {
                        HOperatorSet.SetColor(hv_WindowHandle, hv_Box_COPY_INP_TMP.TupleSelect(
                            1));
                    }
                    // catch (Exception) 
                    catch (HalconException HDevExpDefaultException1)
                    {
                        HDevExpDefaultException1.ToHTuple(out hv_Exception);
                        hv_Exception = "Wrong value of control parameter Box[1] (must be a 'true', 'false', or a valid color string)";
                        throw new HalconException(hv_Exception);
                    }
                }
            }
            if ((int)(new HTuple(((hv_Box_COPY_INP_TMP.TupleSelect(0))).TupleNotEqual("false"))) != 0)
            {
                //Valid color?
                try
                {
                    HOperatorSet.SetColor(hv_WindowHandle, hv_Box_COPY_INP_TMP.TupleSelect(0));
                }
                // catch (Exception) 
                catch (HalconException HDevExpDefaultException1)
                {
                    HDevExpDefaultException1.ToHTuple(out hv_Exception);
                    hv_Exception = "Wrong value of control parameter Box[0] (must be a 'true', 'false', or a valid color string)";
                    throw new HalconException(hv_Exception);
                }
                //Calculate box extents
                hv_String_COPY_INP_TMP = (" " + hv_String_COPY_INP_TMP) + " ";
                hv_Width = new HTuple();
                for (hv_Index = 0; (int)hv_Index <= (int)((new HTuple(hv_String_COPY_INP_TMP.TupleLength()
                    )) - 1); hv_Index = (int)hv_Index + 1)
                {
                    HOperatorSet.GetStringExtents(hv_WindowHandle, hv_String_COPY_INP_TMP.TupleSelect(
                        hv_Index), out hv_Ascent, out hv_Descent, out hv_W, out hv_H);
                    hv_Width = hv_Width.TupleConcat(hv_W);
                }
                hv_FrameHeight = hv_MaxHeight * (new HTuple(hv_String_COPY_INP_TMP.TupleLength()
                    ));
                hv_FrameWidth = (((new HTuple(0)).TupleConcat(hv_Width))).TupleMax();
                hv_R2 = hv_R1 + hv_FrameHeight;
                hv_C2 = hv_C1 + hv_FrameWidth;
                //Display rectangles
                HOperatorSet.GetDraw(hv_WindowHandle, out hv_DrawMode);
                HOperatorSet.SetDraw(hv_WindowHandle, "fill");
                //Set shadow color
                HOperatorSet.SetColor(hv_WindowHandle, hv_ShadowColor);
                if ((int)(hv_UseShadow) != 0)
                {
                    HOperatorSet.DispRectangle1(hv_WindowHandle, hv_R1 + 1, hv_C1 + 1, hv_R2 + 1, hv_C2 + 1);
                }
                //Set box color
                HOperatorSet.SetColor(hv_WindowHandle, hv_Box_COPY_INP_TMP.TupleSelect(0));
                HOperatorSet.DispRectangle1(hv_WindowHandle, hv_R1, hv_C1, hv_R2, hv_C2);
                HOperatorSet.SetDraw(hv_WindowHandle, hv_DrawMode);
            }
            //Write text.
            for (hv_Index = 0; (int)hv_Index <= (int)((new HTuple(hv_String_COPY_INP_TMP.TupleLength()
                )) - 1); hv_Index = (int)hv_Index + 1)
            {
                hv_CurrentColor = hv_Color_COPY_INP_TMP.TupleSelect(hv_Index % (new HTuple(hv_Color_COPY_INP_TMP.TupleLength()
                    )));
                if ((int)((new HTuple(hv_CurrentColor.TupleNotEqual(""))).TupleAnd(new HTuple(hv_CurrentColor.TupleNotEqual(
                    "auto")))) != 0)
                {
                    HOperatorSet.SetColor(hv_WindowHandle, hv_CurrentColor);
                }
                else
                {
                    HOperatorSet.SetRgb(hv_WindowHandle, hv_Red, hv_Green, hv_Blue);
                }
                hv_Row_COPY_INP_TMP = hv_R1 + (hv_MaxHeight * hv_Index);
                HOperatorSet.SetTposition(hv_WindowHandle, hv_Row_COPY_INP_TMP, hv_C1);
                HOperatorSet.WriteString(hv_WindowHandle, hv_String_COPY_INP_TMP.TupleSelect(
                    hv_Index));
            }
            //Reset changed window settings
            HOperatorSet.SetRgb(hv_WindowHandle, hv_Red, hv_Green, hv_Blue);
            HOperatorSet.SetPart(hv_WindowHandle, hv_Row1Part, hv_Column1Part, hv_Row2Part,
                hv_Column2Part);

            return;
        }

        // Chapter: File
        // Short Description: Get all image files under the given path 
        public void list_image_files(HTuple hv_ImageDirectory, HTuple hv_Extensions, HTuple hv_Options,
            out HTuple hv_ImageFiles)
        {



            // Local iconic variables 

            // Local control variables 

            HTuple hv_HalconImages = null, hv_OS = null;
            HTuple hv_Directories = null, hv_Index = null, hv_Length = null;
            HTuple hv_network_drive = null, hv_Substring = new HTuple();
            HTuple hv_FileExists = new HTuple(), hv_AllFiles = new HTuple();
            HTuple hv_i = new HTuple(), hv_Selection = new HTuple();
            HTuple hv_Extensions_COPY_INP_TMP = hv_Extensions.Clone();
            HTuple hv_ImageDirectory_COPY_INP_TMP = hv_ImageDirectory.Clone();

            // Initialize local and output iconic variables 
            //This procedure returns all files in a given directory
            //with one of the suffixes specified in Extensions.
            //
            //input parameters:
            //ImageDirectory: as the name says
            //   If a tuple of directories is given, only the images in the first
            //   existing directory are returned.
            //   If a local directory is not found, the directory is searched
            //   under %HALCONIMAGES%/ImageDirectory. If %HALCONIMAGES% is not set,
            //   %HALCONROOT%/images is used instead.
            //Extensions: A string tuple containing the extensions to be found
            //   e.g. ['png','tif',jpg'] or others
            //If Extensions is set to 'default' or the empty string '',
            //   all image suffixes supported by HALCON are used.
            //Options: as in the operator list_files, except that the 'files'
            //   option is always used. Note that the 'directories' option
            //   has no effect but increases runtime, because only files are
            //   returned.
            //
            //output parameter:
            //ImageFiles: A tuple of all found image file names
            //
            if ((int)((new HTuple((new HTuple(hv_Extensions_COPY_INP_TMP.TupleEqual(new HTuple()))).TupleOr(
                new HTuple(hv_Extensions_COPY_INP_TMP.TupleEqual(""))))).TupleOr(new HTuple(hv_Extensions_COPY_INP_TMP.TupleEqual(
                "default")))) != 0)
            {
                hv_Extensions_COPY_INP_TMP = new HTuple();
                hv_Extensions_COPY_INP_TMP[0] = "ima";
                hv_Extensions_COPY_INP_TMP[1] = "tif";
                hv_Extensions_COPY_INP_TMP[2] = "tiff";
                hv_Extensions_COPY_INP_TMP[3] = "gif";
                hv_Extensions_COPY_INP_TMP[4] = "bmp";
                hv_Extensions_COPY_INP_TMP[5] = "jpg";
                hv_Extensions_COPY_INP_TMP[6] = "jpeg";
                hv_Extensions_COPY_INP_TMP[7] = "jp2";
                hv_Extensions_COPY_INP_TMP[8] = "jxr";
                hv_Extensions_COPY_INP_TMP[9] = "png";
                hv_Extensions_COPY_INP_TMP[10] = "pcx";
                hv_Extensions_COPY_INP_TMP[11] = "ras";
                hv_Extensions_COPY_INP_TMP[12] = "xwd";
                hv_Extensions_COPY_INP_TMP[13] = "pbm";
                hv_Extensions_COPY_INP_TMP[14] = "pnm";
                hv_Extensions_COPY_INP_TMP[15] = "pgm";
                hv_Extensions_COPY_INP_TMP[16] = "ppm";
                //
            }
            if ((int)(new HTuple(hv_ImageDirectory_COPY_INP_TMP.TupleEqual(""))) != 0)
            {
                hv_ImageDirectory_COPY_INP_TMP = ".";
            }
            HOperatorSet.GetSystem("image_dir", out hv_HalconImages);
            HOperatorSet.GetSystem("operating_system", out hv_OS);
            if ((int)(new HTuple(((hv_OS.TupleSubstr(0, 2))).TupleEqual("Win"))) != 0)
            {
                hv_HalconImages = hv_HalconImages.TupleSplit(";");
            }
            else
            {
                hv_HalconImages = hv_HalconImages.TupleSplit(":");
            }
            hv_Directories = hv_ImageDirectory_COPY_INP_TMP.Clone();
            for (hv_Index = 0; (int)hv_Index <= (int)((new HTuple(hv_HalconImages.TupleLength()
                )) - 1); hv_Index = (int)hv_Index + 1)
            {
                hv_Directories = hv_Directories.TupleConcat(((hv_HalconImages.TupleSelect(hv_Index)) + "/") + hv_ImageDirectory_COPY_INP_TMP);
            }
            HOperatorSet.TupleStrlen(hv_Directories, out hv_Length);
            HOperatorSet.TupleGenConst(new HTuple(hv_Length.TupleLength()), 0, out hv_network_drive);
            if ((int)(new HTuple(((hv_OS.TupleSubstr(0, 2))).TupleEqual("Win"))) != 0)
            {
                for (hv_Index = 0; (int)hv_Index <= (int)((new HTuple(hv_Length.TupleLength())) - 1); hv_Index = (int)hv_Index + 1)
                {
                    if ((int)(new HTuple(((((hv_Directories.TupleSelect(hv_Index))).TupleStrlen()
                        )).TupleGreater(1))) != 0)
                    {
                        HOperatorSet.TupleStrFirstN(hv_Directories.TupleSelect(hv_Index), 1, out hv_Substring);
                        if ((int)(new HTuple(hv_Substring.TupleEqual("//"))) != 0)
                        {
                            if (hv_network_drive == null)
                                hv_network_drive = new HTuple();
                            hv_network_drive[hv_Index] = 1;
                        }
                    }
                }
            }
            hv_ImageFiles = new HTuple();
            for (hv_Index = 0; (int)hv_Index <= (int)((new HTuple(hv_Directories.TupleLength()
                )) - 1); hv_Index = (int)hv_Index + 1)
            {
                HOperatorSet.FileExists(hv_Directories.TupleSelect(hv_Index), out hv_FileExists);
                if ((int)(hv_FileExists) != 0)
                {
                    HOperatorSet.ListFiles(hv_Directories.TupleSelect(hv_Index), (new HTuple("files")).TupleConcat(
                        hv_Options), out hv_AllFiles);
                    hv_ImageFiles = new HTuple();
                    for (hv_i = 0; (int)hv_i <= (int)((new HTuple(hv_Extensions_COPY_INP_TMP.TupleLength()
                        )) - 1); hv_i = (int)hv_i + 1)
                    {
                        HOperatorSet.TupleRegexpSelect(hv_AllFiles, (((".*" + (hv_Extensions_COPY_INP_TMP.TupleSelect(
                            hv_i))) + "$")).TupleConcat("ignore_case"), out hv_Selection);
                        hv_ImageFiles = hv_ImageFiles.TupleConcat(hv_Selection);
                    }
                    HOperatorSet.TupleRegexpReplace(hv_ImageFiles, (new HTuple("\\\\")).TupleConcat(
                        "replace_all"), "/", out hv_ImageFiles);
                    if ((int)(hv_network_drive.TupleSelect(hv_Index)) != 0)
                    {
                        HOperatorSet.TupleRegexpReplace(hv_ImageFiles, (new HTuple("//")).TupleConcat(
                            "replace_all"), "/", out hv_ImageFiles);
                        hv_ImageFiles = "/" + hv_ImageFiles;
                    }
                    else
                    {
                        HOperatorSet.TupleRegexpReplace(hv_ImageFiles, (new HTuple("//")).TupleConcat(
                            "replace_all"), "/", out hv_ImageFiles);
                    }

                    return;
                }
            }

            return;
        }

        // Chapter: Graphics / Text
        // Short Description: Set font independent of OS 
        public void set_display_font(HTuple hv_WindowHandle, HTuple hv_Size, HTuple hv_Font,
            HTuple hv_Bold, HTuple hv_Slant)
        {



            // Local iconic variables 

            // Local control variables 

            HTuple hv_OS = null, hv_BufferWindowHandle = new HTuple();
            HTuple hv_Ascent = new HTuple(), hv_Descent = new HTuple();
            HTuple hv_Width = new HTuple(), hv_Height = new HTuple();
            HTuple hv_Scale = new HTuple(), hv_Exception = new HTuple();
            HTuple hv_SubFamily = new HTuple(), hv_Fonts = new HTuple();
            HTuple hv_SystemFonts = new HTuple(), hv_Guess = new HTuple();
            HTuple hv_I = new HTuple(), hv_Index = new HTuple(), hv_AllowedFontSizes = new HTuple();
            HTuple hv_Distances = new HTuple(), hv_Indices = new HTuple();
            HTuple hv_FontSelRegexp = new HTuple(), hv_FontsCourier = new HTuple();
            HTuple hv_Bold_COPY_INP_TMP = hv_Bold.Clone();
            HTuple hv_Font_COPY_INP_TMP = hv_Font.Clone();
            HTuple hv_Size_COPY_INP_TMP = hv_Size.Clone();
            HTuple hv_Slant_COPY_INP_TMP = hv_Slant.Clone();

            // Initialize local and output iconic variables 
            //This procedure sets the text font of the current window with
            //the specified attributes.
            //It is assumed that following fonts are installed on the system:
            //Windows: Courier New, Arial Times New Roman
            //Mac OS X: CourierNewPS, Arial, TimesNewRomanPS
            //Linux: courier, helvetica, times
            //Because fonts are displayed smaller on Linux than on Windows,
            //a scaling factor of 1.25 is used the get comparable results.
            //For Linux, only a limited number of font sizes is supported,
            //to get comparable results, it is recommended to use one of the
            //following sizes: 9, 11, 14, 16, 20, 27
            //(which will be mapped internally on Linux systems to 11, 14, 17, 20, 25, 34)
            //
            //Input parameters:
            //WindowHandle: The graphics window for which the font will be set
            //Size: The font size. If Size=-1, the default of 16 is used.
            //Bold: If set to 'true', a bold font is used
            //Slant: If set to 'true', a slanted font is used
            //
            HOperatorSet.GetSystem("operating_system", out hv_OS);
            // dev_get_preferences(...); only in hdevelop
            // dev_set_preferences(...); only in hdevelop
            if ((int)((new HTuple(hv_Size_COPY_INP_TMP.TupleEqual(new HTuple()))).TupleOr(
                new HTuple(hv_Size_COPY_INP_TMP.TupleEqual(-1)))) != 0)
            {
                hv_Size_COPY_INP_TMP = 16;
            }
            if ((int)(new HTuple(((hv_OS.TupleSubstr(0, 2))).TupleEqual("Win"))) != 0)
            {
                //Set font on Windows systems
                try
                {
                    //Check, if font scaling is switched on
                    HOperatorSet.OpenWindow(0, 0, 256, 256, 0, "buffer", "", out hv_BufferWindowHandle);
                    HOperatorSet.SetFont(hv_BufferWindowHandle, "-Consolas-16-*-0-*-*-1-");
                    HOperatorSet.GetStringExtents(hv_BufferWindowHandle, "test_string", out hv_Ascent,
                        out hv_Descent, out hv_Width, out hv_Height);
                    //Expected width is 110
                    hv_Scale = 110.0 / hv_Width;
                    hv_Size_COPY_INP_TMP = ((hv_Size_COPY_INP_TMP * hv_Scale)).TupleInt();
                    HOperatorSet.CloseWindow(hv_BufferWindowHandle);
                }
                // catch (Exception) 
                catch (HalconException HDevExpDefaultException1)
                {
                    HDevExpDefaultException1.ToHTuple(out hv_Exception);
                    //throw (Exception)
                }
                if ((int)((new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("Courier"))).TupleOr(
                    new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("courier")))) != 0)
                {
                    hv_Font_COPY_INP_TMP = "Courier New";
                }
                else if ((int)(new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("mono"))) != 0)
                {
                    hv_Font_COPY_INP_TMP = "Consolas";
                }
                else if ((int)(new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("sans"))) != 0)
                {
                    hv_Font_COPY_INP_TMP = "Arial";
                }
                else if ((int)(new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("serif"))) != 0)
                {
                    hv_Font_COPY_INP_TMP = "Times New Roman";
                }
                if ((int)(new HTuple(hv_Bold_COPY_INP_TMP.TupleEqual("true"))) != 0)
                {
                    hv_Bold_COPY_INP_TMP = 1;
                }
                else if ((int)(new HTuple(hv_Bold_COPY_INP_TMP.TupleEqual("false"))) != 0)
                {
                    hv_Bold_COPY_INP_TMP = 0;
                }
                else
                {
                    hv_Exception = "Wrong value of control parameter Bold";
                    throw new HalconException(hv_Exception);
                }
                if ((int)(new HTuple(hv_Slant_COPY_INP_TMP.TupleEqual("true"))) != 0)
                {
                    hv_Slant_COPY_INP_TMP = 1;
                }
                else if ((int)(new HTuple(hv_Slant_COPY_INP_TMP.TupleEqual("false"))) != 0)
                {
                    hv_Slant_COPY_INP_TMP = 0;
                }
                else
                {
                    hv_Exception = "Wrong value of control parameter Slant";
                    throw new HalconException(hv_Exception);
                }
                try
                {
                    HOperatorSet.SetFont(hv_WindowHandle, ((((((("-" + hv_Font_COPY_INP_TMP) + "-") + hv_Size_COPY_INP_TMP) + "-*-") + hv_Slant_COPY_INP_TMP) + "-*-*-") + hv_Bold_COPY_INP_TMP) + "-");
                }
                // catch (Exception) 
                catch (HalconException HDevExpDefaultException1)
                {
                    HDevExpDefaultException1.ToHTuple(out hv_Exception);
                    //throw (Exception)
                }
            }
            else if ((int)(new HTuple(((hv_OS.TupleSubstr(0, 2))).TupleEqual("Dar"))) != 0)
            {
                //Set font on Mac OS X systems. Since OS X does not have a strict naming
                //scheme for font attributes, we use tables to determine the correct font
                //name.
                hv_SubFamily = 0;
                if ((int)(new HTuple(hv_Slant_COPY_INP_TMP.TupleEqual("true"))) != 0)
                {
                    hv_SubFamily = hv_SubFamily.TupleBor(1);
                }
                else if ((int)(new HTuple(hv_Slant_COPY_INP_TMP.TupleNotEqual("false"))) != 0)
                {
                    hv_Exception = "Wrong value of control parameter Slant";
                    throw new HalconException(hv_Exception);
                }
                if ((int)(new HTuple(hv_Bold_COPY_INP_TMP.TupleEqual("true"))) != 0)
                {
                    hv_SubFamily = hv_SubFamily.TupleBor(2);
                }
                else if ((int)(new HTuple(hv_Bold_COPY_INP_TMP.TupleNotEqual("false"))) != 0)
                {
                    hv_Exception = "Wrong value of control parameter Bold";
                    throw new HalconException(hv_Exception);
                }
                if ((int)(new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("mono"))) != 0)
                {
                    hv_Fonts = new HTuple();
                    hv_Fonts[0] = "Menlo-Regular";
                    hv_Fonts[1] = "Menlo-Italic";
                    hv_Fonts[2] = "Menlo-Bold";
                    hv_Fonts[3] = "Menlo-BoldItalic";
                }
                else if ((int)((new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("Courier"))).TupleOr(
                    new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("courier")))) != 0)
                {
                    hv_Fonts = new HTuple();
                    hv_Fonts[0] = "CourierNewPSMT";
                    hv_Fonts[1] = "CourierNewPS-ItalicMT";
                    hv_Fonts[2] = "CourierNewPS-BoldMT";
                    hv_Fonts[3] = "CourierNewPS-BoldItalicMT";
                }
                else if ((int)(new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("sans"))) != 0)
                {
                    hv_Fonts = new HTuple();
                    hv_Fonts[0] = "ArialMT";
                    hv_Fonts[1] = "Arial-ItalicMT";
                    hv_Fonts[2] = "Arial-BoldMT";
                    hv_Fonts[3] = "Arial-BoldItalicMT";
                }
                else if ((int)(new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("serif"))) != 0)
                {
                    hv_Fonts = new HTuple();
                    hv_Fonts[0] = "TimesNewRomanPSMT";
                    hv_Fonts[1] = "TimesNewRomanPS-ItalicMT";
                    hv_Fonts[2] = "TimesNewRomanPS-BoldMT";
                    hv_Fonts[3] = "TimesNewRomanPS-BoldItalicMT";
                }
                else
                {
                    //Attempt to figure out which of the fonts installed on the system
                    //the user could have meant.
                    HOperatorSet.QueryFont(hv_WindowHandle, out hv_SystemFonts);
                    hv_Fonts = new HTuple();
                    hv_Fonts = hv_Fonts.TupleConcat(hv_Font_COPY_INP_TMP);
                    hv_Fonts = hv_Fonts.TupleConcat(hv_Font_COPY_INP_TMP);
                    hv_Fonts = hv_Fonts.TupleConcat(hv_Font_COPY_INP_TMP);
                    hv_Fonts = hv_Fonts.TupleConcat(hv_Font_COPY_INP_TMP);
                    hv_Guess = new HTuple();
                    hv_Guess = hv_Guess.TupleConcat(hv_Font_COPY_INP_TMP);
                    hv_Guess = hv_Guess.TupleConcat(hv_Font_COPY_INP_TMP + "-Regular");
                    hv_Guess = hv_Guess.TupleConcat(hv_Font_COPY_INP_TMP + "MT");
                    for (hv_I = 0; (int)hv_I <= (int)((new HTuple(hv_Guess.TupleLength())) - 1); hv_I = (int)hv_I + 1)
                    {
                        HOperatorSet.TupleFind(hv_SystemFonts, hv_Guess.TupleSelect(hv_I), out hv_Index);
                        if ((int)(new HTuple(hv_Index.TupleNotEqual(-1))) != 0)
                        {
                            if (hv_Fonts == null)
                                hv_Fonts = new HTuple();
                            hv_Fonts[0] = hv_Guess.TupleSelect(hv_I);
                            break;
                        }
                    }
                    //Guess name of slanted font
                    hv_Guess = new HTuple();
                    hv_Guess = hv_Guess.TupleConcat(hv_Font_COPY_INP_TMP + "-Italic");
                    hv_Guess = hv_Guess.TupleConcat(hv_Font_COPY_INP_TMP + "-ItalicMT");
                    hv_Guess = hv_Guess.TupleConcat(hv_Font_COPY_INP_TMP + "-Oblique");
                    for (hv_I = 0; (int)hv_I <= (int)((new HTuple(hv_Guess.TupleLength())) - 1); hv_I = (int)hv_I + 1)
                    {
                        HOperatorSet.TupleFind(hv_SystemFonts, hv_Guess.TupleSelect(hv_I), out hv_Index);
                        if ((int)(new HTuple(hv_Index.TupleNotEqual(-1))) != 0)
                        {
                            if (hv_Fonts == null)
                                hv_Fonts = new HTuple();
                            hv_Fonts[1] = hv_Guess.TupleSelect(hv_I);
                            break;
                        }
                    }
                    //Guess name of bold font
                    hv_Guess = new HTuple();
                    hv_Guess = hv_Guess.TupleConcat(hv_Font_COPY_INP_TMP + "-Bold");
                    hv_Guess = hv_Guess.TupleConcat(hv_Font_COPY_INP_TMP + "-BoldMT");
                    for (hv_I = 0; (int)hv_I <= (int)((new HTuple(hv_Guess.TupleLength())) - 1); hv_I = (int)hv_I + 1)
                    {
                        HOperatorSet.TupleFind(hv_SystemFonts, hv_Guess.TupleSelect(hv_I), out hv_Index);
                        if ((int)(new HTuple(hv_Index.TupleNotEqual(-1))) != 0)
                        {
                            if (hv_Fonts == null)
                                hv_Fonts = new HTuple();
                            hv_Fonts[2] = hv_Guess.TupleSelect(hv_I);
                            break;
                        }
                    }
                    //Guess name of bold slanted font
                    hv_Guess = new HTuple();
                    hv_Guess = hv_Guess.TupleConcat(hv_Font_COPY_INP_TMP + "-BoldItalic");
                    hv_Guess = hv_Guess.TupleConcat(hv_Font_COPY_INP_TMP + "-BoldItalicMT");
                    hv_Guess = hv_Guess.TupleConcat(hv_Font_COPY_INP_TMP + "-BoldOblique");
                    for (hv_I = 0; (int)hv_I <= (int)((new HTuple(hv_Guess.TupleLength())) - 1); hv_I = (int)hv_I + 1)
                    {
                        HOperatorSet.TupleFind(hv_SystemFonts, hv_Guess.TupleSelect(hv_I), out hv_Index);
                        if ((int)(new HTuple(hv_Index.TupleNotEqual(-1))) != 0)
                        {
                            if (hv_Fonts == null)
                                hv_Fonts = new HTuple();
                            hv_Fonts[3] = hv_Guess.TupleSelect(hv_I);
                            break;
                        }
                    }
                }
                hv_Font_COPY_INP_TMP = hv_Fonts.TupleSelect(hv_SubFamily);
                try
                {
                    HOperatorSet.SetFont(hv_WindowHandle, (hv_Font_COPY_INP_TMP + "-") + hv_Size_COPY_INP_TMP);
                }
                // catch (Exception) 
                catch (HalconException HDevExpDefaultException1)
                {
                    HDevExpDefaultException1.ToHTuple(out hv_Exception);
                    //throw (Exception)
                }
            }
            else
            {
                //Set font for UNIX systems
                hv_Size_COPY_INP_TMP = hv_Size_COPY_INP_TMP * 1.25;
                hv_AllowedFontSizes = new HTuple();
                hv_AllowedFontSizes[0] = 11;
                hv_AllowedFontSizes[1] = 14;
                hv_AllowedFontSizes[2] = 17;
                hv_AllowedFontSizes[3] = 20;
                hv_AllowedFontSizes[4] = 25;
                hv_AllowedFontSizes[5] = 34;
                if ((int)(new HTuple(((hv_AllowedFontSizes.TupleFind(hv_Size_COPY_INP_TMP))).TupleEqual(
                    -1))) != 0)
                {
                    hv_Distances = ((hv_AllowedFontSizes - hv_Size_COPY_INP_TMP)).TupleAbs();
                    HOperatorSet.TupleSortIndex(hv_Distances, out hv_Indices);
                    hv_Size_COPY_INP_TMP = hv_AllowedFontSizes.TupleSelect(hv_Indices.TupleSelect(
                        0));
                }
                if ((int)((new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("mono"))).TupleOr(new HTuple(hv_Font_COPY_INP_TMP.TupleEqual(
                    "Courier")))) != 0)
                {
                    hv_Font_COPY_INP_TMP = "courier";
                }
                else if ((int)(new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("sans"))) != 0)
                {
                    hv_Font_COPY_INP_TMP = "helvetica";
                }
                else if ((int)(new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("serif"))) != 0)
                {
                    hv_Font_COPY_INP_TMP = "times";
                }
                if ((int)(new HTuple(hv_Bold_COPY_INP_TMP.TupleEqual("true"))) != 0)
                {
                    hv_Bold_COPY_INP_TMP = "bold";
                }
                else if ((int)(new HTuple(hv_Bold_COPY_INP_TMP.TupleEqual("false"))) != 0)
                {
                    hv_Bold_COPY_INP_TMP = "medium";
                }
                else
                {
                    hv_Exception = "Wrong value of control parameter Bold";
                    throw new HalconException(hv_Exception);
                }
                if ((int)(new HTuple(hv_Slant_COPY_INP_TMP.TupleEqual("true"))) != 0)
                {
                    if ((int)(new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("times"))) != 0)
                    {
                        hv_Slant_COPY_INP_TMP = "i";
                    }
                    else
                    {
                        hv_Slant_COPY_INP_TMP = "o";
                    }
                }
                else if ((int)(new HTuple(hv_Slant_COPY_INP_TMP.TupleEqual("false"))) != 0)
                {
                    hv_Slant_COPY_INP_TMP = "r";
                }
                else
                {
                    hv_Exception = "Wrong value of control parameter Slant";
                    throw new HalconException(hv_Exception);
                }
                try
                {
                    HOperatorSet.SetFont(hv_WindowHandle, ((((((("-adobe-" + hv_Font_COPY_INP_TMP) + "-") + hv_Bold_COPY_INP_TMP) + "-") + hv_Slant_COPY_INP_TMP) + "-normal-*-") + hv_Size_COPY_INP_TMP) + "-*-*-*-*-*-*-*");
                }
                // catch (Exception) 
                catch (HalconException HDevExpDefaultException1)
                {
                    HDevExpDefaultException1.ToHTuple(out hv_Exception);
                    if ((int)((new HTuple(((hv_OS.TupleSubstr(0, 4))).TupleEqual("Linux"))).TupleAnd(
                        new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("courier")))) != 0)
                    {
                        HOperatorSet.QueryFont(hv_WindowHandle, out hv_Fonts);
                        hv_FontSelRegexp = (("^-[^-]*-[^-]*[Cc]ourier[^-]*-" + hv_Bold_COPY_INP_TMP) + "-") + hv_Slant_COPY_INP_TMP;
                        hv_FontsCourier = ((hv_Fonts.TupleRegexpSelect(hv_FontSelRegexp))).TupleRegexpMatch(
                            hv_FontSelRegexp);
                        if ((int)(new HTuple((new HTuple(hv_FontsCourier.TupleLength())).TupleEqual(
                            0))) != 0)
                        {
                            hv_Exception = "Wrong font name";
                            //throw (Exception)
                        }
                        else
                        {
                            try
                            {
                                HOperatorSet.SetFont(hv_WindowHandle, (((hv_FontsCourier.TupleSelect(
                                    0)) + "-normal-*-") + hv_Size_COPY_INP_TMP) + "-*-*-*-*-*-*-*");
                            }
                            // catch (Exception) 
                            catch (HalconException HDevExpDefaultException2)
                            {
                                HDevExpDefaultException2.ToHTuple(out hv_Exception);
                                //throw (Exception)
                            }
                        }
                    }
                    //throw (Exception)
                }
            }
            // dev_set_preferences(...); only in hdevelop

            return;
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

        public Boolean InitializeParameters(string modelPath)
        {
            HOperatorSet.GenEmptyObj(out ho_Image);
            HOperatorSet.GenEmptyObj(out ho_defect_region);
            HOperatorSet.GenEmptyObj(out ho_gold_wire);
            HOperatorSet.GenEmptyObj(out ho_map_rect);
            HOperatorSet.GenEmptyObj(out ho_defect_rect);
            try
            {
                //if (HDevWindowStack.IsOpen())
                //{
                //    HOperatorSet.SetDraw(HDevWindowStack.GetActive(), "margin");
                //}
                //HOperatorSet.SetSystem("width", 4096);
                //HOperatorSet.SetSystem("height", 4096);
                //*********************** input **************************
                //model path

                hv_model_path = modelPath;
                //pcb golden model
                hv_pcb_sub_reg_num = 0;
                hv_pcb_sobel_scale = 0.5;
                hv_pcb_dark_thresh = 8;
                hv_pcb_light_thresh = 12;
                hv_pcb_score_thresh = 0.5;
                hv_pcb_angle_start = -0.1;
                hv_pcb_angle_extent = 0.2;
                hv_pcb_search_size = 201;
                hv_pcb_closing_size = 3;
                hv_pcb_select_operation = "and";
                hv_pcb_width_thresh = 10;
                hv_pcb_height_thresh = 10;
                hv_pcb_area_thresh = 100;
                hvec_vec_pcb_golden_param = (new HTupleVector(1).Insert(0, new HTupleVector(new HTuple())));
                hvec_vec_pcb_golden_param.Clear();
                hvec_vec_pcb_golden_param[0] = new HTupleVector(hv_pcb_sub_reg_num).Clone();
                hvec_vec_pcb_golden_param[1] = new HTupleVector(hv_pcb_sobel_scale).Clone();
                hvec_vec_pcb_golden_param[2] = new HTupleVector(hv_pcb_dark_thresh).Clone();
                hvec_vec_pcb_golden_param[3] = new HTupleVector(hv_pcb_light_thresh).Clone();
                hvec_vec_pcb_golden_param[6] = new HTupleVector(hv_pcb_score_thresh).Clone();
                hvec_vec_pcb_golden_param[7] = new HTupleVector(hv_pcb_angle_start).Clone();
                hvec_vec_pcb_golden_param[8] = new HTupleVector(hv_pcb_angle_extent).Clone();
                hvec_vec_pcb_golden_param[9] = new HTupleVector(hv_pcb_search_size).Clone();
                hvec_vec_pcb_golden_param[10] = new HTupleVector(hv_pcb_closing_size).Clone();
                hvec_vec_pcb_golden_param[11] = new HTupleVector(hv_pcb_select_operation).Clone();
                hvec_vec_pcb_golden_param[12] = new HTupleVector(hv_pcb_width_thresh).Clone();
                hvec_vec_pcb_golden_param[13] = new HTupleVector(hv_pcb_height_thresh).Clone();
                hvec_vec_pcb_golden_param[14] = new HTupleVector(hv_pcb_area_thresh).Clone();
                //ic golden model
                hv_ic_sub_reg_num = 0;
                hv_ic_sobel_scale = 0.5;
                hv_ic_dark_thresh = 3;
                hv_ic_light_thresh = 3;
                hv_ic_score_thresh = 0.3;
                hv_ic_angle_start = -0.02;
                hv_ic_angle_extent = 0.04;
                hv_ic_search_size = 51;
                hv_ic_closing_size = 1;
                hv_ic_select_operation = "and";
                hv_ic_width_thresh = 5;
                hv_ic_height_thresh = 5;
                hv_ic_area_thresh = 25;
                hvec_vec_ic_golden_param = (new HTupleVector(1).Insert(0, new HTupleVector(new HTuple())));
                hvec_vec_ic_golden_param.Clear();
                hvec_vec_ic_golden_param[0] = new HTupleVector(hv_ic_sub_reg_num).Clone();
                hvec_vec_ic_golden_param[1] = new HTupleVector(hv_ic_sobel_scale).Clone();
                hvec_vec_ic_golden_param[2] = new HTupleVector(hv_ic_dark_thresh).Clone();
                hvec_vec_ic_golden_param[3] = new HTupleVector(hv_ic_light_thresh).Clone();
                hvec_vec_ic_golden_param[6] = new HTupleVector(hv_ic_score_thresh).Clone();
                hvec_vec_ic_golden_param[7] = new HTupleVector(hv_ic_angle_start).Clone();
                hvec_vec_ic_golden_param[8] = new HTupleVector(hv_ic_angle_extent).Clone();
                hvec_vec_ic_golden_param[9] = new HTupleVector(hv_ic_search_size).Clone();
                hvec_vec_ic_golden_param[10] = new HTupleVector(hv_ic_closing_size).Clone();
                hvec_vec_ic_golden_param[11] = new HTupleVector(hv_ic_select_operation).Clone();
                hvec_vec_ic_golden_param[12] = new HTupleVector(hv_ic_width_thresh).Clone();
                hvec_vec_ic_golden_param[13] = new HTupleVector(hv_ic_height_thresh).Clone();
                hvec_vec_ic_golden_param[14] = new HTupleVector(hv_ic_area_thresh).Clone();
                //ic position
                hv_pos_row_thresh = 20;
                hv_pos_col_thresh = 20;
                hv_pos_angle_thresh = 0.015;
                hvec_vec_pos_param = (new HTupleVector(1).Insert(0, new HTupleVector(new HTuple())));
                hvec_vec_pos_param.Clear();
                hvec_vec_pos_param[0] = new HTupleVector(hv_pos_row_thresh).Clone();
                hvec_vec_pos_param[1] = new HTupleVector(hv_pos_col_thresh).Clone();
                hvec_vec_pos_param[2] = new HTupleVector(hv_pos_angle_thresh).Clone();
                //bond wire
                hv_ic_radius_low = 3;
                hv_ic_radius_high = 7;
                hv_lp_match_thresh = 0.7;
                hv_lp_angle_extent = 0.1;
                hv_line_search_len = 25;
                hv_line_clip_len = 7;
                hv_line_width = 5;
                hv_line_contrast = 20;
                hv_line_min_seg_len = 5;
                hv_line_angle_extent = 0.3;
                hv_line_max_gap = 30;
                hvec_vec_bond_wire_param = (new HTupleVector(1).Insert(0, new HTupleVector(new HTuple())));
                hvec_vec_bond_wire_param.Clear();
                hvec_vec_bond_wire_param[4] = new HTupleVector(hv_ic_radius_low).Clone();
                hvec_vec_bond_wire_param[5] = new HTupleVector(hv_ic_radius_high).Clone();
                hvec_vec_bond_wire_param[6] = new HTupleVector(hv_lp_match_thresh).Clone();
                hvec_vec_bond_wire_param[7] = new HTupleVector(hv_lp_angle_extent).Clone();
                hvec_vec_bond_wire_param[8] = new HTupleVector(hv_line_search_len).Clone();
                hvec_vec_bond_wire_param[9] = new HTupleVector(hv_line_clip_len).Clone();
                hvec_vec_bond_wire_param[10] = new HTupleVector(hv_line_width).Clone();
                hvec_vec_bond_wire_param[11] = new HTupleVector(hv_line_contrast).Clone();
                hvec_vec_bond_wire_param[12] = new HTupleVector(hv_line_min_seg_len).Clone();
                hvec_vec_bond_wire_param[13] = new HTupleVector(hv_line_angle_extent).Clone();
                hvec_vec_bond_wire_param[14] = new HTupleVector(hv_line_max_gap).Clone();
                //epoxy
                hv_epoxy_inspect_size = 30;
                hv_epoxy_dark_light = 1;
                hv_epoxy_edge_sigma = 0.4;
                hv_epoxy_edge_thresh = 2;
                hv_epoxy_dist_thresh = 25;
                hvec_vec_epoxy_param = (new HTupleVector(1).Insert(0, new HTupleVector(new HTuple())));
                hvec_vec_epoxy_param.Clear();
                hvec_vec_epoxy_param[0] = new HTupleVector(hv_epoxy_inspect_size).Clone();
                hvec_vec_epoxy_param[1] = new HTupleVector(hv_epoxy_dark_light).Clone();
                hvec_vec_epoxy_param[2] = new HTupleVector(hv_epoxy_edge_sigma).Clone();
                hvec_vec_epoxy_param[3] = new HTupleVector(hv_epoxy_edge_thresh).Clone();
                hvec_vec_epoxy_param[4] = new HTupleVector(hv_epoxy_dist_thresh).Clone();
                // stop(); only in hdevelop
                hv_R = 11;
                hv_C = 9;
                hv_M = 3;
                hv_N = 4;
                return true;
            }
            catch (HalconException EXP)
            {
                LOG.Error(EXP.Message);
                return false;
            }
        }
        public void LoadAllModel()
        {



        }

        // Main procedure 
        public void Action()
        {
            try
            {
                //********************** load model ***************************
                hvec_vec_pcb_golden_object.Dispose(); hvec_vec_ic_golden_object.Dispose(); hvec_vec_bond_wire_object.Dispose();
                //JSLF_AOI_load_all_model(out hvec_vec_pcb_golden_object, out hvec_vec_ic_golden_object,
                //    out hvec_vec_bond_wire_object, hv_model_path, hvec_vec_pcb_golden_param,
                //    hvec_vec_ic_golden_param, hvec_vec_bond_wire_param, out hvec_vec_pcb_golden_param,
                //    out hvec_vec_ic_golden_param, out hvec_vec_bond_wire_param, out hv_iFlag_model,
                //    out hv_err_msg);
                if ((int)(new HTuple(hv_iFlag_model.TupleNotEqual(0))) != 0)
                {
                    set_display_font(3600, 24, "mono", "true", "false");
                    disp_message(3600, hv_err_msg, "window", 12, 12, "red", "true");
                }
                hv_is_update = 0;
                if ((int)(hv_is_update) != 0)
                {
                    //pcb model param
                    //pcb_sub_reg_num := 0
                    //pcb_sobel_scale := 0.5
                    //pcb_dark_thresh := 4.0
                    //pcb_light_thresh := 3
                    //vec_model_param.at(1) := [pcb_sub_reg_num, pcb_sobel_scale, pcb_dark_thresh, pcb_light_thresh]
                    //ic model param
                    //ic_sub_reg_num := 0
                    //ic_sobel_scale := 0.5
                    //ic_dark_thresh := 5
                    //ic_light_thresh := 3
                    //vec_model_param.at(2) := [ic_sub_reg_num, ic_sobel_scale, ic_dark_thresh, ic_light_thresh]
                    //JSLF_AOI_update_thresh_image (vec_model_object, pcb_golden_model, ic_golden_model, vec_model_param, iFlag2)
                    //vec_model_object.at(0) := pcb_golden_model
                    //vec_model_object.at(1) := ic_golden_model
                }
                // stop(); only in hdevelop
                //********************** test *********************************
                //这个是存图像的地址，根据需要更改
                hv_data_path = "./0119";
                list_image_files(hv_data_path, "tiff", "recursive", out hv_ImageFiles);
                hv_defect_rows = new HTuple();
                hv_defect_cols = new HTuple();
                for (hv_dir_ind = 1; (int)hv_dir_ind <= (int)((new HTuple(hv_ImageFiles.TupleLength()
                    )) - 1); hv_dir_ind = (int)hv_dir_ind + 1)
                {
                    hv_image_path = hv_ImageFiles.TupleSelect(hv_dir_ind);
                    HOperatorSet.TupleRegexpMatch(hv_image_path, "/(\\d{1,2})-(\\d{1,2})-(\\d{1,2}).tiff",
                        out hv_Matches);
                    HOperatorSet.TupleNumber(hv_Matches, out hv_image_key);
                    ho_Image.Dispose();
                    HOperatorSet.ReadImage(out ho_Image, hv_image_path);
                    HOperatorSet.CountSeconds(out hv_Seconds);
                    for (hv_Index = 1; (int)hv_Index <= 1; hv_Index = (int)hv_Index + 1)
                    {
                        ho_defect_region.Dispose(); ho_gold_wire.Dispose();
                        //JSLF_AOI_inspection(ho_Image, hvec_vec_pcb_golden_object, hvec_vec_ic_golden_object,
                        //    hvec_vec_bond_wire_object, out ho_defect_region, out ho_gold_wire,
                        //    hv_image_key, hvec_vec_pcb_golden_param, hvec_vec_ic_golden_param,
                        //    hvec_vec_pos_param, hvec_vec_bond_wire_param, hvec_vec_epoxy_param,
                        //    hv_R, hv_C, hv_M, hv_N, out hv_defect_r, out hv_defect_c, out hv_defect_type,
                        //    out hv_iFlag, out hv_err_msg);
                    }
                    HOperatorSet.CountSeconds(out hv_Seconds1);
                    hv_dt = hv_Seconds1 - hv_Seconds;
                    if ((int)(new HTuple(hv_iFlag.TupleEqual(0))) != 0)
                    {
                        if (HDevWindowStack.IsOpen())
                        {
                            HOperatorSet.ClearWindow(HDevWindowStack.GetActive());
                        }
                        if (HDevWindowStack.IsOpen())
                        {
                            HOperatorSet.SetLineWidth(HDevWindowStack.GetActive(), 1);
                        }
                        if (HDevWindowStack.IsOpen())
                        {
                            HOperatorSet.SetColor(HDevWindowStack.GetActive(), "green");
                        }
                        if (HDevWindowStack.IsOpen())
                        {
                            HOperatorSet.DispObj(ho_Image, HDevWindowStack.GetActive());
                        }
                        set_display_font(3600, 64, "mono", "true", "false");
                        disp_message(3600, "OK", "window", 12, 12, "green", "true");
                    }
                    else
                    {
                        hv_defect_rows = hv_defect_rows.TupleConcat(hv_defect_r);
                        hv_defect_cols = hv_defect_cols.TupleConcat(hv_defect_c);
                        //smallest_rectangle2 (defect_region, Row, Column, Phi, Length1, Length2)
                        //gen_rectangle2 (Rectangle, Row, Column, Phi, Length1+5, Length2+5)
                        if (HDevWindowStack.IsOpen())
                        {
                            HOperatorSet.ClearWindow(HDevWindowStack.GetActive());
                        }
                        if (HDevWindowStack.IsOpen())
                        {
                            HOperatorSet.DispObj(ho_Image, HDevWindowStack.GetActive());
                        }
                        set_display_font(3600, 64, "mono", "true", "false");
                        disp_message(3600, "NG", "window", 12, 12, "red", "true");
                        if (HDevWindowStack.IsOpen())
                        {
                            HOperatorSet.SetLineWidth(HDevWindowStack.GetActive(), 1);
                        }
                        if (HDevWindowStack.IsOpen())
                        {
                            HOperatorSet.SetColor(HDevWindowStack.GetActive(), "red");
                        }
                        if (HDevWindowStack.IsOpen())
                        {
                            HOperatorSet.DispObj(ho_defect_region, HDevWindowStack.GetActive());
                        }
                        if (HDevWindowStack.IsOpen())
                        {
                            HOperatorSet.SetLineWidth(HDevWindowStack.GetActive(), 1);
                        }
                    }
                    if (HDevWindowStack.IsOpen())
                    {
                        HOperatorSet.SetColor(HDevWindowStack.GetActive(), "green");
                    }
                    if (HDevWindowStack.IsOpen())
                    {
                        HOperatorSet.DispObj(ho_gold_wire, HDevWindowStack.GetActive());
                    }
                    //stop ()
                }
                // stop(); only in hdevelop

                hv_map_row = new HTuple();
                hv_map_col = new HTuple();
                HTuple end_val187 = hv_R - 1;
                HTuple step_val187 = 1;
                for (hv_r = 0; hv_r.Continue(end_val187, step_val187); hv_r = hv_r.TupleAdd(step_val187))
                {
                    HTuple end_val188 = hv_C - 1;
                    HTuple step_val188 = 1;
                    for (hv_c = 0; hv_c.Continue(end_val188, step_val188); hv_c = hv_c.TupleAdd(step_val188))
                    {
                        hv_map_row = hv_map_row.TupleConcat(hv_r);
                        hv_map_col = hv_map_col.TupleConcat(hv_c);
                    }
                }
                hv_map_rect_size = 5;
                HOperatorSet.TupleGenConst(new HTuple(hv_map_row.TupleLength()), 0, out hv_NewtuplePhi);
                HOperatorSet.TupleGenConst(new HTuple(hv_map_row.TupleLength()), hv_map_rect_size - 1,
                    out hv_NewtupleLen);
                ho_map_rect.Dispose();
                HOperatorSet.GenRectangle2(out ho_map_rect, (hv_map_row * (hv_map_rect_size * 2)) + hv_map_rect_size,
                    (hv_map_col * (hv_map_rect_size * 2)) + hv_map_rect_size, hv_NewtuplePhi, hv_NewtupleLen,
                    hv_NewtupleLen);
                HOperatorSet.TupleGenConst(new HTuple(hv_defect_rows.TupleLength()), 0, out hv_NewtuplePhi);
                HOperatorSet.TupleGenConst(new HTuple(hv_defect_rows.TupleLength()), hv_map_rect_size - 1,
                    out hv_NewtupleLen);
                ho_defect_rect.Dispose();
                HOperatorSet.GenRectangle2(out ho_defect_rect, (hv_defect_rows * (hv_map_rect_size * 2)) + hv_map_rect_size,
                    (hv_defect_cols * (hv_map_rect_size * 2)) + hv_map_rect_size, hv_NewtuplePhi,
                    hv_NewtupleLen, hv_NewtupleLen);
                if (HDevWindowStack.IsOpen())
                {
                    HOperatorSet.ClearWindow(HDevWindowStack.GetActive());
                }
                if (HDevWindowStack.IsOpen())
                {
                    HOperatorSet.SetColor(HDevWindowStack.GetActive(), "green");
                }
                if (HDevWindowStack.IsOpen())
                {
                    HOperatorSet.DispObj(ho_map_rect, HDevWindowStack.GetActive());
                }
                if (HDevWindowStack.IsOpen())
                {
                    HOperatorSet.SetColor(HDevWindowStack.GetActive(), "red");
                }
                if (HDevWindowStack.IsOpen())
                {
                    HOperatorSet.DispObj(ho_defect_rect, HDevWindowStack.GetActive());
                }
                //clear all models
                //JSLF_AOI_clear_all_model(hvec_vec_pcb_golden_param, hvec_vec_ic_golden_param,
                //    hvec_vec_bond_wire_param, out hv_iFlag1);
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    hvec_vec_pcb_golden_object.Clear();
                }
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    hvec_vec_ic_golden_object.Clear();
                }
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    hvec_vec_bond_wire_object.Clear();
                }
                //clear_all_matching_models ()
            }
            catch (HalconException HDevExpDefaultException)
            {
                ho_Image.Dispose();
                ho_defect_region.Dispose();
                ho_gold_wire.Dispose();
                ho_map_rect.Dispose();
                ho_defect_rect.Dispose();
                hvec_vec_pcb_golden_object.Dispose();
                hvec_vec_ic_golden_object.Dispose();
                hvec_vec_bond_wire_object.Dispose();
                throw HDevExpDefaultException;
            }

        }
    }

}


//#if !(NO_EXPORT_MAIN || NO_EXPORT_APP_MAIN)
//public class HDevelopExportApp
//{
//    static void Main(string[] args)
//    {
//        new HDevelopExport();
//    }
//}

