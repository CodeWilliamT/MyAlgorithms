using namespace std;
#include <iostream>
#include <vector>
#include <algorithm>
//随机化优化的快排选择
//因为只关心k，优化快排,只递归一侧区间，到找到k为止,用随机数均化搜索时间。
//class Solution {
//    bool flag;
//    int k;
//    void quick_sort(int L, int R, vector<int>& a)
//    {
//        if (flag)return;
//        if (L >= R)return;
//        int l = L, r = R;
//        int g = rand() % (r - l + 1) + l;
//        swap(a[l], a[g]);//这里的交换是因为第一次不交换的确定性。
//        int x = a[l];
//        while (l < r)
//        {
//            while (a[r] <= x && l < r) r--;
//            while (a[l] >= x && l < r)l++;
//            if (l < r)swap(a[l], a[r]);
//        }
//        a[L] = a[l]; a[l] = x;
//        if (l == k - 1) { flag = true; return; }
//        if (l > k - 1)
//            quick_sort(L, l - 1, a);
//        else
//            quick_sort(r + 1, R, a);
//    }
//public:
//    int findKthLargest(vector<int>& nums, int k) {
//        flag = false;
//        this->k = k;
//        quick_sort(0, nums.size() - 1, nums);
//        return nums[k - 1];
//    }
//};
//堆排序
class Solution {
public:
    //从根节点出发的维护操作
    void HeapifyNode(vector<int>& a, int i, int x) {
        int l = (i << 1) + 1, r = (i << 1) + 2;
        int maxIdx = i;
        if (l <= x && a[l] > a[i])maxIdx = l;//增序则>建大顶堆,降序则<建小顶堆,找第k大则>建大顶堆k次,a[0]就是。
        if (r <= x && a[r] > a[maxIdx])maxIdx = r;
        if (maxIdx != i)
        {
            swap(a[maxIdx], a[i]);
            HeapifyNode(a, maxIdx, x);
        }
    }
    //遍历根节点的构建顶堆操作
    void HeapifyTree(vector<int>& a, int x) {
        int n = (x - 1) >> 1;
        for (int i = n; i > -1; i--)
        {
            HeapifyNode(a, i, x);
        }
    }
    //堆排序
    void HeapSort(vector<int>& a)
    {
        int n = a.size();
        HeapifyTree(a, n - 1);
        swap(a[0], a[n - 1]);
        for (int i = n - 2; i > 0; i--)
        {
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
};
////快排
//class Solution {
//    void quick_sort(int L, int R, vector<int>& a)
//    {
//        if (L >= R)return;
//        int l = L, r = R, x = a[L];
//        while (l < r)
//        {
//            while (a[r] <= x && l < r) r--;
//            while (a[l] >= x && l < r)l++;
//            if (l < r)swap(a[l], a[r]);
//        }
//        a[L] = a[l]; a[l] = x;
//        quick_sort(L, l - 1, a);
//        quick_sort(r + 1, R, a);
//    }
//public:
//    int findKthLargest(vector<int>& nums, int k) {
//        quick_sort(0,nums.size()-1, nums);
//        return nums[k-1];
//    }
//};
//直接用
//class Solution {
//public:
//    int findKthLargest(vector<int>& nums, int k) {
//        sort(nums.begin(), nums.end(), [](int a, int b) {return a > b; });
//        return nums[k - 1];
//    }
//};