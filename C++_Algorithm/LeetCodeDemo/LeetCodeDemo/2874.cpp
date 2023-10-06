using namespace std;
#include <vector>
class Solution {
    typedef long long ll;
public:
    long long maximumTripletValue(vector<int>& nums) {
        int n = nums.size();
        vector<int> lmx(n, 0),rmx(n,0);
        lmx[0] = nums[0];
        rmx[n-1] = nums[n-1];
        for (int i = 1; i < n; i++) {
            lmx[i] = max(lmx[i-1], nums[i]);
            rmx[n-1-i] = max(rmx[n-i], nums[n-1-i]);
        }
        ll rst = 0;
        for (int i = 1; i < n-1; i++) {
            rst = max(rst, (ll)(lmx[i-1] - nums[i]) * rmx[i+1]);
        }
        return rst;
    }
};