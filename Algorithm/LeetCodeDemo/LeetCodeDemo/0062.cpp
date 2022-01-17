using namespace std;
#include <iostream>
#include <vector>
#include <string>
//组合数学，选择数
//从左上角到右下角的过程中，我们需要移动 m+n-2次，
//其中有 m-1次向下移动，n-1n−1 次向右移动。因此路径的总数，就等于从 m+n-2m+n−2 次移动中选择 m-1m−1 次向下移动的方案数
class Solution {
public:
    int uniquePaths(int m, int n) {
        long long ans = 1;
        for (int x = n, y = 1; y < m; ++x, ++y) {
            ans = ans * x / y;
        }
        return ans;
    }
};
//动态规划
//class Solution {
//public:
//    int uniquePaths(int m, int n) {
//        int d[m][n];
//
//        for (int i = 0; i < m; i++)
//            d[i][0] = 1;
//        for (int j = 0; j < n; j++)
//            d[0][j] = 1;
//        for (int i = 1; i < m; i++)
//            for (int j = 1; j < n; j++)
//            {
//                d[i][j] = d[i - 1][j] + d[i][j - 1];
//            }
//        return d[m - 1][n - 1];
//    }
//};
//回溯递归，超时
//class Solution {
//    int dfs(int i, int j, int m, int n)
//    {
//        if (i >= m)return 0;
//        if (j >= n)return 0;
//        if (i == m - 1 && j == n - 1)return 1;
//        return dfs(i + 1, j, m, n) + dfs(i, j + 1, m, n);
//    }
//public:
//    int uniquePaths(int m, int n) {
//        return dfs(0, 0, m, n);
//    }
//};