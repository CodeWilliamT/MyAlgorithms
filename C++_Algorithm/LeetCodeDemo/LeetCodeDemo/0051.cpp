using namespace std;
#include <iostream>
#include <vector>
#include <functional>
//回溯 位运算
//遍历行，判定列左上斜，右上斜是否有Q。
class Solution {
public:
    vector<vector<string>> solveNQueens(int n) {
        vector<vector<string>> rst;
        unsigned short v=0;
        unsigned short l = 0,r=0;
        vector<string> tmp;
        function<void(int)> dfs=[&](int row) {
            if (row == n) {
                rst.push_back(tmp);
                return;
            }
            string s(n,'.');
            for (int i = 0; i < n; i++) {
                if (!(v>>i&1)&&!(l>>row+i&1)&&!(r>>n-1-row+i&1)){
                    v += 1 << i;
                    l += 1 << (row + i);
                    r += 1 << (n - 1 - row + i);
                    s[i] = 'Q';
                    tmp.push_back(s);
                    dfs(row + 1);
                    tmp.pop_back();
                    s[i] = '.';
                    v -= 1 << i;
                    l -= 1 << row + i;
                    r -= 1 << n - 1 - row + i;
                }
            }
        };
        dfs(0);
        return rst;
    }
};