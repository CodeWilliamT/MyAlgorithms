using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Imaging; 

namespace DoBinaryMap
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        string bufferFileName = Application.StartupPath + "\\buffer.png";
        private void btnCreate_Click(object sender, EventArgs e)
        {
            try
            {

                pictureBox1.Image = Image.FromHbitmap(DoBinaryMap.Generate(richTextBox1.Text.Trim()).GetHbitmap());
                string filename = textBox1.Text.Trim();
                pictureBox1.Image.Save(filename, ImageFormat.Png);
                MessageBox.Show("生成成功");

            }
            catch
            {
                MessageBox.Show("生成出错");
            }
        }

        private void btnSolve_Click(object sender, EventArgs e)
        {
            try
            {
                bufferFileName = Application.StartupPath + "\\buffer.png";
                pictureBox1.Image.Save(bufferFileName, ImageFormat.Png);
                richTextBox1.Text = DoBinaryMap.Read(bufferFileName);
                if (richTextBox1.Text == "")
                {
                    MessageBox.Show("识别失败!\n");
                    return;
                }
                MessageBox.Show("识别成功:\n结果为：" + richTextBox1.Text);


            }
            catch (Exception err)
            {
                MessageBox.Show("识别出错\n" + err.Message);
            }
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog();
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    textBox2.Text = ofd.FileName;
                }
            }
            catch
            {
                MessageBox.Show("出错");
            }
        }

        private void btnShow_Click(object sender, EventArgs e)
        {
            try
            {
                pictureBox1.Image = Image.FromFile(textBox2.Text.Trim());
            }
            catch
            {
                MessageBox.Show("出错");
            }
        }
        Form Catchform;
        Point MouseSP;
        Point MouseEP; 
        Graphics g;
        bool CatchStart;
        private void btnCatch_Click(object sender, EventArgs e)
        {
            try
            {
                Catchform = new Form();
                MouseSP = new Point(0, 0);
                MouseEP = new Point(0, 0);
                CatchStart = false;
                Catchform.Location = new Point(-50, -50);
                Catchform.Size = new Size(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width+100, System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height+100);
                Catchform.ControlBox = false;
                Catchform.ShowInTaskbar = false;
                Catchform.MaximizeBox = false;
                Catchform.FormBorderStyle = FormBorderStyle.FixedSingle;
                Catchform.Opacity = 0.1;
                Button btnEsc = new Button();
                Catchform.Controls.Add(btnEsc);
                Catchform.CancelButton = btnEsc;
                btnEsc.Click += btnEsc_Click;
                Catchform.MouseDown+=Catchform_MouseDown;
                Catchform.MouseUp+=Catchform_MouseUp;
                Catchform.MouseClick += Catchform_MouseClick;
                Catchform.MouseMove+=Catchform_MouseMove;
                g = Catchform.CreateGraphics();
                Catchform.Show();
            }
            catch(Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }
        private void btnEsc_Click(object sender, EventArgs e)
        {
            Catchform.Close();
        }

        private void Catchform_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right) MouseSP = Cursor.Position;
            CatchStart = true;
        }

        private void Catchform_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right) MouseEP = Cursor.Position;
            CatchStart = false;
        }
        private void Catchform_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button != MouseButtons.Right) return;
                Catchform.Close();
                int width = Math.Abs(MouseEP.X - MouseSP.X);
                int height = Math.Abs(MouseEP.Y - MouseSP.Y);
                Point UpperLeftPoint = new Point(MouseEP.X < MouseSP.X ? MouseEP.X : MouseSP.X, MouseEP.Y < MouseSP.Y ? MouseEP.Y : MouseSP.Y);
                Image objImage = new Bitmap(width, height);
                Graphics g = Graphics.FromImage(objImage);
                g.CopyFromScreen(UpperLeftPoint, new Point(0, 0), new Size(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height));
                IntPtr dc1 = g.GetHdc();
                g.ReleaseHdc(dc1);
                this.pictureBox1.Image = objImage;
                Clipboard.SetDataObject(objImage);
            }
            catch
            {
                MessageBox.Show("错误操作,截图失败！\n截图：左键按住画矩形区域，右键确认，Esc退出截图");
            }
        }
        private void Catchform_MouseMove(object sender, MouseEventArgs e)
        {
            if (CatchStart)
            {
                g.Clear(Catchform.BackColor);
                int minX = Cursor.Position.X < MouseSP.X ? Cursor.Position.X : MouseSP.X;
                int minY = Cursor.Position.Y < MouseSP.Y ? Cursor.Position.Y : MouseSP.Y;
                int maxX = Cursor.Position.X > MouseSP.X ? Cursor.Position.X : MouseSP.X;
                int maxY = Cursor.Position.Y > MouseSP.Y ? Cursor.Position.Y : MouseSP.Y;
                g.DrawRectangle(new Pen(Color.Red,2), minX-10, minY-10, maxX - minX, maxY - minY);
            }
        }
    }
}
