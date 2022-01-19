struct ListNode {
    int val;
    ListNode* next;
    ListNode() : val(0), next(nullptr) {}
    ListNode(int x) : val(x), next(nullptr) {}
    ListNode(int x, ListNode* next) : val(x), next(next) {}
};
//链表，多指针
//双指针轮流加入
class Solution {
public:
    ListNode* mergeTwoLists(ListNode* l1, ListNode* l2) {
        if (l1 == nullptr)return l2;
        if (l2 == nullptr)return l1;
        ListNode* root=l1->val<l2->val?l1:l2;
        if (l1->val < l2->val)
        {
            root = l1;
            l1 = l1->next;
        }
        else
        {
            root = l2;
            l2 = l2->next;
        }
        ListNode* cur=root;
        while (l1 != nullptr || l2 != nullptr)
        {
            if (l1 == nullptr)
            {
                cur->next = l2;
                cur = cur->next;
                l2 = l2->next;
            }
            else if (l2 == nullptr)
            {
                cur->next = l1;
                cur = cur->next;
                l1 = l1->next;
            }
            else if (l1->val < l2->val)
            {
                cur->next = l1;
                cur = cur->next;
                l1 = l1->next;
            }
            else
            {
                cur->next = l2;
                cur = cur->next;
                l2 = l2->next;
            }
        }
        return root;
    }
};