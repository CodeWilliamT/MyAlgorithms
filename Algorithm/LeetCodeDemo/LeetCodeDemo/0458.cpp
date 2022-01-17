using namespace std;
#include <iostream>
//找规律
//可测试次数=minutesToTest/minutesToDie+1
class Solution {
public:
    int poorPigs(int buckets, int minutesToDie, int minutesToTest) {
        int time = minutesToTest / minutesToDie;
        int rst = ceil(log(buckets)/ log(time));
        return rst;
    }
};