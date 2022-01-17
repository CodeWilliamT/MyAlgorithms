using namespace std;
#include <vector>
//动态规划 动态规划的存储空间化简
//a始终记录最小元素，b 为某个子序列里第二大的数。
//接下来不断更新 a，同时保持 b 尽可能的小。
//如果下一个元素比 b 大，说明找到了三元组。
class Solution {
public:
    bool increasingTriplet(vector<int>& nums) {
        int a = INT32_MAX, b = INT32_MAX;
        for (int& e:nums) {
            if (e <=a) {
                a = e;
            }
            else if(e<=b){
                b = e;
            }
            else {
                return true;
            }
        }
        return false;
    }
};