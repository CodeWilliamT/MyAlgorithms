using namespace std;
#include <iostream>
typedef long long ll;
class C_A {
public:
    ll C(int n, int m) {
        long long res = 1;
        m = min(m, n - m);
        for (int i = 0; i < m; ++i) {
            res *= (n - i);
            res /= (i + 1);
        }
        return res;
    }
    ll A(int n, int m) {
        long long res = 1;
        for (int i = 0; i < m; ++i) {
            res *= (n - i);
        }
        return res;
    }
};