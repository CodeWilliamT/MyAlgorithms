using namespace std;
#include <vector>
//快速排序
//取区间内随机位置一数，各数与其比较，小的搬往左侧，大于等于某数的搬往右侧，直到两侧壁垒指针相同。对两侧区间再分别做这个操作。
class Solution {
    void quickSort(vector<int>& nums, int L, int R) {
        if (L >= R)return;
        int x = rand() % (R + 1 - L) + L;
        int v = nums[x];
        int l = L, r = R;
        while (l < r) {
            while (l < r && nums[l] < v) {
                l++;
            }
            while (l < r && nums[r] >= v) {
                r--;
            }
            if (l < r) {
                swap(nums[l], nums[r]);
                l++;
                r--;
            }
        }
        quickSort(nums, L, l - 1);
        quickSort(nums, r + 1, R);
    }
public:
    vector<int> sortArray(vector<int>& nums) {
        quickSort(nums, 0, nums.size() - 1);
        return nums;
    }
};