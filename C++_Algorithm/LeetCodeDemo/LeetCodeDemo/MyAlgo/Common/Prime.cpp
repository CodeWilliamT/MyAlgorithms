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
#include <stack>
#include <functional>
#include <bitset>
typedef pair<int, bool> pib;
typedef long long ll;
typedef pair<ll, ll> pll;
typedef pair<int, int> pii;

class  Prime{
public:
    //判断一个数是否为质数
    //O(sqrt(n))
    bool is_prime(int n) {
        bool flag = true;
        for (int i = 2; i <= n / i; i++) {
            if (n % i == 0) {
                flag = false;
                break;
            }
        }
        return flag;
    }
    //返回n以内的质数的判定集合。
    //O(N)
    vector<bool> get_prime(int n) {
        vector<bool> rst(n+1,true);
        rst[1] = false;
        for (int i = 2; i <= n; i++)
            for (int j = i * 2; j <= n; j += i)
                rst[j] = false;
        return rst;
    }

    //分解质因数，返回一个数的质因数，质因数个数的哈希键值对
    //<O(N)
    unordered_map<int,int> getNumberFactor(int x) {
        unordered_map<int, int> rst;
        for (int i = 2; i <= x; i++) {
            while (x % i == 0) {
                x /= i;
                rst[i]++;
            }
        }
        return rst;
    }

    //返回一个数的为奇数个数的质因数的乘积
    //<O(N)
    int core(int x) {
        int rst = 1;
        int cnt;
        for (int i = 2; i <= x; i++) {
            cnt = 0;
            while (x % i == 0) {
                x /= i;
                cnt++;
            }
            if (cnt % 2) {
                rst *= i;
            }
        }
        return rst;
    }
};