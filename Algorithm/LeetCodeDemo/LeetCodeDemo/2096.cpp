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
//回溯 细致条件分析
//分析在一个子树中，当只找到起点时；当只找到终点时；都找到时；都找到只在一侧子树时。
class Solution {
	int dfs(string& s,string& d,TreeNode* root, int& startValue, int& destValue)
	{
		if (!root)return 0;
		int left = dfs(s,d, root->left, startValue, destValue);
		int right = dfs(s, d, root->right, startValue, destValue);
		if (left==1) {
			s+='U';
		}
		else if (left == 2) {
			d += 'L';
		}
		if (right == 1) {
			s += 'U';
		}
		if (right == 2) {
			d += 'R';
		}
		int val=0;
		if (root->val == startValue)val = 1;
		if (root->val == destValue)val = 2;
		val = val + left + right;
		return val;
	}
public:
    string getDirections(TreeNode* root, int startValue, int destValue) {
		string s,d;
		dfs(s,d, root, startValue, destValue);
		reverse(d.begin(), d.end());

		return s+d;
    }
};