using namespace std;
#include <iostream>
#include <vector>
#include <queue>
//�������� ����ͼ�У����Ƿ��޻�(��ȫ���ڵ�)���޻�ͼ�����ʱ��
//��ɸ��������Сcost:f[x]=time[x-1]+max(f[pre(x)])
//rst=max(f)
class TopoSort {
public:
    //���Ƿ��޻�
    //���ٸ���,��ǰ��ϵ���飬�Ƿ��1��ʼ
    bool IsNoCircle(int n, vector<vector<int>>& edges, bool from1 = 0) {
        queue<int> q;
        vector<vector<int>> g(n + from1);
        vector<int> in(n + from1);
        for (auto& e : edges) {
            g[e[1]].push_back(e[0]);//ǰ�õ������õ�Ĺ�ϵ��
            in[e[0]]++;//���õ�����
        }
        for (int i = from1; i < n + from1; i++) {
            if (!in[i]) {
                q.push(i);
            }
        }
        int cnt = 0;
        while (!q.empty()) {
            int len = q.size();
            while (len--)
            {
                int cur = q.front();
                //cur��˳��������������˳��
                cnt++;
                q.pop();
                for (auto& e : g[cur]) {
                    in[e]--;
                    if (!in[e])
                        q.push(e);
                }
            }
        }
        return cnt == n;
    }
    //�����˳��,����겻�ɣ����ؿռ�
    //���ٸ���,��ǰ��ϵ���飬�Ƿ��1��ʼ
    vector<int> Orders(int n, vector<vector<int>>& edges, bool from1 = 0) {
        queue<int> q;
        vector<vector<int>> g(n + from1);
        vector<int> in(n + from1);
        vector<int> orders;
        for (auto& e : edges) {
            g[e[1]].push_back(e[0]);//ǰ�õ������õ�Ĺ�ϵ��
            in[e[0]]++;//���õ�����
        }
        for (int i = from1; i < n + from1; i++) {
            if (!in[i]) {
                q.push(i);
            }
        }
        int cnt = 0;
        while (!q.empty()) {
            int len = q.size();
            while (len--)
            {
                int cur = q.front();
                //cur��˳��������������˳��
                cnt++;
                orders.push_back(cur + from1);
                q.pop();
                for (auto& e : g[cur]) {
                    in[e]--;
                    if (!in[e])
                        q.push(e);
                }
            }
        }
        return cnt == n ? orders : vector<int>();
    }
    //ÿ��������Ϊ1������С���ģ�������֪������Ϊ1��
    // ���ٸ���,��ǰ��ϵ���飬�Ƿ��1��ʼ
    int MinCostDefault1(int n, vector<vector<int>>& edges, bool from1 = 0) {
        bool IsNoCircle;
        int MinCost;
        vector<int> orders;
        typedef pair<int, int> pii;
        queue<int> q;
        vector<vector<pii>> g(n + from1);
        vector<int> in(n + from1);
        vector<int> maxCost(n + from1, 0);
        for (auto& e : edges) {
            g[e[1]].push_back({ e[0],1 });//ǰ�õ������õ�Ĺ�ϵ��
            in[e[0]]++;//���õ�����
        }
        for (int i = from1; i < n + from1; i++) {
            if (!in[i]) {
                q.push(i);
            }
        }
        int cnt = 0;
        while (!q.empty()) {
            int len = q.size();
            while (len--)
            {
                int cur = q.front();
                //cur��˳��������������˳��
                cnt++;
                orders.push_back(cur + from1);
                q.pop();
                for (auto& e : g[cur]) {
                    in[e.first]--;
                    maxCost[e.first] = max(maxCost[e.first], e.second + maxCost[cur]);
                    if (!in[e.first])
                        q.push(e.first);
                }
            }
        }
        IsNoCircle = cnt == n;
        MinCost = 0;
        for (int i = from1; i < n + from1; i++) {
            MinCost = max(maxCost[i], MinCost);
        }
        return MinCost;
    }

    //ÿ���߲�ͬ����,���Ƿ��޻������˳����С����
    // ���ٸ���,��ǰ�����ĵĹ�ϵ���飬�Ƿ��1��ʼ
    int MinCost(int n, vector<vector<int>>& edges, bool from1 = 0) {
        bool IsNoCircle;
        int MinCost;
        vector<int> orders;
        typedef pair<int, int> pii;
        queue<int> q;
        vector<vector<pii>> g(n + from1);
        vector<int> in(n + from1);
        vector<int> maxCost(n + from1, 0);
        for (auto& e : edges) {
            g[e[1]].push_back({ e[0],e[2] });//ǰ�õ������õ�Ĺ�ϵ��
            in[e[0]]++;//���õ�����
        }
        for (int i = from1; i < n + from1; i++) {
            if (!in[i]) {
                q.push(i);
            }
        }
        int cnt = 0;
        while (!q.empty()) {
            int len = q.size();
            while (len--)
            {
                int cur = q.front();
                //cur��˳��������������˳��
                cnt++;
                orders.push_back(cur + from1);
                q.pop();
                for (auto& e : g[cur]) {
                    in[e.first]--;
                    maxCost[e.first] = max(maxCost[e.first], e.second + maxCost[cur]);
                    if (!in[e.first])
                        q.push(e.first);
                }
            }
        }
        IsNoCircle = cnt == n;
        MinCost = 0;
        for (int i = from1; i < n + from1; i++) {
            MinCost = max(maxCost[i], MinCost);
        }
        return MinCost;
    }
    //ÿ���㲻ͬ���ġ����Ƿ��޻������˳����С����
    // ���ٸ��㣬��ǰ��ϵ���飬ÿ�������ʱ���ģ��Ƿ��1��ʼ
    int MinCost(int n, vector<vector<int>>& edges, vector<int>& time,bool from1=0) {
        bool IsNoCircle;
        int MinCost;
        vector<int> orders;
        queue<int> q;
        vector<vector<int>> g(n + from1);
        vector<int> in(n + from1);
        vector<int> maxCost(n + from1);
        for (auto& e : edges) {
            g[e[1]].push_back(e[0]);//ǰ�õ������õ�Ĺ�ϵ��
            in[e[0]]++;//���õ�����
        }
        for (int i = from1; i < n + from1; i++) {
            if (!in[i]) {
                q.push(i);
            }
            maxCost[i] = time[i - from1];
        }
        int cnt = 0;
        while (!q.empty()) {
            int len = q.size();
            while (len--)
            {
                int cur = q.front();
                //cur��˳��������������˳��
                cnt++;
                orders.push_back(cur+ from1);
                q.pop();
                for (auto& e : g[cur]) {
                    in[e]--;
                    maxCost[e] = max(maxCost[e], time[e - from1] + maxCost[cur]);
                    if (!in[e])
                        q.push(e);
                }
            }
        }
        IsNoCircle = cnt == n;
        MinCost = 0;
        for (int i = from1; i < n+ from1; i++) {
            MinCost = max(maxCost[i], MinCost);
        }
        return MinCost;
    }
    //��ǰ�����Ƿ���ǰ�ù�ϵ,
    //���ٸ���,ǰ�����ϵ���飬�Ƿ��1��ʼ
    vector<vector<bool>> IsPre(int n, vector<vector<int>>& edges, bool from1 = 0) {
        queue<int> q;
        vector<vector<int>> g(n + from1);
        vector<int> in(n + from1);
        vector<vector<bool>> isPre(n, vector<bool>(n, false));
        for (auto& e : edges) {
            g[e[0]].push_back(e[1]);//ǰ�õ������õ�Ĺ�ϵ��
            in[e[1]]++;//���õ�����
        }
        for (int i = from1; i < n + from1; i++) {
            if (!in[i]) {
                q.push(i);
            }
        }
        int cnt = 0;
        while (!q.empty()) {
            int len = q.size();
            while (len--)
            {
                int cur = q.front();
                //cur��˳��������������˳��
                cnt++;
                q.pop();
                for (auto& e : g[cur]) {

                    isPre[cur][e] = true;
                    for (int i = 0; i < n; ++i) {
                        isPre[i][e] = isPre[i][e] || isPre[i][cur];
                    }
                    in[e]--;
                    if (!in[e])
                        q.push(e);
                }
            }
        }
        return isPre;
    }
    int minimumTime(int n, vector<vector<int>>& relations, vector<int>& time) {
        return MinCost(n, relations, time);
    }
    bool canFinish(int n, vector<vector<int>>& edges) {

        return IsNoCircle(n, edges, 0);
    }
};