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
#include "myAlgo\Structs\TreeNode.cpp"
#define MAXN (int)1e5+1
#define MAXM (int)1e5+1
typedef pair<int, bool> pib;
typedef long long ll;
typedef pair<ll, ll> pll;
typedef pair<int, int> pii;
//模拟
class Solution {
public:
    int minProcessingTime(vector<int>& p, vector<int>& t) {
        sort(p.begin(), p.end());
        sort(t.begin(), t.end());
        int n = p.size();
        int rst = 0;
        for (int i = 0; i < n; i++) {
            for (int j = 4 * i; j < 4 *i+4; j++) {
                rst = max(rst,p[i] + t[4*n-1-j]);
            }
        }
        return rst;
    }
};