using namespace std;
#include <iostream>
#include <unordered_map>
class Node {
public:
    int val;
    Node* next;
    Node* random;

    Node(int _val) {
        val = _val;
        next = NULL;
        random = NULL;
    }
};
class Solution {
public:
    unordered_map<Node*,Node *> visitedhash;
    Node* copyRandomList(Node* head) {
        if (head == nullptr)
        {
            return nullptr;
        }
        auto iter = visitedhash.find(head);
        if (iter != visitedhash.end())
        {
            return iter->second;
        }
        Node* ans = new Node(head->val);
        visitedhash.insert(make_pair(head, ans));
        ans->next = copyRandomList(head->next);
        ans->random = copyRandomList(head->random);
        return ans;
    }
};