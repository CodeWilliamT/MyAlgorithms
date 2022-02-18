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
//步骤，位置，指向概率 f[101][25][25]{}
//答案为k步的概率和。
//f[t][x][y] += f[t - 1][i][j] / 8;
class Solution {
public:
    double knightProbability(int n, int k, int row, int column) {
        if (!k)return 1;
        double rst = 0;
        int dir[8][2] = { {1,2},{2,1},{2,-1},{1,-2},{-1,2},{-2,1},{-2,-1}, {-1,-2} };
        double f[101][25][25]{};
        f[0][row][column] = 1;
        for (int t = 1; t <= k; t++) {
            for (int i = 0; i < n; i++) {
                for (int j = 0; j < n; j++) {
                    for (int q = 0; q < 8; q++) {
                        int x = i + dir[q][0];
                        int y = j + dir[q][1];
                        if (x < 0 || y < 0 || x >= n || y >= n) {
                            continue;
                        }
                        f[t][x][y] += f[t - 1][i][j] / 8;
                        if (t == k)rst += f[t - 1][i][j] / 8;
                    }
                }
            }
        }
        return rst;
    }
};