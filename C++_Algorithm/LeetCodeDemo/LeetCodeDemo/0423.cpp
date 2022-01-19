using namespace std;
#include <iostream>
#include <unordered_map>
//找规律 巧思
//统计字符频率，按特定顺序找特征字符。
class Solution {
private:
public:
    string originalDigits(string s) {
        string scanlist = "0246813579";
        int lst[26]{};
        for (char& c : s)
        {
            lst[c - 'a']++;
        }
        bool check = false;
        int ans[10]{};
        ans[0] = lst['z' - 'a'];
        ans[2] = lst['w' - 'a'];
        ans[4] = lst['u' - 'a'];
        ans[6] = lst['x' - 'a'];
        ans[8] = lst['g' - 'a'];
        ans[1] = lst['o' - 'a'] - ans[0] - ans[2] - ans[4];
        ans[3] = lst['h' - 'a'] - ans[8];
        ans[5] = lst['f' - 'a'] - ans[4];
        ans[7] = lst['s' - 'a'] - ans[6];
        ans[9] = lst['i' - 'a'] - ans[5] - ans[6] - ans[8];
        string rst;
        for (int i = 0; i < 10; i++)
        {
            while (ans[i]--) { rst.push_back(i + '0'); }
        }
        return rst;
    }
};