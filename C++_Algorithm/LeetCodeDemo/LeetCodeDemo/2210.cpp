using namespace std;
#include <vector>
//简单模拟
//求峰谷数量
//求增减不一致数量,大于1则-1，否则为0
class Solution {
public:
    int countHillValley(vector<int>& nums) {
        int pre = 0, now;
        int rst = 0;
        for (int i = 1; i < nums.size(); i++) {
            now = nums[i - 1] < nums[i] ? 1 : nums[i - 1]>nums[i] ? -1 : 0;
            if (now != pre) {
                if (now != 0) {
                    rst++;
                    pre = now;
                }
            }
        }
        return rst>1?rst - 1: 0;
    }
};