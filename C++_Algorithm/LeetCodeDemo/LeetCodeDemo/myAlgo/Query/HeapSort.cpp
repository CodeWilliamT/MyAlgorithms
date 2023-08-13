using namespace std;
#include <iostream>
#include <vector>
//堆集
//有序集合
//log2N插入，log2N查找
class HeapSort{
public:
    //从根节点出发的维护操作
    void HeapifyNode(vector<int>& a, int i, int x) {
        int l = (i << 1) + 1, r = (i << 1) + 2;
        int maxIdx = i;
        if (l <= x && a[l] > a[i])maxIdx = l;//增序则>建大顶堆,降序则<建小顶堆,找第k大则>建大顶堆k次,a[0]就是。
        if (r <= x && a[r] > a[maxIdx])maxIdx = r;
        if (maxIdx != i) {
            swap(a[maxIdx], a[i]);
            HeapifyNode(a, maxIdx, x);
        }
    }
    //遍历根节点的构建顶堆操作
    void HeapifyTree(vector<int>& a, int x) {
        for (int i = x / 2; i >= 0; i--) {
            HeapifyNode(a, i, x);
        }
    }
    //堆排序
    void Sort(vector<int>& a)
    {
        int n = a.size();
        if (!n)return;
        HeapifyTree(a, n - 1);
        swap(a[0], a[n - 1]);
        for (int i = n - 2; i > 0; i--) {
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
