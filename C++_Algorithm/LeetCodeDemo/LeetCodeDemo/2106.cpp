using namespace std;
#include <vector>
#include <algorithm>
#include <functional>
//巧思 两分 动态规划(前缀和)
//思路：遍历左右的边界,先左后右，或先右后左，得到一个区间，然后用 前缀和的差 获取区间和，取最大区间和。
//状态量f[i][j]，先到i框后再到j框的果子数。
class Solution {
public:
    int maxTotalFruits(vector<vector<int>>& fruits, int startPos, int k) {
        int rst = 0;
        int n = fruits.size();
        vector<int> sums(n + 1, 0);
        sums[0] = 0;
        for (int i = 1; i <= n; i++) {
            sums[i] = sums[i - 1] + fruits[i - 1][1];
        }
        int l, r;
        int lidx, ridx;
        l = startPos;
        r = startPos + k;
        //lidx = 0;
        //ridx = n - 1;
        //while (lidx < n && fruits[lidx][0] < l)lidx++;
        //while (ridx > -1 && r < fruits[ridx][0])ridx--;
        lidx=lower_bound(fruits.begin(), fruits.end(), vector<int>{l, 0}) - fruits.begin();
        ridx = upper_bound(fruits.begin(), fruits.end(), vector<int>{r, INT32_MAX}) - fruits.begin();
        ridx--;
        for (int i=0; i <= k; i++) {
            l = startPos - i;
            r = startPos+k - 2 * i;
            while (lidx > 0 &&fruits[lidx-1][0] >= l)lidx--;
            while (ridx > -1 && r<fruits[ridx][0] )ridx--;
            if (lidx >= n || ridx < 0) continue;
            rst = max(sums[ridx + 1] - sums[lidx], rst);
        }
        l = startPos - k;
        r = startPos;
        //lidx = 0;
        //ridx = n - 1;
        //while (lidx < n && fruits[lidx][0] < l)lidx++;
        //while (ridx > -1 && r < fruits[ridx][0])ridx--;
        lidx = lower_bound(fruits.begin(), fruits.end(), vector<int>{l, 0}) - fruits.begin();
        ridx = upper_bound(fruits.begin(), fruits.end(), vector<int>{r, INT32_MAX}) - fruits.begin();
        ridx--;
        for (int i=0; i <= k; i++) {
            l = startPos - k + 2 * i;
            r = startPos + i;
            while (lidx <n && fruits[lidx][0] < l)lidx++;
            while (ridx < n-1 && fruits[ridx+1][0]<=r)ridx++;
            if (ridx >= n || lidx < 0) continue;
            rst = max(sums[ridx + 1] - sums[lidx], rst);
        }
        return rst;
    }
};
//[[0, 1], [2, 1], [3, 1]]
//2
//4
//超时 两分 巧思 深搜 回溯 记忆化搜索 剪枝
//特征：最多折返一次
//方案：先两分查找定位起点位于哪个或哪两个水果筐的相对位置，然后从该位置左右分别进行回溯，限制只能转一次方向。
//class Solution {
//public:
//    int maxTotalFruits(vector<vector<int>>& fruits, int startPos, int k) {
//        int rst = 0;
//        int n = fruits.size();
//        function<void(int, int, int,bool, bool)> dfs = [&](int idx, int steps, int get, bool turn,bool changedturn) {
//            int tmp = fruits[idx][1];
//            if (steps == k) {
//                rst = max(get + tmp, rst);
//                return;
//            }
//            if (steps > k) {
//                rst = max(get, rst);
//                return;
//            }
//            get += tmp;
//            fruits[idx][1] = 0;
//            int l = 0, r = idx - 1,mid;
//            if (!turn || turn && !changedturn) {
//                if (!turn&&r>-1)dfs(r, steps + fruits[idx][0] - fruits[r][0], get, 0, changedturn);
//                if (turn && !changedturn){
//                    while (l < r) {
//                        mid = (l + r + 1) / 2;
//                        if (fruits[mid][1])
//                            l = mid;
//                        else
//                            r = mid - 1;
//                    }
//                    if (fruits[l][1])
//                        dfs(l, steps + fruits[idx][0] - fruits[l][0], get, 0, 1);
//                }
//            }
//            if (turn || !turn && !changedturn) {
//                l = idx + 1, r = n - 1;
//                if (turn&&l<n)dfs(l, steps + fruits[l][0] - fruits[idx][0], get, 1, changedturn);
//                if (!turn && !changedturn){
//                    while (l < r) {
//                        mid = (l + r) / 2;
//                        if (!fruits[mid][1])
//                            l = mid + 1;
//                        else
//                            r = mid;
//                    }
//                    if (r < n && fruits[r][1]) 
//                        dfs(r, steps + fruits[r][0] - fruits[idx][0], get, 1, 1);
//                
//                }
//            }
//            fruits[idx][1] = tmp;
//            rst = max(get, rst);
//        };
//        int idx = lower_bound(fruits.begin(), fruits.end(), vector<int>{startPos, 0}) - fruits.begin();
//        int val=0,t;
//        int left = idx-1,right=idx;
//        if (idx < n&&fruits[idx][0] == startPos && idx < n - 1)val += fruits[idx][1], right++, t = fruits[idx][1], fruits[idx][1] = 0;
//        if (left >= 0)dfs(left, startPos - fruits[left][0], val,0,0);
//        if (right < n) {
//            dfs(right, fruits[right][0] - startPos, val, 1, 0);
//            
//        }
//        return rst;
//    }
//};