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
typedef long long ll;
typedef pair<ll, ll> pll;
typedef pair<int, int> pii;
class Solution {
public:
    int minimumRightShifts(vector<int>& nums) {
        int n = nums.size();
        int mnidx=0;
        for (int i = 0; i < n; i++) {
            if (nums[mnidx] > nums[i]) {
                mnidx = i;
            }
        }
        for (int i = 1; i < n; i++) {
            if (nums[(i - 1 + mnidx)%n] >= nums[(i + mnidx) % n]) {
                return -1;
            }
        }
        return (n-mnidx)%n;
    }
};