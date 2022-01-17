using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppSettingDll;

namespace JX3Calculater
{
    class Common
    {
        public class Zsxjc
        {
            public string xfm;
            public string zsxm;
            public double zsxgj;
            public double zsxpf;
            public double zsxhx;
            public Zsxjc(string xf)
            {
                xfm = xf;
                zsxm = "身法";
                zsxgj = 0;
                zsxpf = 0;
                zsxhx = 0;
            }

        }
        public Common()
        {
            dqxf = "藏剑";
            xfzsxjc = new Dictionary<string, Zsxjc>();
            foreach (string xfm in xf)
            {
                xfzsxjc.Add(xfm, new Zsxjc(xfm));
            }
        }

        /// <summary>
        /// 加速比率
        /// </summary>
        /// <param name="time">每跳间隔</param>
        /// <param name="n">加速段位</param>
        /// <returns>加速率</returns>
        public double jsbl(double time, int n)
        {
            double zs = Math.Ceiling(time * 16);
            return ((int)(zs / (zs - 1.0 * n) * 1024 - 1024+1)) / 10.0;
        }

        public int G_Multiply(int a, double b)
        {
            return (int)Math.Ceiling(a * b/1.024);
        }

        public int G_Multiply(double a, double b)
        {
            return (int)Math.Ceiling(a * b / 1.024);
        }

        public static string[] xf = { "紫霞", "太虚", "花间", "藏剑", "傲血", "易筋", "冰心", "毒经", "天罗", "惊羽", "焚影", "丐帮", "分山", "莫问", "北傲", "凌海", "隐龙" };
        public static string[] zsx = { "根骨", "元气", "身法", "力道" };
        //基础系数
        public static double gghx;
        public static double yqgj;
        public static double yqpf;
        public static double sfhx;
        public static double ldgj;
        public static double ldpf;
        //伤害系数
        public static double mzxs;
        public static double hxxs;
        public static double hxiaoxs;
        public static double pfxs;
        public static double jsxs;
        public static double wsxs;
        //守御系数
        public static double fyxs;
        public static double yxb;
        public static double spsjb;
        //主属性加成字典
        static Dictionary<string, Zsxjc> xfzsxjc;

        //当前心法
        public string dqxf;

        //主属性
        public string zsxm { get { return xfzsxjc[dqxf].zsxm; } set { xfzsxjc[dqxf].zsxm = value; } }
        public double zsxgj { get { return xfzsxjc[dqxf].zsxgj; } set { xfzsxjc[dqxf].zsxgj = value; } }
        public double zsxpf { get { return xfzsxjc[dqxf].zsxpf; } set { xfzsxjc[dqxf].zsxpf = value; } }
        public double zsxhx { get { return xfzsxjc[dqxf].zsxhx; } set { xfzsxjc[dqxf].zsxhx = value; } }

        //基础
        public int gg;
        public int yq;
        public int sf;
        public int ld;
        //攻击
        public int gj;
        public int jcgj;
        public int mz;
        public int hx;
        public int hxiao;
        public int pf;
        public int js;
        public int ws;
        public int wusxx;
        public int wussx;
        public double wus { get { return (wusxx + wussx) / 2.0; } set { } }
        //守御
        public int fy;
        public double pll;
        public double spl;
        public double hj;
        public double yj;
        //技能
        public int jngsxx;
        public int jngssx;
        public double jngs { get { return (jngsxx + jngssx) / 2.0; } set { } }
        public double jnxs;
        public double jnwsxs;
        //增益
        public double hxzy;
        public double hxiaozy;
        public double szhxiaozy{ get { return hxiaozy != 0.1 ? hxiaozy : 0.102; } set { } }
        public double pfzy;
        public double gjzy;
        public double jszy;
        public double jnzs;
        public double jntszs;
        //技能总增益
        public double jnzzs { get { return (int)((int)((1+jnzs) * (1 + jntszs) *1000/ 1.024)*1.024)/1000.0-1; } set { } }
        public double szjnzs { get { return (jnzzs * 1000+ (int)(1+ jnzzs * 100/5)) /1024.0; } set { } }
        public double fyws;
        //附魔
        public int hxfm;
        public int pffm;
        public int hxiaofm;
        public int gjfm;
        public int jsfm;
        public int zsfm;
        public int wusfm;
        public int mzfm;
        public int wsfm;
        //各种中间量
        bool ng { get { if (zsxm == "根骨" || zsxm == "元气") return true; else return false; } set { } }
        int fjcpf
        {
            get
            {
                switch (zsxm)
                {
                    case "根骨":
                        return (int)(gg * zsxpf);
                    case "元气":
                        return (int)(yq * zsxpf);
                    case "身法":
                        return (int)(sf * zsxpf);
                    case "力道":
                        return (int)(ld * zsxpf);
                    default:
                        return 0;
                }
            }
            set { }
        }

        public int jcpf { get { return pf - fjcpf; } set { } }
        //输入量百分比变式
        public double mzl { get { return 0.919 + (int)(1024 * mz / 100.0 / mzxs) / 1000.0; } set { } }
        public double hxl { get { return (int)(1024 * hx / 100.0 / hxxs) / 1000.0; } set { } }
        public double hxiaol { get { return 1.75 + (int)(1024 * hxiao / 100.0 / hxiaoxs) / 1000.0; } set { } }
        public double pfl { get { return (int)(1024 * pf / 100.0 / pfxs) / 1000.0; } set { } }
        public double jsl { get { return (int)(1024 * js / 100.0 / jsxs) / 1000.0; } set { } }
        public double wsl { get { return (int)(1024 * ws / 100.0 / wsxs) / 1000.0; } set { } }
        public double yx { get { return yj * yxb; } set { } }
        public double fyl { get { return (int)(fy * 1024 / (fy + fyxs)) / 1000.0; } set { } }
        //GCD加速阈值表
        public int ydjs { get { return G_Multiply(jsbl(1.5, 0), jsxs); } set { } }
        public int edjs { get { return G_Multiply(jsbl(1.5, 1), jsxs); } set { } }
        public int sdjs { get { return G_Multiply(jsbl(1.5, 2), jsxs); } set { } }
        public int sidjs { get { return G_Multiply(jsbl(1.5, 3), jsxs); } set { } }
        public int wdjs { get { return G_Multiply(jsbl(1.5, 4), jsxs); } set { } }

        //实战伤害面板
        public int szgj { get { return (int)(gj + jcgj * gjzy); } set { } }
        public double szhxl { get { return hxl + hxzy<1? hxl + hxzy:1; } set { } }
        public double szhxiaol { get { return hxiaol + szhxiaozy<3? hxiaol + szhxiaozy : 3; } set { } }
        public double szpfl { get { return (int)(1024 * (pf + jcpf * pfzy) / 100.0 / pfxs) / 1000.0; } set { } }
        public int szjs { get { return (int)(js + jszy * 100.0 * jsxs / 1.024 + 0.5); } set { } }
        public double szjsl { get { return (int)(1024 * szjs / 100.0 / jsxs) / 1000.0; } set { } }
        public int szjngs { get { return (int)(jngs * (1 + szjnzs)); } set { } }
        public int szjngsxx { get { return jngsxx; } set { } }
        public int szjngssx { get { return jngssx; } set { } }
        public double szjnxs { get { return jnxs; } set { } }
        public double szjnwsxs { get { return jnwsxs; } set { } }
        public int szjnyssh { get { return (int)((int)(1024 * (1 + szjnzs) * (szjngs + (int)(szjnxs * szgj) + szjnwsxs * wus))/1024.0); } set { } }
        public double szjnysshxx { get { return (int)((int)(1024 * (1 + szjnzs) * (szjngsxx + (int)(szjnxs * szgj) + (int)(szjnwsxs * wusxx)) ) / 1024.0); } set { } }
        public double szjnysshsx { get { return (int)((int)(1024 * (1 + szjnzs) * (szjngssx + (int)(szjnxs * szgj) + (int)(szjnwsxs * wussx)) )/ 1024.0); } set { } }
        public double pvpszmzl { get { return mzl > 1 ? 1 - szhxl + yj : mzl - szhxl + yj; } set { } }
        public double szfyl { get { return (int)(fy * (1 - fyws) * 1024 / (fy * (1 - fyws) + fyxs)) / 1000.0; } set { } }
        public double pveszpll { get { return mzl > 1.024 + pll ? 0 : 1.024 + pll - mzl; } set { } }
        public double pveszspl { get { return spl - wsl > 0 ? spl - wsl : 0; } set { } }
        public double pveszhxl { get { return 1.024 - pveszpll - pveszspl > szhxl ? szhxl : 1 - pveszpll - pveszspl; } set { } }
        public double pveszmzl { get { return 1.024 - pveszpll - pveszspl - pveszhxl; } set { } }

        public double hxfmszhxl { get { return szhxl + (int)(1024 * hxfm / 100.0 / hxxs) / 1000.0; } set { } }
        public double pffmszpfl { get { return (int)(1024 * (pf + pffm + (jcpf + pffm) * pfzy) / 100.0 / pfxs) / 1000.0; } set { } }
        public double hxiaofmszhxiaol { get { return szhxiaol + (int)(1024 * hxiaofm / 100.0 / hxiaoxs) / 1000.0; } set { } }
        public int gjfmszgj { get { return (int)(gj + gjfm + (jcgj + gjfm) * gjzy); } set { } }
        public int gjfmszjnyssh { get { return (int)((int)(1024 * (1 + szjnzs) * (szjngs + (int)(szjnxs * gjfmszgj) + (int)(szjnwsxs * wus)))/1024.0); } set { } }
        public int jsfmszjs { get { return (int)szjs + jsfm; } set { } }
        public double jsfmszgcd { get { return jsfmszjs > wdjs ? 1.19 : jsfmszjs > sidjs ? 1.25 : jsfmszjs > sdjs ? 1.31 : jsfmszjs > edjs ? 1.38 : jsfmszjs > ydjs ? 1.44 : 1.5; } set { } }
        public int zsfmzjgj
        {
            get
            {
                switch (zsxm)
                {
                    case "根骨":
                        return (int)(zsfm * zsxgj);
                    case "元气":
                        return (int)(zsfm * (yqgj + zsxgj));
                    case "身法":
                        return (int)(zsfm * zsxgj);
                    case "力道":
                        return (int)(zsfm * (ldgj + zsxgj));
                    default:
                        return 0;
                }
            }
            set { }
        }
        public int zsfmzjjcgj
        {
            get
            {
                switch (zsxm)
                {
                    case "根骨":
                        return 0;
                    case "元气":
                        return (int)(zsfm * yqgj);
                    case "身法":
                        return 0;
                    case "力道":
                        return (int)(zsfm * ldgj);
                    default:
                        return 0;
                }
            }
            set { }
        }
        public int zsfmszgj { get { return (int)(gj + zsfmzjgj + (jcgj+ zsfmzjjcgj) * gjzy); } set { } }
        public double zsfmszhxl
        {
            get
            {
                switch (zsxm)
                {
                    case "根骨":
                        return szhxl + (int)(1024 * zsfm * (gghx + zsxhx) / 100.0 / hxxs) / 1000.0;
                    case "元气":
                        return szhxl + (int)(1024 * zsfm * zsxhx / 100.0 / hxxs) / 1000.0;
                    case "身法":
                        return szhxl + (int)(1024 * zsfm * (sfhx + zsxhx) / 100.0 / hxxs) / 1000.0;
                    case "力道":
                        return szhxl + (int)(1024 * zsfm * zsxhx / 100.0 / hxxs) / 1000.0;
                    default:
                        return 0;
                }
            }
            set { }
        }
        public double zsfmszpfl
        {
            get
            {
                switch (zsxm)
                {
                    case "根骨":
                        return (int)(1024 * (pf + zsfm * zsxpf + jcpf * pfzy) / 100.0 / pfxs) / 1000.0;
                    case "元气":
                        return (int)(1024 * (pf + zsfm * (yqpf + zsxpf) + (jcpf + zsfm * yqpf) * pfzy) / 100.0 / pfxs) / 1000.0;
                    case "身法":
                        return (int)(1024 * (pf + zsfm * zsxpf + jcpf * pfzy) / 100.0 / pfxs) / 1000.0;
                    case "力道":
                        return (int)(1024 * (pf + zsfm * (ldpf + zsxpf) + (jcpf + zsfm * ldpf) * pfzy) / 100.0 / pfxs) / 1000.0;
                    default:
                        return 0;
                }
            }
            set { }
        }
        public double wusfmszwus { get { return wus + wusfm; } set { } }
        public int wusfmszjnyssh { get { return (int)((int)(1024 * (1 + szjnzs) * (szjngs + (int)(szjnxs * szgj) + szjnwsxs * wusfmszwus))/1024.0); } set { } }
        public double mzfmpveszpll { get { return mzl + (int)(1024 * mzfm / mzxs / 100.0) / 1000.0 > 1.024 + pll ? 0 : 1.024 + pll - (mzl + (int)(1024 * mzfm / mzxs / 100.0) / 1000.0); } set { } }

        public double mzfmpveszhxl { get { return 1.024 - mzfmpveszpll - pveszspl > szhxl ? szhxl : 1.024 - mzfmpveszpll - pveszspl; } set { } }
        public double mzfmszmzl { get { return 1.024 - mzfmpveszpll - pveszspl - mzfmpveszhxl; } set { } }
        public double wsfmszspl { get { return spl - (wsl + (int)(1024 * wsfm / wsxs / 100.0) / 1000.0) > 0 ? spl - (wsl + (int)(1024 * wsfm / wsxs / 100.0) / 1000.0) : 0; } set { } }
        public double wsfmpveszhxl { get { return 1.024 - pveszpll - wsfmszspl > szhxl ? szhxl : 1.024 - pveszpll - wsfmszspl; } set { } }
        public double wsfmpveszmzl { get { return 1.024 - pveszpll - wsfmszspl - wsfmpveszhxl; } set { } }

        //输出
        //均值
        public double gcd { get { return szjs > wdjs ? 1.19 : szjs > sidjs ? 1.25 : szjs > sdjs ? 1.31 : szjs > edjs ? 1.38 : szjs > ydjs ? 1.44 : 1.5; } set { } }
        public double dcsh { get { return (szhxl * hxdcsh + (1 - szhxl)* mzdcsh); } set { } }
        public double hxdcsh { get { return mzdcsh * 1.75+ mzdcsh * (szhxiaol-1.75)/ 1.024; } set { } }
        public double mzdcsh { get { return szjnyssh * (1.024 + szpfl) / 1.024; } set { } }
        public double mmsh { get { return dcsh / gcd; } set { } }
        //下限
        public double dcshxx { get { return (szhxl * hxdcshxx + (1 - szhxl) * mzdcshxx); } set { } }
        public double hxdcshxx { get { return mzdcshxx * 1.75 + mzdcshxx * (szhxiaol - 1.75) / 1.024; } set { } }
        public double mzdcshxx { get { return szjnysshxx * (1.024 + szpfl) / 1.024; } set { } }
        public double mmshxx { get { return dcshxx / gcd; } set { } }
        //上限
        public double dcshsx { get { return (szhxl * hxdcshsx + (1 - szhxl) * mzdcshsx); } set { } }
        public double hxdcshsx { get { return mzdcshsx * 1.75 + mzdcshsx * (szhxiaol - 1.75) / 1.024; } set { } }
        public double mzdcshsx { get { return szjnysshsx * (1.024 + szpfl) / 1.024; } set { } }
        public double mmshsx { get { return dcshsx / gcd; } set { } }

        public double hxfmdcshts { get { return ((hxfmszhxl * hxdcsh + (1 - hxfmszhxl) * mzdcsh) - dcsh) / dcsh; } set { } }
        public double hxhxfmdcshts { get { return 0; } set { } }
        public double mzhxfmdcshts { get { return 0; } set { } }
        public double hxfmmmshts { get { return hxfmdcshts; } set { } }
        public double pffmdcshts { get { return mzpffmdcshts; } set { } }
        public double hxpffmdcshts { get { return mzpffmdcshts; } set { } }
        public double mzpffmdcshts { get { return (szjnyssh * (1.024 + pffmszpfl) / 1.024 - mzdcsh) / mzdcsh; } set { } }
        public double pffmmmshts { get { return pffmdcshts; } set { } }
        public double hxiaofmdcshts { get { return szhxl * hxhxiaofmdcshts; } set { } }
        public double hxhxiaofmdcshts { get { return (mzdcsh * 1.75 + mzdcsh * (hxiaofmszhxiaol - 1.75) / 1.024-hxdcsh)/hxdcsh; } set { } }
        public double mzhxiaofmdcshts { get { return 0; } set { } }
        public double hxiaofmmmshts { get { return hxiaofmdcshts; } set { } }
        public double gjfmdcshts { get { return mzgjfmdcshts; } set { } }
        public double hxgjfmdcshts { get { return mzgjfmdcshts; } set { } }
        public double mzgjfmdcshts { get { return (gjfmszjnyssh * (1.024 + szpfl) / 1.024 - mzdcsh) / mzdcsh; } set { } }
        public double gjfmmmshts { get { return gjfmdcshts; } set { } }
        public double jsfmdcshts { get { return 0; } set { } }
        public double hxjsfmdcshts { get { return 0; } set { } }
        public double mzjsfmdcshts { get { return 0; } set { } }
        public double jsfmmmshts { get { return (dcsh / jsfmszgcd - dcsh / gcd) / (dcsh / gcd); } set { } }
        public double zsfmdcshts { get { return zsfmszhxl * hxzsfmdcshts + (1 - zsfmszhxl) * mzzsfmdcshts; } set { } }
        public double hxzsfmdcshts { get { return mzzsfmdcshts; } set { } }
        public double mzzsfmdcshts { get { return ((int)((int)(1024 * (1 + szjnzs) * (jngs + szjnxs * zsfmszgj + szjnwsxs * wus))/1024.0) * (1.024 + zsfmszpfl) / 1.024 - mzdcsh) / mzdcsh; } set { } }
        public double zsfmmmshts { get { return zsfmdcshts; } set { } }
        public double wusfmdcshts { get { return mzwusfmdcshts; } set { } }
        public double hxwusfmdcshts { get { return mzwusfmdcshts; } set { } }
        public double mzwusfmdcshts { get { return (wusfmszjnyssh * (1.024 + szpfl) / 1.024 - mzdcsh) / mzdcsh; } set { } }
        public double wusfmmmshts { get { return wusfmdcshts; } set { } }
        //均值
        public double pvpdcsh { get { return (szhxl - yj)* hxpvpdcsh+pvpszmzl* mzpvpdcsh; } set { } }
        public double hxpvpdcsh { get { return mzpvpdcsh * 1.75+ mzpvpdcsh*(szhxiaol-1.75) / 1.024 * (1 - yx); } set { } }
        public double mzpvpdcsh { get { return szjnyssh * (1.024 + szpfl) * (1 - hj) * (1 - szfyl / 1.024) / 1.024; } set { } }
        public double pvpmmsh { get { return pvpdcsh / gcd; } set { } }
        //下限
        public int pvpdcshxx { get { return (int)((szhxl - yj) * hxpvpdcshxx + pvpszmzl * mzpvpdcshxx); } set { } }
        public int hxpvpdcshxx { get { return (int)(mzpvpdcshxx * 1.75 + mzpvpdcshxx * (szhxiaol - 1.75) / 1.024 * (1 - yx)); } set { } }
        public int mzpvpdcshxx { get { return (int)(szjnysshxx * (1.024 + szpfl) * (1 - hj) * (1 - szfyl / 1.024) / 1.024); } set { } }
        public double pvpmmshxx { get { return (int)pvpdcshxx / gcd; } set { } }
        //上限
        public int pvpdcshsx { get { return (int)((szhxl - yj) * hxpvpdcshsx + pvpszmzl * mzpvpdcshsx); } set { } }
        public int hxpvpdcshsx { get { return (int)(mzpvpdcshsx * 1.75 + mzpvpdcshsx * (szhxiaol - 1.75) / 1.024 * (1 - yx)); } set { } }
        public int mzpvpdcshsx { get { return (int)(szjnysshsx * ((1.024 + szpfl) - (int)((1.024 + szpfl)*1000*szfyl/1.024)/1000.0) * (1 - hj) / 1.024); } set { } }
        public double pvpmmshsx { get { return pvpdcshsx / gcd; } set { } }
        //均值
        public double pvedcsh { get { return mzpvedcsh * pveszspl * spsjb / 1.024+ pveszhxl * hxpvedcsh + pveszmzl* mzpvedcsh ; } set { } }
        public double hxpvedcsh { get { return mzpvedcsh*1.75 + mzpvedcsh*(szhxiaol-1.75) / 1.024; } set { } }
        public double mzpvedcsh { get { return szjnyssh * ((1.024 + szpfl) - (int)((1.024 + szpfl) * 1000 * szfyl / 1.024) / 1000.0) / 1.024; } set { } }
        public double pvemmsh { get { return pvedcsh / gcd; } set { } }
        //下限
        public int pvedcshxx { get { return (int)(mzpvedcshxx * pveszspl * spsjb / 1.024 + pveszhxl * hxpvedcshxx + pveszmzl * mzpvedcshxx); } set { } }
        public int hxpvedcshxx { get { return (int)(mzpvedcshxx * 1.75) + (int)(mzpvedcshxx * (szhxiaol - 1.75) / 1.024); } set { } }
        public int mzpvedcshxx { get { return (int)(szjnysshxx * ((1.024 + szpfl) - (int)((1.024 + szpfl) * 1000 * szfyl / 1.024) / 1000.0) / 1.024); } set { } }
        public double pvemmshxx { get { return pvedcshxx / gcd; } set { } }
        //上限
        public int pvedcshsx { get { return (int)(mzpvedcshsx * pveszspl * spsjb / 1.024 + pveszhxl * hxpvedcshsx + pveszmzl * mzpvedcshsx); } set { } }
        public int hxpvedcshsx { get { return (int)(mzpvedcshsx * 1.75) + (int)(mzpvedcshsx * (szhxiaol - 1.75) / 1.024); } set { } }
        public int mzpvedcshsx { get { return (int)(szjnysshsx * ((1.024 + szpfl) - (int)((1.024 + szpfl) * 1000 * szfyl / 1.024) / 1000.0) / 1.024); } set { } }
        public double pvemmshsx { get { return pvedcshsx / gcd; } set { } }

        public double mzfmdcshts { get { return (mzpvedcsh * pveszspl * spsjb / 1.024+ mzfmpveszhxl * hxpvedcsh + mzfmszmzl * mzpvedcsh - pvedcsh) / pvedcsh; } set { } }
        public double hxmzfmdcshts { get { return 0; } set { } }
        public double mzmzfmdcshts { get { return 0; } set { } }
        public double mzfmmmshts { get { return mzfmdcshts; } set { } }

        public double wsfmdcshts { get { return (mzpvedcsh * wsfmszspl * spsjb / 1.024 + wsfmpveszhxl * hxpvedcsh + wsfmpveszmzl * mzpvedcsh - pvedcsh) / pvedcsh; } set { } }
        public double hxwsfmdcshts { get { return 0; } set { } }
        public double mzwsfmdcshts { get { return 0; } set { } }
        public double wsfmmmshts { get { return wsfmdcshts; } set { } }

        public void Save(string key)
        {
            AppSetting.SaveObj(xfzsxjc[dqxf], dqxf);
            AppSetting.SaveObj(this, "common" + key);
        }
        public void Load(string key)
        {
            AppSetting.LoadObj(this, "common" + key);
            foreach (string xfm in xf)
            {
                AppSetting.LoadObj(xfzsxjc[xfm], xfm);
            }
        }
    }
}
