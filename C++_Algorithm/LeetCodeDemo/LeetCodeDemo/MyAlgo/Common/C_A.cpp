using namespace std;
#include <iostream>
typedef long long ll;
class C_A {
public:
    //组合数
    //n个东西选m个
    ll C(int n, int m) {
        long long res = 1;
        m = min(m, n - m);
        for (int i = 0; i < m; ++i) {
            res *= (n - i);
            res /= (i + 1);
        }
        return res;
    }
    //排列数
    //n个东西选m个且可交换顺序算不同的
    ll A(int n, int m) {
        long long res = 1;
        for (int i = 0; i < m; ++i) {
            res *= (n - i);
        }
        return res;
    }
};