using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SharpGL;

namespace SharpGLTest
{
    public partial class Form_1 : Form
    {
        public static Form_1 Instance;
        public Form_1()
        {
            InitializeComponent();
            Instance = this;
        }

        private void openGLControl1_Resize(object sender, EventArgs e)
        {
            OpenGL gl = openGLControl1.OpenGL;

            //  设置当前矩阵模式,对投影矩阵应用随后的矩阵操作
            gl.MatrixMode(OpenGL.GL_PROJECTION);

            // 重置当前指定的矩阵为单位矩阵,将当前的用户坐标系的原点移到了屏幕中心
            gl.LoadIdentity();

            // 创建透视投影变换
            gl.Perspective(30.0f, (double)Width / (double)Height, 5, 100.0);

            // 视点变换
            gl.LookAt(-5, 5, -5, 0, 0, 0, 0, 1, 0);

            // 设置当前矩阵为模型视图矩阵
            gl.MatrixMode(OpenGL.GL_MODELVIEW);
        }


        private void openGLControl1_OpenGLDraw(object sender, RenderEventArgs args)
        {

            SharpGL.OpenGL gl = this.openGLControl1.OpenGL;
            //清除深度缓存 
            gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);

            //重置当前指定的矩阵为单位矩阵,将当前的用户坐标系的原点移到了屏幕中心
            gl.LoadIdentity();

            //坐标轴变换位置到(-1.5,0,-6)
            gl.Translate(-1.5f, 0f, -6f);

            gl.Begin(OpenGL.GL_TRIANGLES);
            {
                //顶点 
                gl.Vertex(0.0f, 1.0f, 0.0f);
                //左端点      
                gl.Vertex(-1.0f, -1.0f, 0.0f);
                //右端点       
                gl.Vertex(1.0f, -1.0f, 0.0f);
            }
            gl.End();

            //把当前坐标系右移3个单位，注意此时是相对上面(-1.5,0,-6)点定位 
            gl.Translate(3f, 0f, 0f);

            gl.Begin(OpenGL.GL_QUADS);
            {
                gl.Vertex(-1.0f, 1.0f, 0.0f);
                gl.Vertex(1.0f, 1.0f, 0.0f);
                gl.Vertex(1.0f, -1.0f, 0.0f);
                gl.Vertex(-1.0f, -1.0f, 0.0f);
            }
            gl.End();
            gl.Flush();   //强制刷新
        }
    }
}
