using namespace std;
#include <vector>
//分析 模拟
//返回至少与一个其他点同行或同列的点数=总数-独立点数
//独立点g[i][j]为:独行,独列,为1；
class Solution {
public:
    int countServers(vector<vector<int>>& g) {
        int n = g.size();
        int m = g[0].size();
        vector<int> r(m,0),c(n,0);
        int rst = 0;
        for (int i = 0; i < n; i++) {
            for (int j = 0; j < m; j++) {
                if (g[i][j]) {
                    rst++;
                    r[j]++;
                    c[i]++;
                }
            }
        }
        for (int i = 0; i < n; i++) {
            for (int j = 0; j < m; j++) {
                if (r[j]==1&&c[i]==1&&g[i][j]) {
                    rst--;
                }
            }
        }
        return rst;
    }
};