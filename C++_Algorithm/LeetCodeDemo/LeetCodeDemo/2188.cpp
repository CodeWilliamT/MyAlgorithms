using namespace std;
#include <vector>
//动态规划 分治
//求跑x圈的最短用时
//先求1条轮胎跑i圈的最短用时f[i]，作为边界
//然后背包问题类似，分治思想，
//换胎的最小用时为f[i] = min(f[i], f[j] + f[i - j] + changeTime);
class Solution {
public:
    int minimumFinishTime(vector<vector<int>>& tires, int changeTime, int numLaps) {
        int n = tires.size();
        vector<int> f(numLaps + 1, INT32_MAX);
        long long still,change,delta;
        for (int i = 0; i < n; i++) {
            f[1] = min(f[1],tires[i][0]);
            delta = tires[i][0];
            still = delta;
            for (int j = 2; j <=numLaps; j++) {
                delta*= tires[i][1];
                still += delta;
                if (still > INT32_MAX)break;
                f[j] = min(f[j],(int)still);
            }
        }
        for (int i = 2; i <= numLaps; i++) {
            for (int j = 1; j < i; j++) {
                f[i] = min(f[i], f[j] + f[i - j] + changeTime);
            }
        }
        return f[numLaps];
    }
};