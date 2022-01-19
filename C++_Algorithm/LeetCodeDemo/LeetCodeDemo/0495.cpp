using namespace std;
#include <iostream>
#include <vector>
//简单题，朴素实现
class Solution {
public:
    int findPoisonedDuration(vector<int>& timeSeries, int duration) {
        int ans = duration;
        int tmp;
        for (int i = 0; i < timeSeries.size()-1; i++)
        {
            tmp = timeSeries[i + 1] - timeSeries[i];
            if (tmp > duration)ans += duration;
            else ans += tmp;
        }
        return ans;
    }
};