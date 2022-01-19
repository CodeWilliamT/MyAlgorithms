using namespace std;
#include <iostream>
#include <unordered_set>
struct ListNode {
    int val;
    ListNode* next;
    ListNode(int x) : val(x), next(NULL) {}
};

//Usually hash
class Solution {
public:
    ListNode* detectCycle(ListNode* head) {
        unordered_set<ListNode*> set;
        ListNode* cur = head;
        while (cur)
        {
            if (set.count(cur))return cur;
            set.insert(cur);
            cur = cur->next;
        }
        return NULL;
    }
};

//fast&slow ptr
//class Solution {
//public:
//    ListNode* detectCycle(ListNode* head) {
//        ListNode* slow = head;
//        ListNode* fast = head;
//        while (slow&&fast)
//        {
//            slow = slow->next;
//            if(!fast->next)return NULL;
//            fast = fast->next->next;
//            if (slow == fast)
//            {
//                ListNode* ptr = head;
//                while (ptr != slow) {
//                    ptr = ptr->next;
//                    slow = slow->next;
//                }
//                return ptr;
//            }
//        }
//        return NULL;
//    }
//};
