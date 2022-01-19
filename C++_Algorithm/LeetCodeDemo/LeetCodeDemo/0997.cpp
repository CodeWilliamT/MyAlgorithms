using namespace std;
#include <vector>
//简单题 朴素实现
//满足：1.不信人；2.都相信
class Solution {
public:
    int findJudge(int n, vector<vector<int>>& trust) {
        int v[1001]{};
        for (auto& e : trust) {
            v[e[0]] = -1;
            if (v[e[1]] >= 0)v[e[1]]++;
        }
        vector<int> rst;
        for (int i = 1; i <= n; i++)
        {
            if (v[i]==n-1)rst.push_back(i);
        }
        if (rst.size() == 1)return rst[0];
        return -1;
    }
};