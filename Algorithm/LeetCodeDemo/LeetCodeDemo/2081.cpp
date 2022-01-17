using namespace std;
#include <iostream>
#include <vector>
#include <string>
//朴素实现 细节分析题
//按场景分类
class Solution {
private:
    bool checkNumOfK(long long num,int k)
    {
        long long digl = 1;
        long long a = num;
        while (a > 0)a /= k, digl *= k;
        digl /= k;
        long long digr = 1;
        while (digl > digr)
        {
            if (num / digl % k != num / digr % k)
                return false;
            digl /= k;
            digr *= k;
        }
        return true;
    }
public:
    long long kMirror(int k, int n) {
        long long rst = 0;
        int cnt = 0;
        string snum,rsnum;
        long long num;
        for (int j = 10; j < INT32_MAX / 10; j *= 10)
        {
            for (int i = j/10; i < j; i++)
            {
                snum = to_string(i);
                rsnum = snum;
                reverse(rsnum.begin(), rsnum.end());
                snum.pop_back();
                num = stoll(snum+rsnum);
                if (checkNumOfK(num, k))
                {
                    cnt++;
                    rst += num;
                    if (cnt >= n)return rst;
                }
            }
            for (int i = j/10; i < j; i++)
            {
                snum = to_string(i);
                rsnum = snum;
                reverse(rsnum.begin(), rsnum.end());
                num = stoll(snum + rsnum);
                if (checkNumOfK(num, k))
                {
                    cnt++;
                    rst += num;
                    if (cnt >= n)return rst;
                }
            }
        }
        return rst;
    }
};