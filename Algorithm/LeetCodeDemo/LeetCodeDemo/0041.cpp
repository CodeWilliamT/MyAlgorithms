using namespace std;
#include <iostream>
#include <vector>
//巧思
//遍历，将符合0<x<=n的x都甩到x-1的位置上，然后从小到大遍历，nums[i]!=i+1的就是答案i+1。
class Solution {
public:
    int firstMissingPositive(vector<int>& nums) {
        int n = nums.size();
        for (int i = 0; i < n; ++i) {
            //连甩
            while (nums[i] > 0 && nums[i] <= n && nums[nums[i] - 1] != nums[i]) {
                swap(nums[nums[i] - 1], nums[i]);
            }
        }
        for (int i = 0; i < n; ++i) {
            if (nums[i] != i + 1) {
                return i + 1;
            }
        }
        return n + 1;
    }
};