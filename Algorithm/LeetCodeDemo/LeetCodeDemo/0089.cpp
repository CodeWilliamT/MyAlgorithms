using namespace std;
#include <vector>
//找规律 朴素实现
//ret[i] = (i >> 1) ^ i;
class Solution {
public:
    vector<int> grayCode(int n) {
        int len = 1 << n;
        vector<int> ret(len);
        for (int i = 0; i < len; i++) {
            ret[i] = (i >> 1) ^ i;
        }
        return ret;
    }
};