using namespace std;
#include <vector>
#include <queue>
#include <set>
//线段树
//找最处理任务最多的服务器
//A[i] 记录服务器的下一次有空的开始时间。
//d[i] 记录服务器处理的任务数。
//n个请求*在[i%k,k),[0,i%k)找到最小f[idx]的idx,f[idx]=i+l[i],d[idx]++。
class Solution {
    vector<int>T;//线段树数组
    vector<int>A;//原本的数组
    /// <summary>
    /// 更新线段树上的区间结果
    /// </summary>
    /// <param name="node">当前区间的线段树索引</param>
    /// <param name="l">要更新的原数组当前区间的左边缘元素索引</param>
    /// <param name="r">要更新的原数组当前区间的右边缘元素索引</param>
    void merge(int node, int l, int r) {
        int nl = node << 1, nr = nl | 1;
        //区间最小值，根据区间要存什么更改
        T[node] = min(T[nl],T[nr]);
    }
    /// <summary>
    /// 构建线段树,计算区间属性值(区间最大，最小之类的)，
    /// 计算从下往上，先计算构建子区间,后父区间。
    /// </summary>
    /// <param name="node">当前区间的线段树索引</param>
    /// <param name="l">要构建的原数组当前区间的左边缘元素索引</param>
    /// <param name="r">要构建的原数组当前区间的右边缘元素索引</param>
    void build(int node, int l, int r) {
        if (l == r) {
            //当单元素时为原数组元素本身，根据区间要存什么更改
            T[node] = 0;
            return;
        }
        int m = (l + r) / 2;
        build(node << 1, l, m);
        build((node << 1) | 1, m + 1, r);
        merge(node, l, r);
    }
    /// <summary>
    /// 查询原数组的区间内小于等于v从左往右第一个数的索引。
    /// 如果是单元素了，就修改该元素，否则两分查询目标元素所在的区间，
    /// 直到查询并修改到要修改元素的元素了，修改它，再修改区间数据。
    /// </summary>
    /// <param name="node">当前区间的线段树索引</param>
    /// <param name="l">线段树节点索引指向的原数组区间的左边缘元素索引</param>
    /// <param name="r">线段树节点索引指向的原数组区间的右边缘元素索引</param>
    /// <param name="ql">要查询的区间包含的左边界索引</param>
    /// <param name="qr">要查询的区间包含的右边界索引</param>
    /// <param name="v">要查询的小于的值</param>
    /// <returns>该区间的某属性</returns>
    int query(int node, int l, int r, int ql, int qr,int v) {
        if (l == r) {
            //当单元素时为原数组元素本身，根据区间要存什么更改
            if (l >= ql && l <= qr&&A[l]<=v)
                return l;
            return -1;
        }
        int m = (l + r) / 2;
        int val;
        if (ql <= m&&T[node << 1]<=v) 
            val=query(node<<1, l, m, ql, qr,v);
        if (val!=-1)
            return val;
        if (m <qr && T[node << 1|1] <= v) 
            val=query(node<<1|1, m + 1, r, ql, qr,v);
        return val;
    }
    /// <summary>
    /// 修改原数组元素，然后更新区间数据。
    /// 如果是单元素了，就修改该元素，否则两分查询目标元素所在的区间，
    /// 直到查询并修改到要修改元素的元素了，修改它，再修改区间数据。
    /// </summary>
    /// <param name="node">当前区间的线段树索引</param>
    /// <param name="l">线段树节点索引指向的原数组区间的左边缘元素索引</param>
    /// <param name="r">线段树节点索引指向的原数组区间的右边缘元素索引</param>
    /// <param name="i">原数组要改的索引</param>
    /// <param name="v">原数组要改成的值</param>
    void update(int node, int l, int r, int i, int v) {
        if (l == r) {
            //当单元素时为原数组元素本身，根据区间要存什么更改
            A[l] = v;
            T[node] = v;
            return;
        }
        int m = (l + r) / 2;
        if (i <= m) update(node << 1, l, m, i, v);
        else update(node << 1 | 1, m + 1, r, i, v);
        merge(node, l, r);
    }
public:
    vector<int> busiestServers(int k, vector<int>& a, vector<int>& l) {
        int n = a.size();
        A = vector<int>(k, 0);
        T= vector<int>(4*k, 0);
        vector<int>d(k, 0);
        build(1, 0, k - 1);
        int idx,pos;
        int mx = 0;
        for (int i = 0; i < n; i++) {
            if (T[1] > a[i])
                continue;
            pos = i % k;
            idx = query(1,0,k-1, pos, k - 1,a[i]);
            if (idx==-1) {
                idx = query(1,0,k-1, 0, pos - 1, a[i]);
            }
            update(1, 0, k - 1, idx, a[i] + l[i]);
            d[idx]++;
            mx = max(d[idx], mx);
        }
        vector<int> rst;
        for (int i = 0; i < k; i++) {
            if (d[i] >= mx) {
                rst.push_back(i);
            }
        }
        return rst;
    }
};

//优先队列 有序集合 题解标签再做了下
//将繁忙的放入一个按结束时间排列的优先队列，pq<服务器最早可用时间，服务器索引>，
//有序集合st：存可用的服务器的索引。
//每次查询将可用的从pq里调出来加到
class Solution {
public:
    vector<int> busiestServers(int k, vector<int>& a, vector<int>& l) {
        set<int> st;
        using pii = pair<int, int>;
        priority_queue<pii, vector<pii>, greater<pii>> pq;
        vector<int>d(k, 0);
        int n = a.size();
        for (int i = 0; i < k; i++) {
            st.insert(i);
        }
        int idx;
        set<int>::iterator pos;
        pii cur;
        for (int i = 0; i < n; i++) {
            while (!pq.empty() && pq.top().first <= a[i]) {
                cur = pq.top();
                st.insert(cur.second);
                pq.pop();
            }
            if (st.empty()) {
                continue;
            }
            pos = st.lower_bound(i%k);
            if (pos == st.end()) {
                pos = st.begin();
            }
            idx = *pos;
            d[idx]++;
            pq.push({ a[i] + l[i],idx });
            st.erase(pos);
        }
        int mx = *max_element(d.begin(),d.end());
        vector<int>rst;
        for (int i = 0; i < k; i++) {
            if (d[i] == mx) {
                rst.push_back(i);
            }
        }
        return rst;
    }
};