using namespace std;
#include <iostream>
#include <vector>

//求字符串的连续非空子字符串的 不同字符数的总和。
//找规律
//连续子字符串：记录各个字符在什么位置变化。
//答案=各个字符当前位置到前一个位置(没有则为-1)的距离*(n-当前出现位置)的和
class Solution {
public:
    long long appealSum(string s) {
        int n = s.size();
        vector<int> headcnt(26,-1);
        long long rst = 0;
        for (int i = 0; i < n; i++) {
            rst += (i- headcnt[s[i] - 'a']) * (n - i);
            headcnt[s[i] - 'a']=i;
        }
        return rst;
    }
};