using namespace std;
#include <iostream>
//找规律 贪心
//尽量多用最多的那个，重复过多了就换次长的那个。
class Solution {
public:
    string longestDiverseString(int a, int b, int c) {
        int mx, repeats[3]{};
        string s;
        while (a || b || c) {
            mx = max(a, max(b, c));
            if (a == mx) {
                if (repeats[0] < 2) {
                _a:
                    s += "a";
                    a--;
                    repeats[0]++;
                    repeats[1] = 0;
                    repeats[2] = 0;
                }
                else if (b >= c) {
                    if (!b)return s;
                    goto _b;
                }
                else {
                    goto _c;
                }
            }
            else if (b == mx) {
                if (repeats[1] < 2) {
                _b:
                    s += "b";
                    b--;
                    repeats[0] = 0;
                    repeats[1]++;
                    repeats[2] = 0;
                }
                else if (a >= c) {
                    if (!a)return s;
                    goto _a;
                }
                else {
                    goto _c;
                }
            }
            else if (c == mx) {
                if (repeats[2] < 2) {
                _c:
                    s += "c";
                    c--;
                    repeats[0] = 0;
                    repeats[1]=0;
                    repeats[2]++;
                }
                else if (a >= b) {
                    if (!a)return s;
                    goto _a;
                }
                else {
                    goto _b;
                }
            }
            else {
                return s;
            }
        }
        return s;
    }
};