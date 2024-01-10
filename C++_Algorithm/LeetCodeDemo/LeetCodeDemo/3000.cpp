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
    int areaOfMaxDiagonal(vector<vector<int>>& ds) {
        int s = 0, l = 0;
        int tmpl, tmps;
        for (auto& d:ds) {
            tmpl = d[0] * d[0] + d[1] * d[1];
            tmps = d[0] * d[1];
            if (tmpl > l) {
                l = tmpl; s = tmps;
            }
            else if (tmpl == l) {
                s = max(tmps, s);
            }
        }
        return s;
    }
};