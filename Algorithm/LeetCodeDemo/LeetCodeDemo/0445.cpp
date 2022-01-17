using namespace std;
#include <iostream>
#include <vector>
struct ListNode {
	int val;
	ListNode* next;
	ListNode() : val(0), next(nullptr) {}
	ListNode(int x) : val(x), next(nullptr) {}
	ListNode(int x, ListNode* next) : val(x), next(next) {}
};
//麻烦题 朴素实现
//遍历存入数组，倒序处理。
class Solution {
public:
    ListNode* addTwoNumbers(ListNode* l1, ListNode* l2) {
		vector<int> tmp1, tmp2;
		ListNode* cur=l1;
		while (cur != nullptr)
		{
			tmp1.push_back(cur->val);
			cur = cur->next;
		}
		cur = l2;
		while (cur != nullptr)
		{
			tmp2.push_back(cur->val);
			cur = cur->next;
		}
		int flag = 0,v;

		ListNode* next = nullptr;
		while (flag||!tmp1.empty() || !tmp2.empty()) {
			v = flag;
			if (!tmp1.empty()) {
				v += tmp1.back();
				tmp1.pop_back();
			}
			if (!tmp2.empty()) {
				v += tmp2.back();
				tmp2.pop_back();
			}
			flag = v / 10;
			v = v % 10;
			cur = new ListNode(v, next);
			next = cur;
		}
		return cur;
    }
};