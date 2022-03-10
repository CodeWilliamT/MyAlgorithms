using namespace std;
#include <vector>
//找规律 差分
//暴力找解，找k使变化后索引大于等于值的尽可能多，求最小k。
//找kO(n),遍历积分O(n).
//前面不能动，后面的应该行
//每次转换，前面k个索引增大+n-k，后面k个索引减小-k.
//看题解后用差分
//对于每个数i，必然存在使答案产生增多的位置，以及可能存在减少的的位置，记在差分数组对应位置上
//k>i时,差为(i+n-k)-nums[i]>=0加分,
//设up=i+n-nums[i],
//i+1<=k<=up时得分,即k=i+1时分数+1,
//up+1<=k<=n-1时失去增加的分数,即up<=n-2时k=up+1时失去分数,up<=i则不加分，即k=i+1时失去分数。
//k<=i时,差为(i-k)-nums[i]>=0加分,
//设down=i-nums[i],
//0<=k<=down时得分,即k=0时分数+1,
//down+1<=k<=i时失去增加的分数,即down<=i-1&down<n-1时k=down+1时失去分数,down<=-1则不加分,即k=0时失去分数。
//则各个k的得分=初始态+差分前缀和,找答案完事
class Solution {
public:
    int bestRotation(vector<int>& nums) {
        int n = nums.size();
        int up, down;
        vector<int> diff(n, 0);
        for (int i = 0; i < n; i++) {
            up = i + n - nums[i];
            down = i - nums[i];
            if (i < n - 1)
            {
                diff[i + 1]++;
                if (up <= i) {
                    diff[i + 1]--;
                }
                else if (up < n - 1) {
                    diff[up + 1]--;
                }
            }
            diff[0]++;
            if (down < 0) {
                diff[0]--;
            }
            else if (down <= i && down < n - 1) {
                diff[down + 1]--;
            }
        }
        int k = 0, score = 0, mx = 0;
        for (int i = 0; i < n; i++) {
            score += diff[i];
            if (score > mx) {
                mx = score;
                k = i;
            }
        }
        return k;
    }
};