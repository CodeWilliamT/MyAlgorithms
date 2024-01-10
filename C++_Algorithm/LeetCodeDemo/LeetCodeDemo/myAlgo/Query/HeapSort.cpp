using namespace std;
#include <iostream>
#include <vector>
//堆排序
//O(nlogn),O(1)
//适用边加元素边找某极值元素。
//有序集合
//log2N插入，log2N查找
class HeapSt{
public:
	//从根节点出发的维护操作
	//原始数组，正在平衡的树的索引，原始元素的索引
	void HeapifyNode(vector<int>& a, int i, int x) {
		int l = (i << 1) + 1, r = (i << 1) + 2;
		int maxIdx = i;
		if (l <= x && a[l] > a[i])maxIdx = l;//增序则>建大顶堆,降序则<建小顶堆,找第k大则>建大顶堆k次,a[0]就是极值。
		if (r <= x && a[r] > a[maxIdx])maxIdx = r;
		if (maxIdx != i){
			swap(a[maxIdx], a[i]);
			HeapifyNode(a, maxIdx, x);
		}
	}
	//遍历根节点的构建顶堆操作
	void HeapifyTree(vector<int>& a, int x) {
		int n = (x - 1) >> 1;
		for (int i = n; i > -1; i--){
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
		for (int i = n - 2; i > n - k - 1; i--){
			HeapifyNode(a, 0, i);
			if (i > n - k)swap(a[0], a[i]);
		}
		return a[0];
	}
	//使用：排序
	// O(nlog(n))
	vector<int> sortArray(vector<int>& nums) {
		HeapSort(nums);
		return nums;
	}
	//使用：插入一个元素并排序
	// O(log(n))
	vector<int> InsertAndSortArray(vector<int>& nums,int x) {
		HeapSort(nums);
		nums.push_back(x);
		HeapifyTree(nums, nums.size()-1);
		return nums;
	}
};
