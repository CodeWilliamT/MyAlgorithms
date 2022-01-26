using namespace std;
#include <vector>
#include <queue>
//广搜
//算出从0开始到所有节点的最短用时,跟第二短用时
class Solution {
public:
    int secondMinimum(int n, vector<vector<int>>& edges, int time, int change) {
        vector<vector<int>> g(n+1);
        vector<int> mn(n+1, INT32_MAX);
        vector<int> f(n+1, INT32_MAX);
        for (auto& e : edges) {
            g[e[0]].push_back(e[1]);
            g[e[1]].push_back(e[0]);
        }
        queue<int> q;
        q.push(1);
        int steps = 0,cost=0,size,cur;
        while (!q.empty()) {
            size = q.size();
            while (size--) {
                cur = q.front();
                q.pop();
                if (cost<=mn[cur]) {
                    if(cost< mn[cur])
                        f[cur] = mn[cur];
                    mn[cur] = cost;
                }
                else if(cost < f[cur]){
                    f[cur] = cost;
                }
                else {
                    continue;
                }
                for (auto&e:g[cur]) {
                    q.push(e);
                }
            }
            if ((cost / change) % 2) {
                cost = (cost / change + 1) * change;
            }
            steps++;
            cost += time;
        }
        return f[n];
    }
};