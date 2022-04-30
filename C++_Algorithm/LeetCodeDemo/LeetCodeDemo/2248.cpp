using namespace std;
#include <vector>
//哈希
class Solution {
public:
    vector<int> intersection(vector<vector<int>>& nums) {
        unsigned short mp[1001]{};
        int mx=0;
        for (auto& e : nums) {
            for (int& x : e) {
                mp[x]++;
                mx = max(mx, x);
            }
        }
        vector<int> rst;
        for (int i = 1; i <= mx;i++) {
            if (mp[i] == nums.size()) {
                rst.push_back(i);
            }
        }
        return rst;
    }
};