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
//动态规划
//找乘积0最多的行列组合。
//枚举转角点。计算转角点的4个方向的转角路径乘积，比出最大的。
//数字太大：三个数组记录每个数有几个5与几个2。
//到某一列的第几行，到某一行的第几列，的前缀5数目，2数目，所以是4个数组
class Solution {
    int countNum(int num, int x) {
        int rst = 0;
        while (num % x == 0) {
            num /=x;
            rst++;
        }
        return rst;
    }
public:
    int maxTrailingZeros(vector<vector<int>>& grid) {
        int m = grid.size();
        int n = grid[0].size();
        vector<vector<int>> rowFives(m+1, vector<int>(n, 0)), rowTwos(m+1, vector<int>(n, 0));
        vector<vector<int>> colFives(m, vector<int>(n+1, 0)), colTwos(m, vector<int>(n+1, 0));
        for (int i = 1; i <= m; i++) {
            for (int j = 0; j < n; j++) {
                    rowFives[i][j] = rowFives[i-1][j]+countNum(grid[i-1][j], 5);
                    rowTwos[i][j] = rowTwos[i-1][j]+countNum(grid[i-1][j], 2);
            }
        }
        for (int i = 0; i < m; i++) {
            for (int j = 1; j <=n; j++) {
                colFives[i][j] = colFives[i][j - 1] + countNum(grid[i][j-1], 5);
                colTwos[i][j] = colTwos[i][j - 1] + countNum(grid[i][j-1], 2);
            }
        }
        int rst = 0;
        for (int i = 0; i < m; i++) {
            for (int j = 0; j < n; j++) {
                rst = max({ rst, min(rowFives[i+1][j]+ colFives[i][j],rowTwos[i + 1][j] + colTwos[i][j]),
                    min(rowFives[i + 1][j] + colFives[i][n]-colFives[i][j+1],rowTwos[i + 1][j] + colTwos[i][n] - colTwos[i][j + 1]),
                    min(rowFives[m][j] -rowFives[i + 1][j] + colFives[i][j+1],rowTwos[m][j] - rowTwos[i + 1][j] + colTwos[i][j + 1]),
                    min(rowFives[m][j] - rowFives[i][j] + colFives[i][n] - colFives[i][j + 1],rowTwos[m][j] - rowTwos[i][j] + colTwos[i][n] - colTwos[i][j + 1])
                    });
            }
        }
        return rst;
    }
};