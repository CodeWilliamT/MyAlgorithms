using namespace std;
#include <iostream>
#include <vector>
#include <string>
#include <algorithm>
#include <unordered_set>
#include <unordered_map>
#include <set>
#include <map>
#include <queue>
#include <stack>
#include <functional>
#include <bitset>
//简单模拟
class Solution {
public:
    int findClosestNumber(vector<int>& nums) {
        int rst = nums[0];
        for (int& e : nums) {
            if (abs(e) < abs(rst)) {
                rst = e;
            }
            else if (abs(e) == abs(rst) && rst < 0) {
                rst = e;
            }
        }
        return rst;
    }
};