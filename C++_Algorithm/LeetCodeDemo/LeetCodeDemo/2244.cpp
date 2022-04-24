using namespace std;
#include <unordered_map>
//找规律 枚举
//计数，然后看每一个难度的数目cnt[i]
//某难度只有1个就不行，2个3个就是1次，大于等于3个就是cnt[i]/3+cnt[i]%3?1:0;
//即只有1个就不行，大于1就是cnt[i]/3+(cnt[i]%3?1:0);
class Solution {
public:
    int minimumRounds(vector<int>& tasks) {
        unordered_map<int, int> mp;
        for (int& e : tasks) {
            mp[e]++;
        }
        int rst = 0;
        for (auto& e : mp) {
            if (e.second == 1)return -1;
            rst+= e.second / 3 + (e.second % 3 ? 1 : 0);
        }
        return rst;
    }
};