using namespace std;
#include <iostream>
#include <stack>
//设计题 栈 队列
//入栈s1为空则元素记为队头，出栈s2为空则把s1都拿出来，看队头按s2空不空判断
class MyQueue {
    int front;
    stack<int> s1, s2;
public:
    MyQueue() {

        while (!s1.empty())s1.pop();
        while (!s2.empty())s2.pop();
        front = -1;
    }

    void push(int x) {
        if (s1.empty())
            front = x;
        s1.push(x);
    }

    int pop() {
        if (s2.empty())
        {
            while (!s1.empty())
            {
                s2.push(s1.top());
                s1.pop();
            }
        }
        int tmp = s2.top();
        s2.pop();
        return tmp;
    }

    int peek() {
        if (empty())return -1;
        return s2.empty()?front:s2.top();
    }

    bool empty() {
        return s1.empty() && s2.empty();
    }
};