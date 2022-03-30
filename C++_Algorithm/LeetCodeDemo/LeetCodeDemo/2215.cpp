using namespace std;
#include <vector>
#include <unordered_set>
#include <unordered_map>
//哈希
class Solution {
public:
    vector<vector<int>> findDifference(vector<int>& nums1, vector<int>& nums2) {
        unordered_map<int,int> mp;
        unordered_set<int> st1,st2;
        for (int& e : nums1) {
            if (!st1.count(e)) {
                st1.insert(e);
                mp[e]++;
            }
        }
        for (int& e : nums2) {
            if (!st2.count(e)) {
                st2.insert(e);
                mp[e]++;
            }
        }
        vector<vector<int>> rst(2);
        for (auto& e : mp) {
            if (e.second == 1)
            {
                if (st1.count(e.first)) {
                    rst[0].push_back(e.first);
                }
                else {

                    rst[1].push_back(e.first);
                }
            }
        }
        return rst;
    }
};