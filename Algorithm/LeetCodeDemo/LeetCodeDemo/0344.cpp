using namespace std;
#include <iostream>
#include <vector>
#include <string>
#include <algorithm>
//简单题，朴素实现
//指针
class Solution {

    void swp(char* a, char* b)
    {
        *a = *a + *b;
        *b = *a - *b;
        *a = *a - *b;
    }
public:
    void reverseString(vector<char>& s) {
        for (int i = 0; i < s.size() / 2; i++)
        {
            swp(&s[i], &s[s.size()-1 - i]);
        }
    }
};
//简单题，朴素实现
//绝对引用
//class Solution {
//    void swp(char& a, char& b)
//    {
//        a = a + b;
//        b = a - b;
//        a = a - b;
//    }
//public:
//    void reverseString(vector<char>& s) {
//        for (int i = 0; i < s.size() / 2; i++)
//        {
//            swp(s[i], s[s.size() - 1 - i]);
//        }
//    }
//};
//简单题，朴素实现
//已有函数
//class Solution {
//public:
//    void reverseString(vector<char>& s) {
//        return reverse(s.begin(), s.end());
//    }
//};