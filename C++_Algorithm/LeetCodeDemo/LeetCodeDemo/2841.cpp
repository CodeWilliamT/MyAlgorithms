using namespace std;
#include <vector>
#include <unordered_map>
class Solution {
    typedef long long ll;
public:
    long long maxSum(vector<int>& nums, int m, int k) {
        int n = nums.size();
        ll sum = 0, rst = 0;
        int unique = 0;
        unordered_map<int,int>mp;
        for (int i = 0; i < n; i++) {
            if (!mp[nums[i]])
                unique++;
            mp[nums[i]]++;
            sum += nums[i];
            if (i >= k) {
                mp[nums[i - k]]--;
                sum -= nums[i - k];
                if (!mp[nums[i - k]])
                    unique--;
            }
            if (i >= k - 1&& unique>=m) {
                rst = max(rst, sum);
            }
        }
        return rst;
    }
};