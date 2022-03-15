using namespace std;
#include <vector>
#include <unordered_map>
#include <functional>
//图论 欧拉通路
//已知有向图的边集，求每条边均经过巧好一次的边排列。(一条或全部)
//1.通过给出的有向边构建邻接表g，统计各顶点入度in, 出度out.
//2.确定出度比入度大1的点作为起点s(求全部则s为数组, 遍历数组执行3, 4两部)
//3.然后从起点起, 深搜回溯遍历其指向的其他顶点。(注意先读取顶点，邻接表上删顶点，然后递归该顶点，然后再存该尝试的边到路径中)
//4.倒置输出的路径便为欧拉通路。
//起点：出度比入度大1的点。
//特殊：
//顶点不连续：用哈希映射顶点
//可能存在环：起始第一个元素作为开始。
class Solution {
public:
    vector<vector<int>> validArrangement(vector<vector<int>>& pairs) {
        vector<vector<int>> rst;
        int n = pairs.size();
        unordered_map<int, int> in, out;
        unordered_map<int, vector<int>> g;
        for (auto& e : pairs)
        {
            in[e[1]]++;
            out[e[0]]++;
            g[e[0]].push_back(e[1]);
        }
        //如果要找全部欧拉通路则s为数组
        int s = pairs[0][0];
        for (auto& e : out)
        {
            if (e.second == in[e.first] + 1)
                s = e.first;
        }
        function<void(int)> dfs = [&](int u)
        {
            int v;
            while (!g[u].empty())
            {
                v = g[u].back();
                g[u].pop_back();
                dfs(v);
                rst.push_back({ u,v });
            }
        };
        //如果要找全部欧拉通路则s为数组，遍历s
        dfs(s);
        reverse(rst.begin(), rst.end());
        return rst;
    }
};