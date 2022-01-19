using namespace std;
#include <iostream>
#include <vector>
//朴素实现
class Solution {
    bool isR(string s) {
        int n = s.size();
        for (int i = 0; i < n; i++) {
            if (s[i] != s[n - 1 - i])return false;
        }
        return true;
    }
public:
    string firstPalindrome(vector<string>& words) {
        for (string& s : words) {
            if (isR(s))
                return s;
        }
        return "";
    }
};