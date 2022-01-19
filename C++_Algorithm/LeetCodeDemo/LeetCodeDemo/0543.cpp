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
class Solution {
private:
	int ans = 0;
	int MaxDepthOfBinaryTree(TreeNode* root) {
		if (root == nullptr)return 0;
		int leftDepth = MaxDepthOfBinaryTree(root->left);
		int rightDepth = MaxDepthOfBinaryTree(root->right);
		ans = max(ans, leftDepth + rightDepth);
		return max(leftDepth, rightDepth)+1;
	}
public:
	int diameterOfBinaryTree(TreeNode* root) {
		if (root == nullptr)return 0;
		MaxDepthOfBinaryTree(root);
		return ans;
	}
};