using namespace std;
#include <iostream>
#include <vector>
#include <string>
#include <numeric>
#include <algorithm>
#include <unordered_set>
#include <unordered_map>
#include <set>
#include <map>
#include <queue>
#include <stack>
#include <functional>
#include <bitset>
#include "myAlgo\LCParse\TreeNode.cpp"
#define MAXN (int)1e5+1
#define MAXM (int)1e5+1
typedef pair<int, bool> pib;
typedef long long ll;
typedef pair<ll, ll> pll;
typedef pair<int, int> pii;
//滑动窗口
class Solution {
public:
    //适用：已知正整数数组 nums 和一个整数 k，求子数组内所有元素的乘积严格小于 k 的连续子数组的数目
    int numSubarrayProductLessThanK(vector<int>& nums, int k) {
        int n = nums.size();
        int v = 1;
        int rst = 0;
        int l = 0, r = 0;//夹逼区间，含l不含r；
        while (l < n) {
            if (v >= k&&l<r) {
                v /= nums[l];
                l++;
            }
            else{
                rst += r - l;//以r结尾的子区间有r-l个
                if (r < n) {
                    v *= nums[r];
                    r++;
                }
                else {
                    break;
                }
            }
        }
        return rst;
    }
};