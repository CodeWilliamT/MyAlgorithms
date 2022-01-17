using namespace std;
#include <iostream>
#include <vector>
#include <algorithm>
//两分查找 简单 朴素实现
class Solution {
public:
    vector<int> targetIndices(vector<int>& nums, int target) {
        sort(nums.begin(), nums.end());
        auto it = lower_bound(nums.begin(), nums.end(), target);
        if (it == nums.end())return {};
        vector<int> rst;
        int idx = it - nums.begin();
        while (idx<nums.size()&&nums[idx] == target)
        {
            rst.push_back(idx);
            idx++;
        }
        return rst;
    }
};