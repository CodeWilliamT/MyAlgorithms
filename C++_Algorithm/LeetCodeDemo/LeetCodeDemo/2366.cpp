using namespace std;
#include <vector>
//找规律 贪心
//越拆越小，所以靠后的数字是前面数字能拆的最大值。
//新拆的数字，最小的是前一个数的最大值。
//倒序遍历，前后比较，
//前比后大则 看 前%后 是否有余，不整除，拆分计数+=除出来的份数+余数的一份-去掉本身的一份，最小那份的值为（前面的数/总份数）取整
//否则整除，拆分计数+=除出来的份数-去掉本身的一份
//前不比后大，则更新lower为前数
class Solution {
    typedef long long ll;
public:
    long long minimumReplacement(vector<int>& nums) {
        ll rst=0;
        int n = nums.size(),lower=nums[n-1];
        for (int i = n - 2; i > -1; i--) {
            if (nums[i] > lower) {
                if (nums[i] % lower) {
                    rst += nums[i] / lower;//不整除，加除出来的份数+余数的一份-去掉本身的一份
                    lower=nums[i]/(nums[i] / lower+1);//不整除，最小那份的值为（前面的数/总份数）取整
                }
                else {
                    rst += nums[i] / lower-1;//整除，加除出来的份数-去掉本身的一份
                }
            }
            else {
                lower = nums[i];//前不比后大，则更新lower为前数
            }
        }
        return rst;
    }
};