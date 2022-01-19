using namespace std;
#include <iostream>
#include <vector>
//找规律 巧思
//记下最高山的索引跟高度，每次找山峰(i==0||h[i-1]<=h[i])&&(i == n - 1 || h[i] >= h[i + 1]),
//计算原本最高山峰跟当前的山峰间的雨量并更新并累加答案，然后更新最高山峰。
class Solution {
public:
    int trap(vector<int>& h) {
        int topi = -1;
        int mn;
        int rst = 0;
        bool flag = 0;
        int top = 0;
        int n = h.size();
        for (int i = 0; i < n; i++)
        {
            if ((i==0||h[i-1]<=h[i])&&(i == n - 1 || h[i] >= h[i + 1]))
            {
                if (topi == -1)
                {
                    topi = i;
                    top = h[i];
                }
                else
                {
                    mn = min(h[topi], h[i]);
                    for (int j = topi + 1; j < i; j++)
                    {
                        if (mn > h[j])
                        {
                            rst += mn - h[j];
                            h[j] = mn;
                        }
                    }
                    if (h[topi] < h[i])
                    {
                        topi = i;
                    }
                }
            }
        }
        return rst;
    }
};