using namespace std;
#include <iostream>
#include <vector>
#include <queue>
//优先队列 自定义排序
class Solution {
public:
    vector<int> kthSmallestPrimeFraction(vector<int>& a, int k) {
        int n = a.size();
        auto cmp = [](pair<int, int> a, pair<int, int> b) {return a.first*1.0 / a.second > b.first * 1.0 / b.second; };
        priority_queue < pair<int, int>,vector<pair<int, int>>, decltype(cmp)> pq(cmp);
        for (int i = 0; i < n; i++)
            for (int j = i + 1; j < n; j++)
                pq.push({ a[i], a[j] });
        while (--k)
        {
            pq.pop();
        }
        auto ans = pq.top();
        return {ans.first,ans.second};
    }
};