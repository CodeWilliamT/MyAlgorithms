using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HTHalControl;
using HalconDotNet;
using ToolKits.RegionModify;
using VisionMethonDll;

namespace LeadframeAOI
{
    public partial class Form_FixMapPos : Form
    {
        public static Form_FixMapPos Instance = null;
        public HObject showRegion = null;
        Model LoctionModels = null;
        HTWindowControl htWindow = null;
        RegionModifyForm regionModifyForm = null;
        public Form_FixMapPos()
        {
            InitializeComponent();
            this.Icon = System.Drawing.Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            Instance = this;
        }
        /// <summary>
        /// 窗体实例化方法
        /// </summary>
        /// <param name="htWindow">图像视窗</param>
        /// <param name="Models">操作的模板变量</param>
        /// <param name="regionMode">区域类型</param>
        public Form_FixMapPos(HTWindowControl htWindow, Model Models, RegionModifyForm.RegionMode regionMode)
        {
            InitializeComponent();
            numericUpDown1.Value = (decimal)0.5;
            trackBar1.Value = (int)(numericUpDown1.Value*100);
            this.LoctionModels = Models;
            this.htWindow = htWindow;
            this.htWindow.SetMenuStrip(false);
            Instance = this;
        }

        private void button_DragModelRegion_Click(object sender, EventArgs e)
        {
            if (showRegion != null)
                if (showRegion.IsInitialized())
                    showRegion.Dispose();
            showRegion = regionModifyForm.ModifyRegion;
            htWindow.Focus();
            HObject ho_show_region = null, ho_dragRect=null, ho_update_show_contour=null;
            HOperatorSet.GenRegionContourXld(LoctionModels.showContour, out ho_show_region,
                "filled");
            HOperatorSet.DragRegion1(ho_show_region, out ho_dragRect, htWindow.HTWindow.HalconWindow);
            HOperatorSet.GenContourRegionXld(ho_dragRect, out ho_update_show_contour,
                "border");
            HOperatorSet.ConcatObj(ho_update_show_contour, showRegion, out showRegion);
            if (regionModifyForm != null)
                if (!regionModifyForm.IsDisposed)
                    regionModifyForm.Dispose();
            regionModifyForm = new RegionModifyForm(htWindow, showRegion, RegionModifyForm.RegionMode.contour);
            regionModifyForm.Dock = DockStyle.Fill;
            regionModifyForm.Visible = true;
            panel1.Controls.Add(regionModifyForm);
            App.obj_Vision.ShowImage(htWindow, htWindow.Image, showRegion);
        }

        private void button_GenClipPos_Click(object sender, EventArgs e)
        {

            try
            {
                HTuple hv_iFlag=null;
                VisionMethon.get_mapping_coords_ext(showRegion, App.obj_Vision.hv_xSnapPosLT, App.obj_Vision.hv_ySnapPosLT, App.obj_Vision.List_UV2XYResult[Obj_Camera.SelectedIndex], App.obj_Vision.scaleFactor,
                    App.obj_Pdt.RowNumber, App.obj_Pdt.ColumnNumber * App.obj_Pdt.BlockNumber,
                    out App.obj_Vision.clipMapX, out App.obj_Vision.clipMapY, out App.obj_Vision.clipMapRow, out App.obj_Vision.clipMapCol,
                    out App.obj_Vision.clipMapU, out App.obj_Vision.clipMapV, out App.obj_Vision.hv_dieWidth, out App.obj_Vision.hv_dieHeight, out hv_iFlag);
                if (hv_iFlag.S != "")
                {
                    MessageBox.Show("生成芯片点位失败." + hv_iFlag.S);
                    return;
                }
                HOperatorSet.WriteTuple(App.obj_Vision.clipMapX, App.ProductDir + "\\" + ProductMagzine.ActivePdt + "\\clipMapX.dat");
                HOperatorSet.WriteTuple(App.obj_Vision.clipMapY, App.ProductDir + "\\" + ProductMagzine.ActivePdt + "\\clipMapY.dat");
                HOperatorSet.WriteTuple(App.obj_Vision.clipMapRow, App.ProductDir + "\\" + ProductMagzine.ActivePdt + "\\clipMapRow.dat");
                HOperatorSet.WriteTuple(App.obj_Vision.clipMapCol, App.ProductDir + "\\" + ProductMagzine.ActivePdt + "\\clipMapCol.dat");
                App.obj_Vision.scanIniConfig.WriteDouble("ScanPoints", "dieWidth", App.obj_Vision.hv_dieWidth.D);
                App.obj_Vision.scanIniConfig.WriteDouble("ScanPoints", "dieHeight", App.obj_Vision.hv_dieHeight.D);
                //清除之前的点位
                if (App.obj_Vision.ClipMapPostions != null)
                {
                    App.obj_Vision.ClipMapPostions.Clear();
                }
                else
                {
                    App.obj_Vision.ClipMapPostions = new List<ImagePosition>();
                }
                ImagePosition imagePosition = new ImagePosition();
                imagePosition.z = App.obj_Pdt.ZFocus;
                imagePosition.b = 0;
                App.obj_Vision.clipPosNum = App.obj_Vision.clipMapX.Length;
                for (int i = 0; i < App.obj_Vision.clipPosNum; i++)
                {
                    imagePosition.x = App.obj_Vision.clipMapX.TupleSelect(i);
                    imagePosition.y = App.obj_Vision.clipMapY.TupleSelect(i);
                    imagePosition.r = App.obj_Vision.clipMapRow.TupleSelect(i);
                    imagePosition.c = App.obj_Vision.clipMapCol.TupleSelect(i);
                    App.obj_Vision.ClipMapPostions.Add(imagePosition);
                }
                MessageBox.Show("生成芯片点位完成.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("保存芯片点位失败。\n" + ex.ToString());
            }
        }

        private void button_FindModel_Click(object sender, EventArgs e)
        {
            if(showRegion!=null)
                if(showRegion.IsInitialized()) showRegion.Dispose();
            App.obj_Vision.ShowImage(htWindow, htWindow.Image, null);
            HTuple hv_found_row=null;HTuple hv_found_col=null;
            HTuple hv_found_angle=null; HTuple hv_found_score=null; HTuple hv_update_def_row=null;
            HTuple hv_update_def_col=null; HTuple hv_model_H_new=null; HTuple hv_iFlag=null;
            VisionMethon.find_model_ext(htWindow.Image, htWindow.Image, LoctionModels.showContour, out showRegion,
          LoctionModels.modelType, LoctionModels.modelID, -1, -1, ((double)trackBar1.Value)/100, 0, LoctionModels.defRows, LoctionModels.defCols,
          out hv_found_row, out hv_found_col, out hv_found_angle, out hv_found_score,
          out hv_update_def_row, out hv_update_def_col, out hv_model_H_new, out hv_iFlag);
            App.obj_Vision.ShowImage(htWindow, htWindow.Image, showRegion);
            if (regionModifyForm != null)
                if (!regionModifyForm.IsDisposed)
                    regionModifyForm.Dispose();
            regionModifyForm = new RegionModifyForm(htWindow, showRegion, RegionModifyForm.RegionMode.contour);
            regionModifyForm.Dock = DockStyle.Fill;
            regionModifyForm.Visible = true;
            panel1.Controls.Add(regionModifyForm);

        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            numericUpDown1.Value = ((decimal)trackBar1.Value)/100;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            trackBar1.Value = (int)(numericUpDown1.Value*100) ;
        }

        private void Form_FixMapPos_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.htWindow.SetMenuStrip(true);
        }
    }
}
