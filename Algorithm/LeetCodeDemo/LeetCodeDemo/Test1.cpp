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
//简单题 朴素模拟
class Solution {
public:
    vector<string> divideString(string s, int k, char fill) {
        vector<string> rst;
        string tmp;
        int n = s.size();
        int x = ceil(n * 1.0 / k)*k;
        for (int i = 0; i < x; i++) {
            if (i < n)tmp.push_back(s[i]);
            else tmp.push_back(fill);
            if (i % k == k - 1) {
                rst.push_back(tmp);
                tmp.clear();
            }
        }
        return rst;
    }
};