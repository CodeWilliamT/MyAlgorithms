using namespace std;
#include <iostream>
#include <vector>
//二刷
//找规律
//规律：找分界，从左侧，初始左界为左第一个有高度的，从左往右遇到比左边高，则为右界以及新界，计算内部雨量，循环。
//然后从右往左也来次。
//左侧考虑相等。
class Solution {
public:
    int trap(vector<int>& h) {
        int x = 0;
        int rst=0,cnt=0;
        int n = h.size();
        for (int i = 1; i < n; i++) {
            if (h[i] >= h[x]) {
                x = i;
                rst += cnt;
                cnt = 0;
            }
            else {
                cnt += h[x]-h[i];
            }
        }
        x = n-1;
        cnt = 0;
        for (int i = n-2; i >=0; i--) {
            if (h[i] > h[x]) {
                x = i;
                rst += cnt;
                cnt = 0;
            }
            else {
                cnt += h[x] - h[i];
            }
        }
        return rst;
    }
};
//一刷
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