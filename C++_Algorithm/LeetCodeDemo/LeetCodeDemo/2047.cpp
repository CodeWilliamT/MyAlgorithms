using namespace std;
#include <iostream>
#include <vector>
#include <string>
//细致条件分析
class Solution {
public:
    int countValidWords(string s) {
        int ans = 0;
        int h = 0;
        int n = s.size();
        bool count = 0;
        bool flag = 1;
        bool detect = 0;
        for (int i = 0; i < n; i++)
        {
            if (s[i] >= '0' && s[i] <= '9')flag = 0;
            else if (s[i] == '-')
            {
                if (count)flag = 0;
                count = 1;
                if (i == h || i + 1 == n || s[i + 1] == ' ')
                    flag = 0;
                else if (i + 1 != n)
                {
                    if (s[i + 1] < 'a' || s[i + 1]>'z')
                        flag = 0;
                }
            }
            else if (s[i] == '!' || s[i] == '.' || s[i] == ',')
            {
                if (i + 1 != n && s[i + 1] != ' ')
                    flag = 0;
                else
                    detect = 1;
            }
            else if (s[i] != ' ')
            {
                detect = 1;
            }
            if (i + 1 == n)
            {
                if (flag)
                    ans++;
            }
            else if (s[i] == ' ')
            {
                if (flag && i != 0 && detect)
                    ans++;
                flag = 1;
                count = 0;
                detect = 0;
                while (s[i + 1] == ' ' && i < n - 1)
                {
                    i++;
                }
                h = i + 1;
            }
        }
        return ans;
    }
};