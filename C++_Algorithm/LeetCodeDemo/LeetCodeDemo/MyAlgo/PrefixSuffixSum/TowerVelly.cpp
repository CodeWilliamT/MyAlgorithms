using namespace std;
#include <iostream>
#include <vector>
#include <string>
#include <algorithm>
#include <unordered_set>
#include <unordered_map>
#include <set>
#include <map>
#include <queue>
#include <stack>
#include <functional>
#include <bitset>
#include "myAlgo\Structs\TreeNode.cpp"
typedef pair<int, bool> pib;
//前缀和 后缀和
//给出以数组作为最高高度，构建选一索引作为山峰，构建山峰数组，求高度和的最大值
class TowerVelly {
public:
    long long maximumSumOfHeights(vector<int>& mh) {
        typedef pair<int, int> pii;
        typedef long long ll;
        int n = mh.size();
        vector<ll> lsum(n, 0), rsum(n, 0);//山峰在i的左侧和，右侧和
        vector<pii> lv, rv;
        pii x;
        int idx;
        lsum[0] = mh[0];
        lv.emplace_back(mh[0], 0);
        for (int i = 1; i < n; i++) {
            if (mh[i - 1] <= mh[i]) {
                lsum[i] = lsum[i - 1] + mh[i];
                lv.emplace_back(mh[i], i);
            }
            else {
                while (!lv.empty() && lv.back().first > mh[i]) {
                    x = lv.back();
                    lv.pop_back();
                }
                idx = x.second;
                lv.emplace_back(mh[i], idx);
                lsum[i] = ll(idx > 0 ? lsum[idx - 1] : 0) + (ll)mh[i] * (i - idx+1);

            }
        }
        rsum[n - 1] = mh[n - 1];
        rv.emplace_back(mh[n - 1], n - 1);
        for (int i = n - 2; i > -1; i--) {
            if (mh[i + 1] <= mh[i]) {
                rsum[i] = rsum[i + 1] + mh[i];
                rv.emplace_back(mh[i], i);
            }
            else {
                x = rv.back();
                while (!rv.empty() && rv.back().first > mh[i]) {
                    x = rv.back();
                    rv.pop_back();
                }
                idx = x.second;
                rv.emplace_back(mh[i], idx);
                rsum[i] = ll(idx < n - 1 ? rsum[idx + 1] : 0) + (ll)mh[i] * (idx - i+1);

            }
        }

        ll rst = 0;
        for (int i = 0; i < n; i++) {
            rst = max(rst, lsum[i] +rsum[i]-  mh[i]);
        }
        return rst;
    }
};