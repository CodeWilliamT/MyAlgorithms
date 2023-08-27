using namespace std;
#include <iostream>
typedef long long ll;
typedef pair<ll, ll> pll;
typedef pair<int, int> pii;
class Solution {
public:
    int furthestDistanceFromOrigin(string moves) {
        int rst = 0,tmp=0;

        for (char& c : moves) {
            if (c == 'R')tmp++;
            else if (c == 'L')tmp--;
            else rst++;
        }
        return rst + abs(tmp);
    }
};