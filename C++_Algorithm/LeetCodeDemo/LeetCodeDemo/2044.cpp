using namespace std;
#include <vector>
#include <unordered_map>
#include <functional>
//哈希 回溯深搜 O(2^n)
class Solution {
public:
    int countMaxOrSubsets(vector<int>& nums) {
        unordered_map<int, int> mp;
        int mx=0;
        function<void(int, int)> dfs = [&](int idx, int num) {
            if (idx >= nums.size()){
                mx = max(mx, num);
                mp[num]++;
                return;
            }
            dfs(idx + 1, num | nums[idx]);
            dfs(idx + 1, num);
        };
        dfs(0,0);
        return mp[mx];
    }
};