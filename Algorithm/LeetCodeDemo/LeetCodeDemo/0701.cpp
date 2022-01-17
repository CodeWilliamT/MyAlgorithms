using namespace std;

//二叉树，回溯
//二叉搜索树的元素插入
class Solution {
private:
    struct TreeNode {
        int val;
        TreeNode* left;
        TreeNode* right;
        TreeNode() : val(0), left(nullptr), right(nullptr) {}
        TreeNode(int x) : val(x), left(nullptr), right(nullptr) {}
        TreeNode(int x, TreeNode* left, TreeNode* right) : val(x), left(left), right(right) {}
    };
public:
    TreeNode* insertIntoBST(TreeNode* root, int val) {
        if (root == nullptr)return new TreeNode(val);
        if (val > root->val)
        {
            if (root->right == nullptr)
            {
                root->right = new TreeNode(val);
                return root;
            }
            insertIntoBST(root->right, val);
        }
        else
        {
            if (root->left == nullptr)
            {
                root->left = new TreeNode(val);
                return root;
            }
            insertIntoBST(root->left, val);
        }
        return root;
    }
};