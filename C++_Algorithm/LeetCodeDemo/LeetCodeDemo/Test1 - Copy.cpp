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
typedef long long ll;
typedef pair<ll, ll> pll;
typedef pair<int, int> pii;
//朴素实现
class Solution {
public:
    int countPairs(vector<int>& nums, int target) {
        int n = nums.size();
        int rst = 0;
        for (int i = 0; i < n; i++) {
            for (int j = i + 1; j < n; j++) {
                rst += (nums[i] + nums[j] < target);
            }
        }
        return rst;
    }
};