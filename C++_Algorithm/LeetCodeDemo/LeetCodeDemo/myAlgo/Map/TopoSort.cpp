using namespace std;
#include <iostream>
#include <vector>
#include <queue>
//拓扑排序 有向图中，求是否无环(过全部节点)，无环图的完成时间
//完成该任务的最小cost:f[x]=time[x-1]+max(f[pre(x)])
//rst=max(f)
class TopoSort {
public:
    //求是否无环
    //多少个点（点的最大消耗）,前、后关系数组，是否从1开始
    bool IsNoCircle(int n, vector<vector<int>>& edges, bool from1 = true) {
        queue<int> q;
        vector<vector<int>> g(n + from1);
        vector<int> in(n + from1);
        for (auto& e : edges) {
            g[e[1]].push_back(e[0]);
            in[e[0]]++;
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
    //求完成顺序,如果完不成，返回空集
    //多少个点（点的最大消耗）,前、后关系数组，是否从1开始
    vector<int> Orders(int n, vector<vector<int>>& edges, bool from1 = true) {
        queue<int> q;
        vector<vector<int>> g(n + from1);
        vector<int> in(n + from1);
        vector<int> orders;
        for (auto& e : edges) {
            g[e[1]].push_back(e[0]);
            in[e[0]]++;
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
                    in[e]--;
                    if (!in[e])
                        q.push(e);
                }
            }
        }
        return cnt == n ? orders : vector<int>();
    }
    //求最小消耗（所有已知边消耗为1）
    // 多少个点（点的最大消耗）,前、后的关系数组，是否从1开始
    int MinCostDefault1(int n, vector<vector<int>>& edges, bool from1 = true) {
        bool IsNoCircle;
        int MinCost;
        vector<int> orders;
        typedef pair<int, int> pii;
        queue<int> q;
        vector<vector<pii>> g(n + from1);
        vector<int> in(n + from1);
        vector<int> maxCost(n + from1, 0);
        for (auto& e : edges) {
            g[e[1]].push_back({ e[0],1 });
            in[e[0]]++;
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

    //求是否无环，完成顺序，最小消耗
    // 多少个点（点的最大消耗）,前、后、消耗的关系数组，是否从1开始
    int MinCost(int n, vector<vector<int>>& edges, bool from1 = true) {
        bool IsNoCircle;
        int MinCost;
        vector<int> orders;
        typedef pair<int, int> pii;
        queue<int> q;
        vector<vector<pii>> g(n + from1);
        vector<int> in(n + from1);
        vector<int> maxCost(n + from1, 0);
        for (auto& e : edges) {
            g[e[1]].push_back({ e[0],e[2] });
            in[e[0]]++;
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
    //求是否无环，完成顺序，最小消耗
    // 多少个点（点的最大消耗），前后关系数组，每个点的用时，是否从1开始
    int MinCost(int n, vector<vector<int>>& edges, vector<int>& time,bool from1=true) {
        bool IsNoCircle;
        int MinCost;
        vector<int> orders;
        queue<int> q;
        vector<vector<int>> g(n + from1);
        vector<int> in(n + from1);
        vector<int> maxCost(n + from1);
        for (auto& e : edges) {
            g[e[1]].push_back(e[0]);
            in[e[0]]++;
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
                //cur的顺序就是拓扑排序的顺序
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

    int minimumTime(int n, vector<vector<int>>& relations, vector<int>& time) {
        return MinCost(n, relations, time);
    }
    bool canFinish(int n, vector<vector<int>>& edges) {

        return IsNoCircle(n, edges, 0);
    }
};