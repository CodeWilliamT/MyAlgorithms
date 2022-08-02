using System.ComponentModel;
using System.Drawing;
using System;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace LeadframeAOI.UI.ControlWrap
{
    public sealed class ControlPaintEx
    {
        // Methods
        internal static Rectangle CalculateBackgroundImageRectangle(Rectangle bounds, Image backgroundImage, ImageLayout imageLayout)
        {
            Rectangle rectangle = bounds;
            if (backgroundImage != null)
            {
                switch (imageLayout)
                {
                    case ImageLayout.None:
                        rectangle.Size = backgroundImage.Size;
                        return rectangle;

                    case ImageLayout.Tile:
                        return rectangle;

                    case ImageLayout.Center:
                        {
                            rectangle.Size = backgroundImage.Size;
                            Size size = bounds.Size;
                            if (size.Width > rectangle.Width)
                            {
                                rectangle.X = (size.Width - rectangle.Width) / 2;
                            }
                            if (size.Height > rectangle.Height)
                            {
                                rectangle.Y = (size.Height - rectangle.Height) / 2;
                            }
                            return rectangle;
                        }
                    case ImageLayout.Stretch:
                        rectangle.Size = bounds.Size;
                        return rectangle;

                    case ImageLayout.Zoom:
                        {
                            Size size2 = backgroundImage.Size;
                            float num = ((float)bounds.Width) / ((float)size2.Width);
                            float num2 = ((float)bounds.Height) / ((float)size2.Height);
                            if (num < num2)
                            {
                                rectangle.Width = bounds.Width;
                                rectangle.Height = (int)((size2.Height * num) + 0.5);
                                if (bounds.Y >= 0)
                                {
                                    rectangle.Y = (bounds.Height - rectangle.Height) / 2;
                                }
                                return rectangle;
                            }
                            rectangle.Height = bounds.Height;
                            rectangle.Width = (int)((size2.Width * num2) + 0.5);
                            if (bounds.X >= 0)
                            {
                                rectangle.X = (bounds.Width - rectangle.Width) / 2;
                            }
                            return rectangle;
                        }
                }
            }
            return rectangle;
        }

        public static void DrawBackgroundImage(Graphics g, Image backgroundImage, Color backColor, ImageLayout backgroundImageLayout, Rectangle bounds, Rectangle clipRect)
        {
            DrawBackgroundImage(g, backgroundImage, backColor, backgroundImageLayout, bounds, clipRect, Point.Empty, RightToLeft.No);
        }

        public static void DrawBackgroundImage(Graphics g, Image backgroundImage, Color backColor, ImageLayout backgroundImageLayout, Rectangle bounds, Rectangle clipRect, Point scrollOffset)
        {
            DrawBackgroundImage(g, backgroundImage, backColor, backgroundImageLayout, bounds, clipRect, scrollOffset, RightToLeft.No);
        }

        public static void DrawBackgroundImage(Graphics g, Image backgroundImage, Color backColor, ImageLayout backgroundImageLayout, Rectangle bounds, Rectangle clipRect, Point scrollOffset, RightToLeft rightToLeft)
        {
            if (g == null)
            {
                throw new ArgumentNullException("g");
            }
            if (backgroundImageLayout == ImageLayout.Tile)
            {
                using (TextureBrush brush = new TextureBrush(backgroundImage, WrapMode.Tile))
                {
                    if (scrollOffset != Point.Empty)
                    {
                        Matrix transform = brush.Transform;
                        transform.Translate((float)scrollOffset.X, (float)scrollOffset.Y);
                        brush.Transform = transform;
                    }
                    g.FillRectangle(brush, clipRect);
                    return;
                }
            }
            Rectangle rect = CalculateBackgroundImageRectangle(bounds, backgroundImage, backgroundImageLayout);
            if ((rightToLeft == RightToLeft.Yes) && (backgroundImageLayout == ImageLayout.None))
            {
                rect.X += clipRect.Width - rect.Width;
            }
            using (SolidBrush brush2 = new SolidBrush(backColor))
            {
                g.FillRectangle(brush2, clipRect);
            }
            if (!clipRect.Contains(rect))
            {
                if ((backgroundImageLayout == ImageLayout.Stretch) || (backgroundImageLayout == ImageLayout.Zoom))
                {
                    rect.Intersect(clipRect);
                    g.DrawImage(backgroundImage, rect);
                }
                else if (backgroundImageLayout == ImageLayout.None)
                {
                    rect.Offset(clipRect.Location);
                    Rectangle destRect = rect;
                    destRect.Intersect(clipRect);
                    Rectangle rectangle3 = new Rectangle(Point.Empty, destRect.Size);
                    g.DrawImage(backgroundImage, destRect, rectangle3.X, rectangle3.Y, rectangle3.Width, rectangle3.Height, GraphicsUnit.Pixel);
                }
                else
                {
                    Rectangle rectangle4 = rect;
                    rectangle4.Intersect(clipRect);
                    Rectangle rectangle5 = new Rectangle(new Point(rectangle4.X - rect.X, rectangle4.Y - rect.Y), rectangle4.Size);
                    g.DrawImage(backgroundImage, rectangle4, rectangle5.X, rectangle5.Y, rectangle5.Width, rectangle5.Height, GraphicsUnit.Pixel);
                }
            }
            else
            {
                ImageAttributes imageAttr = new ImageAttributes();
                imageAttr.SetWrapMode(WrapMode.TileFlipXY);
                g.DrawImage(backgroundImage, rect, 0, 0, backgroundImage.Width, backgroundImage.Height, GraphicsUnit.Pixel, imageAttr);
                imageAttr.Dispose();
            }
        }

        public static void DrawCheckedFlag(Graphics graphics, Rectangle rect, Color color)
        {
            PointF[] points = new PointF[] { new PointF(rect.X + (((float)rect.Width) / 4.5f), rect.Y + (((float)rect.Height) / 2.5f)), new PointF(rect.X + (((float)rect.Width) / 2.5f), rect.Bottom - (((float)rect.Height) / 3f)), new PointF(rect.Right - (((float)rect.Width) / 4f), rect.Y + (((float)rect.Height) / 4.5f)) };
            using (Pen pen = new Pen(color, 2f))
            {
                graphics.DrawLines(pen, points);
            }
        }

    }

    internal class NativeMethods
    {
        // Fields
        public static readonly IntPtr FALSE = IntPtr.Zero;
        public static readonly IntPtr TRUE = new IntPtr(1);
        public const int WM_PAINT = 15;

        // Methods
        [DllImport("user32.dll")]
        public static extern IntPtr BeginPaint(IntPtr hWnd, ref PAINTSTRUCT ps);
        [DllImport("user32.dll")]
        public static extern bool EndPaint(IntPtr hWnd, ref PAINTSTRUCT ps);
        [DllImport("user32.dll")]
        public static extern bool GetComboBoxInfo(IntPtr hwndCombo, ref ComboBoxInfo info);
        [DllImport("user32.dll")]
        public static extern int GetWindowRect(IntPtr hwnd, ref RECT lpRect);

        // Nested Types
        public enum ComboBoxButtonState
        {
            STATE_SYSTEM_INVISIBLE = 0x8000,
            STATE_SYSTEM_NONE = 0,
            STATE_SYSTEM_PRESSED = 8
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct ComboBoxInfo
        {
            public int cbSize;
            public NativeMethods.RECT rcItem;
            public NativeMethods.RECT rcButton;
            public NativeMethods.ComboBoxButtonState stateButton;
            public IntPtr hwndCombo;
            public IntPtr hwndEdit;
            public IntPtr hwndList;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct PAINTSTRUCT
        {
            public IntPtr hdc;
            public int fErase;
            public NativeMethods.RECT rcPaint;
            public int fRestore;
            public int fIncUpdate;
            public int Reserved1;
            public int Reserved2;
            public int Reserved3;
            public int Reserved4;
            public int Reserved5;
            public int Reserved6;
            public int Reserved7;
            public int Reserved8;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
            public RECT(int left, int top, int right, int bottom)
            {
                this.Left = left;
                this.Top = top;
                this.Right = right;
                this.Bottom = bottom;
            }

            public RECT(Rectangle rect)
            {
                this.Left = rect.Left;
                this.Top = rect.Top;
                this.Right = rect.Right;
                this.Bottom = rect.Bottom;
            }

            public Rectangle Rect
            {
                get
                {
                    return new Rectangle(this.Left, this.Top, this.Right - this.Left, this.Bottom - this.Top);
                }
            }
            public Size Size
            {
                get
                {
                    return new Size(this.Right - this.Left, this.Bottom - this.Top);
                }
            }
            public static NativeMethods.RECT FromXYWH(int x, int y, int width, int height)
            {
                return new NativeMethods.RECT(x, y, x + width, y + height);
            }

            public static NativeMethods.RECT FromRectangle(Rectangle rect)
            {
                return new NativeMethods.RECT(rect.Left, rect.Top, rect.Right, rect.Bottom);
            }
        }
    }


}
