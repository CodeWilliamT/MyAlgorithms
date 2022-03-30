using namespace std;
#include <iostream>
#include <vector>
//线段树 分治
//求:每次查询修改后的，最长连续子串长度
//每次更改后求与之相连的前后最长重复程度与原本的比大小。
//如何快速获取前后最长重复程度：
//用数组记录该位置该字符在之前及本身总共的重复数，另一个数组从后往前。
//线段树：将区间不断两分，存储第分成的区间的值。
//用一个数组All存储区间内的最长子串长度。
//用一个数组L存储,左边缘连续长度,当前节点区间内左边缘往内的重复子串长度。
//用一个数组R存储,右边缘连续长度,当前节点区间内右边缘往内的重复子串长度。
class Solution {
public:
    int L[400005], R[400005];
    int All[400000];//线段树
    string s;
    /// <summary>
    /// 计算更新区间结果
    /// </summary>
    /// <param name="node">当前区间的线段树索引</param>
    /// <param name="l">当前区间的左边缘元素索引</param>
    /// <param name="r">当前区间的右边缘元素索引</param>
    void merge(int node, int l, int r) {
        int nl = node << 1, nr = nl | 1, m = (l + r) / 2;
        All[node] = max(All[nl], All[nr]);//区间最长重复子串
        L[node] = L[nl];//一般情况下,左边缘连续长度起码等于左子区间的左边缘连续长度
        R[node] = R[nr];//右边缘连续长度起码等于右子区间的左边缘连续长度
        //如果左右子区间边缘字符相等,
        //则计算左子区间右边缘连续长度+右区间左边缘连续长度的和L[nl]+R[nl]，比较是否大于最大值。
        //如果左子区间的最大连续长度等于左区间长度，说明左区间整个连续，
        //则当前节点左边缘连续长应再加右区间的左边缘，右区间同理
        if (s[m] == s[m + 1]) {
            All[node] = max(All[node], R[nl] + L[nr]);
            if (All[nl] == m - l + 1) {
                L[node] += L[nr];
            }
            if (All[nr] == r - m) {
                R[node] += R[nl];
            }
        }
    }
    /// <summary>
    /// 构建线段树,
    /// 计算区间属性值(区间最大，最小之类的)，
    /// 计算从下往上，先计算构建子区间,后父区间。
    /// </summary>
    /// <param name="node">当前区间的线段树索引</param>
    /// <param name="l">当前区间的左边缘元素索引</param>
    /// <param name="r">当前区间的右边缘元素索引</param>
    void build(int node, int l, int r) {
        //如果是单元素区间了，则记录单区间数据。
        if (l == r) {
            L[node] = R[node] = All[node] = 1;
        }
        else {
            int m = (l + r) / 2;
            build(node << 1, l, m);
            build((node << 1) | 1, m + 1, r);
            merge(node, l, r);
        }
    }
    /// <summary>
    /// 查询
    /// 如果是单元素了，就修改该元素，否则两分查询目标元素所在的区间，
    /// 直到查询并修改到要修改元素的元素了，再修改区间数据。
    /// </summary>
    /// <param name="node">当前区间的线段树索引</param>
    /// <param name="l">当前区间的左边缘元素索引</param>
    /// <param name="r">当前区间的右边缘元素索引</param>
    /// <param name="i">要改的索引</param>
    /// <param name="c">要改成的字符</param>
    /// <returns>整个区间的某属性</returns>
    int query(int node, int l, int r, int i, char c) {
        if (l == r) {
            s[l] = c;
        }
        else {
            int m = (l + r) / 2;
            if (i <= m) query(node << 1, l, m, i, c);
            else query((node << 1) | 1, m + 1, r, i, c);
            merge(node, l, r);
        }
        return All[node];
    }
    vector<int> longestRepeating(string s, string qc, vector<int>& qi) {
        this->s = s;

        build(1, 0, s.size() - 1);

        vector<int> res;
        for (int i = 0; i < qc.size(); ++i) {
            res.push_back(query(1, 0, s.size() - 1, qi[i], qc[i]));
        }
        return res;
    }
};