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
//两分查找
//先排序
//然后二分枚举相邻两球之间的最小间距，只需要统计满足当前最小间距下能放下多少个小球，记为 cnt，若 cnt >= m，说明此间距符合条件，则对大值域二分，否则小值域二分。
//最终找到符合条件的最大间距。

class Solution {
public:
    int maxDistance(vector<int>& p, int m) {
        sort(p.begin(), p.end());
        int n = p.size();
        int ans = p[n - 1] - p[0];
        //间距的最小值，最大值
        int l = 1, r = p[n - 1]-p[0],mid;
        while (l<r){
            mid = (l + r+1) / 2;
            if (check(mid, p, m))
                l = mid;
            else
                r = mid - 1;
        }
        return l;
        
    }
    bool check(int val,vector<int>& p, int m){
        int cnt = 1;
        int pre = p[0];
        int n = p.size();
        for (int i = 1; i < n; i++){
            if (p[i] - pre >= val) {
                cnt++;
                pre = p[i];
            }
        }
        return cnt >= m;
    }
};