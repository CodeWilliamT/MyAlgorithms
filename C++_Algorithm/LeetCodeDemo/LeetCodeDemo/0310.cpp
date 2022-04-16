using namespace std;
#include <vector>
#include <functional>
//巧思 深搜
//思考出性质，距离0最远点x；则x与x的最远点y的中间一点(偶数距离)或两点(奇数为根节点。
class Solution {
public:
    vector<int> findMinHeightTrees(int n, vector<vector<int>>& edges) {
        vector<vector<int>> g(n);
        for (auto& e : edges) {
            g[e[0]].push_back(e[1]);
            g[e[1]].push_back(e[0]);
        }
        vector<bool> v(n, 0);
        int mxidx =0,mx=0;
        function<void(int, int, int)> dfs = [&](int from, int idx, int len) {
            if (v[idx]) {
                return;
            }
            v[idx] = 1;
            if (len > mx) {
                mxidx = idx;
                mx = len;
            }
            for (int& e : g[idx]) {
                dfs(from,e, len + 1);
            }
            v[idx] =0;
        };
        dfs(0, 0, 0);
        int x = mxidx;
        dfs(x, x, 0);
        int y = mxidx;
        vector<int> rst;
        dfs = [&](int from, int idx, int len) {
            if (v[idx]) {
                return;
            }
            v[idx] = 1;
            if (len > mx) {
                mxidx = idx;
                mx = len;
            }
            for (int& e : g[idx]) {
                dfs(from, e, len + 1);
                if (v[y]) {
                    goto _end;
                }
            }
        _end:
            if (v[y]) {
                if (len == mx / 2) {
                    rst.push_back(idx);
                }
                if (mx % 2 && len == mx / 2 + 1) {
                    rst.push_back(idx);
                }
                return;
            }
            v[idx] = 0;
        };
        dfs(x, x, 0);
        return rst;
    }
};