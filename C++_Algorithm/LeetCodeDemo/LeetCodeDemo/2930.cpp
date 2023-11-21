using namespace std;
//容斥原理
//不包含a或b或c的集合数set(a|b|c)=set(a)+set(b)+set(c)+set(a&b&c)-set(a&b)-set(a&c)-set(b&c);
class Solution {
typedef long long ll;
#define MOD (int)(1e9+7)
ll p(ll x, ll y,int n) {
    for (int i = 0; i < n; i++) {
        x = (x * y) % MOD;
    }
    return x;
}
public:
    int stringCount(int n) {
        if (n < 4)
            return 0;
        ll total = p(1, 26, n);
        ll noE = p(25 + n, 25, n - 1), noL = p(1, 25, n), noT = p(1, 25, n), noLET = p(23 + n, 23, n - 1);
        ll noLT = p(1, 24, n), noLE= p(24 + n, 24, n - 1), noET=p(24 + n, 24, n - 1);

        return ((total - noE - noL - noT + noLT + noLE + noET - noLET) % MOD + MOD) % MOD;
    }
};