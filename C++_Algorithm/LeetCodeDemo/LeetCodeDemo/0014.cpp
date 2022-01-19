using namespace std;
#include <iostream>
#include <vector>
//简单题 朴素实现
//按位遍历字符串
class Solution {
public:
    string longestCommonPrefix(vector<string>& strs) {
        string rst="";
        char tmp;
        for (int i=0;i<strs[0].size();i++)
        {
            tmp = strs[0][i];
            for (int j = 1; j < strs.size(); j++)
            {
                if (strs[j][i] != tmp)
                {
                    return rst;
                }
            }
            rst += tmp;
        }
        return rst;
    }
};