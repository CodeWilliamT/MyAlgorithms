using System;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Security.Permissions;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace LeadframeAOI.UI.ControlWrap
{
        /// <summary>
    /// 控件的状态
    /// </summary>
    public enum ControlState:int
    {
        /// <summary>
        /// 正常
        /// </summary>
        Normal,
        /// <summary>
        /// 鼠标悬浮
        /// </summary>
        Hover,
        /// <summary>
        /// 鼠标按下
        /// </summary>
        Pressed,
        /// <summary>
        /// 获得焦点
        /// </summary>
        Focused
    }

    /// <summary>
    /// 矩形的圆角
    /// </summary>
    public struct RectangleRadius
    {
        private bool _all;
        private int _topleft;
        private int _topright;
        private int _bottomright;
        private int _bottomleft;
        /// <summary>
        /// 空值，全部设为0
        /// </summary>
        public static readonly RectangleRadius Empty;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="all">所有圆角半径</param>
        public RectangleRadius(int all)
        {
            this._all = true;
            this._topleft = this._topright = this._bottomright = this._bottomleft = all;
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="topleft">topleft角半径</param>
        /// <param name="topright">topright角半径</param>
        /// <param name="bottomright">bottomright角半径</param>
        /// <param name="bottomleft">bottomleft角半径</param>
        public RectangleRadius(int topleft, int topright, int bottomright, int bottomleft)
        {
            this._topleft = topleft;
            this._topright = topright;
            this._bottomright = bottomright;
            this._bottomleft = bottomleft;
            this._all = ((this._topleft == this._topright) && (this._topleft == this._bottomright)) && (this._topleft == this._bottomleft);
        }
        /// <summary>
        /// 所有圆角
        /// </summary>
        public int All
        {
            get
            {
                if (!this._all)
                {
                    return -1;
                }
                return this._topleft;
            }
            set
            {
                if (!this._all || (this._topleft != value))
                {
                    this._all = true;
                    this._topleft = this._topright = this._bottomright = this._bottomleft = value;
                }
            }
        }
        /// <summary>
        /// TopRight圆角值
        /// </summary>
        public int TopRight
        {
            get
            {
                if (this._all)
                {
                    return this._topleft;
                }
                return this._topright;
            }
            set
            {
                if (this._all || (this._topright != value))
                {
                    this._all = false;
                    this._topright = value;
                }
            }
        }
        /// <summary>
        /// TopLeft圆角值
        /// </summary>
        public int TopLeft
        {
            get
            {
                return this._topleft;
            }
            set
            {
                if (this._all || (this._topleft != value))
                {
                    this._all = false;
                    this._topleft = value;
                }
            }
        }
        /// <summary>
        /// BottomLeft圆角值
        /// </summary>
        public int BottomLeft
        {
            get
            {
                if (this._all)
                {
                    return this._topleft;
                }
                return this._bottomleft;
            }
            set
            {
                if (this._all || (this._bottomleft != value))
                {
                    this._all = false;
                    this._bottomleft = value;
                }
            }
        }
        /// <summary>
        /// BottomRight圆角值
        /// </summary>
        public int BottomRight
        {
            get
            {
                if (this._all)
                {
                    return this._topleft;
                }
                return this._bottomright;
            }
            set
            {
                if (this._all || (this._bottomright != value))
                {
                    this._all = false;
                    this._bottomright = value;
                }
            }
        }

        private void ResetAll()
        {
            this.All = 0;
        }

        private void ResetTopRight()
        {
            this.TopRight = 0;
        }

        private void ResetTopLeft()
        {
            this.TopLeft = 0;
        }

        private void ResetBottomLeft()
        {
            this.BottomLeft = 0;
        }

        private void ResetBottomRight()
        {
            this.BottomRight = 0;
        }

        internal void Scale(float dx, float dy)
        {
            this._topleft = (int)(this._topleft * dy);
            this._topright = (int)(this._topright * dx);
            this._bottomright = (int)(this._bottomright * dx);
            this._bottomleft = (int)(this._bottomleft * dy);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public override bool Equals(object other)
        {
            return ((other is RectangleRadius) && (((RectangleRadius)other) == this));
        }
        public override int GetHashCode()
        {
            return ((this._topleft.GetHashCode() ^ this._topright.GetHashCode()) ^ this._bottomleft.GetHashCode() ^ this._bottomright.GetHashCode());
        }
        /// <summary>
        /// 是否相等
        /// </summary>
        /// <param name="r1"></param>
        /// <param name="r2"></param>
        /// <returns></returns>
        public static bool operator ==(RectangleRadius r1, RectangleRadius r2)
        {
            return ((((r1.TopLeft == r2.TopLeft) && (r1.TopRight == r2.TopRight)) && (r1.BottomRight == r2.BottomRight)) && (r1.BottomLeft == r2.BottomLeft));
        }
        /// <summary>
        /// 判断不等
        /// </summary>
        /// <param name="r1"></param>
        /// <param name="r2"></param>
        /// <returns></returns>
        public static bool operator !=(RectangleRadius r1, RectangleRadius r2)
        {
            return !(r1 == r2);
        }

        internal bool ShouldSerializeAll()
        {
            return this._all;
        }
        static RectangleRadius()
        {
            Empty = new RectangleRadius(0);
        }

    }
    /// <summary>
    /// 提供渲染
    /// </summary>
    public class RenderHelper
    {
        /// <summary>
        /// 根据偏移量设置颜色
        /// </summary>
        /// <param name="colorBase">基准色</param>
        /// <param name="a">alpha偏移量</param>
        /// <param name="r">R偏移量</param>
        /// <param name="g">G偏移量</param>
        /// <param name="b">B偏移量</param>
        /// <returns>返回偏移后的颜色</returns>
        public static Color GetColor(Color colorBase, int a, int r, int g, int b)
        {
            int a0 = colorBase.A;
            int r0 = colorBase.R;
            int g0 = colorBase.G;
            int b0 = colorBase.B;
           
            if ((a + a0) > 0xff)
            {
                a = 0xff;
            }
            else
            {
                a = Math.Max(0, a + a0);
            }

            if (r0 == 0)
            {
                r = 10;
            }
            else if ((r + r0) > 0xff)
            {
                r = 0xff;
            }
            else
            {
                r = Math.Max(0, r + r0);
            }
            if (g0 == 0)
            {
                g = 10;
            }
            else if ((g + g0) > 0xff)
            {
                g = 0xff;
            }
            else
            {
                g = Math.Max(0, g + g0);
            }
            if (b0 == 0)
            {
                b = 10;
            }
            else if ((b + b0) > 0xff)
            {
                b = 0xff;
            }
            else
            {
                b = Math.Max(0, b + b0);
            }
            return Color.FromArgb(a, r, g, b);
        }
        /// <summary>
        /// 绘制背景
        /// </summary>
        /// <param name="g"></param>
        /// <param name="rect"></param>
        /// <param name="baseColor"></param>
        /// <param name="borderColor"></param>
        /// <param name="radius"></param>
        /// <param name="basePosition"></param>
        /// <param name="drawBorder"></param>
        /// <param name="drawGlass"></param>
        public static void RenderBackground(Graphics g, Rectangle rect, Color baseColor, Color borderColor, RectangleRadius radius, float basePosition, bool drawBorder, bool drawGlass)
        { 
            #region 
            //using (LinearGradientBrush brush = new LinearGradientBrush(rect, Color.Transparent, Color.Transparent, mode))
            //{
            //    Rectangle rectangle;
            //    SolidBrush brushAlpha;
            //    RectangleF glassRect;
            //    Pen pen;
            //    Color[] colorArray = new Color[] { GetColor(baseColor, 0, 0x23, 0x18, 9), GetColor(baseColor, 0, 13, 8, 3), baseColor, GetColor(baseColor, 0, 0x44, 0x45, 0x36) };
            //    ColorBlend blend = new ColorBlend();
            //    float[] numArray = new float[4];
            //    numArray[1] = basePosition;
            //    numArray[2] = basePosition + 0.05f;
            //    numArray[3] = 1f;
            //    blend.Positions = numArray;
            //    blend.Colors = colorArray;
            //    brush.InterpolationColors = blend;
            //    GraphicsPath path;
            //    if (drawBorder)
            //    {
            //        rect.Width--;
            //        rect.Height--;
            //    }
            //    #region 填充边界
            //    using (path = GraphicsPathHelper.CreatePath(rect, radius, false))
            //    {
            //        g.FillPath(brush, path);
            //    }
            //    #endregion
            //    #region 绘制边线
            //    if (drawBorder)
            //    {
            //        //绘制外边框
            //        using (path = GraphicsPathHelper.CreatePath(rect, radius, false))
            //        {
            //            using (pen = new Pen(borderColor, 1))
            //            {
            //                //绘制路径：
            //                g.DrawPath(pen, path);
            //            }
            //        }
            //        //绘制内边框
            //        //rect.Inflate(-1, -1);
            //        //using (path = GraphicsPathHelper.CreatePath(rect, radius, false))
            //        //{
            //        //    using (pen = new Pen(innerBorderColor))
            //        //    {
            //        //        g.DrawPath(pen, path);
            //        //    }
            //        //}
            //    }
            //    #endregion

            //    #region 绘制玻璃效果

            //    if (drawGlass)
            //    {
            //        #region 绘制玻璃效果的上半部分，basePosition为距离顶端的比例

            //        if (baseColor.A > 80)
            //        {
            //            rectangle = rect;
            //            if (mode == LinearGradientMode.Vertical)
            //            {
            //                rectangle.Height = (int)(rectangle.Height * basePosition);
            //            }
            //            else
            //            {
            //                rectangle.Width = (int)(rect.Width * basePosition);
            //            }
            //            using (GraphicsPath path2 = GraphicsPathHelper.CreatePath(rectangle, new RectangleRadius(radius.TopLeft, radius.TopRight, 4, 4), false))
            //            {
            //                using (brushAlpha = new SolidBrush(Color.FromArgb(80, 0xff, 0xff, 0xff)))
            //                {
            //                    g.FillPath(brushAlpha, path2);
            //                }
            //            }
            //        }
            //        #endregion


            //        #region 绘制玻璃效果的下半部分，basePosition为距离顶端的比例
            //        glassRect = rect;
            //        if (mode == LinearGradientMode.Vertical)
            //        {
            //            glassRect.Y = rect.Y + (rect.Height * basePosition);
            //            glassRect.Height = (rect.Height - (rect.Height * basePosition)) * 2f;
            //        }
            //        else
            //        {
            //            glassRect.X = rect.X + (rect.Width * basePosition);
            //            glassRect.Width = (rect.Width - (rect.Width * basePosition)) * 2f;
            //        }
            //        DrawGlass(g, glassRect, 110, 0);
            //        #endregion

            //    }
            //    #endregion
            //}
            #endregion
            RenderBackground(g, rect, baseColor, borderColor, radius, basePosition, null, 90, drawBorder, drawGlass);

        }
        ///// <summary>
        ///// 利用路径绘制背景，尚不可用
        ///// </summary>
        ///// <param name="g"></param>
        ///// <param name="path"></param>
        ///// <param name="rect"></param>
        ///// <param name="baseColor"></param>
        ///// <param name="borderColor"></param>
        ///// <param name="innerBorderColor"></param>
        ///// <param name="basePosition"></param>
        ///// <param name="drawBorder"></param>
        ///// <param name="drawGlass"></param>
        ///// <param name="mode"></param>
        //public static void RenderBackground(Graphics g, GraphicsPath path,Rectangle rect, Color baseColor, Color borderColor, Color innerBorderColor, float basePosition, bool drawBorder, bool drawGlass, LinearGradientMode mode)
        //{
        //    using (LinearGradientBrush brush = new LinearGradientBrush(rect, Color.Transparent, Color.Transparent, mode))
        //    {
        //        Rectangle rectangle;
        //        SolidBrush brushAlpha;
        //        RectangleF glassRect;
        //        Pen pen;
        //        Color[] colorArray = new Color[] { GetColor(baseColor, 0, 0x23, 0x18, 9), GetColor(baseColor, 0, 13, 8, 3), baseColor, GetColor(baseColor, 0, 0x44, 0x45, 0x36) };
        //        ColorBlend blend = new ColorBlend();
        //        float[] numArray = new float[4];
        //        numArray[1] = basePosition;
        //        numArray[2] = basePosition + 0.05f;
        //        numArray[3] = 1f;
        //        blend.Positions = numArray;
        //        blend.Colors = colorArray;
        //        brush.InterpolationColors = blend;
        //        if (drawBorder)
        //        {
        //            rect.Width--;
        //            rect.Height--;
        //        }
        //        #region 填充边界
        //        g.FillPath(brush, path);
        //        #endregion
        //        #region 绘制边线
        //        if (drawBorder)
        //        {
        //            //绘制外边框
        //            using (pen = new Pen(borderColor, 1))
        //            {
        //                //绘制路径：
        //                g.DrawPath(pen, path);
        //            }
        //            //绘制内边框
        //            //rect.Inflate(-1, -1);
        //            //{
        //            //    using (pen = new Pen(innerBorderColor))
        //            //    {
        //            //        g.DrawPath(pen, path);
        //            //    }
        //            //}
        //        }
        //        #endregion

        //        #region 绘制玻璃效果

        //        if (drawGlass)
        //        {
        //            #region 绘制玻璃效果的上半部分，basePosition为距离顶端的比例

        //            if (baseColor.A > 80)
        //            {
        //                rectangle = rect;
        //                if (mode == LinearGradientMode.Vertical)
        //                {
        //                    rectangle.Height = (int)(rectangle.Height * basePosition);
        //                }
        //                else
        //                {
        //                    rectangle.Width = (int)(rect.Width * basePosition);
        //                }
        //                using (GraphicsPath path2 = GraphicsPathHelper.CreatePath(rectangle, new RectangleRadius(radius.TopLeft, radius.TopRight, 4, 4), false))
        //                {
        //                    using (brushAlpha = new SolidBrush(Color.FromArgb(80, 0xff, 0xff, 0xff)))
        //                    {
        //                        g.FillPath(brushAlpha, path2);
        //                    }
        //                }
        //            }
        //            #endregion


        //            #region 绘制玻璃效果的下半部分，basePosition为距离顶端的比例
        //            glassRect = rect;
        //            if (mode == LinearGradientMode.Vertical)
        //            {
        //                glassRect.Y = rect.Y + (rect.Height * basePosition);
        //                glassRect.Height = (rect.Height - (rect.Height * basePosition)) * 2f;
        //            }
        //            else
        //            {
        //                glassRect.X = rect.X + (rect.Width * basePosition);
        //                glassRect.Width = (rect.Width - (rect.Width * basePosition)) * 2f;
        //            }
        //            DrawGlass(g, glassRect, 110, 0);
        //            #endregion

        //        }
        //        #endregion
        //    }
        //}

       /// <summary>
        /// 按照LinearGradientBrush绘制带圆角的矩形
       /// </summary>
       /// <param name="g"></param>
       /// <param name="rect"></param>
       /// <param name="baseColor"></param>
       /// <param name="borderColor"></param>
       /// <param name="radius"></param>
       /// <param name="basePosition"></param>
       /// <param name="brush"></param>
       /// <param name="angle"></param>
       /// <param name="drawBorder"></param>
       /// <param name="drawGlass"></param>
        public static void RenderBackground(Graphics g, Rectangle rect, Color baseColor, Color borderColor, RectangleRadius radius, float basePosition,LinearGradientBrush brush, float angle, bool drawBorder, bool drawGlass)
        {
            GraphicsPath path;
            if (brush == null)
            {
                brush = new LinearGradientBrush(rect, Color.Transparent, Color.Transparent, angle);
                {
                    Color[] colorArray = new Color[] { GetColor(baseColor, 0, 0x23, 0x18, 9), GetColor(baseColor, 0, 13, 8, 3), baseColor, GetColor(baseColor, 0, 0x44, 0x45, 0x36) };
                    ColorBlend blend = new ColorBlend();
                    float[] numArray = new float[4];
                    numArray[1] = basePosition;
                    numArray[2] = basePosition + 0.05f;
                    numArray[3] = 1f;
                    blend.Positions = numArray;
                    blend.Colors = colorArray;
                    brush.InterpolationColors = blend;
                }
            }
           
            #region 填充边界
            using (path = GraphicsPathHelper.CreatePathWithRadius(rect, radius, true))
            {

                g.FillPath(brush, path);
                brush.Dispose();
            }

            #endregion
            Rectangle rectangle;
            SolidBrush brushAlpha;
            RectangleF glassRect;
            Pen pen;
            //if (drawBorder)
            //{
            //    rect.Width --;
            //    rect.Height --;
            //}
                
            #region 绘制边线
            if (drawBorder)
            {
                //绘制外边框
                using (path = GraphicsPathHelper.CreatePathWithRadius(rect, radius, true))
                {
                    using (pen = new Pen(borderColor, 1))
                    {
                        //绘制路径：
                        g.DrawPath(pen, path);
                    }
                }
            }
            #endregion

            #region 绘制玻璃效果

            if (drawGlass)
            {
                #region 绘制玻璃效果的上半部分，basePosition为距离顶端的比例

                if (baseColor.A > 80)
                {
                    rectangle = rect;
                    //if (angle == 90 || angle == -90)
                    {
                        rectangle.Height = (int)(rectangle.Height * basePosition);
                    }
                    //else
                    //{
                    //    rectangle.Width = (int)(rect.Width * basePosition);
                    //}
                    using (GraphicsPath path2 = GraphicsPathHelper.CreatePathWithRadius(rectangle, new RectangleRadius(radius.TopLeft, radius.TopRight, 4, 4), false))
                    {
                        using (brushAlpha = new SolidBrush(Color.FromArgb(80, 0xff, 0xff, 0xff)))
                        {
                            g.FillPath(brushAlpha, path2);
                        }
                    }
                }
                #endregion


                #region 绘制玻璃效果的下半部分，basePosition为距离顶端的比例
                glassRect = rect;
                //if (angle == 90 || angle == -90)
                {
                    glassRect.Y = rect.Y + (rect.Height * basePosition);
                    glassRect.Height = (rect.Height - (rect.Height * basePosition)) * 2f;
                }
                //else
                //{
                //glassRect.X = rect.X + (rect.Width * basePosition);
                //glassRect.Width = (rect.Width - (rect.Width * basePosition)) * 2f;
                //}
                DrawGlass(g, glassRect, 110, 0);
                #endregion

            }
            #endregion
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="g"></param>
        /// <param name="rect"></param>
        /// <param name="baseColor"></param>
        /// <param name="borderColor"></param>
        /// <param name="radius"></param>
        /// <param name="basePosition"></param>
        /// <param name="angle"></param>
        /// <param name="drawBorder"></param>
        /// <param name="drawGlass"></param>
        public static void RenderBackground(Graphics g, Rectangle rect, Color baseColor, Color borderColor, RectangleRadius radius, float basePosition, float angle, bool drawBorder, bool drawGlass)
        {
            RenderBackground(g, rect, baseColor, borderColor, radius, basePosition, null, angle, drawBorder, drawGlass);
        }
        /// <summary>
        /// 绘制玻璃效果，白色
        /// </summary>
        /// <param name="g"></param>
        /// <param name="glassRect"></param>
        /// <param name="alphaCenter"></param>
        /// <param name="alphaSurround"></param>
        public static void DrawGlass(Graphics g, RectangleF glassRect, int alphaCenter, int alphaSurround)
        {
            DrawGlass(g, glassRect, Color.White, alphaCenter, alphaSurround);
        }
        /// <summary>
        /// 绘制玻璃效果
        /// </summary>
        /// <param name="g"></param>
        /// <param name="glassRect"></param>
        /// <param name="glassColor"></param>
        /// <param name="alphaCenter"></param>
        /// <param name="alphaSurround"></param>
        public static void DrawGlass(Graphics g, RectangleF glassRect, Color glassColor, int alphaCenter, int alphaSurround)
        {
            using (GraphicsPath path = new GraphicsPath())
            {
                path.AddEllipse(glassRect);
                using (PathGradientBrush brush = new PathGradientBrush(path))
                {
                    brush.CenterColor = Color.FromArgb(alphaCenter, glassColor);
                    brush.SurroundColors = new Color[] { Color.FromArgb(alphaSurround, glassColor) };
                    brush.CenterPoint = new PointF(glassRect.X + (glassRect.Width / 2f), glassRect.Y + (glassRect.Height / 2f));
                    g.FillPath(brush, path);
                }
            }
        }


    }
    /// <summary>
    /// 提供绘制的路径
    /// </summary>
    public static class GraphicsPathHelper
    {
        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="rect">需要绘制圆角的矩形</param>
        /// <param name="radius">圆角</param>
        /// <param name="correction">修正</param>
        /// <returns>返回GraphicsPath</returns>
        public static GraphicsPath CreatePathWithRadius(Rectangle rect, RectangleRadius radius, bool correction)
        {
            if (correction)
            {
                rect.Height--;
                rect.Width--;
            }
            GraphicsPath path = new GraphicsPath();
            if (radius.TopLeft > 0)
            {
                path.AddArc(rect.X, rect.Y, radius.TopLeft * 2, radius.TopLeft * 2, 180f, 90f);
            }
            else
            {
                //画点
                path.AddLine(rect.X, rect.Y, rect.X, rect.Y);
            }
            if (radius.TopRight > 0)
            {
                path.AddArc((rect.Right - radius.TopRight * 2) , rect.Y , radius.TopRight * 2, radius.TopRight * 2, 270f, 90f);
            }
            else
            {
                //画点
                path.AddLine((rect.Right - radius.TopRight * 2), rect.Y, (rect.Right - radius.TopRight * 2), rect.Y);
            }

            if (radius.BottomRight > 0)
            {
                path.AddArc((rect.Right - radius.BottomRight * 2), (rect.Bottom - radius.BottomRight * 2) , radius.BottomRight * 2, radius.BottomRight * 2, 0f, 90f);

            }
            else
            {
                //画点
                path.AddLine((rect.Right - radius.BottomRight * 2) , (rect.Bottom - radius.BottomRight * 2) , (rect.Right - radius.BottomRight * 2) , (rect.Bottom - radius.BottomRight * 2) );

            }
            if (radius.BottomLeft > 0)
            {
                path.AddArc(rect.X, (rect.Bottom - radius.BottomLeft * 2) , radius.BottomLeft * 2, radius.BottomLeft * 2, 90f, 90f);
            }
            else
            {
                //画点
                path.AddLine(rect.X, (rect.Bottom - radius.BottomLeft * 2) , rect.X, (rect.Bottom - radius.BottomLeft * 2) );

            }
            path.CloseFigure();
            return path;
        }

        public static GraphicsPath CreateCustomPath(Rectangle rect, RectangleRadius radius, bool correction)
        {
            GraphicsPath path = new GraphicsPath();


            return path;
        }
    }


}
