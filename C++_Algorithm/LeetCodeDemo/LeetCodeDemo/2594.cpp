using namespace std;
#include <vector>
typedef pair<int, int> pii;
//两分
//全都不停工，效率,每分钟状态一定。
//团队最短时间=两分枚举时间，恰好全做完的时间。
class Solution {
    typedef long long ll;
public:
    long long repairCars(vector<int>& ranks, int cars) {
        ll mx = 0;
        for (auto& e : ranks) {
            mx= max(mx,1ll*e);
        }
        auto check = [&](ll x) {
            ll num = 0;
            for (auto& e : ranks) {
                num += sqrt(x / e);
            }
            return num>=cars;
        };
        ll l = 0, r = mx *cars * cars;
        ll m;
        while (l < r) {
            m = (l + r) / 2;
            if (check(m))
                r = m;
            else
                l = m + 1;
        }
        return r;
    }
};