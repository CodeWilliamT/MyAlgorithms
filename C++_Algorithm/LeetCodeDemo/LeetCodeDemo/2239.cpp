using namespace std;
#include <vector>
//简单模拟
class Solution {
public:
    int findClosestNumber(vector<int>& nums) {
        int rst = nums[0];
        for (int& e : nums) {
            if (abs(e) < abs(rst)) {
                rst = e;
            }
            else if (abs(e) == abs(rst) && rst < 0) {
                rst = e;
            }
        }
        return rst;
    }
};