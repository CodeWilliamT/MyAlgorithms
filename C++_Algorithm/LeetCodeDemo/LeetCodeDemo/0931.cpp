using namespace std;
#include <vector>
//动态规划 贪心
//f[x][y]=m[x][y]+min({f[x-1][y-1],f[x-1][y],f[x-1][y+1]}),为到当前位置的最小和，递推到最后行,rst=min(f[x][y])。
//可以不存f[x-2]的数据，缓存2行。
//n-1行，n列，比3值
//O(n*3*(n-1))
class Solution {
public:
    int minFallingPathSum(vector<vector<int>>& m) {
        int n = m.size(), rst = INT32_MAX;
        vector<vector<int>> f(2, vector<int>(n, 0));
        for (int i = 0; i < n; i++) {
            for (int j = 0; j < n; j++) {
                f[i % 2][j] = INT32_MAX;
                for (int idx = j - 1; idx <= j + 1; idx++) {
                    if (idx >= 0 && idx < n)
                        f[i % 2][j] = min(f[i % 2][j], f[(i + 1) % 2][idx] + m[i][j]);
                }
                if (i == n - 1)rst = min(rst, f[i % 2][j]);
            }
        }
        return rst;
    }
};