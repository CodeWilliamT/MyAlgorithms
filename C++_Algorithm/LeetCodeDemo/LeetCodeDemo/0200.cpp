using namespace std;
#include <iostream>
#include <vector>
//����(����),���仯����
//����ȫͼ����ÿ��û����ǵĵ�������ǲ����Ѹ��Ʊ�ǡ�
class Solution {
    void dfs(int row, int col, int v[300][300], vector<vector<char>>& grid)
    {
        int m = grid.size();
        int n = grid[0].size();
        if (grid[row][col] != '0')
        {
            if (row > 0 && !v[row-1][col])
            {
                v[row - 1][col] = v[row][col];
                dfs(row-1, col, v, grid);
            }
            if (col > 0 && !v[row][col-1])
            {
                v[row][col-1] = v[row][col];
                dfs(row, col-1, v, grid);
            }
            if (row < m - 1 && !v[row+1][col])
            {
                v[row + 1][col] = v[row][col];
                dfs(row+1, col, v, grid);
            }
            if (col < n - 1 && !v[row][col+1])
            {
                v[row][col+1] = v[row][col];
                dfs(row, col+1, v, grid);
            }
        }
    }
public:
    int numIslands(vector<vector<char>>& grid) {
        int cnt=0;
        int v[300][300]{};
        int m = grid.size();
        int n = grid[0].size();
        for (int i = 0; i < m; i++)
            for (int j = 0; j < n; j++) {
                if (v[i][j]|| grid[i][j] == '0')continue;
                v[i][j] = ++cnt;
                dfs(i, j,  v, grid);
            }
        return cnt;
    }
};