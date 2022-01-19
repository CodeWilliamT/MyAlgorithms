using namespace std;
#include <iostream>
#include <vector>
#include <string>
#include <algorithm>

//动态规划
//状态量F[i] 凑到i最少金币数
//状态转移方程f[i]=min(f[i-c[0]],...f[i-c[n-1]])+1
//边界f[0]=0;
class Solution {
public:
    int coinChange(vector<int>& c, int amount) {
        if (!amount)return 0;
        vector<int> f(amount+1,amount+1);
        f[0] = 0;
        for (int i = 1; i <= amount; i++)
        {
            for (int j = 0; j < c.size(); j++)
            {
                if (i - c[j]>=0)
                    if(f[i - c[j]]!=amount+1)
                        f[i] = min(f[i], f[i - c[j]]+1);
            }
        }
        return f[amount] > amount ? -1 : f[amount];
    }
};