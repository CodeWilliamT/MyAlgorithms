using namespace std;
#include <iostream>
//细致条件分析，回溯，二叉树, 中序遍历
struct TreeNode {
    int val;
    TreeNode* left;
    TreeNode* right;
    TreeNode(int x) : val(x), left(NULL), right(NULL) {}
    
};
class Solution {
public:
    TreeNode* inorderSuccessor(TreeNode* root, TreeNode* p) {
        if (root == nullptr)return root;
        TreeNode* rst = nullptr;
        TreeNode* left = inorderSuccessor(root->left, p);
        if (left!=nullptr&&left->val > p->val &&(rst == nullptr||rst && left->val < rst->val))rst=left;
        if (rst == nullptr && root->val > p->val||(rst&& root->val<rst->val))rst=root;
        TreeNode* right = inorderSuccessor(root->right, p);
        if (right != nullptr && right->val > p->val&& (rst == nullptr || rst && right->val < rst->val))rst=right;
        return rst;
    }
};