//位运算
//变为二进制，找不同
class Solution {
public:
    int minBitFlips(int start, int goal) {
        int rst = 0;
        while (start || goal) {
            rst+=(start & 1) ^ (goal & 1);
            start = start >> 1;
            goal = goal >> 1;
        }
        return rst;
    }
};