using namespace std;
#include <iostream>
#include <unordered_map>
//动态规划+哈希
//d[i]:以a[i]结尾的最长等差子序列长度。
class Solution {
public:
    int longestSubsequence(vector<int>& a, int diff) {
        unordered_map<int,int> d;
        int n = a.size();
        int ans=0;
        for (int i = 0; i < n; i++)
        {
            d[a[i]] = max(d[a[i]], 1+d[a[i] - diff]);
            ans = max(ans, d[a[i]]);
        }
        return ans;
    }
};