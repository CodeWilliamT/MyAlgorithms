using namespace std;
#include <vector>
class Solution {
public:
    int minCount(vector<int>& coins) {
        int rst = 0;
        for (int& e : coins) {
            rst += e / 2 + e % 2;
        }
        return rst;
    }
};