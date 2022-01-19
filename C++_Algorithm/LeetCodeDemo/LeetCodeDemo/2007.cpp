using namespace std;
#include <iostream>
#include <vector>
#include <set>
//哈希，巧思
//无解情形：数组除0外元素的个数为奇数；数组0的个数为奇数；
//将原数组一半数目的0加入答案数组
//将所有元素存入哈希表，从小到大找对应元素的两倍，找到一对,删一对，把找的元素加入答案数组。
class Solution {
public:
    vector<int> findOriginalArray(vector<int>& a) {
        if (a.size() % 2)return {};
        int n = a.size();
        multiset<int >st;
        int count = 0;
        for (int i = 0; i < n; i++)
        {
            if (a[i] == 0)count++;
            else st.insert(a[i]);
        }
        int val;
        vector<int> ans;
        if (st.size() % 2)return {};
        if (count % 2)
            return {};
        else
        {
            while (count)
            {
                count -= 2;
                ans.push_back(0);
            }
        }
        while (!st.empty())
        {
            val = *st.begin();
            auto ite = st.find(val * 2);
            if (ite != st.end())
            {
                ans.push_back(val);
                st.erase(ite);
                st.erase(st.begin());
            }
            else
            {
                return {};
            }
        }
        return ans;
    }
};