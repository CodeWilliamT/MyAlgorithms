#define visionflow  //定义代表使用算法流，屏蔽使用传统代码

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
using VisionFlow;
using VisionFlow_Interface;
using VisionFlow_Funclib;

namespace LeadframeAOI
{
    /// <summary>
    /// 芯片检测结果
    /// </summary>
    public struct StructInspectResult
    {
        /// <summary>芯片是否OK</summary>
        public bool OkOrNg;
        /// <summary>芯片偏移X</summary>
        public double deltaX;
        /// <summary>芯片偏移Y</summary>
        public double deltaY;
        /// <summary>芯片上缺陷类型的数组,索引为缺陷号</summary>
        public HTuple defectType;
        /// <summary>芯片上缺陷对饮图片的数组,索引为缺陷号</summary>
        public HTuple defectImgIdx;
        /// <summary>芯片的世界坐标x,单位mm</summary>
        public double x;
        /// <summary>芯片的世界坐标y,单位mm</summary>
        public double y;
        /// <summary>芯片在料片上排布的纵坐标</summary>
        public int realRow;
        /// <summary>芯片在料片上排布的横坐标</summary>
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
            public double DirtyArea;
            public override string ToString()
            {
                string str = "";
                //str = "主IC缺陷" + ":缺陷个数" + Number + ",最大长度=" + MaxLength + "Pix,最大宽度=" + MaxWidth + "Pix,最大面积=" + MaxArea + "Pix;";
                str = "主IC缺陷" + ":" + "脏污面积=" + DirtyArea + "Pix2;";
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
                //str = "崩边缺陷" + ":"+"缺陷个数" + Number + ",最大长度=" + MaxLength + "Pix,最大宽度=" + MaxWidth + "Pix,最大面积=" + MaxArea + "Pix;";
                str = "崩边缺陷" + ":"+"最大面积=" + MaxArea + "Pix;";
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
            public bool Discovered;
            public HTuple E2C_Ratio;
            public HTuple Lenth_EpoxyOut;
            public override string ToString()
            {
                string str = "";
                //str = "银胶缺陷" + ":" + (UpOut ? "上" : "") + (DownOut ? "下" : "") + (LeftOut ? "左" : "") + (RightOut ? "右" : "") + "溢胶;";
                str = "银胶缺陷" + ":" +"上侧:溢胶距:"+ E2C_Ratio[0]+ ",溢胶截面芯片比:"+ Lenth_EpoxyOut[0]
                    +";"+"下侧:溢胶距:" + E2C_Ratio[1] + ",溢胶截面芯片比:" + Lenth_EpoxyOut[1] + ";"+
                    "左侧:溢胶距:" + E2C_Ratio[2] + ",溢胶截面芯片比:" + Lenth_EpoxyOut[2] + ";"+
                    "右侧:溢胶距:" + E2C_Ratio[3] + ",溢胶截面芯片比:" + Lenth_EpoxyOut[3] + ";";
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
                //str = "IC偏移缺陷" + ":偏移行=" + RowDiff + "Pix,偏移列=" + ColDiff + "Pix,偏移欧式距离=" + DistanceDiff + "Pix,偏移角度=" + AngleDiff + "(弧度);";

                str = "IC偏移缺陷" + ":偏移行=" + RowDiff + "Pix,偏移列=" + ColDiff + "Pix,偏移角度=" + AngleDiff + "(弧度);";
                return str;
            }
            public string ToString(int Idx)
            {
                string str = "";
                switch(Idx)
                {
                    case 0:
                        str = "IC偏移缺陷" + ":偏移行=" + RowDiff + "Pix,偏移列=" + ColDiff + "Pix;";
                        break;
                    case 1:
                        str = "IC偏移缺陷" + ":偏移角度=" + AngleDiff + "(弧度);";
                        break;
                }
                return str;
            }
        }

        public class BondWiresInspect
        {
            public HTuple Radius_FirstBond = new HTuple();
            public HTuple Distance_WireBreak = new HTuple();
            public HTuple Radius_SecondBond = new HTuple();
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
                if (Radius_SecondBond != null && Radius_SecondBond.Length != 0)
                {

                    str += "第二焊点匹配分数:";
                    for (int i = 0; i < Radius_SecondBond.Length; i++)
                    {
                        str += "焊点[" + i + "]:" + Radius_SecondBond[i];
                        if (i < Radius_SecondBond.Length - 1) str += "(0-1.00),";
                    }
                    str += ";";
                }
                return str;
            }
            /// <summary>
            /// 0：第一焊球半径，1：最大断线距离,2:第二焊球半径
            /// </summary>
            /// <param name="Idx">0：第一焊球半径，1：最大断线距离,2:第二焊球半径</param>
            /// <returns></returns>
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
                        if (Radius_SecondBond != null && Radius_SecondBond.Length != 0)
                        {

                            str += "第二焊点半径:";
                            for (int i = 0; i < Radius_SecondBond.Length; i++)
                            {
                                str += "焊点[" + i + "]:" + Radius_SecondBond[i];
                                if (i < Radius_SecondBond.Length - 1) str += "Pix,";
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
                case 3:
                    str = icLoctionDiffs.ToString(1);
                    break;
                case 2:
                case 4:
                    str = icLoctionDiffs.ToString(0);
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
        public  Params P = new Params();
        public List<Network> list_net = new List<Network>();
        NetWorkResult networkResult = new NetWorkResult();
        Data data = new Data();

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

#if visionflow //两个读模板函数本质上没有差异，主要取决于内部函数路径
            VisionFlow_Funclib.FuncLib.JSCC_AOI_read_all_model(out hvec__CoarseMatchObj, out hvec__MainIcObjs, out hvec__MainIcWireObjs, out hvec__MinorIcObjs, out hvec__FrameObjs,
                      out hvec__BondWireObjs, new HTuple(ModelPath), _MainIcModelPara, _MainIcWireModelPara, _MinorIcModelPara, _FrameModelPara,
                      out hv__CoarseModel, out hv__MainIcModel, out hv__MainIcWireModel, out hv__MinorIcModel, out hv__FrameModel, out hvec__BondWireModel,
                      out _ErrCode, out _ErrString);
#else
            JSCC_AOI_read_all_model(out hvec__CoarseMatchObj, out hvec__MainIcObjs, out hvec__MainIcWireObjs, out hvec__MinorIcObjs, out hvec__FrameObjs,
                      out hvec__BondWireObjs, new HTuple(ModelPath), _MainIcModelPara, _MainIcWireModelPara, _MinorIcModelPara, _FrameModelPara,
                      out hv__CoarseModel, out hv__MainIcModel, out hv__MainIcWireModel, out hv__MinorIcModel, out hv__FrameModel, out hvec__BondWireModel,
                      out _ErrCode, out _ErrString);
#endif

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
#if visionflow
                VisionFlow_Funclib.FuncLib.JSCC_AOI_clear_all_model(hv__CoarseModel, hv__MainIcModel, hv__MainIcWireModel, hv__MinorIcModel,
          hvec__BondWireModel, hv__FrameModel, out _ErrCode, out _ErrString);
#else
                JSCC_AOI_clear_all_model(hv__CoarseModel, hv__MainIcModel, hv__MainIcWireModel, hv__MinorIcModel,
          hvec__BondWireModel, hv__FrameModel, out _ErrCode, out _ErrString);
#endif
                if ((int)(new HTuple(_ErrCode.TupleLess(0))) != 0)
                {
                    errString = _ErrString.S;
                    return false;
                }
                isModelRead = false;
            }
            return true;
        }
        public void DisposeNet()
        {
            if (list_net != null)
            {
                if (list_net.Count != 0) list_net[0].Dispose();
                list_net.Clear();
            }
        }
        /// <summary>
        /// 将模板的参数赋值给P
        /// </summary>
        public void ModelParaConfig()
        {

            P.hv_CoarseModel = hv__CoarseModel;

            P.hv_MainIcModel = hv__MainIcModel;
            P.hvec__MainIcObjs = hvec__MainIcObjs;

            P.hv_MinorIcModel = hv__MinorIcModel;
            P.hvec__MinorIcObjs = hvec__MinorIcObjs;

            P.hv_MainWireModel = hv__MainIcWireModel;
            P.hvec__MainWireObjs = hvec__MainIcWireObjs;

            P.hv_FrameModel = hv__FrameModel;
            P.hvec__FrameObjs = hvec__FrameObjs;

            P.hvec_BondWireModel = hvec__BondWireModel;
            P.hvec__BondWireObjs = hvec__BondWireObjs;

            P.BondNumber = hvec__BondWireObjs.At(0).O.CountObj();
            networkResult.wireDefectValue = new HTupleVector(1);

            //HObjectVector wireFailReg = new HObjectVector(1);
            //HTuple wire__DefectType = new HTuple();
            //HTuple wire__DefectImgIdx = new HTuple();
            //HTupleVector wireDefectValue = new HTupleVector(1);
            HObject EmptyRegion;
            HOperatorSet.GenEmptyRegion(out EmptyRegion);

            for (int i = 0; i < 3 * P.BondNumber - 1; i++)
            {
                networkResult.wireFailReg[i] = new HObjectVector((EmptyRegion).CopyObj(1, -1));
                networkResult.wire__DefectType[i] = -2;
                networkResult.wire__DefectImgIdx[i] = -2;
                if (i >= 2 * P.BondNumber)
                    networkResult.wireDefectValue[i] = new HTupleVector(new HTuple(-2));
                else
                    networkResult.wireDefectValue[i] = new HTupleVector(((new HTuple(-2)).TupleConcat(-2)).TupleConcat(-2));

            }
            EmptyRegion.Dispose();
        }


#if visionflow
        public void Initialization(int M, string param_ini, string model_path, string funcLib_path, string json_path)
        {
            //需要先释放所有list_net
            DisposeNet();

            if (P.Load(param_ini))
            {
                if (ReadAllModel(model_path))
                {
                    ModelParaConfig();
                    for (int i = 0; i < M; i++)
                    {
                        Network item_net = new Network();
                        if (AlgInterface.LoadNetwork(item_net, funcLib_path, json_path))
                        {
                            if (AlgInterface.ParaConfigure(item_net, P, networkResult))
                            {
                                list_net.Add(item_net);
                            }
                            else
                            {
                                throw new Exception("网络赋值失败");
                            }
                        }
                        else
                        {
                            throw new Exception("加载网络图失败");
                        }
                    }
                }
                else
                {

                    throw new Exception("加载模板失败");
                    //加载模板失败
                }
            }

            else
            {
                throw new Exception("加载参数失败");
                //加载参数失败
            }
        }

#else
        /// <summary>
        /// 初始化函数 读取模板 
        /// </summary>
        /// <param name="M"></param>
        /// <param name="param_ini"></param>
        /// <param name="model_path"></param>
        /// <param name="funcLib_path"></param>
        /// <param name="json_path"></param>
        public void Initialization(int M, string param_ini, string model_path, string funcLib_path, string json_path)
        {
   
            if (P.Load(param_ini))
            {
                if (ReadAllModel(model_path))
                {
                    ModelParaConfig();  
                }
                else
                {

                    throw new Exception("加载模板失败");
                    //加载模板失败
                }
            }

            else
            {
                throw new Exception("加载参数失败");
                //加载参数失败
            }
        }

#endif

        /// <summary>
        /// <param name="NetIdx">算法网络索引-多线程用</param> 
        /// <param name="Images"> 传入算法的检测图（多通道Concat在一起的图）</param>
        /// <param name="List_CoarseReg">传入算法的芯片检测区域，索引代表视野</param> 
        /// <param name="_Row">传入算法的_CoarseReg的区域的中心坐标</param> 
        /// <param name="_Col">传入算法的_CoarseReg的区域的中心坐标</param> 
        /// <param name="FailRegions">检测结果的缺陷区域，是以一幅图为单位（已没用）</param> 
        /// <param name="Wires">检测结果的金线，是以一幅图为单位（已没用</param> 
        /// <param name="_VWires">检测结果的金线，是以一个芯片为单位，是一个HObjectVector</param> 
        /// <param name="_VFailRegs">检测结果的缺陷区域，是以一个芯片为单位，是一个HObjectVector,List号为芯片号，Hobject号为缺陷序号</param> 
        /// <param name="Results">检测结果数据([ok or ng],[row],[col],[defectImgIdx],[defectType],[InspectDetail])，
        /// Lits号对芯片号,内部HTulpe[defectImgIdx],[defectType]索引号对应缺陷号，defectImgIdx数据指的是总通道号</param> 
        /// <returns></returns>
        public bool Inspection(int NetIdx, HObject Images, List<HObject> List_CoarseReg, HTuple _Row, HTuple _Col, out HObject FailRegions, out HObject Wires, out HObjectVector _VWires,
            out HObjectVector _VFailRegs, out List<StructInspectResult> Results)
        {
            HOperatorSet.GenEmptyObj(out FailRegions);
            HOperatorSet.GenEmptyObj(out Wires);
            _VWires = new HObjectVector(1);
            _VFailRegs = new HObjectVector(1);
            Results = new List<StructInspectResult>();

            HObject AllImages = new HObject();
            HTuple tempresult = new HTuple();

            HObject FailReg = new HObject(), Wire = new HObject();


            HTuple NumImage;
            HOperatorSet.CountObj(Images, out NumImage);
            for (int ImgIdx = 1; ImgIdx <= NumImage; ImgIdx++)
            {
                HObject objectSelected;
                HTuple channels;
                HOperatorSet.SelectObj(Images, out objectSelected, ImgIdx);
                HOperatorSet.CountChannels(objectSelected, out channels);
                if (channels == 3)
                {
                    HObject ImageWire, Green, ImageIc;
                    HOperatorSet.Decompose3(objectSelected, out ImageWire, out Green, out ImageIc);
                    HOperatorSet.ConcatObj(ImageWire, Green, out AllImages);
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.ConcatObj(AllImages, ImageIc, out ExpTmpOutVar_0);
                        AllImages.Dispose();
                        AllImages = ExpTmpOutVar_0;
                    }
                    objectSelected.Dispose();
                    ImageWire.Dispose();
                    Green.Dispose();
                    ImageIc.Dispose();
                }
                else
                {
                    AllImages = Images.CopyObj(1, -1);
                }
            }


            for (int _CoarseRegIdx = 0; _CoarseRegIdx < List_CoarseReg.Count; _CoarseRegIdx++)

            {
                HObject _CoarseReg = List_CoarseReg[_CoarseRegIdx];
                HTuple NumCosrse;
                HOperatorSet.CountObj(_CoarseReg, out NumCosrse);

                for (int i = 0; i <= NumCosrse - 1; i++)
                {
                    HObject _CoarseRegSelected;
                    HOperatorSet.SelectObj(_CoarseReg, out _CoarseRegSelected, i + 1);
                    data.allImages = AllImages;
                    data.coarseReg = _CoarseRegSelected;

                    AlgInterface.FeedData(list_net[NetIdx], data);

                    if (list_net[NetIdx].Run())
                    {
                        HTuple _DefectType, _DefectImgIdx;
                        HTupleVector _coarseDefectValue, _mainEpoxyDefectValue, _wireDefectValue,_mainIcDefectValue, _mainIcWireDefectValue, _frameDefectValue;
                        AlgInterface.GetNetWorkResult(list_net[NetIdx], out FailReg, out Wire, out tempresult, out _DefectType, out _DefectImgIdx, out _coarseDefectValue, out _mainEpoxyDefectValue, out _wireDefectValue, out _mainIcDefectValue, out _mainIcWireDefectValue, out  _frameDefectValue);

                        //结果先判断一下
                        int b = FailReg.CountObj();

                        if (_DefectType.Length != b)
                        {
                            throw new Exception("第" + i + "个检测区错误区域与错误码长度不一致");
                        }

                        if (_DefectImgIdx.Length != _DefectType.Length)
                        {
                            throw new Exception("第" + i + "个错误图号与错误码长度不一致");
                        }



                        _VFailRegs[i] = new HObjectVector(FailReg.CopyObj(1, -1));
                        _VWires[i] = new HObjectVector(Wire.CopyObj(1, -1));
                        HObject UnionFaileReg;
                        HOperatorSet.Union1(FailReg, out UnionFaileReg);
                        {
                            HObject ExpTmpOutVar_0;
                            HOperatorSet.ConcatObj(FailRegions, UnionFaileReg, out ExpTmpOutVar_0);
                            FailRegions.Dispose();
                            FailRegions = ExpTmpOutVar_0;
                        }
                        FailReg.Dispose();
                        UnionFaileReg.Dispose();

                        HOperatorSet.ConcatObj(Wires, Wire, out Wires);
                        Wire.Dispose();

                        ////临时使用
                        //HTuple temImgIdx = new HTuple();
                        //for (int idx =0;idx< _DefectImgIdx.Length;idx++)
                        //{
                        //    if (networkResult.DefectImgIdx[idx] == 0)
                        //        temImgIdx[idx] = 2;
                        //    if(networkResult.DefectImgIdx[idx] == 1)
                        //        temImgIdx[idx] = 0;

                        //}


                        StructInspectResult itemResult = new StructInspectResult() { OkOrNg = (tempresult == 1), realRow = _Row[i], realCol = _Col[i], defectImgIdx = _DefectImgIdx, defectType = _DefectType };

                        itemResult.inspectDetail = new InspectDetail();

                        itemResult.inspectDetail.mainIcInspects.DirtyArea = _mainIcDefectValue.At(1).T.D;
                        itemResult.inspectDetail.icLoctionDiffs.ColDiff = _mainIcDefectValue.At(2).T[0].D;
                        itemResult.inspectDetail.icLoctionDiffs.RowDiff = _mainIcDefectValue.At(2).T[1].D;
                        itemResult.inspectDetail.icLoctionDiffs.AngleDiff = _mainIcDefectValue.At(3).T.D;

                        itemResult.inspectDetail.epoxyInspect.E2C_Ratio = _mainEpoxyDefectValue.At(0).T;
                        itemResult.inspectDetail.epoxyInspect.Lenth_EpoxyOut = _mainEpoxyDefectValue.At(1).T;

                        itemResult.inspectDetail.chipInspects.MaxArea = _frameDefectValue.At(0).T.D;

                        int temBW = _wireDefectValue.Length / 3;
                        for (int BW = 0; BW < temBW - 1; BW++)
                        {
                            itemResult.inspectDetail.bondWiresInspects.Radius_FirstBond.Append(_wireDefectValue.At(BW).T[2]);
                            itemResult.inspectDetail.bondWiresInspects.Radius_SecondBond.Append(_wireDefectValue.At(BW + temBW).T[2]);
                            itemResult.inspectDetail.bondWiresInspects.Distance_WireBreak.Append(_wireDefectValue.At(BW + 2 * temBW).T);

                        }

                        Results.Add(itemResult);

                    }
                    else
                    {
                        throw new Exception("第" + (i + 1) + "个芯片网络执行失败\n 错误说明如下\n" + list_net[NetIdx].LastErrorString);
                    }
                    _CoarseRegSelected.Dispose();

                }

            }

            AllImages.Dispose();
            return true;




        }

#region 私有方法 不使用算法流时调用
        private void JSCC_AOI_read_all_model(out HObjectVector/*{eObjectVector,Dim=1}*/ hvec_o_CoarseObjs,
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
        private void JSCC_AOI_clear_all_model(HTuple hv_i_CoarseMatchModel, HTuple hv_i_MainIcModel,
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

#endregion

    }

}
