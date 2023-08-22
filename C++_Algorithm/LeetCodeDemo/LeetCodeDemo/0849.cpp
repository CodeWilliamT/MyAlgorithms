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
typedef long long ll;
typedef pair<ll, ll> pll;
typedef pair<int, int> pii;
//简单模拟 找规律
//求数组中为1的索引的 最大差值/2。
class Solution {
public:
    int maxDistToClosest(vector<int>& s) {
        int n = s.size();
        int rst = 0,h=-1;
        for (int i = 0; i < n; i++) {
            if (s[i]) {
                rst = h==-1?i - h-1:max(rst, (i - h) / 2);
                h = i;
            }
        }
        rst = max(rst, n-1-h);
        return rst;
    }
};