using namespace std;
#include <string>
//哈希 找规律 模拟
//记录数字出现的次数
//正的第一位尝试用1，其他位从0开始用，低位到高位
//负的第一位尝试用9，高位到低位
class Solution {
public:
    long long smallestNumber(long long num) {
        int v[10]{},len;
        bool sign;
        string s = to_string(num);
        if (s[0] == '0')return 0;
        sign = s[0] == '-';
        len = s.size();
        for (int i = sign; i < len;i++) {
            v[s[i] - '0']++;
        }
        int digit = sign?9:1;
        for (int i = sign; i < len; i++) {
            while (!v[digit])sign?digit--:digit++;
            s[i] = digit + '0';
            v[digit]--;
            if (i == 0) {
                digit = 0;
            }
        }
        num = stoll(s);
        return num;
    }
};