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
//找规律 枚举
//若有解则存在俩等差数列，前n跟后n毫不交替,或者某个地方交替着。
//所以需要确定俩等差数列。
class Solution {
public:
    vector<int> recoverArray(vector<int>& nums) {
        int n = nums.size();
        sort(nums.begin(), nums.end());
        for (int i = 1; i < n; ++i) {
            if (nums[i] == nums[0] || (nums[i] - nums[0]) % 2 != 0) {
                continue;
            }

            vector<int> used(n);
            used[0] = used[i] = true;
            int k = (nums[i] - nums[0]) / 2;
            vector<int> ans;
            ans.push_back(nums[0] + k);

            int left = 0, right = i;
            for (int j = 2; j + j <= n; ++j) {
                while (used[left]) {
                    ++left;
                }
                while (right < n && (used[right] || nums[right] - nums[left] != k * 2)) {
                    ++right;
                }
                if (right == n) {
                    break;
                }
                ans.push_back(nums[left] + k);
                used[left] = used[right] = true;
            }

            if (ans.size() == n / 2) {
                return ans;
            }
        }
        return {};
    }
};