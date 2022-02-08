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
//贪心
//左右左右跳着拿，拿光，看拿最后用的代价。
//左右左右
class Solution {
public:
    int minimumTime(string s) {
        int n = s.size();
        vector<int> rst(n, 2);
        rst[0]=rst[n-1] = 1;
        for (int i = 1;i<n;i++) {
            c = rst;
        }
    }
};