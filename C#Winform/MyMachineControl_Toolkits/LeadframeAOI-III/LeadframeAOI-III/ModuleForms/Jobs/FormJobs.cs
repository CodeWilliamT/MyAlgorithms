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
using Utils;
using HalconDotNet;
using HTHalControl;
//using htWindow;
using System.Threading;
using System.IO;

namespace LeadframeAOI
{

    public enum MachineState
    {
        STATE_IDLE = 0,
        STATE_RUN = 1,
        STATE_PAUSE = 2,
        STATE_ALARM = 3
    }

    public partial class FormJobs : Form
    {
        public delegate void MchineStateChangedDel(MachineState state);
        public event MchineStateChangedDel MchineStateChangedEvent;
        public static FormJobs Instance = null;
        public string frameFolder = "";
        public HObject ImageOrigin;
        public int SelectImgIdx = 0;

        Panel paneltemp1, paneltemp2;
        private HObject ImageShown;
        int SelectChannelIdx = 0;
        public FormJobs()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
            Instance = this;
        }


        public void SetupUI()
        {
            HOperatorSet.SetFont(this.htWindow.HTWindow.HalconWindow, "-Courier New-32-*-*-*-*-1-");
            mappingControl1.OnSelectedDieChanged += ShowClipData;
            Instance = this;
            ShowProdMap();
            //this.lbxLog.MultiColumn = false;
            btnMute.Image = ilst1.Images[0];
            this.btnMute.Text = "蜂鸣";
            RefreshImageList();
            RefreshChannelList();
        }
        #region 方法
        /// <summary>
        /// 更新图片选择项
        /// </summary>
        private void RefreshImageList()
        {
            cbBox_ImgSelect.Invoke(new MethodInvoker(() =>
            {
                int ImgCount = App.obj_ImageInformSet.Count;
                int sltIdx = cbBox_ImgSelect.SelectedIndex;
                cbBox_ImgSelect.Items.Clear();
                for (int i = 0; i < ImgCount; i++)
                {
                    cbBox_ImgSelect.Items.Add("第" + (i + 1) + "张图");
                }
                if (sltIdx == -1 || sltIdx >= ImgCount)
                {
                    cbBox_ImgSelect.SelectedIndex = 0;
                }
                else
                {
                    cbBox_ImgSelect.SelectedIndex = sltIdx;
                }
            }));
        }

        /// <summary>
        /// 更新通道选择项
        /// </summary>
        private void RefreshChannelList()
        {
            cbBox_ChannelSelect.Invoke(new MethodInvoker(() =>
            {
                int ChannelCount = 3;
                int sltIdx = cbBox_ChannelSelect.SelectedIndex;
                cbBox_ChannelSelect.Items.Clear();
                cbBox_ChannelSelect.Items.Add("原图");
                if (ChannelCount > 1)
                {
                    for (int i = 0; i < ChannelCount; i++)
                    {
                        cbBox_ChannelSelect.Items.Add("通道" + (i + 1));
                    }
                }
                if (sltIdx == -1 || sltIdx >= ChannelCount)
                {
                    cbBox_ChannelSelect.SelectedIndex = 0;
                }
                else
                {
                    cbBox_ChannelSelect.SelectedIndex = sltIdx;
                }
            }));
        }
        /// <summary>
        /// 显示所选择的图片通道所对应的图像
        /// </summary>
        /// <param name="Img"></param>
        public void ShowSelectImageChannel(HObject Img)
        {
            if (SelectChannelIdx == 0)
            {
                App.obj_Vision.ShowImage(htWindow, Img, htWindow.Region);
            }
            else
            {
                HObject ImgChannel1, ImgChannel2, ImgChannel3;
                HOperatorSet.GenEmptyObj(out ImgChannel1);
                HOperatorSet.GenEmptyObj(out ImgChannel2);
                HOperatorSet.GenEmptyObj(out ImgChannel3);
                HOperatorSet.GenEmptyObj(out ImageShown);
                ImageShown.Dispose();
                ImgChannel1.Dispose();
                ImgChannel2.Dispose();
                ImgChannel3.Dispose();
                ImageShown = Img;
                HOperatorSet.Decompose3(ImageShown, out ImgChannel1, out ImgChannel2, out ImgChannel3);
                if (SelectChannelIdx == 1)
                {
                    ImageShown = ImgChannel1;
                }
                else if (SelectChannelIdx == 2)
                {
                    ImageShown = ImgChannel2;
                }
                else if (SelectChannelIdx == 3)
                {
                    ImageShown = ImgChannel3;
                }
                App.obj_Vision.ShowImage(htWindow, ImageShown, htWindow.Region);
            }
        }
        /// <summary>
        /// 显示芯片数据
        /// </summary>
        /// <param name="dieInfo">芯片信息</param>
        private void ShowClipData(HTMappingControl.DieInfo dieInfo)
        {
            try
            {
                if (mappingControl1.GetDieState(dieInfo.RowIndex, dieInfo.ColumnIndex) != "不合格")
                {
                    HTUi.TipHint("只能查看不合格芯片！");
                    return;
                }
                if (htWindow.Image != null)
                {
                    htWindow.Image.Dispose();
                }
                string resultFolder = App.obj_Vision.imageFolder+"\\"+"Result"+"\\"+ProductMagzine.ActivePdt+"\\"+App.obj_Process.LotId;
                int frameIdx = LFAOIReview.DataManager.GetFrameIndex(ProductMagzine.ActivePdt, App.obj_Process.LotId, App.obj_Process.FrameId);
                string clipPath = resultFolder + "\\" + frameIdx+"_" + dieInfo.RowIndex + "_" +
                    dieInfo.ColumnIndex;
                HObject clipImg = null; HObject defectRegion = null; HObject wireRegion = null;
                HTuple dxfStatus = null;
                HOperatorSet.ReadImage(out clipImg, clipPath + "_0.tiff");
                HOperatorSet.ReadRegion(out defectRegion, clipPath + "_0.reg");
                HOperatorSet.ReadContourXldDxf(out wireRegion, clipPath + "_0_wire.dxf", new HTuple(), new HTuple(), out dxfStatus);
                if (ImageOrigin != null) ImageOrigin.Dispose();
                ImageOrigin = clipImg;
                ShowSelectImageChannel(ImageOrigin.SelectObj(SelectImgIdx + 1));
                htWindow.Region = defectRegion;
                HOperatorSet.SetDraw(htWindow.HTWindow.HalconWindow, "margin");
                HOperatorSet.SetColor(htWindow.HTWindow.HalconWindow, "yellow");
                HOperatorSet.DispRegion(defectRegion, htWindow.HTWindow.HalconWindow);
                HOperatorSet.SetColor(htWindow.HTWindow.HalconWindow, "green");
                HOperatorSet.DispXld(wireRegion, htWindow.HTWindow.HalconWindow);
            }
            catch (Exception ex)
            {
                HTUi.PopError("查看该视野信息失败\n" + ex.ToString());
            }
        }
        /// <summary>
        /// 使得控件不可用
        /// </summary>
        /// <param name="con"></param>
        private void DisableControls(Control con)
        {
            foreach (Control c in con.Controls)
            {
                DisableControls(c);
                c.ForeColor = System.Drawing.Color.WhiteSmoke;
            }
            con.Enabled = false;
        }

        /// <summary>
        /// 使得控件可用
        /// </summary>
        /// <param name="con"></param>
        private void EnableControls(Control con)
        {
            if (con != null)
            {
                con.Enabled = true;
                EnableControls(con.Parent);
            }
        }


        /// <summary>
        /// 显示图像的委托
        /// </summary>
        /// <param name="image"></param>
        /// <param name="region"></param>
        private delegate void ShowImageDelegate(HObject image, HObject region);
        /// <summary>
        /// 显示图像
        /// </summary>
        /// <param name="image"></param>
        /// <param name="region"></param>
        public void ShowImage(HObject image, HObject region)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new ShowImageDelegate(ShowImage), new object[] { image, region });
            }
            else
            {
                htWindow.ColorName = "green";
                htWindow.SetInteractive(false);
                htWindow.RefreshWindow(image, region, "");//可以不显示区域
                htWindow.SetInteractive(true);
                //htWindow.ColorName = "green";
            }
        }

        /// <summary>
        /// 更新图谱某点的状态的方法的委托
        /// </summary>
        /// <param name="ProdState">状态</param>
        /// <param name="row">纵坐标</param>
        /// <param name="column">横坐标</param>
        delegate void ShowProdStateDelegate(int ProdState, int row, int column);

        /// <summary>
        /// 更新图谱某点的状态的方法
        /// </summary>
        /// <param name="ProdState">状态</param>
        /// <param name="row">纵坐标</param>
        /// <param name="column">横坐标</param>
        public void ShowPRODState(int ProdState, int row, int column)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new ShowProdStateDelegate(ShowPRODState), new object[] { ProdState, row, column });
            }
            else
            {
                switch (ProdState)
                {
                    case 0://未检测
                        mappingControl1.SetDieState(row, column, "未检测");
                        break;
                    case 1://OK
                        mappingControl1.SetDieState(row, column, "合格");
                        break;
                    case 2://NG
                        mappingControl1.SetDieState(row, column, "不合格");
                        break;
                    //case 3://N2K
                    //    mappingControl1.SetDieState(row, column, "复看合格");
                    //    break;
                    //case 4://无芯片
                    //    mappingControl1.SetDieState(row, column, "无芯片");
                    //    break;
                    case 5://匹配失败
                        mappingControl1.SetDieState(row, column, "匹配失败");
                        break;
                    default:
                        break;
                }
            }
        }
        /// <summary>
        /// 颜色字典
        /// </summary>
        private Dictionary<string, Color> dic_DieState_Color;
        public void ShowProdMap()
        {
            //新建颜色字典
            dic_DieState_Color = new Dictionary<string, Color>();
            dic_DieState_Color.Add("未检测", Color.SkyBlue);
            dic_DieState_Color.Add("合格", Color.Green);
            dic_DieState_Color.Add("不合格", Color.Red);
            ////dic_DieState_Color.Add("复看合格", Color.Yellow);
            //dic_DieState_Color.Add("无芯片", Color.Purple); 
            dic_DieState_Color.Add("匹配失败", Color.Pink);
            //初始化图谱
            mappingControl1.BeginInvoke(
                new MethodInvoker(() =>
                {
                    mappingControl1.Initial(App.obj_Pdt.RowNumber,
                        App.obj_Pdt.BlockNumber * App.obj_Pdt.ColumnNumber, dic_DieState_Color, "未检测");
                }));
        }
        /// <summary>
        /// 使得三大机构界面不可用,true代表可用
        /// </summary>
        /// <param name="flag"></param>
        public void SetOtherFrm(bool flag)
        {
            frmChuckModules.Instance.Enabled = flag;
            frmLoadModules.Instance.Enabled = flag;
            frmUnloadModules.Instance.Enabled = flag;
        }
        #endregion
        //Paint paint = new Paint();
        private void tbBsp_Click(object sender, EventArgs e)
        {
            HTM.LoadUI();
        }
        private void tbStart_Click(object sender, EventArgs e)
        {
            if(App.obj_Pdt.MgzIdx==-1)
            {
                HTUi.PopError("请先选择当前产品所用料盒！");
                return;
            }
            if (App.obj_SystemConfig.Marking && App.obj_Pdt.UseMarker)
            {
                if (App.obj_Chuck.IsJetRun() == 0)
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
            if (App.obj_Vision.ScanMapPostions == null)
            {
                HTUi.PopError("无法扫描！该产品扫描点位为空！\n请在图谱生成界面生成扫描点位！");
                return;
            }
            switch (App.obj_Process.CurrentState)
            {
                case MachineState.STATE_IDLE:
                    if (HTUi.PopYesNo("是否开始？\n(Are you sure to START?)"))
                    {
                        btn_InputLotId.Enabled = false;
                        SetOtherFrm(false);
                        App.obj_Process.ProcessStart();
                        ShowProdMap();
                    }
                    break;
                case MachineState.STATE_PAUSE:
                    if (HTUi.PopYesNo("是否继续?\n(Are you sure to RESUME?)"))
                    {
                        App.obj_Process.ProcessResume();
                    }
                    break;
                default:
                    HTUi.PopWarn("当前状态中该操作不可用！\n(The operation isn't enabled in this state!)");
                    break;
            }

            if (MchineStateChangedEvent != null)
            {
                MchineStateChangedEvent(App.obj_Process.CurrentState);
            }
        }
        private void tbPause_Click(object sender, EventArgs e)
        {
            switch (App.obj_Process.CurrentState)
            {
                case MachineState.STATE_RUN:
                    if (HTUi.PopYesNo("确认暂停?\n(Are you sure to pause?)"))
                    {
                        SetOtherFrm(false);
                        btn_InputLotId.Enabled = true;
                        App.obj_Process.ProcessPause();
                    }
                    break;
                case MachineState.STATE_PAUSE:
                    HTUi.PopWarn("机器已处于暂停状态!\n(Machine is already paused!)");
                    break;
                case MachineState.STATE_ALARM:
                    HTUi.PopWarn("请处理报警信息!\n(Please handle the alarm information!)");
                    break;
                case MachineState.STATE_IDLE:
                    HTUi.PopWarn("机器处于空闲状态，当前操作无效！\n(In IDLE state,This operation is invalid!)");
                    break;
            }
            if (MchineStateChangedEvent != null)
            {
                MchineStateChangedEvent(App.obj_Process.CurrentState);
            }
        }


        private async void tbHome_Click(object sender, EventArgs e)
        {

            switch (App.obj_Process.CurrentState)
            {
                case MachineState.STATE_IDLE:
                case MachineState.STATE_PAUSE:
                    if (!HTUi.PopYesNo("确认回零！"))
                    {
                        break;
                    }
                    Form_Wait.ShowForm();
                    await Task.Run(() =>
                    {
                        if (!App.obj_Chuck.Home())
                        {
                            HTUi.PopError(App.obj_Chuck.GetLastErrorString());
                            return;
                        }
                        HTLog.Info("【检测台模块】回零成功！");
                        bool SuccessUnload = false;
                        //if (!App.obj_SystemConfig.LoadManually)
                        //{
                            Thread thread = new Thread(() =>
                            {
                                if (!App.obj_Unload.Home(false))
                                {
                                    HTUi.PopError(App.obj_Unload.GetLastErrorString());
                                    return;
                                }
                                HTLog.Info("【下料模块】回零成功！");
                                SuccessUnload = true;
                            });
                            thread.Start();
                            if (!App.obj_Load.Home(true))
                            {
                                HTUi.PopError(App.obj_Load.GetLastErrorString());
                                return;
                            }
                            HTLog.Info("【上料模块】回零成功！");
                        //}
                        DateTime t1 = DateTime.Now;
                        while (!SuccessUnload)
                        {
                            if (DateTime.Now.Subtract(t1).TotalMilliseconds > 20000)
                            {
                                return;
                            }
                            Thread.Sleep(200);
                        }
                        App.obj_Process.ProcessHome();
                        HTLog.Info("【流程】回零成功！");
                    });

                    Form_Wait.CloseForm();
                    break;
                case MachineState.STATE_RUN:
                case MachineState.STATE_ALARM:
                    HTUi.PopWarn("当前状态中该操作不可用!");
                    break;
            }

            if (MchineStateChangedEvent != null)
            {
                MchineStateChangedEvent(App.obj_Process.CurrentState);
            }
        }


        private void tbClearAlarm_Click(object sender, EventArgs e)
        {
            if (lbxAlarm.Items.Count == 0)
            {
                HTUi.TipHint("目前暂无报警信息！");
                return;
            }
            lbxAlarm.Items.Remove(lbxAlarm.Items[0]);
            if (lbxAlarm.Items.Count == 0)
            {
                App.obj_Process.ClearAlarm();
            }
            if (MchineStateChangedEvent != null)
            {
                MchineStateChangedEvent(App.obj_Process.CurrentState);
            }
        }

        private void tbStop_Click(object sender, EventArgs e)
        {
            if (HTUi.PopYesNo("Are you sure to STOP ?"))
            {
                SetOtherFrm(true);
                btn_InputLotId.Enabled = true ;
                if (lbxAlarm.Items.Count != 0)
                {
                    lbxAlarm.Items.Remove(lbxAlarm.Items[0]);
                }
                App.obj_Process.ProcessStop();
            }
            if (MchineStateChangedEvent != null)
            {
                MchineStateChangedEvent(App.obj_Process.CurrentState);
            }
        }

        private void tbLot_Click(object sender, EventArgs e)
        {
            if (App.obj_SystemConfig.LoadManually)
            {
                HTUi.PopError("手动上下料时结批不可用！");
                return;
            }
            if (App.obj_Process.CurrentState == MachineState.STATE_IDLE || App.obj_Process.CurrentState == MachineState.STATE_ALARM)
            {
                HTUi.PopError("报警状态或空闲状态时结批不可用!");
                return;
            }
            if (App.obj_Process.EndCurrentLot)
            {
                HTUi.PopError("正处于结批状态请勿重复！");
                return;
            }
            if (!HTUi.PopYesNo("确认结批！"))
            {
                return;
            }
            App.obj_Process.EndLot();
            btn_InputLotId.Enabled = true;
        }

        private void btnMute_Click(object sender, EventArgs e)
        {
            if (App.obj_Main.MuteON == false)
            {
                App.obj_Main.MuteON = true;
                this.btnMute.Text = "静音";
                btnMute.Image = ilst1.Images[1];
            }
            else
            {
                App.obj_Main.MuteON = false;
                this.btnMute.Text = "峰鸣";
                btnMute.Image = ilst1.Images[0];
            }
        }



        private void cbBox_ImgSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cbBox_ImgSelect.Items.Count != App.obj_ImageInformSet.Count)
                    RefreshImageList();
                if (cbBox_ImgSelect.SelectedIndex == -1) return;
                if (ImageOrigin == null) return;
                if (!ImageOrigin.IsInitialized()) return;
                if (ImageOrigin != null && ImageOrigin.IsInitialized())
                {
                    if (App.obj_Vision.ListImgRegion != null)
                        htWindow.Region = App.obj_Vision.ListImgRegion[cbBox_ImgSelect.SelectedIndex];
                    ShowSelectImageChannel(ImageOrigin.SelectObj(cbBox_ImgSelect.SelectedIndex + 1));
                    SelectImgIdx = cbBox_ImgSelect.SelectedIndex;
                }
            }
            catch (Exception exp)
            {
                HTUi.PopError(exp.Message);
                if (cbBox_ImgSelect.Items.Count > SelectImgIdx) cbBox_ImgSelect.SelectedIndex = SelectImgIdx;
            }
        }

        private void cbBox_ChannelSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cbBox_ChannelSelect.SelectedIndex == -1) return;
                SelectChannelIdx = cbBox_ChannelSelect.SelectedIndex;
                if (ImageOrigin == null) return;
                if (!ImageOrigin.IsInitialized()) return;
                if (ImageOrigin != null && ImageOrigin.IsInitialized())
                {
                    if (App.obj_Vision.ListImgRegion != null)
                        htWindow.Region = App.obj_Vision.ListImgRegion[SelectImgIdx];
                    ShowSelectImageChannel(ImageOrigin.SelectObj(SelectImgIdx + 1));
                }
            }
            catch (Exception exp)
            {
                HTUi.PopError(exp.Message);
                if (cbBox_ChannelSelect.Items.Count != 0) cbBox_ChannelSelect.SelectedIndex = 0;
            }
        }

        private async void btnSwichPdtFrm_Click(object sender, EventArgs e)
        {
            String pdt = "";
            if (!MSG.Inputbox("Switch Pdt", "请输入产品名(Switch Pdt)", out pdt))
            {
                return;
            }
            if(pdt == "")
            {
                HTUi.PopError("未输入产品名.");
                HTLog.Info("未输入产品名.");
                return;
            }
            var ListPdt = App.obj_Pdt.GetProductList();
            if (!ListPdt.Contains(pdt))
            {
                HTUi.PopError("产品列表中不包括" + pdt + ".");
                HTLog.Info("产品列表中不包括"+ pdt + ".");
                return;
            }

            App.obj_Pdt.SaveActivePdtName(pdt);
            App.obj_Pdt.LoadPdtData();
            App.obj_Pdt.ConfigPdtData();
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
            frmProduct.Instance.RefreshPdtUI();
            Form_Wait.CloseForm();
        }

        private void btn_InputLotId_Click(object sender, EventArgs e)
        {

            if (App.obj_SystemConfig.LotMode == 1)
            {
                if (App.obj_SystemConfig.LotIdMode == 0)
                {
                    Form_InputLotId.ShowDialogForm();
                    if (Form_InputLotId.Instance.DialogResult == DialogResult.Cancel)
                    {
                        return;
                    }
                }
                else if (App.obj_SystemConfig.LotIdMode == 1)
                {
                    App.obj_Process.InitialNewLot();
                }
            }
            else
                return;
            FormJobs.Instance.label5.Invoke(new Action(() => { FormJobs.Instance.label5.Text = "当前产品:" + ProductMagzine.ActivePdt + "  流程状态:待机  批次号:" +App.obj_Process.LotId; }));
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void frmJobs_Load(object sender, EventArgs e)
        {
            try
            {
                paneltemp1 = new Panel();
                paneltemp1.Location = new Point(this.htWindow.Location.X + 32 + (this.htWindow.Size.Width - 32) / 2 - 1, this.htWindow.Location.Y + this.htWindow.Size.Height / 2 - 40);
                paneltemp1.Size = new Size(2, 80);
                paneltemp1.Enabled = false;
                paneltemp1.BackColor = Color.Green;
                this.htWindow.Controls.Add(paneltemp1);
                paneltemp1.BringToFront();
                paneltemp2 = new Panel();
                paneltemp2.Location = new Point(this.htWindow.Location.X + 32 + (this.htWindow.Size.Width - 32) / 2 - 40, this.htWindow.Location.Y + this.htWindow.Size.Height / 2 - 1);
                paneltemp2.Size = new Size(80, 2);
                paneltemp2.Enabled = false;
                paneltemp2.BackColor = Color.Green;
                this.htWindow.Controls.Add(paneltemp2);
                paneltemp2.BringToFront();
            }
            catch (Exception EXP)
            {
                HTLog.Error(EXP.Message);
            }
        }
        
    }
}