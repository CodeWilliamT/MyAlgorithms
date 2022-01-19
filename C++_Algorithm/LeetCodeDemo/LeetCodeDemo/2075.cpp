using namespace std;
#include <iostream>
#include <vector>
//简单题，朴素实现
class Solution {
public:
    string decodeCiphertext(string s, int rows) {
        int n = s.size();
        if (!n)return s;
        int cols = n / rows;
        int i = 0,x=0;
        string ans;
        if (cols == 1||rows==1)ans = s;
        while (rows != 1&&cols!=1&&x<cols)
        {
            ans.push_back(s[i]);
            i +=cols + 1;
            if (i >= n)
                i = ++x;
        }
        while (ans.back() == ' ')
        {
            ans.pop_back();
        }
        return ans;
    }
};