using namespace std;
#include <iostream>
#include <string>
//简单分析
class Solution {
public:
    string generateTheString(int n) {
        string ans = "";
        if (n % 2 != 1)ans += 'a';
        for (int i = 0; i < (n % 2 ? n : n - 1); i++)
        {
            ans += 'b';
        }
        return ans;
    }
};