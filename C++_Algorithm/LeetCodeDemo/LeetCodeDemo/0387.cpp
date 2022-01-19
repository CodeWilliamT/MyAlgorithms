using namespace std;
#include <iostream>
//简单题，哈希，朴素实现
//遍历，记下频率，然后再遍历
class Solution {
public:
    int firstUniqChar(string s) {
        int v[26]{};
        for (auto& c : s)
        {
            v[c - 'a']++;
        }
        for (int i=0;i<s.size();i++)
        {
            if(v[s[i] - 'a']==1)return i;
        }
        return -1;
    }
};