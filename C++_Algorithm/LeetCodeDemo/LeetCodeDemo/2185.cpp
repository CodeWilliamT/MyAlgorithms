using namespace std;
#include <iostream>
#include <vector>
//简单 模拟
//直接遍历
class Solution {
public:
    int prefixCount(vector<string>& words, string pref) {
        int rst = words.size();
        for (int i = 0; i < words.size(); i++) {
            if (words[i].size() < pref.size()) {
                rst--; continue;
            }
            for (int j = 0; j < pref.size(); j++) {
                if (words[i][j] != pref[j]) {
                    rst--;
                    break;
                }
            }
        }
        return rst;
    }
};