using namespace std;
#include <iostream>
//两分
class Solution {
public:
    bool isPerfectSquare(int num) {
        long l = 1, r = (1 << 16) - 1;
        long mid;
        long tmp;
        while (l <= r)
        {
            mid = (l + r) / 2;
            tmp = mid * mid;
            if (tmp == num)
            {
                return true;
            }
            else if (tmp < num)
            {
                l = mid + 1;
            }
            else
            {
                r = mid - 1;
            }
        }
        return false;
    }
};