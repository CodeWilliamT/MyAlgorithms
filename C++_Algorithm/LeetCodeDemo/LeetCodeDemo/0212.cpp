using namespace std;
#include <iostream>
#include <vector>
#include <string>
#include <unordered_map>
//前缀树Trie, 回溯
//从另一个数据集出发的思维
// 
//前缀树Trie + 回溯
//构造前缀树 :
//遍历查询需求表的每个单词.
//每个字符时，如果字符对应节点存在则跳到该节点，不存在则创建.
//遍历每个单词的字符结束后存储该单词。
//回溯探索：
//回溯时，判定前缀树节点是否存在单词；
//剪枝：删除每个叶顶点（不存在childs的节点），返回(无试探后续);
//防重复，抹除试探表路径的点，试探完后回复；
//回溯试探时，试探点字符存在且合法，则进入下一次回溯；
class Solution {
    vector<string> ans;
    struct TrieNode{
        unordered_map<char, TrieNode*> c;
        string w = "";
    };
public:
    vector<string> findWords(vector<vector<char>>& b, vector<string>& ws) {
        ans.clear();
        TrieNode* root = new TrieNode();
        // 1.构建前缀树
        //   遍历查询需求表的每个单词，
        //     每个字符时，如果字符对应节点存在则跳到该节点，不存在则创建
        //   遍历每个单词的字符结束后存储该单词
        for (auto& w : ws){
            TrieNode* cur = root;
            for (char& c : w){
                if (!cur->c.count(c)){
                    cur->c[c] = new TrieNode();
                }
                cur = cur->c[c];
            }
            cur->w = w;
        }
        // 2. 遍历搜索域，从连在跟节点上的字符节点开始进行回溯比对
        for (int i = 0; i < b.size(); i++){
            for (int j = 0; j < b[i].size(); j++){
                if (root->c.count(b[i][j]))
                    backTrack(i,j,root,b);
            }
        }
        return ans;
    }
    // 回溯
    void backTrack(int x, int y,TrieNode* parent, vector<vector<char>>& b){
        TrieNode* cur = parent->c[b[x][y]];
        //如果有解则加入解，然后清空
        if (cur->w != "")ans.push_back(cur->w),cur->w="";
        //剪枝，剪除搜索过的最边界节点。
        if (cur->c.empty()){
            parent->c.erase(parent->c.find(b[x][y]));
            return;
        }
        //4个方向遍历试探
        int rdif[] = { -1,1,0,0 };
        int cdif[] = { 0,0,-1,1 };
        int row, col;
        char tmp = b[x][y];
        //路径字符格防重标记
        b[x][y] = 'X';
        for (int i = 0; i < 4; i++){
            row = x + rdif[i];
            col = y + cdif[i];
            if (row < 0 || row >= b.size() || col < 0 || col >= b[0].size())continue;
            if (cur->c.count(b[row][col])){
                backTrack(row, col, cur,b);
            }
        }
        //还原匹配路径字符格
        b[x][y] = tmp;
    }
};
//int main()
//{
//    Solution s;
//    /*vector<vector<char>> b = { {'a', 'a'} };
//    vector<string> w = { "aaa"};*/
//    vector<vector<char>> b = { {'o', 'a', 'a', 'n'},{'e', 't', 'a', 'e'},{'i', 'h', 'k', 'r'},{'i', 'f', 'l', 'v'} };
//    vector<string> w = {"oath", "pea", "eat", "rain"};
//    s.findWords(b, w);
//
//	return 0;
//}