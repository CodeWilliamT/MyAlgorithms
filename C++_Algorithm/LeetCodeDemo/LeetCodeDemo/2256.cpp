using namespace std;
#include <vector>
//模拟 前缀和
class Solution {
    typedef long long ll;
public:
    int minimumAverageDifference(vector<int>& nums) {
        ll headSum = 0, sum = 0,tailSum=0,val;
        int n = nums.size();
        for (int&e: nums) {
            sum += (ll)e;
        }
        int rst = 0;
        ll mn = LLONG_MAX;
        for (int i = 0; i < n; i++) {
            headSum += nums[i];
            tailSum = sum - headSum;
            val = abs(headSum / (i + 1) - ((i == n - 1) ? 0 : (tailSum / (n - i - 1))));
            if (val < mn) {
                rst = i;
                mn = val;
            }
        }
        return rst;
    }
};