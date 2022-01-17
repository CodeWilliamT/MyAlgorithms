using namespace std;
#include <iostream>
#include <vector>
#include <string>
//动态规划,最大子序和
class Solution {
public:
    int maxProfit(vector<int>& prices) {
        int n = prices.size();
        if (n < 2)return 0;
        int dif = 0;
        int ans = 0;
        vector<int>f(n, 0);;
        for (int i = 1; i < n; i++)
        {
            dif = prices[i] - prices[i - 1];
            f[i] = max(dif + f[i - 1], dif);
            ans = max(ans, f[i]);
        }
        return ans;
    }
};