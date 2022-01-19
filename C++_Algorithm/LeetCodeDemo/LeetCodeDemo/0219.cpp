using namespace std;
#include <vector>
#include <unordered_set>
//哈希 滑动窗口(窗口数据前后维护)
class Solution {
public:
    bool containsNearbyDuplicate(vector<int>& nums, int k) {
        if (!k)return false;
        unordered_set<int> st;
        int n = nums.size();
        for (int i = 0;i < n; i++) {
            if (st.count(nums[i]))
                return true;
            if (i >= k) {
                st.erase(nums[i - k]);
            }
            st.insert(nums[i]);
        }
        return false;
    }
};