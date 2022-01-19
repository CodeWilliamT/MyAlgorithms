using namespace std;
#include <iostream>
#include <vector>
#include <string>
//找规律 递推
//个位0~9，十位10~99 10~2*9*10+9 百位100~999 190~3*9*100+2*9*10+9
class Solution {
public:
    int findNthDigit(int n) {
        int num;
        int digit = 1, base = 0;
        long long size = 1;
        while (n > digit * size * 9)
        {
            base += size * 9;
            n -= digit * size * 9;
            digit++;
            size *= 10;
        }
        num = ceil(n * 1.0 / digit) + base;
        return to_string(num)[(n + digit - 1) % digit] - '0';
    }
};