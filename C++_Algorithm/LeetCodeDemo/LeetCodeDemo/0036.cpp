using namespace std;
#include <vector>
//哈希 数独
class Solution {
public:
    bool isValidSudoku(vector<vector<char>>& b) {
        bool rows[9][10]{}, cols[9][10]{}, sqs[9][10]{};
        int num,r,c,idx;
        for (int i = 0; i < b.size(); i++) {
            for (int j = 0; j < b[i].size(); j++) {
                if (b[i][j] == '.')continue;
                num = b[i][j] - '0';
                idx = i / 3 * 3 + j / 3;
                if (rows[i][num] || cols[j][num]||sqs[idx][num]) {
                    return false;
                }
                rows[i][num] = cols[j][num] = sqs[idx][num] = 1;
            }
        }
        return true;
    }
};