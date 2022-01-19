using namespace std;
#include <iostream>
#include <vector>
#include <string>
#include <set>
//回溯,两分查找
//坑：10^9以上，用stoi应该改为用stoll，并用long long存储,防止OJ编译器不同的int定义带来的转化为不同数的问题。

class Solution {
private:
    void dfs(string s, int num, int limit, set<int>& rst)
    {
        if (s.size() > 10)return;
        long long a = stoll(s);
        if (a > limit)return;
        rst.insert(a);
        if (num != 0)dfs(s + to_string(num - 1), num - 1, limit, rst);
        if (num != 9)dfs(s + to_string(num + 1), num + 1, limit, rst);
    }
public:
    vector<int> countSteppingNumbers(int low, int high) {
        set<int> rst = { 0 };
        for (int i = 1; i < 10; i++)
        {
            dfs(to_string(i), i, high, rst);
        }
        int l = 0, r = rst.size() - 1, mid;
        while (l <= r)
        {
            mid = (l + r) / 2;
            if ((*next(rst.begin(), mid)) < low)
            {
                l = mid + 1;
            }
            else
            {
                r = mid - 1;
            }
        }
        vector<int> ans;
        for (int i = l; i < rst.size(); i++)
        {
            ans.push_back((*next(rst.begin(), i)));
        }
        return ans;
    }
};