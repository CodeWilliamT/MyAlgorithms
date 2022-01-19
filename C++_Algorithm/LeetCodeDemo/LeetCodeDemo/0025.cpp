struct ListNode {
    int val;
    ListNode* next;
    ListNode() : val(0), next(nullptr) {}
    ListNode(int x) : val(x), next(nullptr) {}
    ListNode(int x, ListNode* next) : val(x), next(next) {}
};
//细致条件处理
//链表局部翻转
class Solution {
public:
    ListNode* reverseKGroup(ListNode* head, int k) {
        ListNode* cur = head, * prev = nullptr, * next, * tail, * pretail;
        int idx = 0, ri = 0;
        while (cur != nullptr)
        {
            tail = cur;
            ri = idx;
            while (idx < ri + k && cur != nullptr)
            {
                cur = cur->next;
                idx++;
            }
            if (idx < ri + k)
                return head;
            idx = ri;
            cur = tail;
            while (idx < ri + k && cur != nullptr)
            {
                next = cur->next;
                cur->next = prev;
                prev = cur;
                cur = next;
                idx++;
            }
            tail->next = cur;
            if (!ri)head = prev;
            else pretail->next = prev;
            pretail = tail;
            prev = nullptr;
        }
        return head;
    }
};