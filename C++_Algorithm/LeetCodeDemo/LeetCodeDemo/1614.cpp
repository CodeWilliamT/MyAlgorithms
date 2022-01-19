using namespace std;
#include <iostream>
//朴素实现
//一个计数器，一个答案，当遇到(则计数器累加，遇到)则比较刷新答案，计数器累减
class Solution {
public:
    int maxDepth(string s) {
        int cnt = 0,rst = 0;
        for (char& c : s) {
            if (c == '(')cnt++;
            else if (c == ')') {
                rst = max(rst, cnt);
                cnt--;
            }
        }
        return rst;
    }
};