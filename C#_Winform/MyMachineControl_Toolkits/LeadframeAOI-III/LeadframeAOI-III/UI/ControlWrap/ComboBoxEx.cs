using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Drawing.Drawing2D;

namespace LeadframeAOI.UI.ControlWrap
{
    /// <summary>
    /// 自定义ComboBox
    /// </summary>
    [ToolboxBitmap(typeof(ComboBox))]
    [ToolboxItem(true)]
    public class ComboBoxEx : ComboBox
    {
        // Fields
        private Color _arrowColor = Color.FromArgb(0x13, 0x58, 0x80);
        private Color _baseColor = Color.FromArgb(0x33, 0xa1, 0xe0);
        private Color _borderColor = Color.FromArgb(0x33, 0xa1, 0xe0);
        private bool _bPainting;
        private ControlState _buttonState;
        private IntPtr _editHandle;

        /// <summary>
        /// 构造器
        /// </summary>
        public ComboBoxEx()
        {
            base.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }

        private bool GetComboBoxButtonPressed()
        {
            return (this.GetComboBoxInfo().stateButton == NativeMethods.ComboBoxButtonState.STATE_SYSTEM_PRESSED);
        }

        private NativeMethods.ComboBoxInfo GetComboBoxInfo()
        {
            NativeMethods.ComboBoxInfo cbi;
            cbi = new NativeMethods.ComboBoxInfo();
            cbi.cbSize = Marshal.SizeOf(cbi);
            NativeMethods.GetComboBoxInfo(base.Handle, ref cbi);
            return cbi;

        }

        private Rectangle GetDropDownButtonRect()
        {
            return this.GetComboBoxInfo().rcButton.Rect;
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            NativeMethods.ComboBoxInfo cbi = this.GetComboBoxInfo();
            this._editHandle = cbi.hwndEdit;
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            Point point = base.PointToClient(Cursor.Position);
            if (this.ButtonRect.Contains(point))
            {
                this.ButtonState = ControlState.Hover;
            }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            this.ButtonState = ControlState.Normal;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            Point point = e.Location;
            if (this.ButtonRect.Contains(point))
            {
                this.ButtonState = ControlState.Hover;
            }
            else
            {
                this.ButtonState = ControlState.Normal;
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            this.ButtonState = ControlState.Normal;
        }

        internal void RenderArrowInternal(Graphics g, Rectangle dropDownRect, ArrowDirection direction, Brush brush)
        {
            Point[] csfloat;
            Point point = new Point(dropDownRect.Left + (dropDownRect.Width / 2), dropDownRect.Top + (dropDownRect.Height / 2));
            Point[] points = null;
            switch (direction)
            {
                case ArrowDirection.Left:
                    csfloat = new Point[] { new Point(point.X + 2, point.Y - 3), new Point(point.X + 2, point.Y + 3), new Point(point.X - 1, point.Y) };
                    points = csfloat;
                    break;

                case ArrowDirection.Up:
                    csfloat = new Point[] { new Point(point.X - 3, point.Y + 2), new Point(point.X + 3, point.Y + 2), new Point(point.X, point.Y - 2) };
                    points = csfloat;
                    break;

                case ArrowDirection.Right:
                    csfloat = new Point[] { new Point(point.X - 2, point.Y - 3), new Point(point.X - 2, point.Y + 3), new Point(point.X + 1, point.Y) };
                    points = csfloat;
                    break;

                default:
                    csfloat = new Point[] { new Point(point.X - 2, point.Y - 1), new Point(point.X + 3, point.Y - 1), new Point(point.X, point.Y + 2) };
                    points = csfloat;
                    break;
            }
            g.FillPolygon(brush, points);
        }

        private void RenderComboBox(ref Message m)
        {
            Rectangle rect = new Rectangle(Point.Empty, base.Size);
            Rectangle buttonRect = this.ButtonRect;
            ControlState state = this.ButtonPressed ? ControlState.Pressed : this.ButtonState;
            using (Graphics g = Graphics.FromHwnd(m.HWnd))
            {
                this.RenderComboBoxBackground(g, rect, buttonRect);
                this.RenderConboBoxDropDownButton(g, this.ButtonRect, state);
                this.RenderConboBoxBorder(g, rect);
            }
        }

        private void RenderComboBoxBackground(Graphics g, Rectangle rect, Rectangle buttonRect)
        {
            Color backColor = base.Enabled ? base.BackColor : SystemColors.Control;
            using (SolidBrush brush = new SolidBrush(backColor))
            {
                buttonRect.Inflate(-1, -1);
                rect.Inflate(-1, -1);
                using (Region region = new Region(rect))
                {
                    region.Exclude(buttonRect);
                    region.Exclude(this.EditRect);
                    g.FillRegion(brush, region);
                }
            }
        }

        private void RenderConboBoxBorder(Graphics g, Rectangle rect)
        {
            Color borderColor = base.Enabled ? this._borderColor : Color.FromArgb(128, 142, 168);
            using (Pen pen = new Pen(borderColor))
            {
                rect.Width--;
                rect.Height--;
                g.DrawRectangle(pen, rect);
            }
        }

        private void RenderConboBoxDropDownButton(Graphics g, Rectangle buttonRect, ControlState state)
        {
            Color baseColor;
            //Color backColor = Color.FromArgb(160, 250, 250, 250);
            Color borderColor = base.Enabled ? this._borderColor : Color.FromArgb(128, 142, 168);
            Color arrowColor = base.Enabled ? this._arrowColor : Color.White;
            Rectangle rect = buttonRect;
            if (base.Enabled)
            {
                switch (state)
                {
                    case ControlState.Hover:
                        baseColor = RenderHelper.GetColor(this._baseColor, 0, -33, -22, -13);
                        goto Label_00AE;

                    case ControlState.Pressed:
                        baseColor = RenderHelper.GetColor(this._baseColor, 0, -65, -47, -25);
                        goto Label_00AE;
                }
                baseColor = this._baseColor;
            }
            else
            {
                baseColor = Color.FromArgb(128, 142, 168);
            }
        Label_00AE:
            rect.Inflate(-1, -1);
            this.RenderScrollBarArrowInternal(g, rect, baseColor, borderColor, arrowColor, new RectangleRadius(1), true, true, ArrowDirection.Down);
        }

        internal void RenderScrollBarArrowInternal(Graphics g, Rectangle rect, Color baseColor, Color borderColor, Color arrowColor, RectangleRadius radius, bool drawBorder, bool drawGlass, ArrowDirection arrowDirection)
        {
            RenderHelper.RenderBackground(g, rect, baseColor, borderColor, radius, 0.45f, drawBorder, drawGlass);
            using (SolidBrush brush = new SolidBrush(arrowColor))
            {
                this.RenderArrowInternal(g, rect, arrowDirection, brush);
            }
        }

        private void WmPaint(ref Message m)
        {
            if (base.DropDownStyle == ComboBoxStyle.Simple)
            {
                base.WndProc(ref m);
            }
            else if (base.DropDownStyle == ComboBoxStyle.DropDown)
            {
                if (!this._bPainting)
                {
                    NativeMethods.PAINTSTRUCT ps = new NativeMethods.PAINTSTRUCT();
                    this._bPainting = true;
                    NativeMethods.BeginPaint(m.HWnd, ref ps);
                    this.RenderComboBox(ref m);
                    NativeMethods.EndPaint(m.HWnd, ref ps);
                    this._bPainting = false;
                    m.Result = NativeMethods.TRUE;
                }
                else
                {
                    base.WndProc(ref m);
                }
            }
            else
            {
                base.WndProc(ref m);
                this.RenderComboBox(ref m);
            }
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 15)
            {
                this.WmPaint(ref m);
            }
            else
            {
                base.WndProc(ref m);
            }
        }

        // Properties
        [DefaultValue(typeof(Color), "19, 88, 128")]
        public Color ArrowColor
        {
            get
            {
                return this._arrowColor;
            }
            set
            {
                if (this._arrowColor != value)
                {
                    this._arrowColor = value;
                    base.Invalidate();
                }
            }
        }

        [DefaultValue(typeof(Color), "51, 161, 224")]
        public Color BaseColor
        {
            get
            {
                return this._baseColor;
            }
            set
            {
                if (this._baseColor != value)
                {
                    this._baseColor = value;
                    base.Invalidate();
                }
            }
        }

        [DefaultValue(typeof(Color), "51, 161, 224")]
        public Color BorderColor
        {
            get
            {
                return this._borderColor;
            }
            set
            {
                if (this._borderColor != value)
                {
                    this._borderColor = value;
                    base.Invalidate();
                }
            }
        }

        internal bool ButtonPressed
        {
            get
            {
                return (base.IsHandleCreated && this.GetComboBoxButtonPressed());
            }
        }

        internal Rectangle ButtonRect
        {
            get
            {
                return this.GetDropDownButtonRect();
            }
        }

        internal ControlState ButtonState
        {
            get
            {
                return this._buttonState;
            }
            set
            {
                if (this._buttonState != value)
                {
                    this._buttonState = value;
                    base.Invalidate(this.ButtonRect);
                }
            }
        }

        internal IntPtr EditHandle
        {
            get
            {
                return this._editHandle;
            }
        }

        internal Rectangle EditRect
        {
            get
            {
                if (base.DropDownStyle == ComboBoxStyle.DropDownList)
                {
                    Rectangle rect = new Rectangle(3, 3, (base.Width - this.ButtonRect.Width) - 6, base.Height - 6);
                    if (this.RightToLeft == RightToLeft.Yes)
                    {
                        rect.X += this.ButtonRect.Right;
                    }
                    return rect;
                }
                if (base.IsHandleCreated && (this.EditHandle != IntPtr.Zero))
                {
                    NativeMethods.RECT rcClient = new NativeMethods.RECT();
                    NativeMethods.GetWindowRect(this.EditHandle, ref rcClient);
                    return base.RectangleToClient(rcClient.Rect);
                }
                return Rectangle.Empty;
            }
        }
    }
}
