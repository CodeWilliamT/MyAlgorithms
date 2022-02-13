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
//求任意俩数和，并保存为哈希mp,俩数和为key，指向两数索引
//对target-nums[i]在mp里做两分查找，如果mp[x].count(i)<mp[x].size()/2。
class Solution {
public:
    int threeSumClosest(vector<int>& nums, int target) {
        unordered_map<int, multiset<int>> mp;
        int n = nums.size();
        int sum;
        for (int i = 0; i < n; i++) {
            for (int j = i+1; j < n; j++) {
                sum = nums[i] + nums[j];
                mp[sum].insert(i);
                mp[sum].insert(j);
            }
        }
        for (int i = 0; i < n; i++) {

        }
    }
};