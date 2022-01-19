using namespace std;
#include <iostream>
#include <vector>
#include <string>
#include <unordered_map>
//哈希，map
class Solution {
public:
    int romanToInt(string s) {
        unordered_map<char, int> dic{ {'I',1},{'V',5},{'X',10} ,{'L',50} ,{'C',100} ,{'D',500} ,{'M',1000}};
        int ans = 0;
        for (int i = 0; i < s.size(); i++)
        {
            ans += dic[s[i]];
            if (i > 0&& dic[s[i - 1]] < dic[s[i]])
            {
                ans -= 2 * dic[s[i - 1]];
            }
        }
        return ans;
    }
};