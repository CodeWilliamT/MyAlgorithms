using namespace std;
#include <vector>
//快速排序 O(NlogN) O(1)
//取区间随机一位置x=rand()%(R+1-L)+L，v=nums[x]作比较基准。(否则,比如取第一个数，当数组递增时复杂度为n*n);
//令l = L, r = R, 找个左侧出发找到比该数nums[x]大的nums[l]，右侧出发找到比该数nums[x]小的nums[l]，找到的l <= r, 则交换俩数，直到l大于r结束循环。
//对nums[L, r], nums[l, R]做1，2。
class Solution {
    void quickSort(vector<int>& nums, int L, int R) {
        if (L >= R)return;
        int x = rand() % (R + 1 - L) + L;
        int v = nums[x];
        int l = L, r = R;
        while (l <= r) {
            while (l <= r && nums[l] < v) {
                l++;
            }
            while (l <= r && nums[r] > v) {
                r--;
            }
            if (l <= r) {
                swap(nums[l], nums[r]);
                l++;
                r--;
            }
        }
        quickSort(nums, L, r);
        quickSort(nums, l, R);
    }
public:
    vector<int> sortArray(vector<int>& nums) {
        quickSort(nums, 0, nums.size() - 1);
        return nums;
    }
};

//堆排序 O(NlogN) O(1)
//n/2到0维护顶堆构造出大顶堆(增序)，交换顶底，从n-2到1,一直做 维护顶堆，完事再交换顶底。
class Solution {
public:
    //从根节点出发的维护操作
    void HeapifyNode(vector<int>& a, int i, int x) {
        int l = (i << 1) + 1, r = (i << 1) + 2;
        int maxIdx = i;
        if (l <= x && a[l] > a[i])maxIdx = l;//增序则>建大顶堆,降序则<建小顶堆,找第k大则>建大顶堆k次,a[0]就是。
        if (r <= x && a[r] > a[maxIdx])maxIdx = r;
        if (maxIdx != i){
            swap(a[maxIdx], a[i]);
            HeapifyNode(a, maxIdx, x);
        }
    }
    //遍历根节点的构建顶堆操作
    void HeapifyTree(vector<int>& a, int x) {
        for (int i = x/2; i >=0; i--){
            HeapifyNode(a, i, x);
        }
    }
    //堆排序
    void HeapSort(vector<int>& a)
    {
        int n = a.size();
        HeapifyTree(a, n - 1);
        swap(a[0], a[n - 1]);
        for (int i = n - 2; i > 0; i--){
            HeapifyNode(a, 0, i);
            swap(a[0], a[i]);
        }
    }

    //找第k大的元素
    int findKthLargest(vector<int>& a, int k) {
        int n = a.size();
        HeapifyTree(a, n - 1);
        if (k > 1)swap(a[0], a[n - 1]);
        for (int i = n - 2; i > n - k - 1; i--)
        {
            HeapifyNode(a, 0, i);
            if (i > n - k)swap(a[0], a[i]);
        }
        return a[0];
    }
    //排序
    vector<int> sortArray(vector<int>& nums) {
        HeapSort(nums);
        return nums;
    }
};
