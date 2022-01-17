using namespace std;
#include <vector>
#include <unordered_set>
//哈希
class Solution {
public:
    int countPoints(string rings) {
        vector<unordered_set<char>> st(10);
        char color;
        for (char& c : rings) {
            if (c >= '0' && c <= '9') {
                st[c - '0'].insert(color);
            }
            else {
                color = c;
            }
        }
        int rst = 0;
        for (auto& e : st) {
            if (e.size() == 3)rst++;
        }
        return rst;
    }
};