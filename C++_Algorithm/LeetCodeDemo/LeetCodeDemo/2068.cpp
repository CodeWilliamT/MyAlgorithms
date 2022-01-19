using namespace std;
#include <iostream>
//哈希,朴素实现
class Solution {
public:
    bool checkAlmostEquivalent(string word1, string word2) {
        int v1[26]{}, v2[26]{};
        for (auto c:word1)
            v1[c - 'a']++;
        for (auto c : word2)
            v2[c - 'a']++;
        for (int i = 0; i < 26; i++)
        {
            if (abs(v1[i] - v2[i])>3)return false;
        }
        return true;
    }
};