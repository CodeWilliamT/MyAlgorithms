using namespace std;
#include <iostream>
#include <string>
struct ListNode {
    int val;
    ListNode* next;
    ListNode() : val(0), next(nullptr) {}
    ListNode(int x) : val(x), next(nullptr) {}
    ListNode(int x, ListNode* next) : val(x), next(next) {}
};
//链表
class Solution {
public:
    bool isPalindrome(ListNode* head) {
        string s;
        auto cur = head;
        while (cur != nullptr)
        {
            s.push_back(cur->val + '0');
            cur = cur->next;
        }
        string rs = s;
        reverse(rs.begin(), rs.end());
        return s == rs;
    }
};