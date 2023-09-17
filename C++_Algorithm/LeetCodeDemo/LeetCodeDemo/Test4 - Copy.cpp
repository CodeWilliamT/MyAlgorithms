using namespace std;
#include <iostream>
#include <vector>
#include <string>
#include <algorithm>
#include <unordered_set>
#include <unordered_map>
#include <set>
#include <map>
#include <queue>
#include <stack>
#include <functional>
#include <bitset>
#include "myAlgo\Structs\TreeNode.cpp"
typedef long long ll;
typedef pair<ll, ll> pll;
typedef pair<int, int> pii;
//n点n-1条边，正好树。
//动态规划
class Solution {
public:
    vector<int> minEdgeReversals(int n, vector<vector<int>>& edges) {
        vector<vector<pair<int, int> > > g(n);
        for (auto& e : edges) {
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
    }
};