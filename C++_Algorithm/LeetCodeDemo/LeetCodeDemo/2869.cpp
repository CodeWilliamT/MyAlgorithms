using namespace std;
#include <vector>
typedef pair<int, bool> pib;
typedef long long ll;
typedef pair<ll, ll> pll;
typedef pair<int, int> pii;
class Solution {
public:
    int minOperations(vector<int>& nums, int k) {
        int cnt = k;
        bool st[51]{};
        int e;
        int n = nums.size();
        int i = n - 1;
        for (; i >-1;i--) {
            e = nums[i];
            if (e <= k && !st[e]) {
                cnt--;
                st[e] = 1;
                if (!cnt)
                    return n-i;
            }
        }
        return n-i;
    }
};