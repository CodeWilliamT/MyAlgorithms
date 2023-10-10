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
#define MAXN (int)(1e5+1)
#define MAXM (int)(1e5+1)
#define MOD (int)(1e9+7)
//分析 巧思
//撞了==没撞
class Solution {
public:
    int sumDistance(vector<int>& a, string s, int d) {
        int n = a.size();
        for (int i = 0; i < n; i++) {
            a[i] += (s[i] == 'R' ? d : -d);
        }
        int rst = 0;
        sort(a.begin(), a.end());
        for (int i = 1; i < n; i++) {
            ll l = ((ll)a[i] - a[i - 1]) % MOD;
            rst = ((l*(n - i) * i) % MOD+rst) % MOD;
        }
        return rst;
    }
};