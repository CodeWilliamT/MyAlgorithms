using namespace std;
#include <vector>
//动态规划
//坑：可以跳跃的"最大"长度。
class Solution {
public:
    int jump(vector<int>& nums) {
        int n = nums.size();
        vector<int> f(n);
        for (int i = 0; i < n; i++) {
            f[i] = i;
        }
        int t;
        for (int i = 0; i < n; i++) {
            t = i + nums[i];
            for(int j=i+1;j<t+1&&j<n;j++)
                f[j] = min(f[j], f[i] + 1);
        }
        return f.back();
    }
};