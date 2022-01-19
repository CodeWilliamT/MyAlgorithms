using namespace std;
#include <iostream>
#include <vector>
#include <string>
#include <algorithm>
//动态规划
//状态量F[i] 凑到i的组合数
//状态转移方程f[i]=f[i-c[0]]+...+f[i-c[n-1]];
//边界f[0]=1;
class Solution {
public:
    int change(int amount, vector<int>& coins) {
        vector<int>f(amount + 1);
        f[0] = 1;
        for (int c:coins)
        {
            for (int i = c; i <= amount; i++)
            {
                f[i] += f[i - c];
            }
        }
        return f[amount];
    }
};