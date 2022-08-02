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
    public partial class frmSystemConfig : Form
    {
        public static frmSystemConfig Instance;
        public frmSystemConfig()
        {
            InitializeComponent();
           // StartPosition = FormStartPosition.CenterScreen;
           
            //this.cbxQrLocation.SelectedIndex = Convert.ToInt32(App.obj_SystemConfig.QrChuckLocation); 
            Instance = this;
        }
        public void SetupUI()
        {
            cbxSystemMode.Items.AddRange(new object[] { "离线模式", "DEMO模式", "在线模式" });
            cbxLoadMode.Items.AddRange(new object[] { "手工上下料", "自动上下料" });
            cbxFixTools.Items.AddRange(new object[] { "压板", "压板+载台", "压板+载台+真空" });
            cbxMarkingPen.Items.AddRange(new object[] { "无需标记", "标记" });
            cbxPrdQr.Items.AddRange(new object[] { "无二维码", "有二维码" });
            cbxImageSave.Items.AddRange(new object[] { "保存全部视野", "保存NG视野","不保存视野" });
            cbxScanMode.Items.AddRange(new object[] { "逐点扫描", "X向线性扫描", "Y向线性扫描" });
            cbxUseNGBox.Items.AddRange(new object[] { "不使用","使用"});
            cbxLoadSame.Items.AddRange(new object[] { "不使用", "使用" });
            cbxLotIdMode.Items.AddRange(new object[] { "手动输入", "时间命名" });
            cbxEnHanceLight.Items.AddRange(new object[] { "否", "是" });
            cbxEnHanceLight_Ex.Items.AddRange(new object[] { "否", "是" });
            cbxMarkSave.Items.AddRange(new object[] { "否", "是" });
            cbxScanLot.Items.AddRange(new object[] { "否", "是" });
            cbxScanRecipe.Items.AddRange(new object[] { "否", "是" });
            cbxLotMode.Items.AddRange(new object[] { "料盒对应批次", "流程对应批次" });
            cbxSwichLineWidth.Items.AddRange(new object[] { "否", "是" });
            cbxJawCatchMode.Items.AddRange(new object[] { "电机控制", "IO控制" });
            cbxSystemMode.SelectedIndex = App.obj_SystemConfig.systemRunMode;
            if (App.obj_SystemConfig.LoadManually)
            {
                cbxLoadMode.SelectedIndex = 0;
            }
            else
            {
                cbxLoadMode.SelectedIndex = 1;
            }
            cbxPrdQr.SelectedIndex = Convert.ToInt32(App.obj_SystemConfig.PrdQr);
            cbxFixTools.SelectedIndex = Convert.ToInt32(App.obj_SystemConfig.CatchMode);
            cbxMarkingPen.SelectedIndex = Convert.ToInt32(App.obj_SystemConfig.Marking);
            cbxImageSave.SelectedIndex = Convert.ToInt32(App.obj_SystemConfig.ImageNgSave);
            cbxScanMode.SelectedIndex = App.obj_SystemConfig.ScanMode;
            cbxUseNGBox.SelectedIndex = Convert.ToInt32(App.obj_SystemConfig.UseNGBox);
            cbxLoadSame.SelectedIndex = Convert.ToInt32(App.obj_SystemConfig.LoadSame);
            cbxLotIdMode.SelectedIndex = App.obj_SystemConfig.LotIdMode;
            cbxEnHanceLight.SelectedIndex = App.obj_SystemConfig.UseEnHanceLight;
            cbxEnHanceLight_Ex.SelectedIndex = App.obj_SystemConfig.UseEnHanceLight_Ex;
            cbxMarkSave.SelectedIndex = App.obj_SystemConfig.MarkSave;
            cbxScanLot.SelectedIndex = App.obj_SystemConfig.ScanLot;
            cbxScanRecipe.SelectedIndex = App.obj_SystemConfig.ScanRecipe;
            cbxLotMode.SelectedIndex = App.obj_SystemConfig.LotMode;
            cbxSwichLineWidth.SelectedIndex = App.obj_SystemConfig.SwichLineWidth;
            cbxJawCatchMode.SelectedIndex = App.obj_SystemConfig.JawCatchMode;
        }
        private void btSave_Click(object sender, EventArgs e)
        {  
            App.obj_SystemConfig.systemRunMode = cbxSystemMode.SelectedIndex;
            if (cbxLoadMode.SelectedIndex == 1)//1-自动上下料
            {
                App.obj_SystemConfig.LoadManually = false; 
            }
            else
            {
                App.obj_SystemConfig.LoadManually = true;
            }
            App.obj_SystemConfig.CatchMode = cbxFixTools.SelectedIndex;
            App.obj_SystemConfig.Marking = Convert.ToBoolean(cbxMarkingPen.SelectedIndex);
            App.obj_SystemConfig.PrdQr = Convert.ToBoolean(cbxPrdQr.SelectedIndex);
            App.obj_SystemConfig.ScanMode = cbxScanMode.SelectedIndex;
            App.obj_SystemConfig.UseNGBox = Convert.ToBoolean(cbxUseNGBox.SelectedIndex);
            App.obj_SystemConfig.ImageNgSave = cbxImageSave.SelectedIndex;
            App.obj_SystemConfig.LoadSame= Convert.ToBoolean(cbxLoadSame.SelectedIndex);
            App.obj_SystemConfig.LotIdMode = cbxLotIdMode.SelectedIndex;
            App.obj_SystemConfig.UseEnHanceLight = cbxEnHanceLight.SelectedIndex;
            App.obj_SystemConfig.UseEnHanceLight_Ex = cbxEnHanceLight_Ex.SelectedIndex;
            App.obj_SystemConfig.MarkSave = cbxMarkSave.SelectedIndex;
            App.obj_SystemConfig.ScanLot=cbxScanLot.SelectedIndex;
            App.obj_SystemConfig.ScanRecipe = cbxScanRecipe.SelectedIndex;
            App.obj_SystemConfig.LotMode= cbxLotMode.SelectedIndex;
            App.obj_SystemConfig.SwichLineWidth = cbxSwichLineWidth.SelectedIndex;
            App.obj_SystemConfig.JawCatchMode = cbxJawCatchMode.SelectedIndex;
            //  App.SetSystemMode();
            App.obj_SystemConfig.SaveSystemConfig();        
            HTUi.TipHint("系统设置保存参数成功！");
        }
    }
}