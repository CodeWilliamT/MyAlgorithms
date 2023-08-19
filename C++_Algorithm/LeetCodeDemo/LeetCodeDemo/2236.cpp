struct TreeNode {
	int val;
	TreeNode* left;
	TreeNode* right;
	TreeNode() : val(0), left(nullptr), right(nullptr) {}
	TreeNode(int x) : val(x), left(nullptr), right(nullptr) {}
	TreeNode(int x, TreeNode* left, TreeNode* right) : val(x), left(left), right(right) {}
};
//简单题 与2235并列弱智题
class Solution {
public:
	bool checkTree(TreeNode* root) {
		return root->val == (root->left->val + root->right->val);
	}
};