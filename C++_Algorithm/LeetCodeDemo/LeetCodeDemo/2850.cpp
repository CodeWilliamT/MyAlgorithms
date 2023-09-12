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
#include "myAlgo\Structs\TreeNode.cpp"
//枚举
//
class Solution {
    typedef pair<int, int> pii;
public:
    int minimumMoves(vector<vector<int>>& g) {
        vector<pii> from,to;
        for (int i = 0; i < 3; i++) {
            for (int j = 0; j < 3; j++) {
                if (!g[i][j]) {
                    from.emplace_back(i, j);
                }
                if (g[i][j]>1) {
                    for(int t=0;t<g[i][j]-1;t++)
                        to.emplace_back(i, j);
                }
            }
        }
        int rst = 72;
        int sum;
        do {
            sum = 0;
            for (int i = 0; i < to.size();i++) {
                sum += abs(to[i].first - from[i].first) + abs(to[i].second - from[i].second);
            }
            rst = min(rst, sum);
        } while (next_permutation(from.begin(), from.end()));
        return rst;
    }
};