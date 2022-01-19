using namespace std;
#include <iostream>
#include <vector>
#include <string>
#include <algorithm>
#include <unordered_map>
//哈希
class Solution {
public:
    vector<string> findRelativeRanks(vector<int>& score) {
        unordered_map<int, int> mp;
        int n = score.size();
        vector<int> tmp = score;
        string lst[4] = { "","Gold Medal","Silver Medal","Bronze Medal" };
        sort(tmp.begin(), tmp.end(), [] (int a,int b){return a > b; });
        for (int i = 0; i < n; i++) {
            mp[tmp[i]] = i+1;
        }
        vector<string> rst;
        for (int& e : score)
        {
            if(mp[e]<4)
                rst.push_back(lst[mp[e]]);
            else
                rst.push_back(to_string(mp[e]));
        }
        return rst;
    }
};