using namespace std;
#include <iostream>
#include <vector>
#include <algorithm>
//巧思 两分查找
//先按开始时间排序，然后premax[i]倒序存储元素最大值，存储在i的开始时间以及之后的活动的最大价值。
//然后对于每个活动i从头做二分查找，找到刚好大于等于活动i结束时间的活动s。
//则答案等于max(ans, e[i][2] + premax[s]);
class Solution {
public:
    int maxTwoEvents(vector<vector<int>>& e) {
        sort(e.begin(), e.end());
        int n = e.size();
        int ans = 0;
        vector<int> latermax(n);

        latermax[n-1] = e[n - 1][2];
        for (int i = n - 2; i >= 0; i--)
        {
            latermax[i] = max(latermax[i + 1], e[i][2]);
        }
        int s;
        for (int i = 0; i < n; ++i) {
            ans = max(ans, e[i][2]);
            s = upper_bound(e.begin(), e.end(), vector<int>{e[i][1] + 1, 0, 0}) - e.begin();
            if (s < n)
                ans = max(ans, e[i][2] + latermax[s]);
        }

        return ans;
    }
};