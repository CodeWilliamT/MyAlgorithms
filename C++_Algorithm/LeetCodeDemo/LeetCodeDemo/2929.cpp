using namespace std;
#include <iostream>
//数数
class Solution {
public:
    long long distributeCandies(int n, int limit) {
        long long rst = 0;
        for (int i = 0; i <= n && i <= limit; i++) {//4+
            rst += max(0,1+min(n - i, limit) - max(0,n - limit - i));
        }
        return rst;
    }
};