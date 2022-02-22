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
//树状数组 巧思
//找三个值，在两个数组中位置都是递增的。
//转换出nums2值映射索引的数组mp,比nums2[i]索引大的数目为n-mp[nums2[i]]。
class Solution {
public:
    long long goodTriplets(vector<int>& nums1, vector<int>& nums2) {
        int n = nums1.size();
        vector<int> mp1(n),mp2(n);
        for (int i = 0; i < n; i++) {
            mp1[nums1[i]] = i;
        }
        for (int i = 0; i < n; i++) {
            mp2[i]= mp1[nums2[i]];
        }
        long long less;
        long long rst = 0;
        for (int i = 1; i < n-1; i++) {
            less = 0;
            for (int j = 0; j < i; j++) {
                if (mp2[j] < mp2[i]) {
                    less++;
                }
            }
            rst += less * (n - 1-mp2[i] - (i-less));
        }
        return rst;
    }
};