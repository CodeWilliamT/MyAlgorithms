
#include "myHeader.h"

class BFS {
private:
    vector<vector<int>> g;
    struct SNode {
    };
public:
    int Steps;
    //位图广搜
    BFS(vector<vector<int>>& grid,vector<int>& start, vector<int>& end)
    {
        int n = grid.size();
        int m = grid[0].size();
        queue<vector<int>> q;
        vector<vector<int>> v(n, vector<int>(m, 0));
        int d[4][2] = { {1,0},{0,1},{-1,0},{0,-1} };//按需求改

        q.push(start);
        int steps = 0, witdh;
        vector<int> cur;
        while (!q.empty()) {
            witdh = q.size();
            while (witdh--) {
                cur = q.front();
                q.pop();
                if (cur[0] <0 || cur[0] >n - 1 || cur[1] <0 || cur[1] >m - 1 ) {
                    continue;//处理边界情况
                }
                v[cur[0]][cur[1]] = 1;//打标记
                //处理当前点位信息
                //计算下一点位信息；
                if (cur[0] == end[0] && cur[1] == end[1]) {
                    continue;//抵达终点
                }
                for (int i = 0; i < 4; i++) {
                    q.push({ cur[0] + d[i][0],cur[1] + d[i][1],v[cur[0]][cur[1]] });//加入下一步
                }
            }
            steps++;
        }
        Steps = steps;
    }
    //邻接表广搜
    BFS(int n,vector<vector<int>>& edges, int start, int end)
    {
        vector<int> v(n,0);
        for (auto& e : edges) {
            g[e[0]].push_back(e[1]);
            g[e[1]].push_back(e[0]);
        }
        queue<int> q;
        q.push(start);
        int steps = 0, witdh;
        int cur;
        while (!q.empty()) {
            witdh = q.size();
            while (witdh--) {
                cur = q.front();
                q.pop();
                //处理边界情况
                v[cur] = 1;//打标记
                //处理当前点位信息
                //计算下一点位信息；
                //抵达终点
                if (cur == end) {
                    continue;
                }
                for (auto& e : g[cur]) {
                    q.push(e);//加入下一步
                }
            }
            steps++;
        }
        Steps = steps;
    }
};