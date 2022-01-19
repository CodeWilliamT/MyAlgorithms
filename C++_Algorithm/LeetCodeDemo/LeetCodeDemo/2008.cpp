using namespace std;
#include <iostream>
#include <vector>
#include <string>
//动态规划
//状态量f[i]到i时的最大收益
//状态转移方程f[i]=max(f[i-1],if(r[j][1]==i) {f[r[j][0]]+r[j][2]} )
//边界f[0]=0
class Solution {
public:
    long long maxTaxiEarnings(int n, vector<vector<int>>& r) {
        vector<long long> f(n + 1, 0);
        f[0] = 0;
        vector<vector<vector<int>>> k(n + 1);
        for (int j = 0; j < r.size(); j++)
        {
            k[r[j][1]].push_back({ r[j][0] ,r[j][1] - r[j][0] + r[j][2] });
        }
        for (int i = 1; i <= n; i++)
        {
            f[i] = f[i - 1];
            for (int j = 0; j < k[i].size(); j++)
            {
                f[i] = max(f[i], f[k[i][j][0]] + k[i][j][1]);
            }
        }
        return f[n];
    }
};