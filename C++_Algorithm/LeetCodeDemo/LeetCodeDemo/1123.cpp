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
#include "myAlgo\Structs\TreeNode.cpp"
//struct TreeNode {
//    int val;
//    TreeNode* left;
//    TreeNode* right;
//    TreeNode() : val(0), left(nullptr), right(nullptr) {}
//    TreeNode(int x) : val(x), left(nullptr), right(nullptr) {}
//    TreeNode(int x, TreeNode* left, TreeNode* right) : val(x), left(left), right(right) {}
//};
//深搜 二叉树
//即求左右子树最大深度和最大的节点
//遍历树，找到左右子树深度和最大的节点。
class Solution {
    TreeNode* rst;
    int dpmx = 0;
    int treeDeep(TreeNode* root,int deep) {
        if (!root)return deep-1;
        int left, right;
        left = treeDeep(root->left,deep+1);
        right = treeDeep(root->right,deep+1);
        dpmx = max(dpmx, deep);
        if (left==right&&left == dpmx) {
            rst = root;
        }
        return max({ left,right,deep });
    }

public:
    TreeNode* lcaDeepestLeaves(TreeNode* root) {
        rst = root;
        treeDeep(root, 0);
        return rst;
    }
};