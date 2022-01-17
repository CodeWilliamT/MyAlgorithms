using HalconDotNet;
using HTV_Algorithm;
using System.Collections.Generic;
using System;
using Utils;
using HT_Lib;
using IniDll;
using System.IO;
using System.ComponentModel;
using System.Windows.Forms;

namespace LeadframeAOI
{
    /// <summary>
    /// 检测结果
    /// </summary>
    public struct StructInspectResult
    {
        public bool OkOrNg;
        public double deltaX;
        public double deltaY;
        public HTuple defectType;
        public HTuple defectImgIdx;
        public int realRow;
        public int realCol;
        public InspectDetail inspectDetail;
    }
    /// <summary>
    /// 缺陷细节
    /// </summary>
    public class InspectDetail
    {
        public class MainIcInspect
        {
            public int Number;
            public HTuple Length;
            public HTuple Width;
            public HTuple Area;
            public double MaxLength;
            public double MaxWidth;
            public double MaxArea;
            public override string ToString()
            {
                string str = "";
                str = "主IC缺陷" + ":缺陷个数" + Number + ",最大长度=" + MaxLength + "Pix,最大宽度=" + MaxWidth + "Pix,最大面积=" + MaxArea + "Pix;";
                return str;
            }
        }
        public class MinorIcInspect
        {
            public int Number;
            public HTuple Length;
            public HTuple Width;
            public HTuple Area;
            public double MaxLength;
            public double MaxWidth;
            public double MaxArea;
            public override string ToString()
            {
                string str = "";
                str = "辅IC缺陷" + ":缺陷个数" + Number + ",最大长度=" + MaxLength + "Pix,最大宽度=" + MaxWidth + "Pix,最大面积=" + MaxArea + "Pix;";
                return str;
            }
        }
        public class ChipInspect
        {
            public int Number;
            public HTuple Length;
            public HTuple Width;
            public HTuple Area;
            public double MaxLength;
            public double MaxWidth;
            public double MaxArea;
            public override string ToString()
            {
                string str = "";
                str = "崩边缺陷" + ":缺陷个数" + Number + ",最大长度=" + MaxLength + "Pix,最大宽度=" + MaxWidth + "Pix,最大面积=" + MaxArea + "Pix;";
                return str;
            }
        }
        public class FrameInspect
        {
            public int Number;
            public HTuple Length;
            public HTuple Width;
            public HTuple Area;
            public double MaxLength;
            public double MaxWidth;
            public double MaxArea;
            public override string ToString()
            {
                string str = "";
                str = "框架缺陷" + ":缺陷个数" + Number + ",最大长度=" + MaxLength + "Pix,最大宽度=" + MaxWidth + "Pix,最大面积=" + MaxArea + "Pix;";
                return str;
            }
        }
        public class EpoxyInspect
        {
            public bool UpOut;
            public bool DownOut;
            public bool LeftOut;
            public bool RightOut;
            public override string ToString()
            {
                string str = "";
                str = "银胶缺陷" + ":" + (UpOut ? "上" : "") + (DownOut ? "下" : "") + (LeftOut ? "左" : "") + (RightOut ? "右" : "") + "溢胶;";
                return str;
            }
        }
        public class IcLoctionDiff
        {
            public double RowDiff;
            public double ColDiff;
            public double DistanceDiff;
            public double AngleDiff;
            public override string ToString()
            {
                string str = "";
                str = "IC偏移缺陷" + ":偏移行=" + RowDiff + "Pix,偏移列=" + ColDiff + "Pix,偏移欧式距离=" + DistanceDiff + "Pix,偏移角度=" + AngleDiff + "Pix;";
                return str;
            }
        }

        public class BondWiresInspect
        {
            public HTuple Radius_FirstBond = new HTuple();
            public HTuple Distance_WireBreak = new HTuple();
            public HTuple Score_SecondBond = new HTuple();
            public override string ToString()
            {
                string str = "";
                str = "焊球及金线缺陷" + ":";
                if (Radius_FirstBond != null && Radius_FirstBond.Length != 0)
                {
                    str += "第一焊点半径:";
                    for (int i = 0; i < Radius_FirstBond.Length; i++)
                    {
                        str += "焊点[" + i + "]:" + Radius_FirstBond[i];
                        if (i < Radius_FirstBond.Length - 1) str += "Pix,";
                    }
                    str += ";";
                }
                if (Distance_WireBreak != null && Distance_WireBreak.Length != 0)
                {
                    str += "金线最大断线距:";
                    for (int i = 0; i < Distance_WireBreak.Length; i++)
                    {
                        str += "金线[" + i + "]:" + Distance_WireBreak[i];
                        if (i < Distance_WireBreak.Length - 1) str += "Pix,";
                    }
                    str += ";";
                }
                if (Score_SecondBond != null && Score_SecondBond.Length != 0)
                {

                    str += "第二焊点匹配分数:";
                    for (int i = 0; i < Score_SecondBond.Length; i++)
                    {
                        str += "焊点[" + i + "]:" + Score_SecondBond[i];
                        if (i < Score_SecondBond.Length - 1) str += "(0-1.00),";
                    }
                    str += ";";
                }
                return str;
            }
            public string ToString(int Idx)
            {
                string str = "";
                str = "焊球及金线缺陷" + ":";
                switch (Idx)
                {
                    case 0:
                        if (Radius_FirstBond != null && Radius_FirstBond.Length != 0)
                        {
                            str += "第一焊点半径:";
                            for (int i = 0; i < Radius_FirstBond.Length; i++)
                            {
                                str += "焊点[" + i + "]:" + Radius_FirstBond[i];
                                if (i < Radius_FirstBond.Length - 1) str += "Pix,";
                            }
                            str += ";";
                        }
                        break;
                    case 1:
                        if (Distance_WireBreak != null && Distance_WireBreak.Length != 0)
                        {
                            str += "金线最大断线距:";
                            for (int i = 0; i < Distance_WireBreak.Length; i++)
                            {
                                str += "金线[" + i + "]:" + Distance_WireBreak[i];
                                if (i < Distance_WireBreak.Length - 1) str += "Pix,";
                            }
                            str += ";";
                        }
                        break;
                    case 2:
                        if (Score_SecondBond != null && Score_SecondBond.Length != 0)
                        {

                            str += "第二焊点匹配分数:";
                            for (int i = 0; i < Score_SecondBond.Length; i++)
                            {
                                str += "焊点[" + i + "]:" + Score_SecondBond[i];
                                if (i < Score_SecondBond.Length - 1) str += "(0-1.00),";
                            }
                            str += ";";
                        }
                        break;
                    default:
                        str += "无对应检测项描述;";
                        break;
                }
                return str;
            }
        }

        public MainIcInspect mainIcInspects;
        public MinorIcInspect minorIcInspects;
        public ChipInspect chipInspects;
        public FrameInspect frameInspects;
        public EpoxyInspect epoxyInspect;
        public IcLoctionDiff icLoctionDiffs;
        public BondWiresInspect bondWiresInspects;
        public InspectDetail()
        {
            mainIcInspects = new MainIcInspect();
            minorIcInspects = new MinorIcInspect();
            chipInspects = new ChipInspect();
            frameInspects = new FrameInspect();
            epoxyInspect = new EpoxyInspect();
            icLoctionDiffs = new IcLoctionDiff();
            bondWiresInspects = new BondWiresInspect();
        }
        public override string ToString()
        {
            string str = "";
            System.Reflection.FieldInfo[] infos = this.GetType().GetFields();
            foreach (System.Reflection.FieldInfo fi in infos)
            {
                str += fi.ToString();
            }
            return str;
        }

        public string ToString(int Idx)
        {
            string str = "";

            switch (Idx)
            {
                case 1:
                    str = "芯片缺陷无法识别";
                    break;
                case 2:
                    str = icLoctionDiffs.ToString();
                    break;
                case 3:
                    str = "无法识别芯片";
                    break;
                case 6:
                case 7:
                    str = mainIcInspects.ToString();
                    break;
                case 8:
                    str = chipInspects.ToString();
                    break;
                case 9:
                    str = epoxyInspect.ToString();
                    break;
                case 10:
                case 11:
                case 22:
                    str = bondWiresInspects.ToString(0);
                    break;
                case 12:
                case 13:
                case 20:
                    str = bondWiresInspects.ToString(1);
                    break;
                case 14:
                case 15:
                    str = bondWiresInspects.ToString(2);
                    break;
                case 18:
                    str = frameInspects.ToString();
                    break;
                default:
                    str = "无对应检测项描述";
                    break;
            }
            return str;
        }

    }
    public class AlgApp
    {
        #region 公共变量封装类，所有需要保存或外界使用的字段或属性
        public class Params
        {
            public static IniFiles config = null;
            public class GoldenModelPara
            {
                public double threshDark = 4.0;   //暗图的系数 D = M-d*S
                public double threshLight = 4.0;  //亮图的系数 L = M+l*S
                public double sobelFactor = 0.2;  //边缘系数   D'= D - s*S, L'= L+s*S
                public double closeSize = 6;
                public double minLength = 15;
                public double minWidth = 15;
                public double minArea = 30;

                [CategoryAttribute("模板参数"), DescriptionAttribute("暗图的系数 D = M-d*S")]
                public double ThreshDark { get { return threshDark; } set { threshDark = value; } }
                [CategoryAttribute("模板参数"), DescriptionAttribute("亮图的系数 L = M+l*S")]
                public double ThreshLight { get { return threshLight; } set { threshLight = value; } }
                [CategoryAttribute("模板参数"), DescriptionAttribute("边缘系数   D'= D - s*S, L'= L+s*S")]
                public double SobelFactor { get { return sobelFactor; } set { sobelFactor = value; } }
                [CategoryAttribute("模板参数"), DescriptionAttribute("闭操作大小")]
                public double CloseSize { get { return closeSize; } set { closeSize = value; } }
                [CategoryAttribute("模板参数"), DescriptionAttribute("模板最小长度")]
                public double MinLength { get { return minLength; } set { minLength = value; } }
                [CategoryAttribute("模板参数"), DescriptionAttribute("模板最小宽度")]
                public double MinWidth { get { return minWidth; } set { minWidth = value; } }
                [CategoryAttribute("模板参数"), DescriptionAttribute("模板最小面积")]
                public double MinArea { get { return minArea; } set { minArea = value; } }

                public GoldenModelPara(double threshDark = 4.0,   //暗图的系数 D = M-d*S
                double threshLight = 4.0,  //亮图的系数 L = M+l*S
                double sobelFactor = 0.2,  //边缘系数   D'= D - s*S, L'= L+s*S
                double closeSize = 6,
                double minLength = 15,
                double minWidth = 15,
                double minArea = 30)
                {
                    this.threshDark = threshDark;
                    this.threshLight = threshLight;
                    this.sobelFactor = sobelFactor;
                    this.closeSize = closeSize;
                    this.minLength = minLength;
                    this.minWidth = minWidth;
                    this.minArea = minArea;
                }
            }

            //粗匹配参数
            public double coarseDilationSize = 100.0;   //扩大一圈搜索
            public double dilationSize = 100.0;    //外扩尺寸,算法接口输入用
            public double coarseAngleStart = -7.5; //搜索的起始角度，单位degree
            public double coarseAngleExt = 15.0;   //搜索的角度范围，单位degree
            public double coarseMinScore = 0.55;   //匹配最小分数，0-1.0

            //MainIc 检测参数，其中另一部分在List<GoldenModelPara>中，属于多区域的问题
            public double mainIcDilationSize = 20;
            public double mainIcAngleStart = -7.5; //搜索的起始角度，单位degree
            public double mainIcAngleExt = 15.0;   //搜索的角度范围，单位degree
            public double mainIcMinScore = 0.5;  //匹配最小分数，0-1.0
            public string mainIcSelectOperate = "and"; //找到缺陷后选择的关系        
            public double mainIcNodieMinScore = 0.7;
            public double mainIcNum = 2;
            //新加的2018.9.20
            public double mainIcRowThr = 20;            //芯片偏移横坐标阈值
            public double mainIcColThr = 20;
            public double mainIcAngleThr = 20;           //芯片偏移角度阈值

            //MainIc 检测参数，其中另一部分在List<GoldenModelPara>中，属于多区域的问题
            public double mainIcWireDilationSize = 20;
            public double mainIcWireAngleStart = -7.5; //搜索的起始角度，单位degree
            public double mainIcWireAngleExt = 15.0;   //搜索的角度范围，单位degree
            public double mainIcWireMinScore = 0.5;  //匹配最小分数，0-1.0
            public string mainIcWireSelectOperate = "and"; //找到缺陷后选择的关系        
            public double mainIcWireNodieMinScore = 0.7;
            public double mainIcWireNum = 2;
            public double mainIcWireRowThr = 20;            //芯片偏移横坐标阈值
            public double mainIcWireColThr = 20;
            public double mainIcWireAngleThr = 20;           //芯片偏移角度阈值


            //MinorIc 检测参数，其中另一部分在List<GoldenModelPara>中，属于多区域的问题
            public double minorIcDilationSize = 20;
            public double minorIcAngleStart = -7.5; //搜索的起始角度，单位degree
            public double minorIcAngleExt = 15.0;   //搜索的角度范围，单位degree
            public double minorIcMinScore = 0.5;  //匹配最小分数，0-1.0
            public string minorIcSelectOperate = "and"; //找到缺陷后选择的关系        
            public double minorIcNodieMinScore = 0.7;
            public double minorIcNum = 2;
            //新加的2018.9.20
            public double minorIcRowThr = 20;            //芯片偏移横坐标阈值
            public double minorIcColThr = 20;
            public double minorIcAngleThr = 20;           //芯片偏移角度阈值

            //Frame 检测参数，其中另一部分在List<GoldenModelPara>中，属于多区域的问题
            public double frameDilationSize = 20;
            public double frameAngleStart = -7.5; //搜索的起始角度，单位degree
            public double frameAngleExt = 15.0;   //搜索的角度范围，单位degree
            public double frameMinScore = 0.5;  //匹配最小分数，0-1.0
            public string frameSelectOperate = "and"; //找到缺陷后选择的关系        
            public double frameNodieMinScore = 0.7;
            public double frameNum = 2;




            public List<GoldenModelPara> mainIcPara = new List<GoldenModelPara>() { new GoldenModelPara(3, 4, 0.2, 3, 10, 10, 10), new GoldenModelPara(2, 3, 0.5, 8, 10, 10, 20) };//加载模板所用的参数，由于可能有子区域所以这里是多个，如果子区域个数为n,则算上主区域，下面的长度为n+1,                              // 这里暂时先预设两个方便测试，实际是在做模板的时候决定的
            public List<GoldenModelPara> mainIcWirePara = new List<GoldenModelPara>() { new GoldenModelPara(3, 4, 0.2, 3, 10, 10, 10), new GoldenModelPara(2, 3, 0.5, 8, 10, 10, 20) };
            public List<GoldenModelPara> minorIcPara = new List<GoldenModelPara>() { new GoldenModelPara(3, 4, 0.2, 3, 10, 10, 10), new GoldenModelPara(2, 3, 0.5, 8, 10, 10, 20) };
            public List<GoldenModelPara> framePara = new List<GoldenModelPara>() { new GoldenModelPara(3, 4, 0.2, 3, 10, 10, 10), new GoldenModelPara(2, 3, 0.5, 8, 10, 10, 20) };

            //Epoxy inspect para                                                                                                                       //*Epoxy inspect para
            public double epoxyInspectSize = 70;
            public double epoxyDarkLight = 1;//1:light,0:dark
            public double epoxyEdgeSigma = 0.4;
            public double epoxyEdgeThresh = 2;
            public double epoxyDistThresh = 80;
            public double imgIdx = 1;
            //Bond wire inspect para
            public double minBall1Rad = 9;
            public double maxBall1Rad = 15;
            public double bond2AngleExt = 25.0;    //球2匹配的时候角度范围
            public double bond2MinScore = 0.8;
            public double wireSearchLen = 30;
            public double wireClipLen = 7;
            public double wireWidth = 15;
            public double wireContrast = 20;        //线的对比度
            public double wireMinSegLen = 3;
            public double wireAngleExt = 20;
            public double maxWireGap = 40;          //最大允许断开距离

            [CategoryAttribute("粗匹配参数"), DescriptionAttribute("算法匹配区外扩尺寸")]
            public double CoarseDilationSize { get { return coarseDilationSize; } set { coarseDilationSize = value; } }
            [CategoryAttribute("粗匹配参数"), DescriptionAttribute("接口匹配区外扩尺寸")]
            public double DilationSize { get { return dilationSize; } set { dilationSize = value; } }
            [CategoryAttribute("粗匹配参数"), DescriptionAttribute("匹配起始角度")]
            public double CoarseAngleStart { get { return coarseAngleStart; } set { coarseAngleStart = value; } }
            [CategoryAttribute("粗匹配参数"), DescriptionAttribute("匹配终止角度")]
            public double CoarseAngleExt { get { return coarseAngleExt; } set { coarseAngleExt = value; } }
            [CategoryAttribute("粗匹配参数"), DescriptionAttribute("匹配最小匹配分数")]
            public double CoarseMinScore { get { return coarseMinScore; } set { coarseMinScore = value; } }

            [CategoryAttribute("检测参数(MainIc)"), DescriptionAttribute("MainIc外扩尺寸")]
            public double MainIcDilationSize { get { return mainIcDilationSize; } set { mainIcDilationSize = value; } }
            [CategoryAttribute("检测参数(MainIc)"), DescriptionAttribute("MainIc起始角度")]
            public double MainIcAngleStart { get { return mainIcAngleStart; } set { mainIcAngleStart = value; } }
            [CategoryAttribute("检测参数(MainIc)"), DescriptionAttribute("MainIc终止角度")]
            public double MainIcAngleExt { get { return mainIcAngleExt; } set { mainIcAngleExt = value; } }
            [CategoryAttribute("检测参数(MainIc)"), DescriptionAttribute("MainIc最小匹配分数")]
            public double MainIcMinScore { get { return mainIcMinScore; } set { mainIcMinScore = value; } }
            [CategoryAttribute("检测参数(MainIc)"), DescriptionAttribute("MainIc筛选方法")]
            public string MainIcSelectOperate { get { return mainIcSelectOperate; } set { mainIcSelectOperate = value; } }
            [CategoryAttribute("检测参数(MainIc)"), DescriptionAttribute("MainIc无芯片最小匹配分数")]
            public double MainIcNodieMinScore { get { return mainIcNodieMinScore; } set { mainIcNodieMinScore = value; } }
            [CategoryAttribute("检测参数(MainIc)"), DescriptionAttribute("MainIc芯片横偏移阈值")]
            public double MainIcRowThr { get { return mainIcRowThr; } set { mainIcRowThr = value; } }
            [CategoryAttribute("检测参数(MainIc)"), DescriptionAttribute("MainIc芯片纵偏移阈值")]
            public double MainIcColThr { get { return mainIcColThr; } set { mainIcColThr = value; } }

            [CategoryAttribute("检测参数(MainIc)"), DescriptionAttribute("MainIc芯片角度偏移阈值")]
            public double MainIcAngleThr { get { return mainIcAngleThr; } set { mainIcAngleThr = value; } }

            //0928添加 IC的wire图检测参数
            [CategoryAttribute("检测参数(MainIcWire)"), DescriptionAttribute("MainIcWire外扩尺寸")]
            public double MainIcWireDilationSize { get { return mainIcWireDilationSize; } set { mainIcWireDilationSize = value; } }
            [CategoryAttribute("检测参数(MainIcWire)"), DescriptionAttribute("MainIcWire起始角度")]
            public double MainIcWireAngleStart { get { return mainIcWireAngleStart; } set { mainIcWireAngleStart = value; } }
            [CategoryAttribute("检测参数(MainIcWire)"), DescriptionAttribute("MainIcWire终止角度")]
            public double MainIcWireAngleExt { get { return mainIcWireAngleExt; } set { mainIcWireAngleExt = value; } }
            [CategoryAttribute("检测参数(MainIcWire)"), DescriptionAttribute("MainIcWire最小匹配分数")]
            public double MainIcWireMinScore { get { return mainIcWireMinScore; } set { mainIcWireMinScore = value; } }
            [CategoryAttribute("检测参数(MainIcWire)"), DescriptionAttribute("MainIcWire筛选方法")]
            public string MainIcWireSelectOperate { get { return mainIcWireSelectOperate; } set { mainIcWireSelectOperate = value; } }
            [CategoryAttribute("检测参数(MainIcWire)"), DescriptionAttribute("MainIcWire无芯片最小匹配分数")]
            public double MainIcWireNodieMinScore { get { return mainIcWireNodieMinScore; } set { mainIcWireNodieMinScore = value; } }
            [CategoryAttribute("检测参数(MainIcWire)"), DescriptionAttribute("MainIcWire芯片横偏移阈值")]
            public double MainIcWireRowThr { get { return mainIcWireRowThr; } set { mainIcWireRowThr = value; } }
            [CategoryAttribute("检测参数(MainIcWire)"), DescriptionAttribute("MainIcWire芯片纵偏移阈值")]
            public double MainIcWireColThr { get { return mainIcWireColThr; } set { mainIcWireColThr = value; } }

            [CategoryAttribute("检测参数(MainIcWire)"), DescriptionAttribute("MainIcWire芯片角度偏移阈值")]
            public double MainIcWireAngleThr { get { return mainIcWireAngleThr; } set { mainIcWireAngleThr = value; } }




            [CategoryAttribute("检测参数(MinorIc)"), DescriptionAttribute("MinorIc外扩尺寸")]
            public double MinorIcDilationSize { get { return minorIcDilationSize; } set { minorIcDilationSize = value; } }
            [CategoryAttribute("检测参数(MinorIc)"), DescriptionAttribute("MinorIc起始角度")]
            public double MinorIcAngleStart { get { return minorIcAngleStart; } set { minorIcAngleStart = value; } }
            [CategoryAttribute("检测参数(MinorIc)"), DescriptionAttribute("MinorIc终止角度")]
            public double MinorIcAngleExt { get { return minorIcAngleExt; } set { minorIcAngleExt = value; } }
            [CategoryAttribute("检测参数(MinorIc)"), DescriptionAttribute("MinorIc最小匹配分数")]
            public double MinorIcMinScore { get { return minorIcMinScore; } set { minorIcMinScore = value; } }
            [CategoryAttribute("检测参数(MinorIc)"), DescriptionAttribute("MinorIc筛选方法")]
            public string MinorIcSelectOperate { get { return minorIcSelectOperate; } set { minorIcSelectOperate = value; } }
            [CategoryAttribute("检测参数(MinorIc)"), DescriptionAttribute("MinorIc无芯片最小匹配分数")]
            public double MinorIcNodieMinScore { get { return minorIcNodieMinScore; } set { minorIcNodieMinScore = value; } }
            [CategoryAttribute("检测参数(MinorIc)"), DescriptionAttribute("MinorIc芯片行偏移阈值")]
            public double MinorIcRowThr { get { return minorIcRowThr; } set { minorIcRowThr = value; } }
            [CategoryAttribute("检测参数(MinorIc)"), DescriptionAttribute("MinorIc芯片纵偏移阈值")]
            public double MinorIcColThr { get { return minorIcColThr; } set { minorIcColThr = value; } }
            [CategoryAttribute("检测参数(MinorIc)"), DescriptionAttribute("MinorIc芯片角度偏移阈值")]
            public double MinorIcAngleThr { get { return minorIcAngleThr; } set { minorIcAngleThr = value; } }

            [CategoryAttribute("检测参数(Frame框架)"), DescriptionAttribute("Frame框架外扩尺寸")]
            public double FrameDilationSize { get { return frameDilationSize; } set { frameDilationSize = value; } }
            [CategoryAttribute("检测参数(Frame框架)"), DescriptionAttribute("Frame框架起始角度")]
            public double FrameAngleStart { get { return frameAngleStart; } set { frameAngleStart = value; } }
            [CategoryAttribute("检测参数(Frame框架)"), DescriptionAttribute("Frame框架终止角度")]
            public double FrameAngleExt { get { return frameAngleExt; } set { frameAngleExt = value; } }
            [CategoryAttribute("检测参数(Frame框架)"), DescriptionAttribute("Frame框架最小匹配分数")]
            public double FrameMinScore { get { return frameMinScore; } set { frameMinScore = value; } }
            [CategoryAttribute("检测参数(Frame框架)"), DescriptionAttribute("Frame框架筛选方法")]
            public string FrameSelectOperate { get { return frameSelectOperate; } set { frameSelectOperate = value; } }
            [CategoryAttribute("检测参数(Frame框架)"), DescriptionAttribute("Frame框架无芯片最小匹配分数")]
            public double FrameNodieMinScore { get { return frameNodieMinScore; } set { frameNodieMinScore = value; } }

            [CategoryAttribute("检测参数(Epoxy)"), DescriptionAttribute("Epoxy银胶检测尺寸")]
            public double EpoxyInspectSize { get { return epoxyInspectSize; } set { epoxyInspectSize = value; } }
            [CategoryAttribute("检测参数(Epoxy)"), DescriptionAttribute("Epoxy银胶亮暗系数")]
            public double EpoxyDarkLight { get { return epoxyDarkLight; } set { epoxyDarkLight = value; } }
            [CategoryAttribute("检测参数(Epoxy)"), DescriptionAttribute("Epoxy银胶边缘提取系数")]
            public double EpoxyEdgeSigma { get { return epoxyEdgeSigma; } set { epoxyEdgeSigma = value; } }
            [CategoryAttribute("检测参数(Epoxy)"), DescriptionAttribute("Epoxy银胶边缘筛选分数")]
            public double EpoxyEdgeThresh { get { return epoxyEdgeThresh; } set { epoxyEdgeThresh = value; } }
            [CategoryAttribute("检测参数(Epoxy)"), DescriptionAttribute("Epoxy银胶检测阈值")]
            public double EpoxyDistThresh { get { return epoxyDistThresh; } set { epoxyDistThresh = value; } }
            [CategoryAttribute("检测参数(Epoxy)"), DescriptionAttribute("Epoxy银胶检测图号")]
            public double ImgIdx { get { return imgIdx; } set { imgIdx = value; } }
            [CategoryAttribute("检测参数(Bond wire)"), DescriptionAttribute("Bond第一焊点最小半径")]
            public double MinBall1Rad { get { return minBall1Rad; } set { minBall1Rad = value; } }
            [CategoryAttribute("检测参数(Bond wire)"), DescriptionAttribute("Bond第一焊点最大半径")]
            public double MaxBall1Rad { get { return maxBall1Rad; } set { maxBall1Rad = value; } }
            [CategoryAttribute("检测参数(Bond wire)"), DescriptionAttribute("Bond第二焊点匹配角度范围")]
            public double Bond2AngleExt { get { return bond2AngleExt; } set { bond2AngleExt = value; } }
            [CategoryAttribute("检测参数(Bond wire)"), DescriptionAttribute("Bond第二焊点匹配最小阈值分数")]
            public double Bond2MinScore { get { return bond2MinScore; } set { bond2MinScore = value; } }
            [CategoryAttribute("检测参数(Bond wire)"), DescriptionAttribute("Bond金线检测区域长度")]
            public double WireSearchLen { get { return wireSearchLen; } set { wireSearchLen = value; } }
            [CategoryAttribute("检测参数(Bond wire)"), DescriptionAttribute("Bond金线检测剪切长度")]
            public double WireClipLen { get { return wireClipLen; } set { wireClipLen = value; } }
            [CategoryAttribute("检测参数(Bond wire)"), DescriptionAttribute("Bond金线检测宽度")]
            public double WireWidth { get { return wireWidth; } set { wireWidth = value; } }
            [CategoryAttribute("检测参数(Bond wire)"), DescriptionAttribute("Bond金线检测对比度")]
            public double WireContrast { get { return wireContrast; } set { wireContrast = value; } }
            [CategoryAttribute("检测参数(Bond wire)"), DescriptionAttribute("Bond金线检测最短长度")]
            public double WireMinSegLen { get { return wireMinSegLen; } set { wireMinSegLen = value; } }
            [CategoryAttribute("检测参数(Bond wire)"), DescriptionAttribute("Bond金线角度检测范围")]
            public double WireAngleExt { get { return wireAngleExt; } set { wireAngleExt = value; } }
            [CategoryAttribute("检测参数(Bond wire)"), DescriptionAttribute("Bond金线检测最大连接距离")]
            public double MaxWireGap { get { return maxWireGap; } set { maxWireGap = value; } }


            public bool Save()
            {
                if (!File.Exists(App.ProductDir + "\\" + ProductMagzine.ActivePdt + "\\AlgParams.ini"))
                {
                    config = new IniFiles(App.ProductDir + "\\" + ProductMagzine.ActivePdt + "\\AlgParams.ini");
                }
                for (int i = 0; i < mainIcPara.Count; i++)
                {
                    config.SaveObj(mainIcPara[i], "mainIcPara_" + i);
                }
                for (int i = 0; i < minorIcPara.Count; i++)
                {
                    config.SaveObj(minorIcPara[i], "minorIcPara_" + i);
                }
                for (int i = 0; i < framePara.Count; i++)
                {
                    config.SaveObj(framePara[i], "framePara_" + i);
                }
                mainIcNum = mainIcPara.Count;
                minorIcNum = minorIcPara.Count;
                frameNum = framePara.Count;
                return config.SaveObj(this);

            }
            public bool Load()
            {
                if (File.Exists(App.ProductDir + "\\" + ProductMagzine.ActivePdt + "\\AlgParams.ini"))
                {
                    config = new IniFiles(App.ProductDir + "\\" + ProductMagzine.ActivePdt + "\\AlgParams.ini");
                    config.LoadObj(this);
                    mainIcPara = new List<GoldenModelPara>();
                    GoldenModelPara mainIcItem = null;
                    for (int i = 0; i < mainIcNum; i++)
                    {
                        mainIcItem = new GoldenModelPara();
                        config.LoadObj(mainIcItem, "mainIcPara_" + i);
                        mainIcPara.Add(mainIcItem);
                    }
                    minorIcPara = new List<GoldenModelPara>();
                    GoldenModelPara minorIcItem = null;
                    for (int i = 0; i < minorIcNum; i++)
                    {
                        minorIcItem = new GoldenModelPara();
                        config.LoadObj(minorIcItem, "minorIcPara_" + i);
                        minorIcPara.Add(minorIcItem);
                    }
                    framePara = new List<GoldenModelPara>();
                    GoldenModelPara frameItem = null;
                    for (int i = 0; i < frameNum; i++)
                    {
                        frameItem = new GoldenModelPara();
                        config.LoadObj(frameItem, "framePara_" + i);
                        framePara.Add(frameItem);
                    }
                    return true;
                }
                else
                {
                    return true;
                }
            }
        }
        public Params P = new Params();
        #endregion
        #region 私有变量
        private bool isModelRead = false;
        private string errString = "";
        private HObjectVector hvec__CoarseMatchObj = new HObjectVector(1);
        private HObjectVector hvec__MainIcObjs = new HObjectVector(1);
        private HObjectVector hvec__MainIcWireObjs = new HObjectVector(1);
        private HObjectVector hvec__MinorIcObjs = new HObjectVector(1);
        private HObjectVector hvec__FrameObjs = new HObjectVector(1);
        private HObjectVector hvec__BondWireObjs = new HObjectVector(1);

        private HTuple hv__CoarseModel = null;
        private HTuple hv__MainIcModel = null;
        private HTuple hv__MainIcWireModel = null;
        private HTuple hv__MinorIcModel = null;
        private HTuple hv__FrameModel = null;
        private HTuple hv__NodieModel = null;
        private HTupleVector hvec__BondWireModel = new HTupleVector(1);
        private HTupleVector hvec__EpoxyArgs = new HTupleVector(1);
        #endregion
        #region 共有方法
        public string GetLastErrStr()
        {
            return errString;
        }
        /// <summary>读取所有模板信息</summary>
        /// <param name="ModelPath">模板路径，对于实际项目中模板路径应该存在与产品目录的下一层.例如D:\\PDT1\\Model\\最后的斜杠不能缺失</param>
        /// <returns>True表示成功，False表示失败，可以通过GetLastErrStr获取</returns>
        public bool ReadAllModel(string ModelPath)
        {
            ClearAllModel();
            HTuple _ErrCode = new HTuple();
            HTuple _ErrString = new HTuple();
            var _MainIcModelPara = new HTupleVector(1);
            var _MainIcThreshDark = new HTuple();
            var _MainIcThreshLight = new HTuple();
            var _MainIcSobel = new HTuple();
            for (int i = 0; i < P.mainIcPara.Count; i++)
            {
                _MainIcThreshDark[i] = P.mainIcPara[i].threshDark;
                _MainIcThreshLight[i] = P.mainIcPara[i].threshLight;
                _MainIcSobel[i] = P.mainIcPara[i].sobelFactor;
            }
            _MainIcModelPara = (((new HTupleVector(1).Insert(0, new HTupleVector(_MainIcThreshDark))).Insert(
                    1, new HTupleVector(_MainIcThreshLight))).Insert(2, new HTupleVector(_MainIcSobel)));


            var _MainIcWireModelPara = new HTupleVector(1);
            var _MainIcWireThreshDark = new HTuple();
            var _MainIcWireThreshLight = new HTuple();
            var _MainIcWireSobel = new HTuple();
            for (int i = 0; i < P.mainIcWirePara.Count; i++)
            {
                _MainIcWireThreshDark[i] = P.mainIcWirePara[i].threshDark;
                _MainIcWireThreshLight[i] = P.mainIcWirePara[i].threshLight;
                _MainIcWireSobel[i] = P.mainIcWirePara[i].sobelFactor;
            }
            _MainIcWireModelPara = (((new HTupleVector(1).Insert(0, new HTupleVector(_MainIcWireThreshDark))).Insert(
                    1, new HTupleVector(_MainIcWireThreshLight))).Insert(2, new HTupleVector(_MainIcWireSobel)));



            var _MinorIcModelPara = new HTupleVector(1);
            var _MinorIcThreshDark = new HTuple();
            var _MinorIcThreshLight = new HTuple();
            var _MinorIcSobel = new HTuple();
            for (int i = 0; i < P.minorIcPara.Count; i++)
            {
                _MinorIcThreshDark[i] = P.minorIcPara[i].threshDark;
                _MinorIcThreshLight[i] = P.minorIcPara[i].threshLight;
                _MinorIcSobel[i] = P.minorIcPara[i].sobelFactor;
            }
            _MinorIcModelPara = (((new HTupleVector(1).Insert(0, new HTupleVector(_MinorIcThreshDark))).Insert(
                    1, new HTupleVector(_MinorIcThreshLight))).Insert(2, new HTupleVector(_MinorIcSobel)));

            var _FrameModelPara = new HTupleVector(1);
            var _FrameThreshDark = new HTuple();
            var _FrameThreshLight = new HTuple();
            var _FrameSobel = new HTuple();
            for (int i = 0; i < P.framePara.Count; i++)
            {
                _FrameThreshDark[i] = P.framePara[i].threshDark;
                _FrameThreshLight[i] = P.framePara[i].threshLight;
                _FrameSobel[i] = P.framePara[i].sobelFactor;
            }
            _FrameModelPara = (((new HTupleVector(1).Insert(0, new HTupleVector(_FrameThreshDark))).Insert(
                    1, new HTupleVector(_FrameThreshLight))).Insert(2, new HTupleVector(_FrameSobel)));
            hvec__MainIcObjs.Dispose();
            hvec__MainIcWireObjs.Dispose();
            hvec__MinorIcObjs.Dispose();
            hvec__FrameObjs.Dispose();
            hvec__BondWireObjs.Dispose();
            JSCC_AOI_read_all_model(out hvec__CoarseMatchObj, out hvec__MainIcObjs, out hvec__MainIcWireObjs, out hvec__MinorIcObjs, out hvec__FrameObjs,
                      out hvec__BondWireObjs, new HTuple(ModelPath), _MainIcModelPara, _MainIcWireModelPara, _MinorIcModelPara, _FrameModelPara,
                      out hv__CoarseModel, out hv__MainIcModel, out hv__MainIcWireModel, out hv__MinorIcModel, out hv__FrameModel, out hvec__BondWireModel,
                      out _ErrCode, out _ErrString);
            if ((int)(new HTuple(_ErrCode.TupleLess(0))) != 0)
            {
                errString = _ErrString.S;
                return false;
            }
            isModelRead = true;
            return true;
        }

        public bool ClearAllModel()
        {
            if (isModelRead)
            {
                HTuple _ErrCode = new HTuple();
                HTuple _ErrString = new HTuple();
                JSCC_AOI_clear_all_model(hv__CoarseModel, hv__MainIcModel, hv__MainIcWireModel, hv__MinorIcModel,
          hvec__BondWireModel, hv__FrameModel, out _ErrCode, out _ErrString);
                if ((int)(new HTuple(_ErrCode.TupleLess(0))) != 0)
                {
                    errString = _ErrString.S;
                    return false;
                }
                isModelRead = false;
            }
            return true;
        }

        public bool Save(string xmlFile)
        {
            try
            {

                HTHelper.Xml.SerializeToFile(P, xmlFile);
            }
            catch (Exception ex)
            {
                errString = ex.ToString();
                return false;
            }
            return true;
        }

        public bool Read(string xmlFile)
        {
            try
            {
                P = HTHelper.Xml.DeserializeFromFile<Params>(xmlFile);
            }
            catch (Exception ex)
            {
                errString = ex.ToString();
                return false;
            }
            return true;
        }
        /// <summary>
        /// 检测算法
        /// </summary>
        /// <param name="Images">把所有拍的图concat在一起传进来</param>
        /// <param name="ImageKey">int数组[b, r, c]每张图的Key</param>
        /// <param name="BRCMN">int数组[B, R, C, M, N]，产品参数，总数.分别是Block数，单个BLock的行列数，以及每个视野拍到的行列数</param>
        /// <param name="FailRegions">输出的缺陷区域，每个产品的缺陷concat在一起了</param>
        /// <param name="Wires">金线，contours</param>
        /// <param name="Results">结果队列</param>
        /// <returns>True表示成功执行，False表示中间发生了异常</returns>
        /// out ImgItems,

        public bool Inspection(HObject Images, HObject _CoarseReg, HTuple _Row, HTuple _Col, out HObject FailRegions, out HObject Wires, out HObjectVector _VWires,
            out HObjectVector _VFailRegs, out List<StructInspectResult> Results)
        {
            HObject ho__ImageIC = null, ho__ImageWire = null, ho_Red = null;
            HTuple _ErrCode = new HTuple();
            HTuple _ErrString = new HTuple();
            Results = new List<StructInspectResult>();
            HTupleVector _Result = new HTupleVector(1);
            var _ImageVector = new HObjectVector(1);
            HTuple num = new HTuple();
            HOperatorSet.CountObj(Images, out num);
            HObject _Image = null;
            HObject ImgChannel0, ImgChannel1, ImgChannel2;
            HOperatorSet.GenEmptyObj(out _Image);
            HOperatorSet.GenEmptyObj(out ImgChannel0);
            HOperatorSet.GenEmptyObj(out ImgChannel1);
            HOperatorSet.GenEmptyObj(out ImgChannel2);
            HTuple numChannels = null;
            for (int i = 0; i < num; i++)
            {
                HOperatorSet.CountChannels(Images.SelectObj(i + 1), out numChannels);
                if (numChannels.I == 3)
                {
                    ImgChannel0.Dispose();
                    ImgChannel1.Dispose();
                    ImgChannel2.Dispose();
                    HOperatorSet.Decompose3(Images.SelectObj(i + 1), out ImgChannel0, out ImgChannel1, out ImgChannel2);
                    _ImageVector.Insert(i, new HObjectVector(ImgChannel0));
                    //_ImageVector.Insert(i, new HObjectVector(ImgChannel1));
                    _ImageVector.Insert(i, new HObjectVector(ImgChannel2));
                }
                else
                {
                    _Image.Dispose();
                    _Image = Images.SelectObj(i + 1);
                    _ImageVector.Insert(i, new HObjectVector(_Image));
                }
            }
            _Image.Dispose();
            ImgChannel0.Dispose();
            ImgChannel1.Dispose();
            ImgChannel2.Dispose();

            double[] tmp = new double[4] { P.coarseDilationSize, P.coarseAngleStart * Math.PI / 180.0, P.coarseAngleExt * Math.PI / 180.0, P.coarseMinScore };
            var _CoarseArgs = (new HTupleVector(1).Insert(0, new HTupleVector(hv__CoarseModel))).Insert(
                1, new HTupleVector(new HTuple(tmp)));

            //MainIc inspect para
            var _MainIcCloseSize = new HTuple();
            var _MainIcMinLength = new HTuple();
            var _MainIcMinWidth = new HTuple();
            var _MainIcMinArea = new HTuple();
            for (int i = 0; i < P.mainIcPara.Count; i++)
            {
                _MainIcCloseSize[i] = P.mainIcPara[i].closeSize;
                _MainIcMinLength[i] = P.mainIcPara[i].minLength;
                _MainIcMinWidth[i] = P.mainIcPara[i].minWidth;
                _MainIcMinArea[i] = P.mainIcPara[i].minArea;
            }
            var _MainIcArgs = (((((((((((((new HTupleVector(1).Insert(0, new HTupleVector(hv__MainIcModel))).Insert(
         1, new HTupleVector(P.mainIcDilationSize))).Insert(2, new HTupleVector(P.mainIcAngleStart))).Insert(
         3, new HTupleVector(P.mainIcAngleExt))).Insert(4, new HTupleVector(P.mainIcMinScore))).Insert(
         5, new HTupleVector(_MainIcCloseSize))).Insert(6, new HTupleVector(_MainIcMinLength))).Insert(
         7, new HTupleVector(_MainIcMinWidth))).Insert(8, new HTupleVector(_MainIcMinArea))).Insert(
         9, new HTupleVector(P.mainIcSelectOperate))).Insert(10, new HTupleVector(P.mainIcRowThr))).Insert(
         11, new HTupleVector(P.mainIcColThr))).Insert(12, new HTupleVector(P.mainIcAngleThr)));

            //MainIcwire inspect para
            var _MainIcWireCloseSize = new HTuple();
            var _MainIcWireMinLength = new HTuple();
            var _MainIcWireMinWidth = new HTuple();
            var _MainIcWireMinArea = new HTuple();
            for (int i = 0; i < P.mainIcWirePara.Count; i++)
            {
                _MainIcWireCloseSize[i] = P.mainIcWirePara[i].closeSize;
                _MainIcWireMinLength[i] = P.mainIcWirePara[i].minLength;
                _MainIcWireMinWidth[i] = P.mainIcWirePara[i].minWidth;
                _MainIcWireMinArea[i] = P.mainIcWirePara[i].minArea;
            }
            var _MainIcWireArgs = (((((((((((((new HTupleVector(1).Insert(0, new HTupleVector(hv__MainIcWireModel))).Insert(
         1, new HTupleVector(P.mainIcWireDilationSize))).Insert(2, new HTupleVector(P.mainIcWireAngleStart))).Insert(
         3, new HTupleVector(P.mainIcWireAngleExt))).Insert(4, new HTupleVector(P.mainIcWireMinScore))).Insert(
         5, new HTupleVector(_MainIcWireCloseSize))).Insert(6, new HTupleVector(_MainIcWireMinLength))).Insert(
         7, new HTupleVector(_MainIcWireMinWidth))).Insert(8, new HTupleVector(_MainIcWireMinArea))).Insert(
         9, new HTupleVector(P.mainIcWireSelectOperate))).Insert(10, new HTupleVector(P.mainIcWireRowThr))).Insert(
         11, new HTupleVector(P.mainIcWireColThr))).Insert(12, new HTupleVector(P.mainIcWireAngleThr)));

            //MinorIc inspect para
            var _MinorIcCloseSize = new HTuple();
            var _MinorIcMinLength = new HTuple();
            var _MinorIcMinWidth = new HTuple();
            var _MinorIcMinArea = new HTuple();
            for (int i = 0; i < P.minorIcPara.Count; i++)
            {
                _MinorIcCloseSize[i] = P.minorIcPara[i].closeSize;
                _MinorIcMinLength[i] = P.minorIcPara[i].minLength;
                _MinorIcMinWidth[i] = P.minorIcPara[i].minWidth;
                _MinorIcMinArea[i] = P.minorIcPara[i].minArea;
            }
            var _MinorIcArgs = (((((((((((((new HTupleVector(1).Insert(0, new HTupleVector(hv__MinorIcModel))).Insert(
          1, new HTupleVector(P.minorIcDilationSize))).Insert(2, new HTupleVector(P.minorIcAngleStart))).Insert(
          3, new HTupleVector(P.minorIcAngleExt))).Insert(4, new HTupleVector(P.minorIcMinScore))).Insert(
          5, new HTupleVector(_MinorIcCloseSize))).Insert(6, new HTupleVector(_MinorIcMinLength))).Insert(
          7, new HTupleVector(_MinorIcMinWidth))).Insert(8, new HTupleVector(_MinorIcMinArea))).Insert(
          9, new HTupleVector(P.minorIcSelectOperate))).Insert(10, new HTupleVector(P.minorIcRowThr))).Insert(
          11, new HTupleVector(P.minorIcColThr))).Insert(12, new HTupleVector(P.minorIcAngleThr)));

            //Frame inspect para
            var _FrameCloseSize = new HTuple();
            var _FrameMinLength = new HTuple();
            var _FrameMinWidth = new HTuple();
            var _FrameMinArea = new HTuple();
            for (int i = 0; i < P.framePara.Count; i++)
            {
                _FrameCloseSize[i] = P.framePara[i].closeSize;
                _FrameMinLength[i] = P.framePara[i].minLength;
                _FrameMinWidth[i] = P.framePara[i].minWidth;
                _FrameMinArea[i] = P.framePara[i].minArea;
            }
            var _FrameArgs = ((((((((((new HTupleVector(1).Insert(0, new HTupleVector(hv__FrameModel))).Insert(
          1, new HTupleVector(P.frameDilationSize))).Insert(2, new HTupleVector(P.frameAngleStart))).Insert(
          3, new HTupleVector(P.frameAngleExt))).Insert(4, new HTupleVector(P.frameMinScore))).Insert(
          5, new HTupleVector(_FrameCloseSize))).Insert(6, new HTupleVector(_FrameMinLength))).Insert(
          7, new HTupleVector(_FrameMinWidth))).Insert(8, new HTupleVector(_FrameMinArea))).Insert(
          9, new HTupleVector(P.frameSelectOperate)));


            //Bond wire inspect para
            tmp = new double[] { P.minBall1Rad, P.maxBall1Rad, P.bond2AngleExt * Math.PI / 180.0, P.bond2MinScore, P.wireSearchLen, P.wireClipLen, P.wireWidth, P.wireContrast, P.wireMinSegLen, P.wireAngleExt * Math.PI / 180.0, P.maxWireGap };

            var _BondWireArgs = hvec__BondWireModel.Concat(new HTupleVector(1).Insert(0, new HTupleVector(new HTuple(tmp))));

            var _EpoxyArgs = ((((((new HTupleVector(1).Insert(0, new HTupleVector(P.epoxyInspectSize))).Insert(
                1, new HTupleVector(P.epoxyDarkLight))).Insert(2, new HTupleVector(P.epoxyEdgeSigma))).Insert(
                3, new HTupleVector(P.epoxyEdgeThresh))).Insert(4, new HTupleVector(P.epoxyDistThresh))).Insert(
                5, new HTupleVector(P.imgIdx)));
            //if (ho__ImageIC != null) ho__ImageIC.Dispose();
            // HOperatorSet.SelectObj(Images, out ho__ImageIC, 3);

            //if (ho__ImageWire != null) ho__ImageWire.Dispose();
            //HOperatorSet.SelectObj(Images, out ho__ImageWire, 1);


            //HOperatorSet.Decompose3(Images, out ho__ImageWire, out ho_Red, out ho__ImageIC);
            //using (HDevDisposeHelper dh = new HDevDisposeHelper())
            //{
            //    _ImageVec = dh.Take(((
            //        dh.Add(new HObjectVector(1)).Insert(0, dh.Add(new HObjectVector(ho__ImageIC)))).Insert(
            //        1, dh.Add(new HObjectVector(ho__ImageWire)))));
            //}
            JSCC_AOI_inspect(_ImageVector, hvec__CoarseMatchObj, hvec__MainIcObjs, hvec__MainIcWireObjs, hvec__MinorIcObjs, hvec__FrameObjs,
                        hvec__BondWireObjs, _CoarseReg, out FailRegions, out Wires, out _VWires, out _VFailRegs,
                        _CoarseArgs, _MainIcArgs, _MainIcWireArgs, _MinorIcArgs, _FrameArgs, _BondWireArgs, _EpoxyArgs, _Row, _Col,
                        out _Result, out _ErrCode, out _ErrString);
            if ((int)(new HTuple(_ErrCode.TupleLess(0))) != 0)
            {
                errString = _ErrString.S;
                return false;
            }
            int _res = 0;
            for (int i = 0; i < _Result.Length; i++)
            {
                _res = _Result.At(i).At(0).At(0).T[0].I;
                if (_res == 0)
                {
                    continue;
                }
                StructInspectResult itemResult = new StructInspectResult() { OkOrNg = (_res == 1), realRow = _Result.At(i).At(1).At(0).T.TupleSelect(0).I, realCol = _Result.At(i).At(2).At(0).T.TupleSelect(0).I, defectType = (_res == 1 ? new HTuple(0) : (_Result.At(i).At(3).At(0).T)), defectImgIdx = (_res == 1 ? new HTuple(0) : (_Result.At(i).At(4).At(0).T)) };
                itemResult.inspectDetail = new InspectDetail();
                if (_Result.At(i).At(5).Length == 0)
                {

                }
                else
                {
                    if (_Result.At(i).At(5).At(0).T.Length == 0)
                    {
                        itemResult.inspectDetail.mainIcInspects.Number = 0;
                        itemResult.inspectDetail.mainIcInspects.Length = 0;
                        itemResult.inspectDetail.mainIcInspects.Width = 0;
                        itemResult.inspectDetail.mainIcInspects.Area = 0;
                        itemResult.inspectDetail.mainIcInspects.MaxLength = 0;
                        itemResult.inspectDetail.mainIcInspects.MaxWidth = 0;
                        itemResult.inspectDetail.mainIcInspects.MaxArea = 0;
                    }
                    else
                    {
                        itemResult.inspectDetail.mainIcInspects.Number = _Result.At(i).At(5).At(0).T.TupleSelect(0).I;
                        itemResult.inspectDetail.mainIcInspects.Length = _Result.At(i).At(5).At(1).T;
                        itemResult.inspectDetail.mainIcInspects.Width = _Result.At(i).At(5).At(2).T;
                        itemResult.inspectDetail.mainIcInspects.Area = _Result.At(i).At(5).At(3).T;
                        itemResult.inspectDetail.mainIcInspects.MaxLength = _Result.At(i).At(5).At(4).T.TupleSelect(0).I;
                        itemResult.inspectDetail.mainIcInspects.MaxWidth = _Result.At(i).At(5).At(5).T.TupleSelect(0).I;
                        itemResult.inspectDetail.mainIcInspects.MaxArea = _Result.At(i).At(5).At(6).T.TupleSelect(0).I;
                    };

                    if (_Result.At(i).At(5).At(7).T.Length == 0)
                    {
                        itemResult.inspectDetail.minorIcInspects.Number = 0;
                        itemResult.inspectDetail.minorIcInspects.Length = 0;
                        itemResult.inspectDetail.minorIcInspects.Width = 0;
                        itemResult.inspectDetail.minorIcInspects.Area = 0;
                        itemResult.inspectDetail.minorIcInspects.MaxLength = 0;
                        itemResult.inspectDetail.minorIcInspects.MaxWidth = 0;
                        itemResult.inspectDetail.minorIcInspects.MaxArea = 0;
                    }
                    else
                    {
                        itemResult.inspectDetail.minorIcInspects.Number = _Result.At(i).At(5).At(7).T.TupleSelect(0).I;
                        itemResult.inspectDetail.minorIcInspects.Length = _Result.At(i).At(5).At(8).T;
                        itemResult.inspectDetail.minorIcInspects.Width = _Result.At(i).At(5).At(9).T;
                        itemResult.inspectDetail.minorIcInspects.Area = _Result.At(i).At(5).At(10).T;
                        itemResult.inspectDetail.minorIcInspects.MaxLength = _Result.At(i).At(5).At(11).T.TupleSelect(0).I;
                        itemResult.inspectDetail.minorIcInspects.MaxWidth = _Result.At(i).At(5).At(12).T.TupleSelect(0).I;
                        itemResult.inspectDetail.minorIcInspects.MaxArea = _Result.At(i).At(5).At(13).T.TupleSelect(0).I;
                    }

                    if (_Result.At(i).At(5).At(14).T.Length == 0)
                    {
                        itemResult.inspectDetail.chipInspects.Number = 0;
                        itemResult.inspectDetail.chipInspects.Length = 0;
                        itemResult.inspectDetail.chipInspects.Width = 0;
                        itemResult.inspectDetail.chipInspects.Area = 0;
                        itemResult.inspectDetail.chipInspects.MaxLength = 0;
                        itemResult.inspectDetail.chipInspects.MaxWidth = 0;
                        itemResult.inspectDetail.chipInspects.MaxArea = 0;
                    }
                    else
                    {
                        itemResult.inspectDetail.chipInspects.Number = _Result.At(i).At(5).At(14).T.TupleSelect(0).I;
                        itemResult.inspectDetail.chipInspects.Length = _Result.At(i).At(5).At(15).T;
                        itemResult.inspectDetail.chipInspects.Width = _Result.At(i).At(5).At(16).T;
                        itemResult.inspectDetail.chipInspects.Area = _Result.At(i).At(5).At(17).T;
                        itemResult.inspectDetail.chipInspects.MaxLength = _Result.At(i).At(5).At(18).T.TupleSelect(0).I;
                        itemResult.inspectDetail.chipInspects.MaxWidth = _Result.At(i).At(5).At(19).T.TupleSelect(0).I;
                        itemResult.inspectDetail.chipInspects.MaxArea = _Result.At(i).At(5).At(20).T.TupleSelect(0).I;
                    }

                    if (_Result.At(i).At(5).At(21).T.Length == 0)
                    {
                        itemResult.inspectDetail.frameInspects.Number = 0;
                        itemResult.inspectDetail.frameInspects.Length = 0;
                        itemResult.inspectDetail.frameInspects.Width = 0;
                        itemResult.inspectDetail.frameInspects.Area = 0;
                        itemResult.inspectDetail.frameInspects.MaxLength = 0;
                        itemResult.inspectDetail.frameInspects.MaxWidth = 0;
                        itemResult.inspectDetail.frameInspects.MaxArea = 0;
                    }
                    else
                    {
                        itemResult.inspectDetail.frameInspects.Number = _Result.At(i).At(5).At(21).T.TupleSelect(0).I;
                        itemResult.inspectDetail.frameInspects.Length = _Result.At(i).At(5).At(22).T;
                        itemResult.inspectDetail.frameInspects.Width = _Result.At(i).At(5).At(23).T;
                        itemResult.inspectDetail.frameInspects.Area = _Result.At(i).At(5).At(24).T;
                        itemResult.inspectDetail.frameInspects.MaxLength = _Result.At(i).At(5).At(25).T.TupleSelect(0).I;
                        itemResult.inspectDetail.frameInspects.MaxWidth = _Result.At(i).At(5).At(26).T.TupleSelect(0).I;
                        itemResult.inspectDetail.frameInspects.MaxArea = _Result.At(i).At(5).At(27).T.TupleSelect(0).I;
                    }

                    itemResult.inspectDetail.epoxyInspect.UpOut = _Result.At(i).At(5).At(28).T.TupleSelect(0).I == 1;
                    itemResult.inspectDetail.epoxyInspect.DownOut = _Result.At(i).At(5).At(29).T.TupleSelect(0).I == 1;
                    itemResult.inspectDetail.epoxyInspect.LeftOut = _Result.At(i).At(5).At(30).T.TupleSelect(0).I == 1;
                    itemResult.inspectDetail.epoxyInspect.RightOut = _Result.At(i).At(5).At(31).T.TupleSelect(0).I == 1;

                    itemResult.inspectDetail.icLoctionDiffs.RowDiff = _Result.At(i).At(5).At(32).T.TupleSelect(0).D;
                    itemResult.inspectDetail.icLoctionDiffs.ColDiff = _Result.At(i).At(5).At(33).T.TupleSelect(0).D;
                    itemResult.inspectDetail.icLoctionDiffs.DistanceDiff = _Result.At(i).At(5).At(34).T.TupleSelect(0).D;
                    itemResult.inspectDetail.icLoctionDiffs.AngleDiff = _Result.At(i).At(5).At(35).T.TupleSelect(0).D;

                    itemResult.inspectDetail.bondWiresInspects.Radius_FirstBond = _Result.At(i).At(5).At(36).T;
                    itemResult.inspectDetail.bondWiresInspects.Distance_WireBreak = _Result.At(i).At(5).At(37).T;
                    itemResult.inspectDetail.bondWiresInspects.Score_SecondBond = _Result.At(i).At(5).At(38).T;

                }


                Results.Add(itemResult);
            }
            return true;
        }
        #endregion
        #region 私有方法
        public void HDevelopStop()
        {
            MessageBox.Show("Press button to continue", "Program stop");
        }


        public void JSCC_AOI_inspect(HObjectVector/*{eObjectVector,Dim=1}*/ hvec_i_Images,
            HObjectVector/*{eObjectVector,Dim=1}*/ hvec_i_CoarseObjs, HObjectVector/*{eObjectVector,Dim=1}*/ hvec_i_MainIcObjs,
            HObjectVector/*{eObjectVector,Dim=1}*/ hvec_i_MainIcWireObjs, HObjectVector/*{eObjectVector,Dim=1}*/ hvec_i_MinorIcObjs,
            HObjectVector/*{eObjectVector,Dim=1}*/ hvec_i_FrameObjs, HObjectVector/*{eObjectVector,Dim=1}*/ hvec_i_BondWireObjs,
            HObject ho_i_CoarseReg, out HObject ho_o_FailRegs, out HObject ho_o_Wires, out HObjectVector/*{eObjectVector,Dim=1}*/ hvec_o_VWires,
            out HObjectVector/*{eObjectVector,Dim=1}*/ hvec_o_VFailRegs, HTupleVector/*{eTupleVector,Dim=1}*/ hvec_i_CoarseArgs,
            HTupleVector/*{eTupleVector,Dim=1}*/ hvec_i_MainIcArgs, HTupleVector/*{eTupleVector,Dim=1}*/ hvec_i_MainIcWireArgs,
            HTupleVector/*{eTupleVector,Dim=1}*/ hvec_i_MinorIcArgs, HTupleVector/*{eTupleVector,Dim=1}*/ hvec_i_FrameArgs,
            HTupleVector/*{eTupleVector,Dim=1}*/ hvec_i_BondWireArgs, HTupleVector/*{eTupleVector,Dim=1}*/ hvec_i_EpoxyArgs,
            HTuple hv_i_Row, HTuple hv_i_Col, out HTupleVector/*{eTupleVector,Dim=3}*/ hvec_o_Result,
            out HTuple hv_o_ErrCode, out HTuple hv_o_ErrString)
        {


            using (HDevThreadContext context = new HDevThreadContext())
            {
                // +++ Threading variables 
                HDevThread devThread;


                // Stack for temporary objects 
                HObject[] OTemp = new HObject[20];

                // Local iconic variables 

                HObject ho__MainIcDarkImgs = null, ho__MainIcLightImgs = null;
                HObject ho_ObjectSelected = null, ho__MatchRegUnit = null;

                // Local copy input parameter variables 

                HObjectVector hvec_i_MainIcObjs_COPY_INP_TMP = hvec_i_MainIcObjs.Clone();


                // Local control variables 

                HTuple hv_Number1 = null, hv__IndexToSelect = null;
                HTuple hv__UseMultiThread = null, hv_Number = null, hv_i = null;
                HTuple hv__Threads = null;

                HTupleVector hvec__VResult = new HTupleVector(1);
                HTupleVector hvec__DefectImgIdx = new HTupleVector(1), hvec__VErrCodes = new HTupleVector(1);
                HTupleVector hvec__VErrStrs = new HTupleVector(1), hvec__Value = new HTupleVector(2);
                HTupleVector hvec__VThreads = new HTupleVector(1);
                // Initialize local and output iconic variables 
                HOperatorSet.GenEmptyObj(out ho_o_FailRegs);
                HOperatorSet.GenEmptyObj(out ho_o_Wires);
                HOperatorSet.GenEmptyObj(out ho__MainIcDarkImgs);
                HOperatorSet.GenEmptyObj(out ho__MainIcLightImgs);
                HOperatorSet.GenEmptyObj(out ho_ObjectSelected);
                HOperatorSet.GenEmptyObj(out ho__MatchRegUnit);
                hvec_o_VWires = new HObjectVector(1);
                hvec_o_VFailRegs = new HObjectVector(1);
                try
                {
                    //***********************************
                    //******芯片脱离 ：=1
                    //******芯片偏移、堆叠 ：=2
                    //******芯片转角 ：=3
                    //******芯片反向 ：=4
                    //******错误芯片 ：=5
                    //******墨点芯片 ：=6
                    //******芯片银胶污染、异物 ：=7
                    //******芯片崩角 ：=8
                    //******银胶过多、过少 ：=9
                    //******金球大小异常 ：=10
                    //******金球偏移 ：=11
                    //******断线 ：=12
                    //******塌陷、弯曲 ：=13
                    //******第二焊点脱落 ：=14
                    //******第二焊点偏移 ：=15
                    //******钉架、内引脚变形 ：=16
                    //******钉架银胶污染 ：=17
                    //******框架氧化 ：=18
                    //******框架镀层异常 ：=19
                    //******配线错误 ：=20
                    //******混料 ：=21
                    //******第一焊点剥离 ：=22
                    //******2ND鱼尾缺口 ：=23
                    //******尾丝长 ：=24
                    //******双丝 ：=25
                    //******散热片划伤 ：=26
                    //******框架变形 ：=27

                    //**********************
                    //检测o_Value
                    //0:mainIC 缺陷 个数、长、宽、面积、最大长、最大宽、最大面积  没有缺陷则为[]       错误1 6 7
                    //1:minorIC缺陷 个数、长、宽、面积、最大长、最大宽、最大面积
                    //2:chip   缺陷 个数、长、宽、面积、最大长、最大宽、最大面积                           8
                    //3:Frame  缺陷 个数、长、宽、面积、最大长、最大宽、最大面积                           18
                    //4:银胶    上 下 左 右 1 1 1 1  没有则为0                                          9
                    //5：:ic  偏移行 偏移列 偏移欧式距离 偏移角度                                         2
                    //6： 焊点的半径若没有找到则为-1                                                     10 11 22
                    //   金线的最大断线距离 若没有金线则为-1                                             12 13 20
                    //   第二焊点的匹配分数 一般小于0.6认为第二焊点脱落                                 14  15

                    //* 检测结果的数据结构定义
                    //o_Result:{[A], [B], [C], [D, E.....]}
                    //A:-1:表示检测出来NG, 1:表示检测出来OK, 0-表示此位置本来就没有（非缺失），比如每个视野拍两个，一共只有三列芯片，那么第二次拍照必然会少一个
                    //B,C:表示芯片在整个板子(非block)中的实际位置，
                    //D.....后面是实际的缺陷类型，可能有多个，也可能只有一个

                    hv_o_ErrCode = 0;
                    hv_o_ErrString = "";
                    ho_o_FailRegs.Dispose();
                    HOperatorSet.GenEmptyObj(out ho_o_FailRegs);
                    ho_o_Wires.Dispose();
                    HOperatorSet.GenEmptyObj(out ho_o_Wires);
                    hvec_o_Result = new HTupleVector(3);
                    //

                    //*****************************************************************************************
                    ho__MainIcDarkImgs.Dispose();
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        ho__MainIcDarkImgs = hvec_i_MainIcObjs_COPY_INP_TMP[4].O.CopyObj(1, -1);
                    }
                    ho__MainIcLightImgs.Dispose();
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        ho__MainIcLightImgs = hvec_i_MainIcObjs_COPY_INP_TMP[5].O.CopyObj(1, -1);
                    }
                    HOperatorSet.CountObj(ho__MainIcDarkImgs, out hv_Number1);
                    hv__IndexToSelect = 1;
                    hv__UseMultiThread = 0;
                    HOperatorSet.CountObj(ho_i_CoarseReg, out hv_Number);
                    HTuple end_val61 = hv_Number - 1;
                    HTuple step_val61 = 1;
                    for (hv_i = 0; hv_i.Continue(end_val61, step_val61); hv_i = hv_i.TupleAdd(step_val61))
                    {
                        if ((int)(new HTuple(hv_Number1.TupleEqual(hv_Number))) != 0)
                        {
                            hv__IndexToSelect = hv_i + 1;
                        }
                        ho_ObjectSelected.Dispose();
                        HOperatorSet.SelectObj(ho__MainIcDarkImgs, out ho_ObjectSelected, hv__IndexToSelect);
                        hvec_i_MainIcObjs_COPY_INP_TMP[4] = new HObjectVector((ho_ObjectSelected).CopyObj(1, -1));
                        ho_ObjectSelected.Dispose();
                        HOperatorSet.SelectObj(ho__MainIcLightImgs, out ho_ObjectSelected, hv__IndexToSelect);
                        ho__MatchRegUnit.Dispose();
                        HOperatorSet.SelectObj(ho_i_CoarseReg, out ho__MatchRegUnit, hv_i + 1);
                        if ((int)(hv__UseMultiThread) != 0)
                        {
                            devThread = new HDevThread(context,
                              (HDevThread.ProcCallback)delegate(HDevThread devThreadCB)
                              {
                                  try
                                  {
                                      // Input parameters
                                      HObjectVector cbhvec_i_Images = devThreadCB.GetInputIconicParamVector(0);
                                      HObject cbho_i_CoarseReg = devThreadCB.GetInputIconicParamObject(1);
                                      HObjectVector cbhvec_i_MainIcObj = devThreadCB.GetInputIconicParamVector(2);
                                      HObjectVector cbhvec_i_MainIcWireObj = devThreadCB.GetInputIconicParamVector(3);
                                      HObjectVector cbhvec_i_MinorIcObj = devThreadCB.GetInputIconicParamVector(4);
                                      HObjectVector cbhvec_i_FrameObj = devThreadCB.GetInputIconicParamVector(5);
                                      HObjectVector cbhvec_i_BondWireObj = devThreadCB.GetInputIconicParamVector(6);
                                      HTupleVector cbhvec_i_CoarseArgs = devThreadCB.GetInputCtrlParamVector(7);
                                      HTupleVector cbhvec_i_MainIcArgs = devThreadCB.GetInputCtrlParamVector(8);
                                      HTupleVector cbhvec_i_MainIcWireArgs = devThreadCB.GetInputCtrlParamVector(9);
                                      HTupleVector cbhvec_i_MinorIcArgs = devThreadCB.GetInputCtrlParamVector(10);
                                      HTupleVector cbhvec_i_FrameArgs = devThreadCB.GetInputCtrlParamVector(11);
                                      HTupleVector cbhvec_i_BondWireArgs = devThreadCB.GetInputCtrlParamVector(12);
                                      HTupleVector cbhvec_i_EpoxyArgs = devThreadCB.GetInputCtrlParamVector(13);

                                      // Output parameters
                                      HObject cbho_o_FailRegs;
                                      HObject cbho_o_Wires;
                                      HTuple cbhv_o_DefectType;
                                      HTuple cbhv_o_DefectImgIdx;
                                      HTuple cbhv_o_ErrCode;
                                      HTuple cbhv_o_ErrStr;
                                      HTupleVector cbhvec_o_Value;

                                      // Call JSCC_AOI_inspect_unit
                                      JSCC_AOI_inspect_unit(cbhvec_i_Images, cbho_i_CoarseReg, cbhvec_i_MainIcObj,
                                                    cbhvec_i_MainIcWireObj, cbhvec_i_MinorIcObj, cbhvec_i_FrameObj,
                                                    cbhvec_i_BondWireObj, out cbho_o_FailRegs, out cbho_o_Wires,
                                                    cbhvec_i_CoarseArgs, cbhvec_i_MainIcArgs, cbhvec_i_MainIcWireArgs,
                                                    cbhvec_i_MinorIcArgs, cbhvec_i_FrameArgs, cbhvec_i_BondWireArgs,
                                                    cbhvec_i_EpoxyArgs, out cbhv_o_DefectType, out cbhv_o_DefectImgIdx,
                                                    out cbhv_o_ErrCode, out cbhv_o_ErrStr, out cbhvec_o_Value);

                                      // Store output parameters in thread object
                                      devThreadCB.StoreOutputIconicParamObject(0, cbho_o_FailRegs);
                                      devThreadCB.StoreOutputIconicParamObject(1, cbho_o_Wires);
                                      devThreadCB.StoreOutputCtrlParamTuple(2, cbhv_o_DefectType);
                                      devThreadCB.StoreOutputCtrlParamTuple(3, cbhv_o_DefectImgIdx);
                                      devThreadCB.StoreOutputCtrlParamTuple(4, cbhv_o_ErrCode);
                                      devThreadCB.StoreOutputCtrlParamTuple(5, cbhv_o_ErrStr);
                                      devThreadCB.StoreOutputCtrlParamVector(6, cbhvec_o_Value);

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
                              }, 14, 7);
                            // Set thread procedure call arguments 
                            devThread.SetInputIconicParamVector(0, hvec_i_Images);
                            devThread.SetInputIconicParamObject(1, ho__MatchRegUnit);
                            devThread.SetInputIconicParamVector(2, hvec_i_MainIcObjs_COPY_INP_TMP);
                            devThread.SetInputIconicParamVector(3, hvec_i_MainIcWireObjs);
                            devThread.SetInputIconicParamVector(4, hvec_i_MinorIcObjs);
                            devThread.SetInputIconicParamVector(5, hvec_i_FrameObjs);
                            devThread.SetInputIconicParamVector(6, hvec_i_BondWireObjs);
                            devThread.SetInputCtrlParamVector(7, hvec_i_CoarseArgs);
                            devThread.SetInputCtrlParamVector(8, hvec_i_MainIcArgs);
                            devThread.SetInputCtrlParamVector(9, hvec_i_MainIcWireArgs);
                            devThread.SetInputCtrlParamVector(10, hvec_i_MinorIcArgs);
                            devThread.SetInputCtrlParamVector(11, hvec_i_FrameArgs);
                            devThread.SetInputCtrlParamVector(12, hvec_i_BondWireArgs);
                            devThread.SetInputCtrlParamVector(13, hvec_i_EpoxyArgs);
                            {
                                HTuple at_idx = new HTuple();
                                at_idx[0] = hv_i;
                                devThread.BindOutputIconicParamVector(0, false, hvec_o_VFailRegs, at_idx);
                            }
                            {
                                HTuple at_idx = new HTuple();
                                at_idx[0] = hv_i;
                                devThread.BindOutputIconicParamVector(1, false, hvec_o_VWires, at_idx);
                            }
                            {
                                HTuple at_idx = new HTuple();
                                at_idx[0] = hv_i;
                                devThread.BindOutputCtrlParamVector(2, false, hvec__VResult, at_idx);
                            }
                            {
                                HTuple at_idx = new HTuple();
                                at_idx[0] = hv_i;
                                devThread.BindOutputCtrlParamVector(3, false, hvec__DefectImgIdx, at_idx);
                            }
                            {
                                HTuple at_idx = new HTuple();
                                at_idx[0] = hv_i;
                                devThread.BindOutputCtrlParamVector(4, false, hvec__VErrCodes, at_idx);
                            }
                            {
                                HTuple at_idx = new HTuple();
                                at_idx[0] = hv_i;
                                devThread.BindOutputCtrlParamVector(5, false, hvec__VErrStrs, at_idx);
                            }
                            {
                                HTuple at_idx = new HTuple();
                                at_idx[0] = hv_i;
                                devThread.BindOutputCtrlParamVector(6, false, hvec__Value, at_idx);
                            }

                            // Start proc line in thread
                            {
                                HTuple TmpThreadId;
                                devThread.ParStart(out TmpThreadId);
                                hvec__VThreads[hv_i].T = TmpThreadId;
                            }

                        }
                        else
                        {
                            hvec__VThreads[hv_i] = new HTupleVector(hv_i).Clone();
                            {
                                HObject ExpTmpOutVar_0; HObject ExpTmpOutVar_1; HTuple ExpTmpOutVar_2; HTuple ExpTmpOutVar_3;
                                HTuple ExpTmpOutVar_4; HTuple ExpTmpOutVar_5; HTupleVector ExpTmpOutVar_6 = new HTupleVector(1);
                                JSCC_AOI_inspect_unit(hvec_i_Images, ho__MatchRegUnit, hvec_i_MainIcObjs_COPY_INP_TMP,
                                    hvec_i_MainIcWireObjs, hvec_i_MinorIcObjs, hvec_i_FrameObjs, hvec_i_BondWireObjs,
                                    out ExpTmpOutVar_0, out ExpTmpOutVar_1, hvec_i_CoarseArgs, hvec_i_MainIcArgs,
                                    hvec_i_MainIcWireArgs, hvec_i_MinorIcArgs, hvec_i_FrameArgs, hvec_i_BondWireArgs,
                                    hvec_i_EpoxyArgs, out ExpTmpOutVar_2, out ExpTmpOutVar_3, out ExpTmpOutVar_4,
                                    out ExpTmpOutVar_5, out ExpTmpOutVar_6);
                                hvec_o_VFailRegs[hv_i].O = ExpTmpOutVar_0;
                                ExpTmpOutVar_0.Dispose();
                                hvec_o_VWires[hv_i].O = ExpTmpOutVar_1;
                                ExpTmpOutVar_1.Dispose();
                                hvec__VResult[hv_i].T = ExpTmpOutVar_2;
                                hvec__DefectImgIdx[hv_i].T = ExpTmpOutVar_3;
                                hvec__VErrCodes[hv_i].T = ExpTmpOutVar_4;
                                hvec__VErrStrs[hv_i].T = ExpTmpOutVar_5;
                                hvec__Value[hv_i] = ExpTmpOutVar_6;
                            }
                        }
                    }
                    hv__Threads = hvec__VThreads.ConvertVectorToTuple();
                    if ((int)(hv__UseMultiThread) != 0)
                    {
                        HDevThread.ParJoin(hv__Threads);
                    }
                    //对结果进行整合
                    HTuple end_val81 = hv_Number - 1;
                    HTuple step_val81 = 1;
                    for (hv_i = 0; hv_i.Continue(end_val81, step_val81); hv_i = hv_i.TupleAdd(step_val81))
                    {
                        if ((int)(new HTuple((hvec__VThreads[hv_i].T).TupleEqual(new HTuple()))) != 0)
                        {
                            hvec_o_Result[hv_i][0] = (new HTupleVector(1).Insert(0, new HTupleVector(new HTuple(0))));
                            continue;
                        }
                        if ((int)(new HTuple((hvec__VErrCodes[hv_i].T).TupleLess(0))) != 0)
                        {
                            hv_o_ErrCode = hvec__VErrCodes[hv_i].T.Clone();
                            hv_o_ErrString = (("Error occurs at i=" + hv_i) + ":") + hvec__VErrStrs[hv_i].T;
                            HDevelopStop();
                        }
                        using (HDevDisposeHelper dh = new HDevDisposeHelper())
                        {
                            HObject ExpTmpOutVar_0;
                            HOperatorSet.ConcatObj(ho_o_FailRegs, hvec_o_VFailRegs[hv_i].O, out ExpTmpOutVar_0
                                );
                            ho_o_FailRegs.Dispose();
                            ho_o_FailRegs = ExpTmpOutVar_0;
                        }
                        using (HDevDisposeHelper dh = new HDevDisposeHelper())
                        {
                            HObject ExpTmpOutVar_0;
                            HOperatorSet.ConcatObj(ho_o_Wires, hvec_o_VWires[hv_i].O, out ExpTmpOutVar_0
                                );
                            ho_o_Wires.Dispose();
                            ho_o_Wires = ExpTmpOutVar_0;
                        }
                        if ((int)(new HTuple((hvec__VResult[hv_i].T).TupleEqual(new HTuple()))) != 0)
                        {
                            hvec_o_Result[hv_i] = ((((((new HTupleVector(2).Insert(0, (new HTupleVector(1).Insert(0, new HTupleVector(new HTuple(1)))))).Insert(
                                1, (new HTupleVector(1).Insert(0, new HTupleVector(new HTuple(hv_i_Row.TupleSelect(
                                hv_i))))))).Insert(2, (new HTupleVector(1).Insert(0, new HTupleVector(new HTuple(hv_i_Col.TupleSelect(
                                hv_i))))))).Insert(3, new HTupleVector(1))).Insert(4, new HTupleVector(1))).Insert(
                                5, hvec__Value[hv_i]));
                        }
                        else
                        {
                            hvec_o_Result[hv_i] = ((((((new HTupleVector(2).Insert(0, (new HTupleVector(1).Insert(0, new HTupleVector(new HTuple(-1)))))).Insert(
                                1, (new HTupleVector(1).Insert(0, new HTupleVector(new HTuple(hv_i_Row.TupleSelect(
                                hv_i))))))).Insert(2, (new HTupleVector(1).Insert(0, new HTupleVector(new HTuple(hv_i_Col.TupleSelect(
                                hv_i))))))).Insert(3, (new HTupleVector(1).Insert(0, new HTupleVector(hvec__VResult[hv_i]))))).Insert(
                                4, (new HTupleVector(1).Insert(0, new HTupleVector(hvec__DefectImgIdx[hv_i]))))).Insert(
                                5, hvec__Value[hv_i]));
                        }
                    }
                    ho__MainIcDarkImgs.Dispose();
                    ho__MainIcLightImgs.Dispose();
                    ho_ObjectSelected.Dispose();
                    ho__MatchRegUnit.Dispose();

                    return;
                }
                catch (HalconException HDevExpDefaultException)
                {
                    ho__MainIcDarkImgs.Dispose();
                    ho__MainIcLightImgs.Dispose();
                    ho_ObjectSelected.Dispose();
                    ho__MatchRegUnit.Dispose();

                    throw HDevExpDefaultException;
                }
            }
        }
        public void JSCC_AOI_inspect_unit(HObjectVector/*{eObjectVector,Dim=1}*/ hvec_i_Images,
      HObject ho_i_CoarseReg, HObjectVector/*{eObjectVector,Dim=1}*/ hvec_i_MainIcObj,
      HObjectVector/*{eObjectVector,Dim=1}*/ hvec_i_MainIcWireObj, HObjectVector/*{eObjectVector,Dim=1}*/ hvec_i_MinorIcObj,
      HObjectVector/*{eObjectVector,Dim=1}*/ hvec_i_FrameObj, HObjectVector/*{eObjectVector,Dim=1}*/ hvec_i_BondWireObj,
      out HObject ho_o_FailRegs, out HObject ho_o_Wires, HTupleVector/*{eTupleVector,Dim=1}*/ hvec_i_CoarseArgs,
      HTupleVector/*{eTupleVector,Dim=1}*/ hvec_i_MainIcArgs, HTupleVector/*{eTupleVector,Dim=1}*/ hvec_i_MainIcWireArgs,
      HTupleVector/*{eTupleVector,Dim=1}*/ hvec_i_MinorIcArgs, HTupleVector/*{eTupleVector,Dim=1}*/ hvec_i_FrameArgs,
      HTupleVector/*{eTupleVector,Dim=1}*/ hvec_i_BondWireArgs, HTupleVector/*{eTupleVector,Dim=1}*/ hvec_i_EpoxyArgs,
      out HTuple hv_o_DefectType, out HTuple hv_o_DefectImgIdx, out HTuple hv_o_ErrCode,
      out HTuple hv_o_ErrStr, out HTupleVector/*{eTupleVector,Dim=1}*/ hvec_o_Value)
        {




            // Stack for temporary objects 
            HObject[] OTemp = new HObject[20];

            // Local iconic variables 

            HObject ho__MatchReg = null, ho__InspectReg = null;
            HObject ho__SubRegs = null, ho__RejectReg = null, ho__ImgDark = null;
            HObject ho__ImgLight = null, ho__NewMatchReg, ho_RegionDilation = null;
            HObject ho__MainIcFailRegs = null, ho__ChipReg = null, ho__NewInspectReg = null;
            HObject ho__MainIcWireFailRegs = null, ho__MainIcWireChipReg = null;
            HObject ho__MinorChipReg = null, ho__MinorIcFailRegs = null;
            HObject ho__FrameMatchReg = null, ho__FrameInspectReg = null;
            HObject ho__FrameSubRegs = null, ho__FrameRejectReg = null;
            HObject ho__FrameImgDark = null, ho__FrameImgLight = null, ho__NewFrameMatchReg = null;
            HObject ho__NewFrameSubRegs = null, ho__FrameAllFailRegs = null;
            HObject ho__FrameFailRegs = null, ho__DefectRegion, ho_RegionAffineTrans = null;
            HObject ho__Bond1Regs = null, ho__Bond2Regs = null, ho__NewBond1Regs;
            HObject ho__NewBond2Regs, ho__reg = null, ho__reg1 = null, ho__FailRegs;
            HObject ho__Wires, ho__Bond1Balls, ho__Bond2Balls, ho__WireRes;
            HObject ho_ConnectedRegions, ho_RegionUnion = null, ho__MinorFailRegs = null;
            HObject ho__FrameConRegions = null, ho_FrameRegs = null;

            // Local control variables 

            HTuple hv__Model = null, hv__ID = null, hv__Type = null;
            HTuple hv__ImgIdx = null, hv__Para = null, hv__DilationSize = null;
            HTuple hv__AngleStart = null, hv__AngleExt = null, hv__MinScore = null;
            HTuple hv_o_Row = null, hv_o_Col = null, hv_o_Angle = null;
            HTuple hv__ErrCode = null, hv__ErrString = null, hv__HomMatModel2Img = null;
            HTuple hv__FindIcImgIdx = null, hv__CloseSize = null, hv__MainIcMinWidth = null;
            HTuple hv__MainIcMinHeight = null, hv__MainIcMinArea = null;
            HTuple hv__MainIcSelectOperate = null, hv__MainIcRowThr = null;
            HTuple hv__MainIcColThr = null, hv__MainIcAngleThr = null;
            HTuple hv__MainIcRow = new HTuple(), hv__MainIcColumn = new HTuple();
            HTuple hv__MainIcAngle = new HTuple(), hv__num = new HTuple();
            HTuple hv_o_score = new HTuple(), hv__MainIcHomModel2Img = new HTuple();
            HTuple hv__MainIcWireMinWidth = null, hv__MainIcWireMinHeight = null;
            HTuple hv__MainIcWireMinArea = null, hv__MainIcWireSelectOperate = null;
            HTuple hv__MainIcWireHomModel2Img = new HTuple(), hv__MinorIcMinWidth = new HTuple();
            HTuple hv__MinorIcMinHeight = new HTuple(), hv__MinorIcMinArea = new HTuple();
            HTuple hv__MinorIcSelectOperate = new HTuple(), hv__MinorIcRowThr = new HTuple();
            HTuple hv__MinorIcColThr = new HTuple(), hv__MinorIcAngleThr = new HTuple();
            HTuple hv_o_MinorRow = new HTuple(), hv_o_MinorColumn = new HTuple();
            HTuple hv_o_MinorAngle = new HTuple(), hv__MinorIcHomModel2Img = new HTuple();
            HTuple hv__FrameModel = new HTuple(), hv__FrameID = new HTuple();
            HTuple hv__FrameType = new HTuple(), hv__FrameImgIdx = new HTuple();
            HTuple hv__FrameFindIcImgIdx = new HTuple(), hv__FrameDilationSize = new HTuple();
            HTuple hv__FrameAngleStart = new HTuple(), hv__FrameAngleExt = new HTuple();
            HTuple hv__FrameMinScore = new HTuple(), hv__FrameCloseSize = new HTuple();
            HTuple hv__FrameMinWidth = new HTuple(), hv__FrameMinHeight = new HTuple();
            HTuple hv__FrameMinArea = new HTuple(), hv__FrameSelectOperate = new HTuple();
            HTuple hv__FrameHomModel2Img = new HTuple(), hv__FrameRow = new HTuple();
            HTuple hv__FrameColumn = new HTuple(), hv__FrameAngle = new HTuple();
            HTuple hv__EpoxyInspectSize = null, hv__EpoxyDarkLight = null;
            HTuple hv__EpoxyEdgeSigma = null, hv__EpoxyEdgeThresh = null;
            HTuple hv__EpoxyDistThresh = null, hv_o_EpoxyResult = null;
            HTuple hv_RowDiff = new HTuple(), hv_ColDiff = new HTuple();
            HTuple hv_AngleDiff = new HTuple(), hv_Distance = new HTuple();
            HTuple hv__Bond1OnIc = null, hv__Bond1ImgIdx = null, hv__Bond2OnIc = null;
            HTuple hv__Bond2ImgIdx = null, hv__Bond2BallNum = null;
            HTuple hv__WireImgIdx = null, hv__Paras = null, hv__MinBall1Rad = null;
            HTuple hv__MaxBall1Rad = null, hv__SearchLen = null, hv__ClipLen = null;
            HTuple hv__WireWidth = null, hv__WireContrast = null, hv__MinSegLen = null;
            HTuple hv__WireAngleExt = null, hv__MaxWireGap = null;
            HTuple hv_Number1 = null, hv_i = null, hv_Index = new HTuple();
            HTuple hv_Number = null;

            HTupleVector hvec_o_bondwire_Value = new HTupleVector(1);
            HTupleVector hvec_TempValue = new HTupleVector(1);
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_o_FailRegs);
            HOperatorSet.GenEmptyObj(out ho_o_Wires);
            HOperatorSet.GenEmptyObj(out ho__MatchReg);
            HOperatorSet.GenEmptyObj(out ho__InspectReg);
            HOperatorSet.GenEmptyObj(out ho__SubRegs);
            HOperatorSet.GenEmptyObj(out ho__RejectReg);
            HOperatorSet.GenEmptyObj(out ho__ImgDark);
            HOperatorSet.GenEmptyObj(out ho__ImgLight);
            HOperatorSet.GenEmptyObj(out ho__NewMatchReg);
            HOperatorSet.GenEmptyObj(out ho_RegionDilation);
            HOperatorSet.GenEmptyObj(out ho__MainIcFailRegs);
            HOperatorSet.GenEmptyObj(out ho__ChipReg);
            HOperatorSet.GenEmptyObj(out ho__NewInspectReg);
            HOperatorSet.GenEmptyObj(out ho__MainIcWireFailRegs);
            HOperatorSet.GenEmptyObj(out ho__MainIcWireChipReg);
            HOperatorSet.GenEmptyObj(out ho__MinorChipReg);
            HOperatorSet.GenEmptyObj(out ho__MinorIcFailRegs);
            HOperatorSet.GenEmptyObj(out ho__FrameMatchReg);
            HOperatorSet.GenEmptyObj(out ho__FrameInspectReg);
            HOperatorSet.GenEmptyObj(out ho__FrameSubRegs);
            HOperatorSet.GenEmptyObj(out ho__FrameRejectReg);
            HOperatorSet.GenEmptyObj(out ho__FrameImgDark);
            HOperatorSet.GenEmptyObj(out ho__FrameImgLight);
            HOperatorSet.GenEmptyObj(out ho__NewFrameMatchReg);
            HOperatorSet.GenEmptyObj(out ho__NewFrameSubRegs);
            HOperatorSet.GenEmptyObj(out ho__FrameAllFailRegs);
            HOperatorSet.GenEmptyObj(out ho__FrameFailRegs);
            HOperatorSet.GenEmptyObj(out ho__DefectRegion);
            HOperatorSet.GenEmptyObj(out ho_RegionAffineTrans);
            HOperatorSet.GenEmptyObj(out ho__Bond1Regs);
            HOperatorSet.GenEmptyObj(out ho__Bond2Regs);
            HOperatorSet.GenEmptyObj(out ho__NewBond1Regs);
            HOperatorSet.GenEmptyObj(out ho__NewBond2Regs);
            HOperatorSet.GenEmptyObj(out ho__reg);
            HOperatorSet.GenEmptyObj(out ho__reg1);
            HOperatorSet.GenEmptyObj(out ho__FailRegs);
            HOperatorSet.GenEmptyObj(out ho__Wires);
            HOperatorSet.GenEmptyObj(out ho__Bond1Balls);
            HOperatorSet.GenEmptyObj(out ho__Bond2Balls);
            HOperatorSet.GenEmptyObj(out ho__WireRes);
            HOperatorSet.GenEmptyObj(out ho_ConnectedRegions);
            HOperatorSet.GenEmptyObj(out ho_RegionUnion);
            HOperatorSet.GenEmptyObj(out ho__MinorFailRegs);
            HOperatorSet.GenEmptyObj(out ho__FrameConRegions);
            HOperatorSet.GenEmptyObj(out ho_FrameRegs);
            try
            {
                hv_o_ErrCode = 0;
                hv_o_ErrStr = "";
                hv_o_DefectType = new HTuple();
                hv_o_DefectImgIdx = new HTuple();
                //定义 输出结果
                hvec_o_Value = new HTupleVector(1);

                //0:mainIC 缺陷  个数 长[]、宽[]、面积[]、最大长、最大宽、最大面积  没有缺陷则为0
                //1:minorIC缺陷平均长、平均宽、平均面积、最大长、最大宽、最大面积
                //2:chip   缺陷平均长、平均宽、平均面积、最大宽、最大面积
                //3:Frame  缺陷平均长、平均宽、平均面积、最大宽、最大面积             27
                //4:银胶    上 下 左 右 [1,1,1,1]  没有则为0
                //5：:ic  偏移欧式距离   偏移角度
                //6： 焊点的半径若没有找到则为-1
                //   金线的最大断线距离 若没有金线则为-1
                //   第二焊点的匹配分数 一般小于0.6认为第二焊点脱落
                ho_o_FailRegs.Dispose();
                HOperatorSet.GenEmptyObj(out ho_o_FailRegs);
                ho_o_Wires.Dispose();
                HOperatorSet.GenEmptyObj(out ho_o_Wires);
                //---------------1. 粗匹配

                hv__Model = hvec_i_CoarseArgs[0].T.Clone();
                hv__ID = hv__Model[0];
                hv__Type = hv__Model[1];
                hv__ImgIdx = hv__Model[2];
                hv__Para = hvec_i_CoarseArgs[1].T.Clone();
                hv__DilationSize = hv__Para[0];
                hv__AngleStart = hv__Para[1];
                hv__AngleExt = hv__Para[2];
                hv__MinScore = hv__Para[3];
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    HTV.HTV_find_coarse_model(hvec_i_Images[hv__ImgIdx].O, ho_i_CoarseReg, hv__ID,
                        hv__Type, hv__DilationSize, hv__AngleStart, hv__AngleExt, hv__MinScore,
                        out hv_o_Row, out hv_o_Col, out hv_o_Angle, out hv__ErrCode, out hv__ErrString,
                        out hv__HomMatModel2Img);
                }

                if ((int)(new HTuple(hv__ErrCode.TupleNotEqual(0))) != 0)
                {
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.ConcatObj(ho_o_FailRegs, ho_i_CoarseReg, out ExpTmpOutVar_0);
                        ho_o_FailRegs.Dispose();
                        ho_o_FailRegs = ExpTmpOutVar_0;
                    }
                    hv_o_ErrCode = hv__ErrCode.Clone();
                    if ((int)(new HTuple(hv__ErrCode.TupleLess(0))) != 0)
                    {
                        hv_o_ErrStr = "Coarse match failed: " + hv__ErrString;
                    }
                    else if ((int)(new HTuple(hv__ErrCode.TupleEqual(1))) != 0)
                    {
                        hv_o_ErrStr = "Find no product object!";
                        hv_o_DefectType = hv_o_DefectType.TupleConcat(18);
                        hv_o_DefectImgIdx = hv_o_DefectImgIdx.TupleConcat(2);
                    }
                    ho__MatchReg.Dispose();
                    ho__InspectReg.Dispose();
                    ho__SubRegs.Dispose();
                    ho__RejectReg.Dispose();
                    ho__ImgDark.Dispose();
                    ho__ImgLight.Dispose();
                    ho__NewMatchReg.Dispose();
                    ho_RegionDilation.Dispose();
                    ho__MainIcFailRegs.Dispose();
                    ho__ChipReg.Dispose();
                    ho__NewInspectReg.Dispose();
                    ho__MainIcWireFailRegs.Dispose();
                    ho__MainIcWireChipReg.Dispose();
                    ho__MinorChipReg.Dispose();
                    ho__MinorIcFailRegs.Dispose();
                    ho__FrameMatchReg.Dispose();
                    ho__FrameInspectReg.Dispose();
                    ho__FrameSubRegs.Dispose();
                    ho__FrameRejectReg.Dispose();
                    ho__FrameImgDark.Dispose();
                    ho__FrameImgLight.Dispose();
                    ho__NewFrameMatchReg.Dispose();
                    ho__NewFrameSubRegs.Dispose();
                    ho__FrameAllFailRegs.Dispose();
                    ho__FrameFailRegs.Dispose();
                    ho__DefectRegion.Dispose();
                    ho_RegionAffineTrans.Dispose();
                    ho__Bond1Regs.Dispose();
                    ho__Bond2Regs.Dispose();
                    ho__NewBond1Regs.Dispose();
                    ho__NewBond2Regs.Dispose();
                    ho__reg.Dispose();
                    ho__reg1.Dispose();
                    ho__FailRegs.Dispose();
                    ho__Wires.Dispose();
                    ho__Bond1Balls.Dispose();
                    ho__Bond2Balls.Dispose();
                    ho__WireRes.Dispose();
                    ho_ConnectedRegions.Dispose();
                    ho_RegionUnion.Dispose();
                    ho__MinorFailRegs.Dispose();
                    ho__FrameConRegions.Dispose();
                    ho_FrameRegs.Dispose();

                    return;
                }



                //---------------2. 主IC检测
                ho__MatchReg.Dispose();
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    ho__MatchReg = hvec_i_MainIcObj[0].O.CopyObj(1, -1);
                }
                ho__InspectReg.Dispose();
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    ho__InspectReg = hvec_i_MainIcObj[1].O.CopyObj(1, -1);
                }
                ho__SubRegs.Dispose();
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    ho__SubRegs = hvec_i_MainIcObj[3].O.CopyObj(1, -1);
                }
                ho__RejectReg.Dispose();
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    ho__RejectReg = hvec_i_MainIcObj[2].O.CopyObj(1, -1);
                }
                ho__ImgDark.Dispose();
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    ho__ImgDark = hvec_i_MainIcObj[4].O.CopyObj(1, -1);
                }
                ho__ImgLight.Dispose();
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    ho__ImgLight = hvec_i_MainIcObj[5].O.CopyObj(1, -1);
                }
                //
                hv__Model = hvec_i_MainIcArgs[0].T.Clone();
                hv__ID = hv__Model[0];
                hv__Type = hv__Model[1];
                hv__ImgIdx = hv__Model[2];
                hv__FindIcImgIdx = hv__Model[3];
                hv__DilationSize = hvec_i_MainIcArgs[1].T.Clone();
                hv__AngleStart = hvec_i_MainIcArgs[2].T.Clone();
                hv__AngleExt = hvec_i_MainIcArgs[3].T.Clone();
                hv__MinScore = hvec_i_MainIcArgs[4].T.Clone();
                hv__CloseSize = hvec_i_MainIcArgs[5].T.Clone();
                hv__MainIcMinWidth = hvec_i_MainIcArgs[6].T.Clone();
                hv__MainIcMinHeight = hvec_i_MainIcArgs[7].T.Clone();
                hv__MainIcMinArea = hvec_i_MainIcArgs[8].T.Clone();
                hv__MainIcSelectOperate = hvec_i_MainIcArgs[9].T.Clone();
                //
                hv__MainIcRowThr = hvec_i_MainIcArgs[10].T.Clone();
                hv__MainIcColThr = hvec_i_MainIcArgs[11].T.Clone();
                hv__MainIcAngleThr = hvec_i_MainIcArgs[12].T.Clone();


                ho__NewMatchReg.Dispose();
                HOperatorSet.AffineTransRegion(ho__MatchReg, out ho__NewMatchReg, hv__HomMatModel2Img,
                    "nearest_neighbor");

                //首先需要判断有无芯片 没有芯片输出3 接下来黄金模板匹配
                HObject ho__RectInspectReg;
                HOperatorSet.AffineTransRegion(ho__InspectReg, out ho__RectInspectReg, hv__HomMatModel2Img,
                    "nearest_neighbor");

                //HTV.HTV_inspect_nodie(hvec_i_Images, ho__RectInspectReg, hv__ImgIdx, out hv__ErrCode);

                if ((int)(new HTuple(hv__ErrCode.TupleNotEqual(3))) != 0)
                {
                    ho_RegionDilation.Dispose();
                    HTV.HTV_match_region_dilation(ho__NewMatchReg, out ho_RegionDilation, hv__DilationSize);
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        HTV.HTV_find_model(hvec_i_Images[hv__FindIcImgIdx].O, ho_RegionDilation, hv__ID,
                            hv__Type, hv__AngleStart, hv__AngleExt, hv__MinScore, 1, out hv__MainIcRow,
                            out hv__MainIcColumn, out hv__MainIcAngle, out hv__num, out hv_o_score);
                    }
                    if ((int)(new HTuple(hv__num.TupleEqual(0))) != 0)
                    {
                        hv__ErrCode = 1;
                    }
                    else
                    {
                        ho__MainIcFailRegs.Dispose(); ho__ChipReg.Dispose();
                        HTV.HTV_inspect_golden_model(hvec_i_Images, ho__NewMatchReg, ho__InspectReg,
                            ho__RejectReg, ho__SubRegs, ho__ImgDark, ho__ImgLight, ho_i_CoarseReg,
                            out ho__MainIcFailRegs, out ho__ChipReg, hv__MainIcRow, hv__MainIcColumn,
                            hv__MainIcAngle, hv__ImgIdx, hv__DilationSize, hv__AngleStart, hv__AngleExt,
                            hv__MinScore, hv__CloseSize, hv__MainIcMinWidth, hv__MainIcMinHeight,
                            hv__MainIcMinArea, hv__MainIcSelectOperate, out hv__MainIcHomModel2Img,
                            out hv__ErrCode, out hv__ErrString);
                    }
                }

                if ((int)((new HTuple(hv__ErrCode.TupleEqual(1))).TupleOr(new HTuple(hv__ErrCode.TupleEqual(
                    3)))) != 0)
                {
                    ho__MainIcFailRegs.Dispose();
                    HOperatorSet.GenEmptyObj(out ho__MainIcFailRegs);
                    ho__NewInspectReg.Dispose();
                    HOperatorSet.AffineTransRegion(ho__InspectReg, out ho__NewInspectReg, hv__HomMatModel2Img,
                        "nearest_neighbor");
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.ConcatObj(ho__MainIcFailRegs, ho__NewInspectReg, out ExpTmpOutVar_0
                            );
                        ho__MainIcFailRegs.Dispose();
                        ho__MainIcFailRegs = ExpTmpOutVar_0;
                    }
                }
                if ((int)(new HTuple(hv__ErrCode.TupleNotEqual(0))) != 0)
                {
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.ConcatObj(ho_o_FailRegs, ho__MainIcFailRegs, out ExpTmpOutVar_0
                            );
                        ho_o_FailRegs.Dispose();
                        ho_o_FailRegs = ExpTmpOutVar_0;
                    }
                    hv_o_ErrCode = hv__ErrCode.Clone();
                    if ((int)(new HTuple(hv__ErrCode.TupleLess(0))) != 0)
                    {
                        hv_o_ErrStr = "MainIC inspect failed: " + hv__ErrString;
                    }
                    else if ((int)(new HTuple(hv__ErrCode.TupleEqual(1))) != 0)
                    {
                        hv_o_ErrStr = "Find no main IC!";
                        hv_o_DefectType = hv_o_DefectType.TupleConcat(1);
                        hv_o_DefectImgIdx = hv_o_DefectImgIdx.TupleConcat(2);
                    }
                    else if ((int)(new HTuple(hv__ErrCode.TupleEqual(3))) != 0)
                    {
                        hv_o_ErrStr = " No main IC!";
                        hv_o_DefectType = hv_o_DefectType.TupleConcat(3);
                        hv_o_DefectImgIdx = hv_o_DefectImgIdx.TupleConcat(2);
                    }
                    else
                    {
                        hv_o_ErrStr = "Too dirty to find ic!";
                        hv_o_DefectType = hv_o_DefectType.TupleConcat(6);
                        hv_o_DefectImgIdx = hv_o_DefectImgIdx.TupleConcat(2);
                    }
                    ho__MatchReg.Dispose();
                    ho__InspectReg.Dispose();
                    ho__SubRegs.Dispose();
                    ho__RejectReg.Dispose();
                    ho__ImgDark.Dispose();
                    ho__ImgLight.Dispose();
                    ho__NewMatchReg.Dispose();
                    ho_RegionDilation.Dispose();
                    ho__MainIcFailRegs.Dispose();
                    ho__ChipReg.Dispose();
                    ho__NewInspectReg.Dispose();
                    ho__MainIcWireFailRegs.Dispose();
                    ho__MainIcWireChipReg.Dispose();
                    ho__MinorChipReg.Dispose();
                    ho__MinorIcFailRegs.Dispose();
                    ho__FrameMatchReg.Dispose();
                    ho__FrameInspectReg.Dispose();
                    ho__FrameSubRegs.Dispose();
                    ho__FrameRejectReg.Dispose();
                    ho__FrameImgDark.Dispose();
                    ho__FrameImgLight.Dispose();
                    ho__NewFrameMatchReg.Dispose();
                    ho__NewFrameSubRegs.Dispose();
                    ho__FrameAllFailRegs.Dispose();
                    ho__FrameFailRegs.Dispose();
                    ho__DefectRegion.Dispose();
                    ho_RegionAffineTrans.Dispose();
                    ho__Bond1Regs.Dispose();
                    ho__Bond2Regs.Dispose();
                    ho__NewBond1Regs.Dispose();
                    ho__NewBond2Regs.Dispose();
                    ho__reg.Dispose();
                    ho__reg1.Dispose();
                    ho__FailRegs.Dispose();
                    ho__Wires.Dispose();
                    ho__Bond1Balls.Dispose();
                    ho__Bond2Balls.Dispose();
                    ho__WireRes.Dispose();
                    ho_ConnectedRegions.Dispose();
                    ho_RegionUnion.Dispose();
                    ho__MinorFailRegs.Dispose();
                    ho__FrameConRegions.Dispose();
                    ho_FrameRegs.Dispose();

                    return;
                }





                //---------------2. 主IC检测 针对Wire上的图像进行检测
                if ((int)(new HTuple((((hvec_i_MainIcWireArgs[0].T).TupleSelect(0))).TupleNotEqual(
                    "none"))) != 0)
                {
                    ho__MatchReg.Dispose();
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        ho__MatchReg = hvec_i_MainIcWireObj[0].O.CopyObj(1, -1);
                    }
                    ho__InspectReg.Dispose();
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        ho__InspectReg = hvec_i_MainIcWireObj[1].O.CopyObj(1, -1);
                    }
                    ho__SubRegs.Dispose();
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        ho__SubRegs = hvec_i_MainIcWireObj[3].O.CopyObj(1, -1);
                    }
                    ho__RejectReg.Dispose();
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        ho__RejectReg = hvec_i_MainIcWireObj[2].O.CopyObj(1, -1);
                    }
                    ho__ImgDark.Dispose();
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        ho__ImgDark = hvec_i_MainIcWireObj[4].O.CopyObj(1, -1);
                    }
                    ho__ImgLight.Dispose();
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        ho__ImgLight = hvec_i_MainIcWireObj[5].O.CopyObj(1, -1);
                    }
                    //
                    hv__Model = hvec_i_MainIcWireArgs[0].T.Clone();
                    hv__ID = hv__Model[0];
                    hv__Type = hv__Model[1];
                    hv__ImgIdx = hv__Model[2];
                    hv__FindIcImgIdx = hv__Model[3];
                    hv__DilationSize = hvec_i_MainIcWireArgs[1].T.Clone();
                    hv__AngleStart = hvec_i_MainIcWireArgs[2].T.Clone();
                    hv__AngleExt = hvec_i_MainIcWireArgs[3].T.Clone();
                    hv__MinScore = hvec_i_MainIcWireArgs[4].T.Clone();
                    hv__CloseSize = hvec_i_MainIcWireArgs[5].T.Clone();
                    hv__MainIcWireMinWidth = hvec_i_MainIcWireArgs[6].T.Clone();
                    hv__MainIcWireMinHeight = hvec_i_MainIcWireArgs[7].T.Clone();
                    hv__MainIcWireMinArea = hvec_i_MainIcWireArgs[8].T.Clone();
                    hv__MainIcWireSelectOperate = hvec_i_MainIcWireArgs[9].T.Clone();


                    if ((int)(new HTuple(hv_o_ErrCode.TupleEqual(0))) != 0)
                    {
                        ho__MainIcWireFailRegs.Dispose(); ho__MainIcWireChipReg.Dispose();
                        HTV.HTV_inspect_golden_model(hvec_i_Images, ho__NewMatchReg, ho__InspectReg,
                            ho__RejectReg, ho__SubRegs, ho__ImgDark, ho__ImgLight, ho_i_CoarseReg,
                            out ho__MainIcWireFailRegs, out ho__MainIcWireChipReg, hv__MainIcRow,
                            hv__MainIcColumn, hv__MainIcAngle, hv__ImgIdx, hv__DilationSize, hv__AngleStart,
                            hv__AngleExt, hv__MinScore, hv__CloseSize, hv__MainIcWireMinWidth, hv__MainIcWireMinHeight,
                            hv__MainIcWireMinArea, hv__MainIcWireSelectOperate, out hv__MainIcWireHomModel2Img,
                            out hv__ErrCode, out hv__ErrString);
                    }
                    else
                    {
                        ho__MainIcWireFailRegs.Dispose();
                        HOperatorSet.GenEmptyObj(out ho__MainIcWireFailRegs);
                    }

                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.ConcatObj(ho__MainIcFailRegs, ho__MainIcWireFailRegs, out ExpTmpOutVar_0
                            );
                        ho__MainIcFailRegs.Dispose();
                        ho__MainIcFailRegs = ExpTmpOutVar_0;
                    }
                }

                //--------------3. 辅IC检测

                if ((int)(new HTuple((((hvec_i_MinorIcArgs[0].T).TupleSelect(0))).TupleNotEqual(
                    "none"))) != 0)
                {

                    ho__MatchReg.Dispose();
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        ho__MatchReg = hvec_i_MinorIcObj[0].O.CopyObj(1, -1);
                    }
                    ho__InspectReg.Dispose();
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        ho__InspectReg = hvec_i_MinorIcObj[1].O.CopyObj(1, -1);
                    }
                    ho__SubRegs.Dispose();
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        ho__SubRegs = hvec_i_MinorIcObj[3].O.CopyObj(1, -1);
                    }
                    ho__RejectReg.Dispose();
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        ho__RejectReg = hvec_i_MinorIcObj[2].O.CopyObj(1, -1);
                    }
                    ho__ImgDark.Dispose();
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        ho__ImgDark = hvec_i_MinorIcObj[4].O.CopyObj(1, -1);
                    }
                    ho__ImgLight.Dispose();
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        ho__ImgLight = hvec_i_MinorIcObj[5].O.CopyObj(1, -1);
                    }
                    //
                    hv__Model = hvec_i_MinorIcArgs[0].T.Clone();
                    hv__ID = hv__Model[0];
                    hv__Type = hv__Model[1];
                    hv__ImgIdx = hv__Model[2];

                    hv__DilationSize = hvec_i_MinorIcArgs[1].T.Clone();
                    hv__AngleStart = hvec_i_MinorIcArgs[2].T.Clone();
                    hv__AngleExt = hvec_i_MinorIcArgs[3].T.Clone();
                    hv__MinScore = hvec_i_MinorIcArgs[4].T.Clone();
                    hv__CloseSize = hvec_i_MinorIcArgs[5].T.Clone();
                    hv__MinorIcMinWidth = hvec_i_MinorIcArgs[6].T.Clone();
                    hv__MinorIcMinHeight = hvec_i_MinorIcArgs[7].T.Clone();
                    hv__MinorIcMinArea = hvec_i_MinorIcArgs[8].T.Clone();
                    hv__MinorIcSelectOperate = hvec_i_MinorIcArgs[9].T.Clone();
                    //
                    hv__MinorIcRowThr = hvec_i_MinorIcArgs[10].T.Clone();
                    hv__MinorIcColThr = hvec_i_MinorIcArgs[11].T.Clone();
                    hv__MinorIcAngleThr = hvec_i_MinorIcArgs[12].T.Clone();

                    ho__NewMatchReg.Dispose();
                    HOperatorSet.AffineTransRegion(ho__MatchReg, out ho__NewMatchReg, hv__HomMatModel2Img,
                        "nearest_neighbor");
                    HOperatorSet.AffineTransRegion(ho__InspectReg, out ho__RectInspectReg, hv__HomMatModel2Img,
                     "nearest_neighbor");

                    HTV.HTV_inspect_nodie(hvec_i_Images, ho__RectInspectReg, hv__ImgIdx, out hv__ErrCode);
                    if ((int)(new HTuple(hv__ErrCode.TupleNotEqual(3))) != 0)
                    {
                        //1. match object
                        ho_RegionDilation.Dispose();
                        HTV.HTV_match_region_dilation(ho__MatchReg, out ho_RegionDilation, hv__DilationSize);
                        using (HDevDisposeHelper dh = new HDevDisposeHelper())
                        {
                            HTV.HTV_find_model(hvec_i_Images[hv__FindIcImgIdx].O, ho_RegionDilation, hv__ID,
                                hv__Type, hv__AngleStart, hv__AngleExt, hv__MinScore, 1, out hv_o_MinorRow,
                                out hv_o_MinorColumn, out hv_o_MinorAngle, out hv__num, out hv_o_score);
                        }
                        if ((int)(new HTuple(hv__num.TupleEqual(0))) != 0)
                        {
                            hv__ErrCode = 1;
                        }
                        else
                        {
                            //ho__MainIcFailRegs.Dispose();
                            ho__MinorChipReg.Dispose();
                            HTV.HTV_inspect_golden_model(hvec_i_Images, ho__NewMatchReg, ho__InspectReg,
                                ho__RejectReg, ho__SubRegs, ho__ImgDark, ho__ImgLight, ho_i_CoarseReg,
                                out ho__MinorIcFailRegs, out ho__MinorChipReg, hv_o_MinorRow, hv_o_MinorColumn,
                                hv_o_MinorAngle, hv__ImgIdx, hv__DilationSize, hv__AngleStart, hv__AngleExt,
                                hv__MinScore, hv__CloseSize, hv__MinorIcMinWidth, hv__MinorIcMinHeight,
                                hv__MinorIcMinArea, hv__MinorIcSelectOperate, out hv__MinorIcHomModel2Img,
                                out hv__ErrCode, out hv__ErrString);
                        }

                    }

                    if ((int)(new HTuple(hv__ErrCode.TupleLess(0))) != 0)
                    {
                        {
                            HObject ExpTmpOutVar_0;
                            HOperatorSet.ConcatObj(ho_o_FailRegs, ho__MinorIcFailRegs, out ExpTmpOutVar_0
                                );
                            ho_o_FailRegs.Dispose();
                            ho_o_FailRegs = ExpTmpOutVar_0;
                        }
                        hv_o_ErrCode = hv__ErrCode.Clone();
                        if ((int)(new HTuple(hv__ErrCode.TupleLess(0))) != 0)
                        {
                            hv_o_ErrStr = "MainorIC inspect failed: " + hv__ErrString;
                        }
                        else if ((int)(new HTuple(hv__ErrCode.TupleEqual(1))) != 0)
                        {
                            hv_o_ErrStr = "Find no minor IC!";
                            hv_o_DefectType = hv_o_DefectType.TupleConcat(6);
                            hv_o_DefectImgIdx = hv_o_DefectImgIdx.TupleConcat(2);
                        }
                        else if ((int)(new HTuple(hv__ErrCode.TupleEqual(3))) != 0)
                        {
                            hv_o_ErrStr = " no minor IC!";
                            hv_o_DefectType = hv_o_DefectType.TupleConcat(3);
                            hv_o_DefectImgIdx = hv_o_DefectImgIdx.TupleConcat(2);
                        }
                        ho__MatchReg.Dispose();
                        ho__InspectReg.Dispose();
                        ho__SubRegs.Dispose();
                        ho__RejectReg.Dispose();
                        ho__ImgDark.Dispose();
                        ho__ImgLight.Dispose();
                        ho__NewMatchReg.Dispose();
                        ho_RegionDilation.Dispose();
                        ho__MainIcFailRegs.Dispose();
                        ho__ChipReg.Dispose();
                        ho__NewInspectReg.Dispose();
                        ho__MainIcWireFailRegs.Dispose();
                        ho__MainIcWireChipReg.Dispose();
                        ho__MinorChipReg.Dispose();
                        ho__MinorIcFailRegs.Dispose();
                        ho__FrameMatchReg.Dispose();
                        ho__FrameInspectReg.Dispose();
                        ho__FrameSubRegs.Dispose();
                        ho__FrameRejectReg.Dispose();
                        ho__FrameImgDark.Dispose();
                        ho__FrameImgLight.Dispose();
                        ho__NewFrameMatchReg.Dispose();
                        ho__NewFrameSubRegs.Dispose();
                        ho__FrameAllFailRegs.Dispose();
                        ho__FrameFailRegs.Dispose();
                        ho__DefectRegion.Dispose();
                        ho_RegionAffineTrans.Dispose();
                        ho__Bond1Regs.Dispose();
                        ho__Bond2Regs.Dispose();
                        ho__NewBond1Regs.Dispose();
                        ho__NewBond2Regs.Dispose();
                        ho__reg.Dispose();
                        ho__reg1.Dispose();
                        ho__FailRegs.Dispose();
                        ho__Wires.Dispose();
                        ho__Bond1Balls.Dispose();
                        ho__Bond2Balls.Dispose();
                        ho__WireRes.Dispose();
                        ho_ConnectedRegions.Dispose();
                        ho_RegionUnion.Dispose();
                        ho__MinorFailRegs.Dispose();
                        ho__FrameConRegions.Dispose();
                        ho_FrameRegs.Dispose();

                        return;
                    }
                }
                else
                {
                    ho__MinorIcFailRegs.Dispose();
                    HOperatorSet.GenEmptyObj(out ho__MinorIcFailRegs);
                }







                //---------------4. Frame检测

                if ((int)(new HTuple((((hvec_i_FrameArgs[0].T).TupleSelect(0))).TupleNotEqual(
                    "none"))) != 0)
                {

                    ho__FrameMatchReg.Dispose();
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        ho__FrameMatchReg = hvec_i_FrameObj[0].O.CopyObj(1, -1);
                    }
                    ho__FrameInspectReg.Dispose();
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        ho__FrameInspectReg = hvec_i_FrameObj[1].O.CopyObj(1, -1);
                    }
                    ho__FrameSubRegs.Dispose();
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        ho__FrameSubRegs = hvec_i_FrameObj[3].O.CopyObj(1, -1);
                    }
                    ho__FrameRejectReg.Dispose();
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        ho__FrameRejectReg = hvec_i_FrameObj[2].O.CopyObj(1, -1);
                    }
                    ho__FrameImgDark.Dispose();
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        ho__FrameImgDark = hvec_i_FrameObj[4].O.CopyObj(1, -1);
                    }
                    ho__FrameImgLight.Dispose();
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        ho__FrameImgLight = hvec_i_FrameObj[5].O.CopyObj(1, -1);
                    }
                    //
                    hv__FrameModel = hvec_i_FrameArgs[0].T.Clone();
                    hv__FrameID = hv__FrameModel[0];
                    hv__FrameType = hv__FrameModel[1];
                    hv__FrameImgIdx = hv__FrameModel[2];
                    hv__FrameFindIcImgIdx = hv__FrameModel[3];


                    hv__FrameDilationSize = hvec_i_FrameArgs[1].T.Clone();
                    hv__FrameAngleStart = hvec_i_FrameArgs[2].T.Clone();
                    hv__FrameAngleExt = hvec_i_FrameArgs[3].T.Clone();
                    hv__FrameMinScore = hvec_i_FrameArgs[4].T.Clone();
                    hv__FrameCloseSize = hvec_i_FrameArgs[5].T.Clone();
                    hv__FrameMinWidth = hvec_i_FrameArgs[6].T.Clone();
                    hv__FrameMinHeight = hvec_i_FrameArgs[7].T.Clone();
                    hv__FrameMinArea = hvec_i_FrameArgs[8].T.Clone();
                    hv__FrameSelectOperate = hvec_i_FrameArgs[9].T.Clone();


                    ho__NewFrameMatchReg.Dispose();
                    HOperatorSet.AffineTransRegion(ho__FrameMatchReg, out ho__NewFrameMatchReg,
                        hv__HomMatModel2Img, "nearest_neighbor");
                    ho__NewFrameSubRegs.Dispose();
                    HOperatorSet.AffineTransRegion(ho__FrameSubRegs, out ho__NewFrameSubRegs,
                        hv__HomMatModel2Img, "nearest_neighbor");
                    ho__FrameAllFailRegs.Dispose();
                    HTV.HTV_inspect_frame_model(hvec_i_Images, ho__NewFrameMatchReg, ho__FrameInspectReg,
                        ho__FrameRejectReg, ho__FrameSubRegs, ho__FrameImgDark, ho__FrameImgLight,
                        ho_i_CoarseReg, out ho__FrameAllFailRegs, hv__FrameID, hv__FrameType,
                        hv__FrameImgIdx, hv__FrameFindIcImgIdx, hv__FrameDilationSize, hv__FrameAngleStart,
                        hv__FrameAngleExt, hv__FrameMinScore, hv__FrameCloseSize, hv__FrameMinWidth,
                        hv__FrameMinHeight, hv__FrameMinArea, hv__FrameSelectOperate, out hv__FrameHomModel2Img,
                        out hv__FrameRow, out hv__FrameColumn, out hv__FrameAngle, out hv__ErrCode,
                        out hv__ErrString);
                    //if (_ErrCode=1 or _ErrCode=2)
                    //gen_empty_obj (_FrameFailRegs)
                    //affine_trans_region (_FrameInspectReg, _NewFrameInspectReg, _HomMatModel2Img, 'nearest_neighbor')
                    //concat_obj (_FrameFailRegs, _NewFrameInspectReg, _MainIcFailRegs)
                    //endif
                    if ((int)(new HTuple(hv__ErrCode.TupleNotEqual(0))) != 0)
                    {
                        {
                            HObject ExpTmpOutVar_0;
                            HOperatorSet.ConcatObj(ho_o_FailRegs, ho__FrameAllFailRegs, out ExpTmpOutVar_0
                                );
                            ho_o_FailRegs.Dispose();
                            ho_o_FailRegs = ExpTmpOutVar_0;
                        }
                        hv_o_ErrCode = hv__ErrCode.Clone();
                        if ((int)(new HTuple(hv__ErrCode.TupleLess(0))) != 0)
                        {
                            hv_o_ErrStr = "Frame inspect failed: " + hv__ErrString;
                        }
                        else if ((int)(new HTuple(hv__ErrCode.TupleEqual(1))) != 0)
                        {
                            hv_o_ErrStr = "Find no Frame!";
                            hv_o_DefectType = hv_o_DefectType.TupleConcat(18);
                        }
                        else
                        {
                            hv_o_ErrStr = "Too dirty to find Frame!";
                            hv_o_DefectType = hv_o_DefectType.TupleConcat(18);
                            hv_o_DefectImgIdx = hv_o_DefectImgIdx.TupleConcat(0);
                        }
                        ho__MatchReg.Dispose();
                        ho__InspectReg.Dispose();
                        ho__SubRegs.Dispose();
                        ho__RejectReg.Dispose();
                        ho__ImgDark.Dispose();
                        ho__ImgLight.Dispose();
                        ho__NewMatchReg.Dispose();
                        ho_RegionDilation.Dispose();
                        ho__MainIcFailRegs.Dispose();
                        ho__ChipReg.Dispose();
                        ho__NewInspectReg.Dispose();
                        ho__MainIcWireFailRegs.Dispose();
                        ho__MainIcWireChipReg.Dispose();
                        ho__MinorChipReg.Dispose();
                        ho__MinorIcFailRegs.Dispose();
                        ho__FrameMatchReg.Dispose();
                        ho__FrameInspectReg.Dispose();
                        ho__FrameSubRegs.Dispose();
                        ho__FrameRejectReg.Dispose();
                        ho__FrameImgDark.Dispose();
                        ho__FrameImgLight.Dispose();
                        ho__NewFrameMatchReg.Dispose();
                        ho__NewFrameSubRegs.Dispose();
                        ho__FrameAllFailRegs.Dispose();
                        ho__FrameFailRegs.Dispose();
                        ho__DefectRegion.Dispose();
                        ho_RegionAffineTrans.Dispose();
                        ho__Bond1Regs.Dispose();
                        ho__Bond2Regs.Dispose();
                        ho__NewBond1Regs.Dispose();
                        ho__NewBond2Regs.Dispose();
                        ho__reg.Dispose();
                        ho__reg1.Dispose();
                        ho__FailRegs.Dispose();
                        ho__Wires.Dispose();
                        ho__Bond1Balls.Dispose();
                        ho__Bond2Balls.Dispose();
                        ho__WireRes.Dispose();
                        ho_ConnectedRegions.Dispose();
                        ho_RegionUnion.Dispose();
                        ho__MinorFailRegs.Dispose();
                        ho__FrameConRegions.Dispose();
                        ho_FrameRegs.Dispose();

                        return;
                    }
                }
                else
                {
                    ho__FrameAllFailRegs.Dispose();
                    HOperatorSet.GenEmptyObj(out ho__FrameAllFailRegs);
                }

                //-----------------5.银胶检测
                hv__EpoxyInspectSize = hvec_i_EpoxyArgs[0].T;
                hv__EpoxyDarkLight = (long)hvec_i_EpoxyArgs[1].T.D;
                hv__EpoxyEdgeSigma = hvec_i_EpoxyArgs[2].T;
                hv__EpoxyEdgeThresh = hvec_i_EpoxyArgs[3].T;
                hv__EpoxyDistThresh = hvec_i_EpoxyArgs[4].T;
                hv__ImgIdx = (long)hvec_i_EpoxyArgs[5].T.D;
                ho__NewInspectReg.Dispose();
                HOperatorSet.AffineTransRegion(ho__InspectReg, out ho__NewInspectReg, hv__MainIcHomModel2Img,
                    "nearest_neighbor");


                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    ho__DefectRegion.Dispose();
                    HTV.HTV_inspect_expoxy_distance(hvec_i_Images[hv__ImgIdx].O, ho__NewInspectReg,
                        out ho__DefectRegion, hv__EpoxyInspectSize, hv__EpoxyEdgeSigma, hv__EpoxyEdgeThresh,
                        hv__EpoxyDarkLight, hv__EpoxyDistThresh, out hv__ErrCode, out hv__ErrString,
                        out hv_o_EpoxyResult);
                }
                if ((int)(new HTuple(hv__ErrCode.TupleEqual(1))) != 0)
                {
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.ConcatObj(ho_o_FailRegs, ho__DefectRegion, out ExpTmpOutVar_0
                            );
                        ho_o_FailRegs.Dispose();
                        ho_o_FailRegs = ExpTmpOutVar_0;
                    }
                    hv_o_DefectType = hv_o_DefectType.TupleConcat(9);
                    hv_o_DefectImgIdx = hv_o_DefectImgIdx.TupleConcat(0);
                    hv_o_ErrStr = "Epoxy defect ";
                    HOperatorSet.TupleGenConst(4, 0, out hv_o_EpoxyResult);
                }


                hvec_o_Value[28] = new HTupleVector(hv_o_EpoxyResult.TupleSelect(0));
                hvec_o_Value[29] = new HTupleVector(hv_o_EpoxyResult.TupleSelect(1));
                hvec_o_Value[30] = new HTupleVector(hv_o_EpoxyResult.TupleSelect(2));
                hvec_o_Value[31] = new HTupleVector(hv_o_EpoxyResult.TupleSelect(3));




                //--------------6. IC位置偏移
                //----------------.MainIC位置偏移
                if ((int)(new HTuple((((hvec_i_MainIcArgs[0].T).TupleSelect(0))).TupleNotEqual(
                    "none"))) != 0)
                {

                    hv_RowDiff = ((hv_o_Row - hv__MainIcRow)).TupleAbs();
                    hv_ColDiff = ((hv_o_Col - hv__MainIcColumn)).TupleAbs();
                    hv_AngleDiff = ((hv_o_Angle - hv__MainIcAngle)).TupleAbs();
                    if ((int)(new HTuple(hv_AngleDiff.TupleGreater((new HTuple(180)).TupleRad()
                        ))) != 0)
                    {
                        hv_AngleDiff = ((new HTuple(360)).TupleRad()) - hv_AngleDiff;
                    }

                    if ((int)((new HTuple((new HTuple(hv_RowDiff.TupleGreater(hv__MainIcRowThr))).TupleAnd(
                        new HTuple(hv_ColDiff.TupleGreater(hv__MainIcColThr))))).TupleAnd(new HTuple(hv_AngleDiff.TupleGreater(
                        hv__MainIcAngleThr)))) != 0)
                    {
                        ho__InspectReg.Dispose();
                        using (HDevDisposeHelper dh = new HDevDisposeHelper())
                        {
                            ho__InspectReg = hvec_i_MainIcObj[1].O.CopyObj(1, -1);
                        }
                        ho_RegionAffineTrans.Dispose();
                        HOperatorSet.AffineTransRegion(ho__InspectReg, out ho_RegionAffineTrans,
                            hv__MainIcHomModel2Img, "nearest_neighbor");
                        {
                            HObject ExpTmpOutVar_0;
                            HOperatorSet.ConcatObj(ho_o_FailRegs, ho_RegionAffineTrans, out ExpTmpOutVar_0
                                );
                            ho_o_FailRegs.Dispose();
                            ho_o_FailRegs = ExpTmpOutVar_0;
                        }
                        hv_o_ErrCode = 2;
                        hv_o_DefectType = hv_o_DefectType.TupleConcat(2);
                        hv_o_DefectImgIdx = hv_o_DefectImgIdx.TupleConcat(0);
                    }
                    HOperatorSet.DistancePp(hv_o_Row, hv_o_Col, hv__MainIcRow, hv__MainIcColumn,
                        out hv_Distance);
                    hvec_o_Value[32] = new HTupleVector(hv_RowDiff).Clone();
                    hvec_o_Value[33] = new HTupleVector(hv_ColDiff).Clone();
                    hvec_o_Value[34] = new HTupleVector(hv_Distance).Clone();
                    hvec_o_Value[35] = new HTupleVector(hv_AngleDiff).Clone();

                }

                //if (i_MinorIcArgs.at(0)[0]#'none')

                //RowDiff := abs(o_Row-_MinorIcRow)
                //ColDiff := abs(o_Col-_MinorIcColumn)
                //AngleDiff := abs(o_Angle-_MinorIcAngle)
                //if (AngleDiff > rad(180))
                //AngleDiff := rad(360) - AngleDiff
                //endif

                //if (RowDiff > _MinorIcRowThr and ColDiff > _MinorIcColThr and AngleDiff > _MinorIcAngleThr)
                //_InspectReg := i_MainIcObj.at(1)
                //affine_trans_region (_InspectReg, RegionAffineTrans, _MinorIcHomModel2Img, 'nearest_neighbor')
                //concat_obj (o_FailRegs, RegionAffineTrans, o_FailRegs)
                //o_ErrCode := 2
                //o_DefectType := [o_DefectType,2]
                //o_DefectImgIdx := [o_DefectImgIdx,0]
                //endif
                //distance_pp (o_Row, o_Col, _MinorIcRow, _MinorIcColumn, Distance)
                //o_Value.at(32) := RowDiff
                //o_Value.at(33) := ColDiff
                //o_Value.at(34) := Distance
                //o_Value.at(35) := AngleDiff

                //endif




                //------------------7.Wire检测
                hv__ImgIdx = hvec_i_EpoxyArgs[5].T.Clone();
                ho__Bond1Regs.Dispose();
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    ho__Bond1Regs = hvec_i_BondWireObj[0].O.CopyObj(1, -1);
                }
                ho__Bond2Regs.Dispose();
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    ho__Bond2Regs = hvec_i_BondWireObj[1].O.CopyObj(1, -1);
                }
                hv__Bond1OnIc = hvec_i_BondWireArgs[0].T.Clone();
                hv__Bond1ImgIdx = hvec_i_BondWireArgs[1].T.Clone();
                hv__Bond2OnIc = hvec_i_BondWireArgs[2].T.Clone();
                hv__Bond2ImgIdx = hvec_i_BondWireArgs[3].T.Clone();
                hv__Bond2BallNum = hvec_i_BondWireArgs[4].T.Clone();
                hv__WireImgIdx = hvec_i_BondWireArgs[5].T.Clone();
                hv__Model = hvec_i_BondWireArgs[6].T.Clone();
                hv__ID = hv__Model[0];
                hv__Type = hv__Model[1];
                hv__Paras = hvec_i_BondWireArgs[7].T.Clone();
                hv__MinBall1Rad = hv__Paras[0];
                hv__MaxBall1Rad = hv__Paras[1];
                hv__AngleExt = hv__Paras[2];
                hv__MinScore = hv__Paras[3];
                hv__SearchLen = hv__Paras[4];
                hv__ClipLen = hv__Paras[5];
                hv__WireWidth = hv__Paras[6];
                hv__WireContrast = hv__Paras[7];
                hv__MinSegLen = hv__Paras[8];
                hv__WireAngleExt = hv__Paras[9];
                hv__MaxWireGap = hv__Paras[10];

                ho__NewBond1Regs.Dispose();
                HOperatorSet.AffineTransRegion(ho__Bond1Regs, out ho__NewBond1Regs, hv__MainIcHomModel2Img,
                    "nearest_neighbor");
                HOperatorSet.CountObj(ho__Bond2Regs, out hv_Number1);
                ho__NewBond2Regs.Dispose();
                HOperatorSet.GenEmptyObj(out ho__NewBond2Regs);
                //映射第二焊点时，由于其分布的位置不同，因此需要用不同的传递函数处理
                HTuple end_val385 = hv_Number1;
                HTuple step_val385 = 1;
                for (hv_i = 1; hv_i.Continue(end_val385, step_val385); hv_i = hv_i.TupleAdd(step_val385))
                {
                    ho__reg.Dispose();
                    HOperatorSet.SelectObj(ho__Bond2Regs, out ho__reg, hv_i);
                    if ((int)(hv__Bond2OnIc.TupleSelect(hv_i - 1)) != 0)
                    {
                        ho__reg1.Dispose();
                        HOperatorSet.AffineTransRegion(ho__reg, out ho__reg1, hv__MainIcHomModel2Img,
                            "nearest_neighbor");
                    }
                    else
                    {
                        ho__reg1.Dispose();
                        HOperatorSet.AffineTransRegion(ho__reg, out ho__reg1, hv__HomMatModel2Img,
                            "nearest_neighbor");
                    }
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.ConcatObj(ho__NewBond2Regs, ho__reg1, out ExpTmpOutVar_0);
                        ho__NewBond2Regs.Dispose();
                        ho__NewBond2Regs = ExpTmpOutVar_0;
                    }
                }
                ho__FailRegs.Dispose(); ho__Wires.Dispose(); ho__Bond1Balls.Dispose(); ho__Bond2Balls.Dispose();
                HTV.HTV_inspect_bond_wire(hvec_i_Images, ho__NewBond1Regs, ho__NewBond2Regs, out ho__FailRegs,
                    out ho__Wires, out ho__Bond1Balls, out ho__Bond2Balls, hv__Bond1OnIc, hv__Bond1ImgIdx,
                    hv__Bond2OnIc, hv__Bond2ImgIdx, hv__Bond2BallNum, hv__ID, hv__Type, hv__MinBall1Rad,
                    hv__MaxBall1Rad, hv__AngleExt, hv__MinScore, hv__WireImgIdx, hv__SearchLen,
                    hv__ClipLen, hv__WireWidth, hv__WireContrast, hv__MinSegLen, hv__WireAngleExt,
                    hv__MaxWireGap, out hv__ErrCode, out hv__ErrString, out hvec_o_bondwire_Value);

                hvec_o_Value[36] = hvec_o_bondwire_Value[0];
                hvec_o_Value[37] = hvec_o_bondwire_Value[1];
                hvec_o_Value[38] = hvec_o_bondwire_Value[2];

                if ((int)(new HTuple(hv__ErrCode.TupleEqual(-1))) != 0)
                {
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.ConcatObj(ho_o_FailRegs, ho__FailRegs, out ExpTmpOutVar_0);
                        ho_o_FailRegs.Dispose();
                        ho_o_FailRegs = ExpTmpOutVar_0;
                    }
                    hv_o_ErrCode = -1;
                    hv_o_ErrStr = "IC2IC Wire inspect failed: " + hv__ErrString;
                    ho__MatchReg.Dispose();
                    ho__InspectReg.Dispose();
                    ho__SubRegs.Dispose();
                    ho__RejectReg.Dispose();
                    ho__ImgDark.Dispose();
                    ho__ImgLight.Dispose();
                    ho__NewMatchReg.Dispose();
                    ho_RegionDilation.Dispose();
                    ho__MainIcFailRegs.Dispose();
                    ho__ChipReg.Dispose();
                    ho__NewInspectReg.Dispose();
                    ho__MainIcWireFailRegs.Dispose();
                    ho__MainIcWireChipReg.Dispose();
                    ho__MinorChipReg.Dispose();
                    ho__MinorIcFailRegs.Dispose();
                    ho__FrameMatchReg.Dispose();
                    ho__FrameInspectReg.Dispose();
                    ho__FrameSubRegs.Dispose();
                    ho__FrameRejectReg.Dispose();
                    ho__FrameImgDark.Dispose();
                    ho__FrameImgLight.Dispose();
                    ho__NewFrameMatchReg.Dispose();
                    ho__NewFrameSubRegs.Dispose();
                    ho__FrameAllFailRegs.Dispose();
                    ho__FrameFailRegs.Dispose();
                    ho__DefectRegion.Dispose();
                    ho_RegionAffineTrans.Dispose();
                    ho__Bond1Regs.Dispose();
                    ho__Bond2Regs.Dispose();
                    ho__NewBond1Regs.Dispose();
                    ho__NewBond2Regs.Dispose();
                    ho__reg.Dispose();
                    ho__reg1.Dispose();
                    ho__FailRegs.Dispose();
                    ho__Wires.Dispose();
                    ho__Bond1Balls.Dispose();
                    ho__Bond2Balls.Dispose();
                    ho__WireRes.Dispose();
                    ho_ConnectedRegions.Dispose();
                    ho_RegionUnion.Dispose();
                    ho__MinorFailRegs.Dispose();
                    ho__FrameConRegions.Dispose();
                    ho_FrameRegs.Dispose();

                    return;
                }
                if ((int)(new HTuple((new HTuple(hv__ErrCode.TupleLength())).TupleGreater(1))) != 0)
                {
                    //tuple_sort (_ErrCode, _ErrCode)
                    //tuple_uniq (_ErrCode, _ErrCode)
                    if ((int)(new HTuple(((hv__ErrCode.TupleSelect(0))).TupleEqual(0))) != 0)
                    {
                        HOperatorSet.TupleRemove(hv__ErrCode, 0, out hv__ErrCode);
                    }
                    for (hv_Index = 0; (int)hv_Index <= (int)((new HTuple(hv__ErrCode.TupleLength()
                        )) - 1); hv_Index = (int)hv_Index + 1)
                    {
                        if ((int)((new HTuple((new HTuple(((hv__ErrCode.TupleSelect(hv_Index))).TupleEqual(
                            13))).TupleOr(new HTuple(((hv__ErrCode.TupleSelect(hv_Index))).TupleEqual(
                            14))))).TupleOr(new HTuple(((hv__ErrCode.TupleSelect(hv_Index))).TupleEqual(
                            15)))) != 0)
                        {
                            hv_o_DefectImgIdx = hv_o_DefectImgIdx.TupleConcat(0);
                        }
                        else
                        {
                            hv_o_DefectImgIdx = hv_o_DefectImgIdx.TupleConcat(2);
                        }
                    }
                }

                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.ConcatObj(ho_o_Wires, ho__Wires, out ExpTmpOutVar_0);
                    ho_o_Wires.Dispose();
                    ho_o_Wires = ExpTmpOutVar_0;
                }
                HOperatorSet.CountObj(ho__FailRegs, out hv__num);
                if ((int)(new HTuple(hv__num.TupleGreater(0))) != 0)
                {
                    //union1 (_FailRegs, _FailRegs)
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.ConcatObj(ho_o_FailRegs, ho__FailRegs, out ExpTmpOutVar_0);
                        ho_o_FailRegs.Dispose();
                        ho_o_FailRegs = ExpTmpOutVar_0;
                    }
                    hv_o_DefectType = hv_o_DefectType.TupleConcat(hv__ErrCode);
                }




                //-----------------8. 将Bond区域设为IC的免检区（拒绝区）
                ho__WireRes.Dispose();
                HTV.HTV_contours_to_region(ho__Wires, out ho__WireRes, hv__WireWidth);
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.Difference(ho__MainIcFailRegs, ho__WireRes, out ExpTmpOutVar_0
                        );
                    ho__MainIcFailRegs.Dispose();
                    ho__MainIcFailRegs = ExpTmpOutVar_0;
                }
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.Difference(ho__MainIcFailRegs, ho__Bond1Balls, out ExpTmpOutVar_0
                        );
                    ho__MainIcFailRegs.Dispose();
                    ho__MainIcFailRegs = ExpTmpOutVar_0;
                }
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.Difference(ho__MainIcFailRegs, ho__Bond2Balls, out ExpTmpOutVar_0
                        );
                    ho__MainIcFailRegs.Dispose();
                    ho__MainIcFailRegs = ExpTmpOutVar_0;
                }
                ho_ConnectedRegions.Dispose();
                HOperatorSet.Connection(ho__MainIcFailRegs, out ho_ConnectedRegions);
                ho__MainIcFailRegs.Dispose();
                HOperatorSet.SelectShape(ho_ConnectedRegions, out ho__MainIcFailRegs, ((new HTuple("rect2_len1")).TupleConcat(
                    "rect2_len2")).TupleConcat("area"), hv__MainIcSelectOperate, (((((hv__MainIcMinWidth.TupleSelect(
                    0)) / 2.0)).TupleConcat((hv__MainIcMinHeight.TupleSelect(0)) / 2.0))).TupleConcat(
                    hv__MainIcMinArea.TupleSelect(0)), ((new HTuple(999999)).TupleConcat(999999)).TupleConcat(
                    9999999));
                HOperatorSet.CountObj(ho__MainIcFailRegs, out hv_Number);

                if ((int)(hv_Number) != 0)
                {
                    HTV.HTV_calculate_output(ho__MainIcFailRegs, hv_Number, out hvec_TempValue);
                    ho_RegionUnion.Dispose();
                    HOperatorSet.Union1(ho__MainIcFailRegs, out ho_RegionUnion);
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.ConcatObj(ho_o_FailRegs, ho_RegionUnion, out ExpTmpOutVar_0);
                        ho_o_FailRegs.Dispose();
                        ho_o_FailRegs = ExpTmpOutVar_0;
                    }
                    hv_o_DefectType = hv_o_DefectType.TupleConcat(7);
                    hv_o_DefectImgIdx = hv_o_DefectImgIdx.TupleConcat(2);
                    hvec_o_Value[0] = hvec_TempValue[0];
                    hvec_o_Value[1] = hvec_TempValue[1];
                    hvec_o_Value[2] = hvec_TempValue[2];
                    hvec_o_Value[3] = hvec_TempValue[3];
                    hvec_o_Value[4] = hvec_TempValue[4];
                    hvec_o_Value[5] = hvec_TempValue[5];
                    hvec_o_Value[6] = hvec_TempValue[6];


                }

                //-----------------将Bond区域设为minorIC的免检区（拒绝区）
                //HTV_contours_to_region (_Wires, _WireRes, _WireWidth)
                if ((int)(new HTuple((((hvec_i_MinorIcArgs[0].T).TupleSelect(0))).TupleNotEqual(
          "none"))) != 0)
                {
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.Difference(ho__MinorIcFailRegs, ho__WireRes, out ExpTmpOutVar_0
                            );
                        ho__MinorIcFailRegs.Dispose();
                        ho__MinorIcFailRegs = ExpTmpOutVar_0;
                    }
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.Difference(ho__MinorIcFailRegs, ho__Bond1Balls, out ExpTmpOutVar_0
                            );
                        ho__MinorIcFailRegs.Dispose();
                        ho__MinorIcFailRegs = ExpTmpOutVar_0;
                    }
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.Difference(ho__MinorIcFailRegs, ho__Bond2Balls, out ExpTmpOutVar_0
                            );
                        ho__MinorIcFailRegs.Dispose();
                        ho__MinorIcFailRegs = ExpTmpOutVar_0;
                    }
                    ho_ConnectedRegions.Dispose();
                    HOperatorSet.Connection(ho__MinorIcFailRegs, out ho_ConnectedRegions);
                    ho__MinorIcFailRegs.Dispose();
                    HOperatorSet.SelectShape(ho_ConnectedRegions, out ho__MinorIcFailRegs, ((new HTuple("rect2_len1")).TupleConcat(
                        "rect2_len2")).TupleConcat("area"), hv__MinorIcSelectOperate, (((((hv__MinorIcMinWidth.TupleSelect(
                        0)) / 2.0)).TupleConcat((hv__MinorIcMinHeight.TupleSelect(0)) / 2.0))).TupleConcat(
                        hv__MinorIcMinArea.TupleSelect(0)), ((new HTuple(999999)).TupleConcat(
                        999999)).TupleConcat(9999999));
                    HOperatorSet.CountObj(ho__MinorIcFailRegs, out hv_Number);

                    if ((int)(hv_Number) != 0)
                    {
                        HTV.HTV_calculate_output(ho__MinorIcFailRegs, hv_Number, out hvec_TempValue);
                        ho_RegionUnion.Dispose();
                        HOperatorSet.Union1(ho__MinorIcFailRegs, out ho_RegionUnion);
                        {
                            HObject ExpTmpOutVar_0;
                            HOperatorSet.ConcatObj(ho_o_FailRegs, ho_RegionUnion, out ExpTmpOutVar_0
                                );
                            ho_o_FailRegs.Dispose();
                            ho_o_FailRegs = ExpTmpOutVar_0;
                        }
                        hv_o_DefectType = hv_o_DefectType.TupleConcat(7);
                        hv_o_DefectImgIdx = hv_o_DefectImgIdx.TupleConcat(2);
                        hvec_o_Value[7] = hvec_TempValue[0];
                        hvec_o_Value[8] = hvec_TempValue[1];
                        hvec_o_Value[9] = hvec_TempValue[2];
                        hvec_o_Value[10] = hvec_TempValue[3];
                        hvec_o_Value[11] = hvec_TempValue[4];
                        hvec_o_Value[12] = hvec_TempValue[5];
                        hvec_o_Value[13] = hvec_TempValue[6];


                    }

                }
                //------------------9. 将Bond区域设置为Chipping的免检区（拒绝区）
                //HTV_contours_to_region (_Wires, _WireRes, _WireWidth)

                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.Difference(ho__ChipReg, ho__WireRes, out ExpTmpOutVar_0);
                    ho__ChipReg.Dispose();
                    ho__ChipReg = ExpTmpOutVar_0;
                }
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.Difference(ho__ChipReg, ho__Bond1Balls, out ExpTmpOutVar_0);
                    ho__ChipReg.Dispose();
                    ho__ChipReg = ExpTmpOutVar_0;
                }
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.Difference(ho__ChipReg, ho__Bond2Balls, out ExpTmpOutVar_0);
                    ho__ChipReg.Dispose();
                    ho__ChipReg = ExpTmpOutVar_0;
                }
                ho_ConnectedRegions.Dispose();
                HOperatorSet.Connection(ho__ChipReg, out ho_ConnectedRegions);
                ho__ChipReg.Dispose();
                HOperatorSet.SelectShape(ho_ConnectedRegions, out ho__ChipReg, ((new HTuple("rect2_len1")).TupleConcat(
                    "rect2_len2")).TupleConcat("area"), hv__MainIcSelectOperate, (((((hv__MainIcMinWidth.TupleSelect(
                    0)) / 2.0)).TupleConcat((hv__MainIcMinHeight.TupleSelect(0)) / 2.0))).TupleConcat(
                    hv__MainIcMinArea.TupleSelect(0)), ((new HTuple(999999)).TupleConcat(999999)).TupleConcat(
                    9999999));
                HOperatorSet.CountObj(ho__ChipReg, out hv_Number);

                if ((int)(hv_Number) != 0)
                {
                    HTV.HTV_calculate_output(ho__ChipReg, hv_Number, out hvec_TempValue);
                    ho_RegionUnion.Dispose();
                    HOperatorSet.Union1(ho__ChipReg, out ho_RegionUnion);
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.ConcatObj(ho_o_FailRegs, ho_RegionUnion, out ExpTmpOutVar_0);
                        ho_o_FailRegs.Dispose();
                        ho_o_FailRegs = ExpTmpOutVar_0;
                    }
                    hv_o_DefectType = hv_o_DefectType.TupleConcat(8);
                    hv_o_DefectImgIdx = hv_o_DefectImgIdx.TupleConcat(2);
                    hvec_o_Value[14] = hvec_TempValue[0];
                    hvec_o_Value[15] = hvec_TempValue[1];
                    hvec_o_Value[16] = hvec_TempValue[2];
                    hvec_o_Value[17] = hvec_TempValue[3];
                    hvec_o_Value[18] = hvec_TempValue[4];
                    hvec_o_Value[19] = hvec_TempValue[5];
                    hvec_o_Value[20] = hvec_TempValue[6];

                }


                //-----------------10. 将Bond区域设为Frame的免检区（拒绝区）
                //HTV_contours_to_region (_Wires, _WireRes, _WireWidth)
                if ((int)(new HTuple((((hvec_i_FrameArgs[0].T).TupleSelect(0))).TupleNotEqual(
                    "none"))) != 0)
                {
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.Difference(ho__FrameAllFailRegs, ho__WireRes, out ExpTmpOutVar_0
                            );
                        ho__FrameAllFailRegs.Dispose();
                        ho__FrameAllFailRegs = ExpTmpOutVar_0;
                    }
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.Difference(ho__FrameAllFailRegs, ho__Bond1Balls, out ExpTmpOutVar_0
                            );
                        ho__FrameAllFailRegs.Dispose();
                        ho__FrameAllFailRegs = ExpTmpOutVar_0;
                    }
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.Difference(ho__FrameAllFailRegs, ho__Bond2Balls, out ExpTmpOutVar_0
                            );
                        ho__FrameAllFailRegs.Dispose();
                        ho__FrameAllFailRegs = ExpTmpOutVar_0;
                    }
                    ho__FrameConRegions.Dispose();
                    HOperatorSet.Connection(ho__FrameAllFailRegs, out ho__FrameConRegions);

                    //计算银胶区域 并输出剔除银胶以及ic芯片在内的框架错误区域

                    ho_FrameRegs.Dispose();
                    HTV.HTV_Epoxy_calculation(ho__FrameConRegions, ho__NewInspectReg, ho__NewFrameSubRegs,
                        out ho_FrameRegs);


                    ho__FrameFailRegs.Dispose();
                    HOperatorSet.SelectShape(ho_FrameRegs, out ho__FrameFailRegs, ((new HTuple("rect2_len1")).TupleConcat(
                        "rect2_len2")).TupleConcat("area"), hv__FrameSelectOperate, (((((hv__FrameMinWidth.TupleSelect(
                        0)) / 2.0)).TupleConcat((hv__FrameMinHeight.TupleSelect(0)) / 2.0))).TupleConcat(
                        hv__FrameMinArea.TupleSelect(0)), ((new HTuple(999999)).TupleConcat(999999)).TupleConcat(
                        9999999));
                    HOperatorSet.CountObj(ho__FrameFailRegs, out hv_Number);
                    if ((int)(hv_Number) != 0)
                    {
                        HTV.HTV_calculate_output(ho__FrameFailRegs, hv_Number, out hvec_TempValue);
                        ho_RegionUnion.Dispose();
                        HOperatorSet.Union1(ho__FrameFailRegs, out ho_RegionUnion);
                        {
                            HObject ExpTmpOutVar_0;
                            HOperatorSet.ConcatObj(ho_o_FailRegs, ho_RegionUnion, out ExpTmpOutVar_0
                                );
                            ho_o_FailRegs.Dispose();
                            ho_o_FailRegs = ExpTmpOutVar_0;
                        }
                        hv_o_DefectType = hv_o_DefectType.TupleConcat(18);
                        hv_o_DefectImgIdx = hv_o_DefectImgIdx.TupleConcat(0);
                        hvec_o_Value[21] = hvec_TempValue[0];
                        hvec_o_Value[22] = hvec_TempValue[1];
                        hvec_o_Value[23] = hvec_TempValue[2];
                        hvec_o_Value[24] = hvec_TempValue[3];
                        hvec_o_Value[25] = hvec_TempValue[4];
                        hvec_o_Value[26] = hvec_TempValue[5];
                        hvec_o_Value[27] = hvec_TempValue[6];

                    }

                }

                ho__MatchReg.Dispose();
                ho__InspectReg.Dispose();
                ho__SubRegs.Dispose();
                ho__RejectReg.Dispose();
                ho__ImgDark.Dispose();
                ho__ImgLight.Dispose();
                ho__NewMatchReg.Dispose();
                ho_RegionDilation.Dispose();
                ho__MainIcFailRegs.Dispose();
                ho__ChipReg.Dispose();
                ho__NewInspectReg.Dispose();
                ho__MainIcWireFailRegs.Dispose();
                ho__MainIcWireChipReg.Dispose();
                ho__MinorChipReg.Dispose();
                ho__MinorIcFailRegs.Dispose();
                ho__FrameMatchReg.Dispose();
                ho__FrameInspectReg.Dispose();
                ho__FrameSubRegs.Dispose();
                ho__FrameRejectReg.Dispose();
                ho__FrameImgDark.Dispose();
                ho__FrameImgLight.Dispose();
                ho__NewFrameMatchReg.Dispose();
                ho__NewFrameSubRegs.Dispose();
                ho__FrameAllFailRegs.Dispose();
                ho__FrameFailRegs.Dispose();
                ho__DefectRegion.Dispose();
                ho_RegionAffineTrans.Dispose();
                ho__Bond1Regs.Dispose();
                ho__Bond2Regs.Dispose();
                ho__NewBond1Regs.Dispose();
                ho__NewBond2Regs.Dispose();
                ho__reg.Dispose();
                ho__reg1.Dispose();
                ho__FailRegs.Dispose();
                ho__Wires.Dispose();
                ho__Bond1Balls.Dispose();
                ho__Bond2Balls.Dispose();
                ho__WireRes.Dispose();
                ho_ConnectedRegions.Dispose();
                ho_RegionUnion.Dispose();
                ho__MinorFailRegs.Dispose();
                ho__FrameConRegions.Dispose();
                ho_FrameRegs.Dispose();

                return;



            }
            catch (HalconException HDevExpDefaultException)
            {
                ho__MatchReg.Dispose();
                ho__InspectReg.Dispose();
                ho__SubRegs.Dispose();
                ho__RejectReg.Dispose();
                ho__ImgDark.Dispose();
                ho__ImgLight.Dispose();
                ho__NewMatchReg.Dispose();
                ho_RegionDilation.Dispose();
                ho__MainIcFailRegs.Dispose();
                ho__ChipReg.Dispose();
                ho__NewInspectReg.Dispose();
                ho__MainIcWireFailRegs.Dispose();
                ho__MainIcWireChipReg.Dispose();
                ho__MinorChipReg.Dispose();
                ho__MinorIcFailRegs.Dispose();
                ho__FrameMatchReg.Dispose();
                ho__FrameInspectReg.Dispose();
                ho__FrameSubRegs.Dispose();
                ho__FrameRejectReg.Dispose();
                ho__FrameImgDark.Dispose();
                ho__FrameImgLight.Dispose();
                ho__NewFrameMatchReg.Dispose();
                ho__NewFrameSubRegs.Dispose();
                ho__FrameAllFailRegs.Dispose();
                ho__FrameFailRegs.Dispose();
                ho__DefectRegion.Dispose();
                ho_RegionAffineTrans.Dispose();
                ho__Bond1Regs.Dispose();
                ho__Bond2Regs.Dispose();
                ho__NewBond1Regs.Dispose();
                ho__NewBond2Regs.Dispose();
                ho__reg.Dispose();
                ho__reg1.Dispose();
                ho__FailRegs.Dispose();
                ho__Wires.Dispose();
                ho__Bond1Balls.Dispose();
                ho__Bond2Balls.Dispose();
                ho__WireRes.Dispose();
                ho_ConnectedRegions.Dispose();
                ho_RegionUnion.Dispose();
                ho__MinorFailRegs.Dispose();
                ho__FrameConRegions.Dispose();
                ho_FrameRegs.Dispose();

                throw HDevExpDefaultException;
            }
        }


        public void JSCC_AOI_read_all_model(out HObjectVector/*{eObjectVector,Dim=1}*/ hvec_o_CoarseObjs,
            out HObjectVector/*{eObjectVector,Dim=1}*/ hvec_o_MainIcObjs, out HObjectVector/*{eObjectVector,Dim=1}*/ hvec_o_MainIcWireObjs,
            out HObjectVector/*{eObjectVector,Dim=1}*/ hvec_o_MinorIcObjs, out HObjectVector/*{eObjectVector,Dim=1}*/ hvec_o_FrameObjs,
            out HObjectVector/*{eObjectVector,Dim=1}*/ hvec_o_BondWireObjs, HTuple hv_i_ModelPath,
            HTupleVector/*{eTupleVector,Dim=1}*/ hvec_i_MainIcModelPara, HTupleVector/*{eTupleVector,Dim=1}*/ hvec_i_MainIcWireModelPara,
            HTupleVector/*{eTupleVector,Dim=1}*/ hvec_i_MinorIcModelPara, HTupleVector/*{eTupleVector,Dim=1}*/ hvec_i_FrameModelPara,
            out HTuple hv_o_CoarseMatchModel, out HTuple hv_o_MainIcModel, out HTuple hv_o_MainIcWireModel,
            out HTuple hv_o_MinorIcModel, out HTuple hv_o_FrameModel, out HTupleVector/*{eTupleVector,Dim=1}*/ hvec_o_BondWireModel,
            out HTuple hv_o_ErrCode, out HTuple hv_o_ErrStr)
        {



            // Local iconic variables 

            HObject ho__CoarseObjs, ho__MatchReg, ho__InsReg;
            HObject ho__RejReg, ho__SubRegs, ho__ImgMeans, ho__ImgStds;
            HObject ho__ImgDarks, ho__ImgLights, ho__Bond1Regs, ho__Bond2Regs;

            // Local control variables 

            HTuple hv__Path = null, hv_Coarse_ModelID = null;
            HTuple hv_Coarse_ModelType = null, hv_Coarse_ImgIdx = null;
            HTuple hv__ErrCode = null, hv__ErrString = null, hv__ThreshDark = null;
            HTuple hv__ThreshLight = null, hv__SobelScale = null, hv_Main_ModelID = null;
            HTuple hv_Main_ModelType = null, hv_Main_ImgIdx = null;
            HTuple hv_FindIc_ImgIdx = null, hv_FileExists = null, hv_FileExists1 = null;
            HTuple hv__ModelID = new HTuple(), hv__ModelType = new HTuple();
            HTuple hv__ImgIdx = new HTuple(), hv_FileExists2 = null;
            HTuple hv__Bond1OnIc = null, hv__Bond1ImgIdx = null, hv__Bond2OnIc = null;
            HTuple hv__Bond2ImgIdx = null, hv__Bond2BallNums = null;
            HTuple hv_Bond_ModelID = null, hv_Bond_ModelType = null;
            HTuple hv__WireImgIdx = null;
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho__CoarseObjs);
            HOperatorSet.GenEmptyObj(out ho__MatchReg);
            HOperatorSet.GenEmptyObj(out ho__InsReg);
            HOperatorSet.GenEmptyObj(out ho__RejReg);
            HOperatorSet.GenEmptyObj(out ho__SubRegs);
            HOperatorSet.GenEmptyObj(out ho__ImgMeans);
            HOperatorSet.GenEmptyObj(out ho__ImgStds);
            HOperatorSet.GenEmptyObj(out ho__ImgDarks);
            HOperatorSet.GenEmptyObj(out ho__ImgLights);
            HOperatorSet.GenEmptyObj(out ho__Bond1Regs);
            HOperatorSet.GenEmptyObj(out ho__Bond2Regs);
            hvec_o_CoarseObjs = new HObjectVector(1);
            hvec_o_MainIcObjs = new HObjectVector(1);
            hvec_o_MainIcWireObjs = new HObjectVector(1);
            hvec_o_MinorIcObjs = new HObjectVector(1);
            hvec_o_FrameObjs = new HObjectVector(1);
            hvec_o_BondWireObjs = new HObjectVector(1);
            hv_o_CoarseMatchModel = new HTuple();
            hv_o_MainIcModel = new HTuple();
            hv_o_MainIcWireModel = new HTuple();
            hv_o_MinorIcModel = new HTuple();
            hv_o_FrameModel = new HTuple();
            hvec_o_BondWireModel = new HTupleVector(1);
            try
            {
                hv_o_ErrCode = 0;
                hv_o_ErrStr = "";
                //**********CoarseMatch Model
                hv__Path = hv_i_ModelPath + "Coarse/";
                ho__CoarseObjs.Dispose();
                HTV.HTV_read_coarse_model(out ho__CoarseObjs, hv__Path, out hv_Coarse_ModelID,
                    out hv_Coarse_ModelType, out hv_Coarse_ImgIdx, out hv__ErrCode, out hv__ErrString);
                if ((int)(new HTuple(hv__ErrCode.TupleLess(0))) != 0)
                {
                    hv_o_ErrCode = hv__ErrCode.Clone();
                    hv_o_ErrStr = "Load coarse match model failed: " + hv__ErrString;
                    ho__CoarseObjs.Dispose();
                    ho__MatchReg.Dispose();
                    ho__InsReg.Dispose();
                    ho__RejReg.Dispose();
                    ho__SubRegs.Dispose();
                    ho__ImgMeans.Dispose();
                    ho__ImgStds.Dispose();
                    ho__ImgDarks.Dispose();
                    ho__ImgLights.Dispose();
                    ho__Bond1Regs.Dispose();
                    ho__Bond2Regs.Dispose();

                    return;
                }
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    hvec_o_CoarseObjs = dh.Take((
                        dh.Add(new HObjectVector(1)).Insert(0, dh.Add(new HObjectVector(ho__CoarseObjs)))));
                }
                hv_o_CoarseMatchModel = new HTuple();
                hv_o_CoarseMatchModel = hv_o_CoarseMatchModel.TupleConcat(hv_Coarse_ModelID);
                hv_o_CoarseMatchModel = hv_o_CoarseMatchModel.TupleConcat(hv_Coarse_ModelType);
                hv_o_CoarseMatchModel = hv_o_CoarseMatchModel.TupleConcat(hv_Coarse_ImgIdx);
                //
                //*********MainIC Models
                hv__Path = hv_i_ModelPath + "MainIC/";
                hv__ThreshDark = hvec_i_MainIcModelPara[0].T.Clone();
                hv__ThreshLight = hvec_i_MainIcModelPara[1].T.Clone();
                hv__SobelScale = hvec_i_MainIcModelPara[2].T.Clone();
                ho__MatchReg.Dispose(); ho__InsReg.Dispose(); ho__RejReg.Dispose(); ho__SubRegs.Dispose(); ho__ImgMeans.Dispose(); ho__ImgStds.Dispose(); ho__ImgDarks.Dispose(); ho__ImgLights.Dispose();
                HTV.HTV_read_golden_model(out ho__MatchReg, out ho__InsReg, out ho__RejReg, out ho__SubRegs,
                    out ho__ImgMeans, out ho__ImgStds, out ho__ImgDarks, out ho__ImgLights,
                    hv__Path, hv__ThreshDark, hv__ThreshLight, hv__SobelScale, out hv_Main_ModelID,
                    out hv_Main_ModelType, out hv__ErrCode, out hv__ErrString);
                HOperatorSet.ReadTuple(hv__Path + "Image_Index.tup", out hv_Main_ImgIdx);
                HOperatorSet.ReadTuple(hv__Path + "FindIc_ImgIdx.tup", out hv_FindIc_ImgIdx);
                if ((int)(new HTuple(hv__ErrCode.TupleLess(0))) != 0)
                {
                    hv_o_ErrCode = hv__ErrCode.Clone();
                    hv_o_ErrStr = "Load MainIC model failed: " + hv__ErrString;
                    ho__CoarseObjs.Dispose();
                    ho__MatchReg.Dispose();
                    ho__InsReg.Dispose();
                    ho__RejReg.Dispose();
                    ho__SubRegs.Dispose();
                    ho__ImgMeans.Dispose();
                    ho__ImgStds.Dispose();
                    ho__ImgDarks.Dispose();
                    ho__ImgLights.Dispose();
                    ho__Bond1Regs.Dispose();
                    ho__Bond2Regs.Dispose();

                    return;
                }
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    hvec_o_MainIcObjs = dh.Take(((((((((
                        dh.Add(new HObjectVector(1)).Insert(0, dh.Add(new HObjectVector(ho__MatchReg)))).Insert(
                        1, dh.Add(new HObjectVector(ho__InsReg)))).Insert(2, dh.Add(new HObjectVector(ho__RejReg)))).Insert(
                        3, dh.Add(new HObjectVector(ho__SubRegs)))).Insert(4, dh.Add(new HObjectVector(ho__ImgDarks)))).Insert(
                        5, dh.Add(new HObjectVector(ho__ImgLights)))).Insert(6, dh.Add(new HObjectVector(ho__ImgMeans)))).Insert(
                        7, dh.Add(new HObjectVector(ho__ImgStds)))));
                }
                hv_o_MainIcModel = new HTuple();
                hv_o_MainIcModel = hv_o_MainIcModel.TupleConcat(hv_Main_ModelID);
                hv_o_MainIcModel = hv_o_MainIcModel.TupleConcat(hv_Main_ModelType);
                hv_o_MainIcModel = hv_o_MainIcModel.TupleConcat(hv_Main_ImgIdx);
                hv_o_MainIcModel = hv_o_MainIcModel.TupleConcat(hv_FindIc_ImgIdx);


                //**********MainIc wire模板  为了检测IC表面金线

                hv__Path = hv_i_ModelPath + "MainICWire/";
                HOperatorSet.FileExists(hv__Path, out hv_FileExists);
                if ((int)(new HTuple(hv_FileExists.TupleEqual(1))) != 0)
                {

                    hv__ThreshDark = hvec_i_MainIcWireModelPara[0].T.Clone();
                    hv__ThreshLight = hvec_i_MainIcWireModelPara[1].T.Clone();
                    hv__SobelScale = hvec_i_MainIcWireModelPara[2].T.Clone();
                    ho__MatchReg.Dispose(); ho__InsReg.Dispose(); ho__RejReg.Dispose(); ho__SubRegs.Dispose(); ho__ImgMeans.Dispose(); ho__ImgStds.Dispose(); ho__ImgDarks.Dispose(); ho__ImgLights.Dispose();
                    HTV.HTV_read_golden_model(out ho__MatchReg, out ho__InsReg, out ho__RejReg, out ho__SubRegs,
                        out ho__ImgMeans, out ho__ImgStds, out ho__ImgDarks, out ho__ImgLights,
                        hv__Path, hv__ThreshDark, hv__ThreshLight, hv__SobelScale, out hv_Main_ModelID,
                        out hv_Main_ModelType, out hv__ErrCode, out hv__ErrString);
                    HOperatorSet.ReadTuple(hv__Path + "Image_Index.tup", out hv_Main_ImgIdx);
                    HOperatorSet.ReadTuple(hv__Path + "FindIc_ImgIdx.tup", out hv_FindIc_ImgIdx);

                    if ((int)(new HTuple(hv__ErrCode.TupleLess(0))) != 0)
                    {
                        hv_o_ErrCode = hv__ErrCode.Clone();
                        hv_o_ErrStr = "Load MainIC model failed: " + hv__ErrString;
                        ho__CoarseObjs.Dispose();
                        ho__MatchReg.Dispose();
                        ho__InsReg.Dispose();
                        ho__RejReg.Dispose();
                        ho__SubRegs.Dispose();
                        ho__ImgMeans.Dispose();
                        ho__ImgStds.Dispose();
                        ho__ImgDarks.Dispose();
                        ho__ImgLights.Dispose();
                        ho__Bond1Regs.Dispose();
                        ho__Bond2Regs.Dispose();

                        return;
                    }
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        hvec_o_MainIcWireObjs = dh.Take(((((((((
                            dh.Add(new HObjectVector(1)).Insert(0, dh.Add(new HObjectVector(ho__MatchReg)))).Insert(
                            1, dh.Add(new HObjectVector(ho__InsReg)))).Insert(2, dh.Add(new HObjectVector(ho__RejReg)))).Insert(
                            3, dh.Add(new HObjectVector(ho__SubRegs)))).Insert(4, dh.Add(new HObjectVector(ho__ImgDarks)))).Insert(
                            5, dh.Add(new HObjectVector(ho__ImgLights)))).Insert(6, dh.Add(new HObjectVector(ho__ImgMeans)))).Insert(
                            7, dh.Add(new HObjectVector(ho__ImgStds)))));
                    }
                    hv_o_MainIcWireModel = new HTuple();
                    hv_o_MainIcWireModel = hv_o_MainIcWireModel.TupleConcat(hv_Main_ModelID);
                    hv_o_MainIcWireModel = hv_o_MainIcWireModel.TupleConcat(hv_Main_ModelType);
                    hv_o_MainIcWireModel = hv_o_MainIcWireModel.TupleConcat(hv_Main_ImgIdx);
                    hv_o_MainIcWireModel = hv_o_MainIcWireModel.TupleConcat(hv_FindIc_ImgIdx);
                }
                else
                {
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        hvec_o_MainIcWireObjs = dh.Add(new HObjectVector(1));
                    }
                    hv_o_MainIcWireModel = new HTuple();
                    hv_o_MainIcWireModel[0] = "none";
                    hv_o_MainIcWireModel[1] = "none";
                    hv_o_MainIcWireModel[2] = "none";
                    hv_o_MainIcWireModel[3] = "none";
                }


                //*********MinorIC Models
                hv__Path = hv_i_ModelPath + "MinorIC/";
                HOperatorSet.FileExists(hv__Path, out hv_FileExists1);
                if ((int)(new HTuple(hv_FileExists1.TupleEqual(1))) != 0)
                {
                    hv__ThreshDark = hvec_i_MinorIcModelPara[0].T.Clone();
                    hv__ThreshLight = hvec_i_MinorIcModelPara[1].T.Clone();
                    hv__SobelScale = hvec_i_MinorIcModelPara[2].T.Clone();
                    ho__MatchReg.Dispose(); ho__InsReg.Dispose(); ho__RejReg.Dispose(); ho__SubRegs.Dispose(); ho__ImgMeans.Dispose(); ho__ImgStds.Dispose(); ho__ImgDarks.Dispose(); ho__ImgLights.Dispose();
                    HTV.HTV_read_golden_model(out ho__MatchReg, out ho__InsReg, out ho__RejReg, out ho__SubRegs,
                        out ho__ImgMeans, out ho__ImgStds, out ho__ImgDarks, out ho__ImgLights,
                        hv__Path, hv__ThreshDark, hv__ThreshLight, hv__SobelScale, out hv__ModelID,
                        out hv__ModelType, out hv__ErrCode, out hv__ErrString);
                    HOperatorSet.ReadTuple(hv__Path + "Image_Index.tup", out hv__ImgIdx);
                    HOperatorSet.ReadTuple(hv__Path + "FindIc_ImgIdx.tup", out hv_FindIc_ImgIdx);
                    if ((int)(new HTuple(hv__ErrCode.TupleLess(0))) != 0)
                    {
                        hv_o_ErrCode = hv__ErrCode.Clone();
                        hv_o_ErrStr = "Load MainIC model failed: " + hv__ErrString;
                        ho__CoarseObjs.Dispose();
                        ho__MatchReg.Dispose();
                        ho__InsReg.Dispose();
                        ho__RejReg.Dispose();
                        ho__SubRegs.Dispose();
                        ho__ImgMeans.Dispose();
                        ho__ImgStds.Dispose();
                        ho__ImgDarks.Dispose();
                        ho__ImgLights.Dispose();
                        ho__Bond1Regs.Dispose();
                        ho__Bond2Regs.Dispose();

                        return;
                    }
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        hvec_o_MinorIcObjs = dh.Take(((((((((
                            dh.Add(new HObjectVector(1)).Insert(0, dh.Add(new HObjectVector(ho__MatchReg)))).Insert(
                            1, dh.Add(new HObjectVector(ho__InsReg)))).Insert(2, dh.Add(new HObjectVector(ho__RejReg)))).Insert(
                            3, dh.Add(new HObjectVector(ho__SubRegs)))).Insert(4, dh.Add(new HObjectVector(ho__ImgDarks)))).Insert(
                            5, dh.Add(new HObjectVector(ho__ImgLights)))).Insert(6, dh.Add(new HObjectVector(ho__ImgMeans)))).Insert(
                            7, dh.Add(new HObjectVector(ho__ImgStds)))));
                    }
                    hv_o_MinorIcModel = new HTuple();
                    hv_o_MinorIcModel = hv_o_MinorIcModel.TupleConcat(hv__ModelID);
                    hv_o_MinorIcModel = hv_o_MinorIcModel.TupleConcat(hv__ModelType);
                    hv_o_MinorIcModel = hv_o_MinorIcModel.TupleConcat(hv__ImgIdx);
                }
                else
                {
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        hvec_o_MinorIcObjs = dh.Add(new HObjectVector(1));
                    }
                    hv_o_MinorIcModel = new HTuple();
                    hv_o_MinorIcModel[0] = "none";
                    hv_o_MinorIcModel[1] = "none";
                    hv_o_MinorIcModel[2] = "none";
                }


                //*********Frame Models
                hv__Path = hv_i_ModelPath + "Frame/";
                HOperatorSet.FileExists(hv__Path, out hv_FileExists2);
                if ((int)(new HTuple(hv_FileExists2.TupleEqual(1))) != 0)
                {
                    hv__ThreshDark = hvec_i_FrameModelPara[0].T.Clone();
                    hv__ThreshLight = hvec_i_FrameModelPara[1].T.Clone();
                    hv__SobelScale = hvec_i_FrameModelPara[2].T.Clone();
                    ho__MatchReg.Dispose(); ho__InsReg.Dispose(); ho__RejReg.Dispose(); ho__SubRegs.Dispose(); ho__ImgMeans.Dispose(); ho__ImgStds.Dispose(); ho__ImgDarks.Dispose(); ho__ImgLights.Dispose();
                    HTV.HTV_read_golden_model(out ho__MatchReg, out ho__InsReg, out ho__RejReg, out ho__SubRegs,
                        out ho__ImgMeans, out ho__ImgStds, out ho__ImgDarks, out ho__ImgLights,
                        hv__Path, hv__ThreshDark, hv__ThreshLight, hv__SobelScale, out hv_Main_ModelID,
                        out hv_Main_ModelType, out hv__ErrCode, out hv__ErrString);
                    HOperatorSet.ReadTuple(hv__Path + "Image_Index.tup", out hv_Main_ImgIdx);
                    HOperatorSet.ReadTuple(hv__Path + "FindIc_ImgIdx.tup", out hv_FindIc_ImgIdx);

                    if ((int)(new HTuple(hv__ErrCode.TupleLess(0))) != 0)
                    {
                        hv_o_ErrCode = hv__ErrCode.Clone();
                        hv_o_ErrStr = "Load MainIC model failed: " + hv__ErrString;
                        ho__CoarseObjs.Dispose();
                        ho__MatchReg.Dispose();
                        ho__InsReg.Dispose();
                        ho__RejReg.Dispose();
                        ho__SubRegs.Dispose();
                        ho__ImgMeans.Dispose();
                        ho__ImgStds.Dispose();
                        ho__ImgDarks.Dispose();
                        ho__ImgLights.Dispose();
                        ho__Bond1Regs.Dispose();
                        ho__Bond2Regs.Dispose();

                        return;
                    }
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        hvec_o_FrameObjs = dh.Take(((((((((
                            dh.Add(new HObjectVector(1)).Insert(0, dh.Add(new HObjectVector(ho__MatchReg)))).Insert(
                            1, dh.Add(new HObjectVector(ho__InsReg)))).Insert(2, dh.Add(new HObjectVector(ho__RejReg)))).Insert(
                            3, dh.Add(new HObjectVector(ho__SubRegs)))).Insert(4, dh.Add(new HObjectVector(ho__ImgDarks)))).Insert(
                            5, dh.Add(new HObjectVector(ho__ImgLights)))).Insert(6, dh.Add(new HObjectVector(ho__ImgMeans)))).Insert(
                            7, dh.Add(new HObjectVector(ho__ImgStds)))));
                    }
                    hv_o_FrameModel = new HTuple();
                    hv_o_FrameModel = hv_o_FrameModel.TupleConcat(hv_Main_ModelID);
                    hv_o_FrameModel = hv_o_FrameModel.TupleConcat(hv_Main_ModelType);
                    hv_o_FrameModel = hv_o_FrameModel.TupleConcat(hv_Main_ImgIdx);
                    hv_o_FrameModel = hv_o_FrameModel.TupleConcat(hv_FindIc_ImgIdx);
                }
                else
                {

                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        hvec_o_FrameObjs = dh.Add(new HObjectVector(1));
                    }
                    hv_o_FrameModel = new HTuple();
                    hv_o_FrameModel[0] = "none";
                    hv_o_FrameModel[1] = "none";
                    hv_o_FrameModel[2] = "none";
                    hv_o_FrameModel[3] = "none";
                }



                //**********BondWires
                hv__Path = hv_i_ModelPath + "BondWire/";
                ho__Bond1Regs.Dispose(); ho__Bond2Regs.Dispose();
                HTV.HTV_read_bond_wire_model(out ho__Bond1Regs, out ho__Bond2Regs, hv__Path, out hv__Bond1OnIc,
                    out hv__Bond1ImgIdx, out hv__Bond2OnIc, out hv__Bond2ImgIdx, out hv__Bond2BallNums,
                    out hv_Bond_ModelID, out hv_Bond_ModelType, out hv__WireImgIdx, out hv__ErrCode,
                    out hv__ErrString);
                if ((int)(new HTuple(hv__ErrCode.TupleLess(0))) != 0)
                {
                    hv_o_ErrCode = hv__ErrCode.Clone();
                    hv_o_ErrStr = "Load BOND WIRE model failed: " + hv__ErrString;
                    ho__CoarseObjs.Dispose();
                    ho__MatchReg.Dispose();
                    ho__InsReg.Dispose();
                    ho__RejReg.Dispose();
                    ho__SubRegs.Dispose();
                    ho__ImgMeans.Dispose();
                    ho__ImgStds.Dispose();
                    ho__ImgDarks.Dispose();
                    ho__ImgLights.Dispose();
                    ho__Bond1Regs.Dispose();
                    ho__Bond2Regs.Dispose();

                    return;
                }
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    hvec_o_BondWireObjs = dh.Take(((
                        dh.Add(new HObjectVector(1)).Insert(0, dh.Add(new HObjectVector(ho__Bond1Regs)))).Insert(
                        1, dh.Add(new HObjectVector(ho__Bond2Regs)))));
                }
                hvec_o_BondWireModel = (((((((new HTupleVector(1).Insert(0, new HTupleVector(hv__Bond1OnIc))).Insert(
                    1, new HTupleVector(hv__Bond1ImgIdx))).Insert(2, new HTupleVector(hv__Bond2OnIc))).Insert(
                    3, new HTupleVector(hv__Bond2ImgIdx))).Insert(4, new HTupleVector(hv__Bond2BallNums))).Insert(
                    5, new HTupleVector(hv__WireImgIdx))).Insert(6, new HTupleVector(new HTuple(hv_Bond_ModelID.TupleConcat(
                    hv_Bond_ModelType)))));
                ho__CoarseObjs.Dispose();
                ho__MatchReg.Dispose();
                ho__InsReg.Dispose();
                ho__RejReg.Dispose();
                ho__SubRegs.Dispose();
                ho__ImgMeans.Dispose();
                ho__ImgStds.Dispose();
                ho__ImgDarks.Dispose();
                ho__ImgLights.Dispose();
                ho__Bond1Regs.Dispose();
                ho__Bond2Regs.Dispose();

                return;
            }
            catch (HalconException HDevExpDefaultException)
            {
                ho__CoarseObjs.Dispose();
                ho__MatchReg.Dispose();
                ho__InsReg.Dispose();
                ho__RejReg.Dispose();
                ho__SubRegs.Dispose();
                ho__ImgMeans.Dispose();
                ho__ImgStds.Dispose();
                ho__ImgDarks.Dispose();
                ho__ImgLights.Dispose();
                ho__Bond1Regs.Dispose();
                ho__Bond2Regs.Dispose();

                throw HDevExpDefaultException;
            }
        }


        public void JSCC_AOI_clear_all_model(HTuple hv_i_CoarseMatchModel, HTuple hv_i_MainIcModel,
       HTuple hv_i_MainIcWireModel, HTuple hv_i_MinorIcModel, HTupleVector/*{eTupleVector,Dim=1}*/ hvec_i_BondWireModel,
       HTuple hv_i_FrameModel, out HTuple hv_o_ErrCode, out HTuple hv_o_ErrStr)
        {



            // Local iconic variables 

            // Local control variables 

            HTuple hv_Index = new HTuple(), hv_Exception = null;
            // Initialize local and output iconic variables 
            hv_o_ErrCode = 0;
            hv_o_ErrStr = "";
            try
            {
                //------Coarse Match
                HTV.HTV_clear_model(hv_i_CoarseMatchModel.TupleSelect(0), hv_i_CoarseMatchModel.TupleSelect(
                    1));
                //------MainIC
                HTV.HTV_clear_model(hv_i_MainIcModel.TupleSelect(0), hv_i_MainIcModel.TupleSelect(
                    1));

                //------MainICWire
                HTV.HTV_clear_model(hv_i_MainIcWireModel.TupleSelect(0), hv_i_MainIcModel.TupleSelect(
                    1));

                //------MinorIc
                if ((int)(new HTuple(((hv_i_MinorIcModel.TupleSelect(0))).TupleNotEqual("none"))) != 0)
                {
                    HTV.HTV_clear_model(hv_i_MinorIcModel.TupleSelect(0), hv_i_MinorIcModel.TupleSelect(
                        1));
                }

                //-------Frame
                if ((int)(new HTuple(((hv_i_FrameModel.TupleSelect(0))).TupleNotEqual("none"))) != 0)
                {
                    HTV.HTV_clear_model(hv_i_FrameModel.TupleSelect(0), hv_i_FrameModel.TupleSelect(
                        1));
                }

                //------BondWire
                HOperatorSet.TupleFindFirst(hvec_i_BondWireModel[2].T, 0, out hv_Index);
                if ((int)(new HTuple(hv_Index.TupleGreaterEqual(0))) != 0)
                {
                    HTV.HTV_clear_model((hvec_i_BondWireModel[6].T).TupleSelect(0), (hvec_i_BondWireModel[6].T).TupleSelect(
                        1));
                }
            }
            // catch (Exception) 
            catch (HalconException HDevExpDefaultException1)
            {
                HDevExpDefaultException1.ToHTuple(out hv_Exception);
                hv_o_ErrStr = -1;
                hv_o_ErrStr = hv_Exception.Clone();
            }

            return;
        }
        #endregion
    }
}