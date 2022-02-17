using namespace std;
#include <iostream>
#include <vector>
#include <string>
#include <algorithm>
#include <unordered_set>
#include <unordered_map>
#include <set>
#include <map>
#include <queue>
#include <stack>
#include <functional>
//回溯(深搜) 记忆化搜索 动态规划
class Solution {
public:
    double knightProbability(int n, int k, int row, int column) {
        double rst=1;
        int dir[8][2] = { {1,2},{2,1},{2,-1},{1,-2},{-1,2},{-2,1},{-2,-1}, {-1,-2} };
        bool v[25][25]{};
        int f[25][25]{};
        function<bool(int, int, int)> dfs = [&](int x, int y, int step) {
            if (x < 0 || y < 0 || x >= n || y >= n) {
                return false;
            }
            if (v[x][y])
            {
                rst -= f[x][y];
                return true;
            }
            v[x][y] = 1;
            if (step >= k) {
                return true;
            }
            int cnt=0;
            for (int i = 0; i < 8; i++) {
                cnt+=dfs(x + dir[i][0], y + dir[i][1], step + 1);
            }
            f[x][y] = 1-cnt / 8;
            return true;
        };
        dfs(row, column, 0);
        return rst;
    }
};