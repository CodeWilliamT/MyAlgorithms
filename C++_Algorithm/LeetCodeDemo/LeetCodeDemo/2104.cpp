using namespace std;
#include <vector>
//动态规划
class Solution {
public:
    long long subArrayRanges(vector<int>& nums) {
        int n = nums.size();
        vector<vector<int>> mx(n, vector<int>(n)), mn(n, vector<int>(n));

        for (int i = 0; i < n; i++) {
            mx[i][i] = nums[i];
            mn[i][i] = nums[i];
        }
        long long rst = 0;
        for (int i = 0; i < n; i++) {
            for (int j = i+1; j < n; j++) {
                mx[i][j] = max(mx[i][j - 1], nums[j]);
                mn[i][j] = min(mn[i][j - 1], nums[j]);
                rst += mx[i][j] - mn[i][j];
            }
        }
        return rst;
    }
};