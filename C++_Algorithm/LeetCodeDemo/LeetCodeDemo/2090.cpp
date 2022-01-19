using namespace std;
#include <iostream>
#include <vector>
//找规律 朴素实现
//前缀和
class Solution {
public:
    vector<int> getAverages(vector<int>& nums, int k) {
        int n = nums.size();
        vector<int> rst;
        long long sum = 0;
        for (int i = 0; i < n && i < 2 * k + 1; i++)
        {
            sum += nums[i];
        }
        for (int i = 0; i < n; i++)
        {
            if (i < k || i>n - 1 - k) {
                rst.push_back(-1);
                continue;
            }
            rst.push_back(sum / (2 * k + 1));
            sum -= nums[i - k];
            if (i + k + 1 < n)sum += nums[i + k + 1];
        }
        return rst;
    }
};