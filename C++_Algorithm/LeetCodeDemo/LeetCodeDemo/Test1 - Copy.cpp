using namespace std;
#include <iostream>
#include <vector>
//简单模拟
//先求行上每3个的最大值，形成一个为n*(n-2)的数组。
//再求结果。
class Solution {
public:
    vector<vector<int>> largestLocal(vector<vector<int>>& grid) {
        int n = grid.size();
        vector<vector<int>> g(n, vector<int>(n - 2, 0));
        vector<vector<int>> rst(n-2, vector<int>(n - 2, 0));
        for (int i = 0; i < n; i++) {
            for (int j = 0; j < n - 2; j++) {
                g[i][j] = max({ grid[i][j],grid[i][j+1], grid[i][j + 2] });
            }
        }
        for (int i = 0; i < n - 2; i++) {
            for (int j = 0; j < n - 2; j++) {
                rst[i][j] = max({ g[i][j],g[i+1][j], g[i+2][j] });
            }
        }
        return rst;
    }
};