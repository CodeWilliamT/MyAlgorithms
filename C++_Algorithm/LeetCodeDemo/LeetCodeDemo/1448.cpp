using namespace std;
#include <iostream>

 //Definition for a binary tree node.
struct TreeNode {
    int val;
    TreeNode* left;
    TreeNode* right;
    TreeNode() : val(0), left(nullptr), right(nullptr) {}
    TreeNode(int x) : val(x), left(nullptr), right(nullptr) {}
    TreeNode(int x, TreeNode* left, TreeNode* right) : val(x), left(left), right(right) {}
};
//二叉树递归遍历
class Solution {
    int solve(TreeNode* root,int max) {
        if (!root)return 0;
        int rst = 0;
        if (root->val >= max)
        {
            max = root->val;
            rst =1;
        }
        rst = rst + solve(root->left, max) + solve(root->right, max);
        return rst;
    }
public:
    int goodNodes(TreeNode* root) {
        int max = root->val;
        return solve(root,max);
    }
};