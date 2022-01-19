using namespace std;
#include <iostream>
#include <vector>
#include <string>
//Definition for a binary tree node.
struct TreeNode {
    int val;
    TreeNode* left;
    TreeNode* right;
    TreeNode() : val(0), left(nullptr), right(nullptr) {}
    TreeNode(int x) : val(x), left(nullptr), right(nullptr) {}
    TreeNode(int x, TreeNode* left, TreeNode* right) : val(x), left(left), right(right) {}

};
//二叉搜索树
//分析得，删除操作就是找节点
//右子树空则左子树继承根节点
//否则左子树黏在右子树的最左子树上，然后根节点用右子树替换。
class Solution {
    TreeNode* leftbound(TreeNode* root)
    {
        if (root == nullptr)return nullptr;
        if (root->left == nullptr)return root;
        return leftbound(root->left);
    }
    TreeNode* dfs(TreeNode* root, int key)
    {
        if (root == nullptr)return nullptr;
        if (root->val == key)
        {
            if (root->left == nullptr && root->right == nullptr)return nullptr;
            if (root->right == nullptr)
            {
                return root->left;
            }
            else
            {
                leftbound(root->right)->left = root->left;
                return root->right;
            }
        }
        root->left = dfs(root->left, key);
        root->right = dfs(root->right, key);
        return root;
    }
public:
    TreeNode* deleteNode(TreeNode* root, int key) {
        return dfs(root, key);
    }
};