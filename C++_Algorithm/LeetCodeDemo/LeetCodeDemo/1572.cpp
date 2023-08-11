using namespace std;
#include <vector>
//¼òµ¥Ìâ ÆÓËØÄ£Äâ
class Solution {
public:
    int diagonalSum(vector<vector<int>>& mat) {
        int n = mat.size();
        int rst = 0;
        for (int i = 0; i < n; i++) {
            rst += mat[i][i] + (i == n - 1 - i ? 0 : mat[i][n - 1 - i]);
        }
        return rst;
    }
};