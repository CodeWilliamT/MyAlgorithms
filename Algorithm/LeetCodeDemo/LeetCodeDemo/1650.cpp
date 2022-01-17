using namespace std;
#include <iostream>
#include <vector>
#include <string>
#include <unordered_map>
//哈希
class Node {
public:
    int val;
    Node* left;
    Node* right;
    Node* parent;
};

class Solution {
public:
    Node* lowestCommonAncestor(Node* p, Node* q) {
        unordered_map<int, Node*> parents;
        Node* cur, *root;
        for (cur = p; cur != nullptr; cur = cur->parent)
        {
            parents[cur->val] = cur;
        }
        for (cur = q; cur != nullptr; cur = cur->parent)
        {
            if (parents.count(cur->val))
            {
                return cur;
            }
        }
        return nullptr;
    }
};