using namespace std;
#include <iostream>
struct TreeNode {
	int val;
	TreeNode* left;
	TreeNode* right;
	TreeNode(int x) : val(x), left(NULL), right(NULL) {}
};
//二叉树，回溯
class Solution {
public:
	TreeNode* lowestCommonAncestor(TreeNode* root, TreeNode* p, TreeNode* q) {
		if (root == nullptr)
			return root;
		if (root->val == p->val || root->val == q->val)
			return root;
		auto left = lowestCommonAncestor(root->left, p, q);
		auto right = lowestCommonAncestor(root->right, p, q);
		if (left != nullptr && right != nullptr)
			return root;
		if (left != nullptr)
			return left;
		if (right != nullptr)
			return right;
		return nullptr;
	}
};