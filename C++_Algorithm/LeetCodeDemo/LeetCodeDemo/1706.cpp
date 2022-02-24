using namespace std;
#include <vector>
//模拟题 细致条件分析 小数据暴力模拟
//按球，按行级推进，计算相邻方向格是否不合法,不合法记录-1
class Solution {
public:
    vector<int> findBall(vector<vector<int>>& grid) {
        int m = grid.size();
        int n = grid[0].size();
        vector<int> rst;
        int x,prex;
        for (int i = 0; i < n; i++){
            prex = i;
            for (int j = 0; j < m; j++) {
                x = prex + grid[j][prex];
                if (x == -1 || x == n|| grid[j][x] == -grid[j][prex]) {
                    rst.push_back(-1);
                    break;
                }
                prex = x;
            }
            if (rst.size() <= i) {
                rst.push_back(x);
            }
        }
        return rst;
    }
};