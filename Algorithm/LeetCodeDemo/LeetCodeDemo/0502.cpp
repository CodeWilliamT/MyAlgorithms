using namespace std;
#include <iostream>
#include <vector>
#include <string>
#include <algorithm>
#include <queue>
//排序，优先队列
class Solution {
public:
    int findMaximizedCapital(int k, int w, vector<int>& p, vector<int>& c) {
        int n = p.size();
        vector<vector<int>> data(n, vector<int>(2));
        priority_queue<int,vector<int>, less<int> >q;
        for (int i = 0; i < n; i++)
        {
            data[i][0] = c[i];
            data[i][1] = p[i];
        }
        sort(data.begin(), data.end());
        int curr = 0;
        for (int i = 0; i < k; i++)
        {
            for (; curr < data.size() && data[curr][0] <= w; curr++)
            {
                q.push(data[curr][1]);
            }
            if (q.empty())break;
            w = w + q.top();
            q.pop();
        }
        return w;
    }
};
//动态规划
//低效，时效不过
//class Solution {
//public:
//    int findMaximizedCapital(int k, int w, vector<int>& p, vector<int>& c) {
//        int n = p.size();
//        vector<vector<int>> data(n, vector<int>(2));
//        for (int i = 0; i < n; i++)
//        {
//            data[i][0] = p[i];
//            data[i][1] = c[i];
//        }
//        sort(data.begin(), data.end(), [](vector<int> a, vector<int> b) {return a[0] > b[0]; });
//        vector<bool> vis(n, 0);
//        vector<int> f(k + 1, 0);
//        f[0] = w;
//        for (int i = 1; i <= k; i++)
//        {
//            f[i] = f[i - 1];
//            for (int j = 0; j < n; j++)
//            {
//                if (!vis[j] && data[j][1] <= f[i - 1] && f[i] < f[i - 1] + data[j][0])
//                {
//                    f[i] = f[i - 1] + data[j][0];
//                    vis[j] = 1;
//                    break;
//                }
//            }
//        }
//        return f[k];
//    }
//};