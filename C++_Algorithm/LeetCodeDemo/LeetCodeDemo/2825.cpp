using namespace std;
#include <iostream>
#include <string>
//朴素实现
//一个个匹配找呗
class Solution {
public:
    bool canMakeSubsequence(string str1, string str2) {
        int n = str1.size();
        int m = str2.size();
        for (int i = 0,j=0; i < n&&j<m; i++) {
                if (str1[i] == str2[j]
                    || str1[i] == str2[j] - 1
                    || (str1[i] == 'z' && str2[j] == 'a'))
                {
                    if (j == m - 1)
                        return true;
                    j++;
                }
        }
        return false;
    }
};