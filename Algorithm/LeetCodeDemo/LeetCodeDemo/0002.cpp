/**
 * Definition for singly-linked list.
 */

#include <iostream>
using namespace std;

 struct ListNode {
    int val;
    ListNode *next;
    ListNode(int x) : val(x), next(NULL) {}
    };
class Solution {
public:
    ListNode* addTwoNumbers(ListNode* l1, ListNode* l2) {
        int digit, sum;
        ListNode* ans = NULL, * now = NULL, * node;
        ListNode* l1head, * l2head;
        l1head = l1;
        l2head = l2;
        sum = 0;
        while (l1head != NULL || l2head != NULL || sum != 0)
        {
            if (l1head != NULL)
            {
                sum += l1head->val;
                l1head = l1head->next;
            }
            if (l2head != NULL)
            {
                sum += l2head->val;
                l2head = l2head->next;
            }
            digit = sum % 10;
            node = new ListNode(digit);
            if (ans == NULL)
            {
                ans = node;
            }
            if (now != NULL)now->next = node;
            now = node;
            sum = sum / 10;
        }
        return ans;
    }
};

//int main()
//{
//    Solution s;
//    int n;
//    while (cin >> n)
//        cout << s.hammingWeight(n) << endl;
//    return 0;
//}