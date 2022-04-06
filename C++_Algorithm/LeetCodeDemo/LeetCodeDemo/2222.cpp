using namespace std;
#include <vector>
//找规律 数学
//010或者101
//101：1的数目*它之后0的数目*0之后1的数目
//010：对于每个数1+它前面0的数目*它之后0的数目
class Solution {
public:
    long long numberOfWays(string s) {
        int n = s.size();
        vector<long long> l(n, 0), r(n, 0);
        for (int i = 1; i < n; i++) {
            if (s[i - 1] - '0') {
                l[i] = l[i - 1] + 1;
            }
            else {
                l[i] = l[i - 1];
            }
            if (s[n - i] - '0') {
                r[n-i-1] = r[n - i] + 1;
            }
            else {
                r[n - i - 1] = r[n - i];
            }
        }
        long long rst = 0;
        for (int i = 0; i < n - 1; i++) {
            if (s[i] == '0')
                rst += l[i] * r[i];
        }
        l = vector<long long>(n, 0);
        r = vector<long long>(n, 0);
        for (int i = 1; i < n; i++) {
            if (s[i - 1] == '0') {
                l[i] = l[i - 1] + 1;
            }
            else {
                l[i] = l[i - 1];
            }
            if (s[n - i] == '0') {
                r[n - i - 1] = r[n - i] + 1;
            }
            else {
                r[n - i - 1] = r[n - i];
            }
        }
        for (int i = 0; i < n - 1; i++) {
            if (s[i] == '1')
                rst += l[i] * r[i];
        }
        return rst;
    }
};