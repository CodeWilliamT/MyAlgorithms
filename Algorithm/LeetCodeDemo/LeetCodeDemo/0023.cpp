using namespace std;
#include <iostream>
#include <vector>
#include <queue>
struct ListNode {
	int val;
	ListNode* next;
	ListNode() : val(0), next(nullptr) {}
	ListNode(int x) : val(x), next(nullptr) {}
	ListNode(int x, ListNode* next) : val(x), next(next) {}
};
//优先队列
class Solution {
public:
	ListNode* mergeKLists(vector<ListNode*>& lists) {
		priority_queue<int,vector<int>,greater<int>> pq;
		ListNode* cur;
		for (auto& l : lists)
		{
			cur = l;
			while (cur != nullptr)
			{
				pq.push(cur->val);
				cur = cur->next;
			}
		}
		if(pq.empty())return nullptr;
		ListNode* rst=new ListNode(pq.top());
		pq.pop();
		cur = rst;
		while (!pq.empty())
		{
			cur->next= new ListNode(pq.top());
			pq.pop();
			cur = cur->next;
		}
		return rst;
	}
};