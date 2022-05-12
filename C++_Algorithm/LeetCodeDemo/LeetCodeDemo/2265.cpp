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
//回溯 二叉树
typedef pair<int, int> pii;
class Solution {
public:
    int averageOfSubtree(TreeNode* root) {
		int rst = 0;
		function<pii(TreeNode*)> dfs = [&](TreeNode* node) {
			if (!node)return pii{0,0};
			pii left = dfs(node->left);
			pii right = dfs(node->right);
			int cnt = 1 + left.first + right.first;
			int sum = node->val + left.second + right.second;
			if (sum / cnt == node->val)rst++;
			return pii{ cnt,sum};
		};
		dfs(root);
		return rst;
    }
};