using namespace std;
#include <iostream>
#include <vector>
struct TreeNode {
    int val;
    TreeNode* left;
    TreeNode* right;
    TreeNode() : val(0), left(nullptr), right(nullptr) {}
    TreeNode(int x) : val(x), left(nullptr), right(nullptr) {}
    TreeNode(int x, TreeNode* left, TreeNode* right) : val(x), left(left), right(right) {}
};
//简单题 回溯
class Solution {
    void midfind(TreeNode* root, vector<int>& rst)
    {
        if (root->left != nullptr)
            midfind(root->left, rst);
        rst.push_back(root->val);
        if (root->right != nullptr)
            midfind(root->right, rst);
    }
public:
    vector<int> inorderTraversal(TreeNode* root) {
        vector<int> rst;
        if (root == nullptr)
            return rst;
        midfind(root, rst);
        return rst;
    }
};