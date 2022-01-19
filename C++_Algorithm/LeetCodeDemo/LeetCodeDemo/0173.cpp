using namespace std;
#include<iostream>
#include<vector>
struct TreeNode {
    int val;
    TreeNode* left;
    TreeNode* right;
    TreeNode() : val(0), left(nullptr), right(nullptr) {}
    TreeNode(int x) : val(x), left(nullptr), right(nullptr) {}
    TreeNode(int x, TreeNode* left, TreeNode* right) : val(x), left(left), right(right) {}
};
//迭代器，二叉搜索树，二叉树的中序遍历
//数组
class BSTIterator {
private:
    int now;
    vector<TreeNode*> lst;

    TreeNode* leftlimit(TreeNode* root)
    {
        if (root->left == nullptr)return root;
        return leftlimit(root->left);
    }
    TreeNode* midsearch(TreeNode* root)
    {
        if (!root)return nullptr;

        midsearch(root->left);
        lst.push_back(root);
        midsearch(root->right);
        return root;
    }
public:
    BSTIterator(TreeNode* root) {
        lst.clear();
        auto maxleft=leftlimit(root);
        TreeNode* x = new TreeNode(maxleft->val - 1, nullptr, nullptr);
        lst.push_back(x);
        midsearch(root);
        now = 0;
    }

    int next() {
        ++now;
        return lst[now]->val;
    }

    bool hasNext() {
        return now<lst.size();
    }
};
//链表
//class BSTIterator {
//private:
//    list<TreeNode*>::iterator now;
//    list<TreeNode*> lst;
//
//    TreeNode* leftlimit(TreeNode* root)
//    {
//        if (root->left == nullptr)return root;
//        return leftlimit(root->left);
//    }
//    TreeNode* midsearch(TreeNode* root)
//    {
//        if (!root)return nullptr;
//
//        midsearch(root->left);
//        lst.push_back(root);
//        midsearch(root->right);
//        return root;
//    }
//public:
//    BSTIterator(TreeNode* root) {
//        lst.clear();
//        auto maxleft = leftlimit(root);
//        TreeNode* x = new TreeNode(maxleft->val - 1, nullptr, nullptr);
//        lst.push_back(x);
//        midsearch(root);
//        now = lst.begin();
//    }
//
//    int next() {
//        ++now;
//        return (*now)->val;
//    }
//
//    bool hasNext() {
//        return now != prev(lst.end());
//    }
//};