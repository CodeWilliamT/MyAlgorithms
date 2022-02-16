using namespace std;
#include <vector>
//简单题
//异或运算
class Solution {
public:
    int singleNumber(vector<int>& nums) {
        int rst = 0;
        for (auto& e : nums) {
            rst = rst ^ e;
        }
        return rst;
    }
};