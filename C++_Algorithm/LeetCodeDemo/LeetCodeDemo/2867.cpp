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
typedef pair<int, int> pii;
//图 回溯
//从素数点搜点
class Solution {
    vector<bool> get_prime(int n) {
        vector<bool> rst(n+1, true);
        rst[1] = false;
        for (int i = 2; i <= n; i++)
            for (int j = i * 2; j <= n; j += i)
                rst[j] = false;
        return rst;
    }
public:
    long long countPaths(int n, vector<vector<int>>& edges) {
        typedef long long ll;
        typedef pair<ll, ll> pll;
        vector<vector<int> > g(n+1);
        for (auto& e : edges) {//构建邻接表
            g[e[0]].emplace_back(e[1]);
            g[e[1]].emplace_back(e[0]);
        }
        vector<bool> isp=get_prime(n);
        vector<int> d(n, 0);
        ll rst = 0;
        function<pll(int, int)> dfs = [&](int x, int p)->pll{//求解从0点出发的答案,且遍历树
            int w=isp[x];
            ll res0 = 0, res1 = 0;
            if (w)
                ++res1;
            else
                ++res0;
            for (auto y : g[x]) {
                if (y == p) continue;
                auto [a,b]=dfs(y, x); 
                if (w) {
                    rst += res1 * a;
                    res1 += a;
                }
                else {
                    rst += res0 * b + res1 * a;
                    res0 += a;
                    res1 += b;
                }
            }
            return {res0,res1};
        };
        dfs(1, -1);
        return rst;
    }
};