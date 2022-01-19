using namespace std;
#include <iostream>
#include <vector>
//回溯 深搜
class Solution {
private:
    int dir[4][2] = { {1,0},{0,1},{-1,0},{0,-1} };
    int m, n;
    bool dfs(int idx, int x, int y, vector<vector<char>>& board, string& word)
    {
        if (x<0 || x>m - 1 || y<0 || y>n - 1)return false;
        if (word[idx] != board[x][y])return false;
        char back= board[x][y];
        board[x][y] = '#';
        if (idx >= word.size()-1)return true;
        for (int i = 0; i < 4; i++)
        {
            if (dfs(idx + 1, x + dir[i][0], y + dir[i][1], board, word))
                return true;
        }
        board[x][y] = back;
        return false;
    }
public:
    bool exist(vector<vector<char>>& board, string word) {
        m = board.size();
        n= board[0].size();
        for(int i=0;i<m;i++)
            for (int j = 0; j < n; j++) {
                if (dfs(0, i, j, board, word))
                    return true;
            }
        return false;
    }
};