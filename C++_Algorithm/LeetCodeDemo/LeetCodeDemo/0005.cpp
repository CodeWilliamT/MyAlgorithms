using namespace std;
#include <iostream>
#include <vector>
//动态规划
//状态转移量f[x][y]为判定s.substr(x,y+1)是否为回文串，状态递推方程为f[x][y]=f[x + 1][y - 2]&&s[j]==s[j+i];
//边界条件为f[x][0]=true;f[x][1]=s[j]==s[j+i];
class Solution {
public:
    string longestPalindrome(string s) {
        if (s.size() < 2) return s;
        vector<vector<bool>> f(s.size(), vector<bool>(s.size()));
        int h = 0, e = 1;
        for (int i = 0; i < s.size(); i++)
        {
            for (int j = 0; j + i < s.size(); j++)
            {
                if (i == 0)
                {
                    f[j][i] = true;
                    continue;
                }
                if (s[j] == s[j + i])
                {
                    if (i == 1)
                    {
                        f[j][i] = true;
                        if (i + 1 > e)
                        {
                            e = i + 1;
                            h = j;
                        }
                        continue;
                    }
                    if (f[j + 1][i - 2])
                    {
                        f[j][i] = true;
                        if (i + 1 > e)
                        {
                            e = i + 1;
                            h = j;
                        }
                    }
                    else
                    {
                        f[j][i] = false;
                    }
                }
            }
        }
        return s.substr(h, e);
    }
};
//滑动窗口
//class Solution {
//public:
//    string longestPalindrome(string s) {
//        if (s.size() < 2)return s;
//        int n = s.size();
//        string s2 = s;
//        reverse(s2.begin(), s2.end());
//        int ansn = 0;
//        string ans = "";
//        int i1;
//        int i2;
//        int len = 0;
//        int start;
//        string temp;
//        string temp2;
//        for (int k = 1; k < 2 * n; k++)
//        {
//            if (k < n)
//            {
//                i1 = n - k; i2 = 0;
//            }
//            else
//            {
//                i1 = 0; i2 = k - n;
//            }
//            start = i1;
//            len = 0;
//            int j;
//            for (j = 0; i1 + j < n && i2 + j < n; j++)
//            {
//                if (s[i1 + j] != s2[i2 + j])
//                {
//                    if (len > ansn)
//                    {
//                        temp = s.substr(start, i1 + j - start);
//                        temp2 = temp;
//                        reverse(temp2.begin(), temp2.end());
//                        if (temp == temp2)
//                        {
//                            ansn = len;
//                            ans = temp;
//                        }
//                    }
//                    len = 0;
//                    start = i1 + j + 1;
//                }
//                else
//                {
//                    len++;
//                }
//            }
//            if (len > ansn)
//            {
//                temp = s.substr(start, i1 + j - start);
//                temp2 = temp;
//                reverse(temp2.begin(), temp2.end());
//                if (temp == temp2)
//                {
//                    ansn = len;
//                    ans = temp;
//                }
//            }
//        }
//        return ans;
//    }
//};
//回溯
//class Solution {
//public:
//    string longestPalindrome(string s) {
//        if (s.size() < 2) return s;
//        string rst = "";
//        string rstr, str;
//        for (int i = s.size() - 1; i > -1; i--)
//        {
//            rstr = "";
//            for (int j = i; j > -1; j--)
//            {
//                rstr += s[j];
//                str = s.substr(j, i);
//                if (str == rstr)
//                    if (str.size() > rst.size())
//                    {
//                        rst = str;
//                    }
//            }
//        }
//        return rst;
//    }
//};
//int main()
//{
//    Solution s;
//    s.longestPalindrome("babad");
//    return 0;
//}