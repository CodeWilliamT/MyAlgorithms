using namespace std;
#include <vector>
#include <algorithm>
#include <unordered_map>
//哈希
//选k个最大数。
class Solution {
public:
    vector<int> maxSubsequence(vector<int>& nums, int k) {
        vector<int> rst = nums;
        sort(rst.begin(), rst.end());
        unordered_map<int,int> mp;
        for (int i = 0; i < k; i++)
        {
            mp[rst[rst.size() - 1 - i]]++;
        }
        rst.clear();
        for (int i = 0; i < nums.size(); i++)
        {
            if (mp[nums[i]])rst.push_back(nums[i]),mp[nums[i]]--;
        }
        return rst;
    }
};