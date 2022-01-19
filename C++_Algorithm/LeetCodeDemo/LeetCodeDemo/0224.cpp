using namespace std;
#include <iostream>
#include <deque>
#include <string>
#include <vector>
//条件分析处理
//只有加减法，括号代表符号变化，空格代表前后位置判断不可用
class Solution {
public:
    int calculate(string s) {
        int n = s.size();
        int ans = 0;
        int num = 0;
        int sym = 1;
        int flag1 = 1;
        vector<bool> flag2;
        bool prevSym = true;
        for (int i = 0; i < n; i++)
        {
            if (s[i] == '(')
            {
                if (!prevSym)
                {
                    flag1 = -flag1;
                    flag2.push_back(true);
                }
                else
                    flag2.push_back(false);
            }
            if (s[i] == ')')
            {
                if (flag2.back())
                    flag1 = -flag1;
                flag2.pop_back();
            }
            if ('0' <= s[i] && s[i] <= '9')
            {
                int j = 0;
                while ('0' <= s[i + j] && s[i + j] <= '9')j++;
                int num = stoi(s.substr(i, j));
                ans += sym * num;
                i += j - 1;
            }
            if (s[i] == '+')
            {
                prevSym = true;
                sym = flag1;
            }
            if (s[i] == '-')
            {
                prevSym = false;
                sym = -flag1;
            }
        }
        return ans;
    }
};