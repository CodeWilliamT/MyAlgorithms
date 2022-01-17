using namespace std;
#include <iostream>
//简单题，双指针，反转
//反转各个单词
class Solution {
public:
    string reverseWords(string s) {
        int h = 0;
        int n = s.size();
        for (int e=0;e<n;e++)
        {
            if (e > 0 && s[e - 1] == ' ' && s[e] != ' ')
            {
                h = e;
            }
            if (e<n-1&&s[e+1] == ' '||e==n-1&&s[e]!=' ')
            {
                reverse(s.begin() + h, s.begin() + e+1);
            }
        }
        return s;
    }
};