using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Threading.Tasks;

namespace ToolKits.Map
{
    public partial class MapCanvas : UserControl
    {
        #region 字段
        #region 视窗大小
        /// <summary>
        /// 视窗宽度
        /// </summary>
        //private int viewRectWidth;
        /// <summary>
        /// 视窗高度
        /// </summary>
        //private int viewRectHeight;
        /// <summary>
        /// 使用小视窗
        /// </summary>
        private bool useAuxCvs = false;
        #endregion

        #region 画布设置
        /// <summary>
        /// 画布缩放比例
        /// </summary>
        //private float cvsZoom = 1.0f;
        /// <summary>
        /// 插补模式
        /// </summary>
        //private InterpolationMode interMode = InterpolationMode.HighQualityBilinear;
        #endregion

        #region map信息及绘制参数
        /// <summary>
        /// Map信息
        /// </summary>
        private MapInfo mapInfo = null;
        /// <summary>
        /// 主视图最左边单元格的位置
        /// </summary>
        private int mLeft = 0;
        /// <summary>
        /// 主视图最上边单元格的位置
        /// </summary>
        private int mTop = 0;
        /// <summary>
        /// 主视图单元格宽度
        /// </summary>
        private float mWidth = 0;
        /// <summary>
        /// 主视图单元格高度
        /// </summary>
        private float mHeight = 0;
        /// <summary>
        /// 中间间隔
        /// </summary>
        private float mBetween = 1;

        private int mActRow = -1;
        private int mActCol = -1;

        private int aLeft = 0;
        private int aTop = 0;
        private int aWidth = 0;
        private int aHeight = 0;
        private int aRow = 9;
        private int aCol = 9;
        private int aStrRow = 0;
        private int aStrCol = 0;
        private int aActRow = 0;
        private int aActCol = 0;
        #endregion

        #region 鼠标移动与点击相应
        /// <summary>
        /// 主视图active
        /// </summary>
        private bool mActiveGrid = false;
        /// <summary>
        /// 主视图active单元格
        /// </summary>
        private DieGrid mActiveDieGrid = new DieGrid(0, 0);
        /// <summary>
        /// 主视图active的区域
        /// </summary>
        private RectangleF mActiveRectangle;

        private bool aActiveGrid = false;
        private DieGrid aActiveDieGrid = new DieGrid(0, 0);
        private Rectangle aActiveRectangle;

        /// <summary>
        /// 鼠标是否悬浮在非空Die的单元格上
        /// </summary>
        //private bool isEffectHover = false;
        /// <summary>
        /// 悬浮颜色
        /// </summary>
        private Color hoverColor = Color.BlueViolet;
        /// <summary>
        /// 悬浮区域
        /// </summary>
        //private RectangleF hoverRectangle;
        #endregion

        #region 信息缓存
        private int lastBinIndex = 0;
        #endregion

        #region map操作信息
        MapRotateModeEnum rotateMode = MapRotateModeEnum.ROT_NONE;
        MapMirrorModeEnum mirMode = MapMirrorModeEnum.MIR_NONE;
        object[] angles = { 0, 90, 180, 270 };
        object[] mirs = { "不镜像", "水平镜像", "垂直镜像" };
        #endregion
        #endregion

        #region 属性
        /// <summary>
        /// 只激活单元格，不触发响应事件
        /// </summary>
        public DieGrid ActiveDieGrid
        {
            get { return mActiveDieGrid; }
            set
            {
                mActiveDieGrid = value;
                if (mapInfo != null)
                {
                    if (ActiveDieGrid.Row < 0 || ActiveDieGrid.Row >= mapInfo.mapRow ||
                        ActiveDieGrid.Col < 0 || ActiveDieGrid.Col >= mapInfo.mapCol)
                    {
                    }
                    else
                    {
                        CoordChangeByRowCol(ActiveDieGrid.Row, ActiveDieGrid.Col, false);
                    }
                }
            }
        }
        #endregion

        #region 事件
        public delegate void CoordChangeEventHandler(object sender, CoordEventArgs e);
        public event CoordChangeEventHandler CoordChangeEvent;
        public event EventHandler<LoadMapEventArgs> MapLoadEvent;
        public event EventHandler<MapInfo> MapPathEvent;
        public event EventHandler<MapInfo> MapSaveEvent;
        public event EventHandler<DieInfoEventArgs> DieInfoEvent;
        public event EventHandler BinSetHandler;

        /// <summary>
        /// 坐标改变
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        private void CoordChangeByRowCol(int row, int col, bool isEvent = true, bool isMain = true)
        {
            mActiveGrid = false;
            if (mActiveRectangle != null)
            {
                this.MainCvs.Invalidate(new Region(mActiveRectangle));
                //this.MainCvs.Refresh();
            }

            this.mActRow = row;
            this.mActCol = col;
            if (this.useAuxCvs == true)
            {
                this.aActiveGrid = false;
                if (this.aActiveRectangle != null && this.AuxCvs.Visible == true)
                {
                    this.AuxCvs.Invalidate(this.aActiveRectangle);
                }

                this.AuxCvs.Invalidate();
                if (this.AuxCvs.Visible == false)
                {
                    this.aActRow = (this.aRow >> 1) - (((this.aRow & 1) == 1) ? 0 : 1);
                    this.aActCol = (this.aCol >> 1) - (((this.aCol & 1) == 1) ? 0 : 1);
                    this.AuxCvs.Visible = true;
                }

                if (isMain == true)
                {
                    this.aActRow = (this.aRow >> 1) - (((this.aRow & 1) == 1) ? 0 : 1);
                    this.aActCol = (this.aCol >> 1) - (((this.aCol & 1) == 1) ? 0 : 1);
                }
                else
                {
                    this.aActRow = row - this.aStrRow;
                    this.aActCol = col - this.aStrCol;
                }

                this.aActiveGrid = true;
                Rectangle arect = new Rectangle();
                GetAuxRectangleByRowCol(this.aActRow, this.aActCol, ref arect);
                aActiveRectangle = new Rectangle(arect.X - 2, arect.Y - 2, arect.Width + 3, arect.Height + 3);
                this.AuxCvs.Invalidate(new Region(aActiveRectangle));
            }

            mActiveGrid = true;
            RectangleF rect = new RectangleF();
            GetMainRectangleByRowCol(row, col, ref rect);
            mActiveRectangle = new RectangleF(rect.X - 2, rect.Y - 2, rect.Width + 3, rect.Height + 3);
            this.MainCvs.Invalidate(new Region(mActiveRectangle));

            //int row1, col1;
            //if (!mapInfo.mapTransInfo.GetAftRowCol(row, col, out row1, out col1))
            //    return;
            txtRow.Text = row.ToString();
            txtCol.Text = col.ToString();
            txtBin.Text = mapInfo.mapArr[row][col].ToString();

            if (!isEvent) return;
            if (CoordChangeEvent != null)
                CoordChangeEvent(this, new CoordEventArgs(row, col, 0, 0, 0));
        }
        #endregion

        #region 构造器
        public MapCanvas()
        {
            InitializeComponent();

            cmbRotateMode.Items.AddRange(this.angles);
            cmbRotateMode.SelectedIndex = 0;
            cmbMirMode.Items.AddRange(this.mirs);
            cmbMirMode.SelectedIndex = 0;

            // Set the value of the double-buffering style bits to true.
            this.SetStyle(ControlStyles.AllPaintingInWmPaint |
              ControlStyles.UserPaint | ControlStyles.ResizeRedraw |
              ControlStyles.UserPaint | ControlStyles.DoubleBuffer, true);

            InitMapCvsEvent();
        }
        #endregion

        #region 窗体事件
        /*
        protected override void OnLoad(EventArgs e)
        {
            setScrollbarState();
            setScrollbarMaxValue();
            base.OnLoad(e);
        }
         * */

        /*
        protected override void OnResize(EventArgs e)
        {
            setScrollbarState();
            setScrollbarMaxValue();
            base.OnResize(e);
        }
         * */

        /// <summary>
        /// 选中Index切换
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void binRing_SelectedIndexChanged(object sender, EventArgs e)
        {
            lastBinIndex = binRing.SelectedIndex;
        }

        /// <summary>
        /// 设置为左侧Bin
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExSetLeftBin_Click(object sender, EventArgs e)
        {
            if (mapInfo != null && mActRow >= 0 && mActRow < mapInfo.mapRow && mActCol >= 0 && mActCol < mapInfo.mapCol)
            {
                KeyValuePair<int, string> selectItem = (KeyValuePair<int, string>)binRing.SelectedItem;

                lastBinIndex = binRing.SelectedIndex;

                int ind = mapInfo.binInfo.Values.ToList().FindIndex(p => p.Bin == selectItem.Value);
                if (ind < 0)
                {
                    MessageBox.Show(String.Format("当前Bin{0}不在图谱映射表内！", selectItem.Value));
                    return;
                }
                short grade = mapInfo.binInfo.Values.ToList()[ind].Grade;
                mapInfo.mapArr[mActRow][mActCol] = grade;

                if (this.DieInfoEvent != null)
                {
                    Color color = Color.Empty;
                    MapParse.GetMapColorByRowCol(mapInfo, mActRow, mActCol, ref color);
                    this.DieInfoEvent(this, new DieInfoEventArgs(mActRow, mActCol, grade, color));
                }

                //刷新到界面上
                ChangeMapSource(mapInfo);
            }

        }
        /// <summary>
        /// 设置Bin等级表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOpenBinInfoFrm_Click(object sender, EventArgs e)
        {
            if (this.BinSetHandler != null)
                this.BinSetHandler(null, null);
        }
        /// <summary>
        /// 设置参考点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSetRefDie_Click(object sender, EventArgs e)
        {
            if (mapInfo != null && mActRow >= 0 && mActRow < mapInfo.mapRow && mActCol >= 0 && mActCol < mapInfo.mapCol)
            {
                if (mapInfo.RefDie.Count < 1)
                    mapInfo.RefDie.Add(new RefDie(-1, -1));
                mapInfo.RefDie[0].Row = mActRow;
                mapInfo.RefDie[0].Col = mActCol;

                if (this.DieInfoEvent != null)
                {
                    Color color = Color.Empty;
                    short grade = MapParse.GetBinGradeByRowCol(mapInfo, mActRow, mActCol);
                    MapParse.GetMapColorByRowCol(mapInfo, mActRow, mActCol, ref color);
                    this.DieInfoEvent(this, new DieInfoEventArgs(mActRow, mActCol, grade, color, true));
                }

                //刷新到界面上
                ChangeMapSource(mapInfo);
            }
        }
        /// <summary>
        /// 载入Map，规定只能载入原始图谱
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLoadMap_Click(object sender, EventArgs e)
        {
            if (this.MapLoadEvent != null)
            {
                LoadMapEventArgs loadMapEventArgs = new LoadMapEventArgs(this.rotateMode, this.mirMode, this.mapInfo);
                this.MapLoadEvent(this, loadMapEventArgs);
            }
        }
        /// <summary>
        /// 保存Map
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMapSave_Click(object sender, EventArgs e)
        {
            if (mapInfo != null)
            {
                if (this.MapSaveEvent != null)
                    this.MapSaveEvent(this, mapInfo);
            }

        }
        private void cbRotateMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cmbRotateMode.SelectedIndex)
            {
                case 0:
                    this.rotateMode = MapRotateModeEnum.ROT_NONE;
                    break;
                case 1:
                    this.rotateMode = MapRotateModeEnum.ROT_90;
                    break;
                case 2:
                    this.rotateMode = MapRotateModeEnum.ROT_180;
                    break;
                case 3:
                    this.rotateMode = MapRotateModeEnum.ROT_270;
                    break;
                default:
                    this.rotateMode = MapRotateModeEnum.ROT_NONE;
                    break;
            }
        }
        private void cmbMirMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cmbMirMode.SelectedIndex)
            {
                case 0:
                    this.mirMode = MapMirrorModeEnum.MIR_NONE;
                    break;
                case 1:
                    this.mirMode = MapMirrorModeEnum.MIR_HOR;
                    break;
                case 2:
                    this.mirMode = MapMirrorModeEnum.MIR_VET;
                    break;
                default:
                    this.mirMode = MapMirrorModeEnum.MIR_NONE;
                    break;
            }
        }
        private void btnRotateMap_Click(object sender, EventArgs e)
        {
            try
            {
                if (String.IsNullOrEmpty(mapInfo.SrcPath)) return;
                if (!MapParse.Rotate(ref mapInfo, this.rotateMode))
                {
                    MessageBox.Show("Mapping旋转失败！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                this.ChangeMapSource(mapInfo);
                if (this.MapPathEvent != null)
                    this.MapPathEvent(this, this.mapInfo);
            }
            catch (Exception err)
            {
                MessageBox.Show("Map旋转失败！\\详细错误信息：" + err.Message);
            }
        }
        private void btnMirror_Click(object sender, EventArgs e)
        {
            try
            {
                if (String.IsNullOrEmpty(mapInfo.SrcPath)) return;
                if (!MapParse.Mirror(ref mapInfo, this.mirMode))
                {
                    MessageBox.Show("Mapping镜像失败！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                this.ChangeMapSource(mapInfo);
                if (this.MapPathEvent != null)
                    this.MapPathEvent(this, this.mapInfo);
            }
            catch (Exception err)
            {
                MessageBox.Show("Map镜像失败！\\详细错误信息：" + err.Message);
            }
        }
        private void pbNotchDir_MouseHover(object sender, EventArgs e)
        {
            if (this.mapInfo == null) return;
            switch (this.mapInfo.waferDir)
            {
                case WaferDirEnum.DIR_NONE:
                    this.tipInfo.Show("Notch向上", this.pbNotchDir);
                    break;
                case WaferDirEnum.DIR_90:
                    this.tipInfo.Show("Notch向右", this.pbNotchDir);
                    break;
                case WaferDirEnum.DIR_180:
                    this.tipInfo.Show("Notch向下", this.pbNotchDir);
                    break;
                case WaferDirEnum.DIR_270:
                    this.tipInfo.Show("Notch向左", this.pbNotchDir);
                    break;
                default:
                    this.tipInfo.Show("", this.pbNotchDir);
                    break;
            }
        }
        #endregion

        #region PictureBox事件
        /// <summary>
        /// 为控件绑定事件
        /// </summary>
        private void InitMapCvsEvent()
        {
            this.MainCvs.Paint += new PaintEventHandler(MainCvs_Paint);
            this.MainCvs.MouseMove += new MouseEventHandler(MainCvs_MouseMove);
            this.MainCvs.MouseClick += new MouseEventHandler(MainCvs_MouseClick);
            this.MainCvs.MouseLeave += new EventHandler(MainCvs_MouseLeave);
            this.MainCvs.MouseDoubleClick += new MouseEventHandler(MainCvs_MouseDoubleClick);
            this.MainCvs.Resize += new EventHandler(MainCvs_Resize);
            this.AuxCvs.Paint += new PaintEventHandler(AuxCvs_Paint);
            this.AuxCvs.MouseClick += new MouseEventHandler(AuxCvs_MouseClick);
        }

        /// <summary>
        /// 绘制全景图
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainCvs_Paint(object sender, PaintEventArgs e)
        {
            if (mapInfo != null)
            {
                Graphics g = e.Graphics;
                Color color = Color.Empty;

                #region 画单元格
                Dictionary<Color, List<RectangleF>> rectFDic = new Dictionary<Color, List<RectangleF>>();
                Task autoRec = Task.Factory.StartNew(() =>
                {
                    try
                    {
                        for (int i = 0; i < mapInfo.mapRow; i++)
                        {
                            for (int j = 0; j < mapInfo.mapCol; j++)
                            {
                                if (MapParse.GetMapColorByRowCol(mapInfo, i, j, ref color) == true)
                                {
                                    RectangleF rect = new RectangleF(this.mLeft + j * this.mWidth + j * this.mBetween,
                                                                     this.mTop + i * this.mHeight + i * this.mBetween,
                                                                     this.mWidth,
                                                                     this.mHeight);
                                    if (rectFDic.ContainsKey(color))
                                    {
                                        rectFDic[color].Add(rect);
                                    }
                                    else
                                    {
                                        rectFDic[color] = new List<RectangleF>();
                                        rectFDic[color].Add(rect);
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception)
                    {

                    }
                });
                autoRec.Wait();

                foreach (KeyValuePair<Color, List<RectangleF>> var in rectFDic)
                {
                    if (var.Value.Count > 0)
                    {
                        using (SolidBrush brush = new SolidBrush(var.Key))
                        {
                            g.FillRectangles(brush, var.Value.ToArray());
                        }
                        var.Value.Clear();
                    }
                }
                rectFDic.Clear();
                #endregion

                #region 画Hover区域
                //if (this.isEffectHover && hoverRectangle != null)
                //{
                //    RectangleF rec = hoverRectangle;
                //    rec.Inflate(-3, -3);
                //    g.DrawRectangle(new Pen(hoverColor, 2.0f), rec.X, rec.Y,rec.Width, rec.Height);
                //}
                #endregion

                #region 画Active区域
                //选中的单元格重新绘制
                if (this.mActiveGrid && this.mActiveRectangle != null)
                //if (this.mActiveRectangle != null)
                {
                    RectangleF rec = this.mActiveRectangle;
                    rec.Inflate(-1, -1);
                    g.DrawRectangle(new Pen(Color.Black, 1.0f), rec.X, rec.Y, rec.Width, rec.Height);
                    //this.mActiveGrid = false;                    
                }
                #endregion
            }

            /*
            if (mapInfo != null)
            {
                for (int i = 0; i < mapInfo.mapRow; i++)
                {
                    for (int j = 0; j < mapInfo.mapCol; j++)
                    {
                        if (MapParse.GetMapColorByRowCol(mapInfo, i, j, ref color) == true)
                        {
                            DrawGrid(g,
                                i,
                                j,
                                drawMode,
                                Color.Empty,
                                color);
                        }
                    }
                }
            }

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
                rec.Inflate(-1, -1);
                g.DrawRectangle(new Pen(Color.Black, 1.0f), rec);

                //this.mActiveGrid = false;
            }
            */
        }

        /// <summary>
        /// 鼠标移动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainCvs_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            //if (mapInfo != null)
            //{
            //    //清除Hover区域
            //    if (this.isEffectHover && hoverRectangle != null)
            //    {
            //        this.isEffectHover = false;
            //        this.MainCvs.Invalidate(new Region(hoverRectangle));
            //    }

            //    //获取行列
            //    int row = -1, col = -1;
            //    if (GetMainRowColByXY(e.X, e.Y, ref row, ref col) == false)
            //        return;
            //    if (row < 0 || row >= mapInfo.mapRow ||
            //        col < 0 || col >= mapInfo.mapCol)
            //        return;

            //    //设为Hover区域
            //    isEffectHover = true;
            //    GetMainRectangleByRowCol(row, col, ref hoverRectangle);
            //    hoverRectangle.Inflate(3, 3);
            //    this.MainCvs.Invalidate(new Region(hoverRectangle));
            //}
        }

        /// <summary>
        /// 鼠标点击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainCvs_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (mapInfo != null && e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                //获取行列
                int row = -1, col = -1;
                if (GetMainRowColByXY(e.X, e.Y, ref row, ref col) == false)
                    return;
                if (row < 0 || row >= mapInfo.mapRow ||
                    col < 0 || col >= mapInfo.mapCol)
                    return;
                //坐标改变
                CoordChangeByRowCol(row, col);
            }
        }

        /// <summary>
        /// 鼠标离开
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainCvs_MouseLeave(object sender, EventArgs e)
        {
            //if (mapInfo != null)
            //{
            //    //清除Hover区域
            //    if (this.isEffectHover && hoverRectangle != null)
            //    {
            //        this.isEffectHover = false;
            //        this.MainCvs.Invalidate(new Region(hoverRectangle));
            //    }
            //}
        }

        /// <summary>
        /// 双击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainCvs_MouseDoubleClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            //if (e.Button == System.Windows.Forms.MouseButtons.Middle)
            //    Clear_Click(null, e);
        }

        /// <summary>
        /// 缩放
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainCvs_Resize(object sender, EventArgs e)
        {
            if (mapInfo != null)
            {
                InitMapSizeByMapInfo(mapInfo);
                MainCvs.Refresh();
            }
        }

        private void AuxCvs_Paint(object sender, PaintEventArgs e)
        {
            if (mapInfo != null && this.useAuxCvs == true && this.mActRow != -1 && this.mActCol != -1)
            {
                //在内存中画图
                Graphics g = e.Graphics;
                Color color = Color.Empty;

                //this.aActRow = (this.aRow >> 1) - (((this.aRow & 1) == 1) ? 0 : 1);
                //this.aActCol = (this.aCol >> 1) - (((this.aCol & 1) == 1) ? 0 : 1);
                this.aStrRow = this.mActRow - this.aActRow;
                this.aStrCol = this.mActCol - this.aActCol;

                #region 画单元格
                Dictionary<Color, List<Rectangle>> rectFDic = new Dictionary<Color, List<Rectangle>>();
                Task autoRec = Task.Factory.StartNew(() =>
                {
                    for (int i = 0; i < this.aRow; i++)
                    {
                        for (int j = 0; j < this.aCol; j++)
                        {
                            if (MapParse.GetMapColorByRowCol(mapInfo, this.aStrRow + i, this.aStrCol + j, ref color) == true)
                            {
                                Rectangle rect = new Rectangle(this.aLeft + j * this.aWidth + j,
                                    this.aTop + i * this.aHeight + i,
                                    this.aWidth,
                                    this.aHeight);

                                if (rectFDic.ContainsKey(color))
                                {
                                    rectFDic[color].Add(rect);
                                }
                                else
                                {
                                    rectFDic[color] = new List<Rectangle>();
                                    rectFDic[color].Add(rect);
                                }
                            }
                        }
                    }

                });
                autoRec.Wait();

                foreach (KeyValuePair<Color, List<Rectangle>> var in rectFDic)
                {
                    if (var.Value.Count > 0)
                    {
                        using (SolidBrush brush = new SolidBrush(var.Key))
                        {
                            g.FillRectangles(brush, var.Value.ToArray());
                        }
                        var.Value.Clear();
                    }
                }
                rectFDic.Clear();

                #endregion

                #region 画Active区域
                //选中的单元格重新绘制
                if (this.aActiveGrid && this.aActiveRectangle != null)
                //if (this.mActiveRectangle != null)
                {
                    Rectangle rec = this.aActiveRectangle;
                    rec.Inflate(-1, -1);
                    g.DrawRectangle(new Pen(Color.Black, 1.0f), rec.X, rec.Y, rec.Width, rec.Height);
                    //this.mActiveGrid = false;
                }
                #endregion

            }
        }

        private void AuxCvs_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
        }

        private void AuxCvs_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (mapInfo != null && e.Button == System.Windows.Forms.MouseButtons.Left && this.aActiveGrid == true)
            {
                //获取行列
                int row = -1, col = -1;
                if (GetAuxRowColByXY(e.X, e.Y, ref row, ref col) == false)
                    return;
                if (row < 0 || row >= mapInfo.mapRow ||
                    col < 0 || col >= mapInfo.mapCol)
                    return;
                //坐标改变
                CoordChangeByRowCol(row, col, true, false);
            }
        }

        private void AuxCvs_MouseLeave(object sender, EventArgs e)
        {
        }

        private void AuxCvs_MouseDoubleClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
        }

        private void AuxCvs_Resize(object sender, EventArgs e)
        {
        }
        #endregion

        #region 方法
        public void SetWaferDirImage(WaferDirEnum waferDir)
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MapCanvas));
            switch (waferDir)
            {
                case WaferDirEnum.DIR_NONE:
                    this.pbNotchDir.Image = ((System.Drawing.Image)(resources.GetObject("Notch_Top")));
                    break;
                case WaferDirEnum.DIR_90:
                    this.pbNotchDir.Image = ((System.Drawing.Image)(resources.GetObject("Notch_Right")));
                    break;
                case WaferDirEnum.DIR_180:
                    this.pbNotchDir.Image = ((System.Drawing.Image)(resources.GetObject("Notch_Down")));
                    break;
                case WaferDirEnum.DIR_270:
                    this.pbNotchDir.Image = ((System.Drawing.Image)(resources.GetObject("Notch_Left")));
                    break;
                default:
                    this.pbNotchDir.Image = ((System.Drawing.Image)(resources.GetObject("Notch_None")));
                    break;
            }
        }
        /// <summary>
        /// MapCanvas重置
        /// </summary>
        public void Reset()
        {
            Graphics dc = MainCvs.CreateGraphics();
            dc.Clear(Color.White);
            this.pbNotchDir.Image = null;
            //if (mapInfo != null) mapInfo.Clear();
            mapInfo = null;
            mLeft = 0;
            mTop = 0;
            mWidth = 0;
            mHeight = 0;

            this.mActRow = -1;
            this.mActCol = -1;
            this.aActRow = -1;
            this.aActCol = -1;

            this.AuxCvs.Visible = false;

            this.gbBinSet.Visible = false;
            this.gbMapOps.Visible = false;

            //isEffectHover = false;
            mActiveGrid = false;

            aActiveGrid = false;
        }

        /// <summary>
        /// 切换Map来源
        /// </summary>
        /// <param name="mapInfo"></param>
        public void ChangeMapSource(MapInfo mapInfo)
        {
            //MapInfo _mapInfo = new MapInfo(mapInfo.SrcPath);
            //_mapInfo.Copy(mapInfo);
            //复位
            Reset();

            //调整尺寸
            InitMapSizeByMapInfo(mapInfo);

            this.gbBinSet.Visible = true;
            this.gbMapOps.Visible = true;

            //设置面板参数
            #region combobox
            binRing.DataSource = null;
            binRing.Items.Clear();
            List<KeyValuePair<int, string>> listItem = new List<KeyValuePair<int, string>>();
            int index = 0;
            var tmpBinInfo = mapInfo.binInfo.Values.ToLookup(p => p.Bin);
            for (int i = 0; i < tmpBinInfo.Count; i++)
            {

                listItem.Add(new KeyValuePair<int, string>(index++, tmpBinInfo.ElementAt(i).Key));
            }
            binRing.DataSource = listItem;
            binRing.DisplayMember = "Value";
            binRing.ValueMember = "Key";
            if (lastBinIndex >= listItem.Count)
                lastBinIndex = 0;
            binRing.SelectedIndex = lastBinIndex;
            #endregion

            //绘制主Map
            this.MainCvs.Invalidate();

            //绘制小Map
            this.AuxCvs.Invalidate();
        }

        /// <summary>
        /// 刷新某个单元格
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="sel"></param>
        public void UpdateDieGridByRowCol(int row, int col, bool sel)
        {
            //Rectangle rect = new Rectangle();
            //GetRectangleByRowCol(row, col, ref rect);
            //this.MainCvs.Invalidate(rect);

            //if (sel == true)
            //    CoordChangeByRowCol(row, col);
        }

        /// <summary>
        /// 根据给定行列值设置画布绘图区域和画布大小
        /// </summary>
        /// <param name="row">行</param>
        /// <param name="col">列</param>
        /// <returns></returns>
        private bool InitMapSizeByMapInfo(MapInfo mapInfo)
        {
            int h = 0, w = 0;
            int min = 0;
            float width = 0, height = 0;

            this.mapInfo = mapInfo;
            int row = mapInfo.mapRow;
            int col = mapInfo.mapCol;

            //以较小的宽度设置为面板宽度
            w = MainCvs.Width;
            h = MainCvs.Height;
            min = (h < w) ? h : w;

            //主视图单元格大小设置
            this.mBetween = 1.0f;
            this.mLeft = 10;
            this.mTop = 10;
            width = (min - (col - 1) * this.mBetween - 2 * this.mLeft) / col;
            height = (min - (row - 1) * this.mBetween - 2 * this.mTop) / row;

            if (width >= MapMaxMin.GRID_MIN_PIXEL && height >= MapMaxMin.GRID_MIN_PIXEL)
            {
                this.mLeft = (int)((min - col * width - (col - 1) * this.mBetween) / 2 + (w - min) / 2.0);
                this.mTop = (int)((min - row * height - (row - 1) * this.mBetween) / 2 + (h - min) / 2.0);
                this.mWidth = width;
                this.mHeight = height;

                this.useAuxCvs = false;
            }
            else
            {
                this.mBetween = 0.0f;
                width = (min - (col - 1) * this.mBetween - 2 * this.mLeft) / col;
                height = (min - (row - 1) * this.mBetween - 2 * this.mTop) / row;

                this.mLeft = (int)((min - col * width - (col - 1) * this.mBetween) / 2 + (w - min) / 2.0);
                this.mTop = (int)((min - row * height - (row - 1) * this.mBetween) / 2 + (h - min) / 2.0);
                this.mWidth = width;
                this.mHeight = height;

                this.useAuxCvs = true;
            }

            this.AuxCvs.Visible = this.useAuxCvs;

            //辅视图单元格大小设置
            if (this.useAuxCvs == true)
            {
                w = AuxCvs.Width;
                h = AuxCvs.Height;
                min = (h < w) ? h : w;
                if (this.mWidth > this.mHeight)
                {
                    this.aCol = MapMaxMin.AUX_VIEW_ROWCOL;
                    this.aRow = (int)((double)mapInfo.mapRow / mapInfo.mapCol * this.aCol);
                }
                else
                {
                    this.aRow = MapMaxMin.AUX_VIEW_ROWCOL;
                    this.aCol = (int)((double)mapInfo.mapCol / mapInfo.mapRow * this.aRow);
                }
                int left2 = 5;
                int top2 = 5;
                int width2 = (int)((min - (this.aCol - 1) * 1.0 - left2) / this.aCol);
                int height2 = (int)((min - (this.aRow - 1) * 1.0 - top2) / this.aRow);
                do
                {
                    if (width2 <= 0 || height2 <= 0)
                    {
                        return false;
                    }

                    left2 = (int)(((min - this.aCol * width2 - (this.aCol - 1) * 1.0) / 2) + ((w - min) / 2.0));
                    top2 = (int)(((min - this.aRow * height2 - (this.aRow - 1) * 1.0) / 2) + ((h - min) / 2.0));

                    if (left2 < 0)
                    {
                        width2--;
                        continue;
                    }
                    if (top2 < 0)
                    {
                        height2--;
                        continue;
                    }

                    //设置有效绘图区域
                    this.aLeft = left2;
                    this.aTop = top2;
                    this.aWidth = width2;
                    this.aHeight = height2;

                    break;
                } while (true);
            }
            else
            {
                //this.AuxCvs.Visible = false;
            }

            /*
            //面板不够大
            if (width <= MapMaxMin.GRID_MIN_PIXEL || height <= MapMaxMin.GRID_MIN_PIXEL)
            {
                width = (width <= MapMaxMin.GRID_MIN_PIXEL) ? MapMaxMin.GRID_MIN_PIXEL : width;
                height = (height <= MapMaxMin.GRID_MIN_PIXEL) ? MapMaxMin.GRID_MIN_PIXEL : height;

                left = 3;
                top = 3;
                w = left * 2 + col * width + (col - 1) * this.mBetween;
                h = top * 2 + row * height + (row - 1) * this.mBetween;

                //设置有效绘图区域
                this.mLeft = left;
                this.mTop = top;
                this.mWidth = width;
                this.mHeight = height;
            }
            //画面足够大
            else
            {
                while (true)
                {
                    if (width <= 0 || height <= 0)
                    {
                        return false;
                    }

                    left = ((min - col * width - (col - 1) * this.mBetween) >> 1) + ((w - min) >> 1);
                    top = ((min - row * height - (row - 1) * this.mBetween) >> 1) + ((h - min) >> 1);

                    if (left < 0)
                    {
                        width--;
                        continue;
                    }
                    if (top < 0)
                    {
                        height--;
                        continue;
                    }

                    //设置有效绘图区域
                    this.mLeft = left;
                    this.mTop = top;
                    this.mWidth = width;
                    this.mHeight = height;

                    break;
                }
            }
            */

            return true;
        }

        /// <summary>
        /// 绘制单元格
        /// </summary>
        /// <param name="g"></param>
        /// <param name="rect"></param>
        /// <param name="frameColor"></param>
        /// <param name="fillColor"></param>
        private void DrawGrid(Graphics g, RectangleF rect, MapDrawModeEnum mode, Color frameColor, Color fillColor)
        {
            //画边框
            if (mode != MapDrawModeEnum.DrawInterior && frameColor != Color.Empty)
            {
                //实例化画笔
                Pen p = new Pen(Brushes.Blue, 1.0f);
                //设置画笔的样式
                p.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
                //绘制边框
                g.DrawRectangle(p, rect.X, rect.Y, rect.Width, rect.Height);
            }

            //填充
            if (mode != MapDrawModeEnum.DrawFrame && fillColor != Color.Empty)
            {
                //实例化刷子
                SolidBrush brush = new SolidBrush(fillColor);
                //填充矩形
                g.FillRectangle(brush, rect);
            }
        }

        /// <summary>
        /// 绘制单元格
        /// </summary>
        /// <param name="g"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="frameColor"></param>
        /// <param name="fillColor"></param>
        private void DrawMainGrid(Graphics g, int row, int col, MapDrawModeEnum mode, Color frameColor, Color fillColor)
        {
            RectangleF rect;

            //rect = new Rectangle(this.mTop + row * this.mHeight + row,
            //    this.mLeft + col * this.mWidth + col,
            //    this.mHeight,
            //    this.mWidth);

            rect = new RectangleF(this.mLeft + col * this.mWidth + col * this.mBetween,
                this.mTop + row * this.mHeight + row * this.mBetween,
                this.mWidth,
                this.mHeight);

            DrawGrid(g, rect, mode, frameColor, fillColor);
        }

        /// <summary>
        /// 绘制辅视图单元格
        /// </summary>
        /// <param name="g"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="mode"></param>
        /// <param name="frameColor"></param>
        /// <param name="fillColor"></param>
        private void DrawAuxGrid(Graphics g, int row, int col, MapDrawModeEnum mode, Color frameColor, Color fillColor)
        {
            Rectangle rect;

            rect = new Rectangle(this.aLeft + col * this.aWidth + col,
                this.aTop + row * this.aHeight + row,
                this.aWidth,
                this.aHeight);

            DrawGrid(g, rect, mode, frameColor, fillColor);
        }

        /// <summary>
        /// 根据XY坐标得到所在的单元格行列
        /// </summary>
        /// <param name="x">基于画布的X</param>
        /// <param name="y">基于画布的Y</param>
        /// <param name="row">基于Map的行</param>
        /// <param name="col">基于Map的列</param>
        /// <returns></returns>
        private bool GetMainRowColByXY(int x, int y, ref int row, ref int col)
        {
            // 判断画布是否合理
            if (this.mWidth <= 0 || this.mHeight <= 0 ||
                this.mLeft < 0 || this.mTop < 0)
                return false;

            // 判断选中XY是否合理
            if (x < this.mLeft || x > (MainCvs.Width - this.mLeft) ||
                y < this.mTop || y > (MainCvs.Height - this.mTop))
                return false;

            // 获得行列值
            row = (int)((y - this.mTop) / (this.mHeight + this.mBetween));
            col = (int)((x - this.mLeft) / (this.mWidth + this.mBetween));

            return true;
        }

        private bool GetAuxRowColByXY(int x, int y, ref int row, ref int col)
        {
            // 判断画布是否合理
            if (this.aWidth <= 0 || this.aHeight <= 0 ||
                this.aLeft < 0 || this.aTop < 0)
                return false;

            // 判断选中XY是否合理
            if (x < this.aLeft || x > (AuxCvs.Width - this.aLeft) ||
                y < this.aTop || y > (AuxCvs.Height - this.aTop))
                return false;

            // 获得行列值
            row = (int)((y - this.aTop) / (this.aHeight + 1)) + this.aStrRow;
            col = (int)((x - this.aLeft) / (this.aWidth + 1)) + this.aStrCol;

            return true;
        }

        /// <summary>
        /// 根据XY坐标得到所在的单元格矩形
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="rect"></param>
        /// <returns></returns>
        private bool GetMainRectangleByXY(int x, int y, ref RectangleF rect)
        {
            int row = 0, col = 0;
            if (GetMainRowColByXY(x, y, ref row, ref col) == true)
            {
                //rect = new Rectangle(this.mTop + row * this.mHeight,
                //this.mLeft + col * this.mWidth,
                //this.mHeight,
                //this.mWidth);

                rect = new RectangleF(this.mLeft + col * this.mWidth + col * this.mBetween,
                    this.mTop + row * this.mHeight + row * this.mBetween,
                    this.mWidth,
                    this.mHeight);

                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// 根据行列获得单元格所在的矩形
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="rect"></param>
        /// <returns></returns>
        private void GetMainRectangleByRowCol(int row, int col, ref RectangleF rect)
        {
            /*
            if (row < 0 || row >= mapInfo.mapRow ||
                col < 0 || col > mapInfo.mapCol)
                return false;
             */

            //rect = new Rectangle(this.mTop + row * this.mHeight,
            //    this.mLeft + col * this.mWidth,
            //    this.mHeight,
            //    this.mWidth);

            rect = new RectangleF(this.mLeft + col * this.mWidth + col * this.mBetween,
                this.mTop + row * this.mHeight + row * this.mBetween,
                this.mWidth,
                this.mHeight);

            //return true;
        }

        private void GetAuxRectangleByRowCol(int row, int col, ref Rectangle rect)
        {
            rect = new Rectangle(this.aLeft + col * this.aWidth + col * 1,
                this.aTop + row * this.aHeight + row * 1,
                this.aWidth,
                this.aHeight);
        }
        #endregion


    }
}
