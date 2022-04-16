using namespace std;
#include <vector>
#include <queue>
//优先队列
//每次将最小的变大
class Solution {
public:
    int maximumProduct(vector<int>& nums, int k) {
        priority_queue<int, vector<int>, greater<int>> pq;
        for (int& e : nums) {
            pq.push(e);
        }
        long long cur;
        while (k) {
            cur = pq.top();
            pq.pop();
            if (pq.top() == cur) {
                k--;
                cur++;
            }
            else if (pq.top() - cur <k) {
                k -= pq.top() - cur;
                cur = pq.top();
            }
            else {
                cur += k;
                k = 0;
            }
            pq.push(cur);
        }
        long long rst = 1;
        while (!pq.empty()) {
            cur = pq.top();
            rst = (rst * cur) % (long long)(1e9 + 7);
            pq.pop();
        }
        return rst;
    }
};