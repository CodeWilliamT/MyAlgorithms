using namespace std;
#include <vector>
#include <algorithm>
//分治 动态规划 两分查找
//需求1：将k个数组变为非递减的
//性质1：对于给定的一个序列，如果我们希望通过修改最少的元素，使得它单调递增，那么最少需要修改的元素个数，就是「序列的长度」减去「序列的最长递增子序列」的长度。
//求最长非递减子序列：声明缓存数组inc 存储每一个长度下的最小结尾，
//遍历数组，在缓存数组中两分查找当前元素arr[i] 在缓存数组inc中的敲好大过的排位idx， 如果idx==inc.size()，则inc.push_back(arr[i])，否则inc[idx] = min(inc[idx], arr[i]);
//答案为K个最长增长子序列跟遍历子的数组长度的差值和。
class Solution {
public:
    int kIncreasing(vector<int>& arr, int k) {
        int n = arr.size();
        int inc[100001];
        int idx=0,l=0,len;
        int rst = 0;
        for (int t = 0; t < k; t++) {
            l = 0;
            len = 0;
            for (int i = t; i < n; i += k) {
                len++;
                idx=upper_bound(inc, inc+l, arr[i])-inc;
                if (idx == l) {
                    inc[idx] = arr[i];
                    l++;
                }
                else {
                    inc[idx] = min(inc[idx], arr[i]);
                }
            }
            rst += l;
        }
        return n-rst;
    }
};