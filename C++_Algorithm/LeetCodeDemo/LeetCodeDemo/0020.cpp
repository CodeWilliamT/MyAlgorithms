using namespace std;
#include <iostream>
#include <string>
//栈
class Solution {
public:
    bool isValid(string s) {
        string stack="";
        for (auto c : s)
        {
            if (c == '('|| c == '{'|| c == '[')
                stack += c;
            else if (c == ')')
            {
                if (stack.back() == '(')
                    stack.pop_back();
                else
                    return false;
            }
            else if (c == '}') {
                if (stack.back() == '{')
                    stack.pop_back();
                else
                    return false;
            }
            else if (c == ']') {
                if (stack.back() == '[')
                    stack.pop_back();
                else
                    return false;
            }
        }
        if (!stack.empty())
            return false;
        return true;
    }
};