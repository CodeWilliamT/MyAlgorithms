using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Runtime.CompilerServices;
using System.Timers;
using Microsoft.VisualBasic.CompilerServices;
using Microsoft.VisualBasic;

namespace HTImageServer.UI.ControlWrap
{
    /// <summary>
    /// LED显示控件
    /// </summary>
    [ToolboxItem(true)]
    public class LedNumberEx : Control
    {
        #region 字段
        private Color _BackColor;
        private Color _BorderColor;
        private Color _BorderLightColor;
        private Color _BorderShadowColor;
        private BorderStyleEnum _BorderStyle;
        private int _BorderWidth;
        private float _CornerRate;
        private int _CustomLEDNums;
        private int _DarkTime;
        private string _DateTimeFormatString;
        //private bool _DrawBorder;
        private int _FlashInterval;
        private Color _ForeColor;
        private bool _IsAntiAlias;
        private bool _IsCustomLEDNum;
        private bool _IsFlash;
        private bool _IsZeroFirst;
        private float _LEDInterval;
        private const float _LEDIntervalRate = 0.08f;
        private LEDStyleEnum _LEDStyle;
        private float _LEDWidth;
        private const float _LEDWidthRate = 0.45f;
        private float _Margin;
        private const float _MarginRate = 0.04f;
        private float _SegmentInterval;
        private const float _SegmentIntervalRate = 0.01f;
        private float _SegmentWidth;
        private const float _SegmentWidthRate = 0.1f;
        private Color _ShadowColor;
        private string _Text;
        private AlignType _TextAlign;
        [AccessedThroughProperty("Timer1")]
        private System.Timers.Timer _Timer1;
        [AccessedThroughProperty("TimerFlash")]
        private System.Timers.Timer _TimerFlash;
        private bool bolDrawText;
        private IContainer components;
        private System.Threading.Thread m_FlashThread;
        public readonly bool RegVer;
        #endregion

        #region 事件类型
        public event EventHandler LEDClick;

        public event EventHandler LEDDoubleClick;

        #endregion

        /// <summary>
        /// 构造函数
        /// </summary>
        public LedNumberEx()
        {
            InitializeComponent();
            base.Click += new EventHandler(this.LEDNumber_Click);
            base.DoubleClick += new EventHandler(this.LEDNumber_DoubleClick);
            this._LEDStyle = LEDStyleEnum.Custom;
            this._Text = "0";
            this._DateTimeFormatString = "yyyy-MM-dd HH:mm:ss";
            this._TextAlign = AlignType.Right;
            this._IsFlash = false;
            this._FlashInterval = 0x3e8;
            this._DarkTime = 300;
            this._IsCustomLEDNum = false;
            this._CustomLEDNums = 10;
            this._IsZeroFirst = false;
            this._BackColor = Color.Black;
            this._ForeColor = Color.Red;
            this._ShadowColor = Color.FromArgb(100, 100, 100);
            this._IsAntiAlias = false;
            this._CornerRate = 0.25f;
            //this._DrawBorder = true;
            this._BorderStyle = BorderStyleEnum.Outside;
            this._BorderWidth = 3;
            this._BorderColor = Color.Yellow;
            this._BorderLightColor = SystemColors.ControlLight;
            this._BorderShadowColor = SystemColors.ControlDark;
            this.bolDrawText = true;
            this.RegVer = false;
            this.InitializeComponent();
            this.ParameterInitial();
            this.BackColor = this._BackColor;
            this.Timer1 = new System.Timers.Timer();
            this.Timer1.Enabled = false;
            this.Timer1.Interval = 1000;
            this.TimerFlash = new System.Timers.Timer();
            this.TimerFlash.Enabled = false;
            this.TimerFlash.Interval = 1000;
            components = null;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void DrawBorder(Graphics g)
        {
            if (this._BorderWidth != 0)
            {
                SolidBrush brush;
                GraphicsPath path;
                Pen pen;
                RectangleF ef;
                Region region;
                PointF tf;
                RectangleF ef2;
                PointF[] points = new PointF[6];
                float y = 0f;
                float num2 = 0f;
                float x = 0f;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                switch (this._BorderStyle)
                {
                    case BorderStyleEnum.Outside:
                        if ((((double)this._BorderWidth) / 3.0) <= 10.0)
                        {
                            x = (float)(((double)this._BorderWidth) / 3.0);
                            y = (float)(((double)this._BorderWidth) / 3.0);
                            num2 = (float)(((double)this._BorderWidth) / 3.0);
                            break;
                        }
                        y = (float)-Convert.ToDouble(((num2 == 10f) != false));
                        x = this._BorderWidth - (2f * y);
                        break;

                    case BorderStyleEnum.Inside:
                        if ((((double)this._BorderWidth) / 3.0) <= 10.0)
                        {
                            x = (float)(((double)this._BorderWidth) / 3.0);
                            y = (float)(((double)this._BorderWidth) / 3.0);
                            num2 = (float)(((double)this._BorderWidth) / 3.0);
                        }
                        else
                        {
                            y = (float)-Convert.ToDouble(((num2 == 10f) != false));
                            x = this._BorderWidth - (2f * y);
                        }
                        tf = new PointF(0f, (float)this.Height);
                        points[0] = tf;
                        tf = new PointF(0f, 0f);
                        points[1] = tf;
                        tf = new PointF((float)this.Width, 0f);
                        points[2] = tf;
                        tf = new PointF(this.Width - y, y);
                        points[3] = tf;
                        tf = new PointF(y, y);
                        points[4] = tf;
                        tf = new PointF(y, this.Height - y);
                        points[5] = tf;
                        path = new GraphicsPath();
                        path.AddLines(points);
                        brush = new SolidBrush(this._BorderShadowColor);
                        g.FillPath(brush, path);
                        tf = new PointF(0f, (float)this.Height);
                        points[0] = tf;
                        tf = new PointF((float)this.Width, (float)this.Height);
                        points[1] = tf;
                        tf = new PointF((float)this.Width, 0f);
                        points[2] = tf;
                        tf = new PointF(this.Width - y, y);
                        points[3] = tf;
                        tf = new PointF(this.Width - y, this.Height - y);
                        points[4] = tf;
                        tf = new PointF(y, this.Height - y);
                        points[5] = tf;
                        path = new GraphicsPath();
                        path.AddLines(points);
                        brush = new SolidBrush(this._BorderLightColor);
                        g.FillPath(brush, path);
                        tf = new PointF(x + y, (this.Height - y) - x);
                        points[0] = tf;
                        tf = new PointF((this.Width - x) - y, (this.Height - x) - y);
                        points[1] = tf;
                        tf = new PointF((this.Width - x) - y, x + y);
                        points[2] = tf;
                        tf = new PointF((float)(this.Width - this._BorderWidth), (float)this._BorderWidth);
                        points[3] = tf;
                        tf = new PointF((float)(this.Width - this._BorderWidth), (float)(this.Height - this._BorderWidth));
                        points[4] = tf;
                        tf = new PointF((float)this._BorderWidth, (float)(this.Height - this._BorderWidth));
                        points[5] = tf;
                        path = new GraphicsPath();
                        path.AddLines(points);
                        brush = new SolidBrush(this._BorderShadowColor);
                        g.FillPath(brush, path);
                        tf = new PointF(x + y, (this.Height - y) - x);
                        points[0] = tf;
                        tf = new PointF(x + y, x + y);
                        points[1] = tf;
                        tf = new PointF((this.Width - x) - y, x + y);
                        points[2] = tf;
                        tf = new PointF((float)(this.Width - this._BorderWidth), (float)this._BorderWidth);
                        points[3] = tf;
                        tf = new PointF((float)this._BorderWidth, (float)this._BorderWidth);
                        points[4] = tf;
                        tf = new PointF((float)this._BorderWidth, (float)(this.Height - this._BorderWidth));
                        points[5] = tf;
                        path = new GraphicsPath();
                        path.AddLines(points);

                        brush = new SolidBrush(this._BorderLightColor);
                        g.FillPath(brush, path);
                        ef2 = new RectangleF(y, y, this.Width - (2f * y), this.Height - (2f * y));
                        ef = ef2;
                        region = new Region(ef);
                        ef2 = new RectangleF(x + y, x + y, this.Width - (2f * (x + y)), this.Height - (2f * (x + y)));
                        ef = ef2;
                        region.Exclude(ef);
                        brush = new SolidBrush(this._BorderColor);
                        g.FillRegion(brush, region);

                        pen = new Pen(this._BorderShadowColor);
                        g.DrawLine(pen, y + x, y + x, (float)this._BorderWidth, (float)this._BorderWidth);
                        g.DrawLine(pen, this.Width - y, this.Height - y, (float)this.Width, (float)this.Height);
                        brush.Dispose();
                        pen.Dispose();
                        return;

                    case BorderStyleEnum.Flat:
                        x = this._BorderWidth;
                        y = 0f;
                        num2 = 0f;
                        ef2 = new RectangleF(0f, 0f, (float)this.Width, (float)this.Height);
                        ef = ef2;
                        region = new Region(ef);
                        ef2 = new RectangleF(x, x, this.Width - (2f * x), this.Height - (2f * x));
                        ef = ef2;
                        region.Exclude(ef);
                        brush = new SolidBrush(this._BorderColor);
                        g.FillRegion(brush, region);
                        brush.Dispose();
                        return;

                    case BorderStyleEnum.None:
                        return;

                    default:
                        return;
                }
                tf = new PointF(0f, (float)this.Height);
                points[0] = tf;
                tf = new PointF(0f, 0f);
                points[1] = tf;
                tf = new PointF((float)this.Width, 0f);
                points[2] = tf;
                tf = new PointF(this.Width - y, y);
                points[3] = tf;
                tf = new PointF(y, y);
                points[4] = tf;
                tf = new PointF(y, this.Height - y);
                points[5] = tf;
                path = new GraphicsPath();
                path.AddLines(points);
                brush = new SolidBrush(this._BorderLightColor);
                g.FillPath(brush, path);
                tf = new PointF(0f, (float)this.Height);
                points[0] = tf;
                tf = new PointF((float)this.Width, (float)this.Height);
                points[1] = tf;
                tf = new PointF((float)this.Width, 0f);
                points[2] = tf;
                tf = new PointF(this.Width - y, y);
                points[3] = tf;
                tf = new PointF(this.Width - y, this.Height - y);
                points[4] = tf;
                tf = new PointF(y, this.Height - y);
                points[5] = tf;
                path = new GraphicsPath();
                path.AddLines(points);
                brush = new SolidBrush(this._BorderShadowColor);
                g.FillPath(brush, path);
                tf = new PointF(x + y, (this.Height - y) - x);
                points[0] = tf;
                tf = new PointF((this.Width - x) - y, (this.Height - x) - y);
                points[1] = tf;
                tf = new PointF((this.Width - x) - y, x + y);
                points[2] = tf;
                tf = new PointF((float)(this.Width - this._BorderWidth), (float)this._BorderWidth);
                points[3] = tf;
                tf = new PointF((float)(this.Width - this._BorderWidth), (float)(this.Height - this._BorderWidth));
                points[4] = tf;
                tf = new PointF((float)this._BorderWidth, (float)(this.Height - this._BorderWidth));
                points[5] = tf;
                path = new GraphicsPath();
                path.AddLines(points);
                brush = new SolidBrush(this._BorderLightColor);
                g.FillPath(brush, path);
                tf = new PointF(x + y, (this.Height - y) - x);
                points[0] = tf;
                tf = new PointF(x + y, x + y);
                points[1] = tf;
                tf = new PointF((this.Width - x) - y, x + y);
                points[2] = tf;
                tf = new PointF((float)(this.Width - this._BorderWidth), (float)this._BorderWidth);
                points[3] = tf;
                tf = new PointF((float)this._BorderWidth, (float)this._BorderWidth);
                points[4] = tf;
                tf = new PointF((float)this._BorderWidth, (float)(this.Height - this._BorderWidth));
                points[5] = tf;
                path = new GraphicsPath();
                path.AddLines(points);
                brush = new SolidBrush(this._BorderShadowColor);
                g.FillPath(brush, path);
                ef2 = new RectangleF(y, y, this.Width - (2f * y), this.Height - (2f * y));
                ef = ef2;
                region = new Region(ef);
                ef2 = new RectangleF(x + y, x + y, this.Width - (2f * (x + y)), this.Height - (2f * (x + y)));
                ef = ef2;
                region.Exclude(ef);
                brush = new SolidBrush(this._BorderColor);
                g.FillRegion(brush, region);
                pen = new Pen(this._BorderShadowColor);
                g.DrawLine(pen, 0f, 0f, y, y);
                g.DrawLine(pen, (float)(this.Width - this._BorderWidth), (float)(this.Height - this._BorderWidth), (this.Width - y) - x, (this.Height - y) - x);
                brush.Dispose();
                pen.Dispose();
            }
        }

        private void DrawFromLeft(Graphics g, string strText)
        {
            string sLeft = "";
            Rectangle rectToDraw = new Rectangle();
            double num2 = 0;
            rectToDraw.X = 0;
            rectToDraw.Y = (int)Math.Round((double)(this._BorderWidth + this._Margin));
            rectToDraw.Width = (int)Math.Round((double)this._LEDWidth);
            rectToDraw.Height = this.Height - (rectToDraw.Y * 2);
            short length = (short)strText.Length;
            for (short i = 1; i <= length; i++)
            {
                sLeft = strText.Substring(i - 1, 1).ToUpper();
                if (sLeft == ".")
                {
                    rectToDraw.X = (int)Math.Round((double)(((this._BorderWidth + this._Margin) + this._LEDInterval) + (num2 * (this._LEDInterval + this._LEDWidth))));
                    num2 = num2 + 0.5;
                }
                else
                {
                    rectToDraw.X = (int)Math.Round((double)(((this._BorderWidth + this._Margin) + this._LEDInterval) + (num2 * (this._LEDInterval + this._LEDWidth))));
                    num2 = (short)(num2 + 1);
                }
                string str2 = sLeft;
                if ((((((StringType.StrCmp(str2, "0", false) == 0) || (StringType.StrCmp(str2, "1", false) == 0)) || ((StringType.StrCmp(str2, "2", false) == 0) || (StringType.StrCmp(str2, "3", false) == 0))) || (((StringType.StrCmp(str2, "4", false) == 0) || (StringType.StrCmp(str2, "5", false) == 0)) || ((StringType.StrCmp(str2, "6", false) == 0) || (StringType.StrCmp(str2, "7", false) == 0)))) || ((((StringType.StrCmp(str2, "8", false) == 0) || (StringType.StrCmp(str2, "9", false) == 0)) || ((StringType.StrCmp(str2, "-", false) == 0) || (StringType.StrCmp(str2, "A", false) == 0))) || (((StringType.StrCmp(str2, "B", false) == 0) || (StringType.StrCmp(str2, "C", false) == 0)) || ((StringType.StrCmp(str2, "D", false) == 0) || (StringType.StrCmp(str2, "E", false) == 0))))) || (((((StringType.StrCmp(str2, "F", false) == 0) || (StringType.StrCmp(str2, "G", false) == 0)) || ((StringType.StrCmp(str2, "H", false) == 0) || (StringType.StrCmp(str2, "I", false) == 0))) || (((StringType.StrCmp(str2, "J", false) == 0) || (StringType.StrCmp(str2, "L", false) == 0)) || ((StringType.StrCmp(str2, "N", false) == 0) || (StringType.StrCmp(str2, "O", false) == 0)))) || (((StringType.StrCmp(str2, "P", false) == 0) || (StringType.StrCmp(str2, "Q", false) == 0)) || ((StringType.StrCmp(str2, "S", false) == 0) || (StringType.StrCmp(str2, "U", false) == 0)))))
                {
                    this.DrawNumber(g, rectToDraw, this._ForeColor, sLeft);
                }
                else if (StringType.StrCmp(str2, ":", false) == 0)
                {
                    this.DrawTwoP(g, rectToDraw);
                }
                else if (StringType.StrCmp(str2, ".", false) == 0)
                {
                    this.DrawOneP(g, rectToDraw);
                }
            }
        }

        private void DrawFromRight(Graphics g, string strText)
        {
            string sLeft = "";
            Rectangle rectToDraw = new Rectangle();
            double num2 = 0;
            rectToDraw.X = (int)Math.Round((double)((this.Width - this._BorderWidth) - this._Margin));
            rectToDraw.Y = (int)Math.Round((double)(this._BorderWidth + this._Margin));
            rectToDraw.Width = (int)Math.Round((double)this._LEDWidth);
            rectToDraw.Height = this.Height - (rectToDraw.Y * 2);
            for (short i = (short)strText.Length; i >= 1; i--)
            {
                sLeft = strText.Substring(i - 1, 1).ToUpper();
                if (sLeft == ".")
                {
                    num2 = num2 + 0.5;
                    rectToDraw.X = (int)Math.Round((double)(((this.Width - this._BorderWidth) - this._Margin) - (num2 * (this._LEDInterval + this._LEDWidth))));
                }
                else
                {
                    num2 = (num2 + 1);
                    rectToDraw.X = (int)Math.Round((double)(((this.Width - this._BorderWidth) - this._Margin) - (num2 * (this._LEDInterval + this._LEDWidth))));
                }

                string str2 = sLeft;
                if ((((((StringType.StrCmp(str2, "0", false) == 0) || (StringType.StrCmp(str2, "1", false) == 0)) || ((StringType.StrCmp(str2, "2", false) == 0) || (StringType.StrCmp(str2, "3", false) == 0))) || (((StringType.StrCmp(str2, "4", false) == 0) || (StringType.StrCmp(str2, "5", false) == 0)) || ((StringType.StrCmp(str2, "6", false) == 0) || (StringType.StrCmp(str2, "7", false) == 0)))) || ((((StringType.StrCmp(str2, "8", false) == 0) || (StringType.StrCmp(str2, "9", false) == 0)) || ((StringType.StrCmp(str2, "-", false) == 0) || (StringType.StrCmp(str2, "A", false) == 0))) || (((StringType.StrCmp(str2, "B", false) == 0) || (StringType.StrCmp(str2, "C", false) == 0)) || ((StringType.StrCmp(str2, "D", false) == 0) || (StringType.StrCmp(str2, "E", false) == 0))))) || (((((StringType.StrCmp(str2, "F", false) == 0) || (StringType.StrCmp(str2, "G", false) == 0)) || ((StringType.StrCmp(str2, "H", false) == 0) || (StringType.StrCmp(str2, "I", false) == 0))) || (((StringType.StrCmp(str2, "J", false) == 0) || (StringType.StrCmp(str2, "L", false) == 0)) || ((StringType.StrCmp(str2, "N", false) == 0) || (StringType.StrCmp(str2, "O", false) == 0)))) || (((StringType.StrCmp(str2, "P", false) == 0) || (StringType.StrCmp(str2, "Q", false) == 0)) || ((StringType.StrCmp(str2, "S", false) == 0) || (StringType.StrCmp(str2, "U", false) == 0) || (StringType.StrCmp(str2, "|", false) == 0)))))
                {
                    this.DrawNumber(g, rectToDraw, this._ForeColor, sLeft);
                }
                else if (StringType.StrCmp(str2, ":", false) == 0)
                {
                    this.DrawTwoP(g, rectToDraw);
                }
                else if (StringType.StrCmp(str2, ".", false) == 0)
                {
                    this.DrawOneP(g, rectToDraw);
                }
            }
        }

        private void DrawNumber(Graphics g, Rectangle RectToDraw, Color UseColor, string strNumChar)
        {
            string sLeft = strNumChar;
            if (StringType.StrCmp(sLeft, "0", false) == 0)
            {
                this.DrawSegment(g, RectToDraw, UseColor, 1);
                this.DrawSegment(g, RectToDraw, UseColor, 3);
                this.DrawSegment(g, RectToDraw, UseColor, 4);
                this.DrawSegment(g, RectToDraw, UseColor, 5);
                this.DrawSegment(g, RectToDraw, UseColor, 6);
                this.DrawSegment(g, RectToDraw, UseColor, 7);
            }
            else if (StringType.StrCmp(sLeft, "1", false) == 0)
            {
                this.DrawSegment(g, RectToDraw, UseColor, 5);
                this.DrawSegment(g, RectToDraw, UseColor, 7);
            }
            else if (StringType.StrCmp(sLeft, "2", false) == 0)
            {
                this.DrawSegment(g, RectToDraw, UseColor, 1);
                this.DrawSegment(g, RectToDraw, UseColor, 5);
                this.DrawSegment(g, RectToDraw, UseColor, 2);
                this.DrawSegment(g, RectToDraw, UseColor, 6);
                this.DrawSegment(g, RectToDraw, UseColor, 3);
            }
            else if (StringType.StrCmp(sLeft, "3", false) == 0)
            {
                this.DrawSegment(g, RectToDraw, UseColor, 1);
                this.DrawSegment(g, RectToDraw, UseColor, 5);
                this.DrawSegment(g, RectToDraw, UseColor, 2);
                this.DrawSegment(g, RectToDraw, UseColor, 7);
                this.DrawSegment(g, RectToDraw, UseColor, 3);
            }
            else if (StringType.StrCmp(sLeft, "4", false) == 0)
            {
                this.DrawSegment(g, RectToDraw, UseColor, 4);
                this.DrawSegment(g, RectToDraw, UseColor, 2);
                this.DrawSegment(g, RectToDraw, UseColor, 5);
                this.DrawSegment(g, RectToDraw, UseColor, 7);
            }
            else if (StringType.StrCmp(sLeft, "5", false) == 0)
            {
                this.DrawSegment(g, RectToDraw, UseColor, 1);
                this.DrawSegment(g, RectToDraw, UseColor, 4);
                this.DrawSegment(g, RectToDraw, UseColor, 2);
                this.DrawSegment(g, RectToDraw, UseColor, 7);
                this.DrawSegment(g, RectToDraw, UseColor, 3);
            }
            else if (StringType.StrCmp(sLeft, "6", false) == 0)
            {
                this.DrawSegment(g, RectToDraw, UseColor, 1);
                this.DrawSegment(g, RectToDraw, UseColor, 4);
                this.DrawSegment(g, RectToDraw, UseColor, 6);
                this.DrawSegment(g, RectToDraw, UseColor, 3);
                this.DrawSegment(g, RectToDraw, UseColor, 7);
                this.DrawSegment(g, RectToDraw, UseColor, 2);
            }
            else if (StringType.StrCmp(sLeft, "7", false) == 0)
            {
                this.DrawSegment(g, RectToDraw, UseColor, 1);
                this.DrawSegment(g, RectToDraw, UseColor, 5);
                this.DrawSegment(g, RectToDraw, UseColor, 7);
            }
            else if (StringType.StrCmp(sLeft, "8", false) == 0)
            {
                this.DrawSegment(g, RectToDraw, UseColor, 1);
                this.DrawSegment(g, RectToDraw, UseColor, 4);
                this.DrawSegment(g, RectToDraw, UseColor, 6);
                this.DrawSegment(g, RectToDraw, UseColor, 3);
                this.DrawSegment(g, RectToDraw, UseColor, 7);
                this.DrawSegment(g, RectToDraw, UseColor, 5);
                this.DrawSegment(g, RectToDraw, UseColor, 2);
            }
            else if (StringType.StrCmp(sLeft, "9", false) == 0)
            {
                this.DrawSegment(g, RectToDraw, UseColor, 1);
                this.DrawSegment(g, RectToDraw, UseColor, 2);
                this.DrawSegment(g, RectToDraw, UseColor, 3);
                this.DrawSegment(g, RectToDraw, UseColor, 4);
                this.DrawSegment(g, RectToDraw, UseColor, 5);
                this.DrawSegment(g, RectToDraw, UseColor, 7);
            }
            else if (StringType.StrCmp(sLeft, "A", false) == 0)
            {
                this.DrawSegment(g, RectToDraw, UseColor, 1);
                this.DrawSegment(g, RectToDraw, UseColor, 2);
                this.DrawSegment(g, RectToDraw, UseColor, 4);
                this.DrawSegment(g, RectToDraw, UseColor, 5);
                this.DrawSegment(g, RectToDraw, UseColor, 6);
                this.DrawSegment(g, RectToDraw, UseColor, 7);
            }
            else if (StringType.StrCmp(sLeft, "B", false) == 0)
            {
                this.DrawSegment(g, RectToDraw, UseColor, 1);
                this.DrawSegment(g, RectToDraw, UseColor, 2);
                this.DrawSegment(g, RectToDraw, UseColor, 3);
                this.DrawSegment(g, RectToDraw, UseColor, 4);
                this.DrawSegment(g, RectToDraw, UseColor, 5);
                this.DrawSegment(g, RectToDraw, UseColor, 6);
                this.DrawSegment(g, RectToDraw, UseColor, 7);
            }
            else if (StringType.StrCmp(sLeft, "C", false) == 0)
            {
                this.DrawSegment(g, RectToDraw, UseColor, 1);
                this.DrawSegment(g, RectToDraw, UseColor, 3);
                this.DrawSegment(g, RectToDraw, UseColor, 4);
                this.DrawSegment(g, RectToDraw, UseColor, 6);
            }
            else if (StringType.StrCmp(sLeft, "D", false) == 0)
            {
                this.DrawSegment(g, RectToDraw, UseColor, 5);
                this.DrawSegment(g, RectToDraw, UseColor, 2);
                this.DrawSegment(g, RectToDraw, UseColor, 6);
                this.DrawSegment(g, RectToDraw, UseColor, 3);
                this.DrawSegment(g, RectToDraw, UseColor, 7);
            }
            else if (StringType.StrCmp(sLeft, "E", false) == 0)
            {
                this.DrawSegment(g, RectToDraw, UseColor, 1);
                this.DrawSegment(g, RectToDraw, UseColor, 2);
                this.DrawSegment(g, RectToDraw, UseColor, 3);
                this.DrawSegment(g, RectToDraw, UseColor, 4);
                this.DrawSegment(g, RectToDraw, UseColor, 6);
            }
            else if (StringType.StrCmp(sLeft, "F", false) == 0)
            {
                this.DrawSegment(g, RectToDraw, UseColor, 1);
                this.DrawSegment(g, RectToDraw, UseColor, 2);
                this.DrawSegment(g, RectToDraw, UseColor, 4);
                this.DrawSegment(g, RectToDraw, UseColor, 6);
            }
            else if (StringType.StrCmp(sLeft, "G", false) == 0)
            {
                this.DrawSegment(g, RectToDraw, UseColor, 1);
                this.DrawSegment(g, RectToDraw, UseColor, 4);
                this.DrawSegment(g, RectToDraw, UseColor, 6);
                this.DrawSegment(g, RectToDraw, UseColor, 3);
                this.DrawSegment(g, RectToDraw, UseColor, 7);
                this.DrawSegment(g, RectToDraw, UseColor, 2);
            }
            else if (StringType.StrCmp(sLeft, "H", false) == 0)
            {
                this.DrawSegment(g, RectToDraw, UseColor, 4);
                this.DrawSegment(g, RectToDraw, UseColor, 2);
                this.DrawSegment(g, RectToDraw, UseColor, 5);
                this.DrawSegment(g, RectToDraw, UseColor, 7);
                this.DrawSegment(g, RectToDraw, UseColor, 6);
            }
            else if (StringType.StrCmp(sLeft, "I", false) == 0)
            {
                this.DrawSegment(g, RectToDraw, UseColor, 5);
                this.DrawSegment(g, RectToDraw, UseColor, 7);
            }
            else if (StringType.StrCmp(sLeft, "J", false) == 0)
            {
                this.DrawSegment(g, RectToDraw, UseColor, 5);
                this.DrawSegment(g, RectToDraw, UseColor, 7);
                this.DrawSegment(g, RectToDraw, UseColor, 3);
                this.DrawSegment(g, RectToDraw, UseColor, 6);
            }
            else if (StringType.StrCmp(sLeft, "L", false) == 0)
            {
                this.DrawSegment(g, RectToDraw, UseColor, 4);
                this.DrawSegment(g, RectToDraw, UseColor, 6);
                this.DrawSegment(g, RectToDraw, UseColor, 3);
            }
            else if (StringType.StrCmp(sLeft, "N", false) == 0)
            {
                this.DrawSegment(g, RectToDraw, UseColor, 4);
                this.DrawSegment(g, RectToDraw, UseColor, 6);
                this.DrawSegment(g, RectToDraw, UseColor, 1);
                this.DrawSegment(g, RectToDraw, UseColor, 5);
                this.DrawSegment(g, RectToDraw, UseColor, 7);
            }
            else if (StringType.StrCmp(sLeft, "O", false) == 0)
            {
                this.DrawSegment(g, RectToDraw, UseColor, 1);
                this.DrawSegment(g, RectToDraw, UseColor, 3);
                this.DrawSegment(g, RectToDraw, UseColor, 4);
                this.DrawSegment(g, RectToDraw, UseColor, 5);
                this.DrawSegment(g, RectToDraw, UseColor, 6);
                this.DrawSegment(g, RectToDraw, UseColor, 7);
            }
            else if (StringType.StrCmp(sLeft, "P", false) == 0)
            {
                this.DrawSegment(g, RectToDraw, UseColor, 1);
                this.DrawSegment(g, RectToDraw, UseColor, 2);
                this.DrawSegment(g, RectToDraw, UseColor, 4);
                this.DrawSegment(g, RectToDraw, UseColor, 5);
                this.DrawSegment(g, RectToDraw, UseColor, 6);
            }
            else if (StringType.StrCmp(sLeft, "Q", false) == 0)
            {
                this.DrawSegment(g, RectToDraw, UseColor, 2);
                this.DrawSegment(g, RectToDraw, UseColor, 4);
                this.DrawSegment(g, RectToDraw, UseColor, 1);
                this.DrawSegment(g, RectToDraw, UseColor, 5);
                this.DrawSegment(g, RectToDraw, UseColor, 7);
            }
            else if (StringType.StrCmp(sLeft, "S", false) == 0)
            {
                this.DrawSegment(g, RectToDraw, UseColor, 1);
                this.DrawSegment(g, RectToDraw, UseColor, 2);
                this.DrawSegment(g, RectToDraw, UseColor, 3);
                this.DrawSegment(g, RectToDraw, UseColor, 4);
                this.DrawSegment(g, RectToDraw, UseColor, 7);
            }
            else if (StringType.StrCmp(sLeft, "U", false) == 0)
            {
                this.DrawSegment(g, RectToDraw, UseColor, 4);
                this.DrawSegment(g, RectToDraw, UseColor, 6);
                this.DrawSegment(g, RectToDraw, UseColor, 3);
                this.DrawSegment(g, RectToDraw, UseColor, 7);
                this.DrawSegment(g, RectToDraw, UseColor, 5);
            }
            else if (StringType.StrCmp(sLeft, "|", false) == 0)
            {
                this.DrawSegment(g, RectToDraw, UseColor, 4);
                this.DrawSegment(g, RectToDraw, UseColor, 6);
                //this.DrawSegment(g, RectToDraw, UseColor, 7);
                //this.DrawSegment(g, RectToDraw, UseColor, 5);
            }
            else if (StringType.StrCmp(sLeft, "-", false) == 0)
            {
                this.DrawSegment(g, RectToDraw, UseColor, 2);
            }
        }

        private void DrawOneP(Graphics g, RectangleF RectToDraw)
        {
            float width = RectToDraw.Width * 0.2f;
            float x = RectToDraw.X + width * 0.5f;
            float y = (RectToDraw.Y + RectToDraw.Height) - width - this._SegmentWidth / 2.0f;
            SolidBrush brush = new SolidBrush(this._ForeColor);
            g.FillRectangle(brush, x, y, width, width);
            brush.Dispose();
        }

        private void DrawSegment(Graphics g, Rectangle RectToDraw, Color UseColor, int SegNum)
        {
            PointF tf;
            float num = this._CornerRate;
            GraphicsPath path = new GraphicsPath();
            PointF[] points = new PointF[6];
            switch (SegNum)
            {
                case 1:
                    tf = new PointF((this._SegmentWidth * num) + this._SegmentInterval, this._SegmentWidth * num);
                    points[0] = tf;
                    tf = new PointF(this._SegmentInterval + ((2f * this._SegmentWidth) * num), 0f);
                    points[1] = tf;
                    tf = new PointF((RectToDraw.Width - this._SegmentInterval) - ((2f * this._SegmentWidth) * num), 0f);
                    points[2] = tf;
                    tf = new PointF((RectToDraw.Width - this._SegmentInterval) - (this._SegmentWidth * num), this._SegmentWidth * num);
                    points[3] = tf;
                    tf = new PointF((RectToDraw.Width - this._SegmentInterval) - this._SegmentWidth, this._SegmentWidth);
                    points[4] = tf;
                    tf = new PointF(this._SegmentWidth + this._SegmentInterval, this._SegmentWidth);
                    points[5] = tf;
                    break;

                case 2:
                    tf = new PointF(((float)(this._SegmentWidth * 0.5)) + this._SegmentInterval, (float)(RectToDraw.Height * 0.5));
                    points[0] = tf;
                    tf = new PointF(this._SegmentWidth + this._SegmentInterval, (float)((RectToDraw.Height * 0.5) - (this._SegmentWidth * 0.5)));
                    points[1] = tf;
                    tf = new PointF((RectToDraw.Width - this._SegmentInterval) - this._SegmentWidth, (float)((RectToDraw.Height * 0.5) - (this._SegmentWidth * 0.5)));
                    points[2] = tf;
                    tf = new PointF((RectToDraw.Width - this._SegmentInterval) - ((float)(this._SegmentWidth * 0.5)), (float)(RectToDraw.Height * 0.5));
                    points[3] = tf;
                    tf = new PointF((RectToDraw.Width - this._SegmentInterval) - this._SegmentWidth, (float)((RectToDraw.Height * 0.5) + (this._SegmentWidth * 0.5)));
                    points[4] = tf;
                    tf = new PointF(this._SegmentWidth + this._SegmentInterval, (float)((RectToDraw.Height * 0.5) + (this._SegmentWidth * 0.5)));
                    points[5] = tf;
                    break;

                case 3:
                    tf = new PointF((this._SegmentWidth * num) + this._SegmentInterval, RectToDraw.Height - (this._SegmentWidth * num));
                    points[0] = tf;
                    tf = new PointF(this._SegmentWidth + this._SegmentInterval, RectToDraw.Height - this._SegmentWidth);
                    points[1] = tf;
                    tf = new PointF((RectToDraw.Width - this._SegmentInterval) - this._SegmentWidth, RectToDraw.Height - this._SegmentWidth);
                    points[2] = tf;
                    tf = new PointF((RectToDraw.Width - this._SegmentInterval) - (this._SegmentWidth * num), RectToDraw.Height - (this._SegmentWidth * num));
                    points[3] = tf;
                    tf = new PointF((RectToDraw.Width - this._SegmentInterval) - ((2f * this._SegmentWidth) * num), (float)RectToDraw.Height);
                    points[4] = tf;
                    tf = new PointF(((2f * this._SegmentWidth) * num) + this._SegmentInterval, (float)RectToDraw.Height);
                    points[5] = tf;
                    break;

                case 4:
                    tf = new PointF(0f, ((2f * this._SegmentWidth) * num) + this._SegmentInterval);
                    points[0] = tf;
                    tf = new PointF(this._SegmentWidth * num, (this._SegmentWidth * num) + this._SegmentInterval);
                    points[1] = tf;
                    tf = new PointF(this._SegmentWidth, this._SegmentWidth + this._SegmentInterval);
                    points[2] = tf;
                    tf = new PointF(this._SegmentWidth, ((float)((((double)RectToDraw.Height) / 2.0) - (this._SegmentWidth * 0.5))) - this._SegmentInterval);
                    points[3] = tf;
                    tf = new PointF(this._SegmentWidth / 2f, ((float)(((double)RectToDraw.Height) / 2.0)) - this._SegmentInterval);
                    points[4] = tf;
                    tf = new PointF(0f, ((float)((((double)RectToDraw.Height) / 2.0) - (this._SegmentWidth * 0.5))) - this._SegmentInterval);
                    points[5] = tf;
                    break;

                case 5:
                    tf = new PointF(RectToDraw.Width - this._SegmentWidth, this._SegmentWidth + this._SegmentInterval);
                    points[0] = tf;
                    tf = new PointF(RectToDraw.Width - (this._SegmentWidth * num), (this._SegmentWidth * num) + this._SegmentInterval);
                    points[1] = tf;
                    tf = new PointF((float)RectToDraw.Width, ((2f * this._SegmentWidth) * num) + this._SegmentInterval);
                    points[2] = tf;
                    tf = new PointF((float)RectToDraw.Width, ((float)((RectToDraw.Height * 0.5) - (this._SegmentWidth * 0.5))) - this._SegmentInterval);
                    points[3] = tf;
                    tf = new PointF(RectToDraw.Width - (this._SegmentWidth / 2f), ((float)(((double)RectToDraw.Height) / 2.0)) - this._SegmentInterval);
                    points[4] = tf;
                    tf = new PointF(RectToDraw.Width - this._SegmentWidth, ((float)((((double)RectToDraw.Height) / 2.0) - (this._SegmentWidth * 0.5))) - this._SegmentInterval);
                    points[5] = tf;
                    break;

                case 6:
                    tf = new PointF(0f, (float)(((RectToDraw.Height * 0.5) + this._SegmentInterval) + (this._SegmentWidth * 0.5)));
                    points[0] = tf;
                    tf = new PointF((float)(this._SegmentWidth * 0.5), ((float)(RectToDraw.Height * 0.5)) + this._SegmentInterval);
                    points[1] = tf;
                    tf = new PointF(this._SegmentWidth, (float)(((RectToDraw.Height * 0.5) + this._SegmentInterval) + (this._SegmentWidth * 0.5)));
                    points[2] = tf;
                    tf = new PointF(this._SegmentWidth, (RectToDraw.Height - this._SegmentWidth) - this._SegmentInterval);
                    points[3] = tf;
                    tf = new PointF(this._SegmentWidth * num, (RectToDraw.Height - (this._SegmentWidth * num)) - this._SegmentInterval);
                    points[4] = tf;
                    tf = new PointF(0f, (RectToDraw.Height - this._SegmentInterval) - ((2f * this._SegmentWidth) * num));
                    points[5] = tf;
                    break;

                case 7:
                    tf = new PointF(RectToDraw.Width - this._SegmentWidth, (float)(((RectToDraw.Height * 0.5) + this._SegmentInterval) + (this._SegmentWidth * 0.5)));
                    points[0] = tf;
                    tf = new PointF((float)(RectToDraw.Width - (this._SegmentWidth * 0.5)), ((float)(RectToDraw.Height * 0.5)) + this._SegmentInterval);
                    points[1] = tf;
                    tf = new PointF((float)RectToDraw.Width, (float)(((RectToDraw.Height * 0.5) + this._SegmentInterval) + (this._SegmentWidth * 0.5)));
                    points[2] = tf;
                    tf = new PointF((float)RectToDraw.Width, (RectToDraw.Height - this._SegmentInterval) - ((2f * this._SegmentWidth) * num));
                    points[3] = tf;
                    tf = new PointF(RectToDraw.Width - (this._SegmentWidth * num), (RectToDraw.Height - (this._SegmentWidth * num)) - this._SegmentInterval);
                    points[4] = tf;
                    tf = new PointF(RectToDraw.Width - this._SegmentWidth, (RectToDraw.Height - this._SegmentWidth) - this._SegmentInterval);
                    points[5] = tf;
                    break;
            }
            SolidBrush brush = new SolidBrush(UseColor);
            path.AddPolygon(points);
            path.CloseFigure();
            Matrix matrix = new Matrix();
            matrix.Translate((float)RectToDraw.X, (float)RectToDraw.Y);
            path.Transform(matrix);
            if (this._IsAntiAlias)
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
            }
            else
            {
                g.SmoothingMode = SmoothingMode.Default;
            }
            Rectangle rect = new Rectangle(1, 1, this.Width - 2, this.Height - 2);
            g.Clip = new Region(rect);
            g.FillPath(brush, path);
            brush.Dispose();
        }

        private void DrawShadow(Graphics g)
        {
            short num;
            short lEDNum = 0;
            Rectangle rectToDraw = new Rectangle();
            lEDNum = (short)this.GetLEDNum(this._Text.Trim());
            if (this._IsCustomLEDNum && ((this._CustomLEDNums - this.GetLEDNum(this._Text.Trim())) > 0))
            {
                lEDNum = (short)this._CustomLEDNums;
            }
            switch (this._TextAlign)
            {
                case AlignType.Left:
                    {
                        rectToDraw.X = 0;
                        rectToDraw.Y = (int)Math.Round((double)(this._BorderWidth + this._Margin));
                        rectToDraw.Width = (int)Math.Round((double)this._LEDWidth);
                        rectToDraw.Height = this.Height - (rectToDraw.Y * 2);
                        short num5 = (short)(lEDNum - 1);
                        for (num = 0; num <= num5; num = (short)(num + 1))
                        {
                            rectToDraw.X = (int)Math.Round((double)(((this._BorderWidth + this._Margin) + this._LEDInterval) + (num * (this._LEDInterval + this._LEDWidth))));
                            this.DrawNumber(g, rectToDraw, this._ShadowColor, "8");
                        }
                        break;
                    }
                case AlignType.Right:
                    {
                        rectToDraw.X = 0;
                        rectToDraw.Y = (int)Math.Round((double)(this._BorderWidth + this._Margin));
                        rectToDraw.Width = (int)Math.Round((double)this._LEDWidth);
                        rectToDraw.Height = this.Height - (rectToDraw.Y * 2);
                        short num4 = lEDNum;
                        for (num = 1; num <= num4; num = (short)(num + 1))
                        {
                            rectToDraw.X = (int)Math.Round((double)(((this.Width - this._BorderWidth) - this._Margin) - (num * (this._LEDInterval + this._LEDWidth))));
                            this.DrawNumber(g, rectToDraw, this._ShadowColor, "8");
                        }
                        break;
                    }
            }
        }

        private void DrawTwoP(Graphics g, Rectangle RectToDraw)
        {
            //Rectangle rectangle = new Rectangle();
            float width = RectToDraw.Width * 0.2f;
            float x = RectToDraw.X + ((RectToDraw.Width - width) / 2f);
            float y = (RectToDraw.Y + this._SegmentWidth) + ((float)(((((((double)RectToDraw.Height) / 2.0) - this._SegmentWidth) - (this._SegmentWidth / 2f)) - width) * 0.699999988079071));
            float num4 = (float)(((RectToDraw.Y + (((double)RectToDraw.Height) / 2.0)) + (this._SegmentWidth / 2f)) + (((((((double)RectToDraw.Height) / 2.0) - this._SegmentWidth) - (this._SegmentWidth / 2f)) - width) * 0.30000001192092896));
            SolidBrush brush = new SolidBrush(this._ForeColor);
            g.FillRectangle(brush, x, y, width, width);
            g.FillRectangle(brush, x, num4, width, width);
            brush.Dispose();
        }

        private int GetLEDNum(string strText)
        {
            return strText.Trim().Replace(".", "").Length;
        }

        [System.Diagnostics.DebuggerStepThrough]
        private void InitializeComponent()
        {
            this.Name = "LEDNumber";
            Size size = new Size(0x134, 0x2c);
            this.Size = size;
            this.SetStyle(ControlStyles.DoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.ResizeRedraw, true);
        }

        [Description("单击LED控件时发生。"), Browsable(true)]
        private void LEDNumber_Click(object sender, EventArgs e)
        {
            if (this.LEDClick != null)
            {
                this.LEDClick(RuntimeHelpers.GetObjectValue(sender), e);
            }
        }

        [Description("双击LED控件时发生。"), Browsable(true)]
        private void LEDNumber_DoubleClick(object sender, EventArgs e)
        {
            if (this.LEDDoubleClick != null)
            {
                this.LEDDoubleClick(RuntimeHelpers.GetObjectValue(sender), e);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.Clear(this._BackColor);
            this.SuspendLayout();
            if (!this.TimerFlash.Enabled)
            {
                this.bolDrawText = true;
            }
            switch (this._LEDStyle)
            {
                case LEDStyleEnum.Custom:
                    if (this._Text.Trim() != "")
                    {
                        this.DrawShadow(e.Graphics);
                        if (this.bolDrawText)
                        {
                            switch (this._TextAlign)
                            {
                                case AlignType.Left:
                                    this.DrawFromLeft(e.Graphics, this._Text);
                                    break;

                                case AlignType.Right:
                                    {
                                        if (!this._IsCustomLEDNum)
                                        {
                                            this.DrawFromRight(e.Graphics, this._Text);
                                            break;
                                        }
                                        if (!this._IsZeroFirst)
                                        {
                                            this.DrawFromRight(e.Graphics, this._Text);
                                            break;
                                        }
                                        string str = "";
                                        short num2 = (short)(this._CustomLEDNums - this.GetLEDNum(this._Text));
                                        for (short i = 1; i <= num2; i = (short)(i + 1))
                                        {
                                            str = str + "0";
                                        }
                                        this.DrawFromRight(e.Graphics, str + this._Text);
                                        break;
                                    }
                            }
                        }
                    }
                    break;
                case LEDStyleEnum.DateTime:
                    try
                    {
                        this._Text = DateTime.Now.ToString(this._DateTimeFormatString);
                    }
                    catch (Exception exception1)
                    {
                        Microsoft.VisualBasic.CompilerServices.ProjectData.SetProjectError(exception1);
                        Exception exception = exception1;
                        Interaction.Beep();
                        Microsoft.VisualBasic.CompilerServices.ProjectData.ClearProjectError();
                        return;
                    }
                    this.DrawShadow(e.Graphics);
                    if (this.bolDrawText)
                    {
                        switch (this._TextAlign)
                        {
                            case AlignType.Left:
                                this.DrawFromLeft(e.Graphics, this._Text);
                                break;

                            case AlignType.Right:
                                this.DrawFromRight(e.Graphics, this._Text);
                                break;
                        }
                    }
                    break;
            }
            this.DrawBorder(e.Graphics);
            ////
            //using (Pen b = new Pen(Color.DarkGreen,5f))
            //{
            //    Rectangle rect = this.ClientRectangle;
            //    e.Graphics.DrawRectangle(b,rect.X,rect.Y,rect.Width,rect.Height);
            //}


            this.ResumeLayout();
        }

        protected override void OnResize(EventArgs e)
        {
            this.ParameterInitial();
            this.Invalidate();
        }

        private void ParameterInitial()
        {
            this._Margin = (float)Math.Round((double)(this.Height * 0.04f));
            this._LEDWidth = (float)Math.Round((double)(this.Height * 0.45f));
            this._LEDInterval = (float)Math.Round((double)(this.Height * 0.08f));
            this._SegmentWidth = (float)Math.Round((double)(this.Height * 0.1f));
            this._SegmentInterval = (float)Math.Round((double)(this.Height * 0.01f));
            if (this._Margin < 2f)
            {
                this._Margin = 2f;
            }
            if (this._LEDWidth < 5f)
            {
                this._LEDWidth = 5f;
            }
            if (this._LEDInterval < 3f)
            {
                this._LEDInterval = 3f;
            }
            if (this._SegmentWidth < 1f)
            {
                this._SegmentWidth = 1f;
            }
            if (this._SegmentInterval < 1f)
            {
                this._SegmentInterval = 1f;
            }
        }

        #region 颜色或样式重置
        public void ResetBorderColor()
        {
            this._BorderColor = Color.Yellow;
            this.Invalidate();
        }

        public void ResetBorderLightColor()
        {
            this._BorderLightColor = SystemColors.ControlLight;
            this.Invalidate();
        }

        public void ResetBorderShadowColor()
        {
            this._BorderShadowColor = SystemColors.ControlDark;
            this.Invalidate();
        }

        public void ResetBorderStyle()
        {
            this._BorderStyle = BorderStyleEnum.Outside;
            this.Invalidate();
        }

        public void ResetLEDBackColor()
        {
            this._BackColor = Color.Black;
            this.Invalidate();
        }

        public void ResetLEDForeColor()
        {
            this._ForeColor = Color.Red;
            this.Invalidate();
        }

        public void ResetLEDShadowColor()
        {
            this._ShadowColor = Color.FromArgb(100, 100, 100);
            this.Invalidate();
        }

        public void ResetLEDStyle()
        {
            this._LEDStyle = LEDStyleEnum.Custom;
            this.Invalidate();
        }

        public void ResetTextAlign()
        {
            this._TextAlign = AlignType.Right;
            this.Invalidate();
        }
        #endregion

        #region 是否序列化颜色或样式

        public bool ShouldSerializeBorderColor()
        {
            if (this._BorderColor == Color.Yellow)
            {
                return false;
            }
            return true;
        }

        public bool ShouldSerializeBorderLightColor()
        {
            if (this._BorderLightColor == SystemColors.ControlLight)
            {
                return false;
            }
            return true;
        }

        public bool ShouldSerializeBorderShadowColor()
        {
            if (this._BorderShadowColor == SystemColors.ControlDark)
            {
                return false;
            }
            return true;
        }

        public bool ShouldSerializeBorderStyle()
        {
            if (this._BorderStyle == BorderStyleEnum.Outside)
            {
                return false;
            }
            return true;
        }

        public bool ShouldSerializeLEDBackColor()
        {
            if (this._BackColor == Color.Black)
            {
                return false;
            }
            return true;
        }

        public bool ShouldSerializeLEDForeColor()
        {
            if (this._ForeColor == Color.Red)
            {
                return false;
            }
            return true;
        }

        public bool ShouldSerializeLEDShadowColor()
        {
            if (this._ShadowColor == Color.FromArgb(100, 100, 100))
            {
                return false;
            }
            return true;
        }

        public bool ShouldSerializeLEDStyle()
        {
            if (this._LEDStyle == LEDStyleEnum.Custom)
            {
                return false;
            }
            return true;
        }

        public bool ShouldSerializeTextAlign()
        {
            if (this._TextAlign == AlignType.Right)
            {
                return false;
            }
            return true;
        }
        #endregion

        private void subFlashText()
        {
            this.bolDrawText = !this.bolDrawText;
            this.Invalidate();
            System.Threading.Thread.Sleep(this._DarkTime);
            this.bolDrawText = !this.bolDrawText;
            this.Invalidate();
        }

        private void Timer1_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            this.Invalidate();
        }

        private void TimerFlash_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if ((this.m_FlashThread == null) || (this.m_FlashThread.ThreadState != System.Threading.ThreadState.Running))
            {
                this.m_FlashThread = new System.Threading.Thread(new System.Threading.ThreadStart(this.subFlashText));
                this.m_FlashThread.Name = "FlashText";
                this.m_FlashThread.Priority = System.Threading.ThreadPriority.BelowNormal;
                this.m_FlashThread.Start();
            }
        }

        #region 属性

        // Properties
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public override Color BackColor
        {
            get
            {
                return base.BackColor;
            }
            set
            {
                base.BackColor = value;
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
        public override Image BackgroundImage
        {
            get
            {
                return base.BackgroundImage;
            }
            set
            {
                base.BackgroundImage = value;
            }
        }

        [Description("边框颜色。"), Category("LED边框")]
        public Color BorderColor
        {
            get
            {
                return this._BorderColor;
            }
            set
            {
                this._BorderColor = value;
                this.Invalidate();
            }
        }

        [Description("边框亮面颜色。"), Category("LED边框")]
        public Color BorderLightColor
        {
            get
            {
                return this._BorderLightColor;
            }
            set
            {
                this._BorderLightColor = value;
                this.Invalidate();
            }
        }

        [Description("边框暗面颜色。"), Category("LED边框")]
        public Color BorderShadowColor
        {
            get
            {
                return this._BorderShadowColor;
            }
            set
            {
                this._BorderShadowColor = value;
                this.Invalidate();
            }
        }

        [Category("LED边框"), Description("边框风格。")]
        public BorderStyleEnum BorderStyle
        {
            get
            {
                return this._BorderStyle;
            }
            set
            {
                this._BorderStyle = value;
                if (value == BorderStyleEnum.None)
                {
                }
                this.Invalidate();
            }
        }

        [Description("边框宽度。"), DefaultValue(3), Category("LED边框")]
        public int BorderWidth
        {
            get
            {
                return this._BorderWidth;
            }
            set
            {
                if (((value < 0) || (value > this.Height)) || (value > this.Width))
                {
                    throw new ArgumentOutOfRangeException("BorderWidth", "该值必须大于零且小于控件长宽的最小值");
                }
                this._BorderWidth = value;
                this.Invalidate();
            }
        }

        [Category("LED绘制"), Description("段末端尖利度。值在 0～0.5 之间。"), DefaultValue((float)0.25f)]
        public float CornerRate
        {
            get
            {
                return this._CornerRate;
            }
            set
            {
                if ((value < 0f) || (value > 0.5))
                {
                    throw new ArgumentOutOfRangeException("CornerRate", "段末端尖利度必须大等于 0 且小于 0.5");
                }
                this._CornerRate = value;
                this.Invalidate();
            }
        }

        [Description("自定义数码管的显示数量。"), DefaultValue(10), Category("LED自定义数量")]
        public int CustomLEDNums
        {
            get
            {
                return this._CustomLEDNums;
            }
            set
            {
                if ((value < 0) || (value > 50))
                {
                    throw new ArgumentOutOfRangeException("CustomLEDNums", "数码管的显示数量必须大于 0 且小于 50");
                }
                this._CustomLEDNums = value;
                this.Invalidate();
            }
        }

        [Category("LED闪烁"), Description("数码管闪烁时保持熄灭的时间（毫秒）。"), DefaultValue(300)]
        public int DarkTime
        {
            get
            {
                return this._DarkTime;
            }
            set
            {
                if ((value <= 50) || (value > this._FlashInterval))
                {
                    throw new ArgumentOutOfRangeException("DarkTime", "熄灭的时间必须大于 50毫秒 且小于数码管闪烁的间隔时间。");
                }
                this._DarkTime = value;
                this.Invalidate();
            }
        }

        [DefaultValue("yyyy-MM-dd hh:mm:ss"), Category("LED总体"), Description("采用与时间显示相关风格时的格式化字符串。")]
        public string DateTimeFormatString
        {
            get
            {
                return this._DateTimeFormatString;
            }
            set
            {
                value = value.Trim();
                try
                {
                    string str = DateTime.Now.ToString(value);
                }
                catch (Exception exception1)
                {
                    Microsoft.VisualBasic.CompilerServices.ProjectData.SetProjectError(exception1);
                    Exception exception = exception1;
                    throw new InvalidCastException("格式化字符串对时间类型无效");
                }
                this._DateTimeFormatString = value;
                this.Invalidate();
            }
        }

        [Description("数码管闪烁的间隔（毫秒）。"), DefaultValue(0x3e8), Category("LED闪烁")]
        public int FlashInterval
        {
            get
            {
                return this._FlashInterval;
            }
            set
            {
                if ((value < 100) || (value > 0xea60))
                {
                    throw new ArgumentOutOfRangeException("FlashInterval", "数码管闪烁的间隔必须大于 100毫秒 且小于 60000毫秒 (一分钟)");
                }
                this._FlashInterval = value;
                this.TimerFlash.Interval = value;
                this.Invalidate();
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
        public override Font Font
        {
            get
            {
                return base.Font;
            }
            set
            {
                base.Font = value;
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
        public override Color ForeColor
        {
            get
            {
                return base.ForeColor;
            }
            set
            {
                base.ForeColor = value;
            }
        }

        [DefaultValue(false), Description("是否采用反锯齿技术。"), Category("LED绘制")]
        public bool IsAntiAlias
        {
            get
            {
                return this._IsAntiAlias;
            }
            set
            {
                this._IsAntiAlias = value;
                this.Invalidate();
            }
        }

        [Description("是否自定义数码管的显示数量。"), Category("LED自定义数量"), DefaultValue(false)]
        public bool IsCustomLEDNum
        {
            get
            {
                return this._IsCustomLEDNum;
            }
            set
            {
                this._IsCustomLEDNum = value;
                this.Invalidate();
            }
        }

        [DefaultValue(false), Category("LED闪烁"), Description("是否数码管闪烁。")]
        public bool IsFlash
        {
            get
            {
                return this._IsFlash;
            }
            set
            {
                if (!value)
                {
                    this.bolDrawText = true;
                }
                this.TimerFlash.Enabled = value;
                this._IsFlash = value;
                this.Invalidate();
            }
        }

        [Category("LED自定义数量"), Description("是否前位补零。仅右对齐风格和自定义LED数量有效。"), DefaultValue(false)]
        public bool IsZeroFirst
        {
            get
            {
                return this._IsZeroFirst;
            }
            set
            {
                this._IsZeroFirst = value;
                this.Invalidate();
            }
        }

        [Description("LED控件的背景色。"), Category("LED绘制")]
        public Color LEDBackColor
        {
            get
            {
                return this._BackColor;
            }
            set
            {
                this._BackColor = value;
                this.Invalidate();
            }
        }

        [Category("LED绘制"), Description("LED数字的前景色。")]
        public Color LEDForeColor
        {
            get
            {
                return this._ForeColor;
            }
            set
            {
                this._ForeColor = value;
                this.Invalidate();
            }
        }

        [Category("LED绘制"), Description("LED数字的阴影颜色。")]
        public Color LEDShadowColor
        {
            get
            {
                return this._ShadowColor;
            }
            set
            {
                this._ShadowColor = value;
                this.Invalidate();
            }
        }

        [Category("LED总体"), Description("控件显示风格。")]
        public LEDStyleEnum LEDStyle
        {
            get
            {
                return this._LEDStyle;
            }
            set
            {
                switch (value)
                {
                    case LEDStyleEnum.Custom:
                        this.Timer1.Enabled = false;
                        break;

                    case LEDStyleEnum.DateTime:
                        if (StringType.StrCmp(this._DateTimeFormatString, "", false) == 0)
                        {
                            this._DateTimeFormatString = "yyyy-MM-dd HH:mm:ss";
                        }
                        try
                        {
                            string str = DateTime.Now.ToString(this._DateTimeFormatString);
                        }
                        catch (Exception exception1)
                        {
                            Microsoft.VisualBasic.CompilerServices.ProjectData.SetProjectError(exception1);
                            Exception exception = exception1;
                            this.Timer1.Enabled = false;
                            throw new InvalidCastException("格式化字符串对时间类型无效");
                        }
                        this.Timer1.Enabled = true;
                        break;
                }
                this._LEDStyle = value;
                this.Invalidate();
            }
        }
        /// <summary>
        /// 显示内容
        /// </summary>
        [DefaultValue("0"), Category("LED总体"), Description("显示内容。")]
        public override string Text
        {
            get
            {
                return this._Text;
            }
            set
            {
                this._Text = value;
                this.Invalidate();
            }
        }
        /// <summary>
        /// 文本的对齐方式
        /// </summary>
        [Category("LED总体"), Description("文本的对齐方式。")]
        public AlignType TextAlign
        {
            get
            {
                return this._TextAlign;
            }
            set
            {
                this._TextAlign = value;
                this.Invalidate();
            }
        }

        private System.Timers.Timer Timer1
        {
            get
            {
                return this._Timer1;
            }
            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (this._Timer1 != null)
                {
                    this._Timer1.Elapsed -= new ElapsedEventHandler(this.Timer1_Elapsed);
                }
                this._Timer1 = value;
                if (this._Timer1 != null)
                {
                    this._Timer1.Elapsed += new ElapsedEventHandler(this.Timer1_Elapsed);
                }
            }
        }

        private System.Timers.Timer TimerFlash
        {
            get
            {
                return this._TimerFlash;
            }
            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (this._TimerFlash != null)
                {
                    this._TimerFlash.Elapsed -= new ElapsedEventHandler(this.TimerFlash_Elapsed);
                }
                this._TimerFlash = value;
                if (this._TimerFlash != null)
                {
                    this._TimerFlash.Elapsed += new ElapsedEventHandler(this.TimerFlash_Elapsed);
                }
            }
        }
        #endregion

        #region 定义的数据类型
        /// <summary>
        /// 文本对齐类型
        /// </summary>
        public enum AlignType
        {
            /// <summary>
            /// 左对齐
            /// </summary>
            Left,
            /// <summary>
            /// 右对齐
            /// </summary>
            Right
        }
        /// <summary>
        /// 边线类型
        /// </summary>
        public enum BorderStyleEnum
        {
            /// <summary>
            /// 
            /// </summary>
            Outside,
            /// <summary>
            /// 
            /// </summary>
            Inside,
            /// <summary>
            /// 
            /// </summary>
            Flat,
            /// <summary>
            /// 无边框
            /// </summary>
            None
        }
        /// <summary>
        /// 自定义数码管显示类型
        /// </summary>
        public enum LEDStyleEnum
        {
            /// <summary>
            /// Custom显示数字字符
            /// </summary>
            Custom,
            /// <summary>
            /// DateTime表示显示系统时间
            /// </summary>
            DateTime
        }
        private delegate void FlashDelegate();

        #endregion


    }
}
