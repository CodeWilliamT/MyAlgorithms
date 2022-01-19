using namespace std;
#include <iostream>
#include <vector>
//简单题，脑筋急转弯
//取最小的a,b的乘积
class Solution {
public:
    int maxCount(int m, int n, vector<vector<int>>& ops) {
        int mina = m, minb = n;
        for (auto o : ops) {
            mina = min(mina, o[0]);
            minb = min(minb, o[1]);
        }
        return mina * minb;
    }
};