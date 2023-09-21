using namespace std;
#include <iostream>
#include <vector>
#include <string>
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
typedef pair<int, bool> pib;
typedef pair<ll, ll> pll;
typedef pair<int, int> pii;
//复杂模拟 两分
//
class Solution {
    typedef long long ll;
    //左true右false，两分查找使check为true的最大值
public:
    int maxNumberOfAlloys(int n, int k, int budget, vector<vector<int>>& comp, vector<int>& st, vector<int>& cost) {

        auto check = [&](ll x,int y) {
            ll used = 0;
            for (int i = 0; i < n; i++) {
                used += (x * comp[y][i] > st[i])?(x * comp[y][i]- st[i]) * cost[i]:0;
                if (used > budget)
                    return false;
            }
            return  used <= (ll)1*budget;
            };

        auto GetTFEdge = [&](int y) {
            int l = 0, r = 2e8;
            int m;
            while (l < r) {
                m = (l + r + 1) / 2;
                if (check(m,y))
                    l = m;
                else
                    r = m - 1;
            }
            return l;
            };
        int rst = 0;
        for (int i = 0; i < k; i++) {
            rst = max(rst, GetTFEdge(i));
        }
        return rst;
    }
};