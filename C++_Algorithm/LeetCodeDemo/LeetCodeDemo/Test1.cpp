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
//简单模拟 字符串处理
//子串作除数能整除原串的可能性数
class Solution {
public:
    int divisorSubstrings(int num, int k) {
        string s = to_string(num);
        int n = s.size(),subnum;
        string subs;
        int rst = 0;
        for (int i = 0; i + k <= n; i++) {
            subs = s.substr(i, k);
            subnum = stoi(subs);
            if (!subnum)continue;
            if (num % subnum == 0)rst++;
        }
        return rst;
    }
};