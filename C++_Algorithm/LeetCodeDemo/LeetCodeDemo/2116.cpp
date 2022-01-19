using namespace std;
#include <iostream>
//找规律 巧思
//当固定的一侧括号大于另一侧的括号以及可变括号，则不成立。
class Solution {
public:
    bool canBeValid(string s, string locked) {
        int n = s.size(), l = 0, r = 0;
        if (n % 2) return false;
        for (int i = 0; i < n; i++) {
            if (locked[i] == '1' && s[i] == ')') {
                r++;
                if (i + 1 - r < r) return false;
            }
        }
        for (int i = n - 1; i >= 0; i--) {
            if (locked[i] == '1' && s[i] == '(') {
                l++;
                if (n - i - l < l) return false;
            }
        }
        return true;
    }
};