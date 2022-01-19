using namespace std;
#include <iostream>
#include <unordered_map>
//设计题，前缀树，回溯
//
class MapSum {
private:
    struct TrieNode
    {
        unordered_map<char, TrieNode*> childs;
        string str = "";
        int val = 0;
        TrieNode() :childs(NULL), str(""), val(0) {};
    };
    TrieNode* root,*cur;
    int sum(TrieNode* root) {
        if (!root)return 0;
        int ans= root->val;
        for (auto e : root->childs)
        {
            ans =ans+ sum(e.second);
        }
        return ans;
    }
public:
    MapSum() {
        root = new TrieNode();
    }

    void insert(string key, int val) {
        cur = root;
        for (auto c : key)
        {
            if (cur->childs.count(c))
                cur = cur->childs[c];
            else
            {
                cur->childs[c] = new TrieNode();
                cur = cur->childs[c];
            }
        }
        cur->str = key;
        cur->val = val;
    }

    int sum(string prefix) {
        cur = root;
        for (auto c : prefix)
        {
            if (!cur->childs.count(c))
                return 0;
            else
                cur = cur->childs[c];
        }
        return sum(cur);
    }
};

/**
 * Your MapSum object will be instantiated and called as such:
 * MapSum* obj = new MapSum();
 * obj->insert(key,val);
 * int param_2 = obj->sum(prefix);
 */