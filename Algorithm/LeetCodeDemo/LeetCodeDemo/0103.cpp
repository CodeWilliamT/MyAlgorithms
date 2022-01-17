using namespace std;
#include <iostream>
#include <vector>
#include <queue>
#include <stack>

struct TreeNode {
	int val;
	TreeNode* left;
	TreeNode* right;
	TreeNode() : val(0), left(nullptr), right(nullptr) {}
	TreeNode(int x) : val(x), left(nullptr), right(nullptr) {}
	TreeNode(int x, TreeNode* left, TreeNode* right) : val(x), left(left), right(right) {}
};
//队列 栈
//正常队列层序遍历，再用个队列缓存下，单数层用个栈倒一下。
class Solution {
public:
	vector<vector<int>> zigzagLevelOrder(TreeNode* root) {
		vector<vector<int>> rst;
		if (root == nullptr)return rst;
		queue<TreeNode*> q,qtmp;
		stack<TreeNode*> sk;
		TreeNode* cur;
		q.push(root);
		int lv=0;
		while (!q.empty())
		{
			rst.push_back({});
			while (!q.empty())
			{
				cur = q.front();
				if(lv%2)
					sk.push(cur);
				else
					rst[lv].push_back(cur->val);
				if(cur->left!=nullptr)
					qtmp.push(cur->left);
				if (cur->right != nullptr)
					qtmp.push(cur->right);
				q.pop();
			}
			if (lv % 2)
				while (!sk.empty())
				{
					rst[lv].push_back(sk.top()->val);
					sk.pop();
				}
			while (!qtmp.empty())
			{
				q.push(qtmp.front());
				qtmp.pop();
			}
			lv++;
		}
		return rst;
	}
};