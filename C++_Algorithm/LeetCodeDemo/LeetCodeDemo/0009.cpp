using namespace std;
#include <iostream>
//简单题 朴素实现
//取某一位数字x/digt%10,digit为10的某次方。
class Solution {
public:
    bool isPalindrome(int x) {
        if (x < 0)return false;
        if (!x)return true;
        long long digitmax = 1;
        int y = x;
        while (y > 0)digitmax *= 10, y /= 10;
        digitmax /= 10;
        int digit = 1;
        while (digitmax > digit)
        {
            if (x / digitmax % 10 != x / digit % 10)
                return false;
            digitmax /= 10;
            digit *= 10;
        }
        return true;
    }
};