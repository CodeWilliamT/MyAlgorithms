using namespace std;
#include <iostream>
struct TreeNode {
    int val;
    TreeNode* left;
    TreeNode* right;
    TreeNode() : val(0), left(nullptr), right(nullptr) {}
    TreeNode(int x) : val(x), left(nullptr), right(nullptr) {}
    TreeNode(int x, TreeNode* left, TreeNode* right) : val(x), left(left), right(right) {}

};
//二叉树
//回溯，深搜
class Solution {
private:
    int dfs(TreeNode* root, bool& flag)
    {
        if (root == nullptr)return 0;
        int left = dfs(root->left, flag);
        int right = dfs(root->right, flag);
        if (flag)
            if (abs(left - right) > 1)
                flag = false;
        return 1 + max(left, right);
    }
public:
    bool isBalanced(TreeNode* root) {
        if (root == nullptr)return true;
        bool flag = true;
        dfs(root, flag);
        return flag;
    }
};