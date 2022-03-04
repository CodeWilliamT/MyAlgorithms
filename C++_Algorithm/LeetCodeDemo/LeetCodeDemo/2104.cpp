using namespace std;
#include <vector>
//动态规划
class Solution {
public:
    long long subArrayRanges(vector<int>& a) {
        int n = a.size();
        long long rst = 0;
        int mx, mn;
        for (int i = 0; i < n; i++) {
            mx = a[i];
            mn = a[i];
            for (int j = i + 1; j < n; j++) {
                mx = max(mx, a[j]);
                mn = min(mn, a[j]);
                rst += mx - mn;
            }
        }
        return rst;
    }
};