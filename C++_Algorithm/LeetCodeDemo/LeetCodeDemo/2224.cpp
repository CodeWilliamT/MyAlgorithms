using namespace std;
#include <iostream>
//简单模拟
//计算分钟数，取余
class Solution {
    int sToMin(string& s) {
        string s1 = { s[0],s[1] };
        string s2 = { s[3],s[4] };
        return 60 * stoi(s1) + stoi(s2);
    }
public:
    int convertTime(string cur, string cor) {
        int mins1 = sToMin(cur);
        int mins2 = sToMin(cor);
        if (mins1 == mins2)return 0;
        int diff = mins1 < mins2 ? mins2 - mins1 : mins2 + 24 * 60 - mins1;
        int rst = 0;
        int span[3] = { 60,15,5 };
        for (int& e : span) {
            while (diff >= e) {
                rst++;
                diff -= e;
            }
        }
        return rst+diff;
    }
};