using namespace std;
#include <vector>
class Solution {
public:
    int giveGem(vector<int>& gem, vector<vector<int>>& ops) {
        for (auto& e : ops) {
            gem[e[1]] += gem[e[0]]/2;
            gem[e[0]] -= gem[e[0]]/2;
        }
        int mx = 0, mn = INT32_MAX;
        for (auto& e : gem) {
            mx = max(e, mx);
            mn = min(e, mn);
        }
        return mx - mn;
    }
};