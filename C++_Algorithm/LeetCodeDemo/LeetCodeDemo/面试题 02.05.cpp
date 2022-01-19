using namespace std;
#include <iostream>
#include <vector>
#include <string>

//Definition for singly-linked list.
struct ListNode {
    int val;
    ListNode *next;
    ListNode(int x) : val(x), next(NULL) {}
};
 
class Solution {
    string readString(ListNode* root)
    {
        string a = "";
        ListNode* cur = root;
        while (cur != nullptr)
        {
            a.push_back(cur->val + '0');
            cur = cur->next;
        }
        return a;
    }
    string stringAdd(string s1, string s2)
    {
        string s = "";
        int len1 = s1.size();
        int len2 = s2.size();
        int len = max(len1, len2);
        int n1 = 0, n2 = 0;
        bool flag = 0;
        for (int i = 0; i < len; i++)
        {
            if (i < len1)n1 = s1[i] - '0';
            if (i < len2)n2 = s2[i] - '0';
            n1 = n1 + n2 + flag;
            flag = n1 > 9;
            n1 %= 10;
            s.push_back(n1 + '0');
            n1 = 0;
            n2 = 0;
        }
        if (flag)s.push_back('1');
        return s;
    }
    ListNode* writeList(string s)
    {
        if (s == "")return nullptr;
        ListNode* root = new ListNode(s[0] - '0');
        ListNode* cur = root;
        for (int i = 1; i < s.size(); i++)
        {
            cur->next = new ListNode(s[i] - '0');
            cur = cur->next;
        }
        return root;
    }
public:
    ListNode* addTwoNumbers(ListNode* l1, ListNode* l2) {
        string s1 = readString(l1);
        string s2 = readString(l2);
        string sum = stringAdd(s1, s2);
        return writeList(sum);
    }
};