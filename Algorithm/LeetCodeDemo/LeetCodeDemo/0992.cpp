using namespace std;
#include <iostream>
#include <vector>
 //双指针
 //对同一个数据集两个索引进行维护，对其指向的数据进行判定或操作，每个索引遍历数据集单次
 //观察规律
 //利用两个性质
 //性质一：对于任意一左端点，能够与其对应的右端点们必然相邻
 //性质二：当右端点向右移动时，左端点区间也同样向右移动。因为当我们在原有区间的右侧添加元素时，区间中的不同整数数量不会减少而只会不变或增加
 //算法：其实是三指针，左边界迭代器i，右边界最小索引l，右边界最大索引r。利用计数数组记每个数出现次数。
 //遍历左边界，l指向解域右边界最小索引，r指向右边界最大索引,解为每次的r+l
 //时间复杂度O(n)
class Solution {
public:
    int subarraysWithKDistinct(vector<int>& a, int k) {
        int ans = 0;
        int n = a.size();
        int x = 0;
        int v[20001]{};
        int l = 0, r = 0;
        for (int i = 0; i < n - k + 1; i++)
        {
            while (x < k && l < n)
            {
                if (!v[a[l]])
                    x++;
                v[a[l]]++;
                l++;
            }
            if (l == n && x < k)break;
            if (r < l)r = l;
            while (r < n && x == k && v[a[r]])
            {
                r++;
            }
            if (k == x)
                ans += r - l + 1;
            if (v[a[i]] == 1)
            {
                x--;
            }
            v[a[i]]--;
        }
        return ans;
    }
};

 //回溯
 //会超时
 //注意求连续用for足以
//class Solution {
//public:
//    int subarraysWithKDistinct(vector<int>& nums, int k) {
//        int ans = 0;
//        int n = nums.size();
//        bool v[20001];
//        int count;
//        for (int i = 0; i < n; i++)
//        {
//            count = 0;
//            for (int j = i; j < n && count <= k; j++)
//            {
//                v[nums[j]] = 0;
//            }
//            for (int j = i; j < n&& count<=k; j++)
//            {
//                if (!v[nums[j]])
//                {
//                    v[nums[j]] = 1;
//                    count++;
//                }
//                if (count == k)
//                    ans++;
//            }
//        }
//        return ans;
//    }
//};
//int main()
//{
//    Solution s;
//    vector<int> nums = { 1,2,3 };
//    int k = 2;
//    s.subarraysWithKDistinct(nums, k);
//}