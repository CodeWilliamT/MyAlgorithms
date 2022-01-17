//简单题
class Solution {
public:
    bool checkPerfectNumber(int num) {
        int sum = 1;
        for (int i = 2; i <= num/2; i++) {
            sum += (num % i) ? 0 : i;
        }
        return sum == num&& num !=1;
    }
};