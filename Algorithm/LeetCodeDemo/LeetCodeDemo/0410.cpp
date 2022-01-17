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
//二分查找 + 贪心
//两分查找：因为找的是最大和的最小值，
//由题知,答案的值域为[最大元素值，数组和],在值域内两分查找，
//能分小于等于m组则说明x偏大则在小区间找，需要分大于m组说明x偏小在大区间找。
//贪心：查找判定x满足，令每次分组和刚好小等于x，能分出的组数是否小于等于m组
class Solution {
public:
    //贪心：查找判定x满足，令每次分组和刚好小等于x，能分出的组数是否小于等于m组
    bool check(vector<int>& nums, int x, int m) {
        //当前分割子数组的和
        long long sum = 0;
        //已经分割出的子数组的数量
        int cnt = 1;
        for (int i = 0; i < nums.size(); i++) {
            if (sum + nums[i] > x) {
                cnt++;
                sum = nums[i];
            }
            else {
                sum += nums[i];
            }
        }
        return cnt <= m;
    }

    int splitArray(vector<int>& nums, int m) {
        //left初值为nums中最大值，right初值为nums总和
        long long left = 0, right = 0;
        for (int i = 0; i < nums.size(); i++) {
            right += nums[i];
            if (left < nums[i]) {
                left = nums[i];
            }
        }
        //两分查找
        while (left < right) {
            long long mid = (left + right) >> 1;
            if (check(nums, mid, m)) {
                right = mid;
            }
            else {
                left = mid + 1;
            }
        }
        return left;
    }
};
//动态规划
//状态量：前i个元素分为j组的和最大值的最小值
//f[i][j] =min{ max(f[k][j−1],sub(k + 1,i)) }
//边界f[0][0]=0
//class Solution {
//public:
//    int splitArray(vector<int>& nums, int m) {
//        int n = nums.size();
//        vector<vector<long long>> f(n + 1, vector<long long>(m + 1, LLONG_MAX));
//        vector<long long> sub(n + 1, 0);
//        for (int i = 0; i < n; i++) {
//            sub[i + 1] = sub[i] + nums[i];
//        }
//        f[0][0] = 0;
//        for (int i = 1; i <= n; i++) {
//            for (int j = 1; j <= min(i, m); j++) {
//                for (int k = 0; k < i; k++) {
//                    f[i][j] = min(f[i][j], max(f[k][j - 1], sub[i] - sub[k]));
//                }
//            }
//        }
//        return (int)f[n][m];
//    }
//};
//深搜，超时
//迭代量：该搜索的最大值，当前分组右索引，当前组数，剩下数的和，答案，数组，需要分的组数
//将0到m-2组的右分界线从最左侧开始一个个往右慢慢移动。找到最小值
//剪枝：当有解时,后续和大的不要了。
//class Solution {
//    void dfs(int smx,int idx,int gn,int rest,int& ans,vector<int>& a,int m)
//    {
//        if (smx >= ans)return;
//        if (gn >= m-1)
//        {
//            smx = max(rest, smx);
//            ans = min(smx,ans);
//            return;
//        }
//        int sum=0;
//        for (int i = idx + 1; i < a.size(); i++)
//        {
//            sum += a[i];
//            if (sum >= ans)return;
//            smx = max(smx, sum);
//            dfs(smx, i, gn + 1,rest-sum, ans, a, m);
//        }
//    }
//public:
//    int splitArray(vector<int>& a, int m) {
//        int n = a.size();
//        int ans = 1000000000;
//        int asum = 0;
//        for (int i = 0; i < n; i++)
//        {
//            asum += a[i];
//        }
//        dfs(0, -1, 0,asum, ans, a, m);
//        return ans;
//    }
//};