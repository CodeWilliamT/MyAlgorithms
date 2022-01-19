using namespace std;
#include <iostream>
//哈希
//记下频率 比较频率
class Solution {
public:
    bool isAnagram(string s, string t) {
        if (s.size() != t.size())return false;
        int a[26]{}, b[26]{};
        for (int i = 0; i < s.size(); i++)
        {
            a[s[i] - 'a']++;
            b[t[i] - 'a']++;
        }
        for (int i = 0; i < 26; i++)
            if (a[i] != b[i])return false;
        return true;
    }
};