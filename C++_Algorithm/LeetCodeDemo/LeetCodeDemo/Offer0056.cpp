using namespace std;
#include <iostream>
#include <vector>
#include <unordered_map>
//¹þÏ£·¨
class Solution {
public:
    int singleNumber(vector<int>& nums) {
        unordered_map<int,int> mp;
        for (int i = 0; i < nums.size(); i++)
        {
            if (!mp.count(nums[i]))
                mp[nums[i]] = 0;
            else
                mp[nums[i]]++;
            if (mp[nums[i]]== 2)
                mp.erase(mp.find(nums[i]));
        }
        return (*mp.begin()).first;
    }
};