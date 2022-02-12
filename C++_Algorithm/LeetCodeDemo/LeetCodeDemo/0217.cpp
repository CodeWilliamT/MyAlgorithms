using namespace std;
#include <unordered_set>
//哈希 简单
class Solution {
public:
    bool containsDuplicate(vector<int>& nums) {
        unordered_set<int> st;
        for (int& e : nums) {
            if (st.count(e)) {
                return true;
            }
            else 
                st.insert(e);
        }
        return false;
    }
};