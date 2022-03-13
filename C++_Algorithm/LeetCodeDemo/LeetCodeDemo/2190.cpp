using namespace std;
#include <vector>
//哈希
class Solution {
public:
    int mostFrequent(vector<int>& nums, int key) {
        int t[1001]{};
        int n = nums.size();
        int idx=0;
        for (int i = 1; i < n; i++) {
            if (nums[i - 1] == key) {
                t[nums[i]]++;
                if (t[nums[i]] > t[idx])idx = nums[i];
            }
        }
        return idx;
    }
};