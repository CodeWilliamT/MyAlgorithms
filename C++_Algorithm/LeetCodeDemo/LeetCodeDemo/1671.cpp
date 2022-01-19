using namespace std;
#include <iostream>
#include <vector>
#include <algorithm>
//动态规划+贪心+两分
//最长上升子序列问题LIS
//先从左到右遍历，找出nums[i]为最大值的递增序列需要的最大数字个数left[i]
//再从右到左遍历，找出nums[i]为最大值的递增序列需要的最大数字个数right[i]
//那么最少需要删除的个数即是n-(left[i] + right[i])的最大值(left[i]&&right[i])
class Solution {
public:
    int minimumMountainRemovals(vector<int>& nums) {
        int n = nums.size();
        vector<int> d,left(n,0), right(n, 0);
        for (int i = 0; i < n; i++) {
            if (d.empty() || nums[i] > d.back()) {
                d.push_back(nums[i]);
                left[i] = d.size()-1;
            }
            else {
                int pos = lower_bound(d.begin(), d.end(), nums[i]) - d.begin();
                left[i] = pos;
                d[pos] = nums[i];
            }
        }
        d.clear();
        for (int i = n - 1; i > -1; i--) {
            if (d.empty() || nums[i] > d.back()) {
                d.push_back(nums[i]);
                right[i] = d.size()-1;
            }
            else {
                int pos = lower_bound(d.begin(), d.end(), nums[i]) - d.begin();
                right[i] = pos;
                d[pos] = nums[i];
            }
        }
        int maxlen=0;
        for (int i = 0; i < n; i++)
        {
            if(left[i] && right[i])
                maxlen = max(maxlen, left[i] + right[i]+1);
        }
        return n-maxlen;
    }
};