using namespace std;
#include <iostream>
#include <vector>
#include <algorithm>
//深搜，回溯
class Solution {
    void dfs(int x, int idx, vector<int>& c, int t, vector<int>& a, vector<vector<int>>& ans)
    {
        if (x > t)return;
        if (x == t)
        {
            ans.push_back(a);
            return;
        }
        for (int i = idx; i < c.size(); i++)
        {
            x += c[i];
            if (x > t)return;
            a.push_back(c[i]);
            dfs(x, i, c, t, a, ans);
            x -= c[i];
            a.pop_back();
        }
    }
public:
    vector<vector<int>> combinationSum(vector<int>& c, int t) {
        vector<vector<int>> ans;
        vector<int> a;
        sort(c.begin(), c.end());
        dfs(0, 0, c, t, a, ans);
        return ans;
    }
};

//int main()
//{
//    Solution s;
//    vector<int> a = { 2,3,7 };
//    s.combinationSum(a, 7);
//    return 0;
//}