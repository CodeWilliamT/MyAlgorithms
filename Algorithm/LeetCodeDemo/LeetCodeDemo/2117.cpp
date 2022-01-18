using namespace std;
#include <string>
//模拟题，麻烦题(细致条件分析)
//记末尾0，前5数，用double，后6数,数非0长。
//长度10以内，长度10以外，分情况讨论
//坑：
//1：大于10时，尾数位数小于5；
//2：加法超int
//3: 头上个数小于5
class Solution {
public:
    string abbreviateProduct(int left, int right) {
        const long long maxlen = 10000000000;
        const int size = 100000;
        long long zeros = 0, suf = 1;
        double pre = 1;
        int len = 1;
        int prim;
        string s;
        for (int i = left; i <= right; i++) {
            len += (int)log10(pre * i) - (int)log10(pre);
            suf = suf * i;
            while (suf % 10 == 0) {
                zeros++;
                suf /= 10;
                len--;
            }
            pre *= i;
            while (pre >= size)
                pre /= 10;
            if (suf >= maxlen) {
                suf = suf % maxlen;
            }
        }
        if (len < 11) {
            prim = len > 5 ? len > 10 ? 1 : (int)pow(10, (10 - len)) : 100000;
            s=to_string(((long long)pre / prim) * size + suf % size);
        }
        else {
            s =to_string(suf % size);
            while (s.size() < 5) {
                s = "0" + s;
            }
            s = to_string((int)pre) + s;
        }
        return s+"e"+to_string(zeros);
    }
};