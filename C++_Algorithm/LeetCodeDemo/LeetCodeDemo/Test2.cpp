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
//枚举
class Solution {
public:
    int countLatticePoints(vector<vector<int>>& circles) {
        int rst = 0;
        for (int x = 0; x <= 200;x++) {
            for (int y= 0; y <= 200; y++) {
                for (auto& e : circles) {
                    if (pow(x - e[0], 2) + pow(y - e[1], 2) <= pow(e[2], 2)) {
                        rst++;
                        break;
                    }
                }
            }
        }
        return rst;
    }
};