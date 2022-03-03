using namespace std;
#include <vector>
//朴素实现 简单
//奇数多加个0，然后1,-1这么加
class Solution {
public:
    vector<int> sumZero(int n) {
        vector<int> rst;
        if (n % 2)rst.push_back(0);
        n /= 2;
        for (int i = 1; i<=n; i++) {
            rst.push_back(i);
            rst.push_back(-i);
        }
        return rst;
    }
};