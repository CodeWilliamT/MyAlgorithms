using namespace std;
#include <iostream>
#include <vector>
#include <string>
#include <algorithm>
#include <unordered_set>
//搜索
//广搜深搜均可,记忆化搜索
//深搜解
class Solution {
    void doDeleteB(string& b)
    {
        int n = b.size();
        for (int i = 0; i < n - 2; )
        {
            if (b[i] == b[i + 1] && b[i] == b[i + 2])
            {
                int num = i + 3;
                while (b[num] == b[i])num++;
                b.erase(b.begin() + i, b.begin() + num);
                i = i - 2 >= 0 ? i - 2 : i - 1 >= 0 ? i - 1 : 0;
                n = b.size();
                continue;
            }
            i++;
        }
    }
    void dfs(int& ans, string& b, string& h, unordered_set<string>& v)
    {
        if (b.empty())
        {
            int tmp = h.size();
            ans = max(ans, tmp);
            return;
        }
        if (v.count(b + "," + h))return;
        v.insert(b + "," + h);
        int m = h.size();
        int n = b.size();
        for (int i = 0; i < m; i++)
        {
            for (int j = 0; j <= n; j++)
            {
                string b_back = b;
                string h_back = h;
                b.insert(b.begin() + j, h[i]);
                h.erase(h.begin() + i);
                doDeleteB(b);
                dfs(ans, b, h, v);
                b = b_back;
                h = h_back;
            }
        }
    }
public:
    int findMinStep(string b, string h) {
        sort(h.begin(), h.end());
        unordered_set<string> v;
        int ans = -1;
        dfs(ans, b, h, v);
        return ans == -1 ? -1 : h.size() - ans;
    }
};