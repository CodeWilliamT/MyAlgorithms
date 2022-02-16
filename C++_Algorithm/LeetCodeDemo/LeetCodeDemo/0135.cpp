using namespace std;
#include <vector>
//动态规划
//找山峰，左右各来一遍
class Solution {
public:
    int candy(vector<int>& ratings) {
        int n = ratings.size();
        if (n == 1)return 1;
        vector<int> d(n, 1);
        int cnt = 1;
        for (int i = 1; i < n; i++) {
            if (ratings[i] > ratings[i - 1])
                cnt++;
            else
                cnt = 1;
            d[i] = cnt;
        }
        cnt = 1;
        int rst = d[n - 1];
        for (int i = n - 2; i >= 0; i--) {
            if (ratings[i] > ratings[i + 1])
                cnt++;
            else
                cnt = 1;
            d[i] = max(d[i], cnt);
            rst += d[i];
        }
        return rst;
    }
};