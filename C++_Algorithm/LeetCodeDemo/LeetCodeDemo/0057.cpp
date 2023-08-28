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
//模拟
//有序区间数组中，插入新区间
class Solution {
public:
    vector<vector<int>> insert(vector<vector<int>>& a, vector<int>& b) {
        vector<vector<int>> rst;
        a.push_back(b);
        sort(a.begin(), a.end());
        for (auto& e : a) {
            if (rst.empty()||rst.back()[1]<e[0]) {
                rst.push_back(e);
            }
            else if (rst.back()[0] <= e[0]&& e[1] <= rst.back()[1]) {
                continue;
            }
            else if (rst.back()[0] <= e[0] && e[0] <= rst.back()[1]
                || rst.back()[0] <= e[1] && e[1] <= rst.back()[1]) {
                rst.back() = { min(rst.back()[0],e[0]),max(rst.back()[1],e[1]) };
            }
        }
        return rst;
    }
};