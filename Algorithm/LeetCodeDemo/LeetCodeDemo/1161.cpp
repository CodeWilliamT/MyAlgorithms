using namespace std;
#include <queue>
//二叉树
struct TreeNode {
    int val;
    TreeNode* left;
    TreeNode* right;
    TreeNode() : val(0), left(nullptr), right(nullptr) {}
    TreeNode(int x) : val(x), left(nullptr), right(nullptr) {}
    TreeNode(int x, TreeNode* left, TreeNode* right) : val(x), left(left), right(right) {}
    
};
//深搜
class Solution {
public:
    void inorder(TreeNode* node, int level, int& n, int* levelSum) {
        //对当前节点的判定与操作
        if (node == nullptr)return;
        n = max(level, n);
        levelSum[level] += node->val;
        //下一层路径尝试
        inorder(node->left, level + 1, n, levelSum);
        inorder(node->right, level + 1, n, levelSum);
        //全部结果处理
    }

    int maxLevelSum(TreeNode* root) {
        int levelSum[10001]{};
        int n = 1;
        inorder(root, 1,n, levelSum);
        int maxIdx = 1;
        for (int i = 1; i < n; ++i)
            maxIdx = levelSum[i] > levelSum[maxIdx] ? i : maxIdx;
        return maxIdx;
    }
};
//广搜
//struct QNode {
//    TreeNode* treenode;
//    int level;
//    QNode(TreeNode* a, int b) :treenode(a), level(b) {};
//};
//class Solution {
//public:
//    int maxLevelSum(TreeNode* root) {
//        queue<QNode*> q;
//        QNode* cur = new QNode(root, 1);
//        q.push(cur);
//        int sum = 0;
//        int ansSum = root->val;
//        int anslev = 1;
//        int curlev = 1;
//        while (!q.empty())
//        {
//            cur = q.front();
//            q.pop();
//            if (cur->level != curlev)
//            {
//                if (sum > ansSum)
//                {
//                    ansSum = sum;
//                    anslev = curlev;
//                }
//                curlev = cur->level;
//                sum = 0;
//            }
//            sum += cur->treenode->val;
//            if (cur->treenode->left != nullptr)
//                q.push(new QNode(cur->treenode->left, cur->level + 1));
//            if (cur->treenode->right != nullptr)
//                q.push(new QNode(cur->treenode->right, cur->level + 1));
//        }
//        if (sum > ansSum)
//        {
//            ansSum = sum;
//            anslev = curlev;
//        }
//        return anslev;
//    }
//};