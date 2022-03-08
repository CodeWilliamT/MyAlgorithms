using namespace std;
#include <vector>
//细致条件分析 前缀和
//f[x]=从0到x位置共计有多少符合条件的盘子，
//以及用一个数组记录其右侧蜡烛位置，一个数组记录左侧蜡烛,查询合法即做差
class Solution {
public:
    vector<int> platesBetweenCandles(string s, vector<vector<int>>& queries) {
        int n = s.size();
        vector<int> f(n, 0), l(n, 0), r(n, 0);
        int cnt = 0;
        for (int i = 0; i < n; i++) {
            if (s[i] == '|') {
                f[i] = cnt;
            }
            else {
                cnt++;
                f[i] = i > 0 ? f[i - 1] : 0;
            }
        }
        for (int i = 0; i < n; i++) {
            if (s[i] == '|') {
                l[i] = i;
            }
            else {
                l[i] = i > 0 ? l[i - 1] : -1;
            }
        }
        for (int i = n - 1; i >= 0; i--) {
            if (s[i] == '|') {
                r[i] = i;
            }
            else {
                r[i] = i < n - 1 ? r[i + 1] : n;
            }
        }
        vector<int> rst;
        for (auto& e : queries) {
            rst.push_back(l[e[1]] == n || r[e[0]] == -1 || l[e[1]] <= r[e[0]] ? 0 : f[l[e[1]]] - f[r[e[0]]]);
        }
        return rst;
    }
};