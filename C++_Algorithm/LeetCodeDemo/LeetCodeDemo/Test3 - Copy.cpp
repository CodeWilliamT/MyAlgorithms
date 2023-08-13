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
//堆排序
//有序哈希集合，遍历时，边加左元素进集合，边搜右元素，记录最小的。
class Solution {
public:
    int minAbsoluteDifference(vector<int>& nums, int x) {
        int n = nums.size();
        set<int> st;
        int rst=INT32_MAX;
        int l, r,mx=0;
        set<int>::iterator idx;
        for (int i = 0; i + x < n; i++) {
            int j = i + x;
            st.insert(nums[i]);
            mx = max(mx, nums[i]);
            idx = st.lower_bound(nums[j]);
            if (idx == st.end()) {
                rst = min(rst, nums[j] - mx);
                continue;
            }
            r = *idx;
            if (idx == st.begin()) {
                rst = min(rst, abs(r - nums[j]));
                continue;
            }
            if (r == nums[j])return 0;
            l = *prev(idx);
            if (abs(nums[j] - l) <= abs(r - nums[j])) {
                rst = min(rst, abs(nums[j] - l));
            }
            else {
                rst = min(rst, abs(r - nums[j]));
            }
        }
        return rst;
    }
};