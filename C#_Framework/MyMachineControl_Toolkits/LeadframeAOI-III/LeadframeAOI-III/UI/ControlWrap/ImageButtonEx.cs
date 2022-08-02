using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System;

namespace HTImageServer.UI.ControlWrap
{
    public partial class ImageButtonEx : PictureBox
    {
        Image normaImage;
        Image hoverImage;
        Image pressImage;
        /// <summary>
        /// 按钮在正常状态下的背景图片，如果设置此项，则BackColor_Normal项将无效
        /// </summary>
        public Image Image_Normal
        {
            get { return normaImage; }
            set { normaImage = value; }
        }
        /// <summary>
        /// 按钮在鼠标停浮状态下的背景图片，如果设置此项，则BackColor_Hover项将无效
        /// </summary>
        public Image Image_Hover
        {
            get { return hoverImage; }
            set { hoverImage = value; }
        }
        /// <summary>
        /// 按钮在鼠标按下的背景图片，如果设置此项，则BackColor_Press项将无效
        /// </summary>
        public Image Image_Press
        {
            get { return pressImage; }
            set { pressImage = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public ImageButtonEx()
        {
            this.MouseEnter += new EventHandler(ImageButton_MouseEnter);
            this.MouseLeave += new EventHandler(ImageButton_MouseLeave);
            this.MouseDown += new MouseEventHandler(ImageButton_MouseDown);
            this.MouseUp += new MouseEventHandler(ImageButton_MouseLeave);
        }

        private void ImageButton_MouseEnter(object sender, EventArgs e)
        {
            if (hoverImage != null)
                this.Image = hoverImage;
        }
        private void ImageButton_MouseLeave(object sender, EventArgs e)
        {
            if (normaImage != null)
                this.Image = normaImage;
        }
        private void ImageButton_MouseDown(object sender, MouseEventArgs e)
        {
            if (pressImage != null)
                this.Image = pressImage;
        }
    }
}
