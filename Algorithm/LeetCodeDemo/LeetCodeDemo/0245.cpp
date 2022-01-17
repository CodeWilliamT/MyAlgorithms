using namespace std;
#include <iostream>
#include <vector>
#include <string>
#include <map>
//哈希，字典
//map
class Solution {
public:
    int shortestWordDistance(vector<string>& ws, string w1, string w2) {
        map<int, int> hash;
        int n = ws.size();
        for (int i = 0; i < n; i++)
        {
            if (w1 == ws[i])hash[i]=1;
        }
        for (int i = 0; i < n; i++)
        {
            if (w2 == ws[i])hash[i] = 2;
        }
        int ans = n - 1;
        for (auto e=hash.begin();e!=prev(hash.end());++e)
        {
            auto a = *e, b = *next(e);
            if(w1 == w2||a.second==1&&b.second==2||a.second==2&&b.second==1)
                ans = min(ans, b.first - a.first);
        }
        return ans;
    }
};