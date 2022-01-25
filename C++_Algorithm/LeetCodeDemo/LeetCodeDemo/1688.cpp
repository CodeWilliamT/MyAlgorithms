//模拟 简单题
class Solution {
public:
    int numberOfMatches(int n) {
        int rst = 0;
        while (n != 1) {
            rst += n / 2;
            n = n / 2 + n % 2;
        }
        return rst;
    }
};