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
//二叉树、递归回溯深搜
class Solution {
private:
    int dfs(TreeNode* root, int& ans) {
        if (root == nullptr)return 0;
        int left = dfs(root->left, ans);
        int right = dfs(root->right, ans); ans += abs(left - right);
        return root->val + left + right;
    }
public:
    int findTilt(TreeNode* root) {
        if (root == nullptr)return 0;
        int ans = 0;
        dfs(root, ans);
        return ans;
    }
};