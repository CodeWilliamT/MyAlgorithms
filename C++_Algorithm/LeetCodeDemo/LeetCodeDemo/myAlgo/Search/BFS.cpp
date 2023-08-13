using namespace std;
#include "myHeader.h"
typedef pair<int, int> pii;

class BitMapBFS {
private:
    vector<vector<int>> g;
public:
    int minSteps;//抵达终点的步骤数，不能则-1
    //处理特殊边界,能下一步则返回true
    bool Judge(pii& cur) {
        return true;
    }
    //位图广搜，返回抵达终点步骤数，不能则返回-1
    bool BFS(vector<vector<int>>& grid,vector<int>& start, vector<int>& end)
    {
        int n = grid.size();
        int m = grid[0].size();
        g = grid;
        queue<pii> q;
        vector<vector<bool>> v(n, vector<bool>(m, 0));
        int d[4][2] = { {1,0},{0,1},{-1,0},{0,-1} };//按需求改

        q.push({ start[0] ,start[1]});
        int steps = 0;//步骤数
        minSteps = -1;//抵达终点的步骤数，不能则-1
        int witdh;
        pii cur;
        while (!q.empty()) {
            witdh = q.size();
            while (witdh--) {
                cur = q.front();
                q.pop();
                if (cur.first <0 || cur.first >n - 1 || cur.second <0 || cur.second >m - 1 
                    ||v[cur.first][cur.second]
                    || !Judge(cur)) {
                    continue;//处理边界情况
                }
                v[cur.first][cur.second] = 1;//打标记
                //处理当前点位信息
                //计算下一点位信息；
                if (cur.first == end[0] && cur.second == end[1]) {
                    minSteps = steps;
                    continue;//抵达终点
                }
                for (int i = 0; i < 4; i++) {
                    q.push({ cur.first + d[i][0],cur.second + d[i][1]});//加入下一步
                }
            }
            steps++;
        }
        return minSteps>-1;
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
        for(auto&s:starts)
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
                    || v[cur.first][cur.second]
                    || !Judge(cur)) {
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
class EdgeMapBFS {
private:
    vector<vector<int>> g;
public:
    //处理特殊边界,能下一步则返回true
    bool Judge(int cur) {
        return true;
    }
    //邻接表广搜，返回抵达终点步骤数，不能则返回-1
    int BFS(int n, vector<vector<int>>& edges, int start, int end)
    {
        vector<int> v(n, 0);
        for (auto& e : edges) {
            g[e[0]].push_back(e[1]);
            g[e[1]].push_back(e[0]);
        }
        queue<int> q;
        q.push(start);
        int witdh;
        int steps=0;//步骤数
        int reachSteps=-1;//抵达终点步骤数
        int cur;
        while (!q.empty()) {
            witdh = q.size();
            while (witdh--) {
                cur = q.front();
                q.pop();
                if (v[cur]||!Judge(cur)) {
                    continue;//处理边界情况
                }
                v[cur] = 1;//打标记
                //处理当前点位信息
                //计算下一点位信息；
                //抵达终点
                if (cur == end) {
                    reachSteps = steps;
                    continue;
                }
                for (auto& e : g[cur]) {
                    q.push(e);//加入下一步
                }
            }
            steps++;
        }
        return reachSteps;
    }
};