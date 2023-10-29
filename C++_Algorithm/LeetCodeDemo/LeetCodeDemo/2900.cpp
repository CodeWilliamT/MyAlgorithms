#include "myAlgo\Structs\TreeNode.cpp"
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
    vector<string> getWordsInLongestSubsequence(int n, vector<string>& w, vector<int>& g) {
        vector<string> rst;
        vector<int> tmp0,tmp1;
        for (int i = 0; i < n; i++) {
            if (g[i]) {
                if (tmp1.empty() || !g[tmp1.back()]) {
                    tmp1.push_back(i);
                }
                if (!tmp0.empty()&&!g[tmp0.back()]) {
                    tmp0.push_back(i);
                }
            }
            else{
                if (tmp0.empty() || g[tmp0.back()]) {
                    tmp0.push_back(i);
                }
                if (!tmp1.empty() && g[tmp1.back()]) {
                    tmp1.push_back(i);
                }
            }
        }
        if (tmp1.size() > tmp0.size())
            tmp0 = tmp1;
        for (int& e : tmp0) {
            rst.push_back(w[e]);
        }
        return rst;
    }
};