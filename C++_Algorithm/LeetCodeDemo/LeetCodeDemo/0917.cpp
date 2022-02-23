using namespace std;
#include <iostream>
//双指针
class Solution {
public:
    string reverseOnlyLetters(string s) {
        int l = 0, r = s.size() - 1;
        while (l < r) {
            while (r>l&&!isalpha(s[l])) {
                l++;
            }
            while (r>l&&!isalpha(s[r])) {
                r--;
            }
            if (l < r) {
                swap(s[l], s[r]);
                l++, r--;
            }
        }
        return s;
    }
};