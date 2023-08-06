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
#include <bitset>
//广搜
typedef pair<int, int> pii;
class Solution {
public:
    int maximumSafenessFactor(vector<vector<int>>& g) {
        int n = g.size();
        vector<pii> thiefs;
        vector<vector<int>> f(n, vector<int>(n, INT32_MAX));
        vector<vector<int>> v(n, vector<int>(n, 0));
        for (int i = 0; i < n; i++) {
            for (int j = 0; j < n; j++) {
                if (g[i][j]) {
                    thiefs.push_back({ i,j });
                }
            }
        }
        for (int i = 0; i < n; i++) {
            for (int j = 0; j < n; j++) {
                for (pii& t : thiefs) {
                    f[i][j] = min(f[i][j], abs(t.first - i) + abs(t.second - j));
                }
            }
        }
        int d[4][2] = { {1,0},{0,1},{-1,0},{0,-1} };
        queue<vector<int>> q;
        q.push({ 0,0,f[0][0] });
        int size;
        vector<int> cur;
        while (!q.empty()) {
            size = q.size();
            while (size--) {
                cur = q.front();
                q.pop();
                if (cur[0] <0 || cur[0] >n - 1 || cur[1] <0 || cur[1] >n - 1 || cur[2] <= v[cur[0]][cur[1]]
                    || !f[cur[0]][cur[1]]
                    || (cur[2] >= f[cur[0]][cur[1]] && v[cur[0]][cur[1]] == f[cur[0]][cur[1]])) {
                    continue;
                }
                v[cur[0]][cur[1]] = min(cur[2], f[cur[0]][cur[1]]);
                if (cur[0] == n - 1 && cur[1] == n - 1)
                    continue;
                for (int i = 0; i < 4; i++) {
                    q.push({ cur[0] + d[i][0],cur[1] + d[i][1],v[cur[0]][cur[1]] });
                }
            }
        }
        return v[n - 1][n - 1];
    }
};