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
public:
    int minSizeSubarray(vector<int>& nums, int target) {
        int n = nums.size();
        sort(nums.begin(), nums.end());
        auto check = [&](int x) {


            return false;
        };
        int l = 1, r = n;
        int m;
        while (l < r) {
            m = (l + r) / 2;
            if (check(m))
                r = m;
            else
                l = m + 1;
        }
        return r;
    }
};