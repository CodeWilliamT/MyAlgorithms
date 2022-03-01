using namespace std;
#include <iostream>
#include <vector>
//模拟
class Solution {
public:
    string convert(string s, int numRows) {
        if (numRows == 1)return s;
        string rst;
        vector<string> vs(numRows);
        int tmp;
        for (int i = 0; i < s.size(); i++) {
            tmp = i % (numRows * 2 - 2);
            if (tmp < numRows) {
                vs[tmp].push_back(s[i]);
            }
            else {
                vs[numRows * 2 - 2 - tmp].push_back(s[i]);
            }
        }
        for (auto& e : vs) {
            for (auto& c : e) {
                rst.push_back(c);
            }
        }
        return rst;
    }
};