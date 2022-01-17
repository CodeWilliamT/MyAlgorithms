using namespace std;
#include <iostream>
#include <vector>
#include <queue>
//贪心 优先队列
//果子 消失的方式，某棵树吃掉-1,或某天烂掉一棵树归零，果子增加的方式：长出来一棵树。
//
class Solution {
public:
    int eatenApples(vector<int>& apples, vector<int>& days) {
        int n = apples.size();
        int d = 0;
        int rst = 0;
        priority_queue<vector<int>, vector<vector<int>>, greater<vector<int>>> pq;
        for (int i = 0; i < n; i++) {
            d = max(i + days[i], d);
        }
        int cur = 0;
        vector<int> top;
        for (int i = 0; i < d; i++) {
            if (i < n && apples[i] != 0)pq.push({ i + days[i],i });
            if (!pq.empty())top = pq.top();
            while (!pq.empty() && top[0] == i)
            {
                cur -= apples[top[1]];
                pq.pop();
                if (!pq.empty())top = pq.top();
            }
            if (i < n)cur += apples[i];
            if (cur > 0) {
                while (!pq.empty() && apples[top[1]] == 0) {
                    pq.pop();
                    if (!pq.empty())top = pq.top();
                }
                rst++;
                if (!pq.empty())apples[top[1]]--;
                cur--;
            }
        }
        return rst;
    }
};