using namespace std;
#include <vector>
//哈希 简单题
//加之前出现过的数量
class Solution {
public:
    int countKDifference(vector<int>& nums, int k) {
        int v[101]{};
        int rst = 0;
        for (int& e : nums) {
            if (e - k < 101 && e - k >= 0)rst += v[e - k];
            if (e + k < 101 && e + k >= 0)rst += v[e + k];
            v[e]++;
        }
        return rst;
    }
};