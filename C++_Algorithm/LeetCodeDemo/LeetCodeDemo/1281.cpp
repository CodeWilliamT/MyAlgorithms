//¼òµ¥Ä£Äâ
class Solution {
public:
    int subtractProductAndSum(int n) {
        int product = n%10, sum = n % 10;
        n /= 10;
        while (n) {
            product *= n % 10;
            sum += n % 10;
            n /= 10;
        }
        return product - sum;
    }
};