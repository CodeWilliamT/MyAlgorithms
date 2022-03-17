using namespace std;
#include<iostream>
#include<unordered_set>

struct ListNode {
    int val;
    ListNode* next;
    ListNode(int x) : val(x), next(NULL) {}
};

//迭代
class Solution {
public:
    unordered_set<ListNode*> posFlag;
    bool hasCycle(ListNode* head) {
        ListNode* cur = head;
        unordered_set<ListNode*> st;
        while (cur != nullptr)
        {
            if (st.count(cur))return true;
            st.insert(cur);
            cur = cur->next;
        }
        return false;
    }
};
//递归
class Solution {
public:
    unordered_set<ListNode*> posFlag;
    bool hasCycle(ListNode* head) {
        if (head == nullptr)return false;
        if (posFlag.find(head) != posFlag.end())return true;
        posFlag.insert(head);
        return hasCycle(head->next);
    }
};


//fast&slow ptr
class Solution {
public:
    bool hasCycle(ListNode* head) {
        ListNode* slow = head;
        ListNode* fast = head;
        while (slow && fast)
        {
            slow = slow->next;
            if(!fast->next)return false;
            fast = fast->next->next;
            if (slow == fast)return true;
        }
        return false;
    }
};