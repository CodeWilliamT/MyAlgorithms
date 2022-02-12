using namespace std;
#include <vector>
#include <functional>
//深搜 记忆化搜索
//数出1的位置-边界衍生的1的位置=答案
class Solution {
public:
    int numEnclaves(vector<vector<int>>& grid) {
        bool v[501][501]{};
        short dir[4][2] = { {1,0},{0,1},{-1,0},{0,-1} };
        short m = grid.size(), n = grid[0].size();
        short rst = 0;
        for (int i = 0; i < m; i++) {
            for (int j = 0; j < n; j++) {
                rst += grid[i][j];
            }
        }
        function<void(int, int)> dfs=[&](int x, int y) {
            if (x<0||y<0||x>=m||y>=n||v[x][y]|| !grid[x][y])return;
            v[x][y] = 1;
            rst -= 1;
            for (int i = 0; i < 4; i++) {
                dfs(x + dir[i][0], y + dir[i][1]);
            }
        };
        for (int i = 0; i < m; i++) {
            dfs(i, 0);
            dfs(i, n-1);
        }

        for (int j = 0; j < n; j++) {
            dfs(0, j);
            dfs(m-1, j);
        }
        return rst;
    }
};