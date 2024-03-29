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
//��ϣ
class Solution {
#define MAXD (int)(30)
public:
    int findMaximumXOR(vector<int>& nums) {
        int rst=0,next;
        sort(nums.begin(), nums.end());
        unordered_set<int> st;
        for (int k = MAXD; k > -1; k--) {
            st.clear();
            rst = rst << 1;
            next = rst + 1;
            for (int& e : nums) {
                if (st.count(next ^ (e >> k))) {
                    rst = next;
                    break;
                }
                st.insert(e >> k);
            }
        }
        return rst;
    }
};