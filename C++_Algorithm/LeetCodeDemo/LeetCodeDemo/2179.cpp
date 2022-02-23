using namespace std;
#include <vector>
//树状数组 巧思 置换
//找三个值，在两个数组中位置都是递增的。
//转换出nums1值映射索引的数组mp。
//用y=mp[nums2[i]]构成新的数组。
//则求新构成的数组中,左侧小于y的元素*右侧大于y的元素的乘积的总和。
//然后值映射频率，转化为求至nums2[i]元素构成的 y 映射 y出现频率 数组中，小于y元素频率的前缀和。用树状数组计算。
class Solution {
public:
    long long goodTriplets(vector<int>& nums1, vector<int>& nums2) {
        int n = nums1.size();
        vector<int> mp(n),tree(n+1);
        for (int i = 0; i < n; i++) {
            mp[nums1[i]] = i;
        }
        long long cnt;
        long long rst = 0;
        int y;
        for (int i = 0; i < n; i++) {
            //查询前缀和
            cnt = 0;
            y = mp[nums2[i]];
            for (int j = y; j > 0; j -= j & -j) {
                cnt += tree[j];
            }
            rst += cnt * (n - 1- y - (i - cnt));
            //修改前缀和
            for (int j = y+1; j <= n; j += j & -j) {
                tree[j]++;
            }
        }
        return rst;
    }
};