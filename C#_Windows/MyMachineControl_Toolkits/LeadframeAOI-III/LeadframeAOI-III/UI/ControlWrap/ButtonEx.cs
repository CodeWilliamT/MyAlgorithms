using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace LeadframeAOI.UI.ControlWrap
{
        /// <summary>
    /// 自定义的Button
    /// </summary>
   // [ToolboxBitmap(typeof(ComboBox))]
  //  [ToolboxItem(true)]
    public partial class ButtonEx : Button
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ButtonEx()
        {
            //base.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.SupportsTransparentBackColor | ControlStyles.ResizeRedraw | ControlStyles.UserPaint, true);
            base.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            base.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            base.SetStyle(ControlStyles.ResizeRedraw, true);
            base.SetStyle(ControlStyles.UserPaint, true);
            base.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            
            base.BackColor = Color.Transparent;
            //base.Cursor = Cursors.Hand;
        }

        #region 字段
        private int ImageWidth = 1;
        // Fields
        private Color _backColor = Color.FromArgb(0x33, 0xa1, 0xe0);
        private Color _borderColor = Color.FromArgb(0x33, 0xa1, 0xe0);
        private Color _tmpColor ;
        private ControlState _controlState;
        private RectangleRadius _rectangleRadius = new RectangleRadius(5);
        private bool _glassEffect = true;
        private Image _backgroundImage = null;

        #endregion

        #region 属性
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

        /// <summary>
        /// 设置背景色
        /// </summary>
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

        internal ControlState ControlState
        {
            get
            {
                return this._controlState;
            }
            set
            {
                if (this._controlState != value)
                {
                    this._controlState = value;
                    base.Invalidate();
                }
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


        private void CalculateRect(out Rectangle imageRect, out Rectangle textRect)
        {
            imageRect = Rectangle.Empty;
            textRect = Rectangle.Empty;
            if (base.Image == null)
            {
                textRect = new Rectangle(2, 0, base.Width - 4, base.Height);
            }
            else
            {
                int width = base.Image.Width;
                int height;
                if (base.Height < base.Image.Width)
                    width = base.Height;
                height = (int)((double)base.Image.Width * width / base.Image.Height);
                Size size = new Size(width, height);
                switch (base.TextImageRelation)
                {
                    case TextImageRelation.Overlay:
                        imageRect = new Rectangle(new Point(2, (int)((base.Height - height) / 2.0d)), size);
                        textRect = new Rectangle(2, 0, base.Width - 4, base.Height);
                        break;

                    case TextImageRelation.ImageAboveText:
                        imageRect = new Rectangle(new Point((base.Width - this.ImageWidth) / 2, 2), size);
                        textRect = new Rectangle(2, imageRect.Bottom, base.Width, (base.Height - imageRect.Bottom) - 2);
                        break;

                    case TextImageRelation.TextAboveImage:
                        imageRect = new Rectangle(new Point((base.Width - this.ImageWidth) / 2, (int)((base.Height - height) / 2.0d)), size);
                        textRect = new Rectangle(0, 2, base.Width, (base.Height - imageRect.Y) - 2);
                        break;

                    case TextImageRelation.ImageBeforeText:
                        imageRect = new Rectangle(new Point(2, (int)((base.Height - height) / 2.0d)), size);
                        textRect = new Rectangle(imageRect.Right + 2, 0, (base.Width - imageRect.Right) - 4, base.Height);
                        break;

                    case TextImageRelation.TextBeforeImage:
                        imageRect = new Rectangle(new Point((base.Width - this.ImageWidth) - 2, (int)((base.Height - height) / 2.0d)), size);
                        textRect = new Rectangle(2, 0, imageRect.X - 2, base.Height);
                        break;
                }
                if (this.RightToLeft == RightToLeft.Yes)
                {
                    imageRect.X = base.Width - imageRect.Right;
                    textRect.X = base.Width - textRect.Right;
                }
            }
        }

        internal static TextFormatFlags GetTextFormatFlags(ContentAlignment alignment, bool rightToleft)
        {
            TextFormatFlags flags = TextFormatFlags.SingleLine | TextFormatFlags.WordBreak;
            if (rightToleft)
            {
                flags |= TextFormatFlags.RightToLeft | TextFormatFlags.Right;
            }
            ContentAlignment alignment2 = alignment;
            if (alignment2 <= ContentAlignment.MiddleCenter)
            {
                switch (alignment2)
                {
                    case ContentAlignment.TopLeft:
                        return flags;

                    case ContentAlignment.TopCenter:
                        return (flags | TextFormatFlags.HorizontalCenter);

                    case (ContentAlignment.TopCenter | ContentAlignment.TopLeft):
                        return flags;

                    case ContentAlignment.TopRight:
                        return (flags | TextFormatFlags.Right);

                    case ContentAlignment.MiddleLeft:
                        return (flags | TextFormatFlags.VerticalCenter);

                    case ContentAlignment.MiddleCenter:
                        return (flags | (TextFormatFlags.VerticalCenter | TextFormatFlags.HorizontalCenter));
                }
                return flags;
            }
            if (alignment2 <= ContentAlignment.BottomLeft)
            {
                switch (alignment2)
                {
                    case ContentAlignment.MiddleRight:
                        return (flags | (TextFormatFlags.VerticalCenter | TextFormatFlags.Right));

                    case ContentAlignment.BottomLeft:
                        return (flags | TextFormatFlags.Bottom);
                }
                return flags;
            }
            if (alignment2 != ContentAlignment.BottomCenter)
            {
                if (alignment2 != ContentAlignment.BottomRight)
                {
                    return flags;
                }
                return (flags | (TextFormatFlags.Bottom | TextFormatFlags.Right));
            }
            return (flags | (TextFormatFlags.Bottom | TextFormatFlags.HorizontalCenter));
        }

        #region 鼠标事件处理

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button == MouseButtons.Left)
            {
                this.ControlState = ControlState.Pressed;
            }
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            this.ControlState = ControlState.Hover;
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            this.ControlState = ControlState.Normal;
        }
        /// <summary>
        ///    
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if ((e.Button == MouseButtons.Left) && (e.Clicks == 1))
            {
                if (base.ClientRectangle.Contains(e.Location))
                {
                    this.ControlState = ControlState.Hover;
                }
                else
                {
                    this.ControlState = ControlState.Normal;
                }
            }
        }

        #endregion
        /// <summary>
        /// 重写Paint事件执行代码
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            Color controlDark = new Color(); ;
            base.OnPaint(e);
            base.OnPaintBackground(e);
            Rectangle imageRect;
            Rectangle textRect;
            CalculateRect(out imageRect, out textRect);

            Graphics g = e.Graphics;
            //Image img=new Bitmap(this.ClientRectangle.Width,this.ClientRectangle.Height);
            //using (Graphics g = Graphics.FromImage(img))
            {
                //this.CalculateRect(out imageRect, out textRect);
                g.SmoothingMode = SmoothingMode.AntiAlias;
                //Color innerBorderColor = Color.FromArgb(200, 0xff, 0xff, 0xff);

                GraphicsPath gpath = GraphicsPathHelper.CreatePathWithRadius(base.ClientRectangle, _rectangleRadius, true);
                if (this.BackgroundImage != null)
                {
                    using (Brush textureBrush =new TextureBrush(this._backgroundImage))
                    {
                        g.FillPath(textureBrush, gpath);
                    }
                }
                if (base.Enabled)
                {
                    if (_borderColor != _tmpColor)
                    _borderColor = _tmpColor;

                    switch (this.ControlState)
                    {
                        case ControlState.Hover:
                            if (_backColor.R == 0 && _backColor.G == 0 && _backColor.B == 0)
                            { controlDark = Color.FromArgb(255, 10, 10, 10); }
                            else
                            controlDark = RenderHelper.GetColor(this._backColor, 0, -13, -18, -3);
                            break;
                        case ControlState.Pressed:
                            if (_backColor.R == 0 && _backColor.G == 0 && _backColor.B == 0)
                            { controlDark = Color.FromArgb(255, 35,25,35); }
                            else
                            controlDark = RenderHelper.GetColor(this._backColor, 0, -85, -60, -35);
                            break;
                        default:
                            {
                                controlDark = this._backColor;
                            }
                            break;
                    }
                    //_borderColor = this._tmpColor;
                }
                else
                {
                    //controlDark = SystemColors.ControlDark;
                    //_borderColor = SystemColors.ControlDark;
                    controlDark = Color.FromArgb(128,142,168);
                    _borderColor = Color.FromArgb(128, 142, 168);
                }
                if (this.BackgroundImage != null )
                {
                    using (Pen px = new Pen(controlDark, this.ControlState==ControlState.Pressed ? 2.5f : 1.5f))
                    {
                        g.DrawPath(px, gpath);
                        goto Label_01;
                    }
                }

                RenderHelper.RenderBackground(g, base.ClientRectangle, controlDark, _borderColor,  this._rectangleRadius, 0.5f, true, this._glassEffect);
                if (base.Image != null)
                {
                    g.InterpolationMode = InterpolationMode.HighQualityBilinear;

                    int width=base.Image.Width;
                    int height;
                    if (base.Height < base.Image.Width)
                        width = base.Height;
                    height = (int)((double)base.Image.Width * width / base.Image.Height);
                    Size size = new Size(width, height);
                    Point loaction = new Point();
                    loaction.X = 10;
                    loaction.Y =(int)((base.Height-height)/2.0d);

                    ;
                    g.DrawImage(base.Image, imageRect, 0, 0, base.Image.Width, base.Image.Height, GraphicsUnit.Pixel);
                }

            Label_01:
                TextRenderer.DrawText(g, this.Text, this.Font, textRect, this.ForeColor, GetTextFormatFlags(this.TextAlign, this.RightToLeft == RightToLeft.Yes));
                //gx.DrawImage(img,this.ClientRectangle);
                //g.Dispose();
            
            }
        }
    }
}
