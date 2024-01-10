using namespace std;
#include <iostream>
class C_A {
#define MOD (int)(1e9+7)
typedef long long ll;
public:
    //�����
    //n������ѡm��
    ll C(int n, int m) {
        ll res = 1;
        m = min(m, n - m);
        for (int i = 0; i < m; ++i) {
            res *= (n - i);
            res = (res/(i + 1))%MOD;
        }
        return res;
    }
    //������
    //n������ѡm���ҿɽ���˳���㲻ͬ��
    ll A(int n, int m) {
        ll res = 1;
        for (int i = 0; i < m; ++i) {
            res =(res*(n - i)) % MOD;
        }
        return res;
    }
};