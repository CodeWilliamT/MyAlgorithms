using namespace std;
#include <iostream>
#include <vector>
#include <string>
#include <algorithm>
#include <unordered_set>
#include <unordered_map>
#include <set>
#include <map>
#include <queue>
#include <stack>
#include <functional>
//树状数组 置换 巧思
//找三个值，在两个数组中位置都是递增的。
//转换出nums2值映射索引的数组mp,比nums2[i]索引大的数目为n-mp[nums2[i]]。
class Solution {
public:
    long long goodTriplets(vector<int>& nums1, vector<int>& nums2) {
        int n = nums1.size();
        vector<int> p(n);
        for (int i = 0; i < n; ++i)
            p[nums1[i]] = i;
        long long ans = 0;
        vector<int> tree(n + 1);
        long long less = 0;
        for (int i = 1; i < n - 1; ++i) {
            for (int j = p[nums2[i - 1]] + 1; j <= n; j += j & -j) // 将 p[nums2[i-1]]+1 加入树状数组
                ++tree[j];
            int y = p[nums2[i]];
            less = 0;
            for (int j = y; j; j &= j - 1) // 计算 less
                less += tree[j];
            ans += less * (n - 1 - y - (i - less));
        }
        return ans;
    }
};