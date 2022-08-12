using namespace std;
#include <iostream>
#include <vector>
#include <functional>
//深搜
class Solution {
public:
    int reachableNodes(int n, vector<vector<int>>& edges, vector<int>& restricted) {
        vector<bool> v(n, 0);
        vector<vector<int>> g(n);
        for (auto& e : edges) {
            g[e[0]].push_back(e[1]);
            g[e[1]].push_back(e[0]);
        }
        for (int& e : restricted) {
            v[e] = 1;
        }
        int rst = 0;
        function<void(int)> dfs = [&](int cur) {
            if (v[cur])return;
            rst++;
            v[cur] = 1;
            for (int& e : g[cur]) {
                dfs(e);
            }
        };
        dfs(0);
        return rst;
    }
};