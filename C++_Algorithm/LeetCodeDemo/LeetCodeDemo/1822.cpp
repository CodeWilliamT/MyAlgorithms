using namespace std;
#include <iostream>
#include <vector>
//简单题，分治
//注意溢出，所以对每个求下符号,统计负数个数，奇-1偶1，出0返回0
class Solution {
public:
    int arraySign(vector<int>& nums) {
        int cnt = 0;
        for (auto e : nums)
        {
            if (!e)return 0;
            if (e < 0)cnt++;
        }
        return cnt%2?-1:1;
    }
};