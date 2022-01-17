using namespace std;
#include<vector>
#include<iostream>
#include<iterator>
#include<string>
//原地法
//数组3次翻转
//操作	结果
//原始数组	1~2~3~4~5~6~7
//翻转所有元素	7~6~5~4~3~2~1
//翻转[0, k mod n - 1] 区间的元素	5~6~7~4~3~2~1
//翻转[k mod n, n - 1] 区间的元素	5~6~7~1~2~3~4
class Solution {
public:
    void reverse(vector<int>& nums, int start, int end) {
        while (start < end) {
            swap(nums[start], nums[end]);
            start += 1;
            end -= 1;
        }
    }

    void rotate(vector<int>& nums, int k) {
        k %= nums.size();
        reverse(nums, 0, nums.size() - 1);
        reverse(nums, 0, k - 1);
        reverse(nums, k, nums.size() - 1);
    }
};

//另存法，动态数组删插
//class Solution {
//public:
//    void rotate(vector<int>& nums, int k) {
//        int n = nums.size();
//        vector<int> nums2;
//        k %= n;
//        vector<int>::iterator end = nums.end();
//        vector<int>::iterator kbeg = prev(end, k);
//        nums2.insert(nums2.begin(),kbeg, end);
//        nums.erase(kbeg, end);
//        nums.insert(nums.begin(), nums2.begin(), nums2.end());
//    }
//};
//int main()
//{
//    Solution s;
//    vector<int> matrix = {1,2,3,4,5,6};
//    s.rotate(matrix, 2);
//    for(auto item:matrix)
//        cout<<to_string(item)+" ";
//    cout << endl;
//    return 0;
//}