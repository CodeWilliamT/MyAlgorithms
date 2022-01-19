using namespace std;
#include <iostream>
#include <vector>
//找规律
//比较从前删 max(minIdx , maxIdx) + 1，从后删min(n - min(minIdx, maxIdx)，前后各删一个的删除数n - abs(maxIdx-minIdx) + 1)，最小的那个。
class Solution {
public:
    int minimumDeletions(vector<int>& nums) {
        int minIdx=0, maxIdx=0;
        int n = nums.size();
        for (int i = 1; i < n; i++)
        {
            if (nums[i] > nums[maxIdx])maxIdx = i;
            if (nums[i] < nums[minIdx])minIdx = i;
        }
        return min(max(minIdx , maxIdx) + 1, min(n - min(minIdx, maxIdx), n - abs(maxIdx-minIdx) + 1));
    }
};