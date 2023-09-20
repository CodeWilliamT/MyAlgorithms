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
    int minLengthAfterRemovals(vector<int>& nums) {
        unordered_map<int, int> mp;
        int mx = 0;
        int n = nums.size();
        for (int& e : nums) {
            mp[e]++;
            mx = max(mx, mp[e]);
        }
        if (mp.size() == 1)return n;
        return mx>n/2?(n-2*(n-mx)):(n%2?1:0);
    }
};