using namespace std;
#include <iostream>
#include <vector>
#include <string>
#include <queue>
//回溯 优先队列
class Solution {
private:
    void dfs(string& num,int v[10], priority_queue<int, vector<int>, greater<int>>& q)
    {
        if (num.size() >= 3)
        {
            int n = stoi(num);
            if(n%2==0)
                q.push(n);
            return;
        }
        for (int i = 0; i < 10; i++)
        {
            if (i == 0 && num == "")continue;
            if (v[i])
            {
                v[i]--;
                num += i + '0';
                dfs(num, v, q);
                v[i]++;
                num.pop_back();
            }
        }
    }
public:
    vector<int> findEvenNumbers(vector<int>& digits) {
        int v[10]{};
        vector<int> rst;
        priority_queue<int,vector<int>,greater<int>> q;
        for (int& e : digits)
        {
            v[e]++;
        }
        string tmp;
        dfs(tmp, v, q);
        while (!q.empty())
        {
            if (rst.empty() || rst.back() != q.top());
                rst.push_back(q.top());
            q.pop();
        }
        return rst;
    }
};