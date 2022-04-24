using namespace std;
#include <vector>
#include <functional>
//回溯 树 邻接表
//构建邻接表，遍历树
//开个数组用作记录从每个节点出发的最长分支长度,取最大的原本的最长分支+1+现在遍历的节点的最长分支长度+1。
class Solution {
public:
    int longestPath(vector<int>& parent, string s) {
        int n = parent.size();
        vector<vector<int>> g(n);
        for (int i = 1; i < n;i++) {
            g[parent[i]].push_back(i);
        }
        vector<int> f(n, 0);
        int rst = 1;
        function<void(int)> dfs = [&](int x) {
            for (auto& e : g[x]) {
                dfs(e);
                if (s[x] != s[e]) {
                    rst = max(f[x] + 2 + f[e], rst);
                    f[x] = max(f[x], 1 + f[e]);
                }
            }
        };
        dfs(0);
        return rst;
    }
};