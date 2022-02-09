using namespace std;
#include <vector>
#include <algorithm>
//简单题
//小数据，直接排序俩数组然后按位置赋值回去完事
class Solution {
public:
    vector<int> sortEvenOdd(vector<int>& nums) {
        vector<int> odd, even;
        int i=0;
        for (int& e : nums) {
            if(i%2)
                odd.push_back(e);
            else 
                even.push_back(e);
            i++;
        }
        sort(odd.begin(), odd.end());
        sort(even.begin(), even.end(), [](int& a, int& b) {return a > b; });
        i = 0;
        for (int& e : nums) {
            if (i % 2) {
                e = odd.back();
                odd.pop_back();
            }
            else {
                e = even.back();
                even.pop_back();
            }
            i++;
        }
        return nums;
    }
};