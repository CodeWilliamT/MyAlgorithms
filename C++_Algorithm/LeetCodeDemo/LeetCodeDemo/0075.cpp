using namespace std;
#include <iostream>
#include <vector>

//巧思 三指针 原地交换
class Solution {
public:
    void sortColors(vector<int>& nums) {
        int n = nums.size();
        for (int i = 0, s = 0,e=n-1; i <= e&&s<=e;)
        {
            if (nums[i] == 0)swap(nums[i], nums[s]),s++,i=max(s,i);
            else if (nums[i] == 2)swap(nums[i], nums[e]),e--;
            else i++;
        }
    }
};
//哈希
//class Solution {
//public:
//    void sortColors(vector<int>& nums) {
//        int a[3]{};
//        for (int& e : nums)
//        {
//            a[e]++;
//        }
//        int idx=0;
//        for (int i = 0; i < nums.size(); i++)
//        {
//            while (!a[idx])idx++;
//            if (a[idx]--)nums[i] = idx;
//        }
//    }
//};