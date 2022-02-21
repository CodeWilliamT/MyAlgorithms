using namespace std;
#include <vector>
//简单题
//唯一路径。
class Solution {
public:
    bool isOneBitCharacter(vector<int>& bits) {
        bool d;
        for (int i = 0; i < bits.size();) {
            d = bits[i];
            i += bits[i] + 1;
        }
        return d == 0;
    }
};