using namespace std;
#include <iostream>
#include <vector>
#include <string>
#include <algorithm>
//回溯、深搜
//注意去重
//去重：
//不同顺序同组合的重复：从前往后找。
//相同值的重复：当前面相同，并且没用过，则不用当前的。
class Solution {
    void dfs(int idx, int sum, vector<int>& a, vector<vector<int>>& ans, vector<bool>& v, vector<int>& c, int target)
    {
        if (sum == target)
        {
            ans.push_back(a);
            return;
        };
        if (idx == c.size() || sum > target)return;
        for (int i = idx; i < c.size(); i++)
        {
            if (v[i])continue;
            if (i > 0 && !v[i - 1] && c[i - 1] == c[i])continue;
            sum += c[i];
            v[i] = 1;
            a.push_back(c[i]);
            dfs(i + 1, sum, a, ans, v, c, target);
            a.pop_back();
            sum -= c[i];
            v[i] = 0;
        }
    }
public:
    vector<vector<int>> combinationSum2(vector<int>& c, int target) {
        sort(c.begin(), c.end());
        int n = c.size();
        vector<bool> v(n, 0);
        vector<vector<int>> ans;
        vector<int> a;
        int sum = 0;
        dfs(0, sum, a, ans, v, c, target);
        return ans;
    }
};