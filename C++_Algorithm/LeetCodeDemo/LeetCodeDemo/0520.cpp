using namespace std;
#include <iostream>
//朴素实现
//计数器记录大写数目，标志器记录首字母是否大写
class Solution {
public:
    bool detectCapitalUse(string s) {
        int cnt=0,n=s.size();
        bool flag=0;
        for (int i=0;i<n;i++)
        {
            if (s[i] >= 'A' && s[i] <= 'Z')
            {
                cnt++;
                if (!i)flag = 1;
            }
        }
        return cnt == n || !cnt || cnt == 1 && flag;
    }
};