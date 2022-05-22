using namespace std;
#include <vector>
//前缀和 动态规划 简单模拟 
typedef long long ll;
class Solution {
public:
    int waysToSplitArray(vector<int>& nums) {
        ll sum=0;
        for (auto& e : nums) {
            sum += (ll)e;
        }
        ll hsum = 0;
        int n = nums.size();
        int rst = 0;
        for (int i = 0; i < n - 1; i++) {
            hsum += (ll)nums[i];
            if (hsum >= sum - hsum)rst++;
        }
        return rst;
    }
};