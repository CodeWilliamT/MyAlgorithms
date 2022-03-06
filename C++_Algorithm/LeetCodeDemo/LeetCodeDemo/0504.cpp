using namespace std;
#include <iostream>
//模拟 简单
//0，符号，取余
class Solution {
public:
    string convertToBase7(int num) {
        if (!num) return "0";
        string rst;
        bool p = num > 0;
        if (!p)num = -num;
        while (num) {
            rst.push_back(num % 7 + '0');
            num /= 7;
        }
        if (!p)rst.push_back('-');
        reverse(rst.begin(), rst.end());
        return rst;
    }
};