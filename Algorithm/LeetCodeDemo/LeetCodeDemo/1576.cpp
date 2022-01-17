using namespace std;
#include <iostream>
//朴素实现
//while (i > 0 && s[i - 1] == s[i] || i < n - 1 && s[i + 1] == s[i]) s[i] = (s[i] - 'a' + 1) % 26 + 'a';
class Solution {
public:
    string modifyString(string s) {
        int n = s.size();
        for (int i = 0; i < s.size(); i++) {
            if (s[i] == '?') {
                s[i] = 'a';
                while (i > 0 && s[i - 1] == s[i] || i < n - 1 && s[i + 1] == s[i]) {
                    s[i] = (s[i] - 'a' + 1) % 26 + 'a';
                }
            }
        }
        return s;
    }
};