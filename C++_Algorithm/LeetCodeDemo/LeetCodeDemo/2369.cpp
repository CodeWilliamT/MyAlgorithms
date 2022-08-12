using namespace std;
#include <iostream>
#include <vector>
//动态规划 贪心
//用个数组记录前x个是否为可行划分，遍历一遍，4个一判定，同位置已可行就不扫，不可行就继续扫。
class Solution {
    bool check(vector<int>& nums,int l,int r) {
        int n = r-l+1;
        if (n < 2)return false;
        if (n == 2)
            return nums[l] == nums[l + 1];
        if (n == 3)
            return nums[l] == nums[l+1] && nums[l+1] == nums[l+2]|| nums[l] + 1==nums[l + 1]&& nums[l+1] + 1 == nums[l + 2];
        if (n == 4)
            return nums[l] == nums[l+1] && nums[l+2] == nums[l+3];
        return false;
    }
public:
    bool validPartition(vector<int>& nums) {
        int n = nums.size();
        vector<bool> v(n, 0);
        bool rst;
        for (int j = 1; j < 4&&j<n; j++) {
            v[j] = check(nums, 0, j);
        }
        for (int i = 0; i < n; i++) {
            if (v[i])
                for (int j = 2; j <= 4 && i + j < n; j++) {
                    if (!v[i + j]) {
                        v[i + j] = check(nums, i + 1, i + j);
                    }
                }
        }
        return v[n-1];
    }
};