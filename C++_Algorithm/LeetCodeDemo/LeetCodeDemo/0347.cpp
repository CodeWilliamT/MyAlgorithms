using namespace std;
#include <vector>
#include <algorithm>
#include <unordered_map>
//哈希
class Solution {
    using pii = pair<int, int>;
public:
    vector<int> topKFrequent(vector<int>& nums, int k) {
        unordered_map<int, int> mp;
        for (int& e : nums) {
            mp[e]++;
        }
        vector<pii> tmp;
        for (auto& e : mp) {
            tmp.push_back(e);
        }
        sort(tmp.begin(), tmp.end(), [&](pii& x, pii& y) {return x.second>y.second; });
        vector<int> rst;
        for (int i = 0; i < k && i < tmp.size();i++) {
            rst.push_back(tmp[i].first);
        }
        return rst;
    }
};