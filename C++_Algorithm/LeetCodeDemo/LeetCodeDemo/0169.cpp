using namespace std;
#include <iostream>
#include <vector>
#include <unordered_map>
//找规律 投票算法
//因为多过一半，所以对每个计数，是它+1，不是它-1，少于0了，换新人投票。
class Solution {
public:
    int majorityElement(vector<int>& nums) {
        int cnt=0, rst=-1;
        for (int& num : nums)
        {
            if (num == rst){
                cnt++;
            }
            else {
                cnt--;
                if (cnt < 0) {
                    rst = num;
                    cnt = 1;
                }
            }
        }
        return rst;
    }
};
//哈希
//频率从计数，找超过一半的
//class Solution {
//public:
//    int majorityElement(vector<int>& nums) {
//        unordered_map<int,int> st;
//        for (int& num : nums)
//        {
//            st[num]++;
//        }
//        int half = nums.size() / 2;
//        for (auto& e : st)
//        {
//            if (e.second > half)
//            {
//                return e.first;
//            }
//        }
//        return nums[0];
//    }
//};