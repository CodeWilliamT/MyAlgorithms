using namespace std;
#include <iostream>
#include <vector>
#include <string>
//动态规划 分治 空间优化
class Solution {
public:
    int minimumTotal(vector<vector<int>>& triangle) {
        int n = triangle.size();
        if (n == 1)return triangle[0][0];
        vector<vector<int>> f(2, vector<int>(n));
        //int f[2][n];
        //memset(f,0,sizeof(f));
        f[0][0] = triangle[0][0];
        int ans = 10001;
        for (int i = 1; i < n; i++)
        {
            for (int j = 0; j <= i; j++)
            {
                if (j == 0)
                    f[1][0] = f[0][0] + triangle[i][0];
                else if (i == j)
                    f[1][i] = f[0][i - 1] + triangle[i][i];
                else
                    f[1][j] = min(f[0][j], f[0][j - 1]) + triangle[i][j];
                if (i == n - 1)ans = min(ans, f[1][j]);
                if (j > 0)f[0][j - 1] = f[1][j - 1];
            }
            f[0][i] = f[1][i];
        }
        return ans;
    }
};
//动态规划
//状态量f[i][j]为i层的最小路径
//状态转移方程：f[i][j] = min(f[i - 1][j], f[i - 1][j - 1]) + triangle[i][j]; 
//边界： f[i][0] = f[i - 1][0] + triangle[i][0];f[i][i] = f[i - 1][i - 1] + triangle[i][i];
//class Solution {
//public:
//    int minimumTotal(vector<vector<int>>& triangle) {
//        int n = triangle.size();
//        if (n == 1)return triangle[0][0];
//        vector<vector<int>> f = triangle;
//        //int f[n][n];
//        //memset(f,0,sizeof(f));
//        f[0][0] = triangle[0][0];
//        for (int i = 1; i < n; i++)
//        {
//            f[i][0] = f[i - 1][0] + triangle[i][0];
//            f[i][i] = f[i - 1][i - 1] + triangle[i][i];
//        }
//        int ans = min(f[n - 1][0], f[n - 1][n - 1]);
//        for (int i = 1; i < n; i++)
//        {
//            for (int j = 1; j < i; j++)
//            {
//                f[i][j] = min(f[i - 1][j], f[i - 1][j - 1]) + triangle[i][j];
//                if (i == n - 1)ans = min(ans, f[i][j]);
//            }
//        }
//        return ans;
//    }
//};