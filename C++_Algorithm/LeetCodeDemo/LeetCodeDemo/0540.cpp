using namespace std;
#include <vector>
//两分查找 找规律
//若m是奇数那么跟前一个相等，或m是偶数跟后一个相等则表示这段正常，否则就不正常
class Solution {
public:
    int singleNonDuplicate(vector<int>& nums) {
        int l = 0, r = nums.size() - 1, m = (l + r) / 2;
        while (l < r) {
            m = (l + r) / 2;
            if (m % 2 && nums[m] == nums[m - 1] || m % 2 == 0 && nums[m] == nums[m + 1]) {
                l = m + 1;
            }
            else {
                r = m;
            }
        }
        return nums[l];
    }
};