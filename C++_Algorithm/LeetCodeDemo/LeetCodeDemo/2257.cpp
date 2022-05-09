using namespace std;
#include <vector>
//麻烦模拟 找规律 细致条件分析
//往下，往右,往上,往左四个方向遍历一遍刷前驱状态。g覆盖空，W停止覆盖，看剩下的空白。
class Solution {
    typedef long long ll;
public:
    int countUnguarded(int m, int n, vector<vector<int>>& guards, vector<vector<int>>& walls) {
        vector<vector<char>> g(m, vector<char>(n, 0));
        for (auto& e : guards) {
            g[e[0]][e[1]] = 'G';
        }
        for (auto& e : walls) {
            g[e[0]][e[1]] = 'W';
        }
        for (int i = 0; i < m; i++) {
            for (int j = 0; j < n; j++) {
                if (i != 0) {
                    if (g[i][j] != 'W' && g[i][j] != 'G')
                    if (g[i - 1][j] == 'G' || g[i - 1][j] == 'D') {
                        g[i][j] = 'D';
                    }
                }
            }
        }
        for (int i = 0; i < m; i++) {
            for (int j = 0; j < n; j++) {
                if (j != 0) {
                    if (g[i][j] != 'W' && g[i][j] != 'G')
                        if (g[i][j-1] == 'G' || g[i][j-1] == 'R') {
                            g[i][j] = 'R';
                        }
                }
            }
        }
        for (int i = m - 1; i >= 0;i--) {
            for (int j = n - 1; j >= 0;j--) {
                if (i!=m-1) {
                    if (g[i][j] != 'W' && g[i][j] != 'G')
                        if (g[i + 1][j] == 'G' || g[i + 1][j] == 'U') {
                            g[i][j] = 'U';
                        }
                }
            }
        }
        for (int i = m - 1; i >= 0; i--) {
            for (int j = n - 1; j >= 0; j--) {
                if (j != n - 1) {
                    if (g[i][j]!='W'&& g[i][j] != 'G')
                        if (g[i][j + 1] == 'G' || g[i][j + 1] == 'L') {
                            g[i][j] = 'L';
                        }
                }
            }
        }

        int rst=0;
        for (int i = 0; i < m; i++) {
            for (int j = 0; j < n; j++) {
                if (!g[i][j])
                    rst++;
            }
        }
        return rst;
    }
};