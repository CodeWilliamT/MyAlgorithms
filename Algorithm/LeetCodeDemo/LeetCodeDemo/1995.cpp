using namespace std;
#include <vector>
//朴素实现 枚举
class Solution {
public:
    int countQuadruplets(vector<int>& nums) {
        int n = nums.size();
        int rst=0;
        for (int a = 0; a<n-3;a++)
        {
            for (int b = a + 1; b < n - 2; b++) {
                for (int c = b + 1; c < n - 1; c++){
                    for (int d = c + 1; d < n; d++) {
                        if (nums[a] + nums[b] + nums[c] == nums[d]) {
                            rst++;
                        }
                    }
                }
            }
        }
        return rst;
    }
};