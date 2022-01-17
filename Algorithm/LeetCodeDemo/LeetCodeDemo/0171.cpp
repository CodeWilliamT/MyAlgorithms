using namespace std;
#include <iostream>
#include <string>
//简单题，朴素实现
//反序，注意大数
class Solution {
public:
    int titleToNumber(string s) {
        long long ans=0;
        long long digit = 1;
        reverse(s.begin(), s.end());
        for (auto c : s)
        {
            ans += (c - 'A'+1) * digit;
            digit*=26;
        }
        return ans;
    }
};