using namespace std;
#include <iostream>

 //Definition for singly-linked list.
struct ListNode {
    int val;
    ListNode* next;
    ListNode() : val(0), next(nullptr) {}
    ListNode(int x) : val(x), next(nullptr) {}
    ListNode(int x, ListNode* next) : val(x), next(next) {}
};
//迭代
class Solution {
public:
    ListNode* reverseList(ListNode* head) {
        ListNode* pre = nullptr;
        ListNode* cur = head;
        ListNode* next;
        while (cur) {
            next = cur->next;
            cur->next = pre;
            pre = cur;
            cur = next;
        }
        return pre;
    }
};
//递归
//class Solution {
//public:
//    ListNode* reverseList(ListNode* head) {
//        if (!head)return head;
//        if (!head->next)return head;
//        ListNode* newHead = reverseList(head->next);
//        head->next->next = head;
//        head->next = nullptr;
//        return newHead;
//    }
//};