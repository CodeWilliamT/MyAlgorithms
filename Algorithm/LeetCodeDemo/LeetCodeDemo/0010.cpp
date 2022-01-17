using namespace std;
#include <iostream>
#include <vector>
//动态规划
//状态转移方程的函数值f[x][y]：代表到匹配至s的x-1位，p的y-1位是否成功。
//边界条件：空集算能匹配,f[0][0]=true;

class Solution {
public:
    bool isMatch(string s, string p) {
        int m = s.size(), n = p.size();
        vector<vector<bool>>f(m+1, vector<bool>(n+1));
        auto match = [&](int i, int j)
        {
            if (i == 0)return false;
            if (p[j-1] == '.')
            {
                return true;
            }
            return s[i-1] == p[j-1];
        };
        f[0][0] = true;
        for (int i = 0; i <= m; i++)
        {
            for (int j = 1; j <= n; j++)
            {
                if (p[j-1] == '*')
                {
                    if (j > 1)
                        f[i][j] =f[i][j - 2];
                    if (match(i, j - 1)) f[i][j] = f[i][j] || f[i - 1][j];
                }
                else
                {
                    if(match(i, j))f[i][j] =  f[i - 1][j - 1];
                }
            }
        }
        return f[m][n];
    }
};

//int main()
//{
//    Solution s;
//    s.isMatch("aa","a*");
//    return 0;
//}