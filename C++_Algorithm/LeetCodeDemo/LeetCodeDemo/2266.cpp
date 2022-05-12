using namespace std;
#include <iostream>
#include <vector>
//细致条件分析 找规律 数学
//找出除7跟9外的数字连着出现x次的可能性y的规律,f(x)=f(x-3)*f(3)+f(x-2)*f(2)
//找出7跟9的数字连着出现x次的可能性y的规律,f(x)=f(x-4)*f(4)+f(x-3)*f(3)+f(x-2)*f(2)
//将字符串按相同字符进行划分计算。
typedef long long ll;
typedef pair<ll, ll> pll;
typedef pair<int, int> pii;
class Solution {
public:
    int countTexts(string p) {
        int n = p.size();
        vector<ll> cnt3(n + 3, 0);
        vector<ll> cnt4(n + 3, 0);
        int mod = 1e9 + 7;
        cnt3[0] = cnt4[0] = 1;
        cnt3[1] = cnt4[1] = 1;
        cnt3[2] = cnt4[2] = 2;
        cnt3[3] = cnt4[3] = 4;
        for (int i = 4; i <= n; i++) {
            if(i>4)
                cnt4[i] = (cnt4[i] + cnt4[i - 5]) % mod;
            cnt4[i] = (cnt4[i] + (cnt4[i - 4] * 2) % mod) % mod;
            cnt4[i] = (cnt4[i] + (cnt4[i - 3] * 2) % mod) % mod;
            cnt4[i] = (cnt4[i] + (cnt4[i - 2] * 2) % mod) % mod;
            cnt3[i] = (cnt3[i] + cnt3[i - 4]) % mod;
            cnt3[i] = (cnt3[i] + (cnt3[i - 3] * 2) % mod) % mod;
            cnt3[i] = (cnt3[i] + (cnt3[i - 2] * 2) % mod) % mod;
        }
        ll rst = 1;
        int cnt = 1;
        for (int i = 0; i < n; i++) {
            if (i>=n-1||p[i]!=p[i+1]) {
                if (p[i] == '7' || p[i] == '9') {
                    rst = (rst * cnt4[cnt]) % mod;
                }
                else {
                    rst = (rst * cnt3[cnt]) % mod;
                }
                cnt = 1;
            }
            else {
                cnt++;
            }
        }
        return (int)rst;
    }
};