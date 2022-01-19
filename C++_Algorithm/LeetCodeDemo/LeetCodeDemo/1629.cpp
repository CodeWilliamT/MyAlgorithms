using namespace std;
#include <iostream>
#include <vector>
//朴素实现
//记下最大索引，最大时差，
//遍历比较， 当大于最大时差，或等于最大时差且字母更大则刷新最大索引，最大时差。
class Solution {
public:
    char slowestKey(vector<int>& releaseTimes, string keysPressed) {
        int mxidx = 0,mxval= releaseTimes[0];
        for (int i = 1; i < releaseTimes.size(); i++) {
            if (releaseTimes[i]- releaseTimes[i-1] > mxval|| releaseTimes[i] - releaseTimes[i - 1] == mxval && keysPressed[i]> keysPressed[mxidx]) {
                mxidx = i;
                mxval = releaseTimes[i] - releaseTimes[i - 1];
            }
        }
        return keysPressed[mxidx];
    }
};