using namespace std;
#include <vector>
#include <map>
//哈希
//记录所有人，输的+1
class Solution {
public:
    vector<vector<int>> findWinners(vector<vector<int>>& m) {
        map<int, int> mp;
        for (auto& e : m) {
            mp[e[0]] = mp[e[0]];
            mp[e[1]]++;
        }
        vector<vector<int>> rst(2);
        for (auto& e : mp) {
            if (!e.second)
                rst[0].push_back(e.first);
            else if (e.second==1)
                rst[1].push_back(e.first);
        }
        return rst;
    }
};