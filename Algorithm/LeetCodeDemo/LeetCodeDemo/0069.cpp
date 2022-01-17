using namespace std;
//两分
//注意极大数
class Solution {
public:
    int mySqrt(int x) {
        int l = 0;
        long long r = x,mid;
        while (l < r)
        {
            mid = (l + r+1) / 2;
            if (mid * mid <= x)
                l = mid;
            else
                r = mid-1;
        }
        return l;
    }
};