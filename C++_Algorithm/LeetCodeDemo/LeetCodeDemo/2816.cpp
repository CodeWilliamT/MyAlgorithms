using namespace std;
#include <iostream>
#include <vector>
#include <string>
#include <algorithm>
#include <unordered_set>
#include <unordered_map>
#include <set>
#include <map>
#include <queue>
#include <stack>
#include <functional>
#include <bitset>
struct ListNode {
    int val;
    ListNode* next;
    ListNode() : val(0), next(nullptr) {}
    ListNode(int x) : val(x), next(nullptr) {}
    ListNode(int x, ListNode* next) : val(x), next(next) {}
    
};
//链表加法,递归理解
class Solution {
    int dfs(ListNode* node) {
        node->val = node->val * 2;
        if (node->next) {
            node->val += dfs(node->next);
        }
        if (node->val > 9) {
            node->val %= 10;
            return 1;
        }
        else {
            return 0;
        }
    }

public:
    ListNode* doubleIt(ListNode* head) {
        if (dfs(head)) {
            return new ListNode(1, head);
        }
        return head;
    }
};