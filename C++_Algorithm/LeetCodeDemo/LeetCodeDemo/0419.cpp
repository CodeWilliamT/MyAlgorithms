using namespace std;
#include <vector>
//巧思 动态规划 遍历：
//对于x一个位置，如果它的有上家或有左家为x，rst不变，否则rst++，答案为标记大小，
//回溯 深搜：
//找到一个X并且没标记的就对他深搜作rst，rst++，答案为rst。
class Solution {
public:
    int countBattleships(vector<vector<char>>& board) {
        int rst = 0;
        for (int i = 0; i < board.size(); i++) {
            for (int j = 0; j < board[i].size(); j++) {
                if (board[i][j] == 'X')
                    if (i > 0 && board[i - 1][j] == 'X' || j > 0 && board[i][j - 1] == 'X')
                        continue;
                    else
                        rst++;
            }
        }
        return rst;
    }
};