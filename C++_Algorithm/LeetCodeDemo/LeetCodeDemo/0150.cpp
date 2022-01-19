using namespace std;
#include <iostream>
#include <vector>
#include <string>
#include <unordered_set>
//动态数组模拟栈
class Solution {
private:
    int calculate(int a, int b, const char s)
    {
        switch (s)
        {
        case '+':
            return a + b;
            break;
        case '-':
            return a - b;
            break;
        case '*':
            return a * b;
            break;
        case '/':
            return a / b;
            break;
        default:
            break;
        }
        return 0;
    }
public:
    int evalRPN(vector<string>& t) {
        if (t.size() == 1)return stoi(t[0]);
        vector<int> x;
        int a, b;
        unordered_set<char> s = { '+','-','*','/' };
        for (int i = 0; i < t.size(); i++)
        {
            if ('0' <= t[i][0] && t[i][0] <= '9' || t[i].size() > 1)
            {
                x.push_back(stoi(t[i]));
                continue;
            }
            if (s.count(t[i][0]))
            {
                b = x.back();
                x.pop_back();
                a = x.back();
                x.pop_back();
                x.push_back(calculate(a, b, t[i][0]));
            }
        }
        return x.back();
    }
};
//栈
//class Solution {
//private:
//    int calculate(int a, int b, const char s)
//    {
//        switch (s)
//        {
//        case '+':
//            return a + b;
//            break;
//        case '-':
//            return a - b;
//            break;
//        case '*':
//            return a * b;
//            break;
//        case '/':
//            return a / b;
//            break;
//        default:
//            break;
//        }
//        return 0;
//    }
//public:
//    int evalRPN(vector<string>& t) {
//        if (t.size() == 1)return stoi(t[0]);
//        stack<int> x;
//        int a, b;
//        unordered_set<char> s = { '+','-','*','/' };
//        for (int i = 0; i < t.size(); i++)
//        {
//            if ('0' <= t[i][0] && t[i][0] <= '9' || t[i].size() > 1)
//            {
//                x.push(stoi(t[i]));
//                continue;
//            }
//            if (s.count(t[i][0]))
//            {
//                b = x.top();
//                x.pop();
//                a = x.top();
//                x.pop();
//                x.push(calculate(a, b, t[i][0]));
//            }
//        }
//        return x.top();
//    }
//};
//int main()
//{
//    Solution s;
//    vector<string> t = { "10","6","9","3","+","-11","*","/","*","17","+","5","+"};
//    s.evalRPN(t);
//    return 0;
//}