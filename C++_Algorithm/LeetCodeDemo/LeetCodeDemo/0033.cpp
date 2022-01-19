using namespace std;
#include <vector>
#include <algorithm>
//两分 巧思
//第一个前面大于后面的位置为旋转点,记下，对数组前后段分别做一次两分查找target，看有无下标
class Solution {
public:
    int search(vector<int>& nums, int target) {
        int i = 1;
        for (;i < nums.size(); i++)
        {
            if (nums[i-1] > nums[i])
            {
                break;
            }
        }
        auto it=lower_bound(nums.begin(), nums.begin() + i, target);
        if (it != nums.begin() + i&&*it==target)return it - nums.begin();
        it = lower_bound(nums.begin()+i, nums.end(), target);
        if (it != nums.end()&& *it == target)return it - nums.begin();
        return -1;
    }
};