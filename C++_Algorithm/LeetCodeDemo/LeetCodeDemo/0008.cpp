using namespace std;
#include <iostream>
#include <unordered_set>
//麻烦题 细致条件处理
//读入字符串并丢弃无用的前导空格
//检查下一个字符（假设还未到字符末尾）为正还是负号，读取该字符（如果有）。 确定最终结果是负数还是正数。 如果两者都不存在，则假定结果为正。
//读入下一个字符，直到到达下一个非数字字符或到达输入的结尾。字符串的其余部分将被忽略。
//将前面步骤读入的这些数字转换为整数（即，"123" -> 123， "0032" -> 32）。如果没有读入数字，则整数为 0 。必要时更改符号（从步骤 2 开始）。
//如果整数数超过 32 位有符号整数范围[−231, 231 − 1] ，需要截断这个整数，使其保持在这个范围内。具体来说，小于 −231 的整数应该被固定为 −231 ，大于 231 − 1 的整数应该被固定为 231 − 1 。
//返回整数作为最终结果。
class Solution {
public:
    int myAtoi(string s) {
        bool active = false;
        bool sign = 1;
        long long rst=0;
        unordered_set<char> st = { '-','+', '0', '1', '2', '3', '4', '5', '6','7','8','9' };
        for (char c:s)
        {
            if (active)
            {
                if (c < '0' || c>'9')
                {
                    return sign ? rst : -rst;
                }
                rst = rst*10+(c - '0');
                if(rst>INT32_MAX)
                    return sign ? INT32_MAX : INT32_MIN;
            }
            else
            {
                if (c != ' ' && !st.count(c))
                {
                    return 0;
                }
                if (st.count(c))
                {
                    active = 1;
                    if (c == '-')
                    {
                        sign = 0;
                    }
                    else if(c=='+')
                    {
                        sign = 1;
                    }
                    else
                    {
                        rst += c - '0';
                    }
                }
            }
        }
        return sign ? rst : -rst;
    }
};