using namespace std;
#include <vector>
#include <algorithm>
//两分 巧思
//建个数组f[i][j]，表示i矩形以及后面高度大于等于j的有多少个矩形。 
class Solution {
public:
    vector<int> countRectangles(vector<vector<int>>& r, vector<vector<int>>& p) {
        sort(r.begin(), r.end());
        int m = r.size();
        vector<vector<int>> f(m, vector<int>(101, 0));
        for (int i = m-1; i>=0; i--) {
            for (int j = 0; j < 101; j++) {
                if (i == m - 1) {
                    f[i][j] = r[i][1] >= j ? 1 : 0;
                    continue;
                }
                f[i][j] = f[i + 1][j] + (r[i][1] >= j ? 1 : 0);
            }
        }
        int idx;
        int n = p.size();
        vector<int> rst(n, 0);
        for (int i = 0; i < n;i++) {
            idx = lower_bound(r.begin(), r.end(), p[i])-r.begin();
            if (idx >= m)continue;
            rst[i] += f[idx][p[i][1]];
        }
        return rst;
    }
};