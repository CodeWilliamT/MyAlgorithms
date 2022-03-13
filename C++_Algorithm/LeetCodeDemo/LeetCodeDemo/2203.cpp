using namespace std;
#include <vector>
#include <queue>
//图 正权最短路
//获取src1,src2到各个点的最小花费，各个点出发到dest的最小花费。
//找到对同一个点和最小即答案。
class Solution {
    using pll = pair<long long, long long>;
    const long long INF = 100000000000;
    /// <summary>
    /// get min cost from source to target.
    /// From Same source to diff target.
    /// use the min cost to max cost to get less cost,
    /// </summary>
    /// <param name="g">g[from].(pair<int,int>){to,cost}</param>
    /// <param name="v">if visited</param>
    /// <param name="d">source to points cost</param>
    /// <param name="src">source</param>
    /// <param name="tgt">target</param>
    /// <returns>min cost from source to target</returns>
    int Dijkstra(vector<vector<pll>>& g, vector<bool>& v, vector<long long>& d,int src,int tgt) {
        priority_queue<pll, vector<pll>, greater<pll> > q;
        int n = g.size();
        for (int i = 0; i < n; i++)
            d[i] = (i == src ? 0 : INF);
        v = vector<bool>(n, 0);
        q.push({ d[src],src });//起点进入优先队列
        while (!q.empty()) {
            pll u = q.top(); q.pop();
            int x = u.second;
            if (v[x])continue;//已经作为过渡点算过,忽略
            v[x] = 1;
            for (auto& e : g[x])//遍历邻接表
                if (d[e.first] > d[x] + e.second) {
                    d[e.first] = d[x] + e.second;//松弛成功，更新d[v[e]];
                    q.push(make_pair(d[e.first], e.first));//加入优先队列
                }
        }
        return d[tgt];
    }
public:
    long long minimumWeight(int n, vector<vector<int>>& edges, int src1, int src2, int dest) {
        vector<vector<pll>> g(n);
        vector<bool> v(n,0);
        vector<long long> dsrc1(n, 0), dsrc2(n, 0),sdest(n,0);
        for (auto& e : edges) {
            g[e[0]].push_back({ e[1],e[2] });
        }
        Dijkstra(g, v, dsrc1, src1, dest);
        Dijkstra(g, v, dsrc2, src2, dest);
        g.clear();
        g = vector<vector<pll>>(n);
        for (auto& e : edges) {
            g[e[1]].push_back({ e[0],e[2] });
        }
        Dijkstra(g, v, sdest, dest, src1);
        long long rst = INF;
        for (int i = 0; i < n; i++) {
            rst = min(rst, dsrc1[i] + dsrc2[i] + sdest[i]);
        }
        return rst== INF ?-1:rst;
    }
};