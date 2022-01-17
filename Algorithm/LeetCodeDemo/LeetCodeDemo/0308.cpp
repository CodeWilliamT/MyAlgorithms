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
//设计题，巧思，高效
//记录从(0,0)出发各对应点矩形的面积S，S所求=S[右][下]-S[右][上-1]-S[左-1][下]+S[左-1][上-1]
class NumMatrix {
private:
    vector<vector<int>> mx;
    vector<vector<int>> s;
    int m, n;
public:
    NumMatrix(vector<vector<int>>& matrix) {
        mx = matrix;
        s = mx;
        m = mx.size();
        n = m > 0 ? mx[0].size() : 0;
        s[0][0] = mx[0][0];
        for (int i = 1; i < m; i++)
        {
            s[i][0] = s[i - 1][0] + mx[i][0];
        }
        for (int j = 1; j < n; j++)
        {
            s[0][j] = s[0][j - 1] + mx[0][j];
        }
        for (int i = 1; i < m; i++)
        {
            for (int j = 1; j < n; j++)
            {
                s[i][j] = mx[i][j]+s[i-1][j]+s[i][j-1] - s[i - 1][j - 1];
            }
        }
    }

    void update(int row, int col, int val) {
        int diff = val - mx[row][col];
        mx[row][col] = val;
        for (int i = row; i < m; i++)
        {
            for (int j = col; j < n; j++)
            {
                s[i][j] += diff;
            }
        }
    }

    int sumRegion(int row1, int col1, int row2, int col2) {
        int rmin = min(row1, row2)-1;
        int rmax = max(row1, row2);
        int cmin = min(col1, col2)-1;
        int cmax = max(col1, col2);
        int sum = s[rmax][cmax] - (cmin<0?0:s[rmax][cmin]) - (rmin<0?0:s[rmin][cmax]) + ((rmin<0||cmin<0)?0:s[rmin][cmin]);
        return sum;
    }
};
//设计题
//直接朴素实现,低效
//class NumMatrix {
//private:
//    vector<vector<int>> mx;
//    int m, n;
//public:
//    NumMatrix(vector<vector<int>>& matrix) {
//        mx = matrix;
//        m = mx.size();
//        if (m > 0)
//            n = mx[0].size();
//        else
//            n = 0;
//    }
//
//    void update(int row, int col, int val) {
//        mx[row][col] = val;
//    }
//
//    int sumRegion(int row1, int col1, int row2, int col2) {
//        int sum = 0;
//        for (int i = row1; i < row2 + 1; i++)
//        {
//            for (int j = col1; j < col2 + 1; j++)
//            {
//                sum += mx[i][j];
//            }
//        }
//        return sum;
//    }
//};