using namespace std;
#include <iostream>
#include <vector>
#include <map>
//哈希
class Solution {
public:
    int countWords(vector<string>& words1, vector<string>& words2) {
        map<string, int> mp,mp1,mp2;
        for (string& s : words1)
        {
            mp[s]++;
            mp1[s]++;
        }
        for (string& s : words2)
        {
            mp[s]++;
            mp2[s]++;
        }
        int rst = 0;
        for (auto& e : mp)
        {
            if (e.second == 2&&mp1[e.first]==1&& mp2[e.first] == 1)
            {
                rst++;
            }
        }
        return rst;
    }
};