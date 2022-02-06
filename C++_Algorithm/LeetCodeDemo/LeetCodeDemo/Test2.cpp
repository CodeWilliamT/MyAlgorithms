using namespace std;
#include <iostream>
#include <vector>
#include <string>
#include <algorithm>
#include <unordered_set>
#include <unordered_map>
#include <set>
#include <map>
#include <queue>
#include <stack>
#include <functional>
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