using namespace std;
#include <unordered_set>
struct TreeNode {
	int val;
	TreeNode* left;
	TreeNode* right;
	TreeNode() : val(0), left(nullptr), right(nullptr) {}
	TreeNode(int x) : val(x), left(nullptr), right(nullptr) {}
	TreeNode(int x, TreeNode* left, TreeNode* right) : val(x), left(left), right(right) {}
};
//哈希
class Solution {
	unordered_set<int> st;
public:
    bool findTarget(TreeNode* root, int k) {
		if (!root)return false;
		if (st.count(k - root->val))return true;
		st.insert(root->val);
		if(findTarget(root->left,k)|| findTarget(root->right,k))
			return true;
		return false;
    }
};