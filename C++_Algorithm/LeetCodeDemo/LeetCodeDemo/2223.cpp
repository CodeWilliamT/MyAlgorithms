using namespace std;
#include <vector>
//字符串哈希 二分
//构建前缀字符串哈希的数组
//前缀字符串哈希做差求中间字符串的哈希值
//两分确定中间字符串的长度
//坑：范围越界，存次方结果。
class Solution {
    typedef unsigned long long ull;
public:
    long long sumScores(string s) {
        int n = s.size();
        vector<ull> v(n+1);
        vector<ull> power(n + 1,1);
        ull prime = 27;
        ull key = 0;
        for (int i = 0; i<n; i++) {
            key = key * prime + s[i] - 'a';
            v[i+1] = key;
            power[i + 1] = power[i] * prime;
        }
        long long l, r, m;
        long long rst = 0;
        for (int i = 0; i < n; i++) {
            l = 0, r = n - i;
            while (l < r) {
                m = (l + r + 1) / 2;
                key = v[i + m] - v[i] * power[m];
                if (v[m]==key) {
                    l = m;
                }
                else {
                    r = m - 1;
                }
            }
            rst += l;
        }
        return rst;
    }
};