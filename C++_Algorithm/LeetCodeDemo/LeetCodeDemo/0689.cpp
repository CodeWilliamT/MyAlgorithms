using namespace std;
#include <iostream>
#include <vector>
//动态规划 滑动窗口
//互不重叠的3个连续k项最大总和
//连续k项和：前缀和做差
//最大3数和：记录最大的第一个数mx1，索引mx1idx1，最大的两个数和mx12,索引mx12idx1，mx12idx2，最大的三数和mx123，
//同时遍历三个数的值，每次遍历比较第一个数跟mx1,较大则刷新mx1,mx1idx1,
//第二个数与mx1的和跟mx12,较大则刷新mx12,mx12idx1=mx1idx1,mx12idx2=y,
//第三个数与mx12的和跟mx123,较大则刷新mx123,答案{mx12idx1,mx12idx2,z}
//互不重叠：某一最大值刷新的时候，保证与其他最大值范围不重叠
//遍历区间，i,同时计算对应区间总和，变化三个索引x=i-3*k+1,y=i-2*k+1,z=i-k+1保证区间互斥，判定，当总最大值更新，则记录触发更新的索引。
class Solution {
public:
    vector<int> maxSumOfThreeSubarrays(vector<int>& nums, int k) {
        int n = nums.size();
        int max1=0,max12=0,max123=0;
        int sum[3]{};
        int max1idx1 = 0, max12idx1 = 0, max12idx2 = 0;
        vector<int> rst(3,0);
        int x, y, z;
        for (int i = 2*k; i < n; i++) {
            x = i - 3 * k + 1;
            y = i - 2 * k + 1;
            z = i - k + 1;
            sum[0] += nums[x + k - 1];
            sum[1] += nums[y + k - 1];
            sum[2] += nums[z + k - 1];
            if (x >= 0){
                if (sum[0] > max1)
                    max1=sum[0], max1idx1=x;
                if (sum[1]+max1 > max12)
                    max12 = sum[1] + max1, max12idx1 = max1idx1, max12idx2 = y;
                if (sum[2]+ max12 > max123)
                    rst = { max12idx1,max12idx2 ,z }, max123 = sum[2] + max12;
                sum[0] -= nums[x];
                sum[1] -= nums[y];
                sum[2] -= nums[z];
            }
        }
        return rst;
    }
};