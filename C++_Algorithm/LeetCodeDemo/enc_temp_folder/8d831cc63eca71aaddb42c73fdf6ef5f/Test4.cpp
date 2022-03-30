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
//动态规划 背包分治
//d[i][j]:第i个栈拿j次的钱数。
//f[x]：x次能拿的最大钱数。
//f[x]=(y=1~x)max(f[y]+各个栈的d[i][x-y])
class Solution {
public:
    int maxValueOfCoins(vector<vector<int>>& p, int k) {
        int d[101][201]{};
        int f[201]{};
        int n = p.size();
        for (int i = 0; i < n; i++) {
            d[i][1] = p[i][0];
            f[1] = max(f[1], d[i][1]);
            for (int j = 2; j < p[i].size(); j++) {
                d[i][j] += d[i][j-1];
            }
        }
        for (int x = 2; x <= k; x++) {
            for (int y = 1; y <=x; y++) {
                for (int i = 0; i < n; i++) {
                    if(y<=p[i].size())
                        f[x] = max(f[y] + d[i][y], f[x]);
                }
            }
        }
        return f[k];
    }
};