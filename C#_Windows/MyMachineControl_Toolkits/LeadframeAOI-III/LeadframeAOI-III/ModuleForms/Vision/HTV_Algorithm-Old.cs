//
//  基础算法，平时基本不需要修改。
//


using HalconDotNet;
namespace HTV_Algorithm
{
    public class HTV
    {
        // Chapter: Filters / Lines
        // Short Description: Calculates the parameters Sigma, Low, and High for lines_gauss from the maximum width and the contrast of the lines to be extracted. 
        private static void calculate_lines_gauss_parameters(HTuple hv_MaxLineWidth, HTuple hv_Contrast,
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
        private static void fit_line_contour_sort(HObject ho_Contours, HTuple hv_RefRow, HTuple hv_RefCol,
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
        // Procedures 
        public static void HTV_create_golden_model(HObject ho_i_ROIs, HObject ho_i_MatchRegion,
            HObject ho_i_InspectRegion, out HObject ho_o_ImgMean, out HObject ho_o_ImgStd,
            HTuple hv_i_IsMultiModel, HTuple hv_i_ImgFiles, HTuple hv_i_BaseImgIdx, HTuple hv_i_IcModelImgIdx,
            HTuple hv_i_ModelType, HTuple hv_i_AngleStart, HTuple hv_i_AngleExt, HTuple hv_i_MinScore,
            HTuple hv_i_MinTrainSet, HTuple hv_i_IsRefine, HTuple hv_i_RefineThresh, HTuple hv_i_IsDrawRegion,
            out HTuple hv_o_ModelID, out HTuple hv_o_ErrCode, out HTuple hv_o_ErrString)
        {




            // Stack for temporary objects 
            HObject[] OTemp = new HObject[20];

            // Local iconic variables 

            HObject ho_ImageWire, ho_ImageIc, ho_Image = null;
            HObject ho_Red = null, ho_Green = null, ho_Blue = null, ho__matchRegion = null;
            HObject ho_DrawRegion = null, ho_Region1 = null, ho_RegionFillUp = null;
            HObject ho_BinImage = null, ho_ImageReduced5 = null, ho__ImgSumArr = null;
            HObject ho__Img2SumArr = null, ho__ROI = null, ho__ImgSum = null;
            HObject ho__Img2Sum = null, ho__ImgArr = null, ho_Image1 = null;
            HObject ho_ImageAffinTrans = null, ho__goldenImage = null, ho_ImageConverted = null;
            HObject ho_ImageResult = null, ho__ImgSumTemp = null, ho__Img2SumTemp = null;
            HObject ho__ImgMean = null, ho__Img2Mean = null, ho_ImageResult1 = null;
            HObject ho__ImgDev = null, ho__ImgStd = null, ho_ImageScaled1 = null;
            HObject ho__ImgLight = null, ho__ImgDark = null, ho__ImgCount = null;
            HObject ho_ObjectSelected = null, ho_ImageReduced1 = null, ho__LightReg = null;
            HObject ho__DarkReg = null, ho_RegionUnion = null, ho_RegionDilation1 = null;
            HObject ho_BinImage1 = null, ho_ImageConverted3 = null, ho_ImageConverted4 = null;
            HObject ho_ImageResult6 = null, ho_ImageResult4 = null, ho_ObjectSelected1 = null;

            HObjectVector hvec__ImgArrVec = new HObjectVector(1);

            // Local control variables 

            HTuple hv_Index1 = new HTuple(), hv_Width = new HTuple();
            HTuple hv_Height = new HTuple(), hv_Index2 = new HTuple();
            HTuple hv__modelID = new HTuple(), hv_Area = new HTuple();
            HTuple hv_Row7 = new HTuple(), hv_Column7 = new HTuple();
            HTuple hv_Number = new HTuple(), hv__TrainSetNum = new HTuple();
            HTuple hv_Index = new HTuple(), hv_i = new HTuple(), hv__row = new HTuple();
            HTuple hv__col = new HTuple(), hv__angle = new HTuple();
            HTuple hv__num = new HTuple(), hv_HomMat2D1 = new HTuple();
            HTuple hv__ModelNum = new HTuple(), hv__Sum = new HTuple();
            HTuple hv_j = new HTuple(), hv_Min = new HTuple(), hv_Max = new HTuple();
            HTuple hv_Range = new HTuple(), hv_Exception = null;
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_o_ImgMean);
            HOperatorSet.GenEmptyObj(out ho_o_ImgStd);
            HOperatorSet.GenEmptyObj(out ho_ImageWire);
            HOperatorSet.GenEmptyObj(out ho_ImageIc);
            HOperatorSet.GenEmptyObj(out ho_Image);
            HOperatorSet.GenEmptyObj(out ho_Red);
            HOperatorSet.GenEmptyObj(out ho_Green);
            HOperatorSet.GenEmptyObj(out ho_Blue);
            HOperatorSet.GenEmptyObj(out ho__matchRegion);
            HOperatorSet.GenEmptyObj(out ho_DrawRegion);
            HOperatorSet.GenEmptyObj(out ho_Region1);
            HOperatorSet.GenEmptyObj(out ho_RegionFillUp);
            HOperatorSet.GenEmptyObj(out ho_BinImage);
            HOperatorSet.GenEmptyObj(out ho_ImageReduced5);
            HOperatorSet.GenEmptyObj(out ho__ImgSumArr);
            HOperatorSet.GenEmptyObj(out ho__Img2SumArr);
            HOperatorSet.GenEmptyObj(out ho__ROI);
            HOperatorSet.GenEmptyObj(out ho__ImgSum);
            HOperatorSet.GenEmptyObj(out ho__Img2Sum);
            HOperatorSet.GenEmptyObj(out ho__ImgArr);
            HOperatorSet.GenEmptyObj(out ho_Image1);
            HOperatorSet.GenEmptyObj(out ho_ImageAffinTrans);
            HOperatorSet.GenEmptyObj(out ho__goldenImage);
            HOperatorSet.GenEmptyObj(out ho_ImageConverted);
            HOperatorSet.GenEmptyObj(out ho_ImageResult);
            HOperatorSet.GenEmptyObj(out ho__ImgSumTemp);
            HOperatorSet.GenEmptyObj(out ho__Img2SumTemp);
            HOperatorSet.GenEmptyObj(out ho__ImgMean);
            HOperatorSet.GenEmptyObj(out ho__Img2Mean);
            HOperatorSet.GenEmptyObj(out ho_ImageResult1);
            HOperatorSet.GenEmptyObj(out ho__ImgDev);
            HOperatorSet.GenEmptyObj(out ho__ImgStd);
            HOperatorSet.GenEmptyObj(out ho_ImageScaled1);
            HOperatorSet.GenEmptyObj(out ho__ImgLight);
            HOperatorSet.GenEmptyObj(out ho__ImgDark);
            HOperatorSet.GenEmptyObj(out ho__ImgCount);
            HOperatorSet.GenEmptyObj(out ho_ObjectSelected);
            HOperatorSet.GenEmptyObj(out ho_ImageReduced1);
            HOperatorSet.GenEmptyObj(out ho__LightReg);
            HOperatorSet.GenEmptyObj(out ho__DarkReg);
            HOperatorSet.GenEmptyObj(out ho_RegionUnion);
            HOperatorSet.GenEmptyObj(out ho_RegionDilation1);
            HOperatorSet.GenEmptyObj(out ho_BinImage1);
            HOperatorSet.GenEmptyObj(out ho_ImageConverted3);
            HOperatorSet.GenEmptyObj(out ho_ImageConverted4);
            HOperatorSet.GenEmptyObj(out ho_ImageResult6);
            HOperatorSet.GenEmptyObj(out ho_ImageResult4);
            HOperatorSet.GenEmptyObj(out ho_ObjectSelected1);
            hv_o_ModelID = new HTuple();
            try
            {
                hv_o_ErrCode = 0;
                hv_o_ErrString = "";
                ho_ImageWire.Dispose();
                HOperatorSet.GenEmptyObj(out ho_ImageWire);
                ho_ImageIc.Dispose();
                HOperatorSet.GenEmptyObj(out ho_ImageIc);
                try
                {
                    for (hv_Index1 = 0; (int)hv_Index1 <= (int)((new HTuple(hv_i_ImgFiles.TupleLength()
                        )) - 1); hv_Index1 = (int)hv_Index1 + 1)
                    {
                        ho_Image.Dispose();
                        HOperatorSet.ReadImage(out ho_Image, hv_i_ImgFiles.TupleSelect(hv_Index1));
                        ho_Red.Dispose(); ho_Green.Dispose(); ho_Blue.Dispose();
                        HOperatorSet.Decompose3(ho_Image, out ho_Red, out ho_Green, out ho_Blue
                            );
                        {
                            HObject ExpTmpOutVar_0;
                            HOperatorSet.ConcatObj(ho_ImageWire, ho_Red, out ExpTmpOutVar_0);
                            ho_ImageWire.Dispose();
                            ho_ImageWire = ExpTmpOutVar_0;
                        }
                        {
                            HObject ExpTmpOutVar_0;
                            HOperatorSet.ConcatObj(ho_ImageIc, ho_Blue, out ExpTmpOutVar_0);
                            ho_ImageIc.Dispose();
                            ho_ImageIc = ExpTmpOutVar_0;
                        }
                    }

                    //1. 读取基准图
                    if ((int)(new HTuple(hv_i_IcModelImgIdx.TupleEqual(1))) != 0)
                    {
                        ho_Image.Dispose();
                        HOperatorSet.SelectObj(ho_ImageWire, out ho_Image, hv_i_BaseImgIdx + 1);
                    }
                    else
                    {
                        ho_Image.Dispose();
                        HOperatorSet.SelectObj(ho_ImageIc, out ho_Image, hv_i_BaseImgIdx + 1);
                    }
                    //2. 创建匹配模板
                    ho__matchRegion.Dispose();
                    HOperatorSet.Union1(ho_i_MatchRegion, out ho__matchRegion);
                    HOperatorSet.GetImageSize(ho_ImageIc, out hv_Width, out hv_Height);
                    if ((int)(hv_i_IsDrawRegion) != 0)
                    {
                        ho_DrawRegion.Dispose();
                        HOperatorSet.GenEmptyObj(out ho_DrawRegion);
                        for (hv_Index2 = 1; (int)hv_Index2 <= 1; hv_Index2 = (int)hv_Index2 + 1)
                        {
                            ho_Region1.Dispose();
                            HOperatorSet.DrawRegion(out ho_Region1, 3600);
                            {
                                HObject ExpTmpOutVar_0;
                                HOperatorSet.Union2(ho_DrawRegion, ho_Region1, out ExpTmpOutVar_0);
                                ho_DrawRegion.Dispose();
                                ho_DrawRegion = ExpTmpOutVar_0;
                            }
                        }
                        ho_RegionFillUp.Dispose();
                        HOperatorSet.FillUp(ho_DrawRegion, out ho_RegionFillUp);
                        ho_BinImage.Dispose();
                        HOperatorSet.RegionToBin(ho_RegionFillUp, out ho_BinImage, 255, 0, hv_Width.TupleSelect(
                            0), hv_Height.TupleSelect(0));
                        ho_ImageReduced5.Dispose();
                        HOperatorSet.ReduceDomain(ho_BinImage, ho_i_MatchRegion, out ho_ImageReduced5
                            );
                        HOperatorSet.CreateShapeModel(ho_ImageReduced5, "auto", hv_i_AngleStart,
                            hv_i_AngleExt, "auto", "auto", "use_polarity", "auto", "auto", out hv__modelID);
                        HOperatorSet.AreaCenter(ho_ImageReduced5, out hv_Area, out hv_Row7, out hv_Column7);
                        HOperatorSet.SetShapeModelOrigin(hv__modelID, -hv_Row7, -hv_Column7);
                    }
                    else
                    {
                        HTV_create_model(ho_Image, ho__matchRegion, hv_i_ModelType, hv_i_AngleStart,
                            hv_i_AngleExt, out hv__modelID);
                    }

                    //3 从所有图中逐个匹配搜索
                    //(3.1) 初始化
                    HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height);
                    ho_o_ImgMean.Dispose();
                    HOperatorSet.GenEmptyObj(out ho_o_ImgMean);
                    ho_o_ImgStd.Dispose();
                    HOperatorSet.GenEmptyObj(out ho_o_ImgStd);
                    HOperatorSet.CountObj(ho_i_ROIs, out hv_Number);
                    ho__ImgSumArr.Dispose();
                    HOperatorSet.GenEmptyObj(out ho__ImgSumArr);
                    ho__Img2SumArr.Dispose();
                    HOperatorSet.GenEmptyObj(out ho__Img2SumArr);
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        hvec__ImgArrVec = dh.Add(new HObjectVector(1));
                    }

                    HOperatorSet.TupleGenConst(hv_Number, 0, out hv__TrainSetNum);
                    //(3.2) 根据ROI区域的个数逐个匹配,并将匹配到的图位置移动到基础图对应位置
                    //旧的思路是将匹配区域扩大一圈搜索,现在思路是直接输入搜索区域HTV_match_region_dilation (_matchRegion, RegionDilation, i_MatchDilation)
                    //reduce_domain (Image, i_InspectRegion, ImageReduced)
                    //edges_sub_pix (ImageReduced, Edges, 'canny', 1, 20, 40)
                    HTuple end_val51 = hv_Number - 1;
                    HTuple step_val51 = 1;
                    for (hv_Index = 0; hv_Index.Continue(end_val51, step_val51); hv_Index = hv_Index.TupleAdd(step_val51))
                    {
                        ho__ROI.Dispose();
                        HOperatorSet.SelectObj(ho_i_ROIs, out ho__ROI, hv_Index + 1);
                        ho__ImgSum.Dispose();
                        HTV_gen_image_type(ho_Image, out ho__ImgSum, "real");
                        ho__Img2Sum.Dispose();
                        HTV_gen_image_type(ho_Image, out ho__Img2Sum, "real");
                        ho__ImgArr.Dispose();
                        HOperatorSet.GenEmptyObj(out ho__ImgArr);
                        for (hv_i = 0; (int)hv_i <= (int)((new HTuple(hv_i_ImgFiles.TupleLength())) - 1); hv_i = (int)hv_i + 1)
                        {
                            if ((int)(new HTuple(hv_i_IcModelImgIdx.TupleEqual(1))) != 0)
                            {
                                ho_Image.Dispose();
                                HOperatorSet.SelectObj(ho_ImageWire, out ho_Image, hv_i + 1);
                            }
                            else
                            {
                                ho_Image.Dispose();
                                HOperatorSet.SelectObj(ho_ImageIc, out ho_Image, hv_i + 1);
                            }

                            ho_Image1.Dispose();
                            HOperatorSet.SelectObj(ho_ImageIc, out ho_Image1, hv_i + 1);
                            HTuple hv__score;
                            HTV_find_model(ho_Image, ho__ROI, hv__modelID, hv_i_ModelType, -0.1,
                                0.2, hv_i_MinScore, 1, out hv__row, out hv__col, out hv__angle, out hv__num, out hv__score);
                            if ((int)(new HTuple(hv__num.TupleGreater(0))) != 0)
                            {
                                if (hv__TrainSetNum == null)
                                    hv__TrainSetNum = new HTuple();
                                hv__TrainSetNum[hv_Index] = (hv__TrainSetNum.TupleSelect(hv_Index)) + 1;
                                //vector_angle_to_rigid (0, 0, 0, _row, _col, _angle, HomMat2D)
                                //affine_trans_contour_xld (Edges, ContoursAffinTrans, HomMat2D)
                                HOperatorSet.VectorAngleToRigid(hv__row, hv__col, hv__angle, 0, 0,
                                    0, out hv_HomMat2D1);
                                ho_ImageAffinTrans.Dispose();
                                HOperatorSet.AffineTransImage(ho_Image1, out ho_ImageAffinTrans, hv_HomMat2D1,
                                    "nearest_neighbor", "false");
                                ho__goldenImage.Dispose();
                                HOperatorSet.ReduceDomain(ho_ImageAffinTrans, ho_i_InspectRegion, out ho__goldenImage
                                    );
                                ho_ImageConverted.Dispose();
                                HOperatorSet.ConvertImageType(ho__goldenImage, out ho_ImageConverted,
                                    "real");
                                {
                                    HObject ExpTmpOutVar_0;
                                    HOperatorSet.AddImage(ho__ImgSum, ho_ImageConverted, out ExpTmpOutVar_0,
                                        1.0, 0);
                                    ho__ImgSum.Dispose();
                                    ho__ImgSum = ExpTmpOutVar_0;
                                }
                                ho_ImageResult.Dispose();
                                HOperatorSet.MultImage(ho_ImageConverted, ho_ImageConverted, out ho_ImageResult,
                                    1.0, 0);
                                {
                                    HObject ExpTmpOutVar_0;
                                    HOperatorSet.AddImage(ho__Img2Sum, ho_ImageResult, out ExpTmpOutVar_0,
                                        1.0, 0);
                                    ho__Img2Sum.Dispose();
                                    ho__Img2Sum = ExpTmpOutVar_0;
                                }
                                {
                                    HObject ExpTmpOutVar_0;
                                    HOperatorSet.ConcatObj(ho__ImgArr, ho__goldenImage, out ExpTmpOutVar_0
                                        );
                                    ho__ImgArr.Dispose();
                                    ho__ImgArr = ExpTmpOutVar_0;
                                }
                            }
                        }
                        {
                            HObject ExpTmpOutVar_0;
                            HOperatorSet.ConcatObj(ho__ImgSumArr, ho__ImgSum, out ExpTmpOutVar_0);
                            ho__ImgSumArr.Dispose();
                            ho__ImgSumArr = ExpTmpOutVar_0;
                        }
                        {
                            HObject ExpTmpOutVar_0;
                            HOperatorSet.ConcatObj(ho__Img2SumArr, ho__Img2Sum, out ExpTmpOutVar_0);
                            ho__Img2SumArr.Dispose();
                            ho__Img2SumArr = ExpTmpOutVar_0;
                        }
                        hvec__ImgArrVec[hv_Index] = new HObjectVector((ho__ImgArr).CopyObj(1, -1));
                    }
                    HTV_clear_model(hv__modelID, hv_i_ModelType);
                    //4 处理生成平均图和标准差图，生成之前先确认样本数量是否足够
                    //如果是多模板模式则分别计算均值和标准差，否则融合在一起
                    hv__ModelNum = hv_Number.Clone();
                    //这里的小trick是如果非多模板模式就把这些融合起来的东西放在上面数组的第一个位置，然后控制后面的for循环做一次即可
                    if ((int)(new HTuple(hv_i_IsMultiModel.TupleEqual(0))) != 0)
                    {
                        HOperatorSet.TupleSum(hv__TrainSetNum, out hv__Sum);
                        if ((int)(new HTuple(hv__Sum.TupleLess(hv_i_MinTrainSet))) != 0)
                        {
                            hv_o_ErrCode = -1;
                            hv_o_ErrString = "Training set not enough";
                            ho_ImageWire.Dispose();
                            ho_ImageIc.Dispose();
                            ho_Image.Dispose();
                            ho_Red.Dispose();
                            ho_Green.Dispose();
                            ho_Blue.Dispose();
                            ho__matchRegion.Dispose();
                            ho_DrawRegion.Dispose();
                            ho_Region1.Dispose();
                            ho_RegionFillUp.Dispose();
                            ho_BinImage.Dispose();
                            ho_ImageReduced5.Dispose();
                            ho__ImgSumArr.Dispose();
                            ho__Img2SumArr.Dispose();
                            ho__ROI.Dispose();
                            ho__ImgSum.Dispose();
                            ho__Img2Sum.Dispose();
                            ho__ImgArr.Dispose();
                            ho_Image1.Dispose();
                            ho_ImageAffinTrans.Dispose();
                            ho__goldenImage.Dispose();
                            ho_ImageConverted.Dispose();
                            ho_ImageResult.Dispose();
                            ho__ImgSumTemp.Dispose();
                            ho__Img2SumTemp.Dispose();
                            ho__ImgMean.Dispose();
                            ho__Img2Mean.Dispose();
                            ho_ImageResult1.Dispose();
                            ho__ImgDev.Dispose();
                            ho__ImgStd.Dispose();
                            ho_ImageScaled1.Dispose();
                            ho__ImgLight.Dispose();
                            ho__ImgDark.Dispose();
                            ho__ImgCount.Dispose();
                            ho_ObjectSelected.Dispose();
                            ho_ImageReduced1.Dispose();
                            ho__LightReg.Dispose();
                            ho__DarkReg.Dispose();
                            ho_RegionUnion.Dispose();
                            ho_RegionDilation1.Dispose();
                            ho_BinImage1.Dispose();
                            ho_ImageConverted3.Dispose();
                            ho_ImageConverted4.Dispose();
                            ho_ImageResult6.Dispose();
                            ho_ImageResult4.Dispose();
                            ho_ObjectSelected1.Dispose();
                            hvec__ImgArrVec.Dispose();

                            return;
                        }
                        if (hv__TrainSetNum == null)
                            hv__TrainSetNum = new HTuple();
                        hv__TrainSetNum[0] = hv__Sum;
                        ho__ImgArr.Dispose();
                        HOperatorSet.GenEmptyObj(out ho__ImgArr);
                        ho__ImgSumTemp.Dispose();
                        HTV_gen_image_type(ho_Image, out ho__ImgSumTemp, "real");
                        ho__Img2SumTemp.Dispose();
                        HTV_gen_image_type(ho_Image, out ho__Img2SumTemp, "real");
                        HTuple end_val99 = hv_Number - 1;
                        HTuple step_val99 = 1;
                        for (hv_i = 0; hv_i.Continue(end_val99, step_val99); hv_i = hv_i.TupleAdd(step_val99))
                        {
                            ho__ImgSum.Dispose();
                            HOperatorSet.SelectObj(ho__ImgSumArr, out ho__ImgSum, hv_i + 1);
                            ho__Img2Sum.Dispose();
                            HOperatorSet.SelectObj(ho__Img2SumArr, out ho__Img2Sum, hv_i + 1);
                            {
                                HObject ExpTmpOutVar_0;
                                HOperatorSet.AddImage(ho__ImgSumTemp, ho__ImgSum, out ExpTmpOutVar_0,
                                    1.0, 0);
                                ho__ImgSumTemp.Dispose();
                                ho__ImgSumTemp = ExpTmpOutVar_0;
                            }
                            {
                                HObject ExpTmpOutVar_0;
                                HOperatorSet.AddImage(ho__Img2SumTemp, ho__Img2Sum, out ExpTmpOutVar_0,
                                    1.0, 0);
                                ho__Img2SumTemp.Dispose();
                                ho__Img2SumTemp = ExpTmpOutVar_0;
                            }
                            using (HDevDisposeHelper dh = new HDevDisposeHelper())
                            {
                                HObject ExpTmpOutVar_0;
                                HOperatorSet.ConcatObj(ho__ImgArr, hvec__ImgArrVec[hv_i].O, out ExpTmpOutVar_0
                                    );
                                ho__ImgArr.Dispose();
                                ho__ImgArr = ExpTmpOutVar_0;
                            }
                        }
                        ho__ImgSumArr.Dispose();
                        HOperatorSet.CopyObj(ho__ImgSumTemp, out ho__ImgSumArr, 1, 1);
                        ho__Img2SumArr.Dispose();
                        HOperatorSet.CopyObj(ho__Img2SumTemp, out ho__Img2SumArr, 1, 1);
                        hvec__ImgArrVec[0] = new HObjectVector((ho__ImgArr).CopyObj(1, -1));
                        hv__ModelNum = 1;
                    }
                    HTuple end_val111 = hv__ModelNum - 1;
                    HTuple step_val111 = 1;
                    for (hv_i = 0; hv_i.Continue(end_val111, step_val111); hv_i = hv_i.TupleAdd(step_val111))
                    {
                        if ((int)(new HTuple(((hv__TrainSetNum.TupleSelect(hv_i))).TupleLess(hv_i_MinTrainSet))) != 0)
                        {
                            hv_o_ErrCode = -1;
                            hv_o_ErrString = "Training set not enough";
                            ho_ImageWire.Dispose();
                            ho_ImageIc.Dispose();
                            ho_Image.Dispose();
                            ho_Red.Dispose();
                            ho_Green.Dispose();
                            ho_Blue.Dispose();
                            ho__matchRegion.Dispose();
                            ho_DrawRegion.Dispose();
                            ho_Region1.Dispose();
                            ho_RegionFillUp.Dispose();
                            ho_BinImage.Dispose();
                            ho_ImageReduced5.Dispose();
                            ho__ImgSumArr.Dispose();
                            ho__Img2SumArr.Dispose();
                            ho__ROI.Dispose();
                            ho__ImgSum.Dispose();
                            ho__Img2Sum.Dispose();
                            ho__ImgArr.Dispose();
                            ho_Image1.Dispose();
                            ho_ImageAffinTrans.Dispose();
                            ho__goldenImage.Dispose();
                            ho_ImageConverted.Dispose();
                            ho_ImageResult.Dispose();
                            ho__ImgSumTemp.Dispose();
                            ho__Img2SumTemp.Dispose();
                            ho__ImgMean.Dispose();
                            ho__Img2Mean.Dispose();
                            ho_ImageResult1.Dispose();
                            ho__ImgDev.Dispose();
                            ho__ImgStd.Dispose();
                            ho_ImageScaled1.Dispose();
                            ho__ImgLight.Dispose();
                            ho__ImgDark.Dispose();
                            ho__ImgCount.Dispose();
                            ho_ObjectSelected.Dispose();
                            ho_ImageReduced1.Dispose();
                            ho__LightReg.Dispose();
                            ho__DarkReg.Dispose();
                            ho_RegionUnion.Dispose();
                            ho_RegionDilation1.Dispose();
                            ho_BinImage1.Dispose();
                            ho_ImageConverted3.Dispose();
                            ho_ImageConverted4.Dispose();
                            ho_ImageResult6.Dispose();
                            ho_ImageResult4.Dispose();
                            ho_ObjectSelected1.Dispose();
                            hvec__ImgArrVec.Dispose();

                            return;
                        }
                        ho__ImgSum.Dispose();
                        HOperatorSet.SelectObj(ho__ImgSumArr, out ho__ImgSum, hv_i + 1);
                        ho__Img2Sum.Dispose();
                        HOperatorSet.SelectObj(ho__Img2SumArr, out ho__Img2Sum, hv_i + 1);
                        ho__ImgMean.Dispose();
                        HOperatorSet.ScaleImage(ho__ImgSum, out ho__ImgMean, 1.0 / (hv__TrainSetNum.TupleSelect(
                            hv_i)), 0);
                        ho__Img2Mean.Dispose();
                        HOperatorSet.ScaleImage(ho__Img2Sum, out ho__Img2Mean, 1.0 / (hv__TrainSetNum.TupleSelect(
                            hv_i)), 0);
                        ho_ImageResult1.Dispose();
                        HOperatorSet.MultImage(ho__ImgMean, ho__ImgMean, out ho_ImageResult1, 1.0,
                            0);
                        ho__ImgDev.Dispose();
                        HOperatorSet.SubImage(ho__Img2Mean, ho_ImageResult1, out ho__ImgDev, 1.0,
                            0);
                        ho__ImgStd.Dispose();
                        HOperatorSet.SqrtImage(ho__ImgDev, out ho__ImgStd);
                        if ((int)(hv_i_IsRefine) != 0)
                        {
                            ho_ImageScaled1.Dispose();
                            HOperatorSet.ScaleImage(ho__ImgStd, out ho_ImageScaled1, hv_i_RefineThresh,
                                0);
                            ho__ImgLight.Dispose();
                            HOperatorSet.AddImage(ho__ImgMean, ho_ImageScaled1, out ho__ImgLight,
                                1.0, 0);
                            {
                                HObject ExpTmpOutVar_0;
                                HOperatorSet.ConvertImageType(ho__ImgLight, out ExpTmpOutVar_0, "byte");
                                ho__ImgLight.Dispose();
                                ho__ImgLight = ExpTmpOutVar_0;
                            }
                            ho__ImgDark.Dispose();
                            HOperatorSet.SubImage(ho__ImgMean, ho_ImageScaled1, out ho__ImgDark,
                                1.0, 0);
                            {
                                HObject ExpTmpOutVar_0;
                                HOperatorSet.ConvertImageType(ho__ImgDark, out ExpTmpOutVar_0, "byte");
                                ho__ImgDark.Dispose();
                                ho__ImgDark = ExpTmpOutVar_0;
                            }
                            ho__ImgCount.Dispose();
                            HTV_gen_image_type(ho_Image, out ho__ImgCount, "real");
                            ho__ImgSum.Dispose();
                            HTV_gen_image_type(ho_Image, out ho__ImgSum, "real");
                            ho__Img2Sum.Dispose();
                            HTV_gen_image_type(ho_Image, out ho__Img2Sum, "real");
                            ho__ImgArr.Dispose();
                            using (HDevDisposeHelper dh = new HDevDisposeHelper())
                            {
                                ho__ImgArr = hvec__ImgArrVec[hv_i].O.CopyObj(1, -1);
                            }
                            HTuple end_val134 = hv__TrainSetNum.TupleSelect(
                                hv_i);
                            HTuple step_val134 = 1;
                            for (hv_j = 1; hv_j.Continue(end_val134, step_val134); hv_j = hv_j.TupleAdd(step_val134))
                            {
                                ho_ObjectSelected.Dispose();
                                HOperatorSet.SelectObj(ho__ImgArr, out ho_ObjectSelected, hv_j);
                                ho_ImageReduced1.Dispose();
                                HOperatorSet.ReduceDomain(ho_ObjectSelected, ho_i_InspectRegion, out ho_ImageReduced1
                                    );
                                ho__LightReg.Dispose();
                                HOperatorSet.DynThreshold(ho_ImageReduced1, ho__ImgLight, out ho__LightReg,
                                    1, "light");
                                ho__DarkReg.Dispose();
                                HOperatorSet.DynThreshold(ho_ImageReduced1, ho__ImgDark, out ho__DarkReg,
                                    1, "dark");
                                ho_RegionUnion.Dispose();
                                HOperatorSet.Union2(ho__LightReg, ho__DarkReg, out ho_RegionUnion);
                                ho_RegionDilation1.Dispose();
                                HOperatorSet.DilationCircle(ho_RegionUnion, out ho_RegionDilation1,
                                    3.5);
                                //生成一个前景为0，背景为1的图，这样与原图相乘之后，前景区域部分就没有了
                                ho_BinImage1.Dispose();
                                HOperatorSet.RegionToBin(ho_RegionDilation1, out ho_BinImage1, 0, 1,
                                    hv_Width, hv_Height);
                                ho_ImageConverted3.Dispose();
                                HOperatorSet.ConvertImageType(ho_BinImage1, out ho_ImageConverted3,
                                    "real");
                                ho_ImageConverted4.Dispose();
                                HOperatorSet.ConvertImageType(ho_ImageReduced1, out ho_ImageConverted4,
                                    "real");
                                ho_ImageResult6.Dispose();
                                HOperatorSet.MultImage(ho_ImageConverted3, ho_ImageConverted4, out ho_ImageResult6,
                                    1.0, 0);

                                {
                                    HObject ExpTmpOutVar_0;
                                    HOperatorSet.AddImage(ho__ImgSum, ho_ImageResult6, out ExpTmpOutVar_0,
                                        1.0, 0);
                                    ho__ImgSum.Dispose();
                                    ho__ImgSum = ExpTmpOutVar_0;
                                }
                                ho_ImageResult4.Dispose();
                                HOperatorSet.MultImage(ho_ImageResult6, ho_ImageResult6, out ho_ImageResult4,
                                    1.0, 0);
                                {
                                    HObject ExpTmpOutVar_0;
                                    HOperatorSet.AddImage(ho__Img2Sum, ho_ImageResult4, out ExpTmpOutVar_0,
                                        1.0, 0);
                                    ho__Img2Sum.Dispose();
                                    ho__Img2Sum = ExpTmpOutVar_0;
                                }
                                {
                                    HObject ExpTmpOutVar_0;
                                    HOperatorSet.AddImage(ho__ImgCount, ho_ImageConverted3, out ExpTmpOutVar_0,
                                        1.0, 0);
                                    ho__ImgCount.Dispose();
                                    ho__ImgCount = ExpTmpOutVar_0;
                                }
                            }
                            HOperatorSet.MinMaxGray(ho_i_InspectRegion, ho__ImgCount, 0, out hv_Min,
                                out hv_Max, out hv_Range);
                            if ((int)(new HTuple(hv_Min.TupleLess(hv_i_MinTrainSet))) != 0)
                            {
                                hv_o_ErrCode = -2;
                                hv_o_ErrString = "Training set not enough after removing points";
                                ho_ImageWire.Dispose();
                                ho_ImageIc.Dispose();
                                ho_Image.Dispose();
                                ho_Red.Dispose();
                                ho_Green.Dispose();
                                ho_Blue.Dispose();
                                ho__matchRegion.Dispose();
                                ho_DrawRegion.Dispose();
                                ho_Region1.Dispose();
                                ho_RegionFillUp.Dispose();
                                ho_BinImage.Dispose();
                                ho_ImageReduced5.Dispose();
                                ho__ImgSumArr.Dispose();
                                ho__Img2SumArr.Dispose();
                                ho__ROI.Dispose();
                                ho__ImgSum.Dispose();
                                ho__Img2Sum.Dispose();
                                ho__ImgArr.Dispose();
                                ho_Image1.Dispose();
                                ho_ImageAffinTrans.Dispose();
                                ho__goldenImage.Dispose();
                                ho_ImageConverted.Dispose();
                                ho_ImageResult.Dispose();
                                ho__ImgSumTemp.Dispose();
                                ho__Img2SumTemp.Dispose();
                                ho__ImgMean.Dispose();
                                ho__Img2Mean.Dispose();
                                ho_ImageResult1.Dispose();
                                ho__ImgDev.Dispose();
                                ho__ImgStd.Dispose();
                                ho_ImageScaled1.Dispose();
                                ho__ImgLight.Dispose();
                                ho__ImgDark.Dispose();
                                ho__ImgCount.Dispose();
                                ho_ObjectSelected.Dispose();
                                ho_ImageReduced1.Dispose();
                                ho__LightReg.Dispose();
                                ho__DarkReg.Dispose();
                                ho_RegionUnion.Dispose();
                                ho_RegionDilation1.Dispose();
                                ho_BinImage1.Dispose();
                                ho_ImageConverted3.Dispose();
                                ho_ImageConverted4.Dispose();
                                ho_ImageResult6.Dispose();
                                ho_ImageResult4.Dispose();
                                ho_ObjectSelected1.Dispose();
                                hvec__ImgArrVec.Dispose();

                                return;
                            }
                            ho__ImgMean.Dispose();
                            HOperatorSet.DivImage(ho__ImgSum, ho__ImgCount, out ho__ImgMean, 1.0,
                                0);
                            ho__Img2Mean.Dispose();
                            HOperatorSet.DivImage(ho__Img2Sum, ho__ImgCount, out ho__Img2Mean, 1.0,
                                0);
                            ho_ImageResult1.Dispose();
                            HOperatorSet.MultImage(ho__ImgMean, ho__ImgMean, out ho_ImageResult1,
                                1.0, 0);
                            ho__ImgDev.Dispose();
                            HOperatorSet.SubImage(ho__Img2Mean, ho_ImageResult1, out ho__ImgDev,
                                1.0, 0);
                            ho__ImgStd.Dispose();
                            HOperatorSet.SqrtImage(ho__ImgDev, out ho__ImgStd);
                        }
                        {
                            HObject ExpTmpOutVar_0;
                            HOperatorSet.ConcatObj(ho_o_ImgMean, ho__ImgMean, out ExpTmpOutVar_0);
                            ho_o_ImgMean.Dispose();
                            ho_o_ImgMean = ExpTmpOutVar_0;
                        }
                        {
                            HObject ExpTmpOutVar_0;
                            HOperatorSet.ConcatObj(ho_o_ImgStd, ho__ImgStd, out ExpTmpOutVar_0);
                            ho_o_ImgStd.Dispose();
                            ho_o_ImgStd = ExpTmpOutVar_0;
                        }
                    }
                    //6 根据黄金图生成匹配模板
                    ho_ObjectSelected1.Dispose();
                    HOperatorSet.SelectObj(ho_o_ImgMean, out ho_ObjectSelected1, 1);
                    ho__ImgMean.Dispose();
                    HOperatorSet.ConvertImageType(ho_ObjectSelected1, out ho__ImgMean, "byte");
                    HTV_create_model(ho_Image, ho__matchRegion, hv_i_ModelType, hv_i_AngleStart,
                        hv_i_AngleExt, out hv_o_ModelID);
                }
                // catch (Exception) 
                catch (HalconException HDevExpDefaultException1)
                {
                    HDevExpDefaultException1.ToHTuple(out hv_Exception);
                    hv_o_ErrString = hv_Exception.Clone();
                    hv_o_ErrCode = -1;
                }
                ho_ImageWire.Dispose();
                ho_ImageIc.Dispose();
                ho_Image.Dispose();
                ho_Red.Dispose();
                ho_Green.Dispose();
                ho_Blue.Dispose();
                ho__matchRegion.Dispose();
                ho_DrawRegion.Dispose();
                ho_Region1.Dispose();
                ho_RegionFillUp.Dispose();
                ho_BinImage.Dispose();
                ho_ImageReduced5.Dispose();
                ho__ImgSumArr.Dispose();
                ho__Img2SumArr.Dispose();
                ho__ROI.Dispose();
                ho__ImgSum.Dispose();
                ho__Img2Sum.Dispose();
                ho__ImgArr.Dispose();
                ho_Image1.Dispose();
                ho_ImageAffinTrans.Dispose();
                ho__goldenImage.Dispose();
                ho_ImageConverted.Dispose();
                ho_ImageResult.Dispose();
                ho__ImgSumTemp.Dispose();
                ho__Img2SumTemp.Dispose();
                ho__ImgMean.Dispose();
                ho__Img2Mean.Dispose();
                ho_ImageResult1.Dispose();
                ho__ImgDev.Dispose();
                ho__ImgStd.Dispose();
                ho_ImageScaled1.Dispose();
                ho__ImgLight.Dispose();
                ho__ImgDark.Dispose();
                ho__ImgCount.Dispose();
                ho_ObjectSelected.Dispose();
                ho_ImageReduced1.Dispose();
                ho__LightReg.Dispose();
                ho__DarkReg.Dispose();
                ho_RegionUnion.Dispose();
                ho_RegionDilation1.Dispose();
                ho_BinImage1.Dispose();
                ho_ImageConverted3.Dispose();
                ho_ImageConverted4.Dispose();
                ho_ImageResult6.Dispose();
                ho_ImageResult4.Dispose();
                ho_ObjectSelected1.Dispose();
                hvec__ImgArrVec.Dispose();

                return;
            }
            catch (HalconException HDevExpDefaultException)
            {
                ho_ImageWire.Dispose();
                ho_ImageIc.Dispose();
                ho_Image.Dispose();
                ho_Red.Dispose();
                ho_Green.Dispose();
                ho_Blue.Dispose();
                ho__matchRegion.Dispose();
                ho_DrawRegion.Dispose();
                ho_Region1.Dispose();
                ho_RegionFillUp.Dispose();
                ho_BinImage.Dispose();
                ho_ImageReduced5.Dispose();
                ho__ImgSumArr.Dispose();
                ho__Img2SumArr.Dispose();
                ho__ROI.Dispose();
                ho__ImgSum.Dispose();
                ho__Img2Sum.Dispose();
                ho__ImgArr.Dispose();
                ho_Image1.Dispose();
                ho_ImageAffinTrans.Dispose();
                ho__goldenImage.Dispose();
                ho_ImageConverted.Dispose();
                ho_ImageResult.Dispose();
                ho__ImgSumTemp.Dispose();
                ho__Img2SumTemp.Dispose();
                ho__ImgMean.Dispose();
                ho__Img2Mean.Dispose();
                ho_ImageResult1.Dispose();
                ho__ImgDev.Dispose();
                ho__ImgStd.Dispose();
                ho_ImageScaled1.Dispose();
                ho__ImgLight.Dispose();
                ho__ImgDark.Dispose();
                ho__ImgCount.Dispose();
                ho_ObjectSelected.Dispose();
                ho_ImageReduced1.Dispose();
                ho__LightReg.Dispose();
                ho__DarkReg.Dispose();
                ho_RegionUnion.Dispose();
                ho_RegionDilation1.Dispose();
                ho_BinImage1.Dispose();
                ho_ImageConverted3.Dispose();
                ho_ImageConverted4.Dispose();
                ho_ImageResult6.Dispose();
                ho_ImageResult4.Dispose();
                ho_ObjectSelected1.Dispose();
                hvec__ImgArrVec.Dispose();

                throw HDevExpDefaultException;
            }
        }

        public static void HTV_create_model(HObject ho_i_Image, HObject ho_i_ROI, HTuple hv_i_ModelType,
            HTuple hv_i_AngleStart, HTuple hv_i_AngleExt, out HTuple hv_o_ModelID)
        {




            // Local iconic variables 

            HObject ho_ImageReduced, ho_Edges;

            // Local control variables 

            HTuple hv_Area = null, hv__regionRow = null;
            HTuple hv__regionCol = null;
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_ImageReduced);
            HOperatorSet.GenEmptyObj(out ho_Edges);
            hv_o_ModelID = new HTuple();
            try
            {
                ho_ImageReduced.Dispose();
                HOperatorSet.ReduceDomain(ho_i_Image, ho_i_ROI, out ho_ImageReduced);
                ho_Edges.Dispose();
                HOperatorSet.EdgesSubPix(ho_ImageReduced, out ho_Edges, "canny", 1, 20, 40);

                HOperatorSet.AreaCenter(ho_i_ROI, out hv_Area, out hv__regionRow, out hv__regionCol);
                if ((int)(new HTuple(hv_i_ModelType.TupleEqual("ncc"))) != 0)
                {
                    HOperatorSet.CreateNccModel(ho_ImageReduced, "auto", hv_i_AngleStart, hv_i_AngleExt,
                        "auto", "use_polarity", out hv_o_ModelID);
                    HOperatorSet.SetNccModelOrigin(hv_o_ModelID, -hv__regionRow, -hv__regionCol);
                }
                else if ((int)(new HTuple(hv_i_ModelType.TupleEqual("shape"))) != 0)
                {
                    HOperatorSet.CreateShapeModel(ho_ImageReduced, "auto", hv_i_AngleStart, hv_i_AngleExt,
                        "auto", "auto", "use_polarity", "auto", "auto", out hv_o_ModelID);
                    HOperatorSet.SetShapeModelOrigin(hv_o_ModelID, -hv__regionRow, -hv__regionCol);
                }
                else
                {
                    throw new HalconException("Wrong argument [modelType]=" + hv_i_ModelType);
                }
                ho_ImageReduced.Dispose();
                ho_Edges.Dispose();

                return;
            }
            catch (HalconException HDevExpDefaultException)
            {
                ho_ImageReduced.Dispose();
                ho_Edges.Dispose();

                throw HDevExpDefaultException;
            }
        }

        public static void HTV_read_model(HTuple hv_i_ModelDir, out HTuple hv_o_ModelID, out HTuple hv_o_ModelType)
        {



            // Local iconic variables 

            // Local control variables 

            HTuple hv__tup = null;
            // Initialize local and output iconic variables 
            hv_o_ModelID = new HTuple();
            HOperatorSet.ReadTuple(hv_i_ModelDir + "Model_Type.tup", out hv__tup);
            hv_o_ModelType = hv__tup[0];
            if ((int)(new HTuple(hv_o_ModelType.TupleEqual("ncc"))) != 0)
            {
                HOperatorSet.ReadNccModel(hv_i_ModelDir + "Model.dat", out hv_o_ModelID);
            }
            else if ((int)(new HTuple(hv_o_ModelType.TupleEqual("shape"))) != 0)
            {
                HOperatorSet.ReadShapeModel(hv_i_ModelDir + "Model.dat", out hv_o_ModelID);
            }
            else
            {
                throw new HalconException("Wrong argument [modelType]=" + hv_o_ModelType);
            }

            return;
        }

        public static void HTV_gen_image_type(HObject ho_i_Image, out HObject ho_o_Image, HTuple hv_i_Type)
        {




            // Local iconic variables 

            HObject ho_ImageCleared;
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_o_Image);
            HOperatorSet.GenEmptyObj(out ho_ImageCleared);
            try
            {
                ho_ImageCleared.Dispose();
                HOperatorSet.GenImageProto(ho_i_Image, out ho_ImageCleared, 0);
                ho_o_Image.Dispose();
                HOperatorSet.ConvertImageType(ho_ImageCleared, out ho_o_Image, hv_i_Type);
                ho_ImageCleared.Dispose();

                return;
            }
            catch (HalconException HDevExpDefaultException)
            {
                ho_ImageCleared.Dispose();

                throw HDevExpDefaultException;
            }
        }

        public static void HTV_inspect_expoxy_distance(HObject ho_i_Image, HObject ho_i_InspectReg,
            out HObject ho_o_DefectRegion, HTuple hv_i_EpoxyInspectSize, HTuple hv_i_EpoxyEdgeSigma,
            HTuple hv_i_EpoxyEdgeThresh, HTuple hv_i_EpoxyDarkLight, HTuple hv_i_EpoxyDistThresh,
            out HTuple hv_o_ErrCode, out HTuple hv_o_ErrString, out HTuple hv_o_EpoxyResult)
        {




            // Local iconic variables 

            HObject ho_Cross = null, ho_Rectangle = null;

            // Local control variables 

            HTuple hv_o_ErrStr = null, hv_EpoxyRow = new HTuple();
            HTuple hv_EpoxyCol = new HTuple(), hv_Width = new HTuple();
            HTuple hv_Height = new HTuple(), hv_Row = new HTuple();
            HTuple hv_Column = new HTuple(), hv_Phi = new HTuple();
            HTuple hv_Length1 = new HTuple(), hv_Length2 = new HTuple();
            HTuple hv_MeasureHandle = new HTuple(), hv_RowEdge = new HTuple();
            HTuple hv_ColumnEdge = new HTuple(), hv_Amplitude = new HTuple();
            HTuple hv_Distance = new HTuple(), hv_Greater = new HTuple();
            HTuple hv_FirstIndex = new HTuple(), hv_LastIndex = new HTuple();
            HTuple hv_RowTuple = new HTuple(), hv_ColTuple = new HTuple();
            HTuple hv_Distance1 = new HTuple(), hv_Length = new HTuple();
            HTuple hv_EpoxyDist = new HTuple(), hv_GreaterEpoxy = new HTuple();
            HTuple hv_Indices = new HTuple(), hv_TestTuple = new HTuple();
            HTuple hv_Exception = null;
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_o_DefectRegion);
            HOperatorSet.GenEmptyObj(out ho_Cross);
            HOperatorSet.GenEmptyObj(out ho_Rectangle);
            hv_o_ErrString = new HTuple();
            hv_o_EpoxyResult = new HTuple();
            try
            {
                hv_o_ErrCode = 0;
                hv_o_ErrStr = "";
                try
                {
                    ho_o_DefectRegion.Dispose();
                    HOperatorSet.GenEmptyObj(out ho_o_DefectRegion);
                    hv_EpoxyRow = new HTuple();
                    hv_EpoxyCol = new HTuple();
                    HOperatorSet.GetImageSize(ho_i_Image, out hv_Width, out hv_Height);
                    HOperatorSet.SmallestRectangle2(ho_i_InspectReg, out hv_Row, out hv_Column,
                        out hv_Phi, out hv_Length1, out hv_Length2);
                    HOperatorSet.GenMeasureRectangle2(hv_Row, hv_Column, hv_Phi, hv_Length1 + hv_i_EpoxyInspectSize,
                        hv_Length2, hv_Width, hv_Height, "nearest_neighbor", out hv_MeasureHandle);
                    HOperatorSet.MeasurePos(ho_i_Image, hv_MeasureHandle, hv_i_EpoxyEdgeSigma,
                        hv_i_EpoxyEdgeThresh, "all", "all", out hv_RowEdge, out hv_ColumnEdge,
                        out hv_Amplitude, out hv_Distance);
                    HOperatorSet.CloseMeasure(hv_MeasureHandle);
                    HOperatorSet.TupleGreaterElem(hv_Amplitude, 0, out hv_Greater);
                    if ((int)(hv_i_EpoxyDarkLight) != 0)
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
                        ho_o_DefectRegion.Dispose();
                        HOperatorSet.DilationRectangle1(ho_i_Image, out ho_o_DefectRegion, hv_i_EpoxyInspectSize * 2,
                            hv_i_EpoxyInspectSize * 2);
                        hv_o_ErrCode = 1;
                        ho_Cross.Dispose();
                        ho_Rectangle.Dispose();

                        return;
                    }
                    hv_EpoxyRow = hv_EpoxyRow.TupleConcat(hv_RowEdge.TupleSelect(hv_FirstIndex.TupleConcat(
                        hv_LastIndex)));
                    hv_EpoxyCol = hv_EpoxyCol.TupleConcat(hv_ColumnEdge.TupleSelect(hv_FirstIndex.TupleConcat(
                        hv_LastIndex)));
                    ho_Cross.Dispose();
                    HOperatorSet.GenCrossContourXld(out ho_Cross, hv_RowEdge.TupleSelect(hv_FirstIndex.TupleConcat(
                        hv_LastIndex)), hv_ColumnEdge.TupleSelect(hv_FirstIndex.TupleConcat(hv_LastIndex)),
                        6, 0);
                    ho_Rectangle.Dispose();
                    HOperatorSet.GenRectangle2(out ho_Rectangle, hv_Row, hv_Column, hv_Phi + ((new HTuple(90)).TupleRad()
                        ), hv_Length2 + hv_i_EpoxyInspectSize, hv_Length1);
                    HOperatorSet.GenMeasureRectangle2(hv_Row, hv_Column, hv_Phi + 1.5708, hv_Length2 + hv_i_EpoxyInspectSize,
                        hv_Length1, hv_Width, hv_Height, "nearest_neighbor", out hv_MeasureHandle);
                    HOperatorSet.MeasurePos(ho_i_Image, hv_MeasureHandle, hv_i_EpoxyEdgeSigma,
                        hv_i_EpoxyEdgeThresh, "all", "all", out hv_RowEdge, out hv_ColumnEdge,
                        out hv_Amplitude, out hv_Distance);
                    HOperatorSet.CloseMeasure(hv_MeasureHandle);
                    HOperatorSet.TupleGreaterElem(hv_Amplitude, 0, out hv_Greater);
                    if ((int)(hv_i_EpoxyDarkLight) != 0)
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
                        ho_o_DefectRegion.Dispose();
                        HOperatorSet.DilationRectangle1(ho_i_Image, out ho_o_DefectRegion, hv_i_EpoxyInspectSize * 2,
                            hv_i_EpoxyInspectSize * 2);
                        hv_o_ErrCode = 1;
                        ho_Cross.Dispose();
                        ho_Rectangle.Dispose();

                        return;
                    }
                    hv_EpoxyRow = hv_EpoxyRow.TupleConcat(hv_RowEdge.TupleSelect(hv_FirstIndex.TupleConcat(
                        hv_LastIndex)));
                    hv_EpoxyCol = hv_EpoxyCol.TupleConcat(hv_ColumnEdge.TupleSelect(hv_FirstIndex.TupleConcat(
                        hv_LastIndex)));
                    HOperatorSet.TupleGenConst(4, hv_Row, out hv_RowTuple);
                    HOperatorSet.TupleGenConst(4, hv_Column, out hv_ColTuple);
                    HOperatorSet.DistancePp(hv_EpoxyRow, hv_EpoxyCol, hv_RowTuple, hv_ColTuple,
                        out hv_Distance1);
                    hv_Length = new HTuple();
                    hv_Length = hv_Length.TupleConcat(hv_Length1);
                    hv_Length = hv_Length.TupleConcat(hv_Length1);
                    hv_Length = hv_Length.TupleConcat(hv_Length2);
                    hv_Length = hv_Length.TupleConcat(hv_Length2);
                    hv_EpoxyDist = hv_Distance1 - hv_Length;
                    HOperatorSet.TupleGreaterElem(hv_EpoxyDist, hv_i_EpoxyDistThresh, out hv_GreaterEpoxy);
                    HOperatorSet.TupleFind(hv_GreaterEpoxy, 1, out hv_Indices);
                    //gen_cross_contour_xld (Cross1, EpoxyRow, EpoxyCol, 6, Phi)
                    if ((int)(new HTuple(hv_Indices.TupleGreaterEqual(0))) != 0)
                    {
                        ho_o_DefectRegion.Dispose();
                        HOperatorSet.DilationRectangle1(ho_i_InspectReg, out ho_o_DefectRegion,
                            (hv_EpoxyDist.TupleMax()) * 2, (hv_EpoxyDist.TupleMax()) * 2);
                        hv_o_ErrCode = 1;
                        ho_Cross.Dispose();
                        ho_Rectangle.Dispose();

                        return;
                    }

                    HOperatorSet.TupleGenConst(4, 5, out hv_TestTuple);
                    HOperatorSet.TupleGreaterElem(hv_EpoxyDist, hv_TestTuple, out hv_o_EpoxyResult);

                    ho_Cross.Dispose();
                    ho_Rectangle.Dispose();

                    return;
                }
                // catch (Exception) 
                catch (HalconException HDevExpDefaultException1)
                {
                    HDevExpDefaultException1.ToHTuple(out hv_Exception);
                    hv_o_ErrCode = -1;
                    hv_o_ErrString = hv_Exception.Clone();
                    ho_Cross.Dispose();
                    ho_Rectangle.Dispose();

                    return;
                }

            }
            catch (HalconException HDevExpDefaultException)
            {
                ho_Cross.Dispose();
                ho_Rectangle.Dispose();

                throw HDevExpDefaultException;
            }
        }
        public static void HTV_track_wire(HObjectVector/*{eObjectVector,Dim=1}*/ hvec_i_Image,
     out HObject ho_o_TrackReg, out HObject ho_o_Wires, HTuple hv_i_RowBall1, HTuple hv_i_ColBall1,
     HTuple hv_i_RowBall2, HTuple hv_i_ColBall2, HTuple hv_i_SearchLen, HTuple hv_i_ClipLen,
     HTuple hv_i_LineWidth, HTuple hv_i_LineContrast, HTuple hv_i_MinSegLen, HTuple hv_i_AngleExtent,
     HTuple hv_i_MaxGap, out HTuple hv_o_ErrCode, out HTuple hv_o_ErrStr, out HTuple hv_line_gap)
        {




            // Stack for temporary objects 
            HObject[] OTemp = new HObject[20];

            // Local iconic variables 

            HObject ho_ImageReduced, ho_Lines, ho_ContoursSplit;
            HObject ho_ImageReduced1, ho_Lines1, ho_ContoursSplit1;
            HObject ho_SelectedContoursLength, ho_SelectedContours;
            HObject ho_ContoursSort, ho_ObjectSelected, ho_ObjectSelected1 = null;
            HObject ho_ObjectSelected2 = null, ho_ContourLP, ho_ContourIC;
            HObject ho_ObjectConcat, ho_ObjectsConcat, ho_UnionContour;

            // Local control variables 

            HTuple hv_IMAX = null, hv_Distance = null;
            HTuple hv_Phi = null, hv_Sigma = null, hv_Low = null, hv_High = null;
            HTuple hv_Sigma1 = null, hv_Low1 = null, hv_High1 = null;
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
            HTuple hv_ind = null, hv_Index = new HTuple(), hv_DistanceMax = new HTuple();
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_o_TrackReg);
            HOperatorSet.GenEmptyObj(out ho_o_Wires);
            HOperatorSet.GenEmptyObj(out ho_ImageReduced);
            HOperatorSet.GenEmptyObj(out ho_Lines);
            HOperatorSet.GenEmptyObj(out ho_ContoursSplit);
            HOperatorSet.GenEmptyObj(out ho_ImageReduced1);
            HOperatorSet.GenEmptyObj(out ho_Lines1);
            HOperatorSet.GenEmptyObj(out ho_ContoursSplit1);
            HOperatorSet.GenEmptyObj(out ho_SelectedContoursLength);
            HOperatorSet.GenEmptyObj(out ho_SelectedContours);
            HOperatorSet.GenEmptyObj(out ho_ContoursSort);
            HOperatorSet.GenEmptyObj(out ho_ObjectSelected);
            HOperatorSet.GenEmptyObj(out ho_ObjectSelected1);
            HOperatorSet.GenEmptyObj(out ho_ObjectSelected2);
            HOperatorSet.GenEmptyObj(out ho_ContourLP);
            HOperatorSet.GenEmptyObj(out ho_ContourIC);
            HOperatorSet.GenEmptyObj(out ho_ObjectConcat);
            HOperatorSet.GenEmptyObj(out ho_ObjectsConcat);
            HOperatorSet.GenEmptyObj(out ho_UnionContour);
            hv_o_ErrStr = new HTuple();
            hv_line_gap = new HTuple();
            try
            {
                hv_o_ErrCode = 0;
                ho_o_Wires.Dispose();
                HOperatorSet.GenEmptyObj(out ho_o_Wires);
                ho_o_TrackReg.Dispose();
                HOperatorSet.GenEmptyObj(out ho_o_TrackReg);
                hv_IMAX = 9999;
                //gen bond wire detect region
                HOperatorSet.DistancePp(hv_i_RowBall1, hv_i_ColBall1, hv_i_RowBall2, hv_i_ColBall2,
                    out hv_Distance);
                HOperatorSet.LineOrientation(hv_i_RowBall1, hv_i_ColBall1, hv_i_RowBall2, hv_i_ColBall2,
                    out hv_Phi);
                ho_o_TrackReg.Dispose();
                HOperatorSet.GenRectangle2(out ho_o_TrackReg, ((hv_i_RowBall1.TupleConcat(hv_i_RowBall2))).TupleMean()
                    , ((hv_i_ColBall1.TupleConcat(hv_i_ColBall2))).TupleMean(), hv_Phi, (hv_Distance / 2.0) - hv_i_ClipLen,
                    hv_i_SearchLen);
                //ImageWire
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    ho_ImageReduced.Dispose();
                    HOperatorSet.ReduceDomain(hvec_i_Image[1].O, ho_o_TrackReg, out ho_ImageReduced
                        );
                }
                calculate_lines_gauss_parameters(hv_i_LineWidth, hv_i_LineContrast, out hv_Sigma,
                    out hv_Low, out hv_High);
                ho_Lines.Dispose();
                HOperatorSet.LinesGauss(ho_ImageReduced, out ho_Lines, hv_Sigma, hv_Low, hv_High,
                    "light", "false", "none", "false");
                ho_ContoursSplit.Dispose();
                HOperatorSet.SegmentContoursXld(ho_Lines, out ho_ContoursSplit, "lines", 0,
                    2, 1);
                //ImageIC
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    ho_ImageReduced1.Dispose();
                    HOperatorSet.ReduceDomain(hvec_i_Image[0].O, ho_o_TrackReg, out ho_ImageReduced1
                        );
                }
                calculate_lines_gauss_parameters(10, 15, out hv_Sigma1, out hv_Low1, out hv_High1);
                ho_Lines1.Dispose();
                HOperatorSet.LinesGauss(ho_ImageReduced1, out ho_Lines1, hv_Sigma1, hv_Low1,
                    hv_High1, "light", "false", "none", "false");
                ho_ContoursSplit1.Dispose();
                HOperatorSet.SegmentContoursXld(ho_Lines1, out ho_ContoursSplit1, "lines",
                    0, 2, 1);
                //threshold (ImageReduced1, Region, 0, 15)
                //get_image_size (i_Image.at(0), Width, Height)
                //region_to_bin (Region, BinImage, 2, 1, Width, Height)
                //mult_image (ImageReduced, BinImage, ImageResult, 1, 0)
                //calculate_lines_gauss_parameters (i_LineWidth, i_LineContrast, Sigma, Low, High)
                //lines_gauss (ImageResult, Lines, Sigma, Low, High, 'light', 'false', 'none', 'false')
                //segment_contours_xld (Lines, ContoursSplit, 'lines', 0, 2, 1)
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.ConcatObj(ho_ContoursSplit, ho_ContoursSplit1, out ExpTmpOutVar_0
                        );
                    ho_ContoursSplit.Dispose();
                    ho_ContoursSplit = ExpTmpOutVar_0;
                }
                ho_SelectedContoursLength.Dispose();
                HOperatorSet.SelectContoursXld(ho_ContoursSplit, out ho_SelectedContoursLength,
                    "contour_length", hv_i_MinSegLen, 9999, -0.5, 0.5);
                ho_SelectedContours.Dispose();
                HOperatorSet.SelectContoursXld(ho_SelectedContoursLength, out ho_SelectedContours,
                    "direction", hv_Phi - hv_i_AngleExtent, hv_Phi + hv_i_AngleExtent, -0.5, 0.5);
                HOperatorSet.CountObj(ho_SelectedContours, out hv_Number);
                if ((int)(new HTuple(hv_Number.TupleLess(1))) != 0)
                {
                    hv_o_ErrCode = -1;
                    ho_ImageReduced.Dispose();
                    ho_Lines.Dispose();
                    ho_ContoursSplit.Dispose();
                    ho_ImageReduced1.Dispose();
                    ho_Lines1.Dispose();
                    ho_ContoursSplit1.Dispose();
                    ho_SelectedContoursLength.Dispose();
                    ho_SelectedContours.Dispose();
                    ho_ContoursSort.Dispose();
                    ho_ObjectSelected.Dispose();
                    ho_ObjectSelected1.Dispose();
                    ho_ObjectSelected2.Dispose();
                    ho_ContourLP.Dispose();
                    ho_ContourIC.Dispose();
                    ho_ObjectConcat.Dispose();
                    ho_ObjectsConcat.Dispose();
                    ho_UnionContour.Dispose();

                    return;
                }
                //track bond wire
                fit_line_contour_sort(ho_SelectedContours, hv_i_RowBall1, hv_i_ColBall1, out hv_RowBegin_,
                    out hv_ColBegin_, out hv_RowEnd_, out hv_ColEnd_, out hv_Indices);
                ho_ContoursSort.Dispose();
                HOperatorSet.SelectObj(ho_SelectedContours, out ho_ContoursSort, hv_Indices + 1);
                HOperatorSet.LengthXld(ho_ContoursSort, out hv_Length);
                hv_SegLength = new HTuple();
                hv_SegLength[0] = 0;
                hv_SegLength = hv_SegLength.TupleConcat(hv_Length);
                hv_SegLength = hv_SegLength.TupleConcat(0);
                hv_RowBegin = new HTuple();
                hv_RowBegin = hv_RowBegin.TupleConcat(hv_i_RowBall1);
                hv_RowBegin = hv_RowBegin.TupleConcat(hv_RowBegin_);
                hv_RowBegin = hv_RowBegin.TupleConcat(hv_i_RowBall2);
                hv_ColBegin = new HTuple();
                hv_ColBegin = hv_ColBegin.TupleConcat(hv_i_ColBall1);
                hv_ColBegin = hv_ColBegin.TupleConcat(hv_ColBegin_);
                hv_ColBegin = hv_ColBegin.TupleConcat(hv_i_ColBall2);
                hv_RowEnd = new HTuple();
                hv_RowEnd = hv_RowEnd.TupleConcat(hv_i_RowBall1);
                hv_RowEnd = hv_RowEnd.TupleConcat(hv_RowEnd_);
                hv_RowEnd = hv_RowEnd.TupleConcat(hv_i_RowBall2);
                hv_ColEnd = new HTuple();
                hv_ColEnd = hv_ColEnd.TupleConcat(hv_i_ColBall1);
                hv_ColEnd = hv_ColEnd.TupleConcat(hv_ColEnd_);
                hv_ColEnd = hv_ColEnd.TupleConcat(hv_i_ColBall2);
                hv_SegNum = new HTuple(hv_RowBegin.TupleLength());
                HOperatorSet.TupleGenConst(hv_SegNum * hv_SegNum, hv_IMAX, out hv_DistanceMatrix);
                HTuple end_val44 = hv_SegNum - 1;
                HTuple step_val44 = 1;
                for (hv_i = 0; hv_i.Continue(end_val44, step_val44); hv_i = hv_i.TupleAdd(step_val44))
                {
                    hv_Row0 = hv_RowBegin.TupleSelect(hv_i);
                    hv_Col0 = hv_ColBegin.TupleSelect(hv_i);
                    hv_Row1 = hv_RowEnd.TupleSelect(hv_i);
                    hv_Col1 = hv_ColEnd.TupleSelect(hv_i);
                    if (hv_DistanceMatrix == null)
                        hv_DistanceMatrix = new HTuple();
                    hv_DistanceMatrix[(hv_i * hv_SegNum) + hv_i] = 0;
                    HTuple end_val50 = hv_SegNum - 1;
                    HTuple step_val50 = 1;
                    for (hv_j = hv_i + 1; hv_j.Continue(end_val50, step_val50); hv_j = hv_j.TupleAdd(step_val50))
                    {
                        HOperatorSet.DistancePp(hv_Row1, hv_Col1, hv_RowBegin.TupleSelect(hv_j),
                            hv_ColBegin.TupleSelect(hv_j), out hv_DistanceMin);
                        HOperatorSet.AngleLl(hv_Row1, hv_Col1, hv_RowBegin.TupleSelect(hv_j), hv_ColBegin.TupleSelect(
                            hv_j), hv_i_RowBall1, hv_i_ColBall1, hv_i_RowBall2, hv_i_ColBall2,
                            out hv_Angle);
                        if ((int)((new HTuple(hv_DistanceMin.TupleLess(hv_i_MaxGap))).TupleAnd(
                            (new HTuple(((hv_Angle.TupleAbs())).TupleLess(1.0))).TupleOr(new HTuple(hv_DistanceMin.TupleLess(
                            hv_i_LineWidth))))) != 0)
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
                HTuple end_val64 = hv_SegNum - 1;
                HTuple step_val64 = 1;
                for (hv_i = 1; hv_i.Continue(end_val64, step_val64); hv_i = hv_i.TupleAdd(step_val64))
                {
                    hv_DistMin = hv_IMAX.Clone();
                    hv_Prev = 0;
                    HTuple end_val67 = hv_SegNum - 1;
                    HTuple step_val67 = 1;
                    for (hv_j = 0; hv_j.Continue(end_val67, step_val67); hv_j = hv_j.TupleAdd(step_val67))
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
                    HTuple end_val74 = hv_SegNum - 1;
                    HTuple step_val74 = 1;
                    for (hv_j = 0; hv_j.Continue(end_val74, step_val74); hv_j = hv_j.TupleAdd(step_val74))
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
                    hv_o_ErrCode = -1;
                    ho_ImageReduced.Dispose();
                    ho_Lines.Dispose();
                    ho_ContoursSplit.Dispose();
                    ho_ImageReduced1.Dispose();
                    ho_Lines1.Dispose();
                    ho_ContoursSplit1.Dispose();
                    ho_SelectedContoursLength.Dispose();
                    ho_SelectedContours.Dispose();
                    ho_ContoursSort.Dispose();
                    ho_ObjectSelected.Dispose();
                    ho_ObjectSelected1.Dispose();
                    ho_ObjectSelected2.Dispose();
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

                hv_line_gap = new HTuple();
                if ((int)(new HTuple((new HTuple(hv_Road.TupleLength())).TupleGreater(1))) != 0)
                {
                    for (hv_Index = 1; (int)hv_Index <= (int)((new HTuple(hv_Road.TupleLength())) - 1); hv_Index = (int)hv_Index + 1)
                    {
                        ho_ObjectSelected1.Dispose();
                        HOperatorSet.SelectObj(ho_ObjectSelected, out ho_ObjectSelected1, hv_Index);
                        ho_ObjectSelected2.Dispose();
                        HOperatorSet.SelectObj(ho_ObjectSelected, out ho_ObjectSelected2, hv_Index + 1);
                        HOperatorSet.DistanceCc(ho_ObjectSelected1, ho_ObjectSelected2, "point_to_point",
                            out hv_DistanceMin, out hv_DistanceMax);
                        hv_line_gap = hv_line_gap.TupleConcat(hv_DistanceMin);
                    }
                }
                else if ((int)(new HTuple((new HTuple(hv_Road.TupleLength())).TupleEqual(
                    1))) != 0)
                {
                    hv_line_gap = hv_line_gap.TupleConcat(hv_DistanceMin);
                }
                else
                {
                    hv_line_gap = hv_line_gap.TupleConcat(0);
                }

                ho_ContourLP.Dispose();
                HOperatorSet.GenContourPolygonXld(out ho_ContourLP, hv_i_RowBall1.TupleConcat(
                    hv_RowBegin.TupleSelect(hv_Road.TupleSelect((new HTuple(hv_Road.TupleLength()
                    )) - 1))), hv_i_ColBall1.TupleConcat(hv_ColBegin.TupleSelect(hv_Road.TupleSelect(
                    (new HTuple(hv_Road.TupleLength())) - 1))));
                ho_ContourIC.Dispose();
                HOperatorSet.GenContourPolygonXld(out ho_ContourIC, hv_i_RowBall2.TupleConcat(
                    hv_RowEnd.TupleSelect(hv_Road.TupleSelect(0))), hv_i_ColBall2.TupleConcat(
                    hv_ColEnd.TupleSelect(hv_Road.TupleSelect(0))));
                ho_ObjectConcat.Dispose();
                HOperatorSet.ConcatObj(ho_ObjectSelected, ho_ContourLP, out ho_ObjectConcat
                    );
                ho_ObjectsConcat.Dispose();
                HOperatorSet.ConcatObj(ho_ObjectConcat, ho_ContourIC, out ho_ObjectsConcat);
                ho_UnionContour.Dispose();
                HOperatorSet.UnionAdjacentContoursXld(ho_ObjectsConcat, out ho_UnionContour,
                    hv_i_MaxGap, 1, "attr_keep");
                ho_o_Wires.Dispose();
                HOperatorSet.SmoothContoursXld(ho_UnionContour, out ho_o_Wires, 21);
                ho_ImageReduced.Dispose();
                ho_Lines.Dispose();
                ho_ContoursSplit.Dispose();
                ho_ImageReduced1.Dispose();
                ho_Lines1.Dispose();
                ho_ContoursSplit1.Dispose();
                ho_SelectedContoursLength.Dispose();
                ho_SelectedContours.Dispose();
                ho_ContoursSort.Dispose();
                ho_ObjectSelected.Dispose();
                ho_ObjectSelected1.Dispose();
                ho_ObjectSelected2.Dispose();
                ho_ContourLP.Dispose();
                ho_ContourIC.Dispose();
                ho_ObjectConcat.Dispose();
                ho_ObjectsConcat.Dispose();
                ho_UnionContour.Dispose();

                return;
            }
            catch (HalconException HDevExpDefaultException)
            {
                ho_ImageReduced.Dispose();
                ho_Lines.Dispose();
                ho_ContoursSplit.Dispose();
                ho_ImageReduced1.Dispose();
                ho_Lines1.Dispose();
                ho_ContoursSplit1.Dispose();
                ho_SelectedContoursLength.Dispose();
                ho_SelectedContours.Dispose();
                ho_ContoursSort.Dispose();
                ho_ObjectSelected.Dispose();
                ho_ObjectSelected1.Dispose();
                ho_ObjectSelected2.Dispose();
                ho_ContourLP.Dispose();
                ho_ContourIC.Dispose();
                ho_ObjectConcat.Dispose();
                ho_ObjectsConcat.Dispose();
                ho_UnionContour.Dispose();

                throw HDevExpDefaultException;
            }
        }
        public static void HTV_inspect_frame_model(HObjectVector/*{eObjectVector,Dim=1}*/ hvec_i_Images,
            HObject ho_i_MatchReg, HObject ho_i_InspectReg, HObject ho_i_RejectReg, HObject ho_i_SubRegs,
            HObject ho_i_ImgDark, HObject ho_i_ImgLight, HObject ho_i_CoarseReg, out HObject ho_o_FailureReg,
            HTuple hv_i_ModelID, HTuple hv_i_ModelType, HTuple hv_i_ImgIdx, HTuple hv_i_FindIcImgIdx,
            HTuple hv_i_MatchDilationSize, HTuple hv_i_AngleStart, HTuple hv_i_AngleExt,
            HTuple hv_i_MinScore, HTuple hv_i_FailRegCloseSizes, HTuple hv_i_FailRegMinLenths,
            HTuple hv_i_FailRegMinWidths, HTuple hv_i_FailRegMinAreas, HTuple hv_i_FailRegSelectOperation,
            out HTuple hv_o_HomModel2Img, out HTuple hv_o_Row, out HTuple hv_o_Column, out HTuple hv_o_Angle,
            out HTuple hv_o_ErrCode, out HTuple hv_o_ErrString)
        {




            // Stack for temporary objects 
            HObject[] OTemp = new HObject[20];

            // Local iconic variables 

            HObject ho_o_ChipReg, ho_RegionDilation = null;
            HObject ho_ImageAffinTrans = null, ho__inspectImage = null;
            HObject ho__darkRegion = null, ho__lightRegion = null, ho_RegionUnion = null;
            HObject ho_RegionDifference = null, ho_RegionClosing = null;
            HObject ho_ConnectedRegions1 = null, ho__AllRegs = null, ho_EmptyRegion = null;
            HObject ho_ObjectSelected = null, ho_RegionReduced = null, ho_ObjectSelected1 = null;
            HObject ho_RegionIntersection = null, ho_ConnectedRegions = null;
            HObject ho__SelectRegs = null, ho_SelectedRegions = null;

            // Local control variables 

            hv_o_ErrCode = null;
            HTuple hv__num = new HTuple();
            HTuple hv__score = new HTuple();
            HTuple hv__homImg2Model = new HTuple(), hv__Rect2Len1 = new HTuple();
            HTuple hv__Rect2Len2 = new HTuple(), hv__SubRegNum = new HTuple();
            HTuple hv_Number = new HTuple(), hv_i = new HTuple(), hv_Index1 = new HTuple();
            HTuple hv_Area = new HTuple(), hv_Row = new HTuple(), hv_Column = new HTuple();
            HTuple hv_Exception = null;
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_o_FailureReg);
            HOperatorSet.GenEmptyObj(out ho_o_ChipReg);
            HOperatorSet.GenEmptyObj(out ho_RegionDilation);
            HOperatorSet.GenEmptyObj(out ho_ImageAffinTrans);
            HOperatorSet.GenEmptyObj(out ho__inspectImage);
            HOperatorSet.GenEmptyObj(out ho__darkRegion);
            HOperatorSet.GenEmptyObj(out ho__lightRegion);
            HOperatorSet.GenEmptyObj(out ho_RegionUnion);
            HOperatorSet.GenEmptyObj(out ho_RegionDifference);
            HOperatorSet.GenEmptyObj(out ho_RegionClosing);
            HOperatorSet.GenEmptyObj(out ho_ConnectedRegions1);
            HOperatorSet.GenEmptyObj(out ho__AllRegs);
            HOperatorSet.GenEmptyObj(out ho_EmptyRegion);
            HOperatorSet.GenEmptyObj(out ho_ObjectSelected);
            HOperatorSet.GenEmptyObj(out ho_RegionReduced);
            HOperatorSet.GenEmptyObj(out ho_ObjectSelected1);
            HOperatorSet.GenEmptyObj(out ho_RegionIntersection);
            HOperatorSet.GenEmptyObj(out ho_ConnectedRegions);
            HOperatorSet.GenEmptyObj(out ho__SelectRegs);
            HOperatorSet.GenEmptyObj(out ho_SelectedRegions);
            hv_o_HomModel2Img = new HTuple();
            hv_o_Row = new HTuple();
            hv_o_Column = new HTuple();
            hv_o_Angle = new HTuple();
            hv_o_ErrCode = new HTuple();
            try
            {
                hv_o_ErrCode = 0;
                hv_o_ErrString = "";
                ho_o_ChipReg.Dispose();
                HOperatorSet.GenEmptyObj(out ho_o_ChipReg);
                ho_o_FailureReg.Dispose();
                HOperatorSet.GenEmptyObj(out ho_o_FailureReg);
                try
                {
                    //1. match object
                    ho_RegionDilation.Dispose();
                    HTV_match_region_dilation(ho_i_MatchReg, out ho_RegionDilation, hv_i_MatchDilationSize);
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        HTV_find_model(hvec_i_Images[1].O, ho_RegionDilation, hv_i_ModelID, hv_i_ModelType,
                            hv_i_AngleStart, hv_i_AngleExt, hv_i_MinScore, 1, out hv_o_Row, out hv_o_Column,
                            out hv_o_Angle, out hv__num, out hv__score);
                    }

                    if ((int)(new HTuple(hv__num.TupleEqual(0))) != 0)
                    {
                        hv_o_ErrCode = 1;
                        hv_o_ErrString = "Find no object";
                        ho_o_FailureReg.Dispose();
                        HOperatorSet.CopyObj(ho_RegionDilation, out ho_o_FailureReg, 1, 1);
                        ho_o_ChipReg.Dispose();
                        ho_RegionDilation.Dispose();
                        ho_ImageAffinTrans.Dispose();
                        ho__inspectImage.Dispose();
                        ho__darkRegion.Dispose();
                        ho__lightRegion.Dispose();
                        ho_RegionUnion.Dispose();
                        ho_RegionDifference.Dispose();
                        ho_RegionClosing.Dispose();
                        ho_ConnectedRegions1.Dispose();
                        ho__AllRegs.Dispose();
                        ho_EmptyRegion.Dispose();
                        ho_ObjectSelected.Dispose();
                        ho_RegionReduced.Dispose();
                        ho_ObjectSelected1.Dispose();
                        ho_RegionIntersection.Dispose();
                        ho_ConnectedRegions.Dispose();
                        ho__SelectRegs.Dispose();
                        ho_SelectedRegions.Dispose();

                        return;
                    }


                    //2. align image(affine trans image to model)
                    HOperatorSet.VectorAngleToRigid(hv_o_Row, hv_o_Column, hv_o_Angle, 0, 0,
                        0, out hv__homImg2Model);
                    HOperatorSet.HomMat2dInvert(hv__homImg2Model, out hv_o_HomModel2Img);
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        ho_ImageAffinTrans.Dispose();
                        HOperatorSet.AffineTransImage(hvec_i_Images[hv_i_ImgIdx].O, out ho_ImageAffinTrans,
                            hv__homImg2Model, "nearest_neighbor", "false");
                    }
                    ho__inspectImage.Dispose();
                    HOperatorSet.ReduceDomain(ho_ImageAffinTrans, ho_i_InspectReg, out ho__inspectImage
                        );
                    //3. inspection(compare with threshold image)
                    ho__darkRegion.Dispose();
                    HOperatorSet.DynThreshold(ho__inspectImage, ho_i_ImgDark, out ho__darkRegion,
                        1, "dark");
                    ho__lightRegion.Dispose();
                    HOperatorSet.DynThreshold(ho__inspectImage, ho_i_ImgLight, out ho__lightRegion,
                        1, "light");
                    ho_RegionUnion.Dispose();
                    HOperatorSet.Union2(ho__darkRegion, ho__lightRegion, out ho_RegionUnion);
                    ho_RegionDifference.Dispose();
                    HOperatorSet.Difference(ho_RegionUnion, ho_i_RejectReg, out ho_RegionDifference
                        );
                    hv__Rect2Len1 = hv_i_FailRegMinLenths / 2.0;
                    hv__Rect2Len2 = hv_i_FailRegMinWidths / 2.0;
                    ho_RegionClosing.Dispose();
                    HOperatorSet.ClosingCircle(ho_RegionDifference, out ho_RegionClosing, hv_i_FailRegCloseSizes.TupleMin()
                        );
                    ho_ConnectedRegions1.Dispose();
                    HOperatorSet.Connection(ho_RegionClosing, out ho_ConnectedRegions1);

                    HOperatorSet.CountObj(ho_i_SubRegs, out hv__SubRegNum);
                    if ((int)(new HTuple(hv__SubRegNum.TupleGreater(0))) != 0)
                    {
                        ho_RegionDifference.Dispose();
                        HOperatorSet.Difference(ho_i_InspectReg, ho_i_SubRegs, out ho_RegionDifference
                            );
                        ho_RegionUnion.Dispose();
                        HOperatorSet.Union1(ho_RegionDifference, out ho_RegionUnion);
                        ho__AllRegs.Dispose();
                        HOperatorSet.ConcatObj(ho_RegionUnion, ho_i_SubRegs, out ho__AllRegs);
                        HOperatorSet.CountObj(ho_ConnectedRegions1, out hv_Number);
                        ho_EmptyRegion.Dispose();
                        HOperatorSet.GenEmptyRegion(out ho_EmptyRegion);
                        HTuple end_val39 = hv__SubRegNum;
                        HTuple step_val39 = 1;
                        for (hv_i = 0; hv_i.Continue(end_val39, step_val39); hv_i = hv_i.TupleAdd(step_val39))
                        {
                            ho_ObjectSelected.Dispose();
                            HOperatorSet.SelectObj(ho__AllRegs, out ho_ObjectSelected, hv_i + 1);
                            ho_RegionReduced.Dispose();
                            HOperatorSet.GenEmptyObj(out ho_RegionReduced);
                            HTuple end_val42 = hv_Number;
                            HTuple step_val42 = 1;
                            for (hv_Index1 = 1; hv_Index1.Continue(end_val42, step_val42); hv_Index1 = hv_Index1.TupleAdd(step_val42))
                            {
                                ho_ObjectSelected1.Dispose();
                                HOperatorSet.SelectObj(ho_ConnectedRegions1, out ho_ObjectSelected1,
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
                                        HOperatorSet.Union2(ho_RegionReduced, ho_RegionIntersection, out ExpTmpOutVar_0
                                            );
                                        ho_RegionReduced.Dispose();
                                        ho_RegionReduced = ExpTmpOutVar_0;
                                    }
                                }
                            }
                            ho_RegionClosing.Dispose();
                            HOperatorSet.ClosingCircle(ho_RegionReduced, out ho_RegionClosing, hv_i_FailRegCloseSizes.TupleSelect(
                                hv_i));
                            ho_ConnectedRegions.Dispose();
                            HOperatorSet.Connection(ho_RegionClosing, out ho_ConnectedRegions);
                            ho__SelectRegs.Dispose();
                            HOperatorSet.SelectShape(ho_ConnectedRegions, out ho__SelectRegs, ((new HTuple("rect2_len1")).TupleConcat(
                                "rect2_len2")).TupleConcat("area"), hv_i_FailRegSelectOperation,
                                ((((hv__Rect2Len1.TupleSelect(hv_i))).TupleConcat(hv__Rect2Len2.TupleSelect(
                                hv_i)))).TupleConcat(hv_i_FailRegMinAreas.TupleSelect(hv_i)), ((new HTuple(999999)).TupleConcat(
                                999999)).TupleConcat(999999));

                            {
                                HObject ExpTmpOutVar_0;
                                HOperatorSet.ConcatObj(ho_EmptyRegion, ho__SelectRegs, out ExpTmpOutVar_0
                                    );
                                ho_EmptyRegion.Dispose();
                                ho_EmptyRegion = ExpTmpOutVar_0;
                            }

                        }
                        ho_SelectedRegions.Dispose();
                        HOperatorSet.CopyObj(ho_EmptyRegion, out ho_SelectedRegions, 1, -1);
                    }
                    else
                    {
                        ho_SelectedRegions.Dispose();
                        HOperatorSet.SelectShape(ho_ConnectedRegions1, out ho_SelectedRegions,
                            ((new HTuple("rect2_len1")).TupleConcat("rect2_len2")).TupleConcat(
                            "area"), hv_i_FailRegSelectOperation, ((((hv__Rect2Len1.TupleMin())).TupleConcat(
                            hv__Rect2Len2.TupleMin()))).TupleConcat(hv_i_FailRegMinAreas.TupleMin()
                            ), ((new HTuple(999999)).TupleConcat(999999)).TupleConcat(999999));
                    }
                    ho_o_FailureReg.Dispose();
                    HOperatorSet.AffineTransRegion(ho_SelectedRegions, out ho_o_FailureReg, hv_o_HomModel2Img,
                        "nearest_neighbor");
                }
                // catch (Exception) 
                catch (HalconException HDevExpDefaultException1)
                {
                    HDevExpDefaultException1.ToHTuple(out hv_Exception);
                    hv_o_ErrCode = -1;
                    hv_o_ErrString = hv_Exception.Clone();
                    ho_o_ChipReg.Dispose();
                    ho_RegionDilation.Dispose();
                    ho_ImageAffinTrans.Dispose();
                    ho__inspectImage.Dispose();
                    ho__darkRegion.Dispose();
                    ho__lightRegion.Dispose();
                    ho_RegionUnion.Dispose();
                    ho_RegionDifference.Dispose();
                    ho_RegionClosing.Dispose();
                    ho_ConnectedRegions1.Dispose();
                    ho__AllRegs.Dispose();
                    ho_EmptyRegion.Dispose();
                    ho_ObjectSelected.Dispose();
                    ho_RegionReduced.Dispose();
                    ho_ObjectSelected1.Dispose();
                    ho_RegionIntersection.Dispose();
                    ho_ConnectedRegions.Dispose();
                    ho__SelectRegs.Dispose();
                    ho_SelectedRegions.Dispose();

                    return;
                }
                ho_o_ChipReg.Dispose();
                ho_RegionDilation.Dispose();
                ho_ImageAffinTrans.Dispose();
                ho__inspectImage.Dispose();
                ho__darkRegion.Dispose();
                ho__lightRegion.Dispose();
                ho_RegionUnion.Dispose();
                ho_RegionDifference.Dispose();
                ho_RegionClosing.Dispose();
                ho_ConnectedRegions1.Dispose();
                ho__AllRegs.Dispose();
                ho_EmptyRegion.Dispose();
                ho_ObjectSelected.Dispose();
                ho_RegionReduced.Dispose();
                ho_ObjectSelected1.Dispose();
                ho_RegionIntersection.Dispose();
                ho_ConnectedRegions.Dispose();
                ho__SelectRegs.Dispose();
                ho_SelectedRegions.Dispose();

                return;
            }
            catch (HalconException HDevExpDefaultException)
            {
                ho_o_ChipReg.Dispose();
                ho_RegionDilation.Dispose();
                ho_ImageAffinTrans.Dispose();
                ho__inspectImage.Dispose();
                ho__darkRegion.Dispose();
                ho__lightRegion.Dispose();
                ho_RegionUnion.Dispose();
                ho_RegionDifference.Dispose();
                ho_RegionClosing.Dispose();
                ho_ConnectedRegions1.Dispose();
                ho__AllRegs.Dispose();
                ho_EmptyRegion.Dispose();
                ho_ObjectSelected.Dispose();
                ho_RegionReduced.Dispose();
                ho_ObjectSelected1.Dispose();
                ho_RegionIntersection.Dispose();
                ho_ConnectedRegions.Dispose();
                ho__SelectRegs.Dispose();
                ho_SelectedRegions.Dispose();

                throw HDevExpDefaultException;
            }
        }

        public static void HTV_write_model(HTuple hv_i_ModelID, HTuple hv_i_ModelType, HTuple hv_i_ModelDir)
        {


            // Initialize local and output iconic variables 
            if ((int)(new HTuple(hv_i_ModelType.TupleEqual("ncc"))) != 0)
            {
                HOperatorSet.WriteNccModel(hv_i_ModelID, hv_i_ModelDir + "Model.dat");



            }
            else if ((int)(new HTuple(hv_i_ModelType.TupleEqual("shape"))) != 0)
            {
                HOperatorSet.WriteShapeModel(hv_i_ModelID, hv_i_ModelDir + "Model.dat");
            }
            else
            {
                throw new HalconException("Wrong argument [modelType]=" + hv_i_ModelType);

                return;
            }
            HOperatorSet.WriteTuple(hv_i_ModelType, hv_i_ModelDir + "Model_Type.tup");

            return;
        }

        public static void HTV_find_model(HObject ho_i_Image, HObject ho_i_MatchReg, HTuple hv_i_ModelID,
    HTuple hv_i_ModelType, HTuple hv_i_AngleStart, HTuple hv_i_AngleExt, HTuple hv_i_MinScore,
    HTuple hv_i_NumMatches, out HTuple hv_o_Row, out HTuple hv_o_Col, out HTuple hv_o_Angle,
    out HTuple hv_o_NumMatched, out HTuple hv_o_score)
        {




            // Local iconic variables 

            HObject ho_ImageReduced;

            // Local control variables 

            HTuple hv__row = new HTuple(), hv__col = new HTuple();
            HTuple hv__angle = new HTuple(), hv__score = new HTuple();
            HTuple hv_i = null;
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_ImageReduced);
            hv_o_Angle = new HTuple();
            try
            {
                ho_ImageReduced.Dispose();
                HOperatorSet.ReduceDomain(ho_i_Image, ho_i_MatchReg, out ho_ImageReduced);
                hv_o_score = new HTuple();
                if ((int)(new HTuple(hv_i_ModelType.TupleEqual("ncc"))) != 0)
                {
                    HOperatorSet.FindNccModel(ho_ImageReduced, hv_i_ModelID, hv_i_AngleStart.TupleRad(),
                        hv_i_AngleExt.TupleRad(), 0.2, hv_i_NumMatches, 0.5, "true", 0, out hv__row, out hv__col,
                        out hv__angle, out hv__score);
                }
                else if ((int)(new HTuple(hv_i_ModelType.TupleEqual("shape"))) != 0)
                {
                    HOperatorSet.FindShapeModel(ho_ImageReduced, hv_i_ModelID, hv_i_AngleStart.TupleRad(),
                        hv_i_AngleExt.TupleRad(), 0.2, hv_i_NumMatches, 0.5, "least_squares", 0, 0.9, out hv__row,
                        out hv__col, out hv__angle, out hv__score);
                }
                else
                {
                    throw new HalconException("Wrong argument [modelType]=" + hv_i_ModelType);
                }
                hv_o_score = hv__score.Clone();
                //get_shape_model_contours (ModelContours, i_ModelID, 1)
                hv_o_Row = new HTuple();
                hv_o_Col = new HTuple();
                hv_o_NumMatched = 0;
                if ((int)(new HTuple((new HTuple(hv__score.TupleLength())).TupleEqual(0))) != 0)
                {
                    ho_ImageReduced.Dispose();

                    return;
                }
                for (hv_i = 0; (int)hv_i <= (int)((new HTuple(hv__score.TupleLength())) - 1); hv_i = (int)hv_i + 1)
                {
                    if ((int)(new HTuple(((hv__score.TupleSelect(hv_i))).TupleGreaterEqual(hv_i_MinScore))) != 0)
                    {
                        if (hv_o_Row == null)
                            hv_o_Row = new HTuple();
                        hv_o_Row[hv_o_NumMatched] = hv__row.TupleSelect(hv_i);
                        if (hv_o_Col == null)
                            hv_o_Col = new HTuple();
                        hv_o_Col[hv_o_NumMatched] = hv__col.TupleSelect(hv_i);
                        if (hv_o_Angle == null)
                            hv_o_Angle = new HTuple();
                        hv_o_Angle[hv_o_NumMatched] = hv__angle.TupleSelect(hv_i);
                        hv_o_NumMatched = hv_o_NumMatched + 1;
                    }
                }
                ho_ImageReduced.Dispose();

                return;
                ho_ImageReduced.Dispose();

                return;
            }
            catch (HalconException HDevExpDefaultException)
            {
                ho_ImageReduced.Dispose();

                throw HDevExpDefaultException;
            }
        }

        public static void HTV_read_golden_model(out HObject ho_o_MatchReg, out HObject ho_o_InspectReg,
            out HObject ho_o_RejectReg, out HObject ho_o_SubRegs, out HObject ho_o_ImgMeans,
            out HObject ho_o_ImgStds, out HObject ho_o_ImgDarks, out HObject ho_o_ImgLights,
            HTuple hv_i_ModelDir, HTuple hv_i_DarkScaleFactors, HTuple hv_i_LightScaleFactors,
            HTuple hv_i_SobelScaleFactors, out HTuple hv_o_ModelID, out HTuple hv_o_ModelType,
            out HTuple hv_o_ErrCode, out HTuple hv_o_ErrString)
        {



            // Stack for temporary objects 
            HObject[] OTemp = new HObject[20];

            // Local iconic variables 

            HObject ho_RegionDifference = null, ho_RegionUnion = null;
            HObject ho_ObjectsConcat = null, ho__ImgMean = null, ho__ImgStd = null;
            HObject ho_EdgeAmplitude = null, ho__ImgDark = null, ho__ImgLight = null;
            HObject ho_sub_region_ = null, ho_ImageScaled = null, ho__NewStd = null;
            HObject ho_ImageScaled_D = null, ho_ImageSub_D = null, ho__sub_dark_thresh_image = null;
            HObject ho_ImageScaled_L = null, ho_ImageSub_L = null, ho__sub_light_thresh_image = null;
            HObject ho_dark_image_reduce = null, ho_light_image_reduce = null;

            // Local control variables 

            HTuple hv_Number = new HTuple(), hv_Number1 = new HTuple();
            HTuple hv_Index = new HTuple(), hv_index = new HTuple();
            HTuple hv_Exception = null;
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_o_MatchReg);
            HOperatorSet.GenEmptyObj(out ho_o_InspectReg);
            HOperatorSet.GenEmptyObj(out ho_o_RejectReg);
            HOperatorSet.GenEmptyObj(out ho_o_SubRegs);
            HOperatorSet.GenEmptyObj(out ho_o_ImgMeans);
            HOperatorSet.GenEmptyObj(out ho_o_ImgStds);
            HOperatorSet.GenEmptyObj(out ho_o_ImgDarks);
            HOperatorSet.GenEmptyObj(out ho_o_ImgLights);
            HOperatorSet.GenEmptyObj(out ho_RegionDifference);
            HOperatorSet.GenEmptyObj(out ho_RegionUnion);
            HOperatorSet.GenEmptyObj(out ho_ObjectsConcat);
            HOperatorSet.GenEmptyObj(out ho__ImgMean);
            HOperatorSet.GenEmptyObj(out ho__ImgStd);
            HOperatorSet.GenEmptyObj(out ho_EdgeAmplitude);
            HOperatorSet.GenEmptyObj(out ho__ImgDark);
            HOperatorSet.GenEmptyObj(out ho__ImgLight);
            HOperatorSet.GenEmptyObj(out ho_sub_region_);
            HOperatorSet.GenEmptyObj(out ho_ImageScaled);
            HOperatorSet.GenEmptyObj(out ho__NewStd);
            HOperatorSet.GenEmptyObj(out ho_ImageScaled_D);
            HOperatorSet.GenEmptyObj(out ho_ImageSub_D);
            HOperatorSet.GenEmptyObj(out ho__sub_dark_thresh_image);
            HOperatorSet.GenEmptyObj(out ho_ImageScaled_L);
            HOperatorSet.GenEmptyObj(out ho_ImageSub_L);
            HOperatorSet.GenEmptyObj(out ho__sub_light_thresh_image);
            HOperatorSet.GenEmptyObj(out ho_dark_image_reduce);
            HOperatorSet.GenEmptyObj(out ho_light_image_reduce);
            hv_o_ModelID = new HTuple();
            hv_o_ModelType = new HTuple();
            try
            {
                hv_o_ErrCode = 0;
                hv_o_ErrString = "";
                try
                {
                    ho_o_ImgDarks.Dispose();
                    HOperatorSet.GenEmptyObj(out ho_o_ImgDarks);
                    ho_o_ImgLights.Dispose();
                    HOperatorSet.GenEmptyObj(out ho_o_ImgLights);
                    //1. read match model
                    HTV_read_model(hv_i_ModelDir, out hv_o_ModelID, out hv_o_ModelType);
                    //2 read match region and inspection region
                    ho_o_MatchReg.Dispose();
                    HOperatorSet.ReadRegion(out ho_o_MatchReg, hv_i_ModelDir + "Match_Region.reg");
                    ho_o_InspectReg.Dispose();
                    HOperatorSet.ReadRegion(out ho_o_InspectReg, hv_i_ModelDir + "Inspect_Region.reg");
                    ho_o_RejectReg.Dispose();
                    HOperatorSet.ReadRegion(out ho_o_RejectReg, hv_i_ModelDir + "Reject_Region.reg");
                    ho_o_SubRegs.Dispose();
                    HOperatorSet.ReadRegion(out ho_o_SubRegs, hv_i_ModelDir + "Sub_Regions.reg");

                    //3. read mean and std images
                    ho_o_ImgMeans.Dispose();
                    HOperatorSet.ReadImage(out ho_o_ImgMeans, hv_i_ModelDir + "Mean_Image.tiff");
                    ho_o_ImgStds.Dispose();
                    HOperatorSet.ReadImage(out ho_o_ImgStds, hv_i_ModelDir + "Std_Image.tiff");

                    //4. generate dark and light threshold images
                    ho_RegionDifference.Dispose();
                    HOperatorSet.Difference(ho_o_InspectReg, ho_o_SubRegs, out ho_RegionDifference
                        );
                    ho_RegionUnion.Dispose();
                    HOperatorSet.Union1(ho_RegionDifference, out ho_RegionUnion);
                    ho_ObjectsConcat.Dispose();
                    HOperatorSet.ConcatObj(ho_RegionUnion, ho_o_SubRegs, out ho_ObjectsConcat
                        );

                    HOperatorSet.CountObj(ho_o_ImgMeans, out hv_Number);
                    HOperatorSet.CountObj(ho_ObjectsConcat, out hv_Number1);
                    HTuple end_val24 = hv_Number;
                    HTuple step_val24 = 1;
                    for (hv_Index = 1; hv_Index.Continue(end_val24, step_val24); hv_Index = hv_Index.TupleAdd(step_val24))
                    {
                        ho__ImgMean.Dispose();
                        HOperatorSet.SelectObj(ho_o_ImgMeans, out ho__ImgMean, hv_Index);
                        ho__ImgStd.Dispose();
                        HOperatorSet.SelectObj(ho_o_ImgStds, out ho__ImgStd, hv_Index);
                        ho_EdgeAmplitude.Dispose();
                        HOperatorSet.SobelAmp(ho__ImgMean, out ho_EdgeAmplitude, "sum_abs", 5);
                        ho__ImgDark.Dispose();
                        HTV_gen_image_type(ho__ImgMean, out ho__ImgDark, "byte");
                        ho__ImgLight.Dispose();
                        HTV_gen_image_type(ho__ImgMean, out ho__ImgLight, "byte");
                        HTuple end_val30 = hv_Number1 - 1;
                        HTuple step_val30 = 1;
                        for (hv_index = 0; hv_index.Continue(end_val30, step_val30); hv_index = hv_index.TupleAdd(step_val30))
                        {
                            ho_sub_region_.Dispose();
                            HOperatorSet.SelectObj(ho_ObjectsConcat, out ho_sub_region_, hv_index + 1);
                            ho_ImageScaled.Dispose();
                            HOperatorSet.ScaleImage(ho_EdgeAmplitude, out ho_ImageScaled, hv_i_SobelScaleFactors.TupleSelect(
                                hv_index), 0);
                            ho__NewStd.Dispose();
                            HOperatorSet.AddImage(ho__ImgStd, ho_ImageScaled, out ho__NewStd, 1.0,
                                0);
                            ho_ImageScaled_D.Dispose();
                            HOperatorSet.ScaleImage(ho__NewStd, out ho_ImageScaled_D, hv_i_DarkScaleFactors.TupleSelect(
                                hv_index), 0);
                            ho_ImageSub_D.Dispose();
                            HOperatorSet.SubImage(ho__ImgMean, ho_ImageScaled_D, out ho_ImageSub_D,
                                1.0, 0);
                            ho__sub_dark_thresh_image.Dispose();
                            HOperatorSet.ConvertImageType(ho_ImageSub_D, out ho__sub_dark_thresh_image,
                                "byte");
                            ho_ImageScaled_L.Dispose();
                            HOperatorSet.ScaleImage(ho__NewStd, out ho_ImageScaled_L, hv_i_LightScaleFactors.TupleSelect(
                                hv_index), 0);
                            ho_ImageSub_L.Dispose();
                            HOperatorSet.AddImage(ho__ImgMean, ho_ImageScaled_L, out ho_ImageSub_L,
                                1.0, 0);
                            ho__sub_light_thresh_image.Dispose();
                            HOperatorSet.ConvertImageType(ho_ImageSub_L, out ho__sub_light_thresh_image,
                                "byte");
                            ho_dark_image_reduce.Dispose();
                            HOperatorSet.ReduceDomain(ho__sub_dark_thresh_image, ho_sub_region_,
                                out ho_dark_image_reduce);
                            {
                                HObject ExpTmpOutVar_0;
                                HOperatorSet.PaintGray(ho_dark_image_reduce, ho__ImgDark, out ExpTmpOutVar_0
                                    );
                                ho__ImgDark.Dispose();
                                ho__ImgDark = ExpTmpOutVar_0;
                            }
                            ho_light_image_reduce.Dispose();
                            HOperatorSet.ReduceDomain(ho__sub_light_thresh_image, ho_sub_region_,
                                out ho_light_image_reduce);
                            {
                                HObject ExpTmpOutVar_0;
                                HOperatorSet.PaintGray(ho_light_image_reduce, ho__ImgLight, out ExpTmpOutVar_0
                                    );
                                ho__ImgLight.Dispose();
                                ho__ImgLight = ExpTmpOutVar_0;
                            }
                        }
                        {
                            HObject ExpTmpOutVar_0;
                            HOperatorSet.ConcatObj(ho_o_ImgDarks, ho__ImgDark, out ExpTmpOutVar_0);
                            ho_o_ImgDarks.Dispose();
                            ho_o_ImgDarks = ExpTmpOutVar_0;
                        }
                        {
                            HObject ExpTmpOutVar_0;
                            HOperatorSet.ConcatObj(ho_o_ImgLights, ho__ImgLight, out ExpTmpOutVar_0
                                );
                            ho_o_ImgLights.Dispose();
                            ho_o_ImgLights = ExpTmpOutVar_0;
                        }
                    }
                }
                // catch (Exception) 
                catch (HalconException HDevExpDefaultException1)
                {
                    HDevExpDefaultException1.ToHTuple(out hv_Exception);
                    hv_o_ErrCode = -1;
                    hv_o_ErrString = hv_Exception.Clone();
                }
                ho_RegionDifference.Dispose();
                ho_RegionUnion.Dispose();
                ho_ObjectsConcat.Dispose();
                ho__ImgMean.Dispose();
                ho__ImgStd.Dispose();
                ho_EdgeAmplitude.Dispose();
                ho__ImgDark.Dispose();
                ho__ImgLight.Dispose();
                ho_sub_region_.Dispose();
                ho_ImageScaled.Dispose();
                ho__NewStd.Dispose();
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
                ho_RegionDifference.Dispose();
                ho_RegionUnion.Dispose();
                ho_ObjectsConcat.Dispose();
                ho__ImgMean.Dispose();
                ho__ImgStd.Dispose();
                ho_EdgeAmplitude.Dispose();
                ho__ImgDark.Dispose();
                ho__ImgLight.Dispose();
                ho_sub_region_.Dispose();
                ho_ImageScaled.Dispose();
                ho__NewStd.Dispose();
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

        public static void HTV_write_golden_model(HObject ho_i_ImgMean, HObject ho_i_ImgStd,
            HObject ho_i_MatchReg, HObject ho_i_InspectReg, HObject ho_i_RegjectReg, HObject ho_i_SubRegs,
            HTuple hv_i_ModelID, HTuple hv_i_ModelType, HTuple hv_i_ModelDir, HTuple hv_i_IcModelImgIdx,
            HTuple hv_i_ImgIdx, out HTuple hv_o_ErrCode, out HTuple hv_o_ErrString)
        {




            // Local iconic variables 

            // Local control variables 

            HTuple hv_Exception = null;
            // Initialize local and output iconic variables 
            hv_o_ErrCode = 0;
            hv_o_ErrString = "";
            try
            {
                HOperatorSet.WriteTuple(hv_i_ImgIdx, hv_i_ModelDir + "Image_Index.tup");
                HOperatorSet.WriteTuple(hv_i_IcModelImgIdx, hv_i_ModelDir + "FindIc_ImgIdx.tup");
                HOperatorSet.WriteRegion(ho_i_MatchReg, hv_i_ModelDir + "Match_Region.reg");
                HOperatorSet.WriteRegion(ho_i_InspectReg, hv_i_ModelDir + "Inspect_Region.reg");
                HOperatorSet.WriteRegion(ho_i_RegjectReg, hv_i_ModelDir + "Reject_Region.reg");
                HOperatorSet.WriteRegion(ho_i_SubRegs, hv_i_ModelDir + "Sub_Regions.reg");
                HTV_write_model(hv_i_ModelID, hv_i_ModelType, hv_i_ModelDir);
                HOperatorSet.WriteImage(ho_i_ImgMean, "tiff", 0, hv_i_ModelDir + "Mean_Image.tiff");
                HOperatorSet.WriteImage(ho_i_ImgStd, "tiff", 0, hv_i_ModelDir + "Std_Image.tiff");
            }
            // catch (Exception) 
            catch (HalconException HDevExpDefaultException1)
            {
                HDevExpDefaultException1.ToHTuple(out hv_Exception);
                hv_o_ErrCode = -1;
                hv_o_ErrString = hv_Exception.Clone();

                return;
            }

            return;
        }

        public static void HTV_clear_model(HTuple hv_i_ModelID, HTuple hv_i_ModelType)
        {


            // Initialize local and output iconic variables 
            if ((int)(new HTuple(hv_i_ModelType.TupleEqual("ncc"))) != 0)
            {
                HOperatorSet.ClearNccModel(hv_i_ModelID);
            }
            else if ((int)(new HTuple(hv_i_ModelType.TupleEqual("shape"))) != 0)
            {
                HOperatorSet.ClearShapeModel(hv_i_ModelID);
            }
            else
            {
                throw new HalconException("Wrong argument [modelType]=" + hv_i_ModelType);
            }

            return;
        }

        public static void HTV_make_dirs(HTuple hv_i_Dirs)
        {



            // Local control variables 

            HTuple hv_Index = null, hv_FileExists = new HTuple();
            // Initialize local and output iconic variables 
            for (hv_Index = 0; (int)hv_Index <= (int)((new HTuple(hv_i_Dirs.TupleLength())) - 1); hv_Index = (int)hv_Index + 1)
            {
                HOperatorSet.FileExists(hv_i_Dirs.TupleSelect(hv_Index), out hv_FileExists);
                if ((int)(new HTuple(hv_FileExists.TupleEqual(0))) != 0)
                {
                    HOperatorSet.MakeDir(hv_i_Dirs.TupleSelect(hv_Index));
                }
            }

        }

        public static void HTV_write_coarse_model(HObject ho_i_MatchRegs, HTuple hv_i_ModelDir,
            HTuple hv_i_ModelID, HTuple hv_i_ModelType, out HTuple hv_o_ErrCode, out HTuple hv_o_ErrStr)
        {




            // Local iconic variables 

            // Local control variables 

            HTuple hv_Exception = null;
            // Initialize local and output iconic variables 
            hv_o_ErrCode = 0;
            hv_o_ErrStr = "";
            try
            {
                HOperatorSet.WriteRegion(ho_i_MatchRegs, hv_i_ModelDir + "Match_Region.reg");
                HTV_write_model(hv_i_ModelID, hv_i_ModelType, hv_i_ModelDir);
            }
            // catch (Exception) 
            catch (HalconException HDevExpDefaultException1)
            {
                HDevExpDefaultException1.ToHTuple(out hv_Exception);
                hv_o_ErrCode = -1;
                hv_o_ErrStr = hv_Exception.Clone();
            }

            return;
        }


        public static void HTV_inspect_golden_model(HObjectVector/*{eObjectVector,Dim=1}*/ hvec_i_Images,
      HObject ho_i_MatchReg, HObject ho_i_InspectReg, HObject ho_i_RejectReg, HObject ho_i_SubRegs,
      HObject ho_i_ImgDark, HObject ho_i_ImgLight, HObject ho_i_CoarseReg, out HObject ho_o_FailureReg,
      out HObject ho_o_ChipReg, HTuple hv_i_Row, HTuple hv_i_Column, HTuple hv_i_Angle,
      HTuple hv_i_ImgIdx, HTuple hv_i_MatchDilationSize, HTuple hv_i_AngleStart, HTuple hv_i_AngleExt,
      HTuple hv_i_MinScore, HTuple hv_i_FailRegCloseSizes, HTuple hv_i_FailRegMinLenths,
      HTuple hv_i_FailRegMinWidths, HTuple hv_i_FailRegMinAreas, HTuple hv_i_FailRegSelectOperation,
      out HTuple hv_o_HomModel2Img, out HTuple hv_o_ErrCode, out HTuple hv_o_ErrString)
        {




            // Stack for temporary objects 
            HObject[] OTemp = new HObject[20];

            // Local iconic variables 

            HObject ho_ImageAffinTrans = null, ho__inspectImage = null;
            HObject ho__darkRegion = null, ho__lightRegion = null, ho_RegionUnion = null;
            HObject ho_RegionDifference = null, ho_RegionClosing = null;
            HObject ho_ConnectedRegions1 = null, ho__AllRegs = null, ho_EmptyRegion = null;
            HObject ho_ObjectSelected = null, ho_RegionReduced = null, ho_ObjectSelected1 = null;
            HObject ho_RegionIntersection = null, ho_ConnectedRegions = null;
            HObject ho__SelectRegs = null, ho_SelectedRegions = null;

            // Local control variables 

            HTuple hv__homImg2Model = new HTuple(), hv__Rect2Len1 = new HTuple();
            HTuple hv__Rect2Len2 = new HTuple(), hv__SubRegNum = new HTuple();
            HTuple hv_Number = new HTuple(), hv_i = new HTuple(), hv_Index1 = new HTuple();
            HTuple hv_Area = new HTuple(), hv_Row = new HTuple(), hv_Column = new HTuple();
            HTuple hv_Exception = null;
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_o_FailureReg);
            HOperatorSet.GenEmptyObj(out ho_o_ChipReg);
            HOperatorSet.GenEmptyObj(out ho_ImageAffinTrans);
            HOperatorSet.GenEmptyObj(out ho__inspectImage);
            HOperatorSet.GenEmptyObj(out ho__darkRegion);
            HOperatorSet.GenEmptyObj(out ho__lightRegion);
            HOperatorSet.GenEmptyObj(out ho_RegionUnion);
            HOperatorSet.GenEmptyObj(out ho_RegionDifference);
            HOperatorSet.GenEmptyObj(out ho_RegionClosing);
            HOperatorSet.GenEmptyObj(out ho_ConnectedRegions1);
            HOperatorSet.GenEmptyObj(out ho__AllRegs);
            HOperatorSet.GenEmptyObj(out ho_EmptyRegion);
            HOperatorSet.GenEmptyObj(out ho_ObjectSelected);
            HOperatorSet.GenEmptyObj(out ho_RegionReduced);
            HOperatorSet.GenEmptyObj(out ho_ObjectSelected1);
            HOperatorSet.GenEmptyObj(out ho_RegionIntersection);
            HOperatorSet.GenEmptyObj(out ho_ConnectedRegions);
            HOperatorSet.GenEmptyObj(out ho__SelectRegs);
            HOperatorSet.GenEmptyObj(out ho_SelectedRegions);
            hv_o_HomModel2Img = new HTuple();
            try
            {
                hv_o_ErrCode = 0;
                hv_o_ErrString = "";
                ho_o_ChipReg.Dispose();
                HOperatorSet.GenEmptyObj(out ho_o_ChipReg);
                ho_o_FailureReg.Dispose();
                HOperatorSet.GenEmptyObj(out ho_o_FailureReg);
                try
                {
                    //1. match object
                    //HTV_match_region_dilation (i_MatchReg, RegionDilation, i_MatchDilationSize)
                    //*     HTV_find_model (i_Images.at(i_FindIcImgIdx), RegionDilation, i_ModelID, i_ModelType, i_AngleStart, i_AngleExt, i_MinScore, 1, o_Row, o_Column, o_Angle, _num, o_score)

                    //if (_num=0)
                    //o_ErrCode := 1
                    //o_ErrString := 'Find no object'
                    //copy_obj (RegionDilation, o_FailureReg, 1, 1)
                    //return ()
                    //endif

                    //HTV_inspect_nodie (i_Images, i_InspectReg, i_ImgIdx, o_ErrCode)
                    //if (o_ErrCode=1)
                    //o_ErrString := 'Find no object'
                    //return ()
                    //endif


                    //2. align image(affine trans image to model)
                    HOperatorSet.VectorAngleToRigid(hv_i_Row, hv_i_Column, hv_i_Angle, 0, 0,
                        0, out hv__homImg2Model);
                    HOperatorSet.HomMat2dInvert(hv__homImg2Model, out hv_o_HomModel2Img);
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        ho_ImageAffinTrans.Dispose();
                        HOperatorSet.AffineTransImage(hvec_i_Images[hv_i_ImgIdx].O, out ho_ImageAffinTrans,
                            hv__homImg2Model, "nearest_neighbor", "false");
                    }
                    ho__inspectImage.Dispose();
                    HOperatorSet.ReduceDomain(ho_ImageAffinTrans, ho_i_InspectReg, out ho__inspectImage
                        );
                    //3. inspection(compare with threshold image)
                    ho__darkRegion.Dispose();
                    HOperatorSet.DynThreshold(ho__inspectImage, ho_i_ImgDark, out ho__darkRegion,
                        1, "dark");
                    ho__lightRegion.Dispose();
                    HOperatorSet.DynThreshold(ho__inspectImage, ho_i_ImgLight, out ho__lightRegion,
                        1, "light");
                    ho_RegionUnion.Dispose();
                    HOperatorSet.Union2(ho__darkRegion, ho__lightRegion, out ho_RegionUnion);
                    ho_RegionDifference.Dispose();
                    HOperatorSet.Difference(ho_RegionUnion, ho_i_RejectReg, out ho_RegionDifference
                        );
                    hv__Rect2Len1 = hv_i_FailRegMinLenths / 2.0;
                    hv__Rect2Len2 = hv_i_FailRegMinWidths / 2.0;
                    ho_RegionClosing.Dispose();
                    HOperatorSet.ClosingCircle(ho_RegionDifference, out ho_RegionClosing, hv_i_FailRegCloseSizes.TupleMin()
                        );
                    ho_ConnectedRegions1.Dispose();
                    HOperatorSet.Connection(ho_RegionClosing, out ho_ConnectedRegions1);

                    HOperatorSet.CountObj(ho_i_SubRegs, out hv__SubRegNum);
                    if ((int)(new HTuple(hv__SubRegNum.TupleGreater(0))) != 0)
                    {
                        ho_RegionDifference.Dispose();
                        HOperatorSet.Difference(ho_i_InspectReg, ho_i_SubRegs, out ho_RegionDifference
                            );
                        ho_RegionUnion.Dispose();
                        HOperatorSet.Union1(ho_RegionDifference, out ho_RegionUnion);
                        ho__AllRegs.Dispose();
                        HOperatorSet.ConcatObj(ho_RegionUnion, ho_i_SubRegs, out ho__AllRegs);
                        HOperatorSet.CountObj(ho_ConnectedRegions1, out hv_Number);
                        ho_EmptyRegion.Dispose();
                        HOperatorSet.GenEmptyRegion(out ho_EmptyRegion);
                        HTuple end_val45 = hv__SubRegNum;
                        HTuple step_val45 = 1;
                        for (hv_i = 0; hv_i.Continue(end_val45, step_val45); hv_i = hv_i.TupleAdd(step_val45))
                        {
                            ho_ObjectSelected.Dispose();
                            HOperatorSet.SelectObj(ho__AllRegs, out ho_ObjectSelected, hv_i + 1);
                            ho_RegionReduced.Dispose();
                            HOperatorSet.GenEmptyObj(out ho_RegionReduced);
                            HTuple end_val48 = hv_Number;
                            HTuple step_val48 = 1;
                            for (hv_Index1 = 1; hv_Index1.Continue(end_val48, step_val48); hv_Index1 = hv_Index1.TupleAdd(step_val48))
                            {
                                ho_ObjectSelected1.Dispose();
                                HOperatorSet.SelectObj(ho_ConnectedRegions1, out ho_ObjectSelected1,
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
                                        HOperatorSet.Union2(ho_RegionReduced, ho_RegionIntersection, out ExpTmpOutVar_0
                                            );
                                        ho_RegionReduced.Dispose();
                                        ho_RegionReduced = ExpTmpOutVar_0;
                                    }
                                }
                            }
                            ho_RegionClosing.Dispose();
                            HOperatorSet.ClosingCircle(ho_RegionReduced, out ho_RegionClosing, hv_i_FailRegCloseSizes.TupleSelect(
                                hv_i));
                            ho_ConnectedRegions.Dispose();
                            HOperatorSet.Connection(ho_RegionClosing, out ho_ConnectedRegions);
                            ho__SelectRegs.Dispose();
                            HOperatorSet.SelectShape(ho_ConnectedRegions, out ho__SelectRegs, ((new HTuple("rect2_len1")).TupleConcat(
                                "rect2_len2")).TupleConcat("area"), hv_i_FailRegSelectOperation,
                                ((((hv__Rect2Len1.TupleSelect(hv_i))).TupleConcat(hv__Rect2Len2.TupleSelect(
                                hv_i)))).TupleConcat(hv_i_FailRegMinAreas.TupleSelect(hv_i)), ((new HTuple(999999)).TupleConcat(
                                999999)).TupleConcat(999999));
                            if ((int)(new HTuple(hv_i.TupleEqual(0))) != 0)
                            {
                                ho_o_ChipReg.Dispose();
                                HOperatorSet.AffineTransRegion(ho__SelectRegs, out ho_o_ChipReg, hv_o_HomModel2Img,
                                    "nearest_neighbor");
                            }
                            else
                            {
                                {
                                    HObject ExpTmpOutVar_0;
                                    HOperatorSet.ConcatObj(ho_EmptyRegion, ho__SelectRegs, out ExpTmpOutVar_0
                                        );
                                    ho_EmptyRegion.Dispose();
                                    ho_EmptyRegion = ExpTmpOutVar_0;
                                }
                            }
                            ho_SelectedRegions.Dispose();
                            HOperatorSet.CopyObj(ho_EmptyRegion, out ho_SelectedRegions, 1, -1);
                        }

                    }
                    else
                    {
                        ho_SelectedRegions.Dispose();
                        HOperatorSet.SelectShape(ho_ConnectedRegions1, out ho_SelectedRegions,
                            ((new HTuple("rect2_len1")).TupleConcat("rect2_len2")).TupleConcat(
                            "area"), hv_i_FailRegSelectOperation, ((((hv__Rect2Len1.TupleMin())).TupleConcat(
                            hv__Rect2Len2.TupleMin()))).TupleConcat(hv_i_FailRegMinAreas.TupleMin()
                            ), ((new HTuple(999999)).TupleConcat(999999)).TupleConcat(999999));
                    }
                    ho_o_FailureReg.Dispose();
                    HOperatorSet.AffineTransRegion(ho_SelectedRegions, out ho_o_FailureReg, hv_o_HomModel2Img,
                        "nearest_neighbor");
                }
                // catch (Exception) 
                catch (HalconException HDevExpDefaultException1)
                {
                    HDevExpDefaultException1.ToHTuple(out hv_Exception);
                    hv_o_ErrCode = -1;
                    hv_o_ErrString = hv_Exception.Clone();
                    ho_ImageAffinTrans.Dispose();
                    ho__inspectImage.Dispose();
                    ho__darkRegion.Dispose();
                    ho__lightRegion.Dispose();
                    ho_RegionUnion.Dispose();
                    ho_RegionDifference.Dispose();
                    ho_RegionClosing.Dispose();
                    ho_ConnectedRegions1.Dispose();
                    ho__AllRegs.Dispose();
                    ho_EmptyRegion.Dispose();
                    ho_ObjectSelected.Dispose();
                    ho_RegionReduced.Dispose();
                    ho_ObjectSelected1.Dispose();
                    ho_RegionIntersection.Dispose();
                    ho_ConnectedRegions.Dispose();
                    ho__SelectRegs.Dispose();
                    ho_SelectedRegions.Dispose();

                    return;
                }
                ho_ImageAffinTrans.Dispose();
                ho__inspectImage.Dispose();
                ho__darkRegion.Dispose();
                ho__lightRegion.Dispose();
                ho_RegionUnion.Dispose();
                ho_RegionDifference.Dispose();
                ho_RegionClosing.Dispose();
                ho_ConnectedRegions1.Dispose();
                ho__AllRegs.Dispose();
                ho_EmptyRegion.Dispose();
                ho_ObjectSelected.Dispose();
                ho_RegionReduced.Dispose();
                ho_ObjectSelected1.Dispose();
                ho_RegionIntersection.Dispose();
                ho_ConnectedRegions.Dispose();
                ho__SelectRegs.Dispose();
                ho_SelectedRegions.Dispose();

                return;
            }
            catch (HalconException HDevExpDefaultException)
            {
                ho_ImageAffinTrans.Dispose();
                ho__inspectImage.Dispose();
                ho__darkRegion.Dispose();
                ho__lightRegion.Dispose();
                ho_RegionUnion.Dispose();
                ho_RegionDifference.Dispose();
                ho_RegionClosing.Dispose();
                ho_ConnectedRegions1.Dispose();
                ho__AllRegs.Dispose();
                ho_EmptyRegion.Dispose();
                ho_ObjectSelected.Dispose();
                ho_RegionReduced.Dispose();
                ho_ObjectSelected1.Dispose();
                ho_RegionIntersection.Dispose();
                ho_ConnectedRegions.Dispose();
                ho__SelectRegs.Dispose();
                ho_SelectedRegions.Dispose();

                throw HDevExpDefaultException;
            }
        }

        public static void HTV_match_region_dilation(HObject ho_i_MatchReg, out HObject ho_o_MatchReDilation,
            HTuple hv_i_DilationSize)
        {




            // Local iconic variables 

            HObject ho_RegionUnion, ho_RegionTrans;
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_o_MatchReDilation);
            HOperatorSet.GenEmptyObj(out ho_RegionUnion);
            HOperatorSet.GenEmptyObj(out ho_RegionTrans);
            try
            {
                ho_RegionUnion.Dispose();
                HOperatorSet.Union1(ho_i_MatchReg, out ho_RegionUnion);
                ho_RegionTrans.Dispose();
                HOperatorSet.ShapeTrans(ho_RegionUnion, out ho_RegionTrans, "rectangle1");
                ho_o_MatchReDilation.Dispose();
                HOperatorSet.DilationRectangle1(ho_RegionTrans, out ho_o_MatchReDilation, hv_i_DilationSize,
                    hv_i_DilationSize);
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

        public static void HTV_calc_real_model_pos(HTuple hv_i_ModelID, HTuple hv_i_ModelType,
            HTuple hv_i_Row, HTuple hv_i_Col, HTuple hv_i_Angle, out HTuple hv_o_Row, out HTuple hv_o_Col)
        {



            // Local control variables 

            HTuple hv_Row = new HTuple(), hv_Column = new HTuple();
            HTuple hv_i = null, hv_HomMat2D = new HTuple(), hv__row = new HTuple();
            HTuple hv__col = new HTuple();
            // Initialize local and output iconic variables 
            hv_o_Row = new HTuple();
            hv_o_Col = new HTuple();
            if ((int)(new HTuple(hv_i_ModelType.TupleEqual("ncc"))) != 0)
            {
                HOperatorSet.GetNccModelOrigin(hv_i_ModelID, out hv_Row, out hv_Column);
            }
            else if ((int)(new HTuple(hv_i_ModelType.TupleEqual("shape"))) != 0)
            {
                HOperatorSet.GetShapeModelOrigin(hv_i_ModelID, out hv_Row, out hv_Column);
            }
            else
            {
                throw new HalconException("Wrong argument [modelType]=" + hv_i_ModelType);

                return;
            }
            for (hv_i = 0; (int)hv_i <= (int)((new HTuple(hv_i_Row.TupleLength())) - 1); hv_i = (int)hv_i + 1)
            {
                HOperatorSet.VectorAngleToRigid(0, 0, 0, hv_i_Row.TupleSelect(hv_i), hv_i_Col.TupleSelect(
                    hv_i), hv_i_Angle.TupleSelect(hv_i), out hv_HomMat2D);
                HOperatorSet.AffineTransPoint2d(hv_HomMat2D, -hv_Row, -hv_Column, out hv__row,
                    out hv__col);
                if (hv_o_Row == null)
                    hv_o_Row = new HTuple();
                hv_o_Row[hv_i] = hv__row;
                if (hv_o_Col == null)
                    hv_o_Col = new HTuple();
                hv_o_Col[hv_i] = hv__col;
            }

            return;
        }

        public static void HTV_inspect_ball(HObject ho_i_Image, HObject ho_i_PadReg, HTuple hv_i_OnIC,
            HTuple hv_i_MinRadius, HTuple hv_i_MaxRadius, out HTuple hv_o_BallRow, out HTuple hv_o_BallCol,
            out HTuple hv_o_BallRadius, out HTuple hv_o_ErrCode, out HTuple hv_o_ErrStr)
        {




            // Local iconic variables 

            HObject ho_ImageReduced = null, ho_Edges = null;
            HObject ho_Region = null, ho_RegionClosing = null, ho_RegionFillUp = null;
            HObject ho_RegionOpening = null, ho_Contours, ho_Cross, ho_ImageReduced1 = null;
            HObject ho_Region1 = null, ho_RegionFillUp1 = null, ho_RegionOpening1 = null;

            // Local control variables 

            HTuple hv_Area = new HTuple(), hv_Row = new HTuple();
            HTuple hv_Column = new HTuple(), hv_opening_size = new HTuple();
            HTuple hv_Width = null, hv_Height = null, hv_MetrologyHandle = null;
            HTuple hv_Index = null, hv__circles = null, hv_Row1 = null;
            HTuple hv_Column1 = null, hv_Area1 = new HTuple(), hv_Row2 = new HTuple();
            HTuple hv_Column2 = new HTuple(), hv_MetrologyHandle1 = new HTuple();
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_ImageReduced);
            HOperatorSet.GenEmptyObj(out ho_Edges);
            HOperatorSet.GenEmptyObj(out ho_Region);
            HOperatorSet.GenEmptyObj(out ho_RegionClosing);
            HOperatorSet.GenEmptyObj(out ho_RegionFillUp);
            HOperatorSet.GenEmptyObj(out ho_RegionOpening);
            HOperatorSet.GenEmptyObj(out ho_Contours);
            HOperatorSet.GenEmptyObj(out ho_Cross);
            HOperatorSet.GenEmptyObj(out ho_ImageReduced1);
            HOperatorSet.GenEmptyObj(out ho_Region1);
            HOperatorSet.GenEmptyObj(out ho_RegionFillUp1);
            HOperatorSet.GenEmptyObj(out ho_RegionOpening1);
            hv_o_BallRow = new HTuple();
            hv_o_BallCol = new HTuple();
            hv_o_BallRadius = new HTuple();
            try
            {
                hv_o_ErrCode = 0;
                hv_o_ErrStr = "";
                //1. 寻找球的中心
                if ((int)(hv_i_OnIC) != 0)
                {
                    HOperatorSet.AreaCenter(ho_i_PadReg, out hv_Area, out hv_Row, out hv_Column);
                }
                else
                {
                    ho_ImageReduced.Dispose();
                    HOperatorSet.ReduceDomain(ho_i_Image, ho_i_PadReg, out ho_ImageReduced);
                    ho_Edges.Dispose();
                    HOperatorSet.EdgesSubPix(ho_ImageReduced, out ho_Edges, "canny", 1, 30, 60);
                    ho_Region.Dispose();
                    HTV_contours_to_region(ho_Edges, out ho_Region, 0.5);
                    ho_RegionClosing.Dispose();
                    HOperatorSet.ClosingCircle(ho_Region, out ho_RegionClosing, hv_i_MinRadius);
                    ho_RegionFillUp.Dispose();
                    HOperatorSet.FillUpShape(ho_RegionClosing, out ho_RegionFillUp, "area", 1,
                        100);
                    hv_opening_size = hv_i_MaxRadius.Clone();
                    hv_Area = 0;
                    while ((int)((new HTuple(hv_opening_size.TupleGreater(hv_i_MinRadius - 1))).TupleAnd(
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
                        hv_o_ErrCode = 10;
                        hv_o_ErrStr = "Find no circle";
                        ho_ImageReduced.Dispose();
                        ho_Edges.Dispose();
                        ho_Region.Dispose();
                        ho_RegionClosing.Dispose();
                        ho_RegionFillUp.Dispose();
                        ho_RegionOpening.Dispose();
                        ho_Contours.Dispose();
                        ho_Cross.Dispose();
                        ho_ImageReduced1.Dispose();
                        ho_Region1.Dispose();
                        ho_RegionFillUp1.Dispose();
                        ho_RegionOpening1.Dispose();

                        return;
                    }
                    //none
                }
                //2. 生成圆测量窗
                //********************************************
                HOperatorSet.GetImageSize(ho_i_Image, out hv_Width, out hv_Height);
                HOperatorSet.CreateMetrologyModel(out hv_MetrologyHandle);
                HOperatorSet.SetMetrologyModelImageSize(hv_MetrologyHandle, hv_Width, hv_Height);
                HOperatorSet.AddMetrologyObjectCircleMeasure(hv_MetrologyHandle, hv_Row, hv_Column,
                    (hv_i_MinRadius + hv_i_MaxRadius) / 2, hv_i_MaxRadius - hv_i_MinRadius, 2, 1,
                    30, ((new HTuple("measure_transition")).TupleConcat("measure_distance")).TupleConcat(
                    "min_score"), ((new HTuple("positive")).TupleConcat(3)).TupleConcat(0.6),
                    out hv_Index);
                ho_ImageReduced.Dispose();
                HOperatorSet.ReduceDomain(ho_i_Image, ho_i_PadReg, out ho_ImageReduced);
                HOperatorSet.ApplyMetrologyModel(ho_ImageReduced, hv_MetrologyHandle);
                HOperatorSet.GetMetrologyObjectResult(hv_MetrologyHandle, hv_Index, "all",
                    "result_type", "all_param", out hv__circles);
                ho_Contours.Dispose();
                HOperatorSet.GetMetrologyObjectMeasures(out ho_Contours, hv_MetrologyHandle,
                    "all", "all", out hv_Row1, out hv_Column1);
                ho_Cross.Dispose();
                HOperatorSet.GenCrossContourXld(out ho_Cross, hv_Row1, hv_Column1, 6, 0.785398);
                HOperatorSet.ClearMetrologyModel(hv_MetrologyHandle);
                //************************************************
                if ((int)(new HTuple((new HTuple(hv__circles.TupleLength())).TupleNotEqual(
                    3))) != 0)
                {
                    ho_ImageReduced1.Dispose();
                    HOperatorSet.ReduceDomain(ho_i_Image, ho_i_PadReg, out ho_ImageReduced1);
                    ho_Region1.Dispose();
                    HOperatorSet.Threshold(ho_ImageReduced1, out ho_Region1, 0, 150);
                    ho_RegionFillUp1.Dispose();
                    HOperatorSet.FillUp(ho_Region1, out ho_RegionFillUp1);
                    ho_RegionOpening1.Dispose();
                    HOperatorSet.OpeningCircle(ho_RegionFillUp1, out ho_RegionOpening1, 5);
                    HOperatorSet.AreaCenter(ho_RegionOpening1, out hv_Area1, out hv_Row2, out hv_Column2);
                    HOperatorSet.GetImageSize(ho_i_Image, out hv_Width, out hv_Height);
                    HOperatorSet.CreateMetrologyModel(out hv_MetrologyHandle1);
                    HOperatorSet.SetMetrologyModelImageSize(hv_MetrologyHandle1, hv_Width, hv_Height);
                    HOperatorSet.AddMetrologyObjectCircleMeasure(hv_MetrologyHandle1, hv_Row2,
                        hv_Column2, (hv_i_MinRadius + hv_i_MaxRadius) / 2, hv_i_MaxRadius - hv_i_MinRadius,
                        2, 1, 30, ((new HTuple("measure_transition")).TupleConcat("measure_distance")).TupleConcat(
                        "min_score"), ((new HTuple("positive")).TupleConcat(3)).TupleConcat(0.6),
                        out hv_Index);
                    HOperatorSet.ApplyMetrologyModel(ho_ImageReduced1, hv_MetrologyHandle1);
                    HOperatorSet.GetMetrologyObjectResult(hv_MetrologyHandle1, hv_Index, "all",
                        "result_type", "all_param", out hv__circles);
                    ho_Contours.Dispose();
                    HOperatorSet.GetMetrologyObjectMeasures(out ho_Contours, hv_MetrologyHandle1,
                        "all", "all", out hv_Row, out hv_Column);
                    ho_Cross.Dispose();
                    HOperatorSet.GenCrossContourXld(out ho_Cross, hv_Row, hv_Column, 6, 0.785398);
                    HOperatorSet.ClearMetrologyModel(hv_MetrologyHandle1);
                    if ((int)(new HTuple((new HTuple(hv__circles.TupleLength())).TupleNotEqual(
                        3))) != 0)
                    {
                        hv_o_ErrCode = 22;
                        hv_o_ErrStr = "Find no circle";
                        ho_ImageReduced.Dispose();
                        ho_Edges.Dispose();
                        ho_Region.Dispose();
                        ho_RegionClosing.Dispose();
                        ho_RegionFillUp.Dispose();
                        ho_RegionOpening.Dispose();
                        ho_Contours.Dispose();
                        ho_Cross.Dispose();
                        ho_ImageReduced1.Dispose();
                        ho_Region1.Dispose();
                        ho_RegionFillUp1.Dispose();
                        ho_RegionOpening1.Dispose();

                        return;
                    }
                    else
                    {
                        if ((int)(new HTuple((new HTuple(hv__circles.TupleLength())).TupleEqual(
                            3))) != 0)
                        {
                            hv_o_ErrCode = 11;
                            hv_o_ErrStr = "Golden bond shift";
                            ho_ImageReduced.Dispose();
                            ho_Edges.Dispose();
                            ho_Region.Dispose();
                            ho_RegionClosing.Dispose();
                            ho_RegionFillUp.Dispose();
                            ho_RegionOpening.Dispose();
                            ho_Contours.Dispose();
                            ho_Cross.Dispose();
                            ho_ImageReduced1.Dispose();
                            ho_Region1.Dispose();
                            ho_RegionFillUp1.Dispose();
                            ho_RegionOpening1.Dispose();

                            return;
                        }
                    }
                }

                //****************************************************
                //
                if ((int)(new HTuple(((hv__circles.TupleSelect(2))).TupleLess(hv_i_MinRadius))) != 0)
                {
                    hv_o_ErrCode = 10;
                    hv_o_ErrStr = "Too small circle";
                }
                else if ((int)(new HTuple(((hv__circles.TupleSelect(2))).TupleGreater(
                    hv_i_MaxRadius))) != 0)
                {
                    hv_o_ErrCode = 10;
                    hv_o_ErrStr = "Too big circle";
                }
                else
                {
                    hv_o_BallRow = hv__circles[0];
                    hv_o_BallCol = hv__circles[1];
                    hv_o_BallRadius = hv__circles[2];
                }
                //********

                ho_ImageReduced.Dispose();
                ho_Edges.Dispose();
                ho_Region.Dispose();
                ho_RegionClosing.Dispose();
                ho_RegionFillUp.Dispose();
                ho_RegionOpening.Dispose();
                ho_Contours.Dispose();
                ho_Cross.Dispose();
                ho_ImageReduced1.Dispose();
                ho_Region1.Dispose();
                ho_RegionFillUp1.Dispose();
                ho_RegionOpening1.Dispose();

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
                ho_Contours.Dispose();
                ho_Cross.Dispose();
                ho_ImageReduced1.Dispose();
                ho_Region1.Dispose();
                ho_RegionFillUp1.Dispose();
                ho_RegionOpening1.Dispose();

                throw HDevExpDefaultException;
            }
        }


        public static void HTV_Epoxy_calculation(HObject ho_i_FrameConRegions, HObject ho_i_NewInspectReg,
    HObject ho_i_FrameSubRegs, out HObject ho_o_FrameRegs)
        {



            // Stack for temporary objects 
            HObject[] OTemp = new HObject[20];

            // Local iconic variables 

            HObject ho_unionDefectRegion, ho_unionReg;
            HObject ho_connectRegion, ho_Contours, ho_dieEpoxyRegion;
            HObject ho_EpoxyRegion, ho__EpoxyRegion, ho_FrameRegs;

            // Local control variables 

            HTuple hv_dieArea = null, hv_dieCenterRow = null;
            HTuple hv_dieCenterCol = null, hv_IsInside = null, hv_dieInd = null;
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_o_FrameRegs);
            HOperatorSet.GenEmptyObj(out ho_unionDefectRegion);
            HOperatorSet.GenEmptyObj(out ho_unionReg);
            HOperatorSet.GenEmptyObj(out ho_connectRegion);
            HOperatorSet.GenEmptyObj(out ho_Contours);
            HOperatorSet.GenEmptyObj(out ho_dieEpoxyRegion);
            HOperatorSet.GenEmptyObj(out ho_EpoxyRegion);
            HOperatorSet.GenEmptyObj(out ho__EpoxyRegion);
            HOperatorSet.GenEmptyObj(out ho_FrameRegs);
            try
            {



                ho_unionDefectRegion.Dispose();
                HOperatorSet.Union1(ho_i_FrameConRegions, out ho_unionDefectRegion);
                ho_unionReg.Dispose();
                HOperatorSet.Union2(ho_unionDefectRegion, ho_i_NewInspectReg, out ho_unionReg
                    );
                ho_connectRegion.Dispose();
                HOperatorSet.Connection(ho_unionReg, out ho_connectRegion);
                ho_Contours.Dispose();
                HOperatorSet.GenContourRegionXld(ho_connectRegion, out ho_Contours, "border");
                HOperatorSet.AreaCenter(ho_i_NewInspectReg, out hv_dieArea, out hv_dieCenterRow,
                    out hv_dieCenterCol);
                HOperatorSet.TestXldPoint(ho_Contours, hv_dieCenterRow, hv_dieCenterCol, out hv_IsInside);
                HOperatorSet.TupleFind(hv_IsInside, 1, out hv_dieInd);
                ho_dieEpoxyRegion.Dispose();
                HOperatorSet.SelectObj(ho_connectRegion, out ho_dieEpoxyRegion, hv_dieInd + 1);
                ho_EpoxyRegion.Dispose();
                HOperatorSet.Difference(ho_dieEpoxyRegion, ho_i_NewInspectReg, out ho_EpoxyRegion
                    );
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.Connection(ho_EpoxyRegion, out ExpTmpOutVar_0);
                    ho_EpoxyRegion.Dispose();
                    ho_EpoxyRegion = ExpTmpOutVar_0;
                }
                ho__EpoxyRegion.Dispose();
                HOperatorSet.SelectShape(ho_EpoxyRegion, out ho__EpoxyRegion, ((new HTuple("rect2_len1")).TupleConcat(
                    "rect2_len2")).TupleConcat("area"), "and", ((new HTuple(10)).TupleConcat(
                    5)).TupleConcat(30), ((new HTuple(999999)).TupleConcat(999999)).TupleConcat(
                    9999999));


                ho_FrameRegs.Dispose();
                HOperatorSet.Difference(ho_unionReg, ho_dieEpoxyRegion, out ho_FrameRegs);
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.Difference(ho_FrameRegs, ho_i_FrameSubRegs, out ExpTmpOutVar_0
                        );
                    ho_FrameRegs.Dispose();
                    ho_FrameRegs = ExpTmpOutVar_0;
                }
                ho_o_FrameRegs.Dispose();
                HOperatorSet.Connection(ho_FrameRegs, out ho_o_FrameRegs);
                ho_unionDefectRegion.Dispose();
                ho_unionReg.Dispose();
                ho_connectRegion.Dispose();
                ho_Contours.Dispose();
                ho_dieEpoxyRegion.Dispose();
                ho_EpoxyRegion.Dispose();
                ho__EpoxyRegion.Dispose();
                ho_FrameRegs.Dispose();

                return;
            }
            catch (HalconException HDevExpDefaultException)
            {
                ho_unionDefectRegion.Dispose();
                ho_unionReg.Dispose();
                ho_connectRegion.Dispose();
                ho_Contours.Dispose();
                ho_dieEpoxyRegion.Dispose();
                ho_EpoxyRegion.Dispose();
                ho__EpoxyRegion.Dispose();
                ho_FrameRegs.Dispose();

                throw HDevExpDefaultException;
            }
        }
        public static void HTV_gen_mapping(out HObject ho_o_Map, HTuple hv_i_BlockNum, HTuple hv_i_RowNumInBlock,
            HTuple hv_i_CowNumInBlock, HTuple hv_i_GridSize, HTupleVector/*{eTupleVector,Dim=1}*/ hvec_i_Result)
        {



            // Stack for temporary objects 
            HObject[] OTemp = new HObject[20];

            // Local iconic variables 

            HObject ho_RegionLines = null, ho_Circle = null;
            HObject ho_Rectangle = null;

            // Local control variables 

            HTuple hv__Dis = null, hv_b = null, hv_r = new HTuple();
            HTuple hv__Result = new HTuple(), hv_c = new HTuple();
            HTuple hv__res = new HTuple(), hv__top = new HTuple();
            HTuple hv__left = new HTuple(), hv__size = new HTuple();
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_o_Map);
            HOperatorSet.GenEmptyObj(out ho_RegionLines);
            HOperatorSet.GenEmptyObj(out ho_Circle);
            HOperatorSet.GenEmptyObj(out ho_Rectangle);
            try
            {
                ho_o_Map.Dispose();
                HOperatorSet.GenEmptyObj(out ho_o_Map);
                if (HDevWindowStack.IsOpen())
                {
                    HOperatorSet.SetDraw(HDevWindowStack.GetActive(), "margin");
                }
                if (HDevWindowStack.IsOpen())
                {
                    HOperatorSet.SetColor(HDevWindowStack.GetActive(), "green");
                }
                //
                //1. 画棋盘格子
                hv__Dis = hv_i_GridSize / 4;
                HTuple end_val6 = hv_i_BlockNum - 1;
                HTuple step_val6 = 1;
                for (hv_b = 0; hv_b.Continue(end_val6, step_val6); hv_b = hv_b.TupleAdd(step_val6))
                {
                    HTuple end_val7 = hv_i_RowNumInBlock - 1;
                    HTuple step_val7 = 1;
                    for (hv_r = 0; hv_r.Continue(end_val7, step_val7); hv_r = hv_r.TupleAdd(step_val7))
                    {
                        hv__Result = hvec_i_Result[hv_r].T.Clone();
                        HTuple end_val9 = hv_i_CowNumInBlock - 1;
                        HTuple step_val9 = 1;
                        for (hv_c = 0; hv_c.Continue(end_val9, step_val9); hv_c = hv_c.TupleAdd(step_val9))
                        {
                            hv__res = hv__Result.TupleSelect((hv_b * hv_i_CowNumInBlock) + hv_c);
                            hv__top = (hv_i_GridSize * hv_r) + hv__Dis;
                            hv__left = (hv_i_GridSize * (((hv_i_CowNumInBlock * hv_b) + hv_c) + hv_b)) + hv__Dis;
                            hv__size = hv_i_GridSize - (2 * hv__Dis);
                            if ((int)(new HTuple(hv__res.TupleGreater(0))) != 0)
                            {
                                ho_RegionLines.Dispose();
                                HOperatorSet.GenRegionLine(out ho_RegionLines, hv__top + (hv__size / 2),
                                    hv__left, hv__top + hv__size, hv__left + (hv__size / 2));
                                {
                                    HObject ExpTmpOutVar_0;
                                    HOperatorSet.ConcatObj(ho_o_Map, ho_RegionLines, out ExpTmpOutVar_0
                                        );
                                    ho_o_Map.Dispose();
                                    ho_o_Map = ExpTmpOutVar_0;
                                }
                                ho_RegionLines.Dispose();
                                HOperatorSet.GenRegionLine(out ho_RegionLines, hv__top, hv__left + hv__size,
                                    hv__top + hv__size, hv__left + (hv__size / 2));
                                {
                                    HObject ExpTmpOutVar_0;
                                    HOperatorSet.ConcatObj(ho_o_Map, ho_RegionLines, out ExpTmpOutVar_0
                                        );
                                    ho_o_Map.Dispose();
                                    ho_o_Map = ExpTmpOutVar_0;
                                }
                            }
                            else if ((int)(new HTuple(hv__res.TupleLess(0))) != 0)
                            {
                                ho_RegionLines.Dispose();
                                HOperatorSet.GenRegionLine(out ho_RegionLines, hv__top, hv__left, hv__top + hv__size,
                                    hv__left + hv__size);
                                {
                                    HObject ExpTmpOutVar_0;
                                    HOperatorSet.ConcatObj(ho_o_Map, ho_RegionLines, out ExpTmpOutVar_0
                                        );
                                    ho_o_Map.Dispose();
                                    ho_o_Map = ExpTmpOutVar_0;
                                }
                                ho_RegionLines.Dispose();
                                HOperatorSet.GenRegionLine(out ho_RegionLines, hv__top, hv__left + hv__size,
                                    hv__top + hv__size, hv__left);
                                {
                                    HObject ExpTmpOutVar_0;
                                    HOperatorSet.ConcatObj(ho_o_Map, ho_RegionLines, out ExpTmpOutVar_0
                                        );
                                    ho_o_Map.Dispose();
                                    ho_o_Map = ExpTmpOutVar_0;
                                }
                            }
                            else
                            {
                                ho_Circle.Dispose();
                                HOperatorSet.GenCircle(out ho_Circle, hv__top + (hv__size / 2), hv__left + (hv__size / 2),
                                    hv__size / 2);
                                {
                                    HObject ExpTmpOutVar_0;
                                    HOperatorSet.ConcatObj(ho_o_Map, ho_Circle, out ExpTmpOutVar_0);
                                    ho_o_Map.Dispose();
                                    ho_o_Map = ExpTmpOutVar_0;
                                }
                            }
                        }
                    }
                    //1. 画出外框
                    ho_Rectangle.Dispose();
                    HOperatorSet.GenRectangle1(out ho_Rectangle, 0, 0, hv_i_GridSize * hv_i_RowNumInBlock,
                        hv_i_GridSize * (((hv_i_CowNumInBlock * hv_i_BlockNum) + hv_i_BlockNum) - 1));
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.ConcatObj(ho_o_Map, ho_Rectangle, out ExpTmpOutVar_0);
                        ho_o_Map.Dispose();
                        ho_o_Map = ExpTmpOutVar_0;
                    }
                }
                //下面是之前用给定整个Mapping宽度的思路
                //1. 计算画图间隔，考虑Block之间的空白
                //_GridRowNum := i_RowNumInBlock
                //_GridColNum := i_CowNumInBlock*i_BlockNum  + i_BlockNum - 1
                //_GridRowDis := i_GridHeight/_GridRowNum
                //_GridColDis := i_GridSize/_GridColNum
                //
                //2. 画出外框
                //gen_rectangle1 (Rectangle, 0, 0, i_GridHeight, i_GridSize)
                //concat_obj (o_Map, Rectangle, o_Map)
                //3. 画棋盘格子
                //tuple_min2 (_GridRowDis, _GridColDis, _Dis)
                //_Dis := _Dis/4
                //for b := 0 to i_BlockNum-1 by 1
                //for r := 0 to i_RowNumInBlock-1 by 1
                //_Result := i_Result.at(r)
                //for c := 0 to i_CowNumInBlock-1 by 1
                //_res := _Result[b*i_CowNumInBlock+c]
                //_top := _GridRowDis*r+_Dis
                //_left := _GridColDis*(i_CowNumInBlock*b+c+b)+_Dis
                //_height := _GridRowDis-2*_Dis
                //_width := _GridColDis-2*_Dis
                //if (_res>0)
                //gen_region_line (RegionLines, _top+_height/2, _left, _top+_height, _left+_width/2)
                //concat_obj (o_Map, RegionLines, o_Map)
                //gen_region_line (RegionLines, _top, _left+_width, _top+_height, _left+_width/2)
                //concat_obj (o_Map, RegionLines, o_Map)
                //elseif (_res<0)
                //gen_region_line (RegionLines, _top, _left, _top+_height, _left+_width)
                //concat_obj (o_Map, RegionLines, o_Map)
                //gen_region_line (RegionLines, _top, _left+_width, _top+_height, _left)
                //concat_obj (o_Map, RegionLines, o_Map)
                //else
                //gen_ellipse (Ellipse, _top+_height/2, _left+_width/2, 0, _width/2, _height/2)
                //concat_obj (o_Map, Ellipse, o_Map)
                //endif
                //endfor
                //endfor
                //endfor
                ho_RegionLines.Dispose();
                ho_Circle.Dispose();
                ho_Rectangle.Dispose();

                return;
            }
            catch (HalconException HDevExpDefaultException)
            {
                ho_RegionLines.Dispose();
                ho_Circle.Dispose();
                ho_Rectangle.Dispose();

                throw HDevExpDefaultException;
            }
        }

        public static void HTV_read_coarse_model(out HObject ho_o_MatchReg, HTuple hv_i_ModelDir,
            out HTuple hv_o_ModelID, out HTuple hv_o_ModeType, out HTuple hv_o_ImgIdx, out HTuple hv_o_ErrCode,
            out HTuple hv_o_ErrStr)
        {



            // Local iconic variables 

            // Local control variables 

            HTuple hv_O_ErrStr = null, hv__ModelType = new HTuple();
            HTuple hv_Exception = null;
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_o_MatchReg);
            hv_o_ModelID = new HTuple();
            hv_o_ModeType = new HTuple();
            hv_o_ImgIdx = new HTuple();
            hv_o_ErrStr = new HTuple();
            hv_o_ErrCode = 0;
            hv_O_ErrStr = "";
            try
            {
                ho_o_MatchReg.Dispose();
                HOperatorSet.GenEmptyObj(out ho_o_MatchReg);
                //1. read match model
                HTV_read_model(hv_i_ModelDir, out hv_o_ModelID, out hv__ModelType);
                hv_o_ModeType = hv__ModelType.Clone();
                //2 read match region
                ho_o_MatchReg.Dispose();
                HOperatorSet.ReadRegion(out ho_o_MatchReg, hv_i_ModelDir + "Match_Region.reg");
                HOperatorSet.ReadTuple(hv_i_ModelDir + "Image_Index.tup", out hv_o_ImgIdx);
            }
            // catch (Exception) 
            catch (HalconException HDevExpDefaultException1)
            {
                HDevExpDefaultException1.ToHTuple(out hv_Exception);
                hv_o_ErrCode = -1;
                hv_O_ErrStr = hv_Exception.Clone();

                return;
            }

            return;
        }

        public static void HTV_find_coarse_model(HObject ho_i_Image, HObject ho_i_CoarseReg,
            HTuple hv_i_ModelID, HTuple hv_i_ModelType, HTuple hv_i_MatchDilationSize, HTuple hv_i_AngleStart,
            HTuple hv_i_AngleExt, HTuple hv_i_MinScore, out HTuple hv_o_Row, out HTuple hv_o_Col,
            out HTuple hv_o_Angle, out HTuple hv_o_ErrCode, out HTuple hv_o_ErrString, out HTuple hv_o_HomMatModel2Img)
        {




            // Local iconic variables 

            HObject ho_o_MatchReDilation = null;

            // Local control variables 

            HTuple hv__num = new HTuple(), hv_o_score = new HTuple();
            HTuple hv_Exception = null;
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_o_MatchReDilation);
            hv_o_Row = new HTuple();
            hv_o_Col = new HTuple();
            hv_o_Angle = new HTuple();
            hv_o_HomMatModel2Img = new HTuple();
            try
            {
                hv_o_ErrCode = 0;
                hv_o_ErrString = "";
                try
                {
                    ho_o_MatchReDilation.Dispose();
                    HTV_match_region_dilation(ho_i_CoarseReg, out ho_o_MatchReDilation, hv_i_MatchDilationSize);
                    HTV.HTV_find_model(ho_i_Image, ho_o_MatchReDilation, hv_i_ModelID, hv_i_ModelType,
                        hv_i_AngleStart, hv_i_AngleExt, hv_i_MinScore, 1, out hv_o_Row, out hv_o_Col,
                        out hv_o_Angle, out hv__num, out hv_o_score);
                    //1. match object
                    //HTV_Find_All (i_Image, i_ModelType, i_ModelID, i_AngleStart, i_AngleExt, i_MinScore, o_Row, o_Col, _num, o_Angle)
                    if ((int)(new HTuple(hv__num.TupleEqual(0))) != 0)
                    {
                        hv_o_ErrCode = 1;
                        hv_o_ErrString = "Find no object";
                        ho_o_MatchReDilation.Dispose();

                        return;
                    }
                    //2. calculate affine hom mat
                    HOperatorSet.VectorAngleToRigid(0, 0, 0, hv_o_Row, hv_o_Col, hv_o_Angle,
                        out hv_o_HomMatModel2Img);
                }
                // catch (Exception) 
                catch (HalconException HDevExpDefaultException1)
                {
                    HDevExpDefaultException1.ToHTuple(out hv_Exception);
                    hv_o_ErrCode = -1;
                    hv_o_ErrString = hv_Exception.Clone();
                    ho_o_MatchReDilation.Dispose();

                    return;
                }
                ho_o_MatchReDilation.Dispose();

                return;
            }
            catch (HalconException HDevExpDefaultException)
            {
                ho_o_MatchReDilation.Dispose();

                throw HDevExpDefaultException;
            }
        }
        public static void HTV_inspect_bond_wire(HObjectVector/*{eObjectVector,Dim=1}*/ hvec_i_Images,
    HObject ho_i_Bond1Regs, HObject ho_i_Bond2Regs, out HObject ho_o_FailRegs, out HObject ho_o_Wires,
    out HObject ho_o_Bond1Balls, out HObject ho_o_Bond2Balls, HTuple hv_i_Bond1OnIC,
    HTuple hv_i_Bond1ImgIdx, HTuple hv_i_Bond2OnIC, HTuple hv_i_Bond2ImgIdx, HTuple hv_i_Bond2BallNums,
    HTuple hv_i_Bond2ModelID, HTuple hv_i_Bond2ModelType, HTuple hv_i_Bond1RadiusMin,
    HTuple hv_i_Bond1RadiusMax, HTuple hv_i_Bond2AngleSingleExt, HTuple hv_i_Bond2MinScore,
    HTuple hv_i_WireImgIdx, HTuple hv_i_LineSearchLen, HTuple hv_i_LineClipLen,
    HTuple hv_i_LineWidth, HTuple hv_i_LineContrast, HTuple hv_i_LineMinSegLen,
    HTuple hv_i_LineAngleExt, HTuple hv_i_LineMaxGap, out HTuple hv_o_ErrCode, out HTuple hv_o_ErrStr,
    out HTupleVector/*{eTupleVector,Dim=1}*/ hvec_o_bondwire_Value)
        {




            // Stack for temporary objects 
            HObject[] OTemp = new HObject[20];

            // Local iconic variables 

            HObject ho__Bond2Reg = null, ho__Bond1Regs = null;
            HObject ho__Bond1Reg = null, ho__Ball1 = null, ho__Ball2 = null;
            HObject ho__Bond1RegUnion = null, ho__TrackReg = null, ho__Wires = null;

            // Local control variables 

            HTuple hv_Number = new HTuple(), hv__Bond1LastIdx = new HTuple();
            HTuple hv_i = new HTuple(), hv__Bond2BallNum = new HTuple();
            HTuple hv__idx = new HTuple(), hv__Bond1OnIC = new HTuple();
            HTuple hv__Bond1ImgIdx = new HTuple(), hv__WireImgIdx = new HTuple();
            HTuple hv__Bond2OnIC = new HTuple(), hv__Bond2ImgIdx = new HTuple();
            HTuple hv_idx = new HTuple(), hv__ImgIdx = new HTuple();
            HTuple hv__Ball1Row = new HTuple(), hv__Ball1Col = new HTuple();
            HTuple hv__Ball1Rad = new HTuple(), hv__err = new HTuple();
            HTuple hv__str = new HTuple(), hv_Area = new HTuple();
            HTuple hv__Ball2Row = new HTuple(), hv__Ball2Col = new HTuple();
            HTuple hv__Ball2Rad = new HTuple(), hv_Area1 = new HTuple();
            HTuple hv_Row1 = new HTuple(), hv_Column1 = new HTuple();
            HTuple hv_Row2 = new HTuple(), hv_Column2 = new HTuple();
            HTuple hv_Angle = new HTuple(), hv__Row = new HTuple();
            HTuple hv__Col = new HTuple(), hv__Angle = new HTuple();
            HTuple hv__num = new HTuple(), hv_o_bondscore = new HTuple();
            HTuple hv__Dnum = new HTuple(), hv_o_score1 = new HTuple();
            HTuple hv_HomMat2DIdentity = new HTuple(), hv_HomMat2DRotate = new HTuple();
            HTuple hv__NewRow = new HTuple(), hv__NewCol = new HTuple();
            HTuple hv_Indices = new HTuple(), hv_line_gap = new HTuple();
            HTuple hv_Max = new HTuple(), hv_Exception = null;
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_o_FailRegs);
            HOperatorSet.GenEmptyObj(out ho_o_Wires);
            HOperatorSet.GenEmptyObj(out ho_o_Bond1Balls);
            HOperatorSet.GenEmptyObj(out ho_o_Bond2Balls);
            HOperatorSet.GenEmptyObj(out ho__Bond2Reg);
            HOperatorSet.GenEmptyObj(out ho__Bond1Regs);
            HOperatorSet.GenEmptyObj(out ho__Bond1Reg);
            HOperatorSet.GenEmptyObj(out ho__Ball1);
            HOperatorSet.GenEmptyObj(out ho__Ball2);
            HOperatorSet.GenEmptyObj(out ho__Bond1RegUnion);
            HOperatorSet.GenEmptyObj(out ho__TrackReg);
            HOperatorSet.GenEmptyObj(out ho__Wires);
            try
            {
                hv_o_ErrCode = 0;
                hv_o_ErrStr = "";
                //第一个参数为焊点的半径 若没有找到则为-1
                //第二个参数为金线的最大断线距离 若没有金线则为-1
                //第三个参数为第二焊点的匹配分数 一般小于0.6认为第二焊点脱落
                hvec_o_bondwire_Value = (((new HTupleVector(1).Insert(0, new HTupleVector(new HTuple()))).Insert(
                    1, new HTupleVector(new HTuple()))).Insert(2, new HTupleVector(new HTuple())));
                try
                {
                    ho_o_FailRegs.Dispose();
                    HOperatorSet.GenEmptyObj(out ho_o_FailRegs);
                    ho_o_Bond1Balls.Dispose();
                    HOperatorSet.GenEmptyObj(out ho_o_Bond1Balls);
                    ho_o_Bond2Balls.Dispose();
                    HOperatorSet.GenEmptyObj(out ho_o_Bond2Balls);
                    ho_o_Wires.Dispose();
                    HOperatorSet.GenEmptyObj(out ho_o_Wires);
                    //0. prepare
                    HOperatorSet.CountObj(ho_i_Bond2Regs, out hv_Number);
                    hv__Bond1LastIdx = 0;
                    HTuple end_val14 = hv_Number;
                    HTuple step_val14 = 1;
                    for (hv_i = 1; hv_i.Continue(end_val14, step_val14); hv_i = hv_i.TupleAdd(step_val14))
                    {
                        //1.选择每次检测的一组或者多组金球（第二焊点(LeadPad)上可能会有两个或者多个）
                        ho__Bond2Reg.Dispose();
                        HOperatorSet.SelectObj(ho_i_Bond2Regs, out ho__Bond2Reg, hv_i);
                        hv__Bond2BallNum = hv_i_Bond2BallNums.TupleSelect(hv_i - 1);
                        //
                        hv__idx = hv__Bond1LastIdx.Clone();
                        hv__Bond1LastIdx = hv__Bond1LastIdx + hv__Bond2BallNum;
                        ho__Bond1Regs.Dispose();
                        HOperatorSet.SelectObj(ho_i_Bond1Regs, out ho__Bond1Regs, HTuple.TupleGenSequence(
                            hv__idx + 1, hv__Bond1LastIdx, 1));
                        hv__Bond1OnIC = hv_i_Bond1OnIC.TupleSelectRange(hv__idx, hv__Bond1LastIdx - 1);
                        hv__Bond1ImgIdx = hv_i_Bond1ImgIdx.TupleSelectRange(hv__idx, hv__Bond1LastIdx - 1);
                        hv__WireImgIdx = hv_i_WireImgIdx.TupleSelectRange(hv__idx, hv__Bond1LastIdx - 1);
                        hv__Bond2OnIC = hv_i_Bond2OnIC.TupleSelect(hv_i - 1);
                        hv__Bond2ImgIdx = hv_i_Bond2ImgIdx.TupleSelect(hv_i - 1);
                        //
                        HTuple end_val28 = hv__Bond2BallNum;
                        HTuple step_val28 = 1;
                        for (hv_idx = 1; hv_idx.Continue(end_val28, step_val28); hv_idx = hv_idx.TupleAdd(step_val28))
                        {
                            //2. 第一焊点（焊球）检测(在pad上测量一个圆）
                            ho__Bond1Reg.Dispose();
                            HOperatorSet.SelectObj(ho__Bond1Regs, out ho__Bond1Reg, hv_idx);
                            hv__ImgIdx = hv__Bond1ImgIdx.TupleSelect(hv_idx - 1);

                            using (HDevDisposeHelper dh = new HDevDisposeHelper())
                            {
                                HTV_inspect_ball(hvec_i_Images[hv__ImgIdx].O, ho__Bond1Reg, hv__Bond1OnIC.TupleSelect(
                                    hv_idx - 1), hv_i_Bond1RadiusMin, hv_i_Bond1RadiusMax, out hv__Ball1Row,
                                    out hv__Ball1Col, out hv__Ball1Rad, out hv__err, out hv__str);
                            }
                            if ((int)(new HTuple(hv__Ball1Rad.TupleNotEqual(0))) != 0)
                            {
                                hvec_o_bondwire_Value[0] = new HTupleVector((hvec_o_bondwire_Value[0].T).TupleConcat(
                                    hv__Ball1Rad));
                            }
                            else
                            {
                                hvec_o_bondwire_Value[0] = new HTupleVector((hvec_o_bondwire_Value[0].T).TupleConcat(
                                    -1));
                            }
                            if ((int)(new HTuple(hv__err.TupleNotEqual(0))) != 0)
                            {
                                {
                                    HObject ExpTmpOutVar_0;
                                    HOperatorSet.ConcatObj(ho_o_FailRegs, ho__Bond1Reg, out ExpTmpOutVar_0
                                        );
                                    ho_o_FailRegs.Dispose();
                                    ho_o_FailRegs = ExpTmpOutVar_0;
                                }
                                HOperatorSet.AreaCenter(ho__Bond1Reg, out hv_Area, out hv__Ball1Row,
                                    out hv__Ball1Col);
                                hv__Ball1Rad = ((hv_i_Bond1RadiusMin.TupleConcat(hv_i_Bond1RadiusMax))).TupleMean()
                                    ;
                                hv_o_ErrCode = hv_o_ErrCode.TupleConcat(hv__err);
                                //如果找不到直接进入下一批循环
                                //break
                            }
                            ho__Ball1.Dispose();
                            HOperatorSet.GenCircle(out ho__Ball1, hv__Ball1Row, hv__Ball1Col, hv__Ball1Rad);
                            {
                                HObject ExpTmpOutVar_0;
                                HOperatorSet.ConcatObj(ho_o_Bond1Balls, ho__Ball1, out ExpTmpOutVar_0
                                    );
                                ho_o_Bond1Balls.Dispose();
                                ho_o_Bond1Balls = ExpTmpOutVar_0;
                            }
                            //3. 第二焊点（鱼尾）检测(如果在IC上则用焊球的检测方法，否则用模板匹配）
                            hv__ImgIdx = hv__Bond2ImgIdx.Clone();
                            if ((int)(hv__Bond2OnIC) != 0)
                            {
                                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                                {
                                    HTV_inspect_ball(hvec_i_Images[hv__ImgIdx].O, ho__Bond2Reg, 1, hv_i_Bond1RadiusMin,
                                        hv_i_Bond1RadiusMax, out hv__Ball2Row, out hv__Ball2Col, out hv__Ball2Rad,
                                        out hv__err, out hv__str);
                                }
                                if ((int)(new HTuple(hv__err.TupleNotEqual(0))) != 0)
                                {
                                    {
                                        HObject ExpTmpOutVar_0;
                                        HOperatorSet.ConcatObj(ho_o_FailRegs, ho__Bond2Reg, out ExpTmpOutVar_0
                                            );
                                        ho_o_FailRegs.Dispose();
                                        ho_o_FailRegs = ExpTmpOutVar_0;
                                        hv_o_ErrCode = hv_o_ErrCode.TupleConcat(hv__err);
                                    }
                                    //如果找不到直接进入下一组循环
                                    break;
                                }
                                ho__Ball2.Dispose();
                                HOperatorSet.GenCircle(out ho__Ball2, hv__Ball2Row, hv__Ball2Col, hv__Ball2Rad);
                                {
                                    HObject ExpTmpOutVar_0;
                                    HOperatorSet.ConcatObj(ho_o_Bond2Balls, ho__Ball2, out ExpTmpOutVar_0
                                        );
                                    ho_o_Bond2Balls.Dispose();
                                    ho_o_Bond2Balls = ExpTmpOutVar_0;
                                }
                            }
                            else
                            {
                                //模板匹配只需要匹配一次，把num个球都找到，所以只在第一次进入循环时使用
                                if ((int)(new HTuple(hv_idx.TupleEqual(1))) != 0)
                                {
                                    //以两个区域中心连线为基准角度，用于判定找到的金线和这个角度之差
                                    ho__Bond1RegUnion.Dispose();
                                    HOperatorSet.Union1(ho__Bond1Regs, out ho__Bond1RegUnion);
                                    HOperatorSet.AreaCenter(ho__Bond1RegUnion, out hv_Area1, out hv_Row1,
                                        out hv_Column1);
                                    HOperatorSet.AreaCenter(ho__Bond2Reg, out hv_Area1, out hv_Row2,
                                        out hv_Column2);
                                    HOperatorSet.AngleLx(hv_Row2, hv_Column2, hv_Row1, hv_Column1, out hv_Angle);
                                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                                    {
                                        HTV_find_model(hvec_i_Images[hv__ImgIdx].O, ho__Bond2Reg, hv_i_Bond2ModelID,
                                            hv_i_Bond2ModelType, hv_Angle - (hv_i_Bond2AngleSingleExt * 2), hv_i_Bond2AngleSingleExt * 4.0,
                                            hv_i_Bond2MinScore, hv__Bond2BallNum, out hv__Row, out hv__Col,
                                            out hv__Angle, out hv__num, out hv_o_bondscore);
                                    }
                                    hvec_o_bondwire_Value[2] = new HTupleVector((hvec_o_bondwire_Value[2].T).TupleConcat(
                                        hv_o_bondscore));
                                    if ((int)(new HTuple(hv__num.TupleLess(hv__Bond2BallNum))) != 0)
                                    {
                                        {
                                            HObject ExpTmpOutVar_0;
                                            HOperatorSet.DilationRectangle1(ho__Bond2Reg, out ExpTmpOutVar_0,
                                                15, 15);
                                            ho__Bond2Reg.Dispose();
                                            ho__Bond2Reg = ExpTmpOutVar_0;
                                        }
                                        using (HDevDisposeHelper dh = new HDevDisposeHelper())
                                        {
                                            HTV_find_model(hvec_i_Images[hv__ImgIdx].O, ho__Bond2Reg, hv_i_Bond2ModelID,
                                                hv_i_Bond2ModelType, hv_Angle - (hv_i_Bond2AngleSingleExt * 2),
                                                hv_i_Bond2AngleSingleExt * 4.0, hv_i_Bond2MinScore, hv__Bond2BallNum,
                                                out hv__Row, out hv__Col, out hv__Angle, out hv__Dnum, out hv_o_score1);
                                        }
                                        if ((int)(new HTuple(hv__Dnum.TupleLess(hv__Bond2BallNum))) != 0)
                                        {
                                            {
                                                HObject ExpTmpOutVar_0;
                                                HOperatorSet.ConcatObj(ho_o_FailRegs, ho__Bond2Reg, out ExpTmpOutVar_0
                                                    );
                                                ho_o_FailRegs.Dispose();
                                                ho_o_FailRegs = ExpTmpOutVar_0;
                                            }
                                            hv_o_ErrCode = hv_o_ErrCode.TupleConcat(14);
                                            break;
                                        }
                                        else
                                        {
                                            {
                                                HObject ExpTmpOutVar_0;
                                                HOperatorSet.ConcatObj(ho_o_FailRegs, ho__Bond2Reg, out ExpTmpOutVar_0
                                                    );
                                                ho_o_FailRegs.Dispose();
                                                ho_o_FailRegs = ExpTmpOutVar_0;
                                            }
                                            hv_o_ErrCode = hv_o_ErrCode.TupleConcat(15);
                                        }
                                    }

                                    //将找到的球排序(默认按照逆时针顺序画图)
                                    HTV_calc_real_model_pos(hv_i_Bond2ModelID, hv_i_Bond2ModelType, hv__Row,
                                        hv__Col, hv__Angle, out hv__Ball2Row, out hv__Ball2Col);
                                    if ((int)(new HTuple((new HTuple(hv__Row.TupleLength())).TupleGreater(
                                        1))) != 0)
                                    {
                                        HOperatorSet.HomMat2dIdentity(out hv_HomMat2DIdentity);
                                        HOperatorSet.HomMat2dRotate(hv_HomMat2DIdentity, -hv_Angle, 0,
                                            0, out hv_HomMat2DRotate);
                                        HOperatorSet.AffineTransPoint2d(hv_HomMat2DRotate, hv__Ball2Row,
                                            hv__Ball2Col, out hv__NewRow, out hv__NewCol);
                                        HOperatorSet.TupleSortIndex(hv__NewRow, out hv_Indices);
                                        hv__Ball2Row = hv__Ball2Row.TupleSelect(hv_Indices);
                                        hv__Ball2Col = hv__Ball2Col.TupleSelect(hv_Indices);
                                    }
                                    HOperatorSet.TupleGenConst(hv__Bond2BallNum, hv__Ball1Rad + 9, out hv__Ball2Rad);
                                    ho__Ball2.Dispose();
                                    HOperatorSet.GenCircle(out ho__Ball2, hv__Ball2Row, hv__Ball2Col,
                                        hv__Ball2Rad);
                                    {
                                        HObject ExpTmpOutVar_0;
                                        HOperatorSet.ConcatObj(ho_o_Bond2Balls, ho__Ball2, out ExpTmpOutVar_0
                                            );
                                        ho_o_Bond2Balls.Dispose();
                                        ho_o_Bond2Balls = ExpTmpOutVar_0;
                                    }
                                }
                            }
                            //4. 金线检测
                            //HTV_Lead_Wire (i_Images, _WireImgIdx[idx-1], _Ball2Row[idx-1], _Ball2Col[idx-1], _Ball2Rad[idx-1], i_LineWidth, i_LineContrast, _Ball1Row, _Ball1Col, _LeadErr)
                            //if (_LeadErr=0)
                            hv__ImgIdx = hv__WireImgIdx.TupleSelect(hv_idx - 1);
                            ho__TrackReg.Dispose(); ho__Wires.Dispose();
                            HTV_track_wire(hvec_i_Images, out ho__TrackReg, out ho__Wires, hv__Ball1Row,
                                hv__Ball1Col, hv__Ball2Row.TupleSelect(hv_idx - 1), hv__Ball2Col.TupleSelect(
                                hv_idx - 1), hv_i_LineSearchLen, hv_i_LineClipLen, hv_i_LineWidth,
                                hv_i_LineContrast, hv_i_LineMinSegLen, hv_i_LineAngleExt, hv_i_LineMaxGap,
                                out hv__err, out hv__str, out hv_line_gap);

                            if ((int)(new HTuple(hv__err.TupleEqual(0))) != 0)
                            {
                                HOperatorSet.TupleMax(hv_line_gap, out hv_Max);
                                hvec_o_bondwire_Value[1] = new HTupleVector((hvec_o_bondwire_Value[1].T).TupleConcat(
                                    hv_Max));
                                {
                                    HObject ExpTmpOutVar_0;
                                    HOperatorSet.ConcatObj(ho_o_Wires, ho__Wires, out ExpTmpOutVar_0);
                                    ho_o_Wires.Dispose();
                                    ho_o_Wires = ExpTmpOutVar_0;
                                }
                            }
                            else
                            {
                                {
                                    HObject ExpTmpOutVar_0;
                                    HOperatorSet.ConcatObj(ho_o_FailRegs, ho__TrackReg, out ExpTmpOutVar_0
                                        );
                                    ho_o_FailRegs.Dispose();
                                    ho_o_FailRegs = ExpTmpOutVar_0;
                                }
                                hv_o_ErrCode = hv_o_ErrCode.TupleConcat(13);
                                hvec_o_bondwire_Value[1] = new HTupleVector((hvec_o_bondwire_Value[1].T).TupleConcat(
                                    -1));
                            }
                            //else
                            //distance_pp (_Ball1Row, _Ball1Col, _Ball2Row[idx-1], _Ball2Col[idx-1], Distance)
                            //line_orientation (_Ball1Row, _Ball1Col, _Ball2Row[idx-1], _Ball2Col[idx-1], Phi)
                            //gen_rectangle2 (_TrackReg, mean([_Ball1Row, _Ball2Row[idx-1]]), mean([_Ball1Col, _Ball2Col[idx-1]]), Phi, Distance/2.0-i_LineClipLen, i_LineSearchLen)
                            //concat_obj (o_FailRegs, _TrackReg, o_FailRegs)
                            //o_ErrCode := [o_ErrCode,13]
                            //endif
                        }
                    }
                }
                // catch (Exception) 
                catch (HalconException HDevExpDefaultException1)
                {
                    HDevExpDefaultException1.ToHTuple(out hv_Exception);
                    hv_o_ErrCode = -1;
                    hv_o_ErrStr = hv_Exception.Clone();
                }
                ho__Bond2Reg.Dispose();
                ho__Bond1Regs.Dispose();
                ho__Bond1Reg.Dispose();
                ho__Ball1.Dispose();
                ho__Ball2.Dispose();
                ho__Bond1RegUnion.Dispose();
                ho__TrackReg.Dispose();
                ho__Wires.Dispose();

                return;
            }
            catch (HalconException HDevExpDefaultException)
            {
                ho__Bond2Reg.Dispose();
                ho__Bond1Regs.Dispose();
                ho__Bond1Reg.Dispose();
                ho__Ball1.Dispose();
                ho__Ball2.Dispose();
                ho__Bond1RegUnion.Dispose();
                ho__TrackReg.Dispose();
                ho__Wires.Dispose();

                throw HDevExpDefaultException;
            }
        }

        public static void HTV_find_golden_model(HObject ho_Image, HObject ho__matchRegion, HTuple hv_i_ModelType,
    HTuple hv_i_modelID, HTuple hv_i_AngleStart, HTuple hv_i_AngleExt, HTuple hv_i_MinScore,
    out HTuple hv_o_Row, out HTuple hv_o_Col, out HTuple hv_o_NumMatched, out HTuple hv_o_Angle)
        {




            // Local iconic variables 

            HObject ho_ImageReduced, ho_ModelContours = null;

            // Local control variables 

            HTuple hv__row = new HTuple(), hv__col = new HTuple();
            HTuple hv__angle = new HTuple(), hv__score = new HTuple();
            HTuple hv_i = null;
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_ImageReduced);
            HOperatorSet.GenEmptyObj(out ho_ModelContours);
            hv_o_Angle = new HTuple();
            try
            {
                ho_ImageReduced.Dispose();
                HOperatorSet.ReduceDomain(ho_Image, ho__matchRegion, out ho_ImageReduced);
                if ((int)(new HTuple(hv_i_ModelType.TupleEqual("ncc"))) != 0)
                {
                    HOperatorSet.FindNccModel(ho_ImageReduced, hv_i_modelID, hv_i_AngleStart,
                        hv_i_AngleExt, 0.2, 0, 0.5, "true", 0, out hv__row, out hv__col, out hv__angle,
                        out hv__score);
                }
                else if ((int)(new HTuple(hv_i_ModelType.TupleEqual("shape"))) != 0)
                {
                    HOperatorSet.FindShapeModel(ho_ImageReduced, hv_i_modelID, hv_i_AngleStart,
                        hv_i_AngleExt, 0.2, 0, 0.5, "least_squares", 0, 0.5, out hv__row, out hv__col,
                        out hv__angle, out hv__score);
                    ho_ModelContours.Dispose();
                    HOperatorSet.GetShapeModelContours(out ho_ModelContours, hv_i_modelID, 1);
                }
                else
                {
                    throw new HalconException("Wrong argument [modelType]=" + hv_i_ModelType);
                }
                hv_o_Row = new HTuple();
                hv_o_Col = new HTuple();
                hv_o_NumMatched = 0;
                if ((int)(new HTuple((new HTuple(hv__score.TupleLength())).TupleEqual(0))) != 0)
                {
                    ho_ImageReduced.Dispose();
                    ho_ModelContours.Dispose();

                    return;
                }
                for (hv_i = 0; (int)hv_i <= (int)((new HTuple(hv__score.TupleLength())) - 1); hv_i = (int)hv_i + 1)
                {
                    if ((int)(new HTuple(((hv__score.TupleSelect(hv_i))).TupleGreaterEqual(hv_i_MinScore))) != 0)
                    {
                        if (hv_o_Row == null)
                            hv_o_Row = new HTuple();
                        hv_o_Row[hv_o_NumMatched] = hv__row.TupleSelect(hv_i);
                        if (hv_o_Col == null)
                            hv_o_Col = new HTuple();
                        hv_o_Col[hv_o_NumMatched] = hv__col.TupleSelect(hv_i);
                        if (hv_o_Angle == null)
                            hv_o_Angle = new HTuple();
                        hv_o_Angle[hv_o_NumMatched] = hv__angle.TupleSelect(hv_i);
                        hv_o_NumMatched = hv_o_NumMatched + 1;
                    }
                }
                ho_ImageReduced.Dispose();
                ho_ModelContours.Dispose();

                return;
                ho_ImageReduced.Dispose();
                ho_ModelContours.Dispose();

                return;
            }
            catch (HalconException HDevExpDefaultException)
            {
                ho_ImageReduced.Dispose();
                ho_ModelContours.Dispose();

                throw HDevExpDefaultException;
            }
        }

        public static void HTV_contours_to_region(HObject ho_i_Contours, out HObject ho_o_Region,
            HTuple hv_i_DilationSize)
        {




            // Stack for temporary objects 
            HObject[] OTemp = new HObject[20];

            // Local iconic variables 

            HObject ho_NeighborRegions, ho_ClippedContours;
            HObject ho_EmptyObject, ho_ObjectSelected = null, ho_Region = null;
            HObject ho_RegionUnion, ho_RegionDilation;

            // Local control variables 

            HTuple hv_Number = null, hv_I = null, hv_Rows = new HTuple();
            HTuple hv_Columns = new HTuple();
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_o_Region);
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
                HOperatorSet.ClipEndPointsContoursXld(ho_i_Contours, out ho_ClippedContours,
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
                HOperatorSet.DilationCircle(ho_RegionUnion, out ho_RegionDilation, hv_i_DilationSize);
                ho_o_Region.Dispose();
                HOperatorSet.Union1(ho_RegionDilation, out ho_o_Region);
                ho_NeighborRegions.Dispose();
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
                ho_NeighborRegions.Dispose();
                ho_ClippedContours.Dispose();
                ho_EmptyObject.Dispose();
                ho_ObjectSelected.Dispose();
                ho_Region.Dispose();
                ho_RegionUnion.Dispose();
                ho_RegionDilation.Dispose();

                throw HDevExpDefaultException;
            }
        }
        public static void HTV_read_bond_wire_model(out HObject ho_o_Bond1Regs, out HObject ho_o_Bond2Regs,
            HTuple hv_i_ModelDir, out HTuple hv_o_Bond1OnIc, out HTuple hv_o_Bond1ImgIdx,
            out HTuple hv_o_Bond2OnIc, out HTuple hv_o_Bond2ImgIdx, out HTuple hv_o_Bond2BallNums,
            out HTuple hv_o_Bond2ModelID, out HTuple hv_o_Bond2ModelType, out HTuple hv_o_WireImgIdx,
            out HTuple hv_o_ErrCode, out HTuple hv_o_ErrString)
        {



            // Local iconic variables 

            // Local control variables 

            HTuple hv_o_ModelID = new HTuple(), hv_o_ModelType = new HTuple();
            HTuple hv_Index = new HTuple(), hv_Exception = null;
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_o_Bond1Regs);
            HOperatorSet.GenEmptyObj(out ho_o_Bond2Regs);
            hv_o_Bond1OnIc = new HTuple();
            hv_o_Bond1ImgIdx = new HTuple();
            hv_o_Bond2OnIc = new HTuple();
            hv_o_Bond2ImgIdx = new HTuple();
            hv_o_Bond2BallNums = new HTuple();
            hv_o_Bond2ModelID = new HTuple();
            hv_o_Bond2ModelType = new HTuple();
            hv_o_WireImgIdx = new HTuple();
            hv_o_ErrCode = 0;
            hv_o_ErrString = "";
            try
            {
                hv_o_ModelID = -1;
                hv_o_ModelType = "";
                ho_o_Bond1Regs.Dispose();
                HOperatorSet.ReadRegion(out ho_o_Bond1Regs, hv_i_ModelDir + "Bond1_Regions.reg");
                ho_o_Bond2Regs.Dispose();
                HOperatorSet.ReadRegion(out ho_o_Bond2Regs, hv_i_ModelDir + "Bond2_Regions.reg");
                HOperatorSet.ReadTuple(hv_i_ModelDir + "Bond1_OnIC.tup", out hv_o_Bond1OnIc);
                HOperatorSet.ReadTuple(hv_i_ModelDir + "Bond1_Image_Index.tup", out hv_o_Bond1ImgIdx);
                HOperatorSet.ReadTuple(hv_i_ModelDir + "Bond2_OnIC.tup", out hv_o_Bond2OnIc);
                HOperatorSet.ReadTuple(hv_i_ModelDir + "Bond2_Image_Index.tup", out hv_o_Bond2ImgIdx);
                HOperatorSet.ReadTuple(hv_i_ModelDir + "Bond2_BallNums.tup", out hv_o_Bond2BallNums);
                HOperatorSet.ReadTuple(hv_i_ModelDir + "Wire_Image_Index.tup", out hv_o_WireImgIdx);
                HOperatorSet.TupleFindFirst(hv_o_Bond2OnIc, 0, out hv_Index);
                if ((int)(new HTuple(hv_Index.TupleGreaterEqual(0))) != 0)
                {
                    HTV_read_model(hv_i_ModelDir, out hv_o_Bond2ModelID, out hv_o_Bond2ModelType);
                }
            }
            // catch (Exception) 
            catch (HalconException HDevExpDefaultException1)
            {
                HDevExpDefaultException1.ToHTuple(out hv_Exception);
                hv_o_ErrCode = -1;
                hv_o_ErrString = hv_Exception.Clone();
            }

            return;
        }
        public static void HTV_write_bond_wire_model(HObject ho_i_Bond1Regs, HObject ho_i_Bond2Regs,
            HTuple hv_i_ModelDir, HTuple hv_i_Bond1OnIC, HTuple hv_i_Bond1ImgIdx, HTuple hv_i_Bond2OnIC,
            HTuple hv_i_Bond2ImgIdx, HTuple hv_i_Bond2BallNums, HTuple hv_i_Bond2ModelID,
            HTuple hv_i_Bond2ModelType, HTuple hv_i_WireImgIdx, out HTuple hv_o_ErrCode,
            out HTuple hv_o_ErrStr)
        {




            // Local iconic variables 

            // Local control variables 

            HTuple hv_Index1 = new HTuple(), hv_Exception = null;
            // Initialize local and output iconic variables 
            hv_o_ErrCode = 0;
            hv_o_ErrStr = "";
            try
            {
                HOperatorSet.WriteRegion(ho_i_Bond1Regs, hv_i_ModelDir + "Bond1_Regions.reg");
                HOperatorSet.WriteTuple(hv_i_Bond1OnIC, hv_i_ModelDir + "Bond1_OnIc.tup");
                HOperatorSet.WriteTuple(hv_i_Bond1ImgIdx, hv_i_ModelDir + "Bond1_Image_Index.tup");
                HOperatorSet.WriteRegion(ho_i_Bond2Regs, hv_i_ModelDir + "Bond2_Regions.reg");
                HOperatorSet.WriteTuple(hv_i_Bond2OnIC, hv_i_ModelDir + "Bond2_OnIc.tup");
                HOperatorSet.WriteTuple(hv_i_Bond2ImgIdx, hv_i_ModelDir + "Bond2_Image_Index.tup");
                HOperatorSet.WriteTuple(hv_i_Bond2BallNums, hv_i_ModelDir + "Bond2_BallNums.tup");
                HOperatorSet.WriteTuple(hv_i_WireImgIdx, hv_i_ModelDir + "Wire_Image_Index.tup");

                //*仅在Bond2中存在LeadPad上的球时才用模板匹配
                HOperatorSet.TupleFindFirst(hv_i_Bond2OnIC, 0, out hv_Index1);
                if ((int)(new HTuple(hv_Index1.TupleGreaterEqual(0))) != 0)
                {
                    HTV_write_model(hv_i_Bond2ModelID, hv_i_Bond2ModelType, hv_i_ModelDir);
                }
            }
            // catch (Exception) 
            catch (HalconException HDevExpDefaultException1)
            {
                HDevExpDefaultException1.ToHTuple(out hv_Exception);
                hv_o_ErrCode = -1;
                hv_o_ErrStr = hv_Exception.Clone();

                return;
            }

            return;
        }

        public static void HTV_Find_ic(HObject ho__ImageWire, HObject ho_ObjectSelected1, HTuple hv__ModelType,
    HTuple hv__ModelID, HTuple hv__AngleStart, HTuple hv__AngleExt, HTuple hv__MinSearchScore,
    out HTuple hv_o_Row, out HTuple hv_o_Col, out HTuple hv_o_NumMatched, out HTuple hv_o_Angle)
        {




            // Local iconic variables 

            HObject ho_ImageReduced, ho_ModelContours = null;

            // Local control variables 

            HTuple hv__row = new HTuple(), hv__col = new HTuple();
            HTuple hv__angle = new HTuple(), hv__score = new HTuple();
            HTuple hv_i = null;
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_ImageReduced);
            HOperatorSet.GenEmptyObj(out ho_ModelContours);
            hv_o_Angle = new HTuple();
            try
            {
                ho_ImageReduced.Dispose();
                HOperatorSet.ReduceDomain(ho__ImageWire, ho_ObjectSelected1, out ho_ImageReduced
                    );
                if ((int)(new HTuple(hv__ModelType.TupleEqual("ncc"))) != 0)
                {
                    HOperatorSet.FindNccModel(ho_ImageReduced, hv__ModelID, hv__AngleStart, hv__AngleExt,
                        0.2, 1, 0.5, "true", 0, out hv__row, out hv__col, out hv__angle, out hv__score);
                }
                else if ((int)(new HTuple(hv__ModelType.TupleEqual("shape"))) != 0)
                {
                    HOperatorSet.FindShapeModel(ho_ImageReduced, hv__ModelID, hv__AngleStart,
                        hv__AngleExt, 0.2, 1, 0.5, "least_squares", 0, 0.5, out hv__row, out hv__col,
                        out hv__angle, out hv__score);
                    ho_ModelContours.Dispose();
                    HOperatorSet.GetShapeModelContours(out ho_ModelContours, hv__ModelID, 1);
                }
                else
                {
                    throw new HalconException("Wrong argument [modelType]=" + hv__ModelType);
                }
                hv_o_Row = new HTuple();
                hv_o_Col = new HTuple();
                hv_o_NumMatched = 0;
                if ((int)(new HTuple((new HTuple(hv__score.TupleLength())).TupleEqual(0))) != 0)
                {
                    ho_ImageReduced.Dispose();
                    ho_ModelContours.Dispose();

                    return;
                }
                for (hv_i = 0; (int)hv_i <= (int)((new HTuple(hv__score.TupleLength())) - 1); hv_i = (int)hv_i + 1)
                {
                    if ((int)(new HTuple(((hv__score.TupleSelect(hv_i))).TupleGreaterEqual(hv__MinSearchScore))) != 0)
                    {
                        if (hv_o_Row == null)
                            hv_o_Row = new HTuple();
                        hv_o_Row[hv_o_NumMatched] = hv__row.TupleSelect(hv_i);
                        if (hv_o_Col == null)
                            hv_o_Col = new HTuple();
                        hv_o_Col[hv_o_NumMatched] = hv__col.TupleSelect(hv_i);
                        if (hv_o_Angle == null)
                            hv_o_Angle = new HTuple();
                        hv_o_Angle[hv_o_NumMatched] = hv__angle.TupleSelect(hv_i);
                        hv_o_NumMatched = hv_o_NumMatched + 1;
                    }
                }
                ho_ImageReduced.Dispose();
                ho_ModelContours.Dispose();

                return;
                ho_ImageReduced.Dispose();
                ho_ModelContours.Dispose();

                return;
                ho_ImageReduced.Dispose();
                ho_ModelContours.Dispose();

                return;
            }
            catch (HalconException HDevExpDefaultException)
            {
                ho_ImageReduced.Dispose();
                ho_ModelContours.Dispose();

                throw HDevExpDefaultException;
            }
        }
        public static void HTV_Find_Ic_Model(HObjectVector/*{eObjectVector,Dim=1}*/ hvec_i_Images,
    HObject ho_i_MatchReg, HTuple hv_i_FindIcImgIdx, HTuple hv_i_ModelType, HTuple hv_i_ModelID,
    HTuple hv_i_AngleStart, HTuple hv_i_AngleExt, HTuple hv_i_MinScore, out HTuple hv_o_NumMatched,
    out HTuple hv_o_Row, out HTuple hv_o_Col, out HTuple hv_o_Angle)
        {




            // Local iconic variables 

            HObject ho_ImageReduced, ho_ModelContours = null;

            // Local control variables 

            HTuple hv__row = new HTuple(), hv__col = new HTuple();
            HTuple hv__angle = new HTuple(), hv__score = new HTuple();
            HTuple hv_i = null;
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_ImageReduced);
            HOperatorSet.GenEmptyObj(out ho_ModelContours);
            hv_o_Angle = new HTuple();
            try
            {
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    ho_ImageReduced.Dispose();
                    HOperatorSet.ReduceDomain(hvec_i_Images[hv_i_FindIcImgIdx].O, ho_i_MatchReg,
                        out ho_ImageReduced);
                }
                if ((int)(new HTuple(hv_i_ModelType.TupleEqual("ncc"))) != 0)
                {
                    HOperatorSet.FindNccModel(ho_ImageReduced, hv_i_ModelID, hv_i_AngleStart,
                        hv_i_AngleExt, 0.2, 1, 0.5, "true", 0, out hv__row, out hv__col, out hv__angle,
                        out hv__score);
                }
                else if ((int)(new HTuple(hv_i_ModelType.TupleEqual("shape"))) != 0)
                {
                    HOperatorSet.FindShapeModel(ho_ImageReduced, hv_i_ModelID, hv_i_AngleStart,
                        hv_i_AngleExt, 0.2, 1, 0.5, "least_squares", 0, 0.5, out hv__row, out hv__col,
                        out hv__angle, out hv__score);
                    ho_ModelContours.Dispose();
                    HOperatorSet.GetShapeModelContours(out ho_ModelContours, hv_i_ModelID, 1);
                }
                else
                {
                    throw new HalconException("Wrong argument [modelType]=" + hv_i_ModelType);
                }
                hv_o_Row = new HTuple();
                hv_o_Col = new HTuple();
                hv_o_NumMatched = 0;
                if ((int)(new HTuple((new HTuple(hv__score.TupleLength())).TupleEqual(0))) != 0)
                {
                    ho_ImageReduced.Dispose();
                    ho_ModelContours.Dispose();

                    return;
                }
                for (hv_i = 0; (int)hv_i <= (int)((new HTuple(hv__score.TupleLength())) - 1); hv_i = (int)hv_i + 1)
                {
                    if ((int)(new HTuple(((hv__score.TupleSelect(hv_i))).TupleGreaterEqual(hv_i_MinScore))) != 0)
                    {
                        if (hv_o_Row == null)
                            hv_o_Row = new HTuple();
                        hv_o_Row[hv_o_NumMatched] = hv__row.TupleSelect(hv_i);
                        if (hv_o_Col == null)
                            hv_o_Col = new HTuple();
                        hv_o_Col[hv_o_NumMatched] = hv__col.TupleSelect(hv_i);
                        if (hv_o_Angle == null)
                            hv_o_Angle = new HTuple();
                        hv_o_Angle[hv_o_NumMatched] = hv__angle.TupleSelect(hv_i);
                        hv_o_NumMatched = hv_o_NumMatched + 1;
                    }
                }
                ho_ImageReduced.Dispose();
                ho_ModelContours.Dispose();

                return;
                ho_ImageReduced.Dispose();
                ho_ModelContours.Dispose();

                return;
            }
            catch (HalconException HDevExpDefaultException)
            {
                ho_ImageReduced.Dispose();
                ho_ModelContours.Dispose();

                throw HDevExpDefaultException;
            }
        }
        public static void HTV_Find_All(HObject ho_i_Image, HTuple hv_i_ModelType, HTuple hv_i_ModelID,
            HTuple hv_i_AngleStart, HTuple hv_i_AngleExt, HTuple hv_i_MinScore, out HTuple hv_o_Row,
            out HTuple hv_o_Col, out HTuple hv_o_NumMatched, out HTuple hv_o_Angle)
        {




            // Local iconic variables 

            // Local control variables 

            HTuple hv__row = new HTuple(), hv__col = new HTuple();
            HTuple hv__angle = new HTuple(), hv__score = new HTuple();
            HTuple hv_i = null;
            // Initialize local and output iconic variables 
            hv_o_Angle = new HTuple();
            if ((int)(new HTuple(hv_i_ModelType.TupleEqual("ncc"))) != 0)
            {
                HOperatorSet.FindNccModel(ho_i_Image, hv_i_ModelID, hv_i_AngleStart, hv_i_AngleExt,
                    0.2, 0, 0.5, "true", 0, out hv__row, out hv__col, out hv__angle, out hv__score);
            }
            else if ((int)(new HTuple(hv_i_ModelType.TupleEqual("shape"))) != 0)
            {
                HOperatorSet.FindShapeModel(ho_i_Image, hv_i_ModelID, hv_i_AngleStart, hv_i_AngleExt,
                    0.2, 0, 0.5, "least_squares", 0, 0.9, out hv__row, out hv__col, out hv__angle,
                    out hv__score);
            }
            else
            {
                throw new HalconException("Wrong argument [modelType]=" + hv_i_ModelType);
            }
            hv_o_Row = new HTuple();
            hv_o_Col = new HTuple();
            hv_o_NumMatched = 0;
            if ((int)(new HTuple((new HTuple(hv__score.TupleLength())).TupleEqual(0))) != 0)
            {

                return;
            }
            for (hv_i = 0; (int)hv_i <= (int)((new HTuple(hv__score.TupleLength())) - 1); hv_i = (int)hv_i + 1)
            {
                if ((int)(new HTuple(((hv__score.TupleSelect(hv_i))).TupleGreaterEqual(hv_i_MinScore))) != 0)
                {
                    if (hv_o_Row == null)
                        hv_o_Row = new HTuple();
                    hv_o_Row[hv_o_NumMatched] = hv__row.TupleSelect(hv_i);
                    if (hv_o_Col == null)
                        hv_o_Col = new HTuple();
                    hv_o_Col[hv_o_NumMatched] = hv__col.TupleSelect(hv_i);
                    if (hv_o_Angle == null)
                        hv_o_Angle = new HTuple();
                    hv_o_Angle[hv_o_NumMatched] = hv__angle.TupleSelect(hv_i);
                    hv_o_NumMatched = hv_o_NumMatched + 1;
                }
            }

            return;

            return;

            return;
        }
        public static void HTV_Sort_CoarseResult(HTuple hv_i_Row, HTuple hv_i_Col, HTuple hv_i_Angle,
    out HTuple hv_o_NewRow, out HTuple hv_o_NewCol, out HTuple hv_o_NewAngle, out HTupleVector/*{eTupleVector,Dim=1}*/ hvec_o_HomMatModel2Img)
        {



            // Local iconic variables 

            // Local control variables 

            HTuple hv_SortedRow = null, hv_SortedCol = null;
            HTuple hv_SortedAngle = null, hv_sortnum = null, hv_label = null;
            HTuple hv_Index = null, hv_sub = new HTuple(), hv_Index1 = null;
            HTuple hv_start = new HTuple(), hv_end = new HTuple();
            HTuple hv__tempCol = new HTuple(), hv__tempRow = new HTuple();
            HTuple hv__tempAngle = new HTuple(), hv_Index2 = null;
            HTuple hv_HomMat2D = new HTuple();
            // Initialize local and output iconic variables 
            HOperatorSet.TupleSort(hv_i_Row, out hv_SortedRow);
            sort_pairs(hv_i_Row, hv_i_Col, "1", out hv_SortedRow, out hv_SortedCol);
            sort_pairs(hv_i_Row, hv_i_Angle, "1", out hv_SortedRow, out hv_SortedAngle);
            hv_sortnum = new HTuple();
            hv_label = 0;
            hv_o_NewRow = new HTuple();
            hv_o_NewCol = new HTuple();
            hv_o_NewAngle = new HTuple();
            hvec_o_HomMatModel2Img = new HTupleVector(1);
            hv_sortnum = hv_sortnum.TupleConcat(hv_label);
            for (hv_Index = 0; (int)hv_Index <= (int)((new HTuple(hv_SortedRow.TupleLength())) - 1); hv_Index = (int)hv_Index + 1)
            {
                hv_sub = (((hv_SortedRow.TupleSelect(hv_label)) - (hv_SortedRow.TupleSelect(hv_Index)))).TupleAbs()
                    ;
                if ((int)(new HTuple(hv_sub.TupleLess(250))) != 0)
                {
                    continue;
                }
                else
                {
                    hv_label = hv_Index.Clone();
                    hv_sortnum = hv_sortnum.TupleConcat(hv_label);
                }
            }
            for (hv_Index1 = 0; (int)hv_Index1 <= (int)((new HTuple(hv_sortnum.TupleLength())) - 1); hv_Index1 = (int)hv_Index1 + 1)
            {
                if ((int)(new HTuple(hv_Index1.TupleEqual((new HTuple(hv_sortnum.TupleLength()
                    )) - 1))) != 0)
                {
                    HOperatorSet.TupleSelect(hv_sortnum, hv_Index1, out hv_start);
                    hv_end = (new HTuple(hv_SortedRow.TupleLength())) - 1;
                    sort_pairs(hv_SortedCol.TupleSelectRange(hv_start, hv_end), hv_SortedRow.TupleSelectRange(
                        hv_start, hv_end), "1", out hv__tempCol, out hv__tempRow);
                    sort_pairs(hv_SortedCol.TupleSelectRange(hv_start, hv_end), hv_SortedAngle.TupleSelectRange(
                        hv_start, hv_end), "1", out hv__tempCol, out hv__tempAngle);
                }
                else
                {
                    HOperatorSet.TupleSelect(hv_sortnum, hv_Index1, out hv_start);
                    HOperatorSet.TupleSelect(hv_sortnum, hv_Index1 + 1, out hv_end);
                    sort_pairs(hv_SortedCol.TupleSelectRange(hv_start, hv_end - 1), hv_SortedRow.TupleSelectRange(
                        hv_start, hv_end - 1), "1", out hv__tempCol, out hv__tempRow);
                    sort_pairs(hv_SortedCol.TupleSelectRange(hv_start, hv_end - 1), hv_SortedAngle.TupleSelectRange(
                        hv_start, hv_end - 1), "1", out hv__tempCol, out hv__tempAngle);
                }
                hv_o_NewRow = hv_o_NewRow.TupleConcat(hv__tempRow);
                hv_o_NewCol = hv_o_NewCol.TupleConcat(hv__tempCol);
                hv_o_NewAngle = hv_o_NewAngle.TupleConcat(hv__tempAngle);
            }
            for (hv_Index2 = 0; (int)hv_Index2 <= (int)((new HTuple(hv_o_NewAngle.TupleLength()
                )) - 1); hv_Index2 = (int)hv_Index2 + 1)
            {
                HOperatorSet.VectorAngleToRigid(0, 0, 0, hv_o_NewRow.TupleSelect(hv_Index2),
                    hv_o_NewCol.TupleSelect(hv_Index2), hv_o_NewAngle.TupleSelect(hv_Index2),
                    out hv_HomMat2D);
                hvec_o_HomMatModel2Img[hv_Index2] = new HTupleVector(hv_HomMat2D).Clone();
            }

            return;
        }
        public static void HTV_FindCoarse_Correctpos(HTuple hv_i_Row, HTuple hv_i_Col, HTuple hv_i_Angle,
    out HTuple hv_o_NewRow, out HTuple hv_o_NewCol, out HTuple hv_o_NewAngle, out HTuple hv_o_CorrectPosition,
    out HTupleVector/*{eTupleVector,Dim=1}*/ hvec_o_HomMatModel2Img)
        {



            // Local iconic variables 

            // Local control variables 

            HTuple hv_RowMean = null, hv_RowSortNum = null;
            HTuple hv_RowSum = null, hv_RowLabel = null, hv_RowSorted = null;
            HTuple hv_Index3 = null, hv_RowSub = new HTuple(), hv_Temp = new HTuple();
            HTuple hv_test = new HTuple(), hv_ColMean = null, hv_ColSortNum = null;
            HTuple hv_ColSum = null, hv_ColLabel = null, hv_ColSorted = null;
            HTuple hv_Index4 = null, hv_ColSub = new HTuple(), hv_i = null;
            HTuple hv_j = new HTuple(), hv_Index6 = null, hv_Index7 = new HTuple();
            HTuple hv_RowDis = new HTuple(), hv_ColDis = new HTuple();
            HTuple hv_Index8 = null, hv_HomMat2D1 = new HTuple();
            // Initialize local and output iconic variables 
            hv_RowMean = new HTuple();
            hv_RowSortNum = 0;
            hv_RowSum = 0;
            hv_RowLabel = 0;
            HOperatorSet.TupleSort(hv_i_Row, out hv_RowSorted);
            for (hv_Index3 = 0; (int)hv_Index3 <= (int)((new HTuple(hv_RowSorted.TupleLength()
                )) - 1); hv_Index3 = (int)hv_Index3 + 1)
            {
                hv_RowSub = (((hv_RowSorted.TupleSelect(hv_Index3)) - (hv_RowSorted.TupleSelect(
                    hv_RowLabel)))).TupleAbs();
                if ((int)((new HTuple(hv_Index3.TupleEqual((new HTuple(hv_RowSorted.TupleLength()
                    )) - 1))).TupleAnd(new HTuple(hv_RowSub.TupleLess(200)))) != 0)
                {
                    hv_Temp = ((hv_Index3 - hv_RowLabel)).TupleAbs();
                    if ((int)(new HTuple(hv_Temp.TupleEqual(0))) != 0)
                    {
                        hv_Temp = 1;
                        hv_RowSum = hv_RowSum + (hv_RowSorted.TupleSelect(hv_Index3));
                    }
                    hv_RowLabel = hv_Index3.Clone();
                    hv_RowMean = hv_RowMean.TupleConcat(hv_RowSum / hv_Temp);
                    hv_RowSum = 0;
                }
                else if ((int)(new HTuple(hv_RowSub.TupleLess(200))) != 0)
                {
                    hv_test = hv_RowSorted.TupleSelect(hv_Index3);
                    hv_RowSum = hv_RowSum + (hv_RowSorted.TupleSelect(hv_Index3));
                }
                else
                {
                    hv_Temp = ((hv_Index3 - hv_RowLabel)).TupleAbs();
                    hv_RowLabel = hv_Index3.Clone();
                    hv_RowSortNum = hv_RowSortNum.TupleConcat(hv_RowLabel);
                    hv_RowMean = hv_RowMean.TupleConcat(hv_RowSum / hv_Temp);
                    hv_RowSum = 0;
                    hv_Index3 = hv_Index3 - 1;
                }
            }
            hv_ColMean = new HTuple();
            hv_ColSortNum = 0;
            hv_ColSum = 0;
            hv_ColLabel = 0;
            HOperatorSet.TupleSort(hv_i_Col, out hv_ColSorted);
            for (hv_Index4 = 0; (int)hv_Index4 <= (int)((new HTuple(hv_ColSorted.TupleLength()
                )) - 1); hv_Index4 = (int)hv_Index4 + 1)
            {
                hv_ColSub = (((hv_ColSorted.TupleSelect(hv_Index4)) - (hv_ColSorted.TupleSelect(
                    hv_ColLabel)))).TupleAbs();
                if ((int)((new HTuple(hv_Index4.TupleEqual((new HTuple(hv_ColSorted.TupleLength()
                    )) - 1))).TupleAnd(new HTuple(hv_ColSub.TupleLess(200)))) != 0)
                {
                    hv_Temp = ((hv_Index4 - hv_ColLabel)).TupleAbs();
                    if ((int)(new HTuple(hv_Temp.TupleEqual(0))) != 0)
                    {
                        hv_Temp = 1;
                        hv_ColSum = hv_ColSum + (hv_ColSorted.TupleSelect(hv_Index4));
                    }
                    hv_ColLabel = hv_Index4.Clone();
                    hv_ColMean = hv_ColMean.TupleConcat(hv_ColSum / hv_Temp);
                    hv_ColSum = 0;
                }
                else if ((int)(new HTuple(hv_ColSub.TupleLess(200))) != 0)
                {
                    hv_ColSum = hv_ColSum + (hv_ColSorted.TupleSelect(hv_Index4));
                }
                else
                {
                    hv_Temp = ((hv_Index4 - hv_ColLabel)).TupleAbs();
                    hv_ColLabel = hv_Index4.Clone();
                    hv_ColSortNum = hv_ColSortNum.TupleConcat(hv_ColLabel);
                    hv_ColMean = hv_ColMean.TupleConcat(hv_ColSum / hv_Temp);
                    hv_ColSum = 0;
                    hv_Index4 = hv_Index4 - 1;
                }
            }
            hv_o_NewRow = new HTuple();
            hv_o_NewCol = new HTuple();
            hv_o_NewAngle = new HTuple();
            for (hv_i = 0; (int)hv_i <= (int)((new HTuple(hv_RowMean.TupleLength())) - 1); hv_i = (int)hv_i + 1)
            {
                for (hv_j = 0; (int)hv_j <= (int)((new HTuple(hv_ColMean.TupleLength())) - 1); hv_j = (int)hv_j + 1)
                {
                    hv_o_NewRow = hv_o_NewRow.TupleConcat(hv_RowMean.TupleSelect(hv_i));
                    hv_o_NewCol = hv_o_NewCol.TupleConcat(hv_ColMean.TupleSelect(hv_j));
                    hv_o_NewAngle = hv_o_NewAngle.TupleConcat(0);
                }
            }
            hv_o_CorrectPosition = new HTuple();
            HOperatorSet.TupleGenConst(new HTuple(hv_o_NewRow.TupleLength()), 0, out hv_o_CorrectPosition);
            for (hv_Index6 = 0; (int)hv_Index6 <= (int)((new HTuple(hv_o_NewRow.TupleLength()
                )) - 1); hv_Index6 = (int)hv_Index6 + 1)
            {
                for (hv_Index7 = 0; (int)hv_Index7 <= (int)((new HTuple(hv_i_Row.TupleLength())) - 1); hv_Index7 = (int)hv_Index7 + 1)
                {
                    hv_RowDis = (((hv_o_NewRow.TupleSelect(hv_Index6)) - (hv_i_Row.TupleSelect(
                        hv_Index7)))).TupleAbs();
                    hv_ColDis = (((hv_o_NewCol.TupleSelect(hv_Index6)) - (hv_i_Col.TupleSelect(
                        hv_Index7)))).TupleAbs();
                    if ((int)((new HTuple(hv_RowDis.TupleLess(100))).TupleAnd(new HTuple(hv_ColDis.TupleLess(
                        100)))) != 0)
                    {
                        if (hv_o_NewRow == null)
                            hv_o_NewRow = new HTuple();
                        hv_o_NewRow[hv_Index6] = hv_i_Row.TupleSelect(hv_Index7);
                        if (hv_o_NewCol == null)
                            hv_o_NewCol = new HTuple();
                        hv_o_NewCol[hv_Index6] = hv_i_Col.TupleSelect(hv_Index7);
                        if (hv_o_NewAngle == null)
                            hv_o_NewAngle = new HTuple();
                        hv_o_NewAngle[hv_Index6] = hv_i_Angle.TupleSelect(hv_Index7);
                        if (hv_o_CorrectPosition == null)
                            hv_o_CorrectPosition = new HTuple();
                        hv_o_CorrectPosition[hv_Index6] = 1;
                        break;
                    }
                }
            }
            hvec_o_HomMatModel2Img = new HTupleVector(1);
            for (hv_Index8 = 0; (int)hv_Index8 <= (int)((new HTuple(hv_o_NewRow.TupleLength()
                )) - 1); hv_Index8 = (int)hv_Index8 + 1)
            {
                HOperatorSet.VectorAngleToRigid(0, 0, 0, hv_o_NewRow.TupleSelect(hv_Index8),
                    hv_o_NewCol.TupleSelect(hv_Index8), hv_o_NewAngle.TupleSelect(hv_Index8),
                    out hv_HomMat2D1);
                hvec_o_HomMatModel2Img[hv_Index8] = new HTupleVector(hv_HomMat2D1).Clone();
            }

            return;
        }
        public static void sort_pairs(HTuple hv_T1, HTuple hv_T2, HTuple hv_SortMode, out HTuple hv_Sorted1,
    out HTuple hv_Sorted2)
        {



            // Local iconic variables 

            // Local control variables 

            HTuple hv_Indices1 = new HTuple(), hv_Indices2 = new HTuple();
            // Initialize local and output iconic variables 
            hv_Sorted1 = new HTuple();
            hv_Sorted2 = new HTuple();
            //Sort tuple pairs.
            //
            //input parameters:
            //T1: first tuple
            //T2: second tuple
            //SortMode: if set to '1', sort by the first tuple,
            //   if set to '2', sort by the second tuple
            //
            if ((int)((new HTuple(hv_SortMode.TupleEqual("1"))).TupleOr(new HTuple(hv_SortMode.TupleEqual(
                1)))) != 0)
            {
                HOperatorSet.TupleSortIndex(hv_T1, out hv_Indices1);
                hv_Sorted1 = hv_T1.TupleSelect(hv_Indices1);
                hv_Sorted2 = hv_T2.TupleSelect(hv_Indices1);
            }
            else if ((int)((new HTuple((new HTuple(hv_SortMode.TupleEqual("column"))).TupleOr(
                new HTuple(hv_SortMode.TupleEqual("2"))))).TupleOr(new HTuple(hv_SortMode.TupleEqual(
                2)))) != 0)
            {
                HOperatorSet.TupleSortIndex(hv_T2, out hv_Indices2);
                hv_Sorted1 = hv_T1.TupleSelect(hv_Indices2);
                hv_Sorted2 = hv_T2.TupleSelect(hv_Indices2);
            }

            return;
        }
        public static void HTV_Lead_Wire(HObjectVector/*{eObjectVector,Dim=1}*/ hvec_i_Images,
            HTuple hv_i_WireImgIdx, HTuple hv_i_Ball2Row, HTuple hv_i_Ball2Col, HTuple hv_i_Ball2Rad,
            HTuple hv_i_LineWidth, HTuple hv_i_LineContrast, HTuple hv_i_Ball1Row, HTuple hv_i_Ball1Col,
            out HTuple hv_o_Err)
        {




            // Stack for temporary objects 
            HObject[] OTemp = new HObject[20];

            // Local iconic variables 

            HObject ho_Rectangle, ho_Circle, ho_RegionDifference;
            HObject ho_ImageReduced, ho_Lines, ho_ContoursSplit, ho_SelectedContours;
            HObject ho_Outleadwire, ho_outObjectSelected = null, ho_AdjacentContours = null;
            HObject ho_ObjectSelected = null, ho_SelectedContours_;

            // Local control variables 

            HTuple hv__ImgIdx = null, hv_final_selected_length = null;
            HTuple hv_Sigma = null, hv_Low = null, hv_High = null;
            HTuple hv_Phi = null, hv_Number1 = null, hv_Index1 = new HTuple();
            HTuple hv_Length = new HTuple(), hv_Number = null, hv_Index = new HTuple();
            HTuple hv_DistanceMin = new HTuple(), hv_DistanceMax = new HTuple();
            HTuple hv_distance_min = null, hv_distance_max = null;
            HTuple hv_RowBegin = new HTuple(), hv_ColBegin = new HTuple();
            HTuple hv_RowEnd = new HTuple(), hv_ColEnd = new HTuple();
            HTuple hv_Nr = new HTuple(), hv_Nc = new HTuple(), hv_Dist = new HTuple();
            HTuple hv_Distance1 = new HTuple(), hv_Distance2 = new HTuple();
            HTuple hv_Angle1 = new HTuple(), hv_Angle2 = new HTuple();
            HTuple hv_DistanceMinLine = new HTuple(), hv_DistanceMax1 = new HTuple();
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_Rectangle);
            HOperatorSet.GenEmptyObj(out ho_Circle);
            HOperatorSet.GenEmptyObj(out ho_RegionDifference);
            HOperatorSet.GenEmptyObj(out ho_ImageReduced);
            HOperatorSet.GenEmptyObj(out ho_Lines);
            HOperatorSet.GenEmptyObj(out ho_ContoursSplit);
            HOperatorSet.GenEmptyObj(out ho_SelectedContours);
            HOperatorSet.GenEmptyObj(out ho_Outleadwire);
            HOperatorSet.GenEmptyObj(out ho_outObjectSelected);
            HOperatorSet.GenEmptyObj(out ho_AdjacentContours);
            HOperatorSet.GenEmptyObj(out ho_ObjectSelected);
            HOperatorSet.GenEmptyObj(out ho_SelectedContours_);
            try
            {
                hv__ImgIdx = hv_i_WireImgIdx.Clone();
                hv_o_Err = 0;
                hv_final_selected_length = new HTuple();
                //dilation_rectangle1 (ic_pad, RegionDilation, dilation_size, dilation_size)
                ho_Rectangle.Dispose();
                HOperatorSet.GenRectangle2(out ho_Rectangle, hv_i_Ball2Row, hv_i_Ball2Col,
                    0, 30, 30);
                ho_Circle.Dispose();
                HOperatorSet.GenCircle(out ho_Circle, hv_i_Ball2Row, hv_i_Ball2Col, hv_i_Ball2Rad);
                ho_RegionDifference.Dispose();
                HOperatorSet.Difference(ho_Rectangle, ho_Circle, out ho_RegionDifference);
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    ho_ImageReduced.Dispose();
                    HOperatorSet.ReduceDomain(hvec_i_Images[hv__ImgIdx].O, ho_RegionDifference,
                        out ho_ImageReduced);
                }
                calculate_lines_gauss_parameters(hv_i_LineWidth + 5, hv_i_LineContrast, out hv_Sigma,
                    out hv_Low, out hv_High);
                ho_Lines.Dispose();
                HOperatorSet.LinesGauss(ho_ImageReduced, out ho_Lines, hv_Sigma, hv_Low, hv_High,
                    "light", "true", "bar-shaped", "true");
                //**************
                ho_ContoursSplit.Dispose();
                HOperatorSet.SegmentContoursXld(ho_Lines, out ho_ContoursSplit, "lines", 7,
                    1, 0.5);
                HOperatorSet.LineOrientation(hv_i_Ball1Row, hv_i_Ball1Col, hv_i_Ball2Row, hv_i_Ball2Col,
                    out hv_Phi);

                ho_SelectedContours.Dispose();
                HOperatorSet.SelectContoursXld(ho_ContoursSplit, out ho_SelectedContours, "contour_length",
                    4, 999, -0.5, 0.5);

                ho_Outleadwire.Dispose();
                HOperatorSet.SelectContoursXld(ho_SelectedContours, out ho_Outleadwire, "direction",
                    hv_Phi - ((new HTuple(30)).TupleRad()), hv_Phi + ((new HTuple(30)).TupleRad()
                    ), -0.5, 0.5);
                HOperatorSet.CountObj(ho_Outleadwire, out hv_Number1);
                if ((int)(new HTuple(hv_Number1.TupleGreater(0))) != 0)
                {
                    HTuple end_val19 = hv_Number1;
                    HTuple step_val19 = 1;
                    for (hv_Index1 = 1; hv_Index1.Continue(end_val19, step_val19); hv_Index1 = hv_Index1.TupleAdd(step_val19))
                    {
                        ho_outObjectSelected.Dispose();
                        HOperatorSet.SelectObj(ho_Outleadwire, out ho_outObjectSelected, hv_Index1);
                        HOperatorSet.LengthXld(ho_outObjectSelected, out hv_Length);
                        hv_final_selected_length = hv_final_selected_length.TupleConcat(hv_Length);
                    }
                }
                else
                {
                    hv_final_selected_length = 0;
                }

                HOperatorSet.CountObj(ho_SelectedContours, out hv_Number);
                if ((int)(new HTuple(hv_Number.TupleGreater(1))) != 0)
                {
                    ho_AdjacentContours.Dispose();
                    HOperatorSet.GenEmptyObj(out ho_AdjacentContours);
                    HTuple end_val31 = hv_Number;
                    HTuple step_val31 = 1;
                    for (hv_Index = 1; hv_Index.Continue(end_val31, step_val31); hv_Index = hv_Index.TupleAdd(step_val31))
                    {
                        ho_ObjectSelected.Dispose();
                        HOperatorSet.SelectObj(ho_SelectedContours, out ho_ObjectSelected, hv_Index);
                        HOperatorSet.DistancePc(ho_ObjectSelected, hv_i_Ball2Row, hv_i_Ball2Col,
                            out hv_DistanceMin, out hv_DistanceMax);
                        HOperatorSet.LengthXld(ho_ObjectSelected, out hv_Length);
                        if ((int)((new HTuple(hv_DistanceMin.TupleLess(hv_i_Ball2Rad - 1))).TupleAnd(
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
                        hv_o_Err = -1;
                        ho_Rectangle.Dispose();
                        ho_Circle.Dispose();
                        ho_RegionDifference.Dispose();
                        ho_ImageReduced.Dispose();
                        ho_Lines.Dispose();
                        ho_ContoursSplit.Dispose();
                        ho_SelectedContours.Dispose();
                        ho_Outleadwire.Dispose();
                        ho_outObjectSelected.Dispose();
                        ho_AdjacentContours.Dispose();
                        ho_ObjectSelected.Dispose();
                        ho_SelectedContours_.Dispose();

                        return;
                    }
                }
                else if ((int)(new HTuple(hv_Number.TupleEqual(1))) != 0)
                {
                    HOperatorSet.LengthXld(ho_SelectedContours, out hv_Length);
                }


                ho_SelectedContours_.Dispose();
                HOperatorSet.SelectContoursXld(ho_SelectedContours, out ho_SelectedContours_,
                    "direction", hv_Phi - ((new HTuple(35)).TupleRad()), hv_Phi + ((new HTuple(35)).TupleRad()
                    ), -0.5, 0.5);
                HOperatorSet.CountObj(ho_SelectedContours_, out hv_Number);
                if ((int)(new HTuple(hv_Number.TupleEqual(0))) != 0)
                {
                    hv_o_Err = -1;
                    ho_Rectangle.Dispose();
                    ho_Circle.Dispose();
                    ho_RegionDifference.Dispose();
                    ho_ImageReduced.Dispose();
                    ho_Lines.Dispose();
                    ho_ContoursSplit.Dispose();
                    ho_SelectedContours.Dispose();
                    ho_Outleadwire.Dispose();
                    ho_outObjectSelected.Dispose();
                    ho_AdjacentContours.Dispose();
                    ho_ObjectSelected.Dispose();
                    ho_SelectedContours_.Dispose();

                    return;
                }
                hv_distance_min = new HTuple();
                hv_distance_max = new HTuple();

                HTuple end_val58 = hv_Number;
                HTuple step_val58 = 1;
                for (hv_Index = 1; hv_Index.Continue(end_val58, step_val58); hv_Index = hv_Index.TupleAdd(step_val58))
                {
                    ho_ObjectSelected.Dispose();
                    HOperatorSet.SelectObj(ho_SelectedContours_, out ho_ObjectSelected, hv_Index);
                    //
                    HOperatorSet.FitLineContourXld(ho_ObjectSelected, "tukey", -1, 0, 5, 2, out hv_RowBegin,
                        out hv_ColBegin, out hv_RowEnd, out hv_ColEnd, out hv_Nr, out hv_Nc,
                        out hv_Dist);
                    HOperatorSet.DistancePp(hv_RowBegin, hv_ColBegin, hv_i_Ball2Row, hv_i_Ball2Col,
                        out hv_Distance1);
                    HOperatorSet.DistancePp(hv_RowEnd, hv_ColEnd, hv_i_Ball2Row, hv_i_Ball2Col,
                        out hv_Distance2);
                    hv_DistanceMin = ((hv_Distance1.TupleConcat(hv_Distance2))).TupleMin();
                    HOperatorSet.AngleLl(hv_i_Ball2Row, hv_i_Ball2Col, hv_i_Ball1Row, hv_i_Ball1Col,
                        hv_i_Ball2Row, hv_i_Ball2Col, hv_RowBegin, hv_ColBegin, out hv_Angle1);
                    HOperatorSet.AngleLl(hv_i_Ball2Row, hv_i_Ball2Col, hv_i_Ball1Row, hv_i_Ball1Col,
                        hv_i_Ball2Row, hv_i_Ball2Col, hv_RowEnd, hv_ColEnd, out hv_Angle2);
                    if ((int)(new HTuple(((((((hv_Angle1.TupleAbs())).TupleConcat(hv_Angle2.TupleAbs()
                        ))).TupleMax())).TupleGreater(1.5709))) != 0)
                    {
                        hv_DistanceMin = 999;
                        continue;
                    }
                    HOperatorSet.DistanceLc(ho_ObjectSelected, hv_i_Ball2Row, hv_i_Ball2Col,
                        hv_i_Ball1Row, hv_i_Ball1Col, out hv_DistanceMinLine, out hv_DistanceMax1);
                    if ((int)(new HTuple(hv_DistanceMinLine.TupleGreater(7))) != 0)
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
                    hv_o_Err = -1;
                    ho_Rectangle.Dispose();
                    ho_Circle.Dispose();
                    ho_RegionDifference.Dispose();
                    ho_ImageReduced.Dispose();
                    ho_Lines.Dispose();
                    ho_ContoursSplit.Dispose();
                    ho_SelectedContours.Dispose();
                    ho_Outleadwire.Dispose();
                    ho_outObjectSelected.Dispose();
                    ho_AdjacentContours.Dispose();
                    ho_ObjectSelected.Dispose();
                    ho_SelectedContours_.Dispose();

                    return;
                }
                if ((int)(new HTuple(((hv_distance_min.TupleMin())).TupleGreater(5 + hv_i_Ball2Rad))) != 0)
                {
                    hv_o_Err = -1;
                }
                ho_Rectangle.Dispose();
                ho_Circle.Dispose();
                ho_RegionDifference.Dispose();
                ho_ImageReduced.Dispose();
                ho_Lines.Dispose();
                ho_ContoursSplit.Dispose();
                ho_SelectedContours.Dispose();
                ho_Outleadwire.Dispose();
                ho_outObjectSelected.Dispose();
                ho_AdjacentContours.Dispose();
                ho_ObjectSelected.Dispose();
                ho_SelectedContours_.Dispose();

                return;
                ho_Rectangle.Dispose();
                ho_Circle.Dispose();
                ho_RegionDifference.Dispose();
                ho_ImageReduced.Dispose();
                ho_Lines.Dispose();
                ho_ContoursSplit.Dispose();
                ho_SelectedContours.Dispose();
                ho_Outleadwire.Dispose();
                ho_outObjectSelected.Dispose();
                ho_AdjacentContours.Dispose();
                ho_ObjectSelected.Dispose();
                ho_SelectedContours_.Dispose();

                return;
                ho_Rectangle.Dispose();
                ho_Circle.Dispose();
                ho_RegionDifference.Dispose();
                ho_ImageReduced.Dispose();
                ho_Lines.Dispose();
                ho_ContoursSplit.Dispose();
                ho_SelectedContours.Dispose();
                ho_Outleadwire.Dispose();
                ho_outObjectSelected.Dispose();
                ho_AdjacentContours.Dispose();
                ho_ObjectSelected.Dispose();
                ho_SelectedContours_.Dispose();

                return;
                ho_Rectangle.Dispose();
                ho_Circle.Dispose();
                ho_RegionDifference.Dispose();
                ho_ImageReduced.Dispose();
                ho_Lines.Dispose();
                ho_ContoursSplit.Dispose();
                ho_SelectedContours.Dispose();
                ho_Outleadwire.Dispose();
                ho_outObjectSelected.Dispose();
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
                ho_Outleadwire.Dispose();
                ho_outObjectSelected.Dispose();
                ho_AdjacentContours.Dispose();
                ho_ObjectSelected.Dispose();
                ho_SelectedContours_.Dispose();

                throw HDevExpDefaultException;
            }
        }

        public static void HTV_calculate_output(HObject ho_i_FailRegs, HTuple hv_i_Number, out HTupleVector/*{eTupleVector,Dim=1}*/ hvec_o_Value)
        {




            // Local iconic variables 

            HObject ho__Selected = null;

            // Local control variables 

            HTuple hv_len1 = null, hv_len2 = null, hv_Area = null;
            HTuple hv_Index = null, hv__MainIcArea = new HTuple();
            HTuple hv_Row = new HTuple(), hv_Column = new HTuple();
            HTuple hv_Row1 = new HTuple(), hv_Column1 = new HTuple();
            HTuple hv_Row2 = new HTuple(), hv_Column2 = new HTuple();
            HTuple hv_len1Max = null, hv_len2Max = null, hv_AreaMax = null;
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho__Selected);
            hvec_o_Value = new HTupleVector(1);
            try
            {

                hv_len1 = new HTuple();
                hv_len2 = new HTuple();
                hv_Area = new HTuple();

                HTuple end_val5 = hv_i_Number;
                HTuple step_val5 = 1;
                for (hv_Index = 1; hv_Index.Continue(end_val5, step_val5); hv_Index = hv_Index.TupleAdd(step_val5))
                {
                    ho__Selected.Dispose();
                    HOperatorSet.SelectObj(ho_i_FailRegs, out ho__Selected, hv_Index);
                    HOperatorSet.AreaCenter(ho__Selected, out hv__MainIcArea, out hv_Row, out hv_Column);
                    HOperatorSet.SmallestRectangle1(ho__Selected, out hv_Row1, out hv_Column1,
                        out hv_Row2, out hv_Column2);
                    hv_len1 = hv_len1.TupleConcat(hv_Column2 - hv_Column1);
                    hv_len2 = hv_len2.TupleConcat(hv_Row2 - hv_Row1);
                    hv_Area = hv_Area.TupleConcat(hv__MainIcArea);
                }
                HOperatorSet.TupleMax(hv_len1, out hv_len1Max);
                HOperatorSet.TupleMax(hv_len2, out hv_len2Max);
                HOperatorSet.TupleMax(hv_Area, out hv_AreaMax);
                hvec_o_Value[0] = new HTupleVector(hv_i_Number).Clone();
                hvec_o_Value[1] = new HTupleVector(hv_len1).Clone();
                hvec_o_Value[2] = new HTupleVector(hv_len2).Clone();
                hvec_o_Value[3] = new HTupleVector(hv_Area).Clone();
                hvec_o_Value[4] = new HTupleVector(hv_len1Max).Clone();
                hvec_o_Value[5] = new HTupleVector(hv_len2Max).Clone();
                hvec_o_Value[6] = new HTupleVector(hv_AreaMax).Clone();

                ho__Selected.Dispose();

                return;
            }
            catch (HalconException HDevExpDefaultException)
            {
                ho__Selected.Dispose();

                throw HDevExpDefaultException;
            }
        }

        public static void HTV_inspect_nodie(HObjectVector/*{eObjectVector,Dim=1}*/ hvec_i_Images,
     HObject ho_i_InspectReg, HTuple hv_i_ImgIdx, out HTuple hv_o_ErrCode)
        {




            // Local iconic variables 

            HObject ho_Image = null;

            // Local control variables 

            HTuple hv_o_ErrString = null, hv_Row = new HTuple();
            HTuple hv_Column = new HTuple(), hv_Phi = new HTuple();
            HTuple hv_Length1 = new HTuple(), hv_Length2 = new HTuple();
            HTuple hv_Width = new HTuple(), hv_Height = new HTuple();
            HTuple hv_MetrologyHandle = new HTuple(), hv_Index = new HTuple();
            HTuple hv_o_Parameter = new HTuple(), hv_Exception = null;
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_Image);
            try
            {
                hv_o_ErrCode = 0;
                hv_o_ErrString = "";
                try
                {
                    ho_Image.Dispose();
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        ho_Image = hvec_i_Images[hv_i_ImgIdx].O.CopyObj(1, -1);
                    }
                    HOperatorSet.SmallestRectangle2(ho_i_InspectReg, out hv_Row, out hv_Column,
                        out hv_Phi, out hv_Length1, out hv_Length2);
                    HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height);
                    HOperatorSet.CreateMetrologyModel(out hv_MetrologyHandle);
                    HOperatorSet.SetMetrologyModelImageSize(hv_MetrologyHandle, hv_Width, hv_Height);
                    HOperatorSet.AddMetrologyObjectRectangle2Measure(hv_MetrologyHandle, hv_Row,
                        hv_Column, hv_Phi, hv_Length1, hv_Length2, 50, 5, 1, 50, new HTuple(),
                        new HTuple(), out hv_Index);
                    HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandle, "all", "measure_transition",
                        "negative");
                    HOperatorSet.ApplyMetrologyModel(ho_Image, hv_MetrologyHandle);
                    HOperatorSet.GetMetrologyObjectResult(hv_MetrologyHandle, "all", "all", "result_type",
                        "all_param", out hv_o_Parameter);
                    HOperatorSet.ClearMetrologyModel(hv_MetrologyHandle);
                    if ((int)(new HTuple((new HTuple(hv_o_Parameter.TupleLength())).TupleEqual(
                        0))) != 0)
                    {
                        hv_o_ErrCode = 3;
                    }
                }
                // catch (Exception) 
                catch (HalconException HDevExpDefaultException1)
                {
                    HDevExpDefaultException1.ToHTuple(out hv_Exception);
                    hv_o_ErrCode = -1;
                    hv_o_ErrString = hv_Exception.Clone();
                }
                ho_Image.Dispose();

                return;
            }
            catch (HalconException HDevExpDefaultException)
            {
                ho_Image.Dispose();

                throw HDevExpDefaultException;
            }
        }
    }
}