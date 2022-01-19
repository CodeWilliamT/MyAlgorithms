using namespace std;
#include <iostream>
#include <unordered_map>
//哈希
//简单题，数量级大可用双指针优化
class Solution {
public:
    int countVowelSubstrings(string s) {
        int n = s.size();
        unordered_map<char,int> hash;
        int ans = 0;
        for (int i = 0; i < n; i++)
        {
            for (int j = i; j < n; j++)
            {
                hash[s[j]]++;
                if (hash.size() == 5 && hash['a'] && hash['e'] && hash['i'] && hash['u'] && hash['o'])
                    ans++;
            }
            hash.clear();
        }
        return ans;
    }
};