using namespace std;
#include <iostream>
#include <unordered_map>
#include <vector>

class Solution {
public:
    vector<int> twoSum(vector<int>& nums, int target) {
        unordered_map<int, int> mp;
        vector<int> ans;
        for (int i = 0; i < nums.size(); i++)
        {
            if (mp.count(nums[i])) { ans.push_back(mp[nums[i]]); ans.push_back(i); return ans; }
            mp[target - nums[i]] = i;
        }
        return ans;
    }
};

//int main()
//{
//    Solution s;
//    int n;
//    while (cin >> n)
//        cout << s.hammingWeight(n) << endl;
//    return 0;
//}