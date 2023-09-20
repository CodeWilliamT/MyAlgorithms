using namespace std;
#include <vector>
#include <functional>
typedef pair<int, int> pii;
//n点n-1条边，正好树。
//动态规划 换根DP
class Solution {
public:
    vector<int> minEdgeReversals(int n, vector<vector<int>>& edges) {
        vector<vector<pair<int, int> > > g(n);
        for (auto& e : edges) {//构建邻接表，出边为正权，入边为负权
            g[e[0]].emplace_back(e[1], 1);
            g[e[1]].emplace_back(e[0], -1);
        }
        vector<int> dp(n);
        function<void(int, int)> dfs = [&](int u, int p) {
            for (auto [v, w] : g[u]) {
                if (v == p) continue;
                dfs(v, u);
                dp[u] += dp[v] + (w == -1);
            }
        };
        dfs(0, -1);
        function<void(int, int)> change = [&](int u, int p) {
            for (auto [v, w] : g[u]) {
                if (v == p) continue;
                dp[v] += dp[u] - (dp[v] + (w == -1)) + (w == 1);
                change(v, u);
            }
        };
        change(0, -1);
        return dp;
    }
};