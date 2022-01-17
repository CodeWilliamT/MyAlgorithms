using namespace std;
#include <iostream>
#include <vector>
//简单题
//b[i][j] = a[i * n + j];
class Solution {
public:
    vector<vector<int>> construct2DArray(vector<int>& a, int m, int n) {
        int num = a.size();
        if (num != m * n)return {};
        vector<vector<int>> b(m,vector<int>(n));
        for (int i = 0; i < m; i++)
        {
            for (int j = 0; j < n; j++)
            {
                b[i][j] = a[i * n + j];
            }
        }
        return b;
    }
};