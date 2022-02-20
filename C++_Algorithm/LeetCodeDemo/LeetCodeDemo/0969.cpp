using namespace std;
#include <iostream>
#include <vector>
#include <string>
#include <algorithm>
#include <unordered_set>
#include <unordered_map>
#include <set>
#include <map>
#include <queue>
#include <stack>
#include <functional>
//巧思 模拟
//将最大数调到最左然后拉到最右。
class Solution {
public:
    vector<int> pancakeSort(vector<int>& arr) {
        int idx[101];
        int n = arr.size();
        for (int i = 0; i < n; i++) {
            idx[i] = i;
        }
        sort(idx, idx + n, [&](int a, int b) {return arr[a] > arr[b]; });
        vector<int> rst;
        int t = 0, p, q;
        for (int i = 0; i < n-1; i++) {
            q = n - i;
            p = idx[i]<q-1? idx[i] + t + 1:idx[i];
            if (p != q) {
                rst.push_back(p);
                rst.push_back(q);
                t = q - p;
                idx[q] = 0;
            }
        }
        return rst;
    }
};