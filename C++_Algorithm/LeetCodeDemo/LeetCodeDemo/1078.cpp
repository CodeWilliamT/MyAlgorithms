using namespace std;
#include <iostream>
#include <vector>
#include <string>
//字符串匹配 find 朴素实现
//idx=string.find(substring,right);(if failed, idx=-1)
class Solution {
public:
    vector<string> findOcurrences(string text, string first, string second) {
        int n = text.size();
        int n1 = first.size();
        int n2 = second.size();
        int idx;
        string ans;
        vector<string> rst;
        
        for (idx = text.find(first + " " + second); idx != -1; idx = text.find(first + " " + second, idx + 1)) {
            if ((idx != 0 && text[idx - 1] != ' '))
                continue;
            ans = "";
            for (int i = idx + n1 + n2 + 2;i<n; i++) {
                if (text[i] == ' ')break;
                ans.push_back(text[i]);
            }
            if(ans!="")
                rst.push_back(ans);
        }
        return rst;
    }
};