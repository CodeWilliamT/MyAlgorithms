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
#include "myAlgo\Structs\TreeNode.cpp"
typedef long long ll;
typedef pair<ll, ll> pll;
typedef pair<int, int> pii;
//哈希 枚举 异或特性x^b==y y^b==x;
class Solution {
public:
    int countPairs(vector<vector<int>>& coordinates, int k) {
        int ret = 0;
        map<int, map<int, int> > mp;
        for (auto& v : coordinates) {
            int x = v[0], y = v[1];
            for (int i = 0; i <= k; i++) {
                int j = k - i;
                if (mp.count(x ^ i) && mp[x ^ i].count(y ^ j)) ret += mp[x ^ i][y ^ j];
            }
            mp[x][y]++;
        }
        return ret;
    }
};