using namespace std;
#include <iostream>
#include <vector>
//动态规划
//找波谷
class Solution {
public:
    vector<int> goodDaysToRobBank(vector<int>& security, int time) {
        int n = security.size();
        vector<bool> noincrease(n,1);
        vector<bool> nodecrease(n, 1);
        for (int i = 1; i < n; i++)
        {
            noincrease[i] = security[i - 1] >= security[i];
            nodecrease[i-1] = security[i - 1] <= security[i];
        }
        vector<bool> ans(n, 1);
        int suml = 0;
        int sumr = 0;
        for (int i = 1; i < n-time; i++)
        {
            suml += noincrease[i];
            sumr += nodecrease[n-1-i];
            if (i >= time)
            {
                if (i > time)
                {
                    suml -= noincrease[i - time];
                    sumr -= nodecrease[n - 1 - i+time];
                }
                if (suml != time)
                {
                    ans[i] = 0;
                }
                if (sumr != time)
                {
                    ans[n-1-i] = 0;
                }
            }
        }
        vector<int> rst;
        for (int i = time; i < n - time; i++)
        {
            if (ans[i])rst.push_back(i);
        }
        return rst;
    }
};