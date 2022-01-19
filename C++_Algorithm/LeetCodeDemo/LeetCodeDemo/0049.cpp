using namespace std;
#include <iostream>
#include <vector>
#include <unordered_map>
//哈希
//单词按字符构成分类
//遍历单词，遍历字符，记录频率，频率转字符串，相同字符串代表相同频率则为同类字符串，存入哈希表mp，根据mp映射频率字符串与vector分组索引。
class Solution {
public:
    vector<vector<string>> groupAnagrams(vector<string>& strs) {
        vector<vector<string>> rst;
        unordered_map<string, int> mp;
        int lst[26]{};
        string tmp;
        for (auto& s : strs)
        {
            memset(lst, 0, sizeof(lst));
            tmp = "";
            for (auto& c : s)
            {
                lst[c - 'a']++;
            }
            for (int i = 0; i < 26; i++)
            {
                tmp.push_back(lst[i]);
            }
            if (!mp.count(tmp))
            {
                mp[tmp] = rst.size();
                rst.push_back({ s });
            }
            else
            {
                rst[mp[tmp]].push_back(s);
            }
        }
        return rst;
    }
};