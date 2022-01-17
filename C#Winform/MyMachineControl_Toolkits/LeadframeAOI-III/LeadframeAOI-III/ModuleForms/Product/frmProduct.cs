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
using System.IO;
using HT_Lib;
using HalconDotNet;


namespace LeadframeAOI
{
    public partial class frmProduct : Form
    {
        public static frmProduct Instance;

        public frmProduct()
        {
            InitializeComponent();
            htWindow1.SetTablePanelVisible(false);
            htWindow1.SetStatusStripVisible(false);
            htWindow1.SetStripEnable(false);
            htWindow2.SetTablePanelVisible(false);
            htWindow2.SetStatusStripVisible(false);
            htWindow2.SetStripEnable(false);
            Instance = this;
            //var pdt = App.obj_Pdt.GetProductList();//这是个list
            //lbxProductCategory.DataSource = pdt;
            //prgProductMagzine.SelectedObject = App.obj_Pdt;
            //prgProductMagzine.Refresh();
            //Instance = this;
            //lblCurrentProd.Text = "当前产品：" + ProductMagzine.ActivePdt;
        }

        public void SetupUI()
        {
            var pdt = App.obj_Pdt.GetProductList();//这是个list
            lbxProductCategory.DataSource = pdt;
            prgProductMagzine.SelectedObject = App.obj_Pdt;
            RefreshPdtUI();
        }
        public void RefreshPdtUI()
        {

            prgProductMagzine.Refresh();
            HTLog.Info(String.Format("产品名：{0}", ProductMagzine.ActivePdt));
            FormJobs.Instance.label5.Invoke(new Action(() => { FormJobs.Instance.label5.Text = "当前产品:" + ProductMagzine.ActivePdt + "  流程状态:待机"; }));
            groupBox1.Text = "当前料片：" + ProductMagzine.ActivePdt;
            lblCurrentProd.Text = "当前产品：" + ProductMagzine.ActivePdt;

            //frmCaptureImage.Instance.txtTestImageFolder.Text = App.ProductFile + @"\" + ProductMagzine.ActivePdt + @"\image";

            if (File.Exists(App.ProductDir + "\\" + ProductMagzine.ActivePdt + "\\" + "ClipImage.tiff"))
            {
                FileStream fileStream = new FileStream(App.ProductDir + "\\" + ProductMagzine.ActivePdt + "\\" + "ClipImage.tiff", FileMode.Open, FileAccess.Read);
                int byteLength = (int)fileStream.Length;
                byte[] fileBytes = new byte[byteLength];
                fileStream.Read(fileBytes, 0, byteLength);
                //文件流关閉,文件解除锁定
                fileStream.Close();
                pictureBox1.BackgroundImage = Image.FromStream(new MemoryStream(fileBytes));
            }
            else
            {
                pictureBox1.BackgroundImage = null;
            }
            HObject imgActivePdt = null;
            if (File.Exists(App.ProductDir + "\\" + ProductMagzine.ActivePdt + "\\" + "frameMapImg.tiff"))
            {
                HOperatorSet.ReadImage(out imgActivePdt, App.ProductDir + "\\" + ProductMagzine.ActivePdt + "\\" + "frameMapImg.tiff");
                App.obj_Vision.ShowImage(htWindow1, imgActivePdt, null);
            }
            else
            {
                imgActivePdt = null;
                App.obj_Vision.ShowImage(htWindow1, imgActivePdt, null);
            }
        }
        private async void btnLoadProduct_Click(object sender, EventArgs e)
        {
            if (lbxProductCategory.SelectedIndex == -1) return;
            btnLoadProduct.Enabled = false;

            string pdt = null;
            pdt = lbxProductCategory.SelectedValue.ToString();
            App.obj_Pdt.SaveActivePdtName(pdt);
            App.obj_Pdt.LoadPdtData();
            App.obj_Pdt.ConfigPdtData();
            btnLoadProduct.Enabled = true;
            Form_Wait.ShowForm();
            await Task.Run(new Action(() =>
            {
                try
                {
                    App.obj_Vision.InitPdtData();
                }
                catch (Exception EXP)
                {
                    HTLog.Error(String.Format("{0}加载新产品失败\n", EXP.ToString()));
                }
                FrmAutoMapping.Instance.BeginInvoke(new Action(FrmAutoMapping.Instance.SetupUI));
                    FunctionTest.Instance.BeginInvoke(new Action(FunctionTest.Instance.RefreshLotList));
                    HTLog.Info("加载产品完成.");
                    HTUi.TipHint("加载产品完成");
                    if (App.obj_Pdt.MgzIdx == -1)
                    {
                        HTLog.Error("注意：当前产品未选择料盒型号，开始流程需要选择料盒型号.");
                    }
            }));
            RefreshPdtUI();
            Form_Wait.CloseForm();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            var pdt = lbxProductCategory.SelectedValue;
            if (!App.obj_Pdt.DeleteProduct((string)pdt))
            {
                HTUi.PopWarn("不能删除已激活的产品！");
            }
            if (HTUi.PopYesNo("是否删除选中产品？"))
            {

                lbxProductCategory.DataSource = App.obj_Pdt.GetProductList();
            }
        }

        private void btnCreateProduct_Click(object sender, EventArgs e)
        {

            String newfile = "";
            if (!MSG.Inputbox("Copy&New", "请输入新的产品名", out newfile))
            {
                return;
            }

            if (newfile == string.Empty)
            {
                HTUi.PopWarn("命名错误，名字不能为空");
                return;
            }
            var pdtlist = App.obj_Pdt.GetProductList();
            foreach (var pdt in pdtlist)
            {
                if (pdt == newfile)
                {
                    HTUi.PopWarn("无法新建，当前产品已存在");
                    return;
                }
                string souce = (string)ProductMagzine.ActivePdt;
                if (!App.obj_Pdt.CreateProduct(souce, newfile))
                {
                    HTUi.PopWarn("产品创建失败!");
                    return;
                }
                HTLog.Info("产品创建成功!");
                HTUi.TipHint("产品创建成功!");
                lbxProductCategory.DataSource = App.obj_Pdt.GetProductList();
                return;
            }


        }

        private void btnRename_Click(object sender, EventArgs e)
        {
            string souce = (string)lbxProductCategory.SelectedValue;
            if (souce == ProductMagzine.ActivePdt)
            {
                HTUi.PopError("当前产品名称不可修改");
                return;
            }

            String newname = "";
            if (!MSG.Inputbox("new", "rename", out newname))
            {
                return;
            }

            if (newname == string.Empty)
            {
                HTUi.PopWarn("命名错误，名字不能为空");
                return;
            }
            var pdtlist = App.obj_Pdt.GetProductList();
            foreach (var pdt in pdtlist)
            {
                if (pdt == newname)
                {
                    HTUi.PopWarn("命名冲突，当前产品已存在");
                    return;
                }
            }
            App.obj_Pdt.ChangeProdcutName(newname, souce);
            HTUi.TipHint("重命名成功!");
            lbxProductCategory.DataSource = App.obj_Pdt.GetProductList();
            return;
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            Form_Wait.ShowForm();
            await Task.Run(new Action(() =>
            {
                string db_Path = App.ProductDir + @"\" + ProductMagzine.ActivePdt + @"\ParaPdt.db";
                App.obj_Pdt.ConfigParaFile(db_Path);
                //if (!App.obj_Load.Save())
                //{
                //    HTLog.Error("上料参数保存失败");
                //    return;
                //}
                //if (!App.obj_Unload.Save())
                //{
                //    HTLog.Error("下料参数保存失败");
                //    return;
                //}
                if (!App.obj_Pdt.Save())
                {
                    HTLog.Error("产品参数保存失败");
                    return;
                }
                else
                {
                    HTLog.Info("产品参数保存成功");
                    frmCaptureImage.Instance.ShowProdMap();
                    FormJobs.Instance.ShowProdMap();
                    prgProductMagzine.BeginInvoke(new MethodInvoker(() => prgProductMagzine.Refresh()));
                    App.obj_Pdt.ProductInfoChanged();

                    App.obj_Load.z_LoadUnLoadFirstPos = App.obj_Pdt.HeightFirstSlot;
                    App.obj_Load.slotLayersCount = App.obj_Pdt.SlotNumber;
                    App.obj_Load.heightSlotLayer = (App.obj_Pdt.HeightLastSlot - App.obj_Pdt.HeightFirstSlot) / (App.obj_Pdt.SlotNumber - 1);
                    App.obj_Load.heightSlotLayer_y = (App.obj_Pdt.HeightLastSlot_y - App.obj_Pdt.HeightFirstSlot_y) / (App.obj_Pdt.SlotNumber - 1);
                    App.obj_Load.FrameLength = App.obj_Pdt.FrameLength;
                    App.obj_Unload.z_LoadUnLoadFirstPos = App.obj_Pdt.HeightFirstSlot_Unload;
                    App.obj_Unload.slotLayersCount = App.obj_Pdt.SlotNumber;
                    App.obj_Unload.heightSlotLayer = (App.obj_Pdt.HeightLastSlot_Unload - App.obj_Pdt.HeightFirstSlot_Unload) / (App.obj_Pdt.SlotNumber - 1);
                    App.obj_Unload.heightSlotLayer_y = (App.obj_Pdt.HeightLastSlot_Unload_y - App.obj_Pdt.HeightFirstSlot_Unload_y) / (App.obj_Pdt.SlotNumber - 1);
                    App.obj_Unload.FrameLength = App.obj_Pdt.FrameLength;

                }
                if(App.obj_Pdt.MgzIdx==-1)
                {
                    HTLog.Error("注意：当前产品未选择料盒型号，开始流程需要选择料盒型号.");
                    return;
                }
            }));
            Form_Wait.CloseForm();
        }

        private async void btnFixTrack_Click(object sender, EventArgs e)
        {
            switch (HTUi.PopSelect("请在以下选项中选择配置导轨方式。","直接配置","轴调试助手","取消"))
            {
                case 0:
                    Form_Wait.ShowForm();
                    await Task.Run(new Action(() =>
                    {
                        if (!App.obj_Chuck.Fix_Track(App.obj_Pdt.FrameWidth))
                        {
                            HTLog.Error(App.obj_Chuck.GetLastErrorString());
                            return;
                        }
                        HTLog.Info("导轨宽度配置成功.");
                    }));
                    Form_Wait.CloseForm();
                    break;
                case 1:
                    if (HTM.LoadUI() < 0)
                    {
                        HTUi.PopError("打开轴调试助手界面失败");
                    }
                    break;
                default:
                    break;
            }
        }

        private void lbxProductCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbxProductCategory.SelectedIndex == -1) return;
            if (lbxProductCategory.SelectedItem.ToString() == "")
            {
                groupBox2.Text = "选中料片：" + "无";
                lblSelectProd.Text = "选中产品：" + "无";
            }
            else
            {
                HObject imgSelectPdt = null;
                lblSelectProd.Text = "选中产品：" + lbxProductCategory.SelectedItem.ToString();
                string selectPdt = lbxProductCategory.SelectedItem.ToString();
                string selectPdtPath = App.ProductDir + "\\" + selectPdt;
                if (File.Exists(selectPdtPath + "\\" + "ClipImage.tiff"))
                {
                    FileStream fileStream = new FileStream(selectPdtPath + "\\" + "ClipImage.tiff", FileMode.Open, FileAccess.Read);
                    int byteLength = (int)fileStream.Length;
                    byte[] fileBytes = new byte[byteLength];
                    fileStream.Read(fileBytes, 0, byteLength);
                    //文件流关閉,文件解除锁定
                    fileStream.Close();
                    pictureBox2.BackgroundImage = Image.FromStream(new MemoryStream(fileBytes));
                }
                else
                {
                    pictureBox2.BackgroundImage = null;
                }
                if (File.Exists(selectPdtPath + "\\" + "frameMapImg.tiff"))
                {
                    HOperatorSet.ReadImage(out imgSelectPdt, selectPdtPath + "\\" + "frameMapImg.tiff");
                    groupBox2.Text = "选中料片：" + lbxProductCategory.SelectedItem.ToString();
                    App.obj_Vision.ShowImage(htWindow2, imgSelectPdt, null);
                }
                else
                {
                    imgSelectPdt = null;
                    App.obj_Vision.ShowImage(htWindow2, imgSelectPdt, null);
                }
            }
        }

        private void btnBoxManager_Click(object sender, EventArgs e)
        {

            Form_BoxManage frm = Form_BoxManage.Instance;
            if (frm == null || frm.IsDisposed)
            {
                frm = new Form_BoxManage();
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

        private void frmProduct_Enter(object sender, EventArgs e)
        {
            prgProductMagzine.Refresh();
        }

        private void tableLayoutPanel4_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
