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

//动态规划
//存每行与前一行的最小、第二小值及最小值的x;
class Solution {
public:
    int minFallingPathSum(vector<vector<int>>& g) {
        int n = g.size();
        int min1, min2;
        int min1x;

        int premin1=0, premin2=0;
        int premin1x=-1;
        for (int i = 0; i < n; i++) {
            min1 = premin1+100, min2 = premin1+100;
            min1x = -1;
            for (int j = 0; j < n; j++) {
                if (j== premin1x){
                    if (g[i][j] + premin2 < min1) {
                        min2 = min1;
                        min1 = g[i][j] + premin2;
                        min1x = j;
                    }
                    else if (g[i][j] + premin2 < min2) {
                        min2 = g[i][j] + premin2;
                    }
                }
                else {
                    if (g[i][j] + premin1 < min1) {
                        min2 = min1;
                        min1 = g[i][j] + premin1;
                        min1x = j;
                    }
                    else if (g[i][j] + premin1 < min2) {
                        min2 = g[i][j] + premin1;
                    }
                }
            }
            premin1 = min1, premin2 = min2;
            premin1x = min1x;
        }
        return min1;
    }
};