using namespace std;
#include<iostream>
#include<vector>
//动态规划
//一直往下加，加着加着变负数了就舍去，重新开始加，记录每次加完后最大的。
class Solution {
public:
    int maxSubArray(vector<int>& nums) {
        int rst = nums[0], preMax=0;
        for (auto& x : nums)
        {
            preMax = preMax > 0 ? preMax + x : x;
            rst = max(preMax, rst);
        }
        return rst;
    }
};