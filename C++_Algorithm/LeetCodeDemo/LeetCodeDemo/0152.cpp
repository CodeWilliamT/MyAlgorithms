using namespace std;
#include <iostream>
#include <vector>
#include <string>
//动态规划
//最大子序和问题改，多个符号，符号用极值处理，优化，由于只跟前后有关，只存前一次的极值mx
class Solution {
public:
    int maxProduct(vector<int>& nums) {
        int n = nums.size();
        if (n == 0)return 0;
        if (n == 1)return nums[0];
        int ans = nums[0];
        int mx= ans, mn= ans, pmx= ans;
        for (int i = 1; i < n; i++)
        {
            pmx = mx;
            mx = max(nums[i] * pmx, max(nums[i] * mn, nums[i]));
            mn = min(nums[i] * mn, min(nums[i] * pmx, nums[i]));
            ans = max(mx, ans);
        }
        return ans;
    }
};
//动态规划
//最大子序和问题多个符号改，符号用极值处理，数组
//class Solution {
//public:
//    int maxProduct(vector<int>& nums) {
//        int n = nums.size();
//        if (n ==0)return 0;
//        if (n == 1)return nums[0];
//        int ans= nums[0];
//        vector<int> mx(nums);
//        vector<int> mn(nums);
//        for (int i = 1; i < n; i++)
//        {
//            mx[i] = max(nums[i] * mx[i - 1], max(nums[i] * mn[i - 1], nums[i]));
//            mn[i] = min(nums[i] * mn[i - 1], min(nums[i] * mx[i - 1], nums[i]));
//            ans = max(mx[i], ans);
//        }
//        return ans;
//    }
//};