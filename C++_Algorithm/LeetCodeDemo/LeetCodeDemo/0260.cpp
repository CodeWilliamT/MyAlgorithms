using namespace std;
#include <iostream>
#include <vector>
#include <unordered_set>
//异或判重
//进阶，线性时间复杂度，常数空间复杂度
class Solution {
public:
    vector<int> singleNumber(vector<int>& nums) {
        int xorsum = 0;
        for (int num : nums) {
            xorsum ^= num;
        }
        // 防止溢出
        int lsb = (xorsum == INT_MIN ? xorsum : xorsum & (-xorsum));
        int type1 = 0, type2 = 0;
        for (int num : nums) {
            if (num & lsb) {
                type1 ^= num;
            }
            else {
                type2 ^= num;
            }
        }
        return { type1, type2 };
    }
};

//哈希
//简单哈希
//class Solution {
//public:
//    vector<int> singleNumber(vector<int>& a) {
//        vector<int> ans;
//        unordered_set<int> st;
//        
//        for (int i = 0; i < a.size(); i++)
//        {
//            auto iter = st.find(a[i]);
//            if (iter == st.end())st.insert(a[i]);
//            else st.erase(iter);
//        }
//        for (auto e : st)
//        {
//            ans.push_back(e);
//        }
//        return ans;
//    }
//};
