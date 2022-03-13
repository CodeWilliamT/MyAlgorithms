using namespace std;
#include <vector>
#include <unordered_set>
//哈希 模拟
//全部染色
class Solution {
public:
    int digArtifacts(int n, vector<vector<int>>& a, vector<vector<int>>& d) {
        int g[1000][1000]{};
        int rst= a.size();
        for (int k = 0; k < a.size();k++) {
            for (int i = a[k][0]; i <= a[k][2]; i++) {
                for (int j = a[k][1]; j <= a[k][3]; j++) {
                    g[i][j] = k+1;
                }
            }
        }
        for (auto& e : d) {
            g[e[0]][e[1]] = 0;
        }
        unordered_set<int> st;
        for (int i = 0; i < n; i++) {
            for (int j = 0; j < n; j++) {
                if (g[i][j]&&!st.count(g[i][j])) {
                    st.insert(g[i][j]);
                }
            }
        }
        rst -= st.size();
        return rst;
    }
};