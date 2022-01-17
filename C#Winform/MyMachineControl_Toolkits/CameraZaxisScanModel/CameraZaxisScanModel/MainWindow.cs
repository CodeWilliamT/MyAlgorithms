#define HTNetOnline //HTM是否处于在线模式
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HalconDotNet;
using HTHalControl;
using System.Threading;
using HTM_BSP;

namespace CameraZaxisScanModel
{
    /// <summary>
    /// Edit By TWL
    /// </summary>
    public partial class MainWindow : Form
    {
        public MainWindow()
        {
            InitializeComponent();
            //初始化视觉模块
            try
            {
                //视觉模块初始化
                Program.obj_Vision.Initialize();
                LOG("视觉模块初始化完成");

                //加载视觉模块界面
                obj_CameraUI = new Obj_CameraUI();
                obj_CameraUI.TopLevel = false;
                panel1.Controls.Add(obj_CameraUI);
                obj_CameraUI.Show();

                //*********************************
                //轴运动模块初始化
#if HTNetOnline
                AxisPart.Initialize(AxisPart.HTMOnline_mode.ON);
                textBox_NowPos.Text = AxisPart.Get_Z_NowPos().ToString();//当前轴位置
#else
                AxisPart.Initialize(AxisPart.HTMOnline_mode.OFF);
                textBox_NowPos.Text = "0.0000";//当前轴位置
#endif
                LOG("轴运动模块初始化完成");
                numericUpDown_Start.Value = (decimal)AxisPart.Pos_maxLimit;
                numericUpDown_End.Value = (decimal)AxisPart.Pos_minLimit;
            }
            catch(Exception ex)
            {
                LOG("初始化失败");
                MessageBox.Show("初始化失败\n" + ex.Message);
            }
        }

        Obj_CameraUI obj_CameraUI = null;
        bool isMove = false;

        //显示图像
        private delegate void ShowImageDelegate(HTWindowControl htWindow, HObject image, HObject region);
        public void ShowImage(HTWindowControl htWindow, HObject image, HObject region)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new ShowImageDelegate(ShowImage), new object[] { htWindow, image, region });
            }
            else
            {
                htWindow.ColorName = "red";
                htWindow.SetInteractive(false);
                htWindow.RefreshWindow(image, region, "");//可以不显示区域
                htWindow.SetInteractive(true);
                htWindow.ColorName = "green";
            }
        }
        /// <summary>
        /// 用来描述调试信息,描述轴运动，ByTWL
        /// </summary>
        /// <param name="str"></param>
        public delegate void LOGInfoDelegate(string str);
        public void LOG(string str)
        {
            if (listBox1.InvokeRequired == true)
            {
                LOGInfoDelegate d = new LOGInfoDelegate(LOG);
                this.Invoke(d, str);
            }
            else
            {
                listBox1.Items.Add(DateTime.Now.ToLongTimeString() + ":" + str);
                listBox1.TopIndex = listBox1.Items.Count - 1;
                listBox1.ClearSelected();
            }
        }
        private void button_Up_Click(object sender, EventArgs e)
        {
            button_Up.Enabled = false;
            try
            {
                //如果可移动
                //*********************************
                //轴向负方向移X个脉冲位
                AxisPart.Z_RSMove((double)(numericUpDown_distance.Value));
                LOG("上移" + numericUpDown_distance.Value.ToString() + "个脉冲位");
                textBox_NowPos.Text = AxisPart.Get_Z_NowPos().ToString();
                //textBox_NowPos.Text = (Convert.ToDecimal(textBox_NowPos.Text) + numericUpDown_distance.Value).ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("上移失败\n" + ex.Message);
            }
            finally
            {
                button_Up.Enabled = true;
            }
        }

        private void button_Down_Click(object sender, EventArgs e)
        {
            button_Down.Enabled = false;
            try
            {
                //如果可移动
                //*********************************
                //轴向正方向移X个脉冲位
                AxisPart.Z_RSMove((double)(-numericUpDown_distance.Value));
                LOG("下移" + numericUpDown_distance.Value.ToString() + "个脉冲位"); 
                textBox_NowPos.Text = AxisPart.Get_Z_NowPos().ToString();
                //textBox_NowPos.Text = (Convert.ToDecimal(textBox_NowPos.Text) - numericUpDown_distance.Value).ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("下移失败\n" + ex.Message);
            }
            finally
            {
                button_Down.Enabled = true;
            }
        }

        private void button_Move_Click(object sender, EventArgs e)
        {
            //*********************************
            //轴线判定轴是否可移动至起点以及终点，如果不能，给出错误信息，renturn

            Thread thread_move;
            if (isMove)
            {
                isMove = !isMove;
                button_Move.Text = "开始移动";
            }
            else
            {

                if (!Program.obj_Vision.obj_camera[Obj_Camera.SelectedIndex].camera.IsSoftwareTrigger)
                {
                    MessageBox.Show("当前相机为硬触发模式，请切换为软触发！");
                    return;
                }
                isMove = true;
                button_Move.Text = "停止移动";
                thread_move = new Thread(() =>
                {
                    try
                    {
                        //*********************************
                        //轴移动到起点
                        AxisPart.Z_Move((double)(numericUpDown_Start.Value));
                        LOG("移动到起点" + numericUpDown_Start.Value.ToString() + "脉冲位");
                        textBox_NowPos.BeginInvoke(new MethodInvoker(() => textBox_NowPos.Text = AxisPart.Get_Z_NowPos().ToString()));
                        Thread.Sleep((int)numericUpDown_Delay.Value);
                        int Movetimes;
                        int lastDistance;
                        double nowPos = (double)numericUpDown_Start.Value;
                        if (numericUpDown_Start.Value < numericUpDown_End.Value)//若轴向正方向移动
                        {
                            Movetimes = (int)((numericUpDown_End.Value - numericUpDown_Start.Value) / numericUpDown_distance.Value);
                            lastDistance = (int)(numericUpDown_End.Value - numericUpDown_distance.Value * (decimal)Movetimes - numericUpDown_Start.Value);
                            for (int i = 0; i < Movetimes; i++)
                            {
                                //*********************************
                                //轴向正方向移动一个步进距离
                                AxisPart.Z_Move((double)(nowPos + Convert.ToDouble(numericUpDown_distance.Value)));
                                LOG("轴位置从" + nowPos.ToString() + " 到 " + (nowPos + Convert.ToDouble(numericUpDown_distance.Value)).ToString());

                                
                                //当前轴位置
                                nowPos = nowPos + Convert.ToDouble(numericUpDown_distance.Value);//建议直接获取轴位置
                                textBox_NowPos.BeginInvoke(new MethodInvoker(() => textBox_NowPos.Text = AxisPart.Get_Z_NowPos().ToString()));
                                if (checkBox_SavePic.Checked)
                                {
                                    Thread.Sleep(50);
                                    Program.obj_Vision.Acq = Program.obj_Vision.obj_camera[Obj_Camera.SelectedIndex].Snap(1, 5000);
                                    switch (Program.obj_Vision.Acq.GrabStatus)
                                    {
                                        case "GrabPass":
                                            obj_CameraUI.Image.Dispose();
                                            obj_CameraUI.Image = Program.obj_Vision.Acq.Image.CopyObj(1, -1);
                                            ShowImage(htWindow, obj_CameraUI.Image, null);
                                            HOperatorSet.WriteImage(obj_CameraUI.Image, "tiff", 0, Program.obj_Vision.obj_camera[Obj_Camera.SelectedIndex].cameraPath+"\\" + i.ToString()+".tiff");
                                            Program.obj_Vision.Acq.Image.Dispose();
                                            LOG("相机Camera_" + Obj_Camera.SelectedIndex.ToString() + " 拍照,存图于:\n" + Program.obj_Vision.obj_camera[Obj_Camera.SelectedIndex].cameraPath);
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                Thread.Sleep((int)numericUpDown_Delay.Value);
                                if (!isMove) return;//若请求停止
                            }
                            if (lastDistance != 0)//移动最后一段距离
                            {
                                //*********************************
                                //轴移动lastDistance距离
                                AxisPart.Z_Move((double)(nowPos + Convert.ToDouble(lastDistance)));
                                LOG("轴位置从" + nowPos.ToString() + " 到 " + (nowPos + Convert.ToDouble(lastDistance)).ToString());
                                //当前轴位置
                                nowPos = nowPos + Convert.ToDouble(lastDistance);//建议直接获取轴位置
                                textBox_NowPos.BeginInvoke(new MethodInvoker(() => textBox_NowPos.Text = AxisPart.Get_Z_NowPos().ToString()));
                                if (checkBox_SavePic.Checked)
                                {
                                    Thread.Sleep(50);
                                    Program.obj_Vision.Acq = Program.obj_Vision.obj_camera[Obj_Camera.SelectedIndex].Snap(1, 5000);
                                    switch (Program.obj_Vision.Acq.GrabStatus)
                                    {
                                        case "GrabPass":
                                            obj_CameraUI.Image.Dispose();
                                            obj_CameraUI.Image = Program.obj_Vision.Acq.Image.CopyObj(1, -1);
                                            ShowImage(htWindow, obj_CameraUI.Image, null);
                                            HOperatorSet.WriteImage(obj_CameraUI.Image, "tiff", 0, Program.obj_Vision.obj_camera[Obj_Camera.SelectedIndex].cameraPath + "\\" + Movetimes.ToString() + ".tiff");
                                            Program.obj_Vision.Acq.Image.Dispose();
                                            LOG("相机Camera_" + Obj_Camera.SelectedIndex.ToString() + " 拍照,存图于:\n" + Program.obj_Vision.obj_camera[Obj_Camera.SelectedIndex].cameraPath);
                                            break;
                                        default:
                                            break;
                                    }
                                }
                            }
                        }
                        else//若轴向负方向移动
                        {
                            Movetimes = (int)((numericUpDown_Start.Value-numericUpDown_End.Value) / numericUpDown_distance.Value);
                            lastDistance = (int)(numericUpDown_Start.Value - numericUpDown_distance.Value * (decimal)Movetimes - numericUpDown_End.Value);
                            for (int i = 0; i < Movetimes; i++)
                            {
                                //*********************************
                                //轴移动一个步进距离

                                AxisPart.Z_Move((double)(nowPos - Convert.ToDouble(numericUpDown_distance.Value)));

                                LOG("轴位置从" + nowPos.ToString() + " 到 " + (nowPos - Convert.ToDouble(numericUpDown_distance.Value)).ToString());

                                //当前轴位置
                                nowPos = nowPos - Convert.ToDouble(numericUpDown_distance.Value);//建议直接获取轴位置
                                textBox_NowPos.BeginInvoke(new MethodInvoker(() => textBox_NowPos.Text = AxisPart.Get_Z_NowPos().ToString()));
                                if (checkBox_SavePic.Checked)
                                {
                                    Thread.Sleep(50);
                                    Program.obj_Vision.Acq = Program.obj_Vision.obj_camera[Obj_Camera.SelectedIndex].Snap(1, 5000);
                                    switch (Program.obj_Vision.Acq.GrabStatus)
                                    {
                                        case "GrabPass":
                                            obj_CameraUI.Image.Dispose();
                                            obj_CameraUI.Image = Program.obj_Vision.Acq.Image.CopyObj(1, -1);
                                            ShowImage(htWindow, obj_CameraUI.Image, null);
                                            HOperatorSet.WriteImage(obj_CameraUI.Image, "tiff", 0, Program.obj_Vision.obj_camera[Obj_Camera.SelectedIndex].cameraPath + "\\" + i.ToString() + ".tiff");
                                            Program.obj_Vision.Acq.Image.Dispose();
                                            LOG("相机Camera_" + Obj_Camera.SelectedIndex.ToString() + " 拍照,存图于:\n" + Program.obj_Vision.obj_camera[Obj_Camera.SelectedIndex].cameraPath);
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                Thread.Sleep((int)numericUpDown_Delay.Value);
                                if (!isMove) return;//若请求停止
                            }
                            if (lastDistance != 0)//移动最后一段距离
                            {
                                //*********************************
                                //轴移动lastDistance距离

                                AxisPart.Z_Move((double)(nowPos - Convert.ToDouble(lastDistance)));
                                LOG("轴位置从" + nowPos.ToString() + " 到 " + (nowPos - Convert.ToDouble(lastDistance)).ToString());
                                //当前轴位置
                                nowPos = nowPos - Convert.ToDouble(lastDistance);//建议直接获取轴位置
                                textBox_NowPos.BeginInvoke(new MethodInvoker(() => textBox_NowPos.Text = AxisPart.Get_Z_NowPos().ToString()));
                                if (checkBox_SavePic.Checked)
                                {
                                    Thread.Sleep(50);
                                    Program.obj_Vision.Acq = Program.obj_Vision.obj_camera[Obj_Camera.SelectedIndex].Snap(1, 5000);
                                    switch (Program.obj_Vision.Acq.GrabStatus)
                                    {
                                        case "GrabPass":
                                            obj_CameraUI.Image.Dispose();
                                            obj_CameraUI.Image = Program.obj_Vision.Acq.Image.CopyObj(1, -1);
                                            ShowImage(htWindow, obj_CameraUI.Image, null);
                                            HOperatorSet.WriteImage(obj_CameraUI.Image, "tiff", 0, Program.obj_Vision.obj_camera[Obj_Camera.SelectedIndex].cameraPath + "\\" + Movetimes.ToString() + ".tiff");
                                            Program.obj_Vision.Acq.Image.Dispose();
                                            LOG("相机Camera_" + Obj_Camera.SelectedIndex.ToString() + " 拍照,存图于:\n" + Program.obj_Vision.obj_camera[Obj_Camera.SelectedIndex].cameraPath);
                                            break;
                                        default:
                                            break;
                                    }
                                }
                            }
                        }
                        isMove = !isMove;
                        this.Invoke(new MethodInvoker(() => button_Move.Text = "开始移动"));
                    }
                    catch (Exception ex)
                    {
                        if (isMove)
                        {
                            isMove = !isMove;
                            button_Move.Text = "开始移动";
                        }
                        MessageBox.Show(ex.Message);
                    }
                });
                thread_move.Start();
            }
        }

        private void button_HTMUI_Click(object sender, EventArgs e)
        {
            HTM.LoadUI();
        }

        private void button_listClear_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
        }

        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            HTM.Discard();
            Program.obj_Vision.CloseAllCamera();
        }
    }
}
