using namespace std;
#include <vector>
#include <algorithm>
//深搜
class Solution {
public:
    int dfs(vector<vector<int>>& graph, vector<bool>& visited, int u) {
        sort(begin(graph[u]), end(graph[u]), [&](int x, int y) {
            return graph[x].size() > graph[y].size();
        });
        int n = 0;
        int d = 0;
        bool b = false;
        visited[u] = true;
        for (int v : graph[u]) {
            if (visited[v]) continue;
            if (graph[v].size() > graph[u].size()) return 0;
            ++d;
        }
        for (int v : graph[u]) {
            if (graph[v].size() == graph[u].size()) b = true;
            if (visited[v] || graph[v].size() > graph[u].size()) continue;
            int t = dfs(graph, visited, v);
            if (t == 0) return 0;
            if (t < 0) b = true;
            n += abs(t);
        }
        return n == d ? b ? -(n + 1) : n + 1 : 0;
    }

    int checkWays(vector<vector<int>> const& pairs) {
        vector<vector<int>> graph;
        for (const auto& e : pairs) {
            if (graph.size() < e[1])
                graph.resize(e[1]);
            int u = e[0] - 1;
            int v = e[1] - 1;
            graph[u].push_back(v);
            graph[v].push_back(u);
        }
        int m = 0;
        int n = graph.size();
        vector<bool> visited(n, false);
        int root = 0;
        for (int i = 0; i < n; ++i) {
            if (graph[i].size() > 0) ++m;
            if (graph[i].size() > graph[root].size()) root = i;
        }
        int ans = dfs(graph, visited, root);
        return ans != 0 && abs(ans) == m ? ans < 0 ? 2 : 1 : 0;
    }
};