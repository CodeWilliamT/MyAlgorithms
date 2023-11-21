#include "myAlgo\LCParse\TreeNode.cpp"
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
    int minChanges(string s) {
        int n = s.size();
        int rst = 0;
        int cnt = 1;
        for (int i = 1; i < n; i++) {
            if (s[i] == s[i - 1]) {
                cnt++;
            }
            else {
                if (cnt % 2) {
                    rst++;
                    cnt = 1;
                }
                else {
                    cnt = 0;
                }
                cnt++;
            }
        }
        return rst;
    }
};