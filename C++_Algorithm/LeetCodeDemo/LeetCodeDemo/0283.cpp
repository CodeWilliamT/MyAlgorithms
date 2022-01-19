using namespace std;
#include <iostream>
#include <vector>
//双指针，快慢指针，基础
class Solution {
public:
    void moveZeroes(vector<int>& nums) {
        int fast=0,slow=0;
        for (int fast = 0; fast <nums.size(); fast++)
        {
            if (nums[fast])nums[slow++]=nums[fast];
        }
        for (int i = slow; i < nums.size(); i++)
        {
            nums[i] = 0;
        }
    }
};