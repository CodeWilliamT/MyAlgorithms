using namespace std;
#include <iostream>
#include <vector>
//记忆化搜索，回溯(深搜)，动态规划
//所有→深搜，1000000000 + 7大数据→记忆化搜索
class Solution {
public:
    int dfs(int pos, int fu, vector<int>& locs, int& s, int& e, vector<vector<int>>& f)
    {
        //油不够的不处理
        if (fu < 0)return 0;
        //分析过的状态直接返回结果。
        if (f[pos][fu] != -1)return f[pos][fu];
        //开始分析该状态
        f[pos][fu] = 0;
        //若当前为终点答案+1
        if (pos == e)
            f[pos][fu] +=1;
        //若没油了直接返回结果
        if (fu == 0)return f[pos][fu];
        for (int i = 0; i < locs.size(); i++)
        {
            //不动的跳过
            if (i == pos)continue;
            //结果累加
            f[pos][fu]+=dfs(i, fu - abs(locs[pos] - locs[i]), locs, s, e, f);
            f[pos][fu] = f[pos][fu] % (1000000000 + 7);
        }
        return f[pos][fu]%(1000000000 + 7);
    }
    int countRoutes(vector<int>& locs, int s, int e, int fu) {
        int n = locs.size();
        vector<vector<int>> f(n, vector<int>(fu+ 1, -1));
        return dfs(s, fu, locs, s, e, f);
    }
};