using namespace std;
#include <iostream>
//简单模拟
class Solution {
public:
    string largestGoodInteger(string num) {
        bool flag=num[0]==num[1];
        string rst="",tmp;
        for (int i = 2; i < num.size();i++) {
            if (num[i] == num[i - 1]) {
                if (flag) {
                    tmp = string(3, num[i]);
                    if (tmp > rst) {
                        rst = tmp;
                    }
                }
                flag = 1;
            }
            else {
                flag = 0;
            }
        }
        return rst;
    }
};