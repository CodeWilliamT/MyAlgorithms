using namespace std;
#include <iostream>
#include <vector>
//巧思 朴素实现优化
//优化左右乘积使得空间复杂度位O(n+1)
class Solution {
public:
    vector<int> productExceptSelf(vector<int>& nums) {
        int n = nums.size();
        vector<int> tail(n, 1);
        for (int i = 1; i < n; i++)
        {
            tail[n - 1 - i] = nums[n - 1 - i + 1] * tail[n - 1 - i + 1];
        }
        int head = 1;
        for (int i = 0; i < n; i++)
        {
            tail[i] = head * tail[i];
            head *= nums[i];
        }
        return tail;
    }
};
//朴素实现
//左右乘积空间复杂度位O(2n)
//class Solution {
//public:
//    vector<int> productExceptSelf(vector<int>& nums) {
//        int n = nums.size();
//        vector<int> head(n,1),tail(n,1);
//        for (int i = 1; i < n; i++)
//        {
//            head[i] = nums[i-1] * head[i - 1];
//            tail[n - 1 - i] = nums[n-1-i+1] * tail[n - 1 - i+1];
//        }
//        for (int i = 0; i < n; i++)
//        {
//            tail[i] = head[i] * tail[i];
//        }
//        return tail;
//    }
//};