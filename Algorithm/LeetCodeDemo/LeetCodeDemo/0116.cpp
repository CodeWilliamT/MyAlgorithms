using namespace std;
#include <iostream>
#include <queue>

// Definition for a Node.
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

//巧思 深搜 回溯
//利用已建立的指针
class Solution {
public:
    Node* connect(Node* root, Node* parent=nullptr) {
        if (root == nullptr)return nullptr;
        if (parent)
            if (root == parent->left)
                root->next = parent->right;
            else if(parent->next)
                root->next = parent->next->left;

        connect(root->right,root);
        connect(root->left, root);
        return root;
    }
};
//深搜 回溯
//倒序深搜，利用完美二叉树性质 拿数组存之前的记录
//class Solution {
//    Node* v[4096]{};
//    bool bitSize(int num)
//    {
//        while (num % 2 == 0)
//        {
//            num /= 2;
//        }
//        return num == 1;
//    }
//public:
//    Node* connect(Node* root,int idx=0) {
//        if (root == nullptr)return nullptr;
//        if(!bitSize(idx+2))root->next = v[idx + 1];
//        v[idx] = root;
//        connect(root->right, idx * 2 + 2);
//        connect(root->left, idx * 2 + 1);
//        return root;
//    }
//};
//
//广搜
//倒序层序遍历
//class Solution {
//public:
//    Node* connect(Node* root) {
//        queue<Node*> q;
//        if (root)
//            q.push(root);
//        Node* cur, * pre;
//        int n;
//        while (!q.empty())
//        {
//            pre = q.front();
//            q.pop();
//            n = q.size();
//            if (pre->right)
//                q.push(pre->right);
//            if (pre->left)
//                q.push(pre->left);
//            while (n--)
//            {
//                cur = q.front();
//                q.pop();
//                cur->next = pre;
//                pre = cur;
//                if (cur->right)
//                    q.push(cur->right);
//                if (cur->left)
//                    q.push(cur->left);
//            }
//        }
//        return root;
//    }
//};