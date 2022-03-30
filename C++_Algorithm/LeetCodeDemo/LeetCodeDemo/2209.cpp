using namespace std;
#include <iostream>
#include <vector>

//动态规划 背包 前缀和
//体积为k，价值为i结尾遮住的白格数。
//i长度地板用j块地毯遮盖的最多的白格数d[i][j]
//d[i][j]=max(d[i-len][j-1]+t[i-1]-t[i-1-len],d[i-1][j]);
class Solution {
public:
    int minimumWhiteTiles(string s, int k, int len) {
        int n = s.size();
        vector<vector<int>> d(n+1, vector<int>(k+1, 0));
        vector<int> t(n+1, 0);
        t[0] = s[0]-'0';
        for (int i = 1; i < n; i++) {
            t[i] = t[i - 1] + s[i] - '0';
        }
        int rst = t[n-1];
        for (int i = 1; i <=n; i++) {
            for (int j = 1; j <= k; j++) {
                d[i][j] = t[i-1];
                if(i - 1 - len>=0)
                d[i][j] = max(d[i - len][j - 1] + t[i - 1] - t[i - 1 - len], d[i - 1][j]);
                if (i == n) {
                    rst = min(rst, t[n - 1] - d[i][j]);
                }
            }
        }
        return rst;
    }
};