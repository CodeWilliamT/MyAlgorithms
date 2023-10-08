using namespace std;
#include <vector>
//快排 两分
//O(nlogn),O(logn)
class QuickSt {
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


//堆排序
//O(nlogn),O(1)
//适用边加元素边找某极值元素。
//有序集合
//log2N插入，log2N查找
class HeapSt {
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
//归并排序 两分
//一直对半分到最少，一直归并。
//时间复杂度:O(nlogn)
//空间复杂度 : O(n)
class MergeSt {
	vector<int> tmp;
	void mergeSort(vector<int>& nums, int l, int r) {
		if (l >= r) return;
		int mid = (l + r) >> 1;
		mergeSort(nums, l, mid);
		mergeSort(nums, mid + 1, r);
		int i = l, j = mid + 1;
		int cnt = 0;
		while (i <= mid && j <= r) {
			if (nums[i] <= nums[j]) {
				tmp[cnt++] = nums[i++];
			}
			else {
				tmp[cnt++] = nums[j++];
			}
		}
		while (i <= mid) {
			tmp[cnt++] = nums[i++];
		}
		while (j <= r) {
			tmp[cnt++] = nums[j++];
		}
		for (int i = 0; i < r - l + 1; ++i) {
			nums[i + l] = tmp[i];
		}
	}
public:
	vector<int> sortArray(vector<int>& nums) {
		tmp.resize((int)nums.size(), 0);
		mergeSort(nums, 0, (int)nums.size() - 1);
		return nums;
	}
};

//基数排序
//O(n+k) O(n+k) 限制:正整数
class RadixSt {
private:
	vector<int> temp;
	const int Radix = 8;//位宽
	int c[8];
public:
	//按位计数排序
	void countSort(vector<int>& a, int byte){
		int n = a.size();
		memset(c, 0, sizeof(c));
		int i, digit, maxIndex;
		int weight = pow(Radix, byte);
		//数组c记录数组a在byte位上的各个值的数目
		for (i = 0; i < n; i++){
			digit = a[i] / weight % Radix;
			c[digit]++;
		}
		//数组c改为记录数组a在byte位上的从小到大至各个值的总数
		//即记录数组a在byte位上的从小到大至各个值的最大序号+1
		for (i = 1; i < Radix; i++){
			c[i] = c[i - 1] + c[i];
		}
		//因为利用最大序号所以需倒序
		for (i = n - 1; i > -1; i--){
			//排序组根据记录的序号赋值；序号--；
			digit = a[i] / (int)weight % Radix;
			//最大序号
			maxIndex = c[digit] - 1;
			temp[maxIndex] = a[i];
			c[digit]--;
		}
		a = temp;
	}

	//乱序数组
	void radixSort(vector<int>& a){
		int i;
		temp = vector<int>(a.size());
		//按位做计数排序
		for (i = 0; i < a.size(); i++){
			countSort(a, i);
		}
	}
	//使用
	vector<int> sortArray(vector<int>& nums) {
		radixSort(nums);
		return nums;
	}
};
//冒泡排序
//相邻小的丢前面
//O(n^2),O(1)
class PopSt {
	vector<int> PopSort(vector<int>& nums) {
		int n = nums.size();
		for (int i = 0; i < n; i++) {
			for (int j = i+1; j < n; j++) {
				if (nums[j-1]>nums[j]) {
					swap(nums[j - 1], nums[j]);
				}
			}
		}
		return nums;
	}
public:
	//排序
	vector<int> sortArray(vector<int>& nums) {
		PopSort(nums);
		return nums;
	}
};
//插入排序
//每次比出未排序里最小的丢前面
//O(n^2),O(1)
class SelectSt {
	vector<int> SelectSort(vector<int>& nums) {
		int n = nums.size();
		for (int i = 0; i < n; i++) {
			for (int j = i + 1; j < n; j++) {
				if (nums[i] > nums[j]) {
					swap(nums[i], nums[j]);
				}
			}
		}
		return nums;
	}
public:
	//排序
	vector<int> sortArray(vector<int>& nums) {
		SelectSort(nums);
		return nums;
	}
};

//选择排序
//每次比出未排序里最小的丢前面
//O(n^2),O(1)
class InsertSt {
	vector<int> InsertSort(vector<int>& nums) {
		int n = nums.size();
		for (int i = 1; i < n; i++) {
			for (int j = 0; j < i; j++) {
				if (nums[i] < nums[j]) {
					swap(nums[i], nums[j]);
				}
			}
		}
		return nums;
	}
public:
	//排序
	vector<int> sortArray(vector<int>& nums) {
		InsertSort(nums);
		return nums;
	}
};