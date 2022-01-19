using namespace std;
#include <iostream>
#include <vector>
#include <string>
#include <unordered_map>
//前缀树Trie + 回溯
//前缀树Trie，简单构造
//深搜(回溯，递归)，查询
class WordDictionary {
private:
    struct TrieNode
    {
        unordered_map<char, TrieNode*> childs;
        string str = "";
        TrieNode() {}
    };
    TrieNode* root;
public:
    WordDictionary() {
        root = new TrieNode();
    }

    void addWord(string word) {
        TrieNode* cur = root;
        for (auto c : word)
        {
            if (!cur->childs.count(c))
            {
                TrieNode* tmp = new TrieNode();
                cur->childs[c] = tmp;
            }
            cur = cur->childs[c];
        }
        cur->str = word;
    }
    bool search(string word, int idx=0, TrieNode* cur = nullptr) {
        if(cur== nullptr)cur = root;

        for (int i=idx;i<word.size();i++)
        {
            if (word[i] == '.')
            {
                for (auto t : cur->childs)
                {
                    if (search(word, i + 1, t.second))
                    {
                        return true;
                    }
                }
                return false;
            }
            if (!cur->childs.count(word[i]))
            {
                return false;
            }
            cur = cur->childs[word[i]];
        }
        return cur->str != "";
    }
};