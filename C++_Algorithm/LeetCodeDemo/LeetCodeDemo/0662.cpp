using namespace std;
#include <iostream>
#include <vector>
#include <string>
#include <unordered_map>
#include <queue>
//Definition for a binary tree node.
struct TreeNode {
    int val;
    TreeNode* left;
    TreeNode* right;
    TreeNode() : val(0), left(nullptr), right(nullptr) {}
    TreeNode(int x) : val(x), left(nullptr), right(nullptr) {}
    TreeNode(int x, TreeNode* left, TreeNode* right) : val(x), left(left), right(right) {}
};
//广搜
class Solution {
public:
    int widthOfBinaryTree(TreeNode* root) {
        if (root == nullptr)return 0;
        int ans = 0;
        queue<TreeNode*> q;
        TreeNode* cur;
        int d = 0;
        root->val = 0;
        q.push(root);
        while (!q.empty())
        {
            d = q.front()->val;
            ans = max(q.back()->val - d + 1, ans);
            int n = q.size();
            for (int i = 0; i < n; i++)
            {
                cur = q.front();
                cur->val -= d;
                q.pop();
                if (cur->left)
                {
                    cur->left->val = cur->val * 2;
                    q.push(cur->left);
                }
                if (cur->right)
                {
                    cur->right->val = cur->val * 2 + 1;
                    q.push(cur->right);
                }
            }
        }
        return ans;
    }
};


//深搜
//class Solution {
//    int64_t ans;
//    unordered_map<int, int64_t> left;
//    void dfs(TreeNode* root, int d, int64_t dis)
//    {
//        if (root == nullptr)return;
//        if (!left.count(d))
//        {
//            left[d] = dis;
//        }
//        dis = dis - left[d];
//        ans = max(ans, dis+ 1);
//        solve(root->left, d + 1, dis * 2);
//        solve(root->right, d + 1, dis * 2 + 1);
//    }
//public:
//    int widthOfBinaryTree(TreeNode* root) {
//        if (root == nullptr)return 0;
//        ans = 0;
//        solve(root, 0, 0);
//        return ans;
//    }
//};
//
