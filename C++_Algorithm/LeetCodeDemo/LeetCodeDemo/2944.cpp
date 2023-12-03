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
#define MAXM (int)(1e5+1)
#define MOD (int)(1e9+7)
//dp 贪心
//0 1;1,3;2,5;3,7;
class Solution {
#define MAXN (int)(1001)
public:
    int minimumCoins(vector<int>& p) {
        int n = p.size();
        int f[MAXN]{};
        f[0] = p[0];
        f[1] = p[0];
        for (int i = 2; i < n; i++) {
            f[i] = INT32_MAX;
            for (int j = i; j>=i/2&&j>=1; j--) {
                f[i] = min(f[i], f[j-1]+p[j]);
            }
        }
        return f[n-1];
    }
};