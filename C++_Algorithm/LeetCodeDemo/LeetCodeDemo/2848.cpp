using namespace std;
#include <vector>
#include <algorithm>
class Solution {
public:
    int numberOfPoints(vector<vector<int>>& nums) {
        sort(nums.begin(), nums.end());
        int rst = 0;
        int end=0;
        for (auto&e:nums) {
            if (e[0] > end)
                rst += e[1] - e[0]+1;
            else if (e[1] > end) {
                rst += e[1] - end;
            }
            end = max(end, e[1]);
        }
        return rst;
    }
};