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
//巧思
//电池数大于电脑数，则总分钟/电脑数。
class Solution {
public:
    long long maxRunTime(int n, vector<int>& batteries) {
        long long rest;
        long long tmp;
        int len = batteries.size();
        sort(batteries.begin(), batteries.end());
        for (int i = 0; i < n; i++) {
            rest+= batteries[i];
        }
        return rest;
    }
};