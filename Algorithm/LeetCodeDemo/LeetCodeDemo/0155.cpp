using namespace std;
#include <iostream>
#include <vector>
#include <set>
//栈，可重复哈希集
class MinStack {
private:
    multiset<int> st;
    vector<int> stack;
public:
    MinStack() {
        stack.clear();
        st.clear();
    }

    void push(int val) {
        stack.push_back(val);
        st.insert(val);
    }

    void pop() {
        int val = stack.back();
        stack.pop_back();
        auto iter = st.find(val);
        if (iter != st.end())st.erase(iter);
    }

    int top() {
        return stack.back();
    }

    int getMin() {
        return *st.begin();
    }
};

/**
 * Your MinStack object will be instantiated and called as such:
 * MinStack* obj = new MinStack();
 * obj->push(val);
 * obj->pop();
 * int param_3 = obj->top();
 * int param_4 = obj->getMin();
 */