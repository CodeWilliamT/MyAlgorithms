using namespace std;
#include <iostream>
#include <vector>
//线段树 模板
//区间修改 懒标记(更新时标记最大子区间，查询还原修复标记区间的子区间)
//单值最大10^9+10^5*10^5=1.1*10^10>2^31
//和最大1.1*10^10*10^5=1.1*10^15
//累计操作 遇到取和时计算

class SegTree {
private:
    vector<int>T;//线段树数组
    vector<bool> lazy;
    int N;

public:
    SegTree(vector<int>& nums) {
        N = nums.size();
        T = vector<int>(N * 4);
        lazy = vector<bool>(N * 4);
        build(1, 0, N - 1, nums);
    }
    void Update(int ul, int ur, int v = 0) {
        update(1, 0, N - 1, ul, ur, v);
    }
    int Query(int ql, int qr) {
        return query(1, 0, N - 1, ql, qr);
    }
private:
    /// <summary>
    /// 构建线段树,计算区间属性值(区间最大，最小之类的)，
    /// 计算从下往上，先计算构建子区间,后父区间。
    /// </summary>
    /// <param name="node">当前区间的线段树索引</param>
    /// <param name="l">要构建的原数组当前区间的左边缘元素索引</param>
    /// <param name="r">要构建的原数组当前区间的右边缘元素索引</param>
    void build(int node, int l, int r, vector<int>& nums) {
        if (l == r) {
            //当单元素时为原数组元素本身，根据区间要存什么更改
            T[node] = nums[l];
            return;
        }
        int m = (l + r) >>1;
        build(node << 1, l, m, nums);
        build((node << 1) | 1, m + 1, r, nums);
        pushUp(node, l, r);
    }

    /// <summary>
    /// 根据左右子区间结果，以及要求什么，更新上级区间结果
    /// </summary>
    /// <param name="node">当前区间的线段树索引</param>
    /// <param name="l">要更新的原数组当前区间的左边缘元素索引</param>
    /// <param name="r">要更新的原数组当前区间的右边缘元素索引</param>
    void pushUp(int node, int l, int r) {
        if (l == r)
            return;
        int nl = node << 1, nr = nl | 1;
        //根据所需区间属性修改
        T[node] = T[nl] + T[nr];
    }

    /// <summary>
    /// 根据标记，更新下级两个区间结果
    /// </summary>
    /// <param name="node">当前区间的线段树索引</param>
    /// <param name="l">要更新的原数组当前区间的左边缘元素索引</param>
    /// <param name="r">要更新的原数组当前区间的右边缘元素索引</param>
    void pushDown(int node, int l, int r) {
        if (l == r|| !lazy[node])
            return;
        int nl = node << 1, nr = nl | 1;
        int m = (l + r) / 2;
        lazy[nl] = !lazy[nl];
        lazy[nr] = !lazy[nr];
        //根据所需区间属性修改
        T[nl] = (m - l + 1) - T[nl];
        T[nr] = (r - (m+1) + 1) - T[nr];
        lazy[node] = 0;
    }
    /// <summary>
    /// 修改原数组元素，然后更新区间数据。
    /// 修改所有最大子区间的变量，并对非单元素作懒惰标记
    /// 直到查询并修改到要修改元素的元素了，修改它，再修改区间数据。
    /// </summary>
    /// <param name="node">当前区间的线段树索引,从1开始</param>
    /// <param name="l">线段树节点索引指向的原数组区间的左边缘元素索引</param>
    /// <param name="r">线段树节点索引指向的原数组区间的右边缘元素索引</param>
    /// <param name="ul">原数组待修改区间左边界索引(包含)</param>
    /// <param name="ur">原数组待修改区间右边界索引(包含)</param>
    /// <param name="v">原数组要改成的值,这里取反所以没用到</param>
    void update(int node, int l, int r, int ul, int ur, int v = 0) {
        if (ul <= l && r <= ur) {
            //当前区间为待修改区间子集，直接通过变化量修改当前区间,不是单元素,就做懒惰标记。
            T[node] = (r - l + 1) - T[node];
            lazy[node] = !lazy[node];
            return;
        }
        pushDown(node, l, r);
        int m = (l + r) / 2;
        if (ul <= m && l<=ur) update(node << 1, l, m, ul, ur, v);
        if (ul <= r && m<ur) update(node << 1 | 1, m + 1, r, ul, ur, v);
        pushUp(node, l, r);
    }


    /// <summary>
    /// 查询原数组的某区间的属性
    /// </summary>
    /// <param name="node">当前区间的线段树索引</param>
    /// <param name="l">当前区间的线段树索引指向的原数组区间的左边缘元素索引</param>
    /// <param name="r">当前区间的线段树索引指向的原数组区间的右边缘元素索引</param>
    /// <param name="ql">要查询的区间左边界索引()</param>
    /// <param name="qr">要查询的区间包含的右边界索引</param>
    /// <returns>该区间的某属性</returns>
    int query(int node, int l, int r, int ql, int qr) {
        if (l >= ql && r <= qr) {
            return T[node];
        }
        if (r < ql || l > qr) {
            return 0;
        }
        pushDown(node,l,r);
        int rst=0;
        int m = (l + r) >> 1;
        if (ql <= m &&  l<=qr)
            rst += query(node << 1, l, m, ql, qr);
        if (ql<=r&&m < qr)
            rst += query(node << 1 | 1, m + 1, r, ql, qr);
        return rst;
    }
};

class Solution {
public:
    vector<long long> handleQuery(vector<int>& nums1, vector<int>& nums2, vector<vector<int>>& queries) {
        int n = nums1.size();
        vector<long long> rst;
        long long sum2 = 0;
        for (int i = 0; i < n;i++) {
            sum2 += nums2[i];
        }
        SegTree tree(nums1);
        for (auto& q : queries) {
            if (q[0]==1) {
                tree.Update(q[1], q[2]);
            }
            else if (q[0] == 2) {
                sum2 += 1ll*q[1] * tree.Query(0,n-1);
            }
            else if (q[0] == 3) {
                rst.push_back(sum2);
            }
        }
        return rst;
    }
};