using namespace std;
#include <vector>
//模拟
//相邻相加求和到只剩1数
class Solution {
public:
    int triangularSum(vector<int>& a) {
        while (a.size()-1) {
            for (int i = 0; i < a.size()-1; i++) {
                a[i] = (a[i]+a[i + 1])%10;
            }
            a.pop_back();
        }
        return a[0];
    }
};