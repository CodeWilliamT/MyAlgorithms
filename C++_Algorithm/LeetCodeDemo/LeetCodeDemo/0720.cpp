using namespace std;
#include <iostream>
#include <vector>
#include <algorithm>
#include <unordered_map>
//前缀树
class Solution {
    struct TrieNode {
        unordered_map<char, TrieNode*> c;
        int cnt = 0;
    };
public:
    string longestWord(vector<string>& w) {
        if (!w.size())return "";
        sort(w.begin(), w.end());
        int idx = 0;
        int len = 0;
        TrieNode* root = new TrieNode(), * cur;
        for (int i = 0; i < w.size(); i++) {
            cur = root;
            for (int j = 0; j < w[i].size(); j++) {
                if (!cur->c.count(w[i][j])) {
                    if (j == w[i].size() - 1) {
                        cur->c[w[i][j]] = new TrieNode();
                        cur = cur->c[w[i][j]];
                        if (w[i].size() > len || w[i].size() == len && w[i] < w[idx]) {
                            idx = i;
                            len = w[i].size();
                        }
                    }
                    break;
                }
                cur = cur->c[w[i][j]];
            }
        }
        return len ? w[idx] : "";
    }
};