using namespace std;
#include <iostream>
#include <vector>
#include <string>
#include <algorithm>
//深搜回溯
//去重：对原数组排序，保证相同的数字都相邻。若前后元素相同，且前元素未被访问过，说明进入了重复搜索区
class Solution {
    void dfs(int idx, vector<int>& b, vector<bool>& flag, vector<int>& a, vector<vector<int>>& ans)
    {
        if (idx == a.size()) { ans.push_back(b); return; }
        for (int i = 0; i < a.size(); i++)
        {
            if (flag[i]||(i>0&&a[i]==a[i-1]&&!flag[i-1]))continue;
            b.push_back(a[i]);
            flag[i] = 1;
            dfs(idx + 1, b, flag, a, ans);
            b.pop_back();
            flag[i] = 0;
        }
    }
public:
    vector<vector<int>> permuteUnique(vector<int>& a) {
        vector<vector<int>> ans;
        vector<int> b;
        vector<bool>flag(a.size());
        sort(a.begin(), a.end());
        dfs(0, b, flag, a, ans);
        return ans;
    }
};
//深搜回溯
//去重：低效，哈希表，用字符串储存
//class Solution {
//    void dfs(int idx,string& b, vector<bool>& flag,vector<int>& a, unordered_set<string>& ans)
//    {
//        if (idx == a.size()){ans.insert(b);return;}
//        for (int i = 0; i < a.size(); i++)
//        {
//            if (flag[i])continue;
//            b+=to_string(a[i]+20);
//            flag[i] = 1;
//            dfs(idx + 1, b, flag, a, ans);
//            b.pop_back();
//            b.pop_back();
//            flag[i] = 0;
//        }
//    }
//public:
//    vector<vector<int>> permuteUnique(vector<int>& a) {
//        vector<vector<int>> ans;
//        unordered_set<string> ansSet;
//        string b;
//        vector<bool>flag(a.size());
//        dfs(0,b,flag,a, ansSet);
//        vector<int>temp;
//        for (auto e : ansSet)
//        {
//            for (int i = 0; i < e.size(); i+=2)
//            {
//                temp.push_back(stoi(e.substr(i, 2)) - 20);
//            }
//            ans.push_back(temp);
//            temp.clear();
//        }
//        return ans;
//    }
//};