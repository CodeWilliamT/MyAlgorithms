using namespace std;
#include <iostream>
#include <vector>
#include <queue>
//�������� ���޻�ͼ�����ʱ��
//��ɸ��������Сcost:f[x]=time[x-1]+max(f[pre(x)])
//rst=max(f)

class TopoSort {
public:
    /// <summary>
    /// �л�0��ûȦ1
    /// </summary>
    bool IsNoCircle;
    //��С����
    int MinCost;
    //������������˳��
    vector<int> orders;
    /// <summary>
    /// ���ٸ��㣨���������ģ���ǰ���ϵ���飬ÿ�������ʱ���Ƿ��1��ʼ
    /// </summary>
    TopoSort(int n, vector<vector<int>>& edges, vector<int>& time,bool from1=true) {
        queue<int> q;
        vector<vector<int>> g = vector<vector<int>>(n + from1);
        vector<int> in = vector<int>(n + from1);
        vector<int> minCost = vector<int>(n + from1);
        for (auto& e : edges) {
            g[e[1]].push_back(e[0]);
            in[e[0]]++;
        }
        for (int i = from1; i < n + from1; i++) {
            if (!in[i]) {
                q.push(i);
            }
            minCost[i] = time[i - from1];
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
                    minCost[e] = max(minCost[e], time[e - from1] + minCost[cur]);
                    if (!in[e])
                        q.push(e);
                }
            }
        }
        IsNoCircle = cnt == n;
        MinCost = 0;
        for (int i = from1; i < n+ from1; i++) {
            MinCost = max(minCost[i], MinCost);
        }
    }

    /// <summary>
    /// ���ٸ��㣨���������ģ�,ǰ�������ĵĹ�ϵ���飬�Ƿ��1��ʼ
    /// </summary>
    TopoSort(int n, vector<vector<int>>& edges, bool from1 = true) {
        typedef pair<int, int> pii;
        queue<int> q;
        vector<vector<pii>> g = vector<vector<pii>>(n + from1);
        vector<int> in = vector<int>(n + from1);
        vector<int> minCost = vector<int>(n + from1,0);
        if (!edges.empty()&&edges[0].size() == 3) {
            for (auto& e : edges) {
                g[e[1]].push_back({ e[0],e[2] });
                in[e[0]]++;
            }
        }
        else {
            for (auto& e : edges) {
                g[e[1]].push_back({ e[0],1});
                in[e[0]]++;
            }
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
                    minCost[e.first] = max(minCost[e.first], e.second + minCost[cur]);
                    if (!in[e.first])
                        q.push(e.first);
                }
            }
        }
        IsNoCircle = cnt == n;
        MinCost = 0;
        for (int i = from1; i < n + from1; i++) {
            MinCost = max(minCost[i], MinCost);
        }
    }
};

class Solution {
public:
    int minimumTime(int n, vector<vector<int>>& relations, vector<int>& time) {
        TopoSort t(n,relations,time);
        return t.MinCost;
    }
    bool canFinish(int n, vector<vector<int>>& edges) {

        TopoSort t(n, edges, 0);
        return t.IsNoCircle;
    }
};