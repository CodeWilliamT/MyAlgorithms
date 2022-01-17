using namespace std;
#include <iostream>
//简单题 朴素实现
class Solution {
public:
    string truncateSentence(string s, int k) {
        string rst;
        int cnt = 0;
        for (char& c : s)
        {
            if (c == ' '&&++cnt == k )
                    return rst;
            rst.push_back(c);
        }
        return rst;
    }
};