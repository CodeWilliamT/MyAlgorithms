using namespace std;
#include <iostream>
#include <vector>
#include <string>
//动态规划
//状态量：所求值
//状态转移方程：f[i][j] = f[i - 1][j] + f[i][j - 1] - f[i - 1][j - 1]+mat[i - k-1][j - k - 1]+mat[i + k][j + k]-mat[i - k-1][j + k]-mat[i + k][j - k - 1];
//边界：f[0][j]，f[i][0]
class Solution {
public:
    vector<vector<int>> matrixBlockSum(vector<vector<int>>& mat, int k) {
        int m = mat.size();
        int n = mat[0].size();
        vector<vector<int>> f=mat;
        f[0][0] = 0;
        for (int i = 0; i <= k&&i<m; i++)
        {
            for (int j = 0; j <= k&&j<n; j++)
            {
                f[0][0] += mat[i][j];
            }
        }
        for (int j = 1; j < n; j++)
        {
            f[0][j] = f[0][j - 1];
            if(j - k - 1>-1)
                for (int i = 0; i <= k && i < m; i++)
                {
                    f[0][j] -= mat[i][j - k - 1];
                }
            if (j + k<n)

                for (int i = 0; i <= k && i < m; i++)
                {
                    f[0][j] += mat[i][j+k];
                }
        }
        for (int i = 1; i < m; i++)
        {
            f[i][0] = f[i-1][0];
            if (i - k - 1 > -1)
                for (int j = 0; j <= k && j < n; j++)
                {
                    f[i][0] -= mat[i - k - 1][j];
                }
            if (i + k < m)
                for (int j = 0; j <= k && j < n; j++)
                {
                    f[i][0] += mat[i+k][j];
                }
        }
        for (int i = 1; i < m; i++)
        {
            for (int j = 1; j < n; j++)
            {
                f[i][j] = f[i - 1][j] + f[i][j - 1] - f[i - 1][j - 1];
                if(i-k-1>-1&&j-k-1>-1)
                    f[i][j] += mat[i - k-1][j - k - 1];
                if (i + k < m && j + k < n)
                    f[i][j] += mat[i + k][j + k];
                if(i-k-1>-1&&j+k<n)
                    f[i][j] -= mat[i - k-1][j + k];
                if(i+k<m&&j-k-1>-1)
                    f[i][j] -= mat[i + k][j - k - 1];
            }
        }
        return f;
    }
};