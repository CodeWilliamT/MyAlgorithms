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
using HT_Lib;
using DevComponents.DotNetBar.Controls;



namespace LeadframeAOI
{
    public partial class frmLoadModules : Form
    {
        public static frmLoadModules Instance = null;
        private delegate int boolIntPara(bool left, Int16 version);
        private delegate int boolPara(bool left);
        private delegate bool methodBoolIntPara(bool left, Int16 version);
        private delegate bool methodBoolPara(bool left);
        

        public frmLoadModules()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.None;
            this.FormBorderStyle = FormBorderStyle.None;
            Instance = this;
        }
        public void SetupUI()
        {
            //tbLoadMgzMaxPos.Text = App.obj_Load.upLoadPos
            //tbBtmPushMgzSafePos.Text = App.obj_Load.y_BtmPushMgzSafePos.ToString("F2");

            //tbUnLoadMgzPushOverPos.Text = App.obj_Load.y_UnLoadMgzPushOverPos.ToString("F2");

            //tbYLoadUnLoadMgzPos.Text = App.obj_Load.y_LoadMgzPos.ToString("F2");

            //tbZLoadUnLoadMgzPos.Text = App.obj_Load.z_LoadMgzPos.ToString("F2");

            //tbYUnLoadMgzPos.Text = App.obj_Load.y_UnLoadMgzPos.ToString("F2");

            //tbZUnLoadMgzPos.Text = App.obj_Load.z_UnLoadMgzPos.ToString("F2");

            //tbYLoadUnLoadFramePos.Text = App.obj_Load.y_LoadUnLoadFramePos.ToString("F2");

            //tbJawOpenedPos.Text = App.obj_Load.z_JawOpenedPos.ToString("F2");

            //tbJawClosedPos.Text = App.obj_Load.z_JawClosedPos.ToString("F2");

            //tbZLoadUnLoadFramePos.Text = App.obj_Load.z_LoadUnLoadFramePos.ToString("F2");

            //tbTransfeJawLeftSafeCstPos.Text = App.obj_Load.x_TransfeJawLeftSafeCstPos.ToString("F2");

            //tbTransfeJawRightSafeCstPos.Text = App.obj_Load.x_TransfeJawRightSafeCstPos.ToString("F2");


            //tbPushRodSafePos.Text = App.obj_Load.x_PushRodSafePos.ToString("F2");

            //tbPushRodPushOverPos.Text = App.obj_Load.x_PushRodPushOverPos.ToString("F2");

            propertyGrid1.SelectedObject = App.obj_Load;

        }
        /// <summary>
        /// 使得界面控件使能或不使能
        /// </summary>
        /// <param name="enable"></param>
        private void SetButtonEnadble(bool enable)
        {
            foreach (Control ctrl in tableLayoutPanel2.Controls)
            {
                ctrl.Enabled = enable;
            }
        }
        private void Scan_SavePara_Click(object sender, EventArgs e)
        {
            //try
            //{

            //    App.obj_Load.y_BtmPushMgzSafePos = Convert.ToDouble(tbBtmPushMgzSafePos.Text);

            //    App.obj_Load.y_UnLoadMgzPushOverPos = Convert.ToDouble(tbUnLoadMgzPushOverPos.Text);

            //    App.obj_Load.y_LoadMgzPos = Convert.ToDouble(tbYLoadUnLoadMgzPos.Text);

            //    App.obj_Load.z_LoadMgzPos = Convert.ToDouble(tbZLoadUnLoadMgzPos.Text);

            //    App.obj_Load.y_UnLoadMgzPos = Convert.ToDouble(tbYUnLoadMgzPos.Text);

            //    App.obj_Load.z_UnLoadMgzPos = Convert.ToDouble(tbZUnLoadMgzPos.Text);

            //    App.obj_Load.y_LoadUnLoadFramePos = Convert.ToDouble(tbYLoadUnLoadFramePos.Text);

            //    App.obj_Load.z_JawOpenedPos = Convert.ToDouble(tbJawOpenedPos.Text);

            //    App.obj_Load.z_JawClosedPos = Convert.ToDouble(tbJawClosedPos.Text);

            //    App.obj_Load.z_LoadUnLoadFramePos = Convert.ToDouble(tbZLoadUnLoadFramePos.Text);

            //    App.obj_Load.x_TransfeJawLeftSafeCstPos = Convert.ToDouble(tbTransfeJawLeftSafeCstPos.Text);

            //    App.obj_Load.x_TransfeJawRightSafeCstPos = Convert.ToDouble(tbTransfeJawRightSafeCstPos.Text);

                
            //    App.obj_Load.x_PushRodSafePos = Convert.ToDouble(tbPushRodSafePos.Text); 
               
            //    App.obj_Load.x_PushRodPushOverPos = Convert.ToDouble(tbPushRodPushOverPos.Text); 
       

            //}
            //catch (Exception exp)
            //{
            //    HTUi.PopError(exp.ToString());
            //    return;
            //}
            if (!App.obj_Load.Save())
            {
                HTUi.PopError("保存参数失败！");
            }
            HTUi.TipHint("保存参数成功！");

        }

        private void btnCGetLoadX_Click(object sender, EventArgs e)
        {
            App.obj_Chuck.GetXLoadPos();
        }
        private void btnCGetUnloadX_Click(object sender, EventArgs e)
        {
            App.obj_Chuck.GetXUnloadPos();
        }
        private async void btnLoadHome_Click(object sender, EventArgs e)
        {
            
            Form_Wait.ShowForm();
            bool result = await Task.Run(() =>
            {
                return App.obj_Load.Home(true);
            });
            Form_Wait.CloseForm();
            if (result)
            {
                HTUi.TipHint("【上料模块】回零成功!");
                HTLog.Info("【上料模块】回零成功!");
            }
            else
            {
                HTUi.PopError(String.Format("【上料模块】回零失败!详细信息:{0}.", App.obj_Load.GetLastErrorString()));
            }
        }

        private async void btnLoadMgz_Click(object sender, EventArgs e)
        {
            if (!App.obj_Load.DoLoadMgz)
            {
                btnLoadMgz.Text = "停止动作";
                SetButtonEnadble(false);
                btnLoadMgz.Enabled = true;
                int result = await Task.Run(() =>
                {
                    return App.obj_Load.LoadMagezine(true); ;
                });

                SetButtonEnadble(true);
                App.mainWin.Enabled = true;
                btnLoadMgz.Text = "上料盒";
                if (result == 0)
                {
                    HTUi.TipHint("上料盒成功!");
                }
                else if (result == -1)
                {
                    HTUi.PopError(String.Format("上料盒失败!详细信息:{0}.", App.obj_Load.GetLastErrorString()));
                }
                else if (result == -2)
                {
                    HTUi.TipHint(String.Format("无料盒可用，请添加料盒!"));
                }
            }
            else
            {
                App.obj_Load.DoLoadMgz = false;
                btnLoadMgz.Text = "等待结束";
            }
        }


        private async void btnUnloadMgz_Click(object sender, EventArgs e)
        {
            Form_Wait.ShowForm();
            int result = await Task.Run(() =>
            {
                return App.obj_Load.UnloadMagezine(true); ;
            });
            Form_Wait.CloseForm();
            if (result == 0)
            {
                HTUi.TipHint("下料盒成功!");
            }
            else if (result == -1)
            {
                HTUi.PopError(String.Format("下料盒失败!详细信息:{0}.", App.obj_Load.GetLastErrorString()));
            }
            else if (result == -2)
            {
                HTUi.TipHint(String.Format("空料盒仓已满，请清空料盒仓!"));
            }
        }

        private async void btnLoadFrame_Click(object sender, EventArgs e)
        {
            try
            {
               
                int index = Convert.ToInt32(numLIndex.Value);
                Form_Wait.ShowForm();
                int result = await Task.Run(() =>
                {
                    
                    return App.obj_Load.LoadFrameFromMgz(index);
                });
                Form_Wait.CloseForm();
                if (result == -1)
                {
                    HTUi.PopError(App.obj_Load.GetLastErrorString());
                }
                else if(result == 0)
                {
                    HTUi.TipHint("操作成功");
                }
                else if (result == -2)
                {
                    HTUi.TipHint("该层无料片");
                }
            }
            catch (Exception exp)
            {
                HTUi.TipHint(exp.Message);
            }
        }

        private async void btnTransfer2Waitpos_Click(object sender, EventArgs e)
        {
            Form_Wait.ShowForm();
            bool result = await Task.Run(() =>
            {
                return App.obj_Load.TransferFrame2WaitPos();
            });
            Form_Wait.CloseForm();

            if (!result)
            {
                HTUi.PopError(App.obj_Load.GetLastErrorString());
            }
            else
            {
                HTUi.TipHint("操作成功");
            }
        }

        private async void btnTransfer2Loadpos_Click(object sender, EventArgs e)
        {

            Form_Wait.ShowForm();
            bool result = await Task.Run(() =>
            {
                return App.obj_Load.TransferFrame2LoadPos();
            });
            Form_Wait.CloseForm();

            if (!result)
            {
                HTUi.PopError(App.obj_Load.GetLastErrorString());
            }
            else
            {
                HTUi.TipHint("操作成功");
            }
        }


        private async void btnXLeft_Click(object sender, EventArgs e)
        {
            try
            {
                Form_Wait.ShowForm();
                bool result = await Task.Run(() =>
                {
                    return App.obj_Load.PushRodLeft();
                });
                Form_Wait.CloseForm();

                if (!result)
                {
                    HTUi.PopError(App.obj_Load.GetLastErrorString());
                }
                else
                {
                    HTUi.TipHint("操作成功");
                }
            }
            catch (Exception exp)
            {
                HTUi.TipHint(exp.Message);
            }
        }



        private void btnXRight_Click(object sender, EventArgs e)
        {
            try
            {
                Boolean ret = true;

                ret = App.obj_Load.PushRodRight(20);

                if (ret == false)
                {
                    HTUi.PopError(App.obj_Load.GetLastErrorString());
                }
                else
                {
                    HTUi.TipHint("操作成功");
                }
            }
            catch (Exception exp)
            {
                HTUi.TipHint(exp.Message);
            }
        }

        private void btnTool_Click(object sender, EventArgs e)
        {
            if (HTM.LoadUI() < 0)
            {
                HTUi.PopError("打开轴调试助手界面失败");
            }
        }

        private async void btnOpenJaw_Click(object sender, EventArgs e)
        {
            Form_Wait.ShowForm();
            bool result = await Task.Run(() =>
            {
                return App.obj_Load.OpenJaw();
            });
            Form_Wait.CloseForm();

            if (!result)
            {
                HTUi.PopError(App.obj_Load.GetLastErrorString());
            }
            else
            {
                HTUi.TipHint("操作成功");
            }
        }

        private async void btnCloseJaw_Click(object sender, EventArgs e)
        {
            Form_Wait.ShowForm();
            bool result = await Task.Run(() =>
            {
                return App.obj_Load.CloseJaw();
            });
            Form_Wait.CloseForm();

            if (!result)
            {
                HTUi.PopError(App.obj_Load.GetLastErrorString());
            }
            else
            {
                HTUi.TipHint("操作成功");
            }
        }

        private async void btnOpenMgz_Click(object sender, EventArgs e)
        {
            Form_Wait.ShowForm();
            bool result = await Task.Run(() =>
            {
                return App.obj_Load.OpenMgzJaw();
            });
            Form_Wait.CloseForm();

            if (!result)
            {
                HTUi.PopError(App.obj_Load.GetLastErrorString());
            }
            else
            {
                HTUi.TipHint("操作成功");
            }
        }

        private async void btnCloseMgz_Click(object sender, EventArgs e)
        {
            Form_Wait.ShowForm();
            bool result = await Task.Run(() =>
            {
                return App.obj_Load.CloseMgzJaw();
            });
            Form_Wait.CloseForm();

            if (!result)
            {
                HTUi.PopError(App.obj_Load.GetLastErrorString());
            }
            else
            {
                HTUi.TipHint("操作成功");
            }
        }
    }
}
