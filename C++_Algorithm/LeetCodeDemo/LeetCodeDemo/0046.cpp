using namespace std;
#include <vector>
#include <functional>
//回溯(深搜)
class Solution {
public:
    vector<vector<int>> permute(vector<int>& nums) {
        vector<vector<int>> rst;
        vector<int> tmp;
        int n = nums.size();
        int v[6]{};
        function<void()>dfs = [&]() {
            if (tmp.size() >= n) {
                rst.push_back(tmp);
                return;
            }
            for (int i = 0; i < n; i++)
            {
                if (v[i])continue;
                v[i] = 1;
                tmp.push_back(nums[i]);
                dfs();
                v[i] = 0;
                tmp.pop_back();
            }
        };
        dfs();
        return rst;
    }
};