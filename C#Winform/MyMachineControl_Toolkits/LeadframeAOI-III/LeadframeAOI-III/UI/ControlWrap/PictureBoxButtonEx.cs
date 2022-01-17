using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace LeadframeAOI.UI.ControlWrap
{ /// <summary>
    /// 图片按钮
    /// </summary>
    public class PictureButtonEx : PictureBox
    {
        private Color normalBackColor;
        private Color hoverBackColor;
        private Color pressBackColor;

        Image normalBackgroundImage;
        Image hoverBackgroundImage;
        Image pressBackgroundImage;

        /// <summary>
        /// 构造函数
        /// </summary>
        public PictureButtonEx()
        {
            normalBackColor = Color.Transparent;
            hoverBackColor = Color.FromArgb(255, 250, 234);
            pressBackColor = Color.FromArgb(255, 232, 166);
            base.BackColor = Color.Transparent;
            this.MouseEnter += new EventHandler(PictureBoxButton_MouseEnter);
            this.MouseLeave += new EventHandler(PictureBoxButton_MouseLeave);
            this.MouseDown += new MouseEventHandler(PictureBoxButton_MouseDown);
            this.MouseUp += new MouseEventHandler(PictureBoxButton_MouseLeave);
            //this.MouseHover += new EventHandler(PictureBoxButton_MouseEnter);
        }
        #region 属性
        /// <summary>
        /// 背景色
        /// </summary>
        [Browsable(true)]
        public new Color BackColor
        {
            get { return base.BackColor; }
            set { base.BackColor = value; }
        }
        /// <summary>
        /// 背景图片
        /// </summary>
        [Browsable(false)]
        public new Image BackgroundImage
        {
            get { return base.BackgroundImage; }
            set { base.BackgroundImage = value; }
        }
        /// <summary>
        /// 按钮在正常状态下的背景颜色
        /// </summary>
        public Color BackColor_Normal
        {
            get { return normalBackColor; }
            set { normalBackColor = value; }
        }
        /// <summary>
        /// 按钮在正常状态下的背景图片，如果设置此项，则BackColor_Normal项将无效
        /// </summary>
        public Image BackgroundImage_Normal
        {
            get { return normalBackgroundImage; }
            set { normalBackgroundImage = value; }
        }
        /// <summary>
        /// 按钮在鼠标停浮状态下的背景颜色
        /// </summary>
        public Color BackColor_Hover
        {
            get { return hoverBackColor; }
            set { hoverBackColor = value; }
        }
        /// <summary>
        /// 按钮在鼠标停浮状态下的背景图片，如果设置此项，则BackColor_Hover项将无效
        /// </summary>
        public Image BackgroundImage_Hover
        {
            get { return hoverBackgroundImage; }
            set { hoverBackgroundImage = value; }
        }
        /// <summary>
        /// 按钮在鼠标按下的背景颜色
        /// </summary> 
        public Color BackColor_Press
        {
            get { return pressBackColor; }
            set { pressBackColor = value; }
        }
        /// <summary>
        /// 按钮在鼠标按下的背景图片，如果设置此项，则BackColor_Press项将无效
        /// </summary>
        public Image BackgroundImage_Press
        {
            get { return pressBackgroundImage; }
            set { pressBackgroundImage = value; }
        }
        #endregion

        private void PictureBoxButton_MouseEnter(object sender, EventArgs e)
        {
            this.BackColor = hoverBackColor;
            if (hoverBackgroundImage != null)
                this.BackgroundImage = hoverBackgroundImage;
        }
        private void PictureBoxButton_MouseLeave(object sender, EventArgs e)
        {
            this.BackColor = normalBackColor;
            if (normalBackgroundImage != null)
                this.BackgroundImage = normalBackgroundImage;
            else
                this.BackgroundImage = null;
        }
        private void PictureBoxButton_MouseDown(object sender, MouseEventArgs e)
        {
            this.BackColor = pressBackColor;
            if (pressBackgroundImage != null)
                this.BackgroundImage = pressBackgroundImage;
        }
    }
}
