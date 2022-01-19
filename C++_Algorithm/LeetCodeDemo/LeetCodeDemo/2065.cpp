using namespace std;
#include <iostream>
#include <vector>
//深搜,稀疏图(邻接表)
//最大复杂度4^9,所以强搜，注意拿过的就归0
class Solution {
private:
    void dfs(int& ans, int cur, int gold, int rest, vector<vector<vector<int>>>& map, vector<int>& values)
    {

        if (rest < 0)
            return;
        if (cur == 0)
            ans = max(gold, ans);
        if (rest == 0)
            return;
        for (int i = 0; i < map[cur].size(); i++)
        {
            int back = values[map[cur][i][0]];
            values[map[cur][i][0]] = 0;
            dfs(ans, map[cur][i][0], gold + back, rest - map[cur][i][1], map, values);
            values[map[cur][i][0]] = back;
        }
    }
public:
    int maximalPathQuality(vector<int>& values, vector<vector<int>>& edges, int maxTime) {
        int n = values.size();
        vector<vector<vector<int>>> map(n);
        for (auto e : edges) {
            map[e[0]].push_back({ e[1],e[2] });
            map[e[1]].push_back({ e[0],e[2] });
        }
        int ans = values[0];
        int back = values[0];
        values[0] = 0;
        dfs(ans, 0, ans, maxTime, map, values);
        values[0] = back;
        return ans;
    }
};