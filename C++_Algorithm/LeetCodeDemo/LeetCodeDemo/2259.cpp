using namespace std;
#include <iostream>
//简单模拟
//从左往右，若下个数大于它，那么直接换，否则存着找下一个，找到找不到了，则用存着的那个
class Solution {
public:
    string removeDigit(string number, char digit) {
        int n= number.size();
        int idx = 0;
        for (int i = 0; i < n; i++) {
            if (number[i] == digit)
            {
                if (i < n - 1 && number[i + 1]>number[i]) {
                    number.erase(number.begin() + i);
                    return number;
                }
                idx = i;
            }
        }
        number.erase(number.begin() + idx);
        return number;
    }
};