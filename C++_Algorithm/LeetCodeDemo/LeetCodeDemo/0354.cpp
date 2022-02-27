using namespace std;
#include <vector>
#include <algorithm>
//动态规划 分治
//状态量f[i]为i的最大套娃信封数
//状态转移方程：f[i] = max(f[0],...,f[i-1]) + 1; 
//边界： f[i]min=1;
class Solution {
public:
    int maxEnvelopes(vector<vector<int>>& envelopes) {
        int n = envelopes.size();
        vector<int> f(n);
        //int f[n];
        //f[0] = 1;
        sort(envelopes.begin(), envelopes.end());
        int ans = 1;
        for (int i = 1; i < n; i++)
        {
            f[i] = 1;
            for (int j = 0; j < i; j++)
            {
                if (envelopes[i][0] > envelopes[j][0])
                    if (envelopes[i][1] > envelopes[j][1])
                        f[i] = max(f[i], f[j] + 1);
            }
            ans = max(ans, f[i]);
        }
        return ans;
    }
};