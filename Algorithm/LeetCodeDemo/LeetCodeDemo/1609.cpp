using namespace std;
#include <queue>
struct TreeNode {
    int val;
    TreeNode* left;
    TreeNode* right;
    TreeNode() : val(0), left(nullptr), right(nullptr) {}
    TreeNode(int x) : val(x), left(nullptr), right(nullptr) {}
    TreeNode(int x, TreeNode* left, TreeNode* right) : val(x), left(left), right(right) {}
};
//广搜 二叉树的层序遍历 队列
class Solution {
    int mx = 1000001;
    int mn = 0;
public:
    bool isEvenOddTree(TreeNode* root) {
        queue<TreeNode*> q;
        q.push(root);
        TreeNode* cur;
        int lev = 0;
        int n;
        int prev;
        while (!q.empty()) {
            n = q.size();
            prev = lev % 2 ? mx : mn;
            while (n--)
            {
                cur = q.front();
                if (lev % 2 && (cur->val%2|| prev<=cur->val ))
                    return false;
                if (lev % 2==0 && (cur->val % 2==0 || prev>=cur->val))
                    return false;
                prev = cur->val;
                q.pop();
                if (cur->left)q.push(cur->left);
                if (cur->right)q.push(cur->right);
            }
            lev++;
        }
        return true;
    }
};