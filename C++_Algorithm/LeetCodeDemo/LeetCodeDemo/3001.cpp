using namespace std;
#include <iostream>
#include <vector>
#include <string>
#include <numeric>
#include <algorithm>
#include <unordered_set>
#include <unordered_map>
#include <set>
#include <map>
#include <queue>
#include <stack>
#include <functional>
#include <bitset>
typedef pair<int, bool> pib;
typedef pair<int, int> pii;
typedef long long ll;
typedef pair<ll, ll> pll;
typedef pair<ll, int> pli;
#define MAXN (int)(1e5+1)
#define MAXM (int)(1e5+1)
#define MOD (int)(1e9+7)

class Solution {
    bool check(int& x, int& y) {
        return x >= 1 && x <= 8 && y >= 1 && y <= 8;
    }
public:
    int minMovesToCaptureTheQueen(int a, int b, int c, int d, int e, int f) {
        int dir[8][2] = { {1,0},{0,1},{-1,0},{0,-1},{1,1},{1,-1},{-1,1},{-1,-1} };
        int x, y;
        int i = 0;
        for (; i < 4; i++) {
            for (int j = 0;; j++) {
                x = e + j * dir[i][0];
                y = f + j * dir[i][1];
                if (!check(x, y)||x==c&&y==d)
                    break;
                if (x == a && y == b)
                    return 1;
            }
        }
        for (; i < 8; i++) {
            for (int j = 0;; j++) {
                x = e + j * dir[i][0];
                y = f + j * dir[i][1];
                if (!check(x, y) || x == a && y == b)
                    break;
                if (x == c && y == d)
                    return 1;
            }

        }
        return 2;
    }
};