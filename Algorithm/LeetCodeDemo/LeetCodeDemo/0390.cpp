//巧思
//考虑头尾变化
//单数次s += (1 << (i - 1));e -= (e - s) % (1 << i);
//双数次e -= (1 << (i - 1));s += (e - s) % (1 << i);
class Solution {
public:
    int lastRemaining(int n) {
        int e = n, s = 1, i = 1;
        while (s < e) {
            if (i % 2) {
                s += (1 << (i - 1));
                e -= (e - s) % (1 << i);
            }
            else {
                e -= (1 << (i - 1));
                s += (e - s) % (1 << i);
            }
            i++;
        }
        return s;
    }
};