using namespace std;
#include <iostream>
#include <vector>
#include <string>
#include <algorithm>
#include <unordered_set>
#include <unordered_map>
#include <set>
#include <map>
#include <queue>
#include <stack>
#include <functional>
//模拟
class Solution {
public:
    vector<int> sortJumbled(vector<int>& mapping, vector<int>& nums) {
        auto change = [&](int a) {
            int b = 0;
            int prime = 1;
            if (!a)
                b += mapping[a % 10] * prime;
            while (a) {
                b += mapping[a % 10] * prime;
                a /= 10;
                prime *= 10;
            }
            return b;
        };

        sort(nums.begin(), nums.end(), [&](int& a, int& b) {return change(a) < change(b); });
        return nums;
    }
};