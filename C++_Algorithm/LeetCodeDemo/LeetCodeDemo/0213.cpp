using namespace std;
#include <vector>
//0-n-2 与 1-n-1内最大的。
class Solution {
#define MAXN 101
public:
    int rob(vector<int>& nums) {
        int n = nums.size();
        if (n == 1)
            return nums[0];
        if (n == 2)
            return max(nums[0], nums[1]);
        if (n == 3)
            return max({ nums[0], nums[1],nums[2] });
        int f[MAXN]{};
        f[0] = nums[0];
        f[1] = max(nums[0], nums[1]);
        for (int i = 2; i < n-1; i++) {
            f[i] = max(f[i - 2] + nums[i], f[i - 1]);
        }
        int rst = f[n - 2];
        f[1] = nums[1];
        f[2] = max(nums[1], nums[2]);
        for (int i = 3; i < n; i++) {
            f[i] = max(f[i - 2] + nums[i], f[i - 1]);
        }
        return max(rst,f[n-1]);
    }
};