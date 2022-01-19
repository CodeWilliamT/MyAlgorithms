using namespace std;
#include <string>
//简单题 模拟
class Solution {
public:
    bool isSameAfterReversals(int num) {
        string s = to_string(num);
        reverse(s.begin(), s.end());
        int rev = stoi(s);
        s= to_string(rev);
        reverse(s.begin(), s.end());
        rev = stoi(s);
        return rev == num;
    }
};