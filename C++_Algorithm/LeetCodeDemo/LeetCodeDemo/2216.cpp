using namespace std;
#include <vector>
//动态规划
//从右往左删无后效
class Solution {
public:
    int minDeletion(vector<int>& nums) {
        int n = nums.size();
        if (!n)return 0;
        int rst = 0;
        int pre = nums[n - 1];
        for (int i = n-2; i>=0; i--) {
            if (nums[i] == pre) {
                rst++;
            }
            else {
                i--;
                if(i>=0)
                    pre=nums[i];
            }
        }
        return (n-rst)%2? rst + 1:rst;
    }
};