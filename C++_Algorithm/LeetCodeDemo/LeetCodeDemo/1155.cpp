using namespace std;
#include <iostream>
#include <vector>
#include <string>
//动态规划
//状态量dp[i][k]为k个骰子能凑出i的组合数
//状态转移方程：dp[i][k] = (dp[i - 0][k - 1]+... + dp[i - f][k - 1]) % 1000000007;(注意计算中的步骤顺序)
//边界：1个骰子的组合d[i][1]=1。
class Solution {
public:
    int numRollsToTarget(int d, int f, int target) {
        if (!target)return 1;
        if (target<d || target>d * f)return 0;
        vector<vector<int>> dp(target + 1, vector<int>(d + 1));
        for (int i = 1; i <= f && i <= target; i++)
        {
            dp[i][1] = 1;
        }
        for (int k = 2; k <= d; k++)
        {
            for (int i = 1; i <= target; i++)
            {
                for (int j = 1; j <= f && i - j >= 0; j++)
                {
                    dp[i][k] = (dp[i][k] + dp[i - j][k - 1]) % 1000000007;
                }
            }
        }
        return dp[target][d];
    }
};