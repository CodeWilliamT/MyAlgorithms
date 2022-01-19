using namespace std;
#include <iostream>
#include <vector>
#include <string>
//动态规划
//状态转换量d[i][j]代表左侧第0到i个子串与右侧第0到j个子串的编辑距离。
//边界为d[0][j]=j,d[i][0]=i;
//转换方程为d[i][j]=min(d[i-1][j]+1,d[i][j-1]+1,word1[i-1]==word2[j-1]?d[i-1][j-1]:d[i-1][j-1]+1)
class Solution {
public:
    int minDistance(string word1, string word2) {
        const int n = word1.size();
        const int m = word2.size();
        if (!n)return m;
        if (!m)return n;
        vector<vector<int>> d(n + 1, vector<int>(m + 1));
        for (int i = 0; i <= n; i++)
        {
            for (int j = 0; j <= m; j++)
            {
                if (i == 0) { d[0][j] = j; continue; }
                if (j == 0) { d[i][0] = i; continue; }
                d[i][j] = min(d[i - 1][j] + 1, min(d[i][j - 1] + 1, word1[i-1] == word2[j-1] ? d[i - 1][j - 1] : d[i - 1][j - 1] + 1));
            }
        }
        return d[n][m];
    }
};