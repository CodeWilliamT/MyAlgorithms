using namespace std;
#include <iostream>
#include <vector>
#include <string>
//巧思，找规律
//贪心
//在遍历过程中维护未匹配的左括号数量可能的最小值和最大值，根据遍历到的字符更新最小值和最大值：
//如果遇到左括号，则将最小值和最大值分别加 1;
//如果遇到右括号，则将最小值和最大值分别减 1;
//如果遇到星号，则将最小值减 11，将最大值加 1;
//任何情况下，未匹配的左括号数量必须非负，因此当最大值变成负数时，说明没有左括号可以和右括号匹配，返回false。
//当最小值为 0 时，不应将最小值继续减少，以确保最小值非负。
//遍历结束时，所有的左括号都应和右括号匹配，因此只有当最小值为0时，字符串s才是有效的括号字符串。
class Solution {
public:
    bool checkValidString(string s) {
        int lmin = 0, lmax = 0;
        for (int i = 0; i < s.size(); i++)
        {
            if (s[i] == '(')lmin++, lmax++;
            else if (s[i] == '*')
            {
                lmin--;
                lmax++;
            }
            else if (s[i] == ')')lmin--, lmax--;
            if (lmin < 0)lmin = 0;
            if (lmax < 0)
            {
                return false;
            }
        }
        return lmin == 0;
    }
};