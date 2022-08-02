using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ToolKits.DieShow
{
    public partial class DieShow : UserControl
    {
        #region 字段
        /// <summary>
        /// 切割的行数
        /// </summary>
        private int _cutRow = 0;
        /// <summary>
        /// 切割的列数
        /// </summary>
        private int _cutCol = 0;
        /// <summary>
        /// 列向间距
        /// </summary>
        private int _xSpace = 0;
        /// <summary>
        /// 纵向间距
        /// </summary>
        private int _ySpace = 0;
        /// <summary>
        /// 结点大小
        /// </summary>
        private int _nodeSize = 0;
        /// <summary>
        /// Hover状态
        /// </summary>
        private bool isEffectHover = false;
        /// <summary>
        /// Hover矩形区域
        /// </summary>
        private Rectangle hoverRectangle;
        /// <summary>
        /// Hover颜色
        /// </summary>
        private Color hoverColor = Color.BlueViolet;

        /// <summary>
        /// active状态
        /// </summary>
        private bool mActiveGrid = false;
        /// <summary>
        /// active单元格
        /// </summary>
        private int mActiveRow = 0;
        /// <summary>
        /// active单元格
        /// </summary>
        private int mActiveCol = 0;
        /// <summary>
        /// active的区域
        /// </summary>
        private Rectangle mActiveRectangle;

        private int _mode = 0;
        /// <summary>
        /// （0-0）所在的方向（0：左上角；1：右上角；2：右下角；3：左下角）
        /// </summary>
        //private int _dir = 0;
        #endregion

        #region 事件
        public delegate void DieShowClickEventHandler(object sender, DieShowClickEventArgs e);
        public event DieShowClickEventHandler LeftClickEvent;
        public delegate void DieShowDoubleClickEventHandler(object sender, DieShowDoubleClickEventArgs e);
        public event DieShowDoubleClickEventHandler DoubleClickEvent;

        /// <summary>
        /// 坐标改变
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        private void CoordChangeByRowCol(int row, int col)
        {
            mActiveGrid = false;
            if (mActiveRectangle != null)
            {
                this.pictureBox1.Invalidate(mActiveRectangle);
            }

            this.mActiveRow = row;
            this.mActiveCol = col;

            mActiveGrid = true;
            Rectangle rect = new Rectangle();
            GetRectangleByRowCol(row, col, ref rect);
            mActiveRectangle = new Rectangle(rect.X, rect.Y, rect.Width, rect.Height);
            this.pictureBox1.Invalidate(mActiveRectangle);

            //int dirRow, dirCol;
            //GetDirRowCol(this.mActiveRow, this.mActiveCol, out dirRow, out dirCol);

            if (LeftClickEvent != null)
                LeftClickEvent(this, new DieShowClickEventArgs(this.mActiveRow, this.mActiveCol));
        }
        #endregion

        #region 构造器
        public DieShow()
        {
            InitializeComponent();

            // Set the value of the double-buffering style bits to true.
            this.SetStyle(ControlStyles.AllPaintingInWmPaint |
              ControlStyles.UserPaint | ControlStyles.ResizeRedraw |
              ControlStyles.UserPaint | ControlStyles.DoubleBuffer, true);

            InitEventByDieShow();
        }
        #endregion

        #region 接口方法
        /// <summary>
        /// 重置
        /// </summary>
        public void Reset()
        {
            Graphics dc = pictureBox1.CreateGraphics();
            dc.Clear(Color.White);

            _cutRow = 0;
            _cutCol = 0;
            _xSpace = 0;
            _ySpace = 0;
            isEffectHover = false;
            mActiveGrid = false;

        }

        /// <summary>
        /// 绘制芯片示意图
        /// </summary>
        /// <param name="row">行数</param>
        /// <param name="col">列数</param>
        /// <param name="mode">0表示画Die边框，1表示画完整Die</param>
        /// <param name="mode">（0-0）所在的方向（0：左上角；1：右上角；2：右下角；3：左下角）</param>
        public void Draw(int row, int col, int mode = 0)
        {
            //_dir = dir;
            _mode = mode;
            _cutRow = row;
            _cutCol = col;

            _xSpace = this.pictureBox1.Width / (_cutCol + 2);
            _ySpace = this.pictureBox1.Height / (_cutRow + 2);

            _nodeSize = (_xSpace / 10 > _ySpace / 10) ? _ySpace / 10 : _xSpace / 10;

            pictureBox1.Invalidate();
        }

        /// <summary>
        /// 切换到下一颗芯片前使用
        /// </summary>
        public void Clear()
        {
            mActiveGrid = false;
            if (mActiveRectangle != null)
            {
                this.pictureBox1.Invalidate(mActiveRectangle);
            }

            this.mActiveRow = -1;
            this.mActiveCol = -1;

            if (LeftClickEvent != null)
                LeftClickEvent(this, new DieShowClickEventArgs(this.mActiveRow, this.mActiveCol));
        }
        #endregion

        #region PictureBox事件
        private void InitEventByDieShow()
        {
            this.pictureBox1.Paint += new PaintEventHandler(pictureBox1_Paint);
            this.pictureBox1.MouseMove += new MouseEventHandler(pictureBox1_MouseMove);
            this.pictureBox1.MouseClick += new MouseEventHandler(pictureBox1_MouseClick);
            this.pictureBox1.MouseLeave += new EventHandler(pictureBox1_MouseLeave);
            this.pictureBox1.MouseDoubleClick += new MouseEventHandler(pictureBox1_MouseDoubleClick);
            this.pictureBox1.Resize += new EventHandler(pictureBox1_Resize);
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Pen p = new Pen(Color.Black, 1);

            if (_cutRow > 0 && _cutCol > 0)
            {
                if (_mode == 0)
                {
                    //绘制横向线
                    g.DrawLine(p, _xSpace, _ySpace, _xSpace * (_cutCol + 1), _ySpace);
                    g.DrawLine(p, _xSpace, _ySpace * (_cutRow + 1), _xSpace * (_cutCol + 1), _ySpace * (_cutRow + 1));

                    //绘制纵向线
                    g.DrawLine(p, _xSpace, _ySpace, _xSpace, _ySpace * (_cutRow + 1));
                    g.DrawLine(p, _xSpace * (_cutCol + 1), _ySpace, _xSpace * (_cutCol + 1), _ySpace * (_cutRow + 1));
                }
                else if (_mode == 1)
                {
                    //绘制横向线段
                    for (int i = 0; i < _cutRow + 1; i++)
                    {
                        int startX = _xSpace;
                        int startY = _ySpace * i + _ySpace;
                        int endX = _xSpace * _cutCol + _xSpace;
                        int endY = startY;
                        g.DrawLine(p, startX, startY, endX, endY);
                    }
                    //绘制纵向线段
                    for (int i = 0; i < _cutCol + 1; i++)
                    {
                        int startX = _xSpace * i + _xSpace;
                        int startY = _ySpace;
                        int endX = startX;
                        int endY = _ySpace * _cutRow + _ySpace;
                        g.DrawLine(p, startX, startY, endX, endY);
                    }
                }

                //画结点和矩形，并标注编号 
                int mIndex = 0, nIndex = 0;
                Rectangle rect = new Rectangle();
                p = new Pen(Color.LightGreen, 2);     //定义了一个黑色,宽度为2的画笔
                p.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
                for (int i = 0; i < _cutRow + 1; i++)
                {
                    for (int j = 0; j < _cutCol + 1; j++)
                    {
                        if (IsValidateRowCol(i, j) == false)
                            continue;

                        string strDisplay = "";
                        if (i >= _cutRow)
                        {
                            //rowIndex = _curRow + 1;
                            mIndex = 0;
                        }
                        else
                        {
                            //rowIndex = _curRow;
                            mIndex = i;
                        }
                        if (j >= _cutCol)
                        {
                            //colIndex = _curCol + 1;
                            nIndex = 0;
                        }
                        else
                        {
                            //colIndex = _curCol;
                            nIndex = j;
                        }

                        //strDisplay = rowIndex.ToString() + "-" + colIndex.ToString() + "-" + mIndex.ToString() + "-" + nIndex.ToString();
                        strDisplay = mIndex.ToString() + "-" + nIndex.ToString();

                        //int dirRow, dirCol;
                        //GetDirRowCol(i, j, out dirRow, out dirCol);

                        DrawNode(g, i, j, strDisplay);

                        GetRectangleByRowCol(i, j, ref rect);
                        //rect.Inflate(-_xSpace / 5, -_ySpace / 5);
                        g.DrawRectangle(p, rect);
                    }
                }

                //鼠标悬浮时高亮显示
                if (this.isEffectHover && hoverRectangle != null)
                {
                    Rectangle rec = hoverRectangle;
                    rec.Inflate(-3, -3);
                    g.DrawRectangle(new Pen(hoverColor, 2f), rec);
                }

                //选中的单元格重新绘制
                if (this.mActiveGrid && this.mActiveRectangle != null)
                //if (this.mActiveRectangle != null)
                {
                    Rectangle rec = this.mActiveRectangle;
                    rec.Inflate(-5, -5);

                    using (SolidBrush brush = new SolidBrush(Color.FromArgb(80, Color.Maroon)))
                    {
                        g.FillRectangle(brush, rec);
                    }
                }
            }
        }

        private void pictureBox1_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (_cutRow > 0 && _cutCol > 0)
            {
                this.isEffectHover = false;
                if (hoverRectangle != null)
                    this.pictureBox1.Invalidate(hoverRectangle);

                int row = -1, col = -1;
                if (GetRowColByXY(e.X, e.Y, ref row, ref col) == false)
                    return;

                if (IsValidateRowCol(row, col) == false)
                    return;

                isEffectHover = true;
                GetRectangleByRowCol(row, col, ref hoverRectangle);
                hoverRectangle.Inflate(3, 3);
                this.pictureBox1.Invalidate(hoverRectangle);
            }
        }

        private void pictureBox1_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                if (_cutRow > 0 && _cutCol > 0)
                {
                    int row = -1, col = -1;
                    if (GetRowColByXY(e.X, e.Y, ref row, ref col) == false)
                        return;

                    if (IsValidateRowCol(row, col) == false)
                        return;

                    isEffectHover = true;
                    GetRectangleByRowCol(row, col, ref hoverRectangle);
                    hoverRectangle.Inflate(3, 3);
                    this.pictureBox1.Invalidate(hoverRectangle);

                    //坐标改变
                    CoordChangeByRowCol(row, col);
                }
            }
        }

        private void pictureBox1_MouseLeave(object sender, EventArgs e)
        {
            if (_cutRow > 0 && _cutCol > 0)
            {
                this.isEffectHover = false;
                if (hoverRectangle != null)
                {
                    this.pictureBox1.Invalidate(hoverRectangle);
                }
            }
        }

        private void pictureBox1_MouseDoubleClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            //if (e.Button == System.Windows.Forms.MouseButtons.Middle)
            //    Clear_Click(null, e);

            if (_cutRow > 0 && _cutCol > 0)
            {
                mActiveGrid = false;
                if (mActiveRectangle != null)
                {
                    this.pictureBox1.Invalidate(mActiveRectangle);
                }

                this.mActiveRow = -1;
                this.mActiveCol = -1;

                if (DoubleClickEvent != null)
                    DoubleClickEvent(this, new DieShowDoubleClickEventArgs());
            }
        }

        private void pictureBox1_Resize(object sender, EventArgs e)
        {
            if (_cutRow > 0 && _cutCol > 0)
            {
                _xSpace = this.pictureBox1.Width / (_cutCol + 2);
                _ySpace = this.pictureBox1.Height / (_cutRow + 2);

                _nodeSize = (_xSpace / 10 > _ySpace / 10) ? _ySpace / 10 : _xSpace / 10;

                pictureBox1.Refresh();
            }
        }
        #endregion

        #region 方法
        private bool IsValidateRowCol(int row, int col)
        {
            if (_mode == 0)
            {
                if (row == 0 || row == _cutRow ||
                    col == 0 || col == _cutCol)
                    return true;
                else
                    return false;
            }
            else if (_mode == 1)
            {
                if (row >= 0 && row <= _cutRow &&
                    col >= 0 && col <= _cutCol)
                    return true;
                else
                    return false;
            }

            return true;
        }

        private bool GetRowColByXY(int x, int y, ref int row, ref int col)
        {
            if (x < _xSpace / 2 || x > (this.pictureBox1.Width - _xSpace / 2) ||
            y < _ySpace / 2 || y > (this.pictureBox1.Height - _ySpace / 2))
                return false;

            col = (x - _xSpace / 2) / _xSpace;
            row = (y - _ySpace / 2) / _ySpace;

            if (row < 0)
                row = 0;
            else if (row > _cutRow)
                row = _cutRow;

            if (col < 0)
                col = 0;
            else if (col > _cutCol)
                col = _cutCol;

            return true;
        }

        /// <summary>
        /// 根据行列获得单元格所在的矩形
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="rect"></param>
        /// <returns></returns>
        private void GetRectangleByRowCol(int row, int col, ref Rectangle rect)
        {
            rect = new Rectangle(this._xSpace / 2 + col * this._xSpace,
                this._ySpace / 2 + row * this._ySpace,
                this._xSpace,
                this._ySpace);

            //return true;
        }

        ///<summary>
        ///画结点，并标注编号 
        ///</summary>
        private void DrawNode(Graphics g, int row, int col, string nodeDisplay)
        {
            _nodeSize = (_nodeSize > 5) ? 5 : _nodeSize;

            using (SolidBrush brush = new SolidBrush(Color.Black))
            {
                int left = col * _xSpace + _xSpace - _nodeSize;
                int top = row * _ySpace + _ySpace - _nodeSize;

                //绘制节点
                g.FillEllipse(brush, left, top, _nodeSize * 2, _nodeSize * 2);

                //显示文字
                Font font = new Font("Consolas", 10, FontStyle.Regular);
                brush.Color = Color.Red;
                //SizeF txtsize = g.MeasureString(nodeDisplay);
                //设置文字位置
                //PointF point = new PointF();
                left = left + _nodeSize - 18;
                top = top + _nodeSize - 10;
                //g.DrawString(nodeDisplay, font, brush, point);
                g.DrawString(nodeDisplay, font, brush, left, top);
            }
        }

        private void GetDirRowCol(int row, int col, out int dirRow, out int dirCol)
        {
            dirRow = -1;
            dirCol = -1;
            //switch (_dir)
            //{
            //    case 0:
            //        dirRow = row;
            //        dirCol = col;
            //        break;
            //    case 1:
            //        dirRow = row;
            //        dirCol = this._cutCol - col;
            //        break;
            //    case 2:
            //        dirRow = this._cutRow - row;
            //        dirCol = this._cutCol - col;
            //        break;
            //    case 3:
            //        dirRow = this._cutRow - row;
            //        dirCol = col;
            //        break;
            //}
        }

        #endregion
    }

    /// <summary>
    /// 单击事件
    /// </summary>
    public class DieShowClickEventArgs : EventArgs
    {
        public int Row;
        public int Col;

        public DieShowClickEventArgs(int row, int col)
        {
            Row = row;
            Col = col;
        }
    }

    /// <summary>
    /// 双击事件
    /// </summary>
    public class DieShowDoubleClickEventArgs : EventArgs
    {
        public DieShowDoubleClickEventArgs()
        {

        }
    }
}
