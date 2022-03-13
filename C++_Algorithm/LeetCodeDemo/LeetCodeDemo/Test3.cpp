using namespace std;
#include <vector>
//细致条件分析 策略或搜索
//策略：
//删除[0,k-2],答案为max(nums[0,k-2],nums[k]);
class Solution {
public:
    int maximumTop(vector<int>& nums, int k) {
        int n = nums.size();
        if (n == 1) {
            return (k - n) % 2 ? nums[0] : -1;
        }
        if (k == 1)return nums[1];
        int rst = 0,scd=0;
        if (k <= n) {
            for (int i = 0; i < k - 1; i++) {
                rst = max(rst, nums[i]);
            }
            if(k<n)
                rst = max(rst, nums[k]);
            return rst;
        }
        else {
            for (int i = 0; i < n; i++) {
                rst = max(rst, nums[i]);
            }
            return rst;
        }
    }
};