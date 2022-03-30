using namespace std;
#include <iostream>
#include <string>
//找规律 
//第二个匹配就加之前第一个的数目
//比较加最前跟加最后的区别。
class Solution {
public:
    long long maximumSubsequenceCount(string t, string p) {
        long long rst1 = 0, rst2 = 0,x=1;
        for (char& c : t) {
            if (c == p[1]) {
                rst1 += x;
            }
            if (c == p[0]) {
                x++;
            }
        }
        x = 0;
        for (char& c : t) {
            if (c == p[1]) {
                rst2 += x;
            }
            if (c == p[0]) {
                x++;
            }
        }
        rst2 += x;
        return max(rst1,rst2);
    }
};