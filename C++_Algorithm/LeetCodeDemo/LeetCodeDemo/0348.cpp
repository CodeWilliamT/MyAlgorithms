using namespace std;
#include <iostream>
#include <vector>
//设计题
//分为行列斜角计数
class TicTacToe {
private:
    vector<vector<int>> rows, cols, corners;
    int len;
public:
    TicTacToe(int n) {
        len = n;
        rows = vector<vector<int>>(2, vector<int>(n));
        cols = vector<vector<int>>(2, vector<int>(n));
        corners = vector<vector<int>>(2, vector<int>(n));
    }

    int move(int row, int col, int player) {
        rows[player - 1][row]++;
        if (rows[player - 1][row] == len)
            return player;
        cols[player - 1][col]++;
        if (cols[player - 1][col] == len)
            return player;
        if (row == col)corners[player - 1][0]++;
        if (corners[player - 1][0] == len)
            return player;
        if (row == len - 1 - col)corners[player - 1][1]++;
        if (corners[player - 1][1] == len)
            return player;
        return 0;
    }
};