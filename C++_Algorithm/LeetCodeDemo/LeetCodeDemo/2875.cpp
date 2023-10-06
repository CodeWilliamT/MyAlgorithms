using namespace std;
#include <vector>
#include <numeric>
//滑动窗口
//求：判断子数组某属性是否符合条件，求个数目或者最值。
//思路：夹逼思维。累计数据少了，右边界右移，增大区间；多了左边界右移，缩小区间。
//O(n),O(1);
class Solution {
public:
    //适用：已知数组 nums 和一个整数 target，求数组拼接的无限循环数组，满足 元素和 等于 target 的 最短 子数组
    int minSizeSubarray(vector<int>& nums, int target) {
        typedef long long ll;
        int n = nums.size();
        ll total = accumulate(nums.begin(), nums.end(), 0ll);
        int base = (target / total) * n;
        target = target % total;
        if (!target)
            return base;
        int v = 0;
        int rst = INT32_MAX; 
        int l = 0, r = 0;//夹逼区间，含l不含r；区间元素数为r-l;
        while (l < n) {
            if (v >= target&&l<r) {
                if(v==target)
                    rst = min(rst, r - l);
                v -= nums[l];
                l++;
            }
            else{
                if (r < 2 * n) {
                    v += nums[r % n];
                    r++;
                }
                else {
                    break;
                }
            }
        }
        return rst == INT32_MAX ? -1 : base + rst;
    }
    //适用：已知正整数数组 nums 和一个整数 k，求子数组内所有元素的乘积严格小于 k 的连续子数组的数目
    int numSubarrayProductLessThanK(vector<int>& nums, int k) {
        int n = nums.size();
        int v = 1;
        int rst = 0;
        int l = 0, r = 0;//夹逼区间，含l不含r；区间元素数为r-l;
        while (l < n) {
            if (v >= k && l < r) {
                v /= nums[l];
                l++;
            }
            else {
                rst += r - l;//以r结尾的子区间有r-l个
                if (r < n) {
                    v *= nums[r];
                    r++;
                }
                else {
                    break;
                }
            }
        }
        return rst;
    }
};