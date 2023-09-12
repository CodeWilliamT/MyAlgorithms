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

class Solution {
    //求前后点间是否有前置关系,
    //多少个点,前、后关系数组，是否从1开始
    vector<vector<bool>> IsPre(int n, vector<vector<int>>& edges, bool from1 = 0) {
        queue<int> q;
        vector<vector<int>> g(n + from1);
        vector<int> in(n + from1);
        vector<vector<bool>> isPre(n + from1, vector<bool>(n + from1, false));
        for (auto& e : edges) {
            g[e[1]].push_back(e[0]);
            in[e[0]]++;
        }
        for (int i = from1; i < n + from1; i++) {
            if (!in[i]) {
                q.push(i);
            }
        }
        int cnt = 0;
        while (!q.empty()) {
            int len = q.size();
            while (len--)
            {
                int cur = q.front();
                //cur的顺序就是拓扑排序的顺序
                cnt++;
                q.pop();
                for (auto& e : g[cur]) {

                    isPre[cur][e] = true;
                    for (int i = from1; i < n+ from1; i++) {
                        isPre[i][e] = isPre[i][e] || isPre[i][cur];
                    }
                    in[e]--;
                    if (!in[e])
                        q.push(e);
                }
            }
        }
        return isPre;
    }
public:
    vector<bool> checkIfPrerequisite(int n, vector<vector<int>>& ps, vector<vector<int>>& qs) {
        vector<vector<bool>> ispre = IsPre(n, ps,false);
        vector<bool> rst;
        for (auto& q : qs) {
            rst.push_back(ispre[q[0]][q[1]]);
        }
        return rst;
    }
};