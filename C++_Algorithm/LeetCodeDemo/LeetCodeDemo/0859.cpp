using namespace std;
#include <iostream>
//简单题 朴素实现
//先比长度，长度相同则看看是否只有两个字符不同的位置切字符互同,或者没有不同的位置然后字符串内部有相同的字符。
class Solution {
public:
    bool buddyStrings(string s, string goal) {
        if (s.size() != goal.size())return false;
        int cnt = 0;
        int lst[26]{};
        char dig[4]{};
        bool same=false;
        for (int i = 0; i < s.size(); i++)
        {
            if (s[i] != goal[i]) {
                if (cnt < 2)
                    dig[2 * cnt] = s[i], dig[2 * cnt + 1] = goal[i];
                cnt++;
            }
            if (cnt > 2)return false;
            ;
            if (++lst[s[i] - 'a'] > 1)same = 1;
        }
        return cnt == 2&& dig[0] == dig[3]&& dig[1] == dig[2] ||same&&!cnt;
    }
};