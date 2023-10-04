using namespace std;
#include <vector>
#include <functional>
typedef pair<int, int> pii;
//求各个节点出发到其他节点的距离和
//从「以 0 为根」换到「以 2 为根」时，原来 2 的子节点还是 2 的子节点，原来  的子节点还是  的子节点，唯一改变的是 0 和 2 的父子关系
//通过这一变化量求解答案
//O(n)
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
        function<void(int, int)> dfs = [&](int x, int p) {//求解从0点出发的答案,且遍历树
            for (auto [y, w] : g[x]) {
                if (y == p) continue;//相同两点的边不查
                dfs(y, x);
                f[x] += f[y] + (w == -1);
            }
        };
        dfs(0, -1);
        function<void(int, int)> reroot = [&](int x, int p) {//根据变化量求解其他节点的答案
            for (auto [y, w] : g[x]) {
                if (y == p) continue;
                f[y] += f[x] - (f[y] + (w == -1)) + (w == 1);
                reroot(y, x);
            }
        };
        reroot(0, -1);
        return f;
    }

    //无向图，连通树(点全部有边相连)；求以各个节点为根出发，到各顶点的距离的数组。
    vector<int> sumOfDistancesInTree(int n, vector<vector<int>>& edges) {
        vector<vector<int> > g(n);
        for (auto& e : edges) {//构建邻接表
            g[e[0]].emplace_back(e[1]);
            g[e[1]].emplace_back(e[0]);
        }
        vector<int> d(n,0);
        vector<int> size(n, 1);
        function<void(int, int, int)> dfs = [&](int x, int p, int depth) {//求解从0点出发的答案,且遍历树
            d[0] += depth;
            for (auto y : g[x]) {
                if (y == p) continue;
                dfs(y, x, depth + 1);
                size[x] += size[y];
            }
        };
        dfs(0, -1, 0);
        function<void(int, int)> reroot = [&](int x, int p) {//根据变化量求解其他节点的答案
            for (auto y : g[x]) {
                if (y == p) continue;
                d[y] += d[x]+n - 2* size[y];
                reroot(y, x);
            }
        };
        reroot(0, -1);
        return d;
    }
};