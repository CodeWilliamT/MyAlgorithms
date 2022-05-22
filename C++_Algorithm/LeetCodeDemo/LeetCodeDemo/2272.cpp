using namespace std;
#include <iostream>
//求所有子字符串内出现最多跟最少字符的差的最大值。
//枚举 动态规划 前缀和
//1e4的最大长度，复杂度n^2内。
//枚举较多字符跟较少，前者+1，后者-1，求区间最大和(之前的和为0或负则舍去)。
//存储含y的最大区间和，每次v==-1时比较更新，否则直接累加。
class Solution {
public:
    int largestVariance(string s) {
        int n = s.size();
        int premax,ypremax;
        int rst = 0;
        int v;
        for (int x = 'a'; x <='z'; x++) {
            for (int y = 'a'; y <= 'z'; y++) {
                if (x == y)continue;
                premax = 0;
                ypremax = -1e4;
                for (int i = 0; i < n; i++) {
                    v = s[i] == x ? 1 : s[i] == y ? -1 : 0;
                    if (v == -1)
                        ypremax = max({ premax + v,ypremax + v,v });
                    else
                        ypremax += v;
                    premax = max(premax + v, v);
                    rst = max(ypremax, rst);
                }
            }
        }
        return rst;
    }
};