using namespace std;
#include <iostream>
#include <vector>
//简单题
//不是按点来算，是按行列，不用上广搜加动态规划，别想复杂了。。。
class Solution {
public:
    int minCost(vector<int>& s, vector<int>& h, vector<int>& rowCosts, vector<int>& colCosts) {
        int rst = 0;
        int i = s[0];
        while (i != h[0])
        {
            i < h[0] ? i++ : i--;
            rst += rowCosts[i];
        }
        i = s[1];
        while (i != h[1])
        {
            i < h[1] ? i++ : i--;
            rst += colCosts[i];
        }
        return rst;
    }
};