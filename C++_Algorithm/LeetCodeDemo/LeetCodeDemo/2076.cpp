using namespace std;
#include <iostream>
#include <vector>
//并查集+哈希
//黑名单1，好友名单2,自己是自己的朋友
//如果朋友头子之间同一个人有一方为朋友一方为黑名单的，不能交朋友，能交朋友则朋友头子，相互复制对方的黑名单,好友名单
class Solution {
private:
    int findParent(int a, int p[1000])
    {
        if (p[a] == a)return a;
        return findParent(p[a], p);
    }
    void unit(int a, int b, int p[1000])
    {
        int p1 = findParent(a, p);
        int p2 = findParent(b, p);
        if (p1>p2)swap(p1, p2);
        p[p2] = p1;
    }
public:
    vector<bool> friendRequests(int n, vector<vector<int>>& rt, vector<vector<int>>& rq) {
        int v[1000][1000]{};
        int p[1000]{};
        for (int i = 0; i < n; i++)
        {
            v[i][i] = 2;
            p[i] = i;
        }
        for (auto& e : rt)
        {
            v[e[0]][e[1]] = 1;
            v[e[1]][e[0]] = 1;
        }
        vector<bool> ans;
        bool flag;
        for (auto& e : rq)
        {
            flag = true;
            int p1 = findParent(e[0], p);
            int p2 = findParent(e[1], p);
            for (int i = 0; i < n; i++)
            {
                if (v[p1][i] == 1 && v[p2][i] == 2 || v[p2][i] == 1 && v[p1][i] == 2)
                {
                    flag = false;
                    break;
                }
            }
            if(flag)
            {
                ans.push_back(1);
                unit(p1, p2, p);
                for (int i = 0; i < n; i++)
                {
                    if (v[p2][i])v[p1][i] = v[p2][i];
                    else if (v[p1][i])v[p2][i] = v[p1][i];
                }
            }
            else
                ans.push_back(0);
        }
        return ans;
    }
};