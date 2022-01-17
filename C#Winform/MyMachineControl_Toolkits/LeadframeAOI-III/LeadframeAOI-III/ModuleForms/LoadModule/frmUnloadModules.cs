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

namespace LeadframeAOI
{
    public partial class frmUnloadModules : Form
    {
        public static frmUnloadModules Instance = null;

        private delegate int boolIntPara(bool left, Int16 version);
        private delegate int boolPara(bool left);

        private delegate bool methodBoolIntPara(bool left, Int16 version);
        private delegate bool methodBoolPara(bool left);
        public frmUnloadModules()
        {

            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.None;
            this.FormBorderStyle = FormBorderStyle.None;
      //      SetupUI();
            Instance = this;
        }
        public void SetupUI()
        {
            //tbTransfeRightJawPushCstPos.Text = App.obj_Unload.x_TransfeRightJawPushCstPos.ToString("F2");

            //tbTransfeRightJawPushOverCstPos.Text = App.obj_Unload.x_TransfeRightJawPushOverCstPos.ToString("F2");

            //tbBtmPushMgzSafePos.Text = App.obj_Unload.y_BtmPushMgzSafePos.ToString("F2");

            //tbUnLoadMgzPushOverPos.Text = App.obj_Unload.y_UnLoadMgzPushOverPos.ToString("F2");

            //tbYLoadUnLoadMgzPos.Text = App.obj_Unload.y_LoadMgzPos.ToString("F2");

            //tbZLoadUnLoadMgzPos.Text = App.obj_Unload.z_LoadMgzPos.ToString("F2");

            //tbYUnLoadMgzPos.Text = App.obj_Unload.y_UnLoadMgzPos.ToString("F2");

            //tbZUnLoadMgzPos.Text = App.obj_Unload.z_UnLoadMgzPos.ToString("F2");

            //tbYLoadUnLoadFramePos.Text = App.obj_Unload.y_LoadUnLoadFramePos.ToString("F2");

            //tbJawOpenedPos.Text = App.obj_Unload.z_JawOpenedPos.ToString("F2");

            //tbJawClosedPos.Text = App.obj_Unload.z_JawClosedPos.ToString("F2");

            //tbZLoadUnLoadFramePos.Text = App.obj_Unload.z_LoadUnLoadFramePos.ToString("F2");

            //tbTransfeJawLeftSafeCstPos.Text = App.obj_Unload.x_TransfeJawLeftSafeCstPos.ToString("F2");

            //tbTransfeJawRightSafeCstPos.Text = App.obj_Unload.x_TransfeJawRightSafeCstPos.ToString("F2");

            //tbYUnLoadNgFramePos.Text = App.obj_Unload.y_UnLoadNgFramePos.ToString("F2");

            propertyGrid1.SelectedObject = App.obj_Unload;


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

            //    App.obj_Unload.x_TransfeRightJawPushCstPos = Convert.ToDouble(tbTransfeRightJawPushCstPos.Text);

            //    App.obj_Unload.x_TransfeRightJawPushOverCstPos = Convert.ToDouble(tbTransfeRightJawPushOverCstPos.Text);

            //    App.obj_Unload.y_BtmPushMgzSafePos = Convert.ToDouble(tbBtmPushMgzSafePos.Text);

            //    App.obj_Unload.y_UnLoadMgzPushOverPos = Convert.ToDouble(tbUnLoadMgzPushOverPos.Text);

            //    App.obj_Unload.y_LoadMgzPos = Convert.ToDouble(tbYLoadUnLoadMgzPos.Text);

            //    App.obj_Unload.z_LoadMgzPos = Convert.ToDouble(tbZLoadUnLoadMgzPos.Text);

            //    App.obj_Unload.y_UnLoadMgzPos = Convert.ToDouble(tbYUnLoadMgzPos.Text);

            //    App.obj_Unload.z_UnLoadMgzPos = Convert.ToDouble(tbZUnLoadMgzPos.Text);

            //    App.obj_Unload.y_LoadUnLoadFramePos = Convert.ToDouble(tbYLoadUnLoadFramePos.Text);

            //    App.obj_Unload.z_JawOpenedPos = Convert.ToDouble(tbJawOpenedPos.Text);

            //    App.obj_Unload.z_JawClosedPos = Convert.ToDouble(tbJawClosedPos.Text);

            //    App.obj_Unload.z_LoadUnLoadFramePos = Convert.ToDouble(tbZLoadUnLoadFramePos.Text);

            //    App.obj_Unload.x_TransfeJawLeftSafeCstPos = Convert.ToDouble(tbTransfeJawLeftSafeCstPos.Text);

            //    App.obj_Unload.x_TransfeJawRightSafeCstPos = Convert.ToDouble(tbTransfeJawRightSafeCstPos.Text);
                
            //    App.obj_Unload.y_UnLoadNgFramePos = Convert.ToDouble(tbYUnLoadNgFramePos.Text);
            //}
            //catch (Exception exp)
            //{
            //    HTUi.PopError(exp.ToString());
            //    return;
            //}

           
            if (!App.obj_Unload.Save())
            {
                HTUi.PopError("保存参数失败！");
            }
            
            HTUi.TipHint("保存参数成功！");

        }


        private async void btnUnloadHome_Click(object sender, EventArgs e)
        {
            Form_Wait.ShowForm();
            bool result = await Task.Run(() =>
            {
                return App.obj_Unload.Home(false);
            });
            Form_Wait.CloseForm();
            if (result)
            {
                HTUi.TipHint("【下料模块】回零成功!");
                HTLog.Info("【下料模块】回零成功!");
            }
            else
            {
                HTUi.PopError(String.Format("【下料模块】回零失败!详细信息:{0}.", App.obj_Unload.GetLastErrorString()));
            }
        }

        private async void btnLoadMgz_Click(object sender, EventArgs e)
        {


            if (!App.obj_Unload.DoLoadMgz)
            {
                btnLoadMgz.Text = "停止动作";
                SetButtonEnadble(false);
                btnLoadMgz.Enabled = true;
                int result = await Task.Run(() =>
                {
                    return App.obj_Unload.LoadMagezine(true); ;
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
                    HTUi.PopError(String.Format("上料盒失败!详细信息:{0}.", App.obj_Unload.GetLastErrorString()));
                }
                else if (result == -2)
                {
                    HTUi.TipHint(String.Format("无料盒可用，请添加料盒!"));
                }
            }
            else
            {
                App.obj_Unload.DoLoadMgz = false;
                App.mainWin.Enabled = false;
                btnLoadMgz.Text = "等待结束";
            }
        }

        private async void btnULoadFrame_Click(object sender, EventArgs e)
        {
            Form_Wait.ShowForm();
            bool result = await Task.Run(() =>
            {
                return App.obj_Unload.UnloadFrameToMgz();
            });
            Form_Wait.CloseForm();
            if (result)
            {
                HTUi.TipHint("推片到料盒成功!");
            }
            else
            {
                HTUi.PopError(String.Format("推片到料盒失败!详细信息:{0}.", App.obj_Unload.GetLastErrorString()));
            }
        }

        private async void btnUnloadMgz_Click(object sender, EventArgs e)
        {
            Form_Wait.ShowForm();
            int result = await Task.Run(() =>
            {
                return App.obj_Unload.UnloadMagezine(false); 
            });
            Form_Wait.CloseForm();
            if (result == 0)
            {
                HTUi.TipHint("下料盒成功!");
            }
            else if (result == -1)
            {
                HTUi.PopError(String.Format("下料盒失败!详细信息:{0}.", App.obj_Unload.GetLastErrorString()));
            }
            else if (result == -2)
            {
                HTUi.TipHint(String.Format("料盒仓已满，请清空料盒仓!"));
            }
        }

        private void btnTool_Click(object sender, EventArgs e)
        {
            if (HTM.LoadUI() < 0)
            {
                HTUi.PopError("打开轴调试助手界面失败");
            }
        }

        private async void btnCloseNgMgzJaw_Click(object sender, EventArgs e)
        {
            Form_Wait.ShowForm();
            bool result = await Task.Run(() =>
            {
                return App.obj_Unload.NgMgzJawCylinderFunc(0);
            });
            Form_Wait.CloseForm();

            if (result)
            {
                HTUi.TipHint("夹紧成功!");
            }
            else
            {
                HTUi.PopError(String.Format("夹紧失败!详细信息:{0}.", App.obj_Unload.GetLastErrorString()));
            }
        }

        private async void btnOpenNgMgzJaw_Click(object sender, EventArgs e)
        {
            Form_Wait.ShowForm();
            bool result = await Task.Run(() =>
            {
                return App.obj_Unload.NgMgzJawCylinderFunc(1);
            });
            Form_Wait.CloseForm();

            if (result)
            {
                HTUi.TipHint("打开成功!");
            }
            else
            {
                HTUi.PopError(String.Format("打开失败!详细信息:{0}.", App.obj_Unload.GetLastErrorString()));
            }
        }

        private async void btnMoveMgz_Click(object sender, EventArgs e)
        {
            int index = Convert.ToInt32(numOKIdx.Value);

            Form_Wait.ShowForm();
            bool result = await Task.Run(() =>
            {
                return App.obj_Unload.UnlaodMgzMove2ReceiveFrame(index, true);
            });
            Form_Wait.CloseForm();

            if (result)
            {
                HTUi.TipHint(String.Format("良品料盒移动到{0}层成功.", index));
            }
            else
            {
                HTUi.PopError(String.Format("良品料盒移动到{0}层失败!详细信息:{1}.", index, App.obj_Unload.GetLastErrorString()));
            }

        }

        private async void btnMoveNgMgz_Click(object sender, EventArgs e)
        {
            int index = Convert.ToInt32(numNGIdx.Value);

            Form_Wait.ShowForm();
            bool result = await Task.Run(() =>
            {
                return App.obj_Unload.UnlaodMgzMove2ReceiveFrame(index,  false);
            });
            Form_Wait.CloseForm();

            if (result)
            {
                HTUi.TipHint(String.Format("NG料盒移动到{0}层成功.", index));
            }
            else
            {
                HTUi.PopError(String.Format("NG料盒移动到{0}层失败!详细信息:{1}.", index, App.obj_Unload.GetLastErrorString()));
            }
        }

        private async void btnUnloadFrame2Right_Click(object sender, EventArgs e)
        {
            Form_Wait.ShowForm();
            bool result = await Task.Run(() =>
            {
                return App.obj_Unload.TransferFrameToMgz();
            });
            Form_Wait.CloseForm();

            if (result)
            {
                HTUi.TipHint("传片到最右端成功!");
            }
            else
            {
                HTUi.PopError(String.Format("传片到最右端失败!详细信息:{0}.", App.obj_Unload.GetLastErrorString()));
            }
        }

        private async void btnUnloadFrame2Wait_Click(object sender, EventArgs e)
        {
            Form_Wait.ShowForm();
            bool result = await Task.Run(() =>
            {
                return App.obj_Unload.UnloadMove2Wait();
            });
            Form_Wait.CloseForm();

            if (result)
            {
                HTUi.TipHint("传片到等待位成功!");

               
            }
            else
            {
                HTUi.PopError(String.Format("传片到等待位失败!详细信息:{0}.", App.obj_Unload.GetLastErrorString()));
            }
        }

        private void tbNgLIndex_TextChanged(object sender, EventArgs e)
        {

        }

        private async void btnOpenJaw_Click(object sender, EventArgs e)
        {
            Form_Wait.ShowForm();
            bool result = await Task.Run(() =>
            {
                return App.obj_Unload.OpenJaw();
            });
            Form_Wait.CloseForm();

            if (!result)
            {
                HTUi.PopError(App.obj_Unload.GetLastErrorString());
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
                return App.obj_Unload.CloseJaw();
            });
            Form_Wait.CloseForm();

            if (!result)
            {
                HTUi.PopError(App.obj_Unload.GetLastErrorString());
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
                return App.obj_Unload.OpenMgzJaw();
            });
            Form_Wait.CloseForm();

            if (!result)
            {
                HTUi.PopError(App.obj_Unload.GetLastErrorString());
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
                return App.obj_Unload.CloseMgzJaw();
            });
            Form_Wait.CloseForm();

            if (!result)
            {
                HTUi.PopError(App.obj_Unload.GetLastErrorString());
            }
            else
            {
                HTUi.TipHint("操作成功");
            }
        }
    }
}
