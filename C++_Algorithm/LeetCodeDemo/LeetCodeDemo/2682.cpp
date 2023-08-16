using namespace std;
#include <vector>
//¼òµ¥Ä£Äâ
class Solution {
public:
    vector<int> circularGameLosers(int n, int k) {
        int v[51]{};
        v[0] = 1;
        int t = 1, x = k;
        while (!v[x]) {
            v[x] = 1;
            t++;
            x+=k*t;
            if (x >= n)x %=n;
        }
        vector<int> rst;
        for (int i = 0; i < n; i++) {
            if (!v[i])
                rst.push_back(i + 1);
        }
        return rst;
    }
};