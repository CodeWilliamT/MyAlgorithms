using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace LeadframeAOI.UI.ControlWrap
{
    [ToolboxBitmap(typeof(Panel))]
    [ToolboxItem(true)]
    public partial class PanelEx : Panel
    {
        public PanelEx()
        {
            //this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.SupportsTransparentBackColor | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);
            base.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            base.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            base.SetStyle(ControlStyles.ResizeRedraw, true);
            base.SetStyle(ControlStyles.UserPaint, true);
            base.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            base.BackColor = Color.Transparent;

        }

        public PanelEx(IContainer container)
        {
            container.Add(this);
        }

        #region 字段
        // Fields
        private Color _backColor = Color.Transparent;
        private Color _borderColor = Color.Transparent;

        private RectangleRadius _rectangleRadius = new RectangleRadius(0);
        private bool _glassEffect = false;
        private Image _backgroundImage = null;
        private Color _tmpColor;
        private Color _startColor;//渐变的起始色
        private Color _endColor;//渐变的尾色
        private float _angle = 90;//渐变角度
        private bool _enableLinearGradientSet = false;//是否自己设渐变色，否则根据背景色自动处理，是则按照自定义的渐变色来显示
        private float _basePosition = 0.5f;//0-1.0之间
        private float _opacity = 1.0f;//0-1.0之间
        #endregion

        #region 属性
        /// <summary>
        ///设置不透明度
        /// </summary>
        public float LinearGradientOpacity
        {
            get
            {
                return this._opacity;
            }
            set
            {
                if (value <= 1 && value >= 0)
                    this._opacity = value;
                else
                    this._opacity = 1.0f;

                base.Invalidate();
            }

        }
        public float LinearGlassPosition
        {
            get
            {
                return this._basePosition;
            }
            set
            {
                if (value <= 1 && value >= 0)
                    this._basePosition = value;
                else
                    this._basePosition = 1.0f;
                base.Invalidate();
            }

        }
        /// <summary>
        /// 是否自己设渐变色
        /// </summary>
        public bool LinearGradientEnable
        {
            get
            {
                return this._enableLinearGradientSet;
            }
            set
            {
                this._enableLinearGradientSet = value;
                base.Invalidate();
            }
        }
        /// <summary>
        /// 设渐变色角度，0为从左到右
        /// </summary>
        public float LinearGradientBrushAngle
        {
            get
            {
                return this._angle;
            }
            set
            {
                this._angle = value;
                base.Invalidate();
            }
        }
        /// <summary>
        /// 渐变的尾色
        /// </summary>
        public Color LinearGradientBrushEndColor
        {
            get
            {
                return this._endColor;
            }
            set
            {
                this._endColor = value;
                base.Invalidate();
            }
        }

        /// <summary>
        /// 渐变的起始色
        /// </summary>
        public Color LinearGradientBrushStartColor
        {
            get
            {
                return this._startColor;
            }
            set
            {
                this._startColor = value;
                base.Invalidate();
            }
        }


        #region 设置圆角属性
        public RectangleRadius Radius
        {
            get
            {
                return _rectangleRadius;
            }
            set
            {
                if (value != this._rectangleRadius)
                {
                    this._rectangleRadius = value;
                    base.Invalidate();
                }
            }
        }
        public int Radius_All
        {
            get
            {
                return _rectangleRadius.All;
            }
            set
            {
                if (value != this._rectangleRadius.All)
                {
                    this._rectangleRadius.All = value;
                    base.Invalidate();
                }
            }
        }
        public int Radius_TopLeft
        {
            get
            {
                return _rectangleRadius.TopLeft;
            }
            set
            {
                if (value != this._rectangleRadius.TopLeft)
                {
                    this._rectangleRadius.TopLeft = value;
                    base.Invalidate();
                }
            }
        }
        public int Radius_TopRight
        {
            get
            {
                return _rectangleRadius.TopRight;
            }
            set
            {
                if (value != this._rectangleRadius.TopRight)
                {
                    this._rectangleRadius.TopRight = value;
                    base.Invalidate();
                }
            }
        }
        public int Radius_BottomRight
        {
            get
            {
                return _rectangleRadius.BottomRight;
            }
            set
            {
                if (value != this._rectangleRadius.BottomRight)
                {
                    this._rectangleRadius.BottomRight = value;
                    base.Invalidate();
                }
            }
        }
        public int Radius_BottomLeft
        {
            get
            {
                return _rectangleRadius.BottomLeft;
            }
            set
            {
                if (value != this._rectangleRadius.BottomLeft)
                {
                    this._rectangleRadius.BottomLeft = value;
                    base.Invalidate();
                }
            }
        }
        public new Image BackgroundImage
        {
            get
            {
                return this._backgroundImage;
            }
            set
            {
                this._backgroundImage = value;
                base.Invalidate();
            }
        }

        #endregion

        [DefaultValue(typeof(Color), "51, 161, 224")]
        public new Color BackColor
        {
            get
            {
                return this._backColor;
            }
            set
            {
                this._backColor = value;
                base.Invalidate();
            }
        }

        /// <summary>
        /// 玻璃效果
        /// </summary>
        public bool GlassEffect
        {
            get { return _glassEffect; }
            set
            {
                if (this._glassEffect != value)
                {
                    this._glassEffect = value;
                    base.Invalidate();
                }
            }
        }
        /// <summary>
        /// 边框颜色
        /// </summary>
        public Color BorderColor
        {
            get { return _borderColor; }
            set
            {
                if (this._borderColor != value)
                {
                    this._borderColor = value;
                    _tmpColor = _borderColor;
                    base.Invalidate();
                }
            }
        }

        #endregion

        protected override void OnPaint(PaintEventArgs e)
        {
            this.SuspendLayout();
            Color controlDark = new Color();
            base.OnPaint(e);
            //base.OnPaintBackground(e);
            Graphics g = e.Graphics;
            //双缓冲
            //Image img = new Bitmap(this.ClientRectangle.Width, this.ClientRectangle.Height);
            //using (Graphics g = Graphics.FromImage(img))
            {
                Rectangle rect = base.ClientRectangle;
                if (rect.Width <= 0)
                    rect.Width = 1;
                if (rect.Height <= 0)
                    rect.Height = 1;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                GraphicsPath gpath = GraphicsPathHelper.CreatePathWithRadius(rect, _rectangleRadius, true);
                if (_borderColor != _tmpColor)
                    _borderColor = _tmpColor;
                if (this.BackgroundImage != null)
                {
                    using (Brush textureBrush = new TextureBrush(this._backgroundImage))
                    {
                        g.FillPath(textureBrush, gpath);
                    }
                }
                if (!base.Enabled)
                {
                    controlDark = _backColor;
                    _borderColor = SystemColors.ControlDark;
                }
                else
                {
                    controlDark = _backColor;
                }
                LinearGradientBrush brush;
                if (_startColor == null || _endColor == null || !_enableLinearGradientSet || !base.Enabled)
                {
                    brush = null;
                }
                else
                    brush = new LinearGradientBrush(rect, Color.FromArgb((byte)(_opacity * 255), _startColor.R, _startColor.G, _startColor.B), Color.FromArgb((byte)(_opacity * 255), _endColor.R, _endColor.G, _endColor.B), _angle);
                RenderHelper.RenderBackground(g, rect, controlDark, _borderColor, this._rectangleRadius, _basePosition, brush, _angle, true, this._glassEffect);

                //写入gx
                //gx.DrawImage(img, this.ClientRectangle);
                this.ResumeLayout();
            }

        }
        protected override void OnResize(EventArgs eventargs)
        {
            base.OnResize(eventargs);
            base.Refresh();
            //this.ResumeLayout();

        }

    }
}
