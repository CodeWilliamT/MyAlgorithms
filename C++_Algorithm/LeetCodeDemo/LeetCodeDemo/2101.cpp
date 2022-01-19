using namespace std;
#include <vector>
#include <functional>
//图论 邻接表遍历 回溯 深搜
//图论 有向图的最长连接数
//先通过可触发爆炸的有向关系构建有向图的邻接表，然后以各个节点为起点回溯遍历有向邻接表 ，拿大的
class Solution {
    bool check(vector<int>& a, vector<int>& b)
    {
        double mx = a[2];
        double r = sqrt(pow(a[0] - b[0], 2) + pow(a[1] - b[1], 2));
        return r<=mx;
    }
public:
    int maximumDetonation(vector<vector<int>>& bombs) {
        int n = bombs.size();
        vector<vector<int>> f(n);
        for (int i = 0; i < n; i++) {
            for (int j = i+1; j < n; j++) {
                if (check(bombs[i], bombs[j])) {
                    f[i].push_back(j);
                }
                if (check(bombs[j], bombs[i])) {
                    f[j].push_back(i);
                }
            }
        }
        bool v[100]{};
        function<int(int)> dfs = [&](int x) {
            v[x] = 1;
            int sum = 1;
            for (int i = 0; i < f[x].size(); i++)
            {
                if(!v[f[x][i]])
                    sum += dfs(f[x][i]);
            }
            return sum;
        };
        int rst = 0;
        for (int i = 0; i < n; i++) {
            rst=max(rst,dfs(i));
            memset(v, 0, sizeof(v));
        }
        return rst;
    }
};