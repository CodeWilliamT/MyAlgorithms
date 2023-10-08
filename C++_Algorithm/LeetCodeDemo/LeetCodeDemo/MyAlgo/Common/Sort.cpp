using namespace std;
#include <vector>
//���� ����
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


//������
//O(nlogn),O(1)
//���ñ߼�Ԫ�ر���ĳ��ֵԪ�ء�
//���򼯺�
//log2N���룬log2N����
class HeapSt {
public:
	//�Ӹ��ڵ������ά������
	//ԭʼ���飬����ƽ�������������ԭʼԪ�ص�����
	void HeapifyNode(vector<int>& a, int i, int x) {
		int l = (i << 1) + 1, r = (i << 1) + 2;
		int maxIdx = i;
		if (l <= x && a[l] > a[i])maxIdx = l;//������>���󶥶�,������<��С����,�ҵ�k����>���󶥶�k��,a[0]���Ǽ�ֵ��
		if (r <= x && a[r] > a[maxIdx])maxIdx = r;
		if (maxIdx != i){
			swap(a[maxIdx], a[i]);
			HeapifyNode(a, maxIdx, x);
		}
	}
	//�������ڵ�Ĺ������Ѳ���
	void HeapifyTree(vector<int>& a, int x) {
		int n = (x - 1) >> 1;
		for (int i = n; i > -1; i--){
			HeapifyNode(a, i, x);
		}
	}
	//������
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

	//�ҵ�k���Ԫ��
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
	//ʹ�ã�����
	// O(nlog(n))
	vector<int> sortArray(vector<int>& nums) {
		HeapSort(nums);
		return nums;
	}
	//ʹ�ã�����һ��Ԫ�ز�����
	// O(log(n))
	vector<int> InsertAndSortArray(vector<int>& nums,int x) {
		HeapSort(nums);
		nums.push_back(x);
		HeapifyTree(nums, nums.size()-1);
		return nums;
	}
};
//�鲢���� ����
//һֱ�԰�ֵ����٣�һֱ�鲢��
//ʱ�临�Ӷ�:O(nlogn)
//�ռ临�Ӷ� : O(n)
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

//��������
//O(n+k) O(n+k) ����:������
class RadixSt {
private:
	vector<int> temp;
	const int Radix = 8;//λ��
	int c[8];
public:
	//��λ��������
	void countSort(vector<int>& a, int byte){
		int n = a.size();
		memset(c, 0, sizeof(c));
		int i, digit, maxIndex;
		int weight = pow(Radix, byte);
		//����c��¼����a��byteλ�ϵĸ���ֵ����Ŀ
		for (i = 0; i < n; i++){
			digit = a[i] / weight % Radix;
			c[digit]++;
		}
		//����c��Ϊ��¼����a��byteλ�ϵĴ�С����������ֵ������
		//����¼����a��byteλ�ϵĴ�С����������ֵ��������+1
		for (i = 1; i < Radix; i++){
			c[i] = c[i - 1] + c[i];
		}
		//��Ϊ���������������赹��
		for (i = n - 1; i > -1; i--){
			//��������ݼ�¼����Ÿ�ֵ�����--��
			digit = a[i] / (int)weight % Radix;
			//������
			maxIndex = c[digit] - 1;
			temp[maxIndex] = a[i];
			c[digit]--;
		}
		a = temp;
	}

	//��������
	void radixSort(vector<int>& a){
		int i;
		temp = vector<int>(a.size());
		//��λ����������
		for (i = 0; i < a.size(); i++){
			countSort(a, i);
		}
	}
	//ʹ��
	vector<int> sortArray(vector<int>& nums) {
		radixSort(nums);
		return nums;
	}
};
//ð������
//����С�Ķ�ǰ��
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
	//����
	vector<int> sortArray(vector<int>& nums) {
		PopSort(nums);
		return nums;
	}
};
//��������
//ÿ�αȳ�δ��������С�Ķ�ǰ��
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
	//����
	vector<int> sortArray(vector<int>& nums) {
		SelectSort(nums);
		return nums;
	}
};

//ѡ������
//ÿ�αȳ�δ��������С�Ķ�ǰ��
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
	//����
	vector<int> sortArray(vector<int>& nums) {
		InsertSort(nums);
		return nums;
	}
};