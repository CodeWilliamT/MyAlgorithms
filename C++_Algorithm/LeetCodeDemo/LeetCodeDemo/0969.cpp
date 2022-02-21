using namespace std;
#include <vector>
#include <algorithm>
//找规律 模拟
//将最大数调到最左然后拉到最右。
class Solution {
public:
    vector<int> pancakeSort(vector<int>& arr) {
        int n = arr.size();
        vector<int> rst;
        int idx;
        for (int i = n; i >0; i--) {
            idx = max_element(arr.begin(), arr.begin() + i)-arr.begin();
            if (idx + 1 == i)
                continue;
            reverse(arr.begin(), arr.begin() + idx+1);
            reverse(arr.begin(), arr.begin() + i);
            rst.push_back(idx + 1);
            rst.push_back(i);
        }
        return rst;
    }
};