using namespace std;
#include "myHeader.h"
typedef pair<int, int> pii;

class BitMapBFS {
private:
    vector<vector<int>> g;
public:
    int minSteps;//�ִ��յ�Ĳ�������������-1
    //��������߽�,����һ���򷵻�true
    bool Judge(pii& cur) {
        return true;
    }
    //λͼ���ѣ����صִ��յ㲽�����������򷵻�-1
    bool BFS(vector<vector<int>>& grid,vector<int>& start, vector<int>& end)
    {
        int n = grid.size();
        int m = grid[0].size();
        g = grid;
        queue<pii> q;
        vector<vector<bool>> v(n, vector<bool>(m, 0));
        int d[4][2] = { {1,0},{0,1},{-1,0},{0,-1} };//�������

        q.push({ start[0] ,start[1]});
        int steps = 0;//������
        minSteps = -1;//�ִ��յ�Ĳ�������������-1
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
                    continue;//����߽����
                }
                v[cur.first][cur.second] = 1;//����
                //����ǰ��λ��Ϣ
                //������һ��λ��Ϣ��
                if (cur.first == end[0] && cur.second == end[1]) {
                    minSteps = steps;
                    continue;//�ִ��յ�
                }
                for (int i = 0; i < 4; i++) {
                    q.push({ cur.first + d[i][0],cur.second + d[i][1]});//������һ��
                }
            }
            steps++;
        }
        return minSteps>-1;
    }
    //��Դ���·,���ش������С��������ͼ
    //ͼ����㼯��
    vector<vector<int>> MultiBFS(vector<vector<int>>& grid, vector<vector<int>>& starts)
    {
        int n = grid.size();
        int m = grid[0].size();
        vector<vector<int>> minSteps(n, vector<int>(m, 0));
        g = grid;
        queue<pii> q;
        vector<vector<bool>> v(n, vector<bool>(m, 0));
        int d[4][2] = { {1,0},{0,1},{-1,0},{0,-1} };//�������
        for(auto&s:starts)
            q.push({ s[0],s[1] });
        int steps = 0;//������
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
                    continue;//����߽����
                }
                v[cur.first][cur.second] = 1;//����
                //����ǰ��λ��Ϣ
                minSteps[cur.first][cur.second] = steps;
                //������һ��λ��Ϣ��
                for (int i = 0; i < 4; i++) {
                    q.push({ cur.first + d[i][0],cur.second + d[i][1] });//������һ��
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
    //��������߽�,����һ���򷵻�true
    bool Judge(int cur) {
        return true;
    }
    //�ڽӱ���ѣ����صִ��յ㲽�����������򷵻�-1
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
        int steps=0;//������
        int reachSteps=-1;//�ִ��յ㲽����
        int cur;
        while (!q.empty()) {
            witdh = q.size();
            while (witdh--) {
                cur = q.front();
                q.pop();
                if (v[cur]||!Judge(cur)) {
                    continue;//����߽����
                }
                v[cur] = 1;//����
                //����ǰ��λ��Ϣ
                //������һ��λ��Ϣ��
                //�ִ��յ�
                if (cur == end) {
                    reachSteps = steps;
                    continue;
                }
                for (auto& e : g[cur]) {
                    q.push(e);//������һ��
                }
            }
            steps++;
        }
        return reachSteps;
    }
};