using namespace std;
#include <unordered_set>
//哈希
class Solution {
public:
    long long minimumPossibleSum(int n, int t) {
        long long rst = 0;
        int x=1;
        unordered_set<int> st;
        while (n--) {
            while (st.count(t - x)) {
                x++;
            }
            rst += x;
            st.insert(x++);
        }
        return rst;
    }
};