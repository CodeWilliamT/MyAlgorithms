using namespace std;
#include <queue>
//优先队列
class Solution {
public:
    int longestConsecutive(vector<int>& nums) {
        if (!nums.size())return 0;
        priority_queue<int, vector<int>, greater<int>> pq;
        for (int& e : nums) {
            pq.push(e);
        }
        int rst = 0, cnt = 1;
        int pre = pq.top();
        pq.pop();
        while (!pq.empty()) {
            if (pre == pq.top()) {

            }
            else if (pre + 1 == pq.top()) {
                cnt++;
            }
            else {
                rst = max(cnt, rst);
                cnt = 1;
            }
            pre = pq.top();
            pq.pop();
        }
        rst = max(cnt, rst);
        return rst;
    }
};