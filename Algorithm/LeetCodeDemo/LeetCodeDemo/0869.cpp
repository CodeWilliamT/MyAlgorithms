using namespace std;
#include <iostream>
#include <vector>
#include <string>
//回溯
class Solution {
    bool isPowerOf2(int n)
    {
        if (n == 1) return true;
        while (n % 2 == 0)
        {
            if (n == 2)return true;
            n /= 2;
        }
        return false;
    }
    bool dfs(string ans, int used, string& s)
    {
        if (ans.size() == s.size())
        {
            return isPowerOf2(stoi(ans));
        }
        for (int i = 0; i < s.size(); i++) 
        {
            if(s[i] == '0'&&used==0)continue;
            if ((used >> i )% 2)continue;
            if (dfs(ans + s[i], used + (1 << i), s))
                return true;
        }
        return false;
    }
public:
    bool reorderedPowerOf2(int n) {
        if (isPowerOf2(n)) return true;
        string s = to_string(n);
        return dfs("", 0, s);
    }
};