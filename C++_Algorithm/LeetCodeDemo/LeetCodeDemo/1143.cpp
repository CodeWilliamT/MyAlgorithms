using namespace std;
#include <iostream>
#include <vector>
#include <string>

//动态规划
//状态量f[i][j]匹配到s1[i-1],s2[j-1]的最大匹配数
//状态转移方程f[i][j]=s1[i-1]==s2[j-1]?f[i-1][j-1]+1:max(f[i - 1][j], f[i][j - 1]).
//边界f[0][0]=0
class Solution {
public:
    int longestCommonSubsequence(string s1, string s2) {
        int n = s1.size() + 1, m = s2.size() + 1;
        vector<vector<int>> f(n, vector<int>(m));
        f[0][0] = 0;
        for (int i = 1; i < n; i++)
        {
            for (int j = 1; j < m; j++)
            {
                f[i][j] = s1[i - 1] == s2[j - 1] ? f[i - 1][j - 1] + 1 : max(f[i - 1][j], f[i][j - 1]);
            }
        }
        return f[n - 1][m - 1];
    }
};