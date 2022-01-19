using namespace std;
#include <iostream>
#include <vector>
//巧思，二分查找
//因为可返回其中一个，利用峰值特性进行二分查找
//峰值特性：而峰值会在升序的右边，降序的左边
class Solution {
public:
    int search(vector<int>& nums, int l, int r) {
        if (l == r)
            return l;
        int mid = (l + r) / 2;
        if (nums[mid] > nums[mid + 1])
            return search(nums, l, mid);
        return search(nums, mid + 1, r);
    }
    int findPeakElement(vector<int>& nums) {
        return search(nums, 0, nums.size() - 1);
    }
};
//优化比较的线性扫描
//找到递增的位置判定后面有没有递减
//class Solution {
//public:
//    int findPeakElement(vector<int>& nums) {
//        int n = nums.size();
//        if (n == 1)return 0;
//        if (nums[0] > nums[1])return 0;
//        int ans = 0;
//        for (int i = 1; i < n - 1; i++)
//        {
//            while (nums[i - 1] >= nums[i] && i < n - 1)i++;
//            if (i < n - 1)
//                if (nums[i] > nums[i + 1])
//                {
//                    return i;
//                }
//        }
//        return n - 1;
//    }
//};