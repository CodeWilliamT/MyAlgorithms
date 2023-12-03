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
//分析
//求移除全部线段后，最大正方形面积。
//求移除全部线段后，横纵中，线段距离最大值，较小的平方。
class Solution {
public:
    int maximizeSquareHoleArea(int n, int m, vector<int>& hs, vector<int>& vs) {
        sort(hs.begin(), hs.end());
        sort(vs.begin(), vs.end());
        int cnthmx = 1, cnt=1;
        for (int i = 0; i < hs.size(); i++) {
            if (!i&& hs[i] == 1) {
                continue;
            }
            if (i==0||hs[i] == hs[i - 1]+1) {
                cnt++;
            }
            else {
                cnt = 2;
            }
            cnthmx = max(cnthmx, cnt);
        }
        int cntvmx=1;
        cnt = 1;
        for (int i = 0; i < vs.size(); i++) {
            if (!i && vs[i] == 1) {
                continue;
            }
            if (i == 0 || vs[i] == vs[i - 1]+1) {
                cnt++;
            }
            else {
                cnt = 2;
            }
            cntvmx = max(cntvmx, cnt);
        }
        return min(cnthmx * cnthmx, cntvmx * cntvmx);
    }
};