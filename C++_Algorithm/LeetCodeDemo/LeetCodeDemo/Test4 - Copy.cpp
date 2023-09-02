using namespace std;
#include <iostream>
#include <algorithm>
//美丽值是最大值：出现最多的前k个元素的出现数的和。
//序列数=前k个元素的出现数的乘积的和%mod。
class Solution {
public:
    typedef long long ll;
    ll C(int n, int m) {
        long long res = 1;
        m = min(m, n - m);
        for (int i = 0; i < m; ++i) {
            res *= (n - i);
            res /= (i + 1);
        }
        return res;
    }
    int countKSubsequencesWithMaxBeauty(string s, int k) {
        ll f[26]{};
        int cnt=0;
        for (char& c : s) {
            if (!f[c - 'a'])cnt++;
            f[c-'a']++;
        }
        if (cnt < k)
            return 0;
        sort(f,f+26);
        ll rst=1;
        int mod = 1e9 + 7;
        int equaltails = 0;
        for (int i = 0; i < k; i++) {
            rst = (rst*f[25 - i])%mod;
            if (f[25 - i] == f[26 - k])
                equaltails++;

        }
        int tmp = 0;
        for (int i = 0; i < 26; i++) {
            if (f[25 - i] == f[26 - k])
                tmp++;
        }
        rst = ((C(tmp, equaltails) % mod) * rst) % mod;
        return rst;
    }
};