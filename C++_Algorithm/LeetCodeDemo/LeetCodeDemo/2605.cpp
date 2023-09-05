using namespace std;
#include <vector>
class Solution {
public:
    int minNumber(vector<int>& nums1, vector<int>& nums2) {
        int a = 9, b = 9;
        int x[10]{};
        for (int& e : nums1) {
            a = min(a, e);
            x[e]++;
        }
        for (int& e : nums2) {
            b = min(b, e);
            x[e]++;
        }
        for (int i = 1; i < 10; i++) {
            if (x[i] == 2) {
                return i;
            }
        }
        return min(10 * a + b, 10 * b + a);
    }
};