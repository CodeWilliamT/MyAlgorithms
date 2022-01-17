﻿using namespace std;
#include <iostream>
#include <vector>

//思路：
//边界：1个的时候为本身，2个的时候为较大的。
//从边界，得第三个开始状态转移方程：
//前i个所能偷的最大值 = Max(偷当前值 + 前i - 2的最大值，前i - 1的最大值)
class Solution {
public:
    int rob(vector<int>& nums) {
        int a = 0;
        int len = nums.size();
        auto dp = vector<int>(len);
        if (len == 1)return nums[0];
        if (len == 2)return max(nums[0],nums[1]);
        dp[0] = nums[0];
        dp[1]= max(nums[0], nums[1]);
        for (int i = 2; i < nums.size(); i++)
        {
            dp[i] = max(nums[i]+dp[i-2],dp[i-1]);
        }
        return dp[len - 1];
    }
};

//int main()
//{
//    vector<int> a = { 1, 2, 3, 1 };
//    Solution s;
//    cout<<s.rob(a)<<endl;
//}