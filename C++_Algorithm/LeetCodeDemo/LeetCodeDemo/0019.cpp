struct ListNode {
	int val;
	ListNode* next;
	ListNode() : val(0), next(nullptr) {}
	ListNode(int x) : val(x), next(nullptr) {}
	ListNode(int x, ListNode* next) : val(x), next(next) {}
};
//双指针
class Solution {
public:
    ListNode* removeNthFromEnd(ListNode* head, int n) {
		ListNode* cur=head,*pretarget=cur;
		int idx=0;
		while (cur != nullptr) {
			cur = cur->next;
			if (idx > n)
				pretarget = pretarget->next;
			idx++;
		}
		if (idx == n) {
			return head->next;
		}
		pretarget->next = pretarget->next->next;
		return head;
    }
};