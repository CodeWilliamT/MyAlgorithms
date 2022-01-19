using namespace std;
#include <vector>
#include <algorithm>
//双指针 利用好单调性 
class Solution {
public:
    vector<vector<int>> threeSum(vector<int>& nums) {
        sort(nums.begin(), nums.end());
        int n= nums.size();
        int val;
        vector<vector<int>> rst;
        int k;
        for (int i = 0; i < n; i++) {
            if (i>0&&nums[i-1]== nums[i])continue;
            k = n - 1;
            for (int j = i + 1; j < n; j++) {
                if (j>i+1&&nums[j-1] == nums[j])continue;
                val = -(nums[i] + nums[j]);
                if (val < nums[j])break;
                while(nums[k] > val)k--;
                if(nums[k]==val&&k>j)
                    rst.push_back({ nums[i],nums[j],val });
            }
        }
        return rst;
    }
};