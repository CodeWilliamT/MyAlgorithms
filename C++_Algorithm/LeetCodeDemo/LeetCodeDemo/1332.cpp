using namespace std;
#include <iostream>
//巧思
//只有俩字符，可以删一类字符就行了，最大是2.
//判定本身是不是回文，是就是1，不是就是2
class Solution {
public:
    int removePalindromeSub(string s) {
        for (int i = 0; i < s.size() / 2; i++) {
            if (s[s.size() - 1 - i] != s[i])
                return 2;
        }
        return 1;
    }
};