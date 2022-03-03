using namespace std;
#include <string>
//简单 模拟
class Solution {
public:
    int addDigits(int num) {
        string s;
        while (num > 9) {
            s = to_string(num);
            num = 0;
            for (char& c : s) {
                num += c - '0';
            }
        }
        return num;
    }
};
//找规律
//看题解的数学推导
class Solution {
public:
    int addDigits(int num) {
        return (num - 1) % 9 + 1;
    }
};