using namespace std;
#include <iostream>
#include <vector>
#include <queue>
//拓扑排序 求无环图的完成时间
//完成该任务的最小cost:f[x]=time[x-1]+max(f[pre(x)])
//rst=max(f)

class TopoSort {
public:
    /// <summary>
    /// 有环0，没圈1
    /// </summary>
    bool IsNoCircle;
    //最小消耗
    int MinCost;
    //拓扑完成任务的顺序
    vector<int> orders;
    /// <summary>
    /// 多少个点（点的最大消耗），前后关系数组，每个点的用时，是否从1开始
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
                //cur的顺序就是拓扑排序的顺序

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
    /// 多少个点（点的最大消耗）,前、后、消耗的关系数组，是否从1开始
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
                //cur的顺序就是拓扑排序的顺序
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