using namespace std;
#include <vector>
//两分
//两分找合适的糖果数目
class Solution {
public:
    int maximumCandies(vector<int>& c, long long k) {
        sort(c.begin(), c.end());
        long long sum = 0;
        for (auto& e : c) {
            sum += (long long)e;
        }
        long long l = 0, r = sum / k,m;
        while (l < r) {
            m = (l + r + 1) / 2;
            if (check(c,k,m))
                l = m;
            else
                r = m - 1;
        }
        return l;
    }
    bool check(vector<int>& c,long long k, long long num) {
        int n = c.size();
        long long rst = 0;
        for (int i = n - 1; i >= 0; i--) {
            rst += (long long)c[i] / num;
            if (rst >= k) {
                return true;
            }
        }
        return false;
    }
};