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
//回溯 二叉树结构体
class Solution {
private:
	bool dfs(TreeNode* root,long long mn, long long mx)
	{
		if (root == nullptr)
			return true;
		if(root->val<=mn||root->val>=mx)
			return false;
		return dfs(root->left, mn, root->val)&& dfs(root->right, root->val, mx);
	}
public:
    bool isValidBST(TreeNode* root) {
		return dfs(root, LONG_MIN, LONG_MAX);
    }
};