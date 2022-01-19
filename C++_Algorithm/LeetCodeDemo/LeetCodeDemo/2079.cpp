using namespace std;
#include <iostream>
#include <vector>
//简单题，朴素实现
class Solution {
public:
    int wateringPlants(vector<int>& p, int c) {
        int n = p.size();
        int rst = 0;
        int r = c;
        for (int i = 0; i < n; i++)
        {
            if (p[i] <= r)r -= p[i], rst++;
            else rst += 2 * i + 1, r = c - p[i];
        }
        return rst;
    }
};