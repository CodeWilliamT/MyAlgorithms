using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Utils;
using HalconDotNet;
using HTHalControl;
using System.Threading;
using HT_Lib;
using ToolKits.TemplateEdit;
using System.IO;
using ToolKits.FunctionModule;

namespace LeadframeAOI
{
    /// <summary>
    /// 多颗DIE检测标定、光源亮度标定
    /// </summary>
    public partial class FormUV2XY : Form
    {
        public HObject Image = new HObject();
        public static FormUV2XY Instance = null;
        MainTemplateForm mTmpFrm = null;
        Form modelForm = null;
        Model aoiModels;
        HTuple aoiHoms;
        HTuple Pos_Uv2Xy = null;
        ToolKits.FunctionModule.Vision tool_vision;
        public FormUV2XY()
        {
            InitializeComponent();
            tool_vision = new ToolKits.FunctionModule.Vision();
            aoiHoms = new HTuple();
            Instance = this;
        }

        public void SetupUI()
        {
            try
            {
                dgvUVXY2D.Rows.Add();
                if (Obj_Camera.Num_Camera > Obj_Camera.SelectedIndex)
                {
                    if (App.obj_Vision.List_UV2XYResult[Obj_Camera.SelectedIndex].Type!=HTupleType.INTEGER)
                    {
                        SetDGVValue(ref dgvUVXY2D, App.obj_Vision.List_UV2XYResult[Obj_Camera.SelectedIndex]);
                    }
                }
            }
            catch (Exception ex)
            {
                HTUi.PopError("" + ex.ToString());
            }
        }

        /// <summary>
        /// UV2XY标定时加载光源、点位和视觉参数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLoadPara_Click(object sender, EventArgs e)
        {
            try
            {
                //   App.obj_light.FlashMultiLight(LightUseFor.ScanPoint1st);
                HTUi.TipHint("参数加载成功");
            }
            catch (Exception exp)
            {
                HTUi.PopError(exp.ToString());
                return;
            }
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSavePara_Click(object sender, EventArgs e)
        {

            try
            {
                if (!App.obj_Operations.Save())
                {
                    HTUi.PopError("保存数据失败！\n" + App.obj_Operations.GetLastErrorString());
                }
            }
            catch (Exception ex)
            {
                HTUi.PopError("保存数据失败！\n" + ex.ToString());
            }
        }
        private delegate void ShowImageDelegate(HTWindowControl htWindow, HObject image, HObject region);
        public void ShowImage(HTWindowControl htWindow, HObject image, HObject region)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new ShowImageDelegate(ShowImage), new object[] { htWindow, image, region });
            }
            else
            {
                htWindow.ColorName = "green";
                htWindow.SetInteractive(false);
                if (htWindow.Image == null || !htWindow.Image.IsInitialized())
                    htWindow.RefreshWindow(image, region, "fit");//适应窗口
                else
                    htWindow.RefreshWindow(image, region, "");//可以不显示区域
                htWindow.SetInteractive(true);
            }
        }

        private void btnCreateCalibModel2D_Click(object sender, EventArgs e)
        {
            if (modelForm == null || modelForm.IsDisposed)
            {
                modelForm = new Form();
                if (this.mTmpFrm != null && !this.mTmpFrm.IsDisposed) this.mTmpFrm.Dispose();
                this.mTmpFrm = new MainTemplateForm(ToolKits.TemplateEdit.MainTemplateForm.TemplateScence.Match,
                                 this.htWindowCalibration, new MainTemplateForm.TemplateParam());
                modelForm.Controls.Clear();
                this.modelForm.Controls.Add(this.mTmpFrm);
                this.mTmpFrm.Dock = DockStyle.Fill;
                modelForm.Size = new Size(300, 450);
                modelForm.TopMost = true;
                modelForm.Show();
                int SH = Screen.PrimaryScreen.Bounds.Height;
                int SW = Screen.PrimaryScreen.Bounds.Width;
                modelForm.Location = new Point(SW - modelForm.Size.Width, SH / 8);

                Task.Run(() =>
                {
                    while (!mTmpFrm.WorkOver)
                    {
                        Thread.Sleep(200);
                    }
                    HTUi.TipHint("创建标定模板完成!");
                    HTLog.Info("创建标定模板完成!");
                    modelForm.Close();
                });
            }
            else
            {
                modelForm.Activate();
            }
        }

        private async void btnSaveCalibModel2D_Click(object sender, EventArgs e)
        {
            if (!this.mTmpFrm.tmpResult.createTmpOK)
            {
                HTUi.PopError("创建标定模板失败！");
                return;
            }
            Form_Wait.ShowForm();
            bool result = await Task.Run(() =>
            {
                try
                {
                    this.aoiModels = new Model();
                    this.aoiModels.Dispose();
                    this.aoiModels.showContour = this.mTmpFrm.tmpResult.showContour.CopyObj(1, -1);
                    this.aoiModels.defRows = this.mTmpFrm.tmpResult.defRows;
                    this.aoiModels.defCols = this.mTmpFrm.tmpResult.defCols;
                    this.aoiModels.modelType = this.mTmpFrm.tmpResult.modelType;
                    this.aoiModels.modelID = Vision.CopyModel(this.mTmpFrm.tmpResult.modelID, this.mTmpFrm.tmpResult.modelType);
                    this.aoiModels.scoreThresh = this.mTmpFrm.mte_TmpPrmValues.Score;
                    this.aoiModels.angleStart = this.mTmpFrm.mte_TmpPrmValues.AngleStart;
                    this.aoiModels.angleExtent = this.mTmpFrm.mte_TmpPrmValues.AngleExtent;
                        //保存至硬盘
                    string folderPath = App.SystemUV2XYDir + "\\Camera_" + Obj_Camera.SelectedIndex;
                    App.obj_Operations.CalibrUV2XYModelPath = folderPath;
                    if (!this.aoiModels.WriteModel(folderPath))
                    {
                        HTLog.Error("保存标定模板失败！");
                        HTUi.PopError("保存标定模板失败！");
                        return false;
                    }
                    Pos_Uv2Xy = new HTuple();
                    App.obj_Operations.XUv2xy = App.obj_Chuck.GetXPos();
                    App.obj_Operations.YUv2xy = App.obj_Chuck.GetYPos();
                    App.obj_Operations.ZUv2xy = App.obj_Chuck.GetZPos();
                    Pos_Uv2Xy.Append(App.obj_Operations.XUv2xy);
                    Pos_Uv2Xy.Append(App.obj_Operations.YUv2xy);
                    Pos_Uv2Xy.Append(App.obj_Operations.ZUv2xy);
                    HOperatorSet.WriteTuple(Pos_Uv2Xy, folderPath + "\\Pos_Uv2Xy.tup");
                    HTUi.TipHint("保存标定模板成功！");
                    HTLog.Info("保存标定模板成功！");
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            });
            Form_Wait.CloseForm();
            if (!result)
            {
                HTLog.Error("保存标定模板失败！");
                HTUi.PopError("保存标定模板失败！");
                return;
            }
            HTUi.TipHint("保存标定模板成功！");
            HTLog.Info("保存标定模板成功！");
        }

        private async void btnUVXYCalib2D_Click(object sender, EventArgs e)
        {

            if (this.aoiModels == null)
            {
                this.aoiModels = new Model();
                string folderPath = App.SystemUV2XYDir + "\\Camera_" + Obj_Camera.SelectedIndex;
                if (!this.aoiModels.ReadModel(folderPath))
                {
                    HTUi.PopError("未创建2D标定模板！");
                    return;
                }
                HOperatorSet.ReadTuple( folderPath + "\\Pos_Uv2Xy.tup",out Pos_Uv2Xy);
            }
            //设定走位范围
            CalibSearchRange csRangeFrm = new CalibSearchRange();
            if (csRangeFrm.ShowDialog() != DialogResult.OK) return;
            double xyRange = csRangeFrm.Range;
            //条件：标定板中心在2D相机视野中心位置时的chuck x y的粗略位置已知，即                              
            //料片视野为左下芯片为中心作为初始点位
            App.obj_Operations.XUv2xy = Pos_Uv2Xy[0];//App.obj_Chuck.ref_x + ((App.obj_Pdt.BlockNumber - 1) * App.obj_Pdt.BlockSpace + (App.obj_Pdt.ColumnNumber - 1) * App.obj_Pdt.ColomnSpace)/2 + App.obj_Pdt.RightEdge;
            App.obj_Operations.YUv2xy = Pos_Uv2Xy[1];//App.obj_Chuck.ref_y + App.obj_Pdt.TopEdge + (App.obj_Pdt.RowNumber - 1) * App.obj_Pdt.RowSpace/2;
            App.obj_Operations.ZUv2xy = Pos_Uv2Xy[2];//App.obj_Pdt.ZFocus;
            HTuple uvHxy = new HTuple();
            bool status = await Task.Run(() =>
            {
                return App.obj_Operations.calibUV2XY
                              (
                                  Obj_Camera.SelectedIndex,
                                  ref uvHxy,
                                  this.aoiModels,
                                  xyRange, 20
                              );
            });
            if (!status)
            {
                HTUi.PopError("2D相机uvHxy标定失败！");
                return;
            }
            for (int i = 0; i < dgvUVXY2D.RowCount; i++)
            {
                dgvUVXY2D.Rows[i].Cells[0].Value = uvHxy[i * 3].D;
                dgvUVXY2D.Rows[i].Cells[1].Value = uvHxy[i * 3 + 1].D;
                dgvUVXY2D.Rows[i].Cells[2].Value = uvHxy[i * 3 + 2].D;
            }
            HTUi.TipHint("2D相机uvHxy标定成功！");
            HTLog.Info("2D相机uvHxy标定成功！");

        }
        /// <summary>
        /// 设置DGV某个行列范围内的数据，赋值给tuple
        /// </summary>
        /// <param name="dgv"></param>
        /// <param name="rowInd">起始行与终止行数组</param>
        /// <param name="colInd">起始列与终止列数组</param>
        /// <param name="tuple"></param>
        private void SetDGVValue(ref DataGridView dgv, HTuple tuple)
        {
            for (int i = 0; i < dgv.RowCount; i++)
            {
                dgv.Rows[i].Cells[0].Value = tuple[i * 3].D;
                dgv.Rows[i].Cells[1].Value = tuple[i * 3 + 1].D;
                dgv.Rows[i].Cells[2].Value = tuple[i * 3 + 2].D;
            }
        }
        /// <summary>
        /// 获取DGV某个行列范围内的数据，赋值给tuple
        /// </summary>
        /// <param name="dgv"></param>
        /// <param name="rowInd">起始行与终止行数组</param>
        /// <param name="colInd">起始列与终止列数组</param>
        /// <param name="tuple"></param>
        private void GetDGVValue(DataGridView dgv, int[] rowInd, int[] colInd, ref HTuple tuple)
        {
            tuple = new HTuple();
            for (int i = rowInd[0]; i < rowInd[1] - rowInd[0] + 1; i++)
            {
                for (int j = colInd[0]; j < colInd[1] - colInd[0] + 1; j++)
                {
                    tuple.Append(Convert.ToDouble(dgv.Rows[i].Cells[j].Value));
                }
            }
        }

        private void btnSaveUVXY2D_Click(object sender, EventArgs e)
        {
            this.btnSaveUVXY2D.Focus();
            HTuple uvHxy = null;
            try
            {
                GetDGVValue(dgvUVXY2D, new int[] { 0, 1 }, new int[] { 0, 2 }, ref uvHxy);
            }
            catch (Exception)
            {
                MessageBox.Show("获取2D相机UV-XY矩阵数据失败！");
                return;
            }
            //保存
            string filePath = App.SystemUV2XYDir + "\\Camera_" + Obj_Camera.SelectedIndex+ "\\" + "UV2XY" + ".dat";
            try
            {
                string dir = Directory.GetParent(filePath).FullName;
                if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
                tool_vision.save_hom2d(filePath, uvHxy);
                App.obj_Vision.List_UV2XYResult[Obj_Camera.SelectedIndex] = uvHxy;
            }
            catch (Exception ex)
            {
                HTUi.PopError("2D相机UV-XY矩阵保存失败！\n"+ex.ToString());
                return;
            }
            HTUi.TipHint("2D相机UV-XY矩阵保存成功！");
            HTLog.Info("2D相机UV-XY矩阵保存成功！");
        }


        private void btnSnap_Click(object sender, EventArgs e)
        {
            try
            {
                if (App.obj_Vision.RunMode == 1) return;
                App.obj_Chuck.SWPosTrig();
                //4. 取图
                HOperatorSet.GenEmptyObj(out Image);
                string errStr = "";
                errStr = App.obj_Vision.obj_camera[Obj_Camera.SelectedIndex].CaputreImages(ref Image, 1, 5000);//获取图片
                ShowImage(htWindowCalibration, Image, null);
                if (errStr != "")
                {
                    HTUi.PopError(errStr);
                    btnSnap.Enabled = true;
                }
            }
            catch (Exception EXP)
            {
                HTUi.PopError(EXP.Message);
                btnSnap.Enabled = true;
            }
        }

        private void btnTool_Click(object sender, EventArgs e)
        {
            if (HTM.LoadUI() < 0)
            {
                HTUi.PopError("打开轴调试助手界面失败");
            }
        }

        private void btnCmrAxisTool_Click(object sender, EventArgs e)
        {
            FrmCamAxisMotion frm = FrmCamAxisMotion.Instance;
            if (frm == null || frm.IsDisposed)
            {
                frm = new FrmCamAxisMotion();
                frm.TopMost = true;
                int SH = Screen.PrimaryScreen.Bounds.Height;
                int SW = Screen.PrimaryScreen.Bounds.Width;
                frm.Show();
                frm.Location = new Point((SW - frm.Size.Width) / 2, (SH - frm.Size.Height) / 2);
            }
            else
            {
                frm.Activate();
            }
        }
    }
}
