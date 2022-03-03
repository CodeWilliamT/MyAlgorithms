using namespace std;
#include <vector>
#include <algorithm>
#include <unordered_set>
//巧思
//找数组中唯一重复数
//O(1)空间O,O(NlogN)时间
//排序后跟前面相同的就是
class Solution {
public:
    int findDuplicate(vector<int>& nums) {
        sort(nums.begin(), nums.end());
        for (int i = 1; i < nums.size();i++) {
            if(nums[i]==nums[i-1])return nums[i];
        }
        return nums.back();
    }
};
//哈希
//找数组中唯一重复数
//O(N)时间空间
class Solution {
public:
    int findDuplicate(vector<int>& nums) {
        unordered_set<int> st;
        for (int& e : nums) {
            if (!st.count(e)) {
                st.insert(e);
            }
            else
                return e;
        }
        return nums.back();
    }
};
//floyd快慢指针 确实没想到
//索引跟值看成有向边，链表，同一个节点被两次指向了，则为重复
class Solution {
public:
    int findDuplicate(vector<int>& nums) {
        int slow = 0, fast = 0;
        do {
            slow = nums[slow];
            fast = nums[nums[fast]];
        } while (slow != fast);
        slow = 0;
        while (slow != fast) {
            slow = nums[slow];
            fast = nums[fast];
        }
        return slow;
    }
};
