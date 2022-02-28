using namespace std;
#include <vector>
//枚举
//小数据 共计2^16，枚举约64000个状态 判定是否合法
//判定各个状态时各点的入度出度是否相等
class Solution {
public:
    int maximumRequests(int n, vector<vector<int>>& r) {
        int len = r.size();
        int total = 1 << len;
        int in[21]{};
        int rst=0;
        int cnt = 0;
        bool flag;
        for (int i = 0; i < total; i++) {
            cnt = 0;
            for (int j = 0; j < len; j++) {
                if (i >> j & 1) {
                    in[r[j][0]]++;
                    in[r[j][1]]--;
                    cnt++;
                }
            }
            flag = 1;
            for (int j = 0; j < n; j++) {
                if (in[j]) {
                    flag = 0;
                }
                in[j] = 0;
            }
            if (flag) {
                rst = max(rst, cnt);
            }
        }
        return rst;
    }
};