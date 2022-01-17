using namespace std;
#include <iostream>
#include <vector>
#include <algorithm>
//贪心+动态规划+两分 O(nlog(n))
//核心
//生成一个各个长度的子序列的最小尾数的排序数组，不断二分查找各个元素的排位进行操作。
//维护一个数组 inc[i]，存储长度为 i +1的最长上升子序列的末尾元素的最小值
//具体
//求最长非递减子序列：
//声明缓存数组inc 存储每一个长度下的最小结尾，
//遍历数组，
//在缓存数组中两分查找当前元素nums[i] 在缓存数组inc中的(恰好大过 = 非严格递增)(恰好大于等于 = 严格递增)的排位idx，
//如果idx == inc.size()，则
//inc.push_back(nums[i])，
//否则
//inc[idx] = min(inc[idx], nums[i]);
//c++动态数组
class Solution {
public:
    int lengthOfLIS(vector<int>& nums) {
        int n = nums.size();
        vector<int> inc;
        int idx;
        for (int i = 0; i < n; i++) {
            idx = lower_bound(inc.begin(), inc.end(), nums[i]) - inc.begin();
            if (idx == inc.size())
                inc.push_back(nums[i]);
            else
                inc[idx] = min(inc[idx], nums[i]);
        }
        return inc.size();
    }
};
//c的定长数组
//class Solution {
//public:
//    int lengthOfLIS(vector<int>& nums) {
//        int n = nums.size();
//        int inc[2500]{};
//        int len = 0;
//        int idx;
//        for (int i = 0; i < n; i++) {
//            idx = lower_bound(inc, inc + len, nums[i]) - inc;
//            if (idx == len)
//                inc[len++] = nums[i];
//            else
//                inc[idx] = min(inc[idx], nums[i]);
//        }
//        return len;
//    }
//};
//class Solution {
//public:
//    int lengthOfLIS(vector<int>& nums) {
//        int n = nums.size();
//        if (n < 2)return n;
//        vector<int> d(n + 1, 0);
//        int len = 0;
//        for (int i = 0; i < n; i++) {
//            int l = 0, r = len, mid;
//            while (l < r) {
//                mid = (l + r) >> 1;
//                if (d[mid] < nums[i])
//                    l = mid + 1;
//                else
//                    r = mid;
//            }
//            if (l == len)
//                len++;
//            d[l] = nums[i];
//        }
//        return len;
//    }
//};
////动态规划 O(n²)
//状态量dp[i] 为考虑前 i 个元素，以a[i]为结尾的最长上升子序列的长度
//边界d[i]=1
//状态转移方程dp[i]=max(dp[j]+1,dp[i])
//class Solution {
//public:
//    int lengthOfLIS(vector<int>& a) {
//        int n = a.size();
//        if (n < 2)return n;
//        int ans = 0;
//        vector<int> d(n,0);
//        int preX;
//        for (int i = 0; i < n; i++)
//        {
//            d[i] = 1;
//            preX = a[i];
//            for (int j = 0; j < i; j++)
//            {
//                if (a[j] < a[i])
//                {
//                    d[i] = max(d[i], d[j] + 1);//内循环比较寻找更优解刷新d[i]
//                }
//            }
//            ans = max(ans, d[i]);
//        }
//        return ans;
//    }
//};