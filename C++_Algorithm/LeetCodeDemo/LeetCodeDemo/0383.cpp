using namespace std;
#include <iostream>
//哈希
class Solution {
public:
    bool canConstruct(string ransomNote, string magazine) {
        int l[26]{};
        for (char& c : magazine)
        {
            l[c - 'a']++;
        }
        for (char& c : ransomNote)
        {
            if (--l[c - 'a']<0)return false;
        }
        return true;
    }
};