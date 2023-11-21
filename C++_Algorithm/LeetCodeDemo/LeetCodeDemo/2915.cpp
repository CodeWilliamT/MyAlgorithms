using namespace std;
#include <iostream>
#include <vector>
#include <algorithm>
#include <unordered_map>
typedef pair<int, bool> pib;
typedef pair<int, int> pii;
typedef long long ll;
typedef pair<ll, ll> pll;
typedef pair<ll, int> pli;
#define MAXN (int)(1e5+1)
#define MAXM (int)(1e5+1)
#define MOD (int)(1e9+7)
class Solution {
public:
    int lengthOfLongestSubsequence(vector<int>& nums, int target) {
        sort(nums.begin(), nums.end());
        int n = nums.size();
        int rst = -1;
        int v;
        unordered_map<int,int> mp,mpnext;
        mp[0] = 0;
        for (int i = 0; i < n; i++) {
            for (auto& [x, y] : mp)
                mpnext[x] = y;
            for (auto& [x,y] : mp) {
                v = x + nums[i];
                if (v > target)
                    continue;
                mpnext[v] = max(mpnext[v], y + 1);
                if (v == target) {
                    rst = max(rst, mpnext[v]);
                }
            }
            for (auto& [x, y] : mpnext)
                mp[x] = y;
        }
        return rst;
    }
};