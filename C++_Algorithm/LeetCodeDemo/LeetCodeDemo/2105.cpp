using namespace std;
#include <vector>
//双指针 简单题 朴素实现
class Solution {
public:
    int minimumRefill(vector<int>& p, int ca, int cb) {
        int n = p.size();
        int i,j;
        int a=ca, b=cb;
        int rst = 0;
        for (i = 0; i < n/2; i++) {
            j = n - 1 - i;
            if (p[i] > a) {
                rst++;
                a = ca;
            }
            if (p[j] > b) {
                rst++;
                b = cb;
            }
            a -= p[i];
            b -= p[j];
        }
        if (n % 2) {
            a = max(a, b);
            if (p[i] > a)
            {
                rst++;
            }
        }
        return rst;
    }
};