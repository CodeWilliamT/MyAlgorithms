using namespace std;
#include <vector>
#include <functional>
//深搜(回溯)
//求得到最小 组分数(按位与) 的和 的最多分组数
//最最小组分数应为所有数的按位与的结果。
class Solution {
public:
    int maxSubarrays(vector<int>& nums) {
        int tgt = nums[0];
        int n = nums.size();
        for (int i = 1; i < n; i++) {
            tgt = tgt & nums[i];
        }
        function<int(int,int,int)> dfs = [&](int cur,int groups,int sums) {
            if (cur == n) {//判定状态可行性、边界、去重，若状态不可行，则跳过
                return sums == tgt? groups :0;
            }
            groups++;
            int andrst=nums[cur];
            int tmp;
            if (andrst + sums <= tgt) {
                tmp=dfs(cur + 1, groups, sums + andrst);
                if (tmp) {
                    return tmp;
                }
            }
            for (int i = cur + 1; i < n; i++) {
                andrst = andrst & nums[i];
                if (andrst + sums > tgt)
                    continue;
                tmp = dfs(i + 1, groups, sums + andrst);
                if (tmp) {
                    return tmp;
                }
            }
            return andrst + sums == tgt?groups:0;
        };
        return dfs(0, 0, 0);
    }
};