using namespace std;
#include <vector>
//哈希
//找有的，找不重的。
class Solution { 
public:
    vector<vector<int>> findDifference(vector<int>& nums1, vector<int>& nums2) {
        bool mp1[2001]{}, mp2[2001]{}, mp[2001]{};
        for (int& e : nums1) {
            if (!mp1[e+1000]) {
                mp1[e + 1000]=1;
            }
        }
        for (int& e : nums2) {
            if (!mp2[e + 1000]) {
                mp2[e + 1000] = 1;
            }
        }
        vector<vector<int>> rst(2);
        for (int& e : nums1) {
            if (!mp2[e + 1000]&&!mp[e+1000]) {
                rst[0].push_back(e);
                mp[e + 1000] = 1;
            }
        }
        for (int& e : nums2) {
            if (!mp1[e + 1000] && !mp[e + 1000]) {
                rst[1].push_back(e);
                mp[e + 1000] = 1;
            }
        }
        return rst;
    }
};