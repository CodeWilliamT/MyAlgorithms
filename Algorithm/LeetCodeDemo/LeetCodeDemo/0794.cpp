using namespace std;
#include <iostream>
#include <vector>
//细致条件分析
//若无胜者:差值cntx- cnto==0|| cntx - cnto == 1；
//若有胜者:不能都获胜;胜者为0则差值为1;胜者为1则差值为0;
class Solution {
public:
    bool validTicTacToe(vector<string>& b) {
        int cntx = 0, cnto = 0;
        for (auto& v : b) {
            for (char& c : v) {
                if (c == 'X')cntx++;
                else if (c == 'O')cnto++;
            }
        }
        bool win = 0;
        bool winer = 0;
        if (b[1][1] != ' ' && b[0][0] == b[1][1] && b[1][1] == b[2][2])win = 1, winer = b[1][1] == 'O';
        if (b[1][1] != ' ' && b[0][2] == b[1][1] && b[1][1] == b[2][0]) {
            if (win && winer!= (b[1][1] == 'O'))return false;
            win = 1, winer = b[1][1] == 'O';
        }
        for (int i = 0; i < 3; i++)
        {
            if (b[i][i] != ' ' && b[i][0] == b[i][1] && b[i][1] == b[i][2]) {
                if (win && winer != (b[i][i] == 'O'))return false;
                win = 1, winer = b[i][i] == 'O';
            }
            if (b[i][i] != ' ' && b[0][i] == b[1][i] && b[1][i] == b[2][i]) {
                if (win && winer != (b[i][i] == 'O'))return false;
                win = 1, winer = b[i][i] == 'O';
            }
        }
        int delta = cntx - cnto;
        if (win)
            if (!winer && delta == 1 || winer && delta == 0)return true;
            else
                return false;
        return delta == 0 || delta == 1;
    }
};