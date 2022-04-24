using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;

namespace ToolKits.Map
{
    public class MapMaxMin
    {
        #region MAP数据极值
        public const int MAX_MAP_ROW = 1000;                              //map最大行数
        public const int MAX_MAP_COL = 1000;                              //map最大列数
        public const int MAX_MAP_FILE_COLS = 2000;                        //文本最大行数
        public const int MAX_BIN_NUM = DieGradeDefault.OTHER_DIE;         //分级最大数目

        public const int GRID_MIN_PIXEL = 20;//9;                         //单元格最小像素个数
        public const int AUX_VIEW_ROWCOL = 9;//9;                         //放大视野行列数
        #endregion
    }
    /// <summary>
    /// Map原点选取
    /// </summary>
    public enum MapOrigionDirEnum : int
    {
        /// <summary>
        /// 左上角为原点
        /// </summary>
        LeftTop = 0,
        /// <summary>
        /// 右上角为原点
        /// </summary>
        RightTop = 1,
        /// <summary>
        /// 右下角为原点
        /// </summary>
        RightBottom = 2,
        /// <summary>
        /// 左下角为原点
        /// </summary>
        LeftBottom = 3
    }
    /// <summary>
    /// Map旋转模式枚举
    /// </summary>
    public enum MapRotateModeEnum : int
    {
        /// <summary>
        /// 不旋转
        /// </summary>
        ROT_NONE = 0,
        /// <summary>
        /// 顺时针旋转90度
        /// </summary>
        ROT_90 = 1,
        /// <summary>
        /// 顺时针旋转180度
        /// </summary>
        ROT_180 = 2,
        /// <summary>
        /// 顺时针旋转270度
        /// </summary>
        ROT_270 = 3
    }
    /// <summary>
    /// Wafer方向枚举
    /// </summary>
    public enum WaferDirEnum : int
    {
        /// <summary>
        /// NOTCH口朝上
        /// </summary>
        DIR_NONE = 0,
        /// <summary>
        /// NOTCH口朝右
        /// </summary>
        DIR_90 = 1,
        /// <summary>
        /// NOTCH口朝下
        /// </summary>
        DIR_180 = 2,
        /// <summary>
        /// NOTCH口朝左
        /// </summary>
        DIR_270 = 3
    }
    /// <summary>
    /// 士兰微图谱方向定义类
    /// </summary>
    public class WaferDirSLWEnum
    {
        /// <summary>
        /// Notch口朝上0度
        /// </summary>
        public const string TOP = "UP";
        /// <summary>
        /// Notch口朝右90度
        /// </summary>
        public const string RIGHT = "RIGHT";
        /// <summary>
        /// Notch口朝下180度
        /// </summary>
        public const string DOWN = "DOWN";
        /// <summary>
        /// Notch口朝左20度
        /// </summary>
        public const string LEFT = "LEFT";
        /// <summary>
        /// 转为通用图谱方向
        /// </summary>
        /// <param name="slwWaferDir">士兰微图谱方向</param>
        /// <returns></returns>
        public static string SetWaferDirNum(string slwWaferDir)
        {
            switch (slwWaferDir)
            {
                case TOP:
                    return "0";
                case RIGHT:
                    return "90";
                case DOWN:
                    return "180";
                case LEFT:
                    return "270";
                default:
                    return "0";
            }
        }
        /// <summary>
        /// 获取士兰微图谱方向
        /// </summary>
        /// <param name="waferDirNum">通用图谱方向</param>
        /// <returns></returns>
        public static string GetWaferDirNum(string waferDirNum)
        {
            switch (waferDirNum)
            {
                case "0":
                    return TOP;
                case "90":
                    return RIGHT;
                case "180":
                    return DOWN;
                case "270":
                    return LEFT;
                default:
                    return TOP;
            }
        }
    }
    /// <summary>
    /// Map镜像模式枚举
    /// </summary>
    public enum MapMirrorModeEnum : int
    {
        /// <summary>
        /// 不镜像
        /// </summary>
        MIR_NONE,
        /// <summary>
        /// 水平镜像
        /// </summary>
        MIR_HOR,
        /// <summary>
        /// 垂直镜像
        /// </summary>
        MIR_VET,
    }

    /// <summary>
    /// Map行走模式枚举
    /// </summary>
    public enum MapMovePathEnum : int
    {
        /// <summary>
        /// 从左到右
        /// </summary>
        PATH_L2R,
        /// <summary>
        /// 从右到左
        /// </summary>
        PATH_R2L,
        /// <summary>
        /// 从上到下
        /// </summary>
        PATH_T2B,
        /// <summary>
        /// 从下到上
        /// </summary>
        PATH_B2T,

        /// <summary>
        /// 左上弓字形
        /// </summary>
        PATH_LTS,
        /// <summary>
        /// 右下弓字形
        /// </summary>
        PATH_RBS,
        /// <summary>
        /// 左下弓字形
        /// </summary>
        PATH_LBS,
        /// <summary>
        /// 右上弓字形
        /// </summary>
        PATH_RTS,
    }
    /// <summary>
    /// 芯片类型的默认等级
    /// </summary>
    public class DieGradeDefault
    {
        public const short REFERENCE_DIE = -1;
        public const short GOOD_DIE = 0;
        public const short DEFECT_DIE = 100;
        public const short BLANK_DIE = 200;
        public const short FORBIDDEN_DIE = 300;
        public const short OTHER_DIE = 400;
    }
    /// <summary>
    /// 芯片类型的等级范围
    /// </summary>
    public class DieGradeRange
    {
        public short Min { get; private set; }
        public short Max { get; private set; }
        public DieGradeRange(short min, short max)
        {
            this.Min = min;
            this.Max = max;
        }
    }
    /// <summary>
    /// 芯片类型详细信息
    /// <para>根据芯片等级设定颜色时，若该等级范围内没有，则设定此等级范围内的默认值</para>
    /// </summary>
    public class DieType
    {
        /// <summary>
        /// 参考点，无等级
        /// </summary>
        public const string REFERENCE_DIE = "Reference Die";
        /// <summary>
        /// 等级0-99
        /// </summary>
        public const string GOOD_DIE = "Good Die";
        /// <summary>
        /// 等级100-199
        /// </summary>
        public const string DEFECT_DIE = "Defect Die";
        /// <summary>
        /// 等级200-299
        /// </summary>
        public const string BLANK_DIE = "Blank Die";
        /// <summary>
        /// 等级300-399
        /// </summary>
        public const string FORBIDDEN_DIE = "Forbidden Die";
        public static DieGradeRange GetDieGradeRange(string dieType)
        {
            switch (dieType)
            {
                case REFERENCE_DIE:
                    return new DieGradeRange(DieGradeDefault.REFERENCE_DIE, DieGradeDefault.REFERENCE_DIE);
                case GOOD_DIE:
                    return new DieGradeRange(DieGradeDefault.GOOD_DIE, DieGradeDefault.DEFECT_DIE - 1);
                case DEFECT_DIE:
                    return new DieGradeRange(DieGradeDefault.DEFECT_DIE, DieGradeDefault.BLANK_DIE - 1);
                case BLANK_DIE:
                    return new DieGradeRange(DieGradeDefault.BLANK_DIE, DieGradeDefault.FORBIDDEN_DIE - 1);
                case FORBIDDEN_DIE:
                    return new DieGradeRange(DieGradeDefault.FORBIDDEN_DIE, DieGradeDefault.OTHER_DIE - 1);
                default://默认blank die
                    return new DieGradeRange(DieGradeDefault.BLANK_DIE, DieGradeDefault.FORBIDDEN_DIE - 1);
            }
        }
        public static bool IsInDieGradeRange(string dieType, int grade)
        {
            DieGradeRange range = GetDieGradeRange(dieType);
            if (grade >= range.Min && grade <= range.Max)
                return true;
            else return false;
        }
    }

    /// <summary>
    /// Map文本格式
    /// </summary>
    public class MapTextFrame
    {
        /// <summary>
        /// 景焱txt格式
        /// </summary>
        public const string HT_Txt = ".txt(HT)";
        /// <summary>
        /// 长电tma格式
        /// </summary>
        public const string JCAP_Tma = ".tma(JCAP)";
        /// <summary>
        /// 长电sinf格式
        /// </summary>
        public const string JCAP_Sinf = ".sinf(JCAP)";
        /// <summary>
        /// 富士通txt格式
        /// </summary>
        public const string FST_Txt = ".txt(FST)";
        /// <summary>
        /// 士兰微txt格式
        /// </summary>
        public const string SLW_Txt = ".txt(SLW)";
        /// <summary>
        /// 获取图谱类型
        /// </summary>
        /// <param name="mapTextFrame"></param>
        /// <returns></returns>
        public static string GetMapType(string mapTextFrame)
        {
            string[] strs = mapTextFrame.Split('(');
            return strs.Length > 0 ? strs[0] : "";
        }
        /// <summary>
        /// 根据索引获取图谱类型
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public static string GetMapType(int index)
        {
            string mapType = GetMapTextName(index);

            return GetMapType(mapType);
        }
        public static string GetMapTextName(int index)
        {
            string mapTextName = "";
            switch (index)
            {
                case 0:
                    mapTextName = HT_Txt;
                    break;
                case 1:
                    mapTextName = JCAP_Tma;
                    break;
                case 2:
                    mapTextName = JCAP_Sinf;
                    break;
                case 3:
                    mapTextName = FST_Txt;
                    break;
                case 4:
                    mapTextName = SLW_Txt;
                    break;
                default:
                    mapTextName = "";
                    break;
            }
            return mapTextName;
        }
    }

    /// <summary>
    /// Map绘制模式
    /// </summary>
    public enum MapDrawModeEnum
    {
        DrawFrame = 1,
        DrawInterior = 2,
        DrawFrameAndInterior = 3,
    }

    /// <summary>
    /// Map Bin信息
    /// </summary>
    [Serializable]
    public class MapBinInfo
    {
        /// <summary>
        /// 转换图谱的字符串
        /// </summary>
        public string Bin { get; set; }
        /// <summary>
        /// bin等级
        /// </summary>
        public short Grade { get; set; }
        /// <summary>
        /// 等级对应的颜色名称
        /// </summary>
        public string ColorName { get; set; }
        /// <summary>
        /// 等级对应的颜色
        /// </summary>
        [XmlIgnore]
        public Color Color
        {
            get
            {
                return ColorTranslator.FromHtml(this.ColorName);
            }
            set
            {
                if (value == null) return;
                this.ColorName = ColorTranslator.ToHtml(value);
            }
        }
        public MapBinInfo() 
        {
            this.Bin = "";
            this.Grade = -1;
            this.ColorName = ColorTranslator.ToHtml(Color.Empty);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="binCode">在图谱中体现的字符</param>
        /// <param name="grade">芯片等级</param>
        /// <param name="color">等级对应的颜色</param>
        public MapBinInfo(string binCode, short grade, Color color)
        {
            Bin = binCode;
            Grade = grade;
            ColorName = ColorTranslator.ToHtml(color);
        }
    }
    [Serializable]
    public class RefDie
    {
        public int Row{get;set;}
        public int Col{get;set;}
        public RefDie(int row,int col)
        {
            Row=row;
            Col=col;
        }
    }
    [Serializable]
    public class MapWorkInfo
    {
        /// <summary>
        /// 当前grid的行列
        /// </summary>
        public int rowIdx;
        public int colIdx;

        /// <summary>
        /// 距前一个grid的行列数
        /// </summary>
        public int rows;
        public int cols;

        public MapWorkInfo(int row, int col)
        {
            rowIdx = row;
            colIdx = col;
        }
    }
    /// <summary>
    /// 长电sinf图谱统计信息
    /// </summary>
    [Serializable]
    public class MapHeaderJCAPSinf
    {
        /// <summary>
        /// 程序名称
        /// </summary>
        public string DEVICE { get; set; }
        /// <summary>
        /// 生产批号 lod id
        /// </summary>
        public string LOT { get; set; }
        /// <summary>
        /// 圆片号wafer id
        /// </summary>
        public string WAFER { get; set; }
        /// <summary>
        /// 圆片方向（180为缺口向下）
        /// </summary>
        public string FNLOC { get; set; }
        /// <summary>
        /// 图谱总行数
        /// </summary>
        public string ROWCT { get; set; }
        /// <summary>
        /// 图谱总列数
        /// </summary>
        public string COLCT { get; set; }
        /// <summary>
        /// Good Die代码
        /// </summary>
        public string BCEQU { get; set; }
        /// <summary>
        /// Ref Die X坐标
        /// </summary>
        public string REFPX { get; set; }
        /// <summary>
        /// Ref Die Y坐标
        /// </summary>
        public string REFPY { get; set; }
        /// <summary>
        /// 单位
        /// </summary>
        public string DUTMS { get; set; }
        /// <summary>
        /// 芯片X方向尺寸
        /// </summary>
        public string XDIES { get; set; }
        /// <summary>
        /// 芯片Y方向尺寸
        /// </summary>
        public string YDIES { get; set; }
        public MapHeaderJCAPSinf()
        {
            this.DEVICE = "__";// System.IO.Path.GetFileNameWithoutExtension(System.Windows.Forms.Application.ExecutablePath);
            this.LOT = "__";
            this.WAFER = "__";
            this.FNLOC = "0";
            this.ROWCT = "0";
            this.COLCT = "0";
            this.BCEQU = "00";
            this.REFPX = "0";
            this.REFPY = "0";
            this.DUTMS = "MM";
            this.XDIES = "0";
            this.YDIES = "0";
        }
        /// <summary>
        /// 浅拷贝
        /// </summary>
        /// <returns></returns>
        public MapHeaderJCAPSinf Clone()
        {
            return this.MemberwiseClone() as MapHeaderJCAPSinf;
        }
        /// <summary>
        /// 深拷贝
        /// </summary>
        /// <returns></returns>
        public MapHeaderJCAPSinf DeepClone()
        {
            MemoryStream stream = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, this);
            stream.Position = 0;
            return formatter.Deserialize(stream) as MapHeaderJCAPSinf;
        }
    }
    /// <summary>
    /// 富士通txt图谱统计信息
    /// </summary>
    [Serializable]
    public class MapHeaderFSTTXT
    {
        /// <summary>
        /// 程序名称
        /// </summary>
        public string DEVICE { get; set; }
        /// <summary>
        /// 生产批号 lod id
        /// </summary>
        public string LOT { get; set; }
        /// <summary>
        /// 圆片号wafer id
        /// </summary>
        public string WAFER { get; set; }
        /// <summary>
        /// 圆片方向（180为缺口向下）
        /// </summary>
        public string FNLOC { get; set; }
        /// <summary>
        /// 图谱总行数
        /// </summary>
        public string ROWCT { get; set; }
        /// <summary>
        /// 图谱总列数
        /// </summary>
        public string COLCT { get; set; }
        /// <summary>
        /// Good Die代码
        /// </summary>
        public string BCEQU { get; set; }
        /// <summary>
        /// Ref Die X坐标
        /// </summary>
        public string REFPX { get; set; }
        /// <summary>
        /// Ref Die Y坐标
        /// </summary>
        public string REFPY { get; set; }
        /// <summary>
        /// 单位
        /// </summary>
        public string DUTMS { get; set; }
        /// <summary>
        /// 芯片X方向尺寸
        /// </summary>
        public string XDIES { get; set; }
        /// <summary>
        /// 芯片Y方向尺寸
        /// </summary>
        public string YDIES { get; set; }
        public MapHeaderFSTTXT()
        {
            this.DEVICE = "__";// System.IO.Path.GetFileNameWithoutExtension(System.Windows.Forms.Application.ExecutablePath);
            this.LOT = "__";
            this.WAFER = "__";
            this.FNLOC = "0";
            this.ROWCT = "0";
            this.COLCT = "0";
            this.BCEQU = "00";
            this.REFPX = "0";
            this.REFPY = "0";
            this.DUTMS = "mm";
            this.XDIES = "0";
            this.YDIES = "0";
        }
        /// <summary>
        /// 浅拷贝
        /// </summary>
        /// <returns></returns>
        public MapHeaderFSTTXT Clone()
        {
            return this.MemberwiseClone() as MapHeaderFSTTXT;
        }
        /// <summary>
        /// 深拷贝
        /// </summary>
        /// <returns></returns>
        public MapHeaderFSTTXT DeepClone()
        {
            MemoryStream stream = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, this);
            stream.Position = 0;
            return formatter.Deserialize(stream) as MapHeaderFSTTXT;
        }
    }
    /// <summary>
    /// 士兰微txt图谱统计信息
    /// </summary>
    [Serializable]
    public class MapHeaderSLWTXT
    {
        /// <summary>
        /// 生产批号 lod id
        /// </summary>
        public string LOT { get; set; }
        /// <summary>
        /// 程序名称
        /// </summary>
        public string DEVICE { get; set; }
        /// <summary>
        /// pieceID
        /// </summary>
        public string PIECEID { get; set; }
        /// <summary>
        /// OK Die数量
        /// </summary>
        public string PASS { get; set; }
        /// <summary>
        /// 圆片方向（180为缺口向下）
        /// </summary>
        public string NOTCH { get; set; }
        public MapHeaderSLWTXT()
        {
            this.LOT = "__";
            this.DEVICE = "__";
            this.PIECEID = "__";
            this.PASS = "__";
            this.NOTCH = "__";
        }
        /// <summary>
        /// 浅拷贝
        /// </summary>
        /// <returns></returns>
        public MapHeaderSLWTXT Clone()
        {
            return this.MemberwiseClone() as MapHeaderSLWTXT;
        }
        /// <summary>
        /// 深拷贝
        /// </summary>
        /// <returns></returns>
        public MapHeaderSLWTXT DeepClone()
        {
            MemoryStream stream = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, this);
            stream.Position = 0;
            return formatter.Deserialize(stream) as MapHeaderSLWTXT;
        }
    }
    /// <summary>
    /// 图谱原点设定与旋转变换(变换主要用于参考点的变换)
    /// </summary>
    [Serializable]
    public class MapTransInfo
    {
        /// <summary>
        /// 旋转前Map行数
        /// </summary>
        public int BefRowCnt { get; private set; }
        /// <summary>
        /// 旋转前Map列数
        /// </summary>
        public int BefColCnt { get; private set; }
        /// <summary>
        /// 旋转后Map行数
        /// </summary>
        public int AftRowCnt { get; set; }
        /// <summary>
        /// 旋转后Map列数
        /// </summary>
        public int AftColCnt { get; set; }
        /// <summary>
        /// 旋转前图谱原点
        /// </summary>
        public MapOrigionDirEnum BefMapOriDir { get; private set; }
        /// <summary>
        /// 旋转后图谱原点
        /// </summary>
        public MapOrigionDirEnum AftMapOriDir { get; private set; }
        /// <summary>
        /// 图谱旋转角度
        /// </summary>
        public MapRotateModeEnum Angle { get; private set; }
        public MapTransInfo() { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mapRow">旋转后图谱行数</param>
        /// <param name="mapCol">旋转后图谱列数</param>
        /// <param name="mapOriDir">旋转前图谱原点</param>
        /// <param name="angle">图谱旋转角度</param>
        public MapTransInfo(int mapRow, int mapCol, MapOrigionDirEnum mapOriDir, MapRotateModeEnum angle)
        {
            this.AftRowCnt = mapRow;
            this.AftColCnt = mapCol;
            this.BefMapOriDir = mapOriDir;
            //int ang = ((int)mapOriDir) * 90 + ((int)angle) * 90;
            //ang = ang >= 360 ? ang - 360 : ang;
            //this.AftMapOriDir = (MapOrigionDirEnum)(ang / 90);
            this.Angle = angle;
            switch (angle)
            {
                case MapRotateModeEnum.ROT_NONE:
                    this.BefRowCnt = this.AftRowCnt;
                    this.BefColCnt = this.AftColCnt;
                    break;
                case MapRotateModeEnum.ROT_90:
                    this.BefRowCnt = this.AftColCnt;
                    this.BefColCnt = this.AftRowCnt;
                    break;
                case MapRotateModeEnum.ROT_180:
                    this.BefRowCnt = this.AftRowCnt;
                    this.BefColCnt = this.AftColCnt;
                    break;
                case MapRotateModeEnum.ROT_270:
                    this.BefRowCnt = this.AftColCnt;
                    this.BefColCnt = this.AftRowCnt;
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// 不旋转的情况下，以任意一角为坐标系与左上角为坐标系原点时的坐标系坐标映射关系,
        /// Map行列数不变
        /// </summary>
        /// <param name="lTRow">以左上角为坐标系原点的行坐标</param>
        /// <param name="lTCol">以左上角为坐标系原点的列坐标</param>
        /// <param name="angle">当前坐标原点相对左上角原点顺时针转过的角度</param>
        /// <param name="anyRow">新坐标系行坐标</param>
        /// <param name="anyCol">新坐标系列坐标</param>
        /// <param name="beforeRotate">是否为旋转之前的坐标映射</param>
        /// <returns></returns>
        public bool lTHAny(int lTRow, int lTCol, MapOrigionDirEnum mapOriDir, out int anyRow, out int anyCol, bool beforeRotate = false)
        {
            anyRow = -1;
            anyCol = -1;
            try
            {
                switch (mapOriDir)
                {
                    case MapOrigionDirEnum.LeftTop:
                        anyRow = lTRow;
                        anyCol = lTCol;
                        break;
                    case MapOrigionDirEnum.RightTop:
                        anyRow = lTRow;
                        anyCol = beforeRotate ? this.BefColCnt - 1 - lTCol : this.AftColCnt - 1 - lTCol;
                        break;
                    case MapOrigionDirEnum.RightBottom:
                        anyRow = beforeRotate ? this.BefRowCnt - 1 - lTRow : this.AftRowCnt - 1 - lTRow;
                        anyCol = beforeRotate ? this.BefColCnt - 1 - lTCol : this.AftColCnt - 1 - lTCol;
                        break;
                    case MapOrigionDirEnum.LeftBottom:
                        anyRow = beforeRotate ? this.BefRowCnt - 1 - lTRow : this.AftRowCnt - 1 - lTRow;
                        anyCol = lTCol;
                        break;
                    default:
                        return false;
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        /// <summary>
        /// 旋转前相对旋转后坐标变换关系，以旋转前原点为坐标系原点不变
        /// </summary>
        /// <param name="befRow"></param>
        /// <param name="befCol"></param>
        /// <param name="aftRow"></param>
        /// <param name="aftCol"></param>
        /// <returns></returns>
        public bool BefHAft(int befRow, int befCol, out int aftRow, out int aftCol)
        {
            aftRow = -1;
            aftCol = -1;
            switch (this.BefMapOriDir)
            {
                case MapOrigionDirEnum.LeftTop:
                    if (!RotateByLeftTopOri(befRow, befCol, BefRowCnt, BefColCnt, this.Angle, out aftRow, out aftCol))
                        return false;
                    break;
                case MapOrigionDirEnum.RightTop:
                    break;
                case MapOrigionDirEnum.RightBottom:
                    break;
                case MapOrigionDirEnum.LeftBottom:
                    break;
                default:
                    return false;
            }
            return true;
        }
        public bool AftHBef(int aftRow, int aftCol, out int befRow, out int befCol)
        {
            befRow = -1;
            befCol = -1;

            MapRotateModeEnum angle = (this.Angle == MapRotateModeEnum.ROT_NONE) ? MapRotateModeEnum.ROT_NONE : ((MapRotateModeEnum)(4 - (int)this.Angle));


            switch (this.BefMapOriDir)
            {
                case MapOrigionDirEnum.LeftTop:
                    if (!RotateByLeftTopOri(aftRow, aftCol, AftRowCnt, AftColCnt, angle, out befRow, out befCol))
                        return false;
                    break;
                case MapOrigionDirEnum.RightTop:
                    break;
                case MapOrigionDirEnum.RightBottom:
                    break;
                case MapOrigionDirEnum.LeftBottom:
                    break;
                default:
                    return false;
            }


            return true;
        }
        private bool RotateByLeftTopOri(int row, int col, int rowCnt, int colCnt, MapRotateModeEnum angle, out int rotRow, out int rotCol)
        {
            rotRow = -1;
            rotCol = -1;
            switch (angle)
            {
                case MapRotateModeEnum.ROT_NONE:
                    rotRow = row;
                    rotCol = col;
                    break;
                case MapRotateModeEnum.ROT_90:
                    rotRow = col;
                    rotCol = rowCnt - 1 - row;
                    break;
                case MapRotateModeEnum.ROT_180:
                    rotRow = rowCnt - 1 - row;
                    rotCol = colCnt - 1 - col;
                    break;
                case MapRotateModeEnum.ROT_270:
                    rotRow = colCnt - 1 - col;
                    rotCol = row;
                    break;
                default:
                    return false;
            }
            return true;
        }
        /// <summary>
        /// 浅拷贝
        /// </summary>
        /// <returns></returns>
        public MapTransInfo Clone()
        {
            return this.MemberwiseClone() as MapTransInfo;
        }
        /// <summary>
        /// 深拷贝
        /// </summary>
        /// <returns></returns>
        public MapTransInfo DeepClone()
        {
            MemoryStream stream = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, this);
            stream.Position = 0;
            return formatter.Deserialize(stream) as MapTransInfo;
        }
    }

    [Serializable]
    public class MapInfo
    {
        /// <summary>
        /// 源图像路径
        /// </summary>
        public string SrcPath;
        /// <summary>
        /// Map文本框架类型(.sinf(JCAP))
        /// </summary>
        public string mapType;
        /// <summary>
        /// 旋转方式
        /// </summary>
        public MapRotateModeEnum rotMode;
        /// <summary>
        /// 镜像方式
        /// </summary>
        public MapMirrorModeEnum mirMode;
        /// <summary>
        /// Wafer方向
        /// </summary>
        public WaferDirEnum waferDir;
        /// <summary>
        /// map行数
        /// </summary>
        public int mapRow;
        /// <summary>
        /// map列数
        /// </summary>
        public int mapCol;
        /// <summary>
        /// map字符数组
        /// </summary>
        public short[][] mapArr;
        public List<RefDie> RefDie;
        /// <summary>
        /// bin信息【BinCode,Grade,Color】
        /// </summary>
        public Dictionary<short, MapBinInfo> binInfo;
        /// <summary>
        /// 
        /// </summary>
        public List<MapWorkInfo> mapWorkInfoLst;
        /// <summary>
        /// map转换关系
        /// </summary>
        public MapTransInfo mapTransInfo;
        /// <summary>
        /// 各类图谱文件的头信息
        /// </summary>
        public object mapHeaderInfo;
        public bool IsConvertMap { get; set; }

        #region 构造器
        /// <summary>
        /// 
        /// </summary>
        /// <param name="path">图谱路径</param>
        /// <param name="type">图谱类型(如.tma(JCAP))</param>
        public MapInfo(string path, string type)
        {
            SrcPath = path;
            mapType = type;
            RefDie = new List<Map.RefDie>();
            binInfo = new Dictionary<short, MapBinInfo>();
            mapTransInfo = new MapTransInfo();
            waferDir = WaferDirEnum.DIR_180;
            IsConvertMap = false;
            switch (mapType)
            {
                case MapTextFrame.JCAP_Sinf:
                    mapHeaderInfo = new MapHeaderJCAPSinf();
                    break;
                case MapTextFrame.FST_Txt:
                    mapHeaderInfo = new MapHeaderFSTTXT();
                    break;
                case MapTextFrame.SLW_Txt:
                    mapHeaderInfo = new MapHeaderSLWTXT();
                    break;
                default:
                    break;
            }
        }
        #endregion

        /// <summary>
        /// 返回某个Grade的Die个数
        /// </summary>
        /// <param name="grade"></param>
        /// <returns></returns>
        public int DieCount(short grade)
        {
            int cnt = 0;
            if (this.mapArr == null) return 0;
            for (int i = 0; i < this.mapRow; i++)
                cnt += this.mapArr[i].Count(p => p == grade);
            return cnt;
        }
        /// <summary>
        /// 返回某类型的Die个数
        /// </summary>
        /// <param name="dieType"></param>
        /// <returns></returns>
        public int DieCount(string dieType)
        {
            int cnt = 0;
            if (this.mapArr == null) return 0;
            DieGradeRange dieGradeRange = DieType.GetDieGradeRange(dieType);

            for (int i = 0; i < this.mapRow; i++)
                cnt += this.mapArr[i].Count(p => p >= dieGradeRange.Min && p <= dieGradeRange.Max);
            return cnt;
        }
        /// <summary>
        /// 浅拷贝
        /// </summary>
        /// <returns></returns>
        public MapInfo Clone()
        {
            return this.MemberwiseClone() as MapInfo;
        }
        /// <summary>
        /// 深拷贝
        /// </summary>
        /// <returns></returns>
        public MapInfo DeepClone()
        {
            MemoryStream stream = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, this);
            stream.Position = 0;
            return formatter.Deserialize(stream) as MapInfo;
        }
        /// <summary>
        /// mapping清除
        /// </summary>
        public void Clear()
        {
            SrcPath = "";
            mapType = "";
            mapRow = 0;
            mapCol = 0;
            mapArr = null;
            IsConvertMap = false;
            if (RefDie != null) RefDie.Clear();
            if (binInfo != null) binInfo.Clear();
            mapHeaderInfo = null;
            if (mapWorkInfoLst != null) mapWorkInfoLst.Clear();
            mapTransInfo = null;
            rotMode = MapRotateModeEnum.ROT_NONE;
            mirMode = MapMirrorModeEnum.MIR_NONE;
            waferDir = WaferDirEnum.DIR_NONE;
        }
    }

    public class CoordEventArgs : EventArgs
    {
        public int Row;
        public int Col;
        public int R;
        public int G;
        public int B;

        public CoordEventArgs(int row, int col, int r, int g, int b)
        {
            Row = row;
            Col = col;
            R = r;
            G = g;
            B = b;
        }
    }

    /// <summary>
    /// 芯片单元格
    /// </summary>
    public struct DieGrid
    {
        public int Row;
        public int Col;

        public DieGrid(int row, int col)
        {
            Row = row;
            Col = col;
        }
    }
    public class DieInfoEventArgs : EventArgs
    {
        public int Row;
        public int Col;
        public short Grade;
        public Color Color;
        public bool RefDie;
        public DieInfoEventArgs(int row, int col, short grade, Color color, bool refDie = false)
        {
            this.Row = row;
            this.Col = col;
            this.Grade = grade;
            this.Color = color;
            this.RefDie = refDie;
        }
    }
    public class LoadMapEventArgs : EventArgs
    {
        public MapRotateModeEnum RotMode { get; private set; }
        public MapMirrorModeEnum MirMode { get; private set; }
        public MapInfo MapInfo;
        public LoadMapEventArgs(MapRotateModeEnum rotMode, MapMirrorModeEnum mirMode, MapInfo mapInfo)
        {
            this.RotMode = rotMode;
            this.MirMode = mirMode;
            this.MapInfo = mapInfo;
        }
    }
}