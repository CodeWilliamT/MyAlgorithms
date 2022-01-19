using namespace std;
#include <iostream>
#include <vector>
//双指针、快慢指针
class Solution {
public:
    int removeDuplicates(vector<int>& nums) {
        if (!nums.size())return 0;
        int fast = 1, slow = 1;
        for (; fast < nums.size(); fast++)
        {
            if (nums[fast] != nums[fast - 1])
            {
                nums[slow++] = nums[fast];
            }
        }
        return slow;
    }
};