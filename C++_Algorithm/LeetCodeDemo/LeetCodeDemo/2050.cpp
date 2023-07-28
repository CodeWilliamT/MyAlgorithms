using namespace std;
#include <iostream>
#include <vector>
#include <queue>
//拓扑排序 求无环图的完成时间
//完成该任务的最小cost:f[x]=time[x-1]+max(f[pre(x)])
//rst=max(f)
class Solution {
public:
    int minimumTime(int n, vector<vector<int>>& relations, vector<int>& time) {
        queue<int> q;
        vector<vector<int>> g(n + 1);
        vector<int> in(n + 1), f(n + 1);
        for (auto& e : relations) {
            g[e[1]].push_back(e[0]);
            in[e[0]]++;
        }
        for (int i = 1; i <= n; i++) {
            if (!in[i]) {
                q.push(i);
            }
            f[i] = time[i-1];
        }
        while (!q.empty()) {
            int cnt = q.size();
            while (cnt--)
            {
                int cur = q.front();
                //cur的顺序就是拓扑排序的顺序
                q.pop();
                for (auto& e : g[cur]) {
                    in[e]--;
                    f[e] = max(f[e], time[e-1] + f[cur]);
                    if (!in[e])
                        q.push(e);
                }
            }
        }
        int rst = 0;
        for (int i = 1; i <= n; i++) {
            rst = max(f[i], rst);
        }
        return rst;
    }
};