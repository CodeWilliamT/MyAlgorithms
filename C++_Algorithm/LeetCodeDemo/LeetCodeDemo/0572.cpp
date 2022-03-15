struct TreeNode {
	int val;
	TreeNode* left;
	TreeNode* right;
	TreeNode() : val(0), left(nullptr), right(nullptr) {}
	TreeNode(int x) : val(x), left(nullptr), right(nullptr) {}
	TreeNode(int x, TreeNode* left, TreeNode* right) : val(x), left(left), right(right) {}
};
//树结构遍历 回溯 模拟
//遍历树，一节点val根target的val相同则遍历该节点子树比对。
class Solution {
	bool checkSubtree(TreeNode* root, TreeNode* subRoot) {
		if (!root)return !subRoot;
		if (!subRoot)return false;
		if (root->val != subRoot->val)
			return false;
		bool left = 0, right = 0;
		left = checkSubtree(root->left, subRoot->left);
		right = checkSubtree(root->right, subRoot->right);
		return left && right;
	}
	bool dfs(TreeNode* root, TreeNode* subRoot) {
		if (!root)return !subRoot;
		if (!subRoot)return false;
		bool rst = false;
		if (root->val == subRoot->val) {
			rst = checkSubtree(root, subRoot);
			if (rst)
				return rst;
		}
		bool left = 0, right = 0;
		left = dfs(root->left, subRoot);
		right = dfs(root->right, subRoot);
		return left || right;
	}
public:
	bool isSubtree(TreeNode* root, TreeNode* subRoot) {
		if (!root)
			return !subRoot;
		return dfs(root, subRoot);
	}
};