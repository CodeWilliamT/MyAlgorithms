using namespace std;
#include <vector>
#include <algorithm>
#include <functional>
//两分
//求数组中最少的k个不相邻数中最大的那个。
class Solution {
public:
    int minCapability(vector<int>& nums, int k) {
        vector<int> arr = nums;
        sort(arr.begin(), arr.end());
        int n = nums.size();

        auto check = [&](int x) {
            int cnt = 0,last=-2;
            for (int i = 0; i < n; i++) {
                if (nums[i] <= arr[x] && i - 1 > last) {
                    cnt++,last=i;
                }
            }
            return cnt>=k;
        };
        int l = 0, r = n - 1;
        int m;
        while (l < r) {
            m = (l + r) / 2;
            if (check(m))
                r = m;
            else
                l = m + 1;
        }
        return arr[r];
    }
};