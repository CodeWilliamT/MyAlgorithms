using namespace std;
#include <iostream>
#include <vector>
//深搜(回溯)
class Solution {
    void dfs(int cur,vector<int>& path,vector<vector<int>>& ans, vector<vector<int>>& g){
        if (cur == g.size() - 1) { ans.push_back(path); return; }
        for (int i = 0; i < g[cur].size(); i++)
        {
            path.push_back(g[cur][i]);
            dfs(g[cur][i], path, ans, g);
            path.pop_back();
        }
    }
public:
    vector<vector<int>> allPathsSourceTarget(vector<vector<int>>& g) {
        vector<vector<int>> ans;
        vector<int> path = {0};
        dfs(0,path,ans, g);
        return ans;
    }
};