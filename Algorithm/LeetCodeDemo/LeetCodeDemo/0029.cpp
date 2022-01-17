using namespace std;
#include <iostream>
//巧思 分治
//按进制思路拆分多个倍数然后累加
class Solution {
public:
    int divide(int dividend, int divisor) {
        long long digit[33]{},a= dividend,b= divisor;
        int len;
        bool flag1 = a > 0;
        bool flag2 = b > 0;
        if (!flag1)a = -a;
        if (!flag2)b = -b;
        digit[0] = b;
        for (len= 1; digit[len-1]< 2147483647; len++)
        {
            digit[len] = digit[len - 1] * 2;
        }
        long long ans=0;
        int i = len-1;
        while (i >= 0&&a - digit[0] >= 0)
        {
            if (a- digit[i] >= 0)
            {
                a -= digit[i];
                ans += (((long long)1) << i);
            }
            i--;
        }
        ans = (flag1 + flag2 == 1) ? -ans : ans;
        if (ans < -2147483648 || ans>2147483647)return 2147483647;
        return ans;
    }
};