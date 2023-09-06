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
typedef long long ll;
typedef pair<ll, ll> pll;
typedef pair<int, int> pii;
//简单模拟
class Solution {
public:
    int countSymmetricIntegers(int low, int high) {
        string s;
        int n,delta;
        int rst=0;
        for (int i = low; i <= high; i++) {
            s = to_string(i);
            n = s.size();
            if (n % 2)continue;
            delta = 0;
            for (int j = 0; j < n/2; j++) {
                delta += s[j];
                delta -= s[n/2+j];
            }
            if (!delta)rst++;
        }
        return rst;
    }
};