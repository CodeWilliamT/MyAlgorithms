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
//哈希
class Solution {
public:
    int mostFrequent(vector<int>& nums, int key) {
        int t[1001]{};
        int n = nums.size();
        int idx=0;
        for (int i = 1; i < n; i++) {
            if (nums[i - 1] == key) {
                t[nums[i]]++;
                if (t[nums[i]] > t[idx])idx = nums[i];
            }
        }
        return idx;
    }
};