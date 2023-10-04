

struct TreeNode {
	int val;
	TreeNode* left;
	TreeNode* right;
	TreeNode() : val(0), left(nullptr), right(nullptr) {}
	TreeNode(int x) : val(x), left(nullptr), right(nullptr) {}
	TreeNode(int x, TreeNode* left, TreeNode* right) : val(x), left(left), right(right) {}
};

//二叉树 简单模拟
//合并二叉树
class Solution {
public:
    TreeNode* mergeTrees(TreeNode* root1, TreeNode* root2) {
		if (!root1 && !root2)return nullptr;
		TreeNode* rst = new TreeNode();
		
		rst->val=(root1?root1->val:0)+(root2?root2->val:0);

		rst->left=mergeTrees(root1?root1->left:nullptr, root2?root2->left:nullptr);
		rst->right=mergeTrees(root1 ? root1->right : nullptr, root2?root2->right : nullptr);
		return rst;
    }
};