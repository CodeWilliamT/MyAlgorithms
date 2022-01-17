using namespace std; 
#include <iostream>
#include <vector>
#include <string>

//Definition for a binary tree node.
struct TreeNode {
    int val;
    TreeNode *left;
    TreeNode *right;
    TreeNode(int x) : val(x), left(NULL), right(NULL) {}
};
class Solution {
    TreeNode* ans;
    TreeNode* dfs(bool fst, bool scd, TreeNode* root, TreeNode* p, TreeNode* q)
    {
        if (root == nullptr)return nullptr;
        if (fst && scd)return root;
        if (root == p )
        {
            fst = true;
            if(scd)
                return root;
        }
        if (root == q)
        {
            scd = true;
            if(fst)
                return root;
        }
        TreeNode* left = dfs(fst,scd,root->left, p, q);
        TreeNode* right = dfs(fst, scd, root->right, p, q);
        if ((root == p||left==p||right==p) && (root==q||right == q || left == q)) 
        {
            fst = true; scd = true;
            ans = root;
            return root; 
        }
        if (left == p || left == q)
        {
            return left;
        }
        if (right == p || right == q)
        {
            return right;
        }
        return root;
    }
public:
    TreeNode* lowestCommonAncestor(TreeNode* root, TreeNode* p, TreeNode* q) {
        bool fst=false, scd= false;
        ans = nullptr;
        dfs(fst, scd, root, p, q);
        return ans;
    }
};