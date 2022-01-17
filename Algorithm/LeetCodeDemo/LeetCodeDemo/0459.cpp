using namespace std;
#include <iostream>

//枚举判断
class Solution {
public:
    bool repeatedSubstringPattern(string s) {
        int len = s.size();
        string catStr = "";
        string subStr = "";
        int time;
        for (int i = 1; i < len / 2 + 1; i++)
        {
            if (len % i)continue;
            time = len / i;
            catStr = "";
            subStr = s.substr(0, i);
            while (time--)
            {
                catStr += subStr;
            }
            if (catStr == s)return true;
        }
        return false;
    }
};

//利用两个s拼接，从1位置后开始匹配s，若s能在第二个字串前找到匹配结果该字串便符合的性质来来判断。
//class Solution {
//public:
//    bool repeatedSubstringPattern(string s) {
//        return (s + s).find(s, 1) != s.size();
//    }
//};