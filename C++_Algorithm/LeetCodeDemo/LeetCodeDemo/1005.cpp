using namespace std;
#include <iostream>
#include <vector>
#include <queue>
//优先队列
class Solution {
public:
    int largestSumAfterKNegations(vector<int>& nums, int k) {
        priority_queue<int, vector<int>, greater<int>> pq;
        int sum=0;
        for (auto& e : nums)
            pq.push(e);
        int cur;
        while (k--)
        {
            cur=pq.top();
            pq.pop();
            cur = -cur;
            pq.push(cur);
        }
        while (!pq.empty())
        {
            cur = pq.top();
            pq.pop();
            sum += cur;
        }
        return sum;
    }
};