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

//模拟 数学
//target每个字符 在s跟target中出现的次数的最小比值。计算
class Solution {
public:
    int rearrangeCharacters(string s, string target) {
        int cnts[26]{}, cntt[26]{};
        for (auto& e : s) {
            cnts[e - 'a']++;
        }
        for (auto& e : target) {
            if(!cnts[e - 'a'])
                return 0;
            cntt[e - 'a']++;
        }
        int rst = 100;
        for (int i = 0; i < 26; i++) {
            if (!cntt[i])continue;
            rst = min(rst, cnts[i] / cntt[i]);
        }
        return rst;
    }
};