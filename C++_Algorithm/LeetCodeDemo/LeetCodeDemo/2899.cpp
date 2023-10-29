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
    vector<int> lastVisitedIntegers(vector<string>& words) {
        vector<int> rst;
        vector<int> tmp;
        int cur = -1;
        for (string& s : words) {
            if (s == "prev") {
                rst.push_back(cur==-1?-1:tmp[cur--]);
            }
            else {
                cur = tmp.size();
                tmp.push_back(stoi(s));
            }
        }
        return rst;
    }
};