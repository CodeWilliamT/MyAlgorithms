using namespace std;
#include <vector>
//找规律
//从小到大买就会产生最大开销
class Solution {
    typedef long long ll;
public:
    long long maxSpending(vector<vector<int>>& v) {
        int c[10]{};
        long long rst = 0;
        int m = v.size();
        int n = v[0].size();
        for (int i = 0; i < m; i++) {
            c[i] = n-1;
        }
        int d = 1, s = 0;
        while (d <= m * n) {
            for (int i = 0; i < m; i++) {
                if (c[i] < 0)continue;
                if (c[s]==-1||v[i][c[i]] < v[s][c[s]]) {
                    s = i;
                }
            }
            rst += (ll)v[s][c[s]]*d;
            c[s]--;
            d++;
        }
        return rst;
    }
};