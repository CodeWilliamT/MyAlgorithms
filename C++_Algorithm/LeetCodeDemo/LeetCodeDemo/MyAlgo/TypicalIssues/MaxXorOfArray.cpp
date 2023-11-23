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
//哈希 异或
//按答案最高位依次异或求出答案。
class Solution {
#define MAXD (int)(20)
public:
    int maximumStrongPairXor(vector<int>& nums) {
        int n = nums.size();
        sort(nums.begin(), nums.end());
        int rst = 0,next, target,y;
        unordered_map<int, int> mp;
        for (int k = MAXD; k > -1; k--) {
            mp.clear();
            rst = rst << 1;
            next = rst + 1;
            for (int& x : nums) {
                target = next ^ (x >> k);
                if (mp.count(target)) {
                    y = mp[target];
                    if (x <= 2 * y) {
                        rst = next;
                        break;
                    }
                }
                mp[x >> k] = x;
            }
        }
        return rst;
    }
};