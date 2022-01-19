using namespace std;
#include <iostream>
#include <vector>
#include <string>
//朴素实现
//简单题
class Solution {
public:
    int finalValueAfterOperations(vector<string>& operations) {
        int ans = 0;
        for (string x : operations)
        {
            if (x[1] == '+')ans++;
            else if (x[1] == '-')ans--;
        }
        return ans;
    }
};