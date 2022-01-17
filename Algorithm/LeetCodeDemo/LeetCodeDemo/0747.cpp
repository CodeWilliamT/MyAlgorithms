using namespace std;
#include <vector>
//简单题 朴素实现
//取俩数刷新，一个最大值，一个次大值。
//当当前值大于最大值时,刷新次大值为原最大值跟最大值。
//当其不大于最大值但大于次大值时，刷新次大值。
class Solution {
public:
    int dominantIndex(vector<int>& nums) {
        int a = -1, b = -1,idx=-1;
        for (int i = 0; i < nums.size();i++) {
            if (nums[i] > a) {
                b = a;
                a = nums[i];
                idx = i;
            }
            else if (nums[i] > b) {
                b = nums[i];
            }
        }
        return a >= 2 *b ? idx : -1;
    }
};