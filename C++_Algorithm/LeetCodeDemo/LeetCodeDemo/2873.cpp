using namespace std;
#include <vector>
class Solution {
    typedef long long ll;
public:
    long long maximumTripletValue(vector<int>& nums) {
        int n = nums.size();
        ll rst = 0;
        for (int i = 0; i < n-2; i++) {
            for (int j = i + 1; j < n - 1; j++) {
                for (int k = j + 1; k < n; k++) {
                    rst = max(rst, (ll)(nums[i] - nums[j]) * nums[k]);
                }
            }
        }
        return rst;
    }
};