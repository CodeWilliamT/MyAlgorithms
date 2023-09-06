using namespace std;
#include <vector>
typedef pair<int, int> pii;
//����
//ȫ����ͣ����Ч��,ÿ����״̬һ����
//�Ŷ����ʱ��=����ö��ʱ�䣬ǡ��ȫ�����ʱ�䡣
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