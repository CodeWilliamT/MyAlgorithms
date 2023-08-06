using namespace std;
#include <iostream>
#include <vector>
#include <string>
#include <algorithm>
#include <unordered_set>
#include <unordered_map>
#include <set>
#include <map>
#include <queue>
#include <stack>
#include <functional>
#include <bitset>
#include "TreeNode.cpp"
typedef long long ll;
typedef pair<ll, ll> pll;
typedef pair<int, int> pii;
//struct TreeNode {
//	int val;
//	TreeNode* left;
//	TreeNode* right;
//	TreeNode() : val(0), left(nullptr), right(nullptr) {}
//	TreeNode(int x) : val(x), left(nullptr), right(nullptr) {}
//	TreeNode(int x, TreeNode* left, TreeNode* right) : val(x), left(left), right(right) {}
//};
//深搜 回溯
//二叉树上距离某点的最大深度
//深搜
//比较 目标点所在分支最大深度-目标点深度， 根的不含目标点的分支的最大深度+目标点深度 的最大值。
class Solution {
	int start;
	int rst=0,tgtlvl;
	typedef pair<bool, int> pbi;
public:
	pbi dfs(TreeNode* root, bool found, int lvl)
	{
		if (root->val == start) {
			found = true;
			tgtlvl = lvl;
		}
		if (found)
			rst = max(rst, lvl - tgtlvl);
		if (!root->left && !root->right)
			return { root->val == start,lvl };
		pbi left = { false,lvl }, right = { false,lvl };
		if (root->left)
			left = dfs(root->left, found, lvl + 1);
		if (root->right)
			right = dfs(root->right, found, lvl + 1);
		if(left.first^right.first)
			rst = max({ rst, (left.first ? right.second : left.second)+ tgtlvl - 2 * lvl });
		return { root->val == start|| left.first || right.first,max(left.second,right.second)};
	}

    int amountOfTime(TreeNode* root, int start) {
		this->start = start;
		dfs(root, false, 0);
		return rst;
    }
};