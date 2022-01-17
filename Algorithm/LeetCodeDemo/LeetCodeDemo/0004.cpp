using namespace std;
#include<vector>
#include<iostream>
#include<iterator>
#include<string>
#include<algorithm>
//转化为找两个有序数组中第k小的数
class Solution {
private:
    //维护两个指针，初始时分别指向两个数组的下标 00 的位置，每次将指向较小值的指针后移一位（如果一个指针已经到达数组末尾，则只需要移动另一个数组的指针）
    double findKst(vector<int>& nums1, vector<int>& nums2, int k)
    {
        int n = nums1.size();
        int m = nums2.size();
        if (k > m + n - 1)return -1;
        int i = 0, j = 0;
        while (true)
        {
            if (i == n)return nums2[k - i];
            if (j == m)return nums1[k - j];
            if (i + j == k)
            {
                return min(nums1[i], nums2[j]);
            }
            if (nums1[i] < nums2[j])i++;
            else j++;
        }
    }
public:
    double findMedianSortedArrays(vector<int>& nums1, vector<int>& nums2) {
        vector<int> mergenums;
        int count = nums1.size() + nums2.size();
        if (count & 1)
        {
            return findKst(nums1, nums2, count / 2);
        }
        else
        {
            return (findKst(nums1, nums2, count / 2-1) +findKst(nums1, nums2, count / 2))*0.5;
        }

    }
};
//stl algorithm merge
//class Solution {
//public:
//    double findMedianSortedArrays(vector<int>& nums1, vector<int>& nums2) {
//        vector<int> mergenums;
//
//        mergenums.insert(mergenums.end(), nums1.begin(), nums1.end());
//        mergenums.insert(mergenums.end(), nums2.begin(), nums2.end());
//        sort(mergenums.begin(),mergenums.end());
//        //merge(nums1.begin(), nums1.end(),nums2.begin(), nums2.end(), back_inserter(mergenums));
//        int n = mergenums.size();
//        return n & 1 ? mergenums[n / 2] : ((mergenums[n / 2 - 1] + mergenums[n / 2]) * 0.5);
//    }
//};