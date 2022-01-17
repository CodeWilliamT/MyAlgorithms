using namespace std;
#include <iostream>
#include <vector>
#include <string>
//简单题
class Solution {
public:
    int numOfPairs(vector<string>& a, string t) {
        int n = a.size();
        int ans = 0;
        for (int i = 0; i < n; i++)
        {
            for (int j = i+1; j < n; j++)
            {
                if (a[i] + a[j] == t)
                {
                    ans++;
                }
                if (a[j]+ a[i] == t)
                {
                    ans++;
                }
            }
        }
        return ans;
    }
};