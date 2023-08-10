using namespace std;
#include <iostream>
#include <vector>
#include <queue>
//广搜 两分
typedef pair<int, int> pii;
class BitMapBFS {
private:
    vector<vector<int>> g;
public:
    //处理特殊边界
    bool Judge(pii& cur, int x) {
        return g[cur.first][cur.second] >= x;
    }
    //位图广搜，返回抵达终点步骤数，不能则返回-1
    int BFS(vector<vector<int>>& grid, vector<int>& start, vector<int>& end, int x)
    {
        int n = grid.size();
        int m = grid[0].size();
        g = grid;
        queue<pii> q;
        vector<vector<bool>> v(n, vector<bool>(m, 0));
        int d[4][2] = { {1,0},{0,1},{-1,0},{0,-1} };//按需求改

        q.push({ start[0] ,start[1] });
        int steps = 0;//步骤数
        int reachSteps = -1;//抵达终点的步骤数，不能则-1
        int witdh;
        pii cur;
        while (!q.empty()) {
            witdh = q.size();
            while (witdh--) {
                cur = q.front();
                q.pop();
                if (cur.first <0 || cur.first >n - 1 || cur.second <0 || cur.second >m - 1
                    || v[cur.first][cur.second]
                    || !Judge(cur, x)) {
                    continue;//处理边界情况
                }
                v[cur.first][cur.second] = 1;//打标记
                //处理当前点位信息
                //计算下一点位信息；
                if (cur.first == end[0] && cur.second == end[1]) {
                    reachSteps = steps;
                    continue;//抵达终点
                }
                for (int i = 0; i < 4; i++) {
                    q.push({ cur.first + d[i][0],cur.second + d[i][1] });//加入下一步
                }
            }
            steps++;
        }
        return reachSteps > -1;
    }

    //多源最广路,返回存各点最小步骤距离的图
    //图，起点集，
    vector<vector<int>> MultiBFS(vector<vector<int>>& grid, vector<vector<int>>& starts)
    {
        int n = grid.size();
        int m = grid[0].size();
        vector<vector<int>> minSteps(n, vector<int>(m, 0));
        g = grid;
        queue<pii> q;
        vector<vector<bool>> v(n, vector<bool>(m, 0));
        int d[4][2] = { {1,0},{0,1},{-1,0},{0,-1} };//按需求改
        for (auto& s : starts)
            q.push({ s[0],s[1] });
        int steps = 0;//步骤数
        int witdh;
        pii  cur;
        while (!q.empty()) {
            witdh = q.size();
            while (witdh--) {
                cur = q.front();
                q.pop();
                if (cur.first <0 || cur.first >n - 1 || cur.second <0 || cur.second >m - 1
                    || v[cur.first][cur.second]) {
                    continue;//处理边界情况
                }
                v[cur.first][cur.second] = 1;//打标记
                //处理当前点位信息
                minSteps[cur.first][cur.second] = steps;
                //计算下一点位信息；
                for (int i = 0; i < 4; i++) {
                    q.push({ cur.first + d[i][0],cur.second + d[i][1] });//加入下一步
                }
            }
            steps++;
        }
        return minSteps;
    }
};
class BinaryCheck {
private:
    vector<int> nums;
    vector<vector<int>> g;
    int N, M;
public:
    bool check(int x) {
        BitMapBFS bfs;
        vector<int> s = { 0,0 }, e = { N - 1,M - 1 };
        return bfs.BFS(g, s, e, x);
    }
    //位图中两分查找
    int GetTEdge(vector<vector<int>> grid, int l, int r) {
        g = grid;
        N = g.size();
        M = g[0].size();
        int m;
        while (l < r) {
            m = (l + r + 1) / 2;
            if (check(m))
                l = m;
            else
                r = m - 1;
        }
        return l;
    }
};


class Solution {
public:
    int maximumSafenessFactor(vector<vector<int>>& g) {
        int n = g.size();
        vector<vector<int>> f;
        vector<vector<int>> thiefs;
        for (int i = 0; i < n; i++) {
            for (int j = 0; j < n; j++) {
                if (g[i][j]) {
                    thiefs.push_back({ i,j });
                }
            }
        }
        BitMapBFS bfs;
        f = bfs.MultiBFS(g, thiefs);
        BinaryCheck b;
        return b.GetTEdge(f, 0, min(f[0][0], f[n - 1][n - 1]));
    }
};