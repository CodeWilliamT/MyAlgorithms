using namespace std;
#include <vector>
#include <unordered_set>
#include <functional>
//回溯 深搜
//对开始的回溯思路，剪枝
class Solution {
public:
    bool hasValidPath(vector<vector<char>>& g) {
        int m = g.size();
        int n = g[0].size();
        vector<vector<vector<bool>>> v(m, vector<vector<bool>>(n, vector<bool>((m + n+1)/2, 0)));
        function<bool(int, int, int)> dfs = [&](int r, int c, int delta) {
            delta += g[r][c] == '(' ? 1 : -1;
            if (delta < 0||delta>(m+n)/2) {
                return false;
            }
            if (v[r][c][delta]) {
                return false;
            }
            v[r][c][delta] = 1;
            if (r == m - 1 && c == n - 1 && delta == 0) {
                return true;
            }
            if (r + 1 < m)
                if (dfs(r + 1, c, delta))
                    return true;
            if (c + 1 < n)
                if (dfs(r, c + 1, delta))
                    return true;
            return false;
        };
        return dfs(0, 0, 0);
    }
};
//动态规划 哈希
//深搜复杂度为2^100次超时。
//然后想到动态规划，然后观察到值域比较小。
//所以用左括号数来作为状态传递，因为值域在0~200内，所以哈希。
//最差时间复杂度为100*100*200=2e6；
//最差空间复杂度为也为2e6
//写题解重新提交了下看了下时效较差，应该是u_set的锅..然后改为数组了
//周赛时间有限没细思。。看了别的用动态规划的，没用set。。。应该用数组做的。。插入还是耗时了点
class Solution {
public:
    bool hasValidPath(vector<vector<char>>& g) {
        int m = g.size();
        int n = g[0].size();
        vector<vector<vector<bool>>> v(m, vector<vector<bool>>(n, vector<bool>(m + n, 0)));
        int val = g[0][0] == '(' ? 1 : -1;
        if (val == -1)return false;
        int cnt;
        v[0][0][val]=1;
        for (int i = 0; i < m; i++) {
            for (int j = 0; j < n; j++) {
                if (i == 0 && j == 0)continue;
                val = g[i][j] == '(' ? 1 : -1;
                if (i > 0) {
                    for (int k = 0; k < m + n;k++) {
                        if (!v[i-1][j][k])continue;
                        cnt = k + val;
                        if (cnt >= 0) {
                            v[i][j][cnt]=1;
                        }
                    }
                }
                if (j > 0) {
                    for (int k = 0; k < m + n; k++) {
                        if (!v[i][j - 1][k])continue;
                        cnt = k + val;
                        if (cnt >= 0) {
                            v[i][j][cnt] = 1;
                        }
                    }
                }
            }
        }
        return v[m-1][n-1][0];
    }
};
//开始的回溯思路
class Solution {
public:
    bool hasValidPath(vector<vector<char>>& g) {
        int m = g.size();
        int n = g[0].size();
        function<bool(int, int, int)> dfs = [&](int r, int c, int delta) {
            delta += g[r][c] == '(' ? 1 : -1;
            if (delta < 0) {
                return false;
            }
            if (r == m - 1 && c == n - 1 && delta == 0) {
                return true;
            }
            if (r + 1 < m)
                if (dfs(r + 1, c, delta))
                    return true;
            if (c + 1 < n)
                if (dfs(r, c + 1, delta))
                    return true;
            return false;
        };
        return dfs(0, 0, 0);
    }
};