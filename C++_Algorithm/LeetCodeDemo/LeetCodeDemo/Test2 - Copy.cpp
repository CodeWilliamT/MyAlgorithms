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
typedef long long ll;
typedef pair<ll, ll> pll;
typedef pair<int, int> pii;
//巧思 找规律
//是否有连续的两个数的和大于等于m
class Solution {
public:
    bool canSplitArray(vector<int>& nums, int m) {
        int n = nums.size();
        if (n <=2)return true;
        int Sum = 0;
        for (int i = 0; i < n-1;i++) {
            if (nums[i] + nums[i + 1] >= m)
                return true;
        }
        return false;
    }
};