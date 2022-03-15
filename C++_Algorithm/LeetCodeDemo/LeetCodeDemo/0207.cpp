using namespace std;
#include <vector>
#include <queue>
//图论
//拓扑排序判环
class Solution {
public:
    bool canFinish(int n, vector<vector<int>>& edges) {
        queue<int> q;
        vector<vector<int>> g(n);
        vector<int> in(n);
        int count=0;
        for (auto& e : edges){
            g[e[1]].push_back(e[0]);
            in[e[0]]++;
        }
        for (int i=0;i<n;i++){
            if (!in[i])
                q.push(i);
        }
        while (!q.empty()){
            int cur = q.front();
            //cur的顺序就是拓扑排序的顺序
            count++;
            q.pop();
            for (auto& e : g[cur]){
                in[e]--;
                if (!in[e])
                    q.push(e);
            }
        }

        return count==n;
    }
};
