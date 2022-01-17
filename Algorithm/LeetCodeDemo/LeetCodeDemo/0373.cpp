using namespace std;
#include <vector>
#include <queue>
//优先队列 小顶堆
//将当前组元素跟下一组元素在队列里比较，处理存k个
class Solution {
public:
    vector<vector<int>> kSmallestPairs(vector<int>& nums1, vector<int>& nums2, int k) {
        auto cmp = [&nums1, &nums2](pair<int, int>& a, pair<int, int>& b) {
            return nums1[a.first] + nums2[a.second] > nums1[b.first] + nums2[b.second];
        };
        int m = nums1.size();
        int n = nums2.size();
        vector<vector<int>> ans;
        priority_queue<pair<int, int>, vector<pair<int, int>>, decltype(cmp)> pq(cmp);
        for (int i = 0; i < min(k, m); i++) {
            pq.emplace(i, 0);
        }
        int x, y;
        while (k-- > 0 && !pq.empty()) {
            x = pq.top().first;
            y = pq.top().second;
            pq.pop();
            ans.push_back({nums1[x], nums2[y]});
            if (y + 1 < n) {
                pq.emplace(x, y + 1);
            }
        }
        return ans;
    }
};