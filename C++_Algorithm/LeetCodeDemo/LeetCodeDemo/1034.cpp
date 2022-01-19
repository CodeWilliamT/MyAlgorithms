using namespace std;
#include <iostream>
#include <vector>
#include <functional>
//深搜 回溯
//先深搜遍历一次，找出所有边界网格作标记，然后再遍历上色。
class Solution {
public:
    vector<vector<int>> colorBorder(vector<vector<int>>& grid, int row, int col, int color) {
        int m = grid.size();
        int n = grid[0].size();
        int dir[4][2] = { {1,0},{-1,0},{0,1},{0,-1} };
        vector<vector<short>> v(m, vector<short>(n, 0));
        function<void(int, int)> dfs = [&](int x, int y) {
            if (v[x][y])return;
            v[x][y] = 1;
            int xi, yi;
            for (int i = 0; i < 4; i++) {
                xi = x + dir[i][0];
                yi = y + dir[i][1];
                if (xi > -1 && xi<m && yi>-1 && yi < n && grid[xi][yi] == grid[x][y])
                    dfs(xi, yi);
                else
                    v[x][y] = 2;
            }
        };
        dfs(row, col);
        for (int i = 0; i < m; i++) {
            for (int j = 0; j < n; j++) {
                if (v[i][j] == 2)
                    grid[i][j] = color;
            }
        }
        return grid;
    }
};