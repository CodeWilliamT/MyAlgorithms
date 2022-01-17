
struct ListNode {
	int val;
	ListNode* next;
	ListNode() : val(0), next(nullptr) {}
	ListNode(int x) : val(x), next(nullptr) {}
	ListNode(int x, ListNode* next) : val(x), next(next) {}
};
//链表+细致条件分析
//需求：第偶数组，跟偶数长度的最后一组进行链表反转
class Solution {
public:
	ListNode* reverseEvenLengthGroups(ListNode* head) {
		int i = 0, len = 1;
		ListNode* cur = head, * prv = nullptr, * nxt = nullptr, * start = nullptr, * tail = nullptr;
		while (cur != nullptr)
		{
			nxt = cur->next;
			if ((len + 1) % 2)
			{
				cur->next = prv;
			}
			prv = cur;
			cur = nxt;
			i++;
			if (i == len) {
				if ((len + 1) % 2) {
					tail->next = prv;
					start->next = cur;
					tail = start;
				}
				else
				{
					 tail = prv;
				}
				i = 0; len++; start = cur;
			}
		}
		if ((len + 1) % 2 && i)
		{
			tail->next = prv;
			start->next = cur;
		}

		if (i)
		{
			if ((i + 1) % 2 && len % 2)
			{
				cur = start;
			}
			else if (i % 2 && (len + 1) % 2)
			{
				cur = prv;
				start = cur;
			}
			else
			{
				return head;
			}
			while (cur != nullptr)
			{
				nxt = cur->next;
				cur->next = prv;
				prv = cur;
				cur = nxt;
			}
			tail->next = prv;
			start->next = cur;
		}
		return head;
	}
};