using namespace std;
#include <iostream>
//简单题 朴素实现
class Solution {
public:
    bool isPalindrome(string s) {
        string b;
        for (auto c : s)
        {
            if (c <= 'Z' && c >= 'A' || c <= 'z' && c >= 'a'
                )
                b.push_back(tolower(c));
            else if (c <= '9' && c >= '0')
                b.push_back(c);
        }
        for (int i = 0; i < b.size() / 2; i++)
        {
            if (b[i] != b[b.size() - 1 - i])return false;
        }
        return true;
    }
};