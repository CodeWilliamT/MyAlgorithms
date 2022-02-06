using namespace std;
#include <vector>
//简单题 哈希
class Solution {
public:
    int sumOfUnique(vector<int>& nums) {
        short v[101]{};
        for (int& e : nums) {
            v[e]++;
        }
        int rst = 0;
        for (int i = 0; i < 101;i++) {
            if (v[i] == 1) {
                rst += i;
            }
        }
        return rst;
    }
};