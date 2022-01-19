using namespace std;
#include<iostream>
#include<vector>
#include<string>
#include<unordered_map>

//深搜穷举回溯
class Solution
{
    string digits;
    vector<string> ans;
    unordered_map<char, string> mp;
    void dfs(string rst,int index)
    {
        if (index >= digits.size())
        {
            ans.push_back(rst);
            return;
        }
        for (auto e : mp[digits[index]])
        {
            dfs(rst+e, index + 1);
        }
    }
public:
    vector<string> letterCombinations(string digits)
    {
        ans.clear();
        if(!digits.size())goto _end;
        this->digits = digits;
        //储存字典
        mp = unordered_map<char, string>{
            {'2', "abc"}, {'3', "def"}, {'4', "ghi"}, {'5', "jkl"}, {'6', "mno"}, {'7', "pqrs"}, {'8', "tuv"}, {'9', "wxyz"} };
        dfs("",0);
        _end:
        return ans;
    }
};
////交换迭代穷举回溯
//class Solution {
//public:
//    vector<string> letterCombinations(string digits) {
//        vector<string> temp;
//        vector<string> temp1;
//        vector<string> temp2;
//        unordered_map<char, string> mp = unordered_map<char, string>{ {'2',"abc"},{'3',"def"},
//            {'4',"ghi"}, {'5',"jkl"}, {'6',"mno"}, {'7',"pqrs"}, {'8',"tuv"}, {'9',"wxyz" }};
//        for (auto d : digits)
//        {
//                temp.push_back(mp[d]);
//        }
//        for (auto d : temp)
//        {
//            int m = temp1.size();
//            int n = temp2.size();
//            for (auto x : d)
//            {
//                if (m == 0 && n == 0)
//                {
//                    string str;
//                    str.push_back(x);
//                    temp1.push_back(str);
//                    continue;
//                }
//                if (n)
//                {
//                    for (int i = 0; i < n; i++)
//                    {
//                        temp1.push_back(temp2[i]+x);
//                    }
//                }
//                else
//                {
//                    for (int i = 0; i < m; i++)
//                    {
//                        temp2.push_back(temp1[i]+x);
//                    }
//                }
//            }
//            if (!m && !n)continue;
//            if (m)temp1.clear();
//            else temp2.clear();
//        }
//        return temp1.size() > 0?temp1:temp2;
//    }
//};