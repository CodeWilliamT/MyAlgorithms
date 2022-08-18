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
#include <bitset>
#include <fstream>
//回溯 穷举 数位穷举构造
//特殊=字符不能重
//2e9，则需要n以下的复杂度。
//按位构造,分情况讨论。
//当长度小于len；当情况=len
typedef long long ll;
class Solution {
public:
    int countSpecialNumbers(int n) {
        string s = to_string(n);
        ll tmp = 1;
        int len = s.size();
        int rst = 0;
        if (len - 1 > 0)tmp = 9;
        for (int i = 0; i < len-1; i++) {
            rst += tmp;
            if(i!=len-2)tmp *= 9-i;
        }
        tmp = tmp / 9 * 10;
        for (int i = 0; i < len; i++) {
            if (s[i] - '1' > 0) {
                    rst += tmp * (s[i] - '1');
            }
            tmp /= 10 - i;
        }
        return rst;
    }
};