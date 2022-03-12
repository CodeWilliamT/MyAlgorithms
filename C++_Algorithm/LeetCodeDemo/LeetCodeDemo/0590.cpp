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
//迭代 多叉树 栈
//N叉树的后续遍历
//子节点从右往左的前序遍历的倒序。
class Solution {
public:
    vector<int> postorder(Node* root) {
        vector<int> rst;
        if (!root)
            return rst;
        vector<Node*> stk;
        stk.push_back(root);
        Node* cur;
        while (!stk.empty()) {
            cur = stk.back();
            stk.pop_back();
            for (auto& e : cur->children) {
                stk.push_back(e);
            }
            rst.push_back(cur->val);
        }
        reverse(rst.begin(), rst.end());
        return rst;
    }
};
//回溯 多叉树
//N叉树的后续遍历
class Solution {
private:
    void dfs(Node* root, vector<int>&rst) {
        if (!root)return;
        for (auto& e : root->children) {
            dfs(e,rst);
        }
        rst.push_back(root->val);
    }
public:
    vector<int> postorder(Node* root) {
        vector<int> rst;
        dfs(root, rst);
        return rst;
    }
};