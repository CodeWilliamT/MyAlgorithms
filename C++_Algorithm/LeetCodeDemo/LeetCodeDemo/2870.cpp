using namespace std;
#include <vector>
#include <unordered_map>
class Solution {
public:
    int minOperations(vector<int>& nums) {
        unordered_map<int, int>mp;
        for (int& e : nums) {
            mp[e]++;
        }
        int rst = 0;
        for (auto& [x, y] : mp) {
            if (y == 1)return -1;
            rst += y / 3;
            if (y % 3) {
                rst++;
            }
        }
        return rst;
    }
};