using namespace std;
#include <vector>
struct ListNode {
    int val;
    ListNode* next;
    ListNode() : val(0), next(nullptr) {}
    ListNode(int x) : val(x), next(nullptr) {}
    ListNode(int x, ListNode* next) : val(x), next(next) {}
};
//朴素模拟 模拟随机 链表
//空间O(n),初始化O(n),查询O(1)
class Solution {
    vector<ListNode*> data;
public:
    Solution(ListNode* head) {
        for (ListNode* node = head; node; node = node->next) {
            data.push_back(node);
        }
    }

    int getRandom() {
        return data[rand() % data.size()]->val;
    }
};

//空间O(1),初始化O(1),查询O(n)
//class Solution {
//    ListNode* head;
//public:
//    Solution(ListNode* head) {
//        this->head = head;
//    }
//
//    int getRandom() {
//        int i = 1, ans = 0;
//        for (auto node = head; node != nullptr; node = node->next) {
//            if (rand() % i == 0) { // 1/i 的概率选中（替换为答案）
//                ans = node->val;
//            }
//            ++i;
//        }
//        return ans;
//    }
//};