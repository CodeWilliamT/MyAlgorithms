using namespace std;
#include <iostream>
#include <vector>
//模拟
class Solution {
public:
    string convert(string s, int numRows) {
        int n = s.size();
        vector<string>  p(numRows);
        string rst;
        int cnt = 0, pa = cnt + numRows, pb = pa + numRows - 2;
        for (int i = 0; i < n; i++) {
            if (i < pa) {
                p[i - cnt].push_back(s[i]);
            }
            else if (i < pb) {
                p[numRows - 1 - (i - (pa - 1))].push_back(s[i]);
            }
            else {
                cnt = i;
                pa = cnt + numRows, pb = pa + numRows - 2;
                p[i - cnt].push_back(s[i]);
            }
        }
        for (auto& e : p) {
            rst += e;
        }
        return rst;
    }
};