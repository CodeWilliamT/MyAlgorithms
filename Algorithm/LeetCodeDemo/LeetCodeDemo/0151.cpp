using namespace std;
#include <iostream>
#include <vector>
#include <string>
//简单题，反转
//将字符串反转，然后遍历单词，将单词反转，注意空格处理。
class Solution {
public:
    string reverseWords(string s) {
        if (s.size() < 2)return s;
        int n = s.size();
        reverse(s.begin(), s.end());
        int l = 0;
        int len;
        int count = 0;
        for (int i = 0; i < n; i++)
        {
            if (s[i] != ' ')
            {
                len = 1;
                if (i != n - 1)
                {
                    i++;
                    while (s[i] != ' ')
                    {
                        len++;
                        i++;
                        if (i > n - 1)break;
                    }
                    i--;
                }
                if (l != 0)
                    count += len + 1;
                else
                    count += len;
                reverse(s.begin() + l, s.begin() + i + 1);
                l += len + 1;
            }
        }
        s.erase(s.begin() + count, s.end());
        return s;
    }
};