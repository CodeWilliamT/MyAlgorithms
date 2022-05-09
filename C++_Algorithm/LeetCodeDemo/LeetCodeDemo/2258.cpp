using namespace std;
#include <vector>
#include <queue>
#include <functional>
//人左上角出发到右下角，人火各1步，人先手，墙挡一切，求最迟出发时间。
//广搜+深搜
//广搜火跟人
//算火，记录每个非墙格子第几步被烧。
//算人，记录每个非墙格子人第几步走到。
//深搜
//看看起点到终点是否存在一条路径上所有格子有人到的步骤数小于等于火，记录每条路径的最小的差值，最小差值最大的就是答案。
class Solution {
    typedef pair<int, int> pii;
    typedef pair<vector<pair<int, int>>, vector<pair<int, int>> > pvv;
    int safe = 1e9;
    int unsafe = -1;
public:
    int maximumMinutes(vector<vector<int>>& g) {
        int m = g.size();
        int n = g[0].size();
        vector<vector<int>> gf(m, vector<int>(n, safe));
        vector<vector<int>> gp(m, vector<int>(n, safe+1));
        queue<pvv> q;
        pvv cur;
        gp[0][0] = 0;
        cur.first.push_back({ 0,0 });
        for (int i = 0; i < m; i++) {
            for (int j = 0; j < n; j++) {
                if (g[i][j] == 1) {
                    gf[i][j] = 0;
                    cur.second.push_back({ i,j });
                }
            }
        }
        q.push(cur);
        int mov[4][2] = { {1,0},{0,1},{-1,0},{0,-1} };
        int x, y;
        int steps = 0;
        int len;
        bool alldone;
        while (!q.empty()) {
            steps++;
            len = q.size();
            alldone = true;
            for (int i = 0; i < len; i++) {
                cur = q.front();
                q.pop();
                pvv next;
                for (auto& e : cur.first) {
                    if (g[e.first][e.second])
                        continue;
                    for (int t = 0; t < 4; t++) {
                        x = e.first + mov[t][0];
                        y = e.second + mov[t][1];
                        //这个格子周围有一个 现在没有火并且人没走过的格子。
                        if (x >= 0 && x < m && y >= 0 && y < n && !g[x][y] && gp[x][y] == safe + 1) {
                            alldone = false;
                            gp[x][y] = steps;
                            next.first.push_back({ x,y });
                        }
                    }
                }
                for (auto& e : cur.second) {
                    for (int t = 0; t < 4; t++) {
                        x = e.first + mov[t][0];
                        y = e.second + mov[t][1];
                        //这个格子周围有一个 现在没有火的格子。
                        if (x >= 0 && x < m && y >= 0 && y < n && !g[x][y]) {
                            alldone = false;
                            g[x][y] = 1;
                            gf[x][y] = steps;
                            next.second.push_back({ x,y });
                        }
                    }
                }
                if(!alldone)
                    q.push(next);
            }
        }
        if (gp[m - 1][n - 1] == safe+1)
            return -1;
        if (gf[m - 1][n - 1] == safe)
            return safe;
        int rst = 0;
        vector<vector<bool>> v(m, vector<bool>(n, 0));
        function<void(int, int, int)> dfs = [&](int curx, int cury, int diff) {
            if (curx == m - 1 && cury == n - 1) {
                rst=max(rst, min(diff, gf[curx][cury] - gp[curx][cury]));
                return;
            }
            if (v[curx][cury]) {
                return;
            }
            v[curx][cury] = 1;
            for (int t = 0; t < 4; t++) {
                x = curx + mov[t][0];
                y = cury + mov[t][1];
                //这个格子周围有一个 现在没有火并且k-1时火走过的格子。
                if (x >= 0 && x < m && y >= 0 && y < n &&gp[x][y]<=safe) {
                    dfs(x, y, min(diff, gf[curx][cury] - gp[curx][cury]-1));
                }
            }
        };
        dfs(0, 0, safe);
        return rst;
    }
};