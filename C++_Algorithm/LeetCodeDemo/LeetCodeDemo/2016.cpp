using namespace std;
#include <vector>
//找规律
//维护最大，最小;维护最小的时候注意最大维护成最小就完事了。
class Solution {
public:
    int maximumDifference(vector<int>& nums) {
        int rst = 0, x = nums[0], y = x;
        for (int i = 1; i < nums.size(); i++) {
            if (nums[i] < x) {
                x = nums[i];
                y = x;
            }
            else if (nums[i] > y) {
                y = nums[i];
                rst = max(y - x, rst);
            }
        }
        return rst ? rst : -1;
    }
};