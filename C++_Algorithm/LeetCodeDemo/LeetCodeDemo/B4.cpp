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
//无向图中找一条4节点的序列使得分数最大。
//枚举 贪心 哈希
//由图建立邻接表。
//邻接表按权大到小排序。
//枚举中间边集合。
class Solution {
public:
    int maximumScore(vector<int>& scores, vector<vector<int>>& edges) {
        int n = scores.size();
        vector<vector<int>> g(n);
        for (auto& e : edges) {
            g[e[0]].push_back(e[1]);
            g[e[1]].push_back(e[0]);
        }
        for (auto& v : g) {
            sort(v.begin(), v.end(), [&](int& a, int& b) {return scores[a] > scores[b]; });
        }
        int rst = -1;
        int tmp;
        for (auto&e: edges) {
            for (int i = 0; i < 4 && i < g[e[0]].size(); i++) {
                for (int j = 0; j < 4 && j < g[e[1]].size(); j++) {
                    if (e[0] != g[e[1]][j] && e[1] != g[e[0]][i] && g[e[0]][i] != g[e[1]][j]) {
                        tmp = scores[e[0]] + scores[e[1]] + scores[g[e[0]][i]] + scores[g[e[1]][j]];
                        rst = max(rst, tmp);
                    }
                }
            }
        }
        return rst;
    }
};