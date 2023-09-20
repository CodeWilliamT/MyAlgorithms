using namespace std;
#include <vector>
#include <functional>
typedef pair<int, int> pii;
//求各个节点出发到其他节点的距离和
//On
class SwitchRootDP {
public:
    //有向图，连通树(点全部有边相连)；求以各个节点为根出发，各自总共反转多少次有向边则到达全部点的数组。
    vector<int> minEdgeReversals(int n, vector<vector<int>>& edges) {
        vector<vector<pair<int, int> > > g(n);
        for (auto& e : edges) {//构建邻接表，绘制所有边，出边为正权，入边为负权
            g[e[0]].emplace_back(e[1], 1);
            g[e[1]].emplace_back(e[0], -1);
        }
        vector<int> f(n);
        function<void(int, int)> dfs = [&](int u, int p) {//从0点出发到各点的需反转边数和
            for (auto [v, w] : g[u]) {
                if (v == p) continue;//相同两点的边不查
                dfs(v, u);
                f[u] += f[v] + (w == -1);
            }
        };
        dfs(0, -1);
        function<void(int, int)> reroot = [&](int u, int p) {
            for (auto [v, w] : g[u]) {
                if (v == p) continue;
                f[v] += f[u] - (f[v] + (w == -1)) + (w == 1);
                reroot(v, u);
            }
        };
        reroot(0, -1);
        return f;
    }

    //无向图，连通树(点全部有边相连)；求以各个节点为根出发，到各顶点的距离的数组。
    vector<int> sumOfDistancesInTree(int n, vector<vector<int>>& edges) {
        vector<vector<int> > g(n);
        for (auto& e : edges) {//构建邻接表，绘制所有边，出边为正权，入边为负权,
            g[e[0]].emplace_back(e[1]);
            g[e[1]].emplace_back(e[0]);
        }
        vector<int> d(n,0);
        vector<int> size(n, 1);
        function<void(int, int, int)> dfs = [&](int u, int p, int depth) {
            d[0] += depth;
            for (auto v : g[u]) {
                if (v == p) continue;
                dfs(v, u, depth + 1);
                size[u] += size[v];
            }
        };
        dfs(0, -1, 0);
        function<void(int, int)> reroot = [&](int u, int p) {
            for (auto v : g[u]) {
                if (v == p) continue;
                d[v] += d[u]+n - 2* size[v];
                reroot(v, u);
            }
        };
        reroot(0, -1);
        return d;
    }
};