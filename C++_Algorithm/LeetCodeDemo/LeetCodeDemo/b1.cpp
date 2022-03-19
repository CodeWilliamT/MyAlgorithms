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
#include <bitset>
//哈希
//判定是否偶数
class Solution {
public:
    bool divideArray(vector<int>& nums) {
        unordered_map<int, int> mp;
        for (int& e : nums) {
            mp[e]++;
        }
        for (auto& e : mp) {
            if (e.second % 2)return false;
        }
        return true;
    }
};