using namespace std;
#include <iostream>
//模拟 细致条件分析
//找到一对RL,或L则开始推算
//然后再补上俩R之间的.
class Solution {
public:
    string pushDominoes(string dominoes) {
        int l, r=0;
        bool hasR=false;
        int n= dominoes.size();
        for (int i = 0; i < n; i++) {
            if (dominoes[i] == '.')continue;
            if (dominoes[i] == 'L') {
                int j=r;
                if(hasR)j=r+1;
                for (int k = i - 1; j <= k; k--) {
                    if (hasR) {
                        if (j != k) {
                            dominoes[j] = dominoes[j - 1];
                            dominoes[k] = dominoes[k + 1];
                        }
                        j++;
                    }
                    else {
                        dominoes[k] = dominoes[k + 1];
                    }
                }
                r = i;
                hasR = 0;
            }
            else if (dominoes[i] == 'R') {
                hasR = 1;
                r = i;
            }
        }
        hasR=0;
        for (int i = 0; i < n; i++) {
            if (dominoes[i] == 'R') {
                hasR = 1;
            }
            else if (dominoes[i] == 'L') {
                hasR = 0;
            }
            if (dominoes[i]=='.'&&hasR && (i == n - 1 || dominoes[i + 1] != 'L')) {
                dominoes[i] = 'R';
            }
        }
        return dominoes;
    }
};