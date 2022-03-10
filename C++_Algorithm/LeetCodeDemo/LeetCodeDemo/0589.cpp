using namespace std;
#include <vector>
class Node {
public:
    int val;
    vector<Node*> children;

    Node() {}

    Node(int _val) {
        val = _val;
    }

    Node(int _val, vector<Node*> _children) {
        val = _val;
        children = _children;
    }
};
//栈 多叉树
//前序遍历
//迭代
class Solution {
public:
    vector<int> preorder(Node* root) {
        vector<int> rst;
        if (!root)return rst;
        vector<Node*> stk;
        stk.push_back(root);
        Node* cur;
        while (!stk.empty()) {
            cur = stk.back();
            stk.pop_back();
            rst.push_back(cur->val);
            for (int i = cur->children.size() - 1; i >= 0;i--) {
                stk.push_back(cur->children[i]);
            }
        }
        return rst;
    }
};
//回溯(深搜) 多叉树
//前序遍历
//递归
class Solution {
    vector<int> rst;
    void dfs(Node* root) {
        if (!root)return;
        rst.push_back(root->val);
        for (auto& e : root->children) {
            dfs(e);
        }
    }
public:
    vector<int> preorder(Node* root) {
        dfs(root);
        return rst;
    }
};