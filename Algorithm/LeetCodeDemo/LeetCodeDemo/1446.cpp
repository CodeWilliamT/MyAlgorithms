using namespace std;
#include <iostream>
//简单题
//前后相同则累积，累积刷新最大值
class Solution {
public:
    int maxPower(string s) {
        int cnt = 0, rst = 0;
        char e=s[0];
        for (char& c : s)
        {
            if (c == e) { cnt++; rst = max(rst, cnt); }
            else {
                e = c; cnt = 1;
            }
        }
        return rst;
    }
};