using namespace std;
#include <vector>
//两分查找
//手打
class Solution {
public:
    int search(vector<int>& nums, int target) {
        int r = nums.size()-1;
        int l = 0,m;
        while (l < r)
        {
            m = (l + r) / 2;
            if (nums[m] < target)
                l = m+1;
            else
                r = m;
        }
        return nums[r]==target?r:-1;
    }
};
//两分查找
//类库自带
//class Solution {
//public:
//    int search(vector<int>& nums, int target) {
//        auto it = lower_bound(nums.begin(), nums.end(), target);
//        if (it == nums.end() || *it != target)return -1;
//        return it - nums.begin();
//    }
//};