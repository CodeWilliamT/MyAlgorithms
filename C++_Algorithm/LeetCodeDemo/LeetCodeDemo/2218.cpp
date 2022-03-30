using namespace std;
#include <vector>
//动态规划 背包 前缀和
//d[i][j]:第i个栈拿j次的钱数。
//f[i][x]：x次在前i个栈里拿能拿的最大钱数。
//f[i][x]=max(f[i][x],f[i][x-j]+d[i][j])
class Solution {
public:
    int maxValueOfCoins(vector<vector<int>>& p, int k) {
        int n = p.size();
        vector<vector<int>> d=p;
        vector<vector<int>> f(n + 1, vector<int>(k + 1, 0));
        for (int i = 0; i <n; i++) {
            d[i][0] = p[i][0];
            for (int j = 1; j <p[i].size(); j++) {
                d[i][j] = d[i][j-1]+p[i][j];
            }
        }
        int rst=0;
        for (int i = 1; i <= n; i++) {
            for (int x = 1; x <= k; x++) {
                f[i][x] = f[i-1][x];
                for (int j = 1; j <= p[i-1].size() && j <= x; j++) {
                    f[i][x] = max(f[i][x], f[i - 1][x - j] + d[i-1][j-1]);
                }
                if (i == n)rst = max(f[n][x],rst);
            }
        }
        return f[n][k];
    }
};