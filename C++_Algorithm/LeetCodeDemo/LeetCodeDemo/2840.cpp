using namespace std;
#include <iostream>
class Solution {
public:
    bool checkStrings(string s1, string s2) {
        int st1[26]{}, st2[26]{};
        int n = s1.size();
        for (int i = 0; i < n; i++) {
            if (i % 2)st1[s1[i] - 'a']++, st1[s2[i]-'a']--;
            else st2[s1[i] - 'a']++, st2[s2[i] - 'a']--;
        }
        for (int i = 0; i < 26; i++) {
            if (st1[i] || st2[i])
                return false;
        }
        return true;
    }
};