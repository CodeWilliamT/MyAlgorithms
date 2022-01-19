using namespace std;
#include <iostream>
#include <vector>
#include <string>
//滑动窗口
//一个从前往后移动，一个从后往前移动,比较重叠部分
//时间复杂度O(N+M)*min(n,m)
class Solution {
public:
    int findLength(vector<int>& A, vector<int>& B) {
        int m = A.size();
        int n = B.size();
        int ans = 0;
        int i = 0;
        int x = m-i-1, y = i;
        int len;
        while (i<m+n)
        {
            len = 0;
            for (int j=0; x+j < m && y+j < n; j++)
            {
                if (A[x + j] == B[y + j])
                {
                    len++;
                    ans = max(ans, len);
                }
                else
                {
                    len = 0;
                }
            }
            x = m > i ? m - i : 0;
            y = m > i ? 0 : i - m;
            i++;
        }
        return ans;
    }
};
//动态规划
//状态量f[i][j]匹配到s1[i-1],s2[j-1]的匹配数
//状态转移方程f[i][j] = nums1[i - 1] == nums2[j - 1] ? f[i - 1][j - 1] + 1 : 0;
//边界f[0][0]=0
//class Solution {
//public:
//    int findLength(vector<int>& nums1, vector<int>& nums2) {
//        int m = nums1.size()+1;
//        int n = nums2.size() + 1;
//        int ans = 0;
//        vector<vector<int>> f(m,vector<int>(n));
//        f[0][0] = 0;
//        for (int i = 1; i < m; i++)
//        {
//            for (int j = 1; j < n; j++)
//            {
//                f[i][j] = nums1[i - 1] == nums2[j - 1] ? f[i - 1][j - 1] + 1 : 0;
//                ans = max(ans, f[i][j]);
//            }
//        }
//        return ans;
//    }
//};
//C
//class Solution {
//public:
//    int findLength(vector<int>& nums1, vector<int>& nums2) {
//        int m = nums1.size() + 1;
//        int n = nums2.size() + 1;
//        int ans = 0;
//        int f[m][n];
//        memset(f, 0, sizeof(f));
//        f[0][0] = 0;
//        for (int i = 1; i < m; i++)
//        {
//            for (int j = 1; j < n; j++)
//            {
//                f[i][j] = nums1[i - 1] == nums2[j - 1] ? f[i - 1][j - 1] + 1 : 0;
//                ans = max(ans, f[i][j]);
//            }
//        }
//        return ans;
//    }
//};