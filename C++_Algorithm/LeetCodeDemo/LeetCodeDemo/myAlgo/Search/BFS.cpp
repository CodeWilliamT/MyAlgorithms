using namespace std;
#include "..\myHeader.h"
typedef pair<int, int> pii;
class CommonBFS {
#define MAXN 6000
#define MAXM 2
    struct Node {
        int x;
        int y;
        string path;
        bool operator==(Node const& a) const {
            return a.x == x && a.y == y;
        }
    };
public:
    //通用广搜，返回抵达终点步骤数，不能则返回-1
    bool BFS(Node& start, Node& end)
    {
        queue<Node> q;
        q.push(start);
        bool v[MAXN * MAXM + MAXM + 1]{};
        auto judge = [&](Node& nd) {//处理特殊边界,能下一步则返回true
            return nd.x >= 0 && nd.x <= MAXN&& nd.y>=0&& nd.y<= MAXM;
        };
        auto hash = [&](Node& nd) {//处理特殊边界,能下一步则返回true
            return nd.x * MAXM + nd.y;
        };
        int steps = 0;//步骤数
        int minSteps = -1;//抵达终点的步骤数，不能则-1
        int witdh;
        Node cur, next;
        while (!q.empty()) {
            witdh = q.size();
            while (witdh--) {
                cur = q.front();
                q.pop();
                if (!judge(cur)||v[hash(cur)]]) {//处理边界情况
                    continue;
                }
                v[hash(cur)] = 1;//打标记
                //查看是否抵达终点；
                if (hash(cur)== hash(end)) {
                    minSteps = steps;
                    continue;//抵达终点,看情况break
                }
                //操作成下一步的节点，加入队列
                for (int i = 0; i < 4; i++) {
                    next = { cur.x+i,cur.y,cur.path + to_string(cur.x + i)};
                    if (!judge(next) || v[hash(next)])
                        continue;
                    q.push(next);//加入下一步
                }
            }
            steps++;
        }
        return minSteps > -1;
    }
};

class BitMapBFS {
private:
public:
    //位图广搜，返回抵达终点步骤数，不能则返回-1
    bool BFS(vector<vector<int>>& g,vector<int>& start, vector<int>& end)
    {
        int n = g.size();
        int m = g[0].size();

        int minSteps;//抵达终点的步骤数，不能则-1
        //处理特殊边界,能下一步则返回true
        auto judge = [&](pii& nd) {
            return nd.first > -1 && nd.first <n&& nd.second >-1 && nd.second <m;
        };
        queue<pii> q;
        q.push({ start[0] ,start[1] });
        vector<vector<bool>> v(n, vector<bool>(m, 0));
        int d[4][2] = { {1,0},{0,1},{-1,0},{0,-1} };//方向
        //int d[8][2] = { {1,0},{0,1},{-1,0},{0,-1},{1,1},{1,-1},{-1,1},{-1,-1} };//方向

        int steps = 0;//步骤数
        minSteps = -1;//抵达终点的步骤数，不能则-1
        int witdh;
        pii cur,next;
        while (!q.empty()) {
            witdh = q.size();
            while (witdh--) {
                cur = q.front();
                q.pop();
                if (!judge(cur)||v[cur.first][cur.second]) {
                    continue;//处理边界情况
                }
                v[cur.first][cur.second] = 1;//打标记
                //计算下一点位信息；
                if (cur.first == end[0] && cur.second == end[1]) {
                    minSteps = steps;
                    continue;//抵达终点
                }
                for (int i = 0; i < 4; i++) {
                    next = { cur.first + d[i][0],cur.second + d[i][1] };
                    if (!judge(next) || v[next.first][next.second]) {
                        continue;//处理边界情况
                    }
                    q.push(next);//加入下一步
                }
            }
            steps++;
        }
        return minSteps>-1;
    }
    //多源最广路,返回存各点最小步骤距离的图
    //图，起点集，
    vector<vector<int>> MultiBFS(vector<vector<int>>& g, vector<vector<int>>& starts)
    {
        int n = g.size();
        int m = g[0].size();
        vector<vector<int>> minSteps(n, vector<int>(m, 0));
        //处理特殊边界,能下一步则返回true
        auto judge = [&](pii& nd) {
            return nd.first >-1 && nd.first <n && nd.second >-1 && nd.second <m;
        };
        queue<pii> q;
        for (auto& s : starts)
            q.push({ s[0],s[1] });
        vector<vector<bool>> v(n, vector<bool>(m, 0));
        int d[4][2] = { {1,0},{0,1},{-1,0},{0,-1} };//方向
        int steps = 0;//步骤数
        int witdh;
        pii  cur,next;
        while (!q.empty()) {
            witdh = q.size();
            while (witdh--) {
                cur = q.front();
                q.pop();
                if (!judge(cur)|| v[cur.first][cur.second]) {
                    continue;//处理边界情况
                }
                v[cur.first][cur.second] = 1;//打标记
                //处理当前点位信息
                minSteps[cur.first][cur.second] = steps;
                //计算下一点位信息；
                for (int i = 0; i < 4; i++) {
                    next = { cur.first + d[i][0],cur.second + d[i][1] };
                    if (!judge(next) || v[cur.first][cur.second])
                        continue;
                    q.push(next);//加入下一步
                }
            }
            steps++;
        }
        return minSteps;
    }
};

class EdgeMapBFS {
public:
    //无向图邻接表广搜，返回抵达终点步骤数，不能则返回-1
    //点数目，边集，起点，终点
    int BFS(int n, vector<vector<int>>& edges, int start, int end)
    {
        vector<vector<int>> g;
        vector<int> v(n, 0);
        for (auto& e : edges) {
            g[e[0]].push_back(e[1]);
            g[e[1]].push_back(e[0]);//有向图则注释掉
        }
        queue<int> q;
        q.push(start);
        int witdh;
        int steps=0;//步骤数
        int minSteps =-1;//抵达终点步骤数
        int cur;
        while (!q.empty()) {
            witdh = q.size();
            while (witdh--) {
                cur = q.front();
                q.pop();
                if (v[cur]) {
                    continue;//去重，处理边界情况
                }
                v[cur] = 1;//打标记
                if (cur == end) {//抵达终点
                    minSteps = steps;
                    continue;
                }
                for (auto& e : g[cur]) {
                    if (v[e]) {
                        continue;//处理边界情况
                    }
                    q.push(e);//加入下一步
                }
            }
            steps++;
        }
        return minSteps;
    }
};