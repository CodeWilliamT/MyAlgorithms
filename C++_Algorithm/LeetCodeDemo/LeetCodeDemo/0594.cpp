using namespace std;
#include <iostream>
#include <vector>
#include <map>
//哈希
//有序按频率排列，相邻频率相加即可
class Solution {
public:
    int findLHS(vector<int>& nums) {
        int ans = 0;
        int mx = nums[0], mn = nums[0];
        int n = nums.size();
        map<int,int>st;
        for (int i = 0; i < n;i++)
        {
            st[nums[i]]++;
        }
        int num,sum;
        for (auto it = st.begin(); it != st.end(); ++it)
        {
            num = (*it).first;
            sum = st[num];
            if (st.count(num + 1)) sum += st[num + 1]; 
            else continue;
            ans = max(sum, ans);
        }
        return ans;
    }
};