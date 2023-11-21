using namespace std;

//数数
class Solution {
public:
    int distributeCandies(int n, int limit) {
        int rst = 0;
        for (int i = 0; i <= n&&i<=limit; i++) {
            for (int j = 0; j <= n-i && j<= limit; j++) {
                if (n - limit-i <=j ) {
                    rst++;
                }
            }
        }
        return rst;
    }
};