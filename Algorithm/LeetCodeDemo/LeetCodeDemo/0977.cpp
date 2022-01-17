using namespace std;
#include <iostream>
#include <vector>
#include <algorithm>
//归并排序
//双指针
//针对对有序序列进行排序
//用两个指针分别指向两个数组极值端元素依次比较，然后依次将极值填入第三个数组中
class Solution {
public:
    vector<int> sortedSquares(vector<int>& nums) {
        int n = nums.size();
        vector<int> ans(n);
        for (int i = 0, j = n - 1, pos = n - 1; i <= j;)
        {
            if (nums[i] * nums[i] > nums[j] * nums[j])
            {
                ans[pos] = nums[i] * nums[i];
                ++i;
            }
            else
            {
                ans[pos] = nums[j] * nums[j];
                --j;
            }
            --pos;
        }
        return ans;
    }
};
//排序
//简单实现
//class Solution {
//public:
//    vector<int> sortedSquares(vector<int>& nums) {
//        vector<int> ans = nums;
//        for (int i = 0; i < ans.size(); i++)
//        {
//            ans[i] *= ans[i];
//        }
//        sort(ans.begin(), ans.end());
//        return ans;
//    }
//};