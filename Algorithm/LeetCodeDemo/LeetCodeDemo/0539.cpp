using namespace std;
#include <iostream>
#include <vector>
#include <string>
//动态规划
//字符串转int后按60*24一个周期，排序后进行找最小差的计算。
//num[i]记为第i时刻的分钟数。
//最小差初值rst=min(num[0]+circle-num[n-1],num[n-1]-num[0]);
//最小差rst=min(rst,min(e-pre,pre+circle-e));
//然后应能不用排序。转为
//动态规划 哈希
//num[i]转为i分钟是否存在时刻。
//最小差初值rst= min(mn + circle - mx, mx - mn);
//最小差rst = min(rst, min(i - pre, pre + circle - i));
class Solution {
public:
    int findMinDifference(vector<string>& timePoints) {
        const int circle = 60 * 24;
        bool num[circle]{};
        int tmp;
        int mn = circle, mx = 0;
        for (auto& s : timePoints) {
            tmp = stoi(s.substr(0,2))*60+ stoi(s.substr(3, 2));
            mn = min(mn, tmp);
            mx = max(mx, tmp);
            if (num[tmp])
                return 0;
            else
                num[tmp] = 1;
        }
        int rst= min(mn + circle - mx, mx - mn);
        int pre = mn;
        for (int i = mn + 1; i < circle; i++) {
            if (num[i]) {
                rst = min(rst, min(i - pre, pre + circle - i));
                pre = i;
            }
        }
        return rst;
    }
};