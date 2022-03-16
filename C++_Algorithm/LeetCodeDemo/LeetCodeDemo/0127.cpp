using namespace std;
#include <vector>
#include <queue>
//最短路
//确定，b对应的索引集作为target，e对应的索引作为source。
//计算列表内e出发到各个单词的最短路。
//找出source到target的最短路。
class Solution {
    using pii = pair<int, int>;
    const int INF = 1000000000;
    /// <summary>
    /// 求从源点到终点的最短距离。
    /// 求从单个源点出发，到各个节点的最短路(最小花费)
    /// 尝试从源点src到某顶点d[v]的花费是否可变为更短的起点src到更近的点u花费d[u]+更近的点u到该点v的边的花费e.second。
    /// 做完整个数组，就能得到从起点出发到各个点的最小花费的数组d。
    /// </summary>
    /// <param name="g">g[from].(pair<long,long>){to,cost}</param>
    /// <param name="v">if visited</param>
    /// <param name="d">从源点到各个点的最小花费</param>
    /// <param name="src">source</param>
    void Dijkstra(vector<vector<pii>>& g, vector<bool>& v, vector<int>& d, int src) {
        priority_queue<pii, vector<pii>, greater<pii> > q;
        int n = g.size();
        for (int i = 0; i < n; i++)
            d[i] = (i == src ? 0 : INF);
        v = vector<bool>(n, 0);
        q.push({ d[src],src });//起点进入优先队列
        while (!q.empty()) {
            pii u = q.top(); q.pop();
            int x = u.second;
            if (v[x])continue;//已经作为过渡点算过,忽略
            v[x] = 1;
            for (auto& e : g[x])//遍历邻接表
                if (d[e.first] > d[x] + e.second) {
                    d[e.first] = d[x] + e.second;//松弛成功，更新d[v[e]];
                    q.push(make_pair(d[e.first], e.first));//加入优先队列
                }
        }
    }
public:
    int ladderLength(string b, string e, vector<string>& w) {
        int n = w.size();
        int len = b.size();
        vector<int> tgt;
        int src = -1;
        int diff;
        for (int i = 0; i < n; i++) {
            if (w[i] == e) {
                src = i;
            }
            diff = 0;
            for (int j = 0; j < len; j++) {
                if (w[i][j] != b[j]) {
                    diff++;
                }
            }
            if (diff == 1) {
                tgt.push_back(i);
            }
        }
        if (src == -1 || !tgt.size())
            return 0;
        vector<vector<pii>> g(n);
        vector<bool> v(n,0);
        for (int i = 0; i < n; i++) {
            for (int j = i+1; j < n; j++) {
                diff = 0;
                for (int k = 0; k < len; k++) {
                    if (w[i][k] != w[j][k]) {
                        diff++;
                    }
                }
                if (diff == 1) {
                    g[i].push_back({ j,1 });
                    g[j].push_back({ i,1 });
                }
            }
        }
        vector<int> d(n, INF);
        Dijkstra(g, v, d, src);
        int rst = INF;
        for (int i = 0; i < tgt.size(); i++) {
            rst = min(rst, d[tgt[i]]);
        }
        return rst != INF ? rst + 2 : 0;
    }
};