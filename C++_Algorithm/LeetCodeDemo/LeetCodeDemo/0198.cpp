using namespace std;
#include <iostream>
#include <vector>


//二刷
class Solution {
#define MAXN 101
public:
    int rob(vector<int>& nums) {
        int f[MAXN]{};
        int n = nums.size();
        if (n == 1)
            return nums[0];
        if (n == 2)
            return max(nums[0], nums[1]);
        f[0] = nums[0];
        f[1] = max(nums[0], nums[1]);
        for (int i = 2; i < n; i++) {
            f[i] = max(f[i - 2] + nums[i], f[i - 1]);
        }
        return f[n-1];
    }
};


//思路：
//边界：1个的时候为本身，2个的时候为较大的。
//从边界，得第三个开始状态转移方程：
//前i个所能偷的最大值 = Max(偷当前值 + 前i - 2的最大值，前i - 1的最大值)
//class Solution {
//public:
//    int rob(vector<int>& nums) {
//        int len = nums.size();
//        auto dp = vector<int>(len);
//        if (len == 1)return nums[0];
//        if (len == 2)return max(nums[0],nums[1]);
//        dp[0] = nums[0];
//        dp[1]= max(nums[0], nums[1]);
//        for (int i = 2; i < nums.size(); i++)
//        {
//            dp[i] = max(nums[i]+dp[i-2],dp[i-1]);
//        }
//        return dp[len - 1];
//    }
//};