using namespace std;
#include <iostream>
#include <vector>
#include <unordered_map>
#include <queue>
//并查集 优先队列
//集合→同时间有多个查询→多集合→并查集，按时间遍历→优先队列，时间，meeing数组。
//无后效性→时刻结束，使得有关系的人孤立。
class Solution {
    int findP(int c, vector<int>& f)
    {
        if (c == f[c])return c;
        return findP(f[c],f);
    }
    void unionP(int x,int y, vector<int>& f)
    {
        int a = findP(x,f);
        int b= findP(y, f);
        f[a] = f[b] = f[x] = f[y] = min(a, b);
    }
public:
    vector<int> findAllPeople(int n, vector<vector<int>>& meetings, int firstPerson) {
        priority_queue<int, vector<int>, greater<int>>pq;
        unordered_map<int,vector<vector<int>>> t;
        vector<int> f(n);
        for (int i = 0; i < n; i++)
            f[i] = i;
        f[firstPerson] = 0;
        for (auto& e : meetings)
        {
            if (!t.count(e[2]))pq.push(e[2]);
            t[e[2]].push_back({ e[0],e[1] });
        }
        for (int i = pq.top(); !pq.empty();)
        {
            for (int j = 0; j < t[i].size(); j++)
            {
                unionP(t[i][j][0], t[i][j][1],f);
            }
            for (int j = 0; j < t[i].size(); j++)
            {
                if (findP(t[i][j][0],f))f[t[i][j][0]] = t[i][j][0];
                if (findP(t[i][j][1], f))f[t[i][j][1]] = t[i][j][1];
            }
            pq.pop();
            i=pq.top();
        }
        vector<int> rst;
        for (int i=0;i<n;i++)
        {
            if(!findP(i,f))rst.push_back(i);
        }
        return rst;
    }
};