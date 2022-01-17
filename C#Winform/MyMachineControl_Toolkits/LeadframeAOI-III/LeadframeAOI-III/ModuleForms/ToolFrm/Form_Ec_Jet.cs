using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using IniDll;
using HT_Lib;
using HalconDotNet;
using ToolKits.TemplateEdit;
using System.Threading;

namespace LeadframeAOI
{
    public partial class Form_Ec_Jet : Form
    {
        public static Form_Ec_Jet Instance;
        public static bool IsX = false;
        public static double RXorY = 4;
        public bool isX =false;
        public double rXorY=4;
        public double Jet_x;
        public double Jet_y;
        public double Jet_z;
        public double Snap_x;
        public double Snap_y;
        public double Snap_z;
        public double Result_x;
        public double Result_y;
        public double Result_x_Fix;
        public double Result_y_Fix;
        public double dr, dc;
        public int CalibLimit=10;
        MainTemplateForm mTmpMdlFrm = null;
        Form toolForm = null;
        Model MarkModel;
        public Form_Ec_Jet()
        {
            InitializeComponent();
            Instance = this;
            App.formIniConfig.LoadObj(this);
            numericUpDown1.Value = (decimal)Jet_x;
            numericUpDown2.Value = (decimal)Jet_y;
            numericUpDown3.Value = (decimal)Jet_z;
            radioButton1.Checked = isX;
            numericUpDown4.Value = (decimal)rXorY;
            numericUpDown5.Value = (decimal)Snap_x;
            numericUpDown6.Value = (decimal)Snap_y;
            numericUpDown7.Value = (decimal)Snap_z;
            num_CalibLimit.Value = (decimal)CalibLimit;
        }

        public static void ShowForm()
        {
            Form_Ec_Jet frm = Form_Ec_Jet.Instance;

            if (frm != null)
            {
                if (!frm.IsDisposed)
                {
                    frm.Dispose();
                }
            }
            frm = new Form_Ec_Jet();
            int SH = Screen.PrimaryScreen.Bounds.Height;
            int SW = Screen.PrimaryScreen.Bounds.Width;
            frm.Location = new Point(0, (SH - frm.Size.Height) / 2);
            frm.StartPosition = FormStartPosition.Manual;
            frm.Show();
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Jet_x = (double)numericUpDown1.Value;
            Jet_y = (double)numericUpDown2.Value;
            Jet_z = (double)numericUpDown3.Value;
            if(!App.obj_Chuck.XYZ_Move(Jet_x, Jet_y, Jet_z))
            {
                HT_Lib.HTLog.Error(App.obj_Chuck.GetLastErrorString());
            }
            if (checkBox_MoveByCapture.Checked) btnCapture_Click(null, null);
        }
        

        private void button_GetFbs_Click(object sender, EventArgs e)
        {
            Jet_x = App.obj_Chuck.GetXPos();
            Jet_y = App.obj_Chuck.GetYPos();
            Jet_z = App.obj_Chuck.GetZPos();
            numericUpDown1.Value = (decimal)Jet_x;
            numericUpDown2.Value = (decimal)Jet_y;
            numericUpDown3.Value = (decimal)Jet_z;

        }

        private void button_Save_Click(object sender, EventArgs e)
        {
            rXorY = (double)numericUpDown4.Value;
            isX = radioButton1.Checked;
            Jet_x = (double)numericUpDown1.Value;
            Jet_y = (double)numericUpDown2.Value;
            Jet_z = (double)numericUpDown3.Value;
            Snap_x = (double)numericUpDown5.Value;
            Snap_y = (double)numericUpDown6.Value;
            Snap_z = (double)numericUpDown7.Value;
            App.formIniConfig.SaveObj(this);
            RXorY = rXorY;
            IsX = isX;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            rXorY = (double)numericUpDown4.Value;
            isX = radioButton1.Checked;
            RXorY = rXorY;
            IsX = isX;
            if (App.obj_SystemConfig.Marking)
            {
                if (App.obj_Chuck.IsHaveInk() == 0)
                {
                    HTUi.PopError("喷印器无墨水！");
                    return;
                }
                if (App.obj_Chuck.IsJetRun() == 0)
                {
                    HTUi.PopError("喷印器未启动！");
                    return;
                }
            }
            else
            {
                HTUi.PopError("未启用喷印器！");
                return;
            }
            Jet_x = App.obj_Chuck.GetXPos();
            Jet_y = App.obj_Chuck.GetYPos();
            Jet_z = App.obj_Chuck.GetZPos();
            App.obj_Chuck.EC_Printer(isX,rXorY);
        }

        private void btnMove2SnapPos_Click(object sender, EventArgs e)
        {
            Snap_x = (double)numericUpDown5.Value;
            Snap_y = (double)numericUpDown6.Value;
            Snap_z = (double)numericUpDown7.Value;
            if(!App.obj_Chuck.XYZ_Move(Snap_x, Snap_y, Snap_z))
            {
                HT_Lib.HTLog.Error(App.obj_Chuck.GetLastErrorString());
            }
            if (checkBox_MoveByCapture.Checked) btnCapture_Click(null, null);
        }

        private void btn_Snap_Click(object sender, EventArgs e)
        {
            try
            {
                App.obj_Vision.obj_camera[Obj_Camera.SelectedIndex].Camera.ClearImageQueue();
                App.obj_Chuck.SWPosTrig();
                //4. 取图
                HOperatorSet.GenEmptyObj(out App.obj_Vision.Image);
                string errStr = "";
                HTuple width, height;
                //HOperatorSet.ReadImage(out App.obj_Vision.Image, "D:\\123.png");
                errStr = App.obj_Vision.obj_camera[Obj_Camera.SelectedIndex].CaputreImages(ref App.obj_Vision.Image, 1, 5000);//获取图片
                //HOperatorSet.GetImageSize(App.obj_Vision.Image, out width, out height);
                //var a = width / 2;
                //var b = height / 2;
                //HOperatorSet.GenCrossContourXld(out App.obj_Vision.showRegion, b, a, 100, 0);
                App.obj_Vision.ShowImage(htWindow, App.obj_Vision.Image, null);
                if (errStr != "")
                {
                    MessageBox.Show(errStr);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btnCrtModel_Click(object sender, EventArgs e)
        {
            if (toolForm == null || toolForm.IsDisposed)
            {
                toolForm = new Form();
                if (this.mTmpMdlFrm != null)
                    if (!this.mTmpMdlFrm.IsDisposed) this.mTmpMdlFrm.Dispose();
                this.mTmpMdlFrm = new MainTemplateForm(ToolKits.TemplateEdit.MainTemplateForm.TemplateScence.Match,
                                 this.htWindow, new MainTemplateForm.TemplateParam());
                toolForm.Controls.Clear();
                this.toolForm.Controls.Add(this.mTmpMdlFrm);
                this.mTmpMdlFrm.Dock = DockStyle.Fill;
                toolForm.Size = new Size(300, 600);
                toolForm.TopMost = true;
                toolForm.Show();
                int SH = Screen.PrimaryScreen.Bounds.Height;
                int SW = Screen.PrimaryScreen.Bounds.Width;
                toolForm.Location = new Point(SW - toolForm.Size.Width, SH / 8);

                Task.Run(() =>
                {
                    while (!mTmpMdlFrm.WorkOver)
                    {
                        Thread.Sleep(200);
                    }
                    HTUi.TipHint("创建模板完成!");
                    HTLog.Info("创建模板完成!");
                    toolForm.Close();
                });
            }
            else
            {
                toolForm.Activate();
            }
        }

        private async void btnSaveModel_Click(object sender, EventArgs e)
        {
            try
            {
                if (!this.mTmpMdlFrm.tmpResult.createTmpOK)
                {
                    HTUi.PopError("创建模板失败！");
                    return;
                }
                MarkModel = new Model();
                MarkModel.Dispose();
                MarkModel.showContour = this.mTmpMdlFrm.tmpResult.showContour.CopyObj(1, -1);
                MarkModel.defRows = this.mTmpMdlFrm.tmpResult.defRows;
                MarkModel.defCols = this.mTmpMdlFrm.tmpResult.defCols;
                MarkModel.modelType = this.mTmpMdlFrm.tmpResult.modelType;
                MarkModel.modelID = Vision.CopyModel(this.mTmpMdlFrm.tmpResult.modelID, this.mTmpMdlFrm.tmpResult.modelType);
                MarkModel.scoreThresh = this.mTmpMdlFrm.mte_TmpPrmValues.Score;
                MarkModel.angleStart = this.mTmpMdlFrm.mte_TmpPrmValues.AngleStart;
                MarkModel.angleExtent = this.mTmpMdlFrm.mte_TmpPrmValues.AngleExtent;
                //保存至硬盘
                string modelPath = App.SystemDir + "\\MarkModel";

                Form_Wait.ShowForm();
                await Task.Run(new Action(() =>
                {
                    try
                    {
                        if (!MarkModel.WriteModel(modelPath))
                        {
                            HTUi.PopError("保存匹配模板失败！");
                            return;
                        }
                        HOperatorSet.WriteImage(htWindow.Image, "tiff", 0, modelPath + "\\MarkImage.tiff");
                        HTUi.TipHint("保存匹配模板成功！");
                        HTLog.Info("保存匹配模板成功！");
                    }
                    catch (Exception ex)
                    {
                        HTUi.PopError("保存匹配模板出错！\n" + ex.ToString());
                    }
                }));
                Form_Wait.CloseForm();
            }
            catch (Exception ex)
            {
                HTUi.PopError("保存匹配模板失败！\n" + ex.ToString());
            }
        }

        private void btnRcgnzMark_Click(object sender, EventArgs e)
        {
            if (this.MarkModel == null)
            {
                this.MarkModel = new Model();
                string folderPath = App.SystemDir + "\\MarkModel";
                if (!this.MarkModel.ReadModel(folderPath))
                {
                    HTUi.PopError("未创建匹配模板！");
                    return;
                }
            }
            HObject ho_showCount = null, ho_update_show_contour = null;
            HTuple hv_found_row, hv_found_col, hv_found_angle, hv_found_score, hv_update_def_row, hv_update_def_col, hv_modelHimg, hv_iFlag1;
            HTuple imageWidth, imageHeight;
            //ho_update_show_contour.Dispose();
            if (MarkModel.matchRegion == null || !MarkModel.matchRegion.IsInitialized())
                HOperatorSet.GetDomain(htWindow.Image, out MarkModel.matchRegion);
            ToolKits.FunctionModule.Vision.find_model(htWindow.Image, MarkModel.matchRegion, MarkModel.showContour, out ho_update_show_contour,
                MarkModel.modelType, MarkModel.modelID, -1, -1, MarkModel.scoreThresh, 1, -1, -1, out hv_found_row,
                out hv_found_col, out hv_found_angle, out hv_found_score, out hv_update_def_row,
                out hv_update_def_col, out hv_modelHimg, out hv_iFlag1);
            if ((int)(new HTuple(hv_iFlag1.TupleNotEqual(0))) != 0)
            {
                ho_showCount.Dispose();
                ho_update_show_contour.Dispose();
                HTLog.Error("匹配失败！");
                return;
            }
            if(hv_found_row.Length==0)
            {
                HTLog.Error("匹配为空！");
                return;
            }
            HOperatorSet.GetImageSize(htWindow.Image, out imageWidth, out imageHeight);
            var a = imageWidth / 20;
            HOperatorSet.GenCrossContourXld(out App.obj_Vision.showRegion, hv_found_row, hv_found_col, a, 0);
            App.obj_Vision.ShowImage(htWindow, App.obj_Vision.Image, App.obj_Vision.showRegion);
            dc = (hv_found_col.D * 2 - imageWidth) / 2;
            dr = (hv_found_row.D * 2 - imageHeight) / 2;
            textBox3.Text = dc.ToString();
            textBox4.Text = dr.ToString();
            Snap_x = App.obj_Chuck.GetXPos();
            Snap_y = App.obj_Chuck.GetYPos();
            Result_x = Jet_x - Snap_x;
            Result_y = Jet_y - Snap_y;
            textBox1.Text = Result_x.ToString();
            textBox2.Text = Result_y.ToString();
        }

        private void btnCcltMarkRs_Click(object sender, EventArgs e)
        {
        }

        private void btnSetMarkPos_Click(object sender, EventArgs e)
        {
            App.obj_Chuck.Ref_Mark_x = Double.Parse(textBox1.Text);
            App.obj_Chuck.Ref_Mark_y = Double.Parse(textBox2.Text);
            App.obj_Chuck.Ref_Mark_z = (Double)numericUpDown3.Value;
            App.obj_Chuck.Mark_Focus_z = (Double)numericUpDown7.Value;
            frmChuckModules.Instance.propertyGrid1.Refresh();
            frmProduct.Instance.prgProductMagzine.Refresh();
            App.obj_Chuck.Save();
        }

        private void btn_GetNowPos_Click(object sender, EventArgs e)
        {

            Snap_x = App.obj_Chuck.GetXPos();
            Snap_y = App.obj_Chuck.GetYPos();
            Snap_z = App.obj_Chuck.GetZPos();
            numericUpDown5.Value = (decimal)Snap_x;
            numericUpDown6.Value = (decimal)Snap_y;
            numericUpDown7.Value = (decimal)Snap_z;

        }

        private void btnZsafe_Click(object sender, EventArgs e)
        {
            if (!App.obj_Chuck.MoveToSafeZPos())
            {
                MessageBox.Show(App.obj_Chuck.GetLastErrorString());
            }
        }

        private void btnCapture_Click(object sender, EventArgs e)
        {
            try
            {
                App.obj_Vision.obj_camera[Obj_Camera.SelectedIndex].Camera.ClearImageQueue();
                App.obj_Chuck.SWPosTrig();
                //4. 取图
                HOperatorSet.GenEmptyObj(out App.obj_Vision.Image);
                string errStr = "";
                HTuple width, height;
                //HOperatorSet.ReadImage(out App.obj_Vision.Image, "D:\\123.png");
                errStr = App.obj_Vision.obj_camera[Obj_Camera.SelectedIndex].CaputreImages(ref App.obj_Vision.Image, 1, 5000);//获取图片
                HOperatorSet.GetImageSize(App.obj_Vision.Image, out width, out height);
                var a = width / 2;
                var b = height / 2;
                HOperatorSet.GenCrossContourXld(out App.obj_Vision.showRegion, b, a, width/20, 0);
                App.obj_Vision.ShowImage(htWindow, App.obj_Vision.Image, App.obj_Vision.showRegion);
                if (errStr != "")
                {
                    MessageBox.Show(errStr);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label12.Text = "X(mm):" + App.obj_Chuck.GetXPos().ToString("F3");
            label11.Text = "Y(mm):" + App.obj_Chuck.GetYPos().ToString("F3");
            label10.Text = "Z(mm):" + App.obj_Chuck.GetZPos().ToString("F3");
        }

        private void btnZfocus_Click(object sender, EventArgs e)
        {

            if (!App.obj_Chuck.Z_Move(App.obj_Pdt.ZFocus))
            {
                MessageBox.Show(App.obj_Chuck.GetLastErrorString());
            }
        }

        private void btnCcJetPos_Click(object sender, EventArgs e)
        {

            numericUpDown1.Value = numericUpDown5.Value+(decimal)App.obj_Chuck.Ref_Mark_x;
            numericUpDown2.Value = numericUpDown6.Value+(decimal)App.obj_Chuck.Ref_Mark_y;
            numericUpDown3.Value = numericUpDown7.Value;
        }

        private void btnJet2CmCalib_Click(object sender, EventArgs e)
        {
            btnRcgnzMark_Click(null, null);
            double calibLimit = 0.001 / Math.Abs(App.obj_Vision.List_UV2XYResult[Obj_Camera.SelectedIndex][1].D);
            if (dr < calibLimit && dc < calibLimit && dr > -calibLimit && dc > calibLimit) goto _end;
            double x = App.obj_Chuck.GetXPos();
            double y = App.obj_Chuck.GetYPos();
            double z = App.obj_Chuck.GetZPos();
            double dx = dc*Math.Abs(App.obj_Vision.List_UV2XYResult[Obj_Camera.SelectedIndex][1].D)/2;
            double dy = -dr*Math.Abs(App.obj_Vision.List_UV2XYResult[Obj_Camera.SelectedIndex][3].D)/2;
            if (dc < calibLimit && dc>-calibLimit) dx = 0;
            if (dr < calibLimit && dc>-calibLimit) dy = 0;
            App.obj_Vision.SnapPos(x + dx, y + dy, z, htWindow, out App.obj_Vision.Image);
        _end:
            btnRcgnzMark_Click(null, null);
            HTuple width, height;
            HOperatorSet.GetImageSize(App.obj_Vision.Image, out width, out height);
            var a = width / 2;
            var b = height / 2;
            HOperatorSet.GenCrossContourXld(out App.obj_Vision.showRegion, b, a, width / 20, 0);
            App.obj_Vision.ShowImage(htWindow, App.obj_Vision.Image, App.obj_Vision.showRegion);
        }

        private void btnCcSnapPos_Click(object sender, EventArgs e)
        {

            numericUpDown5.Value = numericUpDown1.Value - (decimal)App.obj_Chuck.Ref_Mark_x;
            numericUpDown6.Value = numericUpDown2.Value - (decimal)App.obj_Chuck.Ref_Mark_y;
            numericUpDown7.Value = numericUpDown3.Value;
        }

        private void btnPrintReview_Click(object sender, EventArgs e)
        {

            if (App.obj_SystemConfig.Marking)
            {
                if (App.obj_Chuck.IsHaveInk() == 0)
                {
                    HTUi.PopError("喷印器无墨水！");
                    return;
                }
                if (App.obj_Chuck.IsJetRun() == 0)
                {
                    HTUi.PopError("喷印器未启动！");
                    return;
                }
            }
            else
            {
                HTUi.PopError("未启用喷印器！");
                return;
            }
            double x = App.obj_Chuck.GetXPos();
            double y = App.obj_Chuck.GetYPos();
            double z = App.obj_Chuck.GetZPos();
            double dx = Double.Parse(textBox1.Text);
            double dy = Double.Parse(textBox2.Text);
            App.obj_Chuck.EC_Printer(x + dx, y + dy, (double)numericUpDown3.Value,
                isX, rXorY);
            //if (!App.obj_Chuck.XYZ_Move(x+dx, y+dy, (double)numericUpDown3.Value))
            //{
            //    HT_Lib.HTLog.Error(App.obj_Chuck.GetLastErrorString());
            //}
            //rXorY = (double)numericUpDown4.Value;
            //isX = radioButton1.Checked;
            //App.obj_Chuck.EC_Printer(isX, rXorY);
            App.obj_Vision.SnapPos(x, y, z, htWindow, out App.obj_Vision.Image);
            HTuple width, height;
            HOperatorSet.GetImageSize(App.obj_Vision.Image, out width, out height);
            var a = width / 2;
            var b = height / 2;
            HOperatorSet.GenCrossContourXld(out App.obj_Vision.showRegion, b, a, width / 20, 0);
            App.obj_Vision.ShowImage(htWindow, App.obj_Vision.Image, App.obj_Vision.showRegion);

        }

        private void btnMutiCalib_Click(object sender, EventArgs e)
        {
            int t = 0;
            while (t++< CalibLimit)
            {
                btnRcgnzMark_Click(null, null);
                double calibLimit = 0.001 / Math.Abs(App.obj_Vision.List_UV2XYResult[Obj_Camera.SelectedIndex][1].D);
                if (dr < calibLimit && dc < calibLimit && dr>-calibLimit && dc>-calibLimit) goto _end;
                double x = App.obj_Chuck.GetXPos();
                double y = App.obj_Chuck.GetYPos();
                double z = App.obj_Chuck.GetZPos();
                double dx = dc * Math.Abs(App.obj_Vision.List_UV2XYResult[Obj_Camera.SelectedIndex][1].D) / 2;
                double dy = -dr * Math.Abs(App.obj_Vision.List_UV2XYResult[Obj_Camera.SelectedIndex][3].D) / 2;
                if (dc < calibLimit && dc > -calibLimit) dx = 0;
                if (dr < calibLimit && dc > -calibLimit) dy = 0;
                App.obj_Vision.SnapPos(x + dx, y + dy, z, htWindow, out App.obj_Vision.Image);
            }
        _end:
            btnRcgnzMark_Click(null, null);
            HTuple width, height;
            HOperatorSet.GetImageSize(App.obj_Vision.Image, out width, out height);
            var a = width / 2;
            var b = height / 2;
            HOperatorSet.GenCrossContourXld(out App.obj_Vision.showRegion, b, a, width / 20, 0);
            App.obj_Vision.ShowImage(htWindow, App.obj_Vision.Image, App.obj_Vision.showRegion);
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (this.MarkModel == null)
            {
                this.MarkModel = new Model();
                string folderPath = App.SystemDir + "\\MarkModel";
                if (!this.MarkModel.ReadModel(folderPath))
                {
                    HTUi.PopError("未创建匹配模板！");
                    return;
                }
            }
            HObject ho_showCount = null, ho_update_show_contour = null;
            HTuple hv_found_row, hv_found_col, hv_found_angle, hv_found_score, hv_update_def_row, hv_update_def_col, hv_modelHimg, hv_iFlag1;
            HTuple imageWidth, imageHeight;
            //ho_update_show_contour.Dispose();
            if (MarkModel.matchRegion == null || !MarkModel.matchRegion.IsInitialized())
                HOperatorSet.GetDomain(htWindow.Image, out MarkModel.matchRegion);
            ToolKits.FunctionModule.Vision.find_model(htWindow.Image, MarkModel.matchRegion, MarkModel.showContour, out ho_update_show_contour,
                MarkModel.modelType, MarkModel.modelID, -1, -1, MarkModel.scoreThresh, 1, -1, -1, out hv_found_row,
                out hv_found_col, out hv_found_angle, out hv_found_score, out hv_update_def_row,
                out hv_update_def_col, out hv_modelHimg, out hv_iFlag1);
            if ((int)(new HTuple(hv_iFlag1.TupleNotEqual(0))) != 0)
            {
                ho_showCount.Dispose();
                ho_update_show_contour.Dispose();
                HTLog.Error("匹配失败！");
                return;
            }
            if (hv_found_row.Length == 0)
            {
                HTLog.Error("匹配为空！");
                return;
            }
            HOperatorSet.GetImageSize(htWindow.Image, out imageWidth, out imageHeight);
            var a = imageWidth / 20;
            HOperatorSet.GenCrossContourXld(out App.obj_Vision.showRegion, hv_found_row, hv_found_col, a, 0);
            App.obj_Vision.ShowImage(htWindow, App.obj_Vision.Image, App.obj_Vision.showRegion);
            Result_x_Fix = Result_x - (hv_found_col.D * 2 - imageWidth) * Math.Abs(App.obj_Vision.List_UV2XYResult[Obj_Camera.SelectedIndex][1].D) / 2;
            Result_y_Fix = Result_y + (hv_found_row.D * 2 - imageHeight) * Math.Abs(App.obj_Vision.List_UV2XYResult[Obj_Camera.SelectedIndex][3].D) / 2;

            textBox1.Text = Result_x_Fix.ToString();
            textBox2.Text = Result_y_Fix.ToString();
        }
    }
}
