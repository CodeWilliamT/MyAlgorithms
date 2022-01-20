using namespace std;
#include <vector>
#include <unordered_map>
//找规律 哈希表 打草稿数学推算
class Solution {
public:
    vector<long long> getDistances(vector<int>& arr) {
        int n = arr.size();
        vector<long long> res(n);   // 每个元素与相同元素间隔之和
        unordered_map<int, long long> total;   // 每个数值出现下标之和
        unordered_map<int, int> cnt;   // 每个数值出现次数
        // 正向遍历并更新两个哈希表以及间隔之和数组
        for (long long i = 0; i < n; i++) {
            int val = arr[i];
            if (cnt.count(val)) {
                res[i] += i * cnt[val] - total[val];
            }
            total[val] += i;
            ++cnt[val];
        }
        // 清空哈希表，反向遍历并更新两个哈希表以及间隔之和数组
        total.clear();
        cnt.clear();
        for (int i = n - 1; i >= 0; --i) {
            int val = arr[i];
            if (cnt.count(val)) {
                res[i] += total[val] - (long long)i * cnt[val];
            }
            total[val] += i;
            ++cnt[val];
        }
        return res;
    }
};
