using namespace std;
#include <iostream>
#include <algorithm>
//简单模拟
//奇数偶数从大到小排序
class Solution {
public:
    int largestInteger(int num) {
        string s = to_string(num);
        string odd, even;
        for (char& c : s) {
            if ((c - '0') % 2) {
                odd.push_back(c);
            }
            else {
                even.push_back(c);
            }
        }
        sort(odd.begin(), odd.end());
        sort(even.begin(), even.end());
        for (char& c : s) {
            if ((c - '0') % 2) {
                c = odd.back();
                odd.pop_back();
            }
            else {
                c = even.back();
                even.pop_back();
            }
        }
        return stoi(s);
    }
};