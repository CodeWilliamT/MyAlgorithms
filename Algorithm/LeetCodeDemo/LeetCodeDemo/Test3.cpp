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
//动态规划
//f[i]到检索i位置时的最高分（不包括i位置的）
class Solution {
public:
    long long mostPoints(vector<vector<int>>& questions) {
        int n = questions.size(), nxt;
        long long rst=0, score;
        vector<long long> f(n,0);
        for (int i = 0; i < n; i++) {
            nxt = i + questions[i][1] + 1;
            score = f[i] + questions[i][0];
            if (nxt >= n) {
                rst = max(rst, score);
                continue;
            }
            f[nxt] = max(f[nxt], score);
        }
        return rst;
    }
};