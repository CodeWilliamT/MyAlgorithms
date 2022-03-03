class Node {
public:
    int val;
    Node* left;
    Node* right;
    Node* next;
    Node() : val(0), left(NULL), right(NULL), next(NULL) {}
    Node(int _val) : val(_val), left(NULL), right(NULL), next(NULL) {}
    Node(int _val, Node* _left, Node* _right, Node* _next)
        : val(_val), left(_left), right(_right), next(_next) {}
};
//二叉树层序遍历 广搜 
class Solution {
public:
    Node* connect(Node* root) {
        Node* pre,*cur;
        queue<Node*> q;
        if(root)q.push(root);
        int len;
        while (!q.empty()) {
            len = q.size();
            for(int i=0;i<len;i++){
                cur = q.front();
                q.pop();
                if(cur->left)q.push(cur->left);
                if(cur->right)q.push(cur->right);
                if (i>0) {
                    pre->next = cur;
                }
                pre = cur;
            }
        }
        return root;
    }
};