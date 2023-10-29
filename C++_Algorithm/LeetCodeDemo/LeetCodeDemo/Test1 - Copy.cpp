#include "myAlgo\LCParse\TreeNode.cpp"
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
#define MAXN (int)(1e5+1)
#define MAXM (int)(1e5+1)
#define MOD (int)(1e9+7)
class Solution {
    typedef long long ll;
public:
    int sumCounts(vector<int>& nums) {
        int n = nums.size();
        int cnts[101]{};
        ll cnt = 0;
        ll rst = 0;
        for (int i = 0; i < n; i++) {
            memset(cnts, 0, sizeof(cnts));
            cnt = 0;
            for (int j = i; j < n; j++) {
                if (!cnts[nums[j]])
                    cnt++;
                cnts[nums[j]]++;
                rst = (rst+cnt * cnt % MOD)%MOD;
            }
        }
        return rst;
    }
};