using namespace std;
#include <vector>
#include <functional>
//无向树的遍历
class Solution {
public:
    int maxKDivisibleComponents(int n, vector<vector<int>>& edges, vector<int>& values, int k) {
        vector<vector<pair<int, int> > > g(n);
        for (auto& e : edges) {//构建邻接表，绘制所有边，出边为正权，入边为负权
            g[e[0]].emplace_back(e[1], 1);
            g[e[1]].emplace_back(e[0], -1);
        }
        int rst = 0;
        function<int(int, int)> dfs = [&](int x, int p) {//求解从0点出发的答案,且遍历树
            int sums = values[x];
            for (auto [y, w] : g[x]) {
                if (y == p) continue;//相同两点的边不查
                sums+=dfs(y, x);
            }
            if (sums % k == 0) {
                rst++;
                return 0;
            }
            return sums;
        };
        dfs(0, -1);
        return rst;
    }
};