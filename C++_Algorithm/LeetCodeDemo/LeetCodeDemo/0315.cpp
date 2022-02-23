using namespace std;
#include <vector>
//树状数组
//求nums[i]右区间刚好小于nums[i]的元素的数目;
//从右往左变为 值映射频率的数组，求前缀和
//因为是小于所以查询时索引-1,+10000是因为值域变为0-20000,构造前缀树所以+1;
class Solution {
public:
    vector<int> countSmaller(vector<int>& nums) {
        int n = nums.size();
        vector<int> counts(n, 0);
        vector<int> tree(20002, 0);
        int cnt;
        for (int i = 0; i < n; i++) {
            //查询前缀和
            cnt = 0;
            for (int j = nums[n - 1 - i] + 10001 - 1; j > 0; j -= j & -j) {
                cnt += tree[j];
            }
            counts[n - 1 - i] = cnt;
            //修改前缀和
            for (int j = nums[n - 1 - i] + 10001; j <= 20001; j += j & -j) {
                tree[j]++;
            }
        }
        return counts;
    }
};