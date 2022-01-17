using namespace std;
struct TreeNode {
    int val;
    TreeNode* left;
    TreeNode* right;
    TreeNode() : val(0), left(nullptr), right(nullptr) {}
    TreeNode(int x) : val(x), left(nullptr), right(nullptr) {}
    TreeNode(int x, TreeNode* left, TreeNode* right) : val(x), left(left), right(right) {}
};
//二叉树，回溯
//简单元素查找
class Solution {
public:
    TreeNode* searchBST(TreeNode* root, int val) {
        if (root== nullptr)return root;
        if (root->val == val)return root;
        auto tmp = searchBST(root->left, val);
        if (tmp != nullptr)return tmp;
        tmp = searchBST(root->right, val);
        if (tmp != nullptr)return tmp;
        return nullptr;
    }
};