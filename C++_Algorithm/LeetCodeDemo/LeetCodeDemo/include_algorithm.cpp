using namespace std;
#include <vector>
#include <algorithm>
class Solution {
public:
    void include_algorithm() {
        //测试用变量
        int num = 3;
        vector<int> nums = { 3,2,1 };
        vector<int> nums1 = { 1,3,9 };
        vector<int> nums2 = { 2,4,6 };
        int nums3[3] = { 2,3,4 };
        int nums4[3] = { 7,8,9 };
        //void sort(iterator first, iterator last, Compare comp );//快速排序O(nlog2(n))，集合首指针或迭代器，集合尾指针或迭代器，比较函数bool类型传入比较用的2个元素，返回值为true则第一个元素在前。
        sort(nums.begin(), nums.end(), [](int& x, int& y) {return x > y; });//排序成递减数组
        merge(nums1.begin(), nums1.end(),nums2.begin(), nums2.end(), [](int& x,int& y){return x<y;} );//<algorithm>合并有序数组,类似于sort。
        merge(nums3, nums3 + sizeof(nums3), nums4, nums4 + sizeof(nums4),[](int& x, int& y) {return x < y; });//<algorithm>合并有序数组,类似于sort。
        int k = 4;
        nth_element(nums.begin(), nums.begin() + k, nums.end());//将第k小的元素放到第k+1小(k)的位置。
        nth_element(nums.begin(), nums.begin() + k, nums.end(), [](int a, int b) {return a > b; });//将第k大的元素放到第k+1大(k)的位置。
        //两分查找
        int num = 3;
        vector<int> nums = { 3,2,1 };
        vector<int>::iterator it;
        int* address;
        int idx;
        it = lower_bound(nums.begin(), nums.end(), num);//两分查找，在递增数组中，从数组的begin位置到end-1位置二分查找第一个大于或等于num的数字，找到返回该数字的地址，不存在则返回end。通过返回的地址减去起始地址begin,得到找到数字在数组中的下标。
        idx = it - nums.begin();
        address = upper_bound(nums3, nums3 + sizeof(nums3), num);//两分查找，在递增数组中，从数组的begin位置到end-1位置二分查找第一个大于num的数字，找到返回该数字的地址，不存在则返回end。通过返回的地址减去起始地址begin,得到找到数字在数组中的下标。
        idx = address - nums3;
        //最大公约数，最小公倍数
        int a = 1, b = 2;
        //__gcd(a,b);//求a,b的最大公约数,不过VS不能识别,GNU的私货,在Linux下的编译器可用
        //__lcm(a,b);//求a,b的最小公倍数,不过VS不能识别,GNU的私货,在Linux下的编译器可用

    }
};