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
//麻烦模拟 字符串处理
//字符串格式化
//坑贼多：注意longlong越界,注意价格需要左右是边界或空格。
class Solution {
public:
    string discountPrices(string s, int discount) {
        string rst;
        bool isPrice = 0,isSplit=1;
        string tmp;
        char tmps[15];
        int len;
        for (auto& c : s) {
            if (c >= '0' && c <= '9' && isPrice) {
                tmp.push_back(c);
            }
            else {
                if (tmp.size() > 0) {
                    if (c == ' '){
                        len = to_string(stoll(tmp) * (100 - discount) / 100).size() + 4;
                        snprintf(tmps, len, "%.2f", 1.0 * stoll(tmp) * (100 - discount) / 100);
                        rst += tmps;
                        tmp.clear();
                    }
                    else{
                        rst += tmp;
                        tmp.clear();
                    }
                }
                if (c == ' ') {
                    isSplit = 1;
                    isPrice = 0;
                }
                else if (c == '$'&& isSplit) {
                    isSplit = 0;
                    isPrice = 1;
                }
                else {
                    isSplit = 0;
                    isPrice = 0;
                }
                rst.push_back(c);
            }
        }
        if (tmp.size() > 0) {
            len = to_string(stoll(tmp) * (100 - discount) / 100).size() + 4;
            snprintf(tmps, len, "%.2f", 1.0 * stoll(tmp) * (100 - discount) / 100);
            rst += tmps;
            tmp.clear();
        }
        return rst;
    }
};