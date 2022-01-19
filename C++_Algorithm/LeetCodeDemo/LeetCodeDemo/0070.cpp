using namespace std;
#include <iostream>
#include <vector>
#include <string>
//动态规划优化
//参数极简
class Solution {
public:
    int climbStairs(int n) {
        if (n < 2)return 1;
        int f = 1, s = 1;
        for (int i = 2; i <= n; i++)
        {
            s = f + s;
            f = s - f;
        }
        return s;
    }
};
//动态规划
//状态量F[i] 楼梯数
//状态转移方程f[i] = f[i - 1] + f[i - 2];
//边界f[0] = 1, f[1] = 1;
//class Solution {
//public:
//    int climbStairs(int n) {
//        if (n < 2)return 1;
//        vector<int> f(n + 1,1);
//        for (int i = 2; i <= n; i++)
//        {
//            f[i] = f[i - 1] + f[i - 2];
//        }
//        return f[n];
//    }
//};