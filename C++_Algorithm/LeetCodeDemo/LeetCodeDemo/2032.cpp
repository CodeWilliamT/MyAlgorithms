using namespace std;
#include <iostream>
#include <vector>
//简单题
//计数器数组，计数大于1的加入答案数组
class Solution {
public:
    vector<int> twoOutOfThree(vector<int>& a1, vector<int>& a2, vector<int>& a3) {
        int n1 = a1.size();
        int n2 = a2.size();
        int n3 = a3.size();
        int tmp[101]{};
        int tmp2[101]{};
        vector<int> ans;
        for (auto e : a1)
        {
            if (!tmp2[e])
                tmp[e]++;
            tmp2[e]++;
        }
        memset(tmp2, 0, sizeof(tmp2));
        for (auto e : a2)
        {
            if (!tmp2[e])
                tmp[e]++;
            tmp2[e]++;
        }
        memset(tmp2, 0, sizeof(tmp2));
        for (auto e : a3)
        {
            if (!tmp2[e])
                tmp[e]++;
            tmp2[e]++;
        }
        for (int i=1;i<101; i++)
            if(tmp[i]>1)
                ans.push_back(i);
        return ans;
    }
};