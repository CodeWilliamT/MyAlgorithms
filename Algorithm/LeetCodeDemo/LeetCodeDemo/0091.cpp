using namespace std;
#include <iostream>
#include <vector>
#include <string>
#include <unordered_map>
//动态规划，f[x]代表到s[x]的解数，可行解累加
class Solution {
public:
    int numDecodings(string s) {
        if (s[0] == '0')return 0;
        vector<int> f(s.size());
        f[0] = 1;
        for (int i = 1; i < s.size(); i++)
        {
            if (s[i] != '0') 
            {
                f[i] = f[i - 1];
            }
            if (s[i-1] != '0' &&stoi(s.substr(i - 1, 2)) < 27)
            {
                if (i == 1)
                {
                    f[i] += 1;
                }
                else f[i] += f[i - 2];
            }
        }
        return f[s.size()-1];
    }
};