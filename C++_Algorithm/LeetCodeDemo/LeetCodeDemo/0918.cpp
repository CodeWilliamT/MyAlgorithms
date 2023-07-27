using namespace std;
#include <vector>
//动态规划 最大子序列 最小子序列
//rst=Sum(nums)-最小子序列==0？最大子序列:max(最大子序列，Sum(nums)-最小子序列);
class Solution {
public:
    int maxSubarraySumCircular(vector<int>& nums) {
        int n = nums.size();
        if (n == 1)return nums[0];
        int rst = nums[0],subMin=nums[0];
        int preMax = 0, preMin = 0;
        int sum = 0;
        for (auto & x : nums)
        {
            preMax = preMax > 0 ? preMax + x : x;
            preMin = preMin < 0 ? preMin + x : x;
            rst = max(preMax, rst);
            subMin = min(subMin, preMin);
            sum += x;
        }
        if(sum!=subMin)rst = max(rst, sum - subMin);
        return rst;
    }
};