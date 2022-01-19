using namespace std;
#include <vector>
#include <unordered_map>
#include <functional>
//图论 欧拉通路
//确定出度比入度大1的点作为起点
//然后从起点起深搜遍历边集依次遍历顶点，存进入答案。(注意先删顶点，然后递归，然后存答案，然后倒置)
//数据：各个顶点入度计数集，各个顶点出度计数集，各个顶点有向边所连顶点集
//起点：出度比入度大1的点。
//特殊：
//顶点不连续：用哈希映射顶点
//可能存在环：起始第一个元素作为开始。
class Solution {
public:
    vector<vector<int>> validArrangement(vector<vector<int>>& pairs) {
        vector<vector<int>> rst;
        int n = pairs.size();
        unordered_map<int,int> in,out;
        unordered_map<int, vector<int>> edges;
        for (auto& e : pairs)
        {
            in[e[1]]++;
            out[e[0]]++;
            edges[e[0]].push_back(e[1]);
        }
        int s= pairs[0][0];
        for (auto& e : out)
        {
            if (e.second == in[e.first]+1)
                s= e.first;
        }
        function<void(int)> dfs = [&](int u)
        {
            int v;
            while (!edges[u].empty())
            {
                v = edges[u].back();
                edges[u].pop_back();
                dfs(v);
                rst.push_back({ u,v });
            }
        };
        dfs(s);
        reverse(rst.begin(), rst.end());
        return rst;
    }
};