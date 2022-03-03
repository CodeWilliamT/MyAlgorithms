using namespace std;
#include <iostream>
//模拟 大数相加
class Solution {
public:
    string addStrings(string n1, string n2) {
        int up = 0;
        string rst;
        while (up || !n1.empty() || !n2.empty()) {
            if (!n1.empty())up += n1.back()-'0', n1.pop_back();
            if (!n2.empty())up += n2.back()-'0', n2.pop_back();
            rst.push_back((up % 10)+'0');
            up /= 10;
        }
        reverse(rst.begin(), rst.end());
        return rst;
    }
};