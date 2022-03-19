using namespace std;
#include <string>
struct TreeNode {
	int val;
	TreeNode* left;
	TreeNode* right;
	TreeNode() : val(0), left(nullptr), right(nullptr) {}
	TreeNode(int x) : val(x), left(nullptr), right(nullptr) {}
	TreeNode(int x, TreeNode* left, TreeNode* right) : val(x), left(left), right(right) {}
};
//回溯 二叉树 模拟
//让干啥干啥
class Solution {
	string rst;
	void dfs(TreeNode* root) {
		rst.push_back('(');
		if (root) {
			rst += to_string(root->val);
			if (root->left || root->right)
				dfs(root->left);
			if (root->right)
				dfs(root->right);
		}
		rst.push_back(')');
	}
public:
	string tree2str(TreeNode* root) {
		if (root) {
			rst += to_string(root->val);
			if (root->left || root->right)
				dfs(root->left);
			if (root->right)
				dfs(root->right);
		}
		return rst;
	}
};