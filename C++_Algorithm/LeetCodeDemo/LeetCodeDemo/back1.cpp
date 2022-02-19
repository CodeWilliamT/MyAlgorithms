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
class Solution {
public:
    int countPairs(vector<int>& nums, int k) {
        int n = nums.size(),rst=0;
        for (int i = 0; i < n; i++) {
            for (int j = i+1; j < n; j++) {
                if (nums[i] == nums[j] && i * j % k == 0) {
                    rst++;
                }
            }
        }
        return rst;
    }
};