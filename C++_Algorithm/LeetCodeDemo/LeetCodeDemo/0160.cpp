using namespace std;
#include <iostream>
#include <unordered_set>
struct ListNode {
    int val;
    ListNode* next;
    ListNode(int x) : val(x), next(NULL) {}
    
};
//双指针
//每步操作需要同时更新指针 \textit{pA}pA 和 \textit{pB}pB。
//如果指针pA 不为空，则将指针pA 移到下一个节点；如果指针pB 不为空，则将指针pB 移到下一个节点。
//如果指针pA 为空，则将指针pA 移到链表headB 的头节点；如果指针pB 为空，则将指针pB 移到链表headA 的头节点。
//当指针pA 和pB 指向同一个节点或者都为空时，返回它们指向的节点或者null
class Solution {
public:
    ListNode* getIntersectionNode(ListNode* headA, ListNode* headB) {
        if (headA == nullptr || headB == nullptr) {
            return nullptr;
        }
        ListNode* pA = headA, * pB = headB;
        while (pA != pB) {
            pA = pA == nullptr ? headB : pA->next;
            pB = pB == nullptr ? headA : pB->next;
        }
        return pA;
    }
};
//哈希
//哈希记录一个轨迹，然后比对另一个
//class Solution {
//public:
//    ListNode* getIntersectionNode(ListNode* headA, ListNode* headB) {
//        unordered_set<ListNode*> st;
//        ListNode* cur = headA;
//        while (cur)
//        {
//            st.insert(cur);
//            cur = cur->next;
//        }
//        cur = headB;
//        while (cur)
//        {
//            if (st.count(cur))return cur;
//            cur = cur->next;
//        }
//        return cur;
//    }
//};
