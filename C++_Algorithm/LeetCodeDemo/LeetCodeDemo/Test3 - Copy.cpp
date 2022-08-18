using namespace std;
#include <iostream>
//找规律 麻烦模拟
//求最大最小高度。I=+1；D=-1,假设起始值为1，然后记录最大最小高度。
//起始值大值为min(9,10-最大高度)。最小值为 max(1,1-最小高度)
//因为长度为8，所以不考虑最大高度。
//取最小起始值，I=+1；D=-1
class Solution {
public:
    string smallestNumber(string pattern) {
        int mn=0, h=0;
        int n = 1 + pattern.size();
        for (char& c : pattern) {
            h += c == 'I' ? 1 :- 1;
            mn = min(h, mn);
        }
        string rst(n, 0);
        rst[0] = max(1, 1 - mn) + '0';
        for (int i = 0; i < pattern.size(); i++) {
            rst[i + 1] = rst[i] + (pattern[i] == 'I' ? 1 : -1);
        }
        return rst;
    }
};