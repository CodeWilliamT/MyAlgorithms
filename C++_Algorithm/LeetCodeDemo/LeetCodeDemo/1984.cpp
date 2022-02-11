using namespace std;
#include <vector>
#include <algorithm>
//简单题
//排序后 获取nums[i] - nums[i - k + 1]的最小值。
class Solution {
public:
    int minimumDifference(vector<int>& nums, int k) {
        sort(nums.begin(), nums.end());
        int rst = nums[k - 1]- nums[0];
        for (int i = k; i < nums.size(); i++) {
            rst = min(rst,nums[i] - nums[i - k + 1]);
        }
        return rst;
    }
};