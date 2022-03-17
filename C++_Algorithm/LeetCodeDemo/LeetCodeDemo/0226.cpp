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
//哈希 链表
//O(1)修改,两分找位置O(logn)插入。
class MaxStack {
    list<int> lst;
    unordered_map<int, unordered_set<list<int>::iterator>> stk;
public:
    MaxStack() {
        lst.clear();
        stk.clear();
    }

    void push(int x) {
        lst.pu
    }

    int pop() {

    }

    int top() {

    }

    int peekMax() {

    }

    int popMax() {

    }
};