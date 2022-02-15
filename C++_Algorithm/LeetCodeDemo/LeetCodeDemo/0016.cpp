using namespace std;
#include <vector>
#include <algorithm>
#include <unordered_set>
#include <unordered_map>

//双指针
//排序，对于任意数a后的区间头取b，尾取c，
//当等于则返回，当sum<target,b的索引++，sum>target,c的索引--。
class Solution {
public:
    int threeSumClosest(vector<int>& nums, int target) {
        sort(nums.begin(), nums.end());
        int rst = nums[0] + nums[1] + nums[2];
        int n= nums.size();
        int sum;
        for (int i = 0; i < n-2; i++) {
            for (int j = i + 1, k = n-1;j<k;) {
                sum = nums[i] + nums[j] + nums[k];
                if (abs(target - sum) < abs(rst - target)) {
                    rst = sum;
                }
                if (sum == target)
                    return target;
                else if (sum < target) {
                    j++;
                }
                else{
                    k--;
                }
            }
        }
        return rst;
    }
};
//两分 哈希(可重复哈希)
//求任意俩数和，并保存为哈希mp,俩数和为key，指向两数索引
//对target-nums[i]在mp里做两分查找，如果mp[x].count(i)<mp[x].size()/2。
//class Solution {
//public:
//    int threeSumClosest(vector<int>& nums, int target) {
//        sort(nums.begin(), nums.end());
//        unordered_map<int, unordered_multiset<int>> mp;
//        int n = nums.size();
//        int sum;
//        //n^2
//        for (int i = 0; i < n; i++) {
//            for (int j = i + 1; j < n; j++) {
//                sum = nums[i] + nums[j];
//                mp[sum].insert(i);
//                mp[sum].insert(j);
//            }
//        }
//        //6*n^2
//        for (int j = 0; j < 6000; j++) {
//            for (int i = 0; i < n; i++) {
//                sum = target + j - nums[i];
//                if (mp.count(sum) && mp[sum].count(i) < mp[sum].size() / 2) {
//                    return target+j;
//                }
//                sum = target - j - nums[i];
//                if (mp.count(sum) && mp[sum].count(i) < mp[sum].size() / 2) {
//                    return target - j;
//                }
//            }
//        }
//        return nums[0] + nums[1] + nums[2];
//    }
//};