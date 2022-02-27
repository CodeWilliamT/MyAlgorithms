using namespace std;
#include <iostream>
//哈希
//计算各字母总差值
class Solution {
public:
    int minSteps(string s, string t) {
        int letters[26]{};
        for (char& c : s) {
            letters[c - 'a']++;
        }
        for (char& c : t) {
            letters[c - 'a']--;
        }
        int rst = 0;
        for (int i = 0; i < 26; i++) {
            rst += abs(letters[i]);
        }
        return rst;
    }
};