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
typedef long long ll;
typedef pair<ll, ll> pll;
typedef pair<int, int> pii;
//动态规划 背包问题
//数组选n/3个不相邻的数。
//状态[阶段]=f[i][j]:到前i数中选j个的最大和
//复杂度500*500
class Solution {
public:
    int calculate(const vector<int>& slices) {
        int N = slices.size(), n = (N + 1) / 3;
        vector<vector<int>> f(N, vector<int>(n + 1, INT_MIN));
        f[0][0] = 0;
        f[0][1] = slices[0];
        f[1][0] = 0;
        f[1][1] = max(slices[0], slices[1]);
        for (int i = 2; i < N; i++) {
            f[i][0] = 0;
            for (int j = 1; j <= n; j++) {
                f[i][j] = max(f[i - 1][j], f[i - 2][j - 1] + slices[i]);
            }
        }
        return f[N - 1][n];
    }

    int maxSizeSlices(vector<int>& slices) {
        vector<int> v1(slices.begin() + 1, slices.end());
        vector<int> v2(slices.begin(), slices.end() - 1);
        int ans1 = calculate(v1);
        int ans2 = calculate(v2);
        return max(ans1, ans2);
    }
};