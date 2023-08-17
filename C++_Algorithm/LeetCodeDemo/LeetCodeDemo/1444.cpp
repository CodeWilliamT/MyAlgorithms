using namespace std;
#include <vector>
//100!/90!>13!
//不能暴力枚举
//动态规划
//aps[i][i]:i,j与n-1,m-1围的矩形的苹果数
//有苹果再讨论切的方案。
//f[z][i][j]=等宽大矩且多果切的方案和+等高大矩且多果切的方案和。
class Solution {
public:
    int ways(vector<string>& pizza, int k) {
        int aps[50][50]{};
        int f[11][50][50]{};
        int n = pizza.size();
        int m = pizza[0].size();
        int mod = 1e9 + 7;
        for (int i = n - 1; i > -1; i--) {
            for (int j = m - 1; j > -1; j--) {
                aps[i][j] = (pizza[i][j] == 'A') + (i < n - 1 ? aps[i + 1][j] : 0)
                    + (j < m - 1 ? aps[i][j + 1] : 0)
                    - (i < n - 1 && j < m - 1 ? aps[i + 1][j + 1] : 0);
            }
        }
        if (k == 1)return aps[0][0]?1:0;
        f[0][0][0] = 1;
        int rst = 0;
        for (int z = 1; z < k; z++) {
            for (int i = 0; i < n; i++) {
                for (int j = 0; j < m; j++) {
                    if (i == 0 && j == 0)continue;
                    if (!aps[i][j])break;
                    for (int x = 0; i - x > -1; x++) {
                        if (aps[i][j] < aps[i - x][j]) {
                            f[z][i][j] = (f[z][i][j] + f[z - 1][i - x][j]) % mod;
                        }
                    }
                    for (int y = 0; j - y > -1; y++) {
                        if (aps[i][j] < aps[i][j - y]) {
                            f[z][i][j] = (f[z][i][j] + f[z - 1][i][j - y]) % mod;
                        }
                    }
                    if (z == k-1)
                        rst = (rst + f[z][i][j]) % mod;
                }
            }
        }
        return rst;
    }
};