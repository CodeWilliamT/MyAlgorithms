using namespace std;
#include <iostream>
#include "myAlgo\Structs\TreeNode.cpp"
//动态规划 回溯(深搜)
class Solution {
    struct RstNode {
        int selected = 0;
        int noSelected = 0;
    };
public:
    RstNode dfs(TreeNode* cur) {
        if (!cur)//判定状态可行性，若状态不可行，则跳过
            return { 0,0 };
        RstNode l = dfs(cur->left);
        RstNode r = dfs(cur->right);
        return { cur->val + l.noSelected + r.noSelected,max(l.noSelected,l.selected) + max(r.noSelected,r.selected) };
    }
    int rob(TreeNode* root) {
        RstNode rst = dfs(root);
        return max(rst.selected, rst.noSelected);
    }
};