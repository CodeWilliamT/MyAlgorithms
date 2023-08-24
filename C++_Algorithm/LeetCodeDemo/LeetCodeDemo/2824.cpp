using namespace std;
#include <vector>
//朴素实现
class Solution {
public:
    int countPairs(vector<int>& nums, int target) {
        int n = nums.size();
        int rst = 0;
        for (int i = 0; i < n; i++) {
            for (int j = i + 1; j < n; j++) {
                rst += (nums[i] + nums[j] < target);
            }
        }
        return rst;
    }
};