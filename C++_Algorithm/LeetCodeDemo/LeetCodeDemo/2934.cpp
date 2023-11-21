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
//找规律
//如果较大的尾数不是最大数，或者较小的尾数，0到n-2都没有不超过他的，那么-1
//较小尾数分别位于上下，求解操作数，选小操作数。
class Solution {
public:
    int minOperations(vector<int>& nums1, vector<int>& nums2) {
        int n = nums1.size();
        int ft = max(nums1[n - 1], nums2[n - 1]), sd = min(nums1[n - 1], nums2[n - 1]);

        for (int i = 0; i < n; i++) {
            if (nums1[i] > ft|| nums2[i] > ft) {
                return -1;
            }
            if (nums1[i] > sd && nums2[i] > sd) {
                return -1;
            }
        }
        int ans1 = nums1[n-1] != sd;
        int ans2 = nums1[n - 1]== sd;
        for (int i = 0; i < n-1; i++) {
            if (nums1[i] > sd) {
                ans1++;
            }
            if (nums2[i] > sd) {
                ans2++;
            }
        }
        return min(ans1, ans2);
    }
};