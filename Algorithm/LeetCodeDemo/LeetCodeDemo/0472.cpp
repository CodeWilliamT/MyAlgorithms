using namespace std;
#include <iostream>
#include <vector>
#include <algorithm>
#include <unordered_map>
#include <functional>
//前缀树变式 深搜(回溯)
//对每个单次在前缀树里做深搜，找不到则加入前缀树。
class Solution {
    struct TrieNode {
        unordered_map<char, TrieNode*> childs;
        string str = "";
    };
public:
    vector<string> findAllConcatenatedWordsInADict(vector<string>& words) {
        TrieNode* root = new TrieNode();
        int n = words.size();
        sort(words.begin(), words.end(), [](const string& a,const string& b) {return a.size() < b.size(); });
        int mxlen = words[n - 1].size();
        function<bool(string, int)> dfs = [&](string word, int idx) {
            if (word.size() == idx)return true;
            TrieNode* cur = root;
            for (int i = idx; i < word.size(); i++) {
                if (!cur->childs.count(word[i]))
                        return false;
                cur = cur->childs[word[i]];
                if (cur->str != "")
                    if (dfs(word, i+1))
                        return true;
            }
            return false;
        };
        vector<string> rst;

        for (int i = 0; i < n; i++) {
            if (!words[i].size())continue;
            if (dfs(words[i], 0)) {
                rst.emplace_back(words[i]);
            }
            else {
                auto now = root;
                for (int j = 0; j < words[i].size(); j++) {
                    if (!now->childs.count(words[i][j]))
                        now->childs[words[i][j]] = new TrieNode();
                    now = now->childs[words[i][j]];
                }
                now->str = words[i];
            }
        }
        return rst;
    }
};