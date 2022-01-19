using namespace std;
#include <iostream>
#include <string>
//简单题 朴素实现
//注意解决界限问题
class Solution {
public:
    int reverse(int x) {
        bool sign = x < 0;
        string s = to_string(x);
        if (sign)s.erase(s.begin());
        ::reverse(s.begin(), s.end());
        string smn = "2147483648";
        string smx = "2147483647";
        if (s.size() > smn.size()|| s.size() == smn.size()&&(s > smn && sign || s > smx && !sign))return 0;
        x = stoi(s);
        return sign ? -x : x;
    }
};
