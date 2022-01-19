using namespace std;
struct ListNode {
	int val;
	ListNode* next;
	ListNode() : val(0), next(nullptr) {}
	ListNode(int x) : val(x), next(nullptr) {}
	ListNode(int x, ListNode* next) : val(x), next(next) {}
};
//快慢指针
class Solution {
public:
    ListNode* deleteMiddle(ListNode* head) {
		if (head == nullptr || head->next == nullptr)return nullptr;
		ListNode* f,*s,*pre;
		int i = 0;
		f = head;
		s = head;
		while (s!=nullptr)
		{
			if (i % 2) {
				pre = f;
				f = f->next;
			}
			s = s->next;
			i++;
		}
		if (pre != nullptr)pre->next = f->next;
		return head;
    }
};