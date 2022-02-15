using namespace std;
#include <vector>
//双指针
//求(j-i)*min(h[i],h[j])的最大值
//i遍历不同j，i从左，j从右，
//if(h[j]>=h[i])则(j-i)*h[i]为当前i的最大,为(j-i)*h[i],则i++。
//否则，(j-i)*h[j]为当前j的最大,为(j-i)*h[j]，j--;
class Solution {
public:
    int maxArea(vector<int>& h) {
        int n = h.size();
        int hmx = 0, rst = 0;
        for (int i = 0,j=n-1;i<j;) {
            if (h[j] >= h[i]) {
                rst = max(rst, (j - i) * h[i]);
                i++;
            }
            else {
                rst = max(rst, (j - i) * h[j]);
                j--;
            }
        }
        return rst;
    }
};