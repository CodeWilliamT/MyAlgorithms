using namespace std;
#include <iostream>
#include <vector>
//动态规划
//找波谷
class Solution {
public:
    vector<int> goodDaysToRobBank(vector<int>& s, int t) {
        bitset<100000> l, r;
        int n = s.size();
        for (int i = 0; i < n - 1; i++) {
            if (s[i] >= s[i + 1]) {
                l[i] = 1;
            }
            if (s[n - 1 - i] >= s[n - 2 - i]) {
                r[n - 1 - i] = 1;
            }
        }
        int left = 0, right = 0;
        for (int i = 0; i < t; i++) {
            left += l[i];
            if (i + t + 1 < n)
                right += r[i + t + 1];
        }
        vector<int> rst;
        for (int i = t; i < n - t; i++) {
            if (left == t && right == t) {
                rst.push_back(i);
            }
            left += l[i];
            left -= l[i - t];
            right += r[i + t + 1];
            right -= r[i + 1];
        }
        return rst;
    }
};