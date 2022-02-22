using namespace std;
#include <vector>
//朴素模拟
//能被3整除，就直接搞，不能就空
class Solution {
public:
    vector<long long> sumOfThree(long long num) {
        vector<long long> rst;
        if(num % 3==0)
            rst={ num / 3 - 1, num / 3, num / 3 + 1 };
        return rst;
    }
};