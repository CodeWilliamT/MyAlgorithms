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
#include "myAlgo\Structs\TreeNode.cpp"
typedef pair<int, bool> pib;
typedef long long ll;
typedef pair<ll, ll> pll;
typedef pair<int, int> pii;
//
// 选尽量小的数
class Solution {
public:
    int countWays(vector<int>& nums) {
        sort(nums.begin(), nums.end());
        int n = nums.size();
        int rst = nums[0]>0;
        for (int i = 0; i < n; i++) {
            if (nums[i] < i + 1&&(i==n-1||i<n-1&&nums[i+1]>i+1)) {
                rst++;
            }
        }
        return rst;
    }
};