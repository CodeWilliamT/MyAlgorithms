using namespace std;
#include <iostream>
#include <vector>
//巧思 数论 分治
//快速幂与模运算乘法结合律:
//模运算乘法结合律
//(a*b)%c=((a%c)*(b%c))%c;
//快速幂拆分：
//x ^ 77 = x∗x ^ 4∗x ^ 8∗x ^ 64
class Solution {
public:
    static constexpr int mod = 1337;
    int superPow(int a, vector<int>& b) {
        int n = b.size();
        int ans = 1, x = a % mod;
        for (int i = n - 1; i >= 0; --i) {
            ans = ans * qpow(x, b[i]) % mod;
            x = qpow(x, 10);
        }
        return ans;
    }
    int qpow(int x, int n) {
        int ret = 1;
        while (n > 0) {
            if (n & 1) ret = ret * x % mod;
            x = x * x % mod;
            n >>= 1;
        }
        return ret;
    }
};
