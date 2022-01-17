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
//二叉树遍历，递归回溯
class Solution {
    int dfs(TreeNode* root, int& ans)
    {
        if (root == nullptr)return 0;
        int left = 0, right = 0;
        left = max(dfs(root->left, ans), 0);
        right = max(dfs(root->right, ans), 0);
        ans = max(root->val + max(max(left, right), left + right), ans);
        return root->val + max(left, right);
    }
public:
    int maxPathSum(TreeNode* root) {
        int ans = root->val;
        dfs(root, ans);
        return ans;
    }
};