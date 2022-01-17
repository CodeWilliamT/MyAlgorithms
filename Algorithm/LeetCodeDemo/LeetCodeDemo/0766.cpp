using namespace std;
#include <iostream>
#include <vector>
//条件分析，简单题
class Solution {
public:
    bool isToeplitzMatrix(vector<vector<int>>& matrix) {
        int m = matrix.size();
        int n = matrix[0].size();
        int sr = m - 1, sc = 0;
        while (true)
        {
            for (int i = sr, j = sc; i < m && j < n; i++, j++)
            {
                if (matrix[i][j] != matrix[sr][sc])return false;
            }
            if (sr == 0 && sc < n - 1)sc++;
            if (sr > 0)sr--;
            if (sr == 0 && sc == n - 1)break;
        }
        return true;
    }
};