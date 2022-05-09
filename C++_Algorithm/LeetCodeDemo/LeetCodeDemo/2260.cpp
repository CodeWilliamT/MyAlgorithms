using namespace std;
#include <vector>
//哈希
//遍历数组，记录每个数的位置，如果这个数出现过，则做差跟答案比。
class Solution {
public:
    int minimumCardPickup(vector<int>& cards) {
        int n = cards.size();
        unordered_map<int, int>mp;
        int maxn = 1e5+1;
        int rst = maxn;
        for (int i = 0; i < n; i++) {
            if (mp.count(cards[i])) {
                rst = min(rst, 1 + i - mp[cards[i]]);
            }
            mp[cards[i]] = i;
        }
        return rst==maxn?-1:rst;
    }
};