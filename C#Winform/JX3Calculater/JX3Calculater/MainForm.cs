using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using System.Configuration;
using AppSettingDll;

namespace JX3Calculater
{
    public partial class MainForm : Form
    {
        Common common;
        int planIdx;
        int planNum;
        ToolTip toolTip;
        public MainForm()
        {
            InitializeComponent();
            Text = Text + " 编译日期:" + System.IO.File.GetLastWriteTime(this.GetType().Assembly.Location).ToString("yyyyMMdd.HH");
            common = new Common();
            toolTip = new ToolTip();
            SetTooltip();
            cbdqxf.Items.Clear();
            foreach (string xfm in Common.xf)
            {
                cbdqxf.Items.Add(xfm);
            }
            cbzsxm.Items.Clear();
            foreach (string sx in Common.zsx)
            {
                cbzsxm.Items.Add(sx);
            }
            LoadPlan();
        }
        private void SetTooltip()
        {
            //系数
            toolTip.SetToolTip(lbmzxs, "命中系数=命中点数/(命中率-0.919)/100 保留3位小数");
            toolTip.SetToolTip(lbhxxs, "会心系数=会心点数/会心率/100 保留3位小数");
            toolTip.SetToolTip(lbhxiaoxs, "会效系数=会效点数/会效率/100 保留3位小数");
            toolTip.SetToolTip(lbpfxs, "破防系数=破防点数/破防率/100 保留3位小数");
            toolTip.SetToolTip(lbjsxs, "加速系数=加速点数/会心率/100 保留3位小数");
            toolTip.SetToolTip(lbwsxs, "无双系数=无双点数/无双率/100 保留3位小数");
            //伤害
            toolTip.SetToolTip(lbjcpf, "面板攻击-主属性提升的非基础攻击");
            toolTip.SetToolTip(lbwusxx, "(近战武器+重兵)描述的近身伤害提高下限总量+当前武器伤害附魔五彩.");
            toolTip.SetToolTip(lbwussx, "(近战武器+重兵)描述的近身伤害提高上限总量+当前武器伤害附魔五彩.");
            toolTip.SetToolTip(lbyx, "御劲对会心后发生伤害的减伤比例");
            //增益
            toolTip.SetToolTip(lbhxzy, "隐藏增益：唐门弹药1%:千机匣存在弹药则提升1%会心");
            toolTip.SetToolTip(lbhxiaozy, "10%会效的增益实际为10.2%，其他该多少就加多少");
            toolTip.SetToolTip(lbjcpfzy, "所有对破防比例增幅的增益，多增益则累加");
            toolTip.SetToolTip(lbjcgjzy, "所有对攻击比例增幅的增益，多增益则累加");
            toolTip.SetToolTip(lbjnzs, "技能伤害提升增益(秘籍、常规奇穴)，累加，直接去尾保留小数点后三位");
            toolTip.SetToolTip(lbjntszs, "特殊技能伤害提升增益(特殊情形技能增伤)，多增益则加1累乘减1，直接去尾保留小数点后三位，多特殊增益可能产生少量误差，因为实际乘法为(int)((int)((1+技能特殊增益1) * (1 + 技能特殊增益2) *1000/ 1.024)*1.024)/1000.0");

            //输出
            toolTip.SetToolTip(lbszjnyssh, "(1+技能增伤)*(1+技能特殊增伤)*(实战技能固伤 + 实战技能系数 * 实战攻击 + 实战技能武伤系数 * 实战武伤)");
            toolTip.SetToolTip(lbcgshqw, "实战技能原始伤害 * (1 + 实战破防率) * (实战会心率 * 实战会效率 + (1 - 实战会心率))");
            toolTip.SetToolTip(lbpvpshqw, "实战技能原始伤害 * (1 + 实战破防率) * ((实战会心率 - 御劲) * 实战会效率 * (1 - 御效) + pvp实战命中率) * (1 - 化劲)*(1-实战防御率)");
            toolTip.SetToolTip(lbpveshqw, "实战技能原始伤害 * (1 + 实战破防率) * (PVE实战会心率 * 实战会效率 + pve实战命中率+ pve实战识破率 * 识破衰减比) * (1 - 实战防御率)");
            toolTip.SetToolTip(lbjnxs, "技能攻击加成系数。剑三大量技能描述的加成系数错误。会产生几千的技能伤害误差，可实战后在输入参数自行调整。");
        }
        private void LoadPlan()
        {
            planIdx = int.Parse(AppSetting.LoadOne("planIdx"));
            planNum = int.Parse(AppSetting.LoadOne("planNum"));
            cbPlan.Items.Add("默认方案");
            for (int i = 1; i < planNum; i++)
            {
                cbPlan.Items.Add(AppSetting.LoadOne("plan-" + i));
            }
            cbPlan.SelectedIndex = planIdx;
        }
        private void SavePlan()
        {
            AppSetting.SaveOne("planIdx", planIdx.ToString());
            AppSetting.SaveOne("planNum", planNum.ToString());
            for (int i = 1; i < cbPlan.Items.Count; i++)
            {
                AppSetting.SaveOne("plan-" + i, cbPlan.Items[i].ToString());
            }
        }
        private void LoadData()
        {
            common.Load(cbPlan.Items[planIdx].ToString());
            SetupUI();
            btnCalc_Click(null, null);
        }
        private void SaveData()
        {
            SetData();
            SavePlan();
            common.Save(cbPlan.Items[planIdx].ToString());
        }
        private void SetupUI()
        {
            cbdqxf.SelectedItem = common.dqxf;
            //基础系数
            numgghx.Value = (decimal)Common.gghx;
            numyqgj.Value = (decimal)Common.yqgj;
            numyqpf.Value = (decimal)Common.yqpf;
            numsfhx.Value = (decimal)Common.sfhx;
            numldgj.Value = (decimal)Common.ldgj;
            numldpf.Value = (decimal)Common.ldpf;
            //伤害系数
            nummzxs.Value = (decimal)Common.mzxs;
            numhxxs.Value = (decimal)Common.hxxs;
            numhxiaoxs.Value = (decimal)Common.hxiaoxs;
            numpfxs.Value = (decimal)Common.pfxs;
            numjsxs.Value = (decimal)Common.jsxs;
            numwsxs.Value = (decimal)Common.wsxs;
            //守御系数
            numfyxs.Value = (decimal)Common.fyxs;
            numyxb.Value = (decimal)Common.yxb;
            numspsjb.Value = (decimal)Common.spsjb;
            //基础
            numgg.Value = common.gg;
            numyq.Value = common.yq;
            numsf.Value = common.sf;
            numld.Value = common.ld;
            //伤害
            numgj.Value = common.gj;
            numjcgj.Value = common.jcgj;
            nummz.Value = common.mz;
            numhx.Value = common.hx;
            numhxiao.Value = common.hxiao;
            numpf.Value = common.pf;
            numjcpf.Value = common.jcpf;
            numjs.Value = common.js;
            numws.Value = common.ws;
            numwusxx.Value = common.wusxx;
            numwussx.Value = common.wussx;
            //技能
            numjngsxx.Value = (decimal)common.jngsxx;
            numjngssx.Value = (decimal)common.jngssx;
            numjnxs.Value = (decimal)common.jnxs;
            numjnwsxs.Value = (decimal)common.jnwsxs;
            //守御
            numfy.Value = (decimal)common.fy;
            numpll.Value = (decimal)common.pll;
            numspl.Value = (decimal)common.spl;
            numhj.Value = (decimal)common.hj;
            numyj.Value = (decimal)common.yj;
            //GCD加速阈值表
            tbydjs.Text = common.ydjs.ToString();
            tbedjs.Text = common.edjs.ToString();
            tbsdjs.Text = common.sdjs.ToString();
            tbsidjs.Text = common.sidjs.ToString();
            tbwdjs.Text = common.wdjs.ToString();
            //主属性
            cbzsxm.SelectedItem = common.zsxm;
            numzsxgj.Value = (decimal)common.zsxgj;
            numzsxpf.Value = (decimal)common.zsxpf;
            numzsxhx.Value = (decimal)common.zsxhx;
            //增益
            numhxzy.Value = (decimal)common.hxzy;
            numhxiaozy.Value = (decimal)common.hxiaozy;
            numpfzy.Value = (decimal)common.pfzy;
            numgjzy.Value = (decimal)common.gjzy;
            numjszy.Value = (decimal)common.jszy;
            numjnzs.Value = (decimal)common.jnzs;
            numjntszs.Value = (decimal)common.jntszs;
            numfyws.Value = (decimal)common.fyws;
            //附魔
            numhxfm.Value = common.hxfm;
            numpffm.Value = common.pffm;
            numhxiaofm.Value = common.hxiaofm;
            numgjfm.Value = common.gjfm;
            numjsfm.Value = common.jsfm;
            numzsfm.Value = common.zsfm;
            numwusfm.Value = common.wusfm;
            nummzfm.Value = common.mzfm;
            numwsfm.Value = common.wsfm;
        }
        private void SetData()
        {
            //基础系数
            Common.gghx = (double)numgghx.Value;
            Common.yqgj = (double)numyqgj.Value;
            Common.yqpf = (double)numyqpf.Value;
            Common.sfhx = (double)numsfhx.Value;
            Common.ldgj = (double)numldgj.Value;
            Common.ldpf = (double)numldpf.Value;
            //伤害系数
            Common.mzxs = (double)nummzxs.Value;
            Common.hxxs = (double)numhxxs.Value;
            Common.hxiaoxs = (double)numhxiaoxs.Value;
            Common.pfxs = (double)numpfxs.Value;
            Common.jsxs = (double)numjsxs.Value;
            Common.wsxs = (double)numwsxs.Value;
            //守御系数
            Common.fyxs = (double)numfyxs.Value;
            Common.yxb = (double)numyxb.Value;
            Common.spsjb = (double)numspsjb.Value;
            //基础
            common.gg = (int)numgg.Value;
            common.yq = (int)numyq.Value;
            common.sf = (int)numsf.Value;
            common.ld = (int)numld.Value;
            //伤害
            common.gj = (int)numgj.Value;
            common.jcgj = (int)numjcgj.Value;
            common.mz = (int)nummz.Value;
            common.hx = (int)numhx.Value;
            common.hxiao = (int)numhxiao.Value;
            common.pf = (int)numpf.Value;
            common.js = (int)numjs.Value;
            common.ws = (int)numws.Value;
            common.wusxx = (int)numwusxx.Value;
            common.wussx = (int)numwussx.Value;
            //技能
            common.jngsxx = (int)numjngsxx.Value;
            common.jngssx = (int)numjngssx.Value;
            common.jnxs = (double)numjnxs.Value;
            common.jnwsxs = (double)numjnwsxs.Value;
            //守御
            common.fy = (int)numfy.Value;
            common.pll = (double)numpll.Value;
            common.spl = (double)numspl.Value;
            common.hj = (double)numhj.Value;
            common.yj = (double)numyj.Value;
            //主属性
            common.zsxm = cbzsxm.SelectedItem.ToString();
            common.zsxgj = (double)numzsxgj.Value;
            common.zsxpf = (double)numzsxpf.Value;
            common.zsxhx = (double)numzsxhx.Value;
            //增益
            common.hxzy = (double)numhxzy.Value;
            common.hxiaozy = (double)numhxiaozy.Value;
            common.pfzy = (double)numpfzy.Value;
            common.gjzy = (double)numgjzy.Value;
            common.jszy = (double)numjszy.Value;
            common.jnzs = (double)numjnzs.Value;
            common.jntszs = (double)numjntszs.Value;
            common.fyws = (double)numfyws.Value;
            //附魔
            common.hxfm = (int)numhxfm.Value;
            common.pffm = (int)numpffm.Value;
            common.hxiaofm = (int)numhxiaofm.Value;
            common.gjfm = (int)numgjfm.Value;
            common.jsfm = (int)numjsfm.Value;
            common.zsfm = (int)numzsfm.Value;
            common.wusfm = (int)numwusfm.Value;
            common.mzfm = (int)nummzfm.Value;
            common.wsfm = (int)numwsfm.Value;
        }


        private void nummz_ValueChanged(object sender, EventArgs e)
        {
            common.mz = (int)nummz.Value;
            nummzl.Value = (decimal)(0.919 + (int)(100000 * common.mz / 100.0 / Common.mzxs) / 100000.0);
        }

        private void numhx_ValueChanged(object sender, EventArgs e)
        {
            common.hx = (int)numhx.Value;
            numhxl.Value = (decimal)((int)(100000 * common.hx / 100.0 / Common.hxxs) / 100000.0);
        }

        private void numhxiao_ValueChanged(object sender, EventArgs e)
        {
            common.hxiao = (int)numhxiao.Value;
            numhxiaol.Value = (decimal)(1.75 + (int)(100000 * common.hxiao / 100.0 / Common.hxiaoxs) / 100000.0);
        }

        private void numpf_ValueChanged(object sender, EventArgs e)
        {
            common.pf = (int)numpf.Value;
            numjcpf.Value = common.jcpf;
            numpfl.Value = (decimal)((int)(100000 * common.pf / 100.0 / Common.pfxs) / 100000.0);
        }

        private void numjs_ValueChanged(object sender, EventArgs e)
        {
            common.js = (int)numjs.Value;
            numjsl.Value = (decimal)common.jsl;
            tbgcd.Text = common.gcd.ToString();
        }

        private void numws_ValueChanged(object sender, EventArgs e)
        {
            common.ws = (int)numws.Value;
            numwsl.Value = (decimal)((int)(100000 * common.ws / 100.0 / Common.wsxs) / 100000.0);
        }

        private void nummzl_ValueChanged(object sender, EventArgs e)
        {
            if (0.919 + (int)(100000 * common.mz / 100.0 / Common.mzxs)/ 100000.0 != Convert.ToDouble(nummzl.Value))
            {
                nummz.Value = (int)(((double)nummzl.Value - 0.919) * 100.0 * Common.mzxs);
            }
        }

        private void numhxl_ValueChanged(object sender, EventArgs e)
        {
            if ((int)(100000 * common.hx / 100.0 / Common.hxxs) / 100000.0 != Convert.ToDouble(numhxl.Value))
            {
                numhx.Value = (int)((double)numhxl.Value * 100.0 * Common.hxxs);
            }
        }

        private void numhxiaol_ValueChanged(object sender, EventArgs e)
        {
            if (1.75 + (int)(100000 * common.hxiao / 100.0 / Common.hxiaoxs) / 100000.0 != Convert.ToDouble(numhxiaol.Value))
            {
                numhxiao.Value = (int)(((double)numhxiaol.Value - 1.75) * 100.0 * Common.hxiaoxs);
            }
        }

        private void numpfl_ValueChanged(object sender, EventArgs e)
        {
            if ((int)(100000 * common.pf / 100.0 / Common.pfxs) / 100000.0 != Convert.ToDouble(numpfl.Value))
            {
                numpf.Value = (int)((double)numpfl.Value * 100.0 * Common.pfxs);
            }
        }

        private void numjsl_ValueChanged(object sender, EventArgs e)
        {
            common.js = (int)((double)numjsl.Value * 100.0 * Common.jsxs);
            if (common.jsl == Convert.ToDouble(numjsl.Value))
            {
                numjs.Value = common.js;
            }
            else
            {
                common.js = (int)numjs.Value;
            }
        }

        private void numwsl_ValueChanged(object sender, EventArgs e)
        {
            if ((int)(100000 * common.ws / 100.0 / Common.wsxs) / 100000.0 != Convert.ToDouble(numwsl.Value))
            {
                numws.Value = (int)((double)numwsl.Value * 100.0 * Common.wsxs);
            }
        }

        private void numjszy_ValueChanged(object sender, EventArgs e)
        {
            tbgcd.Text = common.gcd.ToString();
        }

        private void numyj_ValueChanged(object sender, EventArgs e)
        {
            common.yj = (double)numyj.Value;
            numyx.Value = (decimal)common.yx;
        }

        private void numyx_ValueChanged(object sender, EventArgs e)
        {
            numyj.Value = (decimal)((double)numyx.Value / Common.yxb);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string str = Interaction.InputBox("请输入新建方案名。", "新建方案", "", -1, -1);
            if (str == "") return;
            cbPlan.Items.Add(str);
            planIdx = planNum;
            planNum++;
            cbPlan.SelectedIndex = planIdx;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (cbPlan.SelectedIndex == 0)
            {
                MessageBox.Show("默认方案不可删除！");
                return;
            }
            if (MessageBox.Show("删除当前方案？?", "Confirm Message", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                cbPlan.Items.RemoveAt(cbPlan.SelectedIndex);
                planIdx--;
                cbPlan.SelectedIndex = planIdx;
                planNum--;
                SavePlan();
                LoadData();
            }
        }
        private void btnLoad_Click(object sender, EventArgs e)
        {
            planIdx = cbPlan.SelectedIndex;
            LoadData();
        }
        private void btnCalc_Click(object sender, EventArgs e)
        {
            SetData();
            //期望数据
            tbdcsh.Text = common.dcshxx.ToString("F0") + "→" + common.dcshsx.ToString("F0");
            tbhxdcsh.Text = common.hxdcshxx.ToString("F0") + "→" + common.hxdcshsx.ToString("F0");
            tbmzdcsh.Text = common.mzdcshxx.ToString("F0") + "→" + common.mzdcshsx.ToString("F0");
            tbmmsh.Text = common.mmshxx.ToString("F0") + "→" + common.mmshsx.ToString("F0");

            tbhxfmdcshts.Text = common.hxfmdcshts.ToString("P4");
            tbhxhxfmdcshts.Text = common.hxhxfmdcshts.ToString("P4");
            tbmzhxfmdcshts.Text = common.mzhxfmdcshts.ToString("P4");
            tbhxfmmmshts.Text = common.hxfmmmshts.ToString("P4");

            tbpffmdcshts.Text = common.pffmdcshts.ToString("P4");
            tbhxpffmdcshts.Text = common.hxpffmdcshts.ToString("P4");
            tbmzpffmdcshts.Text = common.mzpffmdcshts.ToString("P4");
            tbpffmmmshts.Text = common.pffmmmshts.ToString("P4");

            tbhxiaofmdcshts.Text = common.hxiaofmdcshts.ToString("P4");
            tbhxhxiaofmdcshts.Text = common.hxhxiaofmdcshts.ToString("P4");
            tbmzhxiaofmdcshts.Text = common.mzhxiaofmdcshts.ToString("P4");
            tbhxiaofmmmshts.Text = common.hxiaofmmmshts.ToString("P4");

            tbgjfmdcshts.Text = common.gjfmdcshts.ToString("P4");
            tbhxgjfmdcshts.Text = common.hxgjfmdcshts.ToString("P4");
            tbmzgjfmdcshts.Text = common.mzgjfmdcshts.ToString("P4");
            tbgjfmmmshts.Text = common.gjfmmmshts.ToString("P4");

            tbjsfmdcshts.Text = common.jsfmdcshts.ToString("P4");
            tbhxjsfmdcshts.Text = common.hxjsfmdcshts.ToString("P4");
            tbmzjsfmdcshts.Text = common.mzjsfmdcshts.ToString("P4");
            tbjsfmmmshts.Text = common.jsfmmmshts.ToString("P4");

            tbzsfmdcshts.Text = common.zsfmdcshts.ToString("P4");
            tbhxzsfmdcshts.Text = common.hxzsfmdcshts.ToString("P4");
            tbmzzsfmdcshts.Text = common.mzzsfmdcshts.ToString("P4");
            tbzsfmmmshts.Text = common.zsfmmmshts.ToString("P4");

            tbwusfmdcshts.Text = common.wusfmdcshts.ToString("P4");
            tbhxwusfmdcshts.Text = common.hxwusfmdcshts.ToString("P4");
            tbmzwusfmdcshts.Text = common.mzwusfmdcshts.ToString("P4");
            tbwusfmmmshts.Text = common.wusfmmmshts.ToString("P4");

            tbpvpdcsh.Text = common.pvpdcshxx.ToString("F0") + "→" + common.pvpdcshsx.ToString("F0");
            tbhxpvpdcsh.Text = common.hxpvpdcshxx.ToString("F0") + "→" + common.hxpvpdcshsx.ToString("F0");
            tbmzpvpdcsh.Text = common.mzpvpdcshxx.ToString("F0") + "→" + common.mzpvpdcshsx.ToString("F0");
            tbpvpmmsh.Text = common.pvpmmshxx.ToString("F0") + "→" + common.pvpmmshsx.ToString("F0");

            tbpvedcsh.Text = common.pvedcshxx.ToString("F0") + "→" + common.pvedcshsx.ToString("F0");
            tbhxpvedcsh.Text = common.hxpvedcshxx.ToString("F0") + "→" + common.hxpvedcshsx.ToString("F0");
            tbmzpvedcsh.Text = common.mzpvedcshxx.ToString("F0") + "→" + common.mzpvedcshsx.ToString("F0");
            tbpvemmsh.Text = common.pvemmshxx.ToString("F0") + "→" + common.pvemmshsx.ToString("F0");

            tbmzfmdcshts.Text = common.mzfmdcshts.ToString("P4");
            tbhxmzfmdcshts.Text = common.hxmzfmdcshts.ToString("P4");
            tbmzmzfmdcshts.Text = common.mzmzfmdcshts.ToString("P4");
            tbmzfmmmshts.Text = common.mzfmmmshts.ToString("P4");

            tbwsfmdcshts.Text = common.wsfmdcshts.ToString("P4");
            tbhxwsfmdcshts.Text = common.hxwsfmdcshts.ToString("P4");
            tbmzwsfmdcshts.Text = common.mzwsfmdcshts.ToString("P4");
            tbwsfmmmshts.Text = common.wsfmmmshts.ToString("P4");
            //实战数据
            tbszgj.Text = common.szgj.ToString("F0");
            tbszhxl.Text = common.szhxl.ToString("P4");
            tbszhxiaol.Text = common.szhxiaol.ToString("P4");
            tbszpfl.Text = common.szpfl.ToString("P4");
            tbszjsl.Text = common.szjsl.ToString("P4");
            tbszjnyssh.Text = common.szjnyssh.ToString("F0");
            tbszfyl.Text = common.szfyl.ToString("P4");
            tbpvpszmzl.Text = common.pvpszmzl.ToString("P4");
            tbpveszpll.Text = common.pveszpll.ToString("P4");
            tbpveszspl.Text = common.pveszspl.ToString("P4");
            tbpveszhxl.Text = common.pveszhxl.ToString("P4");
            tbpveszmzl.Text = common.pveszmzl.ToString("P4");
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            planIdx = cbPlan.SelectedIndex;
            SaveData();
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.F1:
                    btnAdd_Click(null, null);
                    break;
                case Keys.F2:
                    btnDelete_Click(null, null);
                    break;
                case Keys.F3:
                    btnLoad_Click(null, null);
                    break;
                case Keys.F4:
                    btnSave_Click(null, null);
                    break;
                case Keys.F5:
                    btnCalc_Click(null, null);
                    break;
                case Keys.F6:
                    btnClose_Click(null, null);
                    break;
                default:
                    break;
            }
        }

        private void cbPlan_SelectedIndexChanged(object sender, EventArgs e)
        {
            planIdx = cbPlan.SelectedIndex;
            toolTip.SetToolTip(cbPlan, cbPlan.SelectedItem.ToString());
            LoadData();
        }

        private void cbdqxf_SelectedIndexChanged(object sender, EventArgs e)
        {
            common.dqxf = cbdqxf.SelectedItem.ToString();
            SetupUI();
        }

        private void numgg_ValueChanged(object sender, EventArgs e)
        {
            common.gg = (int)numgg.Value;
            numjcpf.Value = common.jcpf;
        }
        private void numyq_ValueChanged(object sender, EventArgs e)
        {
            common.yq = (int)numyq.Value;
            numjcpf.Value = common.jcpf;
        }

        private void numsf_ValueChanged(object sender, EventArgs e)
        {
            common.sf = (int)numsf.Value;
            numjcpf.Value = common.jcpf;
        }
        private void numld_ValueChanged(object sender, EventArgs e)
        {
            common.ld = (int)numld.Value;
            numjcpf.Value = common.jcpf;
        }

        private void numfyl_ValueChanged(object sender, EventArgs e)
        {
            if ((int)(common.fy * 100000 / (common.fy + Common.fyxs)) / 100000.0 != Convert.ToDouble(numfyl.Value))
            {
                numfy.Value = (int)(Common.fyxs * 1000 / (1000 - (double)numfyl.Value * 1000.0) - Common.fyxs);
            }
        }

        private void numfy_ValueChanged(object sender, EventArgs e)
        {
            common.fy = (int)numfy.Value;
            numfyl.Value = (decimal)((int)(common.fy * 100000 / (common.fy + Common.fyxs)) / 100000.0);
        }

        private void cbTopmost_CheckedChanged(object sender, EventArgs e)
        {
            TopMost = cbTopmost.Checked;
        }
    }
}
