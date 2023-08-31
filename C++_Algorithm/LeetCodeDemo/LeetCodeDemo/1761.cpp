using namespace std;
#include <vector>
//分析 复杂模拟 O(64e6)
//求无向图联通三元组度数的最小值
//先找联通三元组，再找度
//联通三元组：根据边集，构建位图。遍历，点，点后面的点，后面点的后面点：若三点相连（构建位图O(1)，判定）则构成三元组。
//三元组的度：三个点的总度-6。(统计各个点的度)
//最小的是答案。
class Solution {
#define MAXN 401
public:
    int minTrioDegree(int n, vector<vector<int>>& edges) {
        bool g[MAXN][MAXN]{};
        int lk[MAXN]{};
        for (auto& e : edges) {
            if (e[0] > e[1])swap(e[0], e[1]);
            g[e[0]][e[1]] = 1;
            lk[e[0]]++;
            lk[e[1]]++;
        }
        int rst = INT32_MAX;
        for (int i = 1; i <=n; i++) {
            for (int j = i+1; j <= n; j++) {
                for (int k = j + 1; k <= n; k++) {
                    if (g[i][j] && g[i][k] && g[j][k]) {
                        rst = min(lk[i] + lk[j] + lk[k] - 6, rst);
                    }
                }
            }
        }
        return rst== INT32_MAX?-1:rst;
    }
};