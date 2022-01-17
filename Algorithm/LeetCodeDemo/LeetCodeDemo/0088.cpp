using namespace std;
#include <iostream>
#include <vector>
//双指针轮流倒序处理
class Solution {
public:
    void merge(vector<int>& nums1, int m, vector<int>& nums2, int n) {
        if (!n)return;
        if (!m) {
            nums1 = nums2; return;
        }
        int cur = m + n - 1;
        for (int i = m-1, j = n-1; i >-1 || j > -1;cur--)
        {
            if(j <0)
            {
                nums1[cur]=nums1[i];
                i--;
            }
            else if(i<0) {
                nums1[cur] = nums2[j];
                j--;
            }
            else if (nums1[i] > nums2[j]) {
                nums1[cur] = nums1[i];
                i--;
            }
            else {
                nums1[cur] = nums2[j];
                j--;
            }
        }
    }
};