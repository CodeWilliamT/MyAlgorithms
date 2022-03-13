using namespace std;
#include <vector>
//简单模拟
//找到值为key的元素距离<=k的下标
//找到nums[j]==key,然后遍历i。
class Solution {
public:
    vector<int> findKDistantIndices(vector<int>& nums, int key, int k) {
        int n = nums.size();
        int len;
        vector<int> rst;
        for (int i = 0,j=0; j < n; j++) {
            if (nums[j] == key) {
                len = min(j+k, n-1);
                for (i = max(j - k, i); i <= len; i++) {
                    rst.push_back(i);
                }
            }
        }
        return rst;
    }
};