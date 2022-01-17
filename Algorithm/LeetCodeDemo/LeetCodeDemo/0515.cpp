using namespace std;
#include <iostream>
#include <vector>
#include <string>
#include <queue>

//简单广搜

//状态参数存储结构
class Solution {
private:
    struct TreeNode {
        int val;
        TreeNode* left;
        TreeNode* right;
        TreeNode() : val(0), left(nullptr), right(nullptr) {}
        TreeNode(int x, TreeNode* left, TreeNode* right) : val(x), left(left), right(right) {}
    };
public:
    vector<int> largestValues(TreeNode* root) {
        if (!root)return {};
        //声明状态队列
        queue<TreeNode*>q;
        vector<int> ans;
        TreeNode* cur;
        //压入初始状态
        q.push(root);
        while (!q.empty())
        {
            //读取当前状态
            int count = q.size();
            int m = q.front()->val;
            //进行路径尝试，压入尝试后状态
            for (int i = 0; i < count; i++)
            {
                cur = q.front();
                q.pop();
                m = max(cur->val, m);
                if (cur->left)
                    q.push(cur->left);
                if (cur->right)
                    q.push(cur->right);
            }
            ans.push_back(m);
        }
        return ans;
    }
};