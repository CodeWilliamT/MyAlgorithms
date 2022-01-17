using namespace std;
#include <iostream>
#include <vector>
// Definition for a Node.
class Node {
public:
    int val;
    vector<Node*> children;

    Node() {}

    Node(int _val) {
        val = _val;
    }

    Node(int _val, vector<Node*> _children) {
        val = _val;
        children = _children;
    }
};
//树结构，回溯
//返回所有子树返回的深度中最大的那个，没节点则特殊处理。
class Solution {
public:
    int maxDepth(Node* root,int depth=1) {
        if (root==nullptr)return 0;
        int rst = depth;
        for (auto& c : root->children)
        {
            rst =max(maxDepth(c, depth+1), rst);
        }
        return rst;
    }
};