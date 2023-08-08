using namespace std;
#include "myHeader.h"

class DisjoinSet {
private:
    vector<int> f;
    int N;
public:
    //求某点集群
    int findp(int x) {
        if (x == f[x])return x;
        return findp(f[x]);
    }
    void unionp(int x, int y) {
        int a = findp(x);
        int b = findp(y);
        int p = min(a, b);
        f[a] = f[b] = f[x] = f[y] = p;
    }
    /// <summary>
    /// 根据边集建立并查集
    /// </summary>
    /// <param name="n">点数</param>
    /// <param name="edges">边集合({起点，终点})</param>
    /// <param name="plus">最小编号</param>
    void DisJoin(int n, vector<vector<int>>& edges,int plus=1) {
        f = vector<int>(n + plus);
        N = n;
        for (int i = plus; i < n + plus; i++)
            f[i] = i;
        int cost = 0;
        for (auto& e : edges) {
            int x = findp(e[0]);
            int y = findp(e[1]);//找出集合头顶点编号
            if (x != y) { cost += 1; unionp(x, y); }//若在不同集合则合并
        }
    }
    //数集合数
    int CountSets(int plus = 1) {
        set<int> st;
        int p;
        for (int i = 1 + plus; i < N + plus; i++) {
            p = findp(i);
            if (!st.count(p)) {
                st.insert(p);
            }
        }
        return st.size();
    }
    /// <summary>
    /// 按花费从小到大遍历边，该边俩顶点不在一个并查集，则连接集合、花费+=该边权重。
    /// </summary>
    /// <param name="n">节点数目</param>
    /// <param name="edges">边集合({起点，终点，费用})</param>
    /// <param name="plus">最小编号</param>
    /// <returns>求联通所有点的最小花费，不能连通返回-1</returns>
    int Kruskal(int n, vector<vector<int>>& edges, int plus = 1)
    {
        sort(edges.begin(), edges.end(), [&](vector<int>& a, vector<int>& b) {return a[2] < b[2]; });//给边集按权值从小到大排序
        f = vector<int>(n + plus);
        for (int i = plus; i < n + plus; i++)
            f[i] = i;
        int cost = 0;
        for (auto& e : edges) {
            int x = findp(e[0]);
            int y = findp(e[1]);//找出集合头顶点编号
            if (x != y) { cost += e[2]; unionp(x, y); }//若在不同集合则合并
        }
        //遍历所有点判断是否在同一集合
        for (int i = 1 + plus; i < n + plus; i++) {
            if (findp(plus) != findp(i)) {
                return -1;
            }
        }
        return cost;
    }
};