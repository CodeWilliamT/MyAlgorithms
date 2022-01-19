using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HalconDotNet;
using HTHalControl;

namespace ToolKits.RegionModify
{
    public partial class RegionModifyForm : UserControl
    {
        public enum RegionMode
        {
            /// <summary>
            /// 区域
            /// </summary>
            region,
            /// <summary>
            /// 轮廓
            /// </summary>
            contour
        }
        public RegionModifyForm(HTWindowControl htWindow, HObject modifyRegion, RegionMode regionMode)
        {
            InitializeComponent();
            if (modifyRegion == null || !modifyRegion.IsInitialized())
            {
                this.Dispose();
                return;
            }
            this.modifyRegion = modifyRegion.CopyObj(1, -1);
            this.htWindow = htWindow;
            this.regMode = regionMode;
            this.htWindow.RefreshWindow(htWindow.Image, this.modifyRegion, "");
        }


        HTWindowControl htWindow;
        HObject modifyRegion;
        RegionMode regMode;
        /// <summary>
        /// 优化后的区域
        /// </summary>
        public HObject ModifyRegion
        {
            get { return this.modifyRegion; }
        }
        /// <summary>
        /// 找出包含点row,col的区域或轮廓(索引起始为1)
        /// </summary>
        /// <param name="region"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns>true表示找到，false表示未找到</returns>
        private int GetSpecRegion(HObject region, HTuple row, HTuple col, ref HObject selectedRegion)
        {
            HTuple num = null, isInside = null;
            HOperatorSet.CountObj(region, out num);
            int ind = 0;
            for (int i = 1; i <= num; i++)
            {
                ind++;
                selectedRegion.Dispose();
                HOperatorSet.SelectObj(region, out selectedRegion, i);
                if (this.regMode == RegionMode.region)
                    HOperatorSet.TestRegionPoint(selectedRegion, row, col, out isInside);
                else HOperatorSet.TestXldPoint(selectedRegion, row, col, out isInside);
                if (isInside.I == 1) break;
            }
            return (isInside.I == 1) ? ind : -1;
        }
        /// <summary>
        /// 根据索引值更新区域数组updateRegion
        /// </summary>
        /// <param name="updateRegion">被更新的区域数组</param>
        /// <param name="region">用来更新指定索引处区域的区域</param>
        /// <param name="index">需要更新的索引值</param>
        private void UpdateRegion(ref HObject updateRegion, HObject region, int index)
        {
            HObject localRegion = new HObject();
            HOperatorSet.GenEmptyObj(out localRegion);
            HTuple num = null;
            HOperatorSet.CountObj(updateRegion, out num);
            for (int i = 1; i <= num; i++)
            {
                if (i == index)
                    HOperatorSet.ConcatObj(localRegion, region, out localRegion);
                else
                    HOperatorSet.ConcatObj(localRegion, updateRegion.SelectObj(i).CopyObj(1, -1), out localRegion);
            }
            updateRegion.Dispose();
            updateRegion = localRegion.CopyObj(1, -1);
            localRegion.Dispose();
        }

        /// <summary>
        /// 释放所有资源
        /// </summary>
        /// <returns></returns>
        public new bool Dispose()
        {
            try
            {
                if (this.modifyRegion != null) this.modifyRegion.Dispose();

                base.Dispose();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private void btnExModify_Click(object sender, EventArgs e)
        {
            HObject selectedRegion = new HObject();
            HOperatorSet.GenEmptyObj(out selectedRegion);
            HObject cont = new HObject();
            HOperatorSet.GenEmptyObj(out cont);
            try
            {
                if (this.htWindow.RegionType != "Point")
                {
                    MessageBox.Show("请先在需要修改的区域内部画一个点，右键结束！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                selectedRegion.Dispose();
                int ind = GetSpecRegion(this.modifyRegion, this.htWindow.Row, this.htWindow.Column, ref selectedRegion);
                if (ind == -1)
                {
                    selectedRegion.Dispose();
                    MessageBox.Show("所画的点不在任意一个需要修改的区域内，请重新操作！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                cont.Dispose();
                if (this.regMode == RegionMode.region)
                    HOperatorSet.GenContourRegionXld(selectedRegion, out cont, "border");
                else HOperatorSet.CopyObj(selectedRegion, out cont, 1, -1);

                HOperatorSet.DrawXldMod(cont, out selectedRegion, this.htWindow.HTWindow.HalconWindow, "true", "true", "true", "true", "true");

                cont.Dispose();
                if (this.regMode == RegionMode.region)
                    HOperatorSet.GenRegionContourXld(selectedRegion, out cont, "filled");
                else HOperatorSet.CopyObj(selectedRegion, out cont, 1, -1);

                //更新
                UpdateRegion(ref this.modifyRegion, cont, ind);
                this.htWindow.RefreshWindow(this.htWindow.Image, this.modifyRegion, "");

                selectedRegion.Dispose();
                cont.Dispose();
            }
            catch (Exception ex)
            {

                selectedRegion.Dispose();
                cont.Dispose();
                MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnExDelete_Click(object sender, EventArgs e)
        {
            HObject selectedRegion = new HObject();
            HOperatorSet.GenEmptyObj(out selectedRegion);
            HObject cont = new HObject();
            HOperatorSet.GenEmptyObj(out cont);
            try
            {
                if (this.htWindow.RegionType != "Point")
                {
                    MessageBox.Show("请现在需要修改的区域内部画一个点！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                selectedRegion.Dispose();
                int ind = GetSpecRegion(this.modifyRegion, this.htWindow.Row, this.htWindow.Column, ref selectedRegion);
                if (ind == -1)
                {
                    selectedRegion.Dispose();
                    MessageBox.Show("所画的点不在任意一个需要修改的区域内，请重新操作！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                cont.Dispose();
                HOperatorSet.GenEmptyObj(out cont);

                //更新
                UpdateRegion(ref this.modifyRegion, cont, ind);
                this.htWindow.RefreshWindow(this.htWindow.Image, this.modifyRegion, "");

                selectedRegion.Dispose();
                cont.Dispose();
            }
            catch (Exception ex)
            {

                selectedRegion.Dispose();
                cont.Dispose();
                MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnExAdd_Click(object sender, EventArgs e)
        {
            if (this.htWindow.RegionType == "" || this.htWindow.RegionType == "Point" || this.htWindow.RegionType == "Line")
            {
                MessageBox.Show("区域类型不适合添加区域！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            HObject region = new HObject();
            HOperatorSet.GenEmptyObj(out region);
            region.Dispose();
            if (this.regMode == RegionMode.region)
                ToolKits.FunctionModule.Vision.GenROI(this.htWindow, "region", ref region);
            else ToolKits.FunctionModule.Vision.GenROI(this.htWindow, "contour", ref region);
            HOperatorSet.ConcatObj(this.modifyRegion, region.CopyObj(1, -1), out this.modifyRegion);

            this.htWindow.RefreshWindow(this.htWindow.Image, this.modifyRegion, "");

            region.Dispose();
        }
    }
}
