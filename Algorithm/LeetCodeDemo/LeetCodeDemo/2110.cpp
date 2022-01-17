using namespace std;
#include <vector>
//双指针
//9 + ... + 1;
//(j - i) + .. + 1;
//(j - i + 1)* (j - i) / 2;
class Solution {
public:
    long long getDescentPeriods(vector<int>& prices) {
        int n = prices.size();
        long long rst = 0;
        for (long long i = 0, j = 0; i < n;) {
            j = i;
            while (j-i==0||j < n && prices[j] + 1 == prices[j - 1]) {
                j++;
            }
            rst+=(j - i + 1) * (j - i) / 2;
            i = j;
        }
        return rst;
    }
};