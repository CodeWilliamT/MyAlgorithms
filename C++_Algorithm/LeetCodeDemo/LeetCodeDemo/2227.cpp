using namespace std;
#include <iostream>
#include <vector>
#include <unordered_map>
//前缀树 麻烦题 设计题 
//需求：已有可匹配字典，需求O(n)的字符串多次匹配
class Encrypter {
    struct TrieNode {
        unordered_map<char, TrieNode*> c;
        string w = "";
    };
    unordered_map<char, string> mp;
    unordered_map<string, string> mp2;
    TrieNode* root;
public:
    Encrypter(vector<char>& keys, vector<string>& values, vector<string>& dictionary) {
        for (int i = 0; i < keys.size(); i++) {
            mp[keys[i]] = values[i];
            mp2[values[i]].push_back(keys[i]);
        }
        root = new TrieNode();
        for (auto& w : dictionary) {
            TrieNode* cur = root;
            for (char& c : w) {
                if (!cur->c.count(c)) {
                    cur->c[c] = new TrieNode();
                }
                cur = cur->c[c];
            }
            cur->w = w;
        }
    }

    string encrypt(string s) {
        string rst;
        for (auto& c : s) {
            rst += mp[c];
        }
        return rst;
    }
    void dfs(int& rst,string& s,TrieNode* node,int idx, string& t) {
        if (idx >= s.size()&&node->w.size()) {
            rst++;
        }
        string sub = s.substr(idx, 2);
        for (auto&e:mp2[sub]) {

            if (!node->c.count(e)) {
                continue;
            }
            t.push_back(e);
            dfs(rst,s,node->c[e], idx+2, t);
            t.pop_back();
        }
    }
    int decrypt(string s) {
        int n = s.size();
        int rst = 0;
        string t;
        dfs(rst, s,root, 0, t);
        return rst;
    }
};

/**
 * Your Encrypter object will be instantiated and called as such:
 * Encrypter* obj = new Encrypter(keys, values, dictionary);
 * string param_1 = obj->encrypt(word1);
 * int param_2 = obj->decrypt(word2);
 */