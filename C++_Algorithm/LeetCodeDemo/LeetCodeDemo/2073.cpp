using namespace std;
#include <iostream>
#include <vector>
//简单题 朴素实现
class Solution {
public:
    int timeRequiredToBuy(vector<int>& tickets, int k) {
        int n = tickets.size();
        int i = 0;
        int ans=0;
        while (tickets[k] != 0)
        {
            if (tickets[i])
            {
                tickets[i]--;
                ans++;
            }
            i = (i + 1 + n) % n;
        }
        return ans;
    }
};