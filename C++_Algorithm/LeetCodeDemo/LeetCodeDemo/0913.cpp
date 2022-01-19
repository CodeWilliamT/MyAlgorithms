using namespace std;
#include <iostream>
#include <vector>
#include <functional>
//深搜(回溯) 博弈 记忆化搜索(动态规划) 
//鼠先猫后
//鼠最优策略，不能赢不能平则输，能赢则赢，赢不了就平
//猫最优策略：不能赢不能平则输，能赢则赢，赢不了就平
class Solution {
public:
    int catMouseGame(vector<vector<int>>& graph) {
        int n = graph.size();
        vector<vector<vector<int>>> f(n+1, vector<vector<int>>(n+1, vector<int>(2 * n+2, -1)));
        function<int(int,int,int)> dfs = [&](int x,int y,int steps) {
            if (f[x][y][steps] > -1)
                return f[x][y][steps];
            if (x == 0) {
                f[x][y][steps] = 1;
                return 1;
            }
            if (x == y) {
                f[x][y][steps] = 2;
                return 2;
            }
            if (steps >= 2 * n) {
                f[x][y][steps] = 0;
                return 0;
            }
            bool turn = steps % 2;
            int cur = turn ? y : x;
            int nextx, nexty;
            int nextrst;
            int defaultRst=(!turn)+1;
            int rst = defaultRst;
            //博弈处：默认对手赢，不是对手赢，自己能赢则做，赢不了则平。
            for (auto& e:graph[cur]) {
                if (turn&&!e)continue;
                nextx = turn ? x: e;
                nexty = turn ? e : y;
                nextrst = dfs(nextx, nexty, steps + 1);
                if (nextrst != defaultRst) {
                    rst = nextrst;
                    if (nextrst != 0)
                        break;
                }
            }
            f[x][y][steps] = rst;
            return f[x][y][steps];
        };
        return dfs(1, 2, 0);
    }
};