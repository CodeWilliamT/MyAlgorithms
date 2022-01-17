using namespace std;
#include <iostream>
#include <vector>
//巧思
//遍历每个字符，用f[i][j]记录每个字符为中心的最大扩展个数。
class Solution {
public:
    int countPyramids(vector<vector<int>>& grid) {
        int m = grid.size();
        int n = grid[0].size();
        int rst = 0;
        auto  solve = [&](int row, int col)
        {
            int l, r;
            for (int i = 1;; i++) {
                l = col - i;
                r = col + i;
                if (l<0 || r>n - 1 || !grid[row][l] || !grid[row][r]) {
                    return;
                }
                grid[row][col] = i+1;
            }
        };
        auto  check = [&](int row, int col)
        {
            int l, r;
            for (int i = 1;row-i>-1; i++) {
                l = row - i;
                if (grid[l][col] > i)
                    rst++;
                else
                    break;
            }
            for (int i = 1; row + i < m; i++) {
                r = row + i;
                if (grid[r][col] > i)
                    rst++;
                else
                    break;
            }
        };
        for (int i = 0; i < m; i++) {
            for (int j = 1; j < n - 1; j++) {
                if (grid[i][j])
                    solve(i, j);
            }
        }
        for (int i = 0; i < m; i++) {
            for (int j = 1; j < n - 1; j++) {
                if (grid[i][j])
                    check(i, j);
            }
        }
        return rst;
    }
};