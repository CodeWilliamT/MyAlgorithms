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
    //ͨ�ù��ѣ����صִ��յ㲽�����������򷵻�-1
    bool BFS(Node& start, Node& end)
    {
        queue<Node> q;
        q.push(start);
        bool v[MAXN * MAXM + MAXM + 1]{};
        auto judge = [&](Node& nd) {//��������߽�,����һ���򷵻�true
            return nd.x >= 0 && nd.x <= MAXN&& nd.y>=0&& nd.y<= MAXM;
        };
        auto hash = [&](Node& nd) {//��������߽�,����һ���򷵻�true
            return nd.x * MAXM + nd.y;
        };
        int steps = 0;//������
        int minSteps = -1;//�ִ��յ�Ĳ�������������-1
        int witdh;
        Node cur, next;
        while (!q.empty()) {
            witdh = q.size();
            while (witdh--) {
                cur = q.front();
                q.pop();
                if (!judge(cur)||v[hash(cur)]]) {//����߽����
                    continue;
                }
                v[hash(cur)] = 1;//����
                //�鿴�Ƿ�ִ��յ㣻
                if (hash(cur)== hash(end)) {
                    minSteps = steps;
                    continue;//�ִ��յ�,�����break
                }
                //��������һ���Ľڵ㣬�������
                for (int i = 0; i < 4; i++) {
                    next = { cur.x+i,cur.y,cur.path + to_string(cur.x + i)};
                    if (!judge(next) || v[hash(next)])
                        continue;
                    q.push(next);//������һ��
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
    //λͼ���ѣ����صִ��յ㲽�����������򷵻�-1
    bool BFS(vector<vector<int>>& g,vector<int>& start, vector<int>& end)
    {
        int n = g.size();
        int m = g[0].size();

        int minSteps;//�ִ��յ�Ĳ�������������-1
        //��������߽�,����һ���򷵻�true
        auto judge = [&](pii& nd) {
            return nd.first > -1 && nd.first <n&& nd.second >-1 && nd.second <m;
        };
        queue<pii> q;
        q.push({ start[0] ,start[1] });
        vector<vector<bool>> v(n, vector<bool>(m, 0));
        int d[4][2] = { {1,0},{0,1},{-1,0},{0,-1} };//����
        //int d[8][2] = { {1,0},{0,1},{-1,0},{0,-1},{1,1},{1,-1},{-1,1},{-1,-1} };//����

        int steps = 0;//������
        minSteps = -1;//�ִ��յ�Ĳ�������������-1
        int witdh;
        pii cur,next;
        while (!q.empty()) {
            witdh = q.size();
            while (witdh--) {
                cur = q.front();
                q.pop();
                if (!judge(cur)||v[cur.first][cur.second]) {
                    continue;//����߽����
                }
                v[cur.first][cur.second] = 1;//����
                //������һ��λ��Ϣ��
                if (cur.first == end[0] && cur.second == end[1]) {
                    minSteps = steps;
                    continue;//�ִ��յ�
                }
                for (int i = 0; i < 4; i++) {
                    next = { cur.first + d[i][0],cur.second + d[i][1] };
                    if (!judge(next) || v[next.first][next.second]) {
                        continue;//����߽����
                    }
                    q.push(next);//������һ��
                }
            }
            steps++;
        }
        return minSteps>-1;
    }
    //��Դ���·,���ش������С��������ͼ
    //ͼ����㼯��
    vector<vector<int>> MultiBFS(vector<vector<int>>& g, vector<vector<int>>& starts)
    {
        int n = g.size();
        int m = g[0].size();
        vector<vector<int>> minSteps(n, vector<int>(m, 0));
        //��������߽�,����һ���򷵻�true
        auto judge = [&](pii& nd) {
            return nd.first >-1 && nd.first <n && nd.second >-1 && nd.second <m;
        };
        queue<pii> q;
        for (auto& s : starts)
            q.push({ s[0],s[1] });
        vector<vector<bool>> v(n, vector<bool>(m, 0));
        int d[4][2] = { {1,0},{0,1},{-1,0},{0,-1} };//����
        int steps = 0;//������
        int witdh;
        pii  cur,next;
        while (!q.empty()) {
            witdh = q.size();
            while (witdh--) {
                cur = q.front();
                q.pop();
                if (!judge(cur)|| v[cur.first][cur.second]) {
                    continue;//����߽����
                }
                v[cur.first][cur.second] = 1;//����
                //����ǰ��λ��Ϣ
                minSteps[cur.first][cur.second] = steps;
                //������һ��λ��Ϣ��
                for (int i = 0; i < 4; i++) {
                    next = { cur.first + d[i][0],cur.second + d[i][1] };
                    if (!judge(next) || v[cur.first][cur.second])
                        continue;
                    q.push(next);//������һ��
                }
            }
            steps++;
        }
        return minSteps;
    }
};

class EdgeMapBFS {
public:
    //����ͼ�ڽӱ���ѣ����صִ��յ㲽�����������򷵻�-1
    //����Ŀ���߼�����㣬�յ�
    int BFS(int n, vector<vector<int>>& edges, int start, int end)
    {
        vector<vector<int>> g;
        vector<int> v(n, 0);
        for (auto& e : edges) {
            g[e[0]].push_back(e[1]);
            g[e[1]].push_back(e[0]);//����ͼ��ע�͵�
        }
        queue<int> q;
        q.push(start);
        int witdh;
        int steps=0;//������
        int minSteps =-1;//�ִ��յ㲽����
        int cur;
        while (!q.empty()) {
            witdh = q.size();
            while (witdh--) {
                cur = q.front();
                q.pop();
                if (v[cur]) {
                    continue;//ȥ�أ�����߽����
                }
                v[cur] = 1;//����
                if (cur == end) {//�ִ��յ�
                    minSteps = steps;
                    continue;
                }
                for (auto& e : g[cur]) {
                    if (v[e]) {
                        continue;//����߽����
                    }
                    q.push(e);//������һ��
                }
            }
            steps++;
        }
        return minSteps;
    }
};