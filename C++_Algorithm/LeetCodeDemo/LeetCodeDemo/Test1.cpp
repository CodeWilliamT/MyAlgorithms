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
class Solution {
    int cnt1(int x) {
        int cnt = 0;
        while (x) {
            cnt += (x & 1);
            x = x >> 1;
        }
        return cnt;
    }
public:
    int sumIndicesWithKSetBits(vector<int>& nums, int k) {
        int rst=0;
        for (int i = 0; i < nums.size();i++) {
            if (cnt1(i) == k) {
                rst += nums[i];
            }
        }
        return rst;
    }
};