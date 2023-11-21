using namespace std;
#include <iostream>
#include <vector>
#include <string>
#include <numeric>
#include <algorithm>
#include <unordered_set>
#include <unordered_map>
#include <set>
#include <map>
#include <queue>
#include <stack>
#include <functional>
#include <bitset>
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
    int maximumStrongPairXor(vector<int>& nums) {
        int n = nums.size();
        int rst = 0;
        for (int i = 0; i < n; i++) {
            for (int j = i + 1; j < n; j++) {
                if (abs(nums[i]-nums[j])<=min(nums[i], nums[j])) {
                    rst = max(rst, nums[i] ^ nums[j]);
                }
            }
        }
        return rst;
    }
};