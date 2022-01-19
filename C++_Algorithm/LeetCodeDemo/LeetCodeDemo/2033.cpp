using namespace std;
#include <iostream>
#include <vector>
#include <algorithm>
//巧思
//是否可行：所有格子取余相等才行
//操作数：Σ abs((格子-格子的中位数)/x)
class Solution {
public:
    int minOperations(vector<vector<int>>& g, int x) {
        int sum = 0;
        int rest = g[0][0]%x;
        int m = g.size();
        int n = g[0].size();
        vector<int> tmp;
        for (auto e : g)
            for (auto t : e)
            {
                if (rest != t % x)return -1;
                tmp.push_back(t);
            }
        nth_element(tmp.begin(), tmp.begin() + n*m/2, tmp.end());
        int mid = tmp[n * m / 2];
        int ans = 0;
        for (auto e : g)
            for (auto t : e)
            {
                ans += abs((t- mid)/x);
            }
        return ans;
    }
};