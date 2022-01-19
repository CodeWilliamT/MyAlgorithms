using namespace std;
#include <iostream>
#include <vector>
//简单题 细致条件分析
//矩形不相交：1.面积为零 或 2.矩形需要在另一个矩形的至少一侧
class Solution {
public:
    bool isRectangleOverlap(vector<int>& r1, vector<int>& r2) {
        if (r1[0]==r1[2]||r1[1]==r1[3]||r2[0]==r2[2]||r2[1]==r2[3])
        {
            return false;
        }
        if (r1[0]>=r2[2]||r1[2]<=r2[0]||r1[1]>=r2[3]||r1[3]<=r2[1])
        {
            return false;
        }
        return true;
    }
};