using namespace std;
#include <iostream>
#include <vector>
#include <string>
//麻烦题，细致条件分析，栈，朴素实现
//拿个动态数组存某一位的乘积，倒序遍历俩数组按i+j位存入对应位,没有对应位则动态数组扩容，然后遍历数组做进位，然后再将数组倒序转化为string。
class Solution {
public:
    string multiply(string num1, string num2) {
        int m = num1.size();
        int n = num2.size();
        vector<int> rst;
        int a, b;
        for (int i = 0; i < m; i++) {
            for (int j = 0; j < n; j++)
            {
                a = num1[m - 1-i]-'0';
                b = num2[n - 1 - j]-'0';
                while (i + j >= rst.size())
                    rst.push_back(0);
                rst[i + j] += a*b;
            }
        }
        int out = 0;
        for (auto& e : rst)
        {
            out += e;
            e = out % 10;
            out = out / 10;
        }
        while (out)
        {
            rst.push_back(out % 10);
            out = out / 10;
        }
        string ans;
        bool flag = 0;
        int e;
        while (!rst.empty())
        {
            e = rst.back();
            rst.pop_back();
            if (e == 0 && !flag)continue;
            if (e)flag = 1;
            ans.push_back(e + '0');
        }
        if (ans.empty())ans = "0";
        return ans;
    }
};