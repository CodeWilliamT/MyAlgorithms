#include "myAlgo\Structs\TreeNode.cpp"
using namespace std;
#include <iostream>
#include <vector>
#include <string>
#include <numeric>
#include <algorithm>
#include <unordered_set>
#include <unordered_map>
#include <set>
#include <map>
#include <queue>
#include <stack>
#include <functional>
#include <bitset>
typedef pair<int, bool> pib;
typedef pair<int, int> pii;
typedef long long ll;
typedef pair<ll, ll> pll;
typedef pair<ll, int> pli;
#define MAXN (int)(1e5+1)
#define MAXM (int)(1e5+1)
#define MOD (int)(1e9+7)

class Trie {
    struct TrieNode {
        unordered_map<char, TrieNode*> c;
        string w = "";
    };
    TrieNode* root = new TrieNode();
public:
    Trie() {
    }
    //向前缀树中插入字符串 word
    void insert(string w) {
        TrieNode* cur = root;
        for (char& c : w) {//每个字符时，如果字符对应节点存在则跳到该节点，不存在则创建
            if (!cur->c.count(c)) {
                cur->c[c] = new TrieNode();
            }
            cur = cur->c[c];
        }
        cur->w = w;//遍历每个单词的字符结束后存储该单词
    }
    //查询字符串是否在之前插入过。
    bool search(string word) {
        TrieNode* cur = root;
        for (int i = 0; i < word.size(); i++) {
            if (!cur->c.count(word[i]))
                return false;
            cur = cur->c[word[i]];
        }
        return cur->w != "";
    }
    //查询字符串是否是之前插入过的字符串前缀。
    bool startsWith(string prefix) {
        TrieNode* cur = root;
        for (int i = 0; i < prefix.size(); i++) {
            if (!cur->c.count(prefix[i]))
                return false;
            cur = cur->c[prefix[i]];
        }
        return true;
    }
};
