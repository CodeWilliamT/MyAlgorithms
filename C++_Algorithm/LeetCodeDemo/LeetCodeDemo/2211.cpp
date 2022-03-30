using namespace std;
#include <iostream>
#include <vector>
#include <bitset>
//麻烦题 细致条件分析
class Solution {
public:
    int countCollisions(string s) {
        int rst = 0;
        int n = s.size();
        int pre = 0;
        vector<bool> f(n, 0);
        for (int i = 1; i < n; i++) {
            pre = f[i - 1];
            if (pre==0){
                if (s[i - 1] == 'R') {
                    if (s[i] == 'L') {
                        pre = 1;
                    }
                    else if (s[i] == 'S') {
                        pre = 1;
                    }
                    else if (s[i] == 'R') {
                        pre=0;
                    }
                }
                else if (s[i - 1] == 'L' ) {
                    pre = 0;
                }
                else if (s[i - 1] == 'S') {
                    if (s[i] == 'L') {
                        pre = 1;
                    }
                    else {
                        pre = 0;
                    }
                }
            }
            else{
                if (s[i] == 'L') {
                    pre = 1;
                }
                else {
                    pre = 0;
                }
            }
            f[i] = pre;
        }
        for(int i = n-2; i >=0; i--) {
            if(f[i])continue;
            pre = f[i + 1];
            if (pre == 0) {
                if (s[i + 1] == 'L') {
                    if (s[i] == 'R') {
                        pre = 1;
                    }
                    else if (s[i] == 'S') {
                        pre = 1;
                    }
                    else if (s[i] == 'L') {
                        pre = 0;
                    }
                }
                else if (s[i + 1] == 'R') {
                    pre = 0;
                }
                else if (s[i + 1] == 'S') {
                    if (s[i] == 'R') {
                        pre = 1;
                    }
                    else {
                        pre = 0;
                    }
                }
            }
            else {
                if (s[i] == 'R') {
                    pre = 1;
                }
                else {
                    pre = 0;
                }
            }
            f[i] = pre;
        }
        for (int i = 0; i < n; i++) {
            if (f[i]&&s[i]!='S') {
                rst++;
            }
        }
        return rst;
    }
};