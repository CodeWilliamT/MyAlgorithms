using namespace std;
#include <iostream>
#include <vector>
#include <algorithm>
//巧思，找规律
class Solution {
public:
    void wiggleSort(vector<int>& nums) {
        vector<int> a = nums;
        sort(a.begin(), a.end());
        int n = nums.size();
        int j = n - 1, k = (n + 1) / 2 - 1;
        for (int i = 0; i < n; i++)
        {
            if (i % 2)
            {
                nums[i] = a[j--];
            }
            else
            {
                nums[i] = a[k--];
            }
        }
    }
};