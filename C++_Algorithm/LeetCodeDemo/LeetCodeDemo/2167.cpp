using namespace std;
#include <iostream>
//动态规划 贪心 找规律 数学推算 巧思
//从左过来到i的累计代价i+1
//从右过来到j累计代价n-j
//拿i,j之间的代价(idx[j]-idx[i]-1)*2
//拿光全部1用的代价=i+1+n-j+(idx[j]-idx[i]-1)*2=(i-2*idx[i])+ (2*idx[j]-j)+n-1;
//某一个位置i使(i-2*idx[i])最小，跟后面某一个位置j使(2*idx[j]-j)最小，二者之和+n-1就是答案。
class Solution {
public:
    int minimumTime(string s) {
        int n = s.size();
        int ans = n;
        int cnt = 0, mn = 0;
        for (int i = 0; i < n; i++) {
            mn = min(mn, i - 2 * cnt);
            cnt += (s[i] - '0');
            ans = min(ans, mn + 2 * cnt - i);
        }
        return min(ans + n - 1, n);
    }
};