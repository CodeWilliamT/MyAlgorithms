using namespace std;
#include <vector>
#include <functional>
//深搜 模拟 简单
//图像同色改色
class Solution {
public:
    vector<vector<int>> floodFill(vector<vector<int>>& g, int sr, int sc, int nc) {
        int oc = g[sr][sc];
        int dirs[4][2] = { {1,0},{0,1},{-1,0},{0,-1} };
        bool v[51][51]{};
        function<void(int, int)> dfs = [&](int r, int c) {
            if (r < 0 || r >= g.size() || c < 0 || c >= g[0].size())
                return;
            if (g[r][c] != oc)return;
            if (v[r][c])return;
            v[r][c] = 1;
            g[r][c] = nc;
            for (int i = 0; i < 4; i++) {
                dfs(r + dirs[i][0], c + dirs[i][1]);
            }
        };
        dfs(sr, sc);
        return g;
    }
};