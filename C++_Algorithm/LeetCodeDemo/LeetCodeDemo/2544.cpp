using namespace std;
#include <iostream>
//简单题 朴素实现
class Solution {
public:
    int alternateDigitSum(int n) {
        int rst = 0,dig=1,p=1;
        while (n) {
            dig = p*(n % 10);
            rst += dig;
            n /= 10;
            p = -p;
        }
        return dig>0?rst:-rst;
    }
};