using namespace std;
#include <iostream>
#include <vector>
//朴素实现,双指针
//从左往右一次，从右往左一次。拿大的
class Solution {
public:
    int maxDistance(vector<int>& colors) {
        int n = colors.size();
        int i = 0, j = n-1;
        int ans = 0;
        while (colors[i] == colors[j] && i < n)i++;
        ans = j - i;
        i = 0;
        while (colors[i] == colors[j] && j >= 0)j--;
        ans = max(ans, j - i);
        return ans;
    }
};