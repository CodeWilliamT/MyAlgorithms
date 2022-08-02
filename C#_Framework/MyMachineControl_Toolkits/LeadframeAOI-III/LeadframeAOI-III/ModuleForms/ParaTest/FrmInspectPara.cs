using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HT_Lib;

namespace LeadframeAOI
{
    public partial class FrmInspectPara : Form
    {
        public static FrmInspectPara Instance = null;
        public FrmInspectPara()
        {
            InitializeComponent();
            Instance = this;
        }
        public void SetupUI()
        {
            try
            {
                if (!App.obj_AlgApp.P.Load(App.AlgParamsPath))
                {
                    HTUi.PopError("读取检测参数失败！");
                }
                propertyGrid_Inspect.SelectedObject = App.obj_AlgApp.P;
                listBox_MainIcPara.Items.Clear();
                for (int i=0;i< App.obj_AlgApp.P.mainIcPara.Count;i++)
                {
                    listBox_MainIcPara.Items.Add("MainIc_"+i);
                }
                if(App.obj_AlgApp.P.mainIcPara.Count>0) listBox_MainIcPara.SelectedIndex = 0;
                listBox_MinorIcPara.Items.Clear();
                for (int i = 0; i < App.obj_AlgApp.P.minorIcPara.Count; i++)
                {
                    listBox_MinorIcPara.Items.Add("MinorIc_" + i);
                }
                if(App.obj_AlgApp.P.minorIcPara.Count>0) listBox_MinorIcPara.SelectedIndex = 0;
                listBox_FramePara.Items.Clear();
                for (int i = 0; i < App.obj_AlgApp.P.framePara.Count; i++)
                {
                    listBox_FramePara.Items.Add("Frame_" + i);
                }
                if(App.obj_AlgApp.P.framePara.Count > 0) listBox_FramePara.SelectedIndex = 0;
                listBox_WirePara.Items.Clear();
                for (int i = 0; i < App.obj_AlgApp.P.wirePara.Count; i++)
                {
                    listBox_WirePara.Items.Add("Wire_" + i);
                }
                if (App.obj_AlgApp.P.wirePara.Count > 0) listBox_WirePara.SelectedIndex = 0;
                num_ImgThreadMax.Value = App.obj_Vision.ThreadMax;
                //propertyGrid_MainIcPara.SelectedObject = App.obj_AlgApp.P.mainIcPara[listBox_MainIcPara.SelectedIndex
                propertyGrid_Inspect.Refresh();
                propertyGrid_FramePara.Refresh();
                propertyGrid_MainIcPara.Refresh();
                propertyGrid_MinorIcPara.Refresh();
                propertyGrid_WirePara.Refresh();
            }
            catch (Exception ex)
            {
                HTUi.PopError("UI初始化失败！\n" + ex.ToString());
            }
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!App.obj_AlgApp.P.Save(App.AlgParamsPath))
                {
                    HTUi.PopError("保存失败！\n");
                    return;
                }
                VisionFlow_Interface.Params.config.WriteInteger(App.obj_Vision.GetType().ToString(), "ImageThreadMax", App.obj_Vision.ThreadMax);

                App.obj_Vision.ListThreadInspectionFlag.Clear();
                for (int i = 0; i < App.obj_Vision.ThreadMax; i++)
                {
                    App.obj_Vision.ListThreadInspectionFlag.Add(false);
                }
                App.obj_AlgApp.Initialization(App.obj_Vision.ThreadMax, App.AlgParamsPath, App.ModelPath, App.Fuc_LibPath, App.Json_Path);
            }
            catch(Exception ex)
            {
                HTLog.Error("保存失败！"+ex.ToString()); 
                return;
            }
            HTLog.Info("保存成功！");
            HTUi.TipHint("保存成功！");
        }
        private void num_ImgThreadMax_ValueChanged(object sender, EventArgs e)
        {
            App.obj_Vision.ThreadMax = (int)num_ImgThreadMax.Value;
        }
        private void listBox_MainIcPara_SelectedIndexChanged(object sender, EventArgs e)
        {
            propertyGrid_MainIcPara.SelectedObject = App.obj_AlgApp.P.mainIcPara[listBox_MainIcPara.SelectedIndex];
        }
        private void btnAddMainIc_Click(object sender, EventArgs e)
        {
            try
            {
                App.obj_AlgApp.P.mainIcPara.Add(new VisionFlow_Interface.Params.GoldenModelPara());
                listBox_MainIcPara.Items.Add("MainIc_" + (App.obj_AlgApp.P.mainIcPara.Count - 1));
                if (App.obj_AlgApp.P.mainIcPara.Count > 0) listBox_MainIcPara.SelectedIndex = App.obj_AlgApp.P.mainIcPara.Count - 1;
            }
            catch
            {
                HTUi.PopError("添加失败！\n");
            }
        }

        private void btnDelMainIc_Click(object sender, EventArgs e)
        {
            try
            {
                if (App.obj_AlgApp.P.mainIcPara.Count > 0)
                {
                    int selectIdx = listBox_MainIcPara.SelectedIndex;
                    listBox_MainIcPara.SelectedIndex = 0;
                    listBox_MainIcPara.Items.RemoveAt(selectIdx);
                    App.obj_AlgApp.P.mainIcPara.RemoveAt(selectIdx);
                }
                else
                {
                    HTUi.PopError("没有元素可供删除！\n");
                }
                if (App.obj_AlgApp.P.mainIcPara.Count > 0) listBox_MainIcPara.SelectedIndex = App.obj_AlgApp.P.mainIcPara.Count - 1;
            }
            catch(Exception ex)
            {
                HTUi.PopError("删除失败！\n"+ex.ToString());
            }
        }

        private void listBox_MinorIcPara_SelectedIndexChanged(object sender, EventArgs e)
        {

            propertyGrid_MinorIcPara.SelectedObject = App.obj_AlgApp.P.minorIcPara[listBox_MinorIcPara.SelectedIndex];
        }

        private void btnAddMinorIc_Click(object sender, EventArgs e)
        {

            try
            {
                App.obj_AlgApp.P.minorIcPara.Add(new VisionFlow_Interface.Params.GoldenModelPara());
                listBox_MinorIcPara.Items.Add("MinorIc_" + (App.obj_AlgApp.P.minorIcPara.Count - 1));
                if (App.obj_AlgApp.P.minorIcPara.Count > 0) listBox_MinorIcPara.SelectedIndex = App.obj_AlgApp.P.minorIcPara.Count - 1;
            }
            catch
            {
                HTUi.PopError("添加失败！\n");
            }
        }

        private void btnDelMinorIc_Click(object sender, EventArgs e)
        {
            try
            {
                if (App.obj_AlgApp.P.minorIcPara.Count > 0)
                {
                    int selectIdx = listBox_MinorIcPara.SelectedIndex;
                    listBox_MinorIcPara.SelectedIndex = 0;
                    listBox_MinorIcPara.Items.RemoveAt(selectIdx);
                    App.obj_AlgApp.P.minorIcPara.RemoveAt(selectIdx);
                }
                else
                {
                    HTUi.PopError("没有元素可供删除！\n");
                }
                if (App.obj_AlgApp.P.minorIcPara.Count > 0) listBox_MinorIcPara.SelectedIndex = App.obj_AlgApp.P.minorIcPara.Count - 1;
            }
            catch (Exception ex)
            {
                HTUi.PopError("删除失败！\n" + ex.ToString());
            }
        }

        private void listBox_FramePara_SelectedIndexChanged(object sender, EventArgs e)
        {
            propertyGrid_FramePara.SelectedObject = App.obj_AlgApp.P.framePara[listBox_FramePara.SelectedIndex];

        }

        private void btnAddFrame_Click(object sender, EventArgs e)
        {

            try
            {
                App.obj_AlgApp.P.framePara.Add(new VisionFlow_Interface.Params.GoldenModelPara());
                listBox_FramePara.Items.Add("Frame_" + (App.obj_AlgApp.P.framePara.Count - 1));
                if (App.obj_AlgApp.P.framePara.Count > 0) listBox_FramePara.SelectedIndex = App.obj_AlgApp.P.framePara.Count - 1;
            }
            catch
            {
                HTUi.PopError("添加失败！\n");
            }
        }

        private void btnDelFrame_Click(object sender, EventArgs e)
        {
            try
            {
                if (App.obj_AlgApp.P.framePara.Count > 0)
                {
                    int selectIdx = listBox_FramePara.SelectedIndex;
                    listBox_FramePara.SelectedIndex = 0;
                    listBox_FramePara.Items.RemoveAt(selectIdx);
                    App.obj_AlgApp.P.framePara.RemoveAt(selectIdx);
                }
                else
                {
                    HTUi.PopError("没有元素可供删除！\n");
                }
                if (App.obj_AlgApp.P.framePara.Count > 0) listBox_FramePara.SelectedIndex = App.obj_AlgApp.P.framePara.Count - 1;
            }
            catch (Exception ex)
            {
                HTUi.PopError("删除失败！\n" + ex.ToString());
            }
        }

        private void FrmInspectPara_Enter(object sender, EventArgs e)
        {
            SetupUI();
        }

        private void btnAddWire_Click(object sender, EventArgs e)
        {


            try
            {
                App.obj_AlgApp.P.wirePara.Add(new VisionFlow_Interface.Params.WirePara());
                listBox_WirePara.Items.Add("Wire_" + (App.obj_AlgApp.P.wirePara.Count - 1));
                if (App.obj_AlgApp.P.wirePara.Count > 0) listBox_WirePara.SelectedIndex = App.obj_AlgApp.P.wirePara.Count - 1;
            }
            catch
            {
                HTUi.PopError("添加失败！\n");
            }
        }

        private void btnDelWire_Click(object sender, EventArgs e)
        {
            try
            {
                if (App.obj_AlgApp.P.wirePara.Count > 0)
                {
                    int selectIdx = listBox_WirePara.SelectedIndex;
                    listBox_WirePara.SelectedIndex = 0;
                    listBox_WirePara.Items.RemoveAt(selectIdx);
                    App.obj_AlgApp.P.wirePara.RemoveAt(selectIdx);
                }
                else
                {
                    HTUi.PopError("没有元素可供删除！\n");
                }
                if (App.obj_AlgApp.P.wirePara.Count > 0) listBox_WirePara.SelectedIndex = App.obj_AlgApp.P.wirePara.Count - 1;
            }
            catch (Exception ex)
            {
                HTUi.PopError("删除失败！\n" + ex.ToString());
            }
        }

        private void listBox_WirePara_SelectedIndexChanged(object sender, EventArgs e)
        {

            propertyGrid_WirePara.SelectedObject = App.obj_AlgApp.P.wirePara[listBox_WirePara.SelectedIndex];
        }
    }
}
