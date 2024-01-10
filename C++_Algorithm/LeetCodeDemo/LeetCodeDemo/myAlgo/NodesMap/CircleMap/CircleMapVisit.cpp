using namespace std;
#include <vector>
#include <functional>
//深搜(回溯) 图
//有向图中，求以 各个点为起点 的 单路径最多访问点数 的 答案数组。
//
//每个点作为起点都遍历一遍，记录答案。因为遍历过程中会记录其他点的遍历过程，所以O(n)
//遍历时可能有环，环判定：同一点第二次访问记录环深度。
//即便不是基向图也没问题。
//O(n)
class CircleMapVisit {
#define MAXN (int)1e5+1
public:
    //有向图中，求以 各个点为起点 的 单路径最多访问点数 的 答案数组。
    //edges:顶点指向的其他顶点构成的数组。
    //return 每个顶点出发的不分叉路径遇到的最多顶点数
    vector<int> countVisitedNodes(vector<int>& edges) {
        int n = edges.size();
        vector<vector<int> > g(n);
        for (int i = 0; i < n; i++) {//构建邻接表
            g[i].emplace_back(edges[i]);
        }
        vector<int>rst(n, 0);
        int v[MAXN]{};
        int loopflag = -1;
        function<int(int, int, int)> dfs = [&](int x, int p, int depth) {
            if (rst[x]) {
                return rst[x];
            }
            if (v[x]) {
                loopflag = x;
                return depth - v[x];
            }
            v[x] = depth;
            depth++;
            for (auto y : g[x]) {
                int dep = dfs(y, x, depth);
                rst[x] = max(rst[x], dep);
            }
            if (loopflag == -1) {
                rst[x]++;
            }
            if (loopflag == x) {
                loopflag = -1;
            }
            return rst[x];
        };
        for (int i = 0; i < n; i++) {
            if (rst[i])continue;
            loopflag = -1;
            dfs(i, -1, 1);
        }
        return rst;
    }
};