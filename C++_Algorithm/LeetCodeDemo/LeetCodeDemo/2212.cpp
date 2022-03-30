using namespace std;
#include <vector>
//枚举
//枚举选择结果情况筛选结果
class Solution {
public:
    vector<int> maximumBobPoints(int n, vector<int>& a) {
        vector<int> f(n, 0);
        int len = a.size();
        vector<int> rst(len, 0);
        int num,mx=0,cnt=0,plan;
        for (int i = 1; i < (1<<12); i++) {
            num = 0;
            cnt = 0;
            for (int j = 0; j < 12; j++) {
                if (i >> j & 1) {
                    num += a[j] + 1;
                    cnt += j;
                }
            }
            if (num<=n&&cnt > mx) {
                plan = i;
                mx = cnt;
            }
        }
        for (int i = 0; i < 12; i++) {
            if (plan >> i & 1) {
                rst[i]= a[i] + 1;
                n -= a[i] + 1;
            }
        }
        if (n > 0) {
            rst[0] += n;
        }
        return rst;
    }
};