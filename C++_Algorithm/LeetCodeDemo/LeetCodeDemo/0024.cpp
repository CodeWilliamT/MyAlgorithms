struct ListNode {
	int val;
	ListNode* next;
	ListNode() : val(0), next(nullptr) {}
	ListNode(int x) : val(x), next(nullptr) {}
	ListNode(int x, ListNode* next) : val(x), next(next) {}
};
//链表
//交换相隔节点
//(0)-1-2-3-4
//1-0-2-3-4
//1-0(2)-3-4
class Solution {
public:
	ListNode* swapPairs(ListNode* head) {
		if (!head || !head->next)return head;
		int idx = 0;
		auto cur = head;
		auto pre = head->next;
		auto next = pre->next;
		head = head->next;
		while (cur) {
			if (idx % 2 == 0) {
				pre->next = cur;
				cur->next = next;
				pre = cur;
				cur = next;
				if (!next || !next->next)
					break;
				next = next->next;
			}
			else {
				pre->next = next;
				pre = next;
				next = next->next;
			}
			idx++;
		}
		return head;
	}
};