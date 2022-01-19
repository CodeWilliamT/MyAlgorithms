using namespace std;
#include <iostream>
#include <vector>
#include <algorithm>
//两分
class Solution {
public:
    int missingNumber(vector<int>& nums) {
        sort(nums.begin(), nums.end());
        int n = nums.size();
        if (nums.back() != n)return n;
        int l=0, r=n-1, mid;
        while (l<r)
        {
            mid = (l + r) / 2;
            if (nums[mid]==mid)
            {
                l = mid+1;
            }
            else
            {
                r = mid;
            }
        }
        return l;
    }
};