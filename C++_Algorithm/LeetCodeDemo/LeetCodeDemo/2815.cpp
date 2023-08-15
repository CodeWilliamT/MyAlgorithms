using namespace std;
#include <vector>
//简单模拟
class Solution {
public:
    int maxSum(vector<int>& nums) {
        int n = nums.size();
        int rst = -1;
        vector<int> big(n, 0);
        int cur;
        for (int i = 0; i < n; i++) {
            cur = nums[i];
            while (cur) {
                big[i] = max(big[i], cur % 10);
                cur /= 10;
            }
        }
        for (int i = 0; i < n; i++) {
            for (int j = i+1; j < n; j++) {
                if (big[i] == big[j]) {
                    rst = max(rst,nums[i] + nums[j]);
                }
            }
        }
        return rst;
    }
};