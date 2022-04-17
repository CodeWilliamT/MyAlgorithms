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
//简单模拟
class Solution {
public:
    string digitSum(string s, int k) {
        string rst=s;
        int num = 0;
        while (s.size() > k) {
            num = 0;
            rst = "";
            for (int i = 0; i < s.size(); i++) {
                num += s[i] - '0';
                if (i % k == k - 1||i==s.size()-1) {
                    rst += to_string(num);
                    num = 0;
                }
            }
            s = rst;
        }
        return rst;
    }
};