using namespace std;
#include <vector>
#include <algorithm>
//动态规划 两分查找
//记录各个屋子与各个供暖器的最短距离，最大值即答案
class Solution {
public:
    int findRadius(vector<int>& houses, vector<int>& heaters) {
        int n = houses.size();
        int tmp, rst=0,idx;
        sort(heaters.begin(), heaters.end());
        for (int i = 0; i < n; i++) {
            idx = lower_bound(heaters.begin(), heaters.end(),houses[i])- heaters.begin();
            tmp = abs(houses[i] - heaters[0]);
            if(idx>0)
                tmp = abs(houses[i] - heaters[idx - 1]);
            if(idx<heaters.size())
                tmp = min(tmp, abs(houses[i] - heaters[idx]));
            rst = max(rst, tmp);
        }
        return rst;
    }
};