using namespace std;
#include <vector>
#include <functional>
#include <bitset>
//回溯 细致条件分析
//反向构图，
//从各节点出发遍历尝试，每次记录，走过的不走
//输出；
class Solution {
public:
    vector<vector<int>> getAncestors(int n, vector<vector<int>>& edges) {
        vector<vector<int>> rst(n);
        vector<set<int>> tmp(n);
        vector<vector<int>> g(n);
        bitset<1000> v;
        vector<int> f;
        for (auto& e : edges) {
            g[e[1]].push_back(e[0]);
        }
        function<void(int)> dfs=[&](int x) {
            for (auto& e : f) {
                tmp[e].insert(x);
            }
            if (v[x]) {
                for (auto& e : f) {
                    for (auto& t : tmp[x]) {
                        tmp[e].insert(t);
                    }
                }
                return;
            }

            v[x] = 1;
            f.push_back(x);
            for (auto&e:g[x]) {
                dfs(e);
            }
            f.pop_back();
        };
        for (int i = 0; i < n; i++) {
            dfs(i);
            f.clear();
        }

        for (int i = 0; i < n; i++) {
            for (auto& e : tmp[i]) {
                rst[i].push_back(e);
            }
        }
        return rst;
    }
};