using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;
using ToolKits.DataManage.Text;
using ToolKits.FunctionModule;

namespace ToolKits.Map
{
    public class MapParse
    {
        private const string ConvertMap = "ConvertMap";
        public static string Str_TrimSpace(string buf)
        {
            string l_strResult = buf.Replace("\n", "").Replace(" ", "").Replace("\t", "").Replace("\r", "").Replace("\f", "").Replace("\v", "");
            return l_strResult;
        }
        public static short[][] Array2DInit(int row, int col)
        {
            short[][] chrArray = new short[row][];

            for (int i = 0; i < row; i++)
            {
                chrArray[i] = new short[col];
            }

            return chrArray;
        }
        public static string[][] Array2DInitStr(int row, int col)
        {
            string[][] chrArray = new string[row][];

            for (int i = 0; i < row; i++)
            {
                chrArray[i] = new string[col];
            }

            return chrArray;
        }
        /// <summary>
        /// Map解析
        /// </summary>
        /// <param name="mapFilePath"></param>
        /// <param name="mapType"></param>
        /// <param name="pickStr"></param>
        /// <param name="oneByOne"></param>
        /// <param name="blankStr"></param>
        /// <param name="defectStr"></param>
        /// <param name="forbiddenStr"></param>
        /// <param name="mapColor"></param>
        /// <param name="mapRefDie"></param>
        /// <param name="rotMode"></param>
        /// <param name="mirMode"></param>
        /// <param name="path"></param>
        /// <param name="mapInfo"></param>
        /// <returns></returns>
        public static bool Parse(string mapFilePath,
                                 string mapType,
                                 Dictionary<short, MapBinInfo> dMapBinInfo,
                                 MapRotateModeEnum rotMode,
                                 MapMirrorModeEnum mirMode,
                                 MapMovePathEnum path,
                                 ref MapInfo mapInfo)
        {
            if (dMapBinInfo != null && ReParseBinInfo(ref mapInfo, dMapBinInfo) == false)
                return false;

            if (Parse_pre(mapFilePath, mapType, ref mapInfo) == false)
                return false;

            if (Rotate(ref mapInfo, rotMode) == false)
                return false;

            if (Mirror(ref mapInfo, mirMode) == false)
                return false;

            //下面暂时不调试
            //if (ReParseWorkPath(ref mapInfo, path) == false)
            //    return false;

            return true;
        }
        /// <summary>
        /// Map初始解析或者默认解析
        /// </summary>
        /// <param name="mapFilePath"></param>
        /// <param name="mapInfo"></param>
        /// <returns></returns>
        public static bool Parse_pre(string mapFilePath, string mapType, ref MapInfo mapInfo)
        {
            if (mapType == MapTextFrame.JCAP_Tma)
            {
                return Parse_JCAP_Tma(mapFilePath, ref mapInfo);
            }
            else if (mapType == MapTextFrame.JCAP_Sinf)
            {
                return Parse_JCAP_Sinf(mapFilePath, ref mapInfo);
            }
            else if (mapType == MapTextFrame.FST_Txt)
            {
                return Parse_FST_Txt(mapFilePath, ref mapInfo);
            }
            else if (mapType == MapTextFrame.SLW_Txt)
            {
                return Parse_SLW_Txt(mapFilePath, ref mapInfo);
            }
            else
                return false;
        }
        /// <summary>
        /// 保存Map
        /// </summary>
        /// <param name="mapFilePath"></param>
        /// <param name="destMapFilePath"></param>
        /// <param name="mapInfo"></param>
        /// <returns></returns>
        public static bool Parse_Save(string destMapFilePath, string mapType, bool isConvertMap, MapInfo mapInfo, Dictionary<short, MapBinInfo> dMapBinInfo = null)
        {
            try
            {
                //根据后缀做出选择解析方式
                //System.Text.RegularExpressions.Regex.IsMatch(fileName, @"tma$")
                if (mapType == MapTextFrame.JCAP_Tma)
                {
                    return Save_JCAP_Tma(destMapFilePath, isConvertMap, mapInfo, dMapBinInfo);
                }
                else if (mapType == MapTextFrame.JCAP_Sinf)
                {
                    return Save_JCAP_Sinf(destMapFilePath, isConvertMap, mapInfo, dMapBinInfo);
                }
                else if (mapType == MapTextFrame.FST_Txt)
                {
                    return Save_FST_Txt(destMapFilePath, isConvertMap, mapInfo, dMapBinInfo);
                }
                else if (mapType == MapTextFrame.SLW_Txt)
                {
                    return Save_SLW_Txt(destMapFilePath, isConvertMap, mapInfo, dMapBinInfo);
                }
                else
                    return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
        
        #region 解析各类图谱
        /// <summary>
        /// 解析长电tma格式的Map
        /// </summary>
        /// <param name="mapFilePath"></param>
        /// <param name="mapInfo"></param>
        /// <returns></returns>
        public static bool Parse_JCAP_Tma(string mapFilePath, ref MapInfo mapInfo)
        {
            try
            {
                if (!File.Exists(mapFilePath)) return false;
                try
                {
                    mapInfo.mapType = MapTextFrame.JCAP_Tma;
                    mapInfo.SrcPath = mapFilePath;


                    List<string> LstStr = new List<string>();
                    textManipulate.ReadData(mapFilePath, LstStr);
                    if (LstStr.Count < 1) return false;

                    if (Str_TrimSpace(LstStr[0]) == ConvertMap)
                    {
                        mapInfo.IsConvertMap = true;
                    }
                    else mapInfo.IsConvertMap = false;
                    //对类型进行检查。第三行开始，标志位有'|'
                    int binCodeLength = mapInfo.IsConvertMap ? 3 : 1;
                    int rowIndex = mapInfo.IsConvertMap ? 3 : 2;
                    int idx = 0;
                    if (LstStr[rowIndex].Contains("|") == false)
                        return false;
                    else
                        idx = LstStr[rowIndex].IndexOf("|");

                    //对行列值预解析
                    int row = 0, col = 0;
                    string noSpaceStr = Str_TrimSpace(LstStr[rowIndex]);
                    string noNumStr = noSpaceStr.Substring(idx + 1);
                    col = noNumStr.Length / binCodeLength;
                    for (int i = rowIndex; i < LstStr.Count; i++)
                    {
                        if (LstStr[i][idx] == '|')
                            row++;
                    }
                    mapInfo.mapRow = row;
                    mapInfo.mapCol = col;

                    //给字符数组分配内存
                    if (mapInfo.mapArr != null)
                    {
                        mapInfo.mapArr = null;
                    }
                    mapInfo.mapArr = new short[mapInfo.mapRow][];

                    //解析字符数组
                    row = 0;
                    string binCode = "";
                    for (int i = rowIndex; i < mapInfo.mapRow + rowIndex; i++)
                    {
                        noSpaceStr = Str_TrimSpace(LstStr[i]);
                        noNumStr = noSpaceStr.Substring(idx + 1);
                        mapInfo.mapArr[row] = new short[mapInfo.mapCol];
                        for (int j = 0; j < mapInfo.mapCol; j++)
                        {
                            binCode = noNumStr.Substring(j * binCodeLength, binCodeLength);
                            if (mapInfo.IsConvertMap)
                                mapInfo.mapArr[row][j] = Convert.ToInt16(binCode);
                            else
                            {
                                int ind = mapInfo.binInfo.Values.ToList().FindIndex(p => p.Bin == binCode);
                                if (ind < 0)//不存在bin
                                    mapInfo.mapArr[row][j] = DieGradeDefault.BLANK_DIE;
                                else
                                    mapInfo.mapArr[row][j] = mapInfo.binInfo.ElementAt(ind).Value.Grade;
                            }
                        }
                        row++;
                    }
                    LstStr.Clear();
                }
                catch (System.Exception)
                {

                }
            }
            catch (IOException)
            {

            }

            return true;
        }
        /// <summary>
        /// 解析长电sinf格式的Map
        /// </summary>
        /// <param name="mapFilePath"></param>
        /// <param name="mapInfo"></param>
        /// <returns></returns>
        public static bool Parse_JCAP_Sinf(string mapFilePath, ref MapInfo mapInfo)
        {
            try
            {
                if (!File.Exists(mapFilePath)) return false;
                try
                {
                    mapInfo.mapType = MapTextFrame.JCAP_Sinf;
                    mapInfo.SrcPath = mapFilePath;

                    List<string> LstStr = new List<string>();
                    textManipulate.ReadData(mapFilePath, LstStr);
                    if (LstStr.Count < 1) return false;

                    if (Str_TrimSpace(LstStr[0]) == ConvertMap)
                    {
                        mapInfo.IsConvertMap = true;
                    }
                    else mapInfo.IsConvertMap = false;

                    //对类型进行检查。第13行开始，标志位有'RowData'
                    int binCodeLength = mapInfo.IsConvertMap ? 3 : 2;
                    int rowIndex = mapInfo.IsConvertMap ? 13 : 12;
                    int headerInddex = mapInfo.IsConvertMap ? 1 : 0;
                    int idx = 0;
                    if (LstStr[rowIndex].Contains("RowData") == false)
                        return false;
                    else
                        idx = LstStr[rowIndex].IndexOf(":");

                    ((MapHeaderJCAPSinf)mapInfo.mapHeaderInfo).DEVICE = LstStr[headerInddex].Split(':')[1];
                    ((MapHeaderJCAPSinf)mapInfo.mapHeaderInfo).LOT = LstStr[headerInddex + 1].Split(':')[1];
                    ((MapHeaderJCAPSinf)mapInfo.mapHeaderInfo).WAFER = LstStr[headerInddex + 2].Split(':')[1];
                    ((MapHeaderJCAPSinf)mapInfo.mapHeaderInfo).FNLOC = LstStr[headerInddex + 3].Split(':')[1];
                    ((MapHeaderJCAPSinf)mapInfo.mapHeaderInfo).ROWCT = LstStr[headerInddex + 4].Split(':')[1];
                    ((MapHeaderJCAPSinf)mapInfo.mapHeaderInfo).COLCT = LstStr[headerInddex + 5].Split(':')[1];
                    ((MapHeaderJCAPSinf)mapInfo.mapHeaderInfo).BCEQU = LstStr[headerInddex + 6].Split(':')[1];
                    ((MapHeaderJCAPSinf)mapInfo.mapHeaderInfo).REFPX = LstStr[headerInddex + 7].Split(':')[1];
                    ((MapHeaderJCAPSinf)mapInfo.mapHeaderInfo).REFPY = LstStr[headerInddex + 8].Split(':')[1];
                    ((MapHeaderJCAPSinf)mapInfo.mapHeaderInfo).DUTMS = LstStr[headerInddex + 9].Split(':')[1];
                    ((MapHeaderJCAPSinf)mapInfo.mapHeaderInfo).XDIES = LstStr[headerInddex + 10].Split(':')[1];
                    ((MapHeaderJCAPSinf)mapInfo.mapHeaderInfo).YDIES = LstStr[headerInddex + 11].Split(':')[1];

                    //设置参考点
                    RefDie refDie = new RefDie(Convert.ToInt32(((MapHeaderJCAPSinf)mapInfo.mapHeaderInfo).REFPY) - 1,
                                               Convert.ToInt32(((MapHeaderJCAPSinf)mapInfo.mapHeaderInfo).REFPX) - 1);
                    mapInfo.RefDie.Add(refDie);
                    //设置Wafer方向
                    mapInfo.waferDir = (WaferDirEnum)(Convert.ToInt32(((MapHeaderJCAPSinf)mapInfo.mapHeaderInfo).FNLOC) / 90);

                    //对行列值预解析
                    int row = 0, col = 0;
                    string noSpaceStr = Str_TrimSpace(LstStr[rowIndex]);
                    string noNumStr = noSpaceStr.Substring(idx + 1);
                    col = noNumStr.Length / binCodeLength;
                    for (int i = rowIndex; i < LstStr.Count; i++)
                    {
                        if (LstStr[i].Contains("RowData"))
                            row++;
                    }
                    mapInfo.mapRow = row;
                    mapInfo.mapCol = col;

                    //给字符数组分配内存
                    if (mapInfo.mapArr != null)
                    {
                        mapInfo.mapArr = null;
                    }
                    mapInfo.mapArr = new short[mapInfo.mapRow][];

                    //解析字符数组
                    row = 0;
                    string binCode = "";
                    for (int i = rowIndex; i < mapInfo.mapRow + rowIndex; i++)
                    {
                        noSpaceStr = Str_TrimSpace(LstStr[i]);
                        noNumStr = noSpaceStr.Substring(idx + 1);
                        mapInfo.mapArr[row] = new short[mapInfo.mapCol];
                        for (int j = 0; j < mapInfo.mapCol; j++)
                        {
                            binCode = noNumStr.Substring(j * binCodeLength, binCodeLength);
                            if (mapInfo.IsConvertMap)
                                mapInfo.mapArr[row][j] = Convert.ToInt16(binCode);
                            else
                            {
                                int ind = mapInfo.binInfo.Values.ToList().FindIndex(p => p.Bin == binCode);
                                if (ind < 0)//不存在bin
                                    mapInfo.mapArr[row][j] = DieGradeDefault.BLANK_DIE;
                                else
                                    mapInfo.mapArr[row][j] = mapInfo.binInfo.ElementAt(ind).Value.Grade;
                            }
                        }
                        row++;
                    }
                    LstStr.Clear();
                }
                catch (System.Exception)
                {

                }
            }
            catch (IOException)
            {

            }

            return true;
        }
        /// <summary>
        /// 解析富士通txt格式的Map
        /// </summary>
        /// <param name="mapFilePath"></param>
        /// <param name="mapInfo"></param>
        /// <returns></returns>
        public static bool Parse_FST_Txt(string mapFilePath, ref MapInfo mapInfo)
        {
            try
            {
                if (!File.Exists(mapFilePath)) return false;
                try
                {
                    mapInfo.mapType = MapTextFrame.FST_Txt;
                    mapInfo.SrcPath = mapFilePath;

                    List<string> LstStr = new List<string>();
                    textManipulate.ReadData(mapFilePath, LstStr);
                    if (LstStr.Count < 1) return false;

                    if (Str_TrimSpace(LstStr[0]) == ConvertMap)
                    {
                        mapInfo.IsConvertMap = true;
                    }
                    else mapInfo.IsConvertMap = false;

                    //对类型进行检查。第13行开始，标志位有'RowData'
                    int binCodeLength = mapInfo.IsConvertMap ? 3 : 3;
                    int rowIndex = mapInfo.IsConvertMap ? 13 : 12;
                    int headerInddex = mapInfo.IsConvertMap ? 1 : 0;
                    int idx = 0;
                    if (LstStr[rowIndex].Contains("RowData") == false)
                        return false;
                    else
                        idx = LstStr[rowIndex].IndexOf(":");

                    ((MapHeaderFSTTXT)mapInfo.mapHeaderInfo).DEVICE = LstStr[headerInddex].Split(':')[1];
                    ((MapHeaderFSTTXT)mapInfo.mapHeaderInfo).LOT = LstStr[headerInddex + 1].Split(':')[1];
                    ((MapHeaderFSTTXT)mapInfo.mapHeaderInfo).WAFER = LstStr[headerInddex + 2].Split(':')[1];
                    ((MapHeaderFSTTXT)mapInfo.mapHeaderInfo).FNLOC = LstStr[headerInddex + 3].Split(':')[1];
                    ((MapHeaderFSTTXT)mapInfo.mapHeaderInfo).ROWCT = LstStr[headerInddex + 4].Split(':')[1];
                    ((MapHeaderFSTTXT)mapInfo.mapHeaderInfo).COLCT = LstStr[headerInddex + 5].Split(':')[1];
                    ((MapHeaderFSTTXT)mapInfo.mapHeaderInfo).BCEQU = LstStr[headerInddex + 6].Split(':')[1];
                    ((MapHeaderFSTTXT)mapInfo.mapHeaderInfo).REFPX = LstStr[headerInddex + 7].Split(':')[1];
                    ((MapHeaderFSTTXT)mapInfo.mapHeaderInfo).REFPY = LstStr[headerInddex + 8].Split(':')[1];
                    ((MapHeaderFSTTXT)mapInfo.mapHeaderInfo).DUTMS = LstStr[headerInddex + 9].Split(':')[1];
                    ((MapHeaderFSTTXT)mapInfo.mapHeaderInfo).XDIES = LstStr[headerInddex + 10].Split(':')[1];
                    ((MapHeaderFSTTXT)mapInfo.mapHeaderInfo).YDIES = LstStr[headerInddex + 11].Split(':')[1];

                    //设置参考点
                    RefDie refDie = new RefDie(Convert.ToInt32(((MapHeaderFSTTXT)mapInfo.mapHeaderInfo).REFPY) - 1,
                                               Convert.ToInt32(((MapHeaderFSTTXT)mapInfo.mapHeaderInfo).REFPX) - 1);
                    mapInfo.RefDie.Add(refDie);
                    //设置Wafer方向
                    mapInfo.waferDir = (WaferDirEnum)(Convert.ToInt32(((MapHeaderFSTTXT)mapInfo.mapHeaderInfo).FNLOC) / 90);

                    //对行列值预解析
                    int row = 0, col = 0;
                    string noSpaceStr = Str_TrimSpace(LstStr[rowIndex]);
                    string noNumStr = noSpaceStr.Substring(idx + 1);
                    col = noNumStr.Length / binCodeLength;
                    for (int i = rowIndex; i < LstStr.Count; i++)
                    {
                        if (LstStr[i].Contains("RowData"))
                            row++;
                    }
                    mapInfo.mapRow = row;
                    mapInfo.mapCol = col;

                    //给字符数组分配内存
                    if (mapInfo.mapArr != null)
                    {
                        mapInfo.mapArr = null;
                    }
                    mapInfo.mapArr = new short[mapInfo.mapRow][];

                    //解析字符数组
                    row = 0;
                    string binCode = "";
                    for (int i = rowIndex; i < mapInfo.mapRow + rowIndex; i++)
                    {
                        noSpaceStr = Str_TrimSpace(LstStr[i]);
                        noNumStr = noSpaceStr.Substring(idx + 1);
                        mapInfo.mapArr[row] = new short[mapInfo.mapCol];
                        for (int j = 0; j < mapInfo.mapCol; j++)
                        {
                            binCode = noNumStr.Substring(j * binCodeLength, binCodeLength);
                            if (mapInfo.IsConvertMap)
                                mapInfo.mapArr[row][j] = Convert.ToInt16(binCode);
                            else
                            {
                                int ind = mapInfo.binInfo.Values.ToList().FindIndex(p => p.Bin == binCode);
                                if (ind < 0)//不存在bin
                                    mapInfo.mapArr[row][j] = DieGradeDefault.BLANK_DIE;
                                else
                                    mapInfo.mapArr[row][j] = mapInfo.binInfo.ElementAt(ind).Value.Grade;
                            }
                        }
                        row++;
                    }
                    LstStr.Clear();
                }
                catch (System.Exception)
                {

                }
            }
            catch (IOException)
            {

            }

            return true;
        }
        /// <summary>
        /// 解析士兰微txt格式的Map
        /// </summary>
        /// <param name="mapFilePath"></param>
        /// <param name="mapInfo"></param>
        /// <returns></returns>
        public static bool Parse_SLW_Txt(string mapFilePath, ref MapInfo mapInfo)
        {
            try
            {
                if (!File.Exists(mapFilePath)) return false;
                try
                {
                    mapInfo.mapType = MapTextFrame.SLW_Txt;
                    mapInfo.SrcPath = mapFilePath;

                    List<string> LstStr = new List<string>();
                    textManipulate.ReadData(mapFilePath, LstStr);
                    if (LstStr.Count < 1) return false;

                    if (Str_TrimSpace(LstStr[0]) == ConvertMap)
                    {
                        mapInfo.IsConvertMap = true;
                    }
                    else mapInfo.IsConvertMap = false;

                    //对类型进行检查。第13行开始，标志位有'RowData'
                    int binCodeLength = mapInfo.IsConvertMap ? 3 : 1;
                    int rowIndex = mapInfo.IsConvertMap ? 7 : 6;
                    int headerInddex = mapInfo.IsConvertMap ? 1 : 0;
                    int idx = 0;
                    //if (LstStr[rowIndex].Contains("RowData") == false)
                    //    return false;
                    //else
                    //    idx = LstStr[rowIndex].IndexOf(":");

                    ((MapHeaderSLWTXT)mapInfo.mapHeaderInfo).LOT = LstStr[headerInddex].Split(':')[1].Trim();
                    ((MapHeaderSLWTXT)mapInfo.mapHeaderInfo).DEVICE = LstStr[headerInddex + 1].Split(':')[1].Trim();
                    ((MapHeaderSLWTXT)mapInfo.mapHeaderInfo).PIECEID = LstStr[headerInddex + 2].Split(':')[1].Trim();
                    ((MapHeaderSLWTXT)mapInfo.mapHeaderInfo).PASS = LstStr[headerInddex + 3].Split(':')[1].Trim();
                    ((MapHeaderSLWTXT)mapInfo.mapHeaderInfo).NOTCH = WaferDirSLWEnum.SetWaferDirNum(LstStr[headerInddex + 4].Split(':')[1].Trim());

                    //设置参考点
                    //RefDie refDie = new RefDie(Convert.ToInt32(((MapHeaderSLWTXT)mapInfo.mapHeaderInfo).REFPY) - 1,
                    //                           Convert.ToInt32(((MapHeaderSLWTXT)mapInfo.mapHeaderInfo).REFPX) - 1);
                    //mapInfo.RefDie.Add(refDie);
                    //设置Wafer方向
                    mapInfo.waferDir = (WaferDirEnum)(Convert.ToInt32(((MapHeaderSLWTXT)mapInfo.mapHeaderInfo).NOTCH) / 90);

                    //对行列值预解析
                    int row = 0, col = 0;
                    string noSpaceStr = Str_TrimSpace(LstStr[rowIndex]);
                    string noNumStr = noSpaceStr.Substring(idx + 1);
                    col = noNumStr.Length / binCodeLength;
                    for (int i = rowIndex; i < LstStr.Count; i++)
                    {
                        if (Str_TrimSpace(LstStr[i]) != "")
                            row++;
                    }
                    mapInfo.mapRow = row;
                    mapInfo.mapCol = col;

                    //给字符数组分配内存
                    if (mapInfo.mapArr != null)
                    {
                        mapInfo.mapArr = null;
                    }
                    mapInfo.mapArr = new short[mapInfo.mapRow][];

                    //解析字符数组
                    row = 0;
                    string binCode = "";
                    for (int i = rowIndex; i < mapInfo.mapRow + rowIndex; i++)
                    {
                        noSpaceStr = Str_TrimSpace(LstStr[i]);
                        noNumStr = noSpaceStr.Substring(idx + 1);
                        mapInfo.mapArr[row] = new short[mapInfo.mapCol];
                        for (int j = 0; j < mapInfo.mapCol; j++)
                        {
                            binCode = noNumStr.Substring(j * binCodeLength, binCodeLength);
                            if (mapInfo.IsConvertMap)
                                mapInfo.mapArr[row][j] = Convert.ToInt16(binCode);
                            else
                            {
                                int ind = mapInfo.binInfo.Values.ToList().FindIndex(p => p.Bin == binCode);
                                if (ind < 0)//不存在bin
                                    mapInfo.mapArr[row][j] = DieGradeDefault.BLANK_DIE;
                                else
                                    mapInfo.mapArr[row][j] = mapInfo.binInfo.ElementAt(ind).Value.Grade;
                            }
                        }
                        row++;
                    }
                    LstStr.Clear();
                }
                catch (System.Exception)
                {

                }
            }
            catch (IOException)
            {

            }

            return true;
        }
        #endregion

        #region 保存各类图谱
        /// <summary>
        /// 保存长电tma格式的Map
        /// </summary>
        /// <param name="destMapFilePath"></param>
        /// <param name="isConvertMap"></param>
        /// <param name="mapInfo"></param>
        /// <returns></returns>
        public static bool Save_JCAP_Tma(string destMapFilePath, bool isConvertMap, MapInfo mapInfo, Dictionary<short, MapBinInfo> dMapBinInfoJCAPTMA = null)
        {
            //如果当前图谱非JCAP_TMA格式，则必须要给出JCAP_TMA的图谱等级映射表
            if (!isConvertMap && mapInfo.mapType != MapTextFrame.JCAP_Tma)
            {
                if (dMapBinInfoJCAPTMA == null || dMapBinInfoJCAPTMA.Count < 1)
                    return false;
            }

            FileStream fs = new FileStream(destMapFilePath, FileMode.Create);
            StreamWriter sw = new StreamWriter(fs, Encoding.Default);
            try
            {
                if (isConvertMap) sw.WriteLine(ConvertMap);

                string row0 = "    ";
                string row1 = "  ++";
                for (int i = 0; i < mapInfo.mapCol; i++)
                {
                    row0 += (i + 1).ToString("#000");
                    row1 += "-++";
                }
                sw.WriteLine(row0);
                sw.WriteLine(row1);

                //写入新的Map文件
                string binCode = "";
                for (int i = 0; i < mapInfo.mapRow; i++)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append((i + 1).ToString("#000") + "|");

                    for (int j = 0; j < mapInfo.mapCol; j++)
                    {

                        if (isConvertMap)//保存为中间图谱
                            binCode = mapInfo.mapArr[i][j].ToString("#000");
                        else //保存为原始图谱
                        {
                            if (mapInfo.mapType == MapTextFrame.JCAP_Tma)
                            {
                                binCode = mapInfo.binInfo[mapInfo.mapArr[i][j]].Bin;
                            }
                            else //非JCAP_TMA格式图谱保存为JCAP_TMA格式
                            {
                                binCode = dMapBinInfoJCAPTMA[mapInfo.mapArr[i][j]].Bin;
                            }
                        }
                        sb.Append("  " + binCode);
                    }
                    sw.WriteLine(sb.ToString());
                }
                return true;
                //sw.Close();
                //fs.Close();
            }
            catch (System.Exception)
            {
                return false;
            }
            finally
            {
                sw.Close();
                fs.Close();
            }
        }

        /// <summary>
        /// 保存长电sinf格式的Map
        /// </summary>
        /// <param name="srcMapFilePath"></param>
        /// <param name="destMapFilePath"></param>
        /// <param name="mapInfo"></param>
        /// <returns></returns>
        public static bool Save_JCAP_Sinf(string destMapFilePath, bool isConvertMap, MapInfo mapInfo, Dictionary<short, MapBinInfo> dMapBinInfoJCAPSINF = null)
        {
            //如果当前图谱非JCAP_Sinf格式，则必须要给出JCAP_Sinf的图谱等级映射表
            if (!isConvertMap && mapInfo.mapType != MapTextFrame.JCAP_Sinf)
            {
                if (dMapBinInfoJCAPSINF == null || dMapBinInfoJCAPSINF.Count < 1)
                    return false;
            }

            FileStream fs = new FileStream(destMapFilePath, FileMode.Create);
            StreamWriter sw = new StreamWriter(fs, Encoding.Default);
            try
            {
                if (isConvertMap) sw.WriteLine(ConvertMap);

                //写入前12行
                if (mapInfo.mapHeaderInfo == null) return false;

                sw.WriteLine("DEVICE:" + ((MapHeaderJCAPSinf)mapInfo.mapHeaderInfo).DEVICE);
                sw.WriteLine("LOT:" + ((MapHeaderJCAPSinf)mapInfo.mapHeaderInfo).LOT);
                sw.WriteLine("WAFER:" + ((MapHeaderJCAPSinf)mapInfo.mapHeaderInfo).WAFER);
                sw.WriteLine("FNLOC:" + ((MapHeaderJCAPSinf)mapInfo.mapHeaderInfo).FNLOC);
                sw.WriteLine("ROWCT:" + ((MapHeaderJCAPSinf)mapInfo.mapHeaderInfo).ROWCT);
                sw.WriteLine("COLCT:" + ((MapHeaderJCAPSinf)mapInfo.mapHeaderInfo).COLCT);
                sw.WriteLine("BCEQU:" + ((MapHeaderJCAPSinf)mapInfo.mapHeaderInfo).BCEQU);
                sw.WriteLine("REFPX:" + (Convert.ToInt32(((MapHeaderJCAPSinf)mapInfo.mapHeaderInfo).REFPX) + 1).ToString());
                sw.WriteLine("REFPY:" + (Convert.ToInt32(((MapHeaderJCAPSinf)mapInfo.mapHeaderInfo).REFPY) + 1).ToString());
                sw.WriteLine("DUTMS:" + ((MapHeaderJCAPSinf)mapInfo.mapHeaderInfo).DUTMS);
                sw.WriteLine("XDIES:" + ((MapHeaderJCAPSinf)mapInfo.mapHeaderInfo).XDIES);
                sw.WriteLine("YDIES:" + ((MapHeaderJCAPSinf)mapInfo.mapHeaderInfo).YDIES);

                //写入新的Map文件
                string binCode = "";
                for (int i = 0; i < mapInfo.mapRow; i++)
                {
                    StringBuilder sb = new StringBuilder();
                    StringBuilder sbRow = new StringBuilder();
                    sb.Append("RowData:");

                    for (int j = 0; j < mapInfo.mapCol; j++)
                    {
                        if (isConvertMap)//保存为中间图谱
                            binCode = mapInfo.mapArr[i][j].ToString("#000");
                        else //保存为原始图谱
                        {
                            if (mapInfo.mapType == MapTextFrame.JCAP_Sinf)
                            {
                                binCode = mapInfo.binInfo[mapInfo.mapArr[i][j]].Bin;
                            }
                            else //非JCAP_SINF格式图谱保存为JCAP_SINF格式
                            {
                                binCode = dMapBinInfoJCAPSINF[mapInfo.mapArr[i][j]].Bin;
                            }
                        }
                        sb.Append(binCode + " ");
                    }
                    sb.Remove(sb.Length - 1, 1);
                    sw.WriteLine(sb.ToString());
                }
                return true;
            }
            catch (System.Exception)
            {
                return false;
            }
            finally
            {
                sw.Close();
                fs.Close();
            }
        }

        /// <summary>
        /// 保存富士通txt格式的Map
        /// </summary>
        /// <param name="srcMapFilePath"></param>
        /// <param name="destMapFilePath"></param>
        /// <param name="mapInfo"></param>
        /// <returns></returns>
        public static bool Save_FST_Txt(string destMapFilePath, bool isConvertMap, MapInfo mapInfo, Dictionary<short, MapBinInfo> dMapBinInfoFSTTXT = null)
        {
            //如果当前图谱非FST_Txt格式，则必须要给出FST_Txt的图谱等级映射表
            if (!isConvertMap && mapInfo.mapType != MapTextFrame.FST_Txt)
            {
                if (dMapBinInfoFSTTXT == null || dMapBinInfoFSTTXT.Count < 1)
                    return false;
            }

            FileStream fs = new FileStream(destMapFilePath, FileMode.Create);
            StreamWriter sw = new StreamWriter(fs, Encoding.Default);

            try
            {
                if (isConvertMap) sw.WriteLine(ConvertMap);

                //写入前12行
                if (mapInfo.mapHeaderInfo == null) return false;

                sw.WriteLine("DEVICE:" + ((MapHeaderFSTTXT)mapInfo.mapHeaderInfo).DEVICE);
                sw.WriteLine("LOT:" + ((MapHeaderFSTTXT)mapInfo.mapHeaderInfo).LOT);
                sw.WriteLine("WAFER:" + ((MapHeaderFSTTXT)mapInfo.mapHeaderInfo).WAFER);
                sw.WriteLine("FNLOC:" + ((MapHeaderFSTTXT)mapInfo.mapHeaderInfo).FNLOC);
                sw.WriteLine("ROWCT:" + ((MapHeaderFSTTXT)mapInfo.mapHeaderInfo).ROWCT);
                sw.WriteLine("COLCT:" + ((MapHeaderFSTTXT)mapInfo.mapHeaderInfo).COLCT);
                sw.WriteLine("BCEQU:" + ((MapHeaderFSTTXT)mapInfo.mapHeaderInfo).BCEQU);
                sw.WriteLine("REFPX:" + (Convert.ToInt32(((MapHeaderFSTTXT)mapInfo.mapHeaderInfo).REFPX) + 1).ToString());
                sw.WriteLine("REFPY:" + (Convert.ToInt32(((MapHeaderFSTTXT)mapInfo.mapHeaderInfo).REFPY) + 1).ToString());
                sw.WriteLine("DUTMS:" + ((MapHeaderFSTTXT)mapInfo.mapHeaderInfo).DUTMS);
                sw.WriteLine("XDIES:" + ((MapHeaderFSTTXT)mapInfo.mapHeaderInfo).XDIES);
                sw.WriteLine("YDIES:" + ((MapHeaderFSTTXT)mapInfo.mapHeaderInfo).YDIES);

                //写入新的Map文件
                string binCode = "";
                for (int i = 0; i < mapInfo.mapRow; i++)
                {
                    StringBuilder sb = new StringBuilder();
                    StringBuilder sbRow = new StringBuilder();
                    sb.Append("RowData:");

                    for (int j = 0; j < mapInfo.mapCol; j++)
                    {
                        if (isConvertMap)//保存为中间图谱
                            binCode = mapInfo.mapArr[i][j].ToString("#000");
                        else //保存为原始图谱
                        {
                            if (mapInfo.mapType == MapTextFrame.FST_Txt)
                            {
                                binCode = mapInfo.binInfo[mapInfo.mapArr[i][j]].Bin;
                            }
                            else //非FST_Txt格式图谱保存为FST_Txt格式
                            {
                                binCode = dMapBinInfoFSTTXT[mapInfo.mapArr[i][j]].Bin;
                            }
                        }
                        sb.Append(binCode + " ");
                    }
                    sb.Remove(sb.Length - 1, 1);
                    sw.WriteLine(sb.ToString());
                }
                return true;
                //sw.Close();
                //fs.Close();
            }
            catch (System.Exception)
            {
                return false;
            }
            finally
            {
                sw.Close();
                fs.Close();
            }
        }
        
        /// <summary>
        /// 保存士兰微txt格式的Map
        /// </summary>
        /// <param name="srcMapFilePath"></param>
        /// <param name="destMapFilePath"></param>
        /// <param name="mapInfo"></param>
        /// <returns></returns>
        public static bool Save_SLW_Txt(string destMapFilePath, bool isConvertMap, MapInfo mapInfo, Dictionary<short, MapBinInfo> dMapBinInfoSLWTXT = null)
        {
            //如果当前图谱非SLW_Txt格式，则必须要给出SLW_Txt的图谱等级映射表
            if (!isConvertMap && mapInfo.mapType != MapTextFrame.SLW_Txt)
            {
                if (dMapBinInfoSLWTXT == null || dMapBinInfoSLWTXT.Count < 1)
                    return false;
            }

            FileStream fs = new FileStream(destMapFilePath, FileMode.Create);
            StreamWriter sw = new StreamWriter(fs, Encoding.Default);

            try
            {
                if (isConvertMap) sw.WriteLine(ConvertMap);

                //写入前5行
                if (mapInfo.mapHeaderInfo == null) return false;

                sw.WriteLine("LotID: " + ((MapHeaderSLWTXT)mapInfo.mapHeaderInfo).LOT);
                sw.WriteLine("DeviceName: " + ((MapHeaderSLWTXT)mapInfo.mapHeaderInfo).DEVICE);
                sw.WriteLine("PieceID: " + ((MapHeaderSLWTXT)mapInfo.mapHeaderInfo).PIECEID);
                sw.WriteLine("Pass: " + ((MapHeaderSLWTXT)mapInfo.mapHeaderInfo).PASS);
                sw.WriteLine("notch: " +WaferDirSLWEnum.GetWaferDirNum(((MapHeaderSLWTXT)mapInfo.mapHeaderInfo).NOTCH));

                sw.WriteLine("----------------------------------------------------------------------------------------------------------------------------------------------------------");

                //写入新的Map文件
                string binCode = "";
                for (int i = 0; i < mapInfo.mapRow; i++)
                {
                    StringBuilder sb = new StringBuilder();
                    StringBuilder sbRow = new StringBuilder();
                    //sb.Append("RowData:");

                    for (int j = 0; j < mapInfo.mapCol; j++)
                    {
                        if (isConvertMap)//保存为中间图谱
                            binCode = mapInfo.mapArr[i][j].ToString("#000");
                        else //保存为原始图谱
                        {
                            if (mapInfo.mapType == MapTextFrame.SLW_Txt)
                            {
                                binCode = mapInfo.binInfo[mapInfo.mapArr[i][j]].Bin;
                            }
                            else //非SLW_Txt格式图谱保存为SLW_Txt格式
                            {
                                binCode = dMapBinInfoSLWTXT[mapInfo.mapArr[i][j]].Bin;
                            }
                        }
                        sb.Append(binCode);
                    }
                    sw.WriteLine(sb.ToString());
                }
                return true;
                //sw.Close();
                //fs.Close();
            }
            catch (System.Exception)
            {
                return false;
            }
            finally
            {
                sw.Close();
                fs.Close();
            }
        }

        #endregion

        /// <summary>
        /// 根据bin等级映射表重排bin等级
        /// </summary>
        /// <param name="mapInfo"></param>
        /// <param name="dMapBinInfo"></param>
        /// <returns></returns>
        public static bool ReParseBinInfo(ref MapInfo mapInfo, Dictionary<short, MapBinInfo> dMapBinInfo)
        {
            if (mapInfo == null) return false;

            mapInfo.binInfo = SerializeData<Dictionary<short, MapBinInfo>>.Clone(dMapBinInfo);

            return true;
        }
        /// <summary>
        /// 根据旋转模式对Map进行旋转
        /// </summary>
        /// <param name="mapInfo"></param>
        /// <param name="rotMode"></param>
        /// <returns></returns>
        public static bool Rotate(ref MapInfo mapInfo, MapRotateModeEnum rotMode)
        {
            if (IsValid(mapInfo) == false)
                return false;

            mapInfo.rotMode = rotMode;

            if (rotMode == MapRotateModeEnum.ROT_NONE)
                return true;
            else if (rotMode == MapRotateModeEnum.ROT_90)
            {
                short[][] temArray = Array2DInit(mapInfo.mapRow, mapInfo.mapCol);
                temArray = (short[][])(mapInfo.mapArr.Clone());

                mapInfo.mapArr = Array2DInit(mapInfo.mapCol, mapInfo.mapRow);

                for (int i = 0; i < mapInfo.mapRow; i++)
                {
                    for (int j = 0; j < mapInfo.mapCol; j++)
                    {
                        mapInfo.mapArr[j][mapInfo.mapRow - 1 - i] = temArray[i][j];
                    }
                }

                DataTransformer.swap<int>(ref mapInfo.mapRow, ref mapInfo.mapCol);
            }
            else if (rotMode == MapRotateModeEnum.ROT_180)
            {
                short[][] temArray = Array2DInit(mapInfo.mapRow, mapInfo.mapCol);
                temArray = (short[][])(mapInfo.mapArr.Clone());

                mapInfo.mapArr = Array2DInit(mapInfo.mapRow, mapInfo.mapCol);

                for (int i = 0; i < mapInfo.mapRow; i++)
                {
                    for (int j = 0; j < mapInfo.mapCol; j++)
                    {
                        mapInfo.mapArr[mapInfo.mapRow - 1 - i][mapInfo.mapCol - 1 - j] = temArray[i][j];
                    }
                }
            }
            else if (rotMode == MapRotateModeEnum.ROT_270)
            {
                short[][] temArray = Array2DInit(mapInfo.mapRow, mapInfo.mapCol);
                temArray = (short[][])(mapInfo.mapArr.Clone());

                mapInfo.mapArr = Array2DInit(mapInfo.mapCol, mapInfo.mapRow);

                for (int i = 0; i < mapInfo.mapRow; i++)
                {
                    for (int j = 0; j < mapInfo.mapCol; j++)
                    {
                        mapInfo.mapArr[mapInfo.mapCol - 1 - j][i] = temArray[i][j];
                    }
                }

                DataTransformer.swap<int>(ref mapInfo.mapRow, ref mapInfo.mapCol);
            }

            mapInfo.waferDir = GetDstWaferDir(mapInfo.waferDir, rotMode);

            return true;
        }
        /// <summary>
        /// 根据镜像模式对Map进行镜像
        /// </summary>
        /// <param name="mapInfo"></param>
        /// <param name="mirMode"></param>
        /// <returns></returns>
        public static bool Mirror(ref MapInfo mapInfo, MapMirrorModeEnum mirMode)
        {
            if (IsValid(mapInfo) == false)
                return false;

            mapInfo.mirMode = mirMode;

            int tmpRow = mapInfo.mapRow >> 1;
            int tmpCol = mapInfo.mapCol >> 1;

            if (mirMode == MapMirrorModeEnum.MIR_NONE)
                return true;
            else if (mirMode == MapMirrorModeEnum.MIR_HOR)
            {
                for (int i = 0; i < tmpRow; i++)
                {
                    for (int j = 0; j < mapInfo.mapCol; j++)
                    {
                        DataTransformer.swap<short>(ref mapInfo.mapArr[i][j], ref mapInfo.mapArr[mapInfo.mapRow - 1 - i][j]);
                    }
                }
            }
            else if (mirMode == MapMirrorModeEnum.MIR_VET)
            {
                for (int j = 0; j < tmpCol; j++)
                {
                    for (int i = 0; i < mapInfo.mapRow; i++)
                    {
                        DataTransformer.swap<short>(ref mapInfo.mapArr[i][j], ref mapInfo.mapArr[i][mapInfo.mapCol - 1 - j]);
                    }
                }
            }

            return true;
        }
        /// <summary>
        /// 根据输入的路径对Map路径进行解析
        /// </summary>
        /// <param name="mapInfo"></param>
        /// <param name="path"></param>
        /// <param name="mapWork"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        public static bool ReParseWorkPath(ref MapInfo mapInfo, MapMovePathEnum path)
        {
            if (IsValid(mapInfo) == false)
                return false;

            //int count = 0;
            //int pickIdxNum = 0;
            //int grade = -1;
            //for (int i = 0; i < mapInfo.binInfo.Count; i++)
            //{
            //    grade = mapInfo.binInfo[i].grade;
            //    if (grade >= 0 && grade < (int)(MapGradeEnum.GRADE_BLANK))
            //    {
            //        pickIdxNum++;
            //    }
            //}

            //for (int i = 0; i < mapInfo.mapRow; i++)
            //{
            //    for (int j = 0; j < mapInfo.mapCol; j++)
            //    {
            //        if ((IsToPickDie(mapInfo, i, j)))
            //        {
            //            count++;
            //        }
            //    }
            //}

            //if (count == 0)
            //    return false;

            //if (mapInfo.mapWorkInfoLst == null)
            //    mapInfo.mapWorkInfoLst = new List<MapWorkInfo>(count);
            //else
            //    mapInfo.mapWorkInfoLst.Clear();

            //count = 0;
            //for (int binIdx = 0; binIdx < pickIdxNum; binIdx++)
            //{
            //    //左上弓字形
            //    if (path == MapMovePathEnum.PATH_LTS)
            //    {
            //        for (int i = 0; i < mapInfo.mapRow; i++)
            //        {
            //            if ((i & 0x1) == 1)
            //            {
            //                for (int j = mapInfo.mapCol - 1; j >= 0; j--)
            //                {
            //                    if (GetBinGradeByRowCol(mapInfo, i, j) == mapInfo.binInfo[binIdx].grade)
            //                    {
            //                        mapInfo.mapWorkInfoLst.Add(new MapWorkInfo(i, j));
            //                        if (count == 0)
            //                        {
            //                            mapInfo.mapWorkInfoLst[count].rows = 0;
            //                            mapInfo.mapWorkInfoLst[count].cols = 0;
            //                        }
            //                        else
            //                        {
            //                            mapInfo.mapWorkInfoLst[count].rows = mapInfo.mapWorkInfoLst[count].rowIdx - mapInfo.mapWorkInfoLst[count - 1].rowIdx;
            //                            mapInfo.mapWorkInfoLst[count].cols = mapInfo.mapWorkInfoLst[count].colIdx - mapInfo.mapWorkInfoLst[count - 1].colIdx;
            //                        }
            //                        count++;
            //                    }
            //                }
            //            }
            //            else
            //            {
            //                for (int j = 0; j < mapInfo.mapCol; j++)
            //                {
            //                    if (GetBinGradeByRowCol(mapInfo, i, j) == mapInfo.binInfo[binIdx].grade)
            //                    {
            //                        mapInfo.mapWorkInfoLst.Add(new MapWorkInfo(i, j));
            //                        if (count == 0)
            //                        {
            //                            mapInfo.mapWorkInfoLst[count].rows = 0;
            //                            mapInfo.mapWorkInfoLst[count].cols = 0;
            //                        }
            //                        else
            //                        {
            //                            mapInfo.mapWorkInfoLst[count].rows = mapInfo.mapWorkInfoLst[count].rowIdx - mapInfo.mapWorkInfoLst[count - 1].rowIdx;
            //                            mapInfo.mapWorkInfoLst[count].cols = mapInfo.mapWorkInfoLst[count].colIdx - mapInfo.mapWorkInfoLst[count - 1].colIdx;
            //                        }
            //                        count++;
            //                    }
            //                }
            //            }
            //        }
            //    }
            //    //右下弓字形
            //    else if (path == MapMovePathEnum.PATH_RBS)
            //    {
            //        for (int i = mapInfo.mapRow - 1; i >= 0; i--)
            //        {
            //            if (((mapInfo.mapRow - 1 - i) & 0x1) == 1)
            //            {
            //                for (int j = 0; j < mapInfo.mapCol; j++)
            //                {
            //                    if (GetBinGradeByRowCol(mapInfo, i, j) == mapInfo.binInfo[binIdx].grade)
            //                    {
            //                        mapInfo.mapWorkInfoLst.Add(new MapWorkInfo(i, j));
            //                        if (count == 0)
            //                        {
            //                            mapInfo.mapWorkInfoLst[count].rows = 0;
            //                            mapInfo.mapWorkInfoLst[count].cols = 0;
            //                        }
            //                        else
            //                        {
            //                            mapInfo.mapWorkInfoLst[count].rows = mapInfo.mapWorkInfoLst[count].rowIdx - mapInfo.mapWorkInfoLst[count - 1].rowIdx;
            //                            mapInfo.mapWorkInfoLst[count].cols = mapInfo.mapWorkInfoLst[count].colIdx - mapInfo.mapWorkInfoLst[count - 1].colIdx;
            //                        }
            //                        count++;
            //                    }
            //                }
            //            }
            //            else
            //            {
            //                for (int j = mapInfo.mapCol - 1; j >= 0; j--)
            //                {
            //                    if (GetBinGradeByRowCol(mapInfo, i, j) == mapInfo.binInfo[binIdx].grade)
            //                    {
            //                        mapInfo.mapWorkInfoLst.Add(new MapWorkInfo(i, j));
            //                        if (count == 0)
            //                        {
            //                            mapInfo.mapWorkInfoLst[count].rows = 0;
            //                            mapInfo.mapWorkInfoLst[count].cols = 0;
            //                        }
            //                        else
            //                        {
            //                            mapInfo.mapWorkInfoLst[count].rows = mapInfo.mapWorkInfoLst[count].rowIdx - mapInfo.mapWorkInfoLst[count - 1].rowIdx;
            //                            mapInfo.mapWorkInfoLst[count].cols = mapInfo.mapWorkInfoLst[count].colIdx - mapInfo.mapWorkInfoLst[count - 1].colIdx;
            //                        }
            //                        count++;
            //                    }
            //                }
            //            }
            //        }
            //    }
            //    //左下弓字形
            //    else if (path == MapMovePathEnum.PATH_LBS)
            //    {
            //        for (int j = 0; j < mapInfo.mapCol; j++)
            //        {
            //            if ((j & 0x1) == 1)
            //            {
            //                for (int i = 0; i < mapInfo.mapRow; i++)
            //                {
            //                    if (GetBinGradeByRowCol(mapInfo, i, j) == mapInfo.binInfo[binIdx].grade)
            //                    {
            //                        mapInfo.mapWorkInfoLst.Add(new MapWorkInfo(i, j));
            //                        if (count == 0)
            //                        {
            //                            mapInfo.mapWorkInfoLst[count].rows = 0;
            //                            mapInfo.mapWorkInfoLst[count].cols = 0;
            //                        }
            //                        else
            //                        {
            //                            mapInfo.mapWorkInfoLst[count].rows = mapInfo.mapWorkInfoLst[count].rowIdx - mapInfo.mapWorkInfoLst[count - 1].rowIdx;
            //                            mapInfo.mapWorkInfoLst[count].cols = mapInfo.mapWorkInfoLst[count].colIdx - mapInfo.mapWorkInfoLst[count - 1].colIdx;
            //                        }
            //                        count++;
            //                    }
            //                }
            //            }
            //            else
            //            {
            //                for (int i = mapInfo.mapRow - 1; i >= 0; i--)
            //                {
            //                    if (GetBinGradeByRowCol(mapInfo, i, j) == mapInfo.binInfo[binIdx].grade)
            //                    {
            //                        mapInfo.mapWorkInfoLst.Add(new MapWorkInfo(i, j));
            //                        if (count == 0)
            //                        {
            //                            mapInfo.mapWorkInfoLst[count].rows = 0;
            //                            mapInfo.mapWorkInfoLst[count].cols = 0;
            //                        }
            //                        else
            //                        {
            //                            mapInfo.mapWorkInfoLst[count].rows = mapInfo.mapWorkInfoLst[count].rowIdx - mapInfo.mapWorkInfoLst[count - 1].rowIdx;
            //                            mapInfo.mapWorkInfoLst[count].cols = mapInfo.mapWorkInfoLst[count].colIdx - mapInfo.mapWorkInfoLst[count - 1].colIdx;
            //                        }
            //                        count++;
            //                    }
            //                }
            //            }
            //        }
            //    }
            //    //右上弓字形
            //    else if (path == MapMovePathEnum.PATH_RTS)
            //    {
            //        for (int j = mapInfo.mapCol - 1; j >= 0; j--)
            //        {
            //            if (((mapInfo.mapCol - 1 - j) & 0x1) == 1)
            //            {
            //                for (int i = mapInfo.mapRow - 1; i >= 0; i--)
            //                {
            //                    if (GetBinGradeByRowCol(mapInfo, i, j) == mapInfo.binInfo[binIdx].grade)
            //                    {
            //                        mapInfo.mapWorkInfoLst.Add(new MapWorkInfo(i, j));
            //                        if (count == 0)
            //                        {
            //                            mapInfo.mapWorkInfoLst[count].rows = 0;
            //                            mapInfo.mapWorkInfoLst[count].cols = 0;
            //                        }
            //                        else
            //                        {
            //                            mapInfo.mapWorkInfoLst[count].rows = mapInfo.mapWorkInfoLst[count].rowIdx - mapInfo.mapWorkInfoLst[count - 1].rowIdx;
            //                            mapInfo.mapWorkInfoLst[count].cols = mapInfo.mapWorkInfoLst[count].colIdx - mapInfo.mapWorkInfoLst[count - 1].colIdx;
            //                        }
            //                        count++;
            //                    }
            //                }
            //            }
            //            else
            //            {
            //                for (int i = 0; i < mapInfo.mapRow; i++)
            //                {
            //                    if (GetBinGradeByRowCol(mapInfo, i, j) == mapInfo.binInfo[binIdx].grade)
            //                    {
            //                        mapInfo.mapWorkInfoLst.Add(new MapWorkInfo(i, j));
            //                        if (count == 0)
            //                        {
            //                            mapInfo.mapWorkInfoLst[count].rows = 0;
            //                            mapInfo.mapWorkInfoLst[count].cols = 0;
            //                        }
            //                        else
            //                        {
            //                            mapInfo.mapWorkInfoLst[count].rows = mapInfo.mapWorkInfoLst[count].rowIdx - mapInfo.mapWorkInfoLst[count - 1].rowIdx;
            //                            mapInfo.mapWorkInfoLst[count].cols = mapInfo.mapWorkInfoLst[count].colIdx - mapInfo.mapWorkInfoLst[count - 1].colIdx;
            //                        }
            //                        count++;
            //                    }
            //                }
            //            }
            //        }
            //    }
            //}
            return true;
        }
        /// <summary>
        /// 判断Map是否有效
        /// </summary>
        /// <param name="mapInfo"></param>
        /// <returns></returns>
        public static bool IsValid(MapInfo mapInfo)
        {
            if (mapInfo == null)
                return false;

            if (mapInfo.mapRow <= 0 || mapInfo.mapRow >= MapMaxMin.MAX_MAP_ROW ||
                mapInfo.mapCol <= 0 || mapInfo.mapCol >= MapMaxMin.MAX_MAP_COL ||
                mapInfo.binInfo.Count <= 0 || mapInfo.binInfo.Count > MapMaxMin.MAX_BIN_NUM)
            {
                return false;
            }

            return true;
        }
        /// <summary>
        /// 判断Die是否在图谱范围内
        /// </summary>
        /// <param name="mapInfo"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        public static bool IsDieInRange(MapInfo mapInfo, int row, int col)
        {
            if (mapInfo == null)
                return false;

            if (row < 0 || row >= mapInfo.mapRow ||
                col < 0 || col >= mapInfo.mapCol)
                return false;

            return true;
        }
        /// <summary>
        /// 判断Die是否有效,即是否为GOOD_DIE或者DEFECT_DIE
        /// </summary>
        /// <param name="mapInfo"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        public static bool IsDieValid(MapInfo mapInfo, int row, int col)
        {
            short grade = GetBinGradeByRowCol(mapInfo, row, col);
            if (grade >= DieGradeDefault.GOOD_DIE && grade < DieGradeDefault.BLANK_DIE)
                return true;
            else return false;
        }
        /// <summary>
        /// 判断是分布式待分拣的Bin【GOOD DIE】
        /// </summary>
        /// <param name="mapInfo"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        public static bool IsToPickDie(MapInfo mapInfo, int row, int col)
        {
            short grade = -1;
            DieGradeRange goodDieGradeRange = DieType.GetDieGradeRange(DieType.GOOD_DIE);

            grade = GetBinGradeByRowCol(mapInfo, row, col);
            if (grade >= goodDieGradeRange.Min && grade <= goodDieGradeRange.Max)
                return true;
            else return false;
        }
        /// <summary>
        /// 根据行列获取等级信息
        /// </summary>
        /// <param name="mapInfo"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        public static short GetBinGradeByRowCol(MapInfo mapInfo, int row, int col)
        {
            if (!IsDieInRange(mapInfo, row, col))
                return -1;

            return mapInfo.mapArr[row][col];
        }
        /// <summary>
        /// 根据行列获取字符信息
        /// </summary>
        /// <param name="mapInfo"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        public static string GetBinCodeByRowCol(MapInfo mapInfo, int row, int col)
        {
            short grade = GetBinGradeByRowCol(mapInfo, row, col);

            return mapInfo.binInfo[grade].Bin;
        }
        /// <summary>
        /// 根据行列获取单元格的颜色
        /// </summary>
        /// <param name="mapInfo"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        public static bool GetMapColorByRowCol(MapInfo mapInfo, int row, int col, ref Color color)
        {
            if (IsValid(mapInfo) == false)
                return false;

            if (!IsDieInRange(mapInfo, row, col))
                return false;

            if (color != null)
                color = Color.Empty;
            short grade = 0;
            grade = GetBinGradeByRowCol(mapInfo, row, col);
            //先判断是否为参考点
            int ind = mapInfo.RefDie.FindIndex(p => p.Row == row && p.Col == col);
            if (ind >= 0)
                color = mapInfo.binInfo[(short)-(ind + 1)].Color;//参考点默认等级为[-1,-10]
            else
            {
                color = mapInfo.binInfo[grade].Color;
            }
            //若参考点为Defect Die，则要显示Defect Die的颜色
            DieGradeRange defectDieGradeRange = DieType.GetDieGradeRange(DieType.DEFECT_DIE);
            if (grade >= defectDieGradeRange.Min && grade <= defectDieGradeRange.Max)
                color = mapInfo.binInfo[grade].Color;

            return true;
        }
        /// <summary>
        /// 根据等级获取颜色
        /// </summary>
        /// <param name="dMapBinInfo"></param>
        /// <param name="grade"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        public static bool GetMapColorByGrade(Dictionary<short, MapBinInfo> dMapBinInfo, short grade, ref Color color)
        {
            if (dMapBinInfo == null || dMapBinInfo.Count < 1)
                return false;

            if (!dMapBinInfo.ContainsKey(grade))
                return false;

            color = dMapBinInfo[grade].Color;

            return true;
        }
        /// <summary>
        /// 获取与Notch口对应的图谱角度方向
        /// </summary>
        /// <param name="mapInfo"></param>
        /// <returns></returns>
        public static MapRotateModeEnum GetMapAngle(WaferDirEnum waferDir)
        {
            int angle = 2 + (int)waferDir;
            angle = angle >= 4 ? angle - 4 : angle;

            return (MapRotateModeEnum)angle;
        }
        /// <summary>
        /// 获取与图谱角度对应的Notch口方向
        /// </summary>
        /// <param name="mapAngle"></param>
        /// <returns></returns>
        public static WaferDirEnum GetWaferDir(MapRotateModeEnum mapAngle)
        {
            int waferDir = 2 + (int)mapAngle;
            waferDir = waferDir >= 4 ? waferDir - 4 : waferDir;

            return (WaferDirEnum)waferDir;
        }
        /// <summary>
        /// 获取转至目标Notch口方向图谱需要转过的角度
        /// </summary>
        /// <param name="curWaferDir">当前图谱角度</param>
        /// <param name="dstWaferDir">目标Notch方向</param>
        /// <returns>目标图谱角度</returns>
        public static MapRotateModeEnum GetMapRotateAngle(WaferDirEnum curWaferDir, WaferDirEnum dstWaferDir)
        {
            int deltaWaferDir = dstWaferDir - curWaferDir;
            deltaWaferDir = deltaWaferDir < 0 ? (4 + deltaWaferDir) : deltaWaferDir;

            return (MapRotateModeEnum)deltaWaferDir;
        }
        /// <summary>
        /// 已知当前wafer方向和需要旋转的角度，计算目标wafer方向
        /// </summary>
        /// <param name="curWaferDir"></param>
        /// <param name="rotAngle"></param>
        /// <returns></returns>
        public static WaferDirEnum GetDstWaferDir(WaferDirEnum curWaferDir, MapRotateModeEnum rotAngle)
        {
            int dstWaferDir = (int)curWaferDir + (int)rotAngle;
            dstWaferDir = dstWaferDir >= 4 ? dstWaferDir - 4 : dstWaferDir;

            return (WaferDirEnum)dstWaferDir;
        }
        /// <summary>
        /// 计算合并后mapping
        /// </summary>
        /// <param name="mapInfoA"></param>
        /// <param name="mapInfoB"></param>
        /// <param name="mapInfoAB"></param>
        /// <returns></returns>
        public static bool Add(MapInfo mapInfoA, MapInfo mapInfoB, out MapInfo mapInfo)
        {
            mapInfo = null;
            try
            {
                if (mapInfoA == null || mapInfoB == null) return false;
                if (mapInfoA.mapType != mapInfoB.mapType) return false;
                if (mapInfoA.mapRow != mapInfoB.mapRow || mapInfoA.mapCol != mapInfoB.mapCol) return false;

                //需要判断两个mapp是否大小一样；
                try
                {
                    mapInfo = mapInfoA.DeepClone();
                }
                catch (Exception)
                {
                    if (mapInfo != null) mapInfo.Clear();
                    return false;
                }
                //合并mapArr数组
                int row = mapInfo.mapArr.GetLength(0);
                DieGradeRange goodDieGradeRange = DieType.GetDieGradeRange(DieType.GOOD_DIE);
                DieGradeRange defectDieGradeRange = DieType.GetDieGradeRange(DieType.DEFECT_DIE);
                for (int i = 0; i < row; i++)
                {
                    int col = mapInfo.mapArr[i].Length;
                    //都为Good Die按低等级合并
                    //其它按高等级合并
                    for (int j = 0; j < col; j++)
                    {
                        if (mapInfoA.mapArr[i][j] >= goodDieGradeRange.Min && mapInfoA.mapArr[i][j] <= goodDieGradeRange.Max &&
                           mapInfoB.mapArr[i][j] >= goodDieGradeRange.Min && mapInfoB.mapArr[i][j] <= goodDieGradeRange.Max)
                        {
                            mapInfo.mapArr[i][j] = Math.Min(mapInfoA.mapArr[i][j], mapInfoB.mapArr[i][j]);
                        }
                        else
                        {
                            mapInfo.mapArr[i][j] = Math.Max(mapInfoA.mapArr[i][j], mapInfoB.mapArr[i][j]);
                        }
                    }
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
