using namespace std;
#include <iostream>
#include <vector>
#include <functional>
//两分 贪心
//大难时间边界l = 1, r = (long long)totalTrips * time[0]；
//同时出发，看总共的班次数是否大于需要的班次。不够了就往右，够了往左。
class Solution {
public:
    long long minimumTime(vector<int>& time, int totalTrips) {
        long long l = 1, r = (long long)totalTrips * time[0],m;
        function<bool(long long)> check = [&](long long num) {
            long long sum = 0;
            for (int i = 0; i < time.size(); i++) {
                sum += num / time[i];
            }
            return sum>=totalTrips;
        };
        while (l < r) {
            m = (l + r) / 2;
            if (!check(m)) {
                l = m+1;
            }
            else {
                r = m;
            }
        }
        return l;
    }
};