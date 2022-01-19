using namespace std;
#include<iostream>
#include<vector>

class Solution {
public:
    int maxSubArray(vector<int>& nums) {
        int ans = nums[0], preMax=0;
        for (auto& x : nums)
        {
            preMax = preMax > 0 ? preMax + x : x;
            ans = max(preMax, ans);
        }
        return ans;
    }
};