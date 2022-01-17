using namespace std;
#include <iostream>
#include <vector>
//细致条件分析
//分析边界换向条件
class Solution {
public:
    vector<int> spiralOrder(vector<vector<int>>& matrix) {
        int m = matrix.size();
        int n = matrix[0].size();
        vector<int> ans;
        bool v[10][10]{};
        int x = 0, y = 0;
        int arrow[4][2] = { {0,1},{1,0},{0,-1},{-1,0} };
        int d = 0;
        while (ans.size() < m * n)
        {
            ans.push_back(matrix[x][y]);
            v[x][y] = 1;
            x += arrow[d][0];
            y += arrow[d][1];
            if (x<0||y<0||x >= m || y >= n || v[x][y])
            {
                x -= arrow[d][0];
                y -= arrow[d][1];
                d=(d+1)%4;
                x += arrow[d][0];
                y += arrow[d][1];
            }
        }
        return ans;
    }
};