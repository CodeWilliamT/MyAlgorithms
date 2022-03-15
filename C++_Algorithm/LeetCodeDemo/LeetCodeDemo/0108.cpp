struct TreeNode {
	int val;
	TreeNode* left;
	TreeNode* right;
	TreeNode() : val(0), left(nullptr), right(nullptr) {}
	TreeNode(int x) : val(x), left(nullptr), right(nullptr) {}
	TreeNode(int x, TreeNode* left, TreeNode* right) : val(x), left(left), right(right) {}
};
//回溯 两分 树结构遍历
//无限中点构造
class Solution {
	void dfs(vector<int>& nums,int l,int r, TreeNode* root) {
		if (l > r)return;
		int mid = (l + r) / 2;
		root->val = nums[mid];
		if (l <= mid - 1) {
			root->left = new TreeNode();
			dfs(nums, l, mid - 1, root->left);
		}
		if (mid + 1 <= r) {
			root->right = new TreeNode();
			dfs(nums, mid + 1, r, root->right);
		}
	}
public:
	TreeNode* sortedArrayToBST(vector<int>& nums) {
		TreeNode* root=new TreeNode();
		dfs(nums,0, nums.size() - 1,root);
		return root;
	}
};